using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
#if PROTECTED
using RabGRD;
#endif

namespace rabnet
{
    public partial class BuildingsPanel : RabNetPanel
    {
        /// <summary>
        /// Нужна потому что под номер клетки отводится 4 знака
        /// Иначе придется изменять форматирование номеров клеток во всей программе
        /// </summary>
        private int MAX_FARMS_COUNT = 999999;
        /// <summary>
        /// Сколько максимально может предлагаться новых ферм
        /// Защита от расхода памяти
        /// </summary>
        private const int NEW_FARMS_LIMIT = 0xff;

        private bool manual = true;
        const String nuBuild = "Новое строение";
        List<int> nofarms = new List<int>();
        int nofarm = 1;
        TreeNode nodeToAdd = null;
        int action = 0;
        int preBuilding = 0;
        public BuildingsPanel(): base(){}
        int maxfarm = 0;

        /// <summary>
        /// Считает количество ферм в Дереве строений
        /// </summary>
        /// <param name="td"></param>
        /// <returns>Количество ферм</returns>
        public static int getFarmsCount(TreeData td)
        {
            int res = 0;
            String[] st = td.caption.Split(':');
            if (st.Length == 3)
                if (st[1] != "0")
                {
                    res++;
                    return res;
                }
            if (td.items != null)
            {
                for (int i = 0; i < td.items.Length; i++)
                {
                    res += getFarmsCount(td.items[i]);
                }
            }
            return res;

        }

        public BuildingsPanel(RabStatusBar bsb):base(bsb, new BuildingsFilter(bsb))
        {
            colSort = new ListViewColumnSorter(listView1, new int[] { },Options.OPT_ID.BUILD_LIST);
            listView1.ListViewItemSorter = null;
            treeView1.TreeViewNodeSorter = new TVNodeSorter();
            MakeExcel = new RabStatusBar.ExcelButtonClickDelegate(this.makeExcel);
        }

        private void addNoFarm(int farm)
        {
            if (farm == nofarm)
            {
                nofarm++;
                return;
            }
            if (farm < nofarm)
            {
                nofarms.Remove(farm);
                return;
            }
            for (int i = nofarm; i < farm; i++)
            {
#if PROTECTED
                //if (i <= PClient.get().farms())
                if (i <= GRD.Instance.GetFarmsCntCache())
#else
    #if DEMO
                if (i <= 100)
    #endif
#endif              
                if (nofarms.Count<NEW_FARMS_LIMIT)
                    nofarms.Add(i);
                else nofarms[NEW_FARMS_LIMIT-1]=i;
            }
            nofarm = farm + 1;
        }

        private TreeNode makenode(TreeNode parent, String name, TreeData td)
        {
            TreeNode n = null;
            if (parent == null)
                n = treeView1.Nodes.Add(name);
            else
                n = parent.Nodes.Add(name);
            if (td.items!=null)
                for (int i = 0; i < td.items.Length; i++)
                {
                    String[] st = td.items[i].caption.Split(':');
                    TreeNode child = makenode(n, st[2], td.items[i]);
                    child.Tag = st[0] + ":" + st[1];
                    int fid = int.Parse(st[1]);
                    addNoFarm(fid);
                    if (maxfarm < fid)
                        maxfarm = fid;
                    if (int.Parse(st[0]) == preBuilding)
                    {
                        treeView1.SelectedNode = child;
                        treeView1.SelectedNode.Expand();
                        //child.Expand();
                    }
                }
            return n;
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            manual = false;
            treeView1.Nodes.Clear();
            nofarms.Clear();
            nofarm = 1;
            maxfarm = 0;
            TreeData buildTree = Engine.db().buildingsTree();
            TreeNode n = makenode(null, "Ферма", buildTree);
#if PROTECTED
//            if (nofarm<=PClient.get().farms())
            if (nofarm <= GRD.Instance.GetFarmsCnt())
#else
    #if DEMO
                if (Engine.db().getMFCount() <= 100)
    #endif
#endif
            if (nofarm <= MAX_FARMS_COUNT)
                nofarms.Add(nofarm);
            MainForm.protectTest(BuildingsPanel.getFarmsCount(buildTree));
            treeView1.Sort();
            manual = true;
            n.Tag="0:0";
            n.Expand();
            f["shr"] = Engine.opt().getOption(Options.OPT_ID.SHORT_NAMES);
            f["dbl"] = Engine.opt().getOption(Options.OPT_ID.DBL_SURNAME);
            colSort.Prepare();
            IDataGetter dg = DataThread.db().getBuildings(f);
            _rsb.setText(1, dg.getCount().ToString() + " МИНИфермы");
            return dg;
        }

