using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class IncomeForm : Form
    {
        private Catalog zns = null;
        private Catalog brd = null;
        private List<RabNetEngRabbit> rbs = new List<RabNetEngRabbit>();
        public IncomeForm()
        {
            InitializeComponent();
            zns = Engine.db().catalogs().getZones();
            brd = Engine.db().catalogs().getBreeds();
            fillZones();
        }

        private void fillZones()
        {
            String tx = zones.Text;
            zones.Items.Clear();
            zones.Items.Add("");
            foreach (int k in zns.Keys)
                zones.Items.Add(zns[k]);
            zones.Text = tx;
        }

        private int getZone()
        {
            if (zones.Text=="") return 0;
            foreach (int k in zns.Keys)
                if (zones.Text == zns[k])
                    return k;
            return 0;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (new CatalogForm(CatalogForm.CatalogType.ZONES)).ShowDialog();
            fillZones();
        }

        private void Update(bool plast)
        {
            listView1.Items.Clear();
            for (int i = 0; i < rbs.Count; i++)
            {
                String name = "Бесполые";
                if (rbs[i].sex == OneRabbit.RabbitSex.MALE) name = rbs[i].group==1?"Самец":"Самцы";
                if (rbs[i].sex == OneRabbit.RabbitSex.FEMALE) name = rbs[i].group == 1 ? "Самка" : "Самки";
                ListViewItem li = listView1.Items.Add(name);
                li.SubItems.Add(rbs[i].group.ToString());
                li.Tag=rbs[i];
                li.SubItems.Add(rbs[i].fullName);
                li.SubItems.Add(brd[rbs[i].breed]);
                li.SubItems.Add(rbs[i].address);
            }
            if (plast)
            {
                listView1.SelectedItems.Clear();
                ListViewItem it=listView1.Items[listView1.Items.Count - 1];
                it.Selected = true;
                (it.Tag as RabNetEngRabbit).zone = getZone();
                button8.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rbs.Add(new RabNetEngRabbit(Engine.get(),OneRabbit.RabbitSex.MALE));
            Update(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rbs.Add(new RabNetEngRabbit(Engine.get(), OneRabbit.RabbitSex.FEMALE));
            Update(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rbs.Add(new RabNetEngRabbit(Engine.get(), OneRabbit.RabbitSex.VOID));
            Update(true);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            (new RabbitInfo(listView1.SelectedItems[0].Tag as RabNetEngRabbit)).ShowDialog();
            Update(false);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button4.Enabled=button8.Enabled=button9.Enabled=(listView1.SelectedItems.Count==1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            rbs.Remove(listView1.SelectedItems[0].Tag as RabNetEngRabbit);
            Update();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ReplaceForm rpf = new ReplaceForm();
            foreach (RabNetEngRabbit r in rbs)
                rpf.addRabbit(r);
            rpf.ShowDialog();
            Update();
        }

        private bool isMom(RabNetEngRabbit r1)
        {
            return (r1.sex == OneRabbit.RabbitSex.FEMALE && r1.status > 0);
        }

        private void commit(RabNetEngRabbit r1)
        {
            if (r1.tag != "done")
                r1.newCommit();
            r1.tag = "done";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RabNetEngRabbit r in rbs)
                {
                    foreach (RabNetEngRabbit r2 in rbs)
                    {
                        if (r2 != r && r2.newAddress == r.newAddress)
                        {
                            RabNetEngRabbit mom=r;
                            RabNetEngRabbit chl = r2;
                            if (isMom(r2)) { chl = r; mom = r2; }
                            commit(mom);
                            chl.mom=mom.rid;
                            commit(chl);
                        }
                    }
                    commit(r);
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка " + ex.ToString());
            }
        }


    }

}
