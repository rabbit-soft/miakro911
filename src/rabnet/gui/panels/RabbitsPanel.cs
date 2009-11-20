using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class RabbitsPanel : RabNetPanel
    {
        public RabbitsPanel():base()
        {
        }
        public RabbitsPanel(RabStatusBar rsb):base(rsb,new RabbitsFilter(rsb))
        {
            cs = new ListViewColumnSorter(listView1, new int[] { 2, 9 });
            listView1.ListViewItemSorter = null;
        }

        protected override IDataGetter onPrepare(Filters flt)
        {
            listView1.Items.Clear();
            listView1.Hide();
            Options op = Engine.opt();
            flt["shr"] = op.getOption(Options.OPT_ID.SHORT_NAMES);
            flt["sht"] = op.getOption(Options.OPT_ID.SHOW_TIER_TYPE);
            flt["sho"] = op.getOption(Options.OPT_ID.SHOW_TIER_SEC);
            flt["dbl"] = op.getOption(Options.OPT_ID.DBL_SURNAME);
            flt["num"] = op.getOption(Options.OPT_ID.SHOW_NUMBERS);
            listView1.ListViewItemSorter = null;
            IDataGetter dg = DataThread.db().getRabbits(flt);
            rsb.setText(1, dg.getCount().ToString() + " items");
            return dg;
        }

        protected override void onItem(IData data)
        {
            if (data == null)
            {
                listView1.ListViewItemSorter = cs;
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Show();
                return;
            }
            IRabbit rab = (data as IRabbit);
            ListViewItem li = listView1.Items.Add(rab.name());
            li.Tag = rab.id();
            li.SubItems.Add(rab.sex());
            li.SubItems.Add(rab.age().ToString());
            li.SubItems.Add(rab.breed());
            li.SubItems.Add(rab.weight());
            li.SubItems.Add(rab.status());
            li.SubItems.Add(rab.bgp());
            li.SubItems.Add(rab.N());
            li.SubItems.Add(rab.average() == 0 ? "" : rab.average().ToString());
            li.SubItems.Add(rab.rate().ToString());
            li.SubItems.Add(rab.cls());
            li.SubItems.Add(rab.address());
            li.SubItems.Add(rab.notes());
        }

    }
}
