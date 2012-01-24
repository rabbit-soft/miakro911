using System;
using System.Xml;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public class PopulationReport:ReportBase
    {
        public PopulationReport() : base("population", "Движение поголовья по месяцам рождения") { }

        public override void MakeReport()
        {
#if Release || DEBUG
            PeriodForm dlg = new PeriodForm(MenuText);
            dlg.PeriodConstrain = 4;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                int year = int.Parse(dlg.DateValue);
                XmlDocument doc = new XmlDocument();
                DateTime dt = new DateTime(year,1,1);
                while (dt.AddMonths(1) < DateTime.Now.Date && dt.Year==year)
                {
                    if(doc.ChildNodes.Count == 0)
                        doc = Engine.db().makeReport(getSQL(dt));
                    else 
                        doc.FirstChild.AppendChild(doc.ImportNode(Engine.db().makeReport(getSQL(dt)).SelectSingleNode("Rows/Row"),true));
                    dt = dt.AddMonths(1);
                }
                (new ReportViewForm(MenuText, FileName, new XmlDocument[]
                {
                   doc//,dlg.getXml()
                }
                )).ShowDialog();
            }
#endif
        }

        private string getSQL(DateTime dt)
        {
            return String.Format(@"select
  '{2:00}' month,
  '{3:d}' year,
  (select Coalesce(sum(r_group),0) from rabbits where r_born<'{0:s}') + (select Coalesce(sum(r_group),0) from dead where r_born<'{0:s}' and d_date>='{0:s}') bcount,
  (select Coalesce(sum(f_children+f_added),0) from fucks where f_state='okrol' and f_end_date>='{0:s}' and f_end_date<'{1:s}') born,
  (select Coalesce(sum(t_count),0) from income where t_date>= '{0:s}' and t_date<'{1:s}') buy,
  (select Coalesce(sum(r_group),0) from dead where d_reason>2 and d_date>='{0:s}' and d_date<'{1:s}') gonetotal,
  (select Coalesce(sum(r_group),0) from dead where d_reason=3 and d_date>='{0:s}' and d_date<'{1:s}' ) killed,
  (select Coalesce(sum(r_group),0) from dead where d_reason=6 and d_date>='{0:s}' and d_date<'{1:s}' ) dying,
  (select Coalesce(sum(r_group),0) from dead where d_reason=5 and d_date>='{0:s}' and d_date<'{1:s}') sell,
  (select Coalesce(sum(r_group),0) from dead where d_reason>6 and d_date>='{0:s}' and d_date<'{1:s}' ) another,  
  (select Coalesce(sum(r_group),0) from rabbits where r_born<'{1:s}') + (select Coalesce(sum(r_group),0) from dead where r_born<'{1:s}' and d_date>='{1:s}') ecount;", 
                dt.ToString("yyyy-MM-dd"), dt.AddMonths(1).ToString("yyyy-MM-dd"),dt.Month,dt.Year);
        }

        protected override System.IO.Stream getAssembly()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("PopulationPlugIn.population.rdl");
        }
    }
}
