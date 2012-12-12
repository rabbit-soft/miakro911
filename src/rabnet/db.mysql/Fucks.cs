﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using log4net;
using rabnet;

namespace db.mysql
{ 
    class FucksGetter
    {
        private static ILog _logger = LogManager.GetLogger(typeof(FucksGetter));

        public static Fucks GetFucks(MySqlConnection sql, Filters flt)
        {
           string query = String.Format(@"
    ({0:s} FROM fucks 
    WHERE isdead(f_partner)=0 {2:s} {3:s} ORDER BY f_rabid,f_date)
    UNION
    ({1:s} FROM fucks 
    WHERE isdead(f_partner)=1 {2:s} {3:s} ORDER BY f_rabid,f_date);", fuckFields(true), fuckFields(false),
                                                      (flt.safeInt(Filters.RAB_ID, 0) != 0 ? "AND f_rabid=" + flt.safeValue(Filters.RAB_ID) : ""),
                                                      DBHelper.MakeDatePeriod(flt,"f_date"));

            MySqlCommand cmd = new MySqlCommand(query, sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            Fucks f = new Fucks();
            while (rd.Read())
            {
                f.AddFuck(rd.GetInt32("f_id"), rd.GetString("name"),rd.GetString("partner"), rd.GetInt32("f_partner"), rd.GetInt32("f_times"),
                    rd.IsDBNull(rd.GetOrdinal("f_date")) ? DateTime.MinValue : rd.GetDateTime("f_date"),
                    rd.IsDBNull(rd.GetOrdinal("f_end_date")) ? DateTime.MinValue : rd.GetDateTime("f_end_date"),
                    rd.GetString("f_state"), rd.GetInt32("f_children"), rd.GetInt32("f_dead"),
                    rd.IsDBNull(rd.GetOrdinal("breed")) ? 1 :rd.GetInt32("breed"),
                    rd.IsDBNull(rd.GetOrdinal("genom")) ? "" : rd.GetString("genom"), 
                    rd.GetString("f_type"),
                    rd.GetInt32("f_killed"), rd.GetInt32("f_added"), (rd.GetInt32("dead") == 1), rd.IsDBNull(7) ? "" : rd.GetString("worker")
                    );
            }
            rd.Close();
            return f;
        }

        private static string fuckFields(bool alive)
        {
            return String.Format(@"SELECT 
        rabname(f_rabid,2) name,
        f_id,f_date,f_partner,f_times,f_state,f_date,f_end_date,
        (SELECT u_name FROM users WHERE u_id=f_worker) worker, 
        f_children,f_dead,f_type,
        {0:s}name(f_partner,2) partner,
        (SELECT r_breed FROM {1:s} WHERE r_id=f_partner) breed,
        (SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=(SELECT r_genesis FROM dead WHERE r_id=f_partner)) genom,
        f_killed,f_added,{2:d} dead", (alive ? "rab" : "dead"), (alive ? "rabbits" : "dead"), (alive ? 0 : 1));
        }

        public static Fucks AllFuckers(MySqlConnection sql, Filters flt)
        {
            int femaleId = flt.safeInt(Filters.RAB_ID, 0);

            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT 
    r_id,
    rabname(r_id,2) fullname,
    r_status,
    r_last_fuck_okrol AS fuckDate,
    To_Days(NOW())-To_Days(r_born) age,
    r_breed,
    (SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=r_genesis) genom,
    Coalesce(fcs.fucks,0) fucks,
    Coalesce(fcs.children,0) children
FROM rabbits 
LEFT JOIN (SELECT f_partner, SUM(f_times) fucks, SUM(f_children) children FROM fucks WHERE f_rabid={0:d} GROUP BY f_partner)fcs ON f_partner=r_id
WHERE r_sex='male' AND {3:s}
    {1:s}
    {2:s} 
    {4:s}
ORDER BY fullname;",
    femaleId,
    (flt.safeBool(Filters.HETEROSIS,true) ? "" : String.Format(" AND r_breed=(SELECT r2.r_breed FROM rabbits r2 WHERE r_id={0:d})",femaleId)),
    (flt.safeBool(Filters.INBREEDING, true) ? "" : String.Format(@" AND (SELECT COUNT(g_genom) FROM genoms WHERE g_id=r_genesis AND g_genom IN 
        (SELECT g2.g_genom FROM genoms g2 WHERE g2.g_id=(SELECT r3.r_genesis from rabbits r3 WHERE r3.r_id={0:d})))=0",femaleId)),
    (flt.safeBool(Filters.SHOW_CANDIDATE, false) 
        ? String.Format("(r_status>0 OR (r_status=0 AND r_group=1 AND (To_Days(NOW())-To_Days(r_born))>{0:d}) )",flt.safeInt(Filters.MAKE_CANDIDATE,120)) 
        : "r_status=2"),
    (flt.safeBool(Filters.SHOW_REST,false) ? "" ///показывать ли отдыхающих
        : String.Format("AND ( r_last_fuck_okrol IS NULL OR Date(NOW()) > Date(Date_Add(r_last_fuck_okrol,INTERVAL {0:d} DAY)) )",flt.safeInt(Filters.MALE_WAIT,0)) )
    //,(femaleId !=0 ? String.Format("(SELECT SUM(f_times)     FROM fucks WHERE f_partner=r_id AND f_rabid={0:d})",femaleId): "'0'"),
    //(femaleId !=0 ? String.Format("(SELECT SUM(f_children)  FROM fucks WHERE f_partner=r_id AND f_rabid={0:d})",femaleId): "'0'")
    ), sql);
#if DEBUG
            _logger.Debug(cmd.CommandText);
#endif
            MySqlDataReader rd = cmd.ExecuteReader();
#if DEBUG
            _logger.Debug("query is executed");
#endif
            Fucks f = new Fucks();
            while (rd.Read())
            {
                f.AddFuck(0, "",rd.GetString("fullname"), rd.GetInt32("r_id"), rd.GetInt32("fucks"), 
                    rd.IsDBNull(rd.GetOrdinal("fuckDate"))?DateTime.MinValue : rd.GetDateTime("fuckDate"),
                    DateTime.MinValue, "", rd.GetInt32("children"), rd.GetInt32("r_status"), rd.GetInt32("r_breed"),
                    rd.IsDBNull(rd.GetOrdinal("genom")) ? "" : rd.GetString("genom"), "", rd.GetInt32("age"), 0, false, "");
            }
            rd.Close();
            return f;
        }

        public static double[] getMaleChildrenProd(MySqlConnection sql, int rabid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT SUM(f_children) FROM fucks WHERE f_partner={0:d};",rabid), sql);
            int ch = 0;
            int ok = 0;
            double prod = 0;
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
                if (!rd.IsDBNull(0))
                    ch = rd.GetInt32(0);
            rd.Close();
            cmd.CommandText = String.Format(@"SELECT COUNT(f_partner) FROM fucks WHERE f_partner={0:d} AND f_state='okrol'",rabid);
            rd = cmd.ExecuteReader();
            if (rd.Read())
                if (!rd.IsDBNull(0))
                    ok = rd.GetInt32(0);
            rd.Close();
            if (ok != 0)
                prod = (double)ch / ok;
            return new double[] {ch, prod};
        }

        public static void changeWorker(MySqlConnection sql,int fid, int wid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE fucks SET f_worker={0:d} WHERE f_id={1:d};",wid,fid),sql);
            cmd.ExecuteNonQuery();
        }

        public static void changeFucker(MySqlConnection sql, int fid, int fucker)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT f_date FROM fucks WHERE f_id={0:d};",fid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            DateTime dt = DateTime.MinValue;
            if (rd.Read())
                if (!rd.IsDBNull(0))
                    dt=rd.GetDateTime(0);
            rd.Close();
            cmd.CommandText = String.Format(@"SELECT r_event_date FROM rabbits WHERE r_id={0:d};",fucker);
            DateTime ud = DateTime.MinValue;
            rd = cmd.ExecuteReader();
            if (rd.Read())
                if (!rd.IsDBNull(0))
                    ud = rd.GetDateTime(0);
            rd.Close();
            cmd.CommandText = String.Format("UPDATE fucks SET f_partner={0:d} WHERE f_id={1:d};",fucker,fid);
            cmd.ExecuteNonQuery();
            if (dt > ud)
            {
                cmd.CommandText = String.Format(@"UPDATE rabbits SET r_event_date={0:s} WHERE r_id={1:d};",DBHelper.DateToMyString(dt),fucker);
                cmd.ExecuteNonQuery();
            }
        }

