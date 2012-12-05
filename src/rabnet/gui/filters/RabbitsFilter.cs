using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using rabnet.components;

namespace rabnet.filters
{
    public partial class RabbitsFilter : FilterPanel
    {
        public RabbitsFilter(RabStatusBar sb):base(sb,"rabbits",Options.OPT_ID.RAB_FILTER)
        {
            //InitializeComponent();
        }
        public RabbitsFilter() : base() {}

        #region filter_form_process

        private void fillBreeds()
        {
            ICatalogs cts = Engine.db().catalogs();
            Catalog breeds = cts.getBreeds();
            cobBreeds.Items.Clear();
            cobBreeds.Items.Add("--ВСЕ--");
            cobBreeds.SelectedIndex = 0;
            foreach (int k in breeds.Keys)
            {
                cobBreeds.Items.Add(breeds[k]);
            }
        }

        public override Filters getFilters()
        {
            Filters f = new Filters();
            if (!cbSexFemale.Checked || !cbSexMale.Checked || !cbSexVoid.Checked)
                f["sx"] = String.Format("{0:s}{1:s}{2:s}", cbSexMale.Checked ? "m" : "", cbSexFemale.Checked ? "f" : "", cbSexVoid.Checked ? "v" : "");
            if (f.safeValue("sx") == "") f.Remove("sx");
            if (cbDateTo.Checked) f["dt"] = nudDateTo.Value.ToString();
            if (cbDateFrom.Checked) f["Dt"] = nudDateFrom.Value.ToString();
            if (cbWeightFrom.Checked) f["wg"] = nudWeightFrom.Value.ToString();
            if (cbWeightTo.Checked) f["Wg"] = nudWeightTo.Value.ToString();
            //if (cbWorks.SelectedIndex != 0) f["wr"] = cbWorks.SelectedIndex.ToString();
            if (!cbMaleBoy.Checked || !cbMaleCandidate.Checked || !cbMaleProducer.Checked)
                f[Filters.MALE] = String.Format("{0:s}{1:s}{2:s}", cbMaleBoy.Checked ? "b" : "", cbMaleCandidate.Checked ? "c" : "", cbMaleProducer.Checked ? "p" : "");
            if (f.safeValue(Filters.MALE) == "") f.Remove(Filters.MALE);
            if (!cbFemaleGirl.Checked || !cbFemaleBride.Checked || !cbFemaleFirst.Checked || !cbFemaleState.Checked)
                f[Filters.FEMALE] = String.Format("{0:s}{1:s}{2:s}{3:s}", cbFemaleGirl.Checked ? "g" : "", cbFemaleBride.Checked ? "b" : "", cbFemaleFirst.Checked ? "f" : "", cbFemaleState.Checked ? "s" : "");
            if (f.safeValue(Filters.FEMALE) == "")
                f.Remove(Filters.FEMALE);
            if (cobPregnant.SelectedIndex != 0)
                f["pr"] = cobPregnant.SelectedIndex.ToString();

            if (cbPregFrom.Checked)
                f["pf"] = nudPregFrom.Value.ToString();
            if (cbPregTo.Checked)
                f["Pf"] = nudPregTo.Value.ToString();
            if (tbName.Text != "")
                f["nm"] = tbName.Text;
            if (cobBreeds.SelectedIndex != 0)
                f["br"] = cobBreeds.SelectedIndex.ToString();
            return f;
        }

