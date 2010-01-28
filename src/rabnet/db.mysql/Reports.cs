﻿using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Xml;

namespace rabnet
{
    public class ReportType
    {
        public enum Type { TEST,BREEDS };
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
                case ReportType.Type.BREEDS: query = breedsQuery(f); break;
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
            return @"SELECT r_breed br,(SELECT b_name FROM breeds WHERE b_id=br) breed,
(SELECT count(*) FROM rabbits WHERE r_sex='male' AND r_status=2 AND r_breed=br) fuck,
(SELECT count(*) FROM rabbits WHERE r_sex='male' AND r_status=1 AND r_breed=br) kandidat,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='male' AND r_status=0 AND r_breed=br) boys,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status>=2 AND r_breed=br) state,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=1 AND r_breed=br) pervo,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=0 AND r_breed=br AND r_born<=(now()-INTERVAL 121 day)) nevest,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='female' AND r_status=0 AND r_breed=br and r_born>(now()-INTERVAL 121 day)) girl,
(SELECT sum(r_group) FROM rabbits WHERE r_sex='void' AND r_breed=br) bezpolie,
sum(r_group) vsego
FROM rabbits GROUP BY r_breed;";
        }
    }
}
