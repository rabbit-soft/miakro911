using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using log4net;

namespace rabnet
{
    public class sMeal
    {
        /// <summary>
        /// Дата завоза корма
        /// </summary>
        public string StartDate = "";
        /// <summary>
        /// Дата окончания корма
        /// </summary>
        public string EndDate = " - ";
        /// <summary>
        /// Объем кормов (Указывается в КилоГраммах)
        /// </summary>
        public float Amount = 0;
        /// <summary>
        /// Среднее потребление кролика в день(Измеряется в КилоГраммах)
        /// </summary>
        public float Rate = 0;

        public sMeal(DateTime start, DateTime end, float amount, float rate)
        {
            if (start != null)
                this.StartDate = start.ToShortDateString();
            if(end != null)
                this.EndDate = end.ToShortDateString();
            this.Amount = amount;
            this.Rate = rate;
        }

        public sMeal(DateTime start, float amount)
        {
            this.StartDate = start.ToShortDateString();
            this.Amount = amount;
        }

    }

    class Meal
    {
        public static List<sMeal> getMealPeriods(MySqlConnection sql)
        {
            List<sMeal> result = new List<sMeal>();
            MySqlCommand cmd = new MySqlCommand("SELECT m_start_date,m_end_date,m_amount,m_rate FROM meal ORDER BY m_id;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                if (!rd.IsDBNull(1))
                    result.Add(new sMeal(rd.GetDateTime(0), rd.GetDateTime(1), rd.GetFloat(2), rd.GetFloat(3)));
                else result.Add(new sMeal(rd.GetDateTime(0), rd.GetFloat(2)));
            }
            rd.Close();
            return result;
        }
        /// <summary>
        /// Добавляет новый период привоза корма
        /// </summary>
        /// <param name="start">Дата завоза</param>
        /// <param name="amount">Объем корма(кг)</param>
        public static void AddMealPeriod(MySqlConnection sql, DateTime start, float amount)
        {
            //далее закрываем предыдущую дату
            MySqlCommand cmd = new MySqlCommand("SELECT m_id FROM meal WHERE m_end_date IS NULL LIMIT 1", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                int id = rd.GetInt32("m_id");
                rd.Close();
                cmd.CommandText = String.Format("UPDATE meal SET m_end_date='{0:yyyy-MM-dd HH:mm:ss}' WHERE m_id={1};", start, id);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("UPDATE meal SET m_rate=mealCalculate({0}) WHERE m_id={0};", id);
                cmd.ExecuteNonQuery();
            }
            else rd.Close();
            //далее новая дата
            cmd.CommandText = String.Format("INSERT INTO meal(m_start_date,m_amount) VALUES('{0:yyyy-MM-dd HH:mm:ss}',{1});",start,amount);
            cmd.ExecuteNonQuery();
        }

    }
}
