using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using log4net;

namespace rabnet
{
    public partial class FarmChangeForm : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(FarmChangeForm));
        private RabnetConfig.rabDataSource ds = null;
        private bool noFarms = false;

        public bool MiniMode = false;

        public FarmChangeForm()
        {
            InitializeComponent();
            Size = new Size(325, 425);
            MiniMode = false;
            panel2.Visible = false;
            panel2.Left = 14;
            panel2.Top = 44;
            ChangeMode(MiniMode);
        }

        public FarmChangeForm(bool noFarms): this()
        {
            this.noFarms = noFarms;
            groupBox2.Enabled = true;
            Text = "Выберите ферму";
            if (noFarms)
            {
                fname.Text = "Основная ферма";
                fhost.Text="localhost";
                MiniMode = true;
            }
            ChangeMode(MiniMode);
        }

        private void ChangeMode(bool mini)
        {
            if (mini)
            {
                Size = new Size(325, 150);
                panel1.Visible = false;
                panel2.Visible = true;
                button6.Text = "Подробней";
                fhostm.Text = fhost.Text;
            }
            else
            {
                Size = new Size(325, 425);
                panel2.Visible = false;
                panel1.Visible = true;
                button6.Text = "Меньше";
                fhost.Text = fhostm.Text;
            }
            MiniMode = mini;
        }

        public FarmChangeForm(RabnetConfig.rabDataSource ds):this()
        {
            this.ds = ds;
            if (ds == null)
            {
                groupBox2.Enabled = true;
                //MiniMode = true;
                //ChangeMode(MiniMode);
            }
            else
            {
                fname.Text = ds.Name;
                fsavepswd.Checked = ds.SavePassword;
                fhost.Text = fdb.Text = fuser.Text = fpswd.Text = "";
                foreach(String s in ds.Params.ToString().Split(';'))
                {
                    String[] prm = s.Split('=');
                    switch (prm[0].ToLower())
                    {
                        case "host":
                        case "data source": fhost.Text = prm[1]; break;
                        case "uid":
                        case "user id": fuser.Text = prm[1]; break;
                        case "pwd":
                        case "password": fpswd.Text = prm[1]; break;
                        case "database": fdb.Text = prm[1]; break;
                    }
                }

            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            String constr;
            if (MiniMode)
            {
                constr = "host=" + fhostm.Text + ";database=" + fdb.Text + ";uid=" + fuser.Text + ";pwd=" + fpswd.Text + ";charset=utf8";
            }
            else
            {
                constr = "host=" + fhost.Text + ";database=" + fdb.Text + ";uid=" + fuser.Text + ";pwd=" + fpswd.Text + ";charset=utf8";
            }
            if (ds != null)
            {
                ds.Name = fname.Text;
                ds.SavePassword = fsavepswd.Checked;
                ds.Params = new RabnetConfig.sParams(constr);
            }
            else
            {
                RabnetConfig.rabDataSource mds = new RabnetConfig.rabDataSource(System.Guid.NewGuid().ToString(),fname.Text,"db.mysql",constr);
                mds.SavePassword = fsavepswd.Checked;
                RabnetConfig.DataSources.Add(mds);
            }
            try
            {
                RabnetConfig.SaveDataSources();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
                log.Debug(exp.Message);
                Environment.Exit(1000);
            }
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btImportDB_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                if (runmia(openFileDialog1.FileName))
            {
                btOk.PerformClick();
            }
        }

        /// <summary>
        /// Запустить mia_conv
        /// </summary>
        private bool runmia(String prm)
        {
            String prms = "\""+prm + "\" " + fhost.Text + ';' + fdb.Text + ';' + fuser.Text + ';' + fpswd.Text + ';' + ruser.Text + ';' + rpswd.Text;
            prms += " зоотехник;";
            try
            {
                String prg = Path.GetDirectoryName(Application.ExecutablePath) + @"\..\Tools\mia_conv.exe";
#if DEBUG
                if (!File.Exists(prg))//нужно для того чтобы из под дебага можно было запустить Mia_Conv
                {
                    string path = Path.GetFullPath(Application.ExecutablePath);
                    bool recurs = true;
                    string[] drives = Directory.GetLogicalDrives();
                    while (recurs)
                    {
                        foreach (string d in drives)
                        {
                            if (d.ToLower() == path)                            
                                recurs = false;                            
                        }
                        if (!recurs) break;
                        path = Directory.GetParent(path).FullName;
                        string[] dirs = Directory.GetDirectories(path);
                        if (Directory.Exists(path + @"\bin\protected\Tools"))
                        {
                            prg = path + @"\bin\protected\Tools\mia_conv.exe";
                            recurs = false;
                            break;
                        }
                    }
                }
#endif
                Process p = Process.Start(prg, prms);
                p.WaitForExit();
                if (p.ExitCode != 0)             
                    throw new ApplicationException("Ошибка создания БД. " +miaExitCode.GetText(p.ExitCode));
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        private void btCreateDB_Click(object sender, EventArgs e)
        {
            if (runmia("nudb"))
                btOk.PerformClick();
        }

        private void FarmChangeForm_Shown(object sender, EventArgs e)
        {
            if (noFarms)
            {
                if (MiniMode)
                {
                    fhostm.Text = "localhost";
                    fhostm.Focus();
                    fhostm.SelectAll();
                }
                else
                {
                    fhost.Text = "localhost";
                    fhost.Focus();
                    fhost.SelectAll();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ChangeMode(!MiniMode);
        }

    }
}
