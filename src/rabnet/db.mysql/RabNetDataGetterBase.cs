using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    abstract class RabNetDataGetterBase : IDataGetter
    {
        protected int count;
        protected int citem=0;
        protected MySqlConnection sql;
        protected MySqlDataReader rd;
        public RabNetDataGetterBase(MySqlConnection sql)
        {
            this.sql = sql;
            MySqlCommand cmd = new MySqlCommand(countQuery(), sql);
            rd = cmd.ExecuteReader();
            rd.Read();
            count = (int)rd.GetInt32(0);
            rd.Close();
            cmd.CommandText = getQuery();
            rd = cmd.ExecuteReader();
        }
        public int getCount()
        {
            return count;
        }

        public void stop()
        {
            rd.Close();
        }
        public abstract IData nextItem();
        public abstract String getQuery();
        public abstract String countQuery();
        public IData getNextItem()
        {
            if (!rd.Read())
                return null;
            return nextItem();
        }
    }

}
