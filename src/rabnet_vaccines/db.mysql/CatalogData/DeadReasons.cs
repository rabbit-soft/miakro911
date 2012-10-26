using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    /*public interface IDeadReasons
    {
        CatalogData getReasons();
        void ChangeReason(int id, String name);
        int AddReason(String name);
    }*/

    class DeadReasons:ICatalog //: IDeadReasons
    {
        private MySqlConnection sql = null;
        public DeadReasons(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public CatalogData Get()
        {
            CatalogData cd = new CatalogData();
            cd.ColNames = new String[] { "Причина"};
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
            cd.Rows = rws.ToArray();
            return cd;
        }

        public void Change(int id, params String[] args)
        {
            if (args.Length != 1) throw new Exception("incorrect parms count (" + args.Length + ") expected: 1");

            if (id == 0) return;
            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE deadreasons SET d_name='{0:s}' WHERE d_id={1:d};", args[0], id), sql);
            cmd.ExecuteNonQuery();
        }

        public int Add(params String[] args)
        {
            if (args.Length != 1) throw new Exception("incorrect parms count (" + args.Length + ") expected: 1");

            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO deadreasons(d_name) VALUES('{0:s}');", args[0]), sql);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }
    }
}