        internal static List<string> getFuckMonths(MySqlConnection sql)
        {
            List<String> result = new List<String>();
            MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT Date_Format(f_date,'%m.%Y')dt FROM fucks WHERE f_date IS NOT null ORDER BY f_date DESC;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                result.Add(rd.GetString("dt"));
            }
            rd.Close();
            return result;
        }

        /// <summary>
        /// Отменяет Конец Случки, т.е. продолжает сукрольность.
        /// </summary>
        public static void cancelFuckEnd(MySqlConnection sql, int fuckId)
        {
            DateTime ev_date = DateTime.MinValue;
            int rabID = 0;
            string type ="";
            MySqlCommand cmd = new MySqlCommand("", sql);
            // достаем информацию, которую нужно востановить в таблице rabbits по данной крольчихе
            cmd.CommandText = String.Format("SELECT f_rabid,f_date,f_type FROM fucks f WHERE f_id={0:#};",fuckId);
            MySqlDataReader rd = cmd.ExecuteReader();

            if (rd.Read())
            {
                rabID = rd.GetInt32("f_rabid");
                if (!rd.IsDBNull(1))
                    ev_date = rd.GetDateTime("f_date");
                else ev_date = DateTime.Now.AddDays(-30);
                type = rd.GetString("f_type");
            }
            rd.Close();
            cmd.CommandText = String.Format("UPDATE fucks SET f_end_date=null, f_state='sukrol', f_notes='proholost cancel' WHERE f_id={0:#};", fuckId);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format("UPDATE rabbits SET r_event_date='{0}',r_event='{2}',r_rate=r_rate+2 WHERE r_id={1:d};", ev_date.ToString("yyyy-MM-dd"), rabID, type);
            cmd.ExecuteNonQuery();
        }

