using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using rabnet.forms;
using rabnet.filters;
using rabnet.components;
using System.Diagnostics;

namespace rabnet.panels
{

    public partial class WorksPanel : RabNetPanel
    {
        private DateTime reportDate = DateTime.Now;
        public WorksPanel() : base() { }
        public int _makeFlag = 0;
        private bool _fullUpdate = true;
        private Filters runF = null;//todo есть же parent._runF
        private int _selectedItem = -1;

        public WorksPanel(RabStatusBar sb)
            : base(sb, new ZootehFilter())
        {
            _colSort = new WorkPanelColumnSorter(lvZooTech, new int[] { 0, 4 }, Options.OPT_ID.ZOO_LIST);
            _colSort2 = new ListViewColumnSorter(lvZooTech, new int[] { 0, 4 }, Options.OPT_ID.ZOO_LIST);
            lvZooTech.ListViewItemSorter = _colSort;
            MakeExcel = new RSBEventHandler(this.makeExcel);
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            if (_fullUpdate) {
                f[Filters.SHORT] = Engine.opt().safeIntOption(Options.OPT_ID.SHORT_ZOO, 1).ToString();
                f[Filters.DBL_SURNAME] = f[Filters.SHORT] == "1" ? "0" : "1";//Engine.opt().getOption(Options.OPT_ID.DBL_SURNAME);
                f[Filters.FIND_PARTNERS] = Engine.opt().getOption(Options.OPT_ID.FIND_PARTNERS);
                f[Filters.OKROL] = Engine.opt().getOption(Options.OPT_ID.OKROL);
                f[Filters.PRE_OKROL] = Engine.opt().getOption(Options.OPT_ID.PRE_OKROL);
                f[Filters.VUDVOR] = Engine.opt().getOption(Options.OPT_ID.NEST_OUT);
                f[Filters.NEST_OUT_IF_SUKROL] = Engine.opt().getBoolOption(Options.OPT_ID.NEST_OUT_IF_SUKROL) ? "1" : "0";
                f[Filters.COUNT1] = Engine.opt().getOption(Options.OPT_ID.COUNT1);
                f[Filters.COUNT2] = Engine.opt().getOption(Options.OPT_ID.COUNT2);
                f[Filters.COUNT3] = Engine.opt().getOption(Options.OPT_ID.COUNT3);
                f[Filters.BOYS_OUT] = Engine.opt().getOption(Options.OPT_ID.BOYS_OUT);
                f[Filters.GIRLS_OUT] = Engine.opt().getOption(Options.OPT_ID.GIRLS_OUT);
                f[Filters.NEST_IN] = Engine.opt().getOption(Options.OPT_ID.NEST_IN);
                f[Filters.CHILD_NEST] = Engine.opt().getOption(Options.OPT_ID.CHILD_NEST);
                f[Filters.STATE_FUCK] = Engine.opt().getOption(Options.OPT_ID.STATE_FUCK);
                f[Filters.FIRST_FUCK] = Engine.opt().getOption(Options.OPT_ID.FIRST_FUCK);
                f[Filters.HETEROSIS] = Engine.opt().getOption(Options.OPT_ID.GETEROSIS);
                f[Filters.INBREEDING] = Engine.opt().getOption(Options.OPT_ID.INBREEDING);
                f[Filters.MALE_REST] = Engine.opt().getOption(Options.OPT_ID.MALE_WAIT);
                f[Filters.BOYS_BY_ONE] = Engine.opt().getOption(Options.OPT_ID.BOYS_BY_ONE);
                f[Filters.VACC_MOTH] = Engine.opt().getBoolOption(Options.OPT_ID.VACC_MOTHER) ? "1" : "0";

                f[Filters.VACC_SHOW] = "";
                foreach (Vaccine v in Engine.db().GetVaccines(true)) {
                    if (v.Zoo) {//todo опасно vaccines
                        f[Filters.VACC_SHOW] += String.Format("{0:d},", v.ID);
                    }
                }
                f[Filters.VACC_SHOW] = f[Filters.VACC_SHOW].TrimEnd(',');

                _selectedItem = -1;
                base.onPrepare(f);
                lvZooTech.Items.Clear();
                reportDate = DateTime.Parse(f[Filters.DATE]);

                // указываем что зоотехплан не на сегодня
                if (reportDate.Date != DateTime.Now.Date) {
                    lvZooTech.BackColor = Color.Linen;
                    _rsb.SetText(2, reportDate.ToString("dddd, dd MMMM yyyy"), true);
                } else {
                    lvZooTech.BackColor = SystemColors.Window;
                    _rsb.SetText(2, "");
                }
            }
            if (lvZooTech.SelectedItems.Count == 1) {
                _selectedItem = lvZooTech.SelectedItems[0].Index;
            }
            runF = f;
            fillLogs(f);
            //DataThread.Get().Stop();
            if (!_fullUpdate) {
                _fullUpdate = true;
                return null;
            }
            return Engine.db2().zooTeh(f);//возвращает ZooTehNullGetter
        }

