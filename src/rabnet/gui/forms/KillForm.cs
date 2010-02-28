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
        private List<int> youngers = new List<int>();
        bool confirm = true;
        private ListViewColumnSorter cs = null;
        public KillForm()
        {
            InitializeComponent();
            initialHints();
            dateDays1.DateValue = DateTime.Now;
            confirm = Engine.opt().getIntOption(Options.OPT_ID.CONFIRM_KILL) == 1;
            Catalog c=Engine.db().catalogs().getDeadReasons();
            comboBox1.Items.Add("");
            for (int i = 0; i < c.Count; i++)
                comboBox1.Items.Add(c[i+3]);
            cs = new ListViewColumnSorter(listView1, new int[] { 3, 4, 5 }, Options.OPT_ID.KILL_LIST);
            update();
        }

        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;

            toolTip.SetToolTip(numericUpDown1, "Количество списываемых кроликов из выделенной записи");
            toolTip.SetToolTip(button1,"Убрать выделенную запись из плана списаний");
            toolTip.SetToolTip(button2,"Списать кроликов");
            toolTip.SetToolTip(dateDays1,"Дата списания");
            toolTip.SetToolTip(comboBox1,"Причина списания кроликов");
            toolTip.SetToolTip(textBox1, "Здесь можно оставить любой коментарий по данному списанию");
        }
        public void addRabbit(int id)
        {
            //try
            {
                addRabbit(Engine.get().getRabbit(id));
            }
            /*
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
             * */
        }

        public void addRabbit(RabNetEngRabbit r)
        {
            rbs.Add(r);
            int id = rbs.Count - 1;
            r.tag = r.address;
            foreach (OneRabbit or in r.youngers)
            {
                addRabbit(or.id);
                rbs[rbs.Count - 1].tag = r.address;
            }
            update();
        }

        public void update()
        {
            cs.Prepare();
            listView1.Items.Clear();
            foreach (RabNetEngRabbit r in rbs)
            {
                ListViewItem li = listView1.Items.Add(r.fullName);
                li.Tag=r.rid;
                li.SubItems.Add(r.tag);
                String sex = "?";
                if (r.sex == OneRabbit.RabbitSex.FEMALE) sex = "Ж";
                if (r.sex == OneRabbit.RabbitSex.MALE) sex = "М";
                li.SubItems.Add(sex);
                li.SubItems.Add(r.age.ToString());
                li.SubItems.Add(r.group.ToString());
                li.SubItems.Add(r.group.ToString());
            }
            //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            cs.Restore();
            updateLabels();
        }

        private void KillForm_Load(object sender, EventArgs e)
        {
            update();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled=false;
            button1.Enabled = false;
            if (listView1.SelectedItems.Count ==1)
            {
                numericUpDown1.Maximum = int.Parse(listView1.SelectedItems[0].SubItems[4].Text);
                numericUpDown1.Value = int.Parse(listView1.SelectedItems[0].SubItems[5].Text);
                numericUpDown1.Enabled = true;
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool doConfirm()
        {
            if (!confirm) return true;
            return MessageBox.Show(this,"Списать "+updateLabels().ToString()+" кроликов?","Подтверждение",
                MessageBoxButtons.YesNo)==DialogResult.Yes; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                if (!doConfirm())
                {
                    DialogResult = DialogResult.None;
                    return;
                }
                foreach (RabNetEngRabbit r in rbs)
                {
                    int cnt = int.Parse(listView1.Items[i].SubItems[5].Text);
                    if (cnt != 0)
                    {
                        int reason = 0;
                        if (comboBox1.SelectedIndex > 0)
                            reason = comboBox1.SelectedIndex + 2;
                        r.killIt(dateDays1.DateValue, reason, textBox1.Text, cnt);
                    }
                    i++;
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            listView1.SelectedItems[0].SubItems[5].Text = numericUpDown1.Value.ToString();
            updateLabels();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1) return;
            int id=(int)listView1.SelectedItems[0].Tag;
            foreach (RabNetEngRabbit r in rbs)
            {
                if (r.rid==id)
                {
                    rbs.Remove(r);
                    update();
                    return;
                }
            }
        }

        private int updateLabels()
        {
            int str = 0;
            int cnt = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                int c = int.Parse(listView1.Items[i].SubItems[5].Text);
                cnt += c;
                str +=(c != 0 ? 1 : 0);
            }
            label4.Text = "Строк: " + str.ToString();
            label5.Text = "Кроликов: " + cnt.ToString();
            return cnt;
        }
    }
}
