using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using log4net;
using System;

namespace rabnet.Components
{
	public partial class RabbitField : UserControl
	{
		protected static readonly ILog log = LogManager.GetLogger(typeof(RabbitField));

		public event EvSearchGoingOn SearchGoingOn;

		private RabbitGen _rootRabbitData;
		private RabbitBar _rootRabbit;
		private RabbitPair _rootRabbitPair;
		private Dictionary<int, RabbitPair> _RabbitPairs = new Dictionary<int, RabbitPair>();

		public RabbitField()
		{
			InitializeComponent();
			//this.mouse
		}

		private Boolean _orderedGenom = false;
		public Boolean OrderedGenom
		{
			get { return _orderedGenom; }
			set
			{
				_orderedGenom = value;
				if (_rootRabbitPair != null)
				{
					_rootRabbitPair.OrderedGenom = value;
				}
				if (_rootRabbit != null)
				{
					_rootRabbit.OrderedGenom = value;
				}
			}
		}

		private Boolean SearchProc(RabbitCommandMessage cmd)
		{
			if (SearchGoingOn != null)
			{
				return SearchGoingOn(cmd);
			}
			return false;
		}

		public Boolean SearchField(RabbitCommandMessage cmd)
		{
			Boolean res = false;
			if (_rootRabbitPair != null)
			{
				res = res || _rootRabbitPair.SearchFromChild(cmd, false);
			}
			if (_rootRabbit != null)
			{
				res = res || _rootRabbit.SearchFromParent(cmd);
			}
			return res;
		}

		public void DrawRabbit(RabbitGen rbt)
		{
			Dictionary<int, Color> b_colors = new Dictionary<int, Color>();

			b_colors = Engine.db().getBreedColors();

			ProgressPanel.Visible = true;
			RabbitsHolder.Visible = false;

			_rootRabbitData = rbt;

			_rootRabbit = new RabbitBar();
			_rootRabbit.SetRabbit(rbt);

			_rootRabbit.ReplaceGenomeColors(b_colors);

			_rootRabbit.BackColor = this.BackColor;

			_rootRabbit.WindowRabbitID = rbt.rid;

			
			RabbitsHolder.Controls.Add(_rootRabbit);

			RabbitPair rp = new RabbitPair();
			_rootRabbitPair = rp;

			_rootRabbitPair.WindowRabbitID = rbt.rid;

			_rootRabbitPair.SearchGoingOn+=new EvSearchGoingOn(SearchProc);


			rp.Location = new Point(1000, 100);

			int cnt = 0;

			_RabbitPairs.Clear();

			_RabbitPairs.Add(cnt, rp);

			rp.ReplaceGenomeColors(b_colors);

			RabbitGen rabbF = Engine.db().getRabbitGen(rbt.r_father);

			RabbitGen rabbM = Engine.db().getRabbitGen(rbt.r_mother);

			rp.SetMom(rabbM);
			rp.SetDad(rabbF);

			rp._id = cnt;

			RabbitsHolder.SuspendLayout();

			rp.SetParentControl(RabbitsHolder);
			GetPairData(rp, ref cnt);

			CenterTree();
			CenterHolder();
			ProgressPanel.Visible = false;
			RabbitsHolder.Visible = true;
			RabbitsHolder.ResumeLayout();

			this.ScrollControlIntoView(_rootRabbit);
			this.ActiveControl = _rootRabbit;
			//RabbitsHolder.AutoScrollMinSize

		}

		void _RootRabbitPair_SearchGoingOn(object sender, EventArgs e)
		{
			throw new NotImplementedException();
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
			if (_rootRabbitPair == null)
			{
				return;
			}

			if (_rootRabbit == null)
			{
				return;
			}

			int c=0;

			Rectangle r=new Rectangle();

			_rootRabbitPair.OrganizePos(ref r, ref c);

			_rootRabbit.Left = c - (int)(_RabbitPairs[0].Width / 2f) + 10;
			_rootRabbit.Top = 0 + 10;

			_rootRabbitPair.SetNewLocation(c - (int)(_RabbitPairs[0].Width / 2f) + 10, _rootRabbit.Height + 10 + 10);

			RabbitsHolder.Width = r.Width + 20;
			RabbitsHolder.Height = r.Height + _rootRabbit.Height + 10 + 20;

			_rootRabbit.Genom = _rootRabbitPair.Genom;
		}

		public void GetPairData(RabbitPair mrp, ref int c)
		{

			log.Debug(string.Format("Getting data for rabbit pair. (cnt:{0:d})",c));

			Application.DoEvents();


			if (mrp.GetMom() != null)
			{
				log.Debug(string.Format("Rabbit pair #{0:d} has mom.", c));

				RabbitGen rabbM = Engine.db().getRabbitGen(mrp.GetMom().r_mother);

				RabbitGen rabbF = Engine.db().getRabbitGen(mrp.GetMom().r_father);

				if ((rabbM != null) || (rabbF != null))
				{

					c++;

					RabbitPair rp = new RabbitPair();
					rp._id = c;

					_RabbitPairs.Add(c, rp);


					rp.SetParentControl(RabbitsHolder);
					mrp.SetTreeChildFPair(rp);

					rp.SetMom(rabbM);
					rp.SetDad(rabbF);
					//RabbitsHolder.Controls.Add(rp);

					log.Debug(string.Format("Getting parents for rabbit pair #{0:d} mom.", c));


					GetPairData(rp, ref c);
					log.Debug(string.Format("Getting parents for rabbit pair #{0:d} mom.... Done", c));
				}
				else
				{
					log.Debug(string.Format("There's no parents :(", c));
				}

			}

			if (mrp.GetDad() != null)
			{
				log.Debug(string.Format("Rabbit pair #{0:d} has dad.", c));

				RabbitGen rabbM = Engine.db().getRabbitGen(mrp.GetDad().r_mother);

				RabbitGen rabbF = Engine.db().getRabbitGen(mrp.GetDad().r_father);

				if ((rabbM != null) || (rabbF != null))
				{
					c++;

					RabbitPair rp = new RabbitPair();
					rp._id = c;
					_RabbitPairs.Add(c, rp);

					rp.SetParentControl(RabbitsHolder);
					mrp.SetTreeChildMPair(rp);

					rp.SetMom(rabbM);
					rp.SetDad(rabbF);

					//RabbitsHolder.Controls.Add(rp);

					log.Debug(string.Format("Getting parents for rabbit pair #{0:d} dad.", c));

					GetPairData(rp, ref c);
					log.Debug(string.Format("Getting parents for rabbit pair #{0:d} dad.... Done", c));
				}
				else
				{
					log.Debug(string.Format("There's no parents :(", c));
				}

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
