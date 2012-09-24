using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;

namespace rabnet
{
    public class RabNetEngine
    {
        /// <summary>
        /// Необходимой  версия БД (options o_name='db',o_subname='version')
        /// </summary>
        const int NEED_DB_VERSION = 10;
        private IRabNetDataLayer data = null;
        private IRabNetDataLayer data2 = null;
        private ILog _logger = null;
        private int uid = 0;
        private string group = "none";
        private String uname;
        private String farmname;
        private Options opts=null;
        private RabNetLogs logger = null;
        private RabEngZooTeh zooteh = null;

        public RabNetEngine()
        {
            log4net.Config.XmlConfigurator.Configure();
            _logger = log4net.LogManager.GetLogger(typeof(RabNetEngine));
        }

        /// <summary>
        /// Устанавливает обработчик функций движка
        /// </summary>
        /// <param name="dbext">Тип</param>
        /// <param name="param">Строка подключения</param>
        /// <returns></returns>
        public IRabNetDataLayer initEngine(String dbType,String param)
        {
            if (data!=null)
            {
                if (data2 == data) data2 = null;
                data.Close();
                if (data2 != null) data2.Close();
                data = data2 = null;
            }
            _logger.Debug("initing engine data to " + dbType + " param=" + param);
            if (dbType == "db.mysql")
            {
                //data = new RabNetDbMySql(param);
                //data2 = new RabNetDbMySql(param);
                data = getDataLayer("db.mysql");               
                data2 = data.Clone();
                data.Init(param);
                data2.Init(param);
            }
            /*
            else if (dbext == "db.miafile")
            {
                data = new RabNetDBMiaFile(param);
                data2 = data;
            }*/
            else
            {
                throw new ExDBDriverNotFoud(dbType);
            }
            int ver = options().getIntOption("db", "version", Options.OPT_LEVEL.FARM);
            if (ver != NEED_DB_VERSION)
            {
                if (data2 == data) data2 = null;
                if (data != null) data.Close();
                if (data2 != null) data2.Close();
                data = data2 = null;
                throw new ExDBBadVersion(NEED_DB_VERSION, ver);
            }
            return data;
        }

        public IRabNetDataLayer initEngine(String param)
        {
            return initEngine("db.mysql", param);
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
            _logger.DebugFormat("check uid {0:d} for farm {1:s}", uid,farmName);
            if (uid != 0)
            {
                group = db().userGroup(uid);
                uname = name;
                farmname = farmName;
            }
            return uid;
        }

        public String farmName(){return uname+"@"+farmname;}

        public int userId { get { return uid; } }

        public Options options()
        {
            if (opts == null)
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

        public RabNetEngBuilding getBuilding(int tier)
        {
            return new RabNetEngBuilding(tier, this);
        }

        public RabNetEngBuilding getBuilding(string place)
        {
            return RabNetEngBuilding.fromPlace(place, this);
        }

        /// <summary>
        /// Возраст при котором возводят в Невесты
        /// </summary>
        public int brideAge()
        {
            return options().getIntOption(Options.OPT_ID.MAKE_BRIDE);
        }
        /// <summary>
        /// Возраст при котором возводят в Кандидаты
        /// </summary>
        public int candidateAge()
        {
            return options().getIntOption(Options.OPT_ID.MAKE_CANDIDATE);
        }

        public bool isAdmin()
        {
            return group == sUser.Admin;
        }

        public void delUser(int uid)
        {
            if (uid == userId)
                throw new ApplicationException("Нельзя удалить себя.");
            if (!isAdmin())
                throw new ApplicationException("Нет прав доступа.");
            db().deleteUser(uid);            
        }

        public void updateUser(int uid, string name, int group, string password, bool chpass)
        {
            if (uid == userId && group != 0)
                throw new ApplicationException("Нельзя сменить группу администратора.");
            if (name == "")
                throw new ApplicationException("Пустое имя.");
            if (!isAdmin())
                throw new ApplicationException("Нет прав доступа.");
            db().changeUser(uid, name, group, password, chpass);
        }

        public void addUser(string name, int group, string password)
        {
            if (name == "")
                throw new ApplicationException("Пустое имя.");
            if (db().hasUser(name))
                throw new ApplicationException("Пользователь уже существует.");
            if (!isAdmin())
                throw new ApplicationException("Нет прав доступа.");
            db().addUser(name, group, password);
        }

        public void resurrect(int rid)
        {
            logs().log(RabNetLogs.LogType.RESURRECT, rid);
            db().resurrect(rid);
        }

        public void preOkrol(int rid)
        {
            logs().log(RabNetLogs.LogType.PREOKROL, rid);
        }

        private IRabNetDataLayer getDataLayer(string asmName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, asmName+".dll");
            if (!File.Exists(filePath))
                throw new Exception("MySQL DataLayer dll is not exists");
            //todo проверка на уже загруженность сборки
            Assembly Asm = Assembly.LoadFile(filePath);//загружаем Сборку
            //Type AsmType = Asm.GetType();
            foreach (Type AsmType in Asm.GetTypes())//Проверяем все имеющиеся типы данных (классы)
            {
                if (typeof(IRabNetDataLayer).IsAssignableFrom(AsmType))
                {
                    IRabNetDataLayer db = (IRabNetDataLayer)Activator.CreateInstance(AsmType);
                    return db;
                }
            }
            throw new Exception("could not load DataLayer from assembly");
        }
	}
}
