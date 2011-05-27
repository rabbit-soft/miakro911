using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetEngRabbit
    {
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

        private int id;
        private OneRabbit rab = null;
        private RabNetEngine eng = null;
        public int mom = 0;

        public RabNetEngRabbit(int rid,RabNetEngine dl)
        {
            id = rid;
            eng = dl;
            rab = eng.db().getRabbit(rid);
            if (rab == null)
                throw new ExNoRabbit();
        }

        public RabNetEngRabbit(RabNetEngine dl,OneRabbit.RabbitSex sx)
        {
            id = 0;
            eng = dl;
            String s="void";
            if (sx == OneRabbit.RabbitSex.FEMALE) s = "female";
            if (sx == OneRabbit.RabbitSex.MALE) s = "male";
            rab = new OneRabbit(0, s, DateTime.Now, 0, "00000", 0, 0, 0, "", 1, 1, 0, "", "", 0, DateTime.MinValue, "", DateTime.MinValue, 0, 0, "", "", "00000",0,0,DateTime.Now);
            rab.youngers=new OneRabbit[0];
        }

        public void newCommit()
        {
            if (id != 0) return;
            id = eng.db().newRabbit(rab, mom);
            rab.id = id;
            eng.logs().log(RabNetLogs.LogType.INCOME, id);
        }

        public void commit()
        {
            if (RabID == 0)
                return;
            if (rab.wasname != rab.name)
            {
                if (Group > 1)
                    throw new ExNotOne("переименовать");
                eng.logs().log(RabNetLogs.LogType.RENAME, RabID, 0, "", "", eng.db().makeName(rab.wasname, 0, 0, 1, rab.sex));
            }
            else eng.logs().log(RabNetLogs.LogType.RAB_CHANGE, RabID);
            rab.vac_end.AddDays(Engine.opt().getIntOption(Options.OPT_ID.VACCINE_TIME));
            eng.db().setRabbit(rab);
            rab=eng.db().getRabbit(id);
        }

        public OneRabbit.RabbitSex Sex
        {
            get { return rab.sex; }
            set { rab.sex=value; }
        }
        public DateTime Born
        {
            get { return rab.born; }
            set { rab.born = value; }
        }
        public int Rate
        {
            get { return rab.rate; }
            set { rab.rate = value; }
        }
        public bool Defect
        {
            get { return rab.defect; }
            set { rab.defect = value; }
        }
        public bool Production
        {
            get { return rab.gp; }
            set { rab.gp = value; }
        }
        public bool Realization
        {
            get { return rab.gr; }
            set { rab.gr = value; }
        }
        public bool Spec
        {
            get { return rab.spec; }
            set { rab.spec = value; }
        }
        public int Name
        {
            get { return rab.name; }
            set { rab.name = value; }
        }
        /// <summary>
        /// Фамили по отцу
        /// </summary>
        public int Surname
        {
            get { return rab.surname; }
            set { rab.surname = value; }
        }
        /// <summary>
        /// Фамилия по матери
        /// </summary>
        public int SecondName
        {
            get { return rab.secname; }
            set { rab.secname = value; }
        }
        /// <summary>
        /// Количество кроликов в группе
        /// </summary>
        public int Group
        {
            get { return rab.group; }
            set { rab.group = value; }
        }
        /// <summary>
        /// Порода кролика
        /// </summary>
        public int Breed
        {
            get { return rab.breed;}
            set {rab.breed=value;}
        }
        public int Zone
        {
            get { return rab.zone; }
            set { rab.zone = value; }
        }
        /// <summary>
        /// Заметки
        /// </summary>
        public String Notes
        {
            get { return rab.notes; }
            set { rab.notes = value; }
        }
        public int Status
        {
            get { return rab.status; }
            set { rab.status = value; }
        }
        public DateTime Last_Fuck_Okrol
        {
            get { return rab.lastfuckokrol; }
            set { rab.lastfuckokrol = value; }
        }
        /// <summary>
        /// Дата окончания действия прививки
        /// </summary>
        public DateTime VaccineEnd
        {
            get { return rab.vac_end; }
            set { rab.vac_end = value; }
        }
        public String Genom
        {
            get { return rab.gens; }
            set { rab.gens = value; }
        }
        public bool NoLact
        {
            get { return rab.nolact; }
            set { rab.nolact = value; }
        }
        public bool NoKuk
        {
            get { return rab.nokuk; }
            set { rab.nokuk = value; }
        }
        public int Babies
        {
            get { return rab.babies; }
            set { rab.babies = value; }
        }
        public int Lost
        {
            get { return rab.lost; }
            set { rab.lost = value; }
        }
        public string Tag
        {
            get { return rab.tag; }
            set { rab.tag = value; }
        }
        public string SmallAddress{get{return rab.smallAddress;}}
        public string JustAddress{get{return rab.justAddress;}}
        public string TierType
        {
            get
            {
                string[] values = rab.justAddress.Split(',');
                return values[3];
            }
        }

        public bool SetNest
        { 
            get 
            {
                string[] values = rab.justAddress.Split(',');
                if (values.Length < 3)
                    return false;
                if (values[3] == myBuildingType.Jurta && values[2] == "0" && values[5] == "1")
                    return true;
                if (values[3] == myBuildingType.Female && values[5] == "1")
                    return true;
                if (values[3] == myBuildingType.DualFemale && ((values[2] == "0" && values[5][0] == '1') || (values[2] == "1" && values[5][1] == '1')))
                    return true;
                return false;
            } 
        }
        public string medAddress { get { return rab.medAddress; } }
        public int Parent { get { return rab.parent; } }
        public int RabID { get { return rab.id; } }
        public int EventType{get { return rab.evtype; }}
        public DateTime EventDate { get { return rab.evdate; } }
        public int WasName { get { return rab.wasname; } }
        public String Address { get { return rab.address; } }
        public String NewAddress { get { return rab.nuaddr; } }
        /// <summary>
        /// Полное имя (Азалия Гамбитова-Явина,0)
        /// </summary>
        public String FullName
        {
            get
            {
                if (id == 0) return eng.db().makeName(Name, Surname, SecondName, Group, Sex);
                return rab.fullname; 
            } 
        }
        /// <summary>
        /// Порода
        /// </summary>
        public String BreedName { get { return rab.breedname; } }
        /// <summary>
        /// Бонитировка
        /// </summary>
        public String Bon { get { return rab.bon; } }
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
        public OneRabbit[] Youngers { get { return rab.youngers; } }
        public void setBon(String bon)
        {
            if (RabID == 0)
                rab.bon = bon;
            else
            {
                eng.logs().log(RabNetLogs.LogType.BON, RabID, 0, "", "", bon);
                eng.db().setBon(id, bon);
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
            if (age<eng.brideAge())
                throw new ExNotFucker(this);
            if (EventDate != DateTime.MinValue)
                throw new ExAlreadyFucked(this);
            if (Name == 0) throw new ExNoName();
            if (Group > 1) throw new ExNotOne("случить");
            RabNetEngRabbit f = eng.getRabbit(otherrab);
            if (f.Sex != OneRabbit.RabbitSex.MALE)
                throw new ExNotMale(f);
            if (f.Status < 1)
                throw new ExNotFucker(f);
            if (when > DateTime.Now)
                throw new ExBadDate(when);
            eng.logs().log(RabNetLogs.LogType.FUCK, RabID, otherrab, SmallAddress, f.SmallAddress);
            eng.db().makeFuck(this.id, f.RabID, when.Date, eng.userId);
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
            eng.logs().log(RabNetLogs.LogType.PROHOLOST, RabID);
            eng.db().makeProholost(this.id, when);
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
            int born = eng.db().makeOkrol(this.id, when, children, dead);
            eng.logs().log(RabNetLogs.LogType.OKROL, RabID, born, SmallAddress, "", String.Format("живых {0:d}, мертвых {1:d}", children, dead));
        }

        public void replaceRabbit(int farm,int tier_id,int sec,string address)
        {
            if (id == 0)
            {
                rab.address = address;
                rab.nuaddr = String.Format("{0:d}|{1:d}|{2:d}", farm, tier_id, sec);
            }
            else
            {
                eng.logs().log(RabNetLogs.LogType.REPLACE, RabID, 0, rab.smallAddress,address.Substring(0,5));
                eng.db().replaceRabbit(RabID, farm, tier_id, sec);
            }
            rab.tag = "";
        }
        public void ReplaceYounger(int yid, int farm, int tier, int sec, string address)
        {
            eng.db().replaceYounger(yid, farm, tier, sec);
            foreach (OneRabbit y in Youngers)
                if (y.id == yid)
                    y.tag = "";
            OneRabbit r = eng.db().getRabbit(yid);
            eng.logs().log(RabNetLogs.LogType.REPLACE, yid,0,r.smallAddress,address);
        }
        /// <summary>
        /// Списать кролика
        /// </summary>
        /// <param name="when">Дата списания</param>
        /// <param name="reason">Причина списания</param>
        /// <param name="notes">Заметки по данному списанию</param>
        /// <param name="count">Количество списанных кроликов</param>
        public void killIt(DateTime when, int reason, string notes,int count)
        {
            if (count == Group)
            {
                eng.logs().log(RabNetLogs.LogType.RABBIT_KILLED, RabID, 0, SmallAddress, "", FullName+String.Format(" ({0:d})",Group));
                eng.db().killRabbit(id, when, reason, notes);
            }
            else
            {
                int nid = clone(count, 0, 0, 0);
                RabNetEngRabbit nr = new RabNetEngRabbit(nid, eng);
                nr.killIt(when, reason, notes, count);
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
            if (Sex != OneRabbit.RabbitSex.FEMALE)
                throw new ExNotFemale(this);
            eng.logs().log(RabNetLogs.LogType.COUNT_KIDS, RabID, 0, "", "", String.Format("возраст {0:d} всего {1:d} (умерло {2:d}, притоптано {3:d}, прибавилось {4:d})",age,atall,dead,killed,added));            
            if (dead == 0 && killed == 0 && added == 0) return;
            OneRabbit y = rab.youngers[yid];
            RabNetEngRabbit r = eng.getRabbit(y.id);
            if (atall == 0)
            {
                r.killIt(DateTime.Now, 6, "при подсчете", y.group);
            }
            else
            {
                RabNetEngRabbit clone = eng.getRabbit(r.clone(dead + killed, 0, 0, 0));
                clone.killIt(DateTime.Now, 6, "при подсчете", clone.Group);
                //!!!тут надо списыватьт
                if(added>0)
                    eng.db().countKids(id,dead, killed, added, rab.youngers[yid].id);             
            }
        }

        /// <summary>
        /// Установить пол
        /// </summary>
        /// <param name="sex">Новый пол</param>
        public void setSex(OneRabbit.RabbitSex sex)
        {
            eng.logs().log(RabNetLogs.LogType.SET_SEX, id, 0, "", "", OneRabbit.SexToRU(sex));
            eng.db().setRabbitSex(id, sex);
        }

        /// <summary>
        /// Отделяет несколько кроликов и создает новую группу
        /// </summary>
        /// <param name="count">Сколько кроликов в новой группе</param>
        /// <param name="farm">ID фермы</param>
        /// <param name="tier">Ярус</param>
        /// <param name="sec">Клетка</param>
        /// <returns></returns>
        public int clone(int count,int farm,int tier,int sec)
        {
           if (Group < count) throw new ExBadCount();
           int nid = eng.db().cloneRabbit(id, count, farm, tier, sec, OneRabbit.RabbitSex.VOID, 0);
           RabNetEngRabbit rab = Engine.get().getRabbit(nid);       //+gambit
           eng.logs().log(RabNetLogs.LogType.CLONE_GROUP, id, nid, SmallAddress, rab.SmallAddress, String.Format("{0:d} и {1:d}", Group-count ,count));
           return nid;
        }
        /// <summary>
        /// Обьединить с группой
        /// </summary>
        /// <param name="rabto">ID кролика с которым объединить</param>
        public void combineWidth(int rabto)
        {
            RabNetEngRabbit rab = Engine.get().getRabbit(rabto);    //+gambit
            eng.logs().log(RabNetLogs.LogType.COMBINE, id, rabto, SmallAddress, rab.SmallAddress , "+ " + FullName + " [" + Group.ToString() + "]");
            eng.db().combineGroups(id, rabto);
        }

        public void placeSuckerTo(int mother)
        {
            RabNetEngRabbit mom_to = Engine.get().getRabbit(mother);
            eng.logs().log(RabNetLogs.LogType.PLACE_SUCK, id, mother, "", mom_to.SmallAddress);
            eng.db().placeSucker(id, mother);
        }

    }
}
