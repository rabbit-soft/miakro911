using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    /*public interface IZones
    {
        CatalogData getZones();
        void ChangeZone(int id, String name, String sname);
        int AddZone(int id,String name, String sname);
    }*/

    class Zones : ICatalog//IZones
    {
        private MySqlConnection sql = null;
        public Zones(MySqlConnection sql)
        {
            this.sql = sql;
        }
        public CatalogData Get()
        {
            CatalogData cd = new CatalogData();
            cd.ColNames = new String[] { "ген","зона", "сокращение" };
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
            cd.Rows = rws.ToArray();
            return cd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args">name,sname</param></param>
        /// <exception cref="Exception">Не верное кол-во параметров</exception>
        public void Change(int id, params String[] args)
        {
            if (args.Length != 3) throw new Exception("incorrect parms count (" + args.Length + ") expected: 2");
            //if (id == 0) return;
            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE zones SET z_id={0:d}, z_name='{1:s}',z_short_name='{2:s}' WHERE z_id={3:d};", args[0], args[1], args[2], id), sql);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args">id,name,sname</param>
        /// <exception cref="Exception">Не верное кол-во параметров</exception>
        public int Add(params String[] args)
        {
            if (args.Length != 3) throw new Exception("incorrect parms count (" + args.Length + ") expected: 3");

            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO zones(z_id,z_name,z_short_name) VALUES({0:s},'{1:s}','{2:s}');", args[0], args[1], args[2]), sql);
            cmd.ExecuteNonQuery();
            return int.Parse(args[0]);
        }

    }
}
