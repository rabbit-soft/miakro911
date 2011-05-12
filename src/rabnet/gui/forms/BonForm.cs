using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class BonForm : Form
    {
        private RabNetEngRabbit r;
        public BonForm()
        {
            InitializeComponent();
        }

        public BonForm(int rid)
            : this()
        {
            r = Engine.get().getRabbit(rid);
            label2.Text = r.FullName;
            label3.Text = r.BreedName;
            String bn = r.Bon;
            label7.Text = "классность " + (bn[0] == '1' ? "установлена в ручную" : "унаследована");
            comboBox1.SelectedIndex = int.Parse("" + bn[1]);
            comboBox2.SelectedIndex = int.Parse("" + bn[2]);
            comboBox3.SelectedIndex = int.Parse("" + bn[3]);
            comboBox4.SelectedIndex = int.Parse("" + bn[4]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String bon = "1" + comboBox1.SelectedIndex.ToString() + comboBox2.SelectedIndex.ToString()+
                comboBox3.SelectedIndex.ToString() + comboBox4.SelectedIndex.ToString();
            r.setBon(bon);
            Close();
        }
    }
}
