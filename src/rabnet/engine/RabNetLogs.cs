using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetLogs
    {
        public enum LogType {GETEROSIS_ON,GETEROSIS_OFF,INBREEDING_ON,INBREEDING_OFF,RABBIT_CHANGE};
        private RabNetEngine eng; 
        public RabNetLogs(RabNetEngine eng)
        {
            this.eng = eng;
        }
        public static void log(RabNetEngine eng, LogType type, String text)
        {
            eng.db().RabNetLog((int)type, eng.uId(), text);
        }
        public static void log(RabNetEngine eng, LogType type)
        {
            log(eng, type, "");
        }
        public void log(LogType type, String text)
        {
            log(eng, type, text);
        }
        public void log(LogType type)
        {
            log(type, "");
        }
    }
}
