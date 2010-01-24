using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class WorksPanel : RabNetPanel
    {
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
            switch (type)
            {
                case JobType.OKROL: okrolMenuItem.Visible = true; break;
                case JobType.VUDVOR: vudvorMenuItem.Visible = true; break;
                case JobType.COUNT_KIDS: countsMenuItem.Visible = true; break;
                case JobType.PRE_OKROL: preokrolMenuItem.Visible = true; break;
                case JobType.BOYS_OUT: boysOutMenuItem.Visible = true; break;
                case JobType.GIRLS_OUT: girlsOutMenuItem.Visible = true; break;
                case JobType.FUCK: fuckMenuItem.Visible = true; break;
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
            ZootehJob job = getCurJob();
            if (job == null)
                return;
            switch (job.type)
            {
                case JobType.OKROL:
                    (new OkrolForm(job.id)).ShowDialog();
                    break;
                case JobType.VUDVOR:
                    RabNetEngBuilding b = Engine.get().getBuilding(job.id);
                    if (job.id2 == 0)
                        b.setNest(false);
                    else
                        b.setNest2(false);
                    break;
                case JobType.COUNT_KIDS:
                    (new CountKids(job.id)).ShowDialog();
                    break;
                case JobType.PRE_OKROL:
                    Engine.get().preOkrol(job.id);
                    break;
                case JobType.BOYS_OUT:
                case JobType.GIRLS_OUT:
                    ReplaceForm rf = new ReplaceForm();
                    rf.addRabbit(job.id);
                    if (job.type==JobType.BOYS_OUT)
                        rf.setAction(ReplaceForm.Action.BOYSOUT);
                    rf.ShowDialog();
                    break;
                case JobType.FUCK:
                    (new MakeFuck(job.id)).ShowDialog();
                    break;
            }
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

    }
}
