using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using gamlib;

namespace rabnet.forms
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
                    cbMonth.Items.Add(Helper.toRusMonth(vals[0]) + vals[1]);
                    if (!cbYear.Items.Contains(vals[1]))
                        cbYear.Items.Add(vals[1]);
                }
                cbMonth.SelectedIndex = cbYear.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Нет дат забоя.");
                this.Close();
            }
        }

        private void rbMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMonth.Checked)
            {
                cbMonth.Visible = true;
                cbYear.Visible = false;
            }
            else
            {
                cbYear.Visible = true;
                cbMonth.Visible = false;
            }
            
        }

        internal System.Xml.XmlDocument getXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement row = doc.CreateElement("Row");
            doc.AppendChild(doc.CreateElement("Rows")).AppendChild(row);
            row.AppendChild(doc.CreateElement("period")).AppendChild(doc.CreateTextNode(this.DateValue));
            return doc;
        }

        private void ButcherReportDate_Load(object sender, EventArgs e)
        {
            fillButcherDates();
        }
    }
}
