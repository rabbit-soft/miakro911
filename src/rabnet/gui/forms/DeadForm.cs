using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using rabnet.filters;
using rabnet.components;

namespace rabnet.forms
{
    public partial class DeadForm : Form
    {
        public FilterPanel fp =null;
        public ListViewColumnSorter cs = null;

        public DeadForm()
        {
            InitializeComponent();
            fp = new DeadFilter();
            rsb.FilterPanel = fp;
            cs = new ListViewColumnSorter(listView1, new int[] { 2, 3 },Options.OPT_ID.DEAD_LIST);
            FormSizeSaver.Append(this);
        }

        private IDataGetter rsb_prepareGet()
        {
            cs.Prepare();
            Filters f = fp.getFilters();
            IDataGetter gt = Engine.db2().getDead(f);
            rsb.SetText(1, gt.getCount().ToString() + " записей");
            return gt;
        }

        private void rsb_itemGet(IData data)
        {
            if (data == null)
            {
                cs.Restore();
                return;
            }
            Dead d = (data as Dead);
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
            rsb.Run();
        }

        private void miRestore_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView1.SelectedItems)
            {
                int rid = (int)li.Tag;
                Engine.get().resurrect(rid);
            }
            rsb.Run();
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
                murderDate = murderDate.AddDays(30);
                if (murderDate.Date < DateTime.Now.Date)
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch{}
            
        }

        private void miChangeReason_Click(object sender, EventArgs e)
        {
            int id = (int)listView1.SelectedItems[0].Tag;
            string name = listView1.SelectedItems[0].SubItems[0].Text;
            string reason = listView1.SelectedItems[0].SubItems[6].Text;
            DeadReasonChangeForm frm = new DeadReasonChangeForm(id,name,reason);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                Engine.get().logs().log(RabNetLogs.LogType.CHANGE_DEADREASON, id);
                Engine.db().changeDeadReason(id, frm.NewReason);
            }
        }

    }
}
