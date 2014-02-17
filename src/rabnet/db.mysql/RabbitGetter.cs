﻿using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using rabnet;

namespace db.mysql
{
    class RabbitGetter
    {        

        internal static Rabbit fillRabbit(MySqlDataReader rd)
        {
            return new Rabbit(rd.GetInt32("r_id"), rd.GetString("name"), rd.GetString("r_sex"), rd.GetDateTime("r_born"),
                rd.GetString("breedname"), rd.GetInt32("r_group"),rd.GetString("r_bon"),rd.GetString("address"),
                rd.GetString("r_notes"));
        }

        internal static DeadRabbit fillDeadRabbit(MySqlDataReader rd)
        {
            return new DeadRabbit(rd.GetInt32("r_id"), rd.GetDateTime("d_date"),rd.GetString("name"), rd.GetString("r_sex"),
                            rd.IsDBNull(rd.GetOrdinal("r_born")) ? DateTime.MinValue : rd.GetDateTime("r_born"),
                            rd.GetString("breed"), rd.GetInt32("r_group"),
                            rd.GetString("r_bon"), rd.GetString("place"), rd.GetString("r_notes"));
        }

        internal static AdultRabbit fillAdultRabbit(MySqlDataReader rd)
        {
            return new AdultRabbit(rd.GetInt32("r_id"), rd.GetString("name"), rd.GetString("r_sex"),
                rd.IsDBNull(rd.GetOrdinal("r_born")) ? DateTime.MinValue : rd.GetDateTime("r_born"),
                rd.GetString("breed"), rd.GetInt32("r_group"),
                rd.GetString("r_bon"), rd.GetString("place"), rd.GetString("r_notes"),
                rd.GetInt32("r_rate"), rd.GetString("r_flags"), rd.GetInt32("weight"), rd.GetInt32("r_status"),
                rd.IsDBNull(rd.GetOrdinal("r_event_date")) ? DateTime.MinValue : rd.GetDateTime("r_event_date"),
                rd.IsDBNull(rd.GetOrdinal("suckers")) ? 0 : rd.GetInt32("suckers"),
                rd.IsDBNull(rd.GetOrdinal("suckGroups")) ? 0 : rd.GetInt32("suckGroups"),
                rd.IsDBNull(rd.GetOrdinal("aage")) ? 0 : rd.GetInt32("aage"),
                rd.GetString("vaccines"));
        }

        internal static OneRabbit fillOneRabbit(MySqlDataReader rd)
        {
            return new OneRabbit(rd.GetInt32("r_id"), rd.GetString("r_sex"), rd.GetDateTime("r_born"), rd.GetInt32("r_rate"),
                rd.GetString("r_flags"), rd.GetInt32("r_name"), rd.GetInt32("r_surname"), rd.GetInt32("r_secname"),               
                rd.GetString("address"), rd.GetInt32("r_group"), rd.GetInt32("r_breed"),
                rd.GetInt32("r_zone"),
                rd.IsDBNull(rd.GetOrdinal("r_notes")) ? "" : rd.GetString("r_notes"),
                rd.IsDBNull(rd.GetOrdinal("genom")) ? "" : rd.GetString("genom"),
                rd.GetInt32("r_status"),
                rd.IsDBNull(rd.GetOrdinal("r_last_fuck_okrol")) ? DateTime.MinValue : rd.GetDateTime("r_last_fuck_okrol"),
                rd.IsDBNull(rd.GetOrdinal("r_event")) ? "none" : rd.GetString("r_event"),
                rd.IsDBNull(rd.GetOrdinal("r_event_date")) ? DateTime.MinValue : rd.GetDateTime("r_event_date"),
                rd.IsDBNull(rd.GetOrdinal("r_overall_babies")) ? 0 : rd.GetInt32("r_overall_babies"),
                rd.IsDBNull(rd.GetOrdinal("r_lost_babies")) ? 0 : rd.GetInt32("r_lost_babies"),
                rd.GetString("fullname"),
                rd.GetString("breedname"),
                rd.GetString("r_bon"),
                rd.GetInt32("r_parent"),
                rd.GetInt32("r_okrol"), 
                rd.IsDBNull(rd.GetOrdinal("weight")) ? 0 :rd.GetInt32("weight"),
                rd.IsDBNull(rd.GetOrdinal("weight_date")) ?  DateTime.MinValue: rd.GetDateTime("weight_date"),
                rd.GetInt32("r_mother"), rd.GetInt32("r_father"),
                rd.IsDBNull(rd.GetOrdinal("r_birthplace")) ? 0 : rd.GetInt32("r_birthplace"));
        }

        private static bool isDeadRabbit(MySqlConnection con, int rid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT isdead({0:d});", rid), con);
            MySqlDataReader rd = cmd.ExecuteReader();
            bool dead = false;
            if (rd.Read())
                dead = rd.GetBoolean(0);
            rd.Close();
            return dead;
        }    