        public override void setFilters(Filters f)
        {
            clearFilters();
            cbSexMale.Checked = f.safeValue("sx", "mfv").Contains("m");
            cbSexFemale.Checked = f.safeValue("sx", "mfv").Contains("f");
            cbSexVoid.Checked = f.safeValue("sx", "mfv").Contains("v");
            cbDateTo.Checked = f.ContainsKey("dt"); cbDateFrom_CheckedChanged(null, null);
            if (cbDateTo.Checked)
            { 
                nudDateTo.Value = f.safeInt("dt", 100); 
                nudDateFrom_ValueChanged(null, null);
            }
            cbDateFrom.Checked = f.ContainsKey("Dt"); cbDateTo_CheckedChanged(null, null);
            if (cbDateFrom.Checked)
            { 
                nudDateFrom.Value = f.safeInt("Dt", 60); 
                nudDateTo_ValueChanged(null, null); 
            }
            cbWeightFrom.Checked = f.ContainsKey("wg"); cbWeightFrom_CheckedChanged(null, null);
            if (cbWeightFrom.Checked) 
                nudWeightFrom.Value = f.safeInt("wg", 1000);
            cbWeightTo.Checked = f.ContainsKey("Wg"); cbWeightTo_CheckedChanged(null, null);
            if (cbWeightTo.Checked) 
                nudWeightTo.Value = f.safeInt("Wg", 5000);
            //cbWorks.SelectedIndex = f.safeInt("wr");

            cbMaleBoy.Checked = f.safeValue(Filters.MALE, "bcp").Contains("b");
            cbMaleCandidate.Checked = f.safeValue(Filters.MALE, "bcp").Contains("c");
            cbMaleProducer.Checked = f.safeValue(Filters.MALE, "bcp").Contains("p");

            cbFemaleGirl.Checked = f.safeValue(Filters.FEMALE, "gbfs").Contains("g");
            cbFemaleBride.Checked = f.safeValue(Filters.FEMALE, "gbfs").Contains("b");
            cbFemaleFirst.Checked = f.safeValue(Filters.FEMALE, "gbfs").Contains("f");
            cbFemaleState.Checked = f.safeValue(Filters.FEMALE, "gbfs").Contains("s");         
            cobPregnant.SelectedIndex = f.safeInt("pr");
            cbPregFrom.Checked = f.ContainsKey("pf"); 
            cbPregFrom_CheckedChanged(null, null);
            cbPregTo.Checked = f.ContainsKey("Pf"); 
            cbPregTo_CheckedChanged(null, null);
            if (cbPregFrom.Checked)
            { 
                nudPregFrom.Value = f.safeInt("pf", 10); 
                nudPregFrom_ValueChanged(null, null);
            }
            if (cbPregTo.Checked)
            { 
                nudPregTo.Value = f.safeInt("Pf", 20); 
                nudPregTo_ValueChanged(null, null); 
            }
            tbName.Text = f.safeValue("nm");
            cobBreeds.SelectedIndex = f.safeInt("br",0);
        }
        public override void clearFilters()
        {
            fillBreeds();
            cbSexFemale.Checked = cbSexMale.Checked = cbSexVoid.Checked = true;
            cbDateTo.Checked = cbDateFrom.Checked = false;
            cbDateFrom_CheckedChanged(null, null); 
            cbDateTo_CheckedChanged(null, null);
            cbWeightFrom.Checked = cbWeightTo.Checked = false;
            cbWeightFrom_CheckedChanged(null, null); cbWeightTo_CheckedChanged(null, null);
            //cbWorks.SelectedIndex = 0;
            cbMaleBoy.Checked = cbMaleCandidate.Checked = cbMaleProducer.Checked = true;          
            cobPregnant.SelectedIndex = 0;
            cbPregFrom.Checked = cbPregTo.Checked = false;
            cbPregTo_CheckedChanged(null, null); cbPregFrom_CheckedChanged(null, null);
            cbFemaleBride.Checked = cbFemaleFirst.Checked = cbFemaleGirl.Checked = cbFemaleState.Checked = true;
            cbFilter.Text = "";
            tbName.Text = "";
            cobBreeds.SelectedIndex = 0;
        }
        

