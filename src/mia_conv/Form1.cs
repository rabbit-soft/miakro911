//#define NOCATCH
using System;
using System.Data;
using System.Windows.Forms;

namespace mia_conv
{
    public partial class Form1 : Form
    {
        private MiaFile _mia=null;
        public DataTable Udata=new DataTable();
        private bool auto=false;
        private bool _executed=false;
        public bool Quiet=false;
        public Form1()
        {
            Udata.Columns.Add("������������",typeof(String));
            Udata.Columns.Add("������", typeof(String));
            Udata.Rows.Add("���������","");
            InitializeComponent();
            dataGridView1.DataSource = Udata;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridView1.AutoSize = true;
            for (int i = 0; i < clb1.Items.Count; i++)
                clb1.SetItemChecked(i, false);
            clb1.SetItemChecked(clb1.Items.Count-1, true);
            Environment.ExitCode = 1;
        }
        public Form1(int automode,String file,String h,String db,String u,String p,String r,String rp,String usrs,String scr):this()
        {
            auto = automode>0;
            Quiet = automode == 2;
            if (auto)
            {
                Text += " - ���� �����";
                tb1.Text = file;
                textHost.Text = h;
                textDB.Text = db;
                textUser.Text = u;
                textPassword.Text = p;
                if (r != "")
                {
                    dbnew.Checked = true;
                    textRoot.Text = r;
                    textRootPswd.Text = rp;
                }
                Udata.Clear();
                String[] us = usrs.Split(';');
                for (int i = 0; i < us.Length / 2; i++)
                {
                    Udata.Rows.Add(us[i * 2], us[i * 2 + 1]);
                }
                textBox1.Text = scr;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                tb1.Text = ofd.FileName;
                button2.Enabled = true;
                button2.PerformClick();
                Text = "������ ����� " + tb1.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _mia = new MiaFile(clb1,pb,label11);
            _mia.LoadFromFile(tb1.Text, log);
            button3.Enabled = true;
        }


        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            DateTime dt = dateTimePicker1.Value;
            DateTime dt2 = new DateTime(1899, 12, 30);
            TimeSpan sp = dt - dt2;
            MessageBox.Show("Days=" + String.Format("{0:d}({0:X})", sp.Days));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Enabled = dbnew.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
//
/*#if PROTECTED
            int fms=PClient.get().farms();
            if (fms<0)
            {
                MessageBox.Show("���� ������ �� ������.");
                return;
            }
            if (fms < mia.builds.maxfarm)
            {
                MessageBox.Show(String.Format("������� ����� �������� � �����({0:d}).\n���� ������ ��������� �������� � {1:d} ��������.",mia.builds.maxfarm,fms));
                return;
            }
#endif*/
//
            MDCreator crt = new MDCreator(log);
#if !NOCATCH 
            try
#endif
            {
                pb.Value = 0;
                if (crt.Prepare(dbnew.Checked, textHost.Text, textUser.Text, textPassword.Text, textDB.Text, textRoot.Text, textRootPswd.Text,false,Quiet))
                {
                    button2.Enabled = false;
                    button3.Enabled = false;
                    //crt.oldid = oldid.Checked;
                    crt.Mia = _mia;
                    crt.SetUsers(Udata);
                    crt.FillAll();
                    crt.Finish(textBox1.Text);
                    pb.Value = 0;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    Environment.ExitCode = 0;
                }
            }
#if !NOCATCH 
            catch (Exception ex)
            {
                MessageBox.Show("��������� ������� ����������: "+ex.GetType().ToString()+"\r\n"+ex.Message);
                button2.Enabled = true;
                button3.Enabled = true;
            }
#endif
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (_executed)
                return;
            _executed = true;
            if (auto)
            {
                dbnew.Checked=true;
                button2.Enabled = true;
                button2.PerformClick();
                button3.PerformClick();
                Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ofd2.ShowDialog()==DialogResult.OK)
                textBox1.Text=ofd2.FileName;
        }
    }
}