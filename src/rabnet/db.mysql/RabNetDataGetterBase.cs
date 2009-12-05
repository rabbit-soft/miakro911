using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;
using log4net;

namespace rabnet
{
    abstract class RabNetDataGetterBase : IDataGetter
    {
        protected static ILog log = log4net.LogManager.GetLogger(typeof(RabNetDataGetterBase));
        protected int count;

        protected int citem=0;
        protected MySqlConnection sql;
        protected MySqlDataReader rd;
        protected Filters options = null; 
        protected void Debug(String s)
        {
            log.Debug(this.GetType().ToString()+" "+s);
        }
        public RabNetDataGetterBase(MySqlConnection sql,Filters filters)
        {
            options = filters;
            this.sql = sql;
            String qcmd=countQuery();
            Debug("c_query:"+qcmd);
            MySqlCommand cmd = new MySqlCommand(qcmd, sql);
            rd = cmd.ExecuteReader();
            rd.Read();
            count = (int)rd.GetInt32(0);
            rd.Close();
            cmd.CommandText = getQuery();
            Debug("d_query:" + cmd.CommandText);
            rd = cmd.ExecuteReader();
        }
        public int getCount()
        {
            Debug("count = "+count.ToString());
            return count;
        }
        public void stop()
        {
            Debug("closed");
            rd.Close();
        }
        public abstract IData nextItem();
        public abstract String getQuery();
        public abstract String countQuery();
        public IData getNextItem()
        {
            if (!rd.Read())
            {
                Debug("NULL next item");
                return null;
            }
            return nextItem();
        }
    }

}
