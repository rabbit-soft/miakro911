using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public interface IZones
    {
        CatalogData getZones();
        void ChangeZone(int id, String name, String sname);
        int AddZone(int id,String name, String sname);
    }

    public class Zones : IZones
    {
        private MySqlConnection sql = null;
        public Zones(MySqlConnection sql)
        {
            this.sql = sql;
        }
        public CatalogData getZones()
        {
            CatalogData cd = new CatalogData();
            cd.colnames = new String[] { "ген","зона", "сокращение" };
            MySqlCommand cmd = new MySqlCommand("SELECT z_id,z_name,z_short_name FROM zones ORDER BY z_name;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<CatalogData.Row> rws = new List<CatalogData.Row>();
            while (rd.Read())
            {
                CatalogData.Row rw = new CatalogData.Row();
                rw.key = rd.GetInt32(0);
                rw.data = new String[] { rd.GetString(0),rd.GetString(1), rd.GetString(2) };
                rws.Add(rw);
            }
            rd.Close();
            cd.data = rws.ToArray();
            return cd;
        }

        public void ChangeZone(int id, String name, String sname)
        {
            if (id == 0)
                return;
            MySqlCommand cmd = new MySqlCommand("UPDATE zones SET z_name='" + name + "',z_short_name='" +
                sname + "' WHERE z_id='" + id.ToString() + "';", sql);
            cmd.ExecuteNonQuery();
        }

        public int AddZone(int id,String name, String sname)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO zones(z_name,z_short_name,z_id) VALUES('" +
                name + "','" + sname + "',"+id.ToString()+");", sql);
            cmd.ExecuteNonQuery();
            return id;
        }

    }
}
