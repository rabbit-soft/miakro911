using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using log4net;

namespace rabnet
{
    public class ZooTehNullItem : IData
    {
        public int id = 0;
        public ZooTehNullItem(int id) { this.id=id;}
    }
    public class ZooTehNullGetter : IDataGetter
    {
        #region IDataGetter Members
        private int val;
        const int ZOOTEHITEMS = 9;
        public int getCount()
        {
            val = -1;
            return ZOOTEHITEMS;
        }

        public int getCount2()
        {
            return 0;
        }

        public void stop()
        {
            
        }

        public IData getNextItem()
        {
            val++;
            if (val > ZOOTEHITEMS) return null;
            return new ZooTehNullItem(val);
        }

        #endregion
    }

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
        public string breed;
        public ZooJobItem()
        {
        }
        public ZooJobItem Okrol(int id,String nm,String place,int age,int srok,int status,String br)
        {
            type = 1; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age; this.status = status;
            this.id = id;
            i[0] = srok;
            breed = br;
            return this;
        }
        public ZooJobItem Vudvor(int id, String nm, String place, int age, int srok, int status,int area,string tt,string dlm,String br,int suckers)
        {
            type = 2; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age; this.status = status;
            this.id = id; breed = br;
            i[2] = suckers;
            i[0] = srok;
            i[1] = area;
            if (i[1] == 1 && tt == "jurta")
                i[1] = 0;

            return this;
        }
        public ZooJobItem Counts(int id, String nm, String place, int age,int count,String br,int srok,int yid)
        {
            type = 3; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age; this.status = 0;
            this.id = id; breed = br;
            i[0] = count;
            i[1] = srok;
            i[2] = yid;
            return this;
        }
        public ZooJobItem Preokrol(int id, String nm, String place, int age, int srok,string br)
        {
            type = 4; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age;
            this.id = id; breed = br;
            i[0] = srok;
            return this;
        }
        public ZooJobItem BoysGirlsOut(int id, String nm, String place, int age, int srok,String br)
        {
            type = 5; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age;
            this.id = id; breed = br;
            i[0] = srok;
            return this;
        }
        public ZooJobItem Fuck(int id, String nm, String place, int age, int srok,int status,string boys,int group,string breed)
        {
            type = 6; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age; this.status = status;this.id = id;
            names = boys; this.breed = breed;
            i[0] = srok;
            i[1] = group;
            return this;
        }
        public ZooJobItem Vacc(int id, String nm, String place, int age, int srok,String br)
        {
            type = 7; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age; this.id = id; breed = br;
            i[0] = srok;
            return this;
        }
        public ZooJobItem SetNest(int id, String nm, String place, int age, int srok,int sukr,int children,String br)
        {
            type = 8; name = nm; this.place = Buildings.fullPlaceName(place);
            this.age = age; this.id = id; breed = br;
            i[0] = srok;
            i[1] = sukr;
            i[2] = children;
            return this;
        }
    }

    class ZooTehGetter
    {
        private MySqlConnection sql;
        protected static ILog log = log4net.LogManager.GetLogger(typeof(RabNetDataGetterBase));
        private Filters op;
        public ZooTehGetter(MySqlConnection sql,Filters f)
        {
            this.sql = sql;
            op = f;
        }
        
        public MySqlDataReader reader(String qry)
        {
            MySqlCommand cmd=new MySqlCommand(qry,sql);
            return cmd.ExecuteReader();
        }

        public String getnm(int def)
        {
            return op.safeInt("dbl")==1?"2":def.ToString();
        }
        public String getnm()
        {
            return getnm(0);
        }

        public String brd(String fld)
        {
            return String.Format("(SELECT {0:s} FROM breeds WHERE b_id={1:s}) breed", op.safeInt("shr") == 1 ? "b_short_name" : "b_name",fld);
        }
        public String brd()
        {
            return brd("r_breed");
        }

        public ZooJobItem[] getOkrols(int days)
        {
            MySqlDataReader rd=reader(String.Format(@"SELECT r_id,rabname(r_id,"+getnm()+@") name,rabplace(r_id) place,
(TO_DAYS(NOW())-TO_DAYS(r_event_date)) srok,r_status,(TO_DAYS(NOW())-TO_DAYS(r_born)) age,"+brd()+@" 
FROM rabbits WHERE r_sex='female' AND (TO_DAYS(NOW())-TO_DAYS(r_event_date))>={0:d} ORDER BY srok DESC,
0+LEFT(place,LOCATE(',',place)) ASC;", days));
            List<ZooJobItem> res=new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().Okrol(rd.GetInt32("r_id"),rd.GetString("name"),
                    rd.GetString("place"),rd.GetInt32("age"),rd.GetInt32("srok"),rd.GetInt32("r_status"),rd.GetString("breed")));
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getVudvors(int days)
        {
            MySqlDataReader rd = reader(String.Format(@"SELECT r_id,rabname(r_id," + getnm() + @") name,rabplace(r_id) place,r_tier,r_area,r_event_date,
(TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol)) srok,r_status,(TO_DAYS(NOW())-TO_DAYS(r_born)) age,
t_nest,t_id,t_busy1,t_busy2,t_delims,t_type," + brd() + @",
COALESCE((SELECT SUM(r3.r_group) FROM rabbits r3 WHERE r3.r_parent=rabbits.r_id),0) suckers
FROM rabbits,tiers
WHERE t_id=r_tier AND r_event_date IS NULL AND
r_sex='female' AND (TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol))>={0:d} AND
((t_busy1=r_id AND t_nest like '1%')OR(t_busy2=r_tier_id AND t_nest like '%1'))
ORDER BY srok DESC,0+LEFT(place,LOCATE(',',place)) ASC;", days));
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().Vudvor(rd.GetInt32("t_id"), rd.GetString("name"),
                    rd.GetString("place"), rd.GetInt32("age"), rd.GetInt32("srok"), rd.GetInt32("r_status"),
                    rd.GetInt32("r_area"),rd.GetString("t_type"),rd.GetString("t_delims"),rd.GetString("breed"),rd.GetInt32("suckers")));
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getCounts(int days,int next)
        {
            MySqlDataReader rd = reader(String.Format(@"SELECT r_parent,rabname(r_parent," + getnm() + @") name,
rabplace(r_parent) place,r_group,
(SELECT TO_DAYS(NOW())-TO_DAYS(r3.r_born) FROM rabbits r3 WHERE r3.r_id=rabbits.r_parent) age," 
+ brd("(SELECT r7.r_breed FROM rabbits r7 WHERE r7.r_id=rabbits.r_parent)") + @",
TO_DAYS(NOW())-TO_DAYS(r_born)-{0:d} srok,r_id
FROM rabbits WHERE r_parent<>0 AND (TO_DAYS(NOW())-TO_DAYS(r_born)>={0:d}{1:s}) AND
r_parent NOT IN (SELECT l_rabbit FROM logs WHERE l_type=17 AND (DATE(l_date)<=DATE(NOW()) AND 
DATE(l_date)>=DATE(NOW()- INTERVAL (TO_DAYS(NOW())-TO_DAYS(r_born)-{0:d}) DAY))) ORDER BY age DESC,
0+LEFT(place,LOCATE(',',place)) ASC;", days, 
   next==-1?"":String.Format(" AND TO_DAYS(NOW())-TO_DAYS(r_born)<{0:d}", next)));
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().Counts(rd.GetInt32("r_parent"),rd.GetString("name"),
                    rd.GetString("place"),rd.GetInt32("age"),rd.GetInt32("r_group"),rd.GetString("breed"),rd.GetInt32("srok"),rd.GetInt32("r_id")));
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getPreokrols(int days,int okroldays)
        {
            MySqlDataReader rd = reader(String.Format(@"SELECT r_id,rabname(r_id," + getnm() + @") name,rabplace(r_id) place,
(TO_DAYS(NOW())-TO_DAYS(r_event_date)) srok,r_status,(TO_DAYS(NOW())-TO_DAYS(r_born)) age," + brd() + @" 
FROM rabbits WHERE r_sex='female' AND (TO_DAYS(NOW())-TO_DAYS(r_event_date))>={0:d} AND (TO_DAYS(NOW())-TO_DAYS(r_event_date))<{1:d} AND
r_id NOT IN (SELECT l_rabbit FROM logs WHERE l_type=21 AND DATE(l_date)>=DATE(rabbits.r_event_date)) ORDER BY srok DESC
,0+LEFT(place,LOCATE(',',place)) ASC;", days, okroldays));
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().Preokrol(rd.GetInt32("r_id"), rd.GetString("name"),
                    rd.GetString("place"), rd.GetInt32("age"), rd.GetInt32("srok"),rd.GetString("breed")));
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getBoysGirlsOut(int days, OneRabbit.RabbitSex sex)
        {
            MySqlDataReader rd = reader(String.Format(@"SELECT r_parent,rabname(r_parent," + getnm() + @") name ,rabplace(r_parent) place, 
TO_DAYS(NOW())-TO_DAYS(r_born) age," + brd() + @" 
FROM rabbits WHERE r_parent<>0 AND {0:s} AND (TO_DAYS(NOW())-TO_DAYS(r_born))>={1:d} ORDER BY age DESC,
0+LEFT(place,LOCATE(',',place)) ASC;",
                  (sex==OneRabbit.RabbitSex.FEMALE?"r_sex='female'":"(r_sex='male' OR r_sex='void')"),days));
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().BoysGirlsOut(rd.GetInt32("r_parent"), rd.GetString("name"),
                    rd.GetString("place"), rd.GetInt32("age"), rd.GetInt32("age")-days,rd.GetString("breed")));
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getFucks(int statedays, int firstdays,int brideage,int malewait,bool heterosis,bool inbreeding,int type)
        {
            string query = String.Format(@"SELECT * FROM (SELECT r_id,rabname(r_id," + getnm(1) + @") name,rabplace(r_id) place,
TO_DAYS(NOW())-TO_DAYS(r_born) age,
coalesce((SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id),null,0) suckers,
r_status,
TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol) fromokrol," + (op.safeValue("prt") == "1" ? @"
(SELECT GROUP_CONCAT(rabname(r5.r_id,0) ORDER BY rabname(r5.r_id,0) SEPARATOR ',') FROM rabbits r5
WHERE r5.r_sex='male' AND r_status>0 AND 
(r5.r_last_fuck_okrol IS NULL OR TO_DAYS(NOW())-TO_DAYS(r5.r_last_fuck_okrol)>={3:d}){4:s}{5:s}) partners" : "'' partners") + @",
r_group,
(SELECT {6:s} FROM breeds WHERE b_id=r_breed) breed
FROM rabbits WHERE r_sex='female' AND r_event_date IS NULL AND r_status{7:s}) c 
WHERE age>{0:d} AND r_status=0 OR (r_status=1 AND (suckers=0 OR fromokrol>={1:d})) OR 
(r_status>1 AND (suckers=0 OR fromokrol>={2:d})) 
ORDER BY 0+LEFT(place,LOCATE(',',place)) ASC;",
    brideage, firstdays, statedays, malewait,
(heterosis ? "" : String.Format(" AND r5.r_breed=rabbits.r_breed")),
(inbreeding ? "" : String.Format(@" AND (SELECT COUNT(g_genom) FROM genoms WHERE g_id=rabbits.r_genesis AND g_genom IN (SELECT g2.g_genom FROM genoms g2 WHERE g2.g_id=r5.r_genesis))=0")),
(op.safeInt("shr") == 0 ? "b_name" : "b_short_name"), (type == 1 ? ">0" : "=0")
    );
            MySqlDataReader rd = reader(query);
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
                    rd.GetString("place"), age, srok, state,rd.IsDBNull(7)?"":rd.GetString("partners"),
                    rd.GetInt32("r_group"),rd.GetString("breed")));
            }
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getVacc(int days)
        {
            string query = String.Format(@"SELECT r_id,rabname(r_id," + getnm() + @") name,rabplace(r_id) place,
(TO_DAYS(NOW())-TO_DAYS(r_born)) age," + brd() + @" 
FROM rabbits WHERE (r_flags like '__0__' OR r_flags like '__1__' OR COALESCE(r_vaccine_end,NOW())<=NOW())  AND (TO_DAYS(NOW())-TO_DAYS(r_born))>={0:d} 
ORDER BY r_born DESC,0+LEFT(place,LOCATE(',',place)) ASC;", days);
            MySqlDataReader rd = reader(query);
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
                res.Add(new ZooJobItem().Vacc(rd.GetInt32("r_id"), rd.GetString("name"),
                    rd.GetString("place"), rd.GetInt32("age"), rd.GetInt32("age")-days,rd.GetString("breed")));
            rd.Close();
            return res.ToArray();
        }

        public ZooJobItem[] getSetNest(int wochild, int wchild)
        {
             string query = String.Format(@"SELECT * FROM (SELECT r_id,rabname(r_id," + getnm() + @") name,rabplace(r_id) place,
(TO_DAYS(NOW())-TO_DAYS(r_born)) age,
(TO_DAYS(NOW())-TO_DAYS(r_event_date)) sukr,
(SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id) children," + brd() + @" 
FROM rabbits WHERE r_sex='female' AND r_event_date IS NOT NULL) c 
WHERE (((children IS NULL AND sukr>={0:d}) OR (children>0 AND sukr>={1:d}))) AND
place NOT like '%,%,0,jurta,%,1' ORDER BY sukr DESC,0+LEFT(place,LOCATE(',',place)) ASC;", wochild, wchild);
            log.Debug("db.myasql.ZooTeh: "+query);
            MySqlDataReader rd = reader(query);
            List<ZooJobItem> res = new List<ZooJobItem>();
            while (rd.Read())
            {
                int child=rd.IsDBNull(5)?0:rd.GetInt32("children");
                int sukr=rd.GetInt32("sukr");
                res.Add(new ZooJobItem().SetNest(rd.GetInt32("r_id"), rd.GetString("name"),
                    rd.GetString("place"), rd.GetInt32("age"), (child > 0 ? sukr - wchild : sukr - wochild), sukr, child,rd.GetString("breed")));
            }
           rd.Close();
            return res.ToArray();

        }
    }
}
