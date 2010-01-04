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
        public YoungsPanel():base()
        {
        }
        public YoungsPanel(RabStatusBar sb)
            : base(sb, null)
        {
            cs = new ListViewColumnSorter(listView1, new int[] { 2, 9 });
            listView1.ListViewItemSorter = null;
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            f = new Filters();
            listView1.Items.Clear();
            listView1.Hide();
            Options op = Engine.opt();
            f["shr"] = op.getOption(Options.OPT_ID.SHORT_NAMES);
            f["sht"] = op.getOption(Options.OPT_ID.SHOW_TIER_TYPE);
            f["sho"] = op.getOption(Options.OPT_ID.SHOW_TIER_SEC);
            f["dbl"] = op.getOption(Options.OPT_ID.DBL_SURNAME);
            f["num"] = op.getOption(Options.OPT_ID.SHOW_NUMBERS);
            listView1.ListViewItemSorter = null;
            IDataGetter dg = DataThread.db().getYoungers(f);
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
            li.SubItems.Add(rab.N());
            li.SubItems.Add(rab.age().ToString());
            li.SubItems.Add("");
            li.SubItems.Add(rab.sex());
            li.SubItems.Add(rab.breed());
            li.SubItems.Add(rab.address());
            li.SubItems.Add(rab.cls());
            li.SubItems.Add(rab.status());
            li.SubItems.Add(rab.average()==0?"-":rab.average().ToString());
            li.SubItems.Add("");
            li.SubItems.Add(rab.notes());
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
            
            if (listView1.SelectedItems.Count != 1)
                return;

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
            if (genTree.Nodes.Count > 10)
                genTree.Nodes.RemoveAt(10);
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


    }
}
