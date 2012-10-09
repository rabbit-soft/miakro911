using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections;
using log4net;

namespace updater
{
    public partial class InstallForm : Form
    {
        private ILog _logger = LogManager.GetLogger(typeof(UpdateForm));
        //bool _batch = false;
        public int Result = 0;
        //public string FilenameRabDump = "";
        //public string FilenameRabNet = "";
        //private XmlDocument _xmlRabDump = new XmlDocument();
        //private XmlDocument _xmlRabNet = new XmlDocument();
        //XmlElement _optsRabDump = null;
        //XmlElement _optsRabNet = null;

        public InstallForm()
        {
            InitializeComponent();
        }

        /*public InstallForm(String flRabDump, string flRabNet ,bool bt):this()
        {
            //_batch = bt;
            FilenameRabDump = flRabDump;
            FilenameRabNet = flRabNet;
            PrepareXmlRabDump();
            PrepareXmlRabNet();
            radioButton4.Visible = !bt;
            radioButton4.Enabled = !bt;
        }*/

        /*private const string ConfRabDump ="<?xml version=\"1.0\" encoding=\"utf-8\" ?><configuration><configSections>"+
"<section name=\"rabdumpOptions\" type=\"rabdump.RabdumpConfigHandler,rabdump\"/>"+
"<section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler,Log4net\"/>"+
"</configSections><rabdumpOptions></rabdumpOptions><log4net><appender name=\"FileAppender\" type=\"log4net.Appender.FileAppender\">"+
"<file value=\"dumplog.txt\" /><appendToFile value=\"true\" /><lockingModel type=\"log4net.Appender.FileAppender+MinimalLock\" />"+
"<layout type=\"log4net.Layout.PatternLayout\"><conversionPattern value=\"%date [%thread] %-5level %logger{2} [%property{NDC}] - %message%newline\" />"+
"</layout></appender><root><level value=\"DEBUG\" /><appender-ref ref=\"FileAppender\" /></root></log4net></configuration>";*/


        public class ReverseComparer : IComparer
        {
            public int Compare(Object object1, Object object2)
            {
                return -((IComparable)object1).CompareTo(object2);
            }
        }

        /*private void PrepareXmlRabNet()
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
            } 
            catch{}
        }*/

        /// <summary>
        /// Проверяет содержит ли XML-файл элемент 'rabnetds'
        /// </summary>
        /// <param name="fl">Файл конфигурафии rabNet'a</param>
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

        /// <summary>
        /// Проверяет содержит ли XML-файл элемент 'db' и 'job'
        /// </summary>
        /// <param name="fl">Файл конфигурафии rabDump'a</param>
        public static bool TestRabDumpConfig(string fl)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(fl);
                foreach (XmlNode n in xml.DocumentElement.ChildNodes)
                {
                    if (n.Name == "rabdumpOptions")
                        return true;                  
                }
            }
            catch
            {
                return false;
            }
            return false;
        }  

        private void btExtended_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = !groupBox2.Visible;
            Height = (groupBox2.Visible ? 450 : 310);
            btExtended.Text = "Расширенный режим " + (groupBox2.Visible ? "<<" : ">>");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            tbComp.Enabled = rbRemoteDb.Checked;
            tbFile.Enabled = btFile.Enabled = rbImportFromMia.Checked;
            if (rbImportFromMia.Checked)
            {
                btCheck.Enabled = tbFile.Text != "";
            }
            else if (rbRemoteDb.Checked)
            {
                btCheck.Enabled = tbComp.Text != "";
            }
            else
            {
                btCheck.Enabled = true;
            }
        }

        private void tbComp_TextChanged(object sender, EventArgs e)
        {
            tbHost.Text = tbComp.Text;
            btCheck.Enabled = tbComp.Text != "";
        }

        private void tbHost_TextChanged(object sender, EventArgs e)
        {
            tbComp.Text = tbHost.Text;
        }

        private void btFile_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
                tbFile.Text = ofd.FileName;
        }

        private void AddFarm()
        {
            Program.RNC.LoadDataSources();
            Program.RNC.SaveDataSource(System.Guid.NewGuid().ToString(), tbName.Text, tbHost.Text, tbDb.Text, tbUser.Text, tbPwd.Text);
            Program.RNC.SaveDataSources();          

            Close();
        }

        private void RunMia(String prm) //в RabDump есть аналогичная функция
        {
            try
            {
                rabnet.Run.DBCreate(prm, tbHost.Text, tbDb.Text, tbUser.Text, tbPwd.Text, tbRoot.Text, tbRPwd.Text);
            }
            catch (Exception exc)
            {
                _logger.Error(exc);
                MessageBox.Show(exc.Message);
            }
        }

//        private void RunMia(String prm) //в RabDump есть аналогичная функция
//        {
//            String prms = "\"" + prm + "\" " + tbHost.Text + ';' + tbDb.Text + ';' + tbUser.Text + ';' + tbPwd.Text + ';' + tbRoot.Text + ';' + tbRPwd.Text;
//            prms += " зоотехник;";
//            String prg = Path.GetDirectoryName(Application.ExecutablePath) + @"\mia_conv.exe";
//#if DEBUG
//            if (!File.Exists(prg))//нужно для того чтобы из под дебага можно было запустить Mia_Conv
//            {
//                string path = Path.GetFullPath(Application.ExecutablePath);
//                bool recurs = true;
//                string[] drives = Directory.GetLogicalDrives();
//                while (recurs)
//                {
//                    foreach (string d in drives)
//                    {
//                        if (d.ToLower() == path)
//                            recurs = false;
//                    }
//                    if (!recurs) break;
//                    path = Directory.GetParent(path).FullName;
//                    string[] dirs = Directory.GetDirectories(path);
//                    if (Directory.Exists(path + @"\bin\protected\Tools"))
//                    {
//                        prg = path + @"\bin\protected\Tools\mia_conv.exe";
//                        recurs = false;
//                        break;
//                    }
//                }
//            }
//#endif
//            Process p = Process.Start(prg, prms);
//            p.WaitForExit();
//            if (p.ExitCode != 0)
//                throw new ApplicationException("Ошибка создания БД. " + miaExitCode.GetText(p.ExitCode));
//        }

        private void btCheck_Click(object sender, EventArgs e)
        {
            try
            {
                TopMost = false;
                if (rbJustExit.Checked)
                {
                    //Application.Exit(
                    Environment.Exit(10);
                }
                if (tbName.Text == "")
                    throw new ApplicationException("Введите название фермы");
                if (rbRemoteDb.Checked && tbComp.Text=="")
                    throw new ApplicationException("Введите адрес компьютера");
                if (rbImportFromMia.Checked && tbFile.Text == "")
                    throw new ApplicationException("Выберите файл");

                if (rbMakeNew.Checked)
                {
                    RunMia("nudb");
                }
                else if (rbImportFromMia.Checked)
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
            btCheck.Enabled = tbFile.Text != "";
        }
    }
}
