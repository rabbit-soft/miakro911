using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public class Catalog : Dictionary<int, string> { }

    public interface ICatalog
    {
        CatalogData Get();
        void Change(int id, params string[] args);
        int Add(params string[] args);
    }

    /// <summary>
    /// Класс для заполнения справочников в CatalogForm
    /// </summary>
    public class CatalogData
    {
        public const string IMAGE_MARKER = "#image#";
        public const string COLOR_MARKER = "#color#";
        public const string BOOL_MARKER = "#bool#";

        /// <summary>
        /// Одна строка DataGridView. Имеет, ID и массив значений ячеек
        /// </summary>
        public struct Row
        {
            public int key;
            public String[] data;
            //public byte[] image;
            //public int imageSize;
        }
        /// <summary>
        /// Массив имен столбцов DataGridView
        /// </summary>
        public String[] ColNames;
        /// <summary>
        /// Массив строк DataGridView
        /// </summary>
        public Row[] Rows;
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
            MySqlCommand cmd = new MySqlCommand("UPDATE names SET n_block_date=NULL WHERE n_use=0 AND n_block_date<NOW();", con);
            cmd.ExecuteNonQuery();
            if (sex==0) sex=1;
            String where = "";
            if (sex == 1)
                where = " WHERE n_sex='male'";
            if (sex == 2)
                where = " WHERE n_sex='female'";
            return stdCatalog("SELECT n_id,n_name FROM names" + where + " AND (n_id=" + plusid.ToString() + " OR (n_use=0 AND n_block_date IS NULL)) ORDER BY n_name;");
        }

        public Catalog getDeadReasons()
        {
            return stdCatalog("SELECT d_id,d_name FROM deadreasons WHERE d_id>2 ORDER BY d_id ASC;");
        }

    }
}
