using System;
using System.Collections.Generic;
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
            public OneLog(DateTime dt, string usr, string wrk, string p, string adr)
            {
                date = dt;
                user = usr;
                work = wrk;
                prms = p;
                address = adr;
            }
        }
        public List<OneLog> logs = new List<OneLog>();
        public void addLog(DateTime dt, string usr, string wrk, string p, string address)
        {
            logs.Add(new OneLog(dt, usr, wrk, p, address));
        }
    }
}
