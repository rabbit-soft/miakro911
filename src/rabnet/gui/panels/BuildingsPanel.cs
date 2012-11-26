using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using log4net;
#if PROTECTED
using RabGRD;
#endif

namespace rabnet
{
    public partial class BuildingsPanel : RabNetPanel
    {
        /// <summary>
        /// Нужна потому что под номер клетки отводится 6 знаков
        /// Иначе придется изменять форматирование номеров клеток во всей программе
        /// </summary>
        private const int MAX_FARMS_COUNT = 999999;
        /// <summary>
        /// Сколько максимально может предлагаться новых ферм
        /// Защита от расхода памяти
        /// </summary>
        private const int NEW_FARMS_LIMIT = 0xff;
        public const int DEMO_MAX_FARMS = 10;
        private bool manual = true;
        //protected static readonly ILog _logger = LogManager.GetLogger(typeof(MainForm));
        const String NEW_BUILDING = "Новое строение";
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
        public static int GetFarmsCount(BldTreeData td)
        {
            int res = 0;
            //String[] st = td.caption.Split(':');
            //if (st.Length == 3)
                if (td.TierID != 0)
                {
                    res++;
                    return res;
                }
            if (td.ChildNodes != null)
            {
                for (int i = 0; i < td.ChildNodes.Count; i++)
                {
                    res += GetFarmsCount(td.ChildNodes[i]);
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
#elif DEMO
                if (i <= DEMO_MAX_FARMS)
#endif              
                if (nofarms.Count<NEW_FARMS_LIMIT)
                    nofarms.Add(i);
                else nofarms[NEW_FARMS_LIMIT-1]=i;
            }
            nofarm = farm + 1;
        }

        /// <summary>
        /// Создает ветку в дереве строений.
        /// </summary>
        /// <param name="parent">Ветка-родитель</param>
        /// <param name="name">Название фетки</param>
        /// <param name="td"></param>
        /// <returns></returns>
        private TreeNode makeNode(TreeNode parent, String name, BldTreeData td)
        {
            TreeNode n = null;
            if (parent == null)
                n = treeView1.Nodes.Add(name);
            else
                n = parent.Nodes.Add(name);

            TreeNode child;
            if (td.ChildNodes!=null)
                //for (int i = 0; i < td.Childrens.Count; i++)
                while (td.ChildNodes.Count>0)
                {
                    child = makeNode(n, td.ChildNodes[0].Name, td.ChildNodes[0]);
                    child.Tag = td.ChildNodes[0];                  
                    int fid = td.ChildNodes[0].TierID;
                    addNoFarm(fid);
                    if (maxfarm < fid)
                        maxfarm = fid;
                    if (td.ChildNodes[0].ID == preBuilding)
                    {
                        treeView1.SelectedNode = child;
                        treeView1.SelectedNode.Expand();
                    }
                    td.ChildNodes.RemoveAt(0);
                }
            n.Tag = td;
            return n;
        }

        /// <summary>
        /// Подготовка перед получением данных
        /// </summary>
        protected override IDataGetter onPrepare(Filters f)
        {
            manual = false;
            treeView1.Nodes.Clear();
            nofarms.Clear();
            nofarm = 1;
            maxfarm = 0;
            BldTreeData buildTree = Engine.db().buildingsTree();
            TreeNode n = makeNode(null, "Ферма", buildTree);
#if PROTECTED
//            if (nofarm<=PClient.get().farms())
            if (nofarm <= GRD.Instance.GetFarmsCnt())
#elif DEMO
            if (Engine.db().getMFCount() <= DEMO_MAX_FARMS)
#endif
            if (nofarm <= MAX_FARMS_COUNT)
                nofarms.Add(nofarm); 
            MainForm.ProtectTest(BuildingsPanel.GetFarmsCount(buildTree));
            //treeView1.Sort();
            manual = true;
            //n.Tag = new int[] { 0, 0 };//"0:0";
            n.Expand();
            f[Filters.SHORT] = Engine.opt().getOption(Options.OPT_ID.SHORT_NAMES);
            f[Filters.DBL_SURNAME] = Engine.opt().getOption(Options.OPT_ID.DBL_SURNAME);
            colSort.Prepare();
            IDataGetter dg = DataThread.db().getBuildingsRows(f);
            _rsb.SetText(1, dg.getCount().ToString() + " МИНИфермы");
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

            for (int i = 0; i < b.Sections; i++)
            {
                if (b.Areas[i] != prevnm || b.Farm!=prevfarm)
                {
                    manual = false;
                    ListViewItem it = listView1.Items.Add(Building.Format(b.Farm) + b.Areas[i]);//№
                    prevfarm = b.Farm;
                    prevnm = b.Areas[i];
                    it.Tag = b.ID.ToString();
                    it.SubItems.Add(b.TypeName_Rus);//Ярус
                    it.SubItems.Add(b.dep(i));//Отделение
                    String stat = "unk";
                    if (b.Repair) 
                        stat = "ремонт";
                    else
                    {
                        if (b.Busy[i] == 0) stat = "-";
                        else stat = b.use(i);
                    }
                    it.SubItems.Add(stat);//Статус
                    String nst = "";
                    String htr = "";
                    if (b.NestHeaterCount > 0)
                    {
                        int nid = 0;
                        if (b.NestHeaterCount > 1) nid = i;
                        nst = (b.Nests[nid] == '1') ? "да" : "нет";
                        htr = (b.Heaters[nid] == '0' ? "нет" : (b.Heaters[nid] == '1' ? "выкл" : "вкл"));
                        if (b.TypeName == BuildingType.Jurta)
                            if ((b.Delims[0] == '1' && i == 0) || (b.Delims[0] == '0' && i == 1))
                            {
                                nst = "";
                                htr = "";
                            }
                        if (b.TypeName==BuildingType.Complex)
                            if (i != 0)
                            {
                                nst = "";
                                htr = "";
                            }
                    }
                    it.SubItems.Add(nst);//Гнездовье
                    it.SubItems.Add(htr);//Грелка
                    it.SubItems.Add(getAddress(b.Farm));//Адрес
                    it.SubItems.Add(b.Notes);//Заметки
                    it.Tag = b;
                    manual = true;
                }
            }

			colSort.SemiReady();
        }

        

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
        private String getAddress(int ifid)
        {
            return getAddress(ifid, -1);
        }

        private TreeNode searchFarm(int tierID, TreeNode nd)
        {
            return searchFarm(tierID, -1, nd);
        }
        private TreeNode searchFarm(int tierID,int bid,TreeNode nd)
        {
            BldTreeData td = (nd.Tag as BldTreeData);//.Split(':');
            if (td.TierID == tierID || td.ID == bid)
            {
                return nd;
            }

            TreeNode res = null;
            foreach (TreeNode n in nd.Nodes)
            {
                res = searchFarm(tierID, bid, n);
                if (res != null)
                    return res;
            }
            return null;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1 || listView1.SelectedItems[0] == null || !manual) return;

            ListViewItem li = listView1.SelectedItems[0];
            Building b = li.Tag as Building;
            TreeNode treeRoot = treeView1.Nodes[0];
            treeView1.BeginUpdate();
            treeRoot.Collapse(false);
            //tr.Expand();
            manual = false;
            treeView1.SelectedNode = searchFarm(b.Farm, treeRoot);
            if (treeView1.SelectedNode != null)
                treeView1.SelectedNode.Expand();
            treeView1.EndUpdate();
            manual = true;
        }

