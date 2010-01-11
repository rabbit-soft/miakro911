using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class CountKids : Form
    {
        private RabNetEngRabbit r = null;
        public CountKids()
        {
            InitializeComponent();
        }

        public CountKids(int id):this(Engine.get().getRabbit(id)){}

        public CountKids(RabNetEngRabbit r):this()
        {
            this.r = r;
        }

        private void CountKids_Load(object sender, EventArgs e)
        {
            label1.Text = r.fullName;
            label2.Text = (DateTime.Now - r.youngers[0].born).Days.ToString();
            textBox1.Text = r.youngcount.ToString();
            numericUpDown2.Maximum = numericUpDown1.Maximum = r.youngcount;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int c = r.youngcount;
            int x = c - (int)(numericUpDown1.Value + numericUpDown2.Value);
            textBox1.Text = x.ToString();
            numericUpDown2.Maximum = c - numericUpDown1.Value;
            numericUpDown1.Maximum = c - numericUpDown2.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
