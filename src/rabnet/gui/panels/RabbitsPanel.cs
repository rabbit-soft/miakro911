using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class RabbitsPanel : RabNetPanel
    {
        private bool manual = true;
        public RabbitsPanel():base()
        {
        }
        public RabbitsPanel(RabStatusBar rsb):base(rsb,new RabbitsFilter(rsb))
        {
            cs = new ListViewColumnSorter(listView1, new int[] { 2, 9 });
            listView1.ListViewItemSorter = null;
        }

        protected override IDataGetter onPrepare(Filters flt)
        {
            listView1.Items.Clear();
            listView1.Hide();
            Options op = Engine.opt();
            flt["shr"] = op.getOption(Options.OPT_ID.SHORT_NAMES);
            flt["sht"] = op.getOption(Options.OPT_ID.SHOW_TIER_TYPE);
            flt["sho"] = op.getOption(Options.OPT_ID.SHOW_TIER_SEC);
            flt["dbl"] = op.getOption(Options.OPT_ID.DBL_SURNAME);
            flt["num"] = op.getOption(Options.OPT_ID.SHOW_NUMBERS);
            listView1.ListViewItemSorter = null;
            IDataGetter dg = DataThread.db().getRabbits(flt);
            rsb.setText(1, dg.getCount().ToString() + " items");
            return dg;
        }

        protected override void onItem(IData data)
        {
            if (data == null)
            {
                listView1.ListViewItemSorter = cs.Clear();
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Show();
                return;
            }
            IRabbit rab = (data as IRabbit);
            ListViewItem li = listView1.Items.Add(rab.name());
            li.Tag = rab.id();
            li.SubItems.Add(rab.sex());
            li.SubItems.Add(rab.age().ToString());
            li.SubItems.Add(rab.breed());
            li.SubItems.Add(rab.weight());
            li.SubItems.Add(rab.status());
            li.SubItems.Add(rab.bgp());
            li.SubItems.Add(rab.N());
            li.SubItems.Add(rab.average() == 0 ? "" : rab.average().ToString());
            li.SubItems.Add(rab.rate().ToString());
            li.SubItems.Add(rab.cls());
            li.SubItems.Add(rab.address());
            li.SubItems.Add(rab.notes());
        }

        private void insertNode(TreeNode nd,TreeData data)
        {
            if (data.items!=null)
            for (int i = 0; i < data.items.Length; i++)
                if (data.items[i] != null)
                {
                    TreeNode n = nd.Nodes.Add(data.items[i].caption);
                    insertNode(n, data.items[i]);
                }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!manual)
                return;
            if (listView1.SelectedItems.Count <1)
            {
                setMenu(-1,0);
                return;
            }
            string sx = "";
            for (int i = 0; i < listView1.SelectedItems.Count && sx.Length<2;i++ )
            {
                String s = listView1.SelectedItems[i].SubItems[1].Text;
                if (s[0] == 'С' || s[0] == 'C')
                    s = "S";
                if (!sx.Contains(s))
                    sx += s;
            }
            int isx=3;
            if (sx=="?") isx=0;
            if (sx=="м") isx=1;
            if (sx=="ж") isx=2;
            if (sx == "S") isx = 4;
            setMenu(isx, listView1.SelectedItems.Count);
            if (listView1.SelectedItems.Count != 1)
            {
                return;
            }
            
            for (int ind = 0; ind < genTree.Nodes.Count; ind++)
            {
                int len = genTree.Nodes[ind].Text.IndexOf(",");
                string str = genTree.Nodes[ind].Text.Remove(len);
                if (listView1.SelectedItems[0].SubItems[0].Text.StartsWith(str))
                {
                    genTree.Nodes.RemoveAt(ind);
                    break;
                }
            }
            if (genTree.Nodes.Count > 0)
                genTree.Nodes[0].ForeColor = Color.Gray;
            if (genTree.Nodes.Count > 10)
                genTree.Nodes.RemoveAt(10);
            TreeData dt = Engine.db().rabbitGenTree((int)listView1.SelectedItems[0].Tag);
            if (dt!=null)
            {
                TreeNode tn = genTree.Nodes.Insert(0, dt.caption);
                tn.ForeColor = Color.Blue;
                insertNode(tn, dt);
                tn.ExpandAll();
                tn.EnsureVisible();
            }
        }

        private void setMenu(int sex,int multi)
        {
            makeBon.Visible = passportMenuItem.Visible=proholostMenuItem.Visible=false;
            replaceMenuItem.Visible = placeChMenuItem.Visible= false;
            if (sex < 0) return;
            replaceMenuItem.Visible = true;
            if (multi==1)
                makeBon.Visible = true;
            if (multi == 2)
                placeChMenuItem.Visible = true;
            if (sex != 3 && multi == 1)
            {
                passportMenuItem.Visible = true;
            }
            if (sex == 4)
                proholostMenuItem.Visible = true;
        }

        public override ContextMenuStrip getMenu()
        {
            setMenu(-1,0);
            return actMenu;
        }

        private void SelectAllMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView1.Items)
                li.Selected=true;
            listView1.Show();
        }

        private void passportMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            RabbitInfo ri = new RabbitInfo((int)listView1.SelectedItems[0].Tag);
            ri.ShowDialog();
            rsb.run();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            passportMenuItem.PerformClick();
        }

        private void makeBon_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count!=1)
                return;
            int rid=(int)listView1.SelectedItems[0].Tag;
            (new BonForm(rid)).ShowDialog();
            rsb.run();
        }

        private void proholostMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            int rid = (int)listView1.SelectedItems[0].Tag;
            (new Proholost(rid)).ShowDialog();
            rsb.run();
        }

        private void newRab_Click(object sender, EventArgs e)
        {
            (new IncomeForm()).ShowDialog();
            rsb.run();
        }

        private void replaceMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            ReplaceForm rpf = new ReplaceForm();
            foreach (ListViewItem li in listView1.SelectedItems)
                rpf.addRabbit((int)li.Tag);
            rpf.ShowDialog();
            rsb.run();
        }

        private void placeChMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 2)
                return;
            ReplaceForm rpf = new ReplaceForm();
            rpf.addRabbit((int)listView1.SelectedItems[0].Tag);
            rpf.addRabbit((int)listView1.SelectedItems[1].Tag);
            rpf.setAction(ReplaceForm.Action.CHANGE);
            rpf.ShowDialog();
            rsb.run();
        }



    }
}
