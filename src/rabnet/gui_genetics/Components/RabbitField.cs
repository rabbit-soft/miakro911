using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using log4net;
using System;

namespace rabnet.components
{
	public partial class RabbitField : UserControl
	{
		protected static readonly ILog log = LogManager.GetLogger(typeof(RabbitField));

		public event EvSearchGoingOn SearchGoingOn;

		//private RabbitGen _rootRabbitData;
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
			//_rootRabbitData = rbt;
			_rootRabbit = new RabbitBar();
			_rootRabbit.SetRabbit(rbt);
			_rootRabbit.ReplaceGenomeColors(b_colors);
			_rootRabbit.BackColor = this.BackColor;
    		_rootRabbit.WindowRabbitID = rbt.ID;			
			RabbitsHolder.Controls.Add(_rootRabbit);
            ///рисуем мать и отца выделенного кролика
			RabbitPair parents = new RabbitPair();
			_rootRabbitPair = parents;
			_rootRabbitPair.WindowRabbitID = rbt.ID;
			_rootRabbitPair.SearchGoingOn+=new EvSearchGoingOn(SearchProc);
			parents.Location = new Point(1000, 100);

			int cnt = 0;
			_RabbitPairs.Clear();
			_RabbitPairs.Add(cnt, parents);
			parents.ReplaceGenomeColors(b_colors);

			RabbitGen rabbF = Engine.db().getRabbitGen(rbt.FatherId);
			RabbitGen rabbM = Engine.db().getRabbitGen(rbt.MotherId);

			parents.SetMom(rabbM);
			parents.SetDad(rabbF);
			parents._id = cnt;
			RabbitsHolder.SuspendLayout();
			parents.SetParentControl(RabbitsHolder);
			GetPairData(parents, ref cnt);

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

        /// <summary>
        /// Рекурсивно рисует данные по предкам пары Сацец-Самка
        /// </summary>
        /// <param name="mrp"></param>
        /// <param name="pairsCount"></param>
		public void GetPairData(RabbitPair mrp, ref int pairsCount)
		{
			log.Debug(string.Format("Getting data for rabbit pair. (cnt:{0:d})",pairsCount));
			Application.DoEvents();

			if (mrp.GetMom() != null)
			{
                log.Debug(string.Format("Rabbit pair #{0:d} has mom.", pairsCount));
                RabbitPair rp = GetHalhPairData(mrp.GetMom(),ref pairsCount);
                mrp.SetTreeChildFPair(rp);
			}

			if (mrp.GetDad() != null)
			{
                log.Debug(string.Format("Rabbit pair #{0:d} has dad.", pairsCount));
                RabbitPair rp = GetHalhPairData(mrp.GetDad(), ref pairsCount);
                mrp.SetTreeChildMPair(rp);
			}
		}		

        public RabbitPair GetHalhPairData(RabbitGen rg,ref int pairsCount)
        {
            RabbitGen rabbM = Engine.db().getRabbitGen(rg.MotherId);
            RabbitGen rabbF = Engine.db().getRabbitGen(rg.FatherId);

            if ((rabbM != null) || (rabbF != null))
            {
                pairsCount++;

                RabbitPair rp = new RabbitPair();
                rp._id = pairsCount;
                _RabbitPairs.Add(pairsCount, rp);

                rp.SetParentControl(RabbitsHolder);

                rp.SetMom(rabbM);
                rp.SetDad(rabbF);

                log.Debug(string.Format("Getting parents for rabbit pair #{0:d} mom.", pairsCount));
                GetPairData(rp, ref pairsCount);

                return rp;
            }
            else
            {
                log.Debug(string.Format("There's no parents :(", pairsCount));
                return null;
            }
        }

        private void RabbitField_Resize(object sender, System.EventArgs e)
        {
            CenterHolder();
            ProgressPanel.Left = (int)((this.Width - ProgressPanel.Width) / 2);
            ProgressPanel.Top = (int)((this.Height - ProgressPanel.Height) / 2);
        }
	}
}
