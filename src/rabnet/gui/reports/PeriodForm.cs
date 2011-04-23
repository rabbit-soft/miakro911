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
        //public enum enumReportType { DeadReasons, Deads,Fucks };
        public static XmlDocument nullDocument = new XmlDocument();
        public static XmlElement nullElem = nullDocument.CreateElement("none");

        public readonly myReportType ReportType = myReportType.TEST;

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

        public PeriodForm()
        {
            InitializeComponent();
        }

        public PeriodForm(myReportType type):this()
        {
            
            switch (type)
            {
                case myReportType.DEAD:
                    lbReportName.Text = "Списания";
                    break;
                case myReportType.DEADREASONS:
                    lbReportName.Text = "Причины списаний";
                    break;
                case myReportType.FUCKS_BY_DATE:
                    lbReportName.Text = "Список случек и вязок";
                    break;
            }
            this.ReportType = type;        
        }

        public PeriodForm(string caption):this()
        {
            lbReportName.Text = caption;
            fillDates();
            rbDay_CheckedChanged(null, null);
        }

        private void fillDates()
        {
            cbMonth.Items.Clear();
            cbYear.Items.Clear();

            List<String> dates = null;
            if(ReportType == myReportType.FUCKS_BY_DATE)                
                dates =Engine.get().db().getFuckMonths();
            else dates = Engine.get().db().getDeadsMonths();


            if (dates.Count > 0)
            {
                dtpDay.MaxDate = DateTime.Parse(dates[0]).AddMonths(1);
                dtpDay.MinDate = DateTime.Parse(dates[dates.Count - 1]);
                foreach (String dt in dates)
                {
                    string[] vals = dt.Split('.');
                    cbMonth.Items.Add(XTools.toRusMonth(vals[0]) + vals[1]);
                    if (!cbYear.Items.Contains(vals[1]))
                        cbYear.Items.Add(vals[1]);
                }
                cbMonth.SelectedIndex = 0;
                rbDay_CheckedChanged(null, null);
            }
            else
            {
                MessageBox.Show("Нет дат забоев.");
                this.DialogResult = DialogResult.Cancel;
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

        private void PeriodForm_Shown(object sender, EventArgs e)
        {
            fillDates();
        }
    }
}
