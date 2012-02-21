using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;

namespace rabnet
{
    public class OkrolPlanReport : ReportBase
    {
        public OkrolPlanReport() : base("okrolplan", "План окролов") { }

        public override void MakeReport()
        {
            XmlDocument doc = Engine.db().makeReport(getSQL());
            Dictionary<String, int> dict = new Dictionary<string, int>();
            foreach (XmlNode nd in doc.FirstChild.ChildNodes)
            {
                nd.SelectSingleNode("address").InnerText = Buildings.FullPlaceName(nd.SelectSingleNode("address").InnerText);
                if (!dict.ContainsKey(nd.SelectSingleNode("dt").InnerText))
                    dict.Add(nd.SelectSingleNode("dt").InnerText, 0);
                dict[nd.SelectSingleNode("dt").InnerText]++;
            }
            XmlElement newND,tmp;
            foreach (KeyValuePair<string, int> kvp in dict)
            {
                newND = doc.CreateElement("Row");

                tmp = doc.CreateElement("name");
                tmp.InnerText = "итого";
                newND.AppendChild(tmp);

                tmp = doc.CreateElement("dt");
                tmp.InnerText = kvp.Key;
                newND.AppendChild(tmp);

                tmp = doc.CreateElement("plus");
                tmp.InnerText = kvp.Value.ToString();
                newND.AppendChild(tmp);

                doc.FirstChild.AppendChild(newND);
            }
            ReportViewForm rvf = new ReportViewForm(MenuText, FileName, new XmlDocument[] { doc });
            rvf.ExcelEnabled = false;
            rvf.ShowDialog();
        }

        private string getSQL()
        {
            return String.Format(@"SELECT rabname(r_id,2) name,
(SELECT b_short_name FROM breeds where r_breed=b_id) brd,
rabplace(r_id) address,
Date_Format(DATE_ADD(r_event_date,interval {0:d} day),'%m-%d') dt,
'+' plus
FROM rabbits WHERE r_event_date is not null ORDER BY r_event_date;", Engine.opt().getIntOption(Options.OPT_ID.OKROL));
        }

        private XmlDocument toMatrixRep(XmlDocument doc)
        {
            return null;
        }

        protected override System.IO.Stream getAssembly()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("OkrolPlanPlugIn.okrolplan.rdl");
        }
    }
}
