using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rabnet.Components
{
	public partial class RabbitField : UserControl
	{
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

			int cnt = 0;

			_RabbitPairs.Add(cnt, rp);

			OneRabbit fr;

			OneRabbit mr;

//			fr = Engine.db().getRabbit(_RootRabbitData.parent);



		}
	}
}
