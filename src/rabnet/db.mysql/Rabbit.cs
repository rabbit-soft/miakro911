using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    public class Rabbit:IData{
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
            r.fname = nm;
            //String sun=rd.GetString("surname");
            //String sen=rd.GetString("secname");
            String sx = rd.GetString("r_sex");
            r.fage = rd.GetInt32("age");
            r.fN = "-";
            int cnt = rd.GetInt32("r_group");
            r.faverage = -1;
            if (rd.GetInt32("r_group") > 1)
                r.fN = "[" + String.Format("{0,2:s}",rd.GetString("r_group")) + "]";
            r.fsex = getRSex(sx);
            if (sx == "void")
            {
                r.fstatus = shr ? "Пдс" : "Подсосные";
                if (r.fage<options.safeInt("suc",50))
                    r.fstatus = shr ? "Гнд" : "Гнездовые";
            }
            else if (sx == "male")
            {
                switch (rd.GetInt32("r_status"))
                {
                    case 0: r.fstatus = shr?"Мал":"мальчик"; break;
                    case 1: r.fstatus = shr ? "Кнд" : "кандидат"; break;
                    case 2: r.fstatus = shr ? "Прз" : "производитель"; break;
                }
            }
            else
            {
                if (!rd.IsDBNull(4))
                   r.fsex = "C-" + String.Format("{0,2:d}",rd.GetInt32("sukr"));
                if (r.fage < options.safeInt("brd", 122))
                    r.fstatus = shr ? "Дев" : "Девочка";
                else
                    r.fstatus = shr ? "Нвс" : "Невеста";
                if ((rd.GetInt32("r_status") == 1 && rd.IsDBNull(4))|| (rd.GetInt32("r_status") == 0 && !rd.IsDBNull(4)))
                    r.fstatus = shr ? "Прк" : "Первокролка";
                if (rd.GetInt32("r_status") > 1 || (rd.GetInt32("r_status") == 1 && !rd.IsDBNull(4)))
                    r.fstatus = shr ? "Штн" : "Штатная";
                if (!rd.IsDBNull(12))
                {
                    r.fN = "+" + String.Format("{0,2:s}",rd.GetString("suckers"));
                    r.faverage = rd.GetInt32("aage");
                }
                else
                    r.faverage = -1;
            }
            //if (nm == "")
            //    r.fname += "-" + rd.GetString("r_okrol").ToString();
            r.fbreed = rd.GetString("breed");
            r.fweight = rd.IsDBNull(8) ? "?" : rd.GetString("weight");
            String flg = rd.GetString("r_flags");
            r.fbgp = "";
            if (flg[2] == '1') r.fbgp += "Б";
            if (flg[2] == '2') r.fbgp += "в";
            if (flg[0] != '0') r.fbgp += "ГП";
            if (flg[1] != '0') r.fbgp = "<" + r.fbgp + ">";
            r.frate = rd.GetInt32("r_rate");
            r.fcls=Rabbits.getBon(rd.GetString("r_bon"),shr);
            r.fnotes = rd.GetString("r_notes");
            r.faddress = Buildings.fullPlaceName(rd.GetString("place"), shr, options.safeBool("sht"), options.safeBool("sho"));
//                Buildings.fullRName(rd.GetInt32("r_farm"), rd.GetInt32("r_tier_id"), rd.GetInt32("r_area"),
  //              rd.GetString("t_type"), rd.GetString("t_delims"), shr, options.safeBool("sht"), options.safeBool("sho"));
            return r;
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
                    sres=addWhereOr(sres, "r_sex='female'");
                if (options["sx"].Contains("v"))
                    sres=addWhereOr(sres, "r_sex='void'");
                res = "("+sres+")";
            }
            if (options.ContainsKey("dt"))
                    res = addWhereAnd(res,"(r_born>=NOW()-INTERVAL " + options["dt"] + " DAY)");
            if (options.ContainsKey("Dt"))
                res = addWhereAnd(res,"(r_born<=NOW()-INTERVAL " + options["Dt"] + " DAY)");
            if (options.ContainsKey("wg"))
                res = addWhereAnd(res,"(weight>=" + options["wg"] + ")");
            if (options.ContainsKey("Wg"))
                res = addWhereAnd(res, "(weight<=" + options["Wg"] + ")");
            if (options.ContainsKey("mt") && options.safeValue("sx", "m").Contains("m"))
            {
                String stat = "";
                if (options["mt"].Contains("b"))
                    stat = "r_status=0";
                if (options["mt"].Contains("c"))
                    stat = addWhereOr(stat,"r_status=1");
                if (options["mt"].Contains("p"))
                    stat = addWhereOr(stat, "r_status=2");
                res = addWhereAnd(res,"(r_sex!='male' OR (r_sex='male' AND ("+stat+")))");
            }
            if (options.ContainsKey("ft") && options.safeValue("sx", "f").Contains("f"))
            {
                String stat = "";
                if (options["ft"].Contains("g"))
                    stat = "r_born>(NOW()-INTERVAL " + options["brd"] + " DAY)";
                if (options["ft"].Contains("b"))
                    stat = addWhereOr(stat, "(r_born<=(NOW()-INTERVAL "+options["brd"]+" DAY) AND (r_status=0 AND r_event_date IS NULL))");
                if (options["ft"].Contains("f"))
                    stat = addWhereOr(stat, "((r_status=0 AND r_event_date IS NOT NULL)OR(r_status=1 AND r_event_date IS NULL))");
                if (options["ft"].Contains("s"))
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
                res = addWhereAnd(res,"(r_sex!='female' OR (r_sex='female' AND SUBSTR(r_flags,4,1)="+(int.Parse(options["ku"])+1).ToString()+"))");
            if (options.ContainsKey("nm"))
                res = addWhereAnd(res,"(name like '%" + options["nm"] + "%')");
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
                            stat = "(r_event_date<=NOW()-INTERVAL "+options["pf"]+" DAY)";
                        if (options.ContainsKey("Pf"))
                            stat = addWhereAnd(stat, "(r_event_date>=NOW()-INTERVAL " + options["Pf"] + " DAY)");
                    }
                    else
                        stat = "r_event_date IS NOT NULL";
                }
                res=addWhereAnd(res,"(r_sex!='female' OR (r_sex='female' AND ("+stat+")))");
            }
            if (options.ContainsKey("br"))
                res = addWhereAnd(res, "(r_breed=" + options["br"] + ")");
            if (res == "") return "";
            return " WHERE "+res;
        }

        public override string getQuery()
        {
/*            COALESCE((SELECT n_name FROM names WHERE n_id=r_name),'') name,
COALESCE((SELECT n_surname FROM names WHERE n_id=r_surname),'') surname,
COALESCE((SELECT n_surname FROM names WHERE n_id=r_secname),'') secname,
*/
            String fld = "b_name";
            if (options.safeBool("shr"))
                fld = "b_short_name";
            return String.Format(@" SELECT * FROM (SELECT r_id, 
rabname(r_id,"+(options.safeBool("dbl")?"2":"1")+@") name,
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
rabplace(r_id) place,
r_notes,
r_born,
r_event_date,
r_breed
FROM rabbits WHERE r_parent=0 ORDER BY name) c"+makeWhere()+";");
        }
        /*
        r_farm,
        r_area,
        r_tier_id,
        t_type,
        t_delims,
        t_nest,
        */
        public override string countQuery()
        {
            return @"SELECT COUNT(*),SUM(r_group) FROM (SELECT r_sex,r_born,
rabname(r_id,2) name,r_group,
(SELECT w_weight FROM weights WHERE w_rabid=r_id AND w_date=(SELECT MAX(w_date) FROM weights WHERE w_rabid=r_id)) weight,
r_status,r_flags,r_event_date,r_breed
 FROM rabbits WHERE r_parent=0) c"+makeWhere()+";";
        }


        public static String getBon(String bon,bool shr)
        {
            Char fbon='5';
            for (int i = 1; i < bon.Length; i++)
                if (bon[i] < fbon) fbon = bon[i];
            switch (fbon)
            {
                case '1': return "III";
                case '2': return "II";
                case '3': return "I";
                case '4': return (shr ? "Э" : "Элита");
            }
            return (shr ? "-" : "Нет");
        }

        public static TreeData getRabbitGen(int rabbit, MySqlConnection con)
        {
            return getRabbitGen(rabbit,con,7);
        }

        public static TreeData getRabbitGen(int rabbit,MySqlConnection con,byte level)
        {
            if (rabbit==0)
                return null;
            if (level == 0) return null;
            MySqlCommand cmd = new MySqlCommand(@"SELECT
rabname(r_id,1) name,
(SELECT n_use FROM names WHERE n_id=r_surname) mother,
(SELECT n_use FROM names WHERE n_id=r_secname) father,
r_bon,TO_DAYS(NOW())-TO_DAYS(r_born) FROM rabbits WHERE r_id=" + rabbit.ToString()+";",con);
            MySqlDataReader rd = cmd.ExecuteReader();
            TreeData res = new TreeData();
            if (rd.Read())
            {
                res.caption = rd.GetString(0) + "," + rd.GetInt32(4).ToString() + "," + Rabbits.getBon(rd.GetString("r_bon"), true);
                int mom=rd.IsDBNull(1)?0:rd.GetInt32(1);
                int dad = rd.IsDBNull(2) ? 0 : rd.GetInt32(2);
                rd.Close();
                TreeData m = getRabbitGen(mom, con,level--);
                TreeData d = getRabbitGen(dad, con,level--);
                if (m == null)
                {
                    m = d;
                    d = null;
                }
                if (m != null)
                {
                   res.items = new TreeData[] {m,d};
                }
            }
            rd.Close();
            return res;
        }

        public static String getRSex(String sx)
        {
            String res = "?";
            if (sx == "male") res = "м";
            if (sx == "female") res = "ж";
            return res;
        }

    }

    public class OneRabbit
    {
        public enum RabbitSex{VOID,MALE,FEMALE};
        public static string NullAddress = "бомж";
        public RabbitSex sex;
        public DateTime born;
        public int rate;
        public int id;
        public bool defect;
        public bool gp;
        public bool gr;
        public bool spec;
        public bool risk;
        public int parent;
        public int name;
        public int wasname;
        public int surname;
        public int secname;
        public string address;
        public string justAddress;
        public string smallAddress;
        public int group;
        public int breed;
        public int zone;
        public String notes;
        public string gens;
        public int status;
        public DateTime lastfuckokrol;
        public bool nolact;
        public bool nokuk;
        public DateTime evdate;
        public int evtype;
        public int babies;
        public int lost;
		public int weight_age;
        public String fullname;
        public String breedname;
        public String bon;
        public OneRabbit[] youngers;
        public string tag;
        public string nuaddr="";
        public OneRabbit(int id,string sx,DateTime bd,int rt,string flg,int nm,int sur,int sec,string adr,int grp,int brd,int zn,String nts,
            String gn,int st,DateTime lfo,String evt,DateTime evd,int ob,int lb,String fnm,String bnm,
			String bon, int parent, int wa)
        {
            this.parent = parent;
            tag = "";
            this.id=id;
            sex=RabbitSex.VOID;
            if (sx == "male") sex = RabbitSex.MALE;
            if (sx == "female") sex = RabbitSex.FEMALE;
            born = bd;
            rate = rt;
            name = nm;
            wasname = name;
            gp = flg[0] == '1';
            defect = flg[2] == '1' || flg[2]=='3';
            spec = flg[2] == '2' || flg[2]=='3';
            nokuk = flg[3] == '1';
            nolact = flg[4] == '1';
            risk = flg[4] == '1';
            surname = sur;
            secname = sec;
            if (adr == "")
            {
                justAddress = "";
                smallAddress = NullAddress;
                address = NullAddress;
            }
            else
            {
                justAddress = adr;
                address = Buildings.fullPlaceName(adr, false, true, true);
                smallAddress = Buildings.fullPlaceName(adr, true, false, false);
            }
            group = grp;
            breed = brd;
            zone = zn;
            notes = nts;
            gens = gn;
            this.bon = bon;
            status = st;
            if (sx == "void") status = age()<50?0:1;
            lastfuckokrol = lfo;
            evtype = 0;
            if (evt == "sluchka") evtype = 1;
            if (evt == "vyazka") evtype = 2;
            if (evt == "kuk") evtype = 3;
            evdate = evd;babies = ob;lost = lb;
            fullname = fnm; breedname = bnm;
			weight_age = wa;
        }

        public static String SexToString(RabbitSex s)
        {
            String res = "void";
            if (s == RabbitSex.FEMALE) res = "female";
            if (s == RabbitSex.MALE) res = "male";
            return res;
        }
        public static String SexToRU(RabbitSex s)
        {
            if (s == RabbitSex.FEMALE) return "ж";
            if (s == RabbitSex.MALE) return "м";
            return "?";
        }
        public int age()
        {
            return (DateTime.Now-born).Days;
        }
        public string medAddress{get{return Buildings.fullPlaceName(justAddress, false, true, false);}}
    }

    class RabbitGetter
    {
        public static OneRabbit fillRabbit(MySqlDataReader rd)
        {
            OneRabbit r = new OneRabbit(rd.GetInt32("r_id"), rd.GetString("r_sex"), rd.GetDateTime("r_born"), rd.GetInt32("r_rate"),
                rd.GetString("r_flags"), rd.GetInt32("r_name"), rd.GetInt32("r_surname"), rd.GetInt32("r_secname"),
                rd.GetString("address"), rd.GetInt32("r_group"), rd.GetInt32("r_breed"), rd.GetInt32("r_zone"),
                rd.IsDBNull(16)?"":rd.GetString("r_notes"), rd.IsDBNull(19)?"":rd.GetString("genom"), rd.GetInt32("r_status"),
                rd.IsDBNull(1) ? DateTime.MinValue : rd.GetDateTime("r_last_fuck_okrol"),
                rd.IsDBNull(3) ? "none" : rd.GetString("r_event"), rd.IsDBNull(2) ? DateTime.MinValue : rd.GetDateTime("r_event_date"),
                rd.IsDBNull(4) ? 0 : rd.GetInt32("r_overall_babies"), rd.IsDBNull(5) ? 0 : rd.GetInt32("r_lost_babies"),
                rd.GetString("fullname"), rd.GetString("breedname"),
                rd.GetString("r_bon"),rd.GetInt32("r_parent"),0);
            return r;
        }

        public enum RAB_TYPE {LIVE,DEAD,ANY }
        public static bool isDeadRabbit(MySqlConnection con, int rid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT isdead({0:d});",rid), con);
            MySqlDataReader rd = cmd.ExecuteReader();
            bool dead=false;
            if (rd.Read())
                dead = rd.GetBoolean(0);
            rd.Close();
            return dead;
        }
        public static OneRabbit GetRabbit(MySqlConnection con, int rid)
        {
            return GetRabbit(con, rid, RAB_TYPE.LIVE);
        }
        public static OneRabbit GetRabbit(MySqlConnection con, int rid,RAB_TYPE type)
        {
            if (rid == 0) return null;
            if (type == RAB_TYPE.ANY)
                type = (isDeadRabbit(con, rid) ? RAB_TYPE.DEAD : RAB_TYPE.LIVE);
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT r_id,r_last_fuck_okrol,{0:s},r_overall_babies,r_lost_babies,
r_sex,r_born,r_flags,r_breed,r_zone,r_name,r_surname,r_secname,
{1:s}place(r_id) address,r_group,r_notes,
{1:s}name(r_id,2) fullname,
(SELECT b_name FROM breeds WHERE b_id=r_breed) breedname,
(SELECT COALESCE(GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' '),'') FROM genoms WHERE g_id=r_genesis) genom,
r_status,r_rate,r_bon,r_parent
FROM {2:s} WHERE r_id={3:d};", (type == RAB_TYPE.LIVE ? "r_event_date,r_event" : "NULL r_event_date,'none' r_event"),
                           (type == RAB_TYPE.LIVE ? "rab" : "dead"), (type == RAB_TYPE.LIVE ? "rabbits" : "dead"),
                           rid), con);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (!rd.Read())
            {
                rd.Close();
                return null;
            }
            OneRabbit r = fillRabbit(rd);
            rd.Close();
            r.youngers = getYoungers(con, rid);
            return r;
        }

        public static OneRabbit[] getYoungers(MySqlConnection con, int mom)
        {
            MySqlCommand cmd = new MySqlCommand(@"SELECT r_id,r_last_fuck_okrol,r_event_date,r_event,r_overall_babies,r_lost_babies,
r_sex,r_born,r_flags,r_breed,r_zone,r_name,r_surname,r_secname,
rabplace(r_id) address,r_group,r_notes,
rabname(r_id,2) fullname,
(SELECT b_name FROM breeds WHERE b_id=r_breed) breedname,
(SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=r_genesis) genom,
r_status,r_rate,r_bon,r_parent
FROM rabbits WHERE r_parent=" + mom.ToString() + ";", con);
            List<OneRabbit> rbs = new List<OneRabbit>();
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
                rbs.Add(fillRabbit(rd));
            rd.Close();
            return rbs.ToArray();
        }

        public static void SetRabbit(MySqlConnection con, OneRabbit r)
        {
            int multi = ((r.spec ? 1 : 0) << 1) + (r.defect ? 1 : 0);
            String flags = String.Format("{0:D1}{1:D1}{2:D1}{3:D1}{4:D1}", r.gp ? 1 : 0, r.risk?1:0, multi, r.nokuk?1:0, r.nolact?1:0);
            String qry=String.Format(@"UPDATE rabbits SET 
r_name={0:d},r_surname={1:d},r_secname={2:d},r_breed={3:d},r_zone={4:d},r_group={5:d},r_notes='{6:s}',
r_flags='{7:d}',r_rate={8:d},r_born={9:s}",r.name,r.surname,r.secname,r.breed,r.zone,r.group,r.notes,flags,r.rate,
                                          DBHelper.DateToMyString(r.born));
            if (r.sex != OneRabbit.RabbitSex.VOID)
            {
                qry += String.Format(",r_status={0:d},r_last_fuck_okrol={1:s}",r.status,DBHelper.DateToMyString(r.lastfuckokrol));
            }
            if (r.sex == OneRabbit.RabbitSex.FEMALE)
            {
                String ev="none";
                if (r.evtype==1) ev="sluchka";if (r.evtype==2) ev="vyazka";if (r.evtype==2) ev="kuk";
                qry += String.Format(",r_event='{0:s}',r_event_date={1:s},r_lost_babies={2:d},r_overall_babies={3:d}", ev, DBHelper.DateToMyString(r.evdate), r.babies, r.lost);
            }
            qry+=String.Format(" WHERE r_id={0:d};",r.id);
            MySqlCommand cmd = new MySqlCommand(qry, con);
            cmd.ExecuteNonQuery();
            int gen=DBHelper.makeGenesis(con,r.gens);
            cmd.CommandText = "UPDATE rabbits SET r_genesis=" + gen.ToString() + " WHERE r_id=" + r.id.ToString() + ";";
            cmd.ExecuteNonQuery();
            if (r.wasname != r.name)
            {
                cmd.CommandText = "UPDATE names SET n_use=0,n_block_date=NULL WHERE n_id="+r.wasname.ToString()+";";
                cmd.ExecuteNonQuery();
                cmd.CommandText="UPDATE names SET n_use="+r.id.ToString()+" WHERE n_id="+r.name.ToString()+";";
                cmd.ExecuteNonQuery();
            }
        }

        public static void setBon(MySqlConnection sql, int rabbit, String bon)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE rabbits SET r_bon='"+bon+"' WHERE r_id="+rabbit.ToString()+";", sql);
            cmd.ExecuteNonQuery();
        }

        public static void makeFuck(MySqlConnection sql, int female, int male, DateTime date,int worker)
        {
            OneRabbit f = GetRabbit(sql,female);
            String type = "sluchka";
            if (f.status > 0)
                type = "vyazka";
            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE fucks SET f_last=0 WHERE f_rabid={0:d};",female), sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(@"INSERT INTO fucks(f_rabid,f_date,f_partner,f_state,f_type,f_last,f_notes,f_worker) 
VALUES({0:d},{1:s},{2:d},'sukrol','{3:s}',1,'',{4:d});",female,DBHelper.DateToMyString(date),male,type,worker);
            cmd.ExecuteNonQuery();
//            cmd.CommandText = String.Format("SELECT r_status,TODAYS(r_last_fuck_okrol FROM rabbits WHERE r_id=");
            int rate = 1;
            cmd.CommandText = String.Format("UPDATE rabbits SET r_event_date={0:s},r_event='{1:s}',r_rate=r_rate+{3:d} WHERE r_id={2:d};",
                DBHelper.DateToMyString(date),type,female,rate);
            cmd.ExecuteNonQuery();
            cmd.CommandText=String.Format("UPDATE rabbits SET r_last_fuck_okrol={0:s},r_rate=r_rate+1 WHERE r_id={1:d};",
                DBHelper.DateToMyString(date), male);
            cmd.ExecuteNonQuery();
        }

        public static void MakeProholost(MySqlConnection sql, int rabbit, DateTime date)
        {
            int male = WhosChildren(sql, rabbit);
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE fucks SET f_state='proholost',f_end_date={0:s} WHERE f_state='sukrol' AND f_rabid={1:d};",
                DBHelper.DateToMyString(date),rabbit), sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format("UPDATE rabbits SET r_event_date=NULL,r_event='none',r_rate=r_rate-2 WHERE r_id={0:d};",rabbit);
            cmd.ExecuteNonQuery();
            if (male != 0)
            {
                cmd.CommandText = String.Format("UPDATE rabbits SET r_rate=r_rate-2 WHERE r_id={0:d};", male);
                cmd.ExecuteNonQuery();
            }
        }

        public static int WhosChildren(MySqlConnection sql, int rabbit)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT f_partner FROM fucks WHERE f_state='sukrol' AND f_rabid={0:d};",rabbit), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int res = 0;
            if (rd.Read())
                res = rd.GetInt32(0);
            rd.Close();
            return res;
        }
        
        public static void MakeOkrol(MySqlConnection sql, int rabbit, DateTime date, int children, int dead)
        {
            int father = WhosChildren(sql, rabbit);
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE fucks SET f_state='okrol',f_end_date={0:s},
f_children={1:d},f_dead={2:d} WHERE f_rabid={3:d} AND f_state='sukrol';",
                       DBHelper.DateToMyString(date),children,dead,rabbit),sql);
            cmd.ExecuteNonQuery();
            OneRabbit fml = GetRabbit(sql, rabbit);
            OneRabbit ml = GetRabbit(sql, father,RAB_TYPE.ANY);
            int rt = children-8;
            if (rt != 0 && ml!=null)
            {
                cmd.CommandText = String.Format(@"UPDATE rabbits SET r_rate=r_rate+{0:d} WHERE r_id={1:d};",rt,ml.id);
                cmd.ExecuteNonQuery();
                ml.rate += rt;
            }
            rt -= dead;
            if (children > 0 && date.Month > 8 && date.Month < 12)
                rt += 2;
            cmd.CommandText = String.Format(@"UPDATE rabbits SET r_event_date=NULL,r_event='none',
r_status=r_status+1,r_last_fuck_okrol={1:s},r_overall_babies=COALESCE(r_overall_babies+{2:d},1),
r_lost_babies=COALESCE(r_lost_babies+{3:d},1),r_rate=r_rate+{4:d} WHERE r_id={0:d};", 
                rabbit,DBHelper.DateToMyString(date),children,dead,rt);
            fml.rate += rt;
            cmd.ExecuteNonQuery();
            if (children>0)
            {
                int brd=1;
                if (ml!=null && fml.breed==ml.breed)
                    brd=fml.breed;
                int rate = fml.rate + (ml==null?fml.rate:ml.rate)/2;
                int okrol = fml.status;
                cmd.CommandText = String.Format(@"INSERT INTO rabbits(r_parent,r_mother,r_father,r_born,r_sex,r_group,
r_bon,r_genesis,r_name,r_surname,r_secname,r_breed,r_okrol,r_rate,r_notes) 
VALUES({0:d},{1:d},{2:d},{3:s},'void',{4:d},'{5:s}',{6:d},0,{7:d},{8:d},{9:d},{10:d},{11:d},'');",
      rabbit,rabbit,father,DBHelper.DateToMyString(date),children,DBHelper.commonBon(fml.bon,ml!=null?ml.bon:fml.bon),
      DBHelper.makeCommonGenesis(sql,fml.gens,(ml!=null?ml.gens:fml.gens),fml.zone),fml.name,(ml!=null?ml.name:0),brd,okrol,rate);
                cmd.ExecuteNonQuery();
            }
        }

        public static String makeName(MySqlConnection con,int nm,int sur,int sec,int grp,OneRabbit.RabbitSex sex)
        {
            MySqlCommand cmd=new MySqlCommand(String.Format(@"SELECT
(SELECT CONCAT(n_name,' ') FROM names WHERE n_id={0:d}) name,
(SELECT n_surname FROM names WHERE n_id={1:d}) surname,
(SELECT n_surname FROM names WHERE n_id={2:d}) secname;
",nm,sur,sec),con);
            MySqlDataReader rd = cmd.ExecuteReader();
            String res = "";
            if (rd.Read())
            {
                res = rd.IsDBNull(0)?"":rd.GetString(0);
                String ssur = rd.IsDBNull(1) ? "" : rd.GetString(1);
                String ssec = rd.IsDBNull(2) ? "" : rd.GetString(2);
                if (grp>1)
                {
                    if (ssur!="") ssur+='ы';
                    if (ssec!="") ssec+='ы';
                }
                else if (sex==OneRabbit.RabbitSex.FEMALE)
                {
                    if (ssur != "") ssur += 'а';
                    if (ssec != "") ssec += 'а';
                }
                res += ssur;
                if (ssec != "")
                    res += "-"+ssec;
            }
            rd.Close();
            return res;
        }

        public static int[] freeTier(MySqlConnection sql,int rabbit)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT r_id,r_farm,r_tier,r_tier_id,
r_area,t_busy1,t_busy2,t_busy3,t_busy4,m_upper,m_lower,m_id FROM rabbits,tiers,minifarms WHERE r_id={0:d} AND r_tier=t_id AND m_id=r_farm", rabbit), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int tr = 0; 
            int sc = 0; 
            int frm = 0; 
            int tid = 0; 
            if (rd.Read())
            {
                tr = rd.GetInt32("r_tier");
                sc = rd.GetInt32("r_area");
                frm = rd.GetInt32("r_farm");
                tid = rd.GetInt32("r_tier_id");
                int bs = rd.GetInt32("t_busy" + (sc + 1).ToString());
                if (bs != rabbit)
                    tr = 0;
            }
            rd.Close();
            if (tr != 0)
            {
                cmd.CommandText = String.Format("UPDATE tiers SET t_busy{0:d}=0 WHERE t_id={1:d};", sc + 1, tr);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("select r_id from rabbits where r_farm={0:d} and r_tier={1:d} and r_tier_id={2:d} and r_area={3:d} and r_id<>{4:d} and r_parent=0 limit 1;",frm,tr,tid,sc,rabbit);
                rd = cmd.ExecuteReader();
                if(rd.Read())
                {
                    cmd.CommandText = String.Format("UPDATE tiers SET t_busy{0:d}={2:d} WHERE t_id={1:d};", sc + 1, tr,rd.GetInt32("r_id"));
                    rd.Close();
                    cmd.ExecuteNonQuery();                    
                }else rd.Close();
                
            }
            return new int[] { frm, tid, sc, tr };
        }

        public static void placeRabbit(MySqlConnection sql, int rabbit, int farm, int tier_id, int sec)
        {
            if (farm == 0)
            {
                MySqlCommand c = new MySqlCommand(String.Format("UPDATE rabbits SET r_farm=0,r_tier=0,r_tier_id=0,r_area=0 WHERE r_id={0:d};",rabbit), sql);
                c.ExecuteNonQuery();
                return;
            }
            MySqlCommand cmd = new MySqlCommand("", sql);
            cmd.CommandText = String.Format("SELECT {0:s} FROM minifarms WHERE m_id={1:d};", tier_id == 2 ? "m_lower" : "m_upper", farm);
            MySqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            int ntr = rd.GetInt32(0);
            rd.Close();
            cmd.CommandText = String.Format(@"UPDATE rabbits SET r_farm={0:d},r_tier_id={1:d},r_area={2:d},r_tier={3:d} 
WHERE r_id={4:d};", farm, tier_id, sec, ntr,rabbit);
            cmd.ExecuteNonQuery();
            if (farm!=0)
                cmd.CommandText = String.Format("UPDATE tiers SET t_busy{0:d}={1:d} WHERE t_id={2:d};", sec + 1, rabbit, ntr);
            cmd.ExecuteNonQuery();
        }

        public static void removeParent(MySqlConnection sql,int rabbit)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE rabbits SET r_parent=0 WHERE r_id="+rabbit.ToString()+";",sql);
            cmd.ExecuteNonQuery();
        }

        public static void replaceYounger(MySqlConnection sql, int rabbit, int farm, int tier_id, int sec)
        {
            removeParent(sql, rabbit);
            placeRabbit(sql, rabbit, farm, tier_id, sec);
        }

        public static void setRabbitSex(MySqlConnection sql, int rabbit, OneRabbit.RabbitSex sex)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE rabbits SET r_sex='{0:s}' WHERE r_id={1:d};",OneRabbit.SexToString(sex),rabbit), sql);
            cmd.ExecuteNonQuery();
        }

        public static int cloneRabbit(MySqlConnection sql,int rabbit,int count,int farm,int tier_id,int sec,OneRabbit.RabbitSex sex,int mom)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"INSERT INTO rabbits
