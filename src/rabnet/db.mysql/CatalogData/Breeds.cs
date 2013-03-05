using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    /*public interface IBreeds
    {
        CatalogData getBreeds();
        void ChangeBreed(int id,String name,String sname,String color);
		int AddBreed(String name, String sname, String color);
    }*/

    class Breeds:ICatalog //:IBreeds
    {
        private MySqlConnection sql = null;
        public Breeds(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public CatalogData Get()
        {
            CatalogData cd = new CatalogData();
            cd.ColNames = new String[] {"порода","сокращение",CatalogData.COLOR_MARKER+"Цвет" };
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
            cd.Rows = rws.ToArray();
            return cd;
        }

        public void Change(int id, params String[] args)
        {
            if (args.Length != 3)  throw new Exception("incorrect parms count (" + args.Length+") expected: 3");

            if (id==0)
                return;
            MySqlCommand cmd = new MySqlCommand(
                String.Format("UPDATE breeds SET b_name='{0:s}',b_short_name='{1:s}', b_color='{2:s}' WHERE b_id={3:d};", args[0], args[1], args[2], id), sql);
            cmd.ExecuteNonQuery();
        }

        public int Add(params String[] args)
        {
            if (args.Length != 3) throw new Exception("incorrect parms count (" + args.Length + ") expected: 3");

            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO breeds(b_name,b_short_name,b_color) VALUES('{0:s}','{1:s}','{2:s}');",
                args[0],args[1],args[2]), sql);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }

        internal static BreedsList GetBreeds(MySqlConnection sql)
        {
            BreedsList result = new BreedsList();
            MySqlCommand cmd = new MySqlCommand("SELECT b_id,b_name,b_short_name FROM breeds;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                result.Add(new Breed(rd.GetInt32("b_id"), rd.GetString("b_name"), rd.GetString("b_short_name")));
            }
            rd.Close();

            return result;
        }

        internal static int AddBreed(MySqlConnection sql,string name,string shrt,string color)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO breeds(b_name,b_short_name,b_color) VALUES('{0:s}','{1:s}','{2:s}');",name,shrt,color), sql);
            cmd.ExecuteNonQuery();

            return (int)cmd.LastInsertedId;
        }
    }
}
