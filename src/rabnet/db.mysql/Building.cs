using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    class Building:IBuilding
    {
        int sid;
        string sname;
        string stype;
        public Building(int id,string name,string type)
        {
            sid = id;
            sname = name;
            stype = type;
        }
        public int id() { return sid; }
        public string name() { return sname; }
        public string type() { return stype; }
    }

    class Buildings : RabNetDataGetterBase
    {
        public Buildings(MySqlConnection sql, String filters)
            : base(sql)
        {
        }

        public override IData nextItem()
        {
            return new Building(rd.GetInt32(0), rd.GetString(1), rd.GetString(2));
        }

        public override string getQuery()
        {
            return "SELECT m_id,t_id,t_type FROM minifarms,tiers WHERE m_upper=t_id OR m_lower=t_id";
        }

        public override string countQuery()
        {
            return "SELECT COUNT(t_id) FROM minifarms,tiers WHERE m_upper=t_id OR m_lower=t_id;";
        }
    }
}
