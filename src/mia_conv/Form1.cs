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

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < clb1.Items.Count; i++)
                clb1.SetItemChecked(i, false);
            clb1.SetItemChecked(clb1.Items.Count-1, true);

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
        }


        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            DateTime dt = dateTimePicker1.Value;
            DateTime dt2 = new DateTime(1899, 12, 30);
            TimeSpan sp = dt - dt2;
            MessageBox.Show("Days=" + String.Format("{0:d}({0:X})", sp.Days));
        }
    }
}