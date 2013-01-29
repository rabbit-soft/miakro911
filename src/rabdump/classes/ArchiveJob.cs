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
        public int SendEachDays = 3;

        public ArchiveType ArcAType
        {
            get { return (ArchiveType)ArcType; }
            set { ArcType = (int)value; }
        }        

        public ArchiveJob() {}
        public ArchiveJob(string guid, string name, DataSource db, string path, string startAj, int type, int countlimit, int sizelimit, bool sendToserv,int sendEachDays)
            : this()
        {
            this.Guid = guid;
            JobName = name;
            DataSrc = db;
            DumpPath = path;
            DateTime.TryParse(startAj,out StartTime);
            ArcType = type;
            CountLimit = countlimit;
            SizeLimit = sizelimit;
            SendToServ = sendToserv;
            this.SendEachDays = sendEachDays;
        }     
    }

}
