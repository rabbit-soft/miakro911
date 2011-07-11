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
        private bool manual=true;
        string originName, originSurname = null;
        readonly string[] btext = new string[] {"Добавить","Изменить" };
        readonly string[] status = new string[] { "занято", "свободно", "освобождается" };

        public NamesForm(byte sex)
        {
            initNameForm();
            if (sex == 0) tabControl1.SelectedIndex = 0; else tabControl1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 1;
        }

        public NamesForm()
        {
            initNameForm();
            comboBox2.SelectedIndex = 0;
        }

        private void initNameForm()
        {
            InitializeComponent();
            cs = new ListViewColumnSorter(listView1, new int[] { },Options.OPT_ID.NAMES_LIST);
        }

        private void load()
        {
            textBox1.Clear();
            textBox2.Clear();
            button1.Text = btext[0];
            this.originName = this.originSurname = null;
            button1.Enabled = button2.Enabled = false;
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
            string state="занято";
            if (nm.use==0)
            {
                state = "свободно";
                if (nm.td != DateTime.MinValue)
                    state = "освобождается";
            }
            li.SubItems.Add(state);
            li.SubItems.Add((nm.use!=0 || nm.td==DateTime.MinValue)?"-":nm.td.ToShortDateString());
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
                    if (tabControl1.SelectedIndex == 1) 
                        sx = OneRabbit.RabbitSex.FEMALE;
                    Engine.get().db().addName(sx, textBox1.Text.Trim(), textBox2.Text.Trim());
                }
                else
                {
                    Engine.get().db().changeName(this.originName, textBox1.Text.Trim(), textBox2.Text.Trim());
                }
                load();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка: Такое имя уже существует");   
            }         
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!manual) return;
            manual = false;
            String txt = textBox1.Text;
            int i = 0;
            while (i < txt.Length)
            {
            
                if (i==0 && Char.IsLower(txt[0])) txt=Char.ToUpper(txt[0])+txt.Substring(1);
                if (i!=0 && Char.IsUpper(txt[i])) txt=txt.Substring(0,i)+Char.ToLower(txt[i])+txt.Substring(i+1);
                if ((txt[i] < 'A' || txt[i] > 'z')&&(txt[i] < 'А' || txt[i] > 'я') && (txt[i] < '0' || txt[i] > '9') && txt[i] != '-')
                    txt=txt.Remove(i,1);
                else
                    i++;
            }
            if (txt != textBox1.Text)
            {
                textBox1.Text = txt;
                textBox1.SelectionStart = txt.Length;
            }
            manual = true;
            if (textBox1.Text != "" && textBox2.Text != "") button1.Enabled = true; else button1.Enabled = false;
            if (textBox1.Text != "" || textBox2.Text != "") button2.Enabled = true; else button2.Enabled = false;
            if ((sender as TextBox).Name == textBox2.Name) return;
            textBox2.Text = makeSurname(textBox1.Text);            
        }

        private String makeSurname(String nm)
        {
            if (nm == "") return nm;
            if (nm.EndsWith("ъ")) nm = nm.Remove(nm.Length - 1);
            if (nm.EndsWith("ь")) return nm.Remove(nm.Length - 1) + "ев";
            if (nm.EndsWith("ч") || nm.EndsWith("ш")) return nm + "ев";
            string[] soglas = new string[] { "ц", "к", "н", "г", "щ", "з", "х", "ф", "в", "п", "р", "л", "д", "ж", "с", "м", "т", "б" };
            for (int i = 0; i < soglas.Length; i++)
                if (nm.EndsWith(soglas[i])) return nm + "ов";
            string[] glas = new string[] { "у", "а", "э", "и", "я", "й", "е", "ю", "ы", "о" };
            for (int i = 0; i < glas.Length; i++)
                if (nm.EndsWith(glas[i])) 
                    if(nm.Length >0)return nm.Remove(nm.Length - 1) + "ин";
                    else return nm + "ин";

            return nm;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            load();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) e.Cancel = true;
            if (listView1.SelectedItems[0].SubItems[3].Text != status[2]) e.Cancel = true;
        }

        private void miUnBlock_Click(object sender, EventArgs e)
        {
            int id = (int)listView1.SelectedItems[0].Tag;
            if (!Engine.db().unblockName(id))
            {
                MessageBox.Show("Невозможно разблокировать имя."+Environment.NewLine+"Возможно оно входит в состав чьей-то фамилии.");
                return;
            }
            rabStatusBar1.run();
        }

    }
}