        public static OneRabbit GetRabbit(MySqlConnection sql, int rid, RabAliveState type)
        {
            if (rid == 0) return null;
            if (type == RabAliveState.ANY)
                type = (isDeadRabbit(sql, rid) ? RabAliveState.DEAD : RabAliveState.ALIVE);
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT {0:s}
FROM {1:s} WHERE r_id={2:d};", getOneRabbit_FieldsSet(type), (type == RabAliveState.ALIVE ? "rabbits" : "dead"), rid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (!rd.Read())
            {
                rd.Close();
                return null;
            }
            OneRabbit r = fillOneRabbit(rd);
            rd.Close();
            //r.youngers = GetYoungers(sql, rid);//todo убрать в RabNetEngRabbit
            //r.rabVacs = GetRabVacs(sql, rid);
            //if (r.ParentId == 0)
                //r.Neighbors = GetNeighbors(sql, rid);
            return r;
        }
        public static OneRabbit GetRabbit(MySqlConnection con, int rid)
        {
            return GetRabbit(con, rid, RabAliveState.ALIVE);
        }
        
        /// <summary>
        /// Получает список кролико у кого в адресе числится такая же клетка
        /// </summary>
        /// <param name="con"></param>
        /// <param name="rabOwner"></param>
        /// <returns></returns>
        public static OneRabbit[] GetNeighbors(MySqlConnection con, int rabOwner) //TODO не исправлено
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT {0:s}
FROM rabbits r
INNER JOIN (SELECT r_farm,r_tier,r_tier_id,r_area FROM rabbits WHERE r_id={1:d}) rp ON rp.r_farm=r.r_farm 
	AND rp.r_tier=r.r_tier AND rp.r_tier_id=r.r_tier_id AND rp.r_area=r.r_area
WHERE r_id!={1:d} AND r_parent=0;", getOneRabbit_FieldsSet(RabAliveState.ALIVE), rabOwner), con);
            List<OneRabbit> rbs = new List<OneRabbit>();
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
                rbs.Add(fillOneRabbit(rd));
            rd.Close();
            return rbs.ToArray();
        }

        public static void SetRabbit(MySqlConnection con, OneRabbit r)
        {
            int multi = (r.Defect ? 1 : 0);
            String flags = String.Format("{0:D1}{1:D1}{2:D1}{3:D1}{4:D1}", r.Production ? 1 : 0, r.RealizeReady ? 1 : 0, multi, r.NoKuk ? 1 : 0, r.NoLact ? 1 : 0);//TODO возможен косяк
            String query = String.Format(@"UPDATE rabbits SET 
r_name={0:d},r_surname={1:d},r_secname={2:d},r_breed={3:d},r_zone={4:d},r_group={5:d},r_notes=@notes,
r_flags='{6:d}',r_rate={7:d},r_born={8:s} ", r.NameID, r.SurnameID, r.SecnameID, r.BreedID, r.Zone, r.Group, /*r.Notes,*/ flags, r.Rate, DBHelper.DateToSqlString(r.BirthDay));



            if (r.Sex != Rabbit.SexType.VOID)
            {
                query += String.Format(",r_status={0:d},r_last_fuck_okrol={1:s}", r.Status, DBHelper.DateToSqlString(r.LastFuckOkrol));
            }
            if (r.Sex == Rabbit.SexType.FEMALE)
            {
                query += String.Format(",r_event='{0:s}',r_event_date={1:s},r_lost_babies={2:d},r_overall_babies={3:d}", Rabbit.GetEventName(r.EventType), DBHelper.DateToSqlString(r.EventDate), r.KidsLost, r.KidsOverAll);
            }
            query += String.Format(" WHERE r_id={0:d};", r.ID);

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@notes", r.Notes);
            cmd.ExecuteNonQuery();

            int gen = RabbitGenGetter.MakeGenesis(con, r.Genoms);
            cmd.CommandText = String.Format("UPDATE rabbits SET r_genesis={0:d} WHERE r_id={1:d};", gen,r.ID);
            cmd.ExecuteNonQuery();
            if (r.WasNameID != r.NameID)
            {
                ///todo  проверка на используемость
                cmd.CommandText = String.Format("UPDATE names SET n_use=0,n_block_date=NULL WHERE n_id={0:d};", r.WasNameID);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("UPDATE names SET n_use={0:d} WHERE n_id={1:d};", r.ID, r.NameID);
                cmd.ExecuteNonQuery();
            }
        }

        public static void setBon(MySqlConnection sql, int rabbit, String bon)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE rabbits SET r_bon='{0:s}' WHERE r_id={1:d};", bon, rabbit), sql);
            cmd.ExecuteNonQuery();
        }      

