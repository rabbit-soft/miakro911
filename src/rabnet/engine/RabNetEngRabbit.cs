using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetEngRabbit
    {
        #region exceptions
        public class ExNotFemale : ApplicationException
        {
            public ExNotFemale(RabNetEngRabbit r):base("Кролик "+r.FullName+" не является самкой"){}
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
        private int _id;
        private OneRabbit _rab = null;
        private RabNetEngine _eng = null;
        public int Mom = 0;

        public RabNetEngRabbit(int rid,RabNetEngine dl)
        {
            _id = rid;
            _eng = dl;
            _rab = _eng.db().GetRabbit(rid);
            if (_rab == null)
                throw new ExNoRabbit();
        }

        public RabNetEngRabbit(RabNetEngine dl,OneRabbit.RabbitSex sx)
        {
            _id = 0;
            _eng = dl;
            String s="void";
            if (sx == OneRabbit.RabbitSex.FEMALE) s = "female";
            if (sx == OneRabbit.RabbitSex.MALE) s = "male";
            _rab = new OneRabbit(0, s, DateTime.Now, 0, "00000", 0, 0, 0, "", 1, 1, 0, "", "", 0, DateTime.MinValue, "", DateTime.MinValue, 0, 0, "", "", "00000",0,0/*,DateTime.Now*/);
            _rab.youngers=new OneRabbit[0];
        }

        public void newCommit()
        {
            if (_id != 0) return;
            _id = _eng.db().newRabbit(_rab, Mom);
            _rab.id = _id;
            _eng.logs().log(RabNetLogs.LogType.INCOME, _id);
        }

        /// <summary>
        /// Сохраняет изменение данных на сервере
        /// </summary>
        public void Commit()
        {
            if (RabID == 0)
                return;
            if (_rab.wasname != _rab.name)
            {
                if (Group > 1)
                    throw new ExNotOne("переименовать");
                _eng.logs().log(RabNetLogs.LogType.RENAME, RabID, 0, "", "", _eng.db().makeName(_rab.wasname, 0, 0, 1, _rab.sex));
            }
            else _eng.logs().log(RabNetLogs.LogType.RAB_CHANGE, RabID);
            _eng.db().SetRabbit(_rab);
            _rab=_eng.db().GetRabbit(_id);
        }

        //TODO Сделать это на уровне OneRabbit
        #region properties
        public OneRabbit.RabbitSex Sex
        {   
            get { return _rab.sex; }
            set { _rab.sex=value; }
        }
        public DateTime Born
        {
            get { return _rab.born; }
            set { _rab.born = value; }
        }
        public int Rate
        {
            get { return _rab.rate; }
            set { _rab.rate = value; }
        }
        public bool Defect
        {
            get { return _rab.defect; }
            set { _rab.defect = value; }
        }
        public bool Production
        {
            get { return _rab.gp; }
            set { _rab.gp = value; }
        }
        public bool Realization
        {
            get { return _rab.gr; }
            set { _rab.gr = value; }
        }
        public bool Spec
        {
            get { return _rab.spec; }
            set { _rab.spec = value; }
        }
        public int Name
        {
            get { return _rab.name; }
            set { _rab.name = value; }
        }
        /// <summary>
        /// Фамили по отцу
        /// </summary>
        public int Surname
        {
            get { return _rab.surname; }
            set { _rab.surname = value; }
        }
        /// <summary>
        /// Фамилия по матери
        /// </summary>
        public int SecondName
        {
            get { return _rab.secname; }
            set { _rab.secname = value; }
        }
        /// <summary>
        /// Количество кроликов в группе
        /// </summary>
        public int Group
        {
            get { return _rab.group; }
            set { _rab.group = value; }
        }
        /// <summary>
        /// Порода кролика
        /// </summary>
        public int Breed
        {
            get { return _rab.breed;}
            set {_rab.breed=value;}
        }
        public int Zone
        {
            get { return _rab.zone; }
            set { _rab.zone = value; }
        }
        /// <summary>
        /// Заметки
        /// </summary>
        public String Notes
        {
            get { return _rab.notes; }
            set { _rab.notes = value; }
        }
        public int Status
        {
            get { return _rab.status; }
            set { _rab.status = value; }
        }
        public DateTime Last_Fuck_Okrol
        {
            get { return _rab.lastfuckokrol; }
            set { _rab.lastfuckokrol = value; }
        }
        /// <summary>
        /// Прививки кролика
        /// </summary>
        public RabVac[] Vaccines
        {
            get { return _rab.rabVacs; }
            //set { rab.rabVacs = value; }
        }
        public String Genom
        {
            get { return _rab.gens; }
            set { _rab.gens = value; }
        }
        public bool NoLact
        {
            get { return _rab.nolact; }
            set { _rab.nolact = value; }
        }
        public bool NoKuk
        {
            get { return _rab.nokuk; }
            set { _rab.nokuk = value; }
        }
        public int Babies
        {
            get { return _rab.babies; }
            set { _rab.babies = value; }
        }
        public int Lost
        {
            get { return _rab.lost; }
            set { _rab.lost = value; }
        }
        public string Tag
        {
            get { return _rab.tag; }
            set { _rab.tag = value; }
        }
        public string SmallAddress{get{return _rab.smallAddress;}}
        public string JustAddress{get{return _rab.justAddress;}}
        /// <summary>
        /// Нужно для логирования при Клонировании
        /// </summary>
        public string CloneAddress;
        public string TierType
        {
            get
            {
                string[] values = _rab.justAddress.Split(',');
                return values[3];
            }
        }
        public string medAddress { get { return _rab.medAddress; } }
        public int Parent { get { return _rab.parent; } }
        public int RabID { get { return _rab.id; } }
        public int EventType { get { return _rab.evtype; } }
        public DateTime EventDate { get { return _rab.evdate; } }
        public int WasName { get { return _rab.wasname; } }
        public String Address { get { return _rab.address; } }
        public String NewAddress { get { return _rab.nuaddr; } }
        #endregion propertie

        public bool SetNest
        { 
            get 
            {
                string[] values = _rab.justAddress.Split(',');
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
                if (_id == 0) return _eng.db().makeName(Name, Surname, SecondName, Group, Sex);
                return _rab.fullname; 
            } 
        }
        /// <summary>
        /// Порода
        /// </summary>
        public String BreedName { get { return _rab.breedname; } }
        /// <summary>
        /// Бонитировка
        /// </summary>
        public String Bon { get { return _rab.bon; } }
        /// <summary>
        /// Возраст
        /// </summary>
        public int age { get { return (DateTime.Now.Date - Born.Date).Days; } }
        /// <summary>
        /// Общее моличество подсосных крольчат (если это Кормилица)
        /// </summary>
        public int YoungCount 
        { 
            get 
            {
                int c = 0;
                foreach (OneRabbit r in Youngers)
                    c += r.group;
                return c;
            } 
        }
        /// <summary>
        /// Лист с группами подсосных
        /// </summary>
        public OneRabbit[] Youngers 
        { 
            get { return _rab.youngers; } 
        }
        public OneRabbit[] Neighbors
        {
            get { return _rab.neighbors; }
        }
        
        public void SetBon(String bon)
        {
            if (RabID == 0)
                _rab.bon = bon;
            else
            {
                _eng.logs().log(RabNetLogs.LogType.BON, RabID, 0, "", "", bon);
                _eng.db().setBon(_id, bon);
            }
        }
        /// <summary>
        /// Отметить вязку самки
        /// </summary>
        /// <param name="otherrab">ID самца</param>
        /// <param name="when">Дата вязки</param>
        public void FuckIt(int otherrab, DateTime when)
        {
            if (Sex != OneRabbit.RabbitSex.FEMALE)
                throw new ExNotFemale(this);
            if (age<_eng.brideAge())
                throw new ExNotFucker(this);
            if (EventDate != DateTime.MinValue)
                throw new ExAlreadyFucked(this);
            if (Name == 0) throw new ExNoName();
            if (Group > 1) throw new ExNotOne("случить");
            RabNetEngRabbit f = _eng.getRabbit(otherrab);
            if (f.Sex != OneRabbit.RabbitSex.MALE)
                throw new ExNotMale(f);
            if (f.Status < 1)
                throw new ExNotFucker(f);
            if (when > DateTime.Now)
                throw new ExBadDate(when);
            _eng.logs().log(RabNetLogs.LogType.FUCK, RabID, otherrab, SmallAddress, f.SmallAddress);
            _eng.db().makeFuck(this._id, f.RabID, when.Date, _eng.userId);
        }
        /// <summary>
        /// Отметить прохолост (самка не окролилась)
        /// </summary>
        /// <param name="when">Дата установки прохолоста</param>
        public void ProholostIt(DateTime when)
        {
            if (Sex != OneRabbit.RabbitSex.FEMALE)
                throw new ExNotFemale(this);
            if (EventDate == DateTime.MinValue)
                throw new ExNotFucked(this);
            if (when > DateTime.Now)
                throw new ExBadDate(when);
            _eng.logs().log(RabNetLogs.LogType.PROHOLOST, RabID);
            _eng.db().makeProholost(this._id, when);
        }
        /// <summary>
        /// Принять окрол
        /// </summary>
        /// <param name="when">Дата принятия окрола</param>
        /// <param name="children">Количество родившихся живых крольчат</param>
        /// <param name="dead">Количество родившихся мертвых крольчат</param>
        public void OkrolIt(DateTime when, int children, int dead)
        {
            if (Sex != OneRabbit.RabbitSex.FEMALE)
                throw new ExNotFemale(this);
            if (EventDate == DateTime.MinValue)
                throw new ExNotFucked(this);
            if (when > DateTime.Now)
                throw new ExBadDate(when);           
            int born = _eng.db().makeOkrol(this._id, when, children, dead);
            _eng.logs().log(RabNetLogs.LogType.OKROL, RabID, born, SmallAddress, "", String.Format("живых {0:d}, мертвых {1:d}", children, dead));
        }

        public void ReplaceRabbit(int farm,int tier_id,int sec,string address)
        {
            if (_id == 0)
            {
                _rab.address = address;
                _rab.nuaddr = String.Format("{0:d}|{1:d}|{2:d}", farm, tier_id, sec);
            }
            else
            {
                _eng.logs().log(RabNetLogs.LogType.REPLACE, RabID, 0, _rab.smallAddress, address.TrimEnd(' ').Substring(0,address.LastIndexOf(' ')));
                _eng.db().replaceRabbit(RabID, farm, tier_id, sec);
            }
            _rab.tag = "";
        }

        public void ReplaceYounger(int yid, int farm, int tier, int sec, string address)
        {
            _eng.db().replaceYounger(yid, farm, tier, sec);
            foreach (OneRabbit y in Youngers)
                if (y.id == yid)
                    y.tag = "";
            OneRabbit r = _eng.db().GetRabbit(yid);
            _eng.logs().log(RabNetLogs.LogType.REPLACE, yid,0,r.smallAddress,address);
        }

        /// <summary>
        /// Списать кролика
        /// </summary>
        /// <param name="when">Дата списания</param>
        /// <param name="reason">Причина списания</param>
        /// <param name="notes">Заметки по данному списанию</param>
        /// <param name="count">Количество списанных кроликов</param>
        public void KillIt(DateTime when, int reason, string notes,int count)
        {
            if (count == Group)
            {
                _eng.logs().log(RabNetLogs.LogType.RABBIT_KILLED, RabID, 0, SmallAddress == OneRabbit.NullAddress ? CloneAddress : SmallAddress, "", String.Format(" {0:s}[{1:d}] {2:s})",FullName, Group, notes));
                _eng.db().KillRabbit(_id, when, reason, notes);
            }
            else
            {
                int nid = Clone(count, 0, 0, 0);
                RabNetEngRabbit nr = new RabNetEngRabbit(nid, _eng);
                nr.CloneAddress = SmallAddress;
                nr.KillIt(when, reason, notes, count);
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
        /// <param name="yid">ID детей</param>
        public void CountKids(int dead,int killed,int added,int atall,int age,int yid)
        {
            const byte DR_ON_COUNT=5;//deadreason "при подсчете"
            if (Sex != OneRabbit.RabbitSex.FEMALE)
                throw new ExNotFemale(this);
            _eng.logs().log(RabNetLogs.LogType.COUNT_KIDS, RabID, 0, "", "", String.Format("возраст {0:d} всего {1:d} (умерло {2:d}, притоптано {3:d}, прибавилось {4:d})",age,atall,dead,killed,added));            
            if (dead == 0 && killed == 0 && added == 0) return;
            OneRabbit y = _rab.youngers[yid];
            RabNetEngRabbit r = _eng.getRabbit(y.id);        
            if (atall == 0)
            {
                r.CloneAddress = SmallAddress;
                r.KillIt(DateTime.Now, DR_ON_COUNT, "при подсчете", y.group);
            }
            else
            {
                RabNetEngRabbit clone = _eng.getRabbit(r.Clone(dead + killed, 0, 0, 0));
                clone.CloneAddress = SmallAddress;
                clone.KillIt(DateTime.Now, DR_ON_COUNT, "при подсчете", clone.Group);
                //!!!тут надо списывать
                if(added>0)
                    _eng.db().СountKids(_id,dead, killed, added, _rab.youngers[yid].id);             
            }
        }

        /// <summary>
        /// Установить пол
        /// </summary>
        /// <param name="sex">Новый пол</param>
        public void SetSex(OneRabbit.RabbitSex sex)
        {
            _eng.logs().log(RabNetLogs.LogType.SET_SEX, _id, 0, "", "", OneRabbit.SexToRU(sex));
            _eng.db().setRabbitSex(_id, sex);
        }

        /// <summary>
        /// Отделяет несколько кроликов и создает новую группу
        /// </summary>
        /// <param name="count">Сколько кроликов в новой группе</param>
        /// <param name="farm">ID фермы</param>
        /// <param name="tier">Ярус</param>
        /// <param name="sec">Клетка</param>
        /// <returns></returns>
        public int Clone(int count,int farm,int tier,int sec)
        {
           if (Group < count) throw new ExBadCount();
           int nid = _eng.db().cloneRabbit(_id, count, farm, tier, sec, OneRabbit.RabbitSex.VOID, 0);
           RabNetEngRabbit rab = Engine.get().getRabbit(nid);       //+gambit
           _eng.logs().log(RabNetLogs.LogType.CLONE_GROUP, _id, nid, SmallAddress, rab.SmallAddress, String.Format("{0:d} и {1:d}", Group-count ,count));
           return nid;
        }
        /// <summary>
        /// Обьединить с группой
        /// </summary>
        /// <param name="rabto">ID кролика с которым объединить</param>
        public void CombineWidth(int rabto)
        {
            RabNetEngRabbit rab = Engine.get().getRabbit(rabto);    //+gambit
            _eng.logs().log(RabNetLogs.LogType.COMBINE, _id, rabto, SmallAddress, rab.SmallAddress , "+ " + FullName + " [" + Group.ToString() + "]");
            _eng.db().combineGroups(_id, rabto);
        }

        public void PlaceSuckerTo(int mother)
        {
            RabNetEngRabbit mom_to = Engine.get().getRabbit(mother);
            _eng.logs().log(RabNetLogs.LogType.PLACE_SUCK, _id, mother, "", mom_to.SmallAddress);
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
            if (withChildrens)
            {
                foreach (OneRabbit r in _rab.youngers)
                    _eng.db().SetRabbitVaccine(r.id, vid, date);
                _rab.youngers = _eng.db().GetYoungers(_id);
            }
            _rab.rabVacs = _eng.db().GetRabVac(_id);                        
        }
    }
}
