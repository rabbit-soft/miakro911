using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;
using System.Diagnostics;
using Microsoft.Win32;
using rabnet.RNC;

namespace rabnet
{
    public partial class LoginForm : Form
    {
        public static bool stop = true;
        protected static readonly ILog log = LogManager.GetLogger(typeof(LoginForm));
        private RabnetConfig _rnc;
        private Dictionary<int, DataSource> _dss;

        public LoginForm()
        {
            InitializeComponent();
            log.Debug("inited");
            _rnc = new RabnetConfig();
        }

        public void readConfig()
        {
            /*try
            {
                ConfigurationManager.GetSection("rabnetds");
            }
            catch(Exception e)
            {
                log.Error("Read config error: "+e.Message);
                return;
            }*/
            _dss = new Dictionary<int, DataSource>();
            _rnc.LoadDataSources();
            cbFarm.Items.Clear();
            cbUser.Items.Clear();
            foreach (DataSource ds in _rnc.DataSources)
            {
                if (!ds.Hidden)
                {
                    cbFarm.Items.Add(ds.Name);
                    _dss.Add(cbFarm.Items.Count - 1, ds);
                    if (ds.Default)
                    {
                        cbFarm.SelectedIndex = cbFarm.Items.Count - 1;
                        cbFarm_SelectedIndexChanged(null, null);
                    }
                }
            }            
        }

        private void cbFarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFarm.SelectedIndex < 0)
                return;
            cbUser.Focus();
            try
            {
                Application.DoEvents();
                DataSource xs = null;
                foreach (DataSource d in _rnc.DataSources)
                    if (d.Name == cbFarm.Text)
                        xs = d;
                if (xs == null) return;
                Engine.get().initEngine(xs.Type, xs.Params.ToString());
                cbUser.Items.Clear();
                cbUser.Enabled = false;
                tbPassword.Enabled = false;
                List<sUser> usrs = Engine.db().GetUsers();
                if (usrs != null)
                {
                    cbUser.Enabled = true;
                    tbPassword.Enabled = true;
                    foreach (sUser s in usrs)
                    {
                        if (s.Group == sUser.Butcher) continue;
                        cbUser.Items.Add(s.Name);
                        if (xs.DefUser != "" && xs.DefUser == s.Name)
                        {
                            cbUser.SelectedIndex = cbUser.Items.Count - 1;
                            if (xs.DefPassword != "")
                            {
                                tbPassword.Text = xs.DefPassword;
                            }
                            tbPassword.Focus();
                            tbPassword.SelectAll();
                        }
                    }
                }
                else
                {
                    btEnter.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                cbUser.SelectedIndex = cbFarm.SelectedIndex = -1;
                cbFarm.Text=cbUser.Text=tbPassword.Text = "";
                cbFarm.Focus();
                //string message = ex.GetType().ToString() + ": " + ex.Message;
                MessageBox.Show(ex.Message, "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            btEnter.Enabled=(cbUser.Text!="");
            tbPassword.Text = "";
            tbPassword.Focus();
        }

        private void btEnter_Click(object sender, EventArgs e)
        {
            int uid = Engine.get().setUid(cbUser.Text, tbPassword.Text, cbFarm.Text);
            if (uid != 0)
            {
                _rnc.SetDefault(_dss[cbFarm.SelectedIndex], cbUser.Text, tbPassword.Text);
                
//                System.Diagnostics.Debug.WriteLine(RabnetConfigHandler.ds[comboBox1.SelectedIndex].getParamHost());

#if !DEMO
                RabUpdaterClient.Get().SetIP(_rnc.DataSources[cbFarm.SelectedIndex].Params.Host);
                
                bool upRes=RabUpdaterClient.Get().CheckUpdate();
                
                System.Diagnostics.Debug.WriteLine(upRes.ToString());

                if (upRes)
                {
                    DialogResult = DialogResult.Cancel;
                    Hide();
                    LoginForm.stop = false;
                    ProgressForm prg = new ProgressForm();
                    RabUpdaterClient.Get().progressUp = prg.progressUp;
                    prg.progressUp(0);
                    prg.Show();
                    RabUpdaterClient.Get().GetUpdate();
                    while (RabUpdaterClient.Get().GetUpRes() == RabUpdaterClient.UpErrStillWorking)
                    {
                        Application.DoEvents();

                    }
                    prg.Close();
                    prg = null;
                    int upProcRes = RabUpdaterClient.Get().GetUpRes();
                    if (upProcRes != RabUpdaterClient.UpErrFinishedOK)
                    {
                        if (upProcRes != RabUpdaterClient.UpErrBadMD5OnServer)
                        {
                            MessageBox.Show("При обновлении возникла ошибка." + Environment.NewLine + "Поробуйте перезапустить программу и обновить снова, если ошибка будет повторяться, то установите обновление вручную.", "Неполадки при обновлении", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LoginForm.stop = true;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("При обновлении возникла ошибка." + Environment.NewLine + "Файл обновлений на сервере поврежден, обратитесь к администратору!", "Неполадки при обновлении", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LoginForm.stop = true;
                            Close();
                        }
                    }
                    else
                    {
                        Process.Start(RabUpdaterClient.Get().GetUpFilePath(), "/S"); //Batch Mode
                        LoginForm.stop = true;
                        Close();
                    }
                }
                else
                {
#endif
                    DialogResult = DialogResult.OK;
                    Close();
#if !DEMO
                }
#endif
                return;
            }
            MessageBox.Show("Неверное имя пользователя или пароль","Ошибка авторизации",MessageBoxButtons.OK,MessageBoxIcon.Error);
            LoginForm.stop = false;
            DialogResult = DialogResult.Retry;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            //new FarmListForm().ShowDialog();
            new FarmNewForm().ShowDialog();
            Show();
            cbUser.Text = "";
            tbPassword.Text = "";
            readConfig();
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            readConfig();
            tbPassword.SelectAll();
            tbPassword.Focus();
            if (cbFarm.Items.Count == 0)
            {
                new FarmNewForm().ShowDialog();
                readConfig();
            }
        }
    }
}
