using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public class Catalog : Dictionary<int, string>
    {

    }

    public interface ICatalogs
    {
        Catalog stdCatalog(String data);
        Catalog getBreeds();
        Catalog getNames(int sex);
        Catalog getSurNames(int sex, String ends);
        Catalog getZones();
        Catalog getFreeNames(int sex, int plusid);
        Catalog getDeadReasons();
    }

    class Catalogs:ICatalogs
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
            return stdCatalog("SELECT b_id,b_name FROM breeds;");
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
            return stdCatalog("SELECT n_id,CONCAT(n_surname,'" + ends + "') FROM names"+where+" ORDER BY n_surname;");
        }
        public Catalog getZones()
        {
            return stdCatalog("SELECT z_id,z_name FROM zones;");
        }

        public Catalog getFreeNames(int sex,int plusid)
        {
            if (sex==0) sex=1;
            String where = "";
            if (sex == 1)
                where = " WHERE n_sex='male'";
            if (sex == 2)
                where = " WHERE n_sex='female'";
            return stdCatalog("SELECT n_id,n_name FROM names" + where + " AND (n_id=" + plusid.ToString() + " OR (n_use=0 AND n_block_date IS NULL));");
        }

        public Catalog getDeadReasons()
        {
            return stdCatalog("SELECT d_id,d_name FROM deadreasons WHERE d_id>2 ORDER BY d_id ASC;");
        }

    }
}
