using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
   
    /// <summary>
    /// Является List из ZootehJob
    /// </summary>
    public class JobHolder:List<ZootehJob>{}
    
    public class RabEngZooTeh
    {
        private RabNetEngine eng;
        private Filters f = null;
        public RabEngZooTeh(RabNetEngine eng)
        {
            this.eng = eng;
        }

        public ZootehJob[] makeZooTehPlan(Filters f, int type)
        {
            JobHolder zjobs = new JobHolder();
            this.f = f;          
            if (f.safeValue("act", "O").Contains("O") && type == 0)
                zjobs.AddRange(eng.db2().GetZooTechJobs(f,JobType.OKROL));
            if (f.safeValue("act", "V").Contains("V") && type == 1)
                zjobs.AddRange(eng.db2().GetZooTechJobs(f,JobType.VUDVOR));
            if (f.safeValue("act", "C").Contains("C") && type == 2)
                getCounts(zjobs);
            if (f.safeValue("act", "P").Contains("P") && type == 3)
                zjobs.AddRange(eng.db2().GetZooTechJobs(f,JobType.PRE_OKROL));
            if (f.safeValue("act", "R").Contains("R") && type == 4)
                getBoysGirlsOut(zjobs);
            if (f.safeValue("act", "F").Contains("F") && type == 5)
                getFucks(zjobs, 0);
            if (f.safeValue("act", "f").Contains("f") && type == 6)
                getFucks(zjobs, 1);
            if (f.safeValue("act", "v").Contains("v") && type == 7)
                zjobs.AddRange(eng.db2().GetZooTechJobs(f,JobType.VACC));
            if (f.safeValue("act", "N").Contains("N") && type == 8)
                zjobs.AddRange(eng.db2().GetZooTechJobs(f,JobType.SET_NEST));
            if (f.safeValue("act", "B").Contains("B") && type == 9)
                zjobs.AddRange(eng.db2().GetZooTechJobs(f,JobType.BOYS_BY_ONE));
            return zjobs.ToArray();
        }

        private void getCounts(JobHolder jh)
        {
            for (int i = 1; i < 4; i++)
            {
                f["days"] = f.safeInt("count" + i.ToString()).ToString();
                f["next"] = i == 3 ? "-1" : f.safeInt("count" + (i + 1).ToString()).ToString();
                jh.AddRange(eng.db2().GetZooTechJobs(f,JobType.COUNT_KIDS));
            }
        }

        private void getBoysGirlsOut(JobHolder jh)
        {
            jh.AddRange(eng.db2().GetZooTechJobs(f,JobType.BOYS_OUT)); 
            jh.AddRange(eng.db2().GetZooTechJobs(f,JobType.GIRLS_OUT));
        }

        /// <summary>
        /// Добавляет к работам Случки или Вязки
        /// </summary>
        /// <param name="jh">Список работ</param>
        /// <param name="type">0- Случка, 1-Вязка</param>
        private void getFucks(JobHolder jh,int type)
        {
            f[Filters.MAKE_BRIDE] = eng.brideAge().ToString();
            f[Filters.TYPE] = type.ToString();
            jh.AddRange(eng.db2().GetZooTechJobs(f,JobType.FUCK));//ztGetZooFuck(f));
        }
    }
}
