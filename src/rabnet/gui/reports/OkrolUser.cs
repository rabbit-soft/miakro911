using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace rabnet
{
    public partial class OkrolUser : Form
    {
        List<int> ids = new List<int>();
        public OkrolUser()
        {
            InitializeComponent();
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            List<sUser> usrs = Engine.db().getUsers();
            for (int i = 0; i < usrs.Count; i++)
            {
                comboBox1.Items.Add(usrs[i].Name);
                ids.Add(usrs[i].Id);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            //if (dtpFrom.Value > dtpTo.Value)
            dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            /*
            if (dtpTo.Value < dtpFrom.Value)
                dtpFrom.Value = dtpTo.Value.AddMonths(-1).AddDays(1);
             * */
        }

        public int getUser()
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
            row.AppendChild(doc.CreateElement("from")).AppendChild(doc.CreateTextNode(makePrettyDate(dtpFrom.Value)));
            row.AppendChild(doc.CreateElement("to")).AppendChild(doc.CreateTextNode(makePrettyDate(dtpTo.Value)));
            return doc;
        }

        private string makePrettyDate(DateTime uglyDate)
        {
            string result = uglyDate.Day.ToString() + " ";
            switch (uglyDate.Month)
            {
                case 1: result += "Января "; break;
                case 2: result += "Февраля "; break;
                case 3: result += "Марта "; break;
                case 4: result += "Апреля "; break;
                case 5: result += "Мая "; break;
                case 6: result += "Июню "; break;
                case 7: result += "Июля "; break;
                case 8: result += "Августа "; break;
                case 9: result += "Сентября "; break;
                case 10: result += "Октября "; break;
                case 11: result += "Ноября "; break;
                case 12: result += "Декабря "; break;
            }
            return result + uglyDate.Year.ToString()+"г.";
        }
    }
}
