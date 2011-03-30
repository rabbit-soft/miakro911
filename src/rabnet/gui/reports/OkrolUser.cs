using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using X_Tools;

namespace rabnet
{
    public partial class OkrolUser : Form
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

        public string DateValue
        {
            get 
            {
                if (Period == myDatePeriod.Month)
                    return cbMonth.Text;
                else return cbYear.Text;
            }
        }

        List<int> ids = new List<int>();
        public OkrolUser()
        {
            InitializeComponent();
            //dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            List<sUser> usrs = Engine.db().getUsers();
            for (int i = 0; i < usrs.Count; i++)
            {
                comboBox1.Items.Add(usrs[i].Name);
                ids.Add(usrs[i].Id);
            }
            comboBox1.SelectedIndex = 0;
            fillFucksDates();
        }

        private void fillFucksDates()
        {
            cbMonth.Items.Clear();
            cbYear.Items.Clear();
            List<String> dates = Engine.get().db().getFuckMonths();
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
                MessageBox.Show("Нет дат случек.");
                this.Close();
            }
        }

        public int getUser()
        {
            if (comboBox1.SelectedIndex < 0) return 0;
            return ids[comboBox1.SelectedIndex];
        }

        public XmlDocument getXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement row = doc.CreateElement("Row");
            doc.AppendChild(doc.CreateElement("Rows")).AppendChild(row);
            row.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(comboBox1.Text));
            row.AppendChild(doc.CreateElement("from")).AppendChild(doc.CreateTextNode(DateValue));
            //row.AppendChild(doc.CreateElement("to")).AppendChild(doc.CreateTextNode(""));
            return doc;
        }

        private void rbMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMonth.Checked)
            {
                cbMonth.Enabled = true;
                cbMonth.SelectedIndex = 0;
                cbYear.Enabled = false;
                cbYear.SelectedIndex = -1;
            }
            else
            {
                cbMonth.Enabled = false;
                cbMonth.SelectedIndex = -1;
                cbYear.Enabled = true;
                cbYear.SelectedIndex = 0;
            }
        }
    }
}
