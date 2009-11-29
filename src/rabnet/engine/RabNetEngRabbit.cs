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
        public RabNetEngRabbit(int rid,RabNetEngine dl)
        {
            id = rid;
            eng = dl;
            rab = eng.db().getRabbit(rid);
        }
        public void commit()
        {
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
        public int rid { get { return rab.id; } }
        public int evtype{get { return rab.evtype; }}
        public DateTime evdate { get { return rab.evdate; } }
        public int wasname { get { return rab.wasname; } }
        public String address { get { return rab.address; } }
        public String fullName { get { return rab.fullname; } }
        public String breedName { get { return rab.breedname; } }
        public String bon { get { return rab.bon; } }
        public int age { get { return (DateTime.Now.Date - born.Date).Days; } }
        public void setBon(String bon)
        {
            eng.db().setBon(id, bon);
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
            eng.db().makeOkrol(this.id, when, children, dead);
        }
    }
}
