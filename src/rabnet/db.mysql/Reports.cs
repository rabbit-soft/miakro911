using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Xml;

namespace rabnet
{
    public class ReportType
    {
        public enum Type { TEST,BREEDS,AGE,FUCKER,DEAD,DEADREASONS,REALIZE };
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
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=1 AND r_breed=br) pervo,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=0 AND r_breed=br AND r_born<=(now()-INTERVAL {0:s} day)) nevest,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=0 AND r_breed=br and r_born>(now()-INTERVAL {0:s} day)) girl,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='void' AND r_breed=br) bezpolie,
sum(r_group) vsego
FROM rabbits GROUP BY r_breed
union

select 'Итого','',(SELECT count(*) FROM rabbits WHERE r_sex='male' AND r_status=2),
(SELECT count(*) FROM rabbits WHERE r_sex='male' AND r_status=1) kandidat,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='male' AND r_status=0) boys,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status>=2 ) state,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=1 ) pervo,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=0 AND r_born<=(now()-INTERVAL {0:s} day)) nevest,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=0 and r_born>(now()-INTERVAL {0:s} day)) girl,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='void') bezpolie,
sum(r_group) vsego
from rabbits;",f.safeValue("brd","121"));
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
IF(r_sex='male','м',IF(r_sex='female','ж','?')) sex,r_notes comment FROM rabbits WHERE {0:s};",where);
        }
    }
}
