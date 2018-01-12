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
using rabnet;
using rabnet.components;
using rabnet.forms;
using rabnet.filters;
using System.Text.RegularExpressions;
#if PROTECTED
using RabGRD;
#endif

namespace rabnet.panels
{
    public partial class BuildingsPanel : RabNetPanel
    {
        private const int FIELD_NOTES = 7;

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
        public const int DEMO_MAX_FARMS = 100;
        private bool manual = true;
        //protected static readonly ILog _logger = LogManager.GetLogger(typeof(MainForm));
        const String NEW_BUILDING = "Новое строение";
        /// <summary>
        /// Предлагаемые Номера новых клеток
        /// </summary>
        List<int> _freeFarmsId = new List<int>();
        //int _nofarm = 1;
        TreeNode nodeToAdd = null;
        int action = 0;
        int preBuilding = 0;

        /// <summary>
        /// Нужен для быстрого подбора адреса фермы
        /// </summary>
        Dictionary<int, string> farmAddresses = new Dictionary<int, string>();

        public BuildingsPanel() : base() { }
        //int _maxfarm = 0;

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
            if (td.FarmId != 0) {
                res++;
                return res;
            }
            if (td.ChildNodes != null) {
                for (int i = 0; i < td.ChildNodes.Count; i++) {
                    res += GetFarmsCount(td.ChildNodes[i]);
                }
            }
            return res;
        }

        public BuildingsPanel(RabStatusBar bsb)
            : base(bsb, new BuildingsFilter())
        {
            _colSort = new ListViewColumnSorter(listView1, new int[] { }, Options.OPT_ID.BUILD_LIST);
            listView1.ListViewItemSorter = null;
            treeView1.TreeViewNodeSorter = new TVNodeSorter();
            MakeExcel = new RSBEventHandler(this.makeExcel);
        }

        /// <summary>
        /// Создает ветку в дереве строений.
        /// </summary>
        /// <param name="parent">Ветка-родитель</param>
        /// <param name="name">Название ветки</param>
        /// <param name="treeData"></param>
        /// <returns></returns>
        private TreeNode makeNode(TreeNode parent, String name, BldTreeData treeData, List<int> idList)
        {
            TreeNode n = null;
            if (parent == null) {
                n = treeView1.Nodes.Add(name);
            } else {
                n = parent.Nodes.Add(name);
            }

            TreeNode child;
            if (treeData.ChildNodes != null) {
                while (treeData.ChildNodes.Count > 0) {
                    BldTreeData childTreeData = treeData.ChildNodes[0];
                    child = this.makeNode(n, childTreeData.Name, childTreeData, idList);
                    //child.Tag = treeData.ChildNodes[0];
                    if (childTreeData.FarmId != 0) {
                        idList.Add(childTreeData.FarmId);
                    }
                    if (childTreeData.ID == preBuilding) {
                        treeView1.SelectedNode = child;
                        treeView1.SelectedNode.Expand();
                    }

                    if (!this.farmAddresses.ContainsKey(childTreeData.FarmId)) {
                        this.farmAddresses.Add(childTreeData.FarmId, child.FullPath);
                    }
                    treeData.ChildNodes.RemoveAt(0);
                }
            }
            n.Tag = treeData;
            return n;
        }

