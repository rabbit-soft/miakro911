using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class Fucks
    {
        public static class Type
        {
            public const string Null = "нет";
            public const string Vyazka_ENG = "vyazka";
            public const string Vyazka_rus = "вязка";
            public const string Sluchka_ENG = "sluchka";
            public const string Sluchka_rus = "случка";
            public const string Syntetic_ENG = "syntetic";
            public const string Syntetic_rus = "искусственное осеменение";
            public const string Kuk_ENG = "kuk";
            public const string Kuk_rus = "кук";
            public const string Okrol_ENG = "okrol";
            public const string Okrol_rus = "окрол";
            public const string Proholost_ENG = "proholost";
            public const string Proholost_rus = "прохолостание";
            public const string Sukrol_ENG = "sukrol";
            public const string Sukrol_rus = "сукрольна";
        }

        public class Fuck
        {
            public String FemaleName;
            public String PartnerName;
            public int PartnerId;
            public int Id;
            public int Times;
            public DateTime When;
            public String FuckType;
            public DateTime EndDate;
            public String Status;
            public int Children;
            public int Dead;
            public int Killed;
            public int Added;
            public int Breed;
            public String rGenom;
            public bool IsDead;
            public string Worker;
            public Fuck(int id, String femaleName,String partnerName, int pid, int tms, DateTime startDate, DateTime endDate, String state, int children, int dead,
                int brd, String gen, String tp, int killed, int added, bool isDead, string worker)
            {
                this.Id = id;
                PartnerName = partnerName;
                PartnerId = pid;
                Worker = worker;
                Times = tms;
                When = startDate; EndDate = endDate;
                FuckType = Type.Null;
                switch (tp)
                {
                    case Type.Vyazka_ENG: FuckType = Type.Vyazka_rus; break;
                    case Type.Sluchka_ENG: FuckType = Type.Sluchka_rus; break;
                    case Type.Syntetic_ENG: FuckType = Type.Syntetic_rus; break;
                    case Type.Kuk_ENG: FuckType = Type.Kuk_rus; break;
                }
                //if (tp == Type.Vyazka_ENG) type = Type.Vyazka_rus;
                //if (tp == Type.Sluchka_ENG) type = Type.Sluchka_rus;
                //if (tp == Type.Kuk_ENG) type = Type.Kuk_rus;
                Status = Type.Sukrol_rus;
                if (state == Type.Okrol_ENG) Status = Type.Okrol_rus;
                if (state == Type.Proholost_ENG) Status = Type.Proholost_rus;
                Children = children; 
                Dead = dead;
                IsDead = isDead;
                Killed = killed;
                Added = added;
                Breed = brd;
                rGenom = gen;
            }
        }
        public List<Fuck> fucks = new List<Fuck>();
        public void AddFuck(int id, String femaleName, String partnerName, int pId, int times, DateTime startDate, DateTime endDate, String state, int childrens, int dd,
            int brd, String gen, String tp, int kl, int add, bool dead, String wrk)
        {
            fucks.Add(new Fuck(id, femaleName,partnerName, pId, times, startDate, endDate, state, childrens, dd, brd, gen, tp, kl, add, dead, wrk));
        }

        public Fuck LastFuck
        {
            get
            {
                Fuck result = null;
                foreach (Fuck f in this.fucks)
                    if (result == null || result.When < f.When)
                        result = f;
                return result;
            }
        }

    }
}
