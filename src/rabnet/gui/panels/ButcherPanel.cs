using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class ButcherPanel : RabNetPanel
    {
        public ButcherPanel() : base() { }
        public ButcherPanel(RabStatusBar sb): base(sb)
        {
            colSort = new ListViewColumnSorter(lvButcherDates, new int[] {  }, Options.OPT_ID.BUTCHER_LIST);
            lvButcherDates.ListViewItemSorter = null;
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            colSort.Prepare();
            IDataGetter dg = DataThread.db().getButcherDates(f);
            rsb.setText(1, dg.getCount().ToString() + " дат забоя");
            rsb.setText(2, dg.getCount2().ToString() + " забито");
            return dg;
        }

        protected override void onItem(IData data)
        {
            if (data == null)
            {
                colSort.Restore();
                return;
            }
            if (data == null) return;
            ButcherDate bd = (data as ButcherDate);
            ListViewItem lvi = lvButcherDates.Items.Add(bd.Date.ToShortDateString());
            lvi.SubItems.Add(bd.Victims.ToString());

            colSort.SemiReady();
        }

        public override ContextMenuStrip getMenu()
        {
            return actMenu;
        }

        private void lvButcherDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvButcherDates.SelectedItems.Count == 0) return;
            DateTime date = DateTime.Parse(lvButcherDates.SelectedItems[0].SubItems[0].Text);
            List<OneRabbit> rabbits = Engine.get().db().getVictims(date);
            lvVictims.Items.Clear();
            foreach (OneRabbit rab in rabbits)
            {
                ListViewItem lvi = lvVictims.Items.Add(rab.fullname);
                lvi.SubItems.Add(rab.age().ToString());
                lvi.SubItems.Add(rab.group.ToString());
                lvi.SubItems.Add(rab.breedname);
            }
            lvMeat.Items.Clear();
            List<sMeat> meats = Engine.get().db().getMeats(date);
            foreach (sMeat m in meats)
            {
                ListViewItem lvi = lvMeat.Items.Add(m.ProductType);
                lvi.SubItems.Add(m.Amount.ToString()+" "+m.Units);
                lvi.SubItems.Add(m.User);
            }
            
        }
    }
}