        /// <summary>
        /// Подготовка перед получением данных
        /// </summary>
        protected override IDataGetter onPrepare(Filters f)
        {
            base.onPrepare(f);
            manual = false;
            treeView1.Nodes.Clear();
            _freeFarmsId.Clear();
            BldTreeData buildTree = Engine.db().buildingsTree();
            List<int> busyFarmsId = new List<int>();
            TreeNode n = this.makeNode(null, "Ферма", buildTree, busyFarmsId);
            MainForm.ProtectTest(busyFarmsId.Count);
            ///ищем предлагаемые имена
            _freeFarmsId = getNewFarmCandidates(busyFarmsId);
            int allowFarms = 0;
#if PROTECTED
            allowFarms = GRD.Instance.GetFarmsCnt() - Engine.db().getMFCount();
#elif DEMO
            allowFarms = DEMO_MAX_FARMS - Engine.db().getMFCount();
#endif
            if (allowFarms > 0 && _freeFarmsId.Count > allowFarms) {
                int last = _freeFarmsId[_freeFarmsId.Count - 1];
                _freeFarmsId = _freeFarmsId.GetRange(0, allowFarms - 1);
                _freeFarmsId.Add(last);
            }

            MainForm.ProtectTest(BuildingsPanel.GetFarmsCount(buildTree));
            manual = true;
            n.Expand();
            f[Filters.SHORT] = Engine.opt().getOption(Options.OPT_ID.SHORT_NAMES);
            f[Filters.DBL_SURNAME] = Engine.opt().getOption(Options.OPT_ID.DBL_SURNAME);

            IDataGetter dg = Engine.db2().getBuildingsRows(f);
            _rsb.SetText(1, dg.getCount().ToString() + " ярусов");
            _rsb.SetText(2, dg.getCount2().ToString() + " МИНИферм");
            _runF = f;
            return dg;
        }

        /// <summary>
        /// Из списка занятых клеток получает список ID которые можно присвоить новой МИНИферме
        /// </summary>
        /// <param name="busyFarmsId">Список имеющихся МИНИферм</param>
        /// <returns>Список ID предлагаемых МИНИферм</returns>
        private List<int> getNewFarmCandidates(List<int> busyFarmsId)
        {
            busyFarmsId.Sort();
            List<int> result = new List<int>();
            int fId = 1, j = 0;
            for (int i = 0; i < busyFarmsId.Count; i++) {
                if (fId < busyFarmsId[i]) {
                    for (j = fId; j < busyFarmsId[i]; j++) {
                        result.Add(j);
                    }
                    fId = busyFarmsId[i];
                }
                fId++;
            }
            if (result.Count > MAX_FARMS_COUNT - 1) {
                result = result.GetRange(0, MAX_FARMS_COUNT - 1);
            }
            if (fId <= MAX_FARMS_COUNT) {
                result.Add(fId);
            }
            return result;
        }

        /// <summary>
        /// Добавление новой строчки в ListView
        /// </summary>
        /// <param name="data">Одна запись</param>
        protected override void onItem(IData data)
        {
            Building b = data as Building;
            string prevnm = "";
            int prevfarm = 0;

            for (int i = 0; i < b.Sections; i++) {
                if ((b.Areas(i) != prevnm || b.Farm != prevfarm) && !b.IsAbsorbed(i)) {
                    manual = false;
                    ListViewItem it = listView1.Items.Add(Building.Format(b.Farm) + b.Areas(i));//№
                    prevfarm = b.Farm;
                    prevnm = b.Areas(i);
                    it.SubItems.Add(Building.GetNameRus(b.Type, _runF.safeBool(Filters.SHORT)));//Ярус
                    it.SubItems.Add(b.Descr(i, _runF.safeBool(Filters.SHORT)));//Отделение
                    String stat = "unk";
                    if (b.Repair) {
                        stat = "ремонт";
                    } else {
                        stat = b.Busy[i].ID == 0 ? "-" : b.Busy[i].Name;
                    }
                    it.SubItems.Add(stat);//Статус
                    String nst = "";
                    String htr = "";
                    if (b.NestHeaterCount > 0) {
                        int nid = 0;
                        if (b.NestHeaterCount > 1) {
                            nid = i;
                        }
                        nst = (b.Nests[nid] == '1') ? "да" : "нет";
                        htr = (b.Heaters[nid] == '0' ? "нет" : (b.Heaters[nid] == '1' ? "выкл" : "вкл"));
                        if (b.Type == BuildingType.Jurta) {
                            if ((b.Delims[0] == '1' && i == 0) || (b.Delims[0] == '0' && i == 1)) {
                                nst = "";
                                htr = "";
                            }
                        }
                        if (b.Type == BuildingType.Complex) {
                            if (i != 0) {
                                nst = "";
                                htr = "";
                            }
                        }
                    }
                    it.SubItems.Add(nst);//Гнездовье
                    it.SubItems.Add(htr);//Грелка
                    it.SubItems.Add(this.farmAddresses[b.Farm]);//Адрес //todo нужен ли адрес ?
                    it.SubItems.Add(b.Notes[i]);//Заметки
                    it.Tag = b;
                    manual = true;
                }
            }
        }

