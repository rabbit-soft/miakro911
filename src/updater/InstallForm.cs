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
        //bool _batch = false;
        public int Result = 0;
        public string FilenameRabDump = "";
        public string FilenameRabNet = "";
        private XmlDocument _xmlRabDump = new XmlDocument();
        private XmlDocument _xmlRabNet = new XmlDocument();
        XmlElement _optsRabDump = null;
        XmlElement _optsRabNet = null;

        private InstallForm()
        {
            InitializeComponent();
        }
        public InstallForm(String flRabDump, string flRabNet ,bool bt):this()
        {
            //_batch = bt;
            FilenameRabDump = flRabDump;
            FilenameRabNet = flRabNet;
            PrepareXmlRabDump();
            PrepareXmlRabNet();
            radioButton4.Visible = !bt;
            radioButton4.Enabled = !bt;
        }
        private const string ConfRabDump="<?xml version=\"1.0\" encoding=\"utf-8\" ?><configuration><configSections>"+
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

        private void PrepareXmlRabNet()
        {
            try
            {
                _xmlRabNet.Load(FilenameRabNet);
                //xml.LoadXml(conf);
                //            _xmlRabNet.Save(filename);
//                foreach (XmlNode n in _xmlRabNet.DocumentElement.ChildNodes)
//                {
//                    if (n.Name == "rabnetds")
//                    {
                        _optsRabNet = (XmlElement) _xmlRabNet.DocumentElement.FirstChild.ParentNode;
//                        _optsRabNet = (XmlElement)n;
                //                    }
//                }
            } catch
            {
            }
        }

        public static bool TestRabNetConfig(string fl)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(fl);
                foreach (XmlNode n in xml.DocumentElement.ChildNodes)
                {
                    if (n.Name == "rabnetds")
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }


        private void PrepareXmlRabDump()
        {
            try
            {
                _xmlRabDump.LoadXml(ConfRabDump);
                foreach (XmlNode n in _xmlRabDump.DocumentElement.ChildNodes)
                {
                    if (n.Name == "rabdumpOptions")
                    {
                        _optsRabDump = (XmlElement) n;
                    }
                }
                //XmlElement elem = xml.CreateElement("mysql");
                //            string msp=(string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\MySQL AB\\MySQL Server 5.0","Location","C:\\Program Files\\MySQL\\MySQL Server 5.0\\");

                RegistryKey rk1 = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("MySQL AB");
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
                        if (s.StartsWith("MySQL Server"))
                        {
                            string loc = (string) rk1.OpenSubKey(s).GetValue("Location");
                            if (loc != null)
                            {
                                locM = loc + "bin\\mysql.exe";
                                locMd = loc + "bin\\mysqldump.exe";
                                break;
                            }
                        }
                    }
                }



                _optsRabDump.AppendChild(_xmlRabDump.CreateElement("mysql")).AppendChild(_xmlRabDump.CreateTextNode(locM));
                _optsRabDump.AppendChild(_xmlRabDump.CreateElement("mysqldump")).AppendChild(_xmlRabDump.CreateTextNode(locMd));



                string path7Za = Path.GetDirectoryName(Application.ExecutablePath) + @"\..\7z\7za.exe";

                string loc_7B = "";

                if (File.Exists(path7Za))
                {
                    loc_7B = path7Za;
                }
                else
                {
                    //msp = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\7-Zip", "Path", "C:\\Program Files\\7-zip");

                    RegistryKey rk2 = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("7-Zip");
                    if (rk2 == null)
                    {
                        //                    loc7 = "";
                        loc_7B = "";

                    }
                    else
                    {
                        string loc7 = (string) rk2.GetValue("Path");
                        if (loc7 != null)
                        {
                            loc_7B = loc7 + "\\7z.exe";
                        }
                    }
                }



                _optsRabDump.AppendChild(_xmlRabDump.CreateElement("z7")).AppendChild(_xmlRabDump.CreateTextNode(loc_7B));


                XmlElement jb = _xmlRabDump.CreateElement("job");
                jb.AppendChild(_xmlRabDump.CreateElement("name")).AppendChild(_xmlRabDump.CreateTextNode("Все БД в 18:00"));
                jb.AppendChild(_xmlRabDump.CreateElement("db")).AppendChild(_xmlRabDump.CreateTextNode("[все]"));
                string bf = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                bf += "\\Miakro Backups";
                Directory.CreateDirectory(bf);
                jb.AppendChild(_xmlRabDump.CreateElement("path")).AppendChild(_xmlRabDump.CreateTextNode(bf));
                jb.AppendChild(_xmlRabDump.CreateElement("sizelim")).AppendChild(_xmlRabDump.CreateTextNode("0"));
                jb.AppendChild(_xmlRabDump.CreateElement("countlim")).AppendChild(_xmlRabDump.CreateTextNode("0"));
                jb.AppendChild(_xmlRabDump.CreateElement("start")).AppendChild(_xmlRabDump.CreateTextNode("01.01.2010 18:00"));
                jb.AppendChild(_xmlRabDump.CreateElement("type")).AppendChild(_xmlRabDump.CreateTextNode("Ежедневно"));
                jb.AppendChild(_xmlRabDump.CreateElement("repeat")).AppendChild(_xmlRabDump.CreateTextNode("0"));
                _optsRabDump.AppendChild(jb);
                _xmlRabDump.Save(FilenameRabDump);
            } catch
            {
            }
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
            try
            {
                XmlElement dsRabDump = _xmlRabDump.CreateElement("db");
                dsRabDump.AppendChild(_xmlRabDump.CreateElement("name")).AppendChild(_xmlRabDump.CreateTextNode(tbName.Text));
                dsRabDump.AppendChild(_xmlRabDump.CreateElement("host")).AppendChild(_xmlRabDump.CreateTextNode(tbHost.Text));
                dsRabDump.AppendChild(_xmlRabDump.CreateElement("db")).AppendChild(_xmlRabDump.CreateTextNode(tbDb.Text));
                dsRabDump.AppendChild(_xmlRabDump.CreateElement("user")).AppendChild(_xmlRabDump.CreateTextNode(tbUser.Text));
                dsRabDump.AppendChild(_xmlRabDump.CreateElement("password")).AppendChild(_xmlRabDump.CreateTextNode(tbPwd.Text));
                _optsRabDump.AppendChild(dsRabDump);
                _xmlRabDump.Save(FilenameRabDump);
            }
            catch 
            {
            }

            try
            {
                XmlElement dsRabNetDS = _xmlRabNet.CreateElement("rabnetds");
                XmlElement dsRabNet = _xmlRabNet.CreateElement("dataSource");
                dsRabNet.Attributes.Append(_xmlRabNet.CreateAttribute("default")).Value = "1";
                dsRabNet.Attributes.Append(_xmlRabNet.CreateAttribute("name")).Value = tbName.Text;
                dsRabNet.Attributes.Append(_xmlRabNet.CreateAttribute("type")).Value = "db.mysql";
                String param = "host=" + tbHost.Text + ";database=" + tbDb.Text + ";uid=" + tbUser.Text + ";pwd=" + tbPwd.Text + ";charset=utf8";
                dsRabNet.Attributes.Append(_xmlRabNet.CreateAttribute("param")).Value = param;
                _optsRabNet.AppendChild(dsRabNetDS);
                dsRabNetDS.AppendChild(dsRabNet);
                _xmlRabNet.Save(FilenameRabNet);
            }
            catch
            {
            }

            Close();
        }

        private void RunMia(String prm)
        {
            String prms = "\"" + prm + "\" " + tbHost.Text + ';' + tbDb.Text + ';' + tbUser.Text + ';' + tbPwd.Text + ';' + tbRoot.Text + ';' + tbRPwd.Text;
            prms += " зоотехник;";
            String prg = Path.GetDirectoryName(Application.ExecutablePath) + @"\mia_conv.exe";
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
