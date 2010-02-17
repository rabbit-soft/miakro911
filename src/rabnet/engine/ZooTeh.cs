using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public enum JobType {NONE, OKROL, VUDVOR, COUNT_KIDS, PRE_OKROL ,BOYS_OUT,GIRLS_OUT,FUCK,VACC,SET_NEST};

    public class ZootehJob:IData
    {
        public JobType type = JobType.OKROL;
//        public String options = "";
        public int days=0;
        public string job="";
        public string address="";
        public string name="";
        public int age = 0;
        public string comment = "";
        public string names="";
        public string addresses="";
        public int id=0;
        public int id2;
        public int flag = 0;
        public string breed = "";
        private Filters f = null;
        public ZootehJob()
        {
        }
        public ZootehJob(Filters f)
        {
            this.f = f;
        }
        public ZootehJob Okrol(int id,String nm,String ad,int stat,int age,int srok,String br)
        {
            type=JobType.OKROL; job=f.safeInt("shr")==0?"Принять окрол":"Окрл";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id; breed = br;
            comment = "окрол " + stat.ToString();
            return this;
        }
        public ZootehJob Vudvor(int id,String nm,String ad,int stat,int age,int srok,int id2,String br)
        {
            type = JobType.VUDVOR; job = f.safeInt("shr")==0?"Выдворение":"Выдв";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id; breed = br;
            this.id2=id2;
            return this;
        }
        public ZootehJob Counts(int id, String nm, String ad, int age,int count,bool suckers,String br)
        {
            type = JobType.COUNT_KIDS; job = (f.safeInt("shr") == 0 ? "Подсчет " : "Сч") + (suckers ? (f.safeInt("shr") == 0 ? "подсосных" : "Пс") : (f.safeInt("shr") == 0 ? "гнездовых" : "Гн"));
            days = 0; name = nm; address = ad;
            this.age = age; this.id = id; breed = br;
            flag = suckers ? 1 : 0;
            comment = (f.safeInt("shr")==0?"количество ":"колво:") + count.ToString();
            return this;
        }
        public ZootehJob Preokrol(int id, String nm, String ad, int age, int srok,String br)
        {
            type = JobType.PRE_OKROL; job = f.safeInt("shr")==0?"Предокрольный осмотр":"ПрОк";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id; breed = br;
            return this;
        }
        public ZootehJob BoysGirlsOut(int id, String nm, String ad, int age, int srok,bool boys,String br)
        {
            type = boys ? JobType.BOYS_OUT : JobType.GIRLS_OUT;
            job = (f.safeInt("shr") == 0 ? "Отсадка " : "От") + (boys ? (f.safeInt("shr") == 0 ? "мальчиков" : "Ма") : (f.safeInt("shr") == 0 ? "девочек" : "Де"));
            this.id = id; days = srok; breed = br;
            name = nm; address = ad;
            this.age = age;
            return this;
        }
        public ZootehJob Fuck(int id, String nm, String ad, int age, int srok,int status,String boys,int group,string breed)
        {
            type = JobType.FUCK; 
            job = status==0?(f.safeInt("shr")==0?"Случка":"Сл"):(f.safeInt("shr")==0?"Вязка":"Вязк");
            this.id = id; 
            days = srok;
            name = nm; address = ad; this.breed = breed;
            this.age = age;
            comment = f.safeInt("shr")==1?"Нвс":"Невеста";
            if (status > 0)
                comment = f.safeInt("shr") == 1 ? "Прк" : "Первокролка";
            if (status > 1)
                comment = f.safeInt("shr") == 1 ? "Штн" : "Штатная";
            //comment = breed +" - "+comment;
            names = boys;
            flag = group;
            return this;
        }
        public ZootehJob Vacc(int id, String nm, String ad, int age, int srok,String br)
        {
            type = JobType.VACC; job = f.safeInt("shr")==0?"Прививка":"Прив";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id; breed = br;
            return this;
        }
        public ZootehJob SetNest(int id, String nm, String ad, int age, int srok,int sukr,int children,String br)
        {
            type = JobType.SET_NEST; job = f.safeInt("shr")==0?"Установка гнездовья":"Гнзд";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id; breed = br;
            comment = "C-" + sukr.ToString();
            if (children>0) comment+=" " + children.ToString() + (f.safeInt("shr")==0?" подсосных":"пдс");
            return this;
        }
    }

    public class JobHolder:List<ZootehJob>{}
    
    public class RabEngZooTeh
    {
        private RabNetEngine eng;
        private Filters f = null;
        public RabEngZooTeh(RabNetEngine eng)
        {
            this.eng = eng;
        }

        public ZootehJob[] makeZooTehPlan(Filters f)
        {
            JobHolder zjobs = new JobHolder();
            f["shr"] = eng.options().getOption(Options.OPT_ID.SHORT_NAMES);
            f["dbl"] = eng.options().getOption(Options.OPT_ID.DBL_SURNAME);
            f["prt"] = eng.options().getOption(Options.OPT_ID.FIND_PARTNERS);
            this.f = f;
            if (f.safeValue("act","O").Contains("O"))
                getOkrols(zjobs);
            if (f.safeValue("act", "V").Contains("V"))
                getVudvors(zjobs);
            if (f.safeValue("act", "C").Contains("C"))
                getCounts(zjobs);
            if (f.safeValue("act", "P").Contains("P"))
                getPreokrols(zjobs);
            if (f.safeValue("act", "R").Contains("R"))
                getBoysGirlsOut(zjobs);
            if (f.safeValue("act", "F").Contains("F"))
                getFucks(zjobs,0);
            if (f.safeValue("act", "f").Contains("f"))
                getFucks(zjobs, 1);
            if (f.safeValue("act", "v").Contains("v"))
                getVacc(zjobs);
            if (f.safeValue("act", "N").Contains("N"))
                getSetNest(zjobs);
            return zjobs.ToArray();
        }

        public void getOkrols(JobHolder jh)
        {
            int days=eng.options().getIntOption(Options.OPT_ID.OKROL);
            ZooJobItem[] jobs=eng.db().getOkrols(f,days);
            foreach(ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).Okrol(z.id,z.name,z.place,z.status+1,z.age,z.i[0]-days,z.breed));
        }
        public void getVudvors(JobHolder jh)
        {
            int days = eng.options().getIntOption(Options.OPT_ID.VUDVOR);
            ZooJobItem[] jobs = eng.db().getVudvors(f, days);
            foreach (ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).Vudvor(z.id, z.name, z.place, z.status + 1, z.age, z.i[0] - days,z.i[1],z.breed));
        }
        public void getCounts(JobHolder jh)
        {
            for (int i = 0; i < 4; i++)
            {
                Options.OPT_ID cnt = Options.OPT_ID.COUNT1;
                if (i == 1) cnt = Options.OPT_ID.COUNT2;
                if (i == 2) cnt = Options.OPT_ID.COUNT3;
                if (i == 3) cnt = Options.OPT_ID.SUCKERS;
                int days = eng.options().getIntOption(cnt);
                ZooJobItem[] jobs = eng.db().getCounts(f, days);
                foreach (ZooJobItem z in jobs)
                    jh.Add(new ZootehJob(f).Counts(z.id, z.name, z.place, z.age,z.i[0],i==3,z.breed));
            }
        }

        public void getPreokrols(JobHolder jh)
        {
            int days = eng.options().getIntOption(Options.OPT_ID.PRE_OKROL);
            int okroldays = eng.options().getIntOption(Options.OPT_ID.OKROL);
            ZooJobItem[] jobs = eng.db().getPreokrols(f, days, okroldays);
            foreach(ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).Preokrol(z.id,z.name,z.place,z.age,z.i[0],z.breed));
        }

        public void getBoysGirlsOut(JobHolder jh)
        {
            int days = eng.options().getIntOption(Options.OPT_ID.BOYS_OUT);
            ZooJobItem[] jobs = eng.db().getBoysGirlsOut(f, days, OneRabbit.RabbitSex.MALE);
            foreach (ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).BoysGirlsOut(z.id, z.name, z.place, z.age, z.i[0],true,z.breed));
            days = eng.options().getIntOption(Options.OPT_ID.GIRLS_OUT);
            jobs = eng.db().getBoysGirlsOut(f, days, OneRabbit.RabbitSex.FEMALE);
            foreach (ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).BoysGirlsOut(z.id, z.name, z.place, z.age, z.i[0], false,z.breed));
        }

        public void getFucks(JobHolder jh,int type)
        {
            int days1 = eng.options().getIntOption(Options.OPT_ID.STATE_FUCK);
            int days2 = eng.options().getIntOption(Options.OPT_ID.FIRST_FUCK);
            bool heter = eng.options().getIntOption(Options.OPT_ID.GETEROSIS) == 1;
            bool inbr = eng.options().getIntOption(Options.OPT_ID.INBREEDING) == 1;
            int malewait = eng.options().getIntOption(Options.OPT_ID.MALE_WAIT);
            ZooJobItem[] jobs = eng.db().getZooFuck(f, days1, days2, eng.brideAge(), malewait, heter, inbr, type);
            foreach (ZooJobItem z in jobs)
            {
                jh.Add(new ZootehJob(f).Fuck(z.id, z.name, z.place, z.age, z.i[0], z.status,z.names,z.i[1],z.breed));
            }
        }
        public void getVacc(JobHolder jh)
        {
            int days = eng.options().getIntOption(Options.OPT_ID.VACC);
            ZooJobItem[] jobs = eng.db().getVacc(f, days);
            foreach (ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).Vacc(z.id,z.name,z.place,z.age,z.i[0],z.breed));
        }

        public void getSetNest(JobHolder jh)
        {
            int wochild = eng.options().getIntOption(Options.OPT_ID.NEST);
            int wchild = eng.options().getIntOption(Options.OPT_ID.CHILD_NEST);
            ZooJobItem[] jobs = eng.db().getSetNest(f, wochild, wchild);
            foreach (ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).SetNest(z.id, z.name, z.place, z.age, z.i[0],z.i[1],z.i[2],z.breed));
        }

    }
}
