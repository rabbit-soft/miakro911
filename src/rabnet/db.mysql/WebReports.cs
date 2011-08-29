using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    class WebReports
    {

        public static string GetGlobal(MySqlConnection sql, DateTime dt)
        {
            MySqlCommand cmd = new MySqlCommand(getQuery_OneGlogalDay(dt)+";", sql);           
            MySqlDataReader rd = cmd.ExecuteReader();

            string result = "";
            if(rd.Read()) result = oneGlobalDay(rd);
            rd.Close();
            return result;
        }

        public static string[] GetGlobals(MySqlConnection sql, DateTime dt, int days)
        {
            string[] result = new string[days];
            string megaQuery = "" ;
            while (days > 1)
            {
                megaQuery += String.Format("({0:s}) union ", getQuery_OneGlogalDay(dt));
                dt = dt.AddDays(-1);
                days--;
            }
            megaQuery += String.Format("({0:s});", getQuery_OneGlogalDay(dt));
            MySqlCommand cmd = new MySqlCommand(megaQuery, sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int i = 0;
            while (rd.Read())
            {
                result[i] = oneGlobalDay(rd);
                i++;
            }
            rd.Close();
            return result;
        }

        private static string getQuery_OneGlogalDay(DateTime dt)
        {
            return String.Format(@"Select
'{0:s}' date,
(SELECT Count(*) FROM fucks WHERE Date(f_date)='{0:s}' ) fucks,
(SELECT Count(*) FROM fucks WHERE f_state='okrol' AND Date(f_end_date)='{0:s}' ) okrols,
Coalesce((SELECT Count(*) FROM fucks WHERE f_state='proholost' AND Date(f_end_date)='{0:s}'   ),0) proholosts,
Coalesce((SELECT Sum(f_children) FROM fucks WHERE f_state='okrol' AND Date(f_end_date)='{0:s}'),0) born,
Coalesce((SELECT Sum(r_group) FROM dead WHERE d_reason=3 AND Date(d_date)='{0:s}'),0) killed,
Coalesce((SELECT Sum(r_group) FROM dead WHERE d_reason>3 AND Date(d_date)='{0:s}'),0) deads,
( select sum(a) from ( (SELECT Sum(r_group) a FROM rabbits WHERE Date(r_born)<='{0:s}') union all (SELECT Sum(r_group) a FROM dead WHERE Date(r_born)<='{0:s}' AND Date(d_date)>'{0:s}'))rb) rabbits ", dt.ToString("yyyy-MM-dd"));
        }

        private static string oneGlobalDay(MySqlDataReader rd)
        {
            if (rd.IsClosed) return "";
            string result = "";
            result += String.Format("date={0:s};", rd.GetString("date"));
            result += String.Format("fucks={0:s};", rd.GetString("fucks"));
            result += String.Format("okrols={0:s};", rd.GetString("okrols"));
            result += String.Format("proholosts={0:s};", rd.GetString("proholosts"));
            result += String.Format("born={0:s};", rd.GetString("born"));
            result += String.Format("killed={0:s};", rd.GetString("killed"));
            result += String.Format("deads={0:s};", rd.GetString("deads"));
            result += String.Format("rabbits={0:s};", rd.GetString("rabbits"));
            return result;
        }
    }
}
