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

namespace rabnet.RNC
{
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public partial class RabnetConfig
    {
        public enum OptionType
        {
            MysqlPath,
            zip7path,
            rabdump_startupPath,
            serverUrl,
            makeWebReport
        }

        private readonly ILog _logger = LogManager.GetLogger(typeof(RabnetConfig));

        public const string STARTUP = @"Software\Microsoft\Windows\CurrentVersion\Run";
        public const string REGISTRY_PATH = @"Software\9-Bits\Miakro911";
        public const string DATASOURCES_PATH = REGISTRY_PATH + "\\datasources";
        private const string ALL_DB = "[ВСЕ]";

        private List<DataSource> _dataSources = new List<DataSource>();        

        private bool _extracting = false;

        public List<DataSource> DataSources
        {
            get { return _dataSources; }
        }
        
        public string GetOption(OptionType optionType)
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
                    return (string)k.GetValue("sUrl", "");
            }
            return "";
        }

        public void SaveOption(OptionType optionType, string val)
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
                    else k.SetValue("rabdump", val);
                    break;
                case OptionType.serverUrl:
                    k.SetValue("sUrl", val); break;
            }
        }

        public void SetDefault(DataSource  def_ds, String uname, String pswd)
        {
            def_ds.Default = true;
            foreach (DataSource ds in _dataSources)
                if (ds != def_ds) ds.Default = false;
            def_ds.DefUser = uname;
            def_ds.DefPassword =  !def_ds.SavePassword ? "" : pswd;           
            SaveDataSources();
        }

        #region datasources

        /// <summary>
        /// Загружает настройки подключения к БД из реестра
        /// </summary>
        public void LoadDataSources()
        {
            //_logger.Info("loading DataSources");
            if (!_extracting)
                ExtractConfig(System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath);
            _dataSources.Clear();
            RegistryKey rKey = Registry.LocalMachine.CreateSubKey(DATASOURCES_PATH);
            foreach (string guid in rKey.GetSubKeyNames())
            {
                RegistryKey k = Registry.LocalMachine.CreateSubKey(DATASOURCES_PATH + "\\" + guid);
                DataSource ds = new DataSource(guid, (string)k.GetValue("name"), (string)k.GetValue("type"), (string)k.GetValue("params"));
                ds.Default = (string)k.GetValue("def", false) == true.ToString();
                ds.Hidden = (string)k.GetValue("hidden", false) == true.ToString();
                ds.SavePassword = (string)k.GetValue("savepass", false) == true.ToString();
                ds.DefUser = (string)k.GetValue("defuser");
                ds.DefPassword = (string)k.GetValue("defpass");
                ds.WebReport = (string)k.GetValue("webrep", false.ToString()) == true.ToString();
                _dataSources.Add(ds);
            }
            //_logger.Info("loading DataSources finish");
        }

        /// <summary>
        /// Сохраняет настройки подключения к БД
        /// </summary>
        public void SaveDataSources()
        {
            //_logger.Info("saving DataSources");
            RegistryKey rKey = Registry.LocalMachine.CreateSubKey(DATASOURCES_PATH);
            List<string> noDeleted = new List<string>();
            foreach (DataSource ds in _dataSources)
            {
                if (ds.Guid == "")
                    ds.Guid = System.Guid.NewGuid().ToString();
                RegistryKey k = Registry.LocalMachine.CreateSubKey(DATASOURCES_PATH + "\\" + ds.Guid);
                k.SetValue("name", ds.Name, RegistryValueKind.String);
                k.SetValue("type", ds.Type, RegistryValueKind.String);
                k.SetValue("params", ds.Params.ToString(), RegistryValueKind.String);
                k.SetValue("def", ds.Default, RegistryValueKind.Unknown);
                k.SetValue("hidden", ds.Hidden, RegistryValueKind.Unknown);
                k.SetValue("savepass", ds.SavePassword, RegistryValueKind.Unknown);
                k.SetValue("defuser", ds.DefUser, RegistryValueKind.String);
                k.SetValue("defpass", ds.DefPassword, RegistryValueKind.String);
                k.SetValue("webrep", ds.WebReport, RegistryValueKind.String);
                noDeleted.Add(ds.Guid);
            }
            //Удаляем из реестра
            foreach (string guid in rKey.GetSubKeyNames())
                if (!noDeleted.Contains(guid))
                    rKey.DeleteSubKey(guid);
            //_logger.Debug("saving DataSources finish");
        }

        /// <summary>
        /// Удаляет DataSource по Guid
        /// </summary>
        /// <param name="p">Guid - уникальный номер</param>
        public void DeleteDataSource(string guid)
        {
            DataSource remove = null;//удаять из коллекции во время совершения цикла нельзя
            foreach (DataSource ds in _dataSources)
            {
                if (ds.Guid == guid)
                {
                    remove = ds;
                    break;
                }
            }
            if (remove != null)
            {
                //_logger.Debug("delete datasource "+remove.Name);
                _dataSources.Remove(remove);
            }
        }

        /// <summary>
        /// Сохраняет данные подключения к бд.
        /// Если такая запись есть, то изменяет.
        /// Если нет, то добавляет
        /// </summary>
        public void SaveDataSource(string guid, string name, string host, string db, string user, string password)
        {
            DataSource newDS = new DataSource(guid, name, host, db, user, password);
            SaveDataSource(newDS);
        }
        public void SaveDataSource(DataSource newDS)
        {
            if (!containsDataSource(newDS.Guid))//если не содержит такого ДС, то добавляем
                _dataSources.Add(newDS);
            else
                changeDataSource(newDS);
        }

        /// <summary>
        /// Имеются ли Настройки подключения к БД
        /// </summary>
        public bool HaveDataSources()
        {
            LoadDataSources();
            if (_dataSources.Count > 0)
                return true;
            else return false;
        }

        private void changeDataSource(DataSource newDS)
        {
            foreach (DataSource rds in _dataSources)
            {
                if (rds.Guid == newDS.Guid)
                {
                    rds.Name = newDS.Name;
                    rds.Params = newDS.Params;
                    rds.WebReport = newDS.WebReport;
                }
            }
        }

        private bool containsDataSource(string guid)
        {
            foreach (DataSource ds in _dataSources)
            {
                if (ds.Guid == guid)
                    return true;
            }
            return false;
        }

        private string compareDataSource(DataSource ds)
        {
            foreach (DataSource rds in _dataSources)
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
        private List<string> getGuidsByDBName(string name)
        {
            List<string> result = new List<string>();
            foreach (DataSource rda in _dataSources)
            {
                if (rda.Name == name)
                    result.Add(rda.Guid);
            }
            return result;
        }

        private DataSource getDBbyGUID(string guid)
        {
            foreach (DataSource ds in _dataSources)
                if (ds.Guid == guid)
                    return ds;
            return null;
        }

        #endregion datasources

        #region convert_old
        /// <summary>
        /// Выдирает из app.config настройки Подключения к БД и Расписания резервирования.
        /// Сохраняет полученные настройки в реестр.
        /// </summary>
        public void ExtractConfig(string filePath)
        {
            if (!System.IO.File.Exists(filePath)) return;
            //_logger.Info("extracting configs from app.configs");
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
            //_logger.Info("finish extracting");
            //_extracting = false; //один раз за сессию и хватит
        }

        private void extractRabnetds(XmlNode node)
        {
            //_logger.Info("extracting config from RabNet");
            LoadDataSources();
            foreach (XmlNode nd in node.ChildNodes)
            {
                if (nd.Name == "dataSource")
                {
                    DataSource td = new DataSource(System.Guid.NewGuid().ToString(), nd.Attributes.GetNamedItem("name").Value,
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

        private void extractRabDump(XmlNode node)
        {
            //_logger.Info("extracting congig from RabDump");
            LoadDataSources();
            //LoadArchiveJobs();
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
                        DataSource db = new DataSource("",
                            nd.SelectSingleNode("name").InnerText,
                            nd.SelectSingleNode("host").InnerText,
                            nd.SelectSingleNode("db").InnerText,
                            nd.SelectSingleNode("user").InnerText,
                            nd.SelectSingleNode("password").InnerText);
                        if (compareDataSource(db) == "")
                            _dataSources.Add(db);

                        break;
                    //case "job":
                    //    ArchiveJob aj = new ArchiveJob("",
                    //        nd.SelectSingleNode("name").InnerText,
                    //        nd.SelectSingleNode("db").InnerText,
                    //        nd.SelectSingleNode("path").InnerText,
                    //        nd.SelectSingleNode("start").InnerText,
                    //        getAJTypeInt(nd.SelectSingleNode("type").InnerText),
                    //        int.Parse(nd.SelectSingleNode("countlim").InnerText),
                    //        int.Parse(nd.SelectSingleNode("sizelim").InnerText),
                    //        int.Parse(nd.SelectSingleNode("repeat").InnerText),
                    //        DateTime.Now.ToString("yyyy-MM-dd HH:mm"), 5);
                    //    if (aj.DBguid == "[все]")
                    //        aj.DBguid = ALL_DB;

                    //    if (compareArchivejobs(aj) == "")//если не имеется идентичных Расписаний
                    //    {
                    //        if (aj.DBguid != ALL_DB)
                    //        {   //назначаем расписанию Guid Подключения к БД
                    //            List<string> dbguids = getGuidsByDBName(aj.DBguid);
                    //            if (dbguids.Count == 0) break;
                    //            aj.DBguid = dbguids[0];
                    //        }
                    //    }
                    //    else break;
                    //    aj.Guid = System.Guid.NewGuid().ToString();
                    //    _archiveJobs.Add(aj);
                    //    break;
                }
            }
            SaveDataSources();
            //SaveArchiveJobs();
        }
        #endregion convert_old
    }

}