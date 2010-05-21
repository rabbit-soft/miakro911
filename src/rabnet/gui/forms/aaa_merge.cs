using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace rabnet
{
    public partial class aaa_merge : Form
    {
        public aaa_merge()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if ((sender as Button).Name == "button1") textBox1.Text = openFileDialog1.FileName;
            else textBox2.Text = openFileDialog1.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StreamReader sr1 = new StreamReader(textBox1.Text);
            StreamReader sr2 = new StreamReader(textBox2.Text);
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            while (!sr1.EndOfStream)
            {
                list1.Add(sr1.ReadLine());
            }
            sr1.Close();
            while (!sr2.EndOfStream)
            {
                list2.Add(sr2.ReadLine());
            }
            sr2.Close();
            
            for (int i1=0;i1<list1.Count;i1++)
            {
                bool flag = false;
                for (int i2=0;i2<list2.Count;i2++)
                {                  
                    if (list1[i1].ToString() == list2[i2].ToString())
                    {
                        flag = true;   
                    }
                }
                if (!flag) listBox1.Items.Add(list1[i1].ToString());
                
            }
            
        }
    }
}
