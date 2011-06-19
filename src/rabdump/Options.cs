using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace rabdump
{
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    class DataBase : Object
    {
        const string ALL_DB = "[ВСЕ]";
        const string DB = "База Данных";
        const string Obj = " Объект";
        private string _db, _host, _user, _pswd, _nm = "Ферма";

        public string Guid ="";
            
        public static DataBase AllDataBases = new DataBase(ALL_DB);

        [Category(Obj),DisplayName("Название"),Description("")]
        public String Name { get { return _nm; } set { if (value!="") _nm = value; } }
        [Category(DB), DisplayName("Хост"), Description("")]
        public String Host{get{return _host;} set{_host=value;}}
        [Category(DB), DisplayName("Имя БД"), Description("")]
        public String DBName{get{return _db;} set{_db=value;}}
        [Category(DB), DisplayName("Пользователь"), Description("")]
        public String User{get{return _user;} set{_user=value;}}
        [Category(DB), DisplayName("Пароль"), Description("")]
        public String Password{get{return _pswd;} set{_pswd=value;}}

        public DataBase() { }

        public DataBase(String name) : this() 
        { 
            _nm = name;
            if (name == ALL_DB)
                this.Guid = ALL_DB;
        }  

        public DataBase(string guid,string host, string database, string user,string password,string farmName)
        {
            this.Guid = guid;
            _host = host;
            _db = database;
            _user = user;
            _pswd = password;
            _nm = farmName;
        }   

        public override string ToString()
        {
            return _nm;
        }     

        /*public void Save(XmlNode nd,XmlDocument doc)
        {
            XmlElement db = doc.CreateElement("db");
            db.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(Name));
            db.AppendChild(doc.CreateElement("host")).AppendChild(doc.CreateTextNode(Host));
            db.AppendChild(doc.CreateElement("db")).AppendChild(doc.CreateTextNode(DBName));
            db.AppendChild(doc.CreateElement("user")).AppendChild(doc.CreateTextNode(User));
            db.AppendChild(doc.CreateElement("password")).AppendChild(doc.CreateTextNode(Password));
            nd.AppendChild(db);
        }*/   

        /*public void Load(XmlNode nd)
        {
            if (nd.Name != "db") return;
            foreach(XmlNode n in nd.ChildNodes)
            {

                String val = "";
                if (n.ChildNodes.Count>0)
                    val=n.ChildNodes[0].Value;
                switch (n.Name)
                {
                    case "name": Name = val; break;
                    case "host": Host = val; break;
                    case "db": DBName = val; break;
                    case "user": User = val; break;
                    case "password": Password = val; break;
                }
            }
        }

        public static DataBase Load(XmlNode nd,int hz)
        {
            DataBase db = new DataBase();
            db.Load(nd);
            return db;
        }*/
    }

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    class ArchiveJob : Object
    {
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public enum ArcType {При_Запуске,Единожды,Ежедневно,Еженедельно,Ежемесячно};
        const string Obj = " Объект";
        const string Data = "Данные";
        const string Time = "Расписание";
        private string _nm = "Расписание", _bp = "C:\\";
        private int _sl, _cl, _rp;
        private DataBase _db = DataBase.AllDataBases;
        private DateTime _st;
        private ArcType _tp;
        public string Guid ="";
        public bool Busy = false;
        public DateTime LastWork = DateTime.MinValue;

        [Category(Obj), DisplayName("Название"), Description("")]
        public String Name { get { return _nm; } set { if (value!="") _nm = value; } }
        
        [Category(Data), DisplayName("База Данных"), Description(""),
         Editor(typeof(DataBaseEditor), typeof(UITypeEditor))]
        public DataBase DB { get { return _db; } set { _db = value; } }
        
        [Category(Data), DisplayName("Лимит по размеру (МБ)"), Description("")]
        public int SizeLimit { get { return _sl; } set { _sl = value; } }
        
        [Category(Data), DisplayName("Лимит по количеству копий"), Description("")]
        public int CountLimit { get { return _cl; } set { _cl = value; } }
        
        [Category(Data), DisplayName("Путь к резервным копиям"),Description(""),
        Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public String BackupPath { get { return _bp; } set { if (Directory.Exists(value)) _bp = value; } }
        
        [Category(Time), DisplayName("Начало Резервирования"),Description("")]
        public DateTime StartTime { get { return _st; } set { _st = value; } }
        
        [Category(Time), DisplayName("Резервировать"), Description("")]
        public ArcType Type { get { return _tp; } set { _tp = value; } }
        
        [Category(Time), DisplayName("Повторять каждые (часов)"), Description("")]
        public int Repeat { get { return _rp; } set { _rp = value; } }
        
        public override string ToString()
        {
            return _nm;
        }

        public int IntType()
        {
                switch (this.Type)
                {
                    case ArcType.Единожды: return 1;
                    case ArcType.Ежедневно: return 2;
                    case ArcType.Еженедельно: return 3;
                    case ArcType.Ежемесячно: return 4;
                    default: return 0;
                }          
        }

        public ArchiveJob() { }

        public ArchiveJob(string guid, string name, DataBase db, string path, string start, int type, int countlimit,int sizelimit, int repeat)
        {
            this.Guid = guid;
            _nm = name;
            _db = db;           
            _bp = path;
            _st = DateTime.Parse(start);
            _tp = getAJtype(type);
            _cl = countlimit;
            _sl = sizelimit;
            _rp = repeat;
        }

        private ArcType getAJtype(int i)
        {
            switch (i)
            {
                case 1: return ArcType.Единожды;
                case 2: return ArcType.Ежедневно;
                case 3: return ArcType.Еженедельно;
                case 4: return ArcType.Ежемесячно;
                default: return ArcType.При_Запуске;
            }
        }

        /*
        public void Save(XmlNode nd,XmlDocument doc)
        {
            try
            {
                XmlElement j = doc.CreateElement("job");
                j.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(Name));
                j.AppendChild(doc.CreateElement("db")).AppendChild(doc.CreateTextNode(DB.Name));
                j.AppendChild(doc.CreateElement("path")).AppendChild(doc.CreateTextNode(BackupPath));
                j.AppendChild(doc.CreateElement("sizelim")).AppendChild(doc.CreateTextNode(SizeLimit.ToString()));
                j.AppendChild(doc.CreateElement("countlim")).AppendChild(doc.CreateTextNode(CountLimit.ToString()));
                j.AppendChild(doc.CreateElement("start")).AppendChild(doc.CreateTextNode(_st.ToString("dd.MM.yyyy HH:mm")));
                j.AppendChild(doc.CreateElement("type")).AppendChild(doc.CreateTextNode(Type.ToString()));
                j.AppendChild(doc.CreateElement("repeat")).AppendChild(doc.CreateTextNode(Repeat.ToString()));
                nd.AppendChild(j);
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Не все поля были заполненны верно");
            }
        }

        public void Load(XmlNode nd)
        {
            if (nd.Name != "job") return;
            foreach (XmlNode n in nd.ChildNodes)
            {
                String val=n.ChildNodes[0].Value;
                switch(n.Name)
                {
                    case "name": Name = val; break;
                    case "db":
                        if (val == DataBase.AllDataBases.Name) DB = DataBase.AllDataBases;
                        foreach (DataBase db in Options.Get().Databases)
                            if (db.Name == val) 
                                DB = db;
                        break;
                    case "path": BackupPath = val; break;
                    case "sizelim": SizeLimit = int.Parse(val); break;
                    case "countlim": CountLimit= int.Parse(val); break;
                    case "start": _st = DateTime.Parse(val); break;
                    case "type": Type = (ArcType)Enum.Parse(typeof(ArcType),val); break;
                    case "repeat": Repeat = int.Parse(val); break;
                }
            }
        }

        public static ArchiveJob Load(XmlNode nd, int hz)
        {
            ArchiveJob jb = new ArchiveJob();
            jb.Load(nd);
            return jb;
        }*/

        public bool DateCmpNoSec(DateTime dt)
        {
            DateTime cmp1 = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
            DateTime cmp2 = DateTime.Now;
            cmp2 = new DateTime(cmp2.Year, cmp2.Month, cmp2.Day, cmp2.Hour, cmp2.Minute, 0);
            return cmp1 == cmp2;
        }

        public bool DateCmpTime(DateTime dt)
        {
            return (dt.Hour == DateTime.Now.Hour && dt.Minute == DateTime.Now.Minute);
        }

        public bool NeedDump(bool start)
        {
            if (Busy) return false;
            if (start && Type == ArcType.При_Запуске)
                return true;
            if (Repeat>0 && DateCmpNoSec(LastWork.AddHours(Repeat)))
                return true;
            if (Type == ArcType.Единожды && DateCmpNoSec(StartTime))
                return true;
            if (Type == ArcType.Ежедневно && DateCmpTime(StartTime))
                return true;
            if (Type == ArcType.Еженедельно && ((DateTime.Now - StartTime).Days % 7) == 0 && DateCmpTime(StartTime))
                return true;
            if (Type == ArcType.Ежемесячно && StartTime.Day == DateTime.Now.Day && DateCmpTime(StartTime))
                return true;
            return false;
        }
    }

    class DataBaseCollection : List<DataBase>
    {
        public DataBase GetDataBase(string guid)
        {
            if (guid == DataBase.AllDataBases.Name)
                return DataBase.AllDataBases;
            foreach(DataBase db in this)
            {
                if (db.Guid == guid)
                    return db;
            }
            return null;
        }

        public void LoadDBs()
        {
            this.Clear();
            RabnetConfig.LoadDataSources();
            foreach (RabnetConfig.rabDataSource ds in RabnetConfig.DataSources)
            {
                RabnetConfig.sParams p = ds.Params;
                this.Add(new DataBase(ds.Guid, p.Host, p.DataBase, p.User, p.Password, ds.Name));
            }
        }

        public void SaveDBs()
        {
            foreach (DataBase db in this)
            {
                if (db.Guid == "")
                    db.Guid = System.Guid.NewGuid().ToString();                
                    RabnetConfig.SaveDataSource(db.Guid, db.Name, db.Host, db.DBName, db.User, db.Password);

            }
            ///Удаляем удаленные
            string remove = "";
            foreach (RabnetConfig.rabDataSource ds in RabnetConfig.DataSources)
            {
                bool contains = false;
                foreach (DataBase db2 in this)
                {
                    if (db2.Guid == ds.Guid)
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                    remove = ds.Guid;
            }
            if (remove != "")
                RabnetConfig.DeleteDataSource(remove);
            RabnetConfig.SaveDataSources();
        }
    }

    class ArchiveJobCollection : List<ArchiveJob> 
    {
        /// <summary>
        /// Загружает из реестра Расписания Резервирования.
        /// </summary>
        /// <param name="dbc">Коллекция Настроек Подключения к БД. 
        /// Нужна для того, чтобы не отображать Расписания из реестра тех БД, 
        /// которых не существует в передаваемой Коллекции
        /// </param>
        public void LoadAJs(DataBaseCollection dbc)
        {
            RabnetConfig.LoadArchiveJobs();
            foreach (RabnetConfig.rabArchiveJob raj in RabnetConfig.ArchiveJobs)
            {
                if (dbc.GetDataBase(raj.DBguid) != null)
                    this.Add(new ArchiveJob(raj.Guid,
                        raj.JobName,
                        dbc.GetDataBase(raj.DBguid),
                        raj.DumpPath,
                        raj.StartTime,
                        raj.Type,
                        raj.CountLimit,
                        raj.SizeLimit,
                        raj.Repeat));
            }
        }

        /// <summary>
        /// Сохранение в реестре Расписаний резервирования
        /// </summary>
        /// <param name="dbc">Коллекция Настроек Подключения к БД. 
        /// Нужна для того, чтобы удалить Расписания из реестра тех БД, 
        /// которых не существует в передаваемой Коллекции
        /// </param>
        public void SaveAJs(DataBaseCollection dbc)
        {
            ///Удаляем из THIS те Расписания, в которых указана Не существующая БД
            ArchiveJob removeAJ;
            do
            {
                removeAJ = null;
                foreach (ArchiveJob aj in this)
                {
                    if (aj.DB != DataBase.AllDataBases && !dbc.Contains(aj.DB))
                    {
                        removeAJ = aj;
                        break;
                    }
                }
                if (removeAJ != null)                
                    this.Remove(removeAJ);               
            }
            while (removeAJ != null);
            
            ///Сохраняем расписания из THIS в RabnetConfig.ArchiveJobs
            foreach (ArchiveJob aj in this)
            {
                if (aj.Guid == "" || aj.Guid == null)
                    aj.Guid = System.Guid.NewGuid().ToString();
                RabnetConfig.SaveArchiveJob(aj.Guid, aj.Name, aj.DB.Guid, aj.BackupPath, aj.StartTime.ToString(), aj.IntType(), aj.CountLimit, aj.SizeLimit, aj.Repeat);
            }

            ///Удаляем из RabnetConfig.ArchiveJobs то, что удалили из THIS
            RabnetConfig.rabArchiveJob removeRAJ;
            do
            {
                removeRAJ = null;
                foreach (RabnetConfig.rabArchiveJob raj in RabnetConfig.ArchiveJobs)
                {
                    bool contains = false;
                    //проверяем содержит ли в RabnetConfig.ArchiveJobs  элемент которого нет в THIS
                    foreach (ArchiveJob aj in this)
                    {
                        if (aj.Guid == raj.Guid)
                        {
                            contains = true;
                            break;
                        }
                    }
                    //если RabnetConfig.ArchiveJobs содержит элемент которого нет в THIS
                    if (!contains)
                    {
                        removeRAJ = raj;
                        break;
                    }
                }
                if (removeRAJ != null)
                    RabnetConfig.DeleteArchiveJob(removeRAJ.Guid);
            }
            while (removeRAJ != null);
            RabnetConfig.SaveArchiveJobs();
        }
    }


    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    class Options:Object
    {
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public enum Rubool { Да, Нет };
        private static Options _oops = null;
        const String Opt = "Настройки";
        //const String ArKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        //const String ArValue = "rabdump";
        const String Rdo = "rabdumpOptions";
        const String MYSQL_EXE = @"\bin\mysql.exe";
        const String MYSQL_DUMP = @"\bin\mysqldump.exe";
        public String MySqlExePath = "";
        public String MySqlDumpPath = "";
        private String _myPath = @"C:\Program Files\MySQL\MySQL Server 5.1", _p7 = "";
        private Rubool _sas = Rubool.Нет;

        private readonly DataBaseCollection _bds = new DataBaseCollection();
        private readonly ArchiveJobCollection _jobs = new ArchiveJobCollection();

        /// <summary>
        /// Синглтон опций
        /// </summary>
        /// <returns></returns>
        public static Options Get()
        {
            if (_oops == null)
            {
                _oops = new Options();               
                _oops.Load();
            }
            return _oops;
        }

        [Category(Opt), DisplayName("  Путь к MySQL"), Description("Путь к папке с программой MySQL Server\n\rНужен для работы с Базой Данных\n\rПуть по умолчанию: \"C:\\Program Files\\MySQL\\MySQL Server <№ версии>"),
        Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public String MySqlPath 
        { 
            get { return _myPath; } 
            set
            {
                _myPath = value;
                MySqlDumpPath= _myPath+MYSQL_DUMP;
                MySqlExePath= _myPath+MYSQL_EXE;
            }
        }       
              
        [Category(Opt), DisplayName("  7-zip"), Description("Путь к архиватору 7-zip"),
        Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public String Path7Z 
        { 
            get { return _p7; } 
            set { _p7 = value; } }
        
        [Category(Opt), DisplayName(" Базы данных"), Description("Коллекция Настроек подключения к Базам Данных")]
        public DataBaseCollection Databases { get { return _bds; }}
        
        [Category(Opt), DisplayName(" Расписание"),Description("Коллекция расписаний резервирования Баз Данных")]
        public ArchiveJobCollection Jobs { get { return _jobs; } }
        
        [Category(Opt), DisplayName("Запускать при старте системы"),Description("Запускать программу вместе с Windows")]
        public Rubool StartAtStart { get { return _sas; } set { _sas = value; } }

        /// <summary>
        /// Сохраняет ВСЕ настройки программы
        /// </summary>
        public void Save()
        {
            _bds.SaveDBs();
            _jobs.SaveAJs(_bds);
            RabnetConfig.SaveOption(RabnetConfig.OptionType.MysqlPath, _myPath);
            RabnetConfig.SaveOption(RabnetConfig.OptionType.zip7path, _p7);
            //foreach (ArchiveJob jb in Jobs)
                //jb.Save(rn, doc);
            //Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(ArKey);
            //rk.DeleteValue(ArValue, false);
            string s = "";
            if (StartAtStart == Rubool.Да)
                s = Application.StartupPath;    
            RabnetConfig.SaveOption(RabnetConfig.OptionType.rabdump_startupPath, s);
            /*MainForm.log().Debug("saving options");
            XmlDocument doc = new XmlDocument();
            XmlElement rn = doc.CreateElement(Rdo);
            doc.AppendChild(rn);
            rn.AppendChild(doc.CreateElement("mysql")).AppendChild(doc.CreateTextNode(MySqlPath));
            rn.AppendChild(doc.CreateElement("mysqldump")).AppendChild(doc.CreateTextNode(MySqlDumpPath));
            rn.AppendChild(doc.CreateElement("z7")).AppendChild(doc.CreateTextNode(Path7Z));
            foreach (DataBase db in Databases)
                db.Save(rn, doc);
            foreach (ArchiveJob jb in Jobs)
                jb.Save(rn, doc);
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.GetSection(Rdo).SectionInformation.SetRawXml(doc.OuterXml);
            conf.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(Rdo);
            RegistryKey rk=Registry.LocalMachine.CreateSubKey(ArKey);
            rk.DeleteValue(ArValue,false);
            if (StartAtStart==Rubool.Да)
                rk.SetValue(ArValue,Application.ExecutablePath);*/
        }

        /// <summary>
        /// Загружает все настройки программы
        /// </summary>
        public void Load()
        {
            MainForm.log().Debug("loading options");
            Databases.Clear();
            Jobs.Clear();
            _bds.LoadDBs();
            _jobs.LoadAJs(_bds);
            MySqlPath = RabnetConfig.GetOption(RabnetConfig.OptionType.MysqlPath);
            _p7 = RabnetConfig.GetOption(RabnetConfig.OptionType.zip7path);
            /*MainForm.log().Debug("loading options");
            if (nd.Name != Rdo) return;
            Databases.Clear();
            Jobs.Clear();
            foreach (XmlNode n in nd.ChildNodes)
            {
                switch (n.Name)
                {
                    case "db": _oops.Databases.Add(DataBase.Load(n, 0)); break;
                    case "job": _oops.Jobs.Add(ArchiveJob.Load(n, 0)); break;
                    case "z7": 
                    case "mysql": 
                    case "mysqldump": 
                        string xVal;
                        try
                        {
                            xVal = n.ChildNodes[0].Value;
                        }
                        catch
                        {
                            xVal = "";
                        }

                        switch (n.Name)
                        {
                            case "z7": Path7Z = xVal; break;
                            case "mysql": MySqlPath = xVal; break;
                            case "mysqldump": MySqlDumpPath = xVal; break;
                        }
                        break;

                }
            }*/
            StartAtStart = Rubool.Нет;
            //Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(ArKey);
            string val = RabnetConfig.GetOption(RabnetConfig.OptionType.rabdump_startupPath);
            if (val != null)
                if (val == Application.ExecutablePath)
                    StartAtStart = Rubool.Да;
        }

        /*public void Load()
        {
            Load(ConfigurationManager.GetSection(Rdo) as XmlNode);
        }*/

    }
    
}
