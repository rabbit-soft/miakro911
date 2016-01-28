using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace rabnet
{
    static class Run
    {
        private const string MIA_CONV = "mia_conv.exe";
        private const String RABDUMP = "rabdump.exe";
        private const String RABNET = "rabnet.exe";
        private const String UPDATER = "updater.exe";

        /// <summary>
        /// Создает структуру БД для программы rabnet
        /// </summary>
        /// <param name="miaParams">Параметры программы mia_conv</param>
        /// <exception cref="Exception">При неудачном создании БД</exception>
        public static void DBCreate(String miaParams, String host, String db, String user, String pwd, String admin, String apwd)
        {
            String prms = String.Format("\"{0:s}\" {1:s};{2:s};{3:s};{4:s};{5:s};{6:s};", miaParams, host, db, user, pwd, admin, apwd);
            prms += " зоотехник;";

            String prg = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), MIA_CONV);
            if (!File.Exists(prg))
                throw new Exception(String.Format("Не удается найти программу {0:s}{1:s}БД не будет создана", prg, Environment.NewLine));
            Process p = Process.Start(prg, prms);
            p.WaitForExit();
            if (p.ExitCode != 0)
                throw new Exception("Ошибка создания БД. " + miaExitCode.GetText(p.ExitCode));
        }

        public static void RabDump()
        {
            String path = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), RABDUMP);
            if (!File.Exists(path))
                throw new Exception("Не удается найти файл " + path);
            Process p = Process.Start(path);
            //p.WaitForExit();
        }

        /// <summary>
        /// Запускает программу Rabnet
        /// </summary>
        /// <param name="param"></param>
        public static void Rabnet()
        {
            String path = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), RABNET);
            if (!File.Exists(path))
                throw new Exception("Не удается найти файл "+path);
            Process p = Process.Start(path);
            //p.WaitForExit();
        }

        public static string SerachConfig(string p)
        {
            return Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), p + ".exe.config");//TODO дописать
        }

        //private static string searchConfig()
        //{
        //    return "";
        //}

        internal static void Updater()
        {
            String prg = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), UPDATER);
            if (!File.Exists(prg)) {
                throw new Exception(String.Format("Не удается найти программу {0:s}{1:s}БД не будет обновлена", prg, Environment.NewLine));
            }
            Process p = Process.Start(prg);
        }
    }
}
