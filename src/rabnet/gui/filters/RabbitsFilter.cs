using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class RabbitsFilter : FilterPanel
    {
        public RabbitsFilter(RabStatusBar sb):base(sb,"rabbits",Options.OPT_ID.RAB_FILTER)
        {
            //InitializeComponent();
        }
        public RabbitsFilter() : base() { }

        #region filter_form_process

        public override Filters getFilters()
        {
            Filters f = new Filters();
            if (!cbSexFemale.Checked || !cbSexMale.Checked || !cbSexVoid.Checked)
                f["sx"] = String.Format("{0:s}{1:s}{2:s}", cbSexMale.Checked ? "m" : "", cbSexFemale.Checked ? "f" : "", cbSexVoid.Checked ? "v" : "");
            if (f.safeValue("sx") == "") f.Remove("sx");
            if (cbDateFrom.Checked) f["dt"] = nudDateFrom.Value.ToString();
            if (cbDateTo.Checked) f["Dt"] = nudDateTo.Value.ToString();
            if (cbWeightFrom.Checked) f["wg"] = nudWeightFrom.Value.ToString();
            if (cbWeightTo.Checked) f["Wg"] = nudWeightTo.Value.ToString();
            if (cobWorks.SelectedIndex != 0) f["wr"] = cobWorks.SelectedIndex.ToString();
            if (!cbMaleBoy.Checked || !cbMaleCandidate.Checked || !cbMaleProducer.Checked)
                f["mt"] = String.Format("{0:s}{1:s}{2:s}", cbMaleBoy.Checked ? "b" : "", cbMaleCandidate.Checked ? "c" : "", cbMaleProducer.Checked ? "p" : "");
            if (f.safeValue("mt") == "") f.Remove("mt");
            if (!cbFemaleGirl.Checked || !cbFemaleBride.Checked || !cbFemaleFirst.Checked || !cbFemaleState.Checked)
                f["ft"] = String.Format("{0:s}{1:s}{2:s}{3:s}", cbFemaleGirl.Checked ? "g" : "", cbFemaleBride.Checked ? "b" : "", cbFemaleFirst.Checked ? "f" : "", cbFemaleState.Checked ? "s" : "");
            if (f.safeValue("ft") == "") f.Remove("ft");
            if (cobMaleState.SelectedIndex != 0)
                f["ms"] = cobMaleState.SelectedIndex.ToString();
            if (cobFemaleState.SelectedIndex != 0)
                f["fs"] = cobFemaleState.SelectedIndex.ToString();
            if (cobPregnant.SelectedIndex != 0)
                f["pr"] = cobPregnant.SelectedIndex.ToString();
            if (cobFamily.SelectedIndex != 0)
                f["fm"] = cobPregnant.SelectedIndex.ToString();
            if (cobKuk.SelectedIndex != 0)
                f["ku"] = cobPregnant.SelectedIndex.ToString();
            if (cbPregFrom.Checked)
                f["pf"] = nudPregFrom.Value.ToString();
            if (cbPregTo.Checked)
                f["Pf"] = nudPregTo.Value.ToString();
            if (tbName.Text != "")
                f["nm"] = tbName.Text;
            return f;
        }

        public override void setFilters(Filters f)
        {
            clearFilters();
            cbSexMale.Checked = f.safeValue("sx", "mfv").Contains("m");
            cbSexFemale.Checked = f.safeValue("sx", "mfv").Contains("f");
            cbSexVoid.Checked = f.safeValue("sx", "mfv").Contains("v");
            cbDateFrom.Checked = f.ContainsKey("dt"); cbDateFrom_CheckedChanged(null, null);
            if (cbDateFrom.Checked)
            { nudDateFrom.Value = f.safeInt("dt", 200); nudDateFrom_ValueChanged(null, null); }
            cbDateTo.Checked = f.ContainsKey("Dt"); cbDateTo_CheckedChanged(null, null);
            if (cbDateTo.Checked)
            { nudDateTo.Value = f.safeInt("Dt", 100); nudDateTo_ValueChanged(null, null); }
            cbWeightFrom.Checked = f.ContainsKey("wg"); cbWeightFrom_CheckedChanged(null, null);
            if (cbWeightFrom.Checked) nudWeightFrom.Value = f.safeInt("wg", 1000);
            cbWeightTo.Checked = f.ContainsKey("Wg"); cbWeightTo_CheckedChanged(null, null);
            if (cbWeightTo.Checked) nudWeightTo.Value = f.safeInt("Wg", 5000);
            cobWorks.SelectedIndex = f.safeInt("wr");
            cbMaleBoy.Checked = f.safeValue("mt", "bcp").Contains("b");
            cbMaleCandidate.Checked = f.safeValue("mt", "bcp").Contains("c");
            cbMaleProducer.Checked = f.safeValue("mt", "bcp").Contains("p");
            cbFemaleGirl.Checked = f.safeValue("ft", "gbfs").Contains("g");
            cbFemaleBride.Checked = f.safeValue("ft", "gbfs").Contains("b");
            cbFemaleFirst.Checked = f.safeValue("ft", "gbfs").Contains("f");
            cbFemaleState.Checked = f.safeValue("ft", "gbfs").Contains("s");
            cobMaleState.SelectedIndex = f.safeInt("ms");
            cobFemaleState.SelectedIndex = f.safeInt("fs");
            cobPregnant.SelectedIndex = f.safeInt("pr");
            cobFamily.SelectedIndex = f.safeInt("fm");
            cobKuk.SelectedIndex = f.safeInt("ku");
            cbPregFrom.Checked = f.ContainsKey("pf"); cbPregFrom_CheckedChanged(null, null);
            cbPregTo.Checked = f.ContainsKey("Pf"); cbPregTo_CheckedChanged(null, null);
            if (cbPregFrom.Checked)
            { nudPregFrom.Value = f.safeInt("pf", 200); nudPregFrom_ValueChanged(null, null); }
            if (cbPregTo.Checked)
            { nudPregTo.Value = f.safeInt("Pf", 100); nudPregTo_ValueChanged(null, null); }
            tbName.Text = f.safeValue("nm");
        }
        public override void clearFilters()
        {
            cbSexFemale.Checked = cbSexMale.Checked = cbSexVoid.Checked = true;
            cbDateFrom.Checked = cbDateTo.Checked = false;
            cbDateFrom_CheckedChanged(null, null); cbDateTo_CheckedChanged(null, null);
            cbWeightFrom.Checked = cbWeightTo.Checked = false;
            cbWeightFrom_CheckedChanged(null, null); cbWeightTo_CheckedChanged(null, null);
            cobWorks.SelectedIndex = 0;
            cbMaleBoy.Checked = cbMaleCandidate.Checked = cbMaleProducer.Checked = true;
            cobMaleState.SelectedIndex = cobFemaleState.SelectedIndex = cobFamily.SelectedIndex = 0;
            cobKuk.SelectedIndex = 0; cobPregnant.SelectedIndex = 0;
            cbPregFrom.Checked = cbPregTo.Checked = false;
            cbPregTo_CheckedChanged(null, null); cbPregFrom_CheckedChanged(null, null);
            cbFemaleBride.Checked = cbFemaleFirst.Checked = cbFemaleGirl.Checked = cbFemaleState.Checked = true;
            cbFilter.Text = "";
            tbName.Text = "";
        }
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
                cbPregTo.Checked = dtpPregTo.Enabled = nudPregTo.Enabled = false;
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
