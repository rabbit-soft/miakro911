using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Reflection;
using rabnet.RNC;
#if PROTECTED
using RabGRD;
#endif

namespace rabdump
{
    
    class Options//:Object
    {
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public enum Rubool { Да, Нет };
        private static Options _oops = null;
        //const String Opt = "Настройки";
        const String Z7_NAME = @"7z\7za.exe";
        const String RD_OPTIONS = "rabdumpOptions";
        const String MYSQL_EXE = @"\bin\mysql.exe";
        const String MYSQL_DUMP = @"\bin\mysqldump.exe";
        //const String ArKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        //const String ArValue = "rabdump";        
        //public String MySqlExePath = "";
        //public String MySqlDumpPath = "";

        private String _myPath = "";//@"C:\Program Files\MySQL\MySQL Server 5.1"; 
        private String _p7 = "";
        private String _servUrl;
        private bool _sas = false;
        private RabnetConfig _rnc;
        //private readonly List<DataSource> _databases;
        //private readonly List<ArchiveJob> _jobs ;    

        private Options() { }

        /// <summary>
        /// Синглтон опций
        /// </summary>
        /// <returns></returns>
        public static Options Inst
        {
            get
            {
                if (_oops == null)
                {
                    _oops = new Options();
                    _oops.Load();
                }
                return _oops;
            }
        }

        public String MySqlPath
        {
            get 
            {
                if (_myPath == null || _myPath == "")
                {
                    //TODO попробывать найти mysql server в реестре либо на диске
                }
                return _myPath; 
            }
            set
            {
                if(Directory.Exists(value) )
                    _myPath = value;
            }
        }
        public String MySqlExePath {  get {   return _myPath + MYSQL_EXE; } }
        public String MySqlDumpPath { get { return _myPath + MYSQL_DUMP; } }
        public string ServerUrl { get { return _servUrl; } set { _servUrl = value; } }
        public String Path7Z
        {
            get 
            {
                if (_p7==null || _p7 == "")
                {
                    string search = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                    DirectoryInfo di = Directory.GetParent(search);
                    search = Path.Combine(di.FullName, Z7_NAME);
                    if (File.Exists(search))
                        return search;
                }
                return _p7; 
            }
            set 
            { 
                if(File.Exists(value))
                    _p7 = value; 
            }
        }
        public List<DataSource> DataSource { get { return _rnc.DataSources; } }
        public List<ArchiveJob> Jobs { get { return _rnc.ArchiveJobs; } }
        public bool StartAtStart { get { return _sas; } set { _sas = value; } }

        /// <summary>
        /// Сохраняет ВСЕ настройки программы
        /// </summary>
        public void Save()
        {
            _servUrl = prettyServer(_servUrl);
            _rnc.SaveDataSources();
            _rnc.SaveArchiveJobs();
            _rnc.SaveOption(RabnetConfig.OptionType.MysqlPath, _myPath);
            _rnc.SaveOption(RabnetConfig.OptionType.zip7path, _p7);
            _rnc.SaveOption(RabnetConfig.OptionType.serverUrl, _servUrl);
            string s = "";
            if (StartAtStart == true)
                s = Application.ExecutablePath;
            _rnc.SaveOption(RabnetConfig.OptionType.rabdump_startupPath, s);
        }

        /// <summary>
        /// Загружает все настройки программы
        /// </summary>
        public void Load()
        {
            _rnc = new RabnetConfig();
            _rnc.LoadDataSources();
            _rnc.LoadArchiveJobs();            
            MySqlPath = _rnc.GetOption(RabnetConfig.OptionType.MysqlPath);
            _p7 = _rnc.GetOption(RabnetConfig.OptionType.zip7path);
            _servUrl = _rnc.GetOption(RabnetConfig.OptionType.serverUrl);

            StartAtStart = false;

            string val = _rnc.GetOption(RabnetConfig.OptionType.rabdump_startupPath);
            if (val != null)
                if (val == Application.ExecutablePath)
                    StartAtStart = true;
        }

        internal RabnetConfig GetRabnetConfig()
        {
            return _rnc;
        }

        private string prettyServer(string result)
        {
            if (!result.StartsWith("http://")) result = "http://" + result;
            result = result.Replace('\\', '/');
            if (!result.EndsWith("/")) result += "/";
            return result;
        }

        //private void loadDBs()
        //{
        //    Databases.Clear();
        //    _rnc.LoadDataSources();
        //    foreach (DataSource ds in _rnc.DataSources)
        //    {
        //        sParams p = ds.Params;
        //        DataSource db = new DataSource(ds.Guid, p.Host, p.DataBase, p.User, p.Password, ds.Name);
        //        db.WebReport = ds.WebReport;// ? Options.Rubool.Да : Options.Rubool.Нет;
        //        Databases.Add(db);
        //    }
        //}

