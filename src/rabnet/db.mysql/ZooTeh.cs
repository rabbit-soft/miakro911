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
        public int[] i = new int[10];
        public string[] s = new string[10];
        public ZooJobItem()
        {
        }
        public ZooJobItem Okrol(int id,String nm,String place,int age,int srok,int status)
        {
            type = 1; name = nm; place = Buildings.fullPlaceName(place);
            this.age = age; this.status = status;
            this.id = id;
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

    }
}
