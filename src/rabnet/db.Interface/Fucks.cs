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
            public String partner;
            public int partnerid;
            public int id;
            public int times;
            public DateTime when;
            public String type;
            public DateTime enddate;
            public String status;
            public int children;
            public int dead;
            public int killed;
            public int added;
            public int breed;
            public String rgenom;
            public bool isDead;
            public string worker;
            public Fuck(int id, String p, int pid, int tms, DateTime s, DateTime e, String st, int ch, int dd,
                int brd, String gen, String tp, int kl, int add, bool isdead, string wrk)
            {
                this.id = id;
                partner = p;
                partnerid = pid;
                worker = wrk;
                times = tms;
                when = s; enddate = e;
                type = Type.Null;
                switch (tp)
                {
                    case Type.Vyazka_ENG: type = Type.Vyazka_rus; break;
                    case Type.Sluchka_ENG: type = Type.Sluchka_rus; break;
                    case Type.Syntetic_ENG: type = Type.Syntetic_rus; break;
                    case Type.Kuk_ENG: type = Type.Kuk_rus; break;
                }
                //if (tp == Type.Vyazka_ENG) type = Type.Vyazka_rus;
                //if (tp == Type.Sluchka_ENG) type = Type.Sluchka_rus;
                //if (tp == Type.Kuk_ENG) type = Type.Kuk_rus;
                status = Type.Sukrol_rus;
                if (st == Type.Okrol_ENG) status = Type.Okrol_rus;
                if (st == Type.Proholost_ENG) status = Type.Proholost_rus;
                children = ch; 
                dead = dd;
                isDead = isdead;
                killed = kl;
                added = add;
                breed = brd;
                rgenom = gen;
            }
        }
        public List<Fuck> fucks = new List<Fuck>();
        public void AddFuck(int id, String p, int pid, int tms, DateTime s, DateTime e, String st, int ch, int dd,
            int brd, String gen, String tp, int kl, int add, bool dead, String wrk)
        {
            fucks.Add(new Fuck(id, p, pid, tms, s, e, st, ch, dd, brd, gen, tp, kl, add, dead, wrk));
        }

        public Fuck LastFuck
        {
            get
            {
                Fuck result = null;
                foreach (Fuck f in this.fucks)
                    if (result == null || result.when < f.when)
                        result = f;
                return result;
            }
        }

    }
}
