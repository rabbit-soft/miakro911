using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
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
        public ZooTehItem(int id,string name,string r,int ir,int res,string nts,int l,DateTime dt,String adr,int age)
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
            return new ZooTehItem(rd.GetInt32("z_id"), rd.GetString("work"), rd.GetString("rname"), rd.GetInt32("z_rabbit"),
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
}
