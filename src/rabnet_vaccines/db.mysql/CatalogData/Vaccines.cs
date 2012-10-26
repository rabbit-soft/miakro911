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
            cd.ColNames = new String[] { "№", "Название", "Продолжительность (дней)", "Назначать с...(дней)", CatalogData.BOOL_MARKER + "Назначать в ЗооТехПлане" };
            MySqlCommand cmd = new MySqlCommand("SELECT v_id,v_name,v_duration,v_age,v_zootech FROM vaccines;", _sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<CatalogData.Row> rws = new List<CatalogData.Row>();
            while (rd.Read())
            {
                CatalogData.Row rw = new CatalogData.Row();
                rw.key = rd.GetInt32(0);
                rw.data = new String[] { rd.GetString(0), rd.GetString(1), rd.GetString(2), rd.GetString(3), rd.GetString(4) };
                rws.Add(rw);
            }
            rd.Close();
            cd.Rows = rws.ToArray();
            return cd;
        }

        public void Change(int id, params string[] args)
        {
            if (args.Length != 4) throw new Exception("incorrect parms count (" + args.Length + ") expected: 4");
            if (id == 0) return;
            int duration = 0,age=0;
            int.TryParse(args[1], out duration);
            int.TryParse(args[2], out age);
            bool zoo = false;
            bool.TryParse(args[3],out zoo);
            MySqlCommand cmd = new MySqlCommand(
                String.Format("UPDATE vaccines SET v_name='{0:s}',v_duration={1:d},v_age={2:d}, v_zootech={3}  WHERE v_id={4:d};", args[0], duration,age, zoo, id), _sql);
            cmd.ExecuteNonQuery();
        }

        public int Add(params string[] args)
        {
            if (args.Length != 4) throw new Exception("incorrect parms count (" + args.Length + ") expected: 4");
            int duration = 0,age=0;
            int.TryParse(args[1],out duration);
            int.TryParse(args[2], out age);
            bool zoo = false;
            bool.TryParse(args[3],out zoo);
            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO vaccines(v_name,v_duration,v_age,v_zootech) VALUES('{0:s}',{1:d},{2:d},{3});",
                args[0], duration,age, zoo), _sql);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }
    }   
}
