using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Xml;
using log4net;
using rabnet;

namespace db.mysql
{    
#if !DEMO
    class Reports
    {
        MySqlConnection sql = null;
        ILog log = log4net.LogManager.GetLogger(typeof(Reports));
        private DateTime FROM = DateTime.Now;
        private DateTime TO = DateTime.Now;
        private String DFROM = "NOW()";
        private String DTO = "NOW()";

        public Reports(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public XmlDocument makeReport(myReportType type, Filters f)
        {
            String query = "";
            switch (type)
            {
                case myReportType.AGE: query = ageQuery(f); break;
                case myReportType.BY_MONTH: query = rabByMonth(); break;
                case myReportType.BUTCHER_PERIOD: query = butcherQuery(f); break;
                case myReportType.BREEDS: query = breedsQuery(f); break;
                case myReportType.DEAD: query = deadQuery(f); break;
                case myReportType.DEADREASONS: query = deadReasonsQuery(f); break;
                case myReportType.FUCKS_BY_DATE: query = fucksByDate(f); break;
                case myReportType.FUCKER: query = fuckerQuery(f); break;
                case myReportType.REALIZE: query = realize(f); break;                                
                case myReportType.REVISION: return revision(f);               
                case myReportType.SHED: return shedReport(f);
                case myReportType.TEST: query = testQuery(f); break;
                case myReportType.USER_OKROLS: return userOkrolRpt(UserOkrols(f));
            }
#if DEBUG
            log.Debug(query);
#endif
            return makeStdReportXml(query);
        }

        public XmlDocument makeReport(string query)
        {
            return makeStdReportXml(query);
        }

        private XmlDocument makeStdReportXml(String query)
        {
            MySqlCommand cmd = new MySqlCommand(query, sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("Rows");
            xml.AppendChild(root);
            while (rd.Read())
            {
                XmlElement rw = xml.CreateElement("Row");
                for (int i = 0; i < rd.FieldCount; i++)
                {
                    String nm = rd.GetName(i);
                    string vl = rd.IsDBNull(i) ? "" : rd.GetString(i);
                    if (nm.Length>4)
                    if (nm.Substring(0, 4) == "adr_")
                    {
                        nm = nm.Substring(4);
                        vl = Building.FullPlaceName(vl, true, false, false);
                    }
                    XmlElement f = xml.CreateElement(nm);
                    f.AppendChild(xml.CreateTextNode(vl));
                    rw.AppendChild(f);
                }
                root.AppendChild(rw);
            }
            rd.Close();
            return xml;
        }

        private XmlDocument makeRabOfDate(XmlDocument doc_in)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Rows"));
            XmlNodeList lst = doc_in.ChildNodes[0].ChildNodes;
            string name = "";
            string dt = "";
            int state = 0;
            foreach (XmlNode nd in lst)
            {
                if (name == "" && dt == "")
                {
                    name = nd.FirstChild.FirstChild.Value;
                    dt = nd.FirstChild.NextSibling.FirstChild.Value;
                    state = nd.FirstChild.NextSibling.NextSibling.FirstChild.Value == "п" ? state = 0 : state = int.Parse(nd.FirstChild.NextSibling.NextSibling.FirstChild.Value);
                    continue;
                }
                
                if (nd.FirstChild.FirstChild.Value == name && nd.FirstChild.NextSibling.FirstChild.Value == dt)
                {
                    if (nd.FirstChild.NextSibling.NextSibling.FirstChild.Value == "п")
                    {
                        state += 0;
                        continue;
                    }
                    state += int.Parse(nd.FirstChild.NextSibling.NextSibling.FirstChild.Value);
                    continue;
                }
                else 
                {
                    XmlElement el = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                    ReportHelper.Append(el, doc, "name", name);
                    ReportHelper.Append(el, doc, "dt", dt);
                    ReportHelper.Append(el, doc, "state", state.ToString());

                    name = nd.FirstChild.FirstChild.Value;
                    dt = nd.FirstChild.NextSibling.FirstChild.Value;
                    state = nd.FirstChild.NextSibling.NextSibling.FirstChild.Value == "п" ? state = 0 : state = int.Parse(nd.FirstChild.NextSibling.NextSibling.FirstChild.Value);
                }               
            }
            XmlElement el2 = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
            ReportHelper.Append(el2, doc, "name", name);
            ReportHelper.Append(el2, doc, "dt", dt);
            ReportHelper.Append(el2, doc, "state", state.ToString());
            return doc;
        }

        /// <summary>
        /// Окролы по пользователям - Обработка
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private XmlDocument userOkrolRpt(String query)
        {
            XmlDocument doc = makeStdReportXml(query);
            XmlNodeList lst = doc.ChildNodes[0].ChildNodes;
            Dictionary<String, int> sums = new Dictionary<String, int>();//кл-во детей
            Dictionary<String, int> cnts = new Dictionary<String, int>();//кл-во окролов
            foreach (XmlNode nd in lst)
            {
                String nm = nd.FirstChild.FirstChild.Value;
                String v = nd.FirstChild.NextSibling.NextSibling.FirstChild.Value;
                int s = 0;
                int cnt = 0;
                if (v != "п" && v != "-")
                {
                    s += int.Parse(v);
                    cnt += 1;
                }
                if (sums.ContainsKey(nm)) sums[nm] += s;
                else sums.Add(nm, s);
                if (cnts.ContainsKey(nm)) cnts[nm] += cnt;
                else cnts.Add(nm,cnt);
                                
            }
            doc = makeRabOfDate(doc);
            lst = doc.ChildNodes[0].ChildNodes;
            foreach (String k in sums.Keys)
            {
                XmlElement rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                ReportHelper.Append(rw, doc, "name", k);
                ReportHelper.Append(rw, doc, "dt", "Cум.");
                ReportHelper.Append(rw, doc, "state", sums[k].ToString());
            }
            foreach (String k in cnts.Keys)
            {
                XmlElement rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                ReportHelper.Append(rw, doc, "name", k);
                ReportHelper.Append(rw, doc, "dt", "К.Окр.");
                ReportHelper.Append(rw, doc, "state", cnts[k].ToString());
            }
            sums.Clear();
            foreach (XmlNode nd in lst)
            {
                String nm = nd.FirstChild.NextSibling.FirstChild.Value;
                String v = nd.FirstChild.NextSibling.NextSibling.FirstChild.Value;
                int s = 0;
                if (v != "п" && v != "-") s += int.Parse(v);
                if (sums.ContainsKey(nm)) sums[nm] += s;
                else sums.Add(nm, s);
            }
            foreach (String k in sums.Keys)
            {
                XmlElement rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                ReportHelper.Append(rw, doc, "name", "итого");
                ReportHelper.Append(rw, doc, "dt", k);
                ReportHelper.Append(rw, doc, "state", sums[k].ToString());
            }
            return doc;
        }

        public static XmlDocument makeReport(MySqlConnection sql,myReportType type,Filters f)
        {
            return new Reports(sql).makeReport(type, f);
        }

        public static XmlDocument makeReport(MySqlConnection sql, string query)
        {
            return new Reports(sql).makeReport(query);
        }

        private string testQuery(Filters f)
        {
            return "SELECT rabname(r_id,2) f1,(TO_DAYS(NOW())-TO_DAYS(r_born)) f2 FROM rabbits LIMIT 100;";
        }

        /// <summary>
        /// Отчет "Состав пород"
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private string breedsQuery(Filters f)
        {
            return String.Format(@"SELECT r_breed br,(SELECT b_name FROM breeds WHERE b_id=br) breed,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='male' AND r_status=2 AND r_breed=br) fuck,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='male' AND (r_status=1 OR (r_status=0 AND to_days(NOW())-to_days(r_born)>={1:s} )) AND r_breed=br) kandidat,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='male' AND r_status=0 AND (to_days(NOW())-to_days(r_born)<{1:s})  AND r_breed=br) boys,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status>=2 AND r_breed=br) state,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND ((r_status=0 AND r_event_date IS NOT NULL)OR r_status=1 ) AND r_breed=br) pervo,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND (r_status=0 AND r_event_date IS NULL) AND r_breed=br AND r_born<=(now()-INTERVAL {0:s} day)) nevest,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=0 AND r_breed=br and r_born>(now()-INTERVAL {0:s} day)) girl,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='void' AND r_breed=br) bezpolie,
sum(r_group) vsego FROM rabbits GROUP BY r_breed;", f.safeValue("brd","121"),f.safeValue("cnd","120"));
        }

        /// <summary>
        /// Количество по месяцам
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private string ageQuery(Filters f)
        {
            return @"SELECT (TO_DAYS(NOW())-TO_DAYS(r_born)) age,sum(r_group) cnt FROM rabbits GROUP BY age
union SELECT 'Итого',sum(r_group) FROM rabbits;";
        }

        private DateTime[] getDates(Filters f)
        {
            DateTime dfrom = DateTime.Parse(f.safeValue("dfr", DateTime.Now.ToShortDateString()));
            DateTime dto = DateTime.Parse(f.safeValue("dto", DateTime.Now.ToShortDateString()));
            FROM=dfrom;
            TO=dto;
            DFROM = DBHelper.DateToMyString(FROM);
            DTO = DBHelper.DateToMyString(TO);
            return new DateTime[] { dfrom, dto };
        }

        private string fuckerQuery(Filters f)
        {
            getDates(f);
            int partner = f.safeInt("prt");           
            return String.Format(@"(SELECT rabname(f_rabid,2) name,f_children,
IF(f_type='vyazka','Вязка','случка') type,
IF(f_state='proholost','Прохолостание','Окрол') state,
DATE_FORMAT(f_date,'%d.%m.%Y') start,DATE_FORMAT(f_end_date,'%d.%m.%Y') stop 
FROM fucks WHERE f_partner={0:d} AND f_end_date>={1:s} AND f_end_date<={2:s});",
              partner,DFROM,DTO);
        }

        private String deadQuery(Filters f)
        {
            string where = DBHelper.MakeDatePeriod(f, "d_date");   
    
            //if (f.safeValue(Filters.DATE_PERIOD) == "d")
            //{
            //    DateTime dt = DateTime.Parse(f.safeValue(Filters.DATE_VALUE));
            //    where = String.Format("DATE(d_date)='{0:yyyy-MM-dd}'", dt);
            //}
            //else if (f.safeValue(Filters.DATE_PERIOD) == "y")
            //{
            //    where = String.Format("YEAR(d_date)={0}", f.safeValue(Filters.DATE_VALUE));
            //}
            //else if (f.safeValue(Filters.DATE_PERIOD) == "m")
            //{
            //    DateTime dt = DateTime.Parse(f.safeValue(Filters.DATE_VALUE));
            //    where = String.Format("MONTH(d_date)={0:MM} AND YEAR(d_date)={0:yyyy}", dt);
            //}

            where = "WHERE " + String.Format("d_reason!={0:d}", DeadReason_Static.CombineGroups) + (where!="" ? " AND "+where:"");

            return String.Format(@"SELECT DATE_FORMAT(d_date,'%d.%m.%Y') date,
    deadname(r_id,2) name,
    r_group,
    To_Days(d_date)-To_Days(r_born) dage,
    (SELECT d_name FROM deadreasons WHERE d_id=d_reason) reason,
    d_notes 
FROM dead {0:s} 
ORDER BY d_reason,d_date ASC;", where);
            
        }

        private String deadReasonsQuery(Filters f)
        {
            //getDates(f);
            string period = DBHelper.MakeDatePeriod(f, "d_date");

            //if (f.safeValue(Filters.DATE_PERIOD) == "d")
            //{
            //    DateTime dt = DateTime.Parse(f.safeValue(Filters.DATE_VALUE));
            //    period = String.Format("DATE(d_date)='{0:yyyy-MM-dd}'", dt);
            //}
            //else if (f.safeValue(Filters.DATE_PERIOD) == "y")
            //{
            //    period = String.Format("YEAR(d_date)={0}", f.safeValue(Filters.DATE_VALUE));
            //}
            //else
            //{
            //    DateTime dt = DateTime.Parse(f.safeValue(Filters.DATE_VALUE));
            //    period = String.Format("MONTH(d_date)={0:MM} AND YEAR(d_date)={0:yyyy}", dt);
            //}
            if (period != "")
                period = "WHERE " + period;
            string s = String.Format(@"(SELECT 
    SUM(r_group) grp,
    d_reason,
    (SELECT d_name FROM deadreasons WHERE d_reason=d_id) reason 
FROM dead {0} GROUP BY d_reason);",period);
            return s;
        }

        private String realize(Filters f)
        {
            int cnt = f.safeInt("cnt");
            String where = "r_id=0";
            for (int i = 0; i < cnt; i++)
                where += " OR r_id=" + f.safeInt("r" + i.ToString()).ToString();
            return String.Format(@"SELECT rabname(r_id,2) name,TO_DAYS(NOW())-TO_DAYS(r_born) age,
(SELECT b_name FROM breeds WHERE b_id=r_breed) breed, rabplace(r_id) adr_adress,
IF(r_sex='male','м',IF(r_sex='female','ж','?')) sex,'' comment,
r_group FROM rabbits WHERE {0:s} ORDER BY r_farm,r_tier_id,r_area;",where);
        }

        /// <summary>
        /// Окролы по пользователям - Запрос
        /// </summary>
        /// <param name="f"></param>
        /// <returns>SQL-запрос на получение окролов</returns>
        private String UserOkrols(Filters f)
        {
            //getDates(f);
            int user = f.safeInt("user");
            string period = "";
            string format = "";
            string worker = f.safeInt("user", 0)>0 ? "f_worker="+f.safeInt("user"):"";

            if (f.safeValue(Filters.DATE_PERIOD) == "m")
            {
                DateTime dt = DateTime.Parse(f.safeValue(Filters.DATE_VALUE));
                period = String.Format("(MONTH(f_end_date)={0:MM} AND YEAR(f_end_date)={0:yyyy})", dt);
                format = "%d";
            }
            else if (f.safeValue(Filters.DATE_PERIOD) == "y")
            {
                period = String.Format("YEAR(f_end_date)={0}", f.safeValue(Filters.DATE_VALUE));
                format = "%m";
            }

            if (worker != "")
                period = " AND " + period;
            string result = String.Format(@"SELECT 
    CONCAT(' ',anyname(f_partner,0)) name,
    DATE_FORMAT(f_end_date,'{2}') dt,
    IF (f_state='okrol',f_children,IF(f_state='proholost','п','-')) state 
FROM fucks 
WHERE {0:s} {1:s} ORDER BY name,dt;",worker , period, format);

            return result;
        }

        private String getValue(String query)
        {
            MySqlCommand cmd = new MySqlCommand(query, sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            String res = "";
            if (rd.Read())
                res = rd.GetString(0);
            rd.Close();
            return res;
        }

        private int getInt32(String query)
        {
            return int.Parse(getValue(query));
        }

        private int getBuildCount(String type,int bid)
        {
            return getInt32(String.Format(@"SELECT COUNT(t_id) FROM tiers,minifarms WHERE
(t_id=m_upper OR t_id=m_lower) AND inBuilding({0:d},m_id){1:s};",bid, type==""?"":" AND t_type='"+type+"'"));
        }

        private int round(double d)
        {
            return (int)Math.Round(d);
        }

        private void addShedRows(XmlDocument doc,String type, int ideal, int real)
        {
            XmlElement rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
            ReportHelper.Append(rw, doc, "name", type);
            ReportHelper.Append(rw, doc, "type", "идеал");
            ReportHelper.Append(rw, doc, "value", ideal.ToString());
            rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
            ReportHelper.Append(rw, doc, "name", type);
            ReportHelper.Append(rw, doc, "type", "реально");
            ReportHelper.Append(rw, doc, "value", real.ToString());
        }

        private XmlDocument shedReport(Filters f)
        {
            double per_vertep = 3.2;
            double per_female = 6;              
            double pregn_per_tier = 0.3114;     
            double feed_girls_per_tier = 0.6;   
            double feed_boys_per_tier = 2.0;    
            double unkn_sucks_per_tier = 2.7;
            int bid = f.safeInt("bld");
            int nest_out = f.safeInt("nest_out", 38);
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Rows"));
            int alltiers = getBuildCount("", bid);
            int fem = getBuildCount(BuildingType.Female, bid); 
            int dfe = getBuildCount(BuildingType.DualFemale, bid);
            int com = getBuildCount(BuildingType.Complex, bid);
            int jur = getBuildCount(BuildingType.Jurta, bid);
            int qua = getBuildCount(BuildingType.Quarta, bid);
            int ver = getBuildCount(BuildingType.Vertep, bid);
            int bar = getBuildCount(BuildingType.Barin, bid);
            int cab = getBuildCount(BuildingType.Cabin, bid);
            int ideal=round(per_vertep*(ver+bar+4*qua+2*com+cab/2)+per_female* (2 * (dfe + jur) + fem + com + cab));
            int real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) FROM rabbits WHERE (r_parent=0 AND inBuilding({0:d},r_farm))OR
(r_parent!=0 AND inBuilding({0:d},(SELECT r2.r_farm FROM rabbits r2 WHERE r2.r_id=rabbits.r_parent)));",bid));
            addShedRows(doc, "  все", ideal, real);

            ideal = fem + 2 * (dfe + jur) + com;
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) FROM rabbits WHERE r_sex='female' AND 
(r_status>0 OR r_event_date IS NOT NULL) AND inBuilding({0:d},r_farm);",bid));
            addShedRows(doc, "  крольчихи", ideal, real);

            ideal = round(ideal*pregn_per_tier);
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) FROM rabbits WHERE r_sex='female' AND 
r_event_date IS NOT NULL AND inBuilding({0:d},r_farm);", bid));
            addShedRows(doc, "  сукрольные", ideal, real);

            ideal = round(alltiers * feed_girls_per_tier);
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) FROM rabbits,tiers WHERE r_tier=t_id AND 
(t_type='quarta' OR (r_area=1 AND (t_type='complex' OR t_type='cabin'))) AND r_parent=0 AND r_sex='female'
AND r_status=0 AND r_event_date IS NULL AND inBuilding({0:d},r_farm);",bid));
            addShedRows(doc, " Д.откорм", ideal, real);

            ideal = round(alltiers * feed_boys_per_tier);
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) FROM rabbits,tiers WHERE r_tier=t_id AND 
(t_type='quarta' OR (r_area=1 AND (t_type='complex' OR t_type='cabin'))) AND r_parent=0 AND r_sex='male'
AND r_status=0 AND inBuilding({0:d},r_farm);", bid));
            addShedRows(doc, " М.откорм", ideal, real);

            ideal = round(unkn_sucks_per_tier * alltiers);
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) FROM rabbits WHERE r_parent<>0 AND TO_DAYS(NOW())-TO_DAYS(r_born)>={1:d} 
AND inBuilding({0:d},(SELECT r2.r_farm FROM rabbits r2 WHERE r2.r_id=rabbits.r_parent));", bid, nest_out));
            addShedRows(doc, " подсосные", ideal, real);

            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) FROM rabbits WHERE r_parent<>0 AND TO_DAYS(NOW())-TO_DAYS(r_born)<{1:d} 
AND inBuilding({0:d},(SELECT r2.r_farm FROM rabbits r2 WHERE r2.r_id=rabbits.r_parent));", bid, nest_out));
            ideal = real;
            addShedRows(doc, "гнездовые", ideal, real);

            return doc;
        }

