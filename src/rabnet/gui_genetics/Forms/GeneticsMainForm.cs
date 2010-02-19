using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
	public partial class GeneticsMainForm : Form
	{
		private int _RabbitID;
		private OneRabbit _Rabbit;

		public GeneticsMainForm()
		{
			InitializeComponent();
		}
		public void SetID(int id)
		{
			_RabbitID = id;
			_Rabbit = Engine.db().getRabbit(id);

			this.Text = string.Format("Родословная '{0}' ({1:D})", _Rabbit.fullname, _RabbitID);

			rabbitField1.DrawRabbit(_Rabbit);

		}

		private Boolean _BatchClose = false;
		public void CloseBatch()
		{
			_BatchClose = true;
			Close();
		}


		private void button1_Click(object sender, EventArgs e)
		{
			GeneticsManager.CloseAllForms();
		}

		private void GeneticsMainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_BatchClose)
			{
				GeneticsManager.RemoveForm(_RabbitID);
			}
		}
	}
}
