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

namespace rabnet.panels
{
    public partial class WorksPanel : RabNetPanel
    {
        private DateTime repdate = DateTime.Now;
        public WorksPanel():base(){}
        public int _makeFlag = 0;
        private bool fullUpdate=true;
        private Filters runF = null;
        private int itm = -1;

        public WorksPanel(RabStatusBar sb): base(sb, new ZootehFilter())
        {           
            _colSort = new ListViewColumnSorter(lvZooTech, new int[] {0,4},Options.OPT_ID.ZOO_LIST);
            _colSort2 = new ListViewColumnSorter(lvZooTech, new int[] { 0, 4 }, Options.OPT_ID.ZOO_LIST);
            lvZooTech.ListViewItemSorter = _colSort;
            MakeExcel = new RSBEventHandler(this.makeExcel);
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            if (fullUpdate)
            {
                f[Filters.SHORT] = Engine.opt().safeIntOption(Options.OPT_ID.SHORT_ZOO, 1).ToString();
                f[Filters.DBL_SURNAME] = f[Filters.SHORT] == "1" ? "0" : "1";//Engine.opt().getOption(Options.OPT_ID.DBL_SURNAME);
                f[Filters.FIND_PARTNERS] = Engine.opt().getOption(Options.OPT_ID.FIND_PARTNERS);
                f[Filters.OKROL] = Engine.opt().getOption(Options.OPT_ID.OKROL);
                f[Filters.PRE_OKROL] = Engine.opt().getOption(Options.OPT_ID.PRE_OKROL);
                f[Filters.VUDVOR] = Engine.opt().getOption(Options.OPT_ID.NEST_OUT);
                f[Filters.NEST_OUT_IF_SUKROL] = Engine.opt().getBoolOption(Options.OPT_ID.NEST_OUT_IF_SUKROL)?"1":"0";
                f[Filters.COUNT1] = Engine.opt().getOption(Options.OPT_ID.COUNT1);
                f[Filters.COUNT2] = Engine.opt().getOption(Options.OPT_ID.COUNT2);
                f[Filters.COUNT3] = Engine.opt().getOption(Options.OPT_ID.COUNT3);
                //f["count3"] = Engine.opt().getOption(Options.OPT_ID.COUNT_SUCKERS);
                f[Filters.BOYS_OUT] = Engine.opt().getOption(Options.OPT_ID.BOYS_OUT);
                f[Filters.GIRLS_OUT] = Engine.opt().getOption(Options.OPT_ID.GIRLS_OUT);
                //f[Filters.VACC_DAYS] = Engine.opt().getOption(Options.OPT_ID.VACC);
                f[Filters.NEST_IN] = Engine.opt().getOption(Options.OPT_ID.NEST_IN);
                f[Filters.CHILD_NEST] = Engine.opt().getOption(Options.OPT_ID.CHILD_NEST);
                f[Filters.STATE_FUCK] = Engine.opt().getOption(Options.OPT_ID.STATE_FUCK);
                f[Filters.FIRST_FUCK] = Engine.opt().getOption(Options.OPT_ID.FIRST_FUCK);
                f[Filters.HETEROSIS] = Engine.opt().getOption(Options.OPT_ID.GETEROSIS);
                f[Filters.INBREEDING] = Engine.opt().getOption(Options.OPT_ID.INBREEDING);
                f[Filters.MALE_WAIT] = Engine.opt().getOption(Options.OPT_ID.MALE_WAIT);
                //f["vactime"] = Engine.opt().getOption(Options.OPT_ID.VACCINE_TIME);
                f[Filters.BOYS_BY_ONE] = Engine.opt().getOption(Options.OPT_ID.BOYS_BY_ONE);
                f[Filters.VACC_MOTH] = Engine.opt().getBoolOption(Options.OPT_ID.VACC_MOTHER)?"1":"0";
                
                f[Filters.VACC_SHOW] = "";
                foreach (Vaccine v in Engine.db().GetVaccines())
                {
                    if (v.Zoo) //todo опасно vaccines
                        f[Filters.VACC_SHOW] += String.Format("{0:d},", v.ID);
                }
                f[Filters.VACC_SHOW] = f[Filters.VACC_SHOW].TrimEnd(',');

                itm = -1;
                if (lvZooTech.SelectedItems.Count == 1)
                    itm = lvZooTech.SelectedItems[0].Index;
                base.onPrepare(f);
                lvZooTech.Items.Clear();
                repdate = DateTime.Now;
            }
            runF = f;
            fillLogs(f);
            //DataThread.Get().Stop();
            if (!fullUpdate)
            {
                fullUpdate = true;
                return null;
            }
            return Engine.db2().zooTeh(f);//возвращает ZooTehNullGetter
        }

