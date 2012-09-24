using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using rabnet;

namespace db.mysql
{
    class RabbitsDataGetter : RabNetDataGetterBase
    {
        public RabbitsDataGetter(MySqlConnection sql, Filters opts) : base(sql, opts) { }

        internal static AdultRabbit fillAdultRabbit(MySqlDataReader rd)
        {
            return new AdultRabbit(rd.GetInt32("r_id"), rd.GetString("name"), rd.GetString("r_sex"),
                rd.IsDBNull(rd.GetOrdinal("r_born")) ? DateTime.MinValue : rd.GetDateTime("r_born"),
                rd.GetString("breed"), rd.GetInt32("r_group"), rd.GetInt32("r_rate"),
                rd.GetString("r_bon"), rd.GetString("place"), rd.GetString("r_notes"),
                rd.GetString("r_flags"), rd.GetInt32("weight"), rd.GetInt32("r_status"),
                rd.IsDBNull(rd.GetOrdinal("r_event_date")) ? DateTime.MinValue : rd.GetDateTime("r_event_date"),
                rd.IsDBNull(rd.GetOrdinal("suckers")) ? 0 : rd.GetInt32("suckers"),
                rd.IsDBNull(rd.GetOrdinal("aage")) ? 0 : rd.GetInt32("aage"),
                rd.GetString("vaccines"));
        }

        public override IData nextItem() /*получение одной записи в Поголовье*/
        {
            return fillAdultRabbit(rd);
        }

        public override string getQuery()
        {
            String fld = "b_name";
            if (options.safeBool("shr"))
                fld = "b_short_name";
            return String.Format(@" SELECT * FROM (SELECT r_id, 
rabname(r_id,{0:s}) name,
r_okrol,
r_sex,
r_event_date,
TO_DAYS(NOW())-TO_DAYS(r_event_date) sukr,
r_event,
TO_DAYS(NOW())-TO_DAYS(r_born) age,
(SELECT {1:s} FROM breeds WHERE b_id=r_breed) breed,
Coalesce((SELECT w_weight FROM weights WHERE w_rabid=r_id AND w_date=(SELECT MAX(w_date) FROM weights WHERE w_rabid=r_id)),-1) weight,
r_status,
r_flags,
r_group,
Coalesce((SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent=r.r_id),0) suckers,
Coalesce((SELECT AVG(TO_DAYS(NOW())-TO_DAYS(r2.r_born)) FROM rabbits r2 WHERE r2.r_parent=r.r_id),-1) aage,
r_rate,
r_bon,
rabplace(r_id) place,
r_notes,
r_born,
r_breed,
Coalesce((SELECT GROUP_CONCAT('v',v_id ORDER BY v_id) FROM rab_vac rv WHERE rv.r_id=r.r_id ),'') vaccines
FROM rabbits r WHERE r_parent=0 ORDER BY name) c {2:s};", (options.safeBool("dbl") ? "2" : "1"), fld, makeWhere());
        }

