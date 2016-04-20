#if !DEMO
using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using rabnet;

namespace db.mysql
{
    class Meal
    {
        public static List<sMeal> getMealPeriods(MySqlConnection sql)
        {
            List<sMeal> result = new List<sMeal>();
            MySqlCommand cmd = new MySqlCommand("SELECT m_start_date, m_end_date, m_amount, m_rate, m_type, m_id FROM meal ORDER BY m_start_date ASC;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read()) {
                if (!rd.IsDBNull(1) && !rd.IsDBNull(3)) {
                    result.Add(new sMeal(rd.GetInt32(5), rd.GetDateTime(0), rd.GetDateTime(1), rd.GetInt32(2), rd.GetFloat(3), rd.GetString(4)));
                } else {
                    result.Add(new sMeal(rd.GetInt32(5), rd.GetDateTime(0), rd.GetInt32(2), rd.GetString(4)));
                }
            }
            rd.Close();
            return result;
        }
        /// <summary>
        /// Добавляет привоза корма
        /// </summary>
        /// <param name="start">Дата завоза</param>
        /// <param name="amount">Объем корма(кг)</param>
        public static void AddMealIn(MySqlConnection sql, DateTime start, int amount)
        {
            MySqlCommand cmd = new MySqlCommand("", sql);
            cmd.CommandText = String.Format("SELECT m_id FROM meal WHERE m_start_date='{0:yyyy-MM-dd}' AND m_type='in';", start);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read()) {
                if (!rd.IsDBNull(0)) {
                    int mid = rd.GetInt32(0);
                    rd.Close();
                    cmd.CommandText = String.Format("UPDATE meal SET m_amount=m_amount+{1:d} WHERE m_id={0:d}", mid, amount);
                    cmd.ExecuteNonQuery();
                }
            } else {
                if (!rd.IsClosed) {
                    rd.Close();
                }
                cmd.CommandText = String.Format("INSERT INTO meal(m_start_date, m_amount, m_type) VALUES('{0:yyyy-MM-dd}', {1:d}, 'in');", start, amount);
                cmd.ExecuteNonQuery();
            }
            cmd.CommandText = "CALL updateMeal();";
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Добавляет продажу корма
        /// </summary>
        public static void AddMealOut(MySqlConnection sql, DateTime start, int amount)
        {
            MySqlCommand cmd = new MySqlCommand("", sql);
            cmd.CommandText = String.Format("SELECT m_id FROM meal WHERE m_start_date='{0:yyyy-MM-dd}' AND m_type='out';", start);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read()) {
                if (!rd.IsDBNull(0)) {
                    int mid = rd.GetInt32(0);
                    rd.Close();
                    cmd.CommandText = String.Format("UPDATE meal SET m_amount=m_amount+{1:d} WHERE m_id={0:d}", mid, amount);
                    cmd.ExecuteNonQuery();
                }
            } else {
                if (!rd.IsClosed) {
                    rd.Close();
                }
                cmd.CommandText = String.Format("INSERT INTO meal(m_start_date, m_amount, m_type) VALUES('{0:yyyy-MM-dd}', {1:d}, 'out');", start, amount);
                cmd.ExecuteNonQuery();
            }
            Meal.updateMeal(sql);
        }

        public static void DeleteMeal(MySqlConnection sql, int id)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("DELETE FROM meal WHERE m_id={0:d};call updateMeal();", id), sql);
            cmd.ExecuteNonQuery();
        }

        protected static void updateMeal(MySqlConnection sql)
        {
            MySqlCommand cmd = new MySqlCommand("CALL updateMeal();", sql);            
            cmd.ExecuteNonQuery();
        }
    }
}
#endif