#define NEW_QUERY
using System;
using System.Text;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using log4net;
using rabnet;
using System.Diagnostics;

namespace db.mysql
{

    public class ZootehJob_MySql : ZootehJob
    {
        private Filters _flt = null;

        public ZootehJob_MySql(Filters f)
        {
            this._flt = f;
        }

        public ZootehJob_MySql(Filters f, JobType type, MySqlDataReader rd)
        {
            _flt = f;
            this.Type = type;
            this.JobName = getRusJobName(type);
            this.fillData(type, rd);
        }

        private string getRusJobName(JobType type)
        {
            switch (type) {
                case JobType.Okrol: return !_flt.safeBool(Filters.SHORT) ? "Принять окрол" : "Окрол";
                case JobType.NestOut: return !_flt.safeBool(Filters.SHORT) ? "Выдворение" : "Вдв";
                case JobType.CountKids: return !_flt.safeBool(Filters.SHORT) ? "Подсчет гнездовых" : "счтГнезд";
                case JobType.PreOkrol: return !_flt.safeBool(Filters.SHORT) ? "Предокрольный осмотр" : "ПредОкрОс";
                case JobType.GirlsOut: return !_flt.safeBool(Filters.SHORT) ? "Отсадка девочек" : "отсадДев";
                case JobType.BoysOut: return !_flt.safeBool(Filters.SHORT) ? "Отсадка мальчиков" : "отсадМал";
                case JobType.Fuck: return "Вязка";//может смениться на Случку в процессе
                case JobType.Vaccine: return "Прививка";
                case JobType.NestSet: return !_flt.safeBool(Filters.SHORT) ? "Установка гнездовья" : "уст.Гнезд";
                case JobType.BoysByOne: return !_flt.safeBool(Filters.SHORT) ? "Рассадка мальчиков по одному" : "Рсд.М.по1";
                case JobType.SpermTake: return !_flt.safeBool(Filters.SHORT) ? "Забор спермы" : "самДойк";
                default: return "[UJ]";
            }
        }

        private void fillData(JobType type, MySqlDataReader rd)
        {
            this.fillCommonData(rd);
            switch (type) {
                case JobType.Okrol: fillOkrol(rd); break;
                case JobType.NestOut: fillVudvor(rd); break;
                case JobType.CountKids: fillCounts(rd); break;
                case JobType.PreOkrol: break;
                case JobType.GirlsOut: //fall through
                case JobType.BoysOut: fillBoysGirlsOut(rd); break;
                case JobType.Fuck: fillFuck(rd); break;
                case JobType.Vaccine: fillVacc(rd); break;
                case JobType.NestSet: fillSetNest(rd); break;
                case JobType.BoysByOne: fillBoysByOne(rd); break;
                case JobType.SpermTake: fillSpermTake(rd); break;
            }
        }

        private void fillCommonData(MySqlDataReader rd)
        {
            Days = rd.GetInt32("srok");
            ID = rd.GetInt32("r_id");
            RabName = rd.GetString("name");
            Rabplace = rd.GetString("place");
            Address = Building.FullPlaceName(Rabplace);
            RabAge = rd.GetInt32("age");
            RabBreed = rd.GetString("breed");
        }

        private void fillOkrol(MySqlDataReader rd)
        {
            Days -= _flt.safeInt(Filters.OKROL);
            Comment = (_flt.safeInt("shr") == 0 ? "окрол " : "№") + (rd.GetInt32("r_status") + 1).ToString();
        }

        private void fillVudvor(MySqlDataReader rd)
        {
            Days = this.Days - _flt.safeInt("vudvor");
            ID = rd.GetInt32("t_id");
            ID2 = rd.GetInt32("r_area");
            if (ID2 == 1 && Building.ParseType(rd.GetString("t_type")) == BuildingType.Jurta) {
                ID2 = 0;
            }
            Comment = String.Format("№{0:s} {1:s}{2:s}", rd.GetString("r_status"), _flt.safeInt(Filters.SHORT) == 0 ? "подсосных:" : "+", rd.GetString("suckers"));
        }

        private void fillBoysByOne(MySqlDataReader rd)
        {
            Comment = "кол-во:" + rd.GetString("r_group");
        }

