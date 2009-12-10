using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using log4net;
using log4net.Config;

namespace rabnet
{
    public class RabNetEngine
    {
        private IRabNetDataLayer data=null;
        private IRabNetDataLayer data2 = null;
        private ILog log = null;
        private int uid = 0;
        private String uname;
        private String farmname;
        private Options opts=null;
        private RabNetLogs logger = null;
        private RabEngZooTeh zooteh = null;
        public RabNetEngine()
        {
            log4net.Config.XmlConfigurator.Configure();
            log = log4net.LogManager.GetLogger(typeof(RabNetEngine));
        }
        public IRabNetDataLayer initEngine(String dbext,String param)
        {
            if (data!=null)
            {
                data.close();
                data = null;
            }
            log.Debug("initing engine data to "+dbext+" param="+param);
            if (dbext == "db.mysql")
            {
                data = new RabNetDbMySql(param);
                data2 = new RabNetDbMySql(param);
            }
            else if (dbext == "db.miafile")
            {
                data = new RabNetDBMiaFile(param);
                data2 = data;
            }else{
                throw new ExDBDriverNotFoud(dbext);
            }
            return data;
        }
        public IRabNetDataLayer db()
        {
            return data;
        }
        public IRabNetDataLayer db2()
        {
            return data2;
        }
        public int setUid(String name, String password, String farmName)
        {
            uid = db().checkUser(name, password);
            log.DebugFormat("check uid {0:d} for farm {1:s}", uid,farmName);
            if (uid != 0)
            {
                uname = name;
                farmname = farmName;
            }
            return uid;
        }
        public String farmName(){return uname+"@"+farmname;}
        public int uId() { return uid; }
        public Options options()
        {
            if (opts==null)
                opts = new Options(this);
            return opts;
        }

        public RabNetEngRabbit getRabbit(int id)
        {
            return new RabNetEngRabbit(id, this);
        }

        public RabNetLogs logs()
        {
            if (logger == null)
                logger = new RabNetLogs(this);
            return logger;
        }

        public void setInbreeding(bool on)
        {
            options().setOption(Options.OPT_ID.INBREEDING, on ? 1 : 0);
        }
        public void setGeterosis(bool on)
        {
            options().setOption(Options.OPT_ID.GETEROSIS, on ? 1 : 0);
        }
        public RabEngZooTeh zoo()
        {
            if (zooteh == null)
                zooteh = new RabEngZooTeh(this);
            return zooteh;
        }
    }
}
