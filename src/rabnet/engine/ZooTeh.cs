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
            type=JobType.OKROL; job=f.safeInt("shr")==0?"Принять окрол":"Окрол";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id; breed = br;
            comment = (f.safeInt("shr")==0?"окрол ":"№") + stat.ToString();
            return this;
        }
        public ZootehJob Vudvor(int id,String nm,String ad,int stat,int age,int srok,int id2,String br,int suckers)
        {
            type = JobType.VUDVOR; job = f.safeInt("shr")==0?"Выдворение":"Выдв";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id; breed = br;
            this.id2=id2;
            comment = (f.safeInt("shr") == 0 ? "подсосных" : "+") + suckers.ToString();
            return this;
        }
        public ZootehJob Counts(int id, String nm, String ad, int age,int count,bool suckers,String br,int srok)
        {
            type = JobType.COUNT_KIDS; job = (f.safeInt("shr") == 0 ? "Подсчет " : "Подсч") + (suckers ? (f.safeInt("shr") == 0 ? "подсосных" : "Подс") : (f.safeInt("shr") == 0 ? "гнездовых" : "Гнез"));
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id; breed = br;
            flag = suckers ? 1 : 0;
            comment = (f.safeInt("shr")==0?"количество ":"+") + count.ToString();
            return this;
        }
        public ZootehJob Preokrol(int id, String nm, String ad, int age, int srok,String br)
        {
            type = JobType.PRE_OKROL; job = f.safeInt("shr")==0?"Предокрольный осмотр":"ПредОсм";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id; breed = br;
            return this;
        }
        public ZootehJob BoysGirlsOut(int id, String nm, String ad, int age, int srok,bool boys,String br)
        {
            type = boys ? JobType.BOYS_OUT : JobType.GIRLS_OUT;
            job = (f.safeInt("shr") == 0 ? "Отсадка " : "Отсад") + (boys ? (f.safeInt("shr") == 0 ? "мальчиков" : "Ма") : (f.safeInt("shr") == 0 ? "девочек" : "Де"));
            this.id = id; days = srok; breed = br;
            name = nm; address = ad;
            this.age = age;
            return this;
        }
        public ZootehJob Fuck(int id, String nm, String ad, int age, int srok,int status,String boys,int group,string breed)
        {
            type = JobType.FUCK; 
            job = status==0?(f.safeInt("shr")==0?"Случка":"Случ"):(f.safeInt("shr")==0?"Вязка":"Вязк");
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
            type = JobType.SET_NEST; job = f.safeInt("shr")==0?"Установка гнездовья":"УстГнезд";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id; breed = br;
            comment = "C-" + sukr.ToString();
            if (children>0) comment+=" " +(f.safeInt("shr")==0?" подсосных":"+")+ children.ToString();
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

        public ZootehJob[] makeZooTehPlan(Filters f,int type)
        {
            JobHolder zjobs = new JobHolder();
            this.f = f;
            if (f.safeValue("act","O").Contains("O") && type==0)
                getOkrols(zjobs);
            if (f.safeValue("act", "V").Contains("V") && type==1)
                getVudvors(zjobs);
            if (f.safeValue("act", "C").Contains("C") && type == 2)
                getCounts(zjobs);
            if (f.safeValue("act", "P").Contains("P") && type == 3)
                getPreokrols(zjobs);
            if (f.safeValue("act", "R").Contains("R") && type == 4)
                getBoysGirlsOut(zjobs);
            if (f.safeValue("act", "F").Contains("F") && type == 5)
                getFucks(zjobs,0);
            if (f.safeValue("act", "f").Contains("f") && type == 6)
                getFucks(zjobs, 1);
            if (f.safeValue("act", "v").Contains("v") && type == 7)
                getVacc(zjobs);
            if (f.safeValue("act", "N").Contains("N") && type == 8)
                getSetNest(zjobs);
            return zjobs.ToArray();
        }

        public void getOkrols(JobHolder jh)
        {
            int days = f.safeInt("okrol");
            ZooJobItem[] jobs=eng.db2().getOkrols(f,days);
            foreach(ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).Okrol(z.id,z.name,z.place,z.status+1,z.age,z.i[0]-days,z.breed));
        }
        public void getVudvors(JobHolder jh)
        {
            int days = f.safeInt("vudvor");
            ZooJobItem[] jobs = eng.db2().getVudvors(f, days);
            foreach (ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).Vudvor(z.id, z.name, z.place, z.status + 1, z.age, z.i[0] - days,z.i[1],z.breed,z.i[2]));
        }
        public void getCounts(JobHolder jh)
        {
            for (int i = 0; i < 4; i++)
            {
                int days = f.safeInt("count" + i.ToString());
                int next = i==3?-1:f.safeInt("count" + (i+1).ToString());
                ZooJobItem[] jobs = eng.db2().getCounts(f, days,next);
                foreach (ZooJobItem z in jobs)
                    jh.Add(new ZootehJob(f).Counts(z.id, z.name, z.place, z.age,z.i[0],i==3,z.breed,z.i[1]));
            }
        }

        public void getPreokrols(JobHolder jh)
        {
            int days = f.safeInt("preok");
            int okroldays = f.safeInt("okrol");
            ZooJobItem[] jobs = eng.db2().getPreokrols(f, days, okroldays);
            foreach(ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).Preokrol(z.id,z.name,z.place,z.age,z.i[0],z.breed));
        }

        public void getBoysGirlsOut(JobHolder jh)
        {
            int days = f.safeInt("boysout");
            ZooJobItem[] jobs = eng.db2().getBoysGirlsOut(f, days, OneRabbit.RabbitSex.MALE);
            foreach (ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).BoysGirlsOut(z.id, z.name, z.place, z.age, z.i[0],true,z.breed));
            days = f.safeInt("girlsout");
            jobs = eng.db2().getBoysGirlsOut(f, days, OneRabbit.RabbitSex.FEMALE);
            foreach (ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).BoysGirlsOut(z.id, z.name, z.place, z.age, z.i[0], false,z.breed));
        }

        public void getFucks(JobHolder jh,int type)
        {
            int days1 = f.safeInt("sfuck");
            int days2 = f.safeInt("ffuck");
            bool heter = f.safeBool("heter");
            bool inbr = f.safeBool("inbr");
            int malewait = f.safeInt("mwait");
            ZooJobItem[] jobs = eng.db2().getZooFuck(f, days1, days2, eng.brideAge(), malewait, heter, inbr, type);
            foreach (ZooJobItem z in jobs)
            {
                jh.Add(new ZootehJob(f).Fuck(z.id, z.name, z.place, z.age, z.i[0], z.status,z.names,z.i[1],z.breed));
            }
        }
        public void getVacc(JobHolder jh)
        {
            int days = f.safeInt("vacc");
            ZooJobItem[] jobs = eng.db2().getVacc(f, days);
            foreach (ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).Vacc(z.id,z.name,z.place,z.age,z.i[0],z.breed));
        }

        public void getSetNest(JobHolder jh)
        {
            int wochild = f.safeInt("nest");
            int wchild = f.safeInt("cnest");
            ZooJobItem[] jobs = eng.db2().getSetNest(f, wochild, wchild);
            foreach (ZooJobItem z in jobs)
                jh.Add(new ZootehJob(f).SetNest(z.id, z.name, z.place, z.age, z.i[0],z.i[1],z.i[2],z.breed));
        }

    }
}
