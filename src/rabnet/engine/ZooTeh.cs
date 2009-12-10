using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public enum JobType {NONE, OKROL };
    
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
        public ZootehJob()
        {
        }
        public ZootehJob(JobType type)
        {
            this.type = type;
        }
        public ZootehJob Okrol(int id,String nm,String ad,int stat,int age,int srok)
        {
            type=JobType.OKROL; job="Принять окрол";
            days = srok; name = nm; address = ad;
            this.age = age; this.id = id;
            comment = "окрол " + stat.ToString();
            return this;
        }
    }

    public class JobHolder:List<ZootehJob>{}
    
    public class RabEngZooTeh
    {
        private RabNetEngine eng;
        public RabEngZooTeh(RabNetEngine eng)
        {
            this.eng = eng;
        }

        public ZootehJob[] makeZooTehPlan()
        {
            JobHolder zjobs = new JobHolder();
            getOkrols(zjobs);
            return zjobs.ToArray();
        }

        public void getOkrols(JobHolder jh)
        {
            int days=eng.options().getIntOption(Options.OPT_ID.OKROL);
            ZooJobItem[] jobs=eng.db().getOkrols(days);
            foreach(ZooJobItem z in jobs)
                jh.Add(new ZootehJob().Okrol(z.id,z.name,z.place,z.status+1,z.age,z.i[0]-days));
        }
    }
}
