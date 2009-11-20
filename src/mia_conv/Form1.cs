using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mia_conv
{
    public partial class Form1 : Form
    {
        private MiaFile mia=null;
        public DataTable udata=new DataTable();
        private bool auto=false;
        public Form1()
        {
            udata.Columns.Add("Пользователь",typeof(String));
            udata.Columns.Add("Пароль", typeof(String));
            udata.Rows.Add("зоотехник","");
            InitializeComponent();
            dataGridView1.DataSource = udata;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridView1.AutoSize = true;
            for (int i = 0; i < clb1.Items.Count; i++)
                clb1.SetItemChecked(i, false);
            clb1.SetItemChecked(clb1.Items.Count-1, true);

        }
        public Form1(bool automode,String file,String h,String db,String u,String p,String r,String rp,String usrs,String scr):this()
        {
            auto = automode;
            if (auto)
            {
                Text += " - AUTOMODE do not touch";
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
                udata.Clear();
                String[] us = usrs.Split(';');
                for (int i = 0; i < us.Length / 2; i++)
                {
                    udata.Rows.Add(us[i * 2], us[i * 2 + 1]);
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
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mia = new MiaFile(clb1);
            mia.LoadFromFile(tb1.Text, log);
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
            MDCreator crt = new MDCreator(log);
            if (crt.prepare(dbnew.Checked, textHost.Text, textUser.Text, textPassword.Text, textDB.Text, textRoot.Text, textRootPswd.Text))
            {
                button2.Enabled = false;
                button3.Enabled = false;
                //crt.oldid = oldid.Checked;
                crt.mia = mia;
                crt.setUsers(udata);
                crt.fillAll();
                crt.finish(textBox1.Text);
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (auto)
            {
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