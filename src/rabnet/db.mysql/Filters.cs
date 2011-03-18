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
        public TreeData(String text)
            : this()
        {
            caption = text;
        }
    }
    /// <summary>
    /// Класс для заполнения справочников в CatalogForm
    /// </summary>
    public class CatalogData
    {
        /// <summary>
        /// Одна строка DataGridView. Имеет, ID и массив значений ячеек
        /// </summary>
        public struct Row
        {
            public int key;
            public String[] data; 
            public byte[] image;
            public int imageSize;
        }
        /// <summary>
        /// Массив имен столбцов DataGridView
        /// </summary>
        public String[] colnames;
        /// <summary>
        /// Массив строк DataGridView
        /// </summary>
        public Row[] data;
    }

    public class Filters : Dictionary<String, String>
    {
        public string safeValue(String key, String def)
        {
            if (!ContainsKey(key))
                return def;
            return this[key];
        }
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
