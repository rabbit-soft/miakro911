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

        #region IRabNetDataLayer Members

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
        public int exec(String cmd)
        {
            log.Debug("exec query:" + cmd);
            MySqlCommand c = new MySqlCommand(cmd, sql);
            return c.ExecuteNonQuery();
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

        public string getOption(string name, string subname, uint uid)
        {
            MySqlDataReader rd=reader(String.Format("SELECT o_value FROM options WHERE o_name='{0:s}' AND o_subname='{1:s}' AND (o_uid={2:d} OR o_uid=0) ORDER BY o_uid DESC;",
                name,subname,uid));
            string res="";
            if (rd.Read())
                res=rd.GetString(0);
            rd.Close();
            return res;
        }

        public void setOption(string name, string subname, uint uid, string value)
        {
            exec(String.Format("DELETE FROM options WHERE o_name='{0:s}' AND o_subname='{1:s}' AND o_uid={2:d};",
                name,subname,uid));
            exec(String.Format("INSERT INTO options(o_name,o_subname,o_uid,o_value) VALUES('{0:s}','{1:s}',{2:d},'{3:s}');",
                name,subname,uid,value));
        }

        public DateTime now()
        {
            MySqlDataReader rd = reader("SELECT NOW();");
            rd.Read();
            DateTime res = rd.GetDateTime(0);
            rd.Close();
            return res;
        }

        public IDataGetter getRabbits(Filters filters)
        {
            return new Rabbits(sql, filters);
        }

        public IDataGetter getBuildings(Filters filters)
        {
            return new Buildings(sql, filters);
        }

        public String[] getFilterNames(string type)
        {
            MySqlDataReader rd = reader("SELECT f_name FROM filters WHERE f_type='"+type+"';");
            List<String> nms=new List<string>();
            while (rd.Read())
                nms.Add(rd.GetString(0));
            rd.Close();
            return nms.ToArray();
        }

        public Filters getFilter(string type, string name)
        {
            MySqlDataReader rd = reader("SELECT f_filter FROM filters WHERE f_type='"+type+"' AND f_name='"+name+"';");
            Filters f = new Filters();
            if (rd.Read())
                f.fromString(rd.GetString(0));
            rd.Close();
            return f;
        }

        public void setFilter(string type, string name, Filters filter)
        {
            exec("DELETE FROM filters WHERE f_type='"+type+"' AND f_name='"+name+"';");
            exec(String.Format("INSERT INTO filters(f_type,f_name,f_filter) VALUES('{0:s}','{1:s}','{2:s}');",
                type,name,filter.toString()));
        }

        public TreeData rabbitGenTree(int rabbit)
        {
            return Rabbits.getRabbitGen(rabbit,sql);
        }

        public TreeData buildingsTree()
        {
            return Buildings.getTree(0, sql, null);
        }


        public IDataGetter getYoungers(Filters filters)
        {
            return new Youngers(sql, filters);
        }

        public int[] getTiers(int farm)
        {
            if (farm == 0)
                return new int[] { 0, 0 };
            MySqlDataReader rd = reader("SELECT m_upper,m_lower FROM minifarms WHERE m_id=" + farm.ToString() + ";");
            rd.Read();
            int[] trs = new int[] { rd.GetInt32(0), rd.GetInt32(1) };
            rd.Close();
            return trs;
        }

        public Building getBuilding(int tier)
        {
            if (tier == 0)
                return null;
            return Buildings.getTier(tier, sql);
        }

        public IDataGetter getNames(Filters filters)
        {
            return new Names(sql, filters);
        }

        public IDataGetter zooTeh(Filters f)
        {
            return new ZooTeh(sql,f);
        }


        IBreeds IRabNetDataLayer.getBreeds()
        {
            return new Breeds(sql);
        }

        public OneRabbit getRabbit(int rid)
        {
            return RabbitGetter.GetRabbit(sql, rid);
        }
        public void setRabbit(OneRabbit r)
        {
            RabbitGetter.SetRabbit(sql,r);
        }
        public ICatalogs catalogs()
        {
            return new Catalogs(sql);
        }


        public void RabNetLog(int type, int user, int r1,int r2,string a1,string a2,string text)
        {
            Logs.addLog(sql,type,user,r1,r2,a1,a2,text);
        }

        public Fucks getFucks(int rabbit)
        {
            return FucksGetter.GetFucks(sql, rabbit);
        }

        public Fucks allFuckers(int female)
        {
            return FucksGetter.AllFuckers(sql, female);
        }

        public void setBon(int rabbit, string bon)
        {
            RabbitGetter.setBon(sql, rabbit, bon);
        }

        public void makeFuck(int female, int male, DateTime date)
        {
            RabbitGetter.makeFuck(sql, female, male, date);
        }

        public void makeProholost(int female, DateTime when)
        {
            RabbitGetter.MakeProholost(sql, female, when);
        }

        public void makeOkrol(int female, DateTime when, int children, int dead)
        {
            RabbitGetter.MakeOkrol(sql, female, when, children, dead);
        }

        public IZones getZones()
        {
            return new Zones(sql);
        }

        public string makeName(int nm, int sur, int sec, int grp, OneRabbit.RabbitSex sex)
        {
            return RabbitGetter.makeName(sql, nm, sur, sec, grp, sex);
        }

        public Younger[] getSuckers(int mom)
        {
            return Youngers.getSuckers(sql, mom);
        }

        public Building[] getFreeBuilding(Filters f)
        {
            return Buildings.getFreeBuildings(sql, f);
        }

        public void replaceRabbit(int rid, int farm, int tier_id, int sec)
        {
            RabbitGetter.replaceRabbit(sql, rid, farm, tier_id, sec);
        }
        public void replaceYounger(int rid, int farm, int tier_id, int sec)
        {
            RabbitGetter.replaceYounger(sql, rid, farm, tier_id, sec);
        }

        public int newRabbit(OneRabbit r, int mom)
        {
            return RabbitGetter.newRabbit(sql, r, mom);
        }

        public LogList getLogs(Filters f)
        {
            return (new Logs(sql).getLogs(f));
        }

        #endregion
    }
}