        public static void MakeProholost(MySqlConnection sql, int rabbit, int daysPast)
        {
            int male = whosChildren(sql, rabbit);
            string when = DBHelper.DaysPastSqlDate(daysPast);
            MySqlCommand cmd = new MySqlCommand("", sql);
            checkStartEvDate(sql, rabbit);
            cmd.CommandText = String.Format(@"UPDATE fucks SET f_state='proholost',f_end_date={0:s} WHERE f_state='sukrol' AND f_rabid={1:d};",
                when, rabbit);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format("UPDATE rabbits SET r_event_date=NULL,r_event='none',r_rate=r_rate+{1:d} WHERE r_id={0:d};", rabbit,Rate.PROHOLOST_RATE);
            cmd.ExecuteNonQuery();
            if (male != 0)
            {
                cmd.CommandText = String.Format("UPDATE rabbits SET r_rate=r_rate+{1:d} WHERE r_id={0:d};", male,Rate.PROHOLOST_RATE);
                cmd.ExecuteNonQuery();
            }
        }       

        public static int MakeOkrol(MySqlConnection sql, int rabbit, int daysPast, int children, int dead)
        {            
            int father = whosChildren(sql, rabbit);
            string when = DBHelper.DaysPastSqlDate(daysPast);

            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE fucks SET f_state='okrol',f_end_date={0:s},
f_children={1:d},f_dead={2:d} WHERE f_rabid={3:d} AND f_state='sukrol';",
                       when, children, dead, rabbit), sql);
            cmd.ExecuteNonQuery();

            OneRabbit fml = GetRabbit(sql, rabbit);
            OneRabbit ml = GetRabbit(sql, father, RabAliveState.ANY);
            int rt = Rate.CalcRate(children, dead, false);
            if (rt != 0 && ml != null)
            {
                cmd.CommandText = String.Format(@"UPDATE rabbits SET r_rate=r_rate+{0:d} WHERE r_id={1:d};", rt, ml.ID);
                cmd.ExecuteNonQuery();
                ml.Rate += rt;
            }

            // mother status update
            rt = Rate.CalcRate(children, dead, true);
            fml.Rate += rt;
            cmd.CommandText = String.Format(@"UPDATE rabbits SET r_event_date=NULL, r_event='none',
r_status=r_status+1, r_last_fuck_okrol={1:s}, r_overall_babies=COALESCE(r_overall_babies+{2:d},1),
r_lost_babies=COALESCE(r_lost_babies+{3:d},1), r_rate=r_rate+{4:d}
WHERE r_id={0:d};",
                rabbit, when, children, dead, rt);           
            cmd.ExecuteNonQuery();

            if (children > 0)
            {
                int brd = 1;
                if (ml != null && fml.BreedID == ml.BreedID)
                    brd = fml.BreedID;
                int chRate = Rate.CalcChildrenRate(fml.Rate, ml == null ? 0 : ml.Rate);
                int okrol = fml.Status;
                cmd.CommandText = String.Format(@"INSERT 
INTO rabbits(r_parent,r_mother,r_father,r_born,r_sex,r_group,r_bon,r_genesis,r_name,r_surname,r_secname,r_breed,r_okrol,r_rate,r_notes) 
VALUES({0:d},{1:d},{2:d},{3:s},'void',{4:d},'{5:s}',{6:d},0,{7:d},{8:d},{9:d},{10:d},{11:d},'');",
      rabbit, rabbit, father, when, children, DBHelper.commonBon(fml.Bon.ToString(), (ml != null ? ml.Bon.ToString() : fml.Bon.ToString())),
      bornRabbitGenesis(sql,fml,ml),//RabbitGenGetter.MakeCommonGenesis(sql, fml.Genoms, (ml != null ? ml.Genoms : fml.Genoms), fml.Zone),
      fml.NameID, (ml != null ? ml.NameID : 0), brd, okrol, chRate/*,DBHelper.DateToMyString(date)*/);
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
            return 0;
        }

        /// <summary>
        /// Если 7 поколений рожденных прошло через программу, то номера генов отметаются.
        /// </summary>
        /// <returns>Генезис ID</returns>
        private static int bornRabbitGenesis(MySqlConnection sql,OneRabbit fml,OneRabbit ml)
        {
            int fLevel = 0, mLevel = 0;           
            fml.RabGenoms = RabbitGenGetter.GetRabGenoms(sql, fml.ID);
            RabbitGen.GetFullGenLevels(fml.RabGenoms, ref fLevel);
            
            if (ml != null)
            {
                ml.RabGenoms = RabbitGenGetter.GetRabGenoms(sql, ml.ID);
                RabbitGen.GetFullGenLevels(ml.RabGenoms, ref mLevel);
            }

            MySqlCommand cmd = new MySqlCommand("SELECT o_value FROM options WHERE o_name='opt' AND o_subname='rab_gen_depth'", sql);
            object o = cmd.ExecuteScalar();
            if (o != null)
            {
                int rab_gen_depth =0;
                if(int.TryParse(o.ToString(),out rab_gen_depth))
                {
                    if (Math.Min(fLevel, mLevel) >= rab_gen_depth)
                        return 0;
                }
            }

            return RabbitGenGetter.MakeCommonGenesis(sql, fml.Genoms, (ml != null ? ml.Genoms : fml.Genoms), fml.Zone);
        }

        private static int whosChildren(MySqlConnection sql, int rabbit)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT f_partner FROM fucks WHERE f_state='sukrol' AND f_rabid={0:d};", rabbit), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int res = 0;
            if (rd.Read())
                res = rd.GetInt32(0);
            rd.Close();
            return res;
        }

        public static Rabbit[] GetDescendants(MySqlConnection sql, int ascendantId)
        {
            List<Rabbit> res = new List<Rabbit>();
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT r_id,r_born,r_okrol FROM rabbits WHERE r_mother={0:d} OR r_father={0:d}",ascendantId),sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                Rabbit r = new Rabbit();
                r.ID = rd.GetInt32("r_id");
                r.BirthDay = rd.GetDateTime("r_born");
                res.Add(r);
            }
            rd.Close();
            return res.ToArray();
        }

        public static String makeName(MySqlConnection con, int nm, int sur, int sec, int grp, Rabbit.SexType sex)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT
