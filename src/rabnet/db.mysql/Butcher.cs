using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public class ButcherDate : IData
    {
        public DateTime Date;
        public int Victims;
        public int Products;
        public ButcherDate(DateTime Date, int victims, int products)
        {
            this.Date = Date;
            this.Victims = victims;
            this.Products = products;
        }
    }

    public class sMeat
    {
        public int Id;
        public DateTime Date;
        public string ProductType;
        public float Amount;
        public string Units;
        public bool Today;
        public string User;

        public sMeat(int id, DateTime date, string prodType, float amount, string unit, bool today, string user)
        {
            this.Id = id;
            this.Date = date;
            this.ProductType = prodType;
            this.Amount = amount;
            this.Units = unit;
            this.Today = today;
            this.User = user;
        }
    }

    public class ScalePLUSummary
    {
        private int _id;
        private int _prodId;
        public int ProdId
        {
            get { return _prodId; }
        }
        public DateTime Date;
        public string ProdName;
        public int TotalSell;
        public int TotalSumm;
        public int TotalWeight;
        public DateTime Cleared;

        public ScalePLUSummary(int id,DateTime date,int prodid,string prodname,int tsell,int tsumm,int tweight,DateTime clear)
        {
            this._id = id;
            this._prodId =prodid;
            this.Date = date;
            this.ProdName = prodname;
            this.TotalSell = tsell;
            this.TotalSumm = tsumm;
            this.TotalWeight = tweight; 
            this.Cleared = clear; 
        }
    }

    class Butcher:RabNetDataGetterBase
    {
        public Butcher(MySqlConnection sql,Filters f) : base(sql,f) { }

        public override string getQuery()
        {
            return @"CREATE TEMPORARY TABLE aaa as
SELECT Date(d_date) dt, SUM(r_group) cnt FROM dead WHERE d_reason=3 GROUP BY dt;

SELECT DISTINCT dt,cnt,(SELECT COUNT(*) FROM butcher WHERE DATE(b_date)=dt) prod FROM aaa ORDER BY dt DESC;
DROP TABLE aaa;";
        }

        public override string countQuery()
        {
            return @"SELECT (SELECT COUNT(DISTINCT Date(d_date)) FROM dead WHERE d_reason=3) cols,
                            (SELECT COUNT(r_group) FROM dead WHERE d_reason=3) cnt;";
        }

        public static IData getBucherDate(MySqlDataReader rd)
        {
            return new ButcherDate(rd.GetDateTime("dt"), rd.GetInt32("cnt"), rd.GetInt32("prod"));
        }

        public override IData nextItem()
        {
            return getBucherDate(rd);
        }

        /// <summary>
        /// Получает список забитых кроликов
        /// </summary>
        /// <param name="dt">Дата забива</param>
        public static List<OneRabbit> getVictims(MySqlConnection sql, DateTime dt)
        {
            List<OneRabbit> result = new List<OneRabbit>();
            MySqlCommand cmd = new MySqlCommand("", sql);
            cmd.CommandText = String.Format(@"SELECT r_id,r_last_fuck_okrol,NULL r_event_date,'none' r_event,r_overall_babies,r_lost_babies,
r_sex,r_born,r_flags,r_breed,r_zone,r_name,r_surname,r_secname,
deadplace(r_id) address,r_group,r_notes,
deadname(r_id,2) fullname,
(SELECT b_name FROM breeds WHERE b_id=r_breed) breedname,
(SELECT COALESCE(GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' '),'') FROM genoms WHERE g_id=r_genesis) genom,
r_status,r_rate,r_bon,r_parent,COALESCE(r_vaccine_end,NOW()) vac_end 
FROM dead WHERE d_reason=3 AND DATE(d_date)='{0:yyyy-MM-dd}';",dt);
            MySqlDataReader rd = cmd.ExecuteReader();
            while(rd.Read())
            {
                result.Add(RabbitGetter.fillRabbit(rd));
            }
            rd.Close();
            return result;
        }

        public static List<sMeat> GetMeats(MySqlConnection sql, DateTime dt)
        {
            List<sMeat> result = new List<sMeat>();
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT 
    b_id,
    b_date,
    (SELECT p_name FROM products WHERE p_id=b_prodtype) prod,
    b_amount,
    (SELECT p_unit FROM products WHERE p_id=b_prodtype) units,
    (SELECT u_name FROM users WHERE b_user=u_id) user,
    if(DATE(b_date)=DATE(NOW()),'true','false') today 
FROM butcher WHERE DATE(b_date)='{0:yyy-MM-dd}' ORDER by b_date DESC;",dt), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                result.Add(new sMeat(rd.GetInt32("b_id"),
                    rd.GetDateTime("b_date"),
                    rd.GetString("prod"),
                    rd.GetFloat("b_amount"),
                    rd.GetString("units"),
                    rd.GetBoolean("today"),
                    rd.GetString("user"))
                    );
            }
            rd.Close();
            return result;
        }

        public static List<String> getButcherMonths(MySqlConnection sql)
        {
            List<String> result = new List<String>();
            MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT Date_Format(b_date,'%m.%Y')dt FROM butcher ORDER BY b_date;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                result.Add(rd.GetString("dt"));
            }
            rd.Close();
            return result;
        }
    }

    class Scale
    {
        public static List<ScalePLUSummary> getPluSummarys(MySqlConnection sql,DateTime date)
        {
            List<ScalePLUSummary> result = new List<ScalePLUSummary>();
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT s_id,s_date,s_plu_id,s_plu_name,appendPLUSumm(s_tsumm)asm,appendPLUSell(s_tsell)asl,appendPLUWeight(s_tweight)aw,s_cleared FROM scaleprod WHERE DATE(s_date)='{0:yyyy-MM-dd}' ORDER BY s_id DESC;", date), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while(rd.Read())
            {
                result.Add(new ScalePLUSummary(
                    rd.GetInt32("s_id"),
                    rd.GetDateTime("s_date"),
                    rd.GetInt32("s_plu_id"),
                    rd.GetString("s_plu_name"),
                    rd.GetInt32("asm"),
                    rd.GetInt32("asl"),
                    rd.GetInt32("aw"),
                    rd.GetDateTime("s_cleared")));
            }
            rd.Close();
            return result;
        }

        public static void addPLUSummary(MySqlConnection sql,int prodid,string prodname,int tsell,int tsumm,int tweight,DateTime cleared)
        {
            MySqlCommand cmd = new MySqlCommand("", sql);
            cmd.CommandText = String.Format(@"call addPLUSummary({0:d},'{1:s}',{2:d},{3:d},{4:d},'{5:yyyy-MM-dd hh:mm}');",
                                                            prodid,prodname,tsell,tsumm,tweight,cleared);
            cmd.ExecuteNonQuery();
            
        }
    }
}
