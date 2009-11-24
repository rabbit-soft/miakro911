using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public class Breeds
    {
        private MySqlConnection sql = null;
        public Breeds(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public CatalogData getBreeds()
        {
            CatalogData cd = new CatalogData();
            cd.colnames = new String[] {"порода","сокращение" };
            MySqlCommand cmd = new MySqlCommand("SELECT b_id,b_name,b_short_name FROM breeds ORDER BY b_id;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<CatalogData.Row> rws = new List<CatalogData.Row>();
            while (rd.Read())
            {
                CatalogData.Row rw = new CatalogData.Row();
                rw.key=rd.GetInt32(0);
                rw.data = new String[] {rd.GetString(1),rd.GetString(2)};
                rws.Add(rw);
            }
            rd.Close();
            cd.data = rws.ToArray();
            return cd;
        }

        public void ChangeBreed(int id,String name,String sname)
        {
            if (id==0)
                return;
            MySqlCommand cmd = new MySqlCommand("UPDATE breeds SET b_name='"+name+"',b_short_name='"+
                sname+"' WHERE b_id='"+id.ToString()+"';", sql);
            cmd.ExecuteNonQuery();
        }

        public int AddBreed(String name, String sname)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO breeds(b_name,b_short_name) VALUES('"+
                name+"','"+sname+"');", sql);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }
    }
}
