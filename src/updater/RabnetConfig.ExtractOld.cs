using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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
    }
}
