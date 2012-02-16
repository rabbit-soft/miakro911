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
            XmlDocument doc = Engine.db().makeReport(getSQL());
            foreach (XmlNode nd in doc.FirstChild.ChildNodes)
                nd.SelectSingleNode("address").InnerText = Buildings.FullPlaceName(nd.SelectSingleNode("address").InnerText);
            ReportViewForm rvf = new ReportViewForm(MenuText, FileName, new XmlDocument[] { doc });
            rvf.ExcelEnabled = false;
            rvf.ShowDialog();
        }

        private string getSQL()
        {
            return String.Format(@"SELECT rabname(r_id,2) name,
(SELECT b_short_name FROM breeds where r_breed=b_id) brd,
rabplace(r_id) address,
Date_Format(DATE_ADD(r_event_date,interval {0:d} day),'%d.%m') dt,
'+' plus
FROM rabbits WHERE r_event_date is not null ORDER BY dt;", Engine.opt().getIntOption(Options.OPT_ID.OKROL));
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
