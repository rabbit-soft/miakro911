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
            public int breed;
            public String rgenom;
            public Fuck(String p, int pid, int tms, DateTime s, DateTime e, String st, int ch, int dd, 
                int brd, String gen,String tp,int kl)
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
                killed=kl;
                breed = brd;
                rgenom = gen;
            }
        }
        public List<Fuck> fucks=new List<Fuck>();
        public void addFuck(String p,int pid,int tms,DateTime s,DateTime e,String st,int ch,int dd,
            int brd,String gen,String tp,int kl)
        {
            fucks.Add(new Fuck(p,pid,tms,s,e,st,ch,dd,brd,gen,tp,kl));
        }
    }

    public class FucksGetter
    {
        public static Fucks GetFucks(MySqlConnection sql,int rabbit)
        {
            MySqlCommand cmd=new MySqlCommand(@"SELECT f_id,f_date,f_partner,f_times,f_state,f_date,f_end_date,f_children,f_dead,f_type,
rabname(f_partner,2) partner,
(SELECT r_breed FROM rabbits WHERE r_id=f_partner) breed,
(SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=(SELECT r_genesis FROM rabbits WHERE r_id=f_partner)) genom,
f_killed
FROM fucks WHERE f_rabid=" + rabbit.ToString()+" ORDER BY f_date;",sql);
            MySqlDataReader rd=cmd.ExecuteReader();
            Fucks f=new Fucks();
            while(rd.Read())
            {
                f.addFuck(rd.GetString("partner"),rd.GetInt32("f_partner"),rd.GetInt32("f_times"),
                    rd.IsDBNull(5)?DateTime.MinValue:rd.GetDateTime("f_date"),
                    rd.IsDBNull(6)?DateTime.MinValue:rd.GetDateTime("f_end_date"),
                    rd.GetString("f_state"),rd.GetInt32("f_children"),rd.GetInt32("f_dead"),
                    rd.GetInt32("breed"),rd.GetString("genom"),rd.GetString("f_type"),
                    rd.GetInt32("f_killed")
                    );

            }
            rd.Close();
            return f;
        }

        public static Fucks AllFuckers(MySqlConnection sql, int female)
        {
            MySqlCommand cmd = new MySqlCommand(@"SELECT r_id,rabname(r_id,2) fullname,r_status,
r_breed,
(SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=r_genesis) genom,
(SELECT SUM(f_times) FROM fucks WHERE f_partner=r_id AND f_rabid="+female.ToString()+@") fucks,
(SELECT SUM(f_children) FROM fucks WHERE f_partner=r_id AND f_rabid=" + female.ToString() + @") children
FROM rabbits WHERE r_sex='male' AND r_status>0;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            Fucks f = new Fucks();
            while (rd.Read())
            {
                f.addFuck(rd.GetString("fullname"), rd.GetInt32("r_id"), rd.IsDBNull(5)?0:rd.GetInt32("fucks"), DateTime.MinValue,
                    DateTime.MinValue, "", rd.IsDBNull(6) ? 0 : rd.GetInt32("children"), rd.GetInt32("r_status"), rd.GetInt32("r_breed"),
                    rd.GetString("genom"), "",0);
            }
            rd.Close();
            return f;
        }
    }
}