        private FarmDrawer.DrawTier tierFromBuilding(Building b)
        {
            List<string> rabs = new List<string>();
            RabNetEngRabbit rner;
            string livesIn = "";
            for (int i = 0; i < b.Sections; i++)
            {
                livesIn = b.use(i);
                if (b.Busy[i] > 0)
                {
                    rner = Engine.get().getRabbit(b.Busy[i]);
                    if (rner.YoungCount != 0)
                    {
                        foreach(YoungRabbit or in rner.Youngers)
                            livesIn += String.Format(" (+{0:d} в:{1:d})", or.Group,or.Age);
                    }
                    foreach (OneRabbit n in rner.Neighbors)
                        livesIn += String.Format("{0:s}[{1:s}]",Environment.NewLine,n.NameFull); ;
                }
                rabs.Add(livesIn);
            }
           
            return new FarmDrawer.DrawTier(b.ID,b.TypeName,b.Delims,b.Nests,b.Heaters,rabs.ToArray(),b.Repair);
        }

        private void drawFarm(int farm)
        {
            int[] tiers=Engine.db().getTiers(farm);
            FarmDrawer.DrawTier t1 = tierFromBuilding(Engine.db().getBuilding(tiers[0]));
            FarmDrawer.DrawTier t2=null;
            if (tiers[1]!=0)
                t2=tierFromBuilding(Engine.db().getBuilding(tiers[1]));
            farmDrawer1.SetFarm(farm,t1,t2);
            updateMenu();
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                farmDrawer1.SetFarm(0, null, null);
                return;
            }
            preBuilding = buildNum();
            int farm = farmNum();
            if (farm == 0)
            {
                farmDrawer1.SetFarm(0, null, null);
                updateMenu();
                return;
            }
            if (manual)
            {
                manual = false;
                listView1.SelectedItems.Clear();
                Building b = null;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    b = listView1.Items[i].Tag as Building;
                    if ((int)b.Farm == farm)
                    {
                        listView1.Items[i].Selected = true;
                        listView1.Items[i].EnsureVisible();
                    }
                }
                manual = true;
            }
            drawFarm(farm);
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
            drawFarm(e.farm);
        }

        public override ContextMenuStrip getMenu()
        {
            return actMenu;
        }

        public void updateMenu()
        {
            killMenuItem.Visible = replaceMenuItem.Visible =  tsSplitter.Visible =
                addBuildingMenuItem.Visible = addFarmMenuItem.Visible=
                changeFarmMenuItem.Visible = deleteBuildingMenuItem.Visible =
                shedReportMenuItem.Visible = emptyRevMenuItem.Visible = false;
            if (listView1.Focused)
            {
                killMenuItem.Visible = replaceMenuItem.Visible = (listView1.SelectedItems.Count > 0);
            }
            else if (treeView1.Focused && treeView1.SelectedNode!=null)
            {
                addBuildingMenuItem.Visible = addFarmMenuItem.Visible = !isFarm();
                changeFarmMenuItem.Visible = isFarm();
                deleteBuildingMenuItem.Visible = true;
                tsSplitter.Visible = shedReportMenuItem.Visible = emptyRevMenuItem.Visible = !isFarm();                
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
                    for (int i = 0; i < b.Sections; i++)
                        if (b.Busy[i]>0)
                            f.addRabbit(b.Busy[i]);
                }
            }
            if(f.ShowDialog() == DialogResult.OK)
                _rsb.Run();
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
                    for (int i = 0; i < b.Sections; i++)
                        if (b.Busy[i]>0)
                            f.AddRabbit(b.Busy[i]);
                }
            }
            if(f.ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private bool isFarm(TreeNode tn) { return farmNum(tn) != 0; }
        private bool isFarm() { return isFarm(treeView1.SelectedNode); }
        private int farmNum(TreeNode tn) { return (tn.Tag as BldTreeData).TierID; }
        private int farmNum() { return farmNum(treeView1.SelectedNode); }
        private int buildNum(TreeNode tn) { return (tn.Tag as BldTreeData).ID; }
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
                _rsb.Run();
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (action != 0)
            {
                if (e.Label == null || e.Label == "" || e.Label.Contains("/"))
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
            if (e.Label == null || e.Label == "") 
            { 
                e.CancelEdit = true; 
                return; 
            }
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

            TreeNode sNode = treeView1.SelectedNode;
            if (isFarm())
            {
                int[] tiers = Engine.db().getTiers(farmNum());
                bool candelete = true;
                for (int i = 0; i < 2; i++)
                    if (tiers[i]!=0)
                    {
                        Building b = Engine.db().getBuilding(tiers[i]);
                        for (int j = 0; j < b.Sections; j++)
                        {
                            if (b.Busy[j] > 0)
                                candelete = false;
                        }
                    }
                if (candelete)
                {
                    if (askDelete())
                    {
                        preBuilding = buildNum(sNode.Parent);
                        Engine.db().deleteFarm(farmNum());
                        _rsb.Run();
                    }
                }
                else
                    MessageBox.Show(@"Ферма не пуста.");
            }
            else
            {
                if (sNode.Nodes.Count > 0)
                    MessageBox.Show("Имеются вложенные строения");
                else if (askDelete())
                {
                    preBuilding = buildNum(sNode.Parent);
                    Engine.db().deleteBuilding(buildNum());
                    _rsb.Run();
                }
            }
        }

        private void addBuildingMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            if (isFarm()) return;
            nodeToAdd = treeView1.SelectedNode;
            TreeNode nd = nodeToAdd.Nodes.Add(NEW_BUILDING);
            manual = false;
            action = 1;
            nd.Name = "new";
            nd.Tag = new int[]{0,0};
            treeView1.SelectedNode = nd;
            preBuilding = buildNum(nodeToAdd);
            nd.BeginEdit();
            manual = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            _rsb.Run();
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
            if (nofarms.Count == 0 
#if DEMO
                || Engine.db().getMFCount() >= DEMO_MAX_FARMS
#elif PROTECTED
                || Engine.db().getMFCount() >= GRD.Instance.GetFarmsCnt()
#endif
                )
            {
                MessageBox.Show("Достигнуто максимальное количество ферм.");
                return;
            }
            if(new MiniFarmForm(buildNum(), nofarms.ToArray()).ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void changeFarmMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (!isFarm()) return;
            int fid = farmNum();
            MainForm.ProtectTest(Engine.db().getMFCount());
            if (new MiniFarmForm(fid).ShowDialog() == DialogResult.OK) _rsb.Run();
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
            f["nest_out"] = Engine.opt().getOption(Options.OPT_ID.NEST_OUT);
            //f["suck"] = Engine.opt().getOption(Options.OPT_ID.COUNT_SUCKERS);
            ReportViewForm dlg =  new ReportViewForm(
                myReportType.SHED, 
                new XmlDocument[] 
                { 
                    Engine.db().makeReport(myReportType.SHED,f),getBuildDoc(bid)
                });
            dlg.ExcelEnabled = false;
            dlg.ShowDialog();
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
            return i1.CompareTo(i2);
        }

    }

}
