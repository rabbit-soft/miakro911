using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class DeadForm : Form
    {
        public FilterPanel fp =null;
        public ListViewColumnSorter cs = null;
        public DeadForm()
        {
            InitializeComponent();
            fp = new DeadFilter(rsb);
            rsb.filterPanel = fp;
            cs = new ListViewColumnSorter(listView1, new int[] { 2, 3 },Options.OPT_ID.DEAD_LIST);
        }

        private IDataGetter rsb_prepareGet(object sender, EventArgs e)
        {
            cs.Prepare();
            Filters f = fp.getFilters();
            IDataGetter gt = DataThread.db().getDead(f);
            rsb.setText(1, gt.getCount().ToString() + " записей");
            return gt;
        }

        private void rsb_itemGet(object sender, RabStatusBar.RSBItemEvent e)
        {
            if (e.data == null)
            {
                cs.Restore();
                return;
            }
            Dead d = (e.data as Dead);
            ListViewItem li = listView1.Items.Add(d.name);
            li.Tag = d.id;
            li.SubItems.Add(d.deadDate.ToShortDateString());
            li.SubItems.Add(d.age.ToString());
            li.SubItems.Add(d.group.ToString());
            li.SubItems.Add(d.breed);
            li.SubItems.Add(d.address);
            li.SubItems.Add(d.reason);
            li.SubItems.Add(d.notes);
        }

        private void DeadForm_Activated(object sender, EventArgs e)
        {
            rsb.run();
        }

        private void восстановитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView1.SelectedItems)
            {
                int rid = (int)li.Tag;
                Engine.get().resurrect(rid);
            }
            rsb.run();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0) 
            {
                e.Cancel = true;
                return;
            }
            try
            {
                DateTime murderDate = DateTime.Parse(listView1.SelectedItems[0].SubItems[1].Text);
                murderDate = murderDate.AddDays(10);
                if (murderDate.Date < DateTime.Now.Date)
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch{}
            
        }
    }
}
