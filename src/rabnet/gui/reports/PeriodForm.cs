using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using X_Tools;

namespace rabnet
{
    public partial class PeriodForm : Form
    {
        public enum enumReportType { DeadReasons, Deads,Fucks };
        public static XmlDocument nullDocument = new XmlDocument();
        public static XmlElement nullElem = nullDocument.CreateElement("none");

        public readonly enumReportType ReportType;

        public myDatePeriod Period
        {
            get
            {
                if (rbDay.Checked)
                {
                    return myDatePeriod.Day;
                }
                else if (rbYear.Checked)
                {
                    return myDatePeriod.Year;
                }
                else return myDatePeriod.Month;        
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
                    case myDatePeriod.Year:
                        return "y";
                    default:
                        return "m";
                }
            }
        }

        public string DateValue
        {
            get
            {
                switch (Period)
                {
                    case myDatePeriod.Day:
                        return dtpDay.Value.Date.ToShortDateString();
                    case myDatePeriod.Month:
                        return cbMonth.Text;
                    case myDatePeriod.Year:
                        return cbYear.Text;
                    default: return cbMonth.Text;
                }
            }
        }

        public PeriodForm(enumReportType type)
        {
            InitializeComponent();
            switch (type)
            {
                case enumReportType.Deads:
                    lbReportName.Text = "Списания";
                    break;
                case enumReportType.DeadReasons:
                    lbReportName.Text = "Причины списаний";
                    break;
                case enumReportType.Fucks:
                    lbReportName.Text = "Список случек и вязок";
                    break;
            }
            this.ReportType = type;
            fillDates();
            rbDay_CheckedChanged(null, null);
        }

        private void fillDates()
        {
            cbMonth.Items.Clear();
            cbYear.Items.Clear();
            List<String> dates = null;
            if(ReportType != enumReportType.Fucks)
                dates = Engine.get().db().getDeadsMonths();
            else dates =Engine.get().db().getFuckMonths();
            dtpDay.MaxDate = DateTime.Parse(dates[0]).AddMonths(1);
            dtpDay.MinDate = DateTime.Parse(dates[dates.Count-1]);
            if (dates.Count > 0)
            {
                foreach (String dt in dates)
                {
                    string[] vals = dt.Split('.');
                    cbMonth.Items.Add(XTools.toRusMonth(vals[0]) + vals[1]);
                    if (!cbYear.Items.Contains(vals[1]))
                        cbYear.Items.Add(vals[1]);
                }
                cbMonth.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Нет дат забоев.");
                this.Close();
            }
        }

        /// <summary>
        /// Происходит когда изменяют выбранный RadioButton
        /// </summary>
        private void rbDay_CheckedChanged(object sender, EventArgs e)
        {
            if (dtpDay.MaxDate >= DateTime.Now.Date)
                dtpDay.Value = DateTime.Now.Date;         
            else dtpDay.Value = dtpDay.MaxDate;
            dtpDay.Visible = cbMonth.Visible = cbYear.Visible = false;
            cbMonth.SelectedIndex = cbYear.SelectedIndex = -1;
            if (rbDay.Checked)
            {
                dtpDay.Visible = true;
            }
            else if (rbMonth.Checked)
            {
                cbMonth.Visible = true;
                cbMonth.SelectedIndex = 0;
            }
            else if (rbYear.Checked)
            {
                cbYear.Visible = true;
                cbYear.SelectedIndex = 0;
            }
        }

        public XmlDocument getXml()
        {
            /*XmlDocument doc = new XmlDocument();
            XmlElement rows = doc.CreateElement("Rows");
            doc.AppendChild(rows);
            XmlElement row = doc.CreateElement("Row");
            rows.AppendChild(row);
            row.AppendChild(doc.CreateElement("datefrom")).AppendChild(doc.CreateTextNode(DateValue));
            row.AppendChild(doc.CreateElement("dateto")).AppendChild(doc.CreateTextNode(""));
            return doc;*/
            XmlDocument doc = new XmlDocument();
            XmlElement row = doc.CreateElement("Row");
            doc.AppendChild(doc.CreateElement("Rows")).AppendChild(row);
            row.AppendChild(doc.CreateElement("datefrom")).AppendChild(doc.CreateTextNode(DateValue));
            row.AppendChild(doc.CreateElement("dateto")).AppendChild(doc.CreateTextNode(""));
            return doc;
        }
    }
}
