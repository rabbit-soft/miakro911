using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace rabnet
{
    public class OkrolPlanReport : ReportBase
    {
        public OkrolPlanReport() : base("okrolplan", "План окролов") { }

        public override void MakeReport()
        {
            BuildingForm dlg = new BuildingForm();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            XmlDocument doc = Engine.db().makeReport(getSQL(dlg.Build));
            Dictionary<String, int> dict = new Dictionary<string, int>();
            int total = 0;
            foreach (XmlNode nd in doc.FirstChild.ChildNodes)
            {
                nd.SelectSingleNode("address").InnerText = Building.FullPlaceName(nd.SelectSingleNode("address").InnerText);
                if (!dict.ContainsKey(nd.SelectSingleNode("dt").InnerText))
                    dict.Add(nd.SelectSingleNode("dt").InnerText, 0);
                dict[nd.SelectSingleNode("dt").InnerText]++;
                total++;
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

                tmp = doc.CreateElement("address");
                tmp.InnerText = total.ToString();
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

        private string getSQL(int build)
        {
            return String.Format(@"SELECT rabname(r_id,2) name,
(SELECT b_short_name FROM breeds where r_breed=b_id) brd,
rabplace(r_id) address,
Date_Format(DATE_ADD(r_event_date,interval {0:d} day),'%m %d') dt,
'+' plus
FROM rabbits 
WHERE r_event_date is not null AND DATE_ADD(r_event_date,interval {0:d} day)>NOW() 
    AND inBuilding({1:d},substr(rabplace(r_id),1,INSTR(rabplace(r_id),',')-1))
ORDER BY r_event_date;", Engine.opt().getIntOption(Options.OPT_ID.NEST_IN), build);
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
