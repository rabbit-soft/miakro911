using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using log4net;

namespace rabnet.Components
{
	public partial class RabbitField : UserControl
	{
		protected static readonly ILog log = LogManager.GetLogger(typeof(RabbitField));

		private OneRabbit _RootRabbitData;
		private RabbitBar _RootRabbit;
		private Dictionary<int, RabbitPair> _RabbitPairs = new Dictionary<int, RabbitPair>();

		public RabbitField()
		{
			InitializeComponent();
		}

		public void DrawRabbit(OneRabbit rbt)
		{
			ProgressPanel.Visible = true;
			RabbitsHolder.Visible = false;
			_RootRabbitData = rbt;

			_RootRabbit = new RabbitBar();
			this.Controls.Add(_RootRabbit);

			RabbitPair rp = new RabbitPair();
			rp.Location = new Point(1000, 100);

			int cnt = 0;

			_RabbitPairs.Clear();

			_RabbitPairs.Add(cnt, rp);

			RabbitGen rabb = Engine.db().getRabbitGen(_RootRabbitData.id);

			OneRabbit[] prnts = Engine.db().getParents(_RootRabbitData.id, 0);

			OneRabbit fr=prnts[0];

			OneRabbit mr=prnts[1];

			rp.SetMom(fr);
			rp.SetDad(mr);

			rp._id = cnt;

			RabbitsHolder.SuspendLayout();

//			RabbitsHolder.Controls.Add(rp);
			rp.SetParentControl(RabbitsHolder);
			GetPairData(rp, ref cnt, ref prnts);

			CenterTree();
			CenterHolder();
			ProgressPanel.Visible = false;
			RabbitsHolder.Visible = true;
//			this.ScrollControlIntoView(_RabbitPairs[0]);
			RabbitsHolder.ResumeLayout();
			//RabbitsHolder.AutoScrollMinSize

		}

		public void CenterHolder()
		{
			int l = (int)((this.Width - RabbitsHolder.Width) / 2);
			if (l < 0)
			{
				l = 0;
			}
			RabbitsHolder.Left = l;
			RabbitsHolder.Top = 0;
		}

		public void CenterTree()
		{

			if (_RabbitPairs.Count == 0)
			{
				return;
			}
			if (_RabbitPairs[0] == null)
			{
				return;
			}

			int c=0;

			Rectangle r=new Rectangle();

			_RabbitPairs[0].OrganizePos(ref r, ref c);

			_RabbitPairs[0].SetNewLocation(c - (int)(_RabbitPairs[0].Width / 2),0);

			RabbitsHolder.Width = r.Width;
			RabbitsHolder.Height = r.Height;
		}

		public void GetPairData(RabbitPair mrp, ref int c, ref OneRabbit[] prnts)
		{

			log.Debug(string.Format("Getting data for rabbit pair. (cnt:{0:d})",c));

			Application.DoEvents();

			if (mrp.GetMom() != null)
			{
				log.Debug(string.Format("Rabbit pair #{0:d} has mom.", c));
				prnts = Engine.db().getParents(mrp.GetMom().id, 0);
				OneRabbit fr = prnts[0];
				OneRabbit mr = prnts[1];
				c++;

				RabbitPair rp = new RabbitPair();
				rp._id = c;

				_RabbitPairs.Add(c, rp);

				rp.SetMom(fr);
				rp.SetDad(mr);

				rp.SetParentControl(RabbitsHolder);
				mrp.SetTreeChildFPair(rp);
				
				//RabbitsHolder.Controls.Add(rp);

				log.Debug(string.Format("Getting parents for rabbit pair #{0:d} mom.", c));


				GetPairData(rp, ref c,ref prnts);
				log.Debug(string.Format("Getting parents for rabbit pair #{0:d} mom.... Done", c));

			}

			if (mrp.GetDad() != null)
			{
				log.Debug(string.Format("Rabbit pair #{0:d} has dad.", c));
				prnts = Engine.db().getParents(mrp.GetDad().id, 0);
				OneRabbit fr = prnts[0];
				OneRabbit mr = prnts[1];

				c++;

				RabbitPair rp = new RabbitPair();
				rp._id = c;
				_RabbitPairs.Add(c, rp);
				rp.SetMom(fr);
				rp.SetDad(mr);
				rp.SetParentControl(RabbitsHolder);
				mrp.SetTreeChildMPair(rp);
				
				//RabbitsHolder.Controls.Add(rp);

				log.Debug(string.Format("Getting parents for rabbit pair #{0:d} dad.", c));
	
				GetPairData(rp, ref c, ref prnts);
				log.Debug(string.Format("Getting parents for rabbit pair #{0:d} dad.... Done", c));

			}

		}

		private void RabbitField_SizeChanged(object sender, System.EventArgs e)
		{
		}

		private void RabbitField_Resize(object sender, System.EventArgs e)
		{
			CenterHolder();
			ProgressPanel.Left = (int)((this.Width - ProgressPanel.Width)/2);
			ProgressPanel.Top = (int)((this.Height - ProgressPanel.Height)/2);
		}

		private void RabbitField_Scroll(object sender, ScrollEventArgs e)
		{
		}
	}
}
