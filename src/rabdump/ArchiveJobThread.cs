using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using log4net;
using System.IO;
using System.Windows.Forms;

namespace rabdump
{
    class ArchiveJobThread
    {
        private const String Password="ns471lbNITfq3";
        public const int SplitNames = 6;
        private readonly ArchiveJob _j=null;
        private readonly ArchiveJobThread _jobber=null;
        private static readonly ILog log = LogManager.GetLogger(typeof(ArchiveJobThread));
        readonly String _tmppath = "";
        public ArchiveJobThread(ArchiveJob job)
        {
            _j = job;
            _j.Busy = true;
            _jobber = this;
            _tmppath = Path.GetTempPath();
        }

        public int CountBackups(out int sz,out String minFile)
        {
            Directory.CreateDirectory(_j.BackupPath);
            DirectoryInfo inf = new DirectoryInfo(_j.BackupPath);
            DateTime mindt=DateTime.MaxValue;
            int cnt=0;
/*
            sz = 0;
*/
            minFile = "";
            long fsz = 0;
            foreach(FileInfo fi in inf.GetFiles())
            {
                String[] nm = Path.GetFileName(fi.FullName).Split('_');
                if (nm.Length==SplitNames && nm[0]==_j.Name)
                {
                    cnt++;
                    fsz += fi.Length;
                    String h = nm[5].Substring(0, 2);
                    String m = nm[5].Substring(2, 2);
                    String s = nm[5].Substring(4, 2);
                    DateTime dt = new DateTime(int.Parse(nm[2]), int.Parse(nm[3]), int.Parse(nm[4]), int.Parse(h), int.Parse(m), int.Parse(s));
                    if (dt < mindt)
                    {
                        minFile = Path.GetFileName(fi.FullName);
                        mindt = dt;
                    }
                }
            }
            sz =(int)Math.Round((double)fsz/(1024 * 1024));
            return cnt;
        }

        public void CheckLimits()
        {
            int sz=0;
            string min="";
            if (_j.CountLimit > 0)
                while (_j.CountLimit > CountBackups(out sz,out min))
                    File.Delete(_j.BackupPath + "\\" + min);
            CountBackups(out sz, out min);
            if (_j.SizeLimit > 0)
                while (_j.SizeLimit > sz)
                {
                    File.Delete(_j.BackupPath + "\\" + min);
                    CountBackups(out sz, out min);
                }
        }

        public void DumpDB(DataBase db)
        {
            Directory.CreateDirectory(_j.BackupPath);
            String ffname = _j.Name + "_" + db.Name + "_" + DateTime.Now.ToString("yyyy_MM_dd_HHmmss");

            ffname = ffname.Replace("?", "_");
            ffname = ffname.Replace(":", "_");
            ffname = ffname.Replace("\\", "_");
            ffname = ffname.Replace("/", "_");
            ffname = ffname.Replace("*", "_");
            ffname = ffname.Replace("\"", "_");
            ffname = ffname.Replace("<", "_");
            ffname = ffname.Replace(">", "_");
            ffname = ffname.Replace("|", "_");
            ffname = ffname.Replace(" ", "_");


            String fname = _tmppath+@"\"+ffname;
            log.Info("Making dump for " + _j.Name + " to " + ffname);
            String md = Options.Get().MySqlDumpPath;
            if (md == "")
            {
                log.Error("Mysqldump not specified "+md);
                return;
            }
            String pr = String.Format("{0:s} {1:s} {2:s} {3:s} --ignore-table={0:s}.allrabbits",db.DBName,(db.Host==""?"":"-h "+db.Host),
                (db.User==""?"":"-u "+db.User),(db.Password==""?"":"--password="+db.Password));
            try
            {

                ProcessStartInfo inf = new ProcessStartInfo(md, pr);
                
                inf.UseShellExecute = false;
                inf.RedirectStandardOutput = true;
                inf.CreateNoWindow = true;
                inf.StandardOutputEncoding = Encoding.UTF8;

                Process p = Process.Start(inf);
                TextWriter wr=new StreamWriter(fname+".dump",false,Encoding.UTF8);
                wr.Write(p.StandardOutput.ReadToEnd());
                p.WaitForExit();
                wr.Close();
                if (p.ExitCode != 0)
                    throw new ApplicationException("mysqldump run result is " + p.ExitCode.ToString());
                p.Close();
            }
            catch(Exception ex)
            {
                log.Error("Error while "+md+":"+ex.GetType().ToString()+":"+ex.Message);
                try
                {
                    File.Delete(fname + ".dump");
                }
                catch (Exception ex2)
                {
                    log.Error("Error while " + md + ":" + ex.GetType().ToString() + ":" + ex.Message);
                    return;
                }
                return;
            }
            bool is7z=false;
            md = Options.Get().Path7Z;
            if (md == "")
                log.Warn("7z not specified");
            else
                try
                {
                    ProcessStartInfo inf = new ProcessStartInfo(md,string.Format(" a -mx9 -p{0} \"{1}.7z\" \"{2}.dump\"", Password, fname, fname));

                    inf.CreateNoWindow = true;
                    inf.RedirectStandardOutput = true;
                    inf.UseShellExecute = false;

                    Process p = Process.Start(inf);
                    p.WaitForExit();
                    if (p.ExitCode!=0)
                        throw new ApplicationException("7z run result is "+p.ExitCode.ToString());
                    File.Delete(fname + ".dump");
                    is7z = true;
                }
                catch (Exception ex)
                {
                    log.Error("Error while "+md+":"+ex.GetType().ToString()+":"+ex.Message);
                    //return;
                }
            log.Debug("copy " + fname + (is7z ? ".7z" : ".dump")+" to "+_j.BackupPath + "\\" + ffname + (is7z ? ".7z" : ".dump"));
            File.Move(fname + (is7z ? ".7z" : ".dump"), _j.BackupPath + "\\" + ffname + (is7z ? ".7z" : ".dump"));
            CheckLimits();
        }
        public void Run()
        {
            log.Debug("Making dump for "+_j.Name);
            if (_j.DB == DataBase.AllDataBases)
                foreach (DataBase db in Options.Get().Databases)
                    DumpDB(db);
            else
                DumpDB(_j.DB);
            _jobber.OnEndJob();
        }

