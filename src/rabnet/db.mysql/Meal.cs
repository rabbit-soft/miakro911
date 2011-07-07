using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{

    public class sMeal
    {
        public enum MoveType{In,Out};

        public readonly MoveType Type = MoveType.In;
        /// <summary>
        /// Дата завоза корма
        /// </summary>
        public readonly DateTime StartDate = DateTime.MinValue;
        /// <summary>
        /// Дата окончания корма
        /// </summary>
        public readonly DateTime EndDate = DateTime.MinValue;
        /// <summary>
        /// Объем кормов (Указывается в КилоГраммах)
        /// </summary>
        public readonly int Amount = 0;
        /// <summary>
        /// Среднее потребление кролика в день(Измеряется в КилоГраммах)
        /// </summary>
        public readonly float Rate = 0;

        public sMeal(DateTime start, DateTime end, int amount, float rate,string type)
        {
            if (start != null)
                this.StartDate = start;
            if (end != null)
                this.EndDate = end;
            this.Amount = amount;
            this.Rate = rate;
            this.Type = type == "in" ? MoveType.In :  MoveType.Out;
        }

        public sMeal(DateTime start, int amount, string type)
        {
            this.StartDate = start;
            this.Amount = amount;
            this.Type = type == "in" ? MoveType.In : MoveType.Out;
        }

    }

    class Meal
    {
        public static List<sMeal> getMealPeriods(MySqlConnection sql)
        {
            List<sMeal> result = new List<sMeal>();
            MySqlCommand cmd = new MySqlCommand("SELECT m_start_date,m_end_date,m_amount,m_rate,m_type FROM meal ORDER BY m_start_date ASC;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                if (!rd.IsDBNull(1) && !rd.IsDBNull(3))
                    result.Add(new sMeal(rd.GetDateTime(0), rd.GetDateTime(1), rd.GetInt32(2), rd.GetFloat(3),rd.GetString(4)));
                else result.Add(new sMeal(rd.GetDateTime(0), rd.GetInt32(2), rd.GetString(4)));
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
            if (rd.Read())
            {
                if (!rd.IsDBNull(0))
                {
                    int mid = rd.GetInt32(0);
                    rd.Close();
                    cmd.CommandText = String.Format("UPDATE meal SET m_amount=m_amount+{1:d} WHERE m_id={0:d}", mid, amount);
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                if (!rd.IsClosed) rd.Close();
                cmd.CommandText = String.Format("INSERT INTO meal(m_start_date,m_amount,m_type) VALUES('{0:yyyy-MM-dd}',{1:d},'in');", start, amount);
                cmd.ExecuteNonQuery();
            }
            cmd.CommandText = "call updateMeal();";
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
            if (rd.Read())
            {
                if (!rd.IsDBNull(0))
                {
                    int mid = rd.GetInt32(0);
                    rd.Close();
                    cmd.CommandText = String.Format("UPDATE meal SET m_amount=m_amount+{1:d} WHERE m_id={0:d}", mid, amount);
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                if (!rd.IsClosed) rd.Close();
                cmd.CommandText = String.Format("INSERT INTO meal(m_start_date,m_end_date,m_amount,m_type) VALUES('{0:yyyy-MM-dd}','{0:yyyy-MM-dd}',{1:d},'out');", start, amount);
                cmd.ExecuteNonQuery();
            }
            cmd.CommandText = "call updateMeal();";
            cmd.ExecuteNonQuery();    
        }

    }
}
