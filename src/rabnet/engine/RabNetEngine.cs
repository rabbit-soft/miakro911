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
        const int NEED_DB_VERSION = 18;

        private IRabNetDataLayer _data = null;
        private IRabNetDataLayer _data2 = null;
        private ILog _logger = null;
        private int uid = 0;
        private string group = "none";
        private String _uname;
        private String _farmname;
        private Options opts = null;
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
        public IRabNetDataLayer InitEngine(String dbType, String param)
        {
            if (_data != null) {
                if (_data2 == _data) {
                    _data2 = null;
                }
                _data.Close();
                if (_data2 != null) {
                    _data2.Close();
                }
                _data = _data2 = null;
            }
            _logger.Debug("initing engine data to " + dbType + " param=" + param);
            if (dbType == "db.mysql") {
                _data = getDataLayer("db.mysql");
                _data2 = _data.Clone();
                _data.Init(param);
                _data2.Init(param);
            } else {
                throw new DBDriverNotFoudException(dbType);
            }

            int ver = options().getIntOption("db", "version", Options.OPT_LEVEL.FARM);
            if (ver != NEED_DB_VERSION) {
                if (_data2 == _data) {
                    _data2 = null;
                }
                if (_data != null) {
                    _data.Close();
                }
                if (_data2 != null) {
                    _data2.Close();
                }
                _data = _data2 = null;
                throw new DBBadVersionException(NEED_DB_VERSION, ver);
            }
            return _data;
        }

        public IRabNetDataLayer initEngine(String param)
        {
            return InitEngine("db.mysql", param);
        }

        public IRabNetDataLayer db()
        {
            return _data;
        }

        public IRabNetDataLayer db2()
        {
            return _data2;
        }

        public int setUid(String name, String password, String farmName)
        {
            uid = db().checkUser(name, password);
            _logger.DebugFormat("check uid {0:d} for farm {1:s}", uid, farmName);
            if (uid != 0) {
                group = db().userGroup(uid);
                _uname = name;
                _farmname = farmName;
            }
            return uid;
        }

        public String farmName() { return _uname + "@" + _farmname; }

        public int UserID { get { return uid; } }
        public string UserName { get { return _uname; } }
        public string FarmName { get { return _farmname; } }

        public Options options()
        {
            if (opts == null) {
                opts = new Options(this);
            }
            return opts;
        }

        public RabNetEngRabbit getRabbit(int id)
        {
            return new RabNetEngRabbit(id, this);
        }

        public RabNetLogs logs()
        {
            if (logger == null) {
                logger = new RabNetLogs(this);
            }
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
            if (zooteh == null) {
                zooteh = new RabEngZooTeh(this);
            }
            return zooteh;
        }

        public RabNetEngBuilding getBuilding(int tier)
        {
            return new RabNetEngBuilding(tier, this);
        }

        public RabNetEngBuilding getBuilding(string place)
        {
            return RabNetEngBuilding.FromPlace(place, this);
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
            if (uid == UserID) {
                throw new ApplicationException("Нельзя удалить себя.");
            }
            if (!isAdmin()) {
                throw new ApplicationException("Нет прав доступа.");
            }
            db().deleteUser(uid);
        }

        public void updateUser(int uid, string name, int group, string password, bool chpass)
        {
            if (uid == UserID && group != 0) {
                throw new ApplicationException("Нельзя сменить группу администратора.");
            }
            if (name == "") {
                throw new ApplicationException("Пустое имя.");
            }
            if (!isAdmin()) {
                throw new ApplicationException("Нет прав доступа.");
            }
            db().changeUser(uid, name, group, password, chpass);
        }

        public void addUser(string name, int group, string password)
        {
            if (name == "") {
                throw new ApplicationException("Пустое имя.");
            }
            if (db().hasUser(name)) {
                throw new ApplicationException("Пользователь с таким именем уже существует.");
            }
            if (!isAdmin()) {
                throw new ApplicationException("Нет прав доступа.");
            }
            db().addUser(name, group, password);
        }

        public void Resurrect(int rid)
        {
            logs().log(LogType.RESURRECT, rid);
            db().ResurrectRabbit(rid);
        }

        public void preOkrol(int rid)
        {
            logs().log(LogType.PREOKROL, rid);
        }

        private IRabNetDataLayer getDataLayer(string asmName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, asmName + ".dll");
            if (!File.Exists(filePath)) {
                throw new Exception("MySQL DataLayer dll is not exists");
            }
            ///todo проверка на уже загруженность сборки
            Assembly Asm = Assembly.LoadFile(filePath);//загружаем Сборку
            //Type AsmType = Asm.GetType();
            foreach (Type AsmType in Asm.GetTypes()) {//Проверяем все имеющиеся типы данных (классы)            
                if (typeof(IRabNetDataLayer).IsAssignableFrom(AsmType)) {
                    IRabNetDataLayer db = (IRabNetDataLayer)Activator.CreateInstance(AsmType);
                    return db;
                }
            }
            throw new Exception("could not load DataLayer from assembly");
        }

        public string GetDBGuid()
        {
            string guid = options().getOption("db", "guid", Options.OPT_LEVEL.FARM);
            if (guid == "" || guid == "0") {
                guid = Guid.NewGuid().ToString();
                options().setOption("db", "guid", Options.OPT_LEVEL.FARM, guid);
            }
            return guid;
        }

        public void ChangeFucker(Fuck f, int newFucker, DateTime newFuckDate)
        {
            this._data.changeFucker(f.Id, newFucker, newFuckDate);

            // меняем крольчихе дату случки
            RabNetEngRabbit rabFemale = this.getRabbit(f.FemaleId);
            rabFemale.EventDate = newFuckDate;
            rabFemale.Commit();

            if (newFucker != f.PartnerId) {
                RabNetEngRabbit eFucker = this.getRabbit(f.PartnerId);
                Fucks fucks = this.db().GetFucks(new Filters(Filters.FIND_PARTNERS, f.PartnerId));
                eFucker.LastFuckOkrol = fucks.LastFuck.EventDate;
                eFucker.Commit();
            }
        }

    }
}
