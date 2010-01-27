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
        private int gentree=10;
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
            gentree = Engine.opt().getIntOption(Options.OPT_ID.GEN_TREE)-1;
            listView1.Items.Clear();
            listView1.Hide();
            Options op = Engine.opt();
            flt["shr"] = op.getOption(Options.OPT_ID.SHORT_NAMES);
            flt["sht"] = op.getOption(Options.OPT_ID.SHOW_TIER_TYPE);
            flt["sho"] = op.getOption(Options.OPT_ID.SHOW_TIER_SEC);
            flt["dbl"] = op.getOption(Options.OPT_ID.DBL_SURNAME);
            flt["num"] = op.getOption(Options.OPT_ID.SHOW_NUMBERS);
            flt["brd"] = op.getOption(Options.OPT_ID.MAKE_BRIDE);
            listView1.ListViewItemSorter = null;
            IDataGetter dg = DataThread.db().getRabbits(flt);
            rsb.setText(1, dg.getCount().ToString() + " записей");
            rsb.setText(2, dg.getCount2().ToString() + " кроликов");
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
            li.SubItems.Add(rab.average() == -1 ? "" : rab.average().ToString());
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

        private bool isGirl()
        {
            string sd = listView1.SelectedItems[0].SubItems[5].Text;
            return (sd=="Девочка" || sd=="Дев");
        }

        private bool isBride()
        {
            string sd = listView1.SelectedItems[0].SubItems[5].Text;
            return (sd == "Невеста" || sd == "Нвс");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!manual)
                return;
            makeSelectedCount();
            if (listView1.SelectedItems.Count <1)
            {
                setMenu(-1,0,false);
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
            bool kids = false;
            if (listView1.SelectedItems.Count == 1 && listView1.SelectedItems[0].SubItems[7].Text[0] == '+')
                kids = true;
            setMenu(isx, listView1.SelectedItems.Count,kids);
            if (listView1.SelectedItems.Count != 1)
            {
                return;
            }
            if (gentree < 0)
            {
                genTree.Nodes.Clear();
                return;
            }
            for (int ind = 0; ind < genTree.Nodes.Count; ind++)
            {
                int len = genTree.Nodes[ind].Text.IndexOf(",");
                string str = genTree.Nodes[ind].Text.Remove(len);
                if (listView1.SelectedItems[0].SubItems[0].Text.StartsWith(str))
                {
                    if (ind == 0) return;
                    genTree.Nodes.RemoveAt(ind);
                    break;
                }
            }
            if (genTree.Nodes.Count > 0)
                genTree.Nodes[0].ForeColor = Color.Gray;
            while (genTree.Nodes.Count > gentree)
                genTree.Nodes.RemoveAt(gentree);
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

        private void setMenu(int sex,int multi,bool kids)
        {
            makeBon.Visible = passportMenuItem.Visible=proholostMenuItem.Visible=false;
            replaceMenuItem.Visible = placeChMenuItem.Visible= false;
            KillMenuItem.Visible = countKidsMenuItem.Visible=false;
            okrolMenuItem.Visible = fuckMenuItem.Visible= false;
            boysoutMenuItem.Visible = replaceYoungersMenuItem.Visible= false;
            if (sex < 0) return;
            KillMenuItem.Visible = true;
            replaceMenuItem.Visible = true;
            countKidsMenuItem.Visible = replaceYoungersMenuItem.Visible = kids;
                // boysoutMenuItem.Visible=
            if (multi==1)
                makeBon.Visible = true;
            if (multi == 2)
                placeChMenuItem.Visible = true;
            if (sex != 3 && multi == 1)
            {
                passportMenuItem.Visible = true;
            }
            if (sex == 2 && multi == 1)
            {
                fuckMenuItem.Visible = !isGirl();
                fuckMenuItem.Text = isBride() ? "Случка" : "Вязка";
            }
            if (sex == 4)
            {
                proholostMenuItem.Visible = true;
                okrolMenuItem.Visible = true;
            }
        }

        public override ContextMenuStrip getMenu()
        {
            setMenu(-1,0,false);
            return actMenu;
        }

        private void SelectAllMenuItem_Click(object sender, EventArgs e)
        {
            manual = false;
            foreach (ListViewItem li in listView1.Items)
                li.Selected=true;
            listView1.Show();
            manual = true;
            listView1_SelectedIndexChanged(null, null);
        }

        private void passportMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            RabbitInfo ri = new RabbitInfo((int)listView1.SelectedItems[0].Tag);
            if (ri.ShowDialog() == DialogResult.OK)
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
            if( (new BonForm(rid)).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void proholostMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            int rid = (int)listView1.SelectedItems[0].Tag;
            if((new Proholost(rid)).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void newRab_Click(object sender, EventArgs e)
        {
            if((new IncomeForm()).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void replaceMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            ReplaceForm rpf = new ReplaceForm();
            foreach (ListViewItem li in listView1.SelectedItems)
                rpf.addRabbit((int)li.Tag);
            if(rpf.ShowDialog() == DialogResult.OK)
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
            if (rpf.ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void KillMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            KillForm f = new KillForm();
            foreach (ListViewItem li in listView1.SelectedItems)
                f.addRabbit((int)li.Tag);
            if(f.ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void countKidsMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            CountKids f = new CountKids((int)listView1.SelectedItems[0].Tag);
            if (f.ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void okrolMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            if ((new OkrolForm((int)listView1.SelectedItems[0].Tag)).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void fuckMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            if((new MakeFuck((int)listView1.SelectedItems[0].Tag)).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void boysoutMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            ReplaceForm rpf = new ReplaceForm();
            rpf.addRabbit((int)listView1.SelectedItems[0].Tag);
            rpf.setAction(ReplaceForm.Action.BOYSOUT);
            if (rpf.ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void replaceYoungersMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            RabNetEngRabbit r = Engine.get().getRabbit((int)listView1.SelectedItems[0].Tag);
            if (r.youngcount<1) return;
            if((new ReplaceYoungersForm(r.youngers[0].id)).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void makeSelectedCount()
        {
            int rows = listView1.SelectedItems.Count;
            int cnt = 0;
            foreach (ListViewItem li in listView1.SelectedItems)
            {
                String s=li.SubItems[7].Text;
                int c = 1;
                if (s[0] == '+') c += int.Parse(s.Substring(1));
                if (s[0] == '[') c = int.Parse(s.Substring(1, s.Length - 2));
                cnt += c;
            }
            rsb.setText(3, String.Format("Выбрано {0:d} строк - {1:d} кроликов",rows,cnt));
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            manual = false;
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            manual = true;
            listView1_SelectedIndexChanged(null, null);
        }

    }
}
