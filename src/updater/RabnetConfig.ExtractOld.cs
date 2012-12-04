using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Win32;

namespace rabnet.RNC
{
    public partial class RabnetConfig
    {
        /// <summary>
        /// Выдирает из app.config настройки Подключения к БД и Расписания резервирования.
        /// Сохраняет полученные настройки в реестр.
        /// </summary>
        public void ExtractConfig(string filePath)
        {
            if (!System.IO.File.Exists(filePath)) return;
            //_logger.Info("extracting configs from app.configs");
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
                        SaveOption(RNCOption.MysqlPath, nd.InnerText.Replace(@"\bin\mysql.exe", ""));
                        break;
                    case "z7":
                        SaveOption(RNCOption.zip7path, nd.InnerText);
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
                }
            }
            SaveDataSources();
        }

        internal void RelocateRegOptions()
        {
            RegistryKey sourceKey = Registry.LocalMachine.OpenSubKey(REGISTRY_PATH);
            if (sourceKey == null) return;
            RegistryKey destinationKey = _regKey.CreateSubKey(REGISTRY_PATH);           
            recurseCopyKey(sourceKey, destinationKey);
            Registry.LocalMachine.DeleteSubKeyTree(REGISTRY_PATH);
        }

        private void recurseCopyKey(RegistryKey sourceKey, RegistryKey destinationKey)
        {
            foreach (string valueName in sourceKey.GetValueNames())
            {
                object objValue = sourceKey.GetValue(valueName);
                RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
                destinationKey.SetValue(valueName, objValue, valKind);
            }
 
            foreach (string sourceSubKeyName in sourceKey.GetSubKeyNames())
            {
                RegistryKey sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName);
                RegistryKey destSubKey = destinationKey.CreateSubKey(sourceSubKeyName);
                recurseCopyKey(sourceSubKey, destSubKey);
            }
        }
    }
}
