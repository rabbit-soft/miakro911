using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    class Rabbit:IRabbit
    {
        public int fid;
        public String fname;
        public String fsex;
        public int fage;
        public String fbreed;
        public String fweight;
        public String fstatus;
        public String fbgp;
        public String fN;
        public int faverage;
        public int frate;
        public String fcls;
        public String faddress;
        public string fnotes;
        public Rabbit(int id)
        {
            fid=id;
        }
        int IRabbit.id(){return fid;}
        string IRabbit.name() { return fname; }
        string IRabbit.sex() { return fsex; }
        int IRabbit.age() { return fage; }
        string IRabbit.breed() { return fbreed; }
        string IRabbit.weight() { return fweight; }
        string IRabbit.status() { return fstatus; }
        string IRabbit.bgp() { return fbgp; }
        string IRabbit.N() { return fN; }
        int IRabbit.average() { return faverage; }
        int IRabbit.rate() { return frate; }
        string IRabbit.cls() { return fcls; }
        string IRabbit.address() { return faddress; }
        string IRabbit.notes() { return fnotes; }
    }

    class Rabbits:RabNetDataGetterBase
    {
        public Rabbits(MySqlConnection sql, Filters opts) : base(sql, opts) { }
        public override IData nextItem()
        {
            Rabbit r=new Rabbit(rd.GetInt32(0));
            r.fname = "";
            if (options.safeBool("num"))
                r.fname = r.fid.ToString()+" ";
            bool shr = options.safeBool("shr");
            String nm=rd.GetString("name");
            r.fname += nm;
            String sun=rd.GetString("surname");
            String sen=rd.GetString("secname");
            String sx = rd.GetString("r_sex");
            r.fage = rd.GetInt32("age");
            r.fN = "-";
            int cnt = rd.GetInt32("r_group");
            r.faverage = 0;
            if (rd.GetInt32("r_group") > 1)
                r.fN = "[" + rd.GetString("r_group") + "]";
            if (sx == "void")
            {
                r.fstatus = shr ? "Пдс" : "Подсосные";
                r.fstatus = shr ? "Гнд" : "Гнездовые";
                r.fsex = "?";
                if (sun != "")
                {
                    if (nm != "") r.fname += " ";
                    r.fname += sun + "ы";
                    if (options.safeBool("dbl") && sen!="")
                        r.fname += "-" + sen + "ы";
                }
            }
            else if (sx == "male")
            {
                r.fsex = "м";
                if (sun != "")
                {
                    if (nm != "") r.fname += " ";
                    r.fname += sun + (cnt > 1 ? "ы" : "");
                    if (options.safeBool("dbl") && sen!="")
                        r.fname += "-" + sen + (cnt > 1 ? "ы" : "");
                }
                switch (rd.GetInt32("r_status"))
                {
                    case 0: r.fstatus = shr?"Мал":"мальчик"; break;
                    case 1: r.fstatus = shr ? "Кнд" : "кандидат"; break;
                    case 2: r.fstatus = shr ? "Прз" : "производитель"; break;
                }
            }
            else
            {
                r.fsex = "ж";
                if (!rd.IsDBNull(6))
                if (rd.GetInt32(6) != 0)
                    r.fsex = "C-" + rd.GetInt32(6).ToString();
                if (sun != "")
                {
                    if (nm != "") r.fname += " ";
                    r.fname += sun + (cnt>1?"ы":"a");
                    if (options.safeBool("dbl") && sen!="")
                        r.fname += "-" + sen + (cnt > 1 ? "ы" : "a");
                }
                if (r.fage < 122)
                    r.fstatus = shr ? "Дев" : "Девочка";
                else
                    r.fstatus = shr ? "Нвс" : "Невеста";
                if (rd.GetInt32("r_status") == 1)
                    r.fstatus = shr ? "Прк" : "Первокролка";
                if (rd.GetInt32("r_status") > 1)
                    r.fstatus = shr ? "Штн" : "Штатная";
                if (!rd.IsDBNull(14))
                {
                    r.fN = "+" + rd.GetString("suckers");
                    r.faverage = rd.GetInt32("aage");
                }
            }
            if (nm == "")
                r.fname += "-" + rd.GetInt32(4).ToString();
            r.fbreed = rd.GetString(9);
            r.fweight = rd.IsDBNull(10) ? "?" : rd.GetString(10);
            String flg = rd.GetString("r_flags");
            r.fbgp = "";
            if (flg[2] == '1') r.fbgp += "Б";
            if (flg[2] == '2') r.fbgp += "в";
            if (flg[0] != '0') r.fbgp += "ГП";
            if (flg[1] != '0') r.fbgp = "<" + r.fbgp + ">";
            r.frate = rd.GetInt32("r_rate");
            Char fbon = '5';
            String bon=rd.GetString("r_bon");
            for (int i = 1; i < bon.Length; i++)
                if (bon[i] < fbon) fbon = bon[i];
            switch (fbon)
            {
                case '0': r.fcls = shr ? "-" : "Нет"; break;
                case '1': r.fcls = "III";  break;
                case '2': r.fcls = "II"; break;
                case '3': r.fcls = "I"; break;
                case '4': r.fcls = shr ? "Э" : "Элита"; break;

            }
            r.fnotes = rd.GetString("r_notes");
            r.faddress = Buildings.fullRName(rd.GetInt32("r_farm"), rd.GetInt32("r_tier_id"), rd.GetInt32("r_area"),
                rd.GetString("t_type"), rd.GetString("t_delims"), shr, options.safeBool("sht"), options.safeBool("sho"));
            return r;
        }

        private String addwhere(String str,String adder)
        {
            if (str!="") str+=" AND ";
            str+=adder;
            return str;
        }
        private String addwhereOr(String str, String adder)
        {
            if (str != "") str += " OR ";
            str += adder;
            return str;
        }

        public String makeWhere()
        {
            String res = "";
            if (options.ContainsKey("sx"))
            {
                String sres="";
                if (options["sx"].Contains("m"))
                    sres = "r_sex='male'";
                if (options["sx"].Contains("f"))
                    sres=addwhereOr(sres, "r_sex='female'");
                if (options["sx"].Contains("v"))
                    sres=addwhereOr(sres, "r_sex='void'");
                res = "("+sres+")";
            }
            if (options.ContainsKey("dt"))
                    res = addwhere(res,"(r_born>=NOW()-INTERVAL " + options["dt"] + " DAY)");
            if (options.ContainsKey("Dt"))
                res = addwhere(res,"(r_born<=NOW()-INTERVAL " + options["Dt"] + " DAY)");
            if (options.ContainsKey("wg"))
                res = addwhere(res,"(weight>=" + options["wg"] + ")");
            if (options.ContainsKey("Wg"))
                res = addwhere(res, "(weight<=" + options["Wg"] + ")");
            if (options.ContainsKey("mt") && options.safeValue("sx", "m").Contains("m"))
            {
                String stat = "";
                if (options["mt"].Contains("b"))
                    stat = "r_status=0";
                if (options["mt"].Contains("c"))
                    stat = addwhereOr(stat,"r_status=1");
                if (options["mt"].Contains("p"))
                    stat = addwhereOr(stat, "r_status=2");
                res = addwhere(res,"(r_sex!='male' OR (r_sex='male' AND ("+stat+")))");
            }
            if (options.ContainsKey("ft") && options.safeValue("sx", "f").Contains("f"))
            {
                String stat = "";
                if (options["ft"].Contains("g"))
                    stat = "r_born>(NOW()-INTERVAL 122 DAYS)";
                if (options["ft"].Contains("b"))
                    stat = addwhereOr(stat, "(r_born<=(NOW()-INTERVAL 122 DAYS) AND r_status=0)");
                if (options["ft"].Contains("f"))
                    stat = addwhereOr(stat, "r_status=1");
                if (options["ft"].Contains("s"))
                    stat = addwhereOr(stat, "r_status>1");
                res = addwhere(res, "(r_sex!='female OR ('r_sex='female' AND (" + stat + ")))");
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
                res = addwhere(res, "(r_sex!='male' OR (r_sex='male' AND " + stat + "))");
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
                res = addwhere(res, "(r_sex!='female' OR (r_sex='female' AND " + stat + "))");
            }
            if (options.ContainsKey("ku") && options.safeValue("sx", "f").Contains("f"))
                res = addwhere(res,"(r_sex!='female' OR (r_sex='female' AND SUBSTR(r_flags,4,1)="+(int.Parse(options["ku"])+1).ToString()+"))");
            if (options.ContainsKey("nm"))
                res = addwhere(res,"(CONCAT(name,surname) like '%" + options["nm"] + "%')");
            if (options.ContainsKey("pr") && options.safeValue("sx", "f").Contains("f"))
            {
                String stat = "";
                if (options["pr"] == "1")
                    stat = "r_event_date IS NULL";
                if (options["pr"] == "2")
                {
                    
                    if (options.ContainsKey("pf") || options.ContainsKey("pf"))
                    {
                        if (options.ContainsKey("pf"))
                            stat = "(r_event_date>=NOW()-INTERVAL "+options["pf"]+" DAY)";
                        if (options.ContainsKey("Pf"))
                            stat = addwhere(stat, "(r_event_date<=NOW()-INTERVAL " + options["Pf"] + " DAY)");
                    }
                    else
                        stat = "r_event_date IS NOT NULL";
                }
                res=addwhere(res,"(r_sex!='female' OR (r_sex='female' AND ("+stat+")))");
            }
            if (res == "") return "";
            return " WHERE "+res;
        }

        public override string getQuery()
        {
            String fld = "b_name";
            if (options.safeBool("shr"))
                fld = "b_short_name";
            return String.Format(@" SELECT * FROM (SELECT r_id, 
COALESCE((SELECT n_name FROM names WHERE n_id=r_name),'') name,
COALESCE((SELECT n_surname FROM names WHERE n_id=r_surname),'') surname,
COALESCE((SELECT n_surname FROM names WHERE n_id=r_secname),'') secname,
r_okrol,r_sex,
TO_DAYS(NOW())-TO_DAYS(r_event_date) sukr,
r_event,
TO_DAYS(NOW())-TO_DAYS(r_born) age,
(SELECT " + fld+@" FROM breeds WHERE b_id=r_breed) breed,
(SELECT w_weight FROM weights WHERE w_rabid=r_id AND w_date=(SELECT MAX(w_date) FROM weights WHERE w_rabid=r_id)) weight,
r_status,
r_flags,
r_group,
(SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id) suckers,
(SELECT AVG(TO_DAYS(NOW())-TO_DAYS(r2.r_born)) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id) aage,
r_rate,
r_bon,
r_farm,
r_area,
r_tier_id,
t_type,
t_delims,
r_children,
r_notes
FROM rabbits,tiers WHERE r_parent=0 AND r_tier=t_id ORDER BY CONCAT(name,surname)) c"+makeWhere()+";");
        }
        public override string countQuery()
        {
            return @"SELECT COUNT(*) FROM (SELECT r_sex,r_born,
COALESCE((SELECT n_name FROM names WHERE n_id=r_name),'') name,
COALESCE((SELECT n_surname FROM names WHERE n_id=r_surname),'') surname,
COALESCE((SELECT n_surname FROM names WHERE n_id=r_secname),'') secname,
(SELECT w_weight FROM weights WHERE w_rabid=r_id AND w_date=(SELECT MAX(w_date) FROM weights WHERE w_rabid=r_id)) weight,
r_status,r_flags,r_event_date
 FROM rabbits WHERE r_parent=0) c"+makeWhere()+";";
        }
    }
}