        protected override void onItem(IData data)
        {
            ZooTehNullItem it = data as ZooTehNullItem;

#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif

            Filters f = runF;
            foreach (ZootehJob j in Engine.get().zoo().makeZooTehPlan(f, it.id)) {
                ListViewItem li = lvZooTech.Items.Add(j.Days.ToString());
                li.SubItems.Add(j.JobName);
                li.SubItems.Add(j.Address);
                li.SubItems.Add(j.RabName);
                li.SubItems.Add(j.RabAge.ToString());
                li.SubItems.Add(j.RabBreed);
                li.SubItems.Add(j.Comment);
                li.SubItems.Add(j.Partners);/// todo партнеров получать gh
                li.Tag = j;
            }
#if DEBUG
            sw.Stop();
            _logger.DebugFormat("processing time: {0} ZOO_{1}", sw.Elapsed, it.id);
#endif
        }

        protected override void onFinishUpdate()
        {
            base.onFinishUpdate();
            if (_selectedItem > -1 && lvZooTech.Items.Count > _selectedItem) {
                lvZooTech.Items[_selectedItem].Selected = true;
                lvZooTech.Items[_selectedItem].EnsureVisible();
            }
            lvZooTech.Focus();
            _rsb.SetText(1, String.Format("{0:d} записей", lvZooTech.Items.Count));
            (filterPanel as ZootehFilter).RefreshDate();
        }

        /// <summary>
        /// Заполнение listView c логами
        /// </summary>
        /// <param name="f">Коллекция фильтров</param>
        private void fillLogs(Filters f)
        {
            lvLogs.BeginUpdate();
            lvLogs.Items.Clear();
            foreach (LogList.OneLog l in Engine.db().getLogs(f).logs) {
                ListViewItem li = lvLogs.Items.Add(l.date.ToShortDateString() + " " + l.date.ToShortTimeString());
                li.SubItems.Add(l.work);
                li.SubItems.Add(l.address);
                li.SubItems.Add(l.prms);
                li.SubItems.Add(l.user);
            }
            lvLogs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvLogs.EndUpdate();
        }

        /// <summary>
        /// Открывает один из пунктов(работ) в контекстном меню,
        /// когда жмут правой кнопкой на одну из строк в listView с зоотехПланом
        /// </summary>
        /// <param name="type">Тип работы</param>
        /// <param name="job"></param>
        protected void setMenu(JobType type, ZootehJob job)
        {
            foreach (ToolStripMenuItem i in actMenu.Items) {
                if (i == miPrint) {
                    continue;
                }

                i.Visible = i.Name == "mi" + type.ToString();
            }

            switch (type) {
                //case JobType.Okrol: miOkrol.Visible = true; break;
                //case JobType.NestOut: miNestOut.Visible = true; break;
                case JobType.CountKids: miCountKidsChanged.Visible = true; break;
                //case JobType.PreOkrol: miPreOkrol.Visible = true; break;
                //case JobType.BoysOut: miBoysOut.Visible = true; break;
                //case JobType.GirlsOut: miGirlsOut.Visible = true; break;
                case JobType.Fuck:
                    if (job != null) {
                        miFuck.Text = (job.JobName == "Случка" ? "Случить" : "Вязать");
                        miLust.Visible = job.Flag2 == 0;
                    }
                    break;
                //case JobType.Vaccine: miVaccine.Visible = true; break;
                //case JobType.NestSet: miNestSet.Visible = true; break;
                //case JobType.BoysByOne: miBoysByOne.Visible = true; break;
                //case JobType.SpermTake: miSpermTake.Visible = true; break;
            }
        }