        public String makeWhere()
        {
            String res = "";
            if (options.ContainsKey("sx"))
            {
                String sres = "";
                if (options["sx"].Contains("m"))
                    sres = "r_sex='male'";
                if (options["sx"].Contains("f"))
                    sres = addWhereOr(sres, "r_sex='female'");
                if (options["sx"].Contains("v"))
                    sres = addWhereOr(sres, "r_sex='void'");
                res = "(" + sres + ")";
            }
            if (options.ContainsKey("dt"))
                res = addWhereAnd(res, "(r_born>=NOW()-INTERVAL " + options["dt"] + " DAY)");
            if (options.ContainsKey("Dt"))
                res = addWhereAnd(res, "(r_born<=NOW()-INTERVAL " + options["Dt"] + " DAY)");
            if (options.ContainsKey("wg"))
                res = addWhereAnd(res, "(weight>=" + options["wg"] + ")");
            if (options.ContainsKey("Wg"))
                res = addWhereAnd(res, "(weight<=" + options["Wg"] + ")");
            if (options.ContainsKey(Filters.MALE) && options.safeValue("sx", "m").Contains("m"))
            {
                String stat = "";
                if (options[Filters.MALE].Contains("b"))
                    stat = "r_status=0";
                if (options[Filters.MALE].Contains("c"))
                    stat = addWhereOr(stat, "r_status=1");
                if (options[Filters.MALE].Contains("p"))
                    stat = addWhereOr(stat, "r_status=2");
                res = addWhereAnd(res, "(r_sex!='male' OR (r_sex='male' AND (" + stat + ")))");
            }
            if (options.ContainsKey(Filters.FEMALE) && options.safeValue("sx", "f").Contains("f"))
            {
                String stat = "";
                if (options[Filters.FEMALE].Contains("g"))
                    stat = "r_born>(NOW()-INTERVAL " + options["brd"] + " DAY)";
                if (options[Filters.FEMALE].Contains("b"))
                    stat = addWhereOr(stat, "(r_born<=(NOW()-INTERVAL " + options["brd"] + " DAY) AND (r_status=0 AND r_event_date IS NULL))");
                if (options[Filters.FEMALE].Contains("f"))
                    stat = addWhereOr(stat, "((r_status=0 AND r_event_date IS NOT NULL)OR(r_status=1 AND r_event_date IS NULL))");
                if (options[Filters.FEMALE].Contains("s"))
                    stat = addWhereOr(stat, "(r_status>1 OR (r_status=1 AND r_event_date IS NOT NULL))");
                res = addWhereAnd(res, "(r_sex!='female' OR (r_sex='female' AND (" + stat + ")))");
            }
            if (options.ContainsKey("ms") && options.safeValue("sx", "m").Contains("m"))
            {
                String stat = "";
                if (options["ms"] == "1")
                    stat = "SUBSTR(r_flags,1,1)='0' AND SUBSTR(r_flags,3,1)!='1'";
                if (options["ms"] == "2")
                    stat = "SUBSTR(r_flags,3,1)='1'";
                if (options["ms"] == "3")
                    stat = "SUBSTR(r_flags,1,1)='1'";
                res = addWhereAnd(res, "(r_sex!='male' OR (r_sex='male' AND " + stat + "))");
            }
            if (options.ContainsKey("fs") && options.safeValue("sx", "f").Contains("f"))
            {
                String stat = "";
                if (options["fs"] == "1")
                    stat = "SUBSTR(r_flags,1,1)='0' AND SUBSTR(r_flags,3,1)!='1'";
                if (options["fs"] == "2")
                    stat = "SUBSTR(r_flags,3,1)='1'";
                if (options["fs"] == "3")
                    stat = "SUBSTR(r_flags,1,1)='1'";
                res = addWhereAnd(res, "(r_sex!='female' OR (r_sex='female' AND " + stat + "))");
            }
            if (options.ContainsKey("ku") && options.safeValue("sx", "f").Contains("f"))
                res = addWhereAnd(res, "(r_sex!='female' OR (r_sex='female' AND SUBSTR(r_flags,4,1)=" + (int.Parse(options["ku"]) + 1).ToString() + "))");
            if (options.ContainsKey("nm"))
                res = addWhereAnd(res, "(name like '%" + options["nm"] + "%')");
            if (options.ContainsKey("pr") && options.safeValue("sx", "f").Contains("f"))
            {
                String stat = "";
                if (options["pr"] == "1")
                    stat = "r_event_date IS NULL";
                if (options["pr"] == "2")
                {

                    if (options.ContainsKey("pf") || options.ContainsKey("Pf"))
                    {
                        if (options.ContainsKey("pf"))
                            stat = "(r_event_date<=NOW()-INTERVAL " + options["pf"] + " DAY)";
                        if (options.ContainsKey("Pf"))
                        {
                            stat = addWhereAnd(stat, "(r_event_date>=NOW()-INTERVAL " + (options.safeInt("Pf") + 1) + " DAY)");
                        }
                    }
                    else
                        stat = "r_event_date IS NOT NULL";
                }
                res = addWhereAnd(res, "(r_sex!='female' OR (r_sex='female' AND (" + stat + ")))");
            }
            if (options.ContainsKey("br"))
                res = addWhereAnd(res, "(r_breed=" + options["br"] + ")");
            if (res == "") return "";
            return " WHERE " + res;
        }

        public override string countQuery()
        {
            return @"SELECT COUNT(*),SUM(r_group) FROM (SELECT r_sex,r_born,
rabname(r_id,2) name,r_group,
(SELECT w_weight FROM weights WHERE w_rabid=r_id AND w_date=(SELECT MAX(w_date) FROM weights WHERE w_rabid=r_id)) weight,
r_status,r_flags,r_event_date,r_breed
 FROM rabbits WHERE r_parent=0) c" + makeWhere() + ";";
        }   

        public static TreeData getRabbitGen(int rabbit, MySqlConnection con)
        {
            if (rabbit == 0) return null;
            MySqlCommand cmd = new MySqlCommand(@"SELECT
                                                    rabname(r_id,1),
                                                    r_mother,
                                                    r_father,
                                                    r_bon,
                                                    TO_DAYS(NOW())-TO_DAYS(r_born)
                                                FROM rabbits WHERE r_id=" + rabbit.ToString() + " LIMIT 1;", con);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (!rd.HasRows)
            {
                rd.Close();
                cmd.CommandText = @"SELECT
                                        deadname(r_id,1),
                                        r_mother,
                                        r_father,
                                        r_bon,
                                        TO_DAYS(NOW())-TO_DAYS(r_born)
                                   FROM dead WHERE r_id=" + rabbit.ToString() + " LIMIT 1;";
                rd = cmd.ExecuteReader();
            }
            TreeData res = new TreeData();
            if (rd.Read())
            {
                res.caption = rd.GetString(0) + ", " + rd.GetInt32(4).ToString() + "," + Rabbit.GetFBon(rd.GetString("r_bon"), true);
                int mom = rd.IsDBNull(1) ? 0 : rd.GetInt32(1);
                int dad = rd.IsDBNull(2) ? 0 : rd.GetInt32(2);
                rd.Close();
                TreeData m = getRabbitGen(mom, con);
                TreeData d = getRabbitGen(dad, con);
                if (m == null)
                {
                    m = d;
                    d = null;
                }
                if (m != null)
                {
                    res.items = new TreeData[] { m, d };
                }
            }
            rd.Close();
            return res;
        }

        //public static String getRSex(String sx)
        //{
        //    String res = "?";
        //    if (sx == "male") res = "м";
        //    if (sx == "female") res = "ж";
        //    return res;
        //}

        //private string Separate(string s)
        //{
        //    if (s.Length != 0) return ","; else return "";
        //}
    }
}
