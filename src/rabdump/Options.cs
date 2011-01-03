using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;
using Microsoft.Win32;

namespace rabdump
{
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    class DataBase : Object
    {
        const string DB = "База Данных";
        const string Obj = " Объект";
        public DataBase() { }
        public DataBase(String name):this() {_nm=name;}
        private string _db,_host,_user,_pswd,_nm="BadDatabase";
        [Category(Obj),DisplayName("Название"),Description("")]
        public String Name { get { return _nm; } set { if (value!="") _nm = value; } }
        [Category(DB), DisplayName("Хост"), Description("")]
        public String Host{get{return _host;} set{_host=value;}}
        [Category(DB), DisplayName("Имя БД"), Description("")]
        public String DBName{get{return _db;} set{_db=value;}}
        [Category(DB), DisplayName("Пользователь"), Description("")]
        public String User{get{return _user;} set{_user=value;}}
        [Category(DB), DisplayName("Пароль"), Description("")]
        public String Password{get{return _pswd;} set{_pswd=value;}}
        public override string ToString()
        {
            return _nm;
        }
        public static DataBase AllDataBases = new DataBase("[все]");
        public void Save(XmlNode nd,XmlDocument doc)
        {
            XmlElement db=doc.CreateElement("db");
            db.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(Name));
            db.AppendChild(doc.CreateElement("host")).AppendChild(doc.CreateTextNode(Host));
            db.AppendChild(doc.CreateElement("db")).AppendChild(doc.CreateTextNode(DBName));
            db.AppendChild(doc.CreateElement("user")).AppendChild(doc.CreateTextNode(User));
            db.AppendChild(doc.CreateElement("password")).AppendChild(doc.CreateTextNode(Password));
            nd.AppendChild(db);
        }
        public void Load(XmlNode nd)
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
        public static DataBase Load(XmlNode nd,int hz)
        {
            DataBase db = new DataBase();
            db.Load(nd);
            return db;
        }
    }

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    class ArchiveJob : Object
    {
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public enum ArcType {При_Запуске,Единожды,Ежедневно,Еженедельно,Ежемесячно};
        const string Obj = " Объект";
        const string Data = "Данные";
        const string Time = "Расписание";
        private string _nm="BadJob",_bp="C:\\";
        private int _sl, _cl, _rp;
        private DataBase _db=DataBase.AllDataBases;
        private DateTime _st;
        private ArcType _tp;
        public bool Busy = false;
        public DateTime LastWork = DateTime.MinValue;
        [Category(Obj), DisplayName("Название"), Description("")]
        public String Name { get { return _nm; } set { if (value!="") _nm = value; } }
        [Category(Data), DisplayName("База Данных"), Description(""),
         Editor(typeof(DataBaseEditor), typeof(UITypeEditor))]
        public DataBase DB { get { return _db; } set { _db = value; } }
        [Category(Data), DisplayName("Лимит по размеру (МБ)"), Description("")]
        public int SizeLimit { get { return _sl; } set { _sl = value; } }
        [Category(Data), DisplayName("Лимит по количеству копий"), Description("")]
        public int CountLimit { get { return _cl; } set { _cl = value; } }
        [Category(Data), DisplayName("Путь к резервным копиям"),Description(""),
        Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public String BackupPath { get { return _bp; } set { if (Directory.Exists(value)) _bp = value; } }
        [Category(Time), DisplayName("Начало Резервирования"),Description("")]
        public DateTime StartTime { get { return _st; } set { _st = value; } }
        [Category(Time), DisplayName("Резервировать"), Description("")]
        public ArcType Type { get { return _tp; } set { _tp = value; } }
        [Category(Time), DisplayName("Повторять каждые (часов)"), Description("")]
        public int Repeat { get { return _rp; } set { _rp = value; } }
        public override string ToString()
        {
            return _nm;
        }
        public void Save(XmlNode nd,XmlDocument doc)
        {
            try
            {
                XmlElement j = doc.CreateElement("job");
                j.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(Name));
                j.AppendChild(doc.CreateElement("db")).AppendChild(doc.CreateTextNode(DB.Name));
                j.AppendChild(doc.CreateElement("path")).AppendChild(doc.CreateTextNode(BackupPath));
                j.AppendChild(doc.CreateElement("sizelim")).AppendChild(doc.CreateTextNode(SizeLimit.ToString()));
                j.AppendChild(doc.CreateElement("countlim")).AppendChild(doc.CreateTextNode(CountLimit.ToString()));
                j.AppendChild(doc.CreateElement("start")).AppendChild(doc.CreateTextNode(_st.ToString("dd.MM.yyyy HH:mm")));
                j.AppendChild(doc.CreateElement("type")).AppendChild(doc.CreateTextNode(Type.ToString()));
                j.AppendChild(doc.CreateElement("repeat")).AppendChild(doc.CreateTextNode(Repeat.ToString()));
                nd.AppendChild(j);
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Не все поля были заполненны верно");
            }
        }
        public void Load(XmlNode nd)
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
                        foreach (DataBase db in Options.Get().Databases)
                            if (db.Name == val) 
                                DB = db;
                        break;
                    case "path": BackupPath = val; break;
                    case "sizelim": SizeLimit = int.Parse(val); break;
                    case "countlim": CountLimit= int.Parse(val); break;
                    case "start": _st = DateTime.Parse(val); break;
                    case "type": Type = (ArcType)Enum.Parse(typeof(ArcType),val); break;
                    case "repeat": Repeat = int.Parse(val); break;
                }
            }
        }
        public static ArchiveJob Load(XmlNode nd, int hz)
        {
            ArchiveJob jb = new ArchiveJob();
            jb.Load(nd);
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
        public bool NeedDump(bool start)
        {
            if (Busy) return false;
            if (start && Type == ArcType.При_Запуске)
                return true;
            if (Repeat>0 && DateCmpNoSec(LastWork.AddHours(Repeat)))
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
        public enum Rubool { Да, Нет };
        private static Options _oops=null;
        public static Options Get()
        {
            if (_oops == null)
                _oops = new Options();
            return _oops;
        }
        const String Opt="Настройки";
        const String ArKey=@"Software\Microsoft\Windows\CurrentVersion\Run";
        const String ArValue = "rabdump";
        const String Rdo = "rabdumpOptions";
        private String _mp="", _p7="",_mdp="";
        private Rubool _sas=Rubool.Нет;
        private readonly DataBaseCollection _bds=new DataBaseCollection();
        private readonly ArchiveJobCollection _jobs = new ArchiveJobCollection();
        [Category(Opt), DisplayName("mysql"), Description("Путь к исполняемому файлу mysql.exe\n\rНужен для работы с Базой Данных\n\rПуть по умолчанию: \"C:\\Program Files\\MySQL\\MySQL Server <№ версии>\\bin\\mysql.exe\""),
        Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public String MySqlPath { get { return _mp; } set { _mp = value; } }
        [Category(Opt), DisplayName("mysqldump"), Description("Путь к исполняемому файлу mysqldump.exe\n\rНужен для резервного копирования рабочей Базы Данных\n\rПуть по умолчанию: \"C:\\Program Files\\MySQL\\MySQL Server <№ версии>\\bin\\mysqldump.exe\""),
        Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public String MySqlDumpPath { get { return _mdp; } set { _mdp = value; } }
        [Category(Opt), DisplayName("7z"), Description("Путь к арфиватору 7-zip"),
        Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public String Path7Z { get { return _p7; } set { _p7 = value; } }
        [Category(Opt), DisplayName("Базы данных"), Description("Коллекция Настроек подключения к Базам Данных")]
        public DataBaseCollection Databases { get { return _bds; }}
        [Category(Opt), DisplayName("Расписание"),Description("Коллекция расписаний резервирования Баз Данных")]
        public ArchiveJobCollection Jobs { get { return _jobs; } }
        [Category(Opt), DisplayName("Запускать при старте системы"),Description("Запускать программу вместе с Windows")]
        public Rubool StartAtStart { get { return _sas; } set { _sas = value; } }

        public void Save()
        {
            MainForm.log().Debug("saving options");
            XmlDocument doc = new XmlDocument();
            XmlElement rn = doc.CreateElement(Rdo);
            doc.AppendChild(rn);
            rn.AppendChild(doc.CreateElement("mysql")).AppendChild(doc.CreateTextNode(MySqlPath));
            rn.AppendChild(doc.CreateElement("mysqldump")).AppendChild(doc.CreateTextNode(MySqlDumpPath));
            rn.AppendChild(doc.CreateElement("z7")).AppendChild(doc.CreateTextNode(Path7Z));
            foreach (DataBase db in Databases)
                db.Save(rn, doc);
            foreach (ArchiveJob jb in Jobs)
                jb.Save(rn, doc);
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            conf.GetSection(Rdo).SectionInformation.SetRawXml(doc.OuterXml);
            conf.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(Rdo);
            RegistryKey rk=Registry.CurrentUser.CreateSubKey(ArKey);
            rk.DeleteValue(ArValue,false);
            if (StartAtStart==Rubool.Да)
                rk.SetValue(ArValue,Application.ExecutablePath);
        }
        public void DBFromRabNet()
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
        public void Load(XmlNode nd)
        {
            MainForm.log().Debug("loading options from "+nd.Name);
            if (nd.Name != Rdo) return;
            Databases.Clear();
            Jobs.Clear();
            foreach (XmlNode n in nd.ChildNodes)
            {
                switch (n.Name)
                {
                    case "db": _oops.Databases.Add(DataBase.Load(n, 0)); break;
                    case "job": _oops.Jobs.Add(ArchiveJob.Load(n, 0)); break;
                    case "z7": 
                    case "mysql": 
                    case "mysqldump": 
                        string xVal;
                        try
                        {
                            xVal = n.ChildNodes[0].Value;
                        }
                        catch
                        {
                            xVal = "";
                        }

                        switch (n.Name)
                        {
                            case "z7": Path7Z = xVal; break;
                            case "mysql": MySqlPath = xVal; break;
                            case "mysqldump": MySqlDumpPath = xVal; break;
                        }
                        break;

                }
            }
            DBFromRabNet();
            StartAtStart = Rubool.Нет;
            RegistryKey rk=Registry.CurrentUser.CreateSubKey(ArKey);
            string val = (string)rk.GetValue(ArValue);
            if (val != null)
                if (val == Application.ExecutablePath)
                    StartAtStart = Rubool.Да;
        }
        public void Load()
        {
            Load(ConfigurationManager.GetSection(Rdo) as XmlNode);
        }

    }
    

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class RabdumpConfigHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            Options.Get().Load(section);
            return section;
        }
    }
}
