using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{

    public class Name : IData
    {
        public int id;
        public string name;
        public string surname;
        public int use;
        public DateTime td;
        public String sex;
        public Name(int id, String name, String surname, String sex, int use, DateTime dt)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            //this.sex = sex;
            if (sex == "male")
                this.sex = "м";
            else
                this.sex = "ж";
            this.use = use;
            this.td = dt;
        }
    }

    class Names:RabNetDataGetterBase
    {

        public Names(MySqlConnection sql, Filters opts) : base(sql, opts) { }

        public override IData nextItem()
        {
           return new Name(rd.GetInt32("n_id"),rd.GetString("n_name"),rd.GetString("n_surname"),
               rd.GetString("n_sex"),rd.GetInt32("n_use"),rd.IsDBNull(5)?DateTime.MinValue:rd.GetDateTime("n_block_date"));
        }

        private String makeWhereClause()
        {
            String w = "";
            if (options.safeInt("sex") != 0)
                w = "n_sex='" + (options.safeInt("sex")==1?"male":"female") + "'";
            if (options.safeInt("state") != 0)
                w = Rabbits.addWhereAnd(w, "n_use" + (options.safeInt("state")==1?"":"!") + "=0");
            if (options.safeValue("name") != "")
                w = Rabbits.addWhereAnd(w, "n_name like '"+options.safeValue("name")+"%'");
            if (w != "")
                w = " WHERE " + w + " ";
            return w;
        }

        public override string getQuery()
        {
            return "SELECT n_id,n_sex,n_name,n_surname,n_use,n_block_date FROM names "+makeWhereClause()+"ORDER BY n_name;";
        }

        public override string countQuery()
        {
            return "SELECT COUNT(*) FROM names"+makeWhereClause()+";";
        }

    }
}