        //private void saveDBs()
        //{
        //    foreach (DataSource db in Databases)
        //    {
        //        if (db.Guid == "")
        //            db.Guid = System.Guid.NewGuid().ToString();
        //        DataSource newDS = new DataSource(db.Guid, db.Name, db.Params.Host, db.Params.DataBase, db.Params.User, db.Params.Password);
        //        newDS.WebReport = db.WebReport ;//== Options.Rubool.Да;
        //        _rnc.SaveDataSource(newDS);

        //    }
        //    ///Удаляем удаленные
        //    string remove = "";
        //    foreach (DataSource ds in _rnc.DataSources)
        //    {
        //        bool contains = false;
        //        foreach (DataSource db2 in Databases)
        //        {
        //            if (db2.Guid == ds.Guid)
        //            {
        //                contains = true;
        //                break;
        //            }
        //        }
        //        if (!contains)
        //            remove = ds.Guid;
        //    }
        //    if (remove != "")
        //        _rnc.DeleteDataSource(remove);
        //    _rnc.SaveDataSources();
        //}

        //private DataSource getDataBase(string guid)
        //{
        //    //if (guid == DataBase.AllDataBases.Name)
        //    //    return DataBase.AllDataBases;
        //    foreach (DataSource db in Databases)
        //    {
        //        if (db.Guid == guid)
        //            return db;
        //    }
        //    return null;
        //}

        /// <summary>
        /// Загружает из реестра Расписания Резервирования.
        /// </summary>
        /// <param name="dbc">Коллекция Настроек Подключения к БД. 
        /// Нужна для того, чтобы не отображать Расписания из реестра тех БД, 
        /// которых не существует в передаваемой Коллекции
        /// </param>
        //private void loadAJs()
        //{
        //    _rnc.LoadArchiveJobs();
        //    foreach (ArchiveJob raj in _rnc.ArchiveJobs)
        //    {
        //        DataSource ds =  getDataBase(raj.DataSrc.Guid);
        //        if (ds == null) continue;
        //        ArchiveJobs.Add(new ArchiveJob(raj.Guid,
        //            raj.JobName,
        //            ds,
        //            raj.DumpPath,
        //            raj.StartTime.ToShortDateString(),
        //            raj.ArcType,
        //            raj.CountLimit,
        //            raj.SizeLimit,
        //            raj.Repeat,
        //            raj.ServTime.ToShortDateString(), raj.ServType));
        //    }
        //}

        /// <summary>
        /// Сохранение в реестре Расписаний резервирования
        /// </summary>
        /// <param name="dbc">Коллекция Настроек Подключения к БД. 
        /// Нужна для того, чтобы удалить Расписания из реестра тех БД, 
        /// которых не существует в передаваемой Коллекции
        /// </param>
        //private void saveAJs(List<DataSource> dbc)
        //{
        //    ///Удаляем из THIS те Расписания, в которых указана Не существующая БД
        //    ArchiveJob removeAJ;
        //    do
        //    {
        //        removeAJ = null;
        //        foreach (ArchiveJob aj in _jobs)
        //        {
        //            if (/*aj.DataSrc != DataBase.AllDataBases &&*/ !dbc.Contains(aj.DataSrc))
        //            {
        //                removeAJ = aj;
        //                break;
        //            }
        //        }
        //        if (removeAJ != null)
        //            _jobs.Remove(removeAJ);
        //    }
        //    while (removeAJ != null);

        //    ///Сохраняем расписания из THIS в RabnetConfig.ArchiveJobs
        //    foreach (ArchiveJob aj in _jobs)
        //    {
        //        if (aj.Guid == "" || aj.Guid == null)
        //            aj.Guid = System.Guid.NewGuid().ToString();
        //        //_rnc.SaveArchiveJob(aj.Guid, aj.JobName, aj.DataSrc.Guid, aj.DumpPath, aj.StartTime.ToString(), aj.IntType(), aj.CountLimit, aj.SizeLimit, aj.Repeat, aj.ServTime.ToString(), aj.ServType);
        //    }

        //    ///Удаляем из RabnetConfig.ArchiveJobs то, что удалили из THIS
        //    ArchiveJob removeRAJ;
        //    do
        //    {
        //        removeRAJ = null;
        //        foreach (ArchiveJob raj in _rnc.ArchiveJobs)
        //        {
        //            bool contains = false;
        //            //проверяем содержит ли в RabnetConfig.ArchiveJobs  элемент которого нет в THIS
        //            foreach (ArchiveJob aj in _jobs)
        //            {
        //                if (aj.Guid == raj.Guid)
        //                {
        //                    contains = true;
        //                    break;
        //                }
        //            }
        //            //если RabnetConfig.ArchiveJobs содержит элемент которого нет в THIS
        //            if (!contains)
        //            {
        //                removeRAJ = raj;
        //                break;
        //            }
        //        }
        //        if (removeRAJ != null)
        //            _rnc.DeleteArchiveJob(removeRAJ.Guid);
        //    }
        //    while (removeRAJ != null);
        //    _rnc.SaveArchiveJobs();
        //}



    }
}
