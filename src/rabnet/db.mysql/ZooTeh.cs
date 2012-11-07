#define NEW_QUERY
using System;
using System.Text;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using log4net;
using rabnet;

namespace db.mysql
{   

    public class ZootehJob_MySql : ZootehJob
    {
        private Filters _flt = null;
        public ZootehJob_MySql(Filters f)
        {
            this._flt = f;
        }

        public ZootehJob_MySql(Filters f,JobType type,MySqlDataReader rd)
        {
            _flt = f;
            this.Type = type;
            this.JobName = getRusJobName(type);
            fillData(type,rd);
        }

        private string getRusJobName(JobType type)
        {
 	        switch(type)
            {
                case JobType.OKROL: return !_flt.safeBool(Filters.SHORT) ? "Принять окрол" : "Окрол";
                case JobType.VUDVOR: return !_flt.safeBool(Filters.SHORT) ? "Выдворение" : "Вдв";
                case JobType.COUNT_KIDS: return !_flt.safeBool(Filters.SHORT) ? "Подсчет гнездовых" : "счтГнезд";
                case JobType.PRE_OKROL: return !_flt.safeBool(Filters.SHORT) ? "Предокрольный осмотр" : "ПредОкрОс";
                case JobType.GIRLS_OUT: return !_flt.safeBool(Filters.SHORT) ? "Отсадка девочек" : "отсадДев";
                case JobType.BOYS_OUT: return !_flt.safeBool(Filters.SHORT) ? "Отсадка мальчиков" : "отсадМал";
                case JobType.FUCK: return "Вязка";//может смениться на Случку в процессе
                case JobType.VACC: return "Прививка";
                case JobType.SET_NEST: return !_flt.safeBool(Filters.SHORT) ? "Установка гнездовья" : "уст.Гнезд";
                case JobType.BOYS_BY_ONE: return !_flt.safeBool(Filters.SHORT) ? "Рассадка мальчиков по одному" : "Рсд.М.по1";
                default: return "[UJ]";
            }
        }

        private void fillData(JobType type, MySqlDataReader rd)
        {
            fillCommonData(rd);
            switch (type)
            {
                case JobType.OKROL: fillOkrol(rd); break;
                case JobType.VUDVOR: fillVudvor(rd); break;
                case JobType.COUNT_KIDS: fillCounts(rd); break;
                case JobType.PRE_OKROL: break;                
                case JobType.GIRLS_OUT: //fall through
                case JobType.BOYS_OUT: fillBoysGirlsOut(rd); break;               
                case JobType.FUCK: fillFuck(rd); break;
                case JobType.VACC: fillVacc(rd); break;               
                case JobType.SET_NEST: fillSetNest(rd); break;                
                case JobType.BOYS_BY_ONE: fillBoysByOne(rd); break;               
            }
        }

        private void fillCommonData(MySqlDataReader rd)
        {
 	        Days = rd.GetInt32("srok");
            ID=rd.GetInt32("r_id");
            RabName = rd.GetString("name");
            Address = Building.FullPlaceName(rd.GetString("place"));
            RabAge = rd.GetInt32("age");
            RabBreed = rd.GetString("breed");
        }

        private void fillOkrol(MySqlDataReader rd)
        {
            Days -= _flt.safeInt(Filters.OKROL);
            Comment = (_flt.safeInt("shr") == 0 ? "окрол " : "№") + (rd.GetInt32("r_status")+1).ToString();
        }

        private void fillVudvor(MySqlDataReader rd)
        {
            Days = this.Days - _flt.safeInt("vudvor");
            ID = rd.GetInt32("t_id");
            ID2 = rd.GetInt32("r_area");
            if (ID2 == 1 && rd.GetString("t_type") == BuildingType.Jurta)
                ID2 = 0;
            Comment = String.Format("№{0:s} {1:s}{2:s}", rd.GetString("r_status"), _flt.safeInt(Filters.SHORT) == 0 ? "подсосных" : "+", rd.GetString("suckers"));
        }

        private void fillBoysByOne(MySqlDataReader rd)
        {
            Comment = "кол-во:" + rd.GetString("r_group");           
        }

        private void fillBoysGirlsOut(MySqlDataReader rd)
        {
            int sub = _flt.safeInt("type") == (int)Rabbit.SexType.FEMALE ? _flt.safeInt(Filters.GIRLS_OUT) : _flt.safeInt(Filters.BOYS_OUT);
            Days = rd.GetInt32("age") - sub ;
        }