        /// <summary>
        /// Возвращает тип работы выбранной строки 
        /// (тип хранится в свойстве Tag)
        /// </summary>
        /// <returns></returns>
        private ZootehJob getCurJob()
        {
            if (lvZooTech.SelectedItems.Count != 1) {
                return null;
            }
            return (lvZooTech.SelectedItems[0].Tag) as ZootehJob;
        }

        private int getFuckerId(String f, List<String> lst)
        {
            for (int i = 0; i < lst.Count; i++) {
                if (lst[i] == f) {
                    return i;
                }
            }
            lst.Add(f);
            return lst.Count - 1;
        }

        /// <summary>
        /// Выполняет одну из зоотехнических работ
        /// </summary>
        private void makeOneJob(ZootehJob job)
        {
            if (job == null) {
                return;
            }

            _fullUpdate = Engine.opt().getIntOption(Options.OPT_ID.UPDATE_ZOO) == 1;

            if (this.performJob(job, ref _fullUpdate) == DialogResult.OK) {
                int idx = lvZooTech.SelectedItems[0].Index;
                lvZooTech.SelectedItems[0].Remove();
                if (idx < lvZooTech.Items.Count) {
                    lvZooTech.Items[idx].Selected = true;
                }
                _rsb.Run();
            }
        }

