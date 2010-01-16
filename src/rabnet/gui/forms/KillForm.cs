using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class KillForm : Form
    {
        private List<RabNetEngRabbit> rbs = new List<RabNetEngRabbit>();
        public KillForm()
        {
            InitializeComponent();
            dateDays1.DateValue = DateTime.Now;
            update();
        }

        public void addRabbit(int id)
        {
            addRabbit(Engine.get().getRabbit(id));
        }

        public void addRabbit(RabNetEngRabbit r)
        {
            rbs.Add(r);
            foreach (OneRabbit or in r.youngers)
                addRabbit(or.id);
            update();
        }

        public void update()
        {
            listView1.Items.Clear();
            foreach (RabNetEngRabbit r in rbs)
            {
                ListViewItem li = listView1.Items.Add(r.fullName);
                li.Tag=r.rid;
                li.SubItems.Add(r.address);
                String sex = "?";
                if (r.sex == OneRabbit.RabbitSex.FEMALE) sex = "Ж";
                if (r.sex == OneRabbit.RabbitSex.MALE) sex = "М";
                li.SubItems.Add(sex);
                li.SubItems.Add(r.age.ToString());
                li.SubItems.Add(r.group.ToString());
                li.SubItems.Add(r.group.ToString());
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void KillForm_Load(object sender, EventArgs e)
        {
            update();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled=false;
            if (listView1.SelectedItems.Count ==1)
            {
                numericUpDown1.Maximum = int.Parse(listView1.SelectedItems[0].SubItems[4].Text);
                numericUpDown1.Value = int.Parse(listView1.SelectedItems[0].SubItems[5].Text);
                numericUpDown1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i=0;
            foreach (RabNetEngRabbit r in rbs)
            {
                int cnt = int.Parse(listView1.Items[i].SubItems[5].Text);
                if (cnt != 0)
                {
                    r.killIt(dateDays1.DateValue,/*comboBox1.SelectedIndex*/0, textBox1.Text,cnt);
                }
                i++;
            }
            Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            listView1.SelectedItems[0].SubItems[5].Text = numericUpDown1.Value.ToString();
        }
    }
}
