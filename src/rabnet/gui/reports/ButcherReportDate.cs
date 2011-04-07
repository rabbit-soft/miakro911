using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using X_Tools;

namespace rabnet
{
    public enum myDatePeriod { Day,Month, Year }

    public partial class ButcherReportDate : Form
    {      

        public myDatePeriod Period
        {
            get
            {
                if (rbMonth.Checked)
                    return myDatePeriod.Month;
                else
                    return myDatePeriod.Year;
            }
        }

        public string PeriodChar
        {
            get
            {
                switch (Period)
                {
                    case myDatePeriod.Day:
                        return "d";
                    case myDatePeriod.Month:
                        return "m";
                    case myDatePeriod.Year:
                        return "y";
                    default: return "m";
                }
            }
        }

        public string DateValue
        {
            get
            {
                if (Period == myDatePeriod.Month)
                    return cbMonth.Text;
                else return cbYear.Text;
            }
        }

        public ButcherReportDate()
        {
            InitializeComponent();
            fillButcherDates();
        }

        private void fillButcherDates()
        {
            cbMonth.Items.Clear();
            cbYear.Items.Clear();
            List<String> dates = Engine.get().db().getButcherMonths();
            if (dates.Count > 0)
            {
                foreach (String dt in dates)
                {
                    string[] vals = dt.Split('.');
                    cbMonth.Items.Add(XTools.toRusMonth(vals[0]) + vals[1]);
                    if (!cbYear.Items.Contains(vals[1]))
                        cbYear.Items.Add(vals[1]);
                }
            }
            else
            {
                MessageBox.Show("Нет дат забоя.");
                this.Close();
            }
        }

        private void rbMonth_CheckedChanged(object sender, EventArgs e)
        {
            cbMonth.Enabled = cbYear.Enabled = false;
            cbMonth.SelectedIndex = cbYear.SelectedIndex = -1;
            if (rbMonth.Checked)
            {
                cbMonth.Enabled = true;
                if (cbMonth.Items.Count > 0)
                    cbMonth.SelectedIndex = 0;
            }
            else
            {
                cbYear.Enabled = true;
                if (cbYear.Items.Count > 0)
                    cbYear.SelectedIndex = 0;
            }
        }
    }
}
