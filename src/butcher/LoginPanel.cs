using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace butcher
{
    internal delegate void SuccesfulLogin();

    internal partial class LoginPanel : UserControl
    {
        private delegate IntPtr KeyBoardProc(int nCode, IntPtr wParam, IntPtr lParam);

        internal event EventHandler SuccessfulLogin;

        internal readonly Point mustLocation = new Point(12, 12);

        internal LoginPanel()
        {
            InitializeComponent();
            npLogin.AddControl(tbPassword);
            RabnetConfigHandler.Create();
            UpdateFarms();
        }

        /// <summary>
        /// Заполняет ComboBox с фермами
        /// </summary>
        public void UpdateFarms()
        {
            cbFarm.Items.Clear();
            cbUser.Items.Clear();
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
        }

        private void cbFarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbPassword.Enabled = npLogin.Enabled = false; 
            if (cbFarm.SelectedIndex < 0)
                return;
            cbUser.Focus();
            try
            {
                Application.DoEvents();
                RabnetConfigHandler.dataSource xs = null;
                foreach (RabnetConfigHandler.dataSource d in RabnetConfigHandler.dataSources)
                    if (d.name == cbFarm.Text)
                        xs = d;
                if (xs == null) return;
                if (!DBproc.Connect(xs.param))
                    throw new Exception("Не удалось подключиться к Базе Данных");
                cbUser.Items.Clear();
                List<sUser> usrs = DBproc.GetUsers();//Получаем список юзеров
                if (usrs != null && usrs.Count>0)
                {
                    foreach (sUser s in usrs)
                    {
                        cbUser.Items.Add(s.Name);
                        if (xs.defuser != "" && xs.defuser == s.Name)
                        {
                            cbUser.SelectedIndex = cbUser.Items.Count - 1;
                            if (xs.defpassword != "")                          
                                tbPassword.Text = xs.defpassword;                       
                            tbPassword.Focus();
                            tbPassword.SelectAll();
                        }
                    }
                }
                /*else
                {
                    npLogin.OkButtonEnable = false;
                }*/
            }
            catch (Exception ex)
            {
                cbUser.SelectedIndex = cbFarm.SelectedIndex = -1;
                cbFarm.Text = cbUser.Text = tbPassword.Text = "";
                cbFarm.Focus();               
                lbError.Text="Ошибка подключения " + ex.GetType().ToString() + ": " + ex.Message;
            }
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            List<char> numbers = new List<char> ();
            numbers.Add('0');
            numbers.Add('1');
            numbers.Add('2');
            numbers.Add('3');
            numbers.Add('4');
            numbers.Add('5');
            numbers.Add('6');
            numbers.Add('7');
            numbers.Add('8');
            numbers.Add('9');
            TextBox tb = (sender as TextBox);
            try
            {
                ulong.Parse(tb.Text);
            }
            catch (FormatException)
            {
                if (tb.Text != "")
                {
                    for (int i = 0; i < tb.Text.Length; i++)
                    {
                        if (!numbers.Contains(tb.Text[i]))
                        {
                            tb.Text = tb.Text.Remove(i, 1);
                            tb.Select(i, 0);
                            break;
                        }
                    }
                }
            }
        }

        private void tError_Tick(object sender, EventArgs e)
        {
            tError.Stop();
            gbMessage.Hide();
        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbPassword.Enabled = npLogin.Enabled = (cbUser.SelectedIndex > -1);
        }

        private void npLogin_OkButtonClick(object sender, EventArgs e)
        {
            if (DBproc.CheckUser(cbUser.Text, tbPassword.Text))
            {
                this.Hide();
                RabnetConfigHandler.dataSources[cbFarm.SelectedIndex].setDefault(cbUser.Text, tbPassword.Text);
                SuccessfulLogin(sender,e);
            }
            else
            {
                gbMessage.Show();
                lbError.Text = "Не удалось выполнить вход." + Environment.NewLine
                    + "Возможно не правильно введен пароль.";
                //tError.Start();
            }
            tbPassword.Clear();
        }

        private void lbError_TextChanged(object sender, EventArgs e)
        {
            if (lbError.Text != "")
            {
                gbMessage.Show();
                tError.Start();
            }
        }

        private void npLogin_Load(object sender, EventArgs e)
        {

        }
    }

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public static class RabnetConfigHandler 
    {
        public class dataSource
        {
            public String name;
            public String type;
            public String param;
            public bool hidden = false;
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
            public void setDefault(String uname, String pswd)
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
                string[] param_arr = param.Split(';');
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
        public static object Create()
        {
            RegistryKey rKey = Registry.CurrentUser;
            rKey = rKey.CreateSubKey("Software\\9-bits\\Miakro\\butcher");
            XmlDocument doc = new XmlDocument();
            doc.InnerXml = (string)rKey.GetValue("rabnets","<rabnets/>");
            XmlNode section = doc.SelectSingleNode("rabnets");
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
            XmlElement rnds = xml.CreateElement("rabnets");
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
            /*Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection rabnetds = conf.GetSection("rabnetds");
            if (rabnetds == null)
            {
                throw new Exception("bad configuration file");
            }
            rabnetds.SectionInformation.SetRawXml(rnds.OuterXml);
            conf.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("rabnetds");
            */
            RegistryKey rKey = Registry.CurrentUser;
            rKey = rKey.CreateSubKey("Software\\9-bits\\Miakro\\butcher");
            rKey.SetValue("rabnets",xml.InnerXml);
        
        }
    }
}