        /// <summary>
        /// Добавление новой строчки в ListView
        /// </summary>
        /// <param name="data">Одна запись</param>
        protected override void onItem(IData data)
        {
            if (data==null)
            {
                colSort.Restore();
                return;
            }
            Building b = data as Building;
            string prevnm="";
            int prevfarm = 0;

            for (int i = 0; i < b.secs(); i++)
            {
                /*
                if (b.id() == 92)
                {
                    String.Format("");
                }
                 * */
                if (b.area(i) != prevnm || b.farm()!=prevfarm)
                {
                    manual = false;
                    ListViewItem it = listView1.Items.Add(Building.Format(b.farm()) + b.area(i));//№
                    prevfarm = b.farm();
                    prevnm = b.area(i);
                    it.Tag = b.id().ToString();
                    it.SubItems.Add(b.type());//Ярус
                    it.SubItems.Add(b.dep(i));//Отделение
                    String stat = "unk";
                    if (b.repair()) 
                        stat = "ремонт";
                    else
                    {
                        if (b.busy(i) == 0) stat = "-";
                        else stat = b.use(i);
                    }
                    it.SubItems.Add(stat);//Статус
                    String nst = "";
                    String htr = "";
                    if (b.nest_heater_count() > 0)
                    {
                        int nid = 0;
                        if (b.nest_heater_count() > 1) nid = i;
                        nst = (b.nest()[nid] == '1') ? "да" : "нет";
                        htr = (b.heater()[nid] == '0' ? "нет" : (b.heater()[nid] == '1' ? "выкл" : "вкл"));
                        if (b.itype() == myBuildingType.Jurta)
                            if ((b.delims()[0] == '1' && i == 0) || (b.delims()[0] == '0' && i == 1))
                            {
                                nst = "";
                                htr = "";
                            }
                        if (b.itype()==myBuildingType.Complex)
                            if (i != 0)
                            {
                                nst = "";
                                htr = "";
                            }
                    }
                    it.SubItems.Add(nst);//Гнездовье
                    it.SubItems.Add(htr);//Грелка
                    it.SubItems.Add(getAddress(b.farm()));//Адрес
                    it.SubItems.Add(b.notes());//Заметки
                    it.Tag = b;
                    manual = true;
                }
            }

			colSort.SemiReady();
        }

