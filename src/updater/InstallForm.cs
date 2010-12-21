using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections;

namespace updater
{
    public partial class InstallForm : Form
    {
        bool _batch = false;
        public int Result = 0;
        public string Filename = "";
        private XmlDocument xml = new XmlDocument();
        XmlElement _opts = null;
        public InstallForm()
        {
            InitializeComponent();
        }
        public InstallForm(String fl,bool bt):this()
        {
            _batch = bt;
            Filename = fl;
            PrepareXml();
            radioButton4.Visible = !bt;
            radioButton4.Enabled = !bt;
        }
        private const string Conf="<?xml version=\"1.0\" encoding=\"utf-8\" ?><configuration><configSections>"+
"<section name=\"rabdumpOptions\" type=\"rabdump.RabdumpConfigHandler,rabdump\"/>"+
"<section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler,Log4net\"/>"+
"</configSections><rabdumpOptions></rabdumpOptions><log4net><appender name=\"FileAppender\" type=\"log4net.Appender.FileAppender\">"+
"<file value=\"dumplog.txt\" /><appendToFile value=\"true\" /><lockingModel type=\"log4net.Appender.FileAppender+MinimalLock\" />"+
"<layout type=\"log4net.Layout.PatternLayout\"><conversionPattern value=\"%date [%thread] %-5level %logger{2} [%property{NDC}] - %message%newline\" />"+
"</layout></appender><root><level value=\"DEBUG\" /><appender-ref ref=\"FileAppender\" /></root></log4net></configuration>";


        public class ReverseComparer : IComparer
        {
            public int Compare(Object object1, Object object2)
            {
                return -((IComparable)object1).CompareTo(object2);
            }
        }

