using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace CAS
{
    static class ScaleOpt
    {
        private static RegistryKey _reg = Registry.CurrentUser.CreateSubKey(@"Software\9-Bits\lp16");

        public enum OptType { ScaleAddres, ScalePort, 
                              ScanPLUFrom, ScanPLUUntil,
                              ScanMSGFrom, ScanMSGUntil, 
                              Monitoring, ScanDelay}

        private static string getRegName(OptType tp)
        {
            switch (tp)
            {
                case OptType.ScaleAddres: return "adrs";
                case OptType.ScalePort: return "port";
                case OptType.ScanPLUFrom: return "spf";
                case OptType.ScanPLUUntil: return "spu";
                case OptType.ScanDelay: return "freq";
                case OptType.ScanMSGFrom: return "smf";
                case OptType.ScanMSGUntil: return "smu";
                case OptType.Monitoring: return "mon"; 
            }
            return "";
        }

        private static string getDefVal(OptType tp)
        {
            switch (tp)
            {
                case OptType.ScaleAddres: return "192.168.0.5";
                case OptType.ScalePort: return "8111";
                case OptType.ScanPLUFrom: return "1";
                case OptType.ScanPLUUntil: return "10";
                case OptType.ScanMSGFrom: return "1";
                case OptType.ScanMSGUntil: return "20";
                case OptType.ScanDelay: return "5";
                case OptType.Monitoring: return "0";

            }
            return "";
        }

        public static void SaveBoolOpt(OptType op, bool val)
        {
            SaveStrOpt(op, val ? "1" : "0");
        }

        public static void SaveIntOpt(OptType op, int val)
        {
            SaveStrOpt(op, val.ToString());
        }

        public static void SaveStrOpt(OptType op, string val)
        {
            string nm = getRegName(op);
            _reg.SetValue(nm, val,RegistryValueKind.String);
        }

        public static bool GetBoolOpt(OptType op)
        {
            string s = GetStrOpt(op);
            return s == "1";
        }

        public static int GetIntOpt(OptType op)
        {
            string s = GetStrOpt(op);
            int r = 0;
            int.TryParse(s, out r);
            return r;
        }

        public static string GetStrOpt(OptType op)
        {
            string nm = getRegName(op);
            return (string)_reg.GetValue(nm, getDefVal(op));
        }
    }
}
