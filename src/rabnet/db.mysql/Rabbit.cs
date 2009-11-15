using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    class Rabbit:IRabbit
    {
        private int id;
        private string[] dt = { "", "", "", "" };
        public Rabbit(int id,string name,string surname,string secname,string sex)
        {
            this.id = id;
            dt[0] = name;
            dt[1] = surname;
            dt[2] = secname;
            dt[3] = sex;

        }
        int IRabbit.id(){return id;}
        public string name(){return dt[0];}
        public string surname(){return dt[1];}
        public string secname(){return dt[2];}
        public string sex(){return dt[3];}
    }

    class Rabbits:RabNetDataGetterBase
    {
        public Rabbits(MySqlConnection sql,String prms):base(sql)
        {
        }
        public override IData nextItem()
        {
            return new Rabbit(rd.GetInt32(0),rd.GetString(1),rd.GetString(2),rd.GetString(3),rd.GetString(4));
            
        }
        public override string getQuery()
        {
            return String.Format("SELECT r_id,r_name,r_surname,r_secname,r_sex FROM rabbits WHERE r_parent=0;");
        }
        public override string countQuery()
        {
            return "SELECT COUNT(*) FROM rabbits WHERE r_parent=0;";
        }
    }
}
