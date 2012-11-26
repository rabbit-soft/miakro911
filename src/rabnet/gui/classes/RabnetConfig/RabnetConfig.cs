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
using System.IO;

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
        public RegistryKey _regKey = Registry.CurrentUser;
        public const string STARTUP = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        public const string REGISTRY_PATH = @"SOFTWARE\9-Bits\Miakro911";
        public const string DATASOURCES_PATH = REGISTRY_PATH + "\\datasources";
        private const string ALL_DB = "[ВСЕ]";

        private List<DataSource> _dataSources = new List<DataSource>();        

        public List<DataSource> DataSources
        {
            get { return _dataSources; }
        }
        
        public string GetOption(OptionType optionType)
        {
            RegistryKey k = _regKey.CreateSubKey(REGISTRY_PATH);
            switch (optionType)
            {
                case OptionType.MysqlPath:
                    string val = (string)k.GetValue("mysql");
                    if (String.IsNullOrEmpty(val))
                        val= tryToDetectMysqlPath();
                    return val ;
                case OptionType.zip7path: return (string)k.GetValue("z7");
                case OptionType.rabdump_startupPath:
                    k = _regKey.CreateSubKey(STARTUP);
                    return (string)k.GetValue("rabdump", "");
                case OptionType.serverUrl:
                    return (string)k.GetValue("sUrl", "");
            }
            return "";
        }

        private string tryToDetectMysqlPath()
        {
            const string MYSQL_SRV_PATH = @"MySQL AB\MySQL Server 5.1";
            const string NODE64 = "Wow6432Node";
            const string LOC = "Location";

            RegistryKey mKey = Registry.LocalMachine.OpenSubKey("SOFTWARE");
            RegistryKey tryKey = mKey.OpenSubKey(MYSQL_SRV_PATH);
            if(tryKey==null)
                tryKey = mKey.OpenSubKey(Path.Combine(NODE64,MYSQL_SRV_PATH));
            if (tryKey == null) return "";

            return tryKey.GetValue(LOC).ToString();
        }

        public void SaveOption(OptionType optionType, string val)
        {
            if (val == null) val = "";
            RegistryKey k = _regKey.CreateSubKey(REGISTRY_PATH);
            switch (optionType)
            {
                case OptionType.MysqlPath: k.SetValue("mysql", val); break;
                case OptionType.zip7path: k.SetValue("z7", val); break;
                case OptionType.rabdump_startupPath:
                    k = _regKey.CreateSubKey(STARTUP);
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
            _dataSources.Clear();
            RegistryKey rKey = _regKey.CreateSubKey(DATASOURCES_PATH);
            foreach (string guid in rKey.GetSubKeyNames())
            {
                RegistryKey k = _regKey.CreateSubKey(DATASOURCES_PATH + "\\" + guid);
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
            RegistryKey rKey = _regKey.CreateSubKey(DATASOURCES_PATH);
            List<string> noDeleted = new List<string>();
            foreach (DataSource ds in _dataSources)
            {
                if (ds.Guid == "")
                    ds.Guid = System.Guid.NewGuid().ToString();
                RegistryKey k = _regKey.CreateSubKey(DATASOURCES_PATH + "\\" + ds.Guid);
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
            
    }

}