        private void fillBoysGirlsOut(MySqlDataReader rd)
        {
            int sub = _flt.safeInt("type") == (int)Rabbit.SexType.FEMALE ? _flt.safeInt(Filters.GIRLS_OUT) : _flt.safeInt(Filters.BOYS_OUT);
            Days = rd.GetInt32("age") - sub;
        }

        private void fillCounts(MySqlDataReader rd)
        {
            int tmp = DBHelper.GetNullableInt(rd, "suckGroups");
            this.ID2 = tmp;
            Comment = String.Format("{0:s}{1:2,d}{2:s}",
                (_flt.safeInt(Filters.SHORT) == 0 ? "количество: " : "+"),
                rd.GetString("suckers"),
                tmp > 1 ? String.Format(" ({0:d})", tmp) : "");
        }

        private void fillFuck(MySqlDataReader rd)
        {
            int status = rd.GetInt32("r_status");
            int fromok = DBHelper.GetNullableInt(rd, "fromokrol");
            int suck = DBHelper.GetNullableInt(rd, "suckers");
            int srok = 0;
            int group = rd.GetInt32("r_group");

            if (status == 0) {
                srok = this.RabAge - _flt.safeInt(Filters.MAKE_BRIDE);
            } else if (status > 0) {
                if (suck > 0) {
                    srok = fromok - (status == 1 ? _flt.safeInt(Filters.FIRST_FUCK) : _flt.safeInt(Filters.STATE_FUCK));
                } else {
                    srok = fromok;
                }
            }

            Days = srok; //если в common не определится срок и произойдет ошибка
            JobName = status == 0 ? "Случка" : "Вязка";
            Comment = _flt.safeInt(Filters.SHORT) == 1 ? "Нвс" : "Невеста";
            if (group > 1) {
                Comment += String.Format(" [{0:d}]", group);
            }
            if (status > 0) {
                Comment = _flt.safeInt(Filters.SHORT) == 1 ? "Прк" : "Первокролка";
            }
            if (status > 1) {
                Comment = _flt.safeInt(Filters.SHORT) == 1 ? "Штн" : "Штатная";
            }
            if (!rd.IsDBNull(rd.GetOrdinal("lsrok"))) {
                Comment += "  {Стим." + rd.GetString("lsrok") + "дн.}"; //че-то String.Format ругается  
                Flag2 = -1;
            }
            Partners = rd.IsDBNull(rd.GetOrdinal("partners")) ? "" : zooFuckPartnerAddressParce(rd.GetString("partners"));
            Flag = group;

        }

        private void fillSetNest(MySqlDataReader rd)
        {
            int children = DBHelper.GetNullableInt(rd, "children");
            int sukr = rd.GetInt32("sukr");
            Comment = "C-" + sukr.ToString();
            if (children > 0) {
                Days = sukr - _flt.safeInt("cnest");
                Comment += " " + (_flt.safeInt(Filters.SHORT) == 0 ? " подсосных" : "+") + children.ToString();
            } else {
                Days = sukr - _flt.safeInt("nest");
            }

        }
        private void fillVacc(MySqlDataReader rd)
        {
            ID2 = rd.GetInt32("v_id");
            Comment = "\"" + rd.GetString("v_name") + "\"  [" + rd.GetString("r_group") + "]";
        }

        private void fillSpermTake(MySqlDataReader rd)
        {
            if (rd.IsDBNull(rd.GetOrdinal("fromfuck"))) {
                Days = rd.GetInt32("age") - _flt.safeInt(Filters.MAKE_CANDIDATE);
            } else {
                Days = rd.GetInt32("fromfuck");
            }
        }

