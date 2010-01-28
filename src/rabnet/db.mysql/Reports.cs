using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Xml;

namespace rabnet
{
    public class ReportType
    {
        public enum Type { TEST,BREEDS,AGE,FUCKER };
    }

    public class Reports
    {
        MySqlConnection sql = null;
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
                    XmlElement f=xml.CreateElement(rd.GetName(i));
                    f.AppendChild(xml.CreateTextNode(rd.IsDBNull(i) ? "" : rd.GetString(i)));
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

        private string fuckerQuery(Filters f)
        {
            int partner = f.safeInt("prt");
            DateTime from=DateTime.Parse(f.safeValue("dfr",DateTime.Now.ToShortDateString()));
            DateTime to=DateTime.Parse(f.safeValue("dto",DateTime.Now.ToShortDateString()));
            return String.Format(@"(SELECT rabname(f_rabid,2) name,f_children,
IF(f_type='vyazka','Вязка','случка') type,
IF(f_state='proholost','Прохолостание','Окрол') state,
DATE_FORMAT(f_date,'%d.%m.%Y') start,DATE_FORMAT(f_end_date,'%d.%m.%Y') stop 
FROM fucks WHERE f_partner={0:d} AND f_end_date>={1:s} AND f_end_date<={2:s})
union
(SELECT 'Итого:',sum(f_children),'','','','' 
FROM fucks WHERE f_partner={0:d} AND f_end_date>={1:s} AND f_end_date<={2:s}
);",
              partner,DBHelper.DateToMyString(from),DBHelper.DateToMyString(to));
        }
    }
}
