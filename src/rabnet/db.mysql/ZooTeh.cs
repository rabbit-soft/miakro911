using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public class ZooJobItem
    {
        public int type;
        public int id;
        public String name;
        public int age;
        public String place;
        public int status;
        public DateTime date;
        public string names;
        public int[] i = new int[10];
        public string[] s = new string[10];
        public ZooJobItem()
        {
        }
        public ZooJobItem Okrol(int id,String nm,String place,int age,int srok,int status)
        {
            type = 1; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age; this.status = status;
            this.id = id;
            i[0] = srok;
            return this;
        }
        public ZooJobItem Vudvor(int id, String nm, String place, int age, int srok, int status,int area,string tt,string dlm)
        {
            type = 2; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age; this.status = status;
            this.id = id;
            i[0] = srok;
            i[1] = area;
            if (i[1] == 1 && tt == "jurta")
                i[1] = 0;
            return this;
        }
        public ZooJobItem Counts(int id, String nm, String place, int age,int count)
        {
            type = 3; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age; this.status = 0;
            this.id = id;
            i[0] = count;
            return this;
        }
        public ZooJobItem Preokrol(int id, String nm, String place, int age, int srok)
        {
            type = 4; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age;
            this.id = id;
            i[0] = srok;
            return this;
        }
        public ZooJobItem BoysGirlsOut(int id, String nm, String place, int age, int srok)
        {
            type = 5; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age;
            this.id = id;
            i[0] = srok;
            return this;
        }
        public ZooJobItem Fuck(int id, String nm, String place, int age, int srok,int status,string boys)
        {
            type = 6; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age; this.status = status;this.id = id;
            names = boys;
            i[0] = srok;
            return this;
        }
        public ZooJobItem Vacc(int id, String nm, String place, int age, int srok)
        {
            type = 7; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age; this.id = id;
            i[0] = srok;
            return this;
        }
    }
    /*
    public class ZooTehItem:IData
    {
        public DateTime dt;
        public int id;
        public String job;
        public String rabbit;
        public int irab;
        public int done;
        public String notes;
        public int level;
        public string address;
        public int age;
        public int type;
        public ZooTehItem(int type,int id,string name,string r,int ir,int res,string nts,int l,DateTime dt,String adr,int age)
        {
            this.id = id;
            job = name;
            rabbit = r;
            irab = ir;
            done = res;
            notes = nts;
            level = l;
            this.dt = dt;
            address = adr;
            this.age = age;
        }
    }

    class ZooTeh:RabNetDataGetterBase
    {
        public ZooTeh(MySqlConnection sql,Filters filters):base(sql,filters)
        {
        }

        public override IData nextItem()
        {
            return new ZooTehItem(0,rd.GetInt32("z_id"), rd.GetString("work"), rd.GetString("rname"), rd.GetInt32("z_rabbit"),
                rd.GetInt32("z_done"), rd.GetString("z_notes"), rd.GetInt32("z_level"), rd.GetDateTime("z_date"),
                Buildings.fullPlaceName(rd.GetString("rplace")),rd.GetInt32("age"));
        }

        public override string getQuery()
        {
            return @"SELECT z_id,z_date,z_job,
(SELECT j_name FROM jobs WHERE j_id=z_job) work,
z_done,z_job,z_level,z_rabbit,
rabname(z_rabbit,1) rname,
(SELECT TO_DAYS(NOW())-TO_DAYS(r_born) FROM rabbits WHERE r_id=z_rabbit) age,
rabplace(z_rabbit) rplace,
z_notes FROM zooplans WHERE z_done=0 AND z_rabbit IS NOT NULL;";
        }

        public override string countQuery()
        {
            return "SELECT COUNT(*) FROM zooplans WHERE z_done=0  AND z_rabbit IS NOT NULL;";
        }
    }
    */
    class ZooTehGetter
    {
        private MySqlConnection sql;
        public ZooTehGetter(MySqlConnection sql)
        {
            this.sql = sql;
        }
        
        public MySqlDataReader reader(String qry)
        {
            MySqlCommand cmd=new MySqlCommand(qry,sql);
            return cmd.ExecuteReader();
        }

        public ZooJobItem[] getOkrols(int days)
        {
            MySqlDataReader rd=reader(String.Format(@"SELECT r_id,rabname(r_id,2) name,rabplace(r_id) place,
(TO_DAYS(NOW())-TO_DAYS(r_event_date)) srok,r_status,(TO_DAYS(NOW())-TO_DAYS(r_born)) age
FROM rabbits WHERE r_sex='female' AND (TO_DAYS(NOW())-TO_DAYS(r_event_date))>={0:d} ORDER BY srok DESC;", days));
            List<ZooJobItem> res=new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().Okrol(rd.GetInt32("r_id"),rd.GetString("name"),
                    rd.GetString("place"),rd.GetInt32("age"),rd.GetInt32("srok"),rd.GetInt32("r_status")));
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getVudvors(int days)
        {
            MySqlDataReader rd = reader(String.Format(@"SELECT r_id,rabname(r_id,2) name,rabplace(r_id) place,r_tier,r_area,r_event_date,
(TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol)) srok,r_status,(TO_DAYS(NOW())-TO_DAYS(r_born)) age,
t_nest,t_id,t_busy1,t_busy2,t_delims,t_type
FROM rabbits,tiers
WHERE t_id=r_tier AND r_event_date IS NULL AND
r_sex='female' AND (TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol))>={0:d} AND
((t_busy1=r_id AND t_nest like '1%')OR(t_busy2=r_tier_id AND t_nest like '%1'))
ORDER BY srok DESC;", days));
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().Vudvor(rd.GetInt32("t_id"), rd.GetString("name"),
                    rd.GetString("place"), rd.GetInt32("age"), rd.GetInt32("srok"), rd.GetInt32("r_status"),
                    rd.GetInt32("r_area"),rd.GetString("t_type"),rd.GetString("t_delims")));
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getCounts(int days)
        {
            MySqlDataReader rd = reader(String.Format(@"SELECT r_parent,rabname(r_parent,2) name,
rabplace(r_parent) place,r_group 
FROM rabbits WHERE r_parent<>0 AND TO_DAYS(NOW())-TO_DAYS(r_born)={0:d} AND
r_parent NOT IN (SELECT l_rabbit FROM logs WHERE l_type=17 AND DATE(l_date)=DATE(NOW()));", days));
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().Counts(rd.GetInt32("r_parent"),rd.GetString("name"),
                    rd.GetString("place"),days,rd.GetInt32("r_group")));
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getPreokrols(int days,int okroldays)
        {
            MySqlDataReader rd = reader(String.Format(@"SELECT r_id,rabname(r_id,2) name,rabplace(r_id) place,
(TO_DAYS(NOW())-TO_DAYS(r_event_date)) srok,r_status,(TO_DAYS(NOW())-TO_DAYS(r_born)) age
FROM rabbits WHERE r_sex='female' AND (TO_DAYS(NOW())-TO_DAYS(r_event_date))>={0:d} AND (TO_DAYS(NOW())-TO_DAYS(r_event_date))<{1:d} AND
r_id NOT IN (SELECT l_rabbit FROM logs WHERE l_type=21 AND DATE(l_date)>=DATE(rabbits.r_event_date)) ORDER BY srok DESC;", days,okroldays));
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().Preokrol(rd.GetInt32("r_id"), rd.GetString("name"),
                    rd.GetString("place"), rd.GetInt32("age"), rd.GetInt32("srok")));
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getBoysGirlsOut(int days, OneRabbit.RabbitSex sex)
        {
            MySqlDataReader rd=reader(String.Format(@"SELECT r_parent,rabname(r_parent,2) name ,rabplace(r_parent) place, 
TO_DAYS(NOW())-TO_DAYS(r_born) age 
FROM rabbits WHERE r_parent<>0 AND {0:s} AND (TO_DAYS(NOW())-TO_DAYS(r_born))>={1:d} ORDER BY age DESC;",
                  (sex==OneRabbit.RabbitSex.FEMALE?"r_sex='female'":"(r_sex='male' OR r_sex='void')"),days));
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().BoysGirlsOut(rd.GetInt32("r_parent"), rd.GetString("name"),
                    rd.GetString("place"), rd.GetInt32("age"), rd.GetInt32("age")-days));
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getFucks(int statedays, int firstdays,int brideage,int malewait,bool heterosis,bool inbreeding)
        {
            MySqlDataReader rd = reader(String.Format(@"SELECT * FROM (SELECT r_id,rabname(r_id,2) name,rabplace(r_id) place,
TO_DAYS(NOW())-TO_DAYS(r_born) age,
(SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id) suckers,
r_status,
TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol) fromokrol,
(SELECT GROUP_CONCAT(rabname(r5.r_id,0) ORDER BY rabname(r5.r_id,0) SEPARATOR ',') FROM rabbits r5
WHERE r5.r_sex='male' AND r_status>0 AND 
(r5.r_last_fuck_okrol IS NULL OR TO_DAYS(NOW())-TO_DAYS(r5.r_last_fuck_okrol)>={3:d}){4:s}{5:s}) partners
FROM rabbits WHERE r_sex='female' AND r_event_date IS NULL ) c 
WHERE age>{0:d} AND (r_status=0 OR (r_status=1 AND (suckers=0 OR fromokrol>={1:d})) OR (r_status>1 AND (suckers=0 OR fromokrol>={2:d})));",
    brideage,firstdays,statedays,malewait,
(heterosis ? "" : String.Format(" AND r5.r_breed=rabbits.r_breed")),
(inbreeding ? "" : String.Format(@" AND (SELECT COUNT(g_genom) FROM genoms WHERE g_id=rabbits.r_genesis AND g_genom IN 
(SELECT g2.g_genom FROM genoms g2 WHERE g2.g_id=r5.r_genesis))=0"))
    ));
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
            {
                int age = rd.GetInt32("age");
                int state = rd.GetInt32("r_status");
                int fromok = rd.IsDBNull(6)?0:rd.GetInt32("fromokrol");
                int suck = rd.IsDBNull(4)?0:rd.GetInt32("suckers");
                int srok = 0;
                if (state == 0)
                    srok = age - brideage;
                if (state > 0)
                {
                    if (suck > 0)
                        srok = fromok - (state == 1 ? firstdays : statedays);
                    else srok = fromok;
                }
                res.Add(new ZooJobItem().Fuck(rd.GetInt32("r_id"), rd.GetString("name"),
                    rd.GetString("place"), age, srok, state,rd.IsDBNull(7)?"":rd.GetString("partners")));
            }
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getVacc(int days)
        {
            MySqlDataReader rd = reader(String.Format(@"SELECT r_id,rabname(r_id,2) name,rabplace(r_id) place,
(TO_DAYS(NOW())-TO_DAYS(r_born)) age
FROM rabbits WHERE (r_flags like '__0__' OR r_flags like '__1__')  AND (TO_DAYS(NOW())-TO_DAYS(r_born))>={0:d} ORDER BY r_born DESC;", days));
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().Vacc(rd.GetInt32("r_id"), rd.GetString("name"),
                    rd.GetString("place"), rd.GetInt32("age"), rd.GetInt32("age")-days));
            rd.Close();
            return res.ToArray();
        }
    }
}
