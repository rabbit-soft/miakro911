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
        private int grp=0;
        public CountKids()
        {
            InitializeComponent();
        }

        public CountKids(int id):this(id,false){}
        public CountKids(RabNetEngRabbit r):this(r,false){}
        public CountKids(int id, bool suckers):this(Engine.get().getRabbit(id),suckers){}
        public CountKids(RabNetEngRabbit r,bool suckers):this()
        {
            this.r = r;
            if (suckers)
                Text = "Подсчет подсосных";
        }
        public void setGrp(int grp)
        {
            this.grp=grp;
        }
        private void CountKids_Load(object sender, EventArgs e)
        {
            label1.Text = r.FullName;
            comboBox1.Items.Clear();
            for (int i = 0; i < r.Youngers.Length; i++)
                comboBox1.Items.Add(r.Youngers[i].fullname + " (" + r.Youngers[i].group+")");
            comboBox1.SelectedIndex = grp;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int s = comboBox1.SelectedIndex;
            int c = r.Youngers[s].group;
            int x = c - (int)(numericUpDown1.Value + numericUpDown2.Value)+(int)numericUpDown3.Value;
            textBox1.Text = x.ToString();
            numericUpDown2.Maximum = c - numericUpDown1.Value;
            numericUpDown1.Maximum = c - numericUpDown2.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int i = comboBox1.SelectedIndex;
                r.CountKids((int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value,
                    int.Parse(textBox1.Text), r.Youngers[i].age(),i);
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboBox1.SelectedIndex;
            label2.Text = "Возраст:" + r.Youngers[i].age().ToString() +"\nПорода:"+r.BreedName.ToString();
            textBox1.Text = r.Youngers[i].group.ToString();
            numericUpDown2.Value = numericUpDown1.Value=numericUpDown3.Value= 0;
            numericUpDown2.Maximum = numericUpDown1.Maximum = r.Youngers[i].group;

        }

    }
}
