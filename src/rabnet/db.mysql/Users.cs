using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using rabnet;

namespace db.mysql
{
    class Users
    {
        private MySqlConnection sql = null;
        public Users(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public MySqlDataReader reader(String qry)
        {
            MySqlCommand cmd = new MySqlCommand(qry, sql);
            return cmd.ExecuteReader();
        }

        public MySqlCommand exec(String qry)
        {
            MySqlCommand cmd = new MySqlCommand(qry, sql);
            cmd.ExecuteNonQuery();
            return cmd;
        }
        /// <summary>
        /// Возвращает список всех пользователей
        /// </summary>
        public List<sUser> getUsers()
        {
            MySqlDataReader rd = reader("SELECT u_name, u_group, u_id, u_deleted FROM users WHERE u_deleted = 0;");
            List<sUser> res = new List<sUser>();
            while (rd.Read()) {
                res.Add(new sUser(rd.GetInt32("u_id"), rd.GetString("u_name"), rd.GetString("u_group")));
            }
            rd.Close();
            return res;
        }
        /// <summary>
        /// Возвразает одного пользователя по ID
        /// </summary>
        /// <param name="uid">ID пользователя</param>
        /// <returns></returns>
        public sUser getUser(int uid)
        {
            MySqlDataReader rd = reader(String.Format("SELECT u_name,u_group,u_id FROM users WHERE u_id={0};", uid.ToString()));
            sUser res = null;
            while (rd.Read()) {
                res = new sUser(rd.GetInt32("u_id"), rd.GetString("u_name"), rd.GetString("u_group"));
            }
            rd.Close();
            return res;
        }

        public int checkUser(string name, string password)
        {
            MySqlDataReader rd = reader("SELECT u_id FROM users WHERE u_name='" + name + "' AND u_password=MD5('" + password + "');");
            int res = 0;
            if (rd.Read())
                res = rd.GetInt32(0);
            rd.Close();
            return res;
        }

        public string getUserGroup(int uid)
        {
            MySqlDataReader rd = reader(String.Format("SELECT u_group FROM users WHERE u_id={0:d};", uid));
            string res = "none";
            if (rd.Read())
                res = rd.GetString(0);
            rd.Close();
            return res;
        }

        public void deleteUser(int uid)
        {
            //exec(String.Format("DELETE FROM users WHERE u_id={0:d};", uid));
            exec(String.Format("UPDATE users SET u_deleted=1 WHERE u_id={0:d};", uid));
        }


        public static string getGroup(int group)
        {
            String sgrp = sUser.Worker;
            switch (group) {
                case 0: sgrp = sUser.Admin; break;
                case 1: sgrp = sUser.Zootech; break;
                case 2: sgrp = sUser.Butcher; break;
            }
            return sgrp;
        }

        public void updateUser(int uid, string name, int group, string password, bool chpass)
        {
            exec(String.Format("UPDATE users SET u_name='{0:s}',u_group='{1:s}' WHERE u_id={2:d};", name, getGroup(group), uid));
            if (chpass)
                exec(String.Format("UPDATE users SET u_password=MD5('{0:s}') WHERE u_id={1:d};", password, uid));
        }

        public int addUser(string name, int group, string password)
        {
            MySqlCommand cmd = exec(String.Format(@"INSERT INTO users(u_name,u_group,u_password) 
VALUES('{0:s}','{1:s}',MD5('{2:s}'));", name, getGroup(group), password));
            return (int)cmd.LastInsertedId;
        }

        public bool hasUser(string name)
        {
            MySqlDataReader rd = reader(String.Format("SELECT * FROM users WHERE u_name='{0:s}';", name));
            bool res = rd.Read();
            rd.Close();
            return res;
        }

        public void checktb()
        {
            MySqlDataReader rd = reader("SELECT o_value FROM options WHERE o_name='db' AND o_subname='tb';");
            String tb = "";
            if (rd.Read())
                tb = rd.GetString(0);
            rd.Close();
            /* здесь была тайм бомба
            DateTime dt = DateTime.MinValue;
            if (!DateTime.TryParse(tb, out dt)) dt = DateTime.Parse("20.04.2010");
            if (dt.AddMonths(3) < DateTime.Now) throw new Exception("Bad Database CheckSumm. ReInitialize database.");
            */
        }

    }
}