(r_parent,r_father,r_mother,r_name,r_surname,r_secname,r_sex,r_bon,r_okrol,r_breed,r_rate,r_group,
r_flags,r_zone,r_born,r_genesis,r_status,r_last_fuck_okrol,r_event,r_event_date,r_notes) SELECT 
{1:d},r_father,r_mother,0,r_surname,r_secname,r_sex,r_bon,r_okrol,r_breed,r_rate,{2:d},
r_flags,r_zone,r_born,r_genesis,r_status,r_last_fuck_okrol,r_event,r_event_date,r_notes
FROM rabbits WHERE r_id={0:d};",rabbit,mom,count), sql);
            cmd.ExecuteNonQuery();
            int nid = (int)cmd.LastInsertedId;
            cmd.CommandText = String.Format("UPDATE rabbits SET r_group=r_group-{0:d} WHERE r_id={1:d};", count, rabbit);
            cmd.ExecuteNonQuery();
            if (sex != OneRabbit.RabbitSex.VOID)
                setRabbitSex(sql, rabbit, sex);
            if (mom == 0 && farm != 0)
                placeRabbit(sql, nid, farm, tier_id, sec);            
            return nid;
        }

        public static void replaceRabbit(MySqlConnection sql, int rabbit, int farm, int tier_id, int sec)
        {
            freeTier(sql, rabbit);
            placeRabbit(sql, rabbit, farm, tier_id, sec);
        }

        public static int newRabbit(MySqlConnection sql,OneRabbit r,int mom)
        {
            String qry=String.Format(@"INSERT INTO rabbits(r_sex,r_parent) VALUES('{0:s}',{1:d});",OneRabbit.SexToString(r.sex),mom);
            MySqlCommand cmd = new MySqlCommand(qry,sql);
            cmd.ExecuteNonQuery();
            r.id= (int)cmd.LastInsertedId;
            SetRabbit(sql, r);
            if (mom==0 && r.nuaddr != "" && r.nuaddr!=OneRabbit.NullAddress)
            {
                String[] adr = r.nuaddr.Split('|');
                placeRabbit(sql, r.id, int.Parse(adr[0]), int.Parse(adr[1]), int.Parse(adr[2]));
            }
            return r.id;
        }

        public static void freeName(MySqlConnection sql, int rid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT r_name FROM rabbits WHERE r_id={0:d};",rid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int nm = 0;
            if (rd.Read())
                nm = rd.GetInt32(0);
            rd.Close();
            if (nm>0)
            {
                cmd.CommandText = String.Format(@"UPDATE names SET n_use=0,n_block_date=NOW()+INTERVAL 1 YEAR WHERE n_id={0:d};",nm);
                cmd.ExecuteNonQuery();
            }
        }

        public static void killRabbit(MySqlConnection sql, int rid, DateTime when, int reason, string notes)
        {
            int[] place=freeTier(sql, rid);
            freeName(sql, rid);
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT r_id FROM rabbits WHERE r_parent={0:d};", rid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                int c1=rd.GetInt32(0);
                rd.Close();
                if (c1 != 0)
                {
                    placeRabbit(sql, c1, place[0], place[1], place[2]);
                    cmd.CommandText = String.Format("UPDATE rabbits SET r_parent=0 WHERE r_id={0:d};",c1);
                    cmd.ExecuteNonQuery();
                }
            }else rd.Close();
            cmd.CommandText=String.Format("UPDATE rabbits SET r_parent=0,r_farm={1:d},r_tier={2:d},r_area={3:d},r_tier={4:d} WHERE r_parent={0:d};", rid,place[0],place[1],place[2],place[3]);
            cmd.ExecuteNonQuery();
            cmd.CommandText=String.Format(@"CALL killRabbitDate({0:d},{1:d},'{2:s}',{3:s});",
                rid,reason,notes,DBHelper.DateToMyString(when));
            cmd.ExecuteNonQuery();
        }

        public static void countKids(MySqlConnection sql,int rid,int dead,int killed,int added,int yid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE rabbits SET 
r_group=r_group-{0:d}-{1:d}+{2:d},r_rate=r_rate-{4:d} WHERE r_parent={3:d} AND r_id={5:d};",dead,killed,added,rid,killed+dead,yid), sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(@"UPDATE fucks SET 
f_dead=f_dead+{0:d},f_killed=f_killed+{1:d},f_added=f_added+{2:d} WHERE f_rabid={3:d} AND f_last=1;",
                     dead,killed,added,rid);
            cmd.ExecuteNonQuery();
        }

        public static void placeSucker(MySqlConnection sql, int sucker, int mother)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT r_parent FROM rabbits WHERE r_id={0:d};",sucker), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int oldmom = 0;
            if (rd.Read())
                oldmom = rd.GetInt32(0);
            rd.Close();
            if (oldmom == 0)
            {
                freeTier(sql, sucker);
                placeRabbit(sql, sucker, 0, 0, 0);
            }
            cmd.CommandText = String.Format("UPDATE rabbits SET r_parent={0:d} WHERE r_id={1:d};",mother,sucker);
            cmd.ExecuteNonQuery();
        }

        public static void combineGroups(MySqlConnection sql, int rabfrom, int rabto)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("select r_mother,r_father,r_okrol from rabbits where r_id={0:d};", rabfrom), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            
            int[] Arabfrom = new int[3];            
            if (rd.Read())
            {
                Arabfrom[0] = rd.GetInt32("r_mother");
                Arabfrom[1] = rd.GetInt32("r_father");
                Arabfrom[2] = rd.GetInt32("r_okrol");
            }
            rd.Close();            
            cmd.CommandText = String.Format("select r_mother,r_father,r_okrol from rabbits where r_id={0:d};", rabto);
            rd = cmd.ExecuteReader();
            int[] Arabto = new int[3];
            if (rd.Read())
            {
                Arabto[0] = rd.GetInt32("r_mother");
                Arabto[1] = rd.GetInt32("r_father");
                Arabto[2] = rd.GetInt32("r_okrol");
            }
            rd.Close();

            if (Arabfrom[0] == Arabto[0] && Arabfrom[1] == Arabto[1] && Arabfrom[2] == Arabto[2])
            {
                cmd.CommandText = String.Format("SELECT r_group FROM rabbits WHERE r_id={0:d};", rabfrom);
                rd = cmd.ExecuteReader();
                int cnt = 0;
                if (rd.Read())
                    cnt = rd.GetInt32(0);
                rd.Close();
                cmd.CommandText = String.Format("UPDATE rabbits SET r_group=r_group+{0:d} WHERE r_id={1:d};", cnt, rabto);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("CALL killRabbit({0:d},1,'{1:d}');", rabfrom, rabto);
                cmd.ExecuteNonQuery();
            }
            else
            {
                freeTier(sql, rabfrom);
                cmd.CommandText = String.Format("Select r_farm,r_tier,r_tier_id,r_area from rabbits where r_id={0:d};",rabto);
                rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    cmd.CommandText = String.Format("UPDATE rabbits SET r_farm={0:d},r_tier={1:d},r_tier_id={2:d},r_area={3:d} WHERE r_id={4:d}", rd.GetInt32("r_farm"), rd.GetInt32("r_tier"), rd.GetInt32("r_tier_id"), rd.GetInt32("r_area"), rabfrom);
                    rd.Close();
                    cmd.ExecuteNonQuery();
                }
                else rd.Close();
            }
        }

        public static Rabbit[] getMothers(MySqlConnection sql,int age,int agediff)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT * FROM (SELECT r_id,TO_DAYS(NOW())-TO_DAYS(r_born) age,r_status,
(SELECT b_name FROM breeds WHERE b_id=r_breed) breed,r_rate,rabname(r_id,2) name,
(SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id) suckers,
(SELECT AVG(TO_DAYS(NOW())-TO_DAYS(r2.r_born)) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id) aage,
r_born,rabplace(r_id) place,
r_event_date
FROM rabbits WHERE r_sex='female' AND r_group=1 AND (r_status>0 OR (r_status=0 AND r_event_date IS NOT NULL))) c WHERE suckers>0 AND ABS(aage-{0:d})<={1:d};
",age,agediff),sql);
            List<Rabbit> rbs=new List<Rabbit>();
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Rabbit r = new Rabbit(rd.GetInt32("r_id"));
                r.fname = rd.GetString("name");
                r.fbreed = rd.GetString("breed");
                r.frate = rd.GetInt32("r_rate");
                r.fage = rd.GetInt32("age");
                r.faverage = rd.GetInt32("aage");
                r.fN = rd.GetString("suckers");
                r.fstatus = ((rd.GetInt32("r_status") == 1 && rd.IsDBNull(9)) || (rd.GetInt32("r_status") == 0 && !rd.IsDBNull(9))) ? "Первокролка" : "Штатная";
                r.faddress = Buildings.fullPlaceName(rd.GetString("place"), true, false, false);
                rbs.Add(r);
            }
            rd.Close();
            return rbs.ToArray();
        }

        public static int getRabByName(MySqlConnection sql,int name,int agebefore)
        {
            if (name==0) return 0;
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT r_id FROM rabbits WHERE r_name={0:d} AND TO_DAYS(NOW())-TO_DAYS(r_born)>{1:d};",name,agebefore), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int id = 0;
            if (rd.Read())
                id = rd.GetInt32(0);
            rd.Close();
            if (id == 0)
            {
                cmd.CommandText = String.Format(@"SELECT r_id,TO_DAYS(NOW())-TO_DAYS(r_born) age FROM dead 
WHERE r_name={0:d} AND TO_DAYS(NOW())-TO_DAYS(r_born)>{1:d} AND 
TO_DAYS(NOW())-TO_DAYS(r_born)<{1:d}+1000 ORDER BY age ASC;",name,agebefore);
                rd = cmd.ExecuteReader();
                if (rd.Read())
                    id = rd.GetInt32(0);
                rd.Close();
            }
            return id;
        }

        public static OneRabbit getLiveDeadRabbit(MySqlConnection sql, int rabbit)
        {
            if (rabbit == 0) return null;
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT isdead({0:d});",rabbit), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            bool dead=false;
            if (rd.Read())
                dead = rd.GetBoolean(0);
            rd.Close();
            cmd.CommandText = String.Format(@"SELECT 	r_id,
														r_name,
														r_surname,
														r_secname,
														{0:s}name(r_id,0) name,
														{0:s}place(r_id) place,
														r_bon,
														(SELECT w_weight FROM weights WHERE w_rabid=r_id AND w_date=(SELECT MAX(w_date) FROM weights WHERE w_rabid=r_id)) weight,
														(SELECT MAX(w_date) FROM weights WHERE w_rabid=r_id) weight_date,
														(SELECT TO_DAYS(MAX(w_date))-TO_DAYS(r_born) FROM weights WHERE w_rabid=r_id) weight_age,
														r_overall_babies,
														r_okrol,
														r_sex,
														(SELECT TO_DAYS({1:s})-TO_DAYS(r_born)) age,
														r_born, 
														r_lost_babies
											FROM {2:s} WHERE r_id={3:d};", (dead ? "dead" : "rab"), (dead ? "d_date" : "NOW()"), (dead ? "dead" : "rabbits"),rabbit);
            rd = cmd.ExecuteReader();
            if (!rd.Read())
            {
                rd.Close();
                return null;
            }
            OneRabbit r = new OneRabbit(rabbit, rd.GetString("r_sex"), rd.GetDateTime("r_born"), rd.IsDBNull(7) ? 0 : rd.GetInt32("weight"),
                "00000", 0, 0, 0, rd.GetString("place"), 1, rd.GetInt32("r_okrol"), dead?1:0, "", "", rd.GetInt32("age"), DateTime.MinValue, "", rd.IsDBNull(8) ? DateTime.MinValue : rd.GetDateTime("weight_date"),
				rd.IsDBNull(10) ? 0 : rd.GetInt32("r_overall_babies"), rd.IsDBNull(15) ? 0 : rd.GetInt32("r_lost_babies"),
				rd.GetString("name"), "", rd.GetString("r_bon"), 0, rd.IsDBNull(9) ? 0 : rd.GetInt32("weight_age"));
            rd.Close();
            return r;
        }

        public static OneRabbit[] getParents(MySqlConnection sql,int rabbit,int age)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT r_mother,r_father,r_surname,r_secname FROM rabbits WHERE r_id={0:d}",rabbit),sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int mom=0, pap=0, mname=0, pname=0;
            if (rd.Read())
            {
                mom = rd.GetInt32(0);
                pap = rd.GetInt32(1);
                mname = rd.GetInt32(2);
                pname = rd.GetInt32(3);
            }
            rd.Close();
            if (mom == 0)
                mom = getRabByName(sql, mname, age);
            if (pap == 0)
                pap = getRabByName(sql, pname, age);
            return new OneRabbit[] {getLiveDeadRabbit(sql,mom),getLiveDeadRabbit(sql,pap) };
        }

    }
}