        private DialogResult performJob(ZootehJob job, ref bool needUpdate)
        {
            DialogResult res = DialogResult.OK;
            switch (job.Type) {
                case JobType.NestOut:
                    RabNetEngBuilding b = Engine.get().getBuilding(job.ID);
                    b.setNest(false, (job.ID2 == 0 ? 0 : 1));
                    needUpdate = false;
                    break;

                case JobType.PreOkrol:
                    Engine.get().preOkrol(job.ID);
                    //_fullUpdate = false;
                    break;

                case JobType.BoysOut:
                case JobType.GirlsOut:
                    ReplaceForm rf = new ReplaceForm();
                    rf.AddRabbit(job.ID);
                    if (job.Type == JobType.BoysOut) {
                        rf.SetAction(ReplaceForm.Action.BOYSOUT);
                    }
                    res = rf.ShowDialog();
                    break;

                case JobType.CountKids:
                    RabNetEngRabbit rrr = Engine.get().getRabbit(job.ID);
                    CountKids ck = new CountKids(job.ID);
                    int id2 = 0;
                    for (int i = 0; i < rrr.Youngers.Count; i++) {
                        if (rrr.Youngers[i].ID == job.ID2) {
                            id2 = i;
                        }
                    }
                    if (_makeFlag == 0) {
                        //rrr.CountKids(0, 0, 0, rrr.Youngers[id2].Group, rrr.Youngers[id2].Age, 0);
                        ck.MakeCount();
                        ck.Dispose();
                        needUpdate = false;
                    } else {
                        //ck = new CountKids(job.ID, job.Flag == 1);
                        //ck.SetGroup(id2);
                        res = ck.ShowDialog();
                    }
                    break;

                case JobType.Fuck:
                    int id = job.ID;
                    if (_makeFlag == -1) {
                        needUpdate = false;
                        Engine.db().SetRabbitVaccine(id, Vaccine.V_ID_LUST);
                        res = DialogResult.OK;
                        break;
                    }

                    if (job.Flag > 1) {
                        id = 0;
                        ReplaceBrideForm rb = new ReplaceBrideForm(job.ID);
                        res = rb.ShowDialog();
                        if (res == DialogResult.OK) {
                            id = rb.getGirlOut();
                        }
                        res = DialogResult.Cancel;
                    }
                    if (id != 0) {
                        res = (new FuckForm(id)).ShowDialog();
                    }
                    break;

                case JobType.Okrol:
                    res = (new OkrolForm(job.ID)).ShowDialog();
                    break;

                case JobType.Vaccine://прививка
                    RabNetEngRabbit rab = Engine.get().getRabbit(job.ID);
                    AddRabVacForm dlg = new AddRabVacForm(rab, false, job.ID2);
                    res = dlg.ShowDialog();
                    if (res == DialogResult.OK) {
                        rab.SetVaccine(dlg.VacID, dlg.VacDate, false);
                        if (rab.ParentID != 0 && Engine.opt().getBoolOption(Options.OPT_ID.VACC_MOTHER)) {
                            RabNetEngRabbit r2 = Engine.get().getRabbit(rab.ParentID);
                            r2.SetVaccine(dlg.VacID, dlg.VacDate, false);
                        }
                    }
                    needUpdate = false;
                    break;

                case JobType.NestSet://установка гнездовья    
                    RabPlace rp = RabPlace.Parse(job.Rabplace);
                    if (rp.CanHaveNest()) {
                        RabNetEngBuilding rbe = RabNetEngBuilding.FromPlace(job.Rabplace, Engine.get());
                        rbe.setNest(true, rp.Section);
                        res = DialogResult.OK;
                        needUpdate = false;
                        break;
                    }
                    ReplaceForm f = new ReplaceForm();
                    f.AddRabbit(job.ID);
                    f.SetAction(ReplaceForm.Action.SET_NEST);
                    res = f.ShowDialog();
                    break;

                case JobType.BoysByOne:
                    f = new ReplaceForm();
                    f.AddRabbit(job.ID);
                    res = f.ShowDialog();
                    break;

                case JobType.SpermTake:
#if !DEMO
                    RabNetEngRabbit r = Engine.get().getRabbit(job.ID);
                    r.SpermTake();
                    needUpdate = false;
#else
                    DemoErr.DemoNoModuleMsg();
#endif
                    break;
            }

            return res;
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {            
            foreach (ListViewItem lvi in lvZooTech.SelectedItems) {
                if (performJob(lvi.Tag as ZootehJob, ref _fullUpdate) == DialogResult.OK) {
                    lvi.Remove();
                } else {
                    break;
                }
            }
            
            _rsb.Run();
        }

        private void countChangedMenuItem_Click(object sender, EventArgs e)
        {
            _makeFlag = 1;
            makeOneJob(this.getCurJob());
        }

        private void miLust_Click(object sender, EventArgs e)
        {
            _makeFlag = -1;
            makeOneJob(this.getCurJob());
        }

        private void printMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            List<String> fuckers = new List<string>();
            XmlDocument rep = new XmlDocument();
            rep.AppendChild(rep.CreateElement("Rows")).AppendChild(rep.CreateElement("Row")).AppendChild(rep.CreateElement("date")).AppendChild(rep.CreateTextNode(reportDate.ToLongDateString() + " " + reportDate.ToLongTimeString()));
            XmlDocument doc = new XmlDocument();
            XmlDocument fucks = new XmlDocument();
            XmlElement root = doc.CreateElement("Rows");
            doc.AppendChild(root);
            XmlElement fuck = fucks.CreateElement("Rows");
            fucks.AppendChild(fuck);
            XmlElement rw;
            for (int i = 0; i < lvZooTech.Items.Count; i++) {
                ListViewItem li = lvZooTech.Items[i];
                ZootehJob j = (ZootehJob)li.Tag;
                rw = doc.CreateElement("Row");
                rw.AppendChild(doc.CreateElement("type")).AppendChild(doc.CreateTextNode(((int)j.Type).ToString()));
                rw.AppendChild(doc.CreateElement("days")).AppendChild(doc.CreateTextNode(j.Days.ToString()));
                //ReportHelper.Append(
                rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(j.JobName));
                rw.AppendChild(doc.CreateElement("rabbit")).AppendChild(doc.CreateTextNode(j.RabName));
                rw.AppendChild(doc.CreateElement("address")).AppendChild(doc.CreateTextNode(j.Address));
                rw.AppendChild(doc.CreateElement("breed")).AppendChild(doc.CreateTextNode(j.RabBreed));
                rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(j.RabAge.ToString()));
                if (j.Type == JobType.Fuck) {
                    int id = getFuckerId(j.Partners, fuckers);
                    string cmt = String.Format("см. {0:d}{1:d}", (id + 1), j.Flag > 1 ? Environment.NewLine + "N" + j.Flag.ToString() : "");
                    rw.AppendChild(doc.CreateElement("comment")).AppendChild(doc.CreateTextNode(cmt));
                } else {
                    rw.AppendChild(doc.CreateElement("comment")).AppendChild(doc.CreateTextNode(j.Comment));
                }
                root.AppendChild(rw);
            }
            for (int i = 0; i < fuckers.Count; i++) {
                rw = fucks.CreateElement("Row");
                rw.AppendChild(fucks.CreateElement("id")).AppendChild(fucks.CreateTextNode((i + 1).ToString()));
                rw.AppendChild(fucks.CreateElement("names")).AppendChild(fucks.CreateTextNode(fuckers[i]));
                fuck.AppendChild(rw);
            }
            XmlDocument[] xmls = new XmlDocument[] { doc, rep, fucks };
            String plan = "zooteh";
            if (fuckers.Count == 0) {
                plan += "_nofuck";
                xmls = new XmlDocument[] { doc, rep };
            }
            new ReportViewForm("Зоотехплан " + reportDate.ToLongDateString() + " " + reportDate.ToLongTimeString(), plan, xmls).ShowDialog();
#else 
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void makeExcel()
        {
#if !DEMO
            ExcelMaker.MakeExcelFromLV(lvZooTech, "Зоотехплан");
#endif
        }

