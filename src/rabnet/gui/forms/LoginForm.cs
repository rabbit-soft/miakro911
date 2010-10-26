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
            ConfigurationManager.GetSection("rabnetds");
            comboBox1.Items.Clear();
            foreach (RabnetConfigHandler.dataSource ds in RabnetConfigHandler.ds)
                if (!ds.hidden)
                {
                    comboBox1.Items.Add(ds.name);
                    if (ds.def)
                    {
                        comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                        comboBox1_SelectedIndexChanged(null, null);
                    }
                }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
                return;
            comboBox2.Focus();
            try
            {
                Application.DoEvents();
                RabnetConfigHandler.dataSource xs=null;
                foreach (RabnetConfigHandler.dataSource d in RabnetConfigHandler.ds)
                    if (d.name == comboBox1.Text)
                        xs = d;
                if (xs == null) return;
                Engine.get().initEngine(xs.type, xs.param);
                comboBox2.Items.Clear();
                comboBox2.Enabled = false;
                textBox1.Enabled = false;
                List<String> usrs = Engine.db().getUsers(false,0);
                if (usrs != null)
                {
                    comboBox2.Enabled = true;
                    textBox1.Enabled = true;
                    foreach (String s in usrs)
                    {
                        comboBox2.Items.Add(s);
                        if (xs.defuser != "" && xs.defuser == s)
                        {
                            comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
                            if (xs.defpassword != "")
                            {
                                textBox1.Text = xs.defpassword;
                            }
                            textBox1.Focus();
                            textBox1.SelectAll();
                        }
                    }
                }
                else
                {
                    button2.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                comboBox2.SelectedIndex = comboBox1.SelectedIndex = -1;
                comboBox1.Text=comboBox2.Text=textBox1.Text = "";
                comboBox1.Focus();
                MessageBox.Show("Ошибка подключения " + ex.GetType().ToString() + ": " + ex.Message,"Ошибка подключения");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled=(comboBox2.Text!="");
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int uid = Engine.get().setUid(comboBox2.Text, textBox1.Text, comboBox1.Text);
            if (uid != 0)
            {
                RabnetConfigHandler.ds[comboBox1.SelectedIndex].setDefault(comboBox2.Text, textBox1.Text);
                
                System.Diagnostics.Debug.WriteLine(RabnetConfigHandler.ds[comboBox1.SelectedIndex].getParamHost());
                RabUpdaterClient.Get().SetIP(RabnetConfigHandler.ds[comboBox1.SelectedIndex].getParamHost());
                
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



                    while (RabUpdaterClient.Get().GetUpRes()==RabUpdaterClient.UpErrStillWorking)
                    {
                        Application.DoEvents();
                        
                    }

                    prg.Close();

                    prg = null;

                    if (RabUpdaterClient.Get().GetUpRes() != RabUpdaterClient.UpErrFinishedOK)
                    {
                        MessageBox.Show("При обновлении возникла ошибка." + Environment.NewLine + "Поробуйте перезапустить программу и обновить снова, если ошибка будет повторяться, то установите обновление вручную.", "Неполадки при обновлении", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoginForm.stop = true;
                        Close();
                    }
                    else
                    {
                        Process.Start(RabUpdaterClient.Get().GetUpFilePath(),"test");
                        LoginForm.stop = true;
                        Close();

                    }


                }
                else
                {
                                    DialogResult = DialogResult.OK;
                                    Close();
                }






                return;
            }
            MessageBox.Show("Неверное имя пользователя или пароль");
        }



        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
//            if (e.KeyCode == Keys.Enter)
//                button2.PerformClick();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            new FarmListForm().ShowDialog();
            Show();
            comboBox2.Text = "";
            textBox1.Text = "";
            readConfig();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            readConfig();
            textBox1.SelectAll();
            textBox1.Focus();
            if (dbedit)
            {
                new FarmChangeForm(null).ShowDialog();
                stop = true;
                DialogResult = DialogResult.Cancel;
            }
            if (comboBox1.Items.Count == 0)
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
                foreach (dataSource ds in RabnetConfigHandler.ds)
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
        public static List<dataSource> ds = new List<dataSource>();
        public object Create(object parent, object configContext, XmlNode section)
        {
            ds.Clear();
            foreach (XmlNode cn in section.ChildNodes)
            {
                if (cn.Name == "dataSource")
                {
                    ds.Add(new dataSource(cn.Attributes.GetNamedItem("name").Value,
                        cn.Attributes.GetNamedItem("type").Value, cn.Attributes.GetNamedItem("param").Value));
                    dataSource td = ds[ds.Count - 1];
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
            foreach (dataSource d in ds)
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
            conf.GetSection("rabnetds").SectionInformation.SetRawXml(rnds.OuterXml);
            conf.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("rabnetds");
        }
    }
}
