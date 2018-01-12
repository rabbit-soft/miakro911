using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Xml;
using log4net;
using rabnet;

namespace db.mysql
{
    class Reports
    {
        MySqlConnection sql = null;
        ILog _logger = log4net.LogManager.GetLogger(typeof(Reports));
        private DateTime FROM = DateTime.Now;
        private DateTime TO = DateTime.Now;
        private String DFROM = "NOW()";
        private String DTO = "NOW()";
        private const String PROH = "п";

        public Reports(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public XmlDocument makeReport(myReportType type, Filters f)
        {
            String query = "";
            switch (type) {
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
                case myReportType.USER_OKROLS_YEAR:
                case myReportType.USER_OKROLS: return userOkrolRpt(qUserOkrols(f));
            }
#if DEBUG
            _logger.Debug(query);
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
            while (rd.Read()) {
                XmlElement rw = xml.CreateElement("Row");
                for (int i = 0; i < rd.FieldCount; i++) {
                    String nm = rd.GetName(i);
                    string vl = rd.IsDBNull(i) ? "" : rd.GetString(i);
                    if (nm.Length > 4)
                        if (nm.Substring(0, 4) == "adr_") {
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

        /// <summary>
        /// Создает строчку отчета по каждому самцу
        /// </summary>
        private XmlDocument makeRabOfDate(XmlDocument doc_in)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Rows"));
            XmlNodeList lst = doc_in.ChildNodes[0].ChildNodes;
            string name = "";
            string dt = "";
            int totalChildren = 0;
            bool okrolWas = false;
            int pCount = 0;
            foreach (XmlNode nd in lst) {
                if (nd.FirstChild.FirstChild.Value == name && nd.FirstChild.NextSibling.FirstChild.Value == dt) {
                    if (nd.FirstChild.NextSibling.NextSibling.FirstChild.Value == PROH) {
                        pCount++;
                    } else {
                        totalChildren += int.Parse(nd.FirstChild.NextSibling.NextSibling.FirstChild.Value);
                        okrolWas = true;
                    }
                    continue;
                } else if (name != "" && dt != "") {
                    XmlElement el = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                    ReportHelper.Append(el, doc, "name", name);
                    ReportHelper.Append(el, doc, "dt", dt);
                    ReportHelper.Append(el, doc, "state", String.Format("{0:s}{1:s}", okrolWas ? totalChildren.ToString() : "", pCount > 0 ? PROH + pCount.ToString() : "").Trim());
                }

                totalChildren = 0;
                pCount = 0;
                okrolWas = false;
                name = nd.FirstChild.FirstChild.Value;
                dt = nd.FirstChild.NextSibling.FirstChild.Value;
                if (nd.FirstChild.NextSibling.NextSibling.FirstChild.Value == PROH)
                    pCount++;
                else {
                    totalChildren = int.Parse(nd.FirstChild.NextSibling.NextSibling.FirstChild.Value);
                    okrolWas = true;
                }
            }
            XmlElement el2 = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
            ReportHelper.Append(el2, doc, "name", name);
            ReportHelper.Append(el2, doc, "dt", dt);
            ReportHelper.Append(el2, doc, "state", String.Format("{0:s}{1:s}", totalChildren > 0 ? totalChildren.ToString() : "", pCount > 0 ? PROH + (pCount.ToString()) : "").Trim());
            return doc;
        }

        /// <summary>
        /// Окролы по пользователям - Обработка
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private XmlDocument userOkrolRpt(String query)
        {
            const string SUMM = "C", TOTAL = "итого";
            XmlDocument doc = makeStdReportXml(query);
            XmlNodeList lst = doc.ChildNodes[0].ChildNodes;
            Dictionary<String, FuckerSummary> summary = new Dictionary<String, FuckerSummary>();
            Dictionary<String, int> dates = new Dictionary<String, int>();
            //Dictionary<String, int> cnts = new Dictionary<String, int>();//кл-во окролов
            //Dictionary<String, int> proh = new Dictionary<String, int>();//кл-во прохолостов
            //Dictionary<String, float> firt = new Dictionary<String, float>();//фиртильность

            ///вычисляем общие данные по каждому самцу
            foreach (XmlNode nd in lst) {
                String name = nd.FirstChild.FirstChild.Value;
                String date = nd.FirstChild.NextSibling.FirstChild.Value;
                String val = nd.FirstChild.NextSibling.NextSibling.FirstChild.Value;

                if (!summary.ContainsKey(name))
                    summary.Add(name, new FuckerSummary());

                if (val == PROH) {
                    summary[name].Proholosts++;
                    continue;
                } else if (val != "-") {
                    summary[name].Okrols++;
                    summary[name].Born += int.Parse(val);
                }

                if (!dates.ContainsKey(date))
                    dates.Add(date, int.Parse(val));
                else
                    dates[date] += int.Parse(val);
            }
            doc = makeRabOfDate(doc);
            lst = doc.ChildNodes[0].ChildNodes;

            ///создаем стобец "Сумма рожденных крольчат"
            int totalBorn = 0, totalOkrols = 0, totalProholosts = 0;
            foreach (String k in summary.Keys) {
                XmlElement rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                ReportHelper.Append(rw, doc, "name", k);
                ReportHelper.Append(rw, doc, "dt", SUMM);///
                ReportHelper.Append(rw, doc, "state", summary[k].Born.ToString());
                totalBorn += summary[k].Born;

                rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                ReportHelper.Append(rw, doc, "name", k);
                ReportHelper.Append(rw, doc, "dt", "О");
                ReportHelper.Append(rw, doc, "state", summary[k].Okrols.ToString());
                totalOkrols += summary[k].Okrols;

                rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                ReportHelper.Append(rw, doc, "name", k);
                ReportHelper.Append(rw, doc, "dt", "П");
                ReportHelper.Append(rw, doc, "state", summary[k].Proholosts.ToString());
                totalProholosts += summary[k].Proholosts;

                rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                ReportHelper.Append(rw, doc, "name", k);
                ReportHelper.Append(rw, doc, "dt", "Ф");
                double fert = Math.Round((double)summary[k].Okrols / (double)(summary[k].Proholosts + summary[k].Okrols), 2);
                ReportHelper.Append(rw, doc, "state", fert.ToString());
            }

            ///добавляем самую нижнюю суммирующую строчку

            XmlElement ttlRow = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
            ReportHelper.Append(ttlRow, doc, "name", TOTAL);
            ReportHelper.Append(ttlRow, doc, "dt", SUMM);
            ReportHelper.Append(ttlRow, doc, "state", totalBorn.ToString());

            ttlRow = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
            ReportHelper.Append(ttlRow, doc, "name", TOTAL);
            ReportHelper.Append(ttlRow, doc, "dt", "О");
            ReportHelper.Append(ttlRow, doc, "state", totalOkrols.ToString());

            ttlRow = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
            ReportHelper.Append(ttlRow, doc, "name", TOTAL);
            ReportHelper.Append(ttlRow, doc, "dt", "П");
            ReportHelper.Append(ttlRow, doc, "state", totalProholosts.ToString());

            ttlRow = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
            ReportHelper.Append(ttlRow, doc, "name", TOTAL);
            ReportHelper.Append(ttlRow, doc, "dt", "Ф");
            double totalFert = Math.Round((double)totalOkrols / (double)(totalProholosts + totalOkrols), 2);
            ReportHelper.Append(ttlRow, doc, "state", totalFert.ToString());

            foreach (String k in dates.Keys) {
                XmlElement rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                ReportHelper.Append(rw, doc, "name", TOTAL);
                ReportHelper.Append(rw, doc, "dt", k);
                ReportHelper.Append(rw, doc, "state", dates[k].ToString());
            }
            return doc;
        }

        //public static XmlDocument makeReport(MySqlConnection sql, myReportType type, Filters f)
        //{
        //    return new Reports(sql).makeReport(type, f);
        //}

        //public static XmlDocument makeReport(MySqlConnection sql, string query)
        //{
        //    return new Reports(sql).makeReport(query);
        //}

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
sum(r_group) vsego FROM rabbits GROUP BY r_breed;", f.safeValue("brd", "121"), f.safeValue("cnd", "120"));
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
            FROM = dfrom;
            TO = dto;
            DFROM = DBHelper.DateToSqlString(FROM);
            DTO = DBHelper.DateToSqlString(TO);
            return new DateTime[] { dfrom, dto };
        }

        private string fuckerQuery(Filters f)
        {
            getDates(f);
            int partner = f.safeInt("prt");
            return String.Format(@"(SELECT rabname(f_rabid,2) name,f_children,
    IF(f_type='vyazka','Вязка','случка') type,
    IF(f_state='proholost','Прохолостание','Окрол') state,
    DATE_FORMAT(f_date,'%d.%m.%Y') start,
    DATE_FORMAT(f_end_date,'%d.%m.%Y') stop 
FROM fucks 
WHERE f_partner={0:d} AND f_end_date>={1:s} AND f_end_date<={2:s});",
              partner, DFROM, DTO);
        }

        private String deadQuery(Filters f)
        {
            string where = DBHelper.MakeDatePeriod(f, "d_date");

            where = "WHERE " + String.Format("d_reason!={0:d}", DeadReason_Static.CombineGroups) + (where != "" ? " AND " + where : "");

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

            if (period != "")
                period = "WHERE " + period;
            string s = String.Format(@"(SELECT 
    SUM(r_group) grp,
    d_reason,
    (SELECT d_name FROM deadreasons WHERE d_reason=d_id) reason 
FROM dead {0} GROUP BY d_reason);", period);
            return s;
        }

        private String realize(Filters f)
        {
            int cnt = f.safeInt("cnt");
            String where = "r_id=0";
            for (int i = 0; i < cnt; i++)
                where += " OR r_id=" + f.safeInt("r" + i.ToString()).ToString();
            return String.Format(@"SELECT 
    rabname(r_id,2) name,
    TO_DAYS(NOW())-TO_DAYS(r_born) age,
    (SELECT b_name FROM breeds WHERE b_id = r_breed) breed, 
    rabplace(r_id) adr_adress,
    IF(r_sex='male','м',IF(r_sex='female','ж','?')) sex,
    '' comment,
    Concat(r_group,IF(yng_sum>0,Concat(' (+',yng_sum,')'),'')) r_group 
FROM rabbits r
LEFT JOIN (SELECT r_parent,Coalesce(sum(r_group),0) yng_sum FROM rabbits WHERE r_parent IS NOT NULL GROUP BY r_parent) yng ON yng.r_parent=r.r_id
WHERE {0:s} 
ORDER BY r_farm, r_tier_id, r_area;", where);
        }

        /// <summary>
        /// Окролы по пользователям - Запрос
        /// </summary>
        /// <param name="f"></param>
        /// <returns>SQL-запрос на получение окролов</returns>
        private String qUserOkrols(Filters f)
        {
            //getDates(f);
            int user = f.safeInt("user");
            string period = "";
            string format = "";
            string worker = f.safeInt("user", 0) > 0 ? "f_worker=" + f.safeInt("user") : "";

            if (f.safeValue(Filters.DATE_PERIOD) == "m") {
                DateTime dt = DateTime.Parse(f.safeValue(Filters.DATE_VALUE));
                period = String.Format("(MONTH(f_end_date)={0:MM} AND YEAR(f_end_date)={0:yyyy})", dt);
                format = "%d";
            } else if (f.safeValue(Filters.DATE_PERIOD) == "y") {
                period = String.Format("YEAR(f_end_date)={0}", f.safeValue(Filters.DATE_VALUE));
                format = "%m";
            }

            if (worker != "") {
                period = " AND " + period;
            }
            string result = String.Format(@"SELECT 
    CONCAT(' ',anyname(f_partner,0)) name,
    DATE_FORMAT(f_end_date,'{2}') dt,
    IF (f_state = 'okrol', f_children, IF(f_state='proholost','п','-')) state 
FROM fucks 
WHERE {0:s} {1:s} 
ORDER BY name,dt;", worker, period, format);

            return result;
        }

        private String getValue(String query)
        {
            MySqlCommand cmd = new MySqlCommand(query, sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            String res = "";
            if (rd.Read()) {
                res = rd.GetString(0);
            }
            rd.Close();
            return res;
        }

        private int getInt32(String query)
        {
#if DEBUG
            _logger.Debug(query);
#endif
            return int.Parse(getValue(query));
        }

        private int getBuildCount(BuildingType type, int bid)
        {
            return getInt32(String.Format(@"SELECT COUNT(t_id) FROM tiers,minifarms WHERE
(t_id=m_upper OR t_id=m_lower) AND inBuilding({0:d},m_id){1:s};", bid, type == BuildingType.None ? "" : " AND t_type='" + Building.GetName(type) + "'"));
        }

        private int round(double d)
        {
            return (int)Math.Round(d);
        }

        private void addShedRows(XmlDocument doc, String type, int ideal, int real)
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
            const double PER_VERTEP = 3.2;
            const double PER_FEMALE = 6;
            const double PREGN_PER_TIER = 0.3114;
            const double FEED_GIRLS_PER_TIER = 0.6;
            const double FEED_BOYS_PER_TIER = 2.0;
            const double UNKN_SUCKS_PER_TIER = 2;
            const double UNKN_NEST_PER_TIER = 0.7;

            int bid = f.safeInt("bld");
            int nest_out = f.safeInt("nest_out", 38);
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Rows"));
            int alltiers = getBuildCount(BuildingType.None, bid);
            int fem = getBuildCount(BuildingType.Female, bid);
            int dfe = getBuildCount(BuildingType.DualFemale, bid);
            int com = getBuildCount(BuildingType.Complex, bid);
            int jur = getBuildCount(BuildingType.Jurta, bid);
            int qua = getBuildCount(BuildingType.Quarta, bid);
            int ver = getBuildCount(BuildingType.Vertep, bid);
            int bar = getBuildCount(BuildingType.Barin, bid);
            int cab = getBuildCount(BuildingType.Cabin, bid);
            int ideal = round(PER_VERTEP * (ver + bar + 4 * qua + 2 * com + cab / 2) + PER_FEMALE * (2 * (dfe + jur) + fem + com + cab));
            int real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) 
FROM rabbits 
WHERE (r_parent IS NULL AND inBuilding({0:d},r_farm)) 
    OR (r_parent IS NOT NULL AND inBuilding({0:d},(SELECT r2.r_farm FROM rabbits r2 WHERE r2.r_id=rabbits.r_parent)));", bid));
            addShedRows(doc, "  все", ideal, real);

            ideal = fem + 2 * (dfe + jur) + com;
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) 
FROM rabbits 
WHERE r_sex = 'female' AND (r_status > 0 OR r_event_date IS NOT NULL) AND inBuilding({0:d},r_farm);", bid));
            addShedRows(doc, "  крольчихи", ideal, real);

            ideal = round(ideal * PREGN_PER_TIER);
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) 
FROM rabbits 
WHERE r_sex = 'female' AND r_event_date IS NOT NULL AND inBuilding({0:d},r_farm);", bid));
            addShedRows(doc, "  сукрольные", ideal, real);

            ideal = round(alltiers * FEED_GIRLS_PER_TIER);
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) 
FROM rabbits,tiers 
WHERE r_tier = t_id AND (t_type = 'quarta' OR (r_area = 1 AND (t_type = 'complex' OR t_type = 'cabin'))) 
    AND r_parent IS NULL AND r_sex = 'female' AND r_status = 0 AND r_event_date IS NULL AND inBuilding({0:d},r_farm);", bid));
            addShedRows(doc, " Д.откорм", ideal, real);

            ideal = round(alltiers * FEED_BOYS_PER_TIER);
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) 
FROM rabbits,tiers 
WHERE r_tier=t_id AND (t_type = 'quarta' OR (r_area=1 AND (t_type = 'complex' OR t_type = 'cabin'))) 
    AND r_parent IS NULL AND r_sex = 'male' AND r_status = 0 AND inBuilding({0:d},r_farm);", bid));
            addShedRows(doc, " М.откорм", ideal, real);

            ideal = round(UNKN_SUCKS_PER_TIER * alltiers);
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) 
FROM rabbits 
WHERE r_parent IS NOT NULL AND TO_DAYS(NOW())-TO_DAYS(r_born)>={1:d} AND inBuilding({0:d},(SELECT r2.r_farm FROM rabbits r2 WHERE r2.r_id=rabbits.r_parent));", bid, nest_out));
            addShedRows(doc, " подсосные", ideal, real);

            ideal = round(UNKN_NEST_PER_TIER * alltiers);
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) 
FROM rabbits 
WHERE r_parent IS NOT NULL AND TO_DAYS(NOW())-TO_DAYS(r_born) < {1:d} AND inBuilding({0:d},(SELECT r2.r_farm FROM rabbits r2 WHERE r2.r_id=rabbits.r_parent));", bid, nest_out));
            addShedRows(doc, "гнездовые", ideal, real);