        public void OnEndJob()
        {
            _j.Busy = false;
            _j.LastWork = DateTime.Now;
            log.Debug("End of making dump "+_j.Name);
        }

        public static void MakeJob(ArchiveJob j)
        {
            ArchiveJobThread jt = new ArchiveJobThread(j);
            j.Busy = true;
            Thread t = new Thread(jt.Run);
            t.Start();
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

        public static void UndumpDB(string host,string db,string user,string password,string file)
        {
            log.Debug("undumping " + file + " to " + host + ":" + db + ":" + user + ":" + password);
            String tmppath = Path.GetTempPath();
            //String pth=Path.GetDirectoryName(file);
            String fl=Path.GetFileName(file);
            String ext=Path.GetExtension(file);
            String f = tmppath + fl;
            log.Debug("copy "+file+" to "+f);
            if (!Directory.Exists(tmppath))
            {
                Directory.CreateDirectory(tmppath);
            }
            File.Copy(file,f,true);
            if (ext == ".7z")
            {
                log.Debug("decompress 7z");
                String z7 = Options.Get().Path7Z;
                String ff = tmppath + Path.GetFileNameWithoutExtension(f) + ".dump";
                if (z7 == "")
                {
                    throw new ApplicationException("7z not specified");
                }
                ProcessStartInfo inf = new ProcessStartInfo(z7, " e -p" + Password + " \"" + f + "\"");

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
                    throw new ApplicationException("7z run result is " + p.ExitCode.ToString());
                }
                f = ff;
            }
            log.Debug("mysql");
            String sql = Options.Get().MySqlPath;
            if (sql == "")
                throw new ApplicationException("mysql not specified");
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
                byte[] buf=new byte[rd.Length];
                rd.Read(buf, 0, (int)rd.Length);
                rd.Close();
                byte[] b2 = buf;// Encoding.Convert(Encoding.UTF8, Encoding.ASCII, buf);
                mp.StandardInput.BaseStream.Write(b2, 0, b2.Length);
                mp.StandardInput.Close();
                String mout = mp.StandardError.ReadToEnd();
                mp.WaitForExit();
                int res=mp.ExitCode;
                mp.Close();
                if (res != 0 || mout!="")
                    throw new ApplicationException("mysql exit code="+res.ToString()+"\nerror="+mout);
            }
            catch(Exception ex)
            {
                File.Delete(f);
                throw ex;
            }
            File.Delete(f);
        }
    }
}
