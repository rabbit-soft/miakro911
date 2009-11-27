using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    class DBHelper
    {
        public static String DateToMyString(DateTime dt)
        {
            if (dt == DateTime.MinValue)
                return "NULL";
            return String.Format("'{0:D4}-{1:D2}-{2:D2}'",dt.Year,dt.Month,dt.Day);
        }

        public static int makeGenesis(MySqlConnection  sql,String gens)
        {
            MySqlCommand c = new MySqlCommand("", sql);
            c.CommandText = "SELECT g_id FROM genesis WHERE g_key=MD5('" + gens + "');";
            MySqlDataReader rd = c.ExecuteReader();
            int res = 0;
            if (rd.HasRows)
            {
                rd.Read();
                res = rd.GetInt32(0);
                rd.Close();
            }
            else
            {
                rd.Close();
                c.CommandText = "INSERT INTO genesis(g_notes) VALUES('');";
                c.ExecuteNonQuery();
                res = (int)c.LastInsertedId;
                String[] gen = gens.Split(' ');
                foreach (string g in gen)
                {
                    c.CommandText = "INSERT INTO genoms(g_id,g_genom) VALUES(" + res.ToString() + "," + g + ");";
                    c.ExecuteNonQuery();
                }
                c.CommandText = "UPDATE genesis SET g_key=(SELECT MD5(GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ')) FROM genoms WHERE g_id=" + res.ToString() + ") WHERE g_id=" + res.ToString() + ";";
                c.ExecuteNonQuery();
            }
            return res;
        }
    }
}
