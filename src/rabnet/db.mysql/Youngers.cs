using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public class Younger : IData
    {
        public int fid;
        public String fname;
        public string fsex;
        public int fage;
        public String fbreed;
        public String fe = "";
        public String fcls;
        public string faddress;
        public string fnotes;
        public int fcount;
        public int fneighbours;
        public string mom;
        public int momid;
        public Younger(int id,String name,String sex,int age,String breed,String cls,int count,String notes)
        {
            fid = id;
            fname = name;
            fsex = sex;
            fage = age;
            fbreed = breed;
            fcls = cls;
            fnotes = notes;
            fcount = count;
        }
    }

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
        public static IData getYounger(MySqlDataReader rd, bool shr, bool sht,bool sho)
        {
            Younger y = new Younger(rd.GetInt32("r_id"), rd.GetString("name"),
                Rabbits.getRSex(rd.GetString("r_sex")), rd.GetInt32("age"), rd.GetString("breed"),
                Rabbits.getBon(rd.GetString("r_bon"), shr), rd.GetInt32("r_group"), rd.GetString("r_notes"));
            y.fneighbours = rd.GetInt32("neighbours");
            y.mom = rd.GetString("parent");
            y.momid = rd.GetInt32("r_parent");
            y.faddress = Buildings.fullPlaceName(rd.GetString("rplace"), shr,sht,sho);
            return y;
        }
        public override IData nextItem()
        {
            bool shr = options.safeBool("shr");
            return getYounger(rd, shr, options.safeBool("sht"), options.safeBool("sho"));
        }
        /// <summary>
        /// Строка запроса к БД, для получения данных о Молодняке
        /// </summary>
        /// <returns>sql-запрос</returns>
        public override string getQuery()
        {
            return @"SELECT r_id,
rabname(r_id,"+(options.safeBool("dbl")?"2":"1")+@") name,
r_group,r_sex,
(SELECT "+(options.safeBool("shr")?"b_short_name":"b_name")+@" FROM breeds WHERE b_id=r_breed) breed,
r_parent,
rabname(r_parent," + (options.safeBool("dbl") ? "2" : "1") + @") parent,
r_notes,TO_DAYS(NOW())-TO_DAYS(r_born) age,r_bon,
(SELECT SUM(rg.r_group)-rabbits.r_group FROM rabbits rg WHERE rg.r_parent=rabbits.r_parent) neighbours,
rabplace(r_parent) rplace
FROM rabbits WHERE r_parent!=0 ORDER BY name;";
        }
        /// <summary>
        /// Запрос на получение: 
        ///     Общего количества записей, 
        ///     общее количество кроликов,
        ///     общее количество кормилиц.
        /// </summary>
        /// <returns>sql-запрос</returns>
        public override string countQuery()
        {
            //return "SELECT COUNT(*),SUM(r_group) FROM rabbits WHERE r_parent!=0;";
            return @"SELECT COUNT(*),
                            SUM(r_group), 
                            (SELECT count(a) FROM (select DISTINCT r_parent a from rabbits where r_parent<>0) t)mc                            
                    FROM rabbits WHERE r_parent!=0;";
        }

        public static Younger[] getSuckers(MySqlConnection sql, int id)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT r_id,
rabname(r_id,2) name,r_group,r_sex,
(SELECT b_name FROM breeds WHERE b_id=r_breed) breed,
r_parent,
rabname(r_parent,2) parent,
r_notes,TO_DAYS(NOW())-TO_DAYS(r_born) age,r_bon,
(SELECT SUM(rg.r_group)-rabbits.r_group FROM rabbits rg WHERE rg.r_parent=rabbits.r_parent) neighbours,
rabplace(r_parent) rplace
FROM rabbits WHERE r_parent={0:d} ORDER BY name;",id), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<Younger> y = new List<Younger>();
            while(rd.Read())
                y.Add(getYounger(rd,false,false,false) as Younger);
            rd.Close();
            return y.ToArray();
        }

    }
}
