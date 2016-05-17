using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace rabnet.RNC
{
    public partial class RabnetConfig
    {
        public const string ARCHIVEJOBS_PATH = REGISTRY_PATH + "\\archivejobs";

        private List<ArchiveJob> _archiveJobs = new List<ArchiveJob>();
        public List<ArchiveJob> ArchiveJobs
        {
            get { return _archiveJobs; }
        }

        public void LoadArchiveJobs()
        {
            //_logger.Info("loading archiveJobs");
            _archiveJobs.Clear();
            RegistryKey rKey = _regKey.CreateSubKey(ARCHIVEJOBS_PATH);
            DataSource ds;
            foreach (string s in rKey.GetSubKeyNames()) {
                RegistryKey r = _regKey.CreateSubKey(RabnetConfig.ARCHIVEJOBS_PATH + "\\" + s);
                ds = getDBbyGUID((string)r.GetValue("db"));
                if (ds == null) continue; //todo удалить AJ
                _archiveJobs.Add(new ArchiveJob(s,
                        (string)r.GetValue("name"),
                        ds,
                        (string)r.GetValue("path"),
                        (string)r.GetValue("start"),
                        (int)r.GetValue("type"),
                        (int)r.GetValue("cntlimit"),
                        (int)r.GetValue("szlimit"),
                        (int)r.GetValue("srv_send", 0) == 1,
                        (int)r.GetValue("srv_day_delay", 1)));
            }
            //_logger.Info("loading archiveJobs finish");
        }

        /// <summary>
        /// Сохраняет в реестре все расписания
        /// </summary>
        public void SaveArchiveJobs()
        {
            //_logger.Info("saving archiveJobs");
            RegistryKey rKey = _regKey.CreateSubKey(RabnetConfig.ARCHIVEJOBS_PATH);
            foreach (ArchiveJob raj in _archiveJobs) {
                //if (raj.Guid == "" || raj.Guid == null)
                //raj.Guid = System.Guid.NewGuid().ToString();
                RegistryKey r = _regKey.CreateSubKey(RabnetConfig.ARCHIVEJOBS_PATH + "\\" + raj.Guid);
                r.SetValue("name", raj.Name, RegistryValueKind.String);
                r.SetValue("db", raj.DataSrc.Guid, RegistryValueKind.String);
                r.SetValue("path", raj.DumpPath, RegistryValueKind.String);
                r.SetValue("start", raj.StartTime.ToString("yyyy-MM-dd HH:mm"), RegistryValueKind.String);
                r.SetValue("type", raj.ArcType, RegistryValueKind.DWord);
                r.SetValue("cntlimit", raj.CountLimit, RegistryValueKind.DWord);
                r.SetValue("szlimit", raj.SizeLimit, RegistryValueKind.DWord);
                r.SetValue("srv_send", raj.SendToServ ? 1 : 0, RegistryValueKind.DWord);
                r.SetValue("srv_day_delay", raj.SendDayDelay, RegistryValueKind.DWord);
            }
            ///Удаляем удаленные Расписания
            foreach (string s in rKey.GetSubKeyNames()) {
                RegistryKey r = _regKey.CreateSubKey(RabnetConfig.ARCHIVEJOBS_PATH + "\\" + s);
                string dbName = (string)r.GetValue("db");
                bool contains = false;
                foreach (ArchiveJob raj in _archiveJobs) {
                    if (raj.Guid == s) {
                        contains = true;
                        break;
                    }
                }
                if (!contains) {
                    rKey.DeleteSubKey(s);
                }
            }
            //_logger.Info("saving archiveJobs finish");
        }

        public void DeleteArchiveJob(string guid)
        {
            ArchiveJob remove = null;//удаять из коллекции во время совершения цикла нельзя
            foreach (ArchiveJob aj in _archiveJobs) {
                if (aj.Guid == guid) {
                    remove = aj;
                    break;
                }
            }
            if (remove != null) {
                _archiveJobs.Remove(remove);
            }
        }

        private void changeArchiveJob(ArchiveJob newAJ)
        {
            foreach (ArchiveJob raj in _archiveJobs) {
                if (raj.Guid == newAJ.Guid) {
                    raj.Name = newAJ.Name;
                    raj.DataSrc.Guid = newAJ.DataSrc.Guid;
                    raj.DumpPath = newAJ.DumpPath;
                    raj.StartTime = newAJ.StartTime;
                    raj.ArcType = newAJ.ArcType;
                    raj.CountLimit = newAJ.CountLimit;
                    raj.SizeLimit = newAJ.SizeLimit;
                    //raj.Repeat = newAJ.Repeat;
                    //raj.ServTime = newAJ.ServTime;
                    //raj.ServType = newAJ.ServType;
                }
            }
        }

        private bool containsArchiveJob(string guid)
        {
            foreach (ArchiveJob raj in _archiveJobs) {
                if (raj.Guid == guid) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Если в реестре имеется похожее расписание, возвращает GUID.
        /// Возвращает пустую строку если нет похожих.
        /// </summary>
        /// <param name="aj">Расписание с которым сравнить</param>
        /// <returns>GUID похожего расписания</returns>
        private string compareArchivejobs(ArchiveJob aj)
        {
            List<string> dbguids = getGuidsByDBName(aj.DataSrc.Guid);//вместо DBguid передается название БД
            if (aj.DataSrc.Guid != ALL_DB && dbguids.Count == 0) {
                return "";
            }

            foreach (ArchiveJob raj in _archiveJobs) {
                if (aj.DataSrc.Guid != ALL_DB) {
                    bool sameDBguids = false;
                    foreach (string g in dbguids) {
                        if (raj.DataSrc.Guid == g) {
                            sameDBguids = true;
                            break;
                        }
                    }
                    if (!sameDBguids) {
                        continue;
                    }
                }
                if (raj.Name == aj.Name &&
                    raj.DumpPath == aj.DumpPath &&
                    raj.CountLimit == aj.CountLimit &&
                    raj.SizeLimit == aj.SizeLimit &&
                    raj.ArcType == aj.ArcType &&
                    raj.StartTime == aj.StartTime){
                    return raj.Guid;
                }
            }
            return "";
        }
    }
}
