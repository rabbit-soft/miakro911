using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using log4net;
using log4net.Config;

namespace rabnet
{
    public class RabNetEngine
    {
        private IRabNetDataLayer data=null;
        private ILog log = null;
        private int uid = 0;
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
            }
            else if (dbext == "db.miafile")
            {
                data = new RabNetDBMiaFile(param);
            }else{
                throw new ExDBDriverNotFoud(dbext);
            }
            return data;
        }
        public IRabNetDataLayer db()
        {
            return data;
        }
        public int setUid(String name,String password)
        {
            uid = db().checkUser(name, password);
            log.DebugFormat("check uid {0:d}", uid);
            return uid;
        }
    }
}
