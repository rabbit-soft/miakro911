using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using rabnet;

namespace db.mysql
{
    class Names:RabNetDataGetterBase
    {
        public Names(MySqlConnection sql, Filters opts) : base(sql, opts) { }

        public override IData NextItem()
        {
           return new RabName(_rd.GetInt32("n_id"),_rd.GetString("n_name"),_rd.GetString("n_surname"),
               _rd.GetString("n_sex"),_rd.GetInt32("n_use"),_rd.IsDBNull(5)?DateTime.MinValue:_rd.GetDateTime("n_block_date"));
        }

        private String makeWhereClause()
        {
            String w = "";
            if (options.safeInt("sex") != 0)
                w = "n_sex='" + (options.safeInt("sex")==1?"male":"female") + "'";
            if (options.ContainsKey("state"))
            if (options.safeInt("state") == 1)
                w = RabbitsDataGetter.addWhereAnd(w, "(n_use=0 AND n_block_date IS NULL)");
            else
                w = RabbitsDataGetter.addWhereAnd(w, "(n_use<>0 OR n_block_date IS NOT NULL)");

         // if (options.safeValue("name") != "")
         //   w = Rabbits.addWhereAnd(w, "n_name like '"+options.safeValue("name")+"%'");
            if (w != "")
                w = " WHERE " + w + " ";
            return w;
        }

        protected override string getQuery()
        {
            return "SELECT n_id,n_sex,n_name,n_surname,n_use,n_block_date FROM names "+makeWhereClause()+"ORDER BY n_name;";
        }

        protected override string countQuery()
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE names SET n_block_date=NULL WHERE n_use=0 AND n_block_date<NOW();", _sql);
            cmd.ExecuteNonQuery();
            return "SELECT COUNT(*) FROM names" + makeWhereClause() + ";";
        }

        public static int AddName(MySqlConnection sql, Rabbit.SexType sex, String name, String surname)
        {
            MySqlCommand cmd=new MySqlCommand(String.Format(@"INSERT INTO names(n_sex,n_name,n_surname,n_block_date) 
VALUES('{0:s}','{1:s}','{2:s}',NULL);", (sex==Rabbit.SexType.FEMALE)?"female":"male", name, surname),sql);
            cmd.ExecuteNonQuery();
            if (cmd.LastInsertedId > int.MaxValue) ///it can't be
                throw new RabNetException("ID нового имени больше максимально допустимого значения");
            return (int)cmd.LastInsertedId;
        }
        public static void changeName(MySqlConnection sql, string orgName, string name, string surname)
        {
            MySqlCommand cmd=new MySqlCommand(String.Format(@"UPDATE names SET n_name='{0:s}',n_surname='{1:s}' 
WHERE n_name='{2:s}';",name,surname,orgName),sql);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Пытается освободить имя
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Можно ли разблокировать имя</returns>
        public static bool unblockName(MySqlConnection sql, int id)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT COUNT(*) FROM rabbits WHERE r_name={0:d} OR r_surname={0:d} OR r_secname={0:d};",id), sql);
            _logger.Debug(cmd.CommandText);
            if (cmd.ExecuteScalar().ToString() != "0") {
                return false;
            }
            cmd.CommandText = String.Format("UPDATE names SET n_block_date=null WHERE n_id={0:d};",id);
            cmd.ExecuteNonQuery();
            return true;
        }


        internal static RabNamesList GetNames(MySqlConnection sql)
        {
            RabNamesList result = new RabNamesList();
            MySqlCommand cmd = new MySqlCommand("SELECT n_id,n_sex,n_name,n_surname,n_use,n_block_date FROM names ORDER BY n_name;",sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                result.Add(new RabName(rd.GetInt32("n_id"), rd.GetString("n_name"), rd.GetString("n_surname"),
                    rd.GetString("n_sex"), rd.GetInt32("n_use"), rd.IsDBNull(5) ? DateTime.MinValue : rd.GetDateTime("n_block_date")));
            }
            rd.Close();

            return result;
        }
    }
}
