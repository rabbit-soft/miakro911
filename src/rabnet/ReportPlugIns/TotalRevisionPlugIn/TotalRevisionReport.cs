#if !DEMO
using System;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using rabnet.forms;

namespace rabnet
{
    public class TotalRevisionReport:ReportBase
    {
        public TotalRevisionReport() : base("total_revision", "Полная ревизия") { }

        public override void MakeReport()
        {
#if RELEASE || DEBUG
            PeriodForm dlg = new PeriodForm(this.MenuText);
            if (dlg.ShowDialog() == DialogResult.Cancel) return;

            Filters f = new Filters(Filters.MAKE_CANDIDATE,Engine.get().candidateAge(),
                Filters.MAKE_BRIDE,Engine.get().brideAge(),
                Filters.DATE_PERIOD,dlg.PeriodChar,
                Filters.DATE_VALUE, dlg.PeriodValue);
            string s = getSQL(f);
            XmlDocument doc = Engine.db().makeReport(s);

            ReportViewForm rvf = new ReportViewForm(MenuText, FileName, new XmlDocument[] { doc,dlg.GetXml() });
            rvf.ExcelEnabled = false;
            rvf.Show();
#endif
        }

        protected override System.IO.Stream getAssembly()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("TotalRevisionPlugIn.total_revision.rdl");
        }

        protected override String getSQL(Filters flt)
        {
            DateTime from, to;
            string YNG_TEMPL = @"(SELECT Coalesce(Sum(r_group),0) FROM rabbits 
    WHERE (
        (r_sex='male' AND r_status!=2)
            OR (r_sex='female' AND r_status=0 AND r_event_date IS NULL)
            OR (r_sex='void') 
    ) {0:s}) {1:s}",
                YNG_DEAD = "(SELECT Coalesce(Sum(r_group),0) FROM dead WHERE {0:s} AND d_reason IN (5,6)) {1:s}",///кролики которые родились в указанный период и умерли потом
                DT_TO = " AND {0:s}<Date_Add(r_born,INTERVAL {1:d} {2:s}) ",   ///сейчас меньше N месяцев
                DT_FROM = " AND {0:s}>=Date_Add(r_born,INTERVAL {1:d} {2:s}) ";///сейчас больше N месяцев
            DBHelper.GetDatePeriodBounds(flt,out from,out to);
            return String.Format(@"SELECT
(SELECT Coalesce(Sum(r_group),0) FROM rabbits WHERE r_sex='female' AND (r_status>=1 OR r_event_date IS NOT NULL)) females,
(SELECT Coalesce(Sum(r_group),0) FROM rabbits WHERE r_sex='male' AND r_status=2) males,
{0:s}, {1:s}, {2:s}, {3:s}, {4:s},
#(SELECT Coalesce(Sum(r_group),0) FROM rabbits WHERE r_event_date IS NOT NULL) pregnants,
(SELECT Coalesce(Sum(r_group),0) FROM rabbits ) total,
#(SELECT Count(f_rabid) FROM fucks WHERE f_state='okrol' AND {5:s}) per_okrols,
#(SELECT Count(f_rabid) FROM fucks WHERE f_state='proholost' AND {5:s}) per_proholosts,
{6:s}, {7:s}, {8:s}, {9:s}, {10:s}, {11:s} ,
#(SELECT Sum(r_group) FROM dead WHERE {14:s} AND d_reason IN ({12:d},{13:d})) per_dead, #умершие в период
(SELECT Coalesce(Sum(f_children+f_added),0) FROM fucks WHERE f_state='okrol' AND {5:s}) per_born,
#(SELECT Coalesce(Sum(r_group),0) FROM dead WHERE d_reason={15:d} AND {14:s} ) per_killed,
#(SELECT Coalesce(Sum(r_group),0) FROM dead WHERE d_reason={16:d} AND {14:s} ) per_selled,
#(SELECT Coalesce(Sum(r_group),0) FROM dead WHERE d_reason>{13:d} AND {14:s} ) per_dead_other,
(SELECT Coalesce(Sum(r_group),0) FROM rabbits WHERE {17:s} ) per_alive #, #родились в этот период и живы до сих пор
#(SELECT Coalesce(sum(r_group),0) FROM rabbits WHERE r_born<'{18:yyyy-MM-dd}') + (SELECT Coalesce(sum(r_group),0) FROM dead WHERE r_born<'{18:yyyy-MM-dd}' AND d_date>='{19:yyyy-MM-dd}') per_was_alive
; ",         String.Format(YNG_TEMPL, String.Format(DT_TO, "Now()", 1, "month"), "yng1"),//0
             String.Format(YNG_TEMPL, String.Format(DT_FROM, "Now()", 1, "month")+ String.Format(DT_TO, "Now()", 2, "month"), "yng2"),
             String.Format(YNG_TEMPL, String.Format(DT_FROM, "Now()", 2, "month")+ String.Format(DT_TO, "Now()", 3, "month"), "yng3"),
             String.Format(YNG_TEMPL, String.Format(DT_FROM, "Now()", 3, "month")+ String.Format(DT_TO, "Now()", 4, "month"), "yng4"),
             String.Format(YNG_TEMPL, String.Format(DT_FROM, "Now()", 4, "month"), "yng5"),
             DBHelper.MakeDatePeriod(flt, "f_end_date"),//5             
             String.Format(YNG_DEAD, DBHelper.MakeDatePeriod(flt, "r_born")+ String.Format(DT_TO, "d_date", 10, "day"), "yd1"),
             String.Format(YNG_DEAD, DBHelper.MakeDatePeriod(flt, "r_born")+ String.Format(DT_FROM, "d_date", 10, "day")+ String.Format(DT_TO, "d_date", 1, "month"), "yd2"),
             String.Format(YNG_DEAD, DBHelper.MakeDatePeriod(flt, "r_born")+ String.Format(DT_FROM, "d_date", 1, "month")+ String.Format(DT_TO, "d_date", 2, "month"), "yd3"),
             String.Format(YNG_DEAD, DBHelper.MakeDatePeriod(flt, "r_born")+ String.Format(DT_FROM, "d_date", 2, "month")+ String.Format(DT_TO, "d_date", 3, "month"), "yd4"),
             String.Format(YNG_DEAD, DBHelper.MakeDatePeriod(flt, "r_born")+ String.Format(DT_FROM, "d_date", 3, "month")+ String.Format(DT_TO, "d_date", 4, "month"), "yd5"),
             String.Format(YNG_DEAD, DBHelper.MakeDatePeriod(flt, "r_born")+ String.Format(DT_FROM, "d_date", 4, "month"),  "yd6"),
             DeadReason_Static.Dead_KidsCount, //12
             DeadReason_Static.Dead,//13
             DBHelper.MakeDatePeriod(flt,"d_date"),//14
             DeadReason_Static.Killed, //15
             DeadReason_Static.Selled,//16
             DBHelper.MakeDatePeriod(flt,"r_born"),//17
             to,from//18
             );
        }
    }
}
#endif
