using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
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

			this.Controls.Add(rp);
			rp.SetParentControl(this);
			GetPairData(rp, ref cnt);

			CenterTree();

		}

		public void CenterTree()
		{
			_RabbitPairs[0].Left = _RabbitPairs[0].GetSize().Width;//_RabbitPairs[0].GetCenter()+(int)(_RabbitPairs[0].Width/2);
//			_RabbitPairs[0].Left = _RabbitPairs[0].GetSize().Width / 2;
		}

		public void GetPairData(RabbitPair mrp, ref int c)
		{

			log.Debug(string.Format("Getting data for rabbit pair. (cnt:{0:d})",c));

			//CenterTree();
			label1.Text = c.ToString();
			label1.Invalidate();
			if (mrp.GetMom() != null)
			{
				OneRabbit[] prnts = Engine.db().getParents(mrp.GetMom().id, 0);

				OneRabbit fr = prnts[0];

				OneRabbit mr = prnts[1];

				c++;

				RabbitPair rp = new RabbitPair();

				_RabbitPairs.Add(c, rp);

				rp.SetMom(fr);
				rp.SetDad(mr);

				rp.SetParentControl(this);
				mrp.SetTreeChildFPair(rp);
				this.Controls.Add(rp);
				//this.Invalidate();


				GetPairData(rp, ref c);

			}

			if (mrp.GetDad() != null)
			{
				OneRabbit[] prnts = Engine.db().getParents(mrp.GetDad().id, 0);

				OneRabbit fr = prnts[0];

				OneRabbit mr = prnts[1];

				c++;

				RabbitPair rp = new RabbitPair();

				_RabbitPairs.Add(c, rp);

				rp.SetMom(fr);
				rp.SetDad(mr);

				rp.SetParentControl(this);
				mrp.SetTreeChildMPair(rp);
				this.Controls.Add(rp);
				//this.Invalidate();

				GetPairData(rp, ref c);

			}

		}
	}
}
