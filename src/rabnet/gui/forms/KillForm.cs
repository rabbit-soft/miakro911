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
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void KillForm_Load(object sender, EventArgs e)
        {
            update();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = (listView1.SelectedItems.Count > 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView1.SelectedItems)
            {
                int j = 0;
                while(j<rbs.Count)
                {
                    if (rbs[j].rid == (int)li.Tag)
                        rbs.RemoveAt(j);
                    else
                        j++;
                }
            }
            update();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (RabNetEngRabbit r in rbs)
            {
                r.killIt(dateDays1.DateValue,/*comboBox1.SelectedIndex*/0, textBox1.Text);
            }
            Close();
        }
    }
}
