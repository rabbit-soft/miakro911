#if DEBUG
#define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Xml;
using System.Configuration;
using log4net;

[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
public static class RabnetConfig
{
    /// <summary>
    /// Представлет собой Параметры с подключениями к БД
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class sParams
    {
        public readonly string Host;
        public readonly string DataBase;
        public readonly string User;
        public readonly string Password;
        public readonly string Charset = "utf8";

        public sParams(string host, string database, string user, string pwd)
        {
            this.Host = host;
            this.DataBase = database;
            this.User = user;
            this.Password = pwd;
        }

        public sParams(string connectionString)
        {
            string[] prms = connectionString.Split(';');
            foreach (string pair in prms)
            {
                switch (pair.Split('=')[0])
                {
                    case "host": this.Host = pair.Split('=')[1]; break;
                    case "database": this.DataBase = pair.Split('=')[1]; break;
                    case "uid": this.User= pair.Split('=')[1]; break;
                    case "pwd": this.Password = pair.Split('=')[1]; break; 
                }
            }
            /*this.Host = str[0].Split('=')[1];
            this.User = str[1].Split('=')[1];
            this.Password = str[2].Split('=')[1];
            this.DataBase = str[3].Split('=')[1];  
            host=localhost;database=krol;uid=kroliki3;pwd=kroliki   */     
        }

        public override string ToString()
        {
            return String.Format("host={0};database={1};uid={2};pwd={3};charset={4}", Host, DataBase, User, Password, Charset);
        }

    }

    public enum OptionType {MysqlPath,zip7path,rabdump_startupPath,serverUrl }

    private static readonly ILog _logger = LogManager.GetLogger(typeof(RabnetConfig));

    public const string STARTUP = @"Software\Microsoft\Windows\CurrentVersion\Run";
    public const string REGISTRY_PATH = @"Software\9-Bits\Miakro911";
    public const string DATASOURCES_PATH = REGISTRY_PATH+ "\\datasources";
    public const string ARCHIVEJOBS_PATH = REGISTRY_PATH + "\\archivejobs";

    private static bool _extracting = false;
    private const string ALL_DB = "[ВСЕ]";

    public static string GetOption(OptionType optionType)
    {
        RegistryKey k = Registry.LocalMachine.CreateSubKey(REGISTRY_PATH);
        switch (optionType)
        {
            case OptionType.MysqlPath: return (string)k.GetValue("mysql");
            case OptionType.zip7path: return (string)k.GetValue("z7");
            case OptionType.rabdump_startupPath:
                k = Registry.LocalMachine.CreateSubKey(STARTUP);
                return (string)k.GetValue("rabdump", "");
            case OptionType.serverUrl:
                return (string)k.GetValue("sUrl","");
        }
        return "";
    }

    public static void SaveOption(OptionType optionType, string val)
    {
        if (val == null) val = "";
        RegistryKey k = Registry.LocalMachine.CreateSubKey(REGISTRY_PATH);
        switch (optionType)
        {
            case OptionType.MysqlPath: k.SetValue("mysql", val); break;
            case OptionType.zip7path: k.SetValue("z7", val); break;
            case OptionType.rabdump_startupPath:
                k = Registry.LocalMachine.CreateSubKey(STARTUP);
                if (val == "") k.DeleteValue("rabdump", false);
                else k.SetValue("rabdump",val);
                break;
            case OptionType.serverUrl:
                k.SetValue("sUrl", val); break;
        }
    }

#region arcjobs

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class rabArchiveJob
    {
        public string Guid;
        public string JobName;
        public string DBguid;
        public string DumpPath;
        public string StartTime;
        public int Type;
        public int CountLimit;
        public int SizeLimit;
        public int Repeat;
        public string ServTime;
        public int ServType;

        public rabArchiveJob(string guid, string name, string db_duid, string path, string start, int type, int countlimit, int sizelimit, int repeat,string stm, int stp)
        {
            this.Guid = guid;
            this.JobName = name;
            this.DBguid = db_duid;
            this.DumpPath = path;
            this.StartTime = start;
            this.Type = type;
            this.CountLimit = countlimit;
            this.SizeLimit = sizelimit;
            this.Repeat = repeat;
            this.ServTime = stm;
            this.ServType = stp;
        }
    }

    private static List<rabArchiveJob> _archiveJobs = new List<rabArchiveJob>();

    public static List<rabArchiveJob> ArchiveJobs
    {
        get { return _archiveJobs; }
    }

    public static void LoadArchiveJobs()
    {
        _logger.Info("loading archiveJobs");
        _archiveJobs.Clear();
        RegistryKey rKey = Registry.LocalMachine.CreateSubKey(ARCHIVEJOBS_PATH);
        foreach (string s in rKey.GetSubKeyNames())
        {
            RegistryKey r = Registry.LocalMachine.CreateSubKey(RabnetConfig.ARCHIVEJOBS_PATH + "\\" + s);
            _archiveJobs.Add(new rabArchiveJob(s,
                    (string)r.GetValue("name"),
                    (string)r.GetValue("db"),                   
                    (string)r.GetValue("path"),
                    (string)r.GetValue("start"),
                    (int)r.GetValue("type"),
                    (int)r.GetValue("cntlimit"),
                    (int)r.GetValue("szlimit"),
                    (int)r.GetValue("repeat"),
                    (string)r.GetValue("srvtm",DateTime.Now.ToString("yyyy-MM-dd HH:mm")),
                    (int)r.GetValue("srvtp",5)));
        }
        _logger.Info("loading archiveJobs finish");
    }

    /// <summary>
    /// Сохраняет в реестре все расписания
    /// </summary>
    public static void SaveArchiveJobs()
    {
        _logger.Info("saving archiveJobs");
        RegistryKey rKey = Registry.LocalMachine.CreateSubKey(RabnetConfig.ARCHIVEJOBS_PATH);
        foreach (rabArchiveJob raj in _archiveJobs)
        {
            //if (raj.Guid == "" || raj.Guid == null)
                //raj.Guid = System.Guid.NewGuid().ToString();
            RegistryKey r = Registry.LocalMachine.CreateSubKey(RabnetConfig.ARCHIVEJOBS_PATH + "\\" + raj.Guid);
            r.SetValue("name", raj.JobName, RegistryValueKind.String);
            r.SetValue("db", raj.DBguid, RegistryValueKind.String);           
            r.SetValue("path", raj.DumpPath, RegistryValueKind.String);
            r.SetValue("start", raj.StartTime, RegistryValueKind.String);
            r.SetValue("type", raj.Type, RegistryValueKind.DWord);
            r.SetValue("cntlimit", raj.CountLimit, RegistryValueKind.DWord);
            r.SetValue("szlimit", raj.SizeLimit, RegistryValueKind.DWord);
            r.SetValue("repeat", raj.Repeat, RegistryValueKind.DWord);
            r.SetValue("srvtm", raj.ServTime, RegistryValueKind.String);
            r.SetValue("srvtp", raj.ServType, RegistryValueKind.DWord);
        }
        ///Удаляем удаленные Расписания
        foreach (string s in rKey.GetSubKeyNames())
        {
            RegistryKey r = Registry.LocalMachine.CreateSubKey(RabnetConfig.ARCHIVEJOBS_PATH + "\\" + s);
            string dbName = (string)r.GetValue("db");
            bool contains = false;
            foreach (rabArchiveJob raj in _archiveJobs)
            {
                if (raj.Guid == s)
                {
                    contains = true;
                    break;
                }
            }
            if (!contains)
                rKey.DeleteSubKey(s);
        }
        _logger.Info("saving archiveJobs finish");
    }

    /// <summary>
    /// Сохраняет одно расписание в списке ArchiveJobs
    /// </summary>
    public static void SaveArchiveJob(string guid, string name, string db_guid, string path, string start, int type, int countlimit, int sizelimit, int repeat,string srvtm,int srvtp)
    {
        rabArchiveJob newAJ = new rabArchiveJob(guid, name, db_guid, path, start, type, countlimit,sizelimit, repeat,srvtm,srvtp);
        if (!containsArchiveJob(newAJ.Guid))
            _archiveJobs.Add(newAJ);
        else
            changeArchiveJob(newAJ);
    }

    public static void DeleteArchiveJob(string guid)
    {
        rabArchiveJob remove = null;//удаять из коллекции во время совершения цикла нельзя
        foreach (rabArchiveJob aj in _archiveJobs)
        {
            if (aj.Guid == guid)
            {
                remove = aj;
                break;
            }
        }
        if (remove != null)
            _archiveJobs.Remove(remove);
    }

    private static void changeArchiveJob(rabArchiveJob newAJ)
    {
        foreach (rabArchiveJob raj in _archiveJobs)
        {
            if (raj.Guid == newAJ.Guid)
            {
                raj.JobName = newAJ.JobName;
                raj.DBguid = newAJ.DBguid;
                raj.DumpPath = newAJ.DumpPath;
                raj.StartTime = newAJ.StartTime;
                raj.Type = newAJ.Type;
                raj.CountLimit = newAJ.CountLimit;
                raj.SizeLimit = newAJ.SizeLimit;
                raj.Repeat = newAJ.Repeat;
                raj.ServTime = newAJ.ServTime;
                raj.ServType = newAJ.ServType;
            }
        }
    }

    private static bool containsArchiveJob(string guid)
    {
        foreach (rabArchiveJob raj in _archiveJobs)
        {
            if (raj.Guid == guid)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Если в реестре имеется похожее расписание, возвращает GUID.
    /// Возвращает пустую строку если нет похожих.
    /// </summary>
    /// <param name="aj">Расписание с которым сравнить</param>
    /// <returns>GUID похожего расписания</returns>
    private static string compareArchivejobs(rabArchiveJob aj)
    {
        List<string> dbguids = getGuidsByDBName(aj.DBguid);//вместо DBguid передается название БД
        if (aj.DBguid != ALL_DB && dbguids.Count == 0) return "";

        foreach (rabArchiveJob raj in _archiveJobs)
        {
            if (aj.DBguid != ALL_DB)
            {
                bool sameDBguids = false;
                foreach (string g in dbguids)
                {
                    if (raj.DBguid == g)
                    {
                        sameDBguids = true;
                        break;
                    }
                }
                if (!sameDBguids) continue;
            }
            if (raj.JobName == aj.JobName &&
                raj.DumpPath == aj.DumpPath &&
                raj.CountLimit == aj.CountLimit &&
                raj.SizeLimit == aj.SizeLimit &&
                raj.Type == aj.Type &&
                raj.StartTime == aj.StartTime &&
                raj.Repeat == aj.Repeat)
                return raj.Guid;
        }
        return "";
    }

#endregion arcjobs


#region datasources

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class rabDataSource
    {
        /// <summary>
        /// Уникальный номер Настройки. Нужен чтобы определять папку в реестре
        /// </summary>
        public string Guid = "";
        /// <summary>
        /// Отображаемое имя Базы Данных
        /// </summary>
        public String Name;
        /// <summary>
        /// Тип базы данных.Постоянно 'db.mysql'.
        /// </summary>
        public String Type = "db.mysql";
        /// <summary>
        /// Параметры подключения
        /// </summary>
        public sParams Params;
        /// <summary>
        /// Отображать ли БД в выпадающем списке
        /// </summary>
        public bool Hidden = false;
        /// <summary>
        /// Выбирать ли базу данных по умолчанию
        /// </summary>
        public bool Default = false;
        public String DefUser = "";
        public String DefPassword = "";
        public bool SavePassword = false;

        public rabDataSource(string guid, string name, string type, string param)
        {
            this.Guid = guid;
            this.Name = name;
            this.Type = type;
            this.Params = new sParams(param);
        }

        public rabDataSource(string guid, string name, string p_host, string p_db, string p_user, string p_password)
        {
            this.Guid = guid;
            this.Name = name;
            this.Params = new sParams(p_host, p_db, p_user, p_password);
        }

        public void setDefault(String uname, String pswd)
        {
            Default = true;
            foreach (rabDataSource ds in _dataSources)
                if (ds != this) ds.Default = false;
            DefUser = uname;
            if (!SavePassword) DefPassword = "";
            else DefPassword = pswd;
            RabnetConfig.SaveDataSources();
        }

        public override string ToString()
        {
            return String.Format("name={0};hid={1};def={2};du={3};dp={4};sp={5}; params={6}",
                Name,
                Hidden?"1":"0",
                Default?"1":"0",
                DefUser,
                DefPassword,
                SavePassword?"1":"0",
                Params.ToString());
        }
    }

    private static List<rabDataSource> _dataSources = new List<rabDataSource>();

    public static List<rabDataSource> DataSources
    {
        get { return _dataSources; }
    }

    /// <summary>
    /// Загружает настройки подключения к БД
    /// </summary>
    public static void LoadDataSources()
    {
        _logger.Info("loading DataSources");
        if (!_extracting) 
            ExtractConfig(System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath);
        _dataSources.Clear();
        RegistryKey rKey = Registry.LocalMachine.CreateSubKey(DATASOURCES_PATH);
        foreach (string guid in rKey.GetSubKeyNames())
        {
            RegistryKey k = Registry.LocalMachine.CreateSubKey(DATASOURCES_PATH + "\\" + guid);
            rabDataSource ds = new rabDataSource(guid,(string)k.GetValue("name"), (string)k.GetValue("type"), (string)k.GetValue("params"));
            ds.Default = (string)k.GetValue("def", false) == true.ToString();
            ds.Hidden = (string)k.GetValue("hidden", false) == true.ToString();
            ds.SavePassword = (string)k.GetValue("savepass", false) == true.ToString();
            ds.DefUser = (string)k.GetValue("defuser");
            ds.DefPassword = (string)k.GetValue("defpass");
            _dataSources.Add(ds);
        }
        _logger.Info("loading DataSources finish");
    }

    /// <summary>
    /// Сохраняет настройки подключения к БД
    /// </summary>
    public static void SaveDataSources()
    {
        _logger.Info("saving DataSources");
        RegistryKey rKey = Registry.LocalMachine.CreateSubKey(DATASOURCES_PATH);
        List<string> noDeleted = new List<string>();
        foreach (rabDataSource ds in _dataSources)
        {
            if (ds.Guid == "") 
                ds.Guid = System.Guid.NewGuid().ToString();
            RegistryKey k = Registry.LocalMachine.CreateSubKey(DATASOURCES_PATH + "\\" + ds.Guid);
            k.SetValue("name", ds.Name, RegistryValueKind.String);
            k.SetValue("type", ds.Type, RegistryValueKind.String);
            k.SetValue("params", ds.Params.ToString(), RegistryValueKind.String);
            k.SetValue("def", ds.Default, RegistryValueKind.Unknown);
            k.SetValue("hidden", ds.Hidden, RegistryValueKind.Unknown);
            k.SetValue("savepass", ds.SavePassword,RegistryValueKind.Unknown);
            k.SetValue("defuser", ds.DefUser, RegistryValueKind.String);
            k.SetValue("defpass", ds.DefPassword, RegistryValueKind.String);
            noDeleted.Add(ds.Guid);
        }
        //Удаляем из реестра
        foreach (string guid in rKey.GetSubKeyNames())
            if (!noDeleted.Contains(guid))
                rKey.DeleteSubKey(guid);
        _logger.Info("saving DataSources finish");
    }
   
    /// <summary>
    /// Удаляет DataSource по Guid
    /// </summary>
    /// <param name="p">Guid - уникальный номер</param>
    public static void DeleteDataSource(string guid)
    {
        rabDataSource remove = null;//удаять из коллекции во время совершения цикла нельзя
        foreach (rabDataSource ds in _dataSources)
        {
            if (ds.Guid == guid)
            {
                remove = ds;
                break;
            }
        }
        if (remove != null)
        {
            _logger.Debug("delete datasource "+remove.Name);
            _dataSources.Remove(remove);
        }
    }

    /// <summary>
    /// Сохраняет данные подключения к бд.
    /// Если такая запись есть, то изменяет.
    /// Если нет, то добавляет
    /// </summary>
    public static void SaveDataSource(string guid,string name, string host,string db,string user,string password)
    {
        rabDataSource newDS = new rabDataSource(guid, name, host, db, user, password);
        if (!containsDataSource(newDS.Guid))//если не содержит такого ДС, то добавляем
            _dataSources.Add(newDS);
        else
            changeDataSource(newDS);
    }

    /// <summary>
    /// Имеются ли Настройки подключения к БД
    /// </summary>
    public static bool HaveDataSources()
    {
        RabnetConfig.LoadDataSources();
        if (_dataSources.Count > 0)
            return true;
        else return false;
    }

    private static void changeDataSource(rabDataSource newDS)
    {
        foreach (rabDataSource rds in _dataSources)
        {
            if(rds.Guid == newDS.Guid)
            {
                rds.Name = newDS.Name;
                rds.Params = newDS.Params;
            }
        }
    }

    private static bool containsDataSource(string guid)
    {
        foreach (rabDataSource ds in _dataSources)
        {
            if (ds.Guid == guid)
                return true;
        }
        return false;
    }

    private static string compareDataSource(rabDataSource ds)
    {
        foreach (rabDataSource rds in _dataSources)
        {
            if (rds.Name == ds.Name && rds.Params.ToString() == ds.Params.ToString())
                return rds.Guid;
        }
        return "";
    }

    /// <summary>
    /// Возвращает Все Guid Подключений к БД, имеющих заданное имя
    /// </summary>
    /// <param name="name">Имя подключения</param>
    /// <returns>Список Guid'ов</returns>
    private static List<string> getGuidsByDBName(string name)
    {
        List<string> result = new List<string>();
        foreach (rabDataSource rda in _dataSources)
        {
            if (rda.Name == name)
                result.Add(rda.Guid);
        }
        return result;
    }

#endregion datasources


    /// <summary>
    /// Выдирает из app.config настройки Подключения к БД и Расписания резервирования.
    /// Сохраняет полученные настройки в реестр.
    /// </summary>
    public static void ExtractConfig(string filePath)
    {       
        if (!System.IO.File.Exists(filePath)) return;
        _logger.Info("extracting configs from app.configs");
        _extracting = true;
        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);
        XmlNode rootNode = doc.FirstChild.NextSibling; 
        foreach (XmlNode xnd in rootNode.ChildNodes)
        {
            if (xnd.Name == "configSections")
            {
                XmlNode remove = null;
                foreach (XmlNode nd in xnd.ChildNodes)
                {
                    if (nd.Attributes["name"].Value == "rabnetds" || nd.Attributes["name"].Value == "rabdumpOptions")
                        remove = nd;
                }
                if (remove != null)
                    xnd.RemoveChild(remove);
            }
            else if (xnd.Name == "rabnetds")
            {
                extractRabnetds(xnd);
                rootNode.RemoveChild(xnd);
            }
            else if (xnd.Name == "rabdumpOptions")
            {
                extractRabDump(xnd);
                rootNode.RemoveChild(xnd);
            }
        }
#if !DEBUG
        doc.Save(filePath);
#endif
        _logger.Info("finish extracting");
        //_extracting = false; //один раз за сессию и хватит
    }

    private static void extractRabnetds(XmlNode node)
    {
        _logger.Info("extracting config from RabNet");
        LoadDataSources();
        foreach (XmlNode nd in node.ChildNodes)
        {
            if (nd.Name == "dataSource")
            {
                rabDataSource td = new rabDataSource(System.Guid.NewGuid().ToString(), nd.Attributes.GetNamedItem("name").Value,
                    nd.Attributes.GetNamedItem("type").Value, nd.Attributes.GetNamedItem("param").Value);
                if (nd.Attributes.GetNamedItem("default") != null)
                    td.Default = (nd.Attributes.GetNamedItem("default").Value == "1");
                if (nd.Attributes.GetNamedItem("savepassword") != null)
                    td.SavePassword = (nd.Attributes.GetNamedItem("savepassword").Value == "1");
                if (nd.Attributes.GetNamedItem("hidden") != null)
                    td.Hidden = (nd.Attributes.GetNamedItem("hidden").Value == "1");
                if (nd.Attributes.GetNamedItem("user") != null)
                    td.DefUser = nd.Attributes.GetNamedItem("user").Value;
                if (nd.Attributes.GetNamedItem("password") != null)
                    td.DefPassword = nd.Attributes.GetNamedItem("password").Value;
                if (compareDataSource(td) == "")
                    _dataSources.Add(td);
            }          
        }
        SaveDataSources();
    }

    private static void extractRabDump(XmlNode node)
    {
        _logger.Info("extracting congig from RabDump");
        LoadDataSources();
        LoadArchiveJobs();
        foreach (XmlNode nd in node.ChildNodes)
        {
            switch (nd.Name)
            {
                case "mysql":
                    SaveOption(OptionType.MysqlPath, nd.InnerText.Replace(@"\bin\mysql.exe", ""));
                    break;
                case "z7":
                    SaveOption(OptionType.zip7path, nd.InnerText);
                    break;
                case "db":                   
                    rabDataSource db = new rabDataSource("",
                        nd.SelectSingleNode("name").InnerText,
                        nd.SelectSingleNode("host").InnerText,
                        nd.SelectSingleNode("db").InnerText,
                        nd.SelectSingleNode("user").InnerText,
                        nd.SelectSingleNode("password").InnerText);
                    if (compareDataSource(db) == "")
                        _dataSources.Add(db);

                    break;
                case "job":                  
                    rabArchiveJob aj = new rabArchiveJob("",
                        nd.SelectSingleNode("name").InnerText,
                        nd.SelectSingleNode("db").InnerText,
                        nd.SelectSingleNode("path").InnerText,
                        nd.SelectSingleNode("start").InnerText,
                        getAJTypeInt(nd.SelectSingleNode("type").InnerText),
                        int.Parse(nd.SelectSingleNode("countlim").InnerText),
                        int.Parse(nd.SelectSingleNode("sizelim").InnerText),
                        int.Parse(nd.SelectSingleNode("repeat").InnerText),
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm"),5);
                    if (aj.DBguid == "[все]")                  
                        aj.DBguid = ALL_DB;

                    if (compareArchivejobs(aj) == "")//если не имеется идентичных Расписаний
                    {
                        if (aj.DBguid != ALL_DB)
                        {   //назначаем расписанию Guid Подключения к БД
                            List<string> dbguids = getGuidsByDBName(aj.DBguid);
                            if (dbguids.Count == 0) break;
                            aj.DBguid = dbguids[0];
                        }
                    }
                    else break;
                    aj.Guid = System.Guid.NewGuid().ToString();
                    _archiveJobs.Add(aj);                  
                    break;
            }
        }
        SaveDataSources();
        SaveArchiveJobs();
    }

    private static int getAJTypeInt(string s)
    {
        switch (s)
        {
            case "Единожды": return 1;
            case "Ежедневно": return 2;
            case "Еженедельно": return 3;
            case "Ежемесячно": return 4;
            default: return 0;
        }
    }
}


