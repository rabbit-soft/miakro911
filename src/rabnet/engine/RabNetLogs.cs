using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetLogs
    {
        public enum LogType {NONE,INCOME,REPLACE,BON,RENAME,FUCK,OKROL,PROHOLOST,RAB_CHANGE,NEST_ON,NEST_OFF,
            HEATER_OUT,HEATER_OFF,HEATER_ON,REPAIR_ON,REPAIR_OFF,RABBIT_KILLED,COUNT_KIDS,
            SET_SEX,CLONE_GROUP,RESURRECT,PREOKROL,COMBINE,PLACE_SUCK,CHANGE_DEADREASON, VACCINE,
            CH_BIRTH, CH_BREED, CH_GROUP, CH_SURNAME, CH_SECNAME, CH_ZONE, CH_RATE,CH_STATE};
        private RabNetEngine eng; 
        public RabNetLogs(RabNetEngine eng)
        {
            this.eng = eng;
        }
        public static void log(RabNetEngine eng, LogType type, int r1, int r2, string a1, string a2, String text)
        {
            eng.db().RabNetLog((int)type, eng.userId, r1, r2, a1.Trim(), a2.Trim(), text);
        }
        public static void log(RabNetEngine eng, LogType type){log(eng, type, 0,0,"","","");}
        public void log(LogType type){log(type, "");}
        public void log(LogType type, String text){log(eng, type, 0,0,"","",text);}
        public void log(LogType type, int r1) { log(eng, type, r1, 0, "", "", ""); }
        public void log(LogType type, int r1, string address) { log(eng, type, r1, 0, address, "", ""); }
        public void log(LogType type, int r1, string address,string address2) { log(eng, type, r1, 0, address, address2, ""); }
        public void log(LogType type, int r1, int r2, string address) { log(eng, type, r1, r2, address, "", ""); }
        public void log(LogType type, int r1, int r2, string address,string address2) { log(eng, type, r1, r2, address, address2, ""); }
        public void log(LogType type, int r1, int r2, string address, string address2,string text) { log(eng, type, r1, r2, address, address2, text); }
    }
}
