using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Xml;

namespace rabnet
{
    public class ReportType
    {
        public enum Type { TEST,BREEDS,AGE,FUCKER,DEAD,DEADREASONS,REALIZE,USER_OKROLS,SHED,REVISION,BY_MONTH,FUCKS_BY_DATE };
    }

    class Reports
    {
        MySqlConnection sql = null;
        private DateTime FROM = DateTime.Now;
        private DateTime TO = DateTime.Now;
        private String DFROM = "NOW()";
        private String DTO = "NOW()";
        public Reports(MySqlConnection sql)
        {
            this.sql = sql;
        }

        private XmlDocument makeStdReportXml(String query)
        {
            MySqlCommand cmd = new MySqlCommand(query, sql);
            MySqlDataReader rd=cmd.ExecuteReader();
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
                        vl = Buildings.fullPlaceName(vl, true, false, false);
                    }
                    XmlElement f=xml.CreateElement(nm);
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
            XmlDocument dok_out = new XmlDocument();
            dok_out.AppendChild(dok_out.CreateElement("Rows"));
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
                    XmlElement el = (XmlElement)dok_out.DocumentElement.AppendChild(dok_out.CreateElement("Row"));
                    el.AppendChild(dok_out.CreateElement("name")).AppendChild(dok_out.CreateTextNode(name));
                    el.AppendChild(dok_out.CreateElement("dt")).AppendChild(dok_out.CreateTextNode(dt));
                    el.AppendChild(dok_out.CreateElement("state")).AppendChild(dok_out.CreateTextNode(state.ToString()));

