using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class NamesForm : Form
    {
        ListViewColumnSorter cs = null;
        private bool manual = true;
        string[] btext = new string[] {"Добавить","Изменить" };
        public NamesForm()
        {
            InitializeComponent();
            cs = new ListViewColumnSorter(listView1, new int[] { });
            manual = false;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            manual = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (button1.Enabled)
                return;
            if (manual)
                rabStatusBar1.run();
        }

        private IDataGetter rabStatusBar1_prepareGet(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView1.Hide();
            listView1.ListViewItemSorter = null;
            Filters f=new Filters();
            if (comboBox1.SelectedIndex!=0)
                f["sex"] = comboBox1.SelectedIndex.ToString();
            if (comboBox2.SelectedIndex!=0)
                f["state"] = comboBox2.SelectedIndex.ToString();
            if (textBox1.Text != "")
                f["name"] = textBox1.Text;
            IDataGetter gt = DataThread.db().getNames(f);
            rabStatusBar1.setText(1, gt.getCount().ToString() + " items");
            return gt;
        }

        private String makeSurname(String nm)
        {
            return nm;
        }

        private void rabStatusBar1_itemGet(object sender, RabStatusBar.RSBItemEvent e)
        {
            if (e.data == null)
            {
                listView1.ListViewItemSorter = cs.Clear();
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Show();
/*                if (listView1.Items.Count == 0 && comboBox1.SelectedIndex==0 && comboBox2.SelectedIndex==0 && textBox1.Text!="")
                {
                    button1.Enabled = button2.Enabled = true;
                    textBox2.Text = makeSurname(textBox1.Text);
                    comboBox1.SelectedIndex = 1;
                    comboBox2.Enabled = false;
                }
                if (listView1.Items.Count != 0 && textBox1.Text != "" && button1.Enabled)
                {
                    textBox2.Text = "";
                    button1.Enabled = button2.Enabled = false;
                    comboBox2.Enabled = true;
                    comboBox1.SelectedIndex = 0;
                }
*/                return;
            }
            rabnet.Name nm=(e.data as rabnet.Name);
            ListViewItem li = listView1.Items.Add(nm.name);
            li.Tag = nm.id;
            li.SubItems.Add(nm.surname);
            li.SubItems.Add(nm.sex);
            li.SubItems.Add(nm.use != 0 ? "занято" : "свободно");
            li.SubItems.Add(nm.use!=0?"-":nm.td.ToShortDateString());
        }

        private void NamesForm_Activated(object sender, EventArgs e)
        {
            rabStatusBar1.run();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1 || listView1.SelectedItems[0]==null)
                return;
            button1.Enabled = button2.Enabled = true;
            textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[1].Text;
            if (listView1.SelectedItems[0].SubItems[2].Text == "м")
                comboBox1.SelectedIndex = 1;
            else
                comboBox1.SelectedIndex = 2;
            comboBox1.Enabled = comboBox2.Enabled=false;
            button1.Text = btext[1];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text = "";
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = 0;
            button1.Text = btext[0];
            comboBox1.Enabled = comboBox2.Enabled = true;
            listView1.SelectedItems.Clear();
            button1.Enabled = button2.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (manual)
                rabStatusBar1.run();
        }
    }
}