        protected override void onItem(IData data)
        {
            ZooTehNullItem it = data as ZooTehNullItem;            

            Filters f = runF;
            foreach (ZootehJob j in Engine.get().zoo().makeZooTehPlan(f, it.id))
            {
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
        }

        protected override void onFinishUpdate()
        {
            base.onFinishUpdate();
            if (itm > -1 && lvZooTech.Items.Count > itm)
            {
                lvZooTech.Items[itm].Selected = true;
                lvZooTech.Items[itm].EnsureVisible();
            }
            lvZooTech.Focus();
        }

        /// <summary>
        /// Заполнение listView c логами
        /// </summary>
        /// <param name="f">Коллекция фильтров</param>
        private void fillLogs(Filters f)
        {
            lvLogs.Items.Clear();
            foreach (LogList.OneLog l in Engine.db().getLogs(f).logs)
            {
                ListViewItem li = lvLogs.Items.Add(l.date.ToShortDateString() + " " + l.date.ToShortTimeString());
                li.SubItems.Add(l.work);
                li.SubItems.Add(l.address);
                li.SubItems.Add(l.prms);
                li.SubItems.Add(l.user);
            }
            lvLogs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        public override ContextMenuStrip getMenu()
        {
            setMenu(JobType.NONE);
            return actMenu;
        }

        public void setMenu(JobType type)
        {
            setMenu(type, null);
        }
        /// <summary>
        /// Открывает один из пунктов(работ) в контекстном меню,
        /// когда жмут правой кнопкой на одну из строк в listView с зоотехПланом
        /// </summary>
        /// <param name="type">Тип работы</param>
        /// <param name="job"></param>
        public void setMenu(JobType type,ZootehJob job)
        {
            okrolMenuItem.Visible = vudvorMenuItem.Visible= miBoysByOne.Visible=
                countsMenuItem.Visible = preokrolMenuItem.Visible=
                boysOutMenuItem.Visible = girlsOutMenuItem.Visible=
                vaccMenuItem.Visible = fuckMenuItem.Visible =miLust.Visible=
                setNestMenuItem.Visible = countChangedMenuItem.Visible=false;
            switch (type)
            {
                case JobType.OKROL: okrolMenuItem.Visible = true; break;
                case JobType.VUDVOR: vudvorMenuItem.Visible = true; break;
                case JobType.COUNT_KIDS: countChangedMenuItem.Visible=countsMenuItem.Visible = true; break;
                case JobType.PRE_OKROL: preokrolMenuItem.Visible = true; break;
                case JobType.BOYS_OUT: boysOutMenuItem.Visible = true; break;
                case JobType.GIRLS_OUT: girlsOutMenuItem.Visible = true; break;
                case JobType.FUCK: fuckMenuItem.Visible = true;
                    if (job.JobName == "Случка")
                        fuckMenuItem.Text = "Случить";
                    else
                        fuckMenuItem.Text = "Вязать";
                    miLust.Visible = true;
                    break;
                case JobType.VACC: vaccMenuItem.Visible = true; break;
                case JobType.SET_NEST: vaccMenuItem.Visible = true; break;
                case JobType.BOYS_BY_ONE: miBoysByOne.Visible = true; break;
            }
        }

        /// <summary>
        /// Возвращает тип работы выбранной строки 
        /// (тип хранится в свойстве Tag)
        /// </summary>
        /// <returns></returns>
        private ZootehJob getCurJob()
        {
            if (lvZooTech.SelectedItems.Count != 1)
                return null;
            return  (lvZooTech.SelectedItems[0].Tag) as ZootehJob;
        }

        private int getFuckerId(String f, List<String> lst)
        {
            for (int i = 0; i < lst.Count; i++)
                if (lst[i] == f) return i;
            lst.Add(f);
            return lst.Count - 1;
        }

        private void lvZooTech_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainForm.StillWorking();
            if (lvZooTech.SelectedItems.Count != 1)
            {
                setMenu(JobType.NONE);
                return;
            }
            setMenu(getCurJob().Type,getCurJob());
        }

        /// <summary>
        /// Выполняет одну из зоотехнических работ
        /// </summary>
        private void makeJob()
        {
            DialogResult res = DialogResult.OK;
            ZootehJob job = getCurJob();
            if (job == null)
                return;
            fullUpdate = true;
            bool needUpdate = Engine.opt().getIntOption(Options.OPT_ID.UPDATE_ZOO) == 1;
            switch (job.Type)
            {                
                case JobType.VUDVOR:
                    RabNetEngBuilding b = Engine.get().getBuilding(job.ID);
                    if (job.ID2 == 0)
                        b.setNest(false);
                    else
                        b.setNest2(false);
                    needUpdate = false;
                    break;

                case JobType.PRE_OKROL:
                    Engine.get().preOkrol(job.ID);
                    needUpdate = false;
                    break;

                case JobType.BOYS_OUT:
                case JobType.GIRLS_OUT:
                    ReplaceForm rf = new ReplaceForm();
                    rf.AddRabbit(job.ID);
                    if (job.Type==JobType.BOYS_OUT)
                        rf.SetAction(ReplaceForm.Action.BOYSOUT);
                    res=rf.ShowDialog();
                    break; 
                                  
                case JobType.COUNT_KIDS:
                    RabNetEngRabbit rrr = Engine.get().getRabbit(job.ID);
                    CountKids ck;
                    int id2 = 0;
                    for (int i = 0; i < rrr.Youngers.Length; i++)
                        if (rrr.Youngers[i].ID == job.ID2) id2 = i;
                    if (_makeFlag == 0)
                    {
                        rrr.CountKids(0, 0, 0, rrr.Youngers[id2].Group, rrr.Youngers[id2].Age, 0);
                        needUpdate = false;
                    }
                    else
                    {
                        ck = new CountKids(job.ID, job.Flag == 1);
                        ck.setGrp(id2);
                        res = ck.ShowDialog();
                    }
                    break;

                case JobType.FUCK:
                    int id = job.ID;
                    if (_makeFlag == -1)
                    {
                        needUpdate = false;
                        Engine.db().SetRabbitVaccine(id, Vaccine.V_ID_LUST);
                        res = DialogResult.OK;
                        break;
                    }

                    if (job.Flag > 1)
                    {
                        id = 0;
                        ReplaceBrideForm rb=new ReplaceBrideForm(job.ID);
                        res = rb.ShowDialog();
                        if (res == DialogResult.OK)
                            id = rb.getGirlOut();
                        res = DialogResult.Cancel;
                    }
                    if (id != 0)
                        res = (new MakeFuckForm(id)).ShowDialog();
                    break;    
                                
                case JobType.OKROL:
                    res=(new OkrolForm(job.ID)).ShowDialog();
                    break;

                case JobType.VACC://прививка
                    RabNetEngRabbit rab = Engine.get().getRabbit(job.ID);
                    AddRabVacForm dlg = new AddRabVacForm(rab,false,job.ID2);
                    res=dlg.ShowDialog();
                    if ( res== DialogResult.OK)
                    {
                        rab.SetVaccine(dlg.VacID, dlg.VacDate,false);
                        if (rab.ParentID != 0 && Engine.opt().getBoolOption(Options.OPT_ID.VACC_MOTHER))
                        {
                            RabNetEngRabbit r2 = Engine.get().getRabbit(rab.ParentID);
                            r2.SetVaccine(dlg.VacID, dlg.VacDate,false);
                        }                       
                    }
                    needUpdate = false;
                    break;

                case JobType.SET_NEST://установка гнездовья
                    ReplaceForm f = new ReplaceForm();
                    f.AddRabbit(job.ID);
                    f.SetAction(ReplaceForm.Action.SET_NEST);
                    res = f.ShowDialog();
                    break;

                case JobType.BOYS_BY_ONE:
                    f = new ReplaceForm();
                    f.AddRabbit(job.ID);
                    res = f.ShowDialog();
                    break;
            }
            if (res != DialogResult.Cancel)
            {
                int idx = lvZooTech.SelectedItems[0].Index;
                lvZooTech.SelectedItems[0].Remove();
                if (idx<lvZooTech.Items.Count)
                    lvZooTech.Items[idx].Selected = true;
                fullUpdate = needUpdate;
                _rsb.Run();
            }
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            _makeFlag = 0;
            makeJob();
        }

        private void countChangedMenuItem_Click(object sender, EventArgs e)
        {
            _makeFlag = 1;
            makeJob();
        }

        private void miLust_Click(object sender, EventArgs e)
        {
            _makeFlag = -1;
            makeJob();
        }

        private void printMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            List<String> fuckers = new List<string>();
            XmlDocument rep = new XmlDocument();
            rep.AppendChild(rep.CreateElement("Rows")).AppendChild(rep.CreateElement("Row")).AppendChild(rep.CreateElement("date")).AppendChild(rep.CreateTextNode(repdate.ToLongDateString()+" "+repdate.ToLongTimeString()));
            XmlDocument doc = new XmlDocument();
            XmlDocument fucks = new XmlDocument();
            XmlElement root = doc.CreateElement("Rows");
            doc.AppendChild(root);
            XmlElement fuck = fucks.CreateElement("Rows");
            fucks.AppendChild(fuck);
            XmlElement rw;
            for (int i = 0; i < lvZooTech.Items.Count; i++)
            {
                ListViewItem li = lvZooTech.Items[i];
                ZootehJob j=(ZootehJob)li.Tag;
                rw = doc.CreateElement("Row");
                rw.AppendChild(doc.CreateElement("type")).AppendChild(doc.CreateTextNode(((int)j.Type).ToString()));
                rw.AppendChild(doc.CreateElement("days")).AppendChild(doc.CreateTextNode(j.Days.ToString()));
                //ReportHelper.Append(
                rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(j.JobName));
                rw.AppendChild(doc.CreateElement("rabbit")).AppendChild(doc.CreateTextNode(j.RabName));
                rw.AppendChild(doc.CreateElement("address")).AppendChild(doc.CreateTextNode(j.Address));
                rw.AppendChild(doc.CreateElement("breed")).AppendChild(doc.CreateTextNode(j.RabBreed));
                rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(j.RabAge.ToString()));
                if (j.Type == JobType.FUCK)
                {
                    int id = getFuckerId(j.Partners, fuckers);
                    string cmt = String.Format("см. {0:d}{1:d}",(id + 1),j.Flag>1 ? Environment.NewLine+"N"+j.Flag.ToString():"");
                    rw.AppendChild(doc.CreateElement("comment")).AppendChild(doc.CreateTextNode(cmt));
                }
                else
                    rw.AppendChild(doc.CreateElement("comment")).AppendChild(doc.CreateTextNode(j.Comment));
                root.AppendChild(rw);
            }
            for (int i = 0; i < fuckers.Count; i++)
            {
                rw = fucks.CreateElement("Row");
                rw.AppendChild(fucks.CreateElement("id")).AppendChild(fucks.CreateTextNode((i + 1).ToString()));
                rw.AppendChild(fucks.CreateElement("names")).AppendChild(fucks.CreateTextNode(fuckers[i]));
                fuck.AppendChild(rw);
            }
            XmlDocument[] xmls = new XmlDocument[] { doc, rep, fucks };
            String plan = "zooteh";
            if (fuckers.Count == 0)
            {
                plan += "_nofuck";
                xmls = new XmlDocument[] { doc, rep };
            }
            new ReportViewForm("Зоотехплан " + repdate.ToLongDateString() + " " + repdate.ToLongTimeString(), plan,xmls).ShowDialog();
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
            if (lvZooTech.SelectedItems.Count == 0)
            {
                e.Cancel = true;
                return;
            }
            ZootehJob zJob = lvZooTech.SelectedItems[0].Tag as ZootehJob;
            if (zJob.Type == JobType.FUCK)
            {
                miLust.Visible = zJob.Flag2 == 0;
            }
        }

        
    }
}