        private String getAddress(int ifid){return getAddress(ifid, -1);}
        private String getAddress(int ifid,int bid)
        {
            String res = "";
            TreeNode nd = searchFarm(ifid,bid, treeView1.Nodes[0]);
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

        private TreeNode searchFarm(int ifid, TreeNode nd)
        {
            return searchFarm(ifid, -1, nd);
        }
        private TreeNode searchFarm(int ifid,int bid,TreeNode nd)
        {
            String[] s = (nd.Tag as string).Split(':');
            if (int.Parse(s[1]) == ifid || int.Parse(s[0])==bid)
            {
                return nd;
            }
            foreach (TreeNode n in nd.Nodes)
            {
                TreeNode res = searchFarm(ifid,bid, n);
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
			treeView1.BeginUpdate();
			tr.Collapse(false);
            //tr.Expand();
            manual = false;
            treeView1.SelectedNode = searchFarm((int)b.farm(), tr);
            if (treeView1.SelectedNode != null) treeView1.SelectedNode.Expand();
			treeView1.EndUpdate();
            manual = true;
        }

        private FarmDrawer.DrawTier tierFromBuilding(Building b)
        {
            List<string> rabs=new List<string>();
            for (int i=0;i<b.secs();i++)
                    rabs.Add(b.use(i));
            return new FarmDrawer.DrawTier(b.id(),b.itype(),b.delims(),b.nest(),b.heater(),rabs.ToArray(),b.repair());
        }

        private void DrawFarm(int farm)
        {
            int[] tiers=Engine.db().getTiers(farm);
            FarmDrawer.DrawTier t1 = tierFromBuilding(Engine.db().getBuilding(tiers[0]));
            FarmDrawer.DrawTier t2=null;
            if (tiers[1]!=0)
                t2=tierFromBuilding(Engine.db().getBuilding(tiers[1]));
            farmDrawer1.setFarm(farm,t1,t2);
            updateMenu();
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                farmDrawer1.setFarm(0, null, null);
                return;
            }
            preBuilding = buildNum();
            int farm = farmNum();
            if (farm == 0)
            {
                farmDrawer1.setFarm(0, null, null);
                updateMenu();
                return;
            }
            if (manual)
            {
                manual = false;
                listView1.SelectedItems.Clear();
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    Building b = listView1.Items[i].Tag as Building;
                    if ((int)b.farm() == farm)
                    {
                        listView1.Items[i].Selected = true;
                        listView1.Items[i].EnsureVisible();
                    }
                }
                manual = true;
            }
            DrawFarm(farm);
        }