        private void cbWeightFrom_CheckedChanged(object sender, EventArgs e)
        {
            nudWeightFrom.Enabled = cbWeightFrom.Checked;
            if (!cbWeightFrom.Checked)
            {
                nudWeightTo.Minimum = 100;
                return;
            }
            if (cbWeightTo.Checked)
            {
                if (nudWeightFrom.Value > nudWeightTo.Value)
                    nudWeightFrom.Value = nudWeightTo.Value;
                nudWeightTo.Minimum = nudWeightFrom.Value;
                nudWeightFrom.Maximum = nudWeightTo.Value;
            }
        }
        private void cbWeightTo_CheckedChanged(object sender, EventArgs e)
        {
            nudWeightTo.Enabled = cbWeightTo.Checked;
            if (!cbWeightTo.Checked)
            {
                nudWeightFrom.Maximum = 20000;
                return;
            }
            if (cbWeightFrom.Checked)
            {
                if (nudWeightTo.Value < nudWeightFrom.Value)
                    nudWeightTo.Value = nudWeightFrom.Value;
                nudWeightFrom.Maximum = nudWeightTo.Value;
                nudWeightTo.Minimum = nudWeightFrom.Value;
            }
            
        }
        private void nudWeightTo_ValueChanged(object sender, EventArgs e)
        {
            if (cbWeightFrom.Checked)
                nudWeightFrom.Maximum = nudWeightTo.Value;
            else
                nudWeightTo.Minimum = 100;
        }
        private void nudWeightFrom_ValueChanged(object sender, EventArgs e)
        {
            if (cbWeightTo.Checked)
                nudWeightTo.Minimum = nudWeightFrom.Value;
            else
                nudWeightTo.Maximum = 20000;
        }

        private void cbDateTo_CheckedChanged(object sender, EventArgs e)
        {
            dtpDateTo.Enabled = nudDateTo.Enabled = cbDateTo.Checked;
            dtpDateTo.Value = DateTime.Today.AddDays(-(double)nudDateTo.Value);

            if (!cbDateTo.Checked)
            {
                nudDateFrom.Maximum = 5000;
                return;
            }
            if (cbDateFrom.Checked)
            {
                if (nudDateTo.Value < nudDateFrom.Value)nudDateTo.Value = nudDateFrom.Value;               
                nudDateFrom.Maximum = nudDateTo.Value;
                nudDateTo.Minimum = nudDateFrom.Value;
            }
        }
        private void cbDateFrom_CheckedChanged(object sender, EventArgs e)
        {
            dtpDateFrom.Enabled = nudDateFrom.Enabled = cbDateFrom.Checked;
            dtpDateFrom.Value = DateTime.Now.AddDays(-(double)nudDateFrom.Value).Date;
            if (!cbDateFrom.Checked)
            {
                nudDateTo.Minimum = 60;
                return;
            }
            if (cbDateTo.Checked)
            {
                if (nudDateFrom.Value > nudDateTo.Value)
                    nudDateFrom.Value = nudDateTo.Value;
                nudDateTo.Minimum = nudDateFrom.Value;
                nudDateFrom.Maximum = nudDateTo.Value;
            }
        }
        private void dtpDateTo_ValueChanged(object sender, EventArgs e)
        {
            int number;
            number = (DateTime.Today - dtpDateTo.Value).Days;
            if (number < nudDateTo.Minimum)
            {
                nudDateTo.Value = nudDateTo.Minimum;
                dtpDateTo.Value = DateTime.Today.Subtract(new TimeSpan((int)nudDateTo.Minimum, 0, 0, 0));
            }
            else if (number > nudDateTo.Maximum)
            {
                nudDateTo.Value = nudDateTo.Maximum;
                dtpDateTo.Value = DateTime.Today.Subtract(new TimeSpan((int)nudDateTo.Maximum, 0, 0, 0));
            }
            else
            {
                nudDateTo.Value = number;
            } 
        }
        private void dtpDateFrom_ValueChanged(object sender, EventArgs e)
        {
                int number;
                number = (DateTime.Today - dtpDateFrom.Value).Days;
                if (number < nudDateFrom.Minimum)
                {
                    nudDateFrom.Value = nudDateFrom.Minimum;
                    dtpDateFrom.Value = DateTime.Today.Subtract(new TimeSpan((int)nudDateFrom.Minimum, 0, 0, 0));
                }
                else if (number > nudDateFrom.Maximum)
                {
                    nudDateFrom.Value = nudDateFrom.Maximum;
                    dtpDateFrom.Value = DateTime.Today.Subtract(new TimeSpan((int)nudDateFrom.Maximum, 0, 0, 0));
                }
                else
                {
                    nudDateFrom.Value = number;
                }            
        }
        private void nudDateFrom_ValueChanged(object sender, EventArgs e)
        {
            dtpDateFrom.Value = DateTime.Today.Subtract(new TimeSpan((int)nudDateFrom.Value, 0, 0, 0));
            if (cbDateTo.Checked)
                nudDateTo.Minimum = nudDateFrom.Value;
            else nudDateFrom.Maximum = 5000;

        }
        private void nudDateTo_ValueChanged(object sender, EventArgs e)
        {
            dtpDateTo.Value = DateTime.Today.Subtract(new TimeSpan((int)nudDateTo.Value, 0, 0, 0));
            if (cbDateFrom.Checked)
                nudDateFrom.Maximum = nudDateTo.Value;
            else nudDateTo.Minimum = 60;

        }

