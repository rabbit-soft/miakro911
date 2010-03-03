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

        public Names(MySqlConnection sql, Filters opts) : base(sql, opts) 
        {
        }

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
            if (options.ContainsKey("state"))
            if (options.safeInt("state") == 1)
                w = Rabbits.addWhereAnd(w, "(n_use=0 AND n_block_date IS NULL)");
            else
                w = Rabbits.addWhereAnd(w, "(n_use<>0 OR n_block_date IS NOT NULL)");

         // if (options.safeValue("name") != "")
         //   w = Rabbits.addWhereAnd(w, "n_name like '"+options.safeValue("name")+"%'");
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
            MySqlCommand cmd = new MySqlCommand("UPDATE names SET n_block_date=NULL WHERE n_use=0 AND n_block_date<NOW();", sql);
            cmd.ExecuteNonQuery();
            return "SELECT COUNT(*) FROM names" + makeWhereClause() + ";";
        }

        public static void addName(MySqlConnection sql, OneRabbit.RabbitSex sex, String name, String surname)
        {
            MySqlCommand cmd=new MySqlCommand(String.Format(@"INSERT INTO names(n_sex,n_name,n_surname,n_block_date) 
VALUES('{0:s}','{1:s}','{2:s}',NULL)",(sex==OneRabbit.RabbitSex.FEMALE)?"female":"male",name,surname),sql);
            cmd.ExecuteNonQuery();
        }
        public static void changeName(MySqlConnection sql, string orgName, string name, string surname)
        {
            MySqlCommand cmd=new MySqlCommand(String.Format(@"UPDATE names SET n_name='{0:s}',n_surname='{1:s}' 
WHERE n_name='{2:s}';",name,surname,orgName),sql);
            cmd.ExecuteNonQuery();
        }

    }
}