        private XmlDocument revision(Filters f)
        {
            int bld = f.safeInt("bld");
            XmlDocument doc=new XmlDocument();
            doc.AppendChild(doc.CreateElement("Rows"));
            MySqlCommand cmd=new MySqlCommand(String.Format(@"SELECT m_id,t_type,t_busy1,t_busy2,t_busy3,t_busy4 
FROM tiers,minifarms WHERE (t_busy1=0 OR t_busy2=0 OR t_busy3=0 OR t_busy4=0) AND (t_id=m_upper OR t_id=m_lower) AND inBuilding({0:d},m_id);",bld),sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
                for (int i = 0; i < Building.GetRSecCount(rd.GetString(1)); i++)
                    if (rd.GetInt32(i + 2) == 0)
                    {
                        doc.DocumentElement.AppendChild(doc.CreateElement("Row")).AppendChild(
                            doc.CreateElement("address")).AppendChild(doc.CreateTextNode(rd.GetString(0)+Building.GetRSec(rd.GetString(1),i,"000")));
                    }
            rd.Close();
            return doc;
        }

        private string rabByMonth()
        {
            //return "SELECT DATE_FORMAT(r_born,'%m.%Y') date, sum(r_group) count FROM rabbits GROUP BY date ORDER BY year(r_born) desc,month(r_born) desc;";
            /*string s = @"SELECT
                        DATE_FORMAT(r_born,'%m.%Y') date,
                        (SELECT COALESCE(SUM(r_group),0) FROM dead d WHERE MONTH(d.r_born)=MONTH(rabbits.r_born) AND YEAR(d.r_born)=YEAR(rabbits.r_born))+COALESCE(SUM(r_group),0) count,
                        COALESCE(SUM(r_group),0) alife
                        FROM rabbits GROUP BY date ORDER BY year(r_born) desc,month(r_born) desc;";
            Закоментирован 26.05.2011, раскоментировать месяца через 4 */
            string s = @"SELECT
                DATE_FORMAT(r_born,'%m.%Y') date,
                Coalesce(
                    (select sum(f_children+f_added) from fucks where date_format(f_end_date,'%m.%Y')=date),
                    (SELECT COALESCE(SUM(r_group),0) FROM dead d WHERE MONTH(d.r_born)=MONTH(rabbits.r_born) AND YEAR(d.r_born)=YEAR(rabbits.r_born))+COALESCE(SUM(r_group),0) #если есть данные из старой программы
                ) count,
                COALESCE(SUM(r_group),0) alife
                FROM rabbits GROUP BY date ORDER BY year(r_born) desc,month(r_born) desc;";
            return s;
        }

        private string fucksByDate(Filters f)
        {
            string period = " AND "+DBHelper.MakeDatePeriod(f, "f_date");

            //if (f.safeValue(Filters.DATE_PERIOD) == "d")
            //{
            //    DateTime dt = DateTime.Parse(f.safeValue(Filters.DATE_VALUE));
            //    period = String.Format("AND DATE(f_date)='{0:yyyy-MM-dd}'", dt);
            //}
            //else if (f.safeValue(Filters.DATE_PERIOD) == "m")
            //{
            //    DateTime dt = DateTime.Parse(f.safeValue(Filters.DATE_VALUE));
            //    period = String.Format("AND (MONTH(f_date)={0:MM} AND YEAR(f_date)={0:yyyy})", dt);

            //}
            //else if (f.safeValue(Filters.DATE_PERIOD) == "y")
            //{
            //    period = String.Format("AND YEAR(f_date)={0}", f.safeValue(Filters.DATE_VALUE));
            //}

            return String.Format(@"SELECT DATE_FORMAT(f_date,'%d.%m.%Y')date,anyname(f_rabid,2) name,
                                    (SELECT n_name FROM names WHERE n_use=f_partner) partner,
                                    (SELECT u_name FROM users WHERE u_id=f_worker) worker 
                                FROM fucks WHERE f_date is not null {0} ORDER BY f_date DESC, f_worker;",period);
        }

        private string butcherQuery(Filters f)
        {
            string period = DBHelper.MakeDatePeriod(f, "b_date");

            //if (f.safeValue(Filters.DATE_PERIOD) == "d")
            //{
            //    DateTime dt = DateTime.Parse(f.safeValue(Filters.DATE_VALUE));
            //    period = String.Format("DATE(b_date)='{0:yyyy-MM-dd}'", dt);
            //}
            //if (f.safeValue(Filters.DATE_PERIOD) == "m")
            //{
            //    DateTime dt = DateTime.Parse(f.safeValue(Filters.DATE_VALUE));
            //    period = String.Format("(MONTH(b_date)={0:MM} AND YEAR(b_date)={0:yyyy})", dt);
            //}
            //else if (f.safeValue(Filters.DATE_PERIOD) == "y")
            //{
            //    period = String.Format("YEAR(b_date)={0}", f.safeValue(Filters.DATE_VALUE));
            //}
             
            if(period!="")
                period = "WHERE " + period;
            return String.Format(@"SELECT 
    DATE_FORMAT(b_date,'%d.%m.%Y')date,
    (SELECT p_name FROM products WHERE p_id=b_prodtype) prod,
    b_amount,
    (SELECT p_unit FROM products WHERE p_id = b_prodtype) unt,
    (SELECT u_name FROM users WHERE b_user=u_id) user
FROM butcher 
{0} ORDER BY b_date DESC;", period);
        }
    }
#endif
}
