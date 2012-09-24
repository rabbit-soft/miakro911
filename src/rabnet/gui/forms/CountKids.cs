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
                comboBox1.Items.Add(r.Youngers[i].NameFull + " (" + r.Youngers[i].Group+")");
            comboBox1.SelectedIndex = grp;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int s = comboBox1.SelectedIndex;
            int c = r.Youngers[s].Group;
            int x = c - (int)(nudDead.Value + nudKilled.Value)+(int)nudAdd.Value;
            tbAlive.Text = x.ToString();
            nudKilled.Maximum = c - nudDead.Value;
            nudDead.Maximum = c - nudKilled.Value;
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
                r.CountKids((int)nudDead.Value, (int)nudKilled.Value, (int)nudAdd.Value,
                    int.Parse(tbAlive.Text), r.Youngers[i].Age,i);
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
            label2.Text = "Возраст:" + r.Youngers[i].Age.ToString() +"\nПорода:"+r.BreedName.ToString();
            tbAlive.Text = r.Youngers[i].Group.ToString();
            nudKilled.Value = nudDead.Value=nudAdd.Value= 0;
            nudKilled.Maximum = nudDead.Maximum = r.Youngers[i].Group;

        }

    }
}
