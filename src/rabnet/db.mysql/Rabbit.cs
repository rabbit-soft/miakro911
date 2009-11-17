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
            if (options.safeBool("nums", false))
                r.fname = r.fid.ToString()+" ";
            bool shr = options.safeBool("shr");
            r.fname += rd.IsDBNull(1)?"":rd.GetString(1);
            String sun=rd.IsDBNull(2)?"":rd.GetString(2);
            String sen=rd.IsDBNull(3)?"":rd.GetString(3);
            String sx = rd.GetString(4);
            r.fage = rd.GetInt32(8);
            r.fN = "-";
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
                    r.fname += " " + sun + "ы";
                    if (options.safeBool("dbl") && sen!="")
                        r.fname += "-" + sen + "ы";
                }
            }
            else if (sx == "male")
            {
                r.fsex = "м";
                if (sun != "")
                {
                    r.fname += " " + sun;
                    if (options.safeBool("dbl") && sen!="")
                        r.fname += "-" + sen;
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
                    r.fname += " " + sun + "a";
                    if (options.safeBool("dbl") && sen!="")
                        r.fname += "-" + sen + "a";
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
            if (rd.GetInt32(4) != 0)
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

        public String makeWhere()
        {
            String res = "";

            return res;
        }

        public override string getQuery()
        {
            String fld = "b_name";
            if (options.safeBool("shr"))
                fld = "b_short_name";
            return String.Format(@"SELECT r_id, 
(SELECT n_name FROM names WHERE n_id=r_name) name,
(SELECT n_surname FROM names WHERE n_id=r_surname) surname,
(SELECT n_surname FROM names WHERE n_id=r_secname) secname,
r_okrol,r_sex,
TO_DAYS(NOW())-TO_DAYS(r_event_date) sukr,
r_event,
TO_DAYS(NOW())-TO_DAYS(r_born) age,
(SELECT "+fld+@" FROM breeds WHERE b_id=r_breed) breed,
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
FROM rabbits,tiers WHERE r_parent=0 AND r_tier=t_id ORDER BY name;");
        }
        public override string countQuery()
        {
            return "SELECT COUNT(*) FROM rabbits WHERE r_parent=0;";
        }
    }
}
