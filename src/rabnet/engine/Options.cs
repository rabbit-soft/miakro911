using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public enum RUBOOL { Да, Нет };
    public enum BuchTp { Программа, Весы }

    public class Options
    {
        public enum OPT_ID
        {
            NONE, GETEROSIS, INBREEDING, SHORT_NAMES, DBL_SURNAME, SHOW_TIER_TYPE, SHOW_TIER_SEC, RAB_FILTER,
            SHOW_NUMBERS, BUILD_FILTER, OKROL, VUDVOR, COUNT1, COUNT2, COUNT3, MAKE_BRIDE, DEAD_FILTER, PRE_OKROL, 
            ZOO_FILTER, COMBINE_AGE, BOYS_OUT, GIRLS_OUT, STATE_FUCK, FIRST_FUCK, MALE_WAIT, GEN_TREE, CONFIRM_EXIT, 
            VACC, CONFIRM_KILL,CONFIRM_REPLACE, SUCKERS,NEST,CHILD_NEST,RAB_LIST,BUILD_LIST,YOUNG_LIST,DEAD_LIST,REPL_YOUNG_LIST,
            MAKE_FUCK_LIST,NAMES_LIST,UPDATE_ZOO,ZOO_LIST,FIND_PARTNERS,NEXT_SVID,SVID_HEAD,SVID_GEN_DIR,
            KILL_LIST,SHORT_ZOO,VACCINE_TIME,MAKE_CANDIDATE,BUTCHER_LIST,XLS_ASK,XLS_FOLDER,BUILD_FILL_ZERO,BUCHER_TYPE
        }
        public enum OPT_LEVEL {FARM,USER};
        public class ExOptionNotFound:ApplicationException
        {
            public ExOptionNotFound(OPT_ID id) : base("Option " + id.ToString() + " not found int option list") { }
        }   
        struct Option
        {
            public OPT_ID id;
            public OPT_LEVEL level;
            public String name;
            public Option(OPT_ID id, OPT_LEVEL level, String name)
            {
                this.id = id;
                this.level = level;
                this.name = name;
            }
        };
        private static String defNameSpace="opt";
        private RabNetEngine eng;
        private Option[] optlist = {new Option(OPT_ID.GETEROSIS,OPT_LEVEL.FARM,"heterosis"), 
                                   new Option(OPT_ID.INBREEDING,OPT_LEVEL.FARM,"inbreeding"), 
                                   new Option(OPT_ID.SHORT_NAMES,OPT_LEVEL.USER,"short_names"), 
                                   new Option(OPT_ID.DBL_SURNAME,OPT_LEVEL.USER,"dbl_surname"), 
                                   new Option(OPT_ID.SHOW_TIER_TYPE,OPT_LEVEL.USER,"sh_tier_t"), 
                                   new Option(OPT_ID.SHOW_TIER_SEC,OPT_LEVEL.USER,"sh_tier_s"), 
                                   new Option(OPT_ID.RAB_FILTER,OPT_LEVEL.USER,"rab_filter"), 
                                   new Option(OPT_ID.SHOW_NUMBERS,OPT_LEVEL.USER,"sh_num"),
                                   new Option(OPT_ID.BUILD_FILTER,OPT_LEVEL.USER,"build_filter"),
                                   new Option(OPT_ID.OKROL,OPT_LEVEL.FARM,"okrol"), 
                                   new Option(OPT_ID.VUDVOR,OPT_LEVEL.FARM,"vudvor"), 
                                   new Option(OPT_ID.COUNT1,OPT_LEVEL.FARM,"count1"), 
                                   new Option(OPT_ID.COUNT2,OPT_LEVEL.FARM,"count2"), 
                                   new Option(OPT_ID.COUNT3,OPT_LEVEL.FARM,"count3"), 
                                   new Option(OPT_ID.MAKE_BRIDE,OPT_LEVEL.FARM,"bride"), 
                                   new Option(OPT_ID.DEAD_FILTER,OPT_LEVEL.USER,"dead_filter"), 
                                   new Option(OPT_ID.PRE_OKROL,OPT_LEVEL.FARM,"preokrol"), 
                                   new Option(OPT_ID.ZOO_FILTER,OPT_LEVEL.USER,"zoo_filter"), 
                                   new Option(OPT_ID.COMBINE_AGE,OPT_LEVEL.FARM,"combage"), 
                                   new Option(OPT_ID.BOYS_OUT,OPT_LEVEL.FARM,"boysout"), 
                                   new Option(OPT_ID.GIRLS_OUT,OPT_LEVEL.FARM,"girlsout"), 
                                   new Option(OPT_ID.STATE_FUCK,OPT_LEVEL.FARM,"statefuck"), 
                                   new Option(OPT_ID.FIRST_FUCK,OPT_LEVEL.FARM,"firstfuck"), 
                                   new Option(OPT_ID.MALE_WAIT,OPT_LEVEL.FARM,"malewait"), 
                                   new Option(OPT_ID.GEN_TREE,OPT_LEVEL.USER,"gentree"), 
                                   new Option(OPT_ID.CONFIRM_EXIT,OPT_LEVEL.USER,"confirmexit"), 
                                   new Option(OPT_ID.VACC,OPT_LEVEL.FARM,"vacc"), 
                                   new Option(OPT_ID.CONFIRM_KILL,OPT_LEVEL.USER,"confirmkill"), 
                                   new Option(OPT_ID.CONFIRM_REPLACE,OPT_LEVEL.USER,"confirmreplace"),
                                   new Option(OPT_ID.SUCKERS,OPT_LEVEL.FARM,"suckers"), 
                                   new Option(OPT_ID.NEST,OPT_LEVEL.FARM,"nest"), 
                                   new Option(OPT_ID.CHILD_NEST,OPT_LEVEL.FARM,"childnest"), 
                                   new Option(OPT_ID.RAB_LIST,OPT_LEVEL.USER,"rablist"), 
                                   new Option(OPT_ID.BUILD_LIST,OPT_LEVEL.USER,"buildlist"), 
                                   new Option(OPT_ID.YOUNG_LIST,OPT_LEVEL.USER,"younglist"), 
                                   new Option(OPT_ID.DEAD_LIST,OPT_LEVEL.USER,"deadlist"), 
                                   new Option(OPT_ID.REPL_YOUNG_LIST,OPT_LEVEL.USER,"ryounglist"), 
                                   new Option(OPT_ID.MAKE_FUCK_LIST,OPT_LEVEL.USER,"mfucklist"), 
                                   new Option(OPT_ID.NAMES_LIST,OPT_LEVEL.USER,"namelist"), 
                                   new Option(OPT_ID.UPDATE_ZOO,OPT_LEVEL.USER,"updatezoo"), 
                                   new Option(OPT_ID.ZOO_LIST,OPT_LEVEL.USER,"zoolist"), 
                                   new Option(OPT_ID.FIND_PARTNERS,OPT_LEVEL.USER,"findpartner"), 
                                   new Option(OPT_ID.NEXT_SVID,OPT_LEVEL.FARM,"nextsvid"), 
                                   new Option(OPT_ID.SVID_HEAD,OPT_LEVEL.FARM,"svidhead"), 
                                   new Option(OPT_ID.SVID_GEN_DIR,OPT_LEVEL.FARM,"gendir"), 
                                   new Option(OPT_ID.KILL_LIST,OPT_LEVEL.USER,"killlist"), 
                                   new Option(OPT_ID.SHORT_ZOO,OPT_LEVEL.USER,"short_zoo"),
                                   new Option(OPT_ID.VACCINE_TIME,OPT_LEVEL.FARM,"vaccine_time"),
                                   new Option(OPT_ID.MAKE_CANDIDATE,OPT_LEVEL.FARM,"candidate"),
								   new Option(OPT_ID.BUTCHER_LIST,OPT_LEVEL.USER,"butcherlist"),
                                   new Option(OPT_ID.XLS_ASK,OPT_LEVEL.USER,"xls_ask"),
                                   new Option(OPT_ID.XLS_FOLDER,OPT_LEVEL.USER,"xls_folder"),
                                   new Option(OPT_ID.BUILD_FILL_ZERO,OPT_LEVEL.USER,"build_fill_zero"),
                                   new Option(OPT_ID.BUCHER_TYPE,OPT_LEVEL.FARM,"butcher_type")
                                   };
        public Options(RabNetEngine eng)
        {
            this.eng=eng;
        }
        //STD
        public String getOption(String name,String subname,uint uid)
        {
            return eng.db().getOption(name, subname, uid);
        }

        public int getIntOption(String name, String subname, uint uid)
        {
            int res = 0;
            int.TryParse(getOption(name, subname, uid),out res);
            return res;
        }

        public double getFloatOption(String name, String subname, uint uid)
        {
				return float.Parse(getOption(name, subname, uid));
        }

        public int safeIntOption(String name, String subname, uint uid,int def)
        {
            int res = def;
            String s = getOption(name, subname, uid);
            if (!int.TryParse(s,out res)) return def;
            return res;
        }

        public void setOption(String name, String subname, uint uid, String value)
        {
            eng.db().setOption(name, subname, uid, value);
        }

		public void setOption(String name, String subname, uint uid, int value)
		{
			setOption(name, subname, uid, value.ToString());
		}

        public void setOption(String name, String subname, uint uid, double value)
        {
            setOption(name, subname, uid, value.ToString());
        }

        // BY LEVEL
        private uint getUidOfLevel(OPT_LEVEL level)
        {
            if (level==OPT_LEVEL.FARM) return 0;
            int res = eng.userId;
            if (res < 0) return 0;
            return (uint)res;
        }

        public String getOption(String name,String subname,OPT_LEVEL level)
        {
            return getOption(name,subname,getUidOfLevel(level));
        }

        public int getIntOption(String name,String subname,OPT_LEVEL level)
        {
            return getIntOption(name,subname,getUidOfLevel(level));
        }

        public double getFloatOption(String name,String subname,OPT_LEVEL level)
        {
            return getFloatOption(name,subname,getUidOfLevel(level));
        }

        public int safeIntOption(String name, String subname, OPT_LEVEL level,int def)
        {
            return safeIntOption(name, subname, getUidOfLevel(level),def);
        }

        public int safeIntOption(String name, String subname, OPT_LEVEL level)
        {
            return safeIntOption(name, subname, getUidOfLevel(level), 0);
        }
        //public int safeRuBoolOption()

        public void setOption(String name, String subname, OPT_LEVEL level, String value)
        {
            setOption(name, subname, getUidOfLevel(level), value);
        }
        public void setOption(String name, String subname, OPT_LEVEL level, int value)
        {
            setOption(name, subname, getUidOfLevel(level), value);
        }

        public void setOption(String name, String subname, OPT_LEVEL level, double value)
        {
            setOption(name, subname, getUidOfLevel(level), value);
        }

        /// <summary>
        /// Возвращает название опции в Базе Данных
        /// </summary>
        /// <param name="id">enum opt_id</param>
        /// <returns>Название опции в базе данных</returns>
        private Option getOptionById(OPT_ID id)
        {
            for (int i = 0; i < optlist.Length; i++)
                if (optlist[i].id == id) 
                    return optlist[i];
            throw new ExOptionNotFound(id);
        }
        public String getOption(String name,OPT_ID id)
        {
            Option op = getOptionById(id);
            return getOption(name, op.name, op.level);
        }
        public int getIntOption(String name,OPT_ID id)
        {
            Option op = getOptionById(id);
            return getIntOption(name, op.name, op.level);
        }
        public double getFloatOption(String name,OPT_ID id)
        {
            Option op = getOptionById(id);
            return getFloatOption(name, op.name, op.level);
        }
        public int safeIntOption(String name, OPT_ID id, int def)
        {
            Option op = getOptionById(id);
            return safeIntOption(name, op.name, op.level,def);
        }
        public int safeIntOption(String name, OPT_ID id)
        {
            Option op = getOptionById(id);
            return safeIntOption(name, op.name, op.level);
        }
        public void setOption(String name,OPT_ID id,String value)
        {
            Option op=getOptionById(id);
            setOption(name,op.name,op.level,value);
        }
        public void setOption(String name,OPT_ID id,int value)
        {
            Option op = getOptionById(id);
            setOption(name,op.name,op.level,value);
        }
        public void setOption(String name,OPT_ID id,double value)
        {
            Option op=getOptionById(id);
            setOption(name,op.name,op.level,value);
        }

        //by optlist ion def namespace
        public String getOption(OPT_ID id)
        {
            return getOption(defNameSpace, id);
        }
        public int getIntOption(OPT_ID id)
        {
            return getIntOption(defNameSpace, id);
        }
        public double getFloatOption(OPT_ID id)
        {
            return getFloatOption(defNameSpace, id);
        }
        public int safeIntOption(OPT_ID id, int def)
        {
            return safeIntOption(defNameSpace, id, def);
        }
        public int safeIntOption(OPT_ID id)
        {
            return safeIntOption(defNameSpace, id);
        }
        public void setOption(OPT_ID id, String value)
        {
            setOption(defNameSpace, id, value);
        }
        public void setOption(OPT_ID id, int value)
        {
            setOption(defNameSpace, id, value);
        }
        public void setOption(OPT_ID id, double value)
        {
            setOption(defNameSpace, id, value);
        }
    }
}
