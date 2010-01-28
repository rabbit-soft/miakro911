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
        public WorksPanel(RabStatusBar sb)
            : base(sb, new ZootehFilter(sb))
        {
            cs = new ListViewColumnSorter(listView1, new int[] {});
            listView1.ListViewItemSorter = null;
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            listView1.Items.Clear();
            repdate = DateTime.Now;
            foreach (ZootehJob j in Engine.get().zoo().makeZooTehPlan(f))
            {
                ListViewItem li = listView1.Items.Add(j.days.ToString());
                li.SubItems.Add(j.job);
                li.SubItems.Add(j.address);
                li.SubItems.Add(j.name);
                li.SubItems.Add(j.age.ToString());
                li.SubItems.Add(j.names);
                li.SubItems.Add(j.addresses);
                li.SubItems.Add(j.comment);
                li.Tag = j;
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            fillLogs(f);
            DataThread.get().stop();
            return null;
        }
/*        protected override IDataGetter onPrepare(Filters f)
        {
            f = new Filters();
            listView1.Hide();
            listView1.Items.Clear();
            listView1.ListViewItemSorter=null;
            IDataGetter gt = DataThread.db().zooTeh(f);
            rsb.setText(1, gt.getCount().ToString() + " items");
            fillLogs();
            return gt;
        }
*/
        protected override void onItem(IData data)
        {
            /*
            if (data == null)
            {
            //    listView1.ListViewItemSorter = cs.Clear();
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Show();
                return;
            }
            ZooTehItem z = (data as ZooTehItem);
            ListViewItem li = listView1.Items.Add(z.level.ToString());
            li.SubItems.Add(z.job);
            li.SubItems.Add(z.address);
            li.SubItems.Add(z.rabbit);
            li.SubItems.Add(z.age.ToString());
            li.SubItems.Add("");
            li.SubItems.Add("");
            li.SubItems.Add(z.notes);
            li.SubItems.Add(z.dt.ToShortDateString());
            li.SubItems.Add(z.done.ToString());
             * */
        }

        private void fillLogs(Filters f)
        {
            //Filters f=new Filters();
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
            okrolMenuItem.Visible = vudvorMenuItem.Visible = false;
            countsMenuItem.Visible = preokrolMenuItem.Visible= false;
            boysOutMenuItem.Visible = girlsOutMenuItem.Visible = false;
            vaccMenuItem.Visible = fuckMenuItem.Visible = false;
            setNestMenuItem.Visible = false;
            switch (type)
            {
                case JobType.OKROL: okrolMenuItem.Visible = true; break;
                case JobType.VUDVOR: vudvorMenuItem.Visible = true; break;
                case JobType.COUNT_KIDS: countsMenuItem.Visible = true; break;
                case JobType.PRE_OKROL: preokrolMenuItem.Visible = true; break;
                case JobType.BOYS_OUT: boysOutMenuItem.Visible = true; break;
                case JobType.GIRLS_OUT: girlsOutMenuItem.Visible = true; break;
                case JobType.FUCK: fuckMenuItem.Visible = true; break;
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
            setMenu(getCurJob().type);
        }

        private void makeJob()
        {
            DialogResult res=DialogResult.Cancel;
            ZootehJob job = getCurJob();
            if (job == null)
                return;
            switch (job.type)
            {                
                case JobType.VUDVOR:
                    RabNetEngBuilding b = Engine.get().getBuilding(job.id);
                    if (job.id2 == 0)
                        b.setNest(false);
                    else
                        b.setNest2(false);
                    listView1.SelectedItems[0].Remove();
                    break;
                case JobType.PRE_OKROL:
                    Engine.get().preOkrol(job.id);
                    listView1.SelectedItems[0].Remove();
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
                    listView1.SelectedItems[0].Remove();
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
            if(res!=DialogResult.Cancel)
                rsb.run();
        }

        private void okrolMenuItem_Click(object sender, EventArgs e)
        {
            makeJob();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
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
                if (j.type == JobType.FUCK)
                {
                    int id = getFuckerId(j.names, fuckers);
                    rw.AppendChild(xml.CreateElement("fuckers")).AppendChild(xml.CreateTextNode("см. "+id.ToString()));
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
            new ReportViewForm("Зоотехплан " + repdate.ToLongDateString() + " " + repdate.ToLongTimeString(), "zooteh", 
                new XmlDocument[]{xml,rep,fucks}).ShowDialog();
        }

    }
}
