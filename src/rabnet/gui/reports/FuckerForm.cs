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
    public partial class FuckerForm : Form
    {
        int fucker=0;
        List<int> ids = new List<int>();
        public FuckerForm()
        {
            InitializeComponent();
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
        }

        public FuckerForm(int fucker):this()
        {
            this.fucker=fucker;
        }

        public void update()
        {
            comboBox1.Items.Clear();
            ids.Clear();
            int sid = 0;
            Fucks ff = Engine.db().GetAllFuckers(0, true, true, 0);
            foreach (Fucks.Fuck f in ff.fucks)
            {
                comboBox1.Items.Add(f.partner);
                ids.Add(f.partnerid);
                if (f.partnerid == fucker) 
                    sid = comboBox1.Items.Count - 1;
            }
            if (comboBox1.Items.Count>0) 
                comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FuckerForm_Load(object sender, EventArgs e)
        {
            update();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            if (dtpFrom.Value > dtpTo.Value)
                dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            if (dtpTo.Value < dtpFrom.Value)
                dtpFrom.Value = dtpTo.Value.AddMonths(-1).AddDays(1);
        }

        public int getFucker()
        {
            if (comboBox1.SelectedIndex < 0) return 0;
            return ids[comboBox1.SelectedIndex];
        }

        public String getFromDate()
        {
            return dtpFrom.Value.ToShortDateString();
        }
        public String getToDate()
        {
            return dtpTo.Value.ToShortDateString();
        }

        public XmlDocument getXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement row = doc.CreateElement("Row");
            doc.AppendChild(doc.CreateElement("Rows")).AppendChild(row);
            row.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(comboBox1.Text));
            row.AppendChild(doc.CreateElement("from")).AppendChild(doc.CreateTextNode(dtpFrom.Value.ToShortDateString()));
            row.AppendChild(doc.CreateElement("to")).AppendChild(doc.CreateTextNode(dtpTo.Value.ToShortDateString()));
            return doc;
        }
    }
}
