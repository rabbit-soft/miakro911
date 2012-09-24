using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace db.mysql
{
    class Weight
    {
        private MySqlConnection sql;
        public Weight(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public String[] getWeights(int rabbit)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT w_date,w_weight FROM weights 
WHERE w_rabid={0:d} ORDER BY w_date DESC;",rabbit), sql);
            List<String> res = new List<String>();
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                res.Add(rd.GetDateTime(0).ToShortDateString());
                res.Add(rd.GetString(1));
            }
            rd.Close();
            return res.ToArray();
        }

        public void addWeight(int rabbit,int weight,DateTime date)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"INSERT INTO weights(w_rabid,w_date,w_weight) 
VALUES({0:d},{1:s},{2:d});",rabbit,DBHelper.DateToMyString(date.Date),weight), sql);
            cmd.ExecuteNonQuery();
        }

        public void deleteWeight(int rabbit, DateTime date)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"DELETE FROM weights 
WHERE w_rabid={0:d} AND w_date={1:s}",rabbit,DBHelper.DateToMyString(date.Date)), sql);
            cmd.ExecuteNonQuery();
        }
    }
}
