using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace rabnet
{
    public partial class MainForm : Form
    {
        private BuildingsForm buildings = new BuildingsForm();
        protected static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        public MainForm()
        {
            InitializeComponent();
            log.Debug("Program started");
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChangeFarmMenuItem_Click(object sender, EventArgs e)
        {
            (new LoginForm()).ShowDialog();
        }

        private IDataGetter rabStatusBar1_prepareGet(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView1.Hide();
            return Engine.get().db().getRabbits("");
        }

        private void rabStatusBar1_itemGet(object sender, RabStatusBar.RSBItemEvent e)
        {
            if (e.data==null)
            {
                listView1.Show();
                return;
            }
            IRabbit rab = (e.data as IRabbit);
            ListViewItem li = listView1.Items.Add(rab.id().ToString());
            li.SubItems.Add(rab.name());
            li.SubItems.Add(rab.surname());
            li.SubItems.Add(rab.secname());
            li.SubItems.Add(rab.sex());
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            rabStatusBar1.setText(0, DataThread.getdbsafe(false).now().ToShortDateString());
            rabStatusBar1.run();
            Text = Engine.get().farmName();
            dtpDateFrom.Value = DateTime.Today.Subtract(new TimeSpan((int)nudDateFrom.Value, 0, 0, 0));
            dtpDateTo.Value = DateTime.Today.Subtract(new TimeSpan((int)nudDateTo.Value, 0, 0, 0));
            dtpPregFrom.Value = DateTime.Today.Subtract(new TimeSpan((int)nudPregFrom.Value, 0, 0, 0));
            dtpPregTo.Value = DateTime.Today.Subtract(new TimeSpan((int)nudPregTo.Value, 0, 0, 0));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rabStatusBar1.filterHide();
        }

        private void фильтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rabStatusBar1.filterSwitch();
        }

        private void постройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (buildings == null)
                buildings = new BuildingsForm();
            buildings.Visible = !buildings.Visible;
            if (buildings.Visible)
                buildings.BringToFront();
        }

        public String filterTostring()
        {
            return "";
        }

        public void filterFromString()
        {
        }

        #region filter_form_process

        private void cbDateFrom_CheckedChanged(object sender, EventArgs e)
        {
            dtpDateFrom.Enabled = nudDateFrom.Enabled = cbDateFrom.Checked;
        }

        private void cbDateTo_CheckedChanged(object sender, EventArgs e)
        {
            dtpDateTo.Enabled = nudDateTo.Enabled = cbDateTo.Checked;
        }

        private void cbWeightFrom_CheckedChanged(object sender, EventArgs e)
        {
            nudWeightFrom.Enabled = cbWeightFrom.Checked;
        }

        private void cbWeightTo_CheckedChanged(object sender, EventArgs e)
        {
            nudWeightTo.Enabled = cbWeightTo.Checked;
        }

        private void nudDateFrom_ValueChanged(object sender, EventArgs e)
        {
            dtpDateFrom.Value = DateTime.Today.Subtract(new TimeSpan((int)nudDateFrom.Value, 0, 0, 0));
        }

        private void dtpDateFrom_ValueChanged(object sender, EventArgs e)
        {
            nudDateFrom.Value = (DateTime.Today - dtpDateFrom.Value).Days;
        }

        private void dtpDateTo_ValueChanged(object sender, EventArgs e)
        {
            nudDateTo.Value = (DateTime.Today - dtpDateTo.Value).Days;
        }

        private void nudDateTo_ValueChanged(object sender, EventArgs e)
        {
            dtpDateTo.Value = DateTime.Today.Subtract(new TimeSpan((int)nudDateTo.Value, 0, 0, 0));
        }

        private void cbPregFrom_CheckedChanged(object sender, EventArgs e)
        {
            dtpPregFrom.Enabled = nudPregFrom.Enabled = cbPregFrom.Checked;
            if (cbPregFrom.Checked)
                cobPregnant.SelectedIndex = 2;
        }

        private void cbPregTo_CheckedChanged(object sender, EventArgs e)
        {
            dtpPregTo.Enabled = nudPregTo.Enabled = cbPregTo.Checked;
            if (cbPregTo.Checked)
                cobPregnant.SelectedIndex = 2;
        }

        private void cobPregnant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cobPregnant.SelectedIndex != 2)
            {
                cbPregTo.Checked= dtpPregTo.Enabled = nudPregTo.Enabled = false;
                cbPregFrom.Checked = dtpPregFrom.Enabled = nudPregFrom.Enabled = false;
            }
        }

        private void nudPregFrom_ValueChanged(object sender, EventArgs e)
        {
            dtpPregFrom.Value = DateTime.Today.Subtract(new TimeSpan((int)nudPregFrom.Value, 0, 0, 0));
        }
        private void dtpPregFrom_ValueChanged(object sender, EventArgs e)
        {
            nudPregFrom.Value = (DateTime.Today - dtpPregFrom.Value).Days;
        }
        private void dtpPregTo_ValueChanged(object sender, EventArgs e)
        {
            nudPregTo.Value = (DateTime.Today - dtpPregTo.Value).Days;
        }
        private void nudPregTo_ValueChanged(object sender, EventArgs e)
        {
            dtpPregTo.Value = DateTime.Today.Subtract(new TimeSpan((int)nudPregTo.Value, 0, 0, 0));
        }
        #endregion








    }
}
