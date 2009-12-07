using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetEngRabbit
    {
        public class ExNotFemale : ApplicationException
        {
            public ExNotFemale(RabNetEngRabbit r):base("Кролик "+r.fullName+" не является самкой"){}
        }
        public class ExNotMale : ApplicationException
        {
            public ExNotMale(RabNetEngRabbit r) : base("Кролик " + r.fullName + " не является самцом") { }
        }
        public class ExNotFucker : ApplicationException
        {
            public ExNotFucker(RabNetEngRabbit r) : base("Кролик " + r.fullName + " не является половозрелым") { }
        }
        public class ExBadDate : ApplicationException
        {
            public ExBadDate(DateTime dt) : base("Дата " + dt.ToShortDateString() + " в будущем") { }
        }
        public class ExAlreadyFucked:ApplicationException
        {
            public ExAlreadyFucked(RabNetEngRabbit r):base("Крольчиха "+r.fullName+" уже сукрольна"){}
        }
        public class ExNotFucked : ApplicationException
        {
            public ExNotFucked(RabNetEngRabbit r) : base("Крольчиха " + r.fullName + " не сукрольна") { }
        }

        private int id;
        private OneRabbit rab = null;
        private RabNetEngine eng=null;
        public int mom = 0;
        public RabNetEngRabbit(int rid,RabNetEngine dl)
        {
            id = rid;
            eng = dl;
            rab = eng.db().getRabbit(rid);
        }
        public RabNetEngRabbit(RabNetEngine dl,OneRabbit.RabbitSex sx)
        {
            id = 0;
            eng = dl;
            String s="void";
            if (sx==OneRabbit.RabbitSex.FEMALE) s="female";
            if (sx==OneRabbit.RabbitSex.MALE) s="male";
            rab = new OneRabbit(0, s, DateTime.Now, 0, "00000", 0, 0, 0, "", 1, 1, 0, "", "", 0, DateTime.MinValue, "", DateTime.MinValue, 0, 0, "", "", "00000");
            rab.youngers=new OneRabbit[0];
        }
        public void newCommit()
        {
            if (id != 0)
                return;
            id = eng.db().newRabbit(rab, mom);
            rab.id = id;
            eng.logs().log(RabNetLogs.LogType.INCOME, id);
        }
        public void commit()
        {
            if (rid == 0)
                return;
            if (rab.wasname != rab.name)
                eng.logs().log(RabNetLogs.LogType.RENAME,rid,0,"","",eng.db().makeName(rab.wasname,0,0,1,rab.sex));
            else
                eng.logs().log(RabNetLogs.LogType.RAB_CHANGE, rid);
            eng.db().setRabbit(rab);
            rab=eng.db().getRabbit(id);
        }
        public OneRabbit.RabbitSex sex
        {
            get { return rab.sex; }
            set { rab.sex=value; }
        }
        public DateTime born
        {
            get { return rab.born; }
            set { rab.born = value; }
        }
        public int rate
        {
            get { return rab.rate; }
            set { rab.rate = value; }
        }
        public bool defect
        {
            get { return rab.defect; }
            set { rab.defect = value; }
        }
        public bool production
        {
            get { return rab.gp; }
            set { rab.gp = value; }
        }
        public bool realization
        {
            get { return rab.gr; }
            set { rab.gr = value; }
        }
        public bool spec
        {
            get { return rab.spec; }
            set { rab.spec = value; }
        }
        public int name
        {
            get { return rab.name; }
            set { rab.name = value; }
        }
        public int surname
        {
            get { return rab.surname; }
            set { rab.surname = value; }
        }
        public int secname
        {
            get { return rab.secname; }
            set { rab.secname = value; }
        }
        public int group
        {
            get { return rab.group; }
            set { rab.group = value; }
        }
        public int breed
        {
            get { return rab.breed;}
            set {rab.breed=value;}
        }
        public int zone
        {
            get { return rab.zone; }
            set { rab.zone = value; }
        }
        public String notes
        {
            get { return rab.notes; }
            set { rab.notes = value; }
        }
        public int status
        {
            get { return rab.status; }
            set { rab.status = value; }
        }
        public DateTime last_fuck_okrol
        {
            get { return rab.lastfuckokrol; }
            set { rab.lastfuckokrol = value; }
        }
        public String genom
        {
            get { return rab.gens; }
            set { rab.gens = value; }
        }
        public bool nolact
        {
            get { return rab.nolact; }
            set { rab.nolact = value; }
        }
        public bool nokuk
        {
            get { return rab.nokuk; }
            set { rab.nokuk = value; }
        }
        public int babies
        {
            get { return rab.babies; }
            set { rab.babies = value; }
        }
        public int lost
        {
            get { return rab.lost; }
            set { rab.lost = value; }
        }
        public string tag
        {
            get { return rab.tag; }
            set { rab.tag = value; }
        }
        public int rid { get { return rab.id; } }
        public int evtype{get { return rab.evtype; }}
        public DateTime evdate { get { return rab.evdate; } }
        public int wasname { get { return rab.wasname; } }
        public String address { get { return rab.address; } }
        public String newAddress { get { return rab.nuaddr; } }
        public String fullName
        {
            get
            {
            if (id == 0)
                return eng.db().makeName(name, surname, secname, group, sex);
            return rab.fullname; } }
        public String breedName { get { return rab.breedname; } }
        public String bon { get { return rab.bon; } }
        public int age { get { return (DateTime.Now.Date - born.Date).Days; } }
        public int youngcount { get {
            int c = 0;
            foreach (OneRabbit r in youngers)
                c += r.group;
            return c;
        } }
        public OneRabbit[] youngers { get { return rab.youngers; } }
        public void setBon(String bon)
        {
            if (rid == 0)
                rab.bon = bon;
            else
            {
                eng.logs().log(RabNetLogs.LogType.BON, rid, 0, "", "", bon);
                eng.db().setBon(id, bon);
            }
        }
        public void FuckIt(int otherrab,DateTime when)
        {
            if (sex != OneRabbit.RabbitSex.FEMALE)
                throw new ExNotFemale(this);
            if (age<30)
                throw new ExNotFucker(this);
            if (evdate != DateTime.MinValue)
                throw new ExAlreadyFucked(this);
            RabNetEngRabbit f = eng.getRabbit(otherrab);
            if (f.sex != OneRabbit.RabbitSex.MALE)
                throw new ExNotMale(f);
            if (f.status < 1)
                throw new ExNotFucker(f);
            if (when > DateTime.Now)
                throw new ExBadDate(when);
            eng.logs().log(RabNetLogs.LogType.FUCK, rid, otherrab, "", "");
            eng.db().makeFuck(this.id, f.rid, when.Date);
        }
        public void ProholostIt(DateTime when)
        {
            if (sex != OneRabbit.RabbitSex.FEMALE)
                throw new ExNotFemale(this);
            if (evdate == DateTime.MinValue)
                throw new ExNotFucked(this);
            if (when > DateTime.Now)
                throw new ExBadDate(when);
            eng.logs().log(RabNetLogs.LogType.PROHOLOST, rid);
            eng.db().makeProholost(this.id, when);
        }
        public void OkrolIt(DateTime when, int children, int dead)
        {
            if (sex != OneRabbit.RabbitSex.FEMALE)
                throw new ExNotFemale(this);
            if (evdate == DateTime.MinValue)
                throw new ExNotFucked(this);
            if (when > DateTime.Now)
                throw new ExBadDate(when);
            eng.logs().log(RabNetLogs.LogType.OKROL, rid,0,"","",String.Format("живых {0:d}, мертвых {1:d}",children,dead));
            eng.db().makeOkrol(this.id, when, children, dead);
        }

        public void replaceRabbit(int farm,int tier_id,int sec,string address)
        {
            if (rid == 0)
            {
                rab.address = address;
                rab.nuaddr = String.Format("{0:d}|{1:d}|{2:d}", farm, tier_id, sec);
            }
            else
            {
                eng.logs().log(RabNetLogs.LogType.REPLACE, rid, rab.address);
                eng.db().replaceRabbit(rid, farm, tier_id, sec);
            }
            rab.tag = "";
        }
        public void ReplaceYounger(int yid, int farm, int tier, int sec)
        {
            eng.db().replaceYounger(yid, farm, tier, sec);
            foreach (OneRabbit y in youngers)
                if (y.id == yid)
                    y.tag = "";
        }
    }
}
