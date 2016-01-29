using System;
using System.Collections.Generic;
using System.Text;
using rabnet;
using System.Data.Common;

namespace rabnet
{
    public class DBHelper
    {
        public static string Nullable(int v)
        {
            return v == 0 ? "NULL" : v.ToString();
        }

        public static int GetNullableInt(DbDataReader rd, string fieldName)
        {
            if (rd.IsDBNull(rd.GetOrdinal(fieldName))) {
                return 0;
            }
            return rd.GetInt32(rd.GetOrdinal(fieldName));
        }

        public static String DateToSqlString(DateTime dt)
        {
            if (dt == DateTime.MinValue) {
                return "NULL";
            }
            if (dt.Date == DateTime.Now.Date) {
                return "Date(NOW())";
            }
            return String.Format("'{0:D4}-{1:D2}-{2:D2}'", dt.Year, dt.Month, dt.Day);
        }

        public static String DaysPastSqlDate(int daysPast)
        {
            return daysPast <= 0 ? "Date(NOW())" : String.Format("Date_Add(Date(NOW()),INTERVAL -{0:d} DAY)", daysPast);
        }

        public static String commonBon(String b1, String b2)
        {
            string res = "0";
            for (int i = 1; i < 5; i++) {
                res += b1[i] < b2[i] ? b1[i] : b2[i];
            }
            return res;
        }

        //public static String escape(String str)
        //{
        //    return MySqlHelper.EscapeString(str);
        //}

        public static string MakeDatePeriod(Filters f, string dateField)
        {
            DateTime dt;
            string period = "";
            if (!f.ContainsKey(Filters.DATE_PERIOD) || !f.ContainsKey(Filters.DATE_VALUE)) {
                return period;
            }

            if (f.safeValue(Filters.DATE_PERIOD) == "d") {
                if (!DateTime.TryParse(f.safeValue(Filters.DATE_VALUE), out dt)) {
                    return period;
                }
                period = String.Format("DATE({1:s})='{0:yyyy-MM-dd}'", dt, dateField);
            }
            if (f.safeValue(Filters.DATE_PERIOD) == "m") {
                if (!DateTime.TryParse(f.safeValue(Filters.DATE_VALUE), out dt)) {
                    return period;
                }
                period = String.Format("(MONTH({1:s})={0:MM} AND YEAR({1:s})={0:yyyy})", dt, dateField);
            } else if (f.safeValue(Filters.DATE_PERIOD) == "y") {
                period = String.Format("YEAR({1:s})={0}", f.safeValue(Filters.DATE_VALUE), dateField);
            }

            return period;
        }

        public static void GetDatePeriodBounds(Filters f, out DateTime from, out DateTime to)
        {
            from = DateTime.MinValue;
            to = DateTime.MaxValue;

            if (!f.ContainsKey(Filters.DATE_PERIOD) || !f.ContainsKey(Filters.DATE_VALUE)) return;

            if (f.safeValue(Filters.DATE_PERIOD) == "d") {
                if (!DateTime.TryParse(f.safeValue(Filters.DATE_VALUE), out from)) return;
                to = from.AddDays(1);
            }
            if (f.safeValue(Filters.DATE_PERIOD) == "m") {
                if (!DateTime.TryParse(f.safeValue(Filters.DATE_VALUE), out from)) return;
                to = from.AddMonths(1);
            } else if (f.safeValue(Filters.DATE_PERIOD) == "y") {
                if (!DateTime.TryParse(f.safeValue(Filters.DATE_VALUE) + "-01-01", out from)) return;
                to = from.AddYears(1);
            }
        }
    }
}
