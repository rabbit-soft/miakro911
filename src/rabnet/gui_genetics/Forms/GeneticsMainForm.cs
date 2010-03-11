using System;
using System.Windows.Forms;

namespace rabnet
{
	public partial class GeneticsMainForm : Form
	{
		private int _RabbitID;
		private RabbitGen _Rabbit;

		public GeneticsMainForm()
		{
			InitializeComponent();
		}
		public void SetID(int id)
		{
			_RabbitID = id;
			_Rabbit = Engine.db().getRabbitGen(id);

			this.Text = string.Format("Родословная '{0}' ({1:D})", _Rabbit.fullname, _Rabbit.rid);
		}

		private Boolean _BatchClose = false;
		public void CloseBatch()
		{
			_BatchClose = true;
			Close();
		}


		private void GeneticsMainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_BatchClose)
			{
				GeneticsManager.RemoveForm(_RabbitID);
			}
		}

		private void GeneticsMainForm_Shown(object sender, EventArgs e)
		{
			checkBox1.Enabled = false;
			button1.Enabled = false;

			rabbitField1.DrawRabbit(_Rabbit);

			checkBox1.Enabled = true;
			button1.Enabled = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			GeneticsManager.CloseAllForms();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			rabbitField1.OrderedGenom = checkBox1.Checked;
		}

	}
}
