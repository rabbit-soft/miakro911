using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace updater
{
    public partial class InstallForm : Form
    {
        bool batch = false;
        public int result = 0;
        public string filename = "";
        private XmlDocument xml = new XmlDocument();
        private XmlElement rabnet = null;
        public InstallForm()
        {
            InitializeComponent();
        }
        public InstallForm(String fl,bool bt):this()
        {
            batch = bt;
            filename = fl;
            prepareXml();
            radioButton4.Visible = !bt;
            radioButton4.Enabled = !bt;
        }
        private string conf="<?xml version=\"1.0\" encoding=\"utf-8\" ?>"+
"<configuration><configSections><section name=\"rabnetds\" type=\"rabnet.RabnetConfigHandler,rabnet\"/>"+
"<section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler,Log4net\"/>"+
"</configSections><rabnetds/><log4net><appender name=\"FileAppender\" type=\"log4net.Appender.FileAppender\">"+
"<file value=\"log.txt\" /><appendToFile value=\"true\" />"+
"<lockingModel type=\"log4net.Appender.FileAppender+MinimalLock\" /><layout type=\"log4net.Layout.PatternLayout\">"+
"<conversionPattern value=\"%date [%thread] %-5level %logger{2} [%property{NDC}] - %message%newline\" />"+
"</layout></appender><root><level value=\"DEBUG\" /><appender-ref ref=\"FileAppender\" /></root></log4net>"+
"</configuration>";

        public void prepareXml()
        {
            xml.LoadXml(conf);
            xml.Save(filename);
            foreach (XmlNode n in xml.DocumentElement.ChildNodes)
            {
                if (n.Name == "rabnetds")
                    rabnet = (XmlElement)n;
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

        private void addFarm()
        {
            XmlElement ds = xml.CreateElement("dataSource");
            ds.Attributes.Append(xml.CreateAttribute("default")).Value = "1";
            ds.Attributes.Append(xml.CreateAttribute("name")).Value = tbName.Text;
            ds.Attributes.Append(xml.CreateAttribute("type")).Value = "db.mysql";
            String param = "host=" + tbHost.Text + ";database=" + tbDb.Text + ";uid=" + tbUser.Text + ";pwd=" + tbPwd.Text + ";charset=utf8";
            ds.Attributes.Append(xml.CreateAttribute("param")).Value = param;
            rabnet.AppendChild(ds);
            xml.Save(filename);
            Close();
        }

        private void runmia(String prm)
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
                    System.Environment.Exit(10);
                }
                if (tbName.Text == "")
                    throw new ApplicationException("Введите название фермы");
                if (radioButton2.Checked && tbComp.Text=="")
                    throw new ApplicationException("Введите адрес компьютера");
                if (radioButton3.Checked && tbFile.Text == "")
                    throw new ApplicationException("Выберите файл");
                if (radioButton1.Checked)
                {
                    runmia("nudb");
                }
                else if (radioButton3.Checked)
                {
                    runmia(tbFile.Text);
                }
                addFarm();
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
