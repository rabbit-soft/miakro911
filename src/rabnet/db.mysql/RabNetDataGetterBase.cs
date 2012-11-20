using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;
using log4net;
using rabnet;

namespace db.mysql
{
    public abstract class RabNetDataGetterBase : IDataGetter
    {
        protected static ILog _logger = log4net.LogManager.GetLogger(typeof(RabNetDataGetterBase));
        protected int count;
        protected int count2;
        protected int count3; //+gambit  понадобилось для подсчета кормилиц
        protected float count4; //+gambit  надо для подсчета среднего кол-ва детей
        protected int citem=0;
        protected MySqlConnection sql;
        protected MySqlDataReader rd;
        protected Filters options = null; 

        protected void Debug(String s)
        {
            _logger.Debug(this.GetType().ToString()+" "+s);
        }

        public RabNetDataGetterBase(MySqlConnection sql,Filters filters) 
        {
            options = filters;
            this.sql = sql;
            String qcmd = countQuery();//получить количество записей
#if DEBUG
            Debug("QCount: " + qcmd);
#endif
            MySqlCommand cmd = new MySqlCommand(qcmd, sql);
            rd = cmd.ExecuteReader();
            rd.Read();
            count = (int)rd.GetInt32(0);
            count2 = 0;
            if (rd.FieldCount > 1)
                count2 = rd.IsDBNull(1) ? 0 : rd.GetInt32(1);
            if (rd.FieldCount > 2)                                  //+gambit
            {
                count3 = rd.IsDBNull(2) ? 0 : rd.GetInt32(2);
                count4 = (float)count2 / (float)count3;
            }
            rd.Close();
            cmd.CommandText = getQuery();
#if DEBUG
            Debug("QGetIData:" + cmd.CommandText);
#endif
            rd = cmd.ExecuteReader();
        }
        public int getCount()
        {
            //Debug("count = "+count.ToString());
            return count;
        }
        public int getCount2()
        {
            return count2;
        }

        public int getCount3()
        {
            return count3;
        }

        public float getCount4()
        {
            return count4;
        }

        public void stop()
        {
            Debug("closed");
            rd.Close();
        }
        protected abstract String getQuery();
        protected abstract String countQuery();
        public abstract IData NextItem();        
        public IData GetNextItem()
        {
            if (!rd.Read())
            {
                Debug("NULL next item");
                return null;
            }
            return NextItem();
        }
        internal static String addWhereAnd(String str, String adder)
        {
            if (str != "") str += " AND ";
            str += adder;
            return str;
        }
        internal static String addWhereOr(String str, String adder)
        {
            if (str != "") str += " OR ";
            str += adder;
            return str;
        }

    }

}
