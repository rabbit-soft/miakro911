using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using gamlib;

namespace rabnet.forms
{
    public partial class LogsForm : Form
    {
        public LogsForm()
        {
            InitializeComponent();
            dtpDateTo.Value = dtpDateTo.MaxDate = dtpDateFrom.Value = dtpDateFrom.MaxDate = DateTime.Now.Date;
            lgTypes.UpdateList();
            getBuildings();
            fillLogs();
        }

        private void getBuildings()
        {
            cbAddress.Items.Clear();
            cbAddress.Items.Add("Любой");
            BuildingList builds = Engine.db().getBuildings(new Filters());
            foreach (Building b in builds)
            {
                for (int i = 0; i < b.Sections; i++)                
                    cbAddress.Items.Add(b.SmallName(i));               
            }
            cbAddress.SelectedIndex = 0;
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            Helper.checkIntNumber(sender, e);
        }

        private void btSearch_Click(object sender, EventArgs e)
        {            
            fillLogs();
        }

        /// <summary>
        /// Заполнение listView c логами
        /// </summary>
        /// <param name="f">Коллекция фильтров</param>
        private void fillLogs()
        {
#if !DEMO
            Filters f = getFilter();
            lvLogs.Items.Clear();
            foreach (LogList.OneLog l in Engine.db().getLogs(f).logs)
            {
                ListViewItem li = lvLogs.Items.Add(l.date.ToShortDateString() + " " + l.date.ToShortTimeString());
                li.SubItems.Add(l.work);
                li.SubItems.Add(l.address);
                li.SubItems.Add(l.prms);
                li.SubItems.Add(l.user);
            }
            //lvLogs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
#endif
        }

        private Filters getFilter()
        {
            Filters f = new Filters();
            f[Filters.LOGS] = lgTypes.GetChecked();
            if (tbRabID.Text != "")
                f[Filters.RAB_ID] = tbRabID.Text;
            if (cbAddress.SelectedIndex != 0)
                f[Filters.ADDRESS] = cbAddress.Text.Trim();
            if (chPeriod.Checked)
            {
                f[Filters.DATE_FROM] = dtpDateFrom.Value.ToString("yyyy-MM-dd");
                f[Filters.DATE_TO] = dtpDateTo.Value.ToString("yyyy-MM-dd");
            }
            if (tbRabID.Text != "")
                f[Filters.RAB_ID] = tbRabID.Text;
            f[Filters.LIMIT] = nudLogLim.Value.ToString();
            return f;
        }

        private void btShowTypes_Click(object sender, EventArgs e)
        {
            if (btShowTypes.Tag.ToString() == "0")
            {
                splitContainer1.Panel2Collapsed = false;
                btShowTypes.Tag = "1";
                btShowTypes.Text = "Скрыть типы логов";
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
                btShowTypes.Tag = "0";
                btShowTypes.Text = "Показать типы логов";
            }
        }

        private void chPeriod_CheckedChanged(object sender, EventArgs e)
        {
            dtpDateFrom.Enabled = dtpDateTo.Enabled = chPeriod.Checked;
        }
    }
}