(SELECT CONCAT(n_name,' ') FROM names WHERE n_id={0:d}) name,
(SELECT n_surname FROM names WHERE n_id={1:d}) surname,
(SELECT n_surname FROM names WHERE n_id={2:d}) secname;
", nm, sur, sec), con);
            MySqlDataReader rd = cmd.ExecuteReader();
            String res = "";
            if (rd.Read())
            {
                res = rd.IsDBNull(0) ? "" : rd.GetString(0);
                String ssur = rd.IsDBNull(1) ? "" : rd.GetString(1);
                String ssec = rd.IsDBNull(2) ? "" : rd.GetString(2);
                if (grp > 1)
                {
                    if (ssur != "") ssur += 'ы';
                    if (ssec != "") ssec += 'ы';
                }
                else if (sex == Rabbit.SexType.FEMALE)
                {
                    if (ssur != "") ssur += 'а';
                    if (ssec != "") ssec += 'а';
                }
                res += ssur;
                if (ssec != "")
                    res += "-" + ssec;
            }
            rd.Close();
            return res;
        }

        /// <summary>
        /// Освобождает клетку, в которой сидел кролик.
        /// </summary>
        /// <param name="rabbit">ID кролика, который сидит в клетке</param>
        public static int[] freeTier(MySqlConnection sql, int rabbit)
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
                cmd.ExecuteNonQuery();///Освобождает клетку
                ///Проверяет наличие кролика в таблице  rabbits с таким же адресом
                cmd.CommandText = String.Format("SELECT r_id FROM rabbits WHERE r_farm={0:d} AND r_tier={1:d} AND r_tier_id={2:d} AND r_area={3:d} AND r_id<>{4:d} AND r_parent=0 LIMIT 1;", frm, tr, tid, sc, rabbit);
                rd = cmd.ExecuteReader();
                if (rd.Read())///если имеется, то клетка в таблице tiers заселяется найденным кроликом
                {
                    cmd.CommandText = String.Format("UPDATE tiers SET t_busy{0:d}={2:d} WHERE t_id={1:d};", sc + 1, tr, rd.GetInt32("r_id"));
                    rd.Close();
                    cmd.ExecuteNonQuery();
                }
                else rd.Close();

            }
            return new int[] { frm, tid, sc, tr };
        }

        public static void placeRabbit(MySqlConnection sql, int rabbit, int farm, int tierFloor, int sec)
        {
            if (farm == 0)
            {
                MySqlCommand c = new MySqlCommand(String.Format("UPDATE rabbits SET r_farm=0,r_tier=0,r_tier_id=0,r_area=0 WHERE r_id={0:d};", rabbit), sql);
                c.ExecuteNonQuery();
                return;
            }
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT {0:s} FROM minifarms WHERE m_id={1:d};", tierFloor == TierFloor.Lower ? "m_lower" : "m_upper", farm), sql);            
            MySqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            int tierId = rd.GetInt32(0);
            rd.Close();

            cmd.CommandText = String.Format(@"UPDATE rabbits SET r_farm={0:d}, r_tier_id={1:d}, r_area={2:d}, r_tier={3:d} WHERE r_id={4:d};", farm, tierFloor, sec, tierId, rabbit);
            cmd.ExecuteNonQuery();
            if (farm != 0)
                cmd.CommandText = String.Format("UPDATE tiers SET t_busy{0:d}={1:d} WHERE t_id={2:d};", sec + 1, rabbit, tierId);
            cmd.ExecuteNonQuery();
        }

        private static void removeParent(MySqlConnection sql, int rabbit)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE rabbits SET r_parent=0 WHERE r_id=" + rabbit.ToString() + ";", sql);
            cmd.ExecuteNonQuery();
        }

        //public static void replaceYounger(MySqlConnection sql, int rabbit, int farm, int tier_id, int sec)
        //{
        //    removeParent(sql, rabbit);
        //    placeRabbit(sql, rabbit, farm, tier_id, sec);
        //}

        public static void setRabbitSex(MySqlConnection sql, int rabbit, Rabbit.SexType sex)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE rabbits SET r_sex='{0:s}' WHERE r_id={1:d};", Rabbit.SexToString(sex), rabbit), sql);
            cmd.ExecuteNonQuery();
        }

        public static int cloneRabbit(MySqlConnection sql, int rabFromID, int count, Rabbit.SexType sex, int mom)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"INSERT INTO rabbits
    (r_parent,r_father,r_mother,r_name,r_surname,r_secname,r_sex,r_bon,r_okrol,r_breed,r_rate,r_group,
    r_flags,r_zone,r_born,r_genesis,r_status,r_last_fuck_okrol,r_event,r_event_date,r_notes,
    r_farm,r_tier,r_tier_id,r_area) 
