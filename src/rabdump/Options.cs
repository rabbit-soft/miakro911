using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Configuration;
using System.Xml;

namespace rabdump
{
    [Serializable]
    class DataBase:Object
    {
        const string DB = "База Данных";
        const string OBJ = "Объект";
        public DataBase() { }
        public DataBase(String name):this() {nm=name;}
        private string db,host,user,pswd,nm;
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
    }

    [Serializable]
    class ArchiveJob : Object
    {
        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
        public enum ArcType {Единожды,Ежедневно,Еженедельно,Ежемесячно};
        const string OBJ = "Объект";
        const string DATA = "Данные";
        const string TIME = "Расписание";
        private string nm,bp;
        private int sl, cl, rp;
        private DataBase db;
        private DateTime st;
        private ArcType tp;
        [Category(OBJ), DisplayName("Название"), Description("")]
        public String Name { get { return nm; } set { nm = value; } }
        [Category(DATA), DisplayName("База Данных"), Description(""),
         Editor(typeof(DataBaseEditor), typeof(UITypeEditor))]
        public DataBase DB { get { return db; } set { db = value; } }
        [Category(DATA), DisplayName("Лимит по размеру (МБ)"), Description("")]
        public int SizeLimit { get { return sl; } set { sl = value; } }
        [Category(DATA), DisplayName("Лимит по количеству копий"), Description("")]
        public int CountLimit { get { return cl; } set { cl = value; } }
        [Category(DATA), DisplayName("Путь к резервным копиям"),
        Description(""),
        Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public String BackupPath { get { return bp; } set { bp = value; } }
        [Category(TIME), DisplayName("Начало Резервирования"),
        Description(""),
        Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public DateTime StartTime { get { return st; } set { st = value; } }
        [Category(TIME), DisplayName("Резервировать"),Description("")]
        public ArcType Type { get { return tp; } set { tp = value; } }
        [Category(TIME), DisplayName("Повторять каждые (часов)"), Description("")]
        public int Repeat { get { return rp; } set { rp = value; } }
        public override string ToString()
        {
            return nm;
        }

    }

    [Serializable]
    class DataBaseCollection : Collection<DataBase> { }
    [Serializable]
    class ArchiveJobCollection : Collection<DataBase> { }

    [Serializable]
    class Options
    {
        private static Options oops=null;
        public static Options get()
        {
            if (oops == null)
                oops = new Options();
            return oops;
        }
        const String OPT="Настройки";
        private String mp, p7;
        private DataBaseCollection bds=new DataBaseCollection();
        private ArchiveJobCollection jobs = new ArchiveJobCollection();
        [Category(OPT), DisplayName("mysqldump"),
        Description(""),
        Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public String MySqlPath { get { return mp; } set { mp = value; } }
        [Category(OPT), DisplayName("7z"),
        Description(""),
        Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
        public String Path7Z { get { return p7; } set { p7 = value; } }
        [Category(OPT), DisplayName("Базы данных"),
        Description("")]
        public DataBaseCollection Databases { get { return bds; }}
        [Category(OPT), DisplayName("Расписание"),
        Description("")]
        public ArchiveJobCollection Jobs { get { return jobs; } }

        public void save()
        {
            ArrayList a = Properties.Settings.Default.options;
            if (a==null)
                a=new ArrayList();
            a.Clear();
            a.Add(this);
            Properties.Settings.Default.options = a;
            Properties.Settings.Default.Save();
        }
        public void load(XmlNode nd)
        {
            XmlReader rd=new XmlTextReader(
        }
    }
    
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class RabdumpConfigHandler : IConfigurationSectionHandler
    {

        public object Create(object parent, object configContext, XmlNode section)
        {
            Options.get().load(section);
        }
    }
}