        public void PrepareXml()
        {
            xml.LoadXml(Conf);
            foreach (XmlNode n in xml.DocumentElement.ChildNodes)
            {
                if (n.Name=="rabdumpOptions")
                {
                    _opts=(XmlElement)n;
                }
            }
            //XmlElement elem = xml.CreateElement("mysql");
//            string msp=(string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\MySQL AB\\MySQL Server 5.0","Location","C:\\Program Files\\MySQL\\MySQL Server 5.0\\");

            RegistryKey rk1 = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("MySQL AB");
            string loc;
            string locM = "";
            string locMd = "";
            if (rk1 == null)
            {
/*
                loc = "";
*/
                locM = "";
                locMd = "";

            }
            else
            {
                String[] names = rk1.GetSubKeyNames();
                Array.Sort(names, new ReverseComparer());
                foreach (String s in names)
                {
                    loc = (string)rk1.OpenSubKey(s).GetValue("Location");
                    if (loc != null)
                    {
                        locM = loc + "bin\\mysql.exe";
                        locMd = loc + "bin\\mysqldump.exe";
                        break;
                    }
                }
            }



            _opts.AppendChild(xml.CreateElement("mysql")).AppendChild(xml.CreateTextNode(locM));
            _opts.AppendChild(xml.CreateElement("mysqldump")).AppendChild(xml.CreateTextNode(locMd));



            string path7Za=Path.GetDirectoryName(Application.ExecutablePath) + "\\7z\\7za.exe";

            string loc_7B = "";

            if (File.Exists(path7Za))
            {
                loc_7B = path7Za;
            }
            else
            {
                //msp = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\7-Zip", "Path", "C:\\Program Files\\7-zip");

                RegistryKey rk2 = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("7-Zip");
                string loc7;
                if (rk2 == null)
                {
/*
                    loc7 = "";
*/
                    loc_7B = "";

                }
                else
                {
                    loc7 = (string)rk2.GetValue("Path");
                    if (loc7 != null)
                    {
                        loc_7B = loc7 + "\\7z.exe";
                    }
                }
            }




            _opts.AppendChild(xml.CreateElement("z7")).AppendChild(xml.CreateTextNode(loc_7B));


            XmlElement jb = xml.CreateElement("job");
            jb.AppendChild(xml.CreateElement("name")).AppendChild(xml.CreateTextNode("Все БД в 18:00"));
            jb.AppendChild(xml.CreateElement("db")).AppendChild(xml.CreateTextNode("[все]"));
            string bf = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            bf += "\\Miakro Backups";
            Directory.CreateDirectory(bf);
            jb.AppendChild(xml.CreateElement("path")).AppendChild(xml.CreateTextNode(bf));
            jb.AppendChild(xml.CreateElement("sizelim")).AppendChild(xml.CreateTextNode("0"));
            jb.AppendChild(xml.CreateElement("countlim")).AppendChild(xml.CreateTextNode("0"));
            jb.AppendChild(xml.CreateElement("start")).AppendChild(xml.CreateTextNode("01.01.2010 18:00"));
            jb.AppendChild(xml.CreateElement("type")).AppendChild(xml.CreateTextNode("Ежедневно"));
            jb.AppendChild(xml.CreateElement("repeat")).AppendChild(xml.CreateTextNode("0"));
            _opts.AppendChild(jb);
            xml.Save(Filename);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = !groupBox2.Visible;
            Height = (groupBox2.Visible ? 450 : 310);
            button3.Text = "Расширенный режим " + (groupBox2.Visible ? "<<" : ">>");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            tbComp.Enabled = radioButton2.Checked;
            tbFile.Enabled = button2.Enabled = radioButton3.Checked;
            if (radioButton3.Checked)
            {
                button1.Enabled = tbFile.Text != "";
            }
            else if (radioButton2.Checked)
            {
                button1.Enabled = tbComp.Text != "";
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void tbComp_TextChanged(object sender, EventArgs e)
        {
            tbHost.Text = tbComp.Text;
            button1.Enabled = tbComp.Text != "";
        }

        private void tbHost_TextChanged(object sender, EventArgs e)
        {
            tbComp.Text = tbHost.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
                tbFile.Text = ofd.FileName;
        }

        private void AddFarm()
        {
            XmlElement ds = xml.CreateElement("db");
            ds.AppendChild(xml.CreateElement("name")).AppendChild(xml.CreateTextNode(tbName.Text));
            ds.AppendChild(xml.CreateElement("host")).AppendChild(xml.CreateTextNode(tbHost.Text));
            ds.AppendChild(xml.CreateElement("db")).AppendChild(xml.CreateTextNode(tbDb.Text));
            ds.AppendChild(xml.CreateElement("user")).AppendChild(xml.CreateTextNode(tbUser.Text));
            ds.AppendChild(xml.CreateElement("password")).AppendChild(xml.CreateTextNode(tbPwd.Text));
            _opts.AppendChild(ds);
            xml.Save(Filename);
            Close();
        }

        private void RunMia(String prm)
        {
            String prms = "\"" + prm + "\" " + tbHost.Text + ';' + tbDb.Text + ';' + tbUser.Text + ';' + tbPwd.Text + ';' + tbRoot.Text + ';' + tbRPwd.Text;
            prms += " зоотехник;";
            String prg = Path.GetDirectoryName(Application.ExecutablePath) + "\\mia_conv.exe";
            Process p = Process.Start(prg, prms);
            p.WaitForExit();
            if (p.ExitCode != 0)
                throw new ApplicationException("Ошибка создания БД");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try{
                TopMost = false;
                if (radioButton4.Checked)
                {
                    //Application.Exit(
                    Environment.Exit(10);
                }
                if (tbName.Text == "")
                    throw new ApplicationException("Введите название фермы");
                if (radioButton2.Checked && tbComp.Text=="")
                    throw new ApplicationException("Введите адрес компьютера");
                if (radioButton3.Checked && tbFile.Text == "")
                    throw new ApplicationException("Выберите файл");
                if (radioButton1.Checked)
                {
                    RunMia("nudb");
                }
                else if (radioButton3.Checked)
                {
                    RunMia(tbFile.Text);
                }
                AddFarm();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                TopMost = true;
            }
        }

        private void tbFile_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = tbFile.Text != "";
        }
    }
}