        private void WorksPanel_Load(object sender, EventArgs e)
        {
            MainForm.ProtectTest(Engine.db().getMFCount());
        }

        private void actMenu_Opening(object sender, CancelEventArgs e)
        {
            ZootehJob zJob = null;
            if (lvZooTech.SelectedItems.Count == 0) {
                e.Cancel = true;
                return;
            } else if (lvZooTech.SelectedItems.Count == 1) {
                zJob = lvZooTech.SelectedItems[0].Tag as ZootehJob;
                this.setMenu(zJob.Type, zJob);
            } else {                
                JobType jtAssumed = JobType.None;
                foreach (ListViewItem lvi in lvZooTech.Items) {
                    zJob = lvi.Tag as ZootehJob;
                    // проверяем чтобы все выбранные записи имели один тип 
                    if (jtAssumed != JobType.None && jtAssumed != zJob.Type) {
                        jtAssumed = JobType.None;
                        break;
                    }

                    if (jtAssumed == JobType.None) {
                        jtAssumed = zJob.Type;
                    }
                }
                this.setMenu(jtAssumed, null);
            }

        }

    }

    class WorkPanelColumnSorter : ListViewColumnSorter
    {
        /// <summary>
        /// Колонка содержит тип работ
        /// </summary>
        const int COL_TYPE = 1;

        public WorkPanelColumnSorter(ListView lv, int[] intsorts, Options.OPT_ID op)
            : base(lv, intsorts, op)
        {
        }

        public override int Compare(object x, object y)
        {

            int listViewVal_X = (int)((x as ListViewItem).Tag as ZootehJob).Type,
                listViewVal_Y = (int)((y as ListViewItem).Tag as ZootehJob).Type;

            int result = listViewVal_X.CompareTo(listViewVal_Y);

            if (result != 0 || this.sortColumn == COL_TYPE) {
                return result;
            }

            return base.Compare(x, y);
        }
    }
}
