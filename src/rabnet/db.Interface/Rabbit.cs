using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class Rabbit
    {
        public enum SexType { VOID, MALE, FEMALE };
        public const string NULL_ADDRESS = "бомж";
        public const string NULL_BON = "00000";
        public const string NULL_FLAGS = "00000";
        protected int _id;
        protected String _nameFull;
        protected Rabbit.SexType _sex;
        protected DateTime _birthDay;
        protected String _breedName;
        protected string _notes;
        protected int _group;
        protected String _bon;
        protected String _rawAddress;

        public Rabbit() { }
        public Rabbit(int id, string rabname, string sex, DateTime born, string breedname, int group, String bon, string rawAddress, string notes)
        {
            _id = id;
            _nameFull = rabname;
            _sex = Rabbit.SexType.VOID;
            if (sex == "male") _sex = Rabbit.SexType.MALE;
            if (sex == "female") _sex = Rabbit.SexType.FEMALE;
            _birthDay = born;
            _breedName = breedname;
            _group = group;

            _bon = bon;
            _rawAddress = rawAddress;
            _notes = notes;
        }

        public virtual int ID { get { return _id; } set { _id = value; } }
        public virtual String NameFull { get { return _nameFull; } set { _nameFull = value; } }
        public virtual Rabbit.SexType Sex { get { return _sex; } set { _sex = value; } }
        public virtual DateTime BirthDay { get { return _birthDay; } set { _birthDay = value; } }
        public virtual String BreedName { get { return _breedName; } set { _breedName = value; } }
        public virtual string Notes { get { return _notes; } set { _notes = value; } }
        public virtual int Group { get { return _group; } set { _group = value; } }
        public virtual int Age { get { return DateTime.Now.Subtract(BirthDay).Days; } }
        public virtual String AddressFull { get { return _rawAddress == "" ? NULL_ADDRESS : Building.FullPlaceName(_rawAddress, false, true, true); } }
        public virtual String Address { get { return _rawAddress == "" ? NULL_ADDRESS : Building.FullPlaceName(_rawAddress, false, true, false); } set { } }
        public virtual String AddressSmall { get { return _rawAddress == "" ? NULL_ADDRESS : Building.FullPlaceName(_rawAddress, true, false, false); } }
        public virtual String FAddress(bool s_adrTier, bool s_adrDesc) { return Building.FullPlaceName(_rawAddress, false, s_adrTier, s_adrDesc); }
        public virtual String FAddress() { return FAddress(false, false); }
        public virtual string FBon(bool s_short) { return Rabbit.GetFBon(_bon, s_short); }
        public virtual String FSex() { return SexToRU(Sex); }

        #region static
        public static String SexToRU(Rabbit.SexType s)
        {
            if (s == Rabbit.SexType.FEMALE) return "ж";
            if (s == Rabbit.SexType.MALE) return "м";
            return "?";
        }

        public static String SexToString(Rabbit.SexType s)
        {
            String res = "void";
            if (s == Rabbit.SexType.FEMALE) res = "female";
            if (s == Rabbit.SexType.MALE) res = "male";
            return res;
        }

        public static String GetEventName(int evtype)
        {
            String ev = "none";
            if (evtype == 1) ev = "sluchka";
            if (evtype == 2) ev = "vyazka";
            if (evtype == 2) ev = "kuk";
            return ev;
        }
        public static int GetEventType(String eventType)
        {
            int evtype = 0;
            if (eventType == "sluchka") evtype = 1;
            if (eventType == "vyazka") evtype = 2;
            if (eventType == "kuk") evtype = 3;
            return evtype;
        }
        public static Rabbit.SexType GetSexType(string sex)
        {
            SexType res = Rabbit.SexType.VOID;
            if (sex == "male") res = Rabbit.SexType.MALE;
            if (sex == "female") res = Rabbit.SexType.FEMALE;
            return res;
        }

        public static String GetFBon(String bon, bool shr)
        {
            Char fbon = '5';
            for (int i = 1; i < bon.Length; i++)
                if (bon[i] < fbon)
                    fbon = bon[i];
            switch (fbon)
            {
                case '1': return "III";
                case '2': return "II";
                case '3': return "I";
                case '4': return (shr ? "Э" : "Элита");
            }
            return (shr ? "-" : "Нет");
        }
        public static String GetFBon(int bon, bool shr) { return GetFBon(bon.ToString(), shr); }
        #endregion static

        #region private_help
        protected static string replaceChar(string str, int ind, char replacement)
        {
            if (ind >= str.Length) return str;
            return str.Substring(0, ind) + replacement + (str.Length - 1 - ind <= 0 ? "" : str.Substring(ind + 1, str.Length - 1 - ind));
        }
        protected static void commaConcat(ref string ser, string app)
        {
            if (ser.Length != 0)
                ser += "," + app;
            else
                ser += app;
        }
        #endregion private_help
    }

    public class AdultRabbit : Rabbit, IData
    {
        #region static
        //private static bool s_showNum = false;
        //private static int s_brideAge = 122;
        //private static int s_candAge = 120;
        #endregion static
        protected int _rate;
        protected String _flags;
        protected int _weight;
        protected int _status;
        protected DateTime _eventDate;
        private int _kidsAge = -1;
        private int _kidsCount = 0;
        private string _vacFlags = "";

        /*public Rabbit(int id)
        {
            ID = id;
        }*/

        public AdultRabbit(int id, string rabname, string sex, DateTime born, string breedname, int group, String bon, string rawAddress, string notes,
                int rate, string flags, int weight, int status, DateTime eventDate, int kidsCount, int kidsAge, string vacFlags)
            : base(id, rabname, sex, born, breedname, group, bon, rawAddress, notes)
        {
            _rate = rate;
            _flags = flags;
            _weight = weight;
            _status = status;
            _eventDate = eventDate;
            _kidsCount = kidsCount;
            _kidsAge = kidsAge;
            _vacFlags = vacFlags;
           
        }

        #region properties
        public int Rate { get { return _rate; } set { _rate = value; } }
        public int Sukrol { get { return DateTime.Now.Subtract(_eventDate).Days; } }
        public virtual int KidsCount { get { return _kidsCount; } }
        public virtual int KidsAge { get { return _kidsCount > 0 ? _kidsAge : -1; } }

        public String FGroup()
        {
            string res = "-";
            if (Sex == Rabbit.SexType.FEMALE && _kidsCount > 0)
                res = String.Format("+{0,2:d}", _kidsCount);
            else if (Group > 1)
                res = String.Format("[{0,2:d}]", Group);
            return res;
        }


        public string FFlags()
        {
            String flg = _flags;
            string res = "";
            if (flg[2] == '1') commaConcat(ref res, "Б");
            if (flg[0] != '0') commaConcat(ref res, "ГП");
            commaConcat(ref res, _vacFlags);
            if (flg[1] != '0') res = "<" + res + ">";
            return res;
        }

        public String FStatus(bool s_short, int s_candAge, int s_brideAge)
        {
            string res = "";
            if (Sex == Rabbit.SexType.VOID)
            {
                res = s_short ? "Бпл" : "бесполые";
            }
            else if (Sex == Rabbit.SexType.MALE)
            {
                if (_status == 2)
                    res = s_short ? "Прз" : "производитель";
                else if (_status == 1 || Age >= s_candAge)
                    res = s_short ? "Кнд" : (Group > 1 ? "кандидаты" : "кандидат");
                else
                    res = s_short ? "Мал" : (Group > 1 ? "мальчики" : "мальчик");
            }
            else
            {
                if (Age < s_brideAge)
                    res = s_short ? "Дев" : (Group > 1 ? "Девочки" : "Девочка");
                else
                    res = s_short ? "Нвс" : (Group > 1 ? "Невесты" : "Невеста");

                if ((_status == 1 && _eventDate == DateTime.MinValue) || (_status == 0 && _eventDate != DateTime.MinValue))
                    res = s_short ? "Прк" : "Первокролка";
                if (_status > 1 || (_status == 1))
                    res = s_short ? "Штн" : "Штатная";
            }
            return res;
        }
        public String FStatus() { return FStatus(false, 120, 122); }

        public override String FSex()
        {
            String res = base.FSex();
            if (Sex == Rabbit.SexType.FEMALE && _eventDate != DateTime.MinValue)
                res = String.Format("C-{0,2:d}", Sukrol);
            return res;
        }

        public String FWeight() { return _weight == -1 ? "?" : _weight.ToString(); }

        #endregion properties
    }

    public class OneRabbit : AdultRabbit
    {
        #region prop
        //protected string _flags;
        //public Rabbit.Sex sex;
        /// <summary>
        /// Дата рождения
        /// </summary>
        //public DateTime born;
        /// <summary>
        /// Рэйтинг
        /// </summary>
        //public int Rate;
        //public int ID;
        /// <summary>
        /// Кормилица
        /// </summary>
        public int parentId;
        public int nameID;
        public int wasname;
        public int surname;
        public int secname;
        public int BreedID;
        public int zone;
        /// <summary>
        /// Заметки
        /// </summary>
        //public String notes;
        public string gens;
        public DateTime lastfuckokrol;
        public int evtype;
        public int kidsOverAll;
        public int kidsLost;
        /// <summary>
        /// молодняк
        /// </summary>
        public YoungRabbit[] youngers = null;
        public OneRabbit[] neighbors;
        public RabVac[] rabVacs;
        public string tag;
        public string nuaddr = "";
        protected DateTime _weightDate;
        protected string _newAddress = "";
        /// <summary>
        /// Каким окролом родился
        /// </summary>
        private int _okrol;
        #endregion prop
        public OneRabbit(int id, string sx, DateTime born, int rate, string flags, int nameId, int surnameId, int secnameId, string rawAddress, int group, int brd, int zone, String notes,
            String genom, int status, DateTime lastFuckOkrol, String eventType, DateTime eventDate, int overAllBabys, int lostBabys, String fullName, String breedName,
            String bon, int parent, int okrol, int weight, DateTime weightDate)
            : base(id, fullName, sx, born, breedName, group, bon, rawAddress, notes, rate, flags, weight, status, eventDate, 0, -1, "")
        {
            this.parentId = parent;
            tag = "";
            nameID = nameId;
            wasname = nameID;
            surname = surnameId;
            secname = secnameId;
            BreedID = brd;
            this.zone = zone;
            gens = genom;
            if (sx == "void") status = Age < 50 ? 0 : 1; //TODO проверить на необходимость
            lastfuckokrol = lastFuckOkrol;
            evtype = Rabbit.GetEventType(eventType);
            _okrol = okrol;
            kidsOverAll = overAllBabys;
            kidsLost = lostBabys;
            WeightDate = weightDate;
        }
        public int Okrol { get { return _okrol; } }
        public DateTime WeightDate { get { return _weightDate; } set { _weightDate = value; } }
        public override string Address
        {
            get { return _newAddress == "" ? base.Address : _newAddress; }
            set { _newAddress = value; }
        }

        public string Bon
        {
            get { return _bon; }
            set { _bon = value; }
        }
        public DateTime EventDate
        {
            get { return _eventDate; }
            set { _eventDate = value; }
        }
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int WeightAge
        {
            get { return WeightDate.Subtract(BirthDay).Days; }
        }
        #region flags
        /// <summary>
        /// Готовая продукция
        /// </summary>
        public bool Production
        {
            get { return _flags[0] == '1'; }
            set { _flags = replaceChar(_flags, 0, value ? '1' : '0'); }
        }
        /// <summary>
        /// Готов к реализации
        /// </summary>
        public bool RealizeReady
        {
            get { return _flags[1] == '1'; }
            set { _flags = replaceChar(_flags, 1, value ? '1' : '0'); }
        }
        public bool Defect
        {
            get { return _flags[2] == '1' || _flags[2] == '3'; }
            set { _flags = replaceChar(_flags, 2, value ? '1' : '0'); }
        }
        public bool NoKuk
        {
            get { return _flags[3] == '1'; }
            set { _flags = replaceChar(_flags, 3, value ? '1' : '0'); }
        }
        public bool NoLact
        {
            get { return _flags[4] == '1'; }
            set { _flags = replaceChar(_flags, 4, value ? '1' : '0'); }
        }
        //public bool risk;     
        #endregion flags

        public string RawAddress { get { return _rawAddress; } }
    }
}
