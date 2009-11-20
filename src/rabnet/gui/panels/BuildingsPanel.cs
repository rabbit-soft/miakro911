using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class BuildingsPanel : RabNetPanel
    {
        public BuildingsPanel(): base(){}
        public BuildingsPanel(RabStatusBar sb):base(sb,null)
        {
            cs = new ListViewColumnSorter(listView1, new int[] { });
            listView1.ListViewItemSorter = null;
        }

        private TreeNode makenode(TreeNode parent,String name,TreeData td)
        {
            TreeNode n=null;
            if (parent == null)
                n=treeView1.Nodes.Add(name);
            else
                n = parent.Nodes.Add(name);
            if (td.items!=null)
            for (int i = 0; i < td.items.Length; i++)
            {
                String[] st = td.items[i].caption.Split(':');
                (makenode(n, st[2], td.items[i])).Tag=st[0]+":"+st[1];
            }
            return n;
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            treeView1.Nodes.Clear();
            TreeNode n=makenode(null,"Ферма",Engine.db().buildingsTree());
            n.Tag="0:0";
            n.Expand();
            f = new Filters();
            f["shr"] = Engine.opt().getOption(Options.OPT_ID.SHORT_NAMES);
            listView1.Hide();
            listView1.Items.Clear();
            listView1.ListViewItemSorter = null;
            IDataGetter dg = DataThread.db().getBuildings(f);
            rsb.setText(1, dg.getCount().ToString() + " items");
            return dg;
        }

        protected override void onItem(IData data)
        {
            if (data==null)
            {
                listView1.ListViewItemSorter=cs;
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Show();
            }
            IBuilding b = data as IBuilding;
            ListViewItem it = listView1.Items.Add(b.id().ToString());

        }

    }
}
