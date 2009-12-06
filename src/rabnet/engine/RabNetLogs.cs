using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetLogs
    {
        public enum LogType {INCOME,REPLACE};
        private RabNetEngine eng; 
        public RabNetLogs(RabNetEngine eng)
        {
            this.eng = eng;
        }
        public static void log(RabNetEngine eng, LogType type, int r1,int r2,string a1,string a2,String text)
        {
            eng.db().RabNetLog((int)type, eng.uId(),r1,r2,a1,a2, text);
        }
        public static void log(RabNetEngine eng, LogType type){log(eng, type, 0,0,"","","");}
        public void log(LogType type){log(type, "");}
        public void log(LogType type, String text){log(eng, type, 0,0,"","",text);}
        public void log(LogType type, int r1){log(eng, type, r1, 0, "", "", "");}
        public void log(LogType type, int r1, string address) { log(eng, type, r1, 0, address, "", ""); }
        public void log(LogType type, int r1, string address,string address2) { log(eng, type, r1, 0, address, address2, ""); }
        public void log(LogType type, int r1, int r2, string address) { log(eng, type, r1, r2, address, "", ""); }
        public void log(LogType type, int r1, int r2, string address,string address2) { log(eng, type, r1, r2, address, address2, ""); }
        public void log(LogType type, int r1, int r2, string address, string address2,string text) { log(eng, type, r1, r2, address, address2, text); }
    }
}