        /// <summary>
        /// По просьбе Татищево, надо указать у партнеров адрес.
        /// Функция преобразует адрес вида  &10606,0,0,jurta,0,1 в [ 10606б]
        /// </summary>
        /// <param name="partners">Строка партнеров</param>
        /// <returns></returns>
        private string zooFuckPartnerAddressParce(string partners)
        {
            if (partners == "") {
                return "";
            }

            string result = "";
            string[] fuckers = partners.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in fuckers) {
                string[] set = s.Split(new char[] { '&' });
                result += String.Format(" {0:s} [{1:s}],", set[0].Trim(), Building.FullPlaceName(set[1]));
            }
            if (result[result.Length - 1] == ',') {
                result = result.Remove(result.Length - 1);
            }
            return result;
        }
    }

    class ZooTehGetter
    {
        private MySqlConnection sql;
        protected static ILog _logger = log4net.LogManager.GetLogger(typeof(RabNetDataGetterBase));
        private Filters _flt;

        public ZooTehGetter(MySqlConnection sql)
        {
            this.sql = sql;
        }

        #region helpers
        public MySqlDataReader reader(String qry)
        {
            MySqlCommand cmd = new MySqlCommand(qry, sql);
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
            return String.Format("(SELECT {0:s} FROM breeds WHERE b_id={1:s}) breed", _flt.safeInt("shr") == 1 ? "b_short_name" : "b_name", fld);
        }
        public String brd()
        {
            return brd("r_breed");
        }
        public String rabplace()
        {
            return "IF(r_farm IS NULL, '', CONCAT_WS(',', r_farm, r_tier_id, r_area, t_type, t_delims, t_nest))";
        }
        #endregion helpers

        public ZootehJob_MySql[] GetZooTechJobs(Filters f, JobType type)
        {
            _flt = f;

            List<ZootehJob_MySql> res = new List<ZootehJob_MySql>();
            string query = this.getQuery(type);
            if (query == "") {
                _logger.Warn("unknown JobType");
                return res.ToArray();
            }
            MySqlDataReader rd = null;
#if !DEBUG
            try {
#else
            _logger.Debug(type.ToString() + " query: " + query);
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
                MySqlCommand cmd = new MySqlCommand(query, sql);
                rd = cmd.ExecuteReader();
                while (rd.Read()) {
                    res.Add(new ZootehJob_MySql(_flt, type, rd));
                }
                rd.Close();
#if DEBUG
            sw.Stop();
            _logger.DebugFormat("execution time: {0} ZOO_{1}", sw.Elapsed, type);
#else
            } catch (Exception err) {
                _logger.Error(err.Message);
            } finally {
#endif
                if (rd != null && !rd.IsClosed) {
                    rd.Close();
                }
#if !DEBUG
            }
#endif
            return res.ToArray();
        }

        private string getQuery(JobType type)
        {
            switch (type) {
                case JobType.Okrol: return qOkrol();
                case JobType.NestOut: return qVudvod();
                case JobType.CountKids: return qCounts();
                case JobType.PreOkrol: return qPreOkrol();
                case JobType.GirlsOut:
                    _flt["sex"] = ((int)Rabbit.SexType.FEMALE).ToString();
                    return qBoysGirlsOut();
                case JobType.BoysOut:
                    _flt["sex"] = ((int)Rabbit.SexType.MALE).ToString();
                    return qBoysGirlsOut();
                case JobType.Fuck: return qFuck();
                case JobType.Vaccine: return qVacc();
                case JobType.NestSet: return qSetNest();
                case JobType.BoysByOne: return qBoysByOne();
                case JobType.SpermTake: return qSpermTake();
                default: return "";
            }
        }


        private string qOkrol()
        {
            return String.Format(@"SELECT 
    r_id, 
    rabname(r_id, {3}) name, 
#rabplace(r_id) place,
    {4} AS place,
    DATEDIFF('{2}', r_event_date) srok,
    DATEDIFF('{2}', r_born) age, 
    r_status,     
    {1:s}
FROM rabbits 
    LEFT JOIN tiers ON r_tier = t_id
WHERE r_sex = 'female'
HAVING srok >= {0:d};",
                _flt.safeInt(Filters.OKROL),
                brd(),
                _flt.safeValue(Filters.DATE),
                getnm(),
                rabplace()
            );
        }

        private string qVudvod()
        {
            return String.Format(@"SELECT 
    r_id, 
    rabname(r_id,{0:s}) name, 
#rabplace(r_id) place,
    {5} AS place,
    DATEDIFF('{4}', r_last_fuck_okrol) srok,
    DATEDIFF('{4}', r_born) age, 
    {1:s},
    r_tier,
    r_area,
    r_event_date,    
    r_status,   
    t_nest,
    t_id,
    t_busy1,
    t_busy2,
    t_delims,
    t_type,
    COALESCE( (SELECT SUM(r3.r_group) FROM rabbits r3 WHERE r3.r_parent=rabbits.r_id) ,0) suckers
FROM rabbits
    INNER JOIN tiers ON t_id = r_tier AND ((t_busy1 = r_id AND t_nest like '1%') OR (t_busy2 = r_id AND t_nest like '%1' AND t_type='dfemale'))
WHERE r_sex = 'female'         
HAVING srok >= {2:d} AND (r_event_date IS NULL {3:s});",
                getnm(),
                brd(),
                _flt.safeInt(Filters.VUDVOR),
                _flt.safeBool(Filters.NEST_OUT_IF_SUKROL) ? String.Format("OR (r_event_date IS NOT NULL AND srok < {0:d})", _flt.safeInt("nest")) : "",
                _flt[Filters.DATE],
                rabplace()
            );
        }

        private string qCounts()
        {
            const int COUNT_KIDS_LOG = 17;

            return String.Format(@"SELECT 
    DATEDIFF('{5}', r_born) srok_base,
    DATEDIFF('{5}', r_born) - {0:d} srok,
    rp.r_id,
    rp.name,
#rabplace(r_parent) place,
    rp.place,
    aage AS age,
    {4:s},
    sc.suckers,    
    sc.suckGroups        
FROM rabbits r
    
    INNER JOIN (        # поодключаем родителей с адресами
        SELECT 
            r_id, 
            rabname(r_id,{2:s}) name,
            IF(r3.r_farm IS NULL, '', CONCAT_WS(',', r3.r_farm, r3.r_tier_id, r3.r_area, t_type, t_delims, t_nest)) AS place 
        FROM rabbits r3
            LEFT JOIN tiers ON r3.r_tier = t_id
        WHERE r3.r_parent IS NULL AND r_sex = 'female'
    ) rp ON rp.r_id = r.r_parent
        
    LEFT JOIN (         # подключаем суммарное количество детей в группах
        SELECT 
            r2.r_parent, 
            SUM(r2.r_group) suckers, 
            COUNT(1) suckGroups, 
            AVG(DATEDIFF('{5}', r2.r_born)) aage 
        FROM rabbits r2 
        WHERE r2.r_parent IS NOT NULL
        GROUP BY r2.r_parent 
    ) sc ON sc.r_parent = r.r_parent

WHERE r.r_parent IS NOT NULL 
GROUP BY r.r_parent
HAVING (srok_base >= {0:d} {1:s}) 
    AND r_id NOT IN (
        SELECT l_rabbit 
        FROM logs 
        WHERE l_type = {3:d} AND (DATE(l_date) <= DATE(NOW()) 
            AND DATE(l_date) >= DATE(NOW() - INTERVAL srok DAY))
    )
ORDER BY age DESC, 0+LEFT(place,LOCATE(',',place)) ASC;",
                _flt.safeInt("days"),
                (_flt.safeInt("next") == -1 ? "" : String.Format("AND srok_base < {0:d}", _flt.safeInt("next"))),
                getnm(),
                COUNT_KIDS_LOG,
                brd("(SELECT r7.r_breed FROM rabbits r7 WHERE r7.r_id = r.r_parent)"),
                _flt[Filters.DATE]
            );
        }

        private string qPreOkrol()
        {
            const int PRE_ORROL_LOG_TYPE = 21;

            return String.Format(@"SELECT 
    r_id,
    rabname(r_id,{2:s}) name,
#rabplace(r_id) place,
    {6} AS place,
    DATEDIFF('{4}', r_event_date) srok,
    DATEDIFF('{4}', r_born) age,
    r_status,    
    {3:s}
FROM rabbits 
    LEFT JOIN tiers ON r_tier = t_id
WHERE r_sex='female' AND r_id NOT IN (
        SELECT l_rabbit 
        FROM logs 
        WHERE l_type = {5:d} AND DATE(l_date) >= DATE(rabbits.r_event_date)
    ) 
HAVING srok >= {0:d} AND srok < {1:d};",
                _flt.safeInt(Filters.PRE_OKROL),
                _flt.safeInt(Filters.OKROL),
                getnm(),
                brd(),
                _flt[Filters.DATE],
                PRE_ORROL_LOG_TYPE,
                rabplace()
            );
        }

        private string qBoysGirlsOut()
        {

            return String.Format(@"SELECT *, {2}
FROM (" +

        // еси отсадка мальчиков, то ищем бесполые группы сидящие без мамки
    (_flt.safeInt("sex") == (int)Rabbit.SexType.MALE ? @"
	#stand alone boys
	SELECT
		DATEDIFF('{4}', r_born) age,
		DATEDIFF('{4}', r_born) - {1:d} srok, 
        r_id,		    
		rabname(r_id, {3}) name,   
        {5} AS place,
		r_breed
	FROM rabbits 
		LEFT JOIN tiers ON r_tier = t_id
	WHERE r_sex = 'void' AND r_parent IS NULL

	UNION " : "") +

@"
    #suckers
	SELECT
		DATEDIFF('{4}', r_born) age,
		DATEDIFF('{4}', r_born) - {1:d} srok, 
		rp.r_id,		
		rp.name, 
		rp.place,     
		r_breed
	FROM rabbits r		
		INNER JOIN (        # подключаем родителей с адресами
			SELECT 
				r_id, 
				rabname(r_id, {3}) name,
				IF(r3.r_farm IS NULL, '', CONCAT_WS(',', r3.r_farm, r3.r_tier_id, r3.r_area, t_type, t_delims, t_nest)) AS place 
			FROM rabbits r3
				LEFT JOIN tiers ON r3.r_tier = t_id
			WHERE r3.r_parent IS NULL AND r_sex = 'female'
		) rp ON rp.r_id = r.r_parent
	WHERE {0} AND r_parent IS NOT NULL 
) c
WHERE age >= {1:d} 
ORDER BY age DESC, 0+LEFT(place,LOCATE(',',place)) ASC;",
                    (_flt.safeInt("sex") == (int)Rabbit.SexType.FEMALE ? "r_sex = 'female'" : "(r_sex = 'void' OR r_sex = 'male')"),
                    (_flt.safeInt("sex") == (int)Rabbit.SexType.FEMALE ? _flt.safeInt(Filters.GIRLS_OUT) : _flt.safeInt(Filters.BOYS_OUT)),
                    brd(),
                    getnm(),
                    _flt[Filters.DATE],
                    rabplace()
                );



            //            return String.Format(@"SELECT DISTINCT
            //    IF(r_parent != 0, r_parent, r_id) r_id,
            //    DATEDIFF('{4}', r_born) age,
            //    DATEDIFF('{4}', r_born) - {1:d} srok,     
            //    rabname(IF(r_parent != 0, r_parent, r_id), {3:s}) name, 
            //    rabplace(IF(r_parent != 0, r_parent, r_id)) place,     
            //    {2:s} 
            //FROM rabbits 
            //WHERE {0:s} 
            //
            //HAVING age >= {1:d} 
            //ORDER BY age DESC, 0+LEFT(place,LOCATE(',',place)) ASC;",
            //                (_flt.safeInt("sex") == (int)Rabbit.SexType.FEMALE ? "(r_sex = 'female' AND r_parent IS NOT NULL)" : "(r_sex = 'void' OR (r_sex = 'male' AND r_parent IS NOT NULL))"),
            //                (_flt.safeInt("sex") == (int)Rabbit.SexType.FEMALE ? _flt.safeInt(Filters.GIRLS_OUT) : _flt.safeInt(Filters.BOYS_OUT)),
            //                brd(),
            //                getnm(),
            //                _flt[Filters.DATE]
            //            );
        }

        private string qFuck()
        {   ///todo необходимо получать партнеров на уровне выше отдельным запросом и сравнивать гетерозис и инбридинг программно.

            string query = "";

            if (_flt.safeBool(Filters.FIND_PARTNERS)) {
                query += String.Format(@"SET group_concat_max_len = 15000; 
CREATE TEMPORARY TABLE tPartn SELECT 
    rabname(r_id, 1) pname, 
	rabplace(r_id) pplace,
	r_breed pbreed,
	r_genesis pgens
FROM rabbits
WHERE r_sex = 'male' 
    AND r_status > 0 
    AND (r_last_fuck_okrol IS NULL OR DATEDIFF(NOW(), r_last_fuck_okrol) >= {0:d});",
                    _flt.safeInt(Filters.MALE_REST)
                ) + Environment.NewLine;
            }

            query += String.Format(
@"CREATE TEMPORARY TABLE aaa 
    SELECT 
        r.r_id,
        rabname(r_id,{8:s}) name, 
        rabplace(r_id) place, 
        DATEDIFF('{10}', r_born) age,
        COALESCE((SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent = r.r_id), null, 0) suckers,
        r_status,
        DATEDIFF('{10}', r_last_fuck_okrol) fromokrol,
        " + (_flt.safeBool(Filters.FIND_PARTNERS) ? @"(SELECT GROUP_CONCAT( CONCAT(pname,'&', pplace) ORDER BY pname SEPARATOR '|') FROM tPartn {9:s} {4:s}{5:s})" : "''") + @" partners,
        r_group,
        (SELECT {6:s} FROM breeds WHERE b_id = r_breed) breed, 
        0 srok, # такое поле тупо должно быть в результате
        lsrok,
        lshow
    FROM rabbits r
    LEFT JOIN (     #пытаемся найти стимуляцию
            SELECT
                rv.r_id AS lrid, 
                DATEDIFF(NOW(), Max(date)) AS lsrok, 
                v_duration AS ldura, 
                v_age AS lshow 
            FROM rab_vac rv
            INNER JOIN vaccines v ON v.v_id= rv.v_id
            WHERE rv.v_id = -1        #стимуляция (lust)
            GROUP BY rv.r_id
        ) vc ON lrid = r.r_id AND lsrok <= ldura
    WHERE Substr(r_flags,1,1) = '0'       #не брак
        AND Substr(r_flags,3,1) = '0'     #не готовая продукция
        AND r_sex = 'female' 
        AND r_event_date IS NULL        #не сукрольна
        AND r_status{7:s};
		
SELECT * 
FROM aaa 
WHERE (lsrok IS NULL OR lsrok = lshow)
    AND (
        age > {0:d} AND r_status=0                                #невеста
        OR (r_status = 1 AND (suckers = 0 OR fromokrol >= {1:d})) #первокролка
        OR (r_status > 1 AND (suckers = 0 OR fromokrol >= {2:d})) #штатная
    )
ORDER BY 0+LEFT(place,LOCATE(',',place)) ASC;

DROP TABLE IF EXISTS tPartn; 
DROP TABLE IF EXISTS aaa;",
                _flt.safeInt(Filters.MAKE_BRIDE),
                _flt.safeInt(Filters.FIRST_FUCK),
                _flt.safeInt(Filters.STATE_FUCK),
                _flt.safeInt(Filters.MALE_REST), //3
                (_flt.safeBool(Filters.HETEROSIS) ? "" : String.Format("pbreed=r.r_breed")),//4
                (_flt.safeBool(Filters.INBREEDING) ? "" : String.Format("{0:s}(SELECT COUNT(g_genom) FROM genoms WHERE g_id=r.r_genesis AND g_genom IN (SELECT g2.g_genom FROM genoms g2 WHERE g2.g_id=pgens))=0", _flt.safeBool(Filters.HETEROSIS) ? "" : " AND ")),

                (_flt.safeInt(Filters.SHORT) == 0 ? "b_name" : "b_short_name"),//6 
                (_flt.safeInt(Filters.TYPE) == 1 ? ">0" : "=0"),
                getnm(1),
                !_flt.safeBool(Filters.HETEROSIS) || !_flt.safeBool(Filters.INBREEDING) ? "WHERE" : "",
                _flt[Filters.DATE] //10
            );

            return query;
        }

        private string qVacc()
        {
            string show = _flt.safeValue(Filters.VACC_SHOW, "1") != "" ? _flt.safeValue(Filters.VACC_SHOW, "1") : "1";
            return String.Format(@"CREATE TEMPORARY TABLE aaa  SELECT 
    rb.r_id, 
    r_parent,
    rabname(r_id,{0:s}) name, 
    rabplace(r_id) place, 
    DATEDIFF('{6}', r_born) age, 
    r_group,
    TO_DAYS('{6}') - TO_DAYS(
        COALESCE(
            Date_Add(dt, INTERVAL v.v_duration DAY),
            If(
                v_do_after = 0, 
                Date_Add(r_born, INTERVAL v_age DAY), 
                (SELECT Date_Add(Max(`date`), INTERVAL v_age DAY) FROM rab_vac WHERE r_id = rb.r_id AND v_id = v_do_after)    #может получиться NULL если не было сделано предыдущей прививки
            )
        )
    ) srok,     #сколько дней не выполнена работа
    dt,     #когда последний раз делали прививку
    COALESCE(rv_times,0) times,  #сколько раз уже делали данную прививку
    v.v_id,
    v_age, 
    v_do_times,
    v_do_after,
    v_name, 
    {1:s}
FROM rabbits rb
CROSS JOIN vaccines v
LEFT JOIN (
        SELECT 
            r_id rvr_id, v_id rvv_id, Max(`date`) dt, COUNT(*) rv_times 
        FROM rab_vac rv 
        WHERE unabled != 1 
        GROUP BY r_id,v_id
    ) mxdt ON rvv_id = v.v_id AND rvr_id = rb.r_id
WHERE v_id in({2:s}) AND v_id > 0;

{3:s}

SELECT * FROM aaa WHERE (srok IS NOT NULL AND srok>=0) AND (v_do_times=0 OR (times<v_do_times)) {4:s} ORDER BY srok;

DROP TEMPORARY TABLE IF EXISTS aaa; {5:s};",
                getnm(),
                brd(),
                show,
                _flt.safeBool(Filters.VACC_MOTH, true) ? "CREATE TEMPORARY TABLE bbb SELECT DISTINCT r_parent FROM aaa WHERE r_parent !=0;" : "",
                _flt.safeBool(Filters.VACC_MOTH, true) ? "AND r_id NOT IN (SELECT r_parent FROM bbb)" : "",
                _flt.safeBool(Filters.VACC_MOTH, true) ? "DROP TEMPORARY TABLE IF EXISTS bbb;" : "",
                _flt[Filters.DATE]
            );
        }

        private string qSetNest()
        {
            return String.Format(@"SELECT 
    r_id, 
    rabname(r_id,{4}) name, 
#rabplace(r_id) place,
    {5} AS place,
    DATEDIFF('{2}', r_born) age,
    DATEDIFF('{2}', r_event_date) sukr,
    (SELECT SUM(r2.r_group) FROM rabbits r2 WHERE r2.r_parent = rabbits.r_id) children,
    {3},
    0 srok 
FROM rabbits 
    LEFT JOIN tiers ON r_tier = t_id
WHERE r_sex = 'female' AND r_event_date IS NOT NULL
HAVING ( (children IS NULL AND sukr >= {0:d}) OR (children > 0 AND sukr >= {1:d}) ) 
    AND (
        place NOT LIKE '%,%,0,jurta,%,1%' 
        AND place NOT LIKE '%,%,0,female,%,1%' 
        AND place NOT like '%,%,0,dfemale,%,1%' 
        AND place NOT LIKE '%,%,1,dfemale,%,_1'
    )
ORDER BY sukr DESC, 0+LEFT(place,LOCATE(',',place)) ASC;",
                _flt.safeInt(Filters.NEST_IN),
                _flt.safeInt(Filters.CHILD_NEST),
                _flt[Filters.DATE],
                brd(),
                getnm(),
                rabplace()
            );
        }

        private string qBoysByOne()
        {
            return String.Format(@"SELECT 
    r_id, 
    rabname(r_id,{0:s}) name, 
    rabplace(r_id) place, 
    r_group,
    DATEDIFF('{3}', r_born) - {1:d} srok,
    DATEDIFF('{3}', r_born) age,
    {2:s}
FROM rabbits 
WHERE r_group > 1 AND r_sex = 'male'
HAVING age >= {1:d};",
                getnm(),
                _flt.safeInt(Filters.BOYS_BY_ONE),
                brd(),
                _flt[Filters.DATE]
            );
        }

        private string qSpermTake()
        {
            return String.Format(@"SELECT 
    r_id, 
    rabname(r_id,{0:s}) name, 
    rabplace(r_id) place, 
    r_group,
    0 srok, 
    DATEDIFF('{3}', r_last_fuck_okrol) fromfuck,
    DATEDIFF('{3}', r_born) age,
    {2:s}
FROM rabbits
WHERE r_sex='male' AND r_status = 2 AND Date_Add(r_last_fuck_okrol, INTERVAL {1:d} DAY) < '{3}';",
                getnm(),
                _flt.safeInt(Filters.MALE_REST, 2),
                brd(),
                _flt[Filters.DATE]
            );
        }
    }
}
