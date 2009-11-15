using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class Options
    {
        enum OPT_ID { GENESIS, INBREEDING };
        enum OPT_LEVEL {FARM,USER};
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
        private Option[] optlist = {new Option(OPT_ID.GENESIS,OPT_LEVEL.FARM,"genesis"), 
                                   new Option(OPT_ID.INBREEDING,OPT_LEVEL.FARM,"inbreeding"), 
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
            return int.Parse(getOption(name,subname,uid));
        }
        public double getFloatOption(String name, String subname, uint uid)
        {
            return float.Parse(getOption(name, subname, uid));
        }

        public void setOption(String name, String subname, uint uid, String value)
        {
            eng.db().setOption(name, subname, uid, value);
        }
        public void setOption(String name,String subname,uint uid,int value)
        {
            setOption(name,subname,uid,value.ToString());
        }
        public void setOption(String name, String subname, uint uid, double value)
        {
            setOption(name, subname, uid, value.ToString());
        }

        // BY LEVEL
        private uint getUidOfLevel(OPT_LEVEL level)
        {
            if (level==OPT_LEVEL.FARM) return 0;
            int res=eng.uId();
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

        //by oplist in other namespace
        private Option getOptionById(OPT_ID id)
        {
            for (int i = 0; i < optlist.Length; i++)
                if (optlist[i].id == id) return optlist[i];
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
        public void setOption(String name,OPT_ID id,String value)
        {
            Option op=getOptionById(id);
            setOption(name,op.name,op.level,value);
        }
        public void setOption(String name,OPT_ID id,int value)
        {
            Option op=getOptionById(id);
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
