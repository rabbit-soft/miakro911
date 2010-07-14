using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class MakeFuck : Form
    {
        private RabNetEngRabbit rab1 = null;
        private Catalog brds;
        private int rtosel=0;
        bool manual = true;
        int malewait = 0;
        ListViewColumnSorter cs;
        Catalog names = null;
        int selected = 0;
        int action = 0;
        public MakeFuck()
        {
            InitializeComponent();
            initialHints();
            dateDays1.DateValue = DateTime.Now;
            brds = Engine.db().catalogs().getBreeds();
            manual = false;
            cbHeter.Checked=(Engine.opt().getIntOption(Options.OPT_ID.GETEROSIS)==1);
            cbInbreed.Checked = (Engine.opt().getIntOption(Options.OPT_ID.INBREEDING) == 1);
            malewait = Engine.opt().getIntOption(Options.OPT_ID.MALE_WAIT);
            manual = true;
            cs = new ListViewColumnSorter(listView1, new int[] { 3,4 },Options.OPT_ID.MAKE_FUCK_LIST);
            listView1.ListViewItemSorter = cs;
        }
        public MakeFuck(int r1)
            : this(r1,0)
        {
        }

        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(button3, "Показать гены выбранного самца");
        }

        private void fillNames()
        {
            comboBox1.Items.Clear();
            names = Engine.db().catalogs().getFreeNames(2, rab1.name);
            comboBox1.Items.Add("");
            comboBox1.SelectedIndex = 0;
            foreach (int key in names.Keys)
            {
                comboBox1.Items.Add(names[key]);
                if (key == rab1.name)
                    comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            }
            comboBox1.Enabled = button4.Enabled = rab1.name == 0;
        }

        public MakeFuck(int r1, int r2):this()
        {
            rab1 = Engine.get().getRabbit(r1);
            label1.Text = rab1.fullName;
            label2.Text = rab1.breedName;
            rtosel = r2;
            fillTable();
            if (rab1.status > 0)
                Text = button1.Text = "Вязать";
            fillNames();
        }
        public MakeFuck(int r1, int r2, int action):this(r1,r2)
        {
            this.action = action;
        }

        private void fillTable()
        {
            cs.Prepare();
            Fucks fs = Engine.db().allFuckers(rab1.rid,cbHeter.Checked,cbInbreed.Checked,malewait);
            foreach (Fucks.Fuck f in fs.fucks)
            {
                bool heter=(f.breed != rab1.breed);
                bool inbr=RabNetEngHelper.inbreeding(rab1.genom,f.rgenom);
                /*
                if ((!inbr || cbInbreed.Checked) && (!heter || cbHeter.Checked) &&
                    (f.dead>1 || cbCand.Checked) || f.partnerid==rtosel)
                {
                 */
                ListViewItem li = listView1.Items.Add(f.partner);
                li.Tag = f;
                String stat="Мальчик";
                if (f.dead==1)
                    stat="Кандидат";
                if (f.dead==2)
                    stat="Производитель";
                li.SubItems.Add(stat);
                li.SubItems.Add(brds[f.breed]);
                li.SubItems.Add(f.times.ToString());
                li.SubItems.Add(f.children.ToString());
                li.SubItems.Add(heter? "ДА" : "-");
                li.SubItems.Add(inbr ? "ДА" : "-");
                if (rtosel == f.partnerid)
                 {
                    li.Selected = true;
                    li.EnsureVisible();
                 }
                //}
            }
            cs.Restore();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled=button3.Enabled=(listView1.SelectedItems.Count==1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int r2 = (listView1.SelectedItems[0].Tag as Fucks.Fuck).partnerid;
            (new GenomView(rab1.rid, r2)).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (rab1.name == 0 && comboBox1.SelectedIndex != 0)
                {
                    foreach (int k in names.Keys)
                        if (comboBox1.Text == names[k])
                            rab1.name = k;
                    rab1.commit();
                }
                if (listView1.SelectedItems.Count!=1)
                    throw new ApplicationException("Выберите самца");
                int r2 = (listView1.SelectedItems[0].Tag as Fucks.Fuck).partnerid;
                selected = r2;
                if (action==0)
                    rab1.FuckIt(r2, dateDays1.DateValue);
                Close();
            }
            catch (ApplicationException ex)
            {
                DialogResult = DialogResult.None;
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void cbCand_CheckedChanged(object sender, EventArgs e)
        {
            if (!manual)
                return;
            fillTable();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count==1)
                button3.PerformClick();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new NamesForm(1).ShowDialog();
            fillNames();
        }

        public int SelectedFucker { get { return selected; } }

    }
}
