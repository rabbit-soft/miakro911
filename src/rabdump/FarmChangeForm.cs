using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace rabdump
{
    public partial class FarmChangeForm : Form
    {
        public FarmChangeForm()
        {
            InitializeComponent();
            groupBox2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //String constr="host="+fhost.Text+";database="+fdb.Text+";uid="+fuser.Text+";pwd="+fpswd.Text+";charset=utf8";
            DataBase db = new DataBase(fname.Text);

            db.DBName = fdb.Text;
            db.Host = fhost.Text;
            db.User = fuser.Text;
            db.Password = fpswd.Text;

            Options.Get().Databases.Add(db);
            Options.Get().Save();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                if (Runmia(openFileDialog1.FileName))
            {
                button1.PerformClick();
            }
        }

        private bool Runmia(String prm)
        {
            String prms = "\""+prm + "\" " + fhost.Text + ';' + fdb.Text + ';' + fuser.Text + ';' + fpswd.Text + ';' + ruser.Text + ';' + rpswd.Text;
            prms += " зоотехник;";
            try{
                String prg = Path.GetDirectoryName(Application.ExecutablePath) + "\\mia_conv.exe";
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
            if (Runmia("nudb"))
                button1.PerformClick();
        }
    }
}
