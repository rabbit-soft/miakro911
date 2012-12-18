using System;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using rabnet.forms;

namespace rabnet
{
    public class PopulationReport:ReportBase
    {
        private string[] XCL_HEADERS = new string[] { "Месяц", "Год", "Кол-во на начальный период", "Приход всего", "Родилось", 
            "Приоретено","Списано всего","Убой","Продажа племенного поголовья","Падеж","Другие причины","Кол-во на конец периода" };
        public PopulationReport() : base("population", "Наличие и движение поголовья за период") { }

        public override void MakeReport()
        {
#if RELEASE || DEBUG
            PeriodForm dlg = new PeriodForm(MenuText);
            dlg.PeriodConstrain = 4;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                int year = int.Parse(dlg.DateValue);
                XmlDocument doc = new XmlDocument();
                DateTime dt = new DateTime(year,1,1);
                while (dt.Year == year && dt.Date <= DateTime.Now.Date )
                {
                    if ((dlg.MinDate.Year == dt.Year && dlg.MinDate.Month > dt.Month))
                    {
                        dt = dt.AddMonths(1);
                        continue;
                    }
                    if(doc.ChildNodes.Count == 0)
                        doc = Engine.db().makeReport(getSQL(dt));
                    else 
                        doc.FirstChild.AppendChild(doc.ImportNode(Engine.db().makeReport(getSQL(dt)).SelectSingleNode("Rows/Row"),true));
                    dt = dt.AddMonths(1);
                }
                ReportViewForm rvf = new ReportViewForm(MenuText, FileName, new XmlDocument[] { doc }, XCL_HEADERS);
                
                rvf.Show();
            }
#endif
        }

        private string getSQL(DateTime dt)
        {
            return String.Format(@"create temporary table aaa as select
  CONCAT('{2:s}',IF(Month(NOW())='{4:d}' AND Year(NOW())={3:d},' (now)','')) month,
  '{3:d}' year,
  (select Coalesce(sum(r_group),0) from rabbits where r_born<'{0:s}') + (select Coalesce(sum(r_group),0) from dead where r_born<'{0:s}' and d_date>='{0:s}') bcount,
  (select Coalesce(sum(f_children+f_added),0) from fucks where f_state='okrol' and f_end_date>='{0:s}' and f_end_date<'{1:s}') born,
  (select Coalesce(sum(t_count),0) from income where t_date>= '{0:s}' and t_date<'{1:s}') buy,
#(select Coalesce(sum(r_group),0) from dead where d_reason>2 and d_date>='{0:s}' and d_date<'{1:s}') gonetotal,
  (select Coalesce(sum(r_group),0) from dead where d_reason=3 and d_date>='{0:s}' and d_date<'{1:s}' ) killed,
  (select Coalesce(sum(r_group),0) from dead where d_reason=4 and d_date>='{0:s}' and d_date<'{1:s}') sell,
  (select Coalesce(sum(r_group),0) from dead where (d_reason=5 or d_reason=6) and d_date>='{0:s}' and d_date<'{1:s}' ) dying,
  (select Coalesce(sum(r_group),0) from dead where d_reason>6 and d_date>='{0:s}' and d_date<'{1:s}' ) another,  
  (select Coalesce(sum(r_group),0) from rabbits where r_born<'{1:s}') + (select Coalesce(sum(r_group),0) from dead where r_born<'{1:s}' and d_date>='{1:s}') ecount;
select `month`, `year`, bcount, (born+buy) income,born, buy,(killed+dying+sell+another) gonetotal, killed, dying, sell, another, ecount from aaa;
drop temporary table aaa;",
                dt.ToString("yyyy-MM-dd"), dt.AddMonths(1).ToString("yyyy-MM-dd"), toRusMonth(dt.Month), dt.Year,dt.Month);
        }

        protected override System.IO.Stream getAssembly()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("PopulationPlugIn.population.rdl");
        }

        private string toRusMonth(int dt)
        {
            try
            {
                string result = "";
                switch (dt)
                {
                    case 1: result += "Январь "; break;
                    case 2: result += "Февраль "; break;
                    case 3: result += "Март "; break;
                    case 4: result += "Апрель "; break;
                    case 5: result += "Май "; break;
                    case 6: result += "Июнь "; break;
                    case 7: result += "Июль "; break;
                    case 8: result += "Август "; break;
                    case 9: result += "Сентябрь "; break;
                    case 10: result += "Октябрь "; break;
                    case 11: result += "Ноябрь "; break;
                    case 12: result += "Декабрь "; break;
                }
                return result;
            }
            catch
            {
                return "";
            }
        }
    }
}
