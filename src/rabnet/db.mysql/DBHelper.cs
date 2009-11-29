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

        public static String get_genesis(MySqlConnection sql,int rabid)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ') FROM genoms WHERE g_id=(SELECT r_genesis FROM rabbits WHERE r_id="+rabid.ToString()+");", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            String res = "";
            if (rd.Read())
                res = rd.GetString(0);
            rd.Close();
            return res;
        }

        public static String combine_genesis(String g1, String g2)
        {
            String res = "";
            string[] s1 = g1.Split(' ');
            string[] s2 = g2.Split(' ');
            List<int> gn = new List<int>();
            foreach (string s in s1)
            {
                int g=int.Parse(s);int pos=0;
                for (int i=0;i<gn.Count && pos>-1;i++){if (g == gn[i])pos = -1;if (g < gn[i])pos++;}
                if (pos > -1) gn.Insert(pos, g);
            }
            foreach (string s in s2)
            {
                int g = int.Parse(s); int pos = 0;
                for (int i = 0; i < gn.Count && pos > -1; i++) { if (g == gn[i])pos = -1; if (g < gn[i])pos++; }
                if (pos > -1) gn.Insert(pos, g);
            }
            foreach (int i in gn)
                res += i.ToString() + " ";
            return res.Trim();
        }
        public static String combine_genesis(MySqlConnection sql,int r1, int r2)
        {
            return combine_genesis(get_genesis(sql,r1),get_genesis(sql,r2));
        }
        public static int makeCommonGenesis(MySqlConnection sql, int r1, int r2)
        {
            return makeGenesis(sql, combine_genesis(sql, r1, r2));
        }
        public static int makeCommonGenesis(MySqlConnection sql, String g1, String g2)
        {
            return makeGenesis(sql, combine_genesis(g1,g2));
        }
        public static String commonBon(String b1, String b2)
        {
            string res = "0";
            for (int i = 1; i < 5; i++)
                res+= b1[i] < b2[i] ? b1[i] : b2[i];
            return res;
        }
    }
}
