using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Configuration;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Win32;
using System.Windows.Forms;

namespace rabdump
{
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    class DataBase : Object
    {
        const string DB = "База Данных";
        const string OBJ = " Объект";
        public DataBase() { }
        public DataBase(String name):this() {nm=name;}
        private string db,host,user,pswd,nm="BadDatabase";
        [Category(OBJ),DisplayName("Название"),Description("")]
        public String Name { get { return nm; } set { nm = value; } }
        [Category(DB), DisplayName("Хост"), Description("")]
        public String Host{get{return host;} set{host=value;}}
        [Category(DB), DisplayName("Имя БД"), Description("")]
        public String DBName{get{return db;} set{db=value;}}
        [Category(DB), DisplayName("Пользователь"), Description("")]
        public String User{get{return user;} set{user=value;}}
        [Category(DB), DisplayName("Пароль"), Description("")]
        public String Password{get{return pswd;} set{pswd=value;}}
        public override string ToString()
        {
            return nm;
        }
        public static DataBase AllDataBases = new DataBase("[все]");
        public void save(XmlNode nd,XmlDocument doc)
        {
            XmlElement db=doc.CreateElement("db");
            db.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(Name));
            db.AppendChild(doc.CreateElement("host")).AppendChild(doc.CreateTextNode(Host));
            db.AppendChild(doc.CreateElement("db")).AppendChild(doc.CreateTextNode(DBName));
            db.AppendChild(doc.CreateElement("user")).AppendChild(doc.CreateTextNode(User));
            db.AppendChild(doc.CreateElement("password")).AppendChild(doc.CreateTextNode(Password));
            nd.AppendChild(db);
        }
        public void load(XmlNode nd)
        {
            if (nd.Name != "db") return;
            foreach(XmlNode n in nd.ChildNodes)
            {

                String val = "";
                if (n.ChildNodes.Count>0)
                    val=n.ChildNodes[0].Value;
                switch (n.Name)
                {
                    case "name": Name = val; break;
                    case "host": Host = val; break;
                    case "db": DBName = val; break;
                    case "user": User = val; break;
                    case "password": Password = val; break;
                }
            }
        }
        public static DataBase load(XmlNode nd,int hz)
        {
            DataBase db = new DataBase();
            db.load(nd);
            return db;
        }
    }

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    class ArchiveJob : Object
    {
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public enum ArcType {При_Запуске,Единожды,Ежедневно,Еженедельно,Ежемесячно};
        const string OBJ = " Объект";
        const string DATA = "Данные";
        const string TIME = "Расписание";
        private string nm="BadJob",bp="C:\\";
        private int sl, cl, rp;
        private DataBase db=DataBase.AllDataBases;
        private DateTime st;
        private ArcType tp;
        public bool busy = false;
        public DateTime lastWork = DateTime.MinValue;
        [Category(OBJ), DisplayName("Название"), Description("")]
        public String Name { get { return nm; } set { nm = value; } }
        [Category(DATA), DisplayName("База Данных"), Description(""),
         Editor(typeof(DataBaseEditor), typeof(UITypeEditor))]
        public DataBase DB { get { return db; } set { db = value; } }
        [Category(DATA), DisplayName("Лимит по размеру (МБ)"), Description("")]
        public int SizeLimit { get { return sl; } set { sl = value; } }
        [Category(DATA), DisplayName("Лимит по количеству копий"), Description("")]
        public int CountLimit { get { return cl; } set { cl = value; } }
        [Category(DATA), DisplayName("Путь к резервным копиям"),Description(""),
        Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public String BackupPath { get { return bp; } set { bp = value; } }
        [Category(TIME), DisplayName("Начало Резервирования"),Description("")]
        public DateTime StartTime { get { return st; } set { st = value; } }
        [Category(TIME), DisplayName("Резервировать"), Description("")]
        public ArcType Type { get { return tp; } set { tp = value; } }
        [Category(TIME), DisplayName("Повторять каждые (часов)"), Description("")]
        public int Repeat { get { return rp; } set { rp = value; } }
        public override string ToString()
        {
            return nm;
        }
        public void save(XmlNode nd,XmlDocument doc)
        {
            try
            {
                XmlElement j = doc.CreateElement("job");
                j.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(Name));
                j.AppendChild(doc.CreateElement("db")).AppendChild(doc.CreateTextNode(DB.Name));
                j.AppendChild(doc.CreateElement("path")).AppendChild(doc.CreateTextNode(BackupPath));
                j.AppendChild(doc.CreateElement("sizelim")).AppendChild(doc.CreateTextNode(SizeLimit.ToString()));
                j.AppendChild(doc.CreateElement("countlim")).AppendChild(doc.CreateTextNode(CountLimit.ToString()));
                j.AppendChild(doc.CreateElement("start")).AppendChild(doc.CreateTextNode(st.ToString("dd.MM.yyyy HH:mm")));
                j.AppendChild(doc.CreateElement("type")).AppendChild(doc.CreateTextNode(Type.ToString()));
                j.AppendChild(doc.CreateElement("repeat")).AppendChild(doc.CreateTextNode(Repeat.ToString()));
                nd.AppendChild(j);
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Не все поля были заполненны верно");
            }
        }
        public void load(XmlNode nd)
        {
            if (nd.Name != "job") return;
            foreach (XmlNode n in nd.ChildNodes)
            {
                String val=n.ChildNodes[0].Value;
                switch(n.Name)
                {
                    case "name": Name = val; break;
                    case "db":
                        if (val == DataBase.AllDataBases.Name) DB = DataBase.AllDataBases;
                        foreach (DataBase db in Options.get().Databases)
                            if (db.Name == val) 
                                DB = db;
                        break;
                    case "path": BackupPath = val; break;
                    case "sizelim": SizeLimit = int.Parse(val); break;
                    case "countlim": CountLimit= int.Parse(val); break;
                    case "start": st = DateTime.Parse(val); break;
                    case "type": Type = (ArcType)Enum.Parse(typeof(ArcType),val); break;
                    case "repeat": Repeat = int.Parse(val); break;
                }
            }
        }
        public static ArchiveJob load(XmlNode nd, int hz)
        {
            ArchiveJob jb = new ArchiveJob();
            jb.load(nd);
            return jb;
        }

        public bool DateCmpNoSec(DateTime dt)
        {
            DateTime cmp1 = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
            DateTime cmp2 = DateTime.Now;
            cmp2 = new DateTime(cmp2.Year, cmp2.Month, cmp2.Day, cmp2.Hour, cmp2.Minute, 0);
            return cmp1 == cmp2;
        }
        public bool DateCmpTime(DateTime dt)
        {
            return (dt.Hour == DateTime.Now.Hour && dt.Minute == DateTime.Now.Minute);
        }
        public bool needDump(bool start)
        {
            if (busy) return false;
            if (start && Type == ArcType.При_Запуске)
                return true;
            if (Repeat>0 && DateCmpNoSec(lastWork.AddHours(Repeat)))
                return true;
            if (Type == ArcType.Единожды && DateCmpNoSec(StartTime))
                return true;
            if (Type == ArcType.Ежедневно && DateCmpTime(StartTime))
                return true;
            if (Type == ArcType.Еженедельно && ((DateTime.Now - StartTime).Days % 7) == 0 && DateCmpTime(StartTime))
                return true;
            if (Type == ArcType.Ежемесячно && StartTime.Day == DateTime.Now.Day && DateCmpTime(StartTime))
                return true;
            return false;
        }
    }

    class DataBaseCollection : List<DataBase> { }
    class ArchiveJobCollection : List<ArchiveJob> { }

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    class Options:Object
    {
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public enum RUBOOL { Да, Нет };
        private static Options oops=null;
        public static Options get()
        {
            if (oops == null)
                oops = new Options();
            return oops;
        }
        const String OPT="Настройки";
        const String ARKey=@"Software\Microsoft\Windows\CurrentVersion\Run";
        const String ARValue = "rabdump";
        const String RDO = "rabdumpOptions";
        private String mp="", p7="",mdp="";
        private RUBOOL sas=RUBOOL.Нет;
        private DataBaseCollection bds=new DataBaseCollection();
        private ArchiveJobCollection jobs = new ArchiveJobCollection();
        [Category(OPT), DisplayName("mysql"), Description(""),
        Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public String MySqlPath { get { return mp; } set { mp = value; } }
        [Category(OPT), DisplayName("mysqldump"), Description(""),
        Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public String MySqlDumpPath { get { return mdp; } set { mdp = value; } }
        [Category(OPT), DisplayName("7z"),Description(""),
        Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public String Path7Z { get { return p7; } set { p7 = value; } }
        [Category(OPT), DisplayName("Базы данных"),Description("")]
        public DataBaseCollection Databases { get { return bds; }}
        [Category(OPT), DisplayName("Расписание"),Description("")]
        public ArchiveJobCollection Jobs { get { return jobs; } }
        [Category(OPT), DisplayName("Запускать при старте системы"),Description("")]
        public RUBOOL StartAtStart { get { return sas; } set { sas = value; } }

        public void save()
        {
            MainForm.log().Debug("saving options");
            XmlDocument doc = new XmlDocument();
            XmlElement rn = doc.CreateElement(RDO);
            doc.AppendChild(rn);
            rn.AppendChild(doc.CreateElement("mysql")).AppendChild(doc.CreateTextNode(MySqlPath));
            rn.AppendChild(doc.CreateElement("mysqldump")).AppendChild(doc.CreateTextNode(MySqlDumpPath));
            rn.AppendChild(doc.CreateElement("z7")).AppendChild(doc.CreateTextNode(Path7Z));
            foreach (DataBase db in Databases)
                db.save(rn, doc);
            foreach (ArchiveJob jb in Jobs)
                jb.save(rn, doc);
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.GetSection(RDO).SectionInformation.SetRawXml(doc.OuterXml);
            conf.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(RDO);
            RegistryKey rk=Registry.CurrentUser.CreateSubKey(ARKey);
            rk.DeleteValue(ARValue,false);
            if (StartAtStart==RUBOOL.Да)
                rk.SetValue(ARValue,Application.ExecutablePath);
        }
        public void dbFromRabNet()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("rabnet.exe.config");
                foreach (XmlNode nd in doc.DocumentElement.ChildNodes)
                    if (nd.Name=="rabnetds")
                        foreach (XmlNode n in nd.ChildNodes)
                            if (n.Name=="dataSource")
                            {
                                String nm = n.Attributes["name"].Value;
                                String prm = n.Attributes["param"].Value;
                                String hst = "", db = "", usr = "", pwd = "";
                                foreach (String s in prm.Split(';'))
                                {
                                    String[] ss = s.Split('=');
                                    switch (ss[0])
                                    {
                                        case "host": hst = ss[1]; break;
                                        case "database": db = ss[1]; break;
                                        case "user":
                                        case "uid": usr = ss[1]; break;
                                        case "password":
                                        case "pwd": pwd = ss[1]; break;
                                    }
                                }
                                if (hst != "" && db != "")
                                {
                                    bool found=false;
                                    foreach (DataBase d in Databases)
                                        if (d.Host == hst && d.DBName == db) found = true;
                                    if (!found)
                                    {
                                        DataBase d = new DataBase(nm);
                                        d.DBName = db;
                                        d.Host = hst;
                                        d.User = usr;
                                        d.Password = pwd;
                                        Databases.Add(d);
                                    }
                                }
                            }
            }
            catch (Exception)
            {
            }
        }
        public void load(XmlNode nd)
        {
            MainForm.log().Debug("loading options from "+nd.Name);
            if (nd.Name != RDO) return;
            Databases.Clear();
            Jobs.Clear();
            foreach (XmlNode n in nd.ChildNodes)
            {
                switch (n.Name)
                {
                    case "db": oops.Databases.Add(DataBase.load(n, 0)); break;
                    case "job": oops.Jobs.Add(ArchiveJob.load(n, 0)); break;
                    case "z7": Path7Z = n.ChildNodes[0].Value; break;
                    case "mysql": MySqlPath = n.ChildNodes[0].Value; break;
                    case "mysqldump": MySqlDumpPath = n.ChildNodes[0].Value; break;
                }
            }
            dbFromRabNet();
            StartAtStart = RUBOOL.Нет;
            RegistryKey rk=Registry.CurrentUser.CreateSubKey(ARKey);
            string val = (string)rk.GetValue(ARValue);
            if (val != null)
                if (val == Application.ExecutablePath)
                    StartAtStart = RUBOOL.Да;
        }
        public void load()
        {
            load(ConfigurationManager.GetSection(RDO) as XmlNode);
        }

    }
    

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class RabdumpConfigHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            Options.get().load(section);
            return section;
        }
    }
}
