using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using rabnet;

namespace db.mysql
{
    class Deads : RabNetDataGetterBase
    {
        public Deads(MySqlConnection sql, Filters f)
            : base(sql, f)
        {
        }

        public override IData NextItem()
        {
            return new Dead(_rd.GetInt32("r_id"), _rd.GetString("name"), _rd.GetString("place"),
                _rd.GetInt32("age"), _rd.GetDateTime("d_date"),
                _rd.IsDBNull(6) ? "" : _rd.GetString("reason"),
                _rd.IsDBNull(7) ? "" : _rd.GetString("d_notes"),
                _rd.GetString("breed"), _rd.GetInt32("r_group"));
        }

        public string makeWhere()
        {
            string wh = "";
            if (options.safeValue("nm") != "") {
                wh = addWhereAnd(wh, "deadname(r_id,2) like '%" + options.safeValue("nm") + "%'");
            }
            if (wh == "") {
                return "";
            }
            return " WHERE " + wh;
        }

        protected override string getQuery()
        {
            int max = options.safeInt("max", 1000);
            return String.Format(@"SELECT r_id, deadname(r_id,2) name, deadplace(r_id) place,
DATEDIFF(d_date, r_born) age, 
d_date,
(SELECT b_name FROM breeds WHERE b_id=dead.r_breed) breed,
(SELECT d_name FROM deadreasons WHERE d_id=dead.d_reason) reason,
d_notes, r_group
FROM dead {0:s} 
ORDER BY d_date DESC LIMIT {1:d};", makeWhere(), max);
        }

        protected override string countQuery()
        {
            return String.Format(@"SELECT 
    COUNT(1) 
FROM (SELECT r_id, deadname(r_id,2), d_date, TO_DAYS(d_date)-TO_DAYS(r_born) age FROM dead{0:s} LIMIT {1:d}) c;", makeWhere(), options.safeInt("max", 1000));
        }
    }

    class DeadHelper
    {
        private MySqlConnection sql = null;

        public DeadHelper(MySqlConnection sql)
        {
            this.sql = sql;
        }

        /// <summary>
        /// Востановление списанного кролика
        /// </summary>
        /// <param name="rabbit">ID кролика</param>
        public void ResurrectRabbit(int rabbit)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"CALL resurrectRabbit({0:d});", rabbit), sql);
            cmd.ExecuteNonQuery();
            //OneRabbit rab = RabbitGetter.GetRabbit(sql, rabbit);
            cmd.CommandText = String.Format("SELECT r_id, r_name, r_tier, r_farm, r_tier_id, r_area FROM rabbits WHERE r_id={0:d}", rabbit);
            MySqlDataReader rd = cmd.ExecuteReader();
            int name = 0;
            int tid = 0;
            int fid = 0;
            int lev = 0;
            int sec = 0;
            if (rd.Read()) {
                name = DBHelper.GetNullableInt(rd, "r_name");
                tid = DBHelper.GetNullableInt(rd, "r_tier");
                sec = rd.GetInt32("r_area");
                fid = DBHelper.GetNullableInt(rd, "r_farm");
                lev = rd.GetInt32("r_tier_id");
            }
            rd.Close();

            if (name != 0) {
                cmd.CommandText = String.Format("SELECT n_use FROM names WHERE n_id = {0:d};", name);
                rd = cmd.ExecuteReader();
                if (rd.Read()) {
                    if (DBHelper.GetNullableInt(rd, "n_use") != 0) {
                        name = 0;
                    }
                } else {
                    name = 0;
                }
                rd.Close();
            }
            if (fid != 0) {
                cmd.CommandText = String.Format(@"SELECT t_id, t_busy{0:d} FROM tiers, minifarms WHERE m_id={2:d} AND ((t_id=m_upper AND {1:d}<>1) OR (t_id=m_lower AND {1:d}=1));", sec + 1, lev, fid);
                rd = cmd.ExecuteReader();
                if (rd.Read()) {
                    tid = rd.GetInt32(0);
                    if (rd.GetInt32(1) != 0) {
                        fid = 0;
                    }
                } else {
                    fid = 0;
                }
                rd.Close();
            }

            if (name != 0) {
                cmd.CommandText = String.Format(@"UPDATE names SET n_use = {0:d}, n_block_date = NULL WHERE n_id = {0:d};", name);
            } else {
                cmd.CommandText = String.Format(@"UPDATE rabbits SET r_name = NULL WHERE r_id = {0:d};", rabbit);
            }
            cmd.ExecuteNonQuery();

            if (fid != 0) {
                cmd.CommandText = String.Format(@"UPDATE tiers SET t_busy{0:d} = {1:d} WHERE t_id = {2};", sec + 1, rabbit, DBHelper.Nullable(tid));
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format(@"UPDATE rabbits SET r_tier = {0} WHERE r_id = {1:d};", DBHelper.Nullable(tid), rabbit);
            } else {
                cmd.CommandText = String.Format(@"UPDATE rabbits SET r_farm = NULL, r_tier_id = 0, r_area = 0, r_tier = NULL WHERE r_id = {0:d};", rabbit);
            }
            cmd.ExecuteNonQuery();
        }

        public static List<string> getDeadMonths(MySqlConnection sql)
        {
            List<String> result = new List<String>();
            MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT Date_Format(d_date,'%m.%Y')dt FROM dead WHERE d_date IS NOT null ORDER BY d_date DESC;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read()) {
                result.Add(rd.GetString("dt"));
            }
            rd.Close();
            return result;
        }

        internal static void changeDeadReason(MySqlConnection sql, int rid, int reason)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE dead SET d_reason = {1:d} WHERE r_id = {0:d};", rid, reason), sql);
            cmd.ExecuteNonQuery();///todo эта операция выполняется очень долго и выпадает 
        }
    }
}
