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
			_RootRabbitData = rbt;

			_RootRabbit = new RabbitBar();
			this.Controls.Add(_RootRabbit);

			RabbitPair rp = new RabbitPair();
			rp.Location = new Point(1000, 100);

			int cnt = 0;

			_RabbitPairs.Add(cnt, rp);

			OneRabbit[] prnts = Engine.db().getParents(_RootRabbitData.id, 0);

			OneRabbit fr=prnts[0];

			OneRabbit mr=prnts[1];

			rp.SetMom(fr);
			rp.SetDad(mr);

			rp._id = cnt;

			this.Controls.Add(rp);
			rp.SetParentControl(this);
			GetPairData(rp, ref cnt, ref prnts);

			CenterTree();

		}

		public void CenterTree()
		{
			_RabbitPairs[0].Left = _RabbitPairs[0].GetSize().Width;//_RabbitPairs[0].GetCenter()+(int)(_RabbitPairs[0].Width/2);
//			_RabbitPairs[0].Left = _RabbitPairs[0].GetSize().Width / 2;
		}

		public void GetPairData(RabbitPair mrp, ref int c, ref OneRabbit[] prnts)
		{

			log.Debug(string.Format("Getting data for rabbit pair. (cnt:{0:d})",c));

			//CenterTree();
			label1.Text = c.ToString();
			label1.Invalidate();
			if (mrp.GetMom() != null)
			{
				log.Debug(string.Format("Rabbit pair #{0:d} has mom.", c));
				prnts = Engine.db().getParents(mrp.GetMom().id, 0);
				log.Debug(string.Format("#{0:d} 1", c));
				//				OneRabbit[] prnts = Engine.db().getParents(mrp.GetMom().id, 0);

				OneRabbit fr = prnts[0];
				log.Debug(string.Format("#{0:d} 2", c));

				OneRabbit mr = prnts[1];
				log.Debug(string.Format("#{0:d} 3", c));


				c++;

				RabbitPair rp = new RabbitPair();
				rp._id = c;

				_RabbitPairs.Add(c, rp);

				rp.SetMom(fr);
				rp.SetDad(mr);

				rp.SetParentControl(this);
				mrp.SetTreeChildFPair(rp);
				this.Controls.Add(rp);
				//this.Invalidate();

				log.Debug(string.Format("Getting parents for rabbit pair #{0:d} mom.", c));


				GetPairData(rp, ref c,ref prnts);
				log.Debug(string.Format("Getting parents for rabbit pair #{0:d} mom.... Done", c));

			}

			if (mrp.GetDad() != null)
			{
				log.Debug(string.Format("Rabbit pair #{0:d} has dad.", c));
				prnts = Engine.db().getParents(mrp.GetDad().id, 0);
				log.Debug(string.Format("#{0:d} 1", c));
				//				OneRabbit[] prnts = Engine.db().getParents(mrp.GetDad().id, 0);

				OneRabbit fr = prnts[0];
				log.Debug(string.Format("#{0:d} 2", c));

				OneRabbit mr = prnts[1];
				log.Debug(string.Format("#{0:d} 3", c));

//				log.Debug(prnts.ToString());

				c++;

				RabbitPair rp = new RabbitPair();
				rp._id = c;

				log.Debug(string.Format("#{0:d} 4", c));

				_RabbitPairs.Add(c, rp);
				log.Debug(string.Format("#{0:d} 5", c));

				rp.SetMom(fr);
				log.Debug(string.Format("#{0:d} 6", c));
				rp.SetDad(mr);
				log.Debug(string.Format("#{0:d} 7", c));

				rp.SetParentControl(this);
				log.Debug(string.Format("#{0:d} 8", c));
				mrp.SetTreeChildMPair(rp);
				log.Debug(string.Format("#{0:d} 9", c));
				this.Controls.Add(rp);
				//this.Invalidate();

				log.Debug(string.Format("Getting parents for rabbit pair #{0:d} dad.", c));
	
				GetPairData(rp, ref c, ref prnts);
				log.Debug(string.Format("Getting parents for rabbit pair #{0:d} dad.... Done", c));

			}

		}
	}
}
