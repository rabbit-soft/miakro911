using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    //public static class FuckType
    //{
    //    public const string Null = "нет";
    //    public const string Vyazka_ENG = "vyazka";
    //    public const string Vyazka_rus = "вязка";
    //    public const string Sluchka_ENG = "sluchka";
    //    public const string Sluchka_rus = "случка";
    //    public const string Syntetic_ENG = "syntetic";
    //    public const string Syntetic_rus = "искусственное осеменение";
    //    public const string Kuk_ENG = "kuk";
    //    public const string Kuk_rus = "кук";
    //    public const string Okrol_ENG = "okrol";
    //    public const string Okrol_rus = "окрол";
    //    public const string Proholost_ENG = "proholost";
    //    public const string Proholost_rus = "прохолостание";
    //    public const string Sukrol_ENG = "sukrol";
    //    public const string Sukrol_rus = "сукрольна";
    //}

    public enum FuckType { None, Sluchka, Vyazka, Kuk, Syntetic }
    public enum FuckEndType { Sukrol,Okrol, Proholost }

    public class Fuck
    {
        public String FemaleName;
        public int FemaleId;
        public String PartnerName;
        public int PartnerId;
        public int Id;
        public int Times;
        public DateTime EventDate;
        public FuckType FType;
        public DateTime EndDate;
        public FuckEndType FEndType;
        public int Children;
        public int Dead;
        public int Killed;
        public int Added;
        public int Breed;
        public String rGenom;
        public bool IsDead;
        public string Worker;
        public Fuck(int id, int femaleId, String femaleName, int partnerId, String partnerName, int tms, DateTime startDate, DateTime endDate, String fEndType, int children, int dead,
            int brd, String gen, String fType, int killed, int added, bool isDead, string worker)
        {
            this.Id = id;
            this.FemaleName = femaleName;
            this.FemaleId = femaleId;
            PartnerName = partnerName;
            PartnerId = partnerId;
            Worker = worker;
            Times = tms;
            EventDate = startDate; EndDate = endDate;
            this.FType = ParceFuckType(fType);
            //this.FType = FuckType.Null;
            //switch (tp)
            //{
            //    case FuckType.Vyazka_ENG: this.FType = FuckType.Vyazka_rus; break;
            //    case FuckType.Sluchka_ENG: this.FType = FuckType.Sluchka_rus; break;
            //    case FuckType.Syntetic_ENG: this.FType = FuckType.Syntetic_rus; break;
            //    case FuckType.Kuk_ENG: this.FType = FuckType.Kuk_rus; break;
            //}
            this.FEndType = ParceFuckEndType(fEndType);
            //FEndType = FuckType.Sukrol_rus;
            //if (fEndType == FuckType.Okrol_ENG) FEndType = FuckType.Okrol_rus;
            //if (fEndType == FuckType.Proholost_ENG) FEndType = FuckType.Proholost_rus;
            Children = children;
            Dead = dead;
            IsDead = isDead;
            Killed = killed;
            Added = added;
            Breed = brd;
            rGenom = gen;
        }

        public static string GetFuckTypeStr(FuckType ft, bool english)
        {
            switch (ft)
            {
                case FuckType.Sluchka: return english ? "sluchka" : "случка";
                case FuckType.Vyazka: return english ? "vyazka" : "вязка";
                case FuckType.Syntetic: return english ? "syntetic" : "искусственное осеменение";
                case FuckType.Kuk: return english ? "kuk" : "кук";
                default: return english ? "none" : "нет";
            }
        }
        public static string GetFuckTypeStr(FuckType ft) { return GetFuckTypeStr(ft, true); }

        public static FuckType ParceFuckType(string ft)
        {
            switch (ft)
            {
                case "случка":
                case "sluchka": return FuckType.Sluchka;

                case "вязка":
                case "vyazka": return FuckType.Vyazka;

                case "искусственное осеменение":
                case "syntetic": return FuckType.Syntetic;

                case "кук":
                case "kuk": return FuckType.Kuk;

                default: return FuckType.None;
            }
        }

        public static string GetFuckEndTypeStr(FuckEndType ft,bool english)
        {
            switch(ft)
            {
                case FuckEndType.Okrol: return english ? "okrol" : "окрол";
                case FuckEndType.Proholost: return english ? "proholost" : "прохолостание";
                default: return english ? "sukrol" : "сукрольна";
            }
        }
        public static string GetFuckEndTypeStr(FuckEndType ft) { return GetFuckEndTypeStr(ft,true); }

        public static FuckEndType ParceFuckEndType(string fet)
        {
            switch (fet)
            {
                case "окрол":
                case "okrol": return FuckEndType.Okrol;

                case "прохолостание":
                case "proholost": return FuckEndType.Proholost;

                default: return FuckEndType.Sukrol;
            }
        }
    }

    public class Fucks:List<Fuck>
    {
        public void AddFuck(int id, int fId, String femaleName, int pId, String partnerName, int times, DateTime startDate, DateTime endDate, String state, int childrens, int dd,
            int brd, String gen, String tp, int kl, int add, bool dead, String wrk)
        {
            this.Add(new Fuck(id, fId, femaleName, pId, partnerName, times, startDate, endDate, state, childrens, dd, brd, gen, tp, kl, add, dead, wrk));
        }

        public Fuck LastFuck
        {
            get
            {
                Fuck result = null;
                foreach (Fuck f in this)
                    if (result == null || result.EventDate < f.EventDate)
                        result = f;
                return result;
            }
        }

    }
}
