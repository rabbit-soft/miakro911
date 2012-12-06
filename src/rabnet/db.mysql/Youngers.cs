using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using rabnet;

namespace db.mysql
{  
    class Youngers:RabNetDataGetterBase
    {
        public Youngers(MySqlConnection sql, Filters f) : base(sql, f) { }

        /// <summary>
        /// Возвращает одну запись для вкладки Молодняк.
        /// </summary>
        /// <param name="rd">Результат выполнения запроса из  метода "getQuery"</param>
        /// <param name="shr"></param>
        /// <param name="sht"></param>
        /// <param name="sho"></param>
        /// <returns></returns>
        internal static YoungRabbit fillYounger(MySqlDataReader rd, bool shr, bool sht,bool sho)
        {
            YoungRabbit y = new YoungRabbit(rd.GetInt32("r_id"), rd.GetString("name"),
                rd.GetString("r_sex"), rd.GetDateTime("r_born"), rd.GetString("breed"),
                rd.GetInt32("r_group"), rd.GetString("r_bon"), rd.GetString("rplace"), rd.GetString("r_notes"),
                rd.GetInt32("r_parent"), rd.GetString("parent"), rd.GetInt32("neighbours"));
            return y;
        }
        internal static YoungRabbit fillYounger(MySqlDataReader rd) { return fillYounger(rd,false, false, false);}

        public override IData NextItem()
        {
            //bool shr = options.safeBool("shr");
            return fillYounger(_rd,options.safeBool(Filters.SHORT), options.safeBool(Filters.SHOW_BLD_TIERS), options.safeBool(Filters.SHOW_BLD_DESCR));
        }
        /// <summary>
        /// Строка запроса к БД, для получения данных о Молодняке
        /// </summary>
        /// <returns>sql-запрос</returns>
        protected override string getQuery()
        {
            return String.Format(@"SELECT {0:s}
FROM rabbits WHERE r_parent!=0 ORDER BY name;", getFieldSet_Youngers(options.safeBool("dbl"), options.safeBool("shr")));
        }
        /// <summary>
        /// Запрос на получение: 
        ///     Общего количества записей, 
        ///     общее количество кроликов,
        ///     общее количество кормилиц.
        /// </summary>
        /// <returns>sql-запрос</returns>
        protected override string countQuery()
        {
            //return "SELECT COUNT(*),SUM(r_group) FROM rabbits WHERE r_parent!=0;";
            return @"SELECT COUNT(*),
                            SUM(r_group), 
                            (SELECT count(a) FROM (select DISTINCT r_parent a from rabbits where r_parent<>0) t)mc                            
                    FROM rabbits WHERE r_parent!=0;";
        }

        public static YoungRabbit[] GetYoungers(MySqlConnection sql, int id)//TODO проверить
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT {0:s}
FROM rabbits WHERE r_parent={1:d} ORDER BY name;",getFieldSet_Youngers(true,false),id), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<YoungRabbit> y = new List<YoungRabbit>();
            while(rd.Read())
                y.Add(fillYounger(rd));
            rd.Close();
            return y.ToArray();
        }

        private static string getFieldSet_Youngers(bool dblNames,bool sror)
        {
            return String.Format(@"r_id,
    rabname(r_id,{0:s}) name,r_group,r_sex,
    (SELECT {1:s} FROM breeds WHERE b_id=r_breed) breed,
    r_parent,
    rabname(r_parent,{0:s}) parent,
    r_notes,TO_DAYS(NOW())-TO_DAYS(r_born) age,r_bon,
    (SELECT SUM(rg.r_group)-rabbits.r_group FROM rabbits rg WHERE rg.r_parent=rabbits.r_parent) neighbours,
    r_born,
    rabplace(r_parent) rplace", (dblNames ? "2" : "1"), sror ? "b_short_name" : "b_name");
        }
    }
}
