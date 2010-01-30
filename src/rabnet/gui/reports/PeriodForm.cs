using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace rabnet
{
    public partial class PeriodForm : Form
    {
        public enum DType{DATE,DAY,MONTH,YEAR}
        public enum Preset { NONE,CUR_MONTH,PREV_MONTH}
        public static XmlDocument nullDocument = new XmlDocument();
        public static XmlElement nullElem = nullDocument.CreateElement("none");
        public DialogResult RunFilters(Filters f, ref XmlDocument xml, ref XmlElement row)
        {
            DialogResult res = ShowDialog();
            if (res == DialogResult.OK)
            {
                fillFilters(f);
                if (xml != nullDocument)
                    xml=getXml(ref row);
            }
            return res;
        }
        public static DialogResult Run(Filters f,ref XmlDocument xml,ref XmlElement row)
        {
            PeriodForm pf = Run();
            return pf.RunFilters(f,ref xml,ref row);
        }
        public static DialogResult Run(Filters f, Preset preset, ref XmlDocument xml, ref XmlElement row)
        {
            PeriodForm pf = Run(preset);
            return pf.RunFilters(f,ref xml,ref row);
        }
        public static DialogResult Run(Filters f, DateTime from, DateTime to,DType type,ref XmlDocument xml,ref XmlElement row)
        {
            PeriodForm pf = Run(from,to,type);
            return pf.RunFilters(f,ref xml,ref row);
        }
        public static DialogResult Run(Filters f, DateTime from, DType type,ref XmlDocument xml,ref XmlElement row)
        {
            return Run(f, from, DateTime.Now, type,ref xml,ref row);
        }
        public static DialogResult Run(Filters f, DateTime from, DateTime to,ref XmlDocument xml,ref XmlElement row)
        {
            return Run(f, from, to, DType.DATE,ref xml,ref row);
        }
        public static DialogResult Run(Filters f, ref XmlDocument xml){return Run(f, ref xml,ref nullElem);}
        public static DialogResult Run(Filters f, Preset preset, ref XmlDocument xml){return Run(f, preset, ref xml, ref nullElem);}
        public static DialogResult Run(Filters f, DateTime from, DateTime to, DType type, ref XmlDocument xml){return Run(f, from,to,type, ref xml, ref nullElem);}
        public static DialogResult Run(Filters f, DateTime from, DType type, ref XmlDocument xml){return Run(f, from, DateTime.Now, type, ref xml, ref nullElem);}
        public static DialogResult Run(Filters f, DateTime from, DateTime to, ref XmlDocument xml){return Run(f, from, to, DType.DATE, ref xml, ref nullElem);}
        public static DialogResult Run(Filters f){return Run(f,ref nullDocument,ref nullElem);}
        public static DialogResult Run(Filters f, Preset preset){return Run(f, preset, ref nullDocument, ref nullElem);}
        public static DialogResult Run(Filters f, DateTime from, DateTime to, DType type){return Run(f, from, to, type, ref nullDocument, ref nullElem);}
        public static DialogResult Run(Filters f, DateTime from, DType type){return Run(f, from, DateTime.Now, type,ref nullDocument,ref nullElem);}
        public static DialogResult Run(Filters f, DateTime from, DateTime to){return Run(f, from, to, DType.DATE,ref nullDocument,ref nullElem);}
        public static PeriodForm Run()
        {
            return new PeriodForm();
        }
        public static PeriodForm Run(Preset preset)
        {
            return new PeriodForm(preset);
        }
        public static PeriodForm Run(DateTime from, DateTime to, DType type)
        {
            return new PeriodForm(from, to, type);
        }
        public static PeriodForm Run(DateTime from, DType type)
        {
            return Run(from, DateTime.Now, type);
        }
        public static PeriodForm Run(DateTime from, DateTime to)
        {
            return Run(from, to, DType.DATE);
        }

        public PeriodForm()
        {
            InitializeComponent();
            dtpFrom.Value = DateTime.Now.Date;
            dtpTo.Value = DateTime.Now.Date;
        }

        public PeriodForm(DateTime from,DateTime to,DType type):this()
        {
            switch (type)
            {
                case DType.DATE:
                    rbDate.Checked = true;
                    dtpTo.Value = to;
                    break;
                case DType.DAY:
                    rbDay.Checked = true;
                    break;
                case DType.MONTH: rbMonth.Checked = true;
                    break;
                case DType.YEAR:
                    rbYear.Checked = true;
                    break;
            }
            dtpFrom.Value = from.Date;
        }

        public PeriodForm(DateTime from,DateTime to):this(from,to,DType.DATE)
        {

        }
        public PeriodForm(DateTime from, DType type)
            : this(from, DateTime.Now, DType.DATE)
        {

        }
        public PeriodForm(Preset preset):this()
        {
            setPreset(preset);
        }

        public void setPreset(Preset preset)
        {
            switch (preset)
            {
                case Preset.CUR_MONTH: rbMonth.Checked = true;
                    dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    break;
                case Preset.PREV_MONTH: rbMonth.Checked = true;
                    dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                    break;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dtpTo.Enabled = rbDate.Checked;
            dtpFrom_ValueChanged(null, null);
            cbPreset.SelectedIndex = 0;
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            if (rbDate.Checked)
                if (dtpTo.Value < dtpFrom.Value) dtpTo.Value = dtpFrom.Value.Date;
            if (rbDay.Checked) dtpTo.Value = dtpFrom.Value.Date;
            if (rbMonth.Checked) dtpTo.Value=dtpFrom.Value.AddMonths(1).AddDays(-1).Date;
            if (rbYear.Checked) dtpTo.Value = dtpFrom.Value.AddYears(1).AddDays(-1).Date;
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            if (dtpFrom.Value > dtpTo.Value) dtpFrom.Value = dtpTo.Value;
        }

        public string getFrom()
        {
            return dtpFrom.Value.Date.ToShortDateString();
        }
        public string getTo()
        {
            return dtpTo.Value.Date.ToShortDateString();
        }

        public void fillFilters(Filters f)
        {
            f["dfr"]=getFrom();
            f["dto"]=getTo();
        }

        public XmlDocument getXml(ref XmlElement elem)
        {
            return getXml(ref elem, getFrom(), getTo());
        }

        public static XmlDocument getXml(ref XmlElement elem,String from,String to)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement rows = doc.CreateElement("Rows");
            doc.AppendChild(rows);
            XmlElement row = doc.CreateElement("Row");
            rows.AppendChild(row);
            if (elem != nullElem)
                elem = row;
            row.AppendChild(doc.CreateElement("datefrom")).AppendChild(doc.CreateTextNode(from));
            row.AppendChild(doc.CreateElement("dateto")).AppendChild(doc.CreateTextNode(to));
            return doc;
        }

        private void cbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPreset.SelectedIndex == 0) return;
            Preset p = Preset.NONE;
            switch (cbPreset.SelectedIndex)
            {
                case 1:p=Preset.CUR_MONTH;break;
                case 2:p=Preset.PREV_MONTH;break;
            }
            setPreset(p);
        }
    }
}
