#if !DEMO
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using rabnet.forms;

namespace rabnet
{
    public class FemaleSummaryReport : ReportBase
    {
        public FemaleSummaryReport() : base("female_summary", "Сводка по крольчихам") { }

        public override void MakeReport()
        {
            XmlDocument doc = Engine.db().makeReport(getSQL());

            ReportViewForm rvf = new ReportViewForm(MenuText, FileName, new XmlDocument[] { doc });
            rvf.ExcelEnabled = false;
            rvf.ShowDialog();
        }


        protected override System.IO.Stream getAssembly()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("FemaleSummaryPlugIn.female_summary.rdl");
        }

        private string getSQL()
        {
            return @"select * FROM (SELECT r_id,rabname(r_id,2) name,
    b_name AS breed,
    To_Days(NOW())-To_Days(r_born) age,
    CASE r_status
        when 0 then 'невеста'
        when 1 then 'первокролка'
        else 'штатная'
    END status,
    r_overall_babies kids,
    r_lost_babies lost,
    Coalesce(pCount,0) proholosts,
    Coalesce(oCount,0) okrols
    FROM rabbits
    INNER JOIN breeds ON r_breed=b_id
    LEFT JOIN (SELECT f_rabid as p_rab,Count(*) pCount FROM fucks WHERE f_state='proholost' GROUP BY f_rabid) proh ON p_rab=r_id
    LEFT JOIN (SELECT f_rabid as o_rab,Count(*) oCount FROM fucks WHERE f_state='okrol' GROUP BY f_rabid) okr ON o_rab=r_id
WHERE r_sex='female' AND r_name!=0) aaa ORDER BY name;";
        }
    }
}
#endif
