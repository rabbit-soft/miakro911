using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;
using rabnet;

namespace db.mysql
{  
    class Logs
    {
        class Params
        {
            public char Rabbit1 = 'r';
            public char Rabbit2 = 'R';
            public char Place1 = 'p';
            public char Place2 = 'P';
            public char Address1 = 'a';
            public char Address2 = 'A';
            public char Notes = 't';
        }

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

        private String makeWhere(Filters f)
        {
            String res = "";
            if (f.safeValue(Filters.LOGS) != "")///если пустая то показывать все логи
            {
                string tmp = "";
                String[] tps = f.safeValue(Filters.LOGS, "").Split(',');
                for (int i = 0; i < tps.Length - 1; i++)
                    tmp += String.Format("logs.l_type={0:s} OR ", tps[i]);
                res += String.Format("({0:s} logs.l_type={1:s})", tmp, tps[tps.Length - 1]);
            }
            if(f.ContainsKey(Filters.RAB_ID))
                res += String.Format("{0:s}(l_rabbit={1:s} OR l_rabbit2={1:s})", (res != "" ? " AND " : ""), f[Filters.RAB_ID]);
            if(f.ContainsKey(Filters.DATE_FROM) && f.ContainsKey(Filters.DATE_TO))
                res += String.Format("{0:s}Date(l_date) BETWEEN '{1:s}' AND '{2:s}'", (res != "" ? " AND " : ""), f[Filters.DATE_FROM], f[Filters.DATE_TO]);
            if(f.ContainsKey(Filters.ADDRESS))
                res += String.Format("{0:s}(Trim(l_address)='{1:s}' OR Trim(l_address2)='{1:s}')", (res != "" ? " AND " : ""), f[Filters.ADDRESS]);
            if(res!="")
                res = "WHERE "+res;
            return res;
        }

        public LogList getLogs(Filters f)
        {
            int limit = f.safeInt("lim", 100);
            String qry = String.Format(@"SELECT logs.l_date date, logtypes.l_name name, users.u_name user, logtypes.l_params params,
    logs.l_rabbit rabbit, logs.l_rabbit2 rabbit2, logs.l_address address, logs.l_address2 address2,
    logs.l_param param,
    anyname(logs.l_rabbit,2) r1,
    anyname(logs.l_rabbit2,2) r2,
    rabplace(logs.l_rabbit) place,
    rabplace(logs.l_rabbit2) place2
FROM logs 
LEFT JOIN logtypes ON logs.l_type=logtypes.l_type LEFT JOIN users ON l_user=u_id 
{1:s} 
ORDER BY date DESC LIMIT {0:d};", limit, makeWhere(f));
            MySqlCommand cmd = new MySqlCommand(qry, sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            LogList ll = new LogList();
            while (rd.Read())
            {
                String np = rd.GetString("params");
                while (np.IndexOf('$') > -1)
                {
                    String prms = "";
                    char c = np[np.IndexOf('$') + 1];
                    switch (c)
                    {
                        case 'r': prms += rd.GetString("r1"); break;
                        case 'R': prms += rd.GetString("r2"); break;
                        case 'p': prms += Building.FullPlaceName(rd.GetString("place"), true, false, false); break;
                        case 'P': prms += Building.FullPlaceName(rd.GetString("place2"), true, false, false); break;
                        case 'a': prms += rd.GetString("address"); break;
                        case 'A': prms += rd.GetString("address2"); break;
                        case 't': prms += rd.IsDBNull(8) ? "" : rd.GetString("param"); break;
                    }
                    np = np.Replace("$" + c, prms);
                }
                String adr = rd.GetString("address");
                if (adr == "")
                    adr = Building.FullPlaceName(rd.GetString("place"), true, false, false);
                ll.addLog(rd.GetDateTime("date"), rd.IsDBNull(2) ? "" : rd.GetString("user"), rd.GetString("name"), np, adr);
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

        /// <summary>
        /// Возвращает Дату начала работы с программой.
        /// Вычисляется по самому раннему логу.
        /// Если нет логов, то возвращает DateTime.MaxValue
        /// </summary>
        public static DateTime getFarmStartTime(MySqlConnection sql)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT l_date FROM logs ORDER BY l_id ASC LIMIT 1;",sql);
            string res = cmd.ExecuteScalar().ToString();
            DateTime result = DateTime.MaxValue;
            DateTime.TryParse(res,out result);
            return result;
        }

        public void ArchLogs()
        {
            MySqlCommand cmd = new MySqlCommand(@"INSERT INTO logs_arch(l_id,l_date,l_type,l_user,l_rabbit,l_address,l_rabbit2,l_address2,l_param)
  	(SELECT l_id,l_date,l_type,l_user,l_rabbit,l_address,l_rabbit2,l_address2,l_param FROM logs WHERE l_date<Date_Add(NOW(), INTERVAL -12 month) ORDER BY l_id LIMIT 1000);
    DELETE FROM logs WHERE l_date<Date_Add(NOW(), INTERVAL -12 month) ORDER BY l_id LIMIT 1000;", sql);
            cmd.ExecuteNonQuery();
        }
    }
}
