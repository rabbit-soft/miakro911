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
            : base(sb, null)
        {
            cs = new ListViewColumnSorter(listView1, new int[] {});
            listView1.ListViewItemSorter = null;
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            f = new Filters();
            listView1.Hide();
            listView1.Items.Clear();
            listView1.ListViewItemSorter=null;
            IDataGetter gt = DataThread.db().zooTeh(f);
            rsb.setText(1, gt.getCount().ToString() + " items");
            return gt;
        }

        protected override void onItem(IData data)
        {
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
        }

    }
}