            return doc;
        }

        private XmlDocument revision(Filters f)
        {
            int bld = f.safeInt("bld");
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Rows"));
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT m_id, t_type, t_busy1, t_busy2, t_busy3, t_busy4, t_delims
                FROM tiers,minifarms 
                WHERE (t_busy1 = 0 OR t_busy2 = 0 OR t_busy3 = 0 OR t_busy4 = 0) AND (t_id = m_upper OR t_id = m_lower) AND inBuilding({0:d},m_id)
                ORDER BY m_id;", bld), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read()) {
                for (int i = 0; i < Building.GetRSecCount(rd.GetString(1)); i++) {
                    if (rd.IsDBNull(i + 2) || rd.GetInt32(i + 2) == 0) {
                        ///NULL быть не должно
                        doc.DocumentElement.AppendChild(doc.CreateElement("Row")).AppendChild(
                            doc.CreateElement("address")).AppendChild(doc.CreateTextNode(rd.GetString("m_id") + Building.GetSecRus(rd.GetString("t_type"), i, rd.GetString("t_delims"))));
                    }
                }
            }
            rd.Close();
            return doc;
        }

        private string rabByMonth()
        {
            string s = @"SELECT
    DATE_FORMAT(r_born,'%m.%Y') date,
    Coalesce(
        (select sum(f_children+f_added) from fucks where date_format(f_end_date,'%m.%Y')=date),
        (SELECT COALESCE(SUM(r_group),0) FROM dead d WHERE MONTH(d.r_born)=MONTH(rabbits.r_born) AND YEAR(d.r_born)=YEAR(rabbits.r_born))+COALESCE(SUM(r_group),0) #если есть данные из старой программы
    ) count,
    COALESCE(SUM(r_group),0) alife
FROM rabbits 
GROUP BY date 
ORDER BY year(r_born) desc,month(r_born) desc;";
            return s;
        }

        private string fucksByDate(Filters f)
        {
            string period = " AND " + DBHelper.MakeDatePeriod(f, "f_date");

            return String.Format(@"SELECT 
    DATE_FORMAT(f_date,'%d.%m.%Y') date,
    anyname(f_rabid,2) name,
    n_name as partner,
    u_name as worker 
FROM fucks 
    LEFT JOIN names ON n_use = f_partner
    LEFT JOIN users ON u_id = f_worker
WHERE f_date is not null {0} 
ORDER BY f_date DESC, partner;", period);
        }

        private string butcherQuery(Filters f)
        {
            string period = DBHelper.MakeDatePeriod(f, "b_date");

            if (period != "")
                period = "WHERE " + period;
            return String.Format(@"SELECT 
    DATE_FORMAT(b_date,'%d.%m.%Y')date,
    (SELECT p_name FROM products WHERE p_id=b_prodtype) prod,
    b_amount,
    (SELECT p_unit FROM products WHERE p_id = b_prodtype) unt,
    (SELECT u_name FROM users WHERE b_user = u_id) user
FROM butcher 
{0} ORDER BY b_date DESC;", period);
        }
    }
}
