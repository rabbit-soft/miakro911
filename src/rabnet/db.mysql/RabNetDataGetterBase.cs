﻿using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;
using log4net;
using rabnet;
using System.Diagnostics;

namespace db.mysql
{
    public abstract class RabNetDataGetterBase : IDataGetter
    {
        protected static ILog _logger = log4net.LogManager.GetLogger(typeof(RabNetDataGetterBase));
        protected int count;
        protected int count2;
        protected int count3; //+gambit  понадобилось для подсчета кормилиц
        protected float count4; //+gambit  надо для подсчета среднего кол-ва детей
        protected int citem = 0;
        protected MySqlConnection _sql;
        protected MySqlDataReader _rd;
        protected Filters options = null;

        protected void Debug(String s)
        {
            _logger.Debug(this.GetType().ToString() + " " + s);
        }

        public RabNetDataGetterBase(MySqlConnection sql, Filters filters)
        {
            options = filters;
            this._sql = sql;


            String qcmd = this.countQuery();//получить количество записей
#if DEBUG
            Debug("QCount: " + qcmd);
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            MySqlCommand cmd = new MySqlCommand(qcmd, sql);
            _rd = cmd.ExecuteReader();
            _rd.Read();
            count = (int)_rd.GetInt32(0);
            count2 = 0;
            if (_rd.FieldCount > 1) {
                count2 = _rd.IsDBNull(1) ? 0 : _rd.GetInt32(1);
            }
            if (_rd.FieldCount > 2) {                                 //+gambit            
                count3 = _rd.IsDBNull(2) ? 0 : _rd.GetInt32(2);
                count4 = (float)count2 / (float)count3;
            }
            _rd.Close();

#if DEBUG                        
            sw.Stop();
            _logger.DebugFormat("execution time count: {0}", sw.Elapsed);
            sw.Reset();
            sw.Start();
#endif

            cmd.CommandText = this.getQuery();
            _rd = cmd.ExecuteReader();
#if DEBUG
            _logger.DebugFormat("execution time query: {0}", sw.Elapsed);
            Debug("QGetIData:" + cmd.CommandText);
#endif
        }


        public int getCount()
        {
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
            return (float.IsNaN(count4) ? 0 : count4);
        }

        public IData GetNextItem()
        {
            if (!_rd.Read()) {
                Debug("NULL next item");
                return null;
            }
            return NextItem();
        }

        public void Close()
        {
            Debug("closed");
            _rd.Close();
        }

        protected abstract String getQuery();

        protected abstract String countQuery(); //todo от этого метода нужно избавиться и все общие количества вести в Панели

        public abstract IData NextItem();

        internal static String addWhereAnd(String str, String adder)
        {
            if (!String.IsNullOrEmpty(str)) {
                str += " AND ";
            }
            str += adder;
            return str;
        }

        internal static String addWhereOr(String str, String adder)
        {
            if (!String.IsNullOrEmpty(str)) {
                str += " OR ";
            }
            str += adder;
            return str;
        }

    }

}
