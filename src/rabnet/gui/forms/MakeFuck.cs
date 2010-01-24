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
        public MakeFuck()
        {
            InitializeComponent();
            dateDays1.DateValue = DateTime.Now;
            brds = Engine.db().catalogs().getBreeds();
            manual = false;
            cbHeter.Checked=(Engine.opt().getIntOption(Options.OPT_ID.GETEROSIS)==1);
            cbInbreed.Checked = (Engine.opt().getIntOption(Options.OPT_ID.INBREEDING) == 1);
            manual = true;
        }
        public MakeFuck(int r1)
            : this(r1,0)
        {
        }

        public MakeFuck(int r1, int r2):this()
        {
            rab1 = Engine.get().getRabbit(r1);
            label1.Text = rab1.fullName;
            label2.Text = rab1.breedName;
            rtosel = r2;
            fillTable();
        }

        private void fillTable()
        {
            listView1.Items.Clear();
            int malewait = Engine.opt().getIntOption(Options.OPT_ID.MALE_WAIT);
            Fucks fs = Engine.db().allFuckers(rab1.rid,cbHeter.Checked,cbInbreed.Checked,malewait);
            
            foreach (Fucks.Fuck f in fs.fucks)
            {
                bool heter=(f.breed != rab1.breed);
                bool inbr=RabNetEngHelper.inbreeding(rab1.genom,f.rgenom);
                if ((!inbr || cbInbreed.Checked) && (!heter || cbHeter.Checked) &&
                    (f.dead>1 || cbCand.Checked) || f.partnerid==rtosel)
                {
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
                }
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
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
                int r2 = (listView1.SelectedItems[0].Tag as Fucks.Fuck).partnerid;
                rab1.FuckIt(r2, dateDays1.DateValue);
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("Порграмма вызвала исключение " + ex.GetType().ToString() + ": " + ex.Message);
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

    }
}