        private void farmDrawer1_ValueChanged(object sender, BuildingControl.BCEvent e)
        {
            try
            {
                RabNetEngBuilding b = Engine.get().getBuilding(e.tier);
                switch (e.type)
                {
                    case BuildingControl.BCEvent.EVTYPE.REPAIR: b.setRepair(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.NEST: b.setNest(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.NEST2: b.setNest2(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.HEATER: b.setHeater(e.value); break;
                    case BuildingControl.BCEvent.EVTYPE.HEATER2: b.setHeater2(e.value); break;
                    case BuildingControl.BCEvent.EVTYPE.DELIM: b.setDelim(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.DELIM1: b.setDelim1(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.DELIM2: b.setDelim2(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.DELIM3: b.setDelim3(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.VIGUL: b.setVigul(e.value); break;
                }
            }
            catch (RabNetEngBuilding.ExFarmNotEmpty ex)
            {
                if (MessageBox.Show(this, ex.Message + ". Расселить ферму?", "Ферма не пуста", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    replaceMenuItem.PerformClick();
                }
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            DrawFarm(e.farm);
        }

        public override ContextMenuStrip getMenu()
        {
            return actMenu;
        }

        public void updateMenu()
        {
            killMenuItem.Visible = replaceMenuItem.Visible = false;
            addBuildingMenuItem.Visible = addFarmMenuItem.Visible = false;
            changeFarmMenuItem.Visible = deleteBuildingMenuItem.Visible = false;
            shedReportMenuItem.Visible = false;
            if (listView1.Focused)
            {
                killMenuItem.Visible = replaceMenuItem.Visible = (listView1.SelectedItems.Count > 0);
            }
            else if (treeView1.Focused && treeView1.SelectedNode!=null)
            {
                addBuildingMenuItem.Visible = addFarmMenuItem.Visible = !isFarm();
                changeFarmMenuItem.Visible = isFarm();
                deleteBuildingMenuItem.Visible = true;
                shedReportMenuItem.Visible = emptyRevMenuItem.Visible=!isFarm();
            }
        }

        private void killMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            Building b=null;
            KillForm f = new KillForm();
            foreach (ListViewItem li in listView1.SelectedItems)
            {
                Building b2 = li.Tag as Building;
                if (b2 != b)
                {
                    b = b2;
                    for (int i = 0; i < b.secs(); i++)
                        if (b.busy(i)!=0)
                        f.addRabbit(b.busy(i));
                }
            }
            if(f.ShowDialog() == DialogResult.OK)
                _rsb.run();
        }

        private void replaceMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            Building b = null;
            ReplaceForm f = new ReplaceForm();
            foreach (ListViewItem li in listView1.SelectedItems)
            {
                Building b2 = li.Tag as Building;
                if (b2 != b)
                {
                    b = b2;
                    for (int i = 0; i < b.secs(); i++)
                        if (b.busy(i)!=0)
                            f.addRabbit(b.busy(i));
                }
            }
            if(f.ShowDialog() == DialogResult.OK)
                _rsb.run();
        }

        private bool isFarm(TreeNode tn){return farmNum(tn)!=0;}
        private bool isFarm(){return isFarm(treeView1.SelectedNode);}
        private int farmNum(TreeNode tn){return int.Parse((tn.Tag as String).Split(':')[1]);}
        private int farmNum(){return farmNum(treeView1.SelectedNode);}
        private int buildNum(TreeNode tn){return int.Parse((tn.Tag as String).Split(':')[0]);}
        private int buildNum() { return buildNum(treeView1.SelectedNode); }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                treeView1.SelectedNode = e.Node;
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            treeView1.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_DragOver_1(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeNode))) return;
            TreeNode child=e.Data.GetData(typeof(TreeNode)) as TreeNode;
            Point px=treeView1.PointToClient(new Point(e.X,e.Y));
            TreeNode newpar = treeView1.GetNodeAt(px);
            if (newpar == null || child==null) return;
            if (child == treeView1.Nodes[0]) return;
            if (isFarm(newpar)) return;
            if (child == newpar) return;
            if (MessageBox.Show(this, "Переместить " + child.Text + " в " + newpar.Text + "?", "Перемещение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int bid = buildNum(child);
                int to = buildNum(newpar);
                Engine.db().replaceBuilding(bid, to);
                _rsb.run();
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (action != 0)
            {
                if (e.Label == null || e.Label == "")
                {
                    e.CancelEdit = true;
                    treeView1.SelectedNode.Remove();
                }
                else
                {
                    Engine.db().addBuilding(buildNum(nodeToAdd), e.Label);
                }
                nodeToAdd = null;
                action = 0;
            }
            if (e.Label == null || e.Label == "") { e.CancelEdit = true; return; }
            if (!manual) return;
            if (e.Node == treeView1.Nodes[0])
            {
                e.CancelEdit = true;
                return;
            }
            if (e.Node.Text != e.Label)
            {
                Engine.db().setBuildingName(buildNum(e.Node), e.Label);
                e.CancelEdit = false;
            }
            timer1.Start();
        }

        private bool askDelete()
        {
            return (MessageBox.Show(this, "Удалить " + treeView1.SelectedNode.Text + "?", "Удаление",
                MessageBoxButtons.YesNo) == DialogResult.Yes);
        }

        private void deleteBuildingMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (isFarm())
            {
                int[] tiers = Engine.db().getTiers(farmNum());
                bool candelete = true;
                for (int i = 0; i < 2; i++)
                    if (tiers[i]!=0)
                    {
                        Building b = Engine.db().getBuilding(tiers[i]);
                        for (int j = 0; j < b.secs(); j++)
                        {
                            if (b.busy(j) != 0)
                                candelete = false;
                        }
                    }
                if (candelete && askDelete())
                {
                    preBuilding = buildNum(treeView1.SelectedNode.Parent);
                    Engine.db().deleteFarm(farmNum());
                    _rsb.run();
                }
                else
                    MessageBox.Show("Ферма не пуста.");
            }
            else
            {
                if (treeView1.SelectedNode.Nodes.Count > 0)
                    MessageBox.Show("Имеются вложенные строения");
                else if (askDelete())
                {
                    preBuilding = buildNum(treeView1.SelectedNode.Parent);
                    Engine.db().deleteBuilding(buildNum());
                    _rsb.run();
                }
            }
        }

        private void addBuildingMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (isFarm()) return;
            nodeToAdd = treeView1.SelectedNode;
            TreeNode nd=nodeToAdd.Nodes.Add(nuBuild);
            manual = false;
            action = 1;
            nd.Tag = "0:0:new";
            treeView1.SelectedNode = nd;
            preBuilding = buildNum(nodeToAdd);
            nd.BeginEdit();
            manual = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            _rsb.run();
        }

        private void treeView1_Enter(object sender, EventArgs e)
        {
            updateMenu();
        }

        private void listView1_Enter(object sender, EventArgs e)
        {
            updateMenu();
        }

        private void addFarmMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (isFarm()) return;
            if (nofarms.Count == 0)
            {
                MessageBox.Show("Достигнуто максимальное количество ферм.");
                return;
            }
            if(new MiniFarmForm(buildNum(), nofarms.ToArray()).ShowDialog() == DialogResult.OK)
                _rsb.run();
        }

        private void changeFarmMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (!isFarm()) return;
            int fid = farmNum();
            MainForm.protectTest(Engine.db().getMFCount());
            if (new MiniFarmForm(fid).ShowDialog() == DialogResult.OK) _rsb.run();
        }

        private XmlDocument getBuildDoc(int bid)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement rw = (XmlElement)doc.AppendChild(doc.CreateElement("Rows")).AppendChild(doc.CreateElement("Row"));
            rw.AppendChild(doc.CreateElement("date")).AppendChild(doc.CreateTextNode(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString()));
            String ad = bid == 0 ? "farm" : getAddress(-1, bid);
            rw.AppendChild(doc.CreateElement("address")).AppendChild(doc.CreateTextNode(ad));
            return doc;
        }

        private void shedReportMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (treeView1.SelectedNode==null) return;
            if (isFarm()) return;
            Filters f = new Filters();
            int bid=buildNum();
            f["bld"] = bid.ToString();
            f["suck"] = Engine.opt().getOption(Options.OPT_ID.SUCKERS);
            new ReportViewForm(myReportType.SHED, new XmlDocument[]{
                Engine.db().makeReport(myReportType.SHED,f),getBuildDoc(bid)}).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void emptyRevMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (treeView1.SelectedNode == null) return;
            if (isFarm()) return;
            Filters f = new Filters();
            int bid = buildNum();
            f["bld"] = bid.ToString();
            new ReportViewForm(myReportType.REVISION, new XmlDocument[]{
                Engine.db().makeReport(myReportType.REVISION,f),getBuildDoc(bid)}).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void makeExcel()
        {
#if !DEMO
            ExcelMaker.MakeExcelFromLV(listView1, "Строения");
#endif
        }
    }

    public class TVNodeSorter : IComparer
    {
        public string strpart(string str)
        {
            int i = str.Length - 1;
            while (Char.IsDigit(str[i]))
            {
                i--;
                if (i == -1) return str;
            }
            i++;
            return str.Substring(0, i);
        }
        public int Compare(object x, object y)
        {
            string s1 = (x as TreeNode).Text;
            string s2 = (y as TreeNode).Text;
            string ss1 = strpart(s1);
            string ss2 = strpart(s2);
            if (ss1 != ss2)
            {
                if (ss2[0] == '№') return -1;
                if (ss1[0] == '№') return 1;
                return String.Compare(s1, s2);
            }
            int i1 = int.Parse(s1.Substring(ss1.Length));
            int i2 = int.Parse(s2.Substring(ss2.Length));
            return i1 - i2;
        }

    }

}