        private String getAddress(int ifid, int bid)
        {
            TreeNode nd = this.searchFarm(ifid, bid, treeView1.Nodes[0]);
            if (nd != null) {
                return nd.FullPath;
            }
            return "";
        }

        private String getAddress(int ifid)
        {
            return getAddress(ifid, -1);
        }

        private TreeNode searchFarm(int tierID, TreeNode nd)
        {
            return searchFarm(tierID, -1, nd);
        }

        private TreeNode searchFarm(int tierID, int bid, TreeNode nd)
        {
            BldTreeData td = (nd.Tag as BldTreeData);//.Split(':');
            if (td.FarmId == tierID || td.ID == bid) {
                return nd;
            }

            TreeNode res = null;
            foreach (TreeNode n in nd.Nodes) {
                res = this.searchFarm(tierID, bid, n);
                if (res != null) {
                    return res;
                }
            }
            return null;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1 || listView1.SelectedItems[0] == null || !manual) {
                return;
            }

            ListViewItem li = listView1.SelectedItems[0];
            Building b = li.Tag as Building;
            TreeNode treeRoot = treeView1.Nodes[0];
            treeView1.BeginUpdate();
            treeRoot.Collapse(false);
            //tr.Expand();
            manual = false;
            treeView1.SelectedNode = this.searchFarm(b.Farm, treeRoot);
            if (treeView1.SelectedNode != null) {
                treeView1.SelectedNode.Expand();
            }
            treeView1.EndUpdate();
            manual = true;
        }

        private FarmDrawer.DrawTier tierFromBuilding(Building b)
        {
            List<string> rabs = new List<string>();
            RabNetEngRabbit rner;
            string livesIn = "";
            for (int i = 0; i < b.Sections; i++) {
                livesIn = b.Busy[i].Name;
                if (b.Busy[i].ID > 0) {
                    try {
                        rner = Engine.get().getRabbit(b.Busy[i].ID);
                        if (rner.YoungCount != 0) {
                            foreach (YoungRabbit or in rner.Youngers) {
                                livesIn += String.Format(" (+{0:d} в:{1:d})", or.Group, or.Age);
                            }
                        }
                        foreach (OneRabbit n in rner.Neighbors) {
                            livesIn += String.Format("{0:s}[{1:s}]", Environment.NewLine, n.NameFull);
                        }
                    } catch (RabNetException exc) {
                        _logger.Warn(exc);
                        livesIn = "[!" + exc.Message + "!]";
                    }
                }
                rabs.Add(livesIn);
                //if (b.Type == BuildingType.Quarta && i > 0 && b.Delims[i - 1] == '0')
                //i++;
            }

            return new FarmDrawer.DrawTier(b, rabs);
        }

        private void drawFarm(int farm)
        {
            int[] tiers = Engine.db().getTiers(farm);
            FarmDrawer.DrawTier t1 = tierFromBuilding(Engine.db().getBuilding(tiers[0]));
            FarmDrawer.DrawTier t2 = null;
            if (tiers[1] != 0) {
                t2 = tierFromBuilding(Engine.db().getBuilding(tiers[1]));
            }
            farmDrawer1.SetFarm(farm, t1, t2);
            updateMenu();
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null) {
                farmDrawer1.SetFarm(0, null, null);
                return;
            }
            preBuilding = buildNum();
            int farm = farmNum();
            if (farm == 0) {
                farmDrawer1.SetFarm(0, null, null);
                updateMenu();
                return;
            }
            if (manual) {
                manual = false;
                listView1.SelectedItems.Clear();
                Building b = null;
                for (int i = 0; i < listView1.Items.Count; i++) {
                    b = listView1.Items[i].Tag as Building;
                    if ((int)b.Farm == farm) {
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
            try {
                RabNetEngBuilding b = Engine.get().getBuilding(e.tier);
                switch (e.type) {
                    case BuildingControl.BCEvent.EVTYPE.REPAIR: b.setRepair(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.NEST: b.setNest(e.val(), 0); break;
                    case BuildingControl.BCEvent.EVTYPE.NEST2: b.setNest(e.val(), 1); break;
                    case BuildingControl.BCEvent.EVTYPE.HEATER: b.setHeater(e.value, 0); break;
                    case BuildingControl.BCEvent.EVTYPE.HEATER2: b.setHeater(e.value, 1); break;
                    case BuildingControl.BCEvent.EVTYPE.DELIM: b.SetOneDelim(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.DELIM1: b.SetDelim1(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.DELIM2: b.SetDelim2(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.DELIM3: b.SetDelim3(e.val()); break;
                    case BuildingControl.BCEvent.EVTYPE.VIGUL: b.setVigul(e.value); break;
                }
            } catch (RabNetEngBuilding.ExFarmNotEmpty ex) {
                if (MessageBox.Show(this, ex.Message + ". Расселить ферму?", "Ферма не пуста", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    replaceMenuItem.PerformClick();
                }
            } catch (ApplicationException ex) {
                MessageBox.Show(ex.Message);
            }
            drawFarm(e.farm);
        }

        public void updateMenu()
        {
            killMenuItem.Visible = replaceMenuItem.Visible = tsSplitter.Visible =
                addBuildingMenuItem.Visible = addFarmMenuItem.Visible =
                changeFarmMenuItem.Visible = deleteBuildingMenuItem.Visible =
                shedReportMenuItem.Visible = emptyRevMenuItem.Visible = miNotesEdit.Visible = false;

            if (listView1.Focused) {
                killMenuItem.Visible = replaceMenuItem.Visible = miNotesEdit.Visible = (listView1.SelectedItems.Count > 0);
            } else if (treeView1.Focused && treeView1.SelectedNode != null) {
                addBuildingMenuItem.Visible = addFarmMenuItem.Visible = !isFarm();
                changeFarmMenuItem.Visible = isFarm();
                deleteBuildingMenuItem.Visible = true;
                tsSplitter.Visible = shedReportMenuItem.Visible = emptyRevMenuItem.Visible = !isFarm();
            }
        }

        private void killMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1) {
                return;
            }

            Building b = null;
            KillForm f = new KillForm();
            foreach (ListViewItem li in listView1.SelectedItems) {
                Building b2 = li.Tag as Building;
                if (b2 != b) {
                    b = b2;
                    for (int i = 0; i < b.Sections; i++) {
                        if (b.Busy[i].ID > 0) {
                            f.addRabbit(b.Busy[i].ID);
                        }
                    }
                }
            }
            if (f.ShowDialog() == DialogResult.OK) {
                _rsb.Run();
            }
        }

        private void replaceMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1) {
                return;
            }

            Building b = null;
            ReplaceForm rf = new ReplaceForm();
            foreach (ListViewItem li in listView1.SelectedItems) {
                Building b2 = li.Tag as Building;
                if (b2 != b) {
                    b = b2;
                    for (int i = 0; i < b.Sections; i++) {
                        if (b.Busy[i].ID > 0) {
                            rf.AddRabbit(b.Busy[i].ID);
                        }
                    }
                }
            }
            if (rf.ShowDialog() == DialogResult.OK) {
                _rsb.Run();
            }
        }

        private bool isFarm(TreeNode tn)
        {
            return farmNum(tn) != 0;
        }
        private bool isFarm()
        {
            return isFarm(treeView1.SelectedNode);
        }
        private int farmNum(TreeNode tn)
        {
            return (tn.Tag as BldTreeData).FarmId;
        }
        private int farmNum()
        {
            return farmNum(treeView1.SelectedNode);
        }
        private int buildNum(TreeNode tn)
        {
            return (tn.Tag as BldTreeData).ID;
        }
        private int buildNum()
        {
            return buildNum(treeView1.SelectedNode);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right) {
                treeView1.SelectedNode = e.Node;
            }
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            treeView1.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_DragOver_1(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode))) {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeNode))) {
                return;
            }

            TreeNode child = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            Point px = treeView1.PointToClient(new Point(e.X, e.Y));
            TreeNode newpar = treeView1.GetNodeAt(px);

            if (newpar == null || child == null) {
                return;
            }
            if (child == treeView1.Nodes[0]) {
                return;
            }
            if (isFarm(newpar)) {
                return;
            }
            if (child == newpar) {
                return;
            }

            if (MessageBox.Show(this, "Переместить " + child.Text + " в " + newpar.Text + "?", "Перемещение", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                int bid = buildNum(child);
                int to = buildNum(newpar);
                Engine.db().replaceBuilding(bid, to);
                _rsb.Run();
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (action != 0) {
                if (e.Label == null || e.Label == "" || e.Label.Contains("/")) {
                    e.CancelEdit = true;
                    treeView1.SelectedNode.Remove();
                } else {
                    Engine.db().addBuilding(buildNum(nodeToAdd), e.Label);
                }
                nodeToAdd = null;
                action = 0;
            }
            if (e.Label == null || e.Label == "") {
                e.CancelEdit = true;
                return;
            }
            if (!manual) {
                return;
            }
            if (e.Node == treeView1.Nodes[0]) {
                e.CancelEdit = true;
                return;
            }
            if (e.Node.Text != e.Label) {
                Engine.db().setBuildingName(buildNum(e.Node), e.Label);
                e.CancelEdit = false;
            }
            timer1.Start();
        }

        private bool askDelete()
        {
            return (MessageBox.Show(this, "Удалить " + treeView1.SelectedNode.Text + "?", "Удаление", MessageBoxButtons.YesNo) == DialogResult.Yes);
        }

        private void deleteBuildingMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) {
                return;
            }

            TreeNode sNode = treeView1.SelectedNode;
            if (this.isFarm()) {
                int[] tiers = Engine.db().getTiers(farmNum());
                bool candelete = true;
                for (int i = 0; i < 2; i++)
                    if (tiers[i] != 0) {
                        Building b = Engine.db().getBuilding(tiers[i]);
                        for (int j = 0; j < b.Sections; j++) {
                            if (b.Busy[j].ID > 0) {
                                candelete = false;
                            }
                        }
                    }
                if (candelete) {
                    if (askDelete()) {
                        preBuilding = buildNum(sNode.Parent);
                        Engine.db().deleteFarm(farmNum());
                        _rsb.Run();
                    }
                } else {
                    MessageBox.Show(@"Ферма не пуста.");
                }
            } else {
                if (sNode.Nodes.Count > 0) {
                    MessageBox.Show("Имеются вложенные строения");
                } else if (askDelete()) {
                    preBuilding = buildNum(sNode.Parent);
                    Engine.db().deleteBuilding(buildNum());
                    _rsb.Run();
                }
            }
        }

        private void addBuildingMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) {
                return;
            }

            if (isFarm()) {
                return;
            }
            nodeToAdd = treeView1.SelectedNode;
            TreeNode nd = nodeToAdd.Nodes.Add(NEW_BUILDING);
            manual = false;
            action = 1;
            nd.Name = "new";
            nd.Tag = new BldTreeData(0, 0, "");
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
            if (treeView1.SelectedNode == null) {
                return;
            }
            if (isFarm()) {
                return;
            }
            if (_freeFarmsId.Count == 0
#if DEMO
                || Engine.db().getMFCount() >= DEMO_MAX_FARMS
#elif PROTECTED
                || Engine.db().getMFCount() >= GRD.Instance.GetFarmsCnt()
#endif
) {
                MessageBox.Show("Достигнуто максимальное количество ферм.");
                return;
            }
            if (new MiniFarmForm(buildNum(), _freeFarmsId.ToArray()).ShowDialog() == DialogResult.OK) {
                _rsb.Run();
            }
        }

        private void changeFarmMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) {
                return;
            }
            if (!isFarm()) {
                return;
            }
            int fid = farmNum();
            MainForm.ProtectTest(Engine.db().getMFCount());
            if (new MiniFarmForm(fid).ShowDialog() == DialogResult.OK) {
                _rsb.Run();
            }
        }

        private XmlDocument getBuildDoc(int bid)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement rw = (XmlElement)doc.AppendChild(doc.CreateElement("Rows")).AppendChild(doc.CreateElement("Row"));
            rw.AppendChild(doc.CreateElement("date")).AppendChild(doc.CreateTextNode(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString()));
            String ad = bid == 0 ? "farm" : this.getAddress(-1, bid);
            rw.AppendChild(doc.CreateElement("address")).AppendChild(doc.CreateTextNode(ad));
            return doc;
        }

        private void shedReportMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) {
                return;
            }
            if (isFarm()) {
                return;
            }

            Filters f = new Filters();
            int bid = buildNum();
            f["bld"] = bid.ToString();
            f["nest_out"] = Engine.opt().getOption(Options.OPT_ID.NEST_OUT);
            //f["suck"] = Engine.opt().getOption(Options.OPT_ID.COUNT_SUCKERS);
            ReportViewForm dlg = new ReportViewForm(
                myReportType.SHED,
                new XmlDocument[] 
                { 
                    Engine.db().makeReport(myReportType.SHED,f),getBuildDoc(bid)
                });
            dlg.ExcelEnabled = false;
            dlg.Show();
        }

        private void emptyRevMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) {
                return;
            }

            if (this.isFarm()) {
                return;
            }

            Filters f = new Filters();
            int bid = buildNum();
            f["bld"] = bid.ToString();
            new ReportViewForm(myReportType.REVISION, new XmlDocument[]{
                Engine.db().makeReport(myReportType.REVISION,f),getBuildDoc(bid)}).ShowDialog();
        }

        private void makeExcel()
        {
            ExcelMaker.MakeExcelFromLV(listView1, "Строения");
        }

        private void miNotesEdit_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) {
                return;
            }

            Building b = listView1.SelectedItems[0].Tag as Building;
            BuildingNotesEdit dlg = new BuildingNotesEdit(listView1.SelectedItems[0].SubItems[0].Text, listView1.SelectedItems[0].SubItems[FIELD_NOTES].Text);
            if (dlg.ShowDialog() == DialogResult.OK) {
                listView1.SelectedItems[0].SubItems[FIELD_NOTES].Text = dlg.Notes;
                for (int i = 0; i < b.Sections; i++) {
                    if (listView1.SelectedItems[0].SubItems[0].Text == Building.Format(b.Farm) + b.Areas(i)) {
                        b.Notes[i] = dlg.Notes;
                        break;
                    }
                }
                Engine.db().updateBuilding(b);
            }
        }
    }


    public class TVNodeSorter : IComparer
    {
        //public string strpart(string str)
        //{
        //    int i = str.Length - 1;
        //    while (Char.IsDigit(str[i])) {
        //        i--;
        //        if (i == -1) {
        //            return str;
        //        }
        //    }
        //    i++;
        //    return str.Substring(0, i);
        //}

        public int Compare(object x, object y)
        {
            string s1 = (x as TreeNode).Text;
            string s2 = (y as TreeNode).Text;
            string ss1 = Regex.Match(s1, @"\d+").Value;
            string ss2 = Regex.Match(s2, @"\d+").Value;
            return ss1.CompareTo(ss2);
            //if (ss1 != ss2) {
            //    if (ss2[0] == '№') {
            //        return -1;
            //    }
            //    if (ss1[0] == '№') {
            //        return 1;
            //    }
            //    return String.Compare(s1, s2);
            //}
            //int i1 = int.Parse(s1.Substring(ss1.Length));
            //int i2 = int.Parse(s2.Substring(ss2.Length));
            //return i1.CompareTo(i2);
        }

    }

}
