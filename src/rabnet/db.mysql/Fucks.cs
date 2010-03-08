//#define TRIAL
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public class Fucks
    {
        public class Fuck
        {
            public String partner;
            public int partnerid;
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
            public Fuck(String p, int pid, int tms, DateTime s, DateTime e, String st, int ch, int dd, 
                int brd, String gen,String tp,int kl,int add,bool isdead)
            {
                partner = p;partnerid = pid;
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
        public List<Fuck> fucks=new List<Fuck>();
        public void addFuck(String p,int pid,int tms,DateTime s,DateTime e,String st,int ch,int dd,
            int brd,String gen,String tp,int kl,int add,bool dead)
        {
            fucks.Add(new Fuck(p,pid,tms,s,e,st,ch,dd,brd,gen,tp,kl,add,dead));
        }
    }

    class FucksGetter
    {
        public static Fucks GetFucks(MySqlConnection sql,int rabbit)
        {
            MySqlCommand cmd=new MySqlCommand(String.Format(@"(SELECT f_id,f_date,f_partner,f_times,f_state,f_date,f_end_date,f_children,f_dead,f_type,
deadname(f_partner,2) partner,
(SELECT r_breed FROM dead WHERE r_id=f_partner) breed,
(SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=(SELECT r_genesis FROM dead WHERE r_id=f_partner)) genom,
f_killed,f_added,1 dead
FROM fucks WHERE f_rabid={0:d} AND isdead(f_partner)=1 ORDER BY f_date)
UNION
(SELECT f_id,f_date,f_partner,f_times,f_state,f_date,f_end_date,f_children,f_dead,f_type,
rabname(f_partner,2) partner,
(SELECT r_breed FROM rabbits WHERE r_id=f_partner) breed,
(SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=(SELECT r_genesis FROM rabbits WHERE r_id=f_partner)) genom,
f_killed,f_added,0 dead
FROM fucks WHERE f_rabid={0:d} AND isdead(f_partner)=0 ORDER BY f_date);",rabbit),sql);
            MySqlDataReader rd=cmd.ExecuteReader();
            Fucks f=new Fucks();
            while(rd.Read())
            {
                f.addFuck(rd.GetString("partner"),rd.GetInt32("f_partner"),rd.GetInt32("f_times"),
                    rd.IsDBNull(5)?DateTime.MinValue:rd.GetDateTime("f_date"),
                    rd.IsDBNull(6)?DateTime.MinValue:rd.GetDateTime("f_end_date"),
                    rd.GetString("f_state"),rd.GetInt32("f_children"),rd.GetInt32("f_dead"),
                    rd.GetInt32("breed"),rd.IsDBNull(12)?"":rd.GetString("genom"),rd.GetString("f_type"),
                    rd.GetInt32("f_killed"),rd.GetInt32("f_added"),(rd.GetInt32("dead")==1)
                    );

            }
            rd.Close();
#if TRIAL
            Buildings.checkFarms3(sql);
#endif
            return f;
        }

        public static Fucks AllFuckers(MySqlConnection sql, int female,bool geterosis,bool inbreeding,int malewait)
        {
#if TRIAL
            Buildings.checkFarms3(sql);
#endif
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT r_id,rabname(r_id,2) fullname,r_status,
r_breed,
(SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=r_genesis) genom,
(SELECT SUM(f_times) FROM fucks WHERE f_partner=r_id AND f_rabid={0:d}) fucks,
(SELECT SUM(f_children) FROM fucks WHERE f_partner=r_id AND f_rabid={0:d}) children
FROM rabbits WHERE r_sex='male' AND r_status>0 AND (r_last_fuck_okrol IS NULL OR TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol)>={1:d}){2:s}{3:s} ORDER BY fullname;",
female,malewait,
(geterosis?"":String.Format(" AND r_breed=(SELECT r2.r_breed FROM rabbits r2 WHERE r_id={0:d})",female)),
(inbreeding?"":String.Format(@" AND (SELECT COUNT(g_genom) FROM genoms WHERE g_id=r_genesis AND g_genom IN 
(SELECT g2.g_genom FROM genoms g2 WHERE g2.g_id=(SELECT r3.r_genesis from rabbits r3 WHERE r3.r_id={0:d})))=0",female))
), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            Fucks f = new Fucks();
            while (rd.Read())
            {
                f.addFuck(rd.GetString("fullname"), rd.GetInt32("r_id"), rd.IsDBNull(5)?0:rd.GetInt32("fucks"), DateTime.MinValue,
                    DateTime.MinValue, "", rd.IsDBNull(6) ? 0 : rd.GetInt32("children"), rd.GetInt32("r_status"), rd.GetInt32("r_breed"),
                    rd.IsDBNull(4)?"":rd.GetString("genom"), "",0,0,false);
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
    }
}
