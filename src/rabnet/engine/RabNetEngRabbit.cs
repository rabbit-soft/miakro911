using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetEngRabbit
    {
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
        public int wasname { get { return rab.wasname; } }
        public String address { get { return rab.address; } }
    }
}
