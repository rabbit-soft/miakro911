using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public class LogList
    {
        public class OneLog
        {
            public DateTime date;
            public string user;
            public string work;
            public string prms;
            public string address;
            public OneLog(DateTime dt,string usr,string wrk,string p,string adr)
            {
                date = dt;
                user = usr;
                work = wrk;
                prms = p;
                address = adr;
            }
        }
        public List<OneLog>logs=new List<OneLog>();
        public void addLog(DateTime dt, string usr, string wrk, string p,string address)
        {
            logs.Add(new OneLog(dt, usr, wrk, p,address));
        }
    }

    class Logs
    {
        private MySqlConnection sql;
        public Logs(MySqlConnection sql)
        {
            this.sql=sql;
        }

        public static void addLog(MySqlConnection sql, int type, int user, int r1, int r2, string a1, string a2, string text)
        {
            (new Logs(sql)).addLog(type, user, r1, r2, a1, a2, text);
        }

        public void addLog(int type, int user, int r1, int r2, string a1, string a2, string text)
        {
            String qry = String.Format("INSERT INTO logs(l_date,l_type,l_user,l_rabbit,l_rabbit2,l_address,l_address2,l_param) VALUES(NOW(),{0:d},{1:d},{2:d},{3:d},'{4:s}','{5:s}','{6:s}');",
                type, user, r1, r2, a1, a2, text);
            MySqlCommand cmd = new MySqlCommand(qry, sql);
            cmd.ExecuteNonQuery();
        }

        public String makeWhere(Filters f)
        {
            if (f.safeValue("lgs") == "") return "";
            String res = "";
            String[] tps = f.safeValue("lgs", "").Split(',');
            for (int i = 0; i <= tps.Length-1; i++)
                res += "logs.l_type=" + tps[i]+" OR ";
            return " WHERE ("+res+"logs.l_type="+tps[tps.Length-1]+")";
        }

        public LogList getLogs(Filters f)
        {
            int limit = f.safeInt("lim",100);
            String qry=String.Format(@"SELECT logs.l_date date,logtypes.l_name name,users.u_name user,logtypes.l_params params,
logs.l_rabbit rabbit,logs.l_rabbit2 rabbit2,logs.l_address address,logs.l_address2 address2,
logs.l_param param,
anyname(logs.l_rabbit,2) r1,
anyname(logs.l_rabbit2,2) r2,
rabplace(logs.l_rabbit) place,
rabplace(logs.l_rabbit2) place2
FROM logs LEFT JOIN logtypes ON logs.l_type=logtypes.l_type LEFT JOIN users ON l_user=u_id {1:s} 
ORDER BY date DESC LIMIT {0:d};", limit,makeWhere(f));
            MySqlCommand cmd = new MySqlCommand(qry, sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            LogList ll = new LogList();
            while (rd.Read())
            {
                String np=rd.GetString("params");
                while (np.IndexOf('$') > -1)
                {
                    String prms = "";
                    char c = np[np.IndexOf('$') + 1];
                    switch (c)
                    {
                        case 'r': prms += rd.GetString("r1"); break;
                        case 'R': prms += rd.GetString("r2"); break;
                        case 'p': prms += Buildings.fullPlaceName(rd.GetString("place"), true, false, false); break;
                        case 'P': prms += Buildings.fullPlaceName(rd.GetString("place2"), true, false, false); break;
                        case 'a': prms += rd.GetString("address"); break;
                        case 'A': prms += rd.GetString("address2"); break;
                        case 't': prms += rd.IsDBNull(8)?"":rd.GetString("param"); break;
                    }
                    np = np.Replace("$" + c, prms);
                }
                String adr = rd.GetString("address");
                if (adr=="")
                    adr = Buildings.fullPlaceName(rd.GetString("place"), true, false, false);
                ll.addLog(rd.GetDateTime("date"), rd.IsDBNull(2)?"":rd.GetString("user"), rd.GetString("name"), np,adr);
            }
            rd.Close();
            return ll;
        }

        public String[] logNames()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT l_name FROM logtypes ORDER BY l_type ASC;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<String> res = new List<string>();
            while (rd.Read())
                res.Add(rd.GetString(0));
            rd.Close();
            return res.ToArray();
        }
    }
}
