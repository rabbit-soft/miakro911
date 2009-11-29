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
        public MakeFuck()
        {
            InitializeComponent();
            dateDays1.DateValue = DateTime.Now;
            brds = Engine.db().catalogs().getBreeds();
        }
        public MakeFuck(int r1)
            : this(r1,0)
        {
        }

        public MakeFuck(int r1, int r2):this()
        {
            rab1 = Engine.get().getRabbit(r1);
            label1.Text = rab1.fullName;
            label2.Text = rab1.breeName;
            rtosel = r2;
            fillTable();
        }

        private void fillTable()
        {
            listView1.Items.Clear();
            Fucks fs = Engine.db().allFuckers(rab1.rid);
            foreach (Fucks.Fuck f in fs.fucks)
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
                li.SubItems.Add(f.breed == rab1.breed ? "-" : "ДА");
                li.SubItems.Add(RabNetEngHelper.geterosis(rab1.genom,f.rgenom)? "ДА" : "-");
                if (rtosel == f.partnerid)
                    li.Selected = true;
            }
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

    }
}
