using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace butcher
{
    public class sProductType
    {
        public int Id;
        public string Name;
        public string Units;
        public byte[] Image;
        //int ImageSize;

        public sProductType() { }

        public sProductType(int id, string name, string unit, byte[] image)
        {
            this.Id = id;
            this.Name = name;
            this.Units = unit;
            this.Image = image;
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

        public sMeat(int id,DateTime date,string prodType,float amount,string unit,bool today,string user)
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

    public class sUser
    {
        public int Id;
        public string Name;
        public string Group;

        public sUser() { }

        public sUser(int UserID, string UserName, string UserGroup)
        {
            this.Id = UserID;
            this.Name = UserName;
            this.Group = UserGroup;
        }
    }

    internal static class DBproc
    {
        private static int _currentUserID;
        private static MySqlConnection _sql;
        private static string _conStr;

        public static bool Connect(string conStr)
        {
            if (conStr == "") return false;
            _conStr = conStr;
            _sql = new MySqlConnection(_conStr);
            return connected;
        }

        public static void Disconnect(string conStr)
        {
            if (_sql != null)
            {
                _sql.Close();
                _sql.Dispose();
            }
        }

        private static bool connected
        {
            get
            {
                try
                {
                    if (_sql == null)
                        _sql = new MySqlConnection(_conStr);
                    if (_sql.State != System.Data.ConnectionState.Open)
                        _sql.Open();
                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool CheckUser(string user, string pass)
        {
            if (!connected) return false;
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT u_id FROM users WHERE u_name='{0}' AND u_password=MD5('{1}');",user,pass),_sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                _currentUserID = rd.GetInt32("u_id");
                rd.Close();
                return true;
            }
            else
            {
                rd.Close();
                return false;
            }
        }

        public static List<sProductType> GetProducts()
        {
            if (!connected) return null;
            List<sProductType> result = new List<sProductType>();
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT p_id,p_name,p_unit,p_image,p_imgsize FROM products;"), _sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while(rd.Read())
            {
                byte[] img = new byte[rd.GetInt32("p_imgsize")];
                if(img.Length!=0) 
                    rd.GetBytes(rd.GetOrdinal("p_image"), 0, img, 0, img.Length);
                result.Add(new sProductType(rd.GetInt32("p_id"),rd.GetString("p_name"),rd.GetString("p_unit"),img));         
            }
            rd.Close();
            return result;
        }

        public static sProductType GetProduct(int pid)
        {
            if (!connected) return null;
            sProductType result = null;
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT p_id,p_name,p_unit,p_image,p_imgsize FROM products WHERE p_id={0};",pid.ToString()), _sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                byte[] img = new byte[rd.GetInt32("p_imgsize")];
                if (img.Length != 0)
                    rd.GetBytes(rd.GetOrdinal("p_image"), 0, img, 0, img.Length);
                result = new sProductType(rd.GetInt32("p_id"), rd.GetString("p_name"), rd.GetString("p_unit"), img);
            }
            rd.Close();
            return result;
        }

        public static List<sUser> GetUsers()
        {
            if (!connected) return null;
            List<sUser> result = new List<sUser>();
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT u_id,u_name,u_group FROM users WHERE u_group='butcher';"), _sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                result.Add(new sUser(rd.GetInt32("u_id"),rd.GetString("u_name"),rd.GetString("u_group")));
            }
            rd.Close();
            return result;
        }

        public static void AddMeat(int prodType, float amount)
        {
            if (!connected) return;
            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO butcher (b_date,b_prodtype,b_amount,b_user) VALUES(NOW(),{0},@amount,{1});",prodType,_currentUserID), _sql);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.ExecuteNonQuery();
            return;
        }

        public static List<sMeat> GetMeats()
        {
            if (!connected) return null;
            List<sMeat> result = new List<sMeat>();
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT 
    b_id,
    b_date,
    (SELECT p_name FROM products WHERE p_id=b_prodtype) prod,
    b_amount,
    (SELECT p_unit FROM products WHERE p_id=b_prodtype) units,
    (SELECT u_name FROM users WHERE b_user=u_id) user,
    if(DATE(b_date)=DATE(NOW()),'true','false') today 
FROM butcher ORDER by b_date DESC LIMIT 100;"), _sql);
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
    }
}