        private void fillCounts(MySqlDataReader rd)
        {           
            ID = rd.GetInt32("r_parent");
            ID2 = rd.GetInt32("r_id");
            Comment = (_flt.safeInt(Filters.SHORT) == 0 ? "количество " : "+") + rd.GetString("r_group");
        }

        private void fillFuck(MySqlDataReader rd)
        {
            int status = rd.GetInt32("r_status");
            int fromok = rd.IsDBNull(rd.GetOrdinal("fromokrol")) ? 0 : rd.GetInt32("fromokrol");
            int suck = rd.IsDBNull(rd.GetOrdinal("suckers")) ? 0 : rd.GetInt32("suckers");
            int srok = 0;
            int group = rd.GetInt32("r_group");
            if (status == 0)
                srok = this.RabAge - _flt.safeInt("brideAge");
            else if (status > 0)
            {
                if (suck > 0)
                    srok = fromok - (status == 1 ? _flt.safeInt("ffuck") : _flt.safeInt("sfuck"));
                else srok = fromok;
            }

            Days = srok; //если в common не определится срок и произойдет ошибка
            JobName = status == 0 ? "Случка" : "Вязка";
            Comment = _flt.safeInt(Filters.SHORT) == 1 ? "Нвс" : "Невеста";
            if(group>1)
                Comment += String.Format(" [{0:d}]", group);
            if (status > 0)
                Comment = _flt.safeInt(Filters.SHORT) == 1 ? "Прк" : "Первокролка";
            if (status > 1)
                Comment = _flt.safeInt(Filters.SHORT) == 1 ? "Штн" : "Штатная";
            if (!rd.IsDBNull(rd.GetOrdinal("lsrok")))
            {
                Comment += "  {Стим." + rd.GetString("lsrok") + "дн.}"; //че-то String.Format ругается  
                Flag2 = -1;
            }
            Partners = rd.IsDBNull(rd.GetOrdinal("partners")) ? "" : zooFuckPartnerAddressParce(rd.GetString("partners"));
            Flag = group;

        }

        private void fillSetNest(MySqlDataReader rd)
        {
            int children = rd.IsDBNull(rd.GetOrdinal("children")) ? 0 : rd.GetInt32("children");
            int sukr = rd.GetInt32("sukr");
            Comment = "C-" + sukr.ToString();
            if (children > 0)
            {
                Days = sukr - _flt.safeInt("cnest");
                Comment += " " + (_flt.safeInt(Filters.SHORT) == 0 ? " подсосных" : "+") + children.ToString();
            }
            else
                Days=sukr - _flt.safeInt("nest");
            
        }
        private void fillVacc(MySqlDataReader rd)
        {
            ID2 = rd.GetInt32("v_id");
            Comment = "\"" + rd.GetString("v_name")+"\"  [" + rd.GetString("r_group") + "]";           
        }               

