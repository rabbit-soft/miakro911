using System;
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

		private void button2_Click(object sender, EventArgs e)
		{
			rabbitField1.DrawRabbit(_Rabbit);
		}

		private void GeneticsMainForm_Load(object sender, EventArgs e)
		{
		}

		private void GeneticsMainForm_Shown(object sender, EventArgs e)
		{
			rabbitField1.DrawRabbit(_Rabbit);
		}
	}
}