        private void cbPregFrom_CheckedChanged(object sender, EventArgs e)
        {
            dtpPregFrom.Enabled = nudPregFrom.Enabled = cbPregFrom.Checked;
            dtpPregFrom.Value = DateTime.Today.AddDays(-(double)nudPregFrom.Value);
            if (!cbPregFrom.Checked)
            {
                nudPregTo.Minimum = 0;
                if (!cbPregTo.Checked) cobPregnant.SelectedIndex = 0;
                return;
            }
            if (cbPregTo.Checked)
            {
                cobPregnant.SelectedIndex = 2;
                if (nudPregFrom.Value > nudPregTo.Value)
                    nudPregFrom.Value = nudPregTo.Value;
                nudPregTo.Minimum = nudPregFrom.Value;
                nudPregFrom.Maximum = nudPregTo.Value;
            }
        }
        private void cbPregTo_CheckedChanged(object sender, EventArgs e)
        {
            dtpPregTo.Enabled = nudPregTo.Enabled = cbPregTo.Checked;
            dtpPregTo.Value = DateTime.Today.AddDays(-(double)nudPregTo.Value);
            if (!cbPregTo.Checked)
            {
                nudPregFrom.Maximum = 40;
                if (!cbPregFrom.Checked) cobPregnant.SelectedIndex = 0;
                return;
            }
            if (cbPregFrom.Checked)
            {
                cobPregnant.SelectedIndex = 2;
                if (nudPregTo.Value < nudPregFrom.Value)
                    nudPregTo.Value = nudPregFrom.Value;
                nudPregFrom.Maximum = nudPregTo.Value;
                nudPregTo.Minimum = nudPregFrom.Value;
            }
        }
        private void nudPregFrom_ValueChanged(object sender, EventArgs e)
        {
            dtpPregFrom.Value = DateTime.Today.Subtract(new TimeSpan((int)nudPregFrom.Value, 0, 0, 0));
            if (cbPregTo.Checked)
                nudPregTo.Minimum = nudPregFrom.Value;
            else
                nudPregTo.Maximum = 40;            
        }
        private void nudPregTo_ValueChanged(object sender, EventArgs e)
        {
            dtpPregTo.Value = DateTime.Today.Subtract(new TimeSpan((int)nudPregTo.Value, 0, 0, 0));
            if (cbPregFrom.Checked)
                nudPregFrom.Maximum = nudPregTo.Value;
            else
                nudPregTo.Minimum = 0;
        }       
        private void dtpPregFrom_ValueChanged(object sender, EventArgs e)
        {
            int number;
            number = (DateTime.Today - dtpPregFrom.Value).Days;
            if (number < nudPregFrom.Minimum)
            {
                nudPregFrom.Value = nudPregFrom.Minimum;
                dtpPregFrom.Value = DateTime.Today.Subtract(new TimeSpan((int)nudPregFrom.Minimum, 0, 0, 0));
            }
            else if (number > nudPregFrom.Maximum)
            {
                nudPregFrom.Value = nudPregFrom.Maximum;
                dtpPregFrom.Value = DateTime.Today.Subtract(new TimeSpan((int)nudPregFrom.Maximum, 0, 0, 0));
            }
            else
            {
                nudPregFrom.Value = number;
            } 
        }
        private void dtpPregTo_ValueChanged(object sender, EventArgs e)
        {
            int number;
            number = (DateTime.Today - dtpPregTo.Value).Days;
            if (number < nudPregTo.Minimum)
            {
                nudPregTo.Value = nudPregTo.Minimum;
                dtpPregTo.Value = DateTime.Today.Subtract(new TimeSpan((int)nudPregTo.Minimum, 0, 0, 0));
            }
            else if (number > nudPregTo.Maximum)
            {
                nudPregTo.Value = nudPregTo.Maximum;
                dtpPregTo.Value = DateTime.Today.Subtract(new TimeSpan((int)nudPregTo.Maximum, 0, 0, 0));
            }
            else
            {
                nudPregTo.Value = number;
            } 
        }
        private void cobPregnant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cobPregnant.SelectedIndex != 2)
            {
                cbPregTo.Checked = dtpPregTo.Enabled = nudPregTo.Enabled = false;
                cbPregFrom.Checked = dtpPregFrom.Enabled = nudPregFrom.Enabled = false;
            }
        }

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
        }

        #region combobox_checked

        private void cbSexFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSexFemale.CheckState == CheckState.Unchecked & cbSexMale.CheckState == CheckState.Unchecked & cbSexVoid.CheckState == CheckState.Unchecked)
            {
                cbSexFemale.CheckState = CheckState.Checked;
                return;
            }
            if (cbSexFemale.CheckState == CheckState.Unchecked) groupBox6.Enabled = false;
            else groupBox6.Enabled = true;
        }

        private void cbSexMale_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSexFemale.CheckState == CheckState.Unchecked & cbSexMale.CheckState == CheckState.Unchecked & cbSexVoid.CheckState == CheckState.Unchecked)
            {
                cbSexMale.CheckState = CheckState.Checked;
                return;
            }
            if (cbSexMale.CheckState == CheckState.Unchecked) groupBox5.Enabled = false;
            else groupBox5.Enabled = true;

        }

        private void cbSexVoid_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSexFemale.CheckState == CheckState.Unchecked & cbSexMale.CheckState == CheckState.Unchecked & cbSexVoid.CheckState == CheckState.Unchecked)
            {
                cbSexVoid.CheckState = CheckState.Checked;
                return;
            }
        }     

        private void cbFemaleGirl_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbFemaleBride.Checked & !cbFemaleFirst.Checked & !cbFemaleGirl.Checked & !cbFemaleState.Checked)
                cbFemaleGirl.Checked = true;
        }
        private void cbFemaleBride_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbFemaleBride.Checked & !cbFemaleFirst.Checked & !cbFemaleGirl.Checked & !cbFemaleState.Checked)
                cbFemaleBride.Checked = true;
        }
        private void cbFemaleFirst_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbFemaleBride.Checked & !cbFemaleFirst.Checked & !cbFemaleGirl.Checked & !cbFemaleState.Checked)
                cbFemaleFirst.Checked = true;
        }
        private void cbFemaleState_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbFemaleBride.Checked & !cbFemaleFirst.Checked & !cbFemaleGirl.Checked & !cbFemaleState.Checked)
                cbFemaleState.Checked = true;
        }
 

        private void cbMaleBoy_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbMaleBoy.Checked & !cbMaleCandidate.Checked & !cbMaleProducer.Checked)
                cbMaleBoy.Checked = true;
        }
        private void cbMaleCandidate_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbMaleBoy.Checked & !cbMaleCandidate.Checked & !cbMaleProducer.Checked)
                cbMaleCandidate.Checked = true;
        }
        private void cbMaleProducer_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbMaleBoy.Checked & !cbMaleCandidate.Checked & !cbMaleProducer.Checked)
                cbMaleProducer.Checked = true;
        }

        #endregion

    }
}
