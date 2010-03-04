using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public interface IBreeds
    {
        CatalogData getBreeds();
        void ChangeBreed(int id,String name,String sname,String color);
		int AddBreed(String name, String sname, String color);
    }

    class Breeds:IBreeds
    {
        private MySqlConnection sql = null;
        public Breeds(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public CatalogData getBreeds()
        {
            CatalogData cd = new CatalogData();
            cd.colnames = new String[] {"порода","сокращение","#color#Цвет" };
            MySqlCommand cmd = new MySqlCommand("SELECT b_id,b_name,b_short_name,b_color FROM breeds ORDER BY b_id;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<CatalogData.Row> rws = new List<CatalogData.Row>();
            while (rd.Read())
            {
                CatalogData.Row rw = new CatalogData.Row();
                rw.key=rd.GetInt32(0);
				rw.data = new String[] { rd.GetString(1), rd.GetString(2), rd.GetString(3) };
                rws.Add(rw);
            }
            rd.Close();
            cd.data = rws.ToArray();
            return cd;
        }

        public void ChangeBreed(int id,String name,String sname,String color)
        {
            if (id==0)
                return;
            MySqlCommand cmd = new MySqlCommand("UPDATE breeds SET b_name='"+name+"',b_short_name='"+
                sname+"', b_color='"+color+"' WHERE b_id='"+id.ToString()+"';", sql);
            cmd.ExecuteNonQuery();
        }

        public int AddBreed(String name, String sname, String color)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO breeds(b_name,b_short_name,b_color) VALUES('"+
                name+"','"+sname+"','"+color+"');", sql);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }
    }
}
