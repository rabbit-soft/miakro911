using System;
using System.Windows.Forms;
using pEngine;
using RabGRD;

namespace AdminGRD
{
    public partial class MainForm : Form
    {
        RequestSender _reqSend;

        public MainForm()
        {
            InitializeComponent();
            _reqSend = Engine.NewReqSender();
            fillUsers();
        }

        private void fillUsers()
        {
            lbClients.Items.Clear();
            ResponceItem ri = _reqSend.ExecuteMethod(MName.GetClients);
            foreach (sClient cl in (ri.Value as sClient[]))
            {
                ListViewItem lvi = lbClients.Items.Add(cl.Organization);
                lvi.Tag = cl;
            }
        }

        public bool Retry = false;

        private void btAddUser_Click(object sender, System.EventArgs e)
        {
            AddClientForm dlg = new AddClientForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _reqSend.ExecuteMethod(MName.AddClient,
                    MPN.orgName, dlg.Organization,
                    MPN.contact, dlg.Contact,
                    MPN.address, dlg.Address );
            }
            fillUsers();
        }

        private void lbClients_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            sClient c = lbClients.SelectedItems[0].Tag as sClient;
            tbOrgName.Text = c.Organization;
            tbContact.Text = c.ContactMan;
            tbAddress.Text = c.Address;
            listView1.Items.Clear();
            if (c.Dongles == null) return;
            foreach (sDongle d in c.Dongles)
            {
                ListViewItem lvi = listView1.Items.Add(d.Id);
                lvi.SubItems.Add(string.Format("0:8:X",d.Id));
                lvi.SubItems.Add(c.Organization);
                lvi.SubItems.Add(d.Farms);
                lvi.SubItems.Add(d.StartDate);
                lvi.SubItems.Add(d.EndDate);
                lvi.SubItems.Add(d.Flags);
                lvi.SubItems.Add(d.TimeFlags);
                lvi.SubItems.Add(d.TimeFlagsEnd);
            }
        }

        private void btAddKey_Click(object sender, System.EventArgs e)
        {
            if (lbClients.SelectedItems.Count == 0)
            {
                MessageBox.Show("Не выбран пользователь");
                return;
            }

            string org = (lbClients.SelectedItems[0].Tag as sClient).Organization;
            GRDEndUser key = new GRDEndUser();
            if (key.ID == 0)
            {
                MessageBox.Show("Произошла ошибка подключения к ключу.\nПодробности в логах");
                return;
            }
            AddDongleForm dlg = new AddDongleForm(org, key.ID);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ResponceItem s = _reqSend.ExecuteMethod(MName.VendorUpdateDongle,
                        MPN.base64_question, key.GetTRUQuestion(),
                        MPN.orgId, (lbClients.SelectedItems[0].Tag as sClient).Organization,
                        MPN.farms, dlg.Farms.ToString(),
                        MPN.flags, dlg.Flags.ToString(),
                        MPN.startDate, dlg.StartDate,
                        MPN.endDate, dlg.EndDate);

                    //key.SetTRUAnswer(s.Value);    
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
            key.Disconnect();
        }

        protected override void WndProc(ref Message message)
        {
            const int WM_PAINT = 0xf;

            // if the control is in details view mode and columns
            // have been added, then intercept the WM_PAINT message
            // and reset the last column width to fill the list view
            switch (message.Msg)
            {
                case WM_PAINT:
                    if (this.lbClients.View == View.Details && this.lbClients.Columns.Count > 0)
                        this.lbClients.Columns[this.lbClients.Columns.Count - 1].Width = -2;
                    break;
            }

            // pass messages on to the base control for processing
            base.WndProc(ref message);
        }
    }
}
