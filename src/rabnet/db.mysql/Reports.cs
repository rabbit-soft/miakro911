using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Xml;

namespace rabnet
{
    public class ReportType
    {
        public enum Type { TEST };
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
                    f.AppendChild(xml.CreateTextNode(rd.GetString(i)));
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
    }
}
