﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace rabnet
{
    public partial class WorksPanel : RabNetPanel
    {
        private DateTime repdate = DateTime.Now;
        public WorksPanel():base(){}
        public int makeFlag = 0;
        private bool fullUpdate=true;
        private Filters runF = null;
        private int itm = -1;

        public WorksPanel(RabStatusBar sb): base(sb, new ZootehFilter(sb))
        {           
            colSort = new ListViewColumnSorter(lvZooTech, new int[] {0,4},Options.OPT_ID.ZOO_LIST);
            colSort2 = new ListViewColumnSorter(lvZooTech, new int[] { 0, 4 }, Options.OPT_ID.ZOO_LIST);
            lvZooTech.ListViewItemSorter = colSort;
            MakeExcel = new RabStatusBar.ExcelButtonClickDelegate(this.makeExcel);
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            if (fullUpdate)
            {
                f["shr"] = Engine.opt().safeIntOption(Options.OPT_ID.SHORT_ZOO, 1).ToString();
                f["dbl"] = f["shr"] == "1" ? "0" : "1";//Engine.opt().getOption(Options.OPT_ID.DBL_SURNAME);
                f["prt"] = Engine.opt().getOption(Options.OPT_ID.FIND_PARTNERS);
                f["okrol"] = Engine.opt().getOption(Options.OPT_ID.OKROL);
                f["preok"] = Engine.opt().getOption(Options.OPT_ID.PRE_OKROL);
                f["vudvor"] = Engine.opt().getOption(Options.OPT_ID.NEST_OUT);
                f["vd_sukr"] = Engine.opt().getOption(Options.OPT_ID.NEST_OUT_IF_SUKROL);
                f["count1"] = Engine.opt().getOption(Options.OPT_ID.COUNT1);
                f["count2"] = Engine.opt().getOption(Options.OPT_ID.COUNT2);
                f["count3"] = Engine.opt().getOption(Options.OPT_ID.COUNT3);
                //f["count3"] = Engine.opt().getOption(Options.OPT_ID.COUNT_SUCKERS);
                f["boysout"] = Engine.opt().getOption(Options.OPT_ID.BOYS_OUT);
                f["girlsout"] = Engine.opt().getOption(Options.OPT_ID.GIRLS_OUT);
                f[Filters.ZT_VACC_DAYS] = Engine.opt().getOption(Options.OPT_ID.VACC);
                f["nest"] = Engine.opt().getOption(Options.OPT_ID.NEST_IN);
                f["cnest"] = Engine.opt().getOption(Options.OPT_ID.CHILD_NEST);
                f["sfuck"] = Engine.opt().getOption(Options.OPT_ID.STATE_FUCK);
                f["ffuck"] = Engine.opt().getOption(Options.OPT_ID.FIRST_FUCK);
                f["heter"] = Engine.opt().getOption(Options.OPT_ID.GETEROSIS);
                f["inbr"] = Engine.opt().getOption(Options.OPT_ID.INBREEDING);
                f["mwait"] = Engine.opt().getOption(Options.OPT_ID.MALE_WAIT);
                //f["vactime"] = Engine.opt().getOption(Options.OPT_ID.VACCINE_TIME);
                f["bbone"] = Engine.opt().getOption(Options.OPT_ID.BOYS_BY_ONE);
                f[Filters.ZT_VACC_MOTH] = Engine.opt().getBoolOption(Options.OPT_ID.VACC_MOTHER)?"1":"0";
                f[Filters.ZT_VACC_SHOW] = "";
                foreach (CatalogData.Row row in Engine.db().getVaccines().Get().Rows)
                {
                    if (row.data[2] == "1")
                        f[Filters.ZT_VACC_SHOW] += String.Format("{0:d},", row.key);
                }
                f[Filters.ZT_VACC_SHOW] = f[Filters.ZT_VACC_SHOW].TrimEnd(',');
                itm = -1;
                if (lvZooTech.SelectedItems.Count == 1)
                    itm = lvZooTech.SelectedItems[0].Index;
                colSort.Prepare();
                lvZooTech.Items.Clear();
                repdate = DateTime.Now;
            }
            runF = f;
            fillLogs(f);
            DataThread.get().stop();
            if (!fullUpdate)
            {
                fullUpdate = true;
                return null;
            }
            return DataThread.db().zooTeh(f);//возвращает ZooTehNullGetter
        }

        protected override void onItem(IData data)
        {
            ZooTehNullItem it = data as ZooTehNullItem;
            if (it == null)
            {
                colSort.Restore();
                if (itm > -1 && lvZooTech.Items.Count > itm)
                {
                    lvZooTech.Items[itm].Selected = true;
                    lvZooTech.Items[itm].EnsureVisible();
                }
                lvZooTech.Focus();
                return;
            }
            Filters f = runF;
            foreach (ZootehJob j in Engine.get().zoo().makeZooTehPlan(f,it.id))
            {
                ListViewItem li = lvZooTech.Items.Add(j.days.ToString());
                li.SubItems.Add(j.job);
                li.SubItems.Add(j.address);
                li.SubItems.Add(j.name);
                li.SubItems.Add(j.age.ToString());
                li.SubItems.Add(j.breed);
                li.SubItems.Add(j.comment);
                li.SubItems.Add(j.names);
                li.Tag = j;
            }
			colSort.SemiReady();
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
                vaccMenuItem.Visible = fuckMenuItem.Visible =
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
                    if (job.job == "Случка")
                        fuckMenuItem.Text = "Случить";
                    else
                        fuckMenuItem.Text = "Вязать";
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

        private void lvZooTech_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvZooTech.SelectedItems.Count != 1)
            {
                setMenu(JobType.NONE);
                return;
            }
            setMenu(getCurJob().type,getCurJob());
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
            switch (job.type)
            {                
                case JobType.VUDVOR:
                    RabNetEngBuilding b = Engine.get().getBuilding(job.id);
                    if (job.id2 == 0)
                        b.setNest(false);
                    else
                        b.setNest2(false);
                    needUpdate = false;
                    break;
                case JobType.PRE_OKROL:
                    Engine.get().preOkrol(job.id);
                    needUpdate = false;
                    break;
                case JobType.BOYS_OUT:
                case JobType.GIRLS_OUT:
                    ReplaceForm rf = new ReplaceForm();
                    rf.addRabbit(job.id);
                    if (job.type==JobType.BOYS_OUT)
                        rf.setAction(ReplaceForm.Action.BOYSOUT);
                    res=rf.ShowDialog();
                    break;                                   
                case JobType.COUNT_KIDS:
                    RabNetEngRabbit rrr = Engine.get().getRabbit(job.id);
                    CountKids ck;
                    int id2 = 0;
                    for (int i = 0; i < rrr.Youngers.Length; i++)
                        if (rrr.Youngers[i].id == job.id2) id2 = i;
                    if (makeFlag == 0)
                    {
                        rrr.CountKids(0, 0, 0, rrr.Youngers[id2].group, rrr.Youngers[id2].age(), 0);
                        needUpdate = false;
                    }
                    else
                    {
                        ck = new CountKids(job.id, job.flag == 1);
                        ck.setGrp(id2);
                        res = ck.ShowDialog();
                    }
                    break;
                case JobType.FUCK:
                    int id = job.id;
                    if (job.flag > 1)
                    {
                        id = 0;
                        ReplaceBrideForm rb=new ReplaceBrideForm(job.id);
                        res = rb.ShowDialog();
                        if (res == DialogResult.OK)
                            id = rb.getGirlOut();
                        res = DialogResult.Cancel;
                    }
                    if (id!=0) 
                        res=(new MakeFuck(id)).ShowDialog();
                    break;                                    
                case JobType.OKROL:
                    res=(new OkrolForm(job.id)).ShowDialog();
                    break;
                case JobType.VACC: //TODO прививки зоотехплана
                    RabNetEngRabbit rab = Engine.get().getRabbit(job.id);
                    AddRabVacForm dlg = new AddRabVacForm(rab);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        rab.SetVaccine(dlg.VacID, dlg.VacDate,false);
                        if (rab.Parent != 0 && Engine.opt().getBoolOption(Options.OPT_ID.VACC_MOTHER))
                        {
                            RabNetEngRabbit r2 = Engine.get().getRabbit(rab.Parent);
                            r2.SetVaccine(dlg.VacID, dlg.VacDate,false);
                        }
                    }
                    needUpdate = false;
                    break;
                case JobType.SET_NEST://установка гнездовья
                    ReplaceForm f = new ReplaceForm();
                    f.addRabbit(job.id);
                    f.setAction(ReplaceForm.Action.SET_NEST);
                    res = f.ShowDialog();
                    break;
                case JobType.BOYS_BY_ONE:
                    f = new ReplaceForm();
                    f.addRabbit(job.id);
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
                _rsb.run();
            }
        }

        private void okrolMenuItem_Click(object sender, EventArgs e)
        {
            makeFlag = 0;
            makeJob();
        }

        private void lvZooTech_DoubleClick(object sender, EventArgs e)
        {
            makeFlag = 0;
            makeJob();

        }

        private int getFuckerId(String f,List<String> lst)
        {
            for (int i = 0; i < lst.Count; i++)
                if (lst[i] == f) return i;
            lst.Add(f);
            return lst.Count - 1;
        }

        private void printMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            List<String> fuckers = new List<string>();
            XmlDocument rep = new XmlDocument();
            rep.AppendChild(rep.CreateElement("Rows")).AppendChild(rep.CreateElement("Row")).AppendChild(rep.CreateElement("date")).AppendChild(rep.CreateTextNode(repdate.ToLongDateString()+" "+repdate.ToLongTimeString()));
            XmlDocument xml = new XmlDocument();
            XmlDocument fucks = new XmlDocument();
            XmlElement root = xml.CreateElement("Rows");
            xml.AppendChild(root);
            XmlElement fuck = fucks.CreateElement("Rows");
            fucks.AppendChild(fuck);
            XmlElement rw;
            for (int i = 0; i < lvZooTech.Items.Count; i++)
            {
                ListViewItem li = lvZooTech.Items[i];
                ZootehJob j=(ZootehJob)li.Tag;
                rw = xml.CreateElement("Row");
                rw.AppendChild(xml.CreateElement("type")).AppendChild(xml.CreateTextNode(((int)j.type).ToString()));
                rw.AppendChild(xml.CreateElement("days")).AppendChild(xml.CreateTextNode(j.days.ToString()));
                rw.AppendChild(xml.CreateElement("name")).AppendChild(xml.CreateTextNode(j.job));
                rw.AppendChild(xml.CreateElement("rabbit")).AppendChild(xml.CreateTextNode(j.name));
                rw.AppendChild(xml.CreateElement("address")).AppendChild(xml.CreateTextNode(j.address));
                //rw.AppendChild(xml.CreateElement("comment")).AppendChild(xml.CreateTextNode(""));
                rw.AppendChild(xml.CreateElement("breed")).AppendChild(xml.CreateTextNode(j.breed));
                rw.AppendChild(xml.CreateElement("age")).AppendChild(xml.CreateTextNode(j.age.ToString()));
                if (j.type == JobType.FUCK)
                {
                    int id = getFuckerId(j.names, fuckers);
                    rw.AppendChild(xml.CreateElement("comment")).AppendChild(xml.CreateTextNode("см. "+(id+1).ToString()));
                }
                else
                    rw.AppendChild(xml.CreateElement("comment")).AppendChild(xml.CreateTextNode(j.comment));
                root.AppendChild(rw);
            }
            for (int i = 0; i < fuckers.Count; i++)
            {
                rw = fucks.CreateElement("Row");
                rw.AppendChild(fucks.CreateElement("id")).AppendChild(fucks.CreateTextNode((i + 1).ToString()));
                rw.AppendChild(fucks.CreateElement("names")).AppendChild(fucks.CreateTextNode(fuckers[i]));
                fuck.AppendChild(rw);
            }
            XmlDocument[] xmls = new XmlDocument[] { xml, rep, fucks };
            String plan = "zooteh";
            if (fuckers.Count == 0)
            {
                plan += "_nofuck";
                xmls = new XmlDocument[] { xml, rep };
            }
            new ReportViewForm("Зоотехплан " + repdate.ToLongDateString() + " " + repdate.ToLongTimeString(), plan,xmls).ShowDialog();
#else 
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void countChangedMenuItem_Click(object sender, EventArgs e)
        {
            makeFlag = 1;
            makeJob();
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

    }
}
