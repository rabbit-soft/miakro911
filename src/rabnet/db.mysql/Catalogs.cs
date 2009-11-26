using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public interface ICatalogs
    {
        Catalog stdCatalog(String data);
        Catalog getBreeds();
        Catalog getNames(int sex);
        Catalog getSurNames(int sex, String ends);
    }

    public class Catalogs:ICatalogs
    {
        private MySqlConnection con;
        public Catalogs(MySqlConnection sql)
        {
            con = sql;
        }
        public Catalog stdCatalog(String query)
        {
            MySqlCommand cmd = new MySqlCommand(query, con);
            MySqlDataReader rd = cmd.ExecuteReader();
            Catalog res = new Catalog();
            while (rd.Read())
            {
                res[rd.GetInt32(0)] = rd.GetString(1);
            }
            rd.Close();
            return res;
        }
        public Catalog getBreeds()
        {
            return stdCatalog("SELECT b_id,b_name FROM breeds ORDER BY b_name;");
        }
        public Catalog getNames(int sex)
        {
            String where="";
            if (sex == 1)
                where = " WHERE n_sex='male'";
            if (sex==2)
                where = " WHERE n_sex='female'";
            return stdCatalog("SELECT n_id,n_name FROM names"+where+" ORDER BY n_name;");
        }
        public Catalog getSurNames(int sex,String ends)
        {
            String where = "";
            if (sex == 1)
                where = " WHERE n_sex='male'";
            if (sex == 2)
                where = " WHERE n_sex='female'";
            return stdCatalog("SELECT n_id,n_surname+'" + ends + "' FROM names"+where+" ORDER BY n_surname;");
        }

    }
}
