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
        public int Result = 0;

        public InstallForm()
        {
            InitializeComponent();
        }

        public class ReverseComparer : IComparer
        {
            public int Compare(Object object1, Object object2)
            {
                return -((IComparable)object1).CompareTo(object2);
            }
        }

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

        private void runMia(String prm) //в RabDump есть аналогичная функция
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

        private void btCheck_Click(object sender, EventArgs e)
        {
            try
            {
                TopMost = false;
                if (rbJustExit.Checked)
                {
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
                    runMia("nudb");
                }
                else if (rbImportFromMia.Checked)
                {
                    runMia(tbFile.Text);
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
