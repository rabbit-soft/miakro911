using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using rabnet.RNC;

namespace rabnet.RNC
{
    public enum ArchiveType { При_Запуске, Единожды, Ежедневно, Еженедельно, Ежемесячно, Никогда };

    public class ArchiveJob
    {
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]

        public readonly string Guid = "";
        public bool Busy = false;       
        public DateTime LastWork = DateTime.MinValue;        
        public String JobName;
        public DataSource DataSrc;
        public int SizeLimit;
        public int CountLimit;
        public String DumpPath;
        public DateTime StartTime;
        public int ArcType;
        public bool SendToServ=false;

        public ArchiveType ArcAType
        {
            get { return (ArchiveType)ArcType; }
            set { ArcType = (int)value; }
        }        

        public ArchiveJob() {}
        public ArchiveJob(string guid, string name, DataSource db, string path, string startAj, int type, int countlimit, int sizelimit, bool sendToserv)
            : this()
        {
            this.Guid = guid;
            JobName = name;
            DataSrc = db;
            DumpPath = path;
            DateTime.TryParse(startAj,out StartTime);
            ArcType = type;
            //ArchiveType = getAJtype(type);
            CountLimit = countlimit;
            SizeLimit = sizelimit;
            SendToServ = sendToserv;
            //ServType = getAJtype(servType);
        }     

        #region delete

        //private bool checkTime(string val)
        //{
        //    const int maxHour = 23;
        //    const int maxMint = 59;
        //    if (val == "") return false;
        //    if (!val.Contains(":")) return false;
        //    string[] ham = val.Split(':');
        //    try
        //    {
        //        if (int.Parse(ham[0]) > maxHour) return false;
        //        if (int.Parse(ham[1]) > maxMint) return false;
        //    }
        //    catch (FormatException) { return false; }
        //    return true;
        //}

        //const string Obj = " Резервирование";
        //const string Data = "Данные";
        //const string Time = "Расписание";
        //const string Serv = "Удаленное резервирование";

        //private string _nm = "Расписание" _bp = "C:\\";
        //private DateTime _servTime = DateTime.Parse("18:00");
        //private ArcType _servType = ArcType.Никогда;
        //private DataBase _db = DataBase.AllDataBases;
        //private DateTime _st;
        //private ArcType _tp;

        //[Category(Serv), DisplayName("Время"), Description("Врямя отправки резервных копий баз данных на сервер.")]
        //public string ServTime
        //{
        //    get { return _servTime.ToString("HH:mm"); }
        //    set
        //    {
        //        try
        //        {
        //            _servTime = DateTime.Parse(value);
        //        }
        //        catch { }
        //    }
        //}

        //public override string ToString()
        //{
        //    return _nm;
        //}

        ///// <summary>
        ///// Залить ли на сервер РКБД
        ///// </summary>
        ///// <param name="start"></param>
        ///// <returns></returns>
        //public bool NeedServDump(bool start)
        //{
        //    if (ServAType == ArchiveType.Никогда)
        //        return false;
        //    if (start && ServAType == ArchiveType.При_Запуске)
        //        return true;
        //    if (ServAType == ArchiveType.Единожды && DateCmpNoSec(ServTime))
        //        return true;
        //    if (ServAType == ArchiveType.Ежедневно && DateCmpTime(ServTime))
        //        return true;
        //    if (ServAType == ArchiveType.Еженедельно && ((DateTime.Now - ServTime).Days % 7) == 0 && DateCmpTime(ServTime))
        //        return true;
        //    if (ServAType == ArchiveType.Ежемесячно && ServTime.Day == DateTime.Now.Day && DateCmpTime(ServTime))
        //        return true;
        //    return false;
        //}

        //private ArchiveType getAJtype(int i)
        //{
        //    switch (i)
        //    {
        //        case 0: return ArchiveType.При_Запуске;
        //        case 1: return ArchiveType.Единожды;
        //        case 2: return ArchiveType.Ежедневно;
        //        case 3: return ArchiveType.Еженедельно;
        //        case 4: return ArchiveType.Ежемесячно;
        //        default: return ArchiveType.Никогда;
        //    }
        //}

        //public int IntType()
        //{
        //    switch (this.ArchiveType)
        //    {
        //        case ArchiveType.При_Запуске: return 0;
        //        case ArchiveType.Единожды: return 1;
        //        case ArchiveType.Ежедневно: return 2;
        //        case ArchiveType.Еженедельно: return 3;
        //        case ArchiveType.Ежемесячно: return 4;
        //        default: return 5;
        //    }
        //}

        //public int IntServType()
        //{
        //    switch (this.ServType)
        //    {
        //        case ArchiveType.При_Запуске: return 0;
        //        case ArchiveType.Единожды: return 1;
        //        case ArchiveType.Ежедневно: return 2;
        //        case ArchiveType.Еженедельно: return 3;
        //        case ArchiveType.Ежемесячно: return 4;
        //        default: return 5;
        //    }
        //}
        #endregion delete
    }

}
