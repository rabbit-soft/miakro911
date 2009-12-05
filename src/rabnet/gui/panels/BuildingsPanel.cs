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
        private bool manual = true;
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
            f["dbl"] = Engine.opt().getOption(Options.OPT_ID.DBL_SURNAME);
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
                listView1.ListViewItemSorter=cs.Clear();
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Show();
                return;
            }
            Building b = data as Building;
            string prevnm="";
            for (int i = 0; i < b.secs(); i++)
            {
                if (b.id() == 92)
                {
                    String.Format("");
                }
                if (b.area(i) != prevnm)
                {
                    ListViewItem it = listView1.Items.Add(b.farm().ToString() + b.area(i));
                    prevnm = b.area(i);
                    it.Tag = b.id().ToString();
                    it.SubItems.Add(b.type());
                    it.SubItems.Add(b.dep(i));
                    String stat = "unk";
                    if (b.repair()) stat = "ремонт";
                    else
                    {
                        if (b.busy(i) == 0) stat = "-";
                        else
                            stat = b.use(i);
                    }
                    it.SubItems.Add(stat);
                    String nst = "";
                    String htr = "";
                    if (b.nest_heater_count() > 0)
                    {
                        int nid = 0;
                        if (b.nest_heater_count() > 1)
                            nid = i;
                        nst = (b.nest()[nid] == '1') ? "да" : "нет";
                        htr = (b.heater()[nid] == '0' ? "нет" : (b.heater()[nid] == '1' ? "выкл" : "вкл"));
                        if (b.itype() == "jurta")
                            if ((b.delims()[0] == '1' && i == 0) || (b.delims()[0] == '0' && i == 1))
                            {
                                nst = "";
                                htr = "";
                            }
                        if (b.itype()=="complex")
                            if (i != 0)
                            {
                                nst = "";
                                htr = "";
                            }
                    }
                    it.SubItems.Add(nst);
                    it.SubItems.Add(htr);
                    it.SubItems.Add(getAddress(b.farm()));
                    it.SubItems.Add(b.notes());
                    it.Tag = b;
                }
            }
        }

        private String getAddress(int ifid)
        {
            String res = "";
            TreeNode nd = searchFarm(ifid, treeView1.Nodes[0]);
            if (nd!=null)
            {
                res = nd.Text;
                while (nd != treeView1.Nodes[0])
                {
                    nd = nd.Parent;
                    if (nd != treeView1.Nodes[0])
                        res = nd.Text + "/" + res;
                }
            }
            return res;
        }
        
        private TreeNode searchFarm(int ifid,TreeNode nd)
        {
            String[] s = (nd.Tag as string).Split(':');
            if (int.Parse(s[1]) == ifid)
            {
                return nd;
            }
            foreach (TreeNode n in nd.Nodes)
            {
                TreeNode res = searchFarm(ifid, n);
                if (res != null)
                    return res;
            }
            return null;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count<1)
                return;
            if (listView1.SelectedItems[0] == null)
                return;
            if (!manual)
                return;
            ListViewItem li = listView1.SelectedItems[0];
            Building b = li.Tag as Building;
            TreeNode tr = treeView1.Nodes[0];
            tr.Collapse(false);
            tr.Expand();
            manual = false;
            treeView1.SelectedNode = searchFarm(b.farm(), tr);
            if (treeView1.SelectedNode != null) treeView1.SelectedNode.Expand();
            manual = true;
        }

        private FarmDrawer.DrawTier tierFromBuilding(Building b)
        {
            List<string> rabs=new List<string>();
            for (int i=0;i<b.secs();i++)
                    rabs.Add(b.use(i));
            return new FarmDrawer.DrawTier(b.itype(),b.delims(),b.nest(),b.heater(),rabs.ToArray(),b.repair());
        }

        private void DrawFarm(int farm)
        {
            int[] tiers=Engine.db().getTiers(farm);
            FarmDrawer.DrawTier t1 = tierFromBuilding(Engine.db().getBuilding(tiers[0]));
            FarmDrawer.DrawTier t2=null;
            if (tiers[1]!=0)
                t2=tierFromBuilding(Engine.db().getBuilding(tiers[1]));
            farmDrawer1.setFarm(farm,t1,t2);
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                farmDrawer1.setFarm(0, null, null);
                return;
            }
            int farm = int.Parse((treeView1.SelectedNode.Tag as String).Split(':')[1]);
            if (farm == 0)
            {
                farmDrawer1.setFarm(0, null, null);
                return;
            }
            if (manual)
            {
                manual = false;
                listView1.SelectedItems.Clear();
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    Building b = listView1.Items[i].Tag as Building;
                    if (b.farm() == farm)
                    {
                        listView1.Items[i].Selected = true;
                        listView1.Items[i].EnsureVisible();
                    }
                }
                manual = true;
            }
            DrawFarm(farm);
        }

    }
}