        public static void MakeFuck(MySqlConnection sql, int femaleId, int maleId, int daysPast, int worker,bool syntetic)
        {
            OneRabbit f = RabbitGetter.GetRabbit(sql, femaleId);
            String type = Fucks.Type.Sluchka_ENG;
            string when = DBHelper.DaysPastMySQLDate(daysPast);

            if (syntetic)
                type = Fucks.Type.Syntetic_ENG;
            else if (f.Status > 0)
                type = Fucks.Type.Vyazka_ENG;
            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE fucks SET f_last=0 WHERE f_rabid={0:d};", femaleId), sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(@"INSERT INTO fucks(f_rabid,f_date,f_partner,f_state,f_type,f_last,f_notes,f_worker) 
VALUES({0:d},{1:s},{2:d},'sukrol','{3:s}',1,'',{4:d});", femaleId, when, maleId, type, worker);
            cmd.ExecuteNonQuery();
            //            cmd.CommandText = String.Format("SELECT r_status,TODAYS(r_last_fuck_okrol FROM rabbits WHERE r_id=");
            int rate = 1;
            cmd.CommandText = String.Format("UPDATE rabbits SET r_event_date={0:s}, r_event='{1:s}', r_rate=r_rate+{3:d} WHERE r_id={2:d};",
                when, type, femaleId, rate);
            cmd.ExecuteNonQuery();
            if (!syntetic)///если ИО то не ставим, что самец работал
            {
                cmd.CommandText = String.Format("UPDATE rabbits SET r_last_fuck_okrol={0:s}, r_rate=r_rate+1 WHERE r_id={1:d};",
                    when, maleId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
