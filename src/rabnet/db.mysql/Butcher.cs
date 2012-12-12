#if !DEMO
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using rabnet;

namespace db.mysql
{ 
    class Butcher:RabNetDataGetterBase
    {
        public Butcher(MySqlConnection sql,Filters f) : base(sql,f) { }

        protected override string getQuery()
        {
            string table = options.safeInt("type",0) == 1?"scaleprod" :"butcher";
            string dtfield = options.safeInt("type", 0) == 1 ? "s_date" : "b_date";
            string unfield = options.safeInt("type", 0) == 1 ? "appendPLUSell(s_id)" : "b_amount";
            /*return String.Format(@"CREATE TEMPORARY TABLE aaa as
SELECT Date(d_date) dt, SUM(r_group) cnt FROM dead WHERE d_reason=3 GROUP BY dt;

SELECT DISTINCT dt,cnt,(SELECT COUNT(*) FROM {0:s}=dt prod FROM aaa ORDER BY dt DESC

DROP TABLE aaa;",prod);*/
            return String.Format(@"CREATE TEMPORARY TABLE aaa as
SELECT Date(d_date) dt, SUM(r_group) cnt FROM dead WHERE d_reason=3 GROUP BY dt;
CREATE TEMPORARY TABLE bbb as SELECT dt FROM aaa;
SELECT dt,cnt,(SELECT COUNT(*) FROM {0:s} WHERE DATE({1:s})=dt) prod FROM aaa union
SELECT DATE({1:s}) dt,0,SUM({2:s}) FROM {0:s} WHERE DATE({1:s}) not in (SELECT dt FROM bbb) GROUP BY dt ORDER BY dt DESC;
DROP TABLE aaa;
DROP TABLE bbb;",table,dtfield,unfield);
        }

        protected override string countQuery()
        {
            return @"SELECT (SELECT COUNT(DISTINCT Date(d_date)) FROM dead WHERE d_reason=3) cols,
                            (SELECT COUNT(r_group) FROM dead WHERE d_reason=3) cnt;";
        }

        public static IData getBucherDate(MySqlDataReader rd)
        {
            return new ButcherDate(rd.GetDateTime("dt"), rd.GetInt32("cnt"), rd.GetInt32("prod"));
        }

        public override IData NextItem()
        {
            return getBucherDate(_rd);
        }

        /// <summary>
        /// Получает список забитых кроликов
        /// </summary>
        /// <param name="dt">Дата забива</param>
        public static AdultRabbit[] getVictims(MySqlConnection sql, DateTime dt)
        {
            List<AdultRabbit> result = new List<AdultRabbit>();
            MySqlCommand cmd = new MySqlCommand("", sql);
            cmd.CommandText = String.Format(@"SELECT {1:s}
FROM dead WHERE d_reason=3 AND DATE(d_date)='{0:yyyy-MM-dd}';", dt,RabbitGetter.getAdultRabbit_FieldsSet(RabAliveState.DEAD));
            MySqlDataReader rd = cmd.ExecuteReader();
            while(rd.Read())
            {
                result.Add(RabbitGetter.fillAdultRabbit(rd));
            }
            rd.Close();
            return result.ToArray();
        }

        public static List<sMeat> GetMeats(MySqlConnection sql, DateTime dt)
        {
            List<sMeat> result = new List<sMeat>();
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT 
    b_id,
    b_date,
    (SELECT p_name FROM products WHERE p_id=b_prodtype) prod,
    b_amount,
    (SELECT p_unit FROM products WHERE p_id=b_prodtype) units,
    (SELECT u_name FROM users WHERE b_user=u_id) user,
    if(DATE(b_date)=DATE(NOW()),'true','false') today 
FROM butcher WHERE DATE(b_date)='{0:yyy-MM-dd}' ORDER by b_date DESC;",dt), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                result.Add(new sMeat(rd.GetInt32("b_id"),
                    rd.GetDateTime("b_date"),
                    rd.GetString("prod"),
                    rd.GetFloat("b_amount"),
                    rd.GetString("units"),
                    rd.GetBoolean("today"),
                    rd.GetString("user"))
                    );
            }
            rd.Close();
            return result;
        }

        public static List<String> getButcherMonths(MySqlConnection sql)
        {
            List<String> result = new List<String>();
            MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT Date_Format(b_date,'%m.%Y')dt FROM butcher ORDER BY b_date;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                result.Add(rd.GetString("dt"));
            }
            rd.Close();
            return result;
        }
    }

    //class Scale
    //{
    //    public static List<ScalePLUSummary> getPluSummarys(MySqlConnection sql,DateTime date)
    //    {
    //        List<ScalePLUSummary> result = new List<ScalePLUSummary>();
    //        MySqlCommand cmd = new MySqlCommand(String.Format("SELECT s_id,s_date,s_plu_id,s_plu_name,appendPLUSumm(s_id) asm,appendPLUSell(s_id) asl,appendPLUWeight(s_id)aw,s_cleared FROM scaleprod WHERE DATE(s_date)='{0:yyyy-MM-dd}' ORDER BY s_id DESC;", date), sql);
    //        MySqlDataReader rd = cmd.ExecuteReader();
    //        while(rd.Read())
    //        {
    //            result.Add(new ScalePLUSummary(
    //                rd.GetInt32("s_id"),
    //                rd.GetDateTime("s_date"),
    //                rd.GetInt32("s_plu_id"),
    //                rd.GetString("s_plu_name"),
    //                rd.GetInt32("asl"),
    //                rd.GetInt32("asm"),              
    //                rd.GetInt32("aw"),
    //                rd.GetDateTime("s_cleared")));
    //        }
    //        rd.Close();
    //        return result;
    //    }

    //    public static void addPLUSummary(MySqlConnection sql, int prodid, string prodname, int tsell, int tsumm, int tweight, DateTime cleared)
    //    {
    //        try
    //        {
    //            MySqlCommand cmd = new MySqlCommand("", sql);
    //            cmd.CommandText = String.Format(@"call addPLUSummary({0:d},'{1:s}',{2:d},{3:d},{4:d},'{5:yyyy-MM-dd hh:mm}');",
    //                                                            prodid, prodname, tsell, tsumm, tweight, cleared);
    //            cmd.ExecuteNonQuery();
    //        }
    //        catch {}
    //    }

    //    public static void DeletePLUsumary(MySqlConnection sql, int sid,DateTime lc)
    //    {
    //        MySqlCommand cmd = new MySqlCommand(String.Format("call deletePLUsumary({0:d},{1:yyyy-MM-dd hh-mm-ss});",sid,lc), sql);
    //        cmd.ExecuteNonQuery();
    //    }
    //}
}
#endif