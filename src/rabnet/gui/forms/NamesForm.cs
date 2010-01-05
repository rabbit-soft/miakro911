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
//        private bool manual=true;
        string originName, originSurname = null;
        string[] btext = new string[] {"Добавить","Изменить" };

        public NamesForm(byte sex)
        {
            initNameForm();
            if (sex == 0) tabControl1.SelectedIndex = 0; else tabControl1.SelectedIndex = 1;
        }

        public NamesForm()
        {
            initNameForm();
        }

        private void initNameForm()
        {
            InitializeComponent();
            cs = new ListViewColumnSorter(listView1, new int[] { });
//            manual = false;
            comboBox2.SelectedIndex = 0;
//            /manual = true;
        }

        private void load()
        {
            textBox1.Clear();
            textBox2.Clear();
            button1.Text = btext[0];
            this.originName = this.originSurname = null;
            button1.Enabled = button2.Enabled = false;
            comboBox2.Focus();
            rabStatusBar1.run();
        }

        private IDataGetter rabStatusBar1_prepareGet(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView1.Hide();
            listView1.ListViewItemSorter = null;
            Filters f=new Filters();
            if (tabControl1.SelectedIndex == 0) f["sex"] = "1"; else f["sex"] = "2";         
            if (comboBox2.SelectedIndex!=0)
                f["state"] = comboBox2.SelectedIndex.ToString();
            IDataGetter gt = DataThread.db().getNames(f);
            rabStatusBar1.setText(1, gt.getCount().ToString() + " имен");
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
                return;
            }
            rabnet.Name nm=(e.data as rabnet.Name);
            ListViewItem li = listView1.Items.Add(nm.name);
            li.Tag = nm.id;
            li.SubItems.Add(nm.surname);
            li.SubItems.Add(nm.sex);
            li.SubItems.Add(nm.use != 0 ? "занято" : "свободно");
            li.SubItems.Add((nm.use!=0 || nm.td==DateTime.MinValue)?"-":nm.td.ToShortDateString());
        }

        private void NamesForm_Activated(object sender, EventArgs e)
        {
            load();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1 || listView1.SelectedItems[0]==null)
                return;
            button1.Enabled = button2.Enabled = true;
            try
            {
                this.originName = textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
                this.originSurname = textBox2.Text = listView1.SelectedItems[0].SubItems[1].Text;
            }
            catch (ArgumentOutOfRangeException )
            {
                return;
            }
            button1.Text = btext[1];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button1.Text == btext[1]) listView1.SelectedItems.Clear();
            textBox1.Clear();
            textBox2.Clear();
            button1.Text = btext[0];
            this.originName = this.originSurname = null;
            button1.Enabled = button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (button1.Text == btext[0])
                {
                    OneRabbit.RabbitSex sx = OneRabbit.RabbitSex.MALE;
                    if (tabControl1.SelectedIndex == 1) sx = OneRabbit.RabbitSex.FEMALE;
                    Engine.get().db().addName(sx, textBox1.Text, textBox2.Text);
                }
                else
                {
                    Engine.get().db().changeName(this.originName, textBox1.Text, textBox2.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка:" + ex.GetType().ToString() + " " + ex.Message);
            }
            load();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            if (textBox1.Text != "" && textBox2.Text != "") button1.Enabled = true; else button1.Enabled = false;
            if (textBox1.Text != "" || textBox2.Text != "") button2.Enabled = true; else button2.Enabled = false;
        }

    }
}