        /// <summary>
        /// По просьбе Татищево, надо указать у партнеров адрес.
        /// Функция преобразует адрес вида  &10606,0,0,jurta,0,1 в [ 10606б]
        /// </summary>
        /// <param name="partners">Строка партнеров</param>
        /// <returns></returns>
        private string zooFuckPartnerAddressParce(string partners)
        {
            if (partners == "") return "";
            string result = "";
            string[] fuckers = partners.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in fuckers)
            {
                string[] set = s.Split(new char[] { '&' });
                result += String.Format(" {0:s} [{1:s}],", set[0].Trim(), Building.FullPlaceName(set[1]));
            }
            if (result[result.Length - 1] == ',')
                result = result.Remove(result.Length - 1);
            return result;
        }
    }
 
    class ZooTehGetter
    {
        private MySqlConnection sql;
        protected static ILog _logger = log4net.LogManager.GetLogger(typeof(RabNetDataGetterBase));
        private Filters _flt;
        public ZooTehGetter(MySqlConnection sql,Filters f)
        {
            this.sql = sql;
            _flt = f;
        }

        #region helpers
        public MySqlDataReader reader(String qry)
        {
            MySqlCommand cmd=new MySqlCommand(qry,sql);
            return cmd.ExecuteReader();
        }

        public String getnm(int def)
        {
            return _flt.safeInt("dbl") == 1 ? "2" : def.ToString();
        }
        public String getnm()
        {
            return getnm(0);
        }

        public String brd(String fld)
        {
            return String.Format("(SELECT {0:s} FROM breeds WHERE b_id={1:s}) breed", _flt.safeInt("shr") == 1 ? "b_short_name" : "b_name",fld);
        }
        public String brd()
        {
            return brd("r_breed");
        }
        #endregion helpers

        public ZootehJob_MySql[] GetZooTechJobs(JobType type)
        {
            List<ZootehJob_MySql> res = new List<ZootehJob_MySql>();
            string query = getQuery(type);
            if (query == "")
            {
                _logger.Warn("unknown JobType");
                return res.ToArray();
            }            
            MySqlDataReader rd = null;
#if !DEBUG
            try
            {
#else
                _logger.Debug(type.ToString() + " query: " + query);
#endif
                MySqlCommand cmd = new MySqlCommand(query,sql);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                    res.Add(new ZootehJob_MySql(_flt, type, rd));
                rd.Close();
            #if !DEBUG
            }
            catch(Exception err)
            {
                _logger.Error(err.Message);
            }
            finally 
            { 
#endif
                if(rd!=null && !rd.IsClosed)
                    rd.Close(); 
#if !DEBUG
            }
#endif
            return res.ToArray();
        }

        private string getQuery(JobType type)
        {
            switch (type)
            {
                case JobType.OKROL: return qOkrol();
                case JobType.VUDVOR: return qVudvod();
                case JobType.COUNT_KIDS: return qCounts();
                case JobType.PRE_OKROL: return qPreOkrol();
                case JobType.GIRLS_OUT:
                    _flt["sex"] = ((int)Rabbit.SexType.FEMALE).ToString();
                    return qBoysGirlsOut();
                case JobType.BOYS_OUT:
                    _flt["sex"] = ((int)Rabbit.SexType.MALE).ToString();
                    return qBoysGirlsOut();
                case JobType.FUCK: return qFuck();
                case JobType.VACC: return qVacc();
                case JobType.SET_NEST: return qSetNest();
                case JobType.BOYS_BY_ONE: return qBoysByOne();
                default: return "";
            }
        }

        private string qOkrol()
        {
            return String.Format(@"SELECT r_id,rabname(r_id," + getnm() + @") name,rabplace(r_id) place,
(TO_DAYS(NOW())-TO_DAYS(r_event_date)) srok,
r_status,(TO_DAYS(NOW())-TO_DAYS(r_born)) age,{1:s}
FROM rabbits WHERE r_sex='female' AND (TO_DAYS(NOW())-TO_DAYS(r_event_date))>={0:d} ORDER BY srok DESC,
0+LEFT(place,LOCATE(',',place)) ASC;", _flt.safeInt(Filters.OKROL), brd());
        }

        private string qVudvod()
        {
            return String.Format(@"SELECT r_id, rabname(r_id,{0:s}) name, rabplace(r_id) place,(TO_DAYS(NOW())-TO_DAYS(r_born)) age, {1:s},
    r_tier,
    r_area,
    r_event_date,
    (TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol)) srok,
    r_status,   
    t_nest,
    t_id,
    t_busy1,
    t_busy2,
    t_delims,
    t_type,
    COALESCE( (SELECT SUM(r3.r_group) FROM rabbits r3 WHERE r3.r_parent=rabbits.r_id) ,0) suckers
FROM 
    rabbits,tiers
WHERE 
    t_id=r_tier AND 
    (r_event_date IS NULL {3:s}) AND
    r_sex='female' AND 
    (TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol))>={2:d} AND
    ((t_busy1=r_id AND t_nest like '1%') OR (t_busy2=r_id AND t_nest like '%1' AND t_type='dfemale'))
ORDER BY srok DESC,0+LEFT(place,LOCATE(',',place)) ASC;", getnm(),brd(),
                _flt.safeInt(Filters.VUDVOR),
                _flt.safeBool(Filters.NEST_OUT_IF_SUKROL) ? String.Format("OR (r_event_date IS NOT NULL AND (to_days(NOW())-to_days(r_event_date))<{0:d})", _flt.safeInt("nest")) : "");
        }

        private string qCounts()
        {
            return String.Format(@"SELECT 
    r_parent,
    rabname(r_parent,{2:s}) name,
    rabplace(r_parent) place,
    r_group,
    (SELECT TO_DAYS(NOW())-TO_DAYS(r3.r_born) FROM rabbits r3 WHERE r3.r_id=rabbits.r_parent) age,"
    + brd("(SELECT r7.r_breed FROM rabbits r7 WHERE r7.r_id=rabbits.r_parent)") + @",
    TO_DAYS(NOW())-TO_DAYS(r_born)-{0:d} srok,
    r_id
FROM rabbits 
WHERE r_parent<>0 AND (TO_DAYS(NOW())-TO_DAYS(r_born)>={0:d}{1:s}) 
    AND r_parent NOT IN (
                            SELECT l_rabbit FROM logs 
                            WHERE l_type=17 AND (DATE(l_date)<=DATE(NOW()) 
                                AND DATE(l_date)>=DATE( NOW()- INTERVAL (TO_DAYS(NOW())-TO_DAYS(r_born)-{0:d}) DAY) )
                        )
ORDER BY age DESC, 0+LEFT(place,LOCATE(',',place)) ASC;", _flt.safeInt("days"), (_flt.safeInt("next") == -1 ? "" : String.Format(" AND TO_DAYS(NOW())-TO_DAYS(r_born)<{0:d}", _flt.safeInt("next"))), getnm());
        }

        private string qPreOkrol()
        {
            return String.Format(@"SELECT r_id,rabname(r_id," + getnm() + @") name,rabplace(r_id) place,
(TO_DAYS(NOW())-TO_DAYS(r_event_date)) srok,r_status,(TO_DAYS(NOW())-TO_DAYS(r_born)) age," + brd() + @" 
FROM rabbits WHERE r_sex='female' AND (TO_DAYS(NOW())-TO_DAYS(r_event_date))>={0:d} AND (TO_DAYS(NOW())-TO_DAYS(r_event_date))<{1:d} AND
r_id NOT IN (SELECT l_rabbit FROM logs 
WHERE l_type=21 AND DATE(l_date)>=DATE(rabbits.r_event_date)) ORDER BY srok DESC
,0+LEFT(place,LOCATE(',',place)) ASC;", _flt.safeInt("preok"), _flt.safeInt("okrol"));
        }

        private string qBoysGirlsOut()
        {
            return String.Format(@"SELECT if(r_parent!=0,r_parent,r_id) r_id, 0 srok,
    rabname(if(r_parent!=0,r_parent,r_id),{3:s}) name, 
    rabplace(if(r_parent!=0,r_parent,r_id)) place, 
    TO_DAYS(NOW())-TO_DAYS(r_born) age,{2:s} 
FROM rabbits WHERE {0:s} AND (TO_DAYS(NOW())-TO_DAYS(r_born))>={1:d} 
ORDER BY age DESC,0+LEFT(place,LOCATE(',',place)) ASC;",
                (_flt.safeInt("sex") == (int)Rabbit.SexType.FEMALE ? "(r_sex='female' and r_parent<>0)" : "(r_sex='void' OR (r_sex='male' and r_parent<>0))"),
                (_flt.safeInt("sex") == (int)Rabbit.SexType.FEMALE ? _flt.safeInt(Filters.GIRLS_OUT) : _flt.safeInt(Filters.BOYS_OUT)), brd(), getnm());
        }

        private string qFuck()
        {
            return String.Format(
            (_flt.safeBool(Filters.FIND_PARTNERS) ? @"SET group_concat_max_len=4096; 
CREATE TEMPORARY TABLE tPartn SELECT rabname(r_id,0) pname, 
	rabplace(r_id) pplace,
	r_breed pbreed,
	r_genesis pgens
FROM rabbits
WHERE r_sex='male' AND r_status>0 AND (r_last_fuck_okrol IS NULL OR TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol)>={3:d});":"")+
  
@"CREATE TEMPORARY TABLE aaa 
    SELECT r.r_id,rabname(r_id,{8:s}) name,rabplace(r_id) place,TO_DAYS(NOW())-TO_DAYS(r_born) age,
        coalesce((SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent=r.r_id),null,0) suckers,
        r_status,
        TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol) fromokrol," + (_flt.safeBool(Filters.FIND_PARTNERS) ? @"
        (SELECT GROUP_CONCAT( CONCAT(pname,'&', pplace) ORDER BY pname SEPARATOR '|') FROM tPartn
            WHERE {4:s}{5:s})" : "''") + @" partners,
        r_group,
        (SELECT {6:s} FROM breeds WHERE b_id=r_breed) breed, 
        0 srok,
        lsrok,
        lshow
    FROM rabbits r
    LEFT JOIN   #пытаемся найти стимуляцию
        (SELECT rv.r_id AS lrid, To_Days(NOW())-To_Days(Max(date)) AS lsrok, v_duration AS ldura, v_age AS lshow 
            FROM rab_vac rv
            INNER JOIN vaccines v ON v.v_id= rv.v_id
            WHERE rv.v_id=-1 #стимуляция (lust)
            GROUP BY rv.r_id
        ) vc ON lrid=r.r_id AND lsrok<=ldura
    WHERE Substr(r_flags,1,1)='0'       #не брак
        AND Substr(r_flags,3,1)='0'     #не готовая продукция
        AND r_sex='female' AND r_event_date IS NULL AND r_status{7:s};
		
SELECT * FROM aaa 
WHERE (lsrok IS NULL OR lsrok=lshow)
    AND (age>{0:d} AND r_status=0                           #невеста
        OR (r_status=1 AND (suckers=0 OR fromokrol>={1:d})) #первокролка
        OR (r_status>1 AND (suckers=0 OR fromokrol>={2:d})) #штатная
    )
ORDER BY 0+LEFT(place,LOCATE(',',place)) ASC;

DROP TABLE IF EXISTS tPartn; 
DROP TABLE IF EXISTS aaa;", 
        _flt.safeInt(Filters.BRIDE_AGE), _flt.safeInt(Filters.FIRST_FUCK), _flt.safeInt(Filters.STATE_FUCK), 
    
        _flt.safeInt(Filters.MALE_WAIT),//3
        (_flt.safeBool(Filters.HETEROSIS) ? "" : String.Format("pbreed=r.r_breed")),//4
        (_flt.safeBool(Filters.INBREEDING) ? "" : String.Format("{0:s}(SELECT COUNT(g_genom) FROM genoms WHERE g_id=r.r_genesis AND g_genom IN (SELECT g2.g_genom FROM genoms g2 WHERE g2.g_id=pgens))=0", _flt.safeBool(Filters.HETEROSIS) ?"": " AND ")),
    
        (_flt.safeInt(Filters.SHORT) == 0 ? "b_name" : "b_short_name"),//6 
        (_flt.safeInt(Filters.TYPE) == 1 ? ">0" : "=0"), getnm(1) );

//            return String.Format(@"SET group_concat_max_len=4096;   SELECT * FROM (
//        SELECT r_id,rabname(r_id," + getnm(1) + @") name,rabplace(r_id) place,TO_DAYS(NOW())-TO_DAYS(r_born) age,
//        coalesce((SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id),null,0) suckers,
//        r_status,
//        TO_DAYS(NOW())-TO_DAYS(r_last_fuck_okrol) fromokrol," +
//            (_flt.safeBool(Filters.FIND_PARTNERS) ? @"
//        (SELECT GROUP_CONCAT( CONCAT(rabname(r_id,0),'&', rabplace(r_id)) ORDER BY rabname(r5.r_id,0) SEPARATOR '|') FROM rabbits r5
//            WHERE r5.r_sex='male' AND r_status>0 AND (r5.r_last_fuck_okrol IS NULL OR TO_DAYS(NOW())-TO_DAYS(r5.r_last_fuck_okrol)>={3:d}){4:s}{5:s}) partners" : "'' partners") + @"
//        ,r_group,
//        (SELECT {6:s} FROM breeds WHERE b_id=r_breed) breed, 0 srok 
//        FROM rabbits WHERE Substr(r_flags,1,1)='0' AND Substr(r_flags,3,1)='0' AND r_sex='female' AND r_event_date IS NULL AND r_status{7:s}) c 
//WHERE age>{0:d} AND r_status=0 OR (r_status=1 AND (suckers=0 OR fromokrol>={1:d})) OR (r_status>1 AND (suckers=0 OR fromokrol>={2:d}))
//ORDER BY 0+LEFT(place,LOCATE(',',place)) ASC;",
//    _flt.safeInt(Filters.BRIDE_AGE), _flt.safeInt(Filters.FIRST_FUCK), _flt.safeInt(Filters.STATE_FUCK), 
    
//    _flt.safeInt(Filters.MALE_WAIT),//3
//    (_flt.safeBool(Filters.HETEROSIS) ? "" : String.Format(" AND r5.r_breed=rabbits.r_breed")),
//    (_flt.safeBool(Filters.INBREEDING) ? "" : String.Format(@" AND (SELECT COUNT(g_genom) FROM genoms WHERE g_id=rabbits.r_genesis AND g_genom IN (SELECT g2.g_genom FROM genoms g2 WHERE g2.g_id=r5.r_genesis))=0")),
    
//    (_flt.safeInt(Filters.SHORT) == 0 ? "b_name" : "b_short_name"),//6 
//    (_flt.safeInt(Filters.TYPE) == 1 ? ">0" : "=0"));//7

        }

        private string qVacc()
        {
            string show = _flt.safeValue(Filters.VACC_SHOW, "1") != "" ? _flt.safeValue(Filters.VACC_SHOW, "1") : "1";
            return String.Format(@"CREATE TEMPORARY TABLE aaa  SELECT 
    rb.r_id, r_parent,rabname(r_id,{0:s}) name, rabplace(r_id) place, (TO_DAYS(NOW())-TO_DAYS(r_born)) age, v.v_id, r_group,
    to_days(NOW()) - to_days(
        COALESCE(
            (SELECT MAX(`date`) FROM rab_vac WHERE r_id=rb.r_id AND v_id=v.v_id),
            If(
                v_do_after=0, 
                Date_Add(r_born,INTERVAL v_age DAY), 
                (SELECT Date_Add(`date`,INTERVAL v_age DAY) FROM rab_vac WHERE r_id=rb.r_id AND v_id=v_do_after)    #может получиться NULL если не было сделано предыдущей прививки
            )
        )
    ) srok,     #сколько дней не выполнена работа

    ( SELECT `date` FROM rab_vac rv         #находим дату прививки
      WHERE rv.v_id=v.v_id AND rv.r_id=rb.r_id AND unabled!=1       #если ее сделали кролику
        AND CAST(v.v_duration as SIGNED)-CAST(to_days(NOW())-to_days(date) AS SIGNED)>0     #и она еще не кончилась
    ) dt, 
    (SELECT COUNT(*) FROM rab_vac rv WHERE rv.v_id=v.v_id AND rv.r_id=rb.r_id) times,
    v_age, 
    v_do_times,
    v_name, {1:s}
  FROM rabbits rb,vaccines v WHERE v_id in({2:s}) AND v_id>0;
{3:s}
SELECT * FROM aaa WHERE age>=v_age AND dt is NULL AND srok IS NOT NULL AND srok>=0 AND (v_do_times=0 OR (times<v_do_times)) {4:s} ORDER BY srok;
DROP TEMPORARY TABLE IF EXISTS aaa; {5:s}", getnm(), brd(), show,
    _flt.safeBool(Filters.VACC_MOTH, true) ? "CREATE TEMPORARY TABLE bbb SELECT DISTINCT r_parent FROM aaa WHERE r_parent !=0;" : "",
    _flt.safeBool(Filters.VACC_MOTH, true) ? "AND r_id not in (select r_parent FROM bbb)" : "",
    _flt.safeBool(Filters.VACC_MOTH, true) ? "DROP TEMPORARY TABLE IF EXISTS bbb;" : "");
        }

        private string qSetNest()
        {
            return String.Format(@"SELECT * FROM (
        SELECT r_id,rabname(r_id," + getnm() + @") name,rabplace(r_id) place,
        (TO_DAYS(NOW())-TO_DAYS(r_born)) age,
        (TO_DAYS(NOW())-TO_DAYS(r_event_date)) sukr,
        (SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent=rabbits.r_id) children," + brd() + @",
        0 srok 
    FROM rabbits WHERE r_sex='female' AND r_event_date IS NOT NULL) c 
WHERE ((children IS NULL AND sukr>={0:d}) OR (children>0 AND sukr>={1:d})) AND
    (place NOT like '%,%,0,jurta,%,1%' AND place NOT like '%,%,0,female,%,1%' AND place NOT like '%,%,0,dfemale,%,1%' AND place NOT like '%,%,1,dfemale,%,_1')
ORDER BY sukr DESC,0+LEFT(place,LOCATE(',',place)) ASC;", _flt.safeInt("nest"), _flt.safeInt("cnest"));
        }

        private string qBoysByOne()
        {
            return String.Format(@"SELECT 
r_id, rabname(r_id,{0:s}) name, rabplace(r_id) place, r_group,
(TO_DAYS(NOW())-TO_DAYS(r_born)-{1:d}) srok,
(TO_DAYS(NOW())-TO_DAYS(r_born)) age,{2:s}
FROM rabbits WHERE (TO_DAYS(NOW())-TO_DAYS(r_born))>={1:d} and r_group>1 and r_sex='male' ;", getnm(), _flt.safeInt("bbone"), brd());
        }      
    }
}
