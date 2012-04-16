using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Reflection;
#if PROTECTED
using RabGRD;
#endif

namespace rabdump
{
    #region myeditors

    /// <summary>
    /// Предпосылками для создания служило то,
    /// что по умолчанию в форме Редактора Расписаний
    /// Text выглядел как "blabla Collection Editor"
    /// </summary>
    class MyCollectionEditor : CollectionEditor
    {
        protected MyCollectionEditor(Type type) : base(type) { }
        private string _text = "";
        protected string Caption
        {
            set { _text = value; }
        }
        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm collectionForm = base.CreateCollectionForm();
            Form frmCollectionEditorForm = collectionForm as Form;
            frmCollectionEditorForm.Text = _text;


            return collectionForm;
        }
    }

    class DBce : MyCollectionEditor
    {
        public DBce(Type type): base(type){Caption = "Коллекция Баз Данных";}
    }

    class AJce : MyCollectionEditor
    {
        public AJce(Type type) : base(type) { Caption = "Коллекция Расписаний резервирования на локальном компьютере"; }
    }
       
    class CollectionTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,object value, Type destType)
        {
            return "Список...";
        }
    }

    #endregion

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    class DataBase : Object
    {
        const string ALL_DB = "[ВСЕ]";
        const string DB = "База Данных";
        const string Obj = " Основное";
        private string _db, _host, _user, _pswd, _nm = "Ферма";
        private Options.Rubool _wr = Options.Rubool.Нет;

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
        [Category("Дополнительное"), DisplayName("Отправлять статистику"), Description("Отправлять статистику о Ферме на удаленный сервер. Стаститику можно просматривать на сайте.")]
        public Options.Rubool WebReport { get { return _wr; } 
            set { 
#if PROTECTED
                if(GRD.Instance.GetFlag(GRD.FlagType.WebReports))
#endif
                _wr = value; } }

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

       
    }

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    class ArchiveJob : Object
    {
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public enum ArcType { При_Запуске, Единожды, Ежедневно, Еженедельно, Ежемесячно, Никогда };
        const string Obj = " Резервирование";
        const string Data = "Данные";
        const string Time = "Расписание";
        const string Serv = "Удаленное резервирование";
        private string _nm = "Расписание", _bp = "C:\\";
        private int _sl, _cl, _rp;
        private DataBase _db = DataBase.AllDataBases;
        private DateTime _st;
        private ArcType _tp;
        public string Guid ="";
        public bool Busy = false;
        private DateTime _servTime = DateTime.Parse("18:00");
        private ArcType _servType = ArcType.Никогда;
        public DateTime LastWork = DateTime.MinValue;

        [Browsable(false)]
        public DateTime ServDateTime { get { return _servTime; } }

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
        public ArcType arcType { get { return _tp; } set { _tp = value; } }
        
        [Category(Time), DisplayName("Повторять каждые (часов)"), Description("")]
        public int Repeat { get { return _rp; } set { _rp = value; } }

        [Category(Serv), DisplayName("Время"), Description("Врямя отправки резервных копий баз данных на сервер.")]
        public string ServTime
        {
            get { return _servTime.ToString("HH:mm"); }
            set
            {
                try
                {
                    _servTime = DateTime.Parse(value);
                }
                catch { }
            }
        }

        [Category(Serv), DisplayName("Оправлять"), Description("Как часто отправлять РКБД на удаленный сервер")]
        public ArcType ServType { get { return _servType; } set { _servType = value; } }

        
        public override string ToString()
        {
            return _nm;
        }

        public ArchiveJob() 
        {
        }

        public ArchiveJob(string guid, string name, DataBase db, string path, string start, int type, int countlimit,int sizelimit, int repeat,string servTime,int servType):this()
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
            _servTime = DateTime.Parse(servTime);
            _servType = getAJtype(servType);
        }

        private ArcType getAJtype(int i)
        {
            switch (i)
            {
                case 0: return ArcType.При_Запуске;
                case 1: return ArcType.Единожды;
                case 2: return ArcType.Ежедневно;
                case 3: return ArcType.Еженедельно;
                case 4: return ArcType.Ежемесячно;
                default: return ArcType.Никогда;
            }
        }

        public int IntType()
        {
            switch (this.arcType)
            {
                case ArcType.При_Запуске: return 0;
                case ArcType.Единожды: return 1;
                case ArcType.Ежедневно: return 2;
                case ArcType.Еженедельно: return 3;
                case ArcType.Ежемесячно: return 4;
                default: return 5;
            }
        }

        public int IntServType()
        {
            switch (this.ServType)
            {
                case ArcType.При_Запуске: return 0;
                case ArcType.Единожды: return 1;
                case ArcType.Ежедневно: return 2;
                case ArcType.Еженедельно: return 3;
                case ArcType.Ежемесячно: return 4;
                default: return 5;
            }
        }

        /// <summary>
        /// Сейчас ли переданное дата и время
        /// </summary>
        /// <param name="dt">Проверяемое время</param>
        /// <returns></returns>
        public bool DateCmpNoSec(DateTime dt)
        {
            DateTime cmp1 = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
            DateTime cmp2 = DateTime.Now;
            cmp2 = new DateTime(cmp2.Year, cmp2.Month, cmp2.Day, cmp2.Hour, cmp2.Minute, 0);
            return cmp1 == cmp2;
        }

        /// <summary>
        /// Сейчас ли переданное время
        /// </summary>
        public bool DateCmpTime(DateTime dt)
        {
            return (dt.Hour == DateTime.Now.Hour && dt.Minute == DateTime.Now.Minute);
        }

        /// <summary>
        /// Нужно ли делать Резервирование по данному расписанию
        /// </summary>
        /// <param name="start">Старт ли программы</param>
        public bool NeedDump(bool start)
        {
            if (Busy) return false;
            if (arcType == ArcType.Никогда)
                return false;
            if (start && arcType == ArcType.При_Запуске)
                return true;
            if (Repeat>0 && DateCmpNoSec(LastWork.AddHours(Repeat)))
                return true;
            if (arcType == ArcType.Единожды && DateCmpNoSec(StartTime))
                return true;
            if (arcType == ArcType.Ежедневно && DateCmpTime(StartTime))
                return true;
            if (arcType == ArcType.Еженедельно && ((DateTime.Now - StartTime).Days % 7) == 0 && DateCmpTime(StartTime))
                return true;
            if (arcType == ArcType.Ежемесячно && StartTime.Day == DateTime.Now.Day && DateCmpTime(StartTime))
                return true;
            return false;
        }
        /// <summary>
        /// Залить ли на сервер РКБД
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public bool NeedServDump(bool start)
        {
            if (ServType == ArcType.Никогда)
                return false;
            if (start && ServType == ArcType.При_Запуске)
                return true;
            if (ServType == ArcType.Единожды && DateCmpNoSec(_servTime))
                return true;
            if (ServType == ArcType.Ежедневно && DateCmpTime(_servTime))
                return true;
            if (ServType == ArcType.Еженедельно && ((DateTime.Now - _servTime).Days % 7) == 0 && DateCmpTime(_servTime))
                return true;
            if (ServType == ArcType.Ежемесячно && _servTime.Day == DateTime.Now.Day && DateCmpTime(_servTime))
                return true;
            return false;
        }

        private bool checkTime(string val)
        {
            const int maxHour = 23;
            const int maxMint = 59;
            if (val == "") return false;
            if (!val.Contains(":")) return false;
            string[] ham = val.Split(':');
            try
            {
                if(int.Parse(ham[0])>maxHour) return false;
                if(int.Parse(ham[1])>maxMint) return false;
            }
            catch (FormatException) { return false; }
            return true;
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
                DataBase db = new DataBase(ds.Guid, p.Host, p.DataBase, p.User, p.Password, ds.Name);
                db.WebReport = ds.WebReport ?Options.Rubool.Да:Options.Rubool.Нет;
                this.Add(db);
            }
        }

        public void SaveDBs()
        {
            foreach (DataBase db in this)
            {
                if (db.Guid == "")
                    db.Guid = System.Guid.NewGuid().ToString();
                RabnetConfig.rabDataSource newDS= new RabnetConfig.rabDataSource(db.Guid, db.Name, db.Host, db.DBName, db.User, db.Password);
                newDS.WebReport =  db.WebReport == Options.Rubool.Да;
                RabnetConfig.SaveDataSource(newDS);

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
                        raj.Repeat,
                        raj.ServTime,raj.ServType));
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
                RabnetConfig.SaveArchiveJob(aj.Guid, aj.Name, aj.DB.Guid, aj.BackupPath, aj.StartTime.ToString(), aj.IntType(), aj.CountLimit, aj.SizeLimit, aj.Repeat,aj.ServDateTime.ToString(),aj.IntServType());
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

        [Category(Opt), DisplayName(" Базы данных"), Description("Коллекция Настроек подключения к Базам Данных"), Editor(typeof(DBce), typeof(UITypeEditor)), TypeConverter(typeof(CollectionTypeConverter))]
        public DataBaseCollection Databases { get { return _bds; }}

        [Category(Opt), DisplayName(" Расписания резервирования"), Description("Коллекция расписаний резервирования Баз Данных на локальном компьютере"), Editor(typeof(AJce), typeof(UITypeEditor)), TypeConverter(typeof(CollectionTypeConverter))]
        public ArchiveJobCollection Jobs { get { return _jobs; } }

        [Category(Opt), DisplayName("Запускать при старте системы"), Description("Запускать программу вместе с Windows")]
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
            string s = "";
            if (StartAtStart == Rubool.Да)
                s = Application.ExecutablePath;    
            RabnetConfig.SaveOption(RabnetConfig.OptionType.rabdump_startupPath, s);
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
            
            StartAtStart = Rubool.Нет;
            
            string val = RabnetConfig.GetOption(RabnetConfig.OptionType.rabdump_startupPath);
            if (val != null)
                if (val == Application.ExecutablePath)
                    StartAtStart = Rubool.Да;
        }

    }
    
}
