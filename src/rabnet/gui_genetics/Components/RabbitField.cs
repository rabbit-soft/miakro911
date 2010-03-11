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

		private RabbitGen _RootRabbitData;
		private RabbitBar _RootRabbit;
		private RabbitPair _RootRabbitPair;
		private Dictionary<int, RabbitPair> _RabbitPairs = new Dictionary<int, RabbitPair>();

		public RabbitField()
		{
			InitializeComponent();
			//this.mouse
		}

		private Boolean _OrderedGenom = false;
		public Boolean OrderedGenom
		{
			get { return _OrderedGenom; }
			set
			{
				_OrderedGenom = value;
				if (_RootRabbitPair != null)
				{
					_RootRabbitPair.OrderedGenom = value;
				}
				if (_RootRabbit != null)
				{
					_RootRabbit.OrderedGenom = value;
				}
			}
		}

		public void DrawRabbit(RabbitGen rbt)
		{
			Dictionary<int, Color> b_colors = new Dictionary<int, Color>();

			b_colors = Engine.db().getBreedColors();

			ProgressPanel.Visible = true;
			RabbitsHolder.Visible = false;

			_RootRabbitData = rbt;

			_RootRabbit = new RabbitBar();
			_RootRabbit.SetRabbit(rbt);

			_RootRabbit.ReplaceGenomeColors(b_colors);

			_RootRabbit.BackColor = this.BackColor;


			
			RabbitsHolder.Controls.Add(_RootRabbit);

			RabbitPair rp = new RabbitPair();
			_RootRabbitPair = rp;
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

			this.ScrollControlIntoView(_RootRabbit);
			this.ActiveControl = _RootRabbit;
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
			if (_RootRabbitPair == null)
			{
				return;
			}

			if (_RootRabbit == null)
			{
				return;
			}

			int c=0;

			Rectangle r=new Rectangle();

			_RootRabbitPair.OrganizePos(ref r, ref c);

			_RootRabbit.Left = c - (int)(_RabbitPairs[0].Width / 2f) + 10;
			_RootRabbit.Top = 0 + 10;

			_RootRabbitPair.SetNewLocation(c - (int)(_RabbitPairs[0].Width / 2f) + 10, _RootRabbit.Height + 10 + 10);

			RabbitsHolder.Width = r.Width + 20;
			RabbitsHolder.Height = r.Height + _RootRabbit.Height + 10 + 20;

			_RootRabbit.Genom = _RootRabbitPair.Genom;
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
