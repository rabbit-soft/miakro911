using System;
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
        private DateTime repdate=DateTime.Now;
        public WorksPanel():base(){}
        public int makeFlag = 0;
        private bool fullUpdate=true;
        private Filters runF = null;
        private int itm = -1;
        public WorksPanel(RabStatusBar sb)
            : base(sb, new ZootehFilter(sb))
        {
            cs = new ListViewColumnSorter(listView1, new int[] {0,},Options.OPT_ID.ZOO_LIST);
            listView1.ListViewItemSorter = cs;
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            if (fullUpdate)
            {
                f["shr"] = Engine.opt().getOption(Options.OPT_ID.SHORT_NAMES);
                f["dbl"] = Engine.opt().getOption(Options.OPT_ID.DBL_SURNAME);
                f["prt"] = Engine.opt().getOption(Options.OPT_ID.FIND_PARTNERS);
                f["okrol"] = Engine.opt().getOption(Options.OPT_ID.OKROL);
                f["preok"] = Engine.opt().getOption(Options.OPT_ID.PRE_OKROL);
                f["vudvor"] = Engine.opt().getOption(Options.OPT_ID.VUDVOR);
                f["count0"] = Engine.opt().getOption(Options.OPT_ID.COUNT1);
                f["count1"] = Engine.opt().getOption(Options.OPT_ID.COUNT2);
                f["count2"] = Engine.opt().getOption(Options.OPT_ID.COUNT3);
                f["count3"] = Engine.opt().getOption(Options.OPT_ID.SUCKERS);
                f["boysout"] = Engine.opt().getOption(Options.OPT_ID.BOYS_OUT);
                f["girlsout"] = Engine.opt().getOption(Options.OPT_ID.GIRLS_OUT);
                f["vacc"] = Engine.opt().getOption(Options.OPT_ID.VACC);
                f["nest"] = Engine.opt().getOption(Options.OPT_ID.NEST);
                f["cnest"] = Engine.opt().getOption(Options.OPT_ID.CHILD_NEST);
                f["sfuck"] = Engine.opt().getOption(Options.OPT_ID.STATE_FUCK);
                f["ffuck"] = Engine.opt().getOption(Options.OPT_ID.FIRST_FUCK);
                f["heter"] = Engine.opt().getOption(Options.OPT_ID.GETEROSIS);
                f["inbr"] = Engine.opt().getOption(Options.OPT_ID.INBREEDING);
                f["mwait"] = Engine.opt().getOption(Options.OPT_ID.MALE_WAIT);

                itm = -1;
                if (listView1.SelectedItems.Count == 1)
                    itm = listView1.SelectedItems[0].Index;
                cs.Prepare();
                listView1.Items.Clear();
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
            return DataThread.db().zooTeh(f);
        }

        protected override void onItem(IData data)
        {
            ZooTehNullItem it = data as ZooTehNullItem;
            if (it == null)
            {
                cs.Restore();
                if (itm > -1 && listView1.Items.Count > itm)
                {
                    listView1.Items[itm].Selected = true;
                    listView1.Items[itm].EnsureVisible();
                }
                listView1.Focus();
                return;
            }
            Filters f = runF;
            foreach (ZootehJob j in Engine.get().zoo().makeZooTehPlan(f,it.id))
            {
                ListViewItem li = listView1.Items.Add(j.days.ToString());
                li.SubItems.Add(j.job);
                li.SubItems.Add(j.address);
                li.SubItems.Add(j.name);
                li.SubItems.Add(j.age.ToString());
                li.SubItems.Add(j.breed);
                li.SubItems.Add(j.comment);
                li.SubItems.Add(j.names);
                li.Tag = j;
            }

			cs.SemiReady();
        }

        private void fillLogs(Filters f)
        {
            listView2.Items.Clear();
            foreach (LogList.OneLog l in Engine.db().getLogs(f).logs)
            {
                ListViewItem li = listView2.Items.Add(l.date.ToShortDateString() + " " + l.date.ToShortTimeString());
                li.SubItems.Add(l.work);
                li.SubItems.Add(l.address);
                li.SubItems.Add(l.prms);
                li.SubItems.Add(l.user);
            }
            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
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
        public void setMenu(JobType type,ZootehJob job)
        {
            okrolMenuItem.Visible = vudvorMenuItem.Visible = false;
            countsMenuItem.Visible = preokrolMenuItem.Visible= false;
            boysOutMenuItem.Visible = girlsOutMenuItem.Visible = false;
            vaccMenuItem.Visible = fuckMenuItem.Visible = false;
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
            }
        }

        private ZootehJob getCurJob()
        {
            if (listView1.SelectedItems.Count != 1)
                return null;
            return  (listView1.SelectedItems[0].Tag) as ZootehJob;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                setMenu(JobType.NONE);
                return;
            }
            setMenu(getCurJob().type,getCurJob());
        }

        private void makeJob()
        {
            DialogResult res=DialogResult.OK;
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
                    if (makeFlag == 0)
                    {
                        RabNetEngRabbit rr = Engine.get().getRabbit(job.id);
                        rr.CountKids(0, 0, 0, rr.youngcount, rr.youngers[0].age());
                        needUpdate = false;
                    }else
                    res=(new CountKids(job.id,job.flag==1)).ShowDialog();
                    break;
                case JobType.FUCK:
                    int id = job.id;
                    if (job.flag > 1)
                    {
                        id = 0;
                        rf = new ReplaceForm();
                        rf.addRabbit(job.id);
                        rf.setAction(ReplaceForm.Action.ONE_GIRL_OUT);
                        res = rf.ShowDialog();
                        if (res == DialogResult.OK)
                            id = rf.getGirlOut();
                        res = DialogResult.Cancel;
                    }
                    if (id!=0) 
                        res=(new MakeFuck(id)).ShowDialog();
                    break;                                    
                case JobType.OKROL:
                    res=(new OkrolForm(job.id)).ShowDialog();
                    break;
                case JobType.VACC:
                    RabNetEngRabbit r = Engine.get().getRabbit(job.id);
                    r.spec = true;
                    r.commit();
                    needUpdate = false;
                    break;
                case JobType.SET_NEST:
                    ReplaceForm f=new ReplaceForm();
                    f.addRabbit(job.id);
                    f.setAction(ReplaceForm.Action.SET_NEST);
                    res = f.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        res = DialogResult.Cancel;
                        RabNetEngRabbit rr = Engine.get().getRabbit(job.id);
                        RabNetEngBuilding rb = Engine.get().getBuilding(rr.justAddress);
                        if (rb.type == "jurta")
                        {
                            rb.setNest(true);
                            res = DialogResult.OK;
                        }
                    }
                    break;
            }
            if (res != DialogResult.Cancel)
            {
                int idx = listView1.SelectedItems[0].Index;
                listView1.SelectedItems[0].Remove();
                if (idx<listView1.Items.Count)
                    listView1.Items[idx].Selected = true;
                fullUpdate = needUpdate;
                rsb.run();
            }
        }

        private void okrolMenuItem_Click(object sender, EventArgs e)
        {
            makeFlag = 0;
            makeJob();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
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

        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<String> fuckers = new List<string>();
            XmlDocument rep=new XmlDocument();
            rep.AppendChild(rep.CreateElement("Rows")).AppendChild(rep.CreateElement("Row")).AppendChild(rep.CreateElement("date")).AppendChild(rep.CreateTextNode(repdate.ToLongDateString()+" "+repdate.ToLongTimeString()));
            XmlDocument xml = new XmlDocument();
            XmlDocument fucks = new XmlDocument();
            XmlElement root = xml.CreateElement("Rows");
            xml.AppendChild(root);
            XmlElement fuck = fucks.CreateElement("Rows");
            fucks.AppendChild(fuck);
            XmlElement rw;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                ListViewItem li = listView1.Items[i];
                ZootehJob j=(ZootehJob)li.Tag;
                rw = xml.CreateElement("Row");
                rw.AppendChild(xml.CreateElement("type")).AppendChild(xml.CreateTextNode(((int)j.type).ToString()));
                rw.AppendChild(xml.CreateElement("days")).AppendChild(xml.CreateTextNode(j.days.ToString()));
                rw.AppendChild(xml.CreateElement("name")).AppendChild(xml.CreateTextNode(j.job));
                rw.AppendChild(xml.CreateElement("rabbit")).AppendChild(xml.CreateTextNode(j.name));
                rw.AppendChild(xml.CreateElement("address")).AppendChild(xml.CreateTextNode(j.address));
                rw.AppendChild(xml.CreateElement("comment")).AppendChild(xml.CreateTextNode(j.comment));
                rw.AppendChild(xml.CreateElement("breed")).AppendChild(xml.CreateTextNode(j.breed));
                rw.AppendChild(xml.CreateElement("age")).AppendChild(xml.CreateTextNode(j.age.ToString()));
                if (j.type == JobType.FUCK)
                {
                    int id = getFuckerId(j.names, fuckers);
                    rw.AppendChild(xml.CreateElement("fuckers")).AppendChild(xml.CreateTextNode("см. "+(id+1).ToString()));
                }
                else
                    rw.AppendChild(xml.CreateElement("fuckers")).AppendChild(xml.CreateTextNode(""));
                root.AppendChild(rw);
            }
            for (int i = 0; i < fuckers.Count; i++)
            {
                rw = fucks.CreateElement("Row");
                rw.AppendChild(fucks.CreateElement("id")).AppendChild(fucks.CreateTextNode((i + 1).ToString()));
                rw.AppendChild(fucks.CreateElement("names")).AppendChild(fucks.CreateTextNode(fuckers[i]));
                fuck.AppendChild(rw);
            }
            XmlDocument[] xmls=new XmlDocument[]{xml,rep,fucks};
            String plan = "zooteh";
            if (fuckers.Count==0)
            {
                plan += "_nofuck";
                xmls = new XmlDocument[] { xml, rep };
            }
            new ReportViewForm("Зоотехплан " + repdate.ToLongDateString() + " " + repdate.ToLongTimeString(), plan, 
                xmls).ShowDialog();
        }

        private void countChangedMenuItem_Click(object sender, EventArgs e)
        {
            makeFlag = 1;
            makeJob();
        }

    }
}
