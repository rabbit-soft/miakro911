using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    public class Vaccines:ICatalog
    {
        private MySqlConnection _sql = null;

        public Vaccines(MySqlConnection sql)
        {
            this._sql = sql;
        }

        public CatalogData Get()
        {
            CatalogData cd = new CatalogData();
            cd.ColNames = new String[] { "Название", "Продолжительность (дней)", CatalogData.BOOL_MARKER+"Назначать в ЗооТехПлане" };
            MySqlCommand cmd = new MySqlCommand("SELECT v_id,v_name,v_duration,v_zootech FROM vaccines;", _sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<CatalogData.Row> rws = new List<CatalogData.Row>();
            while (rd.Read())
            {
                CatalogData.Row rw = new CatalogData.Row();
                rw.key = rd.GetInt32(0);
                rw.data = new String[] { rd.GetString(1), rd.GetString(2), rd.GetString(3) };
                rws.Add(rw);
            }
            rd.Close();
            cd.Rows = rws.ToArray();
            return cd;
        }

        public void Change(int id, params string[] args)
        {
            if (args.Length != 3) throw new Exception("incorrect parms count (" + args.Length + ") expected: 3");
            if (id == 0) return;
            int duration = 0;
            int.TryParse(args[1], out duration);
            bool zoo = false;
            bool.TryParse(args[2],out zoo);
            MySqlCommand cmd = new MySqlCommand(
                String.Format("UPDATE vaccines SET v_name='{0:s}',v_duration={1:d}, v_zootech={2} WHERE v_id={3:d};", args[0], duration, zoo, id), _sql);
            cmd.ExecuteNonQuery();
        }

        public int Add(params string[] args)
        {
            if (args.Length != 3) throw new Exception("incorrect parms count (" + args.Length + ") expected: 3");
            int duration = 0;
            int.TryParse(args[1],out duration);
            bool zoo = false;
            bool.TryParse(args[2],out zoo);
            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO vaccines(v_name,v_duration,v_zootech) VALUES('{0:s}',{1:d},{2});",
                args[0], duration, zoo), _sql);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }
    }   
}
