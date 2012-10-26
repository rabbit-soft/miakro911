using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class ReplaceYoungersForm : Form
    {
        const int REPLCOL = 8;
        private RabNetEngRabbit r = null;
        private ListViewColumnSorter cs = null;
        private int _selmom=0;
        public ReplaceYoungersForm()
        {
            InitializeComponent();
            cs = new ListViewColumnSorter(listView1, new int[] {1,4,6,7},Options.OPT_ID.REPL_YOUNG_LIST);
            listView1.ListViewItemSorter = cs;
            FormSizeSaver.Append(this);
        }

        public ReplaceYoungersForm(int rid):this()
        {
            r = Engine.get().getRabbit(rid);
            label1.Text = r.FullName;
            label2.Text = "Возраст: " + r.Age.ToString();
            nudCount.Value = nudCount.Maximum = r.Group;
            label5.Text = "Порода: " + r.BreedName;
        }
        public ReplaceYoungersForm(int rid,int selmom):this(rid)
        {
            this._selmom = selmom;
        }
        /// <summary>
        /// Заполняет ListView возможными кормилицами.
        /// </summary>
        public void updateMothers()
        {
            int ad = Engine.opt().getIntOption(Options.OPT_ID.COMBINE_AGE);
            cs.Prepare();
            listView1.Items.Clear();
            foreach (AdultRabbit moth in Engine.db().getMothers(r.Age, ad))
            {
                if (moth.ID != r.Parent)
                {
                    ListViewItem li = listView1.Items.Add(moth.NameFull);
                    li.SubItems.Add(moth.Age.ToString());
                    li.SubItems.Add(moth.BreedName);
                    li.SubItems.Add(moth.FStatus());
                    li.SubItems.Add(moth.Rate.ToString());
                    li.SubItems.Add(moth.FAddress());
                    li.SubItems.Add(moth.KidsAge.ToString());
                    if (_selmom == moth.ID)
                    {
                        li.SubItems.Add((moth.Group + r.Group).ToString()); //TODO выяснить зачем это
                        li.SubItems.Add(r.Group.ToString());
                        nudCount.Value = 0;
                        li.Selected = true;
                        li.EnsureVisible();
                    }
                    else
                    {
                        li.SubItems.Add(moth.KidsCount.ToString());
                        li.SubItems.Add("");
                    }
                    li.Tag = moth.ID;
                }
            }
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Нет кормилиц, которые сидят с крольчатами приближенного возраста."+Environment.NewLine+
                    "Увеличьте значение параметра \"Объединение группы\"."+Environment.NewLine+
                    "Либо отсадите кормилицу от молодняка.");
                btCancel.PerformClick();
                return;
            }
            String txt = listView1.Items[0].SubItems[REPLCOL].Text;
            listView1.Items[0].SubItems[REPLCOL].Text = "10";
            //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.Items[0].SubItems[REPLCOL].Text = txt;
            cs.Restore();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private int groups()
        {
            int res = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (getValue(i) != 0)
                    res++;
            }
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int gr = groups();
            if (gr == 0) return;
            try
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    int vl = getValue(i);
                    if (vl != 0)
                    {
                        RabNetEngRabbit rr = null;
                        gr--;
                        if (gr == 0 && nudCount.Value == 0)
                        {
                            rr = r;
                        }
                        else
                        {
                            rr = Engine.get().getRabbit(r.RabID);
                            rr = Engine.get().getRabbit(rr.Clone(vl, 0, 0, 0));
                        }
                        rr.PlaceSuckerTo((int)listView1.Items[i].Tag);
                    }
                }
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReplaceYoungersForm_Load(object sender, EventArgs e)
        {
            updateMothers();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                nudRepl.Value = 0;
                nudRepl.Enabled = false;
                return;
            }
            nudRepl.Value = 0;
            nudRepl.Maximum = nudCount.Value+getValue();
            nudRepl.Enabled = nudRepl.Maximum!=0;
            nudRepl.Value = getValue();
        }

        private int getValue(int item)
        {
            String v = listView1.Items[item].SubItems[REPLCOL].Text;
            int cnt = 0;
            if (v != "")
                cnt = int.Parse(v);
            return cnt;
        }

        private int getValue()
        {
            return getValue(listView1.SelectedItems[0].Index);
        }

        private void nudRepl_ValueChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            int was = getValue();
            int def = (int)nudRepl.Value - was;
            nudCount.Value -= def;
            listView1.SelectedItems[0].SubItems[REPLCOL].Text = nudRepl.Value == 0 ? "" : nudRepl.Value.ToString();
            int wcnt = int.Parse(listView1.SelectedItems[0].SubItems[REPLCOL - 1].Text);
            listView1.SelectedItems[0].SubItems[REPLCOL - 1].Text = (wcnt + def).ToString();
        }
    }
}