                    name = nd.FirstChild.FirstChild.Value;
                    dt = nd.FirstChild.NextSibling.FirstChild.Value;
                    state = nd.FirstChild.NextSibling.NextSibling.FirstChild.Value == "п" ? state = 0 : state = int.Parse(nd.FirstChild.NextSibling.NextSibling.FirstChild.Value);
                }               
            }
            XmlElement el2 = (XmlElement)dok_out.DocumentElement.AppendChild(dok_out.CreateElement("Row"));
            el2.AppendChild(dok_out.CreateElement("name")).AppendChild(dok_out.CreateTextNode(name));
            el2.AppendChild(dok_out.CreateElement("dt")).AppendChild(dok_out.CreateTextNode(dt));
            el2.AppendChild(dok_out.CreateElement("state")).AppendChild(dok_out.CreateTextNode(state.ToString()));
            return dok_out;
        }

        private XmlDocument UserOkrolRpt(String query)
        {
            XmlDocument doc = makeStdReportXml(query);
            XmlNodeList lst = doc.ChildNodes[0].ChildNodes;
            Dictionary<String,int> sums=new Dictionary<String,int>();
            Dictionary<String, int> cnts = new Dictionary<String, int>();
            foreach (XmlNode nd in lst)
            {
                String nm = nd.FirstChild.FirstChild.Value;
                String v=nd.FirstChild.NextSibling.NextSibling.FirstChild.Value;
                int s=0;
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
                rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(k));
                rw.AppendChild(doc.CreateElement("dt")).AppendChild(doc.CreateTextNode("C"));
                rw.AppendChild(doc.CreateElement("state")).AppendChild(doc.CreateTextNode(sums[k].ToString()));
            }
            foreach (String k in cnts.Keys)
            {
                XmlElement rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(k));
                rw.AppendChild(doc.CreateElement("dt")).AppendChild(doc.CreateTextNode("К"));
                rw.AppendChild(doc.CreateElement("state")).AppendChild(doc.CreateTextNode(cnts[k].ToString()));
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
                rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode("итого"));
                rw.AppendChild(doc.CreateElement("dt")).AppendChild(doc.CreateTextNode(k));
                rw.AppendChild(doc.CreateElement("state")).AppendChild(doc.CreateTextNode(sums[k].ToString()));
            }
            return doc;
        }

        public XmlDocument makeReport(ReportType.Type type, Filters f)
        {
            String query="";
            switch (type)
            {
                case ReportType.Type.TEST:query=testQuery(f);break;
                case ReportType.Type.BREEDS: query = breedsQuery(f); break;
                case ReportType.Type.AGE: query = ageQuery(f); break;
                case ReportType.Type.FUCKER: query = fuckerQuery(f); break;
                case ReportType.Type.DEAD: query = deadQuery(f); break;
                case ReportType.Type.DEADREASONS: query = deadReasonsQuery(f); break;
                case ReportType.Type.REALIZE: query = Realize(f); break;
                case ReportType.Type.USER_OKROLS: return UserOkrolRpt(UserOkrols(f));
                case ReportType.Type.SHED: return ShedReport(f);
                case ReportType.Type.REVISION: return Revision(f);
                case ReportType.Type.BY_MONTH: query = rabByMonth(); break;
                case ReportType.Type.FUCKS_BY_DATE: query=fucksByDate(f); break;
            }
            return makeStdReportXml(query);
        }

        public static XmlDocument makeReport(MySqlConnection sql,ReportType.Type type,Filters f)
        {
            return new Reports(sql).makeReport(type, f);
        }

        private string testQuery(Filters f)
        {
            return "SELECT rabname(r_id,2) f1,(TO_DAYS(NOW())-TO_DAYS(r_born)) f2 FROM rabbits LIMIT 100;";
        }

        private string breedsQuery(Filters f)
        {
            return String.Format(@"SELECT r_breed br,(SELECT b_name FROM breeds WHERE b_id=br) breed,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='male' AND r_status=2 AND r_breed=br) fuck,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='male' AND r_status=1 AND r_breed=br) kandidat,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='male' AND r_status=0 AND r_breed=br) boys,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status>=2 AND r_breed=br) state,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND ((r_status=0 AND r_event_date IS NOT NULL)OR(r_status=1 AND r_event_date IS NULL)) AND r_breed=br) pervo,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND (r_status=0 AND r_event_date IS NULL) AND r_breed=br AND r_born<=(now()-INTERVAL {0:s} day)) nevest,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=0 AND r_breed=br and r_born>(now()-INTERVAL {0:s} day)) girl,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='void' AND r_breed=br) bezpolie,
sum(r_group) vsego
FROM rabbits GROUP BY r_breed
union

select 'Итого','',(SELECT count(*) FROM rabbits WHERE r_sex='male' AND r_status=2),
(SELECT count(*) FROM rabbits WHERE r_sex='male' AND r_status=1) kandidat,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='male' AND r_status=0) boys,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status>=2 ) state,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND ((r_status=0 AND r_event_date IS NOT NULL)OR(r_status=1 AND r_event_date IS NULL))) pervo,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND (r_status=0 AND r_event_date IS NULL) AND r_born<=(now()-INTERVAL {0:s} day)) nevest,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=0 and r_born>(now()-INTERVAL {0:s} day)) girl,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='void') bezpolie,
sum(r_group) vsego
from rabbits;", f.safeValue("brd","121"));
        }

        private string ageQuery(Filters f)
        {
            return "SELECT (TO_DAYS(NOW())-TO_DAYS(r_born)) age,sum(r_group) cnt FROM rabbits GROUP BY age;";
        }

        private DateTime[] getDates(Filters f)
        {
            DateTime dfrom = DateTime.Parse(f.safeValue("dfr", DateTime.Now.ToShortDateString()));
            DateTime dto = DateTime.Parse(f.safeValue("dto", DateTime.Now.ToShortDateString()));
            FROM=dfrom;
            TO=dto;
            DFROM=D(FROM);
            DTO=D(TO);
            return new DateTime[] { dfrom, dto };
        }

        public String D(DateTime date)
        {
            return DBHelper.DateToMyString(date);
        }

        private string fuckerQuery(Filters f)
        {
            int partner = f.safeInt("prt");
            getDates(f);
            return String.Format(@"(SELECT rabname(f_rabid,2) name,f_children,
IF(f_type='vyazka','Вязка','случка') type,
IF(f_state='proholost','Прохолостание','Окрол') state,
DATE_FORMAT(f_date,'%d.%m.%Y') start,DATE_FORMAT(f_end_date,'%d.%m.%Y') stop 
FROM fucks WHERE f_partner={0:d} AND f_end_date>={1:s} AND f_end_date<={2:s})
union
(SELECT 'Итого:',sum(f_children),'','','','' 
FROM fucks WHERE f_partner={0:d} AND f_end_date>={1:s} AND f_end_date<={2:s}
);",
              partner,DFROM,DTO);
        }

        private String deadQuery(Filters f)
        {
            getDates(f);
            return String.Format(@"(SELECT DATE_FORMAT(d_date,'%d.%m.%Y') date,deadname(r_id,2) name,r_group,
(SELECT d_name FROM deadreasons WHERE d_id=d_reason) reason,d_notes FROM dead WHERE 
d_date>={0:s} AND d_date<={1:s} ORDER BY d_date ASC)
UNION ALL (SELECT 'Итого:','',SUM(r_group),'','' FROM dead WHERE 
d_date>={0:s} AND d_date<={1:s});", DFROM, DTO);
        }

        private String deadReasonsQuery(Filters f)
        {
            getDates(f);
            return String.Format(@"(SELECT SUM(r_group) grp,d_reason,
(SELECT d_name FROM deadreasons WHERE d_reason=d_id) reason FROM dead WHERE
d_date>={0:s} AND d_date<={1:s} GROUP BY d_reason)
UNION 
(SELECT SUM(r_group),0,'Итого' FROM dead WHERE d_date>={0:s} AND d_date<={1:s});", DFROM, DTO);
        }

        private String Realize(Filters f)
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

        private String UserOkrols(Filters f)
        {
            getDates(f);
            int user = f.safeInt("user");
            return String.Format(@"SELECT CONCAT(' ',rabname(f_partner,0)) name,DATE_FORMAT(f_end_date,'%d.%m.%Y') dt,
IF (f_state='okrol',f_children,IF(f_state='proholost','п','-')) state 
FROM fucks WHERE f_worker={2:d}
AND f_end_date>={0:s} AND f_end_date<={1:s} ORDER BY name,dt;", DFROM, DTO, user);
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
(t_id=m_upper OR t_id=m_lower) AND inBuilding({0:d},m_id){1:s};",bid,
                                                                type==""?"":" AND t_type='"+type+"'"));
        }

        private int round(double d)
        {
            return (int)Math.Round(d);
        }

        private void addShedRows(XmlDocument doc,String type, int ideal, int real)
        {
            XmlElement rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
            rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(type));
            rw.AppendChild(doc.CreateElement("type")).AppendChild(doc.CreateTextNode("идеал"));
            rw.AppendChild(doc.CreateElement("value")).AppendChild(doc.CreateTextNode(ideal.ToString()));
            rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
            rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(type));
            rw.AppendChild(doc.CreateElement("type")).AppendChild(doc.CreateTextNode("реально"));
            rw.AppendChild(doc.CreateElement("value")).AppendChild(doc.CreateTextNode(real.ToString()));
        }

        private XmlDocument ShedReport(Filters f)
        {
            double per_vertep = 3.2;
            double per_female = 6;              
            double pregn_per_tier = 0.3114;     
            double feed_girls_per_tier = 0.6;   
            double feed_boys_per_tier = 2.0;    
            double unkn_sucks_per_tier = 2.7;
            int bid = f.safeInt("bld");
            int suck = f.safeInt("suck", 50);
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Rows"));
            int alltiers = getBuildCount("", bid);
            int fem = getBuildCount("female", bid);
            int dfe = getBuildCount("dfemale", bid);
            int com = getBuildCount("complex", bid);
            int jur = getBuildCount("jurta", bid);
            int qua = getBuildCount("quarta", bid);
            int ver = getBuildCount("vertep", bid);
            int bar = getBuildCount("barin", bid);
            int cab = getBuildCount("cabin", bid);
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
AND inBuilding({0:d},(SELECT r2.r_farm FROM rabbits r2 WHERE r2.r_id=rabbits.r_parent));", bid, suck));
            addShedRows(doc, " подсосные", ideal, real);
            real = getInt32(String.Format(@"SELECT COALESCE(SUM(r_group),0) FROM rabbits WHERE r_parent<>0 AND TO_DAYS(NOW())-TO_DAYS(r_born)<{1:d} 
AND inBuilding({0:d},(SELECT r2.r_farm FROM rabbits r2 WHERE r2.r_id=rabbits.r_parent));", bid, suck));
            ideal = real;
            addShedRows(doc, "гнездовые", ideal, real);
            return doc;
        }

        private XmlDocument Revision(Filters f)
        {
            int bld = f.safeInt("bld");
            XmlDocument doc=new XmlDocument();
            doc.AppendChild(doc.CreateElement("Rows"));
            MySqlCommand cmd=new MySqlCommand(String.Format(@"SELECT m_id,t_type,t_busy1,t_busy2,t_busy3,t_busy4 
FROM tiers,minifarms WHERE (t_busy1=0 OR t_busy2=0 OR t_busy3=0 OR t_busy4=0) AND (t_id=m_upper OR t_id=m_lower) AND inBuilding({0:d},m_id);",bld),sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
                for (int i = 0; i < Buildings.getRSecCount(rd.GetString(1)); i++)
                    if (rd.GetInt32(i + 2) == 0)
                    {
                        doc.DocumentElement.AppendChild(doc.CreateElement("Row")).AppendChild(
                            doc.CreateElement("address")).AppendChild(doc.CreateTextNode(rd.GetString(0)+Buildings.getRSec(rd.GetString(1),i,"000")));
                    }
            rd.Close();
            return doc;
        }

        private string rabByMonth()
        {
            //return "SELECT DATE_FORMAT(r_born,'%m.%Y') date, sum(r_group) count FROM rabbits GROUP BY date ORDER BY year(r_born) desc,month(r_born) desc;";
            return @"SELECT
                        DATE_FORMAT(r_born,'%m.%Y') date,
                        sum(r_group) count,
                        (SELECT SUM(r_group) FROM dead d WHERE MONTH(d.r_born)=MONTH(rabbits.r_born) AND YEAR(d.r_born)=YEAR(rabbits.r_born)) killed
                        FROM rabbits GROUP BY date ORDER BY year(r_born) desc,month(r_born) desc;";
        }

        private string fucksByDate(Filters f)
        {
            return String.Format(@"SELECT DATE_FORMAT(f_date,'%d.%m.%Y')date,anyname(f_rabid,2) name,
(SELECT n_name FROM names WHERE n_use=f_partner) partner,
(SELECT u_name FROM users WHERE u_id=f_worker) worker 
FROM fucks WHERE f_date is not null ORDER BY f_date DESC, f_worker;",DFROM,DTO);
        }
    }
}
