using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using log4net;

namespace rabnet
{
    public class Fucks
    {
        public static class Type
        {
            public const string Null = "нет";
            public const string Vyazka_ENG = "vyazka";
            public const string Vyazka_rus = "вязка";
            public const string Sluchka_ENG = "sluchka";
            public const string Sluchka_rus = "случка";
            public const string Kuk_ENG = "kuk";
            public const string Kuk_rus = "кук";
            public const string Okrol_ENG = "okrol";
            public const string Okrol_rus = "окрол";
            public const string Proholost_ENG = "proholost";
            public const string Proholost_rus = "прохолостание";
            public const string Sukrol_rus = "сукрольна";
        }

        public class Fuck
        {
            public String partner;
            public int partnerid;
            public int id;
            public int times;
            public DateTime when;
            public String type;
            public DateTime enddate;
            public String status;
            public int children;
            public int dead;
            public int killed;
            public int added;
            public int breed;
            public String rgenom;
            public bool isDead;
            public string worker;
            public Fuck(int id,String p, int pid, int tms, DateTime s, DateTime e, String st, int ch, int dd, 
                int brd, String gen,String tp,int kl,int add,bool isdead,string wrk)
            {
                this.id = id;
                partner = p;
                partnerid = pid;
                worker = wrk;
                times = tms;
                when = s;enddate = e;
                type = "нет";
                if (tp == "vyazka") type = "вязка";
                if (tp == "sluchka") type = "случка";
                if (tp == "kuk") type = "кук";
                status = "сукрольна";
                if (st == "okrol") status = "окрол";
                if (st == "proholost") status = "прохолостание";
                children = ch; dead = dd;
                isDead = isdead;
                killed=kl;
                added = add;
                breed = brd;
                rgenom = gen;
            }
        }
        public List<Fuck> fucks = new List<Fuck>();
        public void addFuck(int id,String p,int pid,int tms,DateTime s,DateTime e,String st,int ch,int dd,
            int brd,String gen,String tp,int kl,int add,bool dead,String wrk)
        {
            fucks.Add(new Fuck(id,p,pid,tms,s,e,st,ch,dd,brd,gen,tp,kl,add,dead,wrk));
        }

    }

    class FucksGetter
    {
        public static Fucks GetFucks(MySqlConnection sql, int rabbit)
        {

            MySqlCommand cmd = new MySqlCommand(String.Format(@"(SELECT f_id,f_date,f_partner,f_times,f_state,f_date,f_end_date,
(SELECT u_name FROM users WHERE u_id=f_worker) worker, 
f_children,f_dead,f_type,
deadname(f_partner,2) partner,
(SELECT r_breed FROM dead WHERE r_id=f_partner) breed,
(SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=(SELECT r_genesis FROM dead WHERE r_id=f_partner)) genom,
f_killed,f_added,1 dead
FROM fucks WHERE f_rabid={0:d} AND isdead(f_partner)=1 ORDER BY f_date)
UNION
(SELECT f_id,f_date,f_partner,f_times,f_state,f_date,f_end_date,
(SELECT u_name FROM users WHERE u_id=f_worker) worker,f_children,f_dead,f_type,
rabname(f_partner,2) partner,
(SELECT r_breed FROM rabbits WHERE r_id=f_partner) breed,
(SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=(SELECT r_genesis FROM rabbits WHERE r_id=f_partner)) genom,
f_killed,f_added,0 dead
FROM fucks WHERE f_rabid={0:d} AND isdead(f_partner)=0 ORDER BY f_date);", rabbit), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            Fucks f = new Fucks();
            while (rd.Read())
            {
                f.addFuck(rd.GetInt32("f_id"), rd.GetString("partner"), rd.GetInt32("f_partner"), rd.GetInt32("f_times"),
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

        public static Fucks AllFuckers(MySqlConnection sql, int female,bool geterosis,bool inbreeding,int malewait)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT 
r_id,
rabname(r_id,2) fullname,
r_status,
r_breed,
(SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=r_genesis) genom,
{4:s} fucks,
{5:s} children
FROM rabbits 
WHERE r_sex='male' AND r_status>0 AND (r_last_fuck_okrol IS NULL OR TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol)>={1:d}){2:s}{3:s} ORDER BY fullname;",
female,
malewait,
(geterosis ? "" : String.Format(" AND r_breed=(SELECT r2.r_breed FROM rabbits r2 WHERE r_id={0:d})",female)),
(inbreeding ? "" : String.Format(@" AND (SELECT COUNT(g_genom) FROM genoms WHERE g_id=r_genesis AND g_genom IN 
    (SELECT g2.g_genom FROM genoms g2 WHERE g2.g_id=(SELECT r3.r_genesis from rabbits r3 WHERE r3.r_id={0:d})))=0",female)),
(female !=0 ? String.Format("(SELECT SUM(f_times)     FROM fucks WHERE f_partner=r_id AND f_rabid={0:d})",female): "'0'"),
(female !=0 ? String.Format("(SELECT SUM(f_children)  FROM fucks WHERE f_partner=r_id AND f_rabid={0:d})",female): "'0'")
), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            Fucks f = new Fucks();
            while (rd.Read())
            {
                f.addFuck(0,rd.GetString("fullname"), rd.GetInt32("r_id"), rd.IsDBNull(5)?0:rd.GetInt32("fucks"), DateTime.MinValue,
                    DateTime.MinValue, "", rd.IsDBNull(6) ? 0 : rd.GetInt32("children"), rd.GetInt32("r_status"), rd.GetInt32("r_breed"),
                    rd.IsDBNull(4)?"":rd.GetString("genom"), "",0,0,false,"");
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
    }
}
