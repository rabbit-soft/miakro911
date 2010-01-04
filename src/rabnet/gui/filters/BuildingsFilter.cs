using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class BuildingsFilter : FilterPanel
    {
        public BuildingsFilter(RabStatusBar sb): base(sb, "buildings", Options.OPT_ID.BUILD_FILTER)
        {
            //InitializeComponent();
        }

        public BuildingsFilter() : base() { }

        public override Filters getFilters()
        {
            Filters f = new Filters();
            if (cbFarm.SelectedIndex != 0) f["frm"] = cbFarm.SelectedIndex.ToString();
            if (cbGnezdo.SelectedIndex != 0) f["gnzd"] = cbGnezdo.SelectedIndex.ToString();
            if (cbGrelka.SelectedIndex != 0) f["grlk"] = cbGrelka.SelectedIndex.ToString();
            if (vertepBox.Checked || urtaBox.Checked || kvartaBox.Checked || barinBox.Checked || krolBox.Checked || dvukrolBox.Checked || komplexBox.Checked || hizhinaBox.Checked)
            {
                f["yar"] = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", vertepBox.Checked ? "v" : "",
                    urtaBox.Checked ? "u" : "", kvartaBox.Checked ? "q" : "", barinBox.Checked ? "b" : "", krolBox.Checked ? "k" : "", dvukrolBox.Checked ? "d" : "", komplexBox.Checked ? "x" : "", hizhinaBox.Checked ? "h" : "");
            }
            return f;
        }

        public override void setFilters(Filters f)
        {
            clearFilters();
            cbFarm.SelectedIndex = f.safeInt("frm");
            cbGnezdo.SelectedIndex = f.safeInt("gnzd");
            cbGrelka.SelectedIndex = f.safeInt("grlk");
            vertepBox.Checked = f.safeValue("yar", "vuqbkdxh").Contains("v");
            urtaBox.Checked= f.safeValue("yar", "vuqbkdxh").Contains("u");
            kvartaBox.Checked= f.safeValue("yar", "vuqbkdxh").Contains("q");
            barinBox.Checked= f.safeValue("yar", "vuqbkdxh").Contains("b");
            krolBox.Checked= f.safeValue("yar", "vuqbkdxh").Contains("k");
            dvukrolBox.Checked= f.safeValue("yar", "vuqbkdxh").Contains("d");
            komplexBox.Checked= f.safeValue("yar", "vuqbkdxh").Contains("x");
            hizhinaBox.Checked = f.safeValue("yar", "vuqbkdxh").Contains("h");
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

    }
}
