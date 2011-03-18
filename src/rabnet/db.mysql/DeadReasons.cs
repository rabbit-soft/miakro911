using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public interface IDeadReasons
    {
        CatalogData getReasons();
        void ChangeReason(int id, String name);
        int AddReason(String name);
    }

    class DeadReasons : IDeadReasons
    {
        private MySqlConnection sql = null;
        public DeadReasons(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public CatalogData getReasons()
        {
            CatalogData cd = new CatalogData();
            cd.colnames = new String[] { "Причина"};
            MySqlCommand cmd = new MySqlCommand("SELECT d_id,d_name FROM deadreasons WHERE d_id>2 ORDER BY d_id;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<CatalogData.Row> rws = new List<CatalogData.Row>();
            while (rd.Read())
            {
                CatalogData.Row rw = new CatalogData.Row();
                rw.key = rd.GetInt32(0);
                rw.data = new String[] { rd.GetString(1) };
                rws.Add(rw);
            }
            rd.Close();
            cd.data = rws.ToArray();
            return cd;
        }

        public void ChangeReason(int id, String name)
        {
            if (id == 0) return;
            MySqlCommand cmd = new MySqlCommand("UPDATE deadreasons SET d_name='" + name + "' WHERE d_id='" + id.ToString() + "';", sql);
            cmd.ExecuteNonQuery();
        }

        public int AddReason(String name)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO deadreasons(d_name) VALUES('" +
                name + "');", sql);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }
    }
}
