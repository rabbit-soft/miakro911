using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    public class Dead : IData
    {
        public int id;
        public string name;
        public string address;
        public int age;
        public DateTime deadDate;
        public string reason;
        public string notes;
        public string breed;
        public int group;
        public Dead(int id,string nm,string ad,int ag,DateTime dd,string rsn,string nts,string brd,int grp)
        {
            this.id = id;  name = nm;
            address = Buildings.fullPlaceName(ad);
            age = ag; deadDate = dd;
            reason = rsn;
            notes = nts; breed = brd;
            group = grp;
        }
    }

    class Deads:RabNetDataGetterBase
    {
        public Deads(MySqlConnection sql, Filters f)
            : base(sql, f)
        {
        }

        public override IData nextItem()
        {
            return new Dead(rd.GetInt32("r_id"), rd.GetString("name"), rd.GetString("place"),
                rd.GetInt32("age"), rd.GetDateTime("d_date"),
                rd.IsDBNull(6) ? "" : rd.GetString("reason"), rd.IsDBNull(7) ? "" : rd.GetString("d_notes"),
                rd.GetString("breed"),rd.GetInt32("r_group"));
        }

        public string makeWhere()
        {
            string wh = "";
            if (options.safeValue("nm") != "")
                wh = addWhereAnd(wh, "deadname(r_id,2) like '%"+options.safeValue("nm")+"%'");
            if (wh == "")
                return "";
            return " WHERE "+wh;
        }

        public override string getQuery()
        {
            int max = options.safeInt("max", 1000);
            return String.Format(@"SELECT r_id,deadname(r_id,2) name,deadplace(r_id) place,
TO_DAYS(d_date)-TO_DAYS(r_born) age,d_date,
(SELECT b_name FROM breeds WHERE b_id=dead.r_breed) breed,
(SELECT d_name FROM deadreasons WHERE d_id=dead.d_reason) reason,
d_notes,r_group
FROM dead{0:s} ORDER BY d_date DESC LIMIT {1:d};", makeWhere(),max);
        }

        public override string countQuery()
        {
            return String.Format(@"SELECT COUNT(*) FROM (SELECT r_id,deadname(r_id,2),d_date,TO_DAYS(d_date)-TO_DAYS(r_born) age 
FROM dead{0:s} LIMIT {1:d}) c;", makeWhere(), options.safeInt("max", 1000));
        }
    }

    class DeadHelper
    {
        private MySqlConnection sql=null;
        public DeadHelper(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public void resurrect(int rabbit)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"CALL resurrectRabbit({0:d});",rabbit), sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format("SELECT r_id,r_name,r_tier,r_farm,r_tier_id,r_area FROM rabbits WHERE r_id={0:d}",rabbit);
            MySqlDataReader rd=cmd.ExecuteReader();
            int name = 0;
            int tid = 0;
            int sec = 0;
            if (rd.Read())
            {
                name = rd.GetInt32("r_name");
                tid = rd.GetInt32("r_tier");
                sec = rd.GetInt32("r_area");
            }
            rd.Close();
            cmd.CommandText = String.Format(@"UPDATE names SET n_use={0:d},n_block_date=NULL WHERE n_id={0:d};",name);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(@"UPDATE tiers SET t_busy{0:d}={1:d} WHERE t_id={2:d};",sec+1,rabbit,tid);
            cmd.ExecuteNonQuery();
        }
    }
}
