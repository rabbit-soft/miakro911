using System;
using System.Collections.Generic;

namespace rabnet
{
    public class Filters : Dictionary<String, String> {
        public string safeValue(String key, String def)
        {
            if (!ContainsKey(key))
                return def;
            return this[key];
        }
        public Filters() : base() { }
        public Filters(String s):base()
        {
            fromString(s);
        }
        public static Filters makeFromString(String s)
        {
            return new Filters(s);
        }
        public String safeValue(String key){return safeValue(key,"");}
        public int safeInt(String key, int def){return int.Parse(safeValue(key, def.ToString()));}
        public int safeInt(String key) { return safeInt(key, 0); }
        public bool safeBool(String key,bool def){return (safeInt(key,(def?1:0))==1);}
        public bool safeBool(String key){return safeBool(key, false);}
        public String toString()
        {
            String res="";
            for (KeyCollection.Enumerator i = Keys.GetEnumerator(); i.MoveNext(); )
            {
                string val = this[i.Current];
                val.Replace("\\","\\\\");
                val.Replace("=","\\1");
                val.Replace(";","\\2");
                res += i.Current + "=" + this[i.Current] + ";";
            }
            return res;
        }
        public void fromString(String str)
        {
            this.Clear();
            foreach(string s in str.Split(';'))
            {
                if (s != "")
                {
                    String[] kv = s.Split('=');
                    kv[1].Replace("\\1", "=");
                    kv[1].Replace("\\2", ";");
                    kv[1].Replace("\\\\", "\\");
                    this[kv[0]] = kv[1];
                }
            }
        }
    } 

    public class ExDBDriverNotFoud : Exception
    {
        public ExDBDriverNotFoud(String driver) : base("Database Driver " + driver + " not found!") { }
    }

    public interface IData
    {
    }

    public interface IDataGetter
    {
        int getCount();
        void stop();
        IData getNextItem();
    }

    public interface IRabbit : IData
    {
        int id();
        String name();
        String sex();
        int age();
        String breed();
        String weight();
        String status();
        String bgp();
        String N();
        int average();
        int rate();
        String cls();
        String address();
        String notes();
    }

    public interface IBuilding : IData
    {
        int id();
        int farm();
        int tier_id();
        string delims();
        string type();
        string itype();
        string notes();
        bool repair();
        int secs();
        string area(int id);
        string dep(int id);
        int nest_heater_count();
        int busy(int id);
        string use(int id);
        string nest();
        string heater();
        string address();
    }

    public class TreeData
    {
        public String caption;
        public TreeData[] items;
        public TreeData(){}
        public TreeData(String text):this()
        {
            caption = text;
        }
    }

    public interface IRabNetDataLayer
    {
        void init(String connectionString);
        void close();
        //ENVIRONMENT
        List<String> getUsers();
        int checkUser(String name, String password);
        String getOption(String name, String subname, uint uid);
        void setOption(String name, String subname, uint uid, String value);
        DateTime now();
        String[] getFilterNames(String type);
        Filters getFilter(String type, String name);
        void setFilter(String type, String name, Filters filter);
        //DATA PROCEDURES
        IDataGetter getRabbits(Filters filters);
        IDataGetter getBuildings(Filters filters);
        TreeData rabbitGenTree(int rabbit);
        TreeData buildingsTree();
        IDataGetter getYoungers(Filters filters);
    }

}