using System;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using rabnet.forms;

namespace rabnet
{
    public class TestReport:ReportBase
    {
        public TestReport() : base("test", "Тестовый отчет") { }
        

        public override void MakeReport()
        {
#if RELEASE || DEBUG
            PeriodForm dlg = new PeriodForm(MenuText);
            Filters f = new Filters();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                f["dttp"] = dlg.PeriodChar;
                f["dtval"] = dlg.DateValue;
                (new ReportViewForm(MenuText, FileName, new XmlDocument[]
                {
                   Engine.db().makeReport(getSQL(f)),
                   dlg.getXml()
                }
                )).ShowDialog();
            }
#endif
        }

        private string getSQL(Filters f)
        {
            string period = "";
            if (f.safeValue("dttp") == "d")
            {
                DateTime dt = DateTime.Parse(f.safeValue("dtval"));
                period = String.Format("DATE(d_date)='{0:yyyy-MM-dd}'", dt);
            }
            else if (f.safeValue("dttp") == "y")
            {
                period = String.Format("YEAR(d_date)={0}", f.safeValue("dtval"));
            }
            else
            {
                DateTime dt = DateTime.Parse(f.safeValue("dtval"));
                period = String.Format("MONTH(d_date)={0:MM} AND YEAR(d_date)={0:yyyy}", dt);
            }
            string s = String.Format(@"
    (SELECT SUM(r_group) grp,
    d_reason,
    (SELECT d_name FROM deadreasons WHERE d_reason=d_id) reason 
FROM dead WHERE {0} GROUP BY d_reason)
UNION 
(SELECT SUM(r_group),0,'Итого' FROM dead WHERE {0});", period);
            return s;
        }

        protected override System.IO.Stream getAssembly()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("TestPlugIn.test.rdl");
        }

    }
}
