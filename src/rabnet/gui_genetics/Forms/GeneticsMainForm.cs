using System;
using System.Windows.Forms;

namespace rabnet
{
	public partial class GeneticsMainForm : Form
	{
		private int _rabbitID;
		private RabbitGen _rabbit;

		public GeneticsMainForm()
		{
			InitializeComponent();
			rabbitField1.SearchGoingOn += new EvSearchGoingOn(rabbitField1_SearchGoingOn);
		}

		private bool rabbitField1_SearchGoingOn(RabbitCommandMessage cmd)
		{
//			cmd.SourceRabbitID = _RabbitID;
			return GeneticsManager.BroadcastSearch(cmd);
		}

		public Boolean SearchWindow(RabbitCommandMessage cmd)
		{
			if (cmd.SourceWindowRabbitID == _rabbitID)
			{
				return false;
			}
			return rabbitField1.SearchField(cmd);
		}

		public void SetID(int id)
		{
			_rabbitID = id;
			_rabbit = Engine.db().getRabbitGen(id);

			this.Text = string.Format("Родословная '{0}' ({1:D})", _rabbit.Fullname, _rabbit.ID);
		}

		private Boolean _batchClose = false;
		public void CloseBatch()
		{
			_batchClose = true;
			Close();
		}


		private void GeneticsMainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!_batchClose)
			{
				RabbitCommandMessage cmd = new RabbitCommandMessage();
				cmd.Command = RabbitCommandMessage.Commands.ForgetWindow;
				cmd.TargetRabbitID = 0;
				cmd.SourceWindowRabbitID = _rabbitID;
				GeneticsManager.BroadcastSearch(cmd);
				GeneticsManager.RemoveForm(_rabbitID);
			}
		}

		private void GeneticsMainForm_Shown(object sender, EventArgs e)
		{
			checkBox1.Enabled = false;
			button1.Enabled = false;

			rabbitField1.DrawRabbit(_rabbit);

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
