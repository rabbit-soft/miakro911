using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    /// <summary>
    /// Нужен в основном Для передачи Параметром программы SQL запросам
    /// </summary>
    public class Filters : Dictionary<String, String>
    {
        #region const
        public const string ID = "id";
        public const string RAB_ID = "rab_id";
        public const string DATE_FROM = "dateFrom";
        public const string DATE_TO = "dateTo";
        public const string LIMIT = "limit";
        public const string ADDRESS = "adr";
        public const string FREE = "free"; 
        /// <summary>
        /// Сокращения
        /// </summary>
        public const string SHORT = "shr";
        public const string MALE = "mt";
        public const string FEMALE = "ft";
        public const string SHOW_BLD_TIERS = "sht";
        public const string SHOW_BLD_DESCR = "sho";
        public const string SHOW_OKROL_NUM = "num";
        public const string MAKE_CANDIDATE = "cand";
        public const string SHOW_CANDIDATE = "sh_cand";
        public const string SHOW_REST = "sh_rest";
        public const string MAKE_BRIDE = "brd";
        /// <summary>
        /// Прививать крольчат с матерью
        /// </summary>
        public const string VACC_MOTH = "vacc_moth";
        /// <summary>
        /// Набор id прививок для отображение
        /// </summary>
        public const string VACC_SHOW = "vacc_show";
        /// <summary>
        /// Отсадка мальчиков в возврасте начиная с...
        /// </summary>
        public const string BOYS_OUT = "boysout";
        /// <summary>
        /// отсадка девочек в возврасте начиная с...
        /// </summary>
        public const string GIRLS_OUT = "girlsout";
        public const string VUDVOR = "vudvor";
        /// <summary>
        /// На какой день принять окрол
        /// </summary>
        public const string OKROL = "okrol";
        public const string DBL_SURNAME = "dbl";
        public const string FIND_PARTNERS = "prt";
        public const string PRE_OKROL = "preok";
        public const string NEST_OUT_IF_SUKROL = "vd_sukr";
        public const string COUNT1 = "count1";
        public const string COUNT2 = "count2";
        public const string COUNT3 = "count3";
        public const string NEST_IN = "nest";
        public const string CHILD_NEST = "cnest";
        public const string STATE_FUCK = "sfuck";
        public const string FIRST_FUCK = "ffuck";
        public const string HETEROSIS = "heter";
        public const string INBREEDING = "inbr";
        public const string BRIDE_AGE = "brideAge";
        public const string MALE_WAIT = "mwait";
        public const string BOYS_BY_ONE = "bbone";
        public const string TYPE = "type";
        public const string TIER = "yar";
        public const string LOGS = "lgs"; 
        #endregion const

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

        public int safeInt(String key, int def) 
        {
            int result = 0;
            string val = safeValue(key, def.ToString());
            int.TryParse(val, out result);
            return result; 
        }//TODO не безопасно
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
