using System;
using System.Collections.Generic;
using System.Text;
using rabnet;
using MySql.Data;
using MySql.Data.MySqlClient;
using log4net;
using log4net.Config;

namespace rabnet
{
    public class RabNetDbMySql:IRabNetDataLayer
    {
        private MySqlConnection sql=null;
        private ILog log = LogManager.GetLogger(typeof(RabNetDbMySql));
        public RabNetDbMySql() {
            log4net.Config.XmlConfigurator.Configure();
            log.Debug("created");
        }
        public RabNetDbMySql(String connectionString)
            : this()
        {
            init(connectionString);
        }
        ~RabNetDbMySql()
        {
            close();
        }
        public void close()
        {
            if (sql != null)
            {
                sql.Close();
                sql = null;
            }
        }
        public void init(String connectionString)
        {
            close();
            log.Debug("init from string "+connectionString);
            sql = new MySqlConnection(connectionString);
            sql.Open();
        }
        public MySqlDataReader reader(String cmd)
        {
            log.Debug("reader query:"+cmd);
            MySqlCommand c=new MySqlCommand(cmd,sql);
            return c.ExecuteReader();
        }

        public List<String> getUsers()
        {
            MySqlDataReader rd = reader("SELECT u_name FROM users;");
            List<String> res=new List<string>();
            while (rd.Read())
                res.Add(rd.GetString(0));
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

    }
}
