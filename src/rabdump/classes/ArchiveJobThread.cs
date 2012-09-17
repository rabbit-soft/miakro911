using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using log4net;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using X_Tools;

namespace rabdump
{
    class ArchiveJobThread
    {
        private const String ZIP_PASSWORD="ns471lbNITfq3";
        public const int SPLIT_NAMES = 6;
        /// <summary>
        /// Расписание, которые выполняется в данный момент.
        /// </summary>
        private readonly ArchiveJob _j = null;
        private readonly ArchiveJobThread _jobber = null;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ArchiveJobThread));
        readonly String _tmppath = "";

        public ArchiveJobThread(ArchiveJob job)
        {
            _j = job;
            //_j.Busy = true;
            _jobber = this;
            _tmppath = Path.GetTempPath();
        }
        ~ArchiveJobThread()
        {
            if (_j.Busy)
                _j.Busy = false;
        }

        public bool JobIsBusy
        {
            get { return _j.Busy; }
        }

        public string JobName
        {
            get { return _j.Name; }
        }

        /// <summary>
        /// Возвращает информацию по дампам расписания:
        /// Количество дампов, Общий размер дампов, Название позднего или Раннего файла
        /// </summary>
        /// <param name="sz">Общий размер дампов расписания</param>
        /// <param name="resFile">Имя файла, зафисит от параметра minimum</param>
        /// <param name="minimum">Самый старый файл дампа, принадлежащий расписанию</param>
        /// <returns>Количество дампов данного расписания</returns>
        public int CountBackups(out int sz,out String resFile, bool minimum)
        {
            _logger.Debug("count backups");
            if (!Directory.Exists(_j.BackupPath))
                Directory.CreateDirectory(_j.BackupPath);
            DirectoryInfo inf = new DirectoryInfo(_j.BackupPath);
            DateTime asDT = minimum ? DateTime.MaxValue : DateTime.MinValue;
            int cnt = 0;// Количество дампов расписания
/*
            sz = 0;
*/
            resFile = "";
            long fsz = 0;
            foreach(FileInfo fi in inf.GetFiles("*_*_*_*_*_*.7z"))
            {
                String[] nm = ParseDumpName(fi.FullName);
                if (nm[0]==_j.Name)
                {
                    cnt++;
                    fsz += fi.Length;
                    String h = nm[5].Substring(0, 2); //TODO а что если дата в имени стерта
                    String m = nm[5].Substring(2, 2);
                    String s = nm[5].Substring(4, 2);
                    DateTime dt = new DateTime(int.Parse(nm[2]), int.Parse(nm[3]), int.Parse(nm[4]), int.Parse(h), int.Parse(m), int.Parse(s));
                    if (minimum)
                    {
                        if (dt < asDT)//если переданная Дата меньше
                        {
                            resFile = Path.GetFileName(fi.FullName);
                            asDT = dt;
                        }
                    }
                    else
                    {
                        if (dt > asDT)//если переданная Дата больше
                        {
                            resFile = Path.GetFileName(fi.FullName);
                            asDT = dt;
                        }
                    }
                }
            }           
            sz =(int)Math.Round((double)fsz/(1024 * 1024));
            return cnt;
        }
        public int CountBackups(out int sz, out String File)
        {
            return CountBackups(out sz, out File, true);
        }

        public bool FileExists(string filename, string md5)
        {
            if (File.Exists(_j.BackupPath + "\\" + filename))
            {
                return pEngine.Helper.GetMD5FromFile(_j.BackupPath + "\\" + filename) == md5;
            }
            else return false;
        }

        /// <summary>
        /// Делает проверку на Лимит Количества и Размера резервных копий.
        /// Удаляет саммые ранние если РК выходят за рамки.
        /// </summary>
        public void CheckLimits()
        {
            try
            {
                _logger.Debug("checking limits");
                int sz = 0;
                string min = "";
                int cnt = CountBackups(out sz, out min);
                if (_j.CountLimit > 0)
                    while (_j.CountLimit < cnt)
                    {
                        File.Delete(_j.BackupPath + "\\" + min);
                        _logger.InfoFormat("Deleting file: {0:s}\\{1:s}", _j.BackupPath, min);
                    }
                //CountBackups(out sz, out min);
                if (_j.SizeLimit > 0)
                    while (_j.SizeLimit < sz)
                    {
                        File.Delete(_j.BackupPath + "\\" + min);
                        _logger.InfoFormat("Deleting file: {0:s}\\{1:s}", _j.BackupPath, min);
                        CountBackups(out sz, out min);
                    }
            }
            catch (Exception exc)
            {
                _logger.Error(exc);
            }
        }

        /// <summary>
        /// Делает Резервирование БазыДанных
        /// </summary>
        /// <param name="j">Расписание резервирования</param>
        public static void MakeJob(ArchiveJob j)
        {
            ArchiveJobThread jt = new ArchiveJobThread(j);
            j.Busy = true;
            Thread t = new Thread(jt.Run);
            t.Start();
        }

        /// <summary>
        /// Запускает резервирование в отдельном потоке
        /// </summary>
        public void Run()
        {
            _logger.Debug("Making dump for " + _j.Name);
            if (_j.DB == DataBase.AllDataBases)
            {
                DataBaseCollection dbs = Options.Get().Databases;
                for (int i = 0; i < dbs.Count - 1; i++)
                    DumpDB(dbs[i]);
            }
            else
                DumpDB(_j.DB);
            _jobber.OnEndJob();
        }

        /// <summary>
        /// При окончании резервирования
        /// </summary>
        public void OnEndJob()
        {
            _j.Busy = false;
            _j.LastWork = DateTime.Now;
            _logger.Debug("End of making dump " + _j.Name);
        }

        public static void RunRabnet(string param)
        {
            try
            {
                Process p = Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + @"\..\RabNet\rabnet.exe", param);
                if (param == "dbedit")
                {
                    p.WaitForExit();
                    Options.Get().Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: "+ex.Message);
            }
        }

        /// <summary>
        /// Делает резервную копию переданной Базы данных
        /// </summary>
        /// <returns>Путь к созданному файлу</returns>
        public string DumpDB(DataBase db)
        {
            Directory.CreateDirectory(_j.BackupPath);
            String ffname = _j.Name.Replace(' ', '+') + "_" + db.Name.Replace(' ', '+') + "_" + DateTime.Now.ToString("yyyy_MM_dd_HHmmss");

            ffname = XTools.SafeFileName(ffname, "-");

            String fname = _tmppath + ffname;
            _logger.Info("Making dump for " + _j.Name + " to " + ffname);
            ///Делайем дамп с помошью mysqldump
            String md = Options.Get().MySqlDumpPath;
            if (md == "")
            {
                _logger.Error("MySQLDump not specified " + md);
                throw new ApplicationException("Путь к MySQLDump не настроен");
            }
            String pr = String.Format("{0:s} {1:s} {2:s} {3:s} --ignore-table={0:s}.allrabbits", db.DBName, (db.Host == "" ? "" : "-h " + db.Host),
                (db.User == "" ? "" : "-u " + db.User), (db.Password == "" ? "" : "--password=" + db.Password));
            try
            {
                ProcessStartInfo inf = new ProcessStartInfo(md, pr);

                inf.UseShellExecute = false;
                inf.RedirectStandardOutput = true;
                inf.CreateNoWindow = true;
                inf.StandardOutputEncoding = Encoding.UTF8;

                Process p = Process.Start(inf);
                TextWriter wr = new StreamWriter(fname + ".dump", false, Encoding.UTF8);
                wr.Write(p.StandardOutput.ReadToEnd());
                p.WaitForExit();
                wr.Close();
                if (p.ExitCode != 0)
                    throw new ApplicationException("MySQLDump вернул результат " + p.ExitCode.ToString());
                p.Close();
            }
            catch (Exception ex)
            {
                _logger.Error("Error while " + md + ":" + ex.GetType().ToString() + ":" + ex.Message);
                try
                {
                    File.Delete(fname + ".dump");
                }
                catch (Exception ex2)
                {
                    _logger.Error("Error while " + md + ":" + ex2.GetType().ToString() + ":" + ex2.Message);
                    //return "";
                }
                return "";
            }
            ///Упаковываем в архив
            bool is7z = false;
            md = Options.Get().Path7Z;
            if (md == "")///если путь к 7zip не настроен, то в папку BackUps копируется .dump-файл
                _logger.Warn("7z not specified");
            else
            {
                try
                {
                    ProcessStartInfo inf = new ProcessStartInfo(md, string.Format(" a -mx9 -p{0} \"{1}.7z\" \"{1}.dump\"", ZIP_PASSWORD, fname));

                    inf.CreateNoWindow = true;
                    inf.RedirectStandardOutput = true;
                    inf.UseShellExecute = false;

                    Process p = Process.Start(inf);
                    p.WaitForExit();
                    if (p.ExitCode != 0)
                        throw new ApplicationException("Ошибка при разархивации: " + p.ExitCode.ToString());
                    File.Delete(fname + ".dump");
                    is7z = true;
                }
                catch (Exception ex)
                {
                    _logger.Error("Error while " + md + ":" + ex.GetType().ToString() + ":" + ex.Message);
                    //return;
                }
            }
            _logger.Debug("copy " + fname + (is7z ? ".7z" : ".dump") + " to " + _j.BackupPath + "\\" + ffname + (is7z ? ".7z" : ".dump"));
            string movepath = _j.BackupPath + "\\" + ffname + (is7z ? ".7z" : ".dump");
            File.Move(fname + (is7z ? ".7z" : ".dump"), movepath);
            CheckLimits();
            _logger.Debug("finishing dumping");
            return movepath;
        }


        /// <summary>
        /// Востановление БД из резервной копии
        /// </summary>
        public static void UndumpDB(string host,string db,string user,string password,string file)
        {
            _logger.Debug("undumping " + file + " to " + host + ":" + db + ":" + user + ":" + password);

            String tmppath = Path.GetTempPath();
            if (!Directory.Exists(tmppath))           
                Directory.CreateDirectory(tmppath);
           
            String sql = Options.Get().MySqlExePath;              
            if (sql == "" || !File.Exists(sql))
                throw new ApplicationException("Путь к MySQL не настроен"); 
         
            //String pth=Path.GetDirectoryName(file);
            String fl = Path.GetFileName(file);
            String ext = Path.GetExtension(file);
            String f = tmppath + fl;
            _logger.Debug("copy "+file+" to "+f);
            File.Copy(file,f,true);

            if (ext == ".7z")//распаковка файла если расширение .7z
            {
                _logger.Debug("decompress 7z");
                String z7 = Options.Get().Path7Z;
                String ff = tmppath + Path.GetFileNameWithoutExtension(f) + ".dump";
                if (z7 == "" || !File.Exists(z7))
                {
                    throw new ApplicationException("Путь к 7z не настроен");
                }
                //ExtractDump(f);
                ProcessStartInfo inf = new ProcessStartInfo(z7, " e -p" + ZIP_PASSWORD + " \"" + f + "\"");

                inf.WorkingDirectory = tmppath;
                inf.CreateNoWindow = true;
                inf.RedirectStandardOutput = true;
                inf.UseShellExecute = false;

                Process p = Process.Start(inf);
                p.WaitForExit();
                int res = p.ExitCode;
                p.Close();
                File.Delete(f);
                if (res != 0)
                {
                    File.Delete(ff);
                    throw new ApplicationException("7z вернул результат: " + z7err(res));
                }

                f = CheckDumpPath(ff);
                if (f == "")
                {
                    throw new ApplicationException("Ошибка при разархивировании");
                }
                _logger.Debug("dumpname: "+f);
            }
            _logger.Debug("mysql");
            
            ///Заливаем данные из .dump-файла в БД
            String prms = String.Format(@"{1:s} {2:s} {3:s} {0:s}", db, (host != "" ? "-h " + host : ""), (user != "" ? "-u " + user : ""), (password != "" ? "--password=" + password : ""));
            try
            {
                ProcessStartInfo pinf = new ProcessStartInfo(sql, prms);

                pinf.UseShellExecute = false;
                pinf.RedirectStandardInput = true;
                pinf.CreateNoWindow = true;
                pinf.RedirectStandardError = true;

                Process mp = Process.Start(pinf);
                FileStream rd = new FileStream(f, FileMode.Open);
                byte[] buf = new byte[rd.Length];
                rd.Read(buf, 0, (int)rd.Length);
                rd.Close();
                byte[] b2 = buf;// Encoding.Convert(Encoding.UTF8, Encoding.ASCII, buf);
                mp.StandardInput.BaseStream.Write(b2, 0, b2.Length);
                mp.StandardInput.Close();
                String mout = mp.StandardError.ReadToEnd();
                mp.WaitForExit();
                int res = mp.ExitCode;
                mp.Close();
                if (res != 0 || mout!="")
                    throw new ApplicationException("MySQL вернул результат "+res.ToString()+"\nerror="+mout);
            }
            catch(Exception ex)
            {
                File.Delete(f);
                throw ex;
            }
            File.Delete(f);
        }

        /// <summary>
        /// РАзархивирует РКБД и возвращает путь к .dump файлу
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ExtractDump(string filePath)
        {
            _logger.InfoFormat("Extracting file: {0:s}",filePath);
            if (filePath.EndsWith(".dump"))
                return filePath;
            else
            {
                string targPath = Path.GetTempPath() + Path.GetFileNameWithoutExtension(filePath) + ".dump";
                if (File.Exists(targPath))
                    File.Delete(targPath);
                ProcessStartInfo inf = new ProcessStartInfo(Options.Get().Path7Z, " e -p" + ZIP_PASSWORD + " \"" + filePath + "\"");

                inf.WorkingDirectory = Path.GetTempPath();
                inf.CreateNoWindow = true;
                inf.RedirectStandardOutput = true;
                inf.UseShellExecute = false;

                Process p = Process.Start(inf);
                p.WaitForExit();
                int res = p.ExitCode;
                p.Close();
                if (res != 0)
                {
                    _logger.ErrorFormat("7z error: {0:s}", z7err(res));
                    return "";
                }
                return CheckDumpPath(targPath);
            }
        }

        /// <summary>
        /// Создание архива
        /// </summary>
        /// <param name="filePath">Путь к файлу, который надо заархивировать</param>
        /// <param name="z7type">Если true, то упаковывается в 7zip,
        /// если false, то упаковать в zip</param>
        /// <returns>Усли пустая строка значит произошла ошибка</returns>
        public static string ZipFile(string filePath, bool z7type)
        {
            _logger.InfoFormat("start Zipping file \"{0:s}\" in {1:s}", filePath, z7type ? "7z" : "zip");
            if (filePath.EndsWith(".zip") || filePath.EndsWith(".7z")) return filePath;
            try
            {
                string trgPath = Path.GetTempPath() + Path.GetFileNameWithoutExtension(filePath);
                ProcessStartInfo inf = new ProcessStartInfo(Options.Get().Path7Z, string.Format(" a -mx9 -p{0} \"{1}.{2:s}\" \"{1}.dump\"", ZIP_PASSWORD, trgPath, z7type ? "7z" : "zip"));
                inf.CreateNoWindow = true;
                inf.RedirectStandardOutput = true;
                inf.UseShellExecute = false;

                Process p = Process.Start(inf);
                p.WaitForExit();
                int res = p.ExitCode;
                p.Close();
                if (res != 0)
                {
                    _logger.ErrorFormat("7z error: {0:s}", z7err(res));
                    return "";
                }
                return trgPath + (z7type ? ".7z" : ".zip");
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("ZipFile", ex);
                return "";
            }
        }
        public static string ZipFile(string filePath)
        {
            return ZipFile(filePath, true);
        }

        /// <summary>
        /// Нужно потому что не всегда название дампа совпадает с названием архива
        /// </summary>
        /// <param name="dumpPath">Ориентировочный путь к дампу</param>
        /// <returns> Путь к Дампу
        /// Если пустая строка,значит произошла ошибка</returns>
        private static string CheckDumpPath(string dumpPath)
        {
            if (!File.Exists(dumpPath))
            {
                string[] files = Directory.GetFiles(Path.GetTempPath(), "*.dump");
                if (files.Length == 1)
                    return files[0];
                else if (files.Length == 0)
                    return "";
                else
                {
                    DateTime dt = DateTime.MinValue;
                    string ourFile = "";
                    foreach (string s in files)
                    {
                        DateTime ct = File.GetCreationTime(s);
                        if (ct > dt)
                        {
                            dt = ct;
                            ourFile = s;
                        }
                    }
                    return ourFile;
                }
            }
            else
                return dumpPath;
        }

        /// <summary>
        /// Получить путь к самому позднему файлу дампа
        /// </summary>
        /// <param name="md5">Хэш дампа</param>
        /// <returns>Полный путь</returns>
        public string GetLatestDump(out string md5)
        {
            int sz;
            string file;
            int count = CountBackups(out sz, out file, false);
            if (count == 0)
            {
                md5 = "";
                return "";
            }
            else
            {
                string path = _j.BackupPath + "\\" + file;
                md5 = pEngine.Helper.GetMD5FromFile(path);
                return path;
            }
        }
        public string GetLatestDump()
        {
            string md5;
            return GetLatestDump(out md5);
        }

        public static string[] ParseDumpName(string fullName)
        {
            string[] result = Path.GetFileName(fullName).Split('_','.');
            for (int i = 0; i < result.Length; i++)
                result[i] = result[i].Replace('+', ' ');
            return result;
        }

        private static string z7err(int res)
        {
            switch (res)
            {
                case 2: return "Архив поврежден";
                default: return res.ToString();
            }
        }

        /*private string getMD5FromFile(string filepath)
        {           
            filepath = ExtractDump(filepath);
            if (filepath == "") return "0";
            FileStream file = new FileStream(filepath, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            File.Delete(filepath);
            return sb.ToString();
        }*/

    }
}
