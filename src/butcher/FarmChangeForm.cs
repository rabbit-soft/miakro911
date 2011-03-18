using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace butcher
{
    public partial class FarmChangeForm : Form
    {
        private RabnetConfigHandler.dataSource ds = null;
        private bool noFarms = false;
        private bool _miniMode;
        public FarmChangeForm()
        {
            InitializeComponent();
            Size = new Size(325, 425);
            _miniMode = false;
            panel2.Visible = false;
            panel2.Left = 14;
            panel2.Top = 44;
            ChangeMode(_miniMode);
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
                _miniMode = true;
            }
            ChangeMode(_miniMode);
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
            _miniMode = mini;
        }

        public FarmChangeForm(RabnetConfigHandler.dataSource ds):this()
        {
            this.ds = ds;
            if (ds == null)
            {
                groupBox2.Enabled = true;
                _miniMode = true;
                ChangeMode(_miniMode);
            }
            else
            {
                fname.Text = ds.name;
                fsavepswd.Checked = ds.savepassword;
                fhost.Text = fdb.Text = fuser.Text = fpswd.Text = "";
                foreach(String s in ds.param.Split(';'))
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

        private void button1_Click(object sender, EventArgs e)
        {
            String constr;
            if (_miniMode)
            {
                constr = "host=" + fhostm.Text + ";database=" + fdb.Text + ";uid=" + fuser.Text + ";pwd=" + fpswd.Text + ";charset=utf8";
            }
            else
            {
                constr = "host=" + fhost.Text + ";database=" + fdb.Text + ";uid=" + fuser.Text + ";pwd=" + fpswd.Text + ";charset=utf8";
            }
            if (ds!=null)
            {
                ds.name = fname.Text;
                ds.savepassword = fsavepswd.Checked;
                ds.param = constr;
            }else{
                RabnetConfigHandler.dataSource mds=new RabnetConfigHandler.dataSource(fname.Text,"db.mysql",constr);
                mds.savepassword = fsavepswd.Checked;
                RabnetConfigHandler.dataSources.Add(mds);
            }
            try
            {
                RabnetConfigHandler.save();
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
                Environment.Exit(1000);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                if (runmia(openFileDialog1.FileName))
            {
                button1.PerformClick();
            }
        }

        private bool runmia(String prm)
        {
            String prms = "\""+prm + "\" " + fhost.Text + ';' + fdb.Text + ';' + fuser.Text + ';' + fpswd.Text + ';' + ruser.Text + ';' + rpswd.Text;
            prms += " зоотехник;";
            try{
                String prg = Path.GetDirectoryName(Application.ExecutablePath) + @"\..\Tools\mia_conv.exe";
                Process p=Process.Start(prg, prms);
                p.WaitForExit();
                if (p.ExitCode != 0)
                    throw new ApplicationException("Ошибка создания БД");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (runmia("nudb"))
                button1.PerformClick();
        }

        private void FarmChangeForm_Shown(object sender, EventArgs e)
        {
            if (noFarms)
            {
                if (_miniMode)
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
            ChangeMode(!_miniMode);
        }

    }
}
