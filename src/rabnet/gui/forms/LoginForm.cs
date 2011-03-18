using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using log4net;
using System.Xml;
using System.Diagnostics;


namespace rabnet
{
    public partial class LoginForm : Form
    {
        public static bool stop = true;
        protected static readonly ILog log = LogManager.GetLogger(typeof(LoginForm));
        /// <summary>
        /// Вызывается ли форма для того чтобы редактировать подключения
        /// </summary>
        private bool dbedit=false;
        public LoginForm()
        {
            InitializeComponent();
            log.Debug("inited");
        }
        public LoginForm(bool dbedit):this()
        {
            this.dbedit = dbedit;
        }

        public void readConfig()
        {
            try
            {
                ConfigurationManager.GetSection("rabnetds");
            }
            catch(Exception e)
            {
                log.Error("Read config error: "+e.Message);
                return;
            }
            cbFarm.Items.Clear();
            foreach (RabnetConfigHandler.dataSource ds in RabnetConfigHandler.dataSources)
            {
                if (!ds.hidden)
                {
                    cbFarm.Items.Add(ds.name);
                    if (ds.def)
                    {
                        cbFarm.SelectedIndex = cbFarm.Items.Count - 1;
                        cbFarm_SelectedIndexChanged(null, null);
                    }
                }
            }

/*            MessageBox.Show("Farms -> " + GRD.Instance.GetFarmsCnt().ToString());
            MessageBox.Show("Genetics -> " + GRD.Instance.GetFlagGenetics().ToString());
            MessageBox.Show("Zootech -> " + GRD.Instance.GetFlagZootech().ToString());

            MessageBox.Show("Prog -> " + GRD.Instance.GetProgType().ToString());
            MessageBox.Show("DateStart -> " + GRD.Instance.GetDateStart().ToString());
            MessageBox.Show("DateEnd -> " + GRD.Instance.GetDateEnd().ToString());
            MessageBox.Show("Org -> " + GRD.Instance.GetOrgName());
 */

            //GRD.Instance.ValidKey();
            
        }

