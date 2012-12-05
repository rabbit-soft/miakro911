using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using gamlib;

namespace rabnet.forms
{
    public partial class PeriodForm : Form
    {
        //public enum enumReportType { DeadReasons, Deads,Fucks };
        public static XmlDocument nullDocument = new XmlDocument();
        public static XmlElement nullElem = nullDocument.CreateElement("none");

        public readonly myReportType ReportType = myReportType.TEST;
        
        private PeriodForm()
        {
            InitializeComponent();
        }

        public PeriodForm(string caption) : this()
        {
            lbReportName.Text = caption;
            fillDates();
            rbDay_CheckedChanged(null, null);
        }

        public PeriodForm(myReportType type) : this()
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

        /// <summary>
        /// За какой период показывать отчет (Год,месяц,Год)
        /// </summary>
        public myDatePeriod PeriodType
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

        /// <summary>
        /// Ограничить выбора периода. Битовая маска ymd=7
        /// </summary>
        public int PeriodConstrain
        {
            get
            {
                int result = 0;
                result += rbDay.Enabled ? 1 : 0;
                result += rbMonth.Enabled ? 2 : 0;
                result += rbYear.Enabled ? 4 : 0;
                return result;
            }
            set
            {
                rbYear.Enabled = rbYear.Checked = cbYear.Enabled = (value & 4) > 0;
                rbMonth.Enabled = rbMonth.Checked = cbMonth.Enabled = (value & 2) > 0;
                rbDay.Enabled = rbDay.Checked = dtpDay.Enabled =(value & 1) > 0;                     
            }
        }

        public string PeriodChar
        {
            get
            {
                switch (PeriodType)
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
                switch (PeriodType)
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
                    cbMonth.Items.Add(Helper.toRusMonth(vals[0]) + vals[1]);
                    if (!cbYear.Items.Contains(vals[1]))
                        cbYear.Items.Add(vals[1]);
                }
                cbMonth.SelectedIndex = 0;
                rbDay_CheckedChanged(null, null);
            }
            else
            {
                if(ReportType == myReportType.FUCKS_BY_DATE)
                    MessageBox.Show("Нет дат случек.");
                else MessageBox.Show("Нет дат забоев.");
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        public DateTime MaxDate { get { return dtpDay.MaxDate; } }
        public DateTime MinDate { get { return dtpDay.MinDate; } }

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
                if (cbMonth.Items.Count>0)
                    cbMonth.SelectedIndex = 0;
            }
            else if (rbYear.Checked)
            {
                cbYear.Visible = true;
                cbYear.SelectedIndex = 0;
            }
        }

        private void PeriodForm_Load(object sender, EventArgs e)
        {
            fillDates();
        }

    }
}