SELECT {1:d},r_father,r_mother,0,r_surname,r_secname,r_sex,r_bon,r_okrol,r_breed,r_rate,{2:d},
    r_flags,r_zone,r_born,r_genesis,r_status,r_last_fuck_okrol,r_event,r_event_date,r_notes,
    r_farm,r_tier,r_tier_id,r_area
FROM rabbits WHERE r_id={0:d};", rabFromID, mom, count), sql);
            cmd.ExecuteNonQuery();
            int cloneID = (int)cmd.LastInsertedId;
            cmd.CommandText = String.Format("UPDATE rabbits SET r_group=r_group-{0:d} WHERE r_id={1:d};", count, rabFromID);
            cmd.ExecuteNonQuery();
            //клонируем прививки
            cmd.CommandText = String.Format("INSERT INTO rab_vac(r_id,v_id,`date`,unabled) SELECT {0:d},v_id,`date`,unabled FROM rab_vac WHERE r_id={1:d};",cloneID,rabFromID);
            cmd.ExecuteNonQuery();
            if (sex != Rabbit.SexType.VOID)
                setRabbitSex(sql, rabFromID, sex);            
            return cloneID;
        }

        public static void replaceRabbit(MySqlConnection sql, int rabbit, int farm, int tierFloor, int sec)
        {
            removeParent(sql, rabbit);
            freeTier(sql, rabbit);
            placeRabbit(sql, rabbit, farm, tierFloor, sec);
        }

        public static int newRabbit(MySqlConnection sql, OneRabbit r, int mom)
        {
            String query = String.Format(@"INSERT INTO rabbits(r_sex,r_parent) VALUES('{0:s}',{1:d});", Rabbit.SexToString(r.Sex), mom);
            MySqlCommand cmd = new MySqlCommand(query, sql);
            cmd.ExecuteNonQuery();
            r.ID = (int)cmd.LastInsertedId;
            SetRabbit(sql, r);
            if (mom == 0 && r.NewAddress != "" && r.NewAddress != Rabbit.NULL_ADDRESS)
            {
                String[] adr = r.NewAddress.Split('|');///RabNetEngRabbit.ReplaceRabbit
                placeRabbit(sql, r.ID, int.Parse(adr[0]), int.Parse(adr[1]), int.Parse(adr[2]));
            }
            if (r.MotherID != 0 || r.FatherID != 0)
            {
                cmd.CommandText = String.Format("UPDATE rabbits SET r_mother={0:d},r_father={1:d} WHERE r_id={2:d}", r.MotherID, r.FatherID, r.ID);
            	cmd.ExecuteNonQuery();
            }
            if (r.BirthPlace != 0)
            {
                cmd.CommandText = String.Format("UPDATE rabbits SET r_birthplace={0:d} WHERE r_id={1:d}", r.BirthPlace, r.ID);
                cmd.ExecuteNonQuery();
            }
            return r.ID;
        }

        /// <summary>
        /// Освобождает имя
        /// </summary>
        /// <param name="rid">ID кролика</param>
        private static void freeName(MySqlConnection sql, int rid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT r_name FROM rabbits WHERE r_id={0:d};", rid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int nm = 0;
            if (rd.Read())
                nm = rd.GetInt32(0);
            rd.Close();
            if (nm > 0)
            {
                cmd.CommandText = String.Format(@"UPDATE names SET n_use=0,n_block_date=NOW()+INTERVAL 1 YEAR WHERE n_id={0:d};", nm);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Списывает кролика.
        /// Если кролик подсосный, то обнуляет поле с Кормилицей.
        /// Если кролик кормилица, то обнуляет детям поле с кормилицей
        /// </summary>
        /// <param name="sql">sql-подключение</param>
        /// <param name="rid">ID кролика</param>
        /// <param name="when">Дата списания</param>
        /// <param name="reason">Причина списания</param>
        /// <param name="notes">Заметки</param>
        public static void killRabbit(MySqlConnection sql, int rid, int daysPast, int reason, string notes)
        {
            if (rid == 0) return;

            string when = DBHelper.DaysPastSqlDate(daysPast);
            int[] place = freeTier(sql, rid);
            freeName(sql, rid);
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT r_parent FROM rabbits WHERE r_id={0:d};", rid), sql);
            if (cmd.ExecuteScalar().ToString() != "0")//если подсосный
            {
                cmd.CommandText = String.Format("UPDATE rabbits SET r_parent=0 WHERE r_id={0:d};", rid);
                cmd.ExecuteNonQuery();
            }
            else
            {
                cmd = new MySqlCommand(String.Format("SELECT r_id FROM rabbits WHERE r_parent={0:d};", rid), sql);
                MySqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    int c1 = rd.GetInt32(0);
                    rd.Close();
                    if (c1 != 0)
                    {
                        placeRabbit(sql, c1, place[0], place[1], place[2]);
                        cmd.CommandText = String.Format("UPDATE rabbits SET r_parent=0 WHERE r_id={0:d};", c1);
                        cmd.ExecuteNonQuery();
                    }
                }
                else rd.Close();
            }
            cmd.CommandText = String.Format("UPDATE rabbits SET r_parent=0,r_farm={1:d},r_tier={2:d},r_area={3:d},r_tier={4:d} WHERE r_parent={0:d};", rid, place[0], place[1], place[2], place[3]);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(@"COMMIT; CALL killRabbitDate({0:d},{1:d},'{2:s}',{3:s});",
                rid, reason, notes, when);

            cmd.ExecuteNonQuery();
        }

        public static void countKids(MySqlConnection sql, int rid, int dead, int killed, int added, int yid)
        {
            /*MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE rabbits SET 
r_group=r_group-{0:d}-{1:d}+{2:d},r_rate=r_rate-{4:d} WHERE r_parent={3:d} AND r_id={5:d};",dead,killed,added,rid,killed+dead,yid), sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(@"UPDATE fucks SET 
f_dead=f_dead+{0:d},f_killed=f_killed+{1:d},f_added=f_added+{2:d} WHERE f_rabid={3:d} AND f_last=1;",
                     dead,killed,added,rid);
            cmd.ExecuteNonQuery();*/
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE rabbits SET r_group=r_group+{0:d} 
WHERE r_parent={1:d} AND r_id={2:d};", added, rid, yid), sql);
            cmd.ExecuteNonQuery();

            cmd.CommandText = String.Format("UPDATE rabbits SET r_rate={0:d} WHERE r_id={1:d};", dead + killed, rid);
            cmd.ExecuteNonQuery();

            cmd.CommandText = String.Format(@"UPDATE fucks SET 
                f_dead=f_dead+{0:d}, f_killed=f_killed+{1:d}, f_added=f_added+{2:d} 
                WHERE f_rabid={3:d} AND f_end_date=(SELECT r_born FROM rabbits WHERE r_id={4:d});", dead, killed, added, rid, yid);
            cmd.ExecuteNonQuery();
        }

        public static void placeSucker(MySqlConnection sql, int sucker, int mother)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT r_parent FROM rabbits WHERE r_id={0:d};", sucker), sql);
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
            cmd.CommandText = String.Format("UPDATE rabbits SET r_parent={0:d} WHERE r_id={1:d};", mother, sucker);
            cmd.ExecuteNonQuery();
        }

        public static void combineGroups(MySqlConnection sql, int rabfrom, int rabto)
        {
            MySqlCommand cmd = new MySqlCommand("", sql);//(String.Format("SELECT r_mother,r_father,r_okrol from rabbits where r_id={0:d};", rabfrom), sql);
            MySqlDataReader rd; //= cmd.ExecuteReader();
            OneRabbit rabFrom = GetRabbit(sql,rabfrom);
            OneRabbit rabTo = GetRabbit(sql, rabto);
            ///если это ранее разбитые кролики на 2 группы
            if (rabFrom.MotherID == rabTo.MotherID && 
                rabFrom.FatherID == rabTo.FatherID && 
                rabFrom.Okrol == rabTo.Okrol && 
                rabFrom.Sex == rabTo.Sex &&
                rabFrom.BreedID == rabTo.BreedID &&
                rabFrom.MotherID!=0 && rabFrom.FatherID!=0)
            {
                cmd.CommandText = String.Format("UPDATE rabbits SET r_group=r_group+{0:d} WHERE r_id={1:d};", rabFrom.Group, rabto);
                cmd.ExecuteNonQuery();
                freeTier(sql, rabfrom);
                cmd.CommandText = String.Format("CALL killRabbit({0:d},2,'{1:d}');", rabfrom, String.Format("Объединен с {0:s} [{1:d}] в {2:s}",rabTo.NameFull,rabTo.ID,rabTo.AddressSmall));
                cmd.ExecuteNonQuery();
            }
            else ///если подселение
            {
                freeTier(sql, rabfrom);
                cmd.CommandText = String.Format("SELECT r_farm, r_tier, r_tier_id, r_area FROM rabbits WHERE r_id={0:d};", rabto);
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

        public static AdultRabbit[] getMothers(MySqlConnection sql, int age, int agediff)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT * FROM (SELECT r_id,
        rabname(r_id,2) name,
        r_sex,
        r_born,
        (SELECT b_name FROM breeds WHERE b_id=r_breed) breed,
        r_group,
        r_rate,
        r_bon,
        rabplace(r_id) place,
        r_notes,
        r_flags,
        '-1' weight,
        r_status,
        r_event_date,
        suckers,
        suckGroups,
        aage,
        '' vaccines
    FROM rabbits r
    LEFT JOIN (SELECT r_parent prnt,SUM(r2.r_group) suckers,COUNT(*) suckGroups, AVG(TO_DAYS(NOW())-TO_DAYS(r2.r_born)) aage FROM rabbits r2 GROUP BY r_parent) sc ON prnt=r.r_id
    WHERE r_sex='female' AND r_group=1 AND (r_status>0 OR (r_status=0 AND r_event_date IS NOT NULL))
) c WHERE suckers>0 AND ABS(aage-{0:d})<={1:d};
", age, agediff), sql);
            List<AdultRabbit> rbs = new List<AdultRabbit>();
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                rbs.Add(fillAdultRabbit(rd));
            }
            rd.Close();
            return rbs.ToArray();
        }

        /// <summary>
        /// Ищет ID кролика по ID имени
        /// </summary>
        /// <param name="sql">MySqlConnection</param>
        /// <param name="nameId">ID имени</param>
        /// <param name="minAge">Минимальный возвраст кролика</param>
        /// <returns></returns>
        public static int getRabByName(MySqlConnection sql, int nameId, int minAge)
        {
            if (nameId == 0) return 0;
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT r_id FROM rabbits WHERE r_name={0:d} AND TO_DAYS(NOW())-TO_DAYS(r_born)>{1:d};", nameId, minAge), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int id = 0;
            if (rd.Read())
                id = rd.GetInt32(0);
            rd.Close();
            if (id == 0)
            {
                cmd.CommandText = String.Format(@"SELECT r_id,TO_DAYS(NOW())-TO_DAYS(r_born) age FROM dead 
WHERE r_name={0:d} AND TO_DAYS(NOW())-TO_DAYS(r_born)>{1:d} AND 
TO_DAYS(NOW())-TO_DAYS(r_born)<{1:d}+1000 ORDER BY age ASC;", nameId, minAge);
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
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT isdead({0:d});", rabbit), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            bool dead = false;
            if (rd.Read())
                dead = rd.GetBoolean(0);
            rd.Close();
            cmd.CommandText = String.Format(@"SELECT {0:s} 
FROM {1:s} WHERE r_id={2:d};", getOneRabbit_FieldsSet(dead ? RabAliveState.DEAD : RabAliveState.ALIVE), (dead ? "dead" : "rabbits"), rabbit);
            rd = cmd.ExecuteReader();
            if (!rd.Read())
            {
                rd.Close();
                return null;
            }
            OneRabbit r = fillOneRabbit(rd);
            //OneRabbit r = new OneRabbit(rabbit, rd.GetString("r_sex"), rd.GetDateTime("r_born"), //TODO ПИЗДЕЦ
            //    rd.IsDBNull(rd.GetOrdinal("weight")) ? 0 : rd.GetInt32("weight"),    "00000", 0, 0, 0, rd.GetString("place"), 1, rd.GetInt32("r_okrol"), dead ? 1 : 0, "", "", rd.GetInt32("status"), DateTime.MinValue, "",
            //    rd.IsDBNull(rd.GetOrdinal("weight_date")) ? DateTime.MinValue : rd.GetDateTime("weight_date"),
            //    rd.IsDBNull(rd.GetOrdinal("r_overall_babies")) ? 0 : rd.GetInt32("r_overall_babies"),
            //    rd.IsDBNull(rd.GetOrdinal("r_lost_babies")) ? 0 : rd.GetInt32("r_lost_babies"),    rd.GetString("name"), "", rd.GetString("r_bon"), 0, 
            //    rd.IsDBNull(rd.GetOrdinal("weight_age")) ? 0 : rd.GetInt32("weight_age")/*,rd.GetDateTime("vac_end")*/);
            rd.Close();
            return r;
        }

        public static OneRabbit[] getParents(MySqlConnection sql, int rabbit, int age)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT r_mother,r_father,r_surname,r_secname FROM rabbits WHERE r_id={0:d}", rabbit), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int mom = 0, pap = 0, mname = 0, pname = 0;
            if (rd.Read())
            {
                mom = rd.GetInt32("r_mother");
                pap = rd.GetInt32("r_father");
                mname = rd.GetInt32("r_surname");
                pname = rd.GetInt32("r_secname");
            }
            rd.Close();
            if (mom == 0)
                mom = getRabByName(sql, mname, age);
            if (pap == 0)
                pap = getRabByName(sql, pname, age);
            return new OneRabbit[] { getLiveDeadRabbit(sql, mom), getLiveDeadRabbit(sql, pap) };
        }

        internal static RabVac[] GetRabVacs(MySqlConnection sql, int rabId)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT   
    rv.v_id, 
    date,
    v_name,
    Greatest(CAST(v.v_duration as SIGNED)-CAST(to_days(NOW())-to_days(date) AS SIGNED),0),
    unabled FROM rab_vac rv 
INNER JOIN vaccines v ON rv.v_id=v.v_id
WHERE r_id={0:d} ORDER BY date", rabId), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<RabVac> result = new List<RabVac>();
            while (rd.Read())
            {
                result.Add(new RabVac(rd.GetInt32(0), rd.GetDateTime(1), rd.GetString(2), rd.GetInt32(3), rd.GetBoolean(4)));
            }
            rd.Close();
            return result.ToArray();
        }

        internal static void SetRabbitVaccine(MySqlConnection sql, int rid, int vid, DateTime date)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO rab_vac(r_id,v_id,`date`) VALUES({0:d},{1:d},{2:s});", rid, vid, DBHelper.DateToSqlString(date)), sql);
            cmd.ExecuteNonQuery();
        }

        internal static void SetRabbitVaccine(MySqlConnection sql, int rid, int vid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO rab_vac(r_id,v_id,`date`) VALUES({0:d},{1:d},NOW());", rid, vid), sql);
            cmd.ExecuteNonQuery();
        }

        internal static void RabVacUnable(MySqlConnection sql, int rid, int vid, bool unable)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE rab_vac SET unabled={2:d} WHERE r_id={0:d} AND v_id={1:d}", rid, vid,unable?1:0), sql);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Функция проверяет стоит ли в последней записи из таблице fucks по данному кролику начало траха.
        /// </summary>
        /// <param name="rabbit">ID кролика</param>
        private static void checkStartEvDate(MySqlConnection sql, int rabbit)
        {
            MySqlCommand cmd = new MySqlCommand("", sql);
            cmd.CommandText = String.Format("SELECT f_date,f_id FROM fucks WHERE f_state='sukrol' AND f_rabid={0:d} LIMIT 1;", rabbit);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                if (rd.IsDBNull(0))//если ли дата начала сукрольности
                {
                    int fid = rd.GetInt32("f_id");
                    rd.Close();
                    cmd.CommandText = String.Format("SELECT r_event_date FROM rabbits WHERE r_id={0:d};", rabbit);
                    rd = cmd.ExecuteReader();
                    if (rd.Read())
                    {
                        if (!rd.IsDBNull(0))
                        {
                            DateTime ev_date = rd.GetDateTime(0);
                            rd.Close();
                            cmd.CommandText = String.Format("UPDATE fucks SET f_date='{0:yyyy-MM-dd}',f_notes='fdate recovered' WHERE f_id={1:#};", ev_date, fid);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            if (!rd.IsClosed) rd.Close();
        }

        private static string getOneRabbit_FieldsSet(RabAliveState type)
        {
            return String.Format(@"r_id,
    r_sex, r_born, r_rate,
    r_flags, r_name, r_surname, r_secname,
    r_mother,r_father,
    {1:s}place(r_id) address, r_group, r_breed, 
    r_zone,
    r_notes,
    (SELECT COALESCE(GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' '),'') FROM genoms WHERE g_id=r_genesis) genom,
    r_status,
    r_last_fuck_okrol, {0:s},
    r_overall_babies,
    r_lost_babies,    
    {1:s}name(r_id,2) fullname,     
    (SELECT b_name FROM breeds WHERE b_id=r_breed) breedname,
    r_bon,    
    r_parent,
    r_okrol,
    r_birthplace,
    (SELECT w_weight FROM weights WHERE w_rabid=r_id AND w_date=(SELECT MAX(w_date) FROM weights WHERE w_rabid=r_id)) weight,
    (SELECT MAX(w_date) FROM weights WHERE w_rabid=r_id) weight_date", 
            (type == RabAliveState.ALIVE ? "r_event_date, r_event" : "NULL r_event_date, 'none' r_event"),
            (type == RabAliveState.ALIVE ? "rab" : "dead"), (type == RabAliveState.ALIVE ? "rabbits" : "dead"));
        }

        internal static string getAdultRabbit_FieldsSet(RabAliveState type)
        {
            return String.Format(@"r_id,
    {1:s}name(r_id,2) name,
    r_sex,
    r_born,
    (SELECT b_name FROM breeds WHERE b_id=r_breed) breed,
    r_group,
    r_rate,
    r_bon,
    {1:s}place(r_id) place,
    r_notes,
    r_flags,
    '-1' weight,
    r_status,
    {0:s},
    {2:s},
    {3:s},
    '' vaccines", 
            (type == RabAliveState.ALIVE ? "r_event_date" : "NULL r_event_date"),
            (type == RabAliveState.ALIVE ? "rab" : "dead"),
            (type == RabAliveState.ALIVE ? "(SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id) suckers" : "0 suckers"),
            (type == RabAliveState.ALIVE ? "(SELECT AVG(TO_DAYS(NOW())-TO_DAYS(r2.r_born)) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id) aage" : "0 aage"));
        }
    }
}
