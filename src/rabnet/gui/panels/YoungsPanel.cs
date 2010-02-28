using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class YoungsPanel : RabNetPanel
    {
        private int gentree = 10;
        private bool manual = true;
        public YoungsPanel():base()
        {
        }
        public YoungsPanel(RabStatusBar sb)
            : base(sb, null)
        {
            cs = new ListViewColumnSorter(listView1, new int[] { 1,2, 8 },Options.OPT_ID.YOUNG_LIST);
            listView1.ListViewItemSorter = null;
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            gentree = Engine.opt().getIntOption(Options.OPT_ID.GEN_TREE);
            f = new Filters();
            Options op = Engine.opt();
            f["shr"] = op.getOption(Options.OPT_ID.SHORT_NAMES);
            f["sht"] = op.getOption(Options.OPT_ID.SHOW_TIER_TYPE);
            f["sho"] = op.getOption(Options.OPT_ID.SHOW_TIER_SEC);
            f["dbl"] = op.getOption(Options.OPT_ID.DBL_SURNAME);
            f["num"] = op.getOption(Options.OPT_ID.SHOW_NUMBERS);
            cs.Prepare();
            IDataGetter dg = DataThread.db().getYoungers(f);
            rsb.setText(1, dg.getCount().ToString() + " строк");
            rsb.setText(2, dg.getCount2().ToString() + " кроликов");
            return dg;
        }

        protected override void onItem(IData data)
        {
            if (data == null)
            {
                cs.Restore();
                return;
            }
            Younger rab = (data as Younger);
            ListViewItem li = listView1.Items.Add(rab.fname);
            li.Tag = rab.fid;
            li.SubItems.Add(rab.fcount.ToString());
            li.SubItems.Add(rab.fage.ToString());
            li.SubItems.Add(rab.fsex);
            li.SubItems.Add(rab.fbreed);
            li.SubItems.Add(rab.faddress);
            li.SubItems.Add(rab.fcls);
            li.SubItems.Add(rab.mom);
            li.SubItems.Add(rab.fneighbours==0?"-":rab.fneighbours.ToString());
        }

        private void insertNode(TreeNode nd, TreeData data)
        {
            if (data.items != null)
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
            setMenu();
            makeSelectedCount();
            if (listView1.SelectedItems.Count != 1)
                return;

            for (int ind = 0; ind < genTree.Nodes.Count; ind++)
            {
                int len = genTree.Nodes[ind].Text.IndexOf("-");
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
            if (genTree.Nodes.Count > gentree)
                genTree.Nodes.RemoveAt(gentree);
            TreeData dt = Engine.db().rabbitGenTree((int)listView1.SelectedItems[0].Tag);
            if (dt != null)
            {
                TreeNode tn = genTree.Nodes.Insert(0, dt.caption);
                tn.ForeColor = Color.Blue;
                insertNode(tn, dt);
                tn.ExpandAll();
                tn.EnsureVisible();
            }
        }

        public override ContextMenuStrip getMenu()
        {
            setMenu();
            return actMenu;
        }

        public void setMenu()
        {
            replaceYoungersMenuItem.Visible = listView1.SelectedItems.Count == 1;
        }

        private void replaceYoungersMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            new ReplaceYoungersForm((int)listView1.SelectedItems[0].Tag).ShowDialog();
            rsb.run();
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
                listView1.DoDragDrop(e.Item, DragDropEffects.Link);
        }

        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                e.Effect = DragDropEffects.Link;
            }
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(ListViewItem))) return;
            Point p=listView1.PointToClient(new Point(e.X,e.Y));
            ListViewItem to = listView1.GetItemAt(p.X, p.Y);
            if (to==null)
                return;
            ListViewItem from=e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
            if (from == null) return;
            if (Math.Abs(int.Parse(to.SubItems[2].Text) - int.Parse(from.SubItems[2].Text)) >
                Engine.opt().getIntOption(Options.OPT_ID.COMBINE_AGE))
            {
                if (MessageBox.Show(this, "Крольчата не подходят по возрасту.\nПоказать возможных кормилиц?",
                    "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (new ReplaceYoungersForm((int)from.Tag).ShowDialog() == DialogResult.OK)
                        rsb.run();
                }
                return;
            }
            RabNetEngRabbit r = Engine.get().getRabbit((int)to.Tag);
            if (new ReplaceYoungersForm((int)from.Tag, r.parent).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void makeSelectedCount()
        {
            int rows = listView1.SelectedItems.Count;
            int cnt = 0;
            foreach (ListViewItem li in listView1.SelectedItems)
            {
                int c = int.Parse(li.SubItems[1].Text);
                cnt += c;
            }
            rsb.setText(3, String.Format("Выбрано {0:d} строк - {1:d} кроликов", rows, cnt));
        }

        private void selectAllMenuItem_Click(object sender, EventArgs e)
        {
            manual = false;
            for (int i = 0; i < listView1.Items.Count; i++)
                listView1.Items[i].Selected = true;
            manual = true;
            listView1_SelectedIndexChanged(null, null);
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
