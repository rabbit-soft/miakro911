//#define A
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
            toolTipsInit();
        }

        private void toolTipsInit()
        {
            toolTip1.InitialDelay = 6000;
            toolTip1.SetToolTip(btEditKey, "Назначить прошивку удаленноку ключу при следующем соединении с сервером");
            toolTip1.SetToolTip(btAddUser, "Добавить нового пользователя");
        }

        private void fillUsers()
        {
            lvDongles.Items.Clear();
            
            lvClients.Items.Clear();
            ResponceItem ri = _reqSend.ExecuteMethod(MethodName.GetClients);
            foreach (sClient cl in (ri.Value as sClient[]))
            {
                ListViewItem lvi = lvClients.Items.Add(cl.Organization);
                lvi.SubItems.Add(cl.ContactMan);
                lvi.SubItems.Add(cl.Money);
                lvi.SubItems.Add(cl.Address);
                lvi.SubItems.Add(cl.SAAS ? "SAAS" : "BOX");
                lvi.Tag = cl;
            }   
        }

        public bool Retry = false;

        private void btAddUser_Click(object sender, System.EventArgs e)
        {
            AddClientForm dlg = new AddClientForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _reqSend.ExecuteMethod(MethodName.AddClient,
                    MethodParamName.orgName, dlg.Organization,
                    MethodParamName.contact, dlg.Contact,
                    MethodParamName.address, dlg.Address,
                    MethodParamName.saas,dlg.SAAS ? "1":"0");
            }
            fillUsers();
        }

        private void lbClients_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            sClient c = lvClients.SelectedItems[0].Tag as sClient;
            if (c.Dongles == null) return;
            lvDongles.Items.Clear();
            foreach (sDongle d in c.Dongles)
            {
                ListViewItem lvi = lvDongles.Items.Add(d.Id);
                lvi.SubItems.Add(string.Format("0x{0:8:X}",d.Id));
                lvi.SubItems.Add(c.Organization);
                lvi.SubItems.Add(d.Farms);
                lvi.SubItems.Add(d.StartDate);
                lvi.SubItems.Add(d.EndDate);
                lvi.SubItems.Add(d.Flags);
                lvi.SubItems.Add(d.TimeFlags);
                lvi.SubItems.Add(d.TimeFlagsEnd);
                lvi.Tag = d;
            }
            if (lvDongles.Items.Count == 1)
                lvDongles.Items[0].Selected = true;
        }

        private void btAddKey_Click(object sender, System.EventArgs e)
        {
            GRDVendorKey key = null ;
            try
            {
                int retCode=0;
                if (lvClients.SelectedItems.Count == 0)               
                    throw new Exception("Не выбран клиент");

                sClient client = lvClients.SelectedItems[0].Tag as sClient;
                key = new GRDVendorKey();
                if (key.ID == 0)
                    throw new Exception("Произошла ошибка подключения к ключу.\nПодробности в логах");
#if !DEBUG
                foreach (sDongle d in client.Dongles)
                    if (d.Id == key.ID.ToString())
                        throw new Exception("Данный клиент уже имеет этот ключ");
#endif
                this.Enabled = false;
                AddDongleForm dlg = new AddDongleForm(client, key.ID);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
#if !A
                    _reqSend.ExecuteMethod(MethodName.VendorAddDongle,
                        MethodParamName.dongleId, key.ID.ToString(),
                        MethodParamName.orgId, client.Id,
                        MethodParamName.model, key.Model.ToString());

                    retCode = key.SetTRUKey();
                    if (retCode != 0) throw new Exception("Ощибка при инициализации ключа: " + retCode.ToString());
                    string q="";
                    retCode = key.GetTRUQuestion(out q);
                    if (retCode != 0) throw new Exception("Ощибка при генерировании числа-вопроса: "+retCode.ToString());
                    
                    ResponceItem s = _reqSend.ExecuteMethod(MethodName.VendorUpdateDongle,
                        MethodParamName.base64_question, q,
                        MethodParamName.orgId, client.Id,
                        MethodParamName.farms, dlg.Farms.ToString(),
                        MethodParamName.flags, dlg.Flags.ToString(),
                        MethodParamName.startDate, dlg.StartDate.ToString("yyyy-MM-dd"),
                        MethodParamName.endDate, dlg.EndDate.ToString("yyyy-MM-dd"),
                        MethodParamName.dongleId, key.ID.ToString());
                    string ans = s.Value.ToString();
                    if (ans == "")
                        throw new Exception("Пустое число-ответ");
                    key.SetTRUAnswer(ans);

#else
                    key.WriteMask((lbClients.SelectedItems[0].Tag as sClient).Organization,
                        dlg.Farms,
                        dlg.Flags,
                        dlg.StartDate,
                        dlg.EndDate);
#endif
                    
                    MessageBox.Show("Прошивка Завершена");
                }
                          
            }
            catch (Exception exc)
            {
                MessageBox.Show(
                    (exc.InnerException !=null) ? exc.InnerException.Message : exc.Message, 
                    "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if(key!=null)
                key.Dispose();
            this.Enabled = true;
            fillUsers();           
        }

        private void btEditKey_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            try
            {
                if (lvDongles.SelectedItems.Count == 0)
                    throw new Exception("Не выбран ключ");
                if (lvClients.SelectedItems.Count == 0)
                    throw new Exception("Не выбран пользователь");

                sClient client = lvClients.SelectedItems[0].Tag as sClient;
                GRDVendorKey key = new GRDVendorKey();
                if (key.ID == 0)
                    throw new Exception("Произошла ошибка подключения к ключу.\nПодробности в логах");

                AddDongleForm dlg = new AddDongleForm(client, (lvDongles.SelectedItems[0].Tag as sDongle));
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string q;
                    int retCode = key.GetTRUQuestion(out q);
                    if (retCode != 0) throw new Exception("Ощибка при генерировании числа-вопроса: " + retCode.ToString());
                    ResponceItem s = _reqSend.ExecuteMethod(MethodName.VendorUpdateDongle, //MName.VendorSheduleDongle,
                        MethodParamName.base64_question, q,
                        MethodParamName.orgId, client.Id,
                        MethodParamName.farms, dlg.Farms.ToString(),
                        MethodParamName.flags, dlg.Flags.ToString(),
                        MethodParamName.startDate, (lvDongles.SelectedItems[0].Tag as sDongle).StartDate,
                        MethodParamName.endDate, dlg.EndDate.ToString("yyyy-MM-dd"),
                        MethodParamName.dongleId, key.ID.ToString());

                    retCode = key.SetTRUAnswer(s.Value.ToString());
                    if (retCode != 0) throw new Exception("Ощибка установки числа ответа: " + retCode.ToString());
                    MessageBox.Show("Прошивка Завершена");
                }
                key.Dispose();              
            }
            catch (Exception exc)
            {
                MessageBox.Show(
                    (exc.InnerException != null) ? exc.InnerException.Message : exc.Message,
                    "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            fillUsers();
            this.Enabled = true;
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
                    if (this.lvClients.View == View.Details && this.lvClients.Columns.Count > 0)
                        this.lvClients.Columns[this.lvClients.Columns.Count - 1].Width = -2;
                    break;
            }

            // pass messages on to the base control for processing
            base.WndProc(ref message);
        }

        private void gbMoney_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btAddMoney_Click(object sender, EventArgs e)
        {
            try
            {
                AddMoneyForm dlg = new AddMoneyForm();
                if (dlg.ShowDialog() == DialogResult.OK)
                    _reqSend.ExecuteMethod(MethodName.AddClientMoney,
                        MethodParamName.orgId, (lvClients.SelectedItems[0].Tag as sClient).Id,
                        MethodParamName.money, dlg.Value.ToString());
                MessageBox.Show("Добавили успешно");
                fillUsers();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message,"",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void обновитьСписокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fillUsers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ret ;
            GRDVendorKey key = new GRDVendorKey();
            ret = key.GetOrgID();
            try
            {               
                /*ret = key.SetTRUKey();
                if (ret != 0)
                    throw new Exception(ret.ToString()); ;
               
                ret = key.WriteMask(
                             "Мистер-Кролл",
                             110,
                             21,
                             DateTime.Parse("2011-01-11"),
                             DateTime.Parse("2012-07-11"));
                throw new Exception(ret.ToString());*/
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
                
            key.Dispose();
        }


    }
}
