using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class TreeData
    {
        public String caption;
        public TreeData[] items;
        public TreeData() { }
        /// <summary>
        /// Данные о ветке, представляют собой 3 значения, разделенные ":"
        /// 1 - Id строения
        /// 2 - Id яруса (tier)
        /// 3 - Название
        /// </summary>
        public TreeData(String text): this()
        {
            caption = text;
        }
    }

    /// <summary>
    /// Нужен в основном Для передачи Параметром программы SQL запросам
    /// </summary>
    public class Filters : Dictionary<String, String>
    {
        public const string MALE = "mt";
        public const string FEMALE = "ft";

        public Filters() : base() { }
        public Filters(String s)
            : base()
        {
            fromString(s);
        }

        public static Filters makeFromString(String s)
        {
            return new Filters(s);
        }
        public string safeValue(String key, String def)
        {
            if (!ContainsKey(key))
                return def;
            return this[key];
        }
        public String safeValue(String key) { return safeValue(key, ""); }

        public int safeInt(String key, int def) { return int.Parse(safeValue(key, def.ToString())); }
        public int safeInt(String key) { return safeInt(key, 0); }

        public bool safeBool(String key, bool def) { return (safeInt(key, (def ? 1 : 0)) == 1); }
        public bool safeBool(String key) { return safeBool(key, false); }

        public String toString()
        {
            String res = "";
            for (KeyCollection.Enumerator i = Keys.GetEnumerator(); i.MoveNext(); )
            {
                string val = this[i.Current];
                val.Replace("\\", "\\\\");
                val.Replace("=", "\\1");
                val.Replace(";", "\\2");
                res += i.Current + "=" + this[i.Current] + ";";
            }
            return res;
        }

        public void fromString(String str)
        {
            this.Clear();
            foreach (string s in str.Split(';'))
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
}