        private void cbFarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFarm.SelectedIndex < 0)
                return;
            cbUser.Focus();
            try
            {
                Application.DoEvents();
                RabnetConfigHandler.dataSource xs=null;
                foreach (RabnetConfigHandler.dataSource d in RabnetConfigHandler.dataSources)
                    if (d.name == cbFarm.Text)
                        xs = d;
                if (xs == null) return;
                Engine.get().initEngine(xs.type, xs.param);
                cbUser.Items.Clear();
                cbUser.Enabled = false;
                tbPassword.Enabled = false;
                List<sUser> usrs = Engine.db().getUsers();
                if (usrs != null)
                {
                    cbUser.Enabled = true;
                    tbPassword.Enabled = true;
                    foreach (sUser s in usrs)
                    {
                        if (s.Group == sUser.Butcher) continue;
                        cbUser.Items.Add(s.Name);
                        if (xs.defuser != "" && xs.defuser == s.Name)
                        {
                            cbUser.SelectedIndex = cbUser.Items.Count - 1;
                            if (xs.defpassword != "")
                            {
                                tbPassword.Text = xs.defpassword;
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
                MessageBox.Show("Ошибка подключения " + ex.GetType().ToString() + ": " + ex.Message,"Ошибка подключения");
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
                RabnetConfigHandler.dataSources[cbFarm.SelectedIndex].setDefault(cbUser.Text, tbPassword.Text);
                
//                System.Diagnostics.Debug.WriteLine(RabnetConfigHandler.ds[comboBox1.SelectedIndex].getParamHost());

#if !DEMO
                RabUpdaterClient.Get().SetIP(RabnetConfigHandler.dataSources[cbFarm.SelectedIndex].getParamHost());
                
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



        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            new FarmListForm().ShowDialog();
            Show();
            cbUser.Text = "";
            tbPassword.Text = "";
            readConfig();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            readConfig();
            tbPassword.SelectAll();
            tbPassword.Focus();
            if (dbedit)
            {
                new FarmChangeForm(null).ShowDialog();
                stop = true;
                DialogResult = DialogResult.Cancel;
            }
            if (cbFarm.Items.Count == 0)
            {
                new FarmChangeForm(true).ShowDialog();
                readConfig();
            }
        }
    }

    [System.Reflection.Obfuscation(Exclude=true,ApplyToMembers=true)]
    public class RabnetConfigHandler : IConfigurationSectionHandler
    {
        public class dataSource
        {
            public String name;
            public String type;
            public String param;
            public bool hidden=false;
            public bool def = false;
            public String defuser = "";
            public String defpassword = "";
            public bool savepassword = false;
            public dataSource(String name, String type, String param)
            {
                this.name = name;
                this.type = type;
                this.param = param;
            }
            public void setDefault(String uname,String pswd)
            {
                def = true;
                foreach (dataSource ds in RabnetConfigHandler.dataSources)
                    if (ds != this) ds.def = false;
                defuser = uname;
                if (!savepassword) defpassword = "";
                else defpassword = pswd;
                RabnetConfigHandler.save();
            }
            public string getParamHost()
            {
                string[] param_arr=param.Split(';');
                foreach (string prm in param_arr)
                {
                    if (prm.Substring(0, 4) == "host")
                    {
                        //System.Diagnostics.Debug.WriteLine(prm);
                        string[] hsts = prm.Split('=');
                        return hsts[1].Trim();
                    }
                }
                return "";
            }
        }

        public static List<dataSource> dataSources = new List<dataSource>();
        public object Create(object parent, object configContext, XmlNode section)
        {
            dataSources.Clear();
            foreach (XmlNode cn in section.ChildNodes)
            {
                if (cn.Name == "dataSource")
                {
                    dataSources.Add(new dataSource(cn.Attributes.GetNamedItem("name").Value,
                        cn.Attributes.GetNamedItem("type").Value, cn.Attributes.GetNamedItem("param").Value));
                    dataSource td = dataSources[dataSources.Count - 1];
                    if (cn.Attributes.GetNamedItem("default") != null)
                        td.def = (cn.Attributes.GetNamedItem("default").Value == "1");
                    if (cn.Attributes.GetNamedItem("savepassword") != null)
                        td.savepassword = (cn.Attributes.GetNamedItem("savepassword").Value == "1");
                    if (cn.Attributes.GetNamedItem("hidden") != null)
                        td.hidden = (cn.Attributes.GetNamedItem("hidden").Value == "1");
                    if (cn.Attributes.GetNamedItem("user") != null)
                        td.defuser = cn.Attributes.GetNamedItem("user").Value;
                    if (cn.Attributes.GetNamedItem("password") != null)
                        td.defpassword = cn.Attributes.GetNamedItem("password").Value;
                }
            }
            return section;
        }

        public static void save()
        {
            XmlDocument xml = new XmlDocument();
            XmlElement rnds = xml.CreateElement("rabnetds");
            xml.AppendChild(rnds);
            foreach (dataSource d in dataSources)
            {
                XmlElement xds = xml.CreateElement("dataSource");
                if (d.def)
                    xds.Attributes.Append(xml.CreateAttribute("default")).Value = "1";
                if (d.hidden)
                    xds.Attributes.Append(xml.CreateAttribute("hidden")).Value = "1";
                if (d.defuser != "")
                    xds.Attributes.Append(xml.CreateAttribute("user")).Value = d.defuser;
                if (d.defpassword != "")
                    xds.Attributes.Append(xml.CreateAttribute("password")).Value = d.defpassword;
                if (d.savepassword)
                    xds.Attributes.Append(xml.CreateAttribute("savepassword")).Value = "1";
                xds.Attributes.Append(xml.CreateAttribute("name")).Value = d.name;
                xds.Attributes.Append(xml.CreateAttribute("type")).Value = d.type;
                xds.Attributes.Append(xml.CreateAttribute("param")).Value = d.param;
                rnds.AppendChild(xds);
            }
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection rabnetds = conf.GetSection("rabnetds");
            if (rabnetds==null)
            {
                throw new Exception("bad configuration file");
            }
            rabnetds.SectionInformation.SetRawXml(rnds.OuterXml);
            conf.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("rabnetds");
        }
    }
}
