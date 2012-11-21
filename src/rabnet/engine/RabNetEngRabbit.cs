using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace rabnet
{
    public class RabNetEngRabbit:OneRabbit
    {
        #region exceptions
        public class ExNotFemale : ApplicationException
        {
            public ExNotFemale(RabNetEngRabbit r):base("Кролик "+r.FullName+" не является самкой"){}
        }
        public class ExBadPastDays : ApplicationException
        {
            public ExBadPastDays() : base("Дни не могут быть меньше нуля") { }
        }
        public class ExNotMale : ApplicationException
        {
            public ExNotMale(RabNetEngRabbit r) : base("Кролик " + r.FullName + " не является самцом") { }
        }
        public class ExNotFucker : ApplicationException
        {
            public ExNotFucker(RabNetEngRabbit r) : base("Кролик " + r.FullName + " не является половозрелым") { }
        }
        public class ExBadDate : ApplicationException
        {
            public ExBadDate(DateTime dt) : base("Дата " + dt.ToShortDateString() + " в будущем") { }
        }
        public class ExAlreadyFucked:ApplicationException
        {
            public ExAlreadyFucked(RabNetEngRabbit r):base("Крольчиха "+r.FullName+" уже сукрольна"){}
        }
        public class ExNotFucked : ApplicationException
        {
            public ExNotFucked(RabNetEngRabbit r) : base("Крольчиха " + r.FullName + " не сукрольна") { }
        }
        public class ExNoName : ApplicationException
        {
            public ExNoName():base("У сукрольной крольчихи должно быть имя"){}
        }
        public class ExNotOne : ApplicationException
        {
            public ExNotOne(String action) : base("Нельзя "+action+" группу крольчих") { }
        }
        public class ExBadCount : ApplicationException
        {
            public ExBadCount():base("Неверное количество."){}
        }
        public class ExNoRabbit : ApplicationException
        {
            public ExNoRabbit() : base("Кролик не существует.") { }
        }
        #endregion exceptions
        //private int _id;
        private OneRabbit _origin = null;
        private String _rabGenoms="";
        private YoungRabbit[] _youngers = null;
        private RabVac[] _rabVacs=null;
        private string _tag = "";
        private OneRabbit[] _neighbors=null;
        private RabNetEngine _eng = null;
        public int Mom = 0;
        /// <summary>
        /// Нужно для логирования при Клонировании
        /// </summary>
        internal string CloneAddress="";

        public RabNetEngRabbit(int rid,RabNetEngine dl)
        {
            _id = rid;
            _eng = dl;
            loadData();            
        }

        private void loadData()
        {
            _origin = _eng.db().GetRabbit(_id);
            if (_origin == null) throw new ExNoRabbit();

            cloneOnThis(_origin);
        }

        public RabNetEngRabbit(RabNetEngine dl,Rabbit.SexType sx)
        {
            _id = 0;
            _eng = dl;
            String s="void";
            if (sx == Rabbit.SexType.FEMALE) s = "female";
            if (sx == Rabbit.SexType.MALE) s = "male";
            //_rab = new OneRabbit(0, s, DateTime.Now, 0, Rabbit.NULL_FLAGS, 0, 0, 0, "", 1, 1, 0, "", "", 0, DateTime.MinValue, "", 
              //  DateTime.MinValue, 0, 0, "", "", Rabbit.NULL_BON,0,1,-1,DateTime.MinValue,0,0);
            _youngers=new YoungRabbit[0];
        }
        //TODO Сделать это на уровне OneRabbit
        
        #region inherited_properties

        //public int ID { get { return _rab.ID; } }
        //public Rabbit.SexType Sex
        //{   
        //    get { return _rab.Sex; }
        //    set { _rab.Sex=value; }
        //}
        ///// <summary>
        ///// Количество кроликов в группе
        ///// </summary>
        //public int Group
        //{
        //    get { return _rab.Group; }
        //    set { _rab.Group = value; }
        //}
        ///// <summary>
        ///// Возраст
        ///// </summary>
        //public int Age { get { return _rab.Age; } }
        //public DateTime BirthDay
        //{
        //    get { return _rab.BirthDay; }
        //    set { _rab.BirthDay = value; }
        //}
        ///// <summary>
        ///// Порода
        ///// </summary>
        //public String BreedName { get { return _rab.BreedName; } }      
        //public String Notes
        //{
        //    get { return _rab.Notes; }
        //    set { _rab.Notes = value; }
        //}

        //public int Rate
        //{
        //    get { return _rab.Rate; }
        //    set { _rab.Rate = value; }
        //}

        //public int Parent { get { return _rab.ParentID; } }
        //public int WasNameID { get { return _rab.WasNameID; } }
        //public int NameID
        //{
        //    get { return _rab.NameID; }
        //    set { _rab.NameID = value; }
        //}
        ///// <summary>
        ///// Фамили по отцу
        ///// </summary>
        //public int SurnameID
        //{
        //    get { return _rab.SurnameID; }
        //    set { _rab.SurnameID = value; }
        //}
        ///// <summary>
        ///// Фамилия по матери
        ///// </summary>
        //public int SecnameID
        //{
        //    get { return _rab.SecnameID; }
        //    set { _rab.SecnameID = value; }
        //}      
        ///// <summary>
        ///// Порода кролика
        ///// </summary>
        //public int BreedID
        //{
        //    get { return _rab.BreedID;}
        //    set {_rab.BreedID=value;}
        //}
        //public int Zone
        //{
        //    get { return _rab.Zone; }
        //    set { _rab.Zone = value; }
        //}
        //public String Genoms
        //{
        //    get { return _rab.Genoms; }
        //    set { _rab.Genoms = value; }
        //}
        ///// <summary>
        ///// Заметки
        ///// </summary>               
        //public DateTime LastFuckOkrol
        //{
        //    get { return _rab.LastFuckOkrol; }
        //    set { _rab.LastFuckOkrol = value; }
        //}
        //public int EventType { get { return _rab.EventType; } }
        //public int KidsOverAll
        //{
        //    get { return _rab.KidsOverAll; }
        //    set { _rab.KidsOverAll = value; }
        //}
        //public int KidsLost
        //{
        //    get { return _rab.KidsLost; }
        //    set { _rab.KidsLost = value; }
        //}

        ///// <summary>
        ///// Бонитировка
        ///// </summary>
        //public String Bon { get { return _rab.Bon; } }
        //public DateTime EventDate { get { return _rab.EventDate; } }
        //public int Status
        //{
        //    get { return _rab.Status; }
        //    set { _rab.Status = value; }
        //}
                     
        //public bool Production
        //{
        //    get { return _rab.Production; }
        //    set { _rab.Production = value; }
        //}
        //public bool RealizeReady
        //{
        //    get { return _rab.RealizeReady; }
        //    set { _rab.RealizeReady = value; }
        //}
        //public bool Defect
        //{
        //    get { return _rab.Defect; }
        //    set { _rab.Defect = value; }
        //}
        //public bool NoKuk
        //{
        //    get { return _rab.NoKuk; }
        //    set { _rab.NoKuk = value; }
        //}
        //public bool NoLact
        //{
        //    get { return _rab.NoLact; }
        //    set { _rab.NoLact = value; }
        //}        
        
        //public string RawAddress { get { return _rab.RawAddress; } }
        //public String Address { get { return _rab.Address; } }
        //public String NewAddress { get { return _rab.NewAddress; } }
        //public string AddressSmall { get { return _rab.AddressSmall; } }
        #endregion inherited_properties

        #region own_props
        public string TierType
        {
            get
            {
                string[] values = this.RawAddress.Split(',');
                return values[3];
            }
        }

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public bool SetNest
        { 
            get 
            {
                string[] values = this.RawAddress.Split(',');
                if (values.Length < 3)
                    return false;
                if (values[3] == BuildingType.Jurta && values[2] == "0" && values[5] == "1")
                    return true;
                if (values[3] == BuildingType.Female && values[5] == "1")
                    return true;
                if (values[3] == BuildingType.DualFemale && ((values[2] == "0" && values[5][0] == '1') || (values[2] == "1" && values[5][1] == '1')))
                    return true;
                return false;
            } 
        }       
        
        /// <summary>
        /// Полное имя (Азалия Гамбитова-Явина,0)
        /// </summary>
        public String FullName
        {
            get
            {
                if (_id == 0) return _eng.db().makeName(_nameID, _surnameID, _secnameID, Group, Sex);
                return this.NameFull; 
            } 
        }

        /// <summary>
        /// Прививки кролика
        /// </summary>
        public RabVac[] Vaccines
        {
            get
            {
                if (_rabVacs == null)
                    _rabVacs = _eng.db().GetRabVac(_id);
                return _rabVacs;
            }
        }
        
        /// <summary>
        /// Общее моличество подсосных крольчат (если это Кормилица)
        /// </summary>
        public int YoungCount 
        { 
            get 
            {
                int c = 0;
                foreach (YoungRabbit r in Youngers)
                    c += r.Group;
                return c;
            } 
        }
        
        /// <summary>
        /// Лист с группами подсосных
        /// </summary>
        public YoungRabbit[] Youngers 
        { 
            get 
            { 
                if(_youngers ==null)
                    return _youngers = _eng.db().GetYoungers(_id);
                return _youngers; 
            } 
        }
        public OneRabbit[] Neighbors
        {
            get 
            {
                if (_neighbors == null)
                    _neighbors = _eng.db().GetNeighbors(_id);
                return _neighbors; 
            }
        }

        public String RabGenoms
        {
            get
            {
                if (String.IsNullOrEmpty(_rabGenoms))
                    _rabGenoms = _eng.db().GetRabGenoms(_id);
                return _rabGenoms;
            }
        }
        #endregion own_props

        #region methods
        /// <summary>
        /// Сохраняет изменение данных на сервере
        /// </summary>
        public void Commit()
        {
            if (_id == 0) return;

            if (this.WasNameID != this.NameID)
            {
                if (Group > 1) throw new ExNotOne("переименовать");
                _eng.logs().log(RabNetLogs.LogType.RENAME, ID, 0, this.AddressSmall, "", _eng.db().makeName(this.WasNameID, 0, 0, 1, this.Sex));
            }
            if(_origin!=null && _origin.Sex != this.Sex)           
                _eng.logs().log(RabNetLogs.LogType.SET_SEX, ID, 0, this.AddressSmall, "",String.Format("{0:s} -> {1:s}", Rabbit.SexToRU(_origin.Sex), Rabbit.SexToRU(this.Sex)));               
            if(_origin!=null && _origin.BirthDay!=this.BirthDay)
                _eng.logs().log(RabNetLogs.LogType.CH_BIRTH, ID, 0, this.AddressSmall,"",String.Format("{0:s} -> {1:s}", _origin.BirthDay.ToShortDateString(), this.BirthDay.ToShortDateString()));               
            if(_origin!=null && _origin.BreedID!=this.BreedID)
                _eng.logs().log(RabNetLogs.LogType.CH_BREED, ID, 0, this.AddressSmall,"",String.Format("{0:s} -> {1:s}", _origin.BreedName, this.BreedName));               
            if(_origin!=null && _origin.Group !=this.Group)
                _eng.logs().log(RabNetLogs.LogType.CH_GROUP, ID, 0, this.AddressSmall,"",String.Format("{0:s} -> {1:s}", _origin.Group, this.Group));               
            if(_origin!=null && _origin.SurnameID !=this.SurnameID)
                _eng.logs().log(RabNetLogs.LogType.CH_SURNAME, ID, 0, this.AddressSmall,"",String.Format("{0:s} -> {1:s}", _origin.SurnameID, this.SurnameID));  
            if(_origin!=null && _origin.SecnameID !=this.SecnameID)
                _eng.logs().log(RabNetLogs.LogType.CH_SECNAME, ID, 0, this.AddressSmall,"",String.Format("{0:s} -> {1:s}", _origin.SecnameID, this.SecnameID));  
            if(_origin!=null && _origin.Zone !=this.Zone)
                _eng.logs().log(RabNetLogs.LogType.CH_ZONE, ID, 0, this.AddressSmall,"",String.Format("{0:s} -> {1:s}", _origin.Zone, this.Zone));  
            if(_origin!=null && _origin.Rate !=this.Rate)
                _eng.logs().log(RabNetLogs.LogType.CH_RATE, ID, 0, this.AddressSmall,"",String.Format("{0:s} -> {1:s}", _origin.Rate, this.Rate));  
            if(_origin!=null && _origin.Status !=this.Status)
                _eng.logs().log(RabNetLogs.LogType.CH_STATE, ID, 0, this.AddressSmall,"",String.Format("{0:s} -> {1:s}", _origin.FStatus(), this.FStatus()));  
            //_eng.logs().log(RabNetLogs.LogType.RAB_CHANGE, ID);
            _eng.db().SetRabbit(this);
            loadData();
            //_origin = _eng.db().GetRabbit(_id);
        }

        /// <summary>
        /// Создать нового кролика из текущего объекта
        /// </summary>
        public void CommitNew()
        {
            if (_id != 0) return;

            _id = _eng.db().newRabbit(this, Mom);
            //_origin.ID = _id;
            _eng.logs().log(RabNetLogs.LogType.INCOME, _id);
        }

        public void SetBon(String bon)
        {
            if (ID == 0)
                this.Bon = bon;
            else
            {
                _eng.logs().log(RabNetLogs.LogType.BON, ID, 0, "", "", bon);
                _eng.db().setBon(_id, bon);
            }
        }
        
        /// <summary>
        /// Отметить вязку самки
        /// </summary>
        /// <param name="maleId">ID самца</param>
        /// <param name="when">Дата вязки</param>
        /// <param name="syntetic">Искусственное осеменение</param>
        public void FuckIt(int maleId, int daysPast, bool syntetic)
        {
            if (Sex != Rabbit.SexType.FEMALE)
                throw new ExNotFemale(this);
            if (Age<_eng.brideAge())
                throw new ExNotFucker(this);
            if (EventDate != DateTime.MinValue)
                throw new ExAlreadyFucked(this);
            if (_nameID == 0) throw new ExNoName();
            if (Group > 1) throw new ExNotOne("случить");
            RabNetEngRabbit f = _eng.getRabbit(maleId);
            if (f.Sex != Rabbit.SexType.MALE)
                throw new ExNotMale(f);
            if (f.Status < 1)
                throw new ExNotFucker(f);
            //if (daysPast <0)
                //throw new ExBadDate(daysPast.ToString());
            _eng.logs().log(RabNetLogs.LogType.FUCK, ID, maleId, AddressSmall, f.AddressSmall, 
                (syntetic ? "ИО" : "стд.") + (daysPast != 0 ? String.Format(" {0:d} дней назад", daysPast) : ""));
            _eng.db().MakeFuck(this._id, f.ID, daysPast, _eng.userId, syntetic);
        }
        public void FuckIt(int otherrab, int daysPast)
        {
            FuckIt(otherrab, daysPast, false);
        }
        
        /// <summary>
        /// Отметить прохолост (самка не окролилась)
        /// </summary>
        /// <param name="when">Дата установки прохолоста</param>
        public void ProholostIt(int daysPast)
        {
            if (Sex != Rabbit.SexType.FEMALE) throw new ExNotFemale(this);
            if (EventDate == DateTime.MinValue) throw new ExNotFucked(this);
            //if (when > DateTime.Now) throw new ExBadDate(when);
            if (daysPast < 0) throw new ExBadPastDays();

            _eng.logs().log(RabNetLogs.LogType.PROHOLOST, ID, 0, AddressSmall, "", daysPast != 0 ? String.Format(" {0:d} дней назад", daysPast) : "");
            _eng.db().makeProholost(this._id, daysPast);
            if(_eng.options().getBoolOption(Options.OPT_ID.NEST_OUT_IF_PROHOLOST))
            {
                //todo пиздец и говнокод и опасно но...
                RabNetEngBuilding rnd = RabNetEngBuilding.FromPlace(this.RawAddress, _eng);
                rnd.RabbitNestOut(this.ID);
            }
        }
        
        /// <summary>
        /// Принять окрол
        /// </summary>
        /// <param name="when">Дата принятия окрола</param>
        /// <param name="children">Количество родившихся живых крольчат</param>
        /// <param name="dead">Количество родившихся мертвых крольчат</param>
        public void OkrolIt(int daysPast, int children, int dead)
        {
            if (Sex != Rabbit.SexType.FEMALE) throw new ExNotFemale(this);
            if (EventDate == DateTime.MinValue) throw new ExNotFucked(this);
            //if (when > DateTime.Now) throw new ExBadDate(when);  

            int born = _eng.db().makeOkrol(this._id, daysPast, children, dead);
            _eng.logs().log(RabNetLogs.LogType.OKROL, ID, born, AddressSmall, "", 
                String.Format("живых {0:d}, мертвых {1:d}{2:s}", children, dead, daysPast != 0 ? String.Format(" {0:d} дней назад",daysPast) : ""));
        }

        public void ReplaceRabbit(int farm,int tier_id,int sec,string address)
        {
            if (_id == 0)
            {
                this.Address = address;
                this.NewAddress = String.Format("{0:d}|{1:d}|{2:d}", farm, tier_id, sec);
            }
            else
            {
                _eng.logs().log(RabNetLogs.LogType.REPLACE, ID, 0, this.AddressSmall, address.TrimEnd(' ').Substring(0,address.LastIndexOf(' ')));
                _eng.db().replaceRabbit(ID, farm, tier_id, sec);
            }
            _tag = "";
        }

        public void ReplaceYounger(int yid, int farm, int tier, int sec, string address)
        {
            _eng.db().replaceYounger(yid, farm, tier, sec);
            /*foreach (YoungRabbit y in Youngers) //TODO warning removing tag
                if (y.ID == yid)
                    y.tag = "";*/
            OneRabbit r = _eng.db().GetRabbit(yid);
            _eng.logs().log(RabNetLogs.LogType.REPLACE, yid,0,r.AddressSmall,address);
        }

        /// <summary>
        /// Списать кролика
        /// </summary>
        /// <param name="when">Дата списания</param>
        /// <param name="reason">Причина списания</param>
        /// <param name="notes">Заметки по данному списанию</param>
        /// <param name="count">Количество списанных кроликов</param>
        public void KillIt(int daysPast, int reason, string notes,int count)
        {
            if (count == Group)
            {
                _eng.logs().log(RabNetLogs.LogType.RABBIT_KILLED, ID, 0, AddressSmall == Rabbit.NULL_ADDRESS ? CloneAddress : AddressSmall, "",
                    String.Format(" {0:s}[{1:d}] {2:s} {3:s}", FullName, Group, notes, (daysPast != 0 ? String.Format(" {0:d} дней назад", daysPast) : "")));
                _eng.db().KillRabbit(_id, daysPast, reason, notes);
            }
            else
            {
                int nid = Clone(count, 0, 0, 0);
                RabNetEngRabbit nr = new RabNetEngRabbit(nid, _eng);
                nr.CloneAddress = AddressSmall;
                nr.KillIt(daysPast, reason, notes, count);
            }
        }

        /// <summary>
        /// Подсчет подсосных\гнездовых
        /// </summary>
        /// <param name="dead">Количество мертвых</param>
        /// <param name="killed">Количество притоптанных</param>
        /// <param name="added">Количество появившихся</param>
        /// <param name="atall">Всего кроликов</param>
        /// <param name="age">Возраст детей</param>
        /// <param name="yInd">Индекс детей</param>
        public void CountKids(int dead,int killed,int added,int atall,int age,int yInd)
        {
            const byte DR_ON_COUNT=5;//deadreason "при подсчете"
            if (Sex != Rabbit.SexType.FEMALE)
                throw new ExNotFemale(this);
            _eng.logs().log(RabNetLogs.LogType.COUNT_KIDS, ID, _youngers[yInd].ID, this.AddressSmall,"", String.Format("  возраст {0:d} всего {1:d} (умерло {2:d}, притоптано {3:d}, прибавилось {4:d})", age, atall, dead, killed, added));            
            if (dead == 0 && killed == 0 && added == 0) return;
            YoungRabbit y = _youngers[yInd];
            RabNetEngRabbit r = _eng.getRabbit(y.ID);        
            if (atall == 0)
            {
                r.CloneAddress = AddressSmall;
                r.KillIt(0, DR_ON_COUNT, "при подсчете", y.Group);
            }
            else
            {
                RabNetEngRabbit clone = _eng.getRabbit(r.Clone(dead + killed, 0, 0, 0));
                clone.CloneAddress = AddressSmall;
                clone.KillIt(0, DR_ON_COUNT, "при подсчете", clone.Group);
                //!!!тут надо списывать
                if(added>0)
                    _eng.db().СountKids(_id,dead, killed, added, _youngers[yInd].ID);             
            }
        }

        /// <summary>
        /// Установить пол
        /// </summary>
        /// <param name="sex">Новый пол</param>
        public void SetSex(Rabbit.SexType sex)
        {
            _eng.logs().log(RabNetLogs.LogType.SET_SEX, _id, 0, "", "", Rabbit.SexToRU(sex));
            _eng.db().setRabbitSex(_id, sex);
        }

        /// <summary>
        /// Отделяет несколько кроликов от Текущей группы и создает новую группу
        /// </summary>
        /// <param name="count">Сколько кроликов в новой группе</param>
        /// <param name="farm">ID фермы</param>
        /// <param name="tier">Ярус</param>
        /// <param name="sec">Клетка</param>
        /// <returns></returns>
        public int Clone(int count,int farm,int tier,int sec)
        {
            if (Group <= count) throw new ExBadCount(); //todo вставил проверку на = , надо попроверять

           int nid = _eng.db().cloneRabbit(_id, count, farm, tier, sec, Rabbit.SexType.VOID, 0);
           RabNetEngRabbit rab = Engine.get().getRabbit(nid);       //+gambit
           _eng.logs().log(RabNetLogs.LogType.CLONE_GROUP, _id, nid, AddressSmall, rab.AddressSmall, String.Format("{0:d} и {1:d}", Group-count ,count));
           return nid;
        }
        
        /// <summary>
        /// Обьединить с группой
        /// </summary>
        /// <param name="rabto">ID кролика с которым объединить</param>
        public void CombineWidth(int rabto)
        {
            RabNetEngRabbit rab = Engine.get().getRabbit(rabto);    //+gambit
            _eng.logs().log(RabNetLogs.LogType.COMBINE, _id, rabto, AddressSmall, rab.AddressSmall , rab.FullName);
            _eng.db().combineGroups(_id, rabto);
        }

        public void PlaceSuckerTo(int mother)
        {
            RabNetEngRabbit mom_to = Engine.get().getRabbit(mother);
            _eng.logs().log(RabNetLogs.LogType.PLACE_SUCK, _id, mother, "", mom_to.AddressSmall);
            _eng.db().placeSucker(_id, mother);
        }

        /// <summary>
        /// Делает привиквку кролику
        /// </summary>
        /// <param name="vid">ID прививки</param>
        /// <param name="date">Дата прививки</param>
        /// <param name="withChildrens">Привить вместе с подсосными</param>
        public void SetVaccine(int vid, DateTime date, bool withChildrens)
        {
            _eng.db().SetRabbitVaccine(_id,vid,date);
            Vaccine v =_eng.db().GetVaccine(vid);
            _eng.logs().log(RabNetLogs.LogType.VACCINE, ID, 0, AddressSmall, "", v.Name);
            if (withChildrens)
            {
                foreach (YoungRabbit r in Youngers)
                {
                    _eng.db().SetRabbitVaccine(r.ID, vid, date);
                }
                _youngers = _eng.db().GetYoungers(_id);
            }
            _rabVacs = _eng.db().GetRabVac(_id);                        
        }

        /// <summary>
        /// Обновляет данные с сервера по подсосным
        /// </summary>
        public void YoungersUpdate()
        {
            _youngers = Engine.db().GetYoungers(_id);
        }
        #endregion methods

        private void cloneOnThis(OneRabbit r)
        {
            Type t = r.GetType();

            FieldInfo[] fis = t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo f in fis)
            {
                f.SetValue(this, f.GetValue(r));
            }
        }
    }
}
