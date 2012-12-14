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
    public partial class BuildingsFilter : FilterPanel
    {
        const string BUILD_CHARS = "vuqbkdxh";

        public BuildingsFilter(): base("buildings", Options.OPT_ID.BUILD_FILTER)
        {
            //InitializeComponent();
        }

        public override Filters getFilters()
        {
            Filters f = new Filters();
            if (cbFarm.SelectedIndex != 0) f[Filters.FARM] = cbFarm.SelectedIndex.ToString();
            if (cbGnezdo.SelectedIndex != 0) f[Filters.NEST_IN] = cbGnezdo.SelectedIndex.ToString();
            if (cbGrelka.SelectedIndex != 0) f[Filters.HETER] = cbGrelka.SelectedIndex.ToString();
            if (vertepBox.Checked || urtaBox.Checked || kvartaBox.Checked || barinBox.Checked || krolBox.Checked || dvukrolBox.Checked || komplexBox.Checked || hizhinaBox.Checked)
            {
                f[Filters.TIER] = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", 
                    vertepBox.Checked ? "v" : "",
                    urtaBox.Checked ? "u" : "", 
                    kvartaBox.Checked ? "q" : "", 
                    barinBox.Checked ? "b" : "", 
                    krolBox.Checked ? "k" : "", 
                    dvukrolBox.Checked ? "d" : "", 
                    komplexBox.Checked ? "x" : "", 
                    hizhinaBox.Checked ? "h" : "");
                if (f[Filters.TIER] == BUILD_CHARS)
                    f.Remove(Filters.TIER);
            }
            return f;
        }

        public override void setFilters(Filters f)
        {
            clearFilters();
            cbFarm.SelectedIndex = f.safeInt(Filters.TIER);
            cbGnezdo.SelectedIndex = f.safeInt(Filters.NEST_IN);
            cbGrelka.SelectedIndex = f.safeInt(Filters.);
            vertepBox.Checked = f.safeValue(Filters.TIER, BUILD_CHARS).Contains("v");
            urtaBox.Checked = f.safeValue(Filters.TIER, BUILD_CHARS).Contains("u");
            kvartaBox.Checked = f.safeValue(Filters.TIER, BUILD_CHARS).Contains("q");
            barinBox.Checked = f.safeValue(Filters.TIER, BUILD_CHARS).Contains("b");
            krolBox.Checked = f.safeValue(Filters.TIER, BUILD_CHARS).Contains("k");
            dvukrolBox.Checked = f.safeValue(Filters.TIER, BUILD_CHARS).Contains("d");
            komplexBox.Checked = f.safeValue(Filters.TIER, BUILD_CHARS).Contains("x");
            hizhinaBox.Checked = f.safeValue(Filters.TIER, BUILD_CHARS).Contains("h");
        }
        public override void clearFilters()
        {
            cbFarm.SelectedIndex = 0;
            cbGnezdo.SelectedIndex = 0;
            cbGrelka.SelectedIndex = 0;
            saveBox.Text = "";
            setAllFarms();
        }

        private void setAllFarms()
        {
            if (vertepBox.Checked && urtaBox.Checked && kvartaBox.Checked && barinBox.Checked && krolBox.Checked && dvukrolBox.Checked && komplexBox.Checked && hizhinaBox.Checked)
            {
                vertepBox.Checked = urtaBox.Checked = kvartaBox.Checked = barinBox.Checked = krolBox.Checked = dvukrolBox.Checked = komplexBox.Checked = hizhinaBox.Checked = false;
            }
            else
            {
                vertepBox.Checked = urtaBox.Checked = kvartaBox.Checked = barinBox.Checked = krolBox.Checked = dvukrolBox.Checked = komplexBox.Checked = hizhinaBox.Checked = true;
            }   
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            setAllFarms();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
        }

    }
}
