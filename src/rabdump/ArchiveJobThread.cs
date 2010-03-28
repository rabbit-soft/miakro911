using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using log4net;
using System.IO;

namespace rabdump
{
    class ArchiveJobThread
    {
        private const String PASSWORD="ns471lbNITfq3";
        private ArchiveJob j=null;
        private ArchiveJobThread jobber=null;
        private readonly ILog log = LogManager.GetLogger(typeof(ArchiveJobThread));
        public ArchiveJobThread(ArchiveJob job)
        {
            j = job;
            j.busy = true;
            jobber = this;
        }

        public void dumpdb(DataBase db)
        {
            String fname = j.BackupPath+"\\"+j.Name + "_" + db.Name + "_" + DateTime.Now.ToString("yyyy_MM_dd_HHmmss");
            log.Info("Making dump for " + j.Name + " to " + fname);
            String md = Options.get().MySqlDumpPath;
            if (md == "")
            {
                log.Error("Mysqldump not specified "+md);
                return;
            }
            String pr = String.Format("{0:s} {1:s} {2:s} {3:s} --ignore-table={0:s}.allrabbits",db.DBName,(db.Host==""?"":"-h "+db.Host),
                (db.User==""?"":"-u "+db.User),(db.Password==""?"":"--password="+db.Password));
            try
            {
                
                ProcessStartInfo inf=new ProcessStartInfo(md,pr);
                inf.UseShellExecute = false;
                inf.RedirectStandardOutput=true;
                inf.CreateNoWindow=true;
                inf.StandardOutputEncoding = Encoding.UTF8;
                Process p = Process.Start(inf);
                TextWriter wr=new StreamWriter(fname+".dump",false,Encoding.UTF8);
                wr.Write(p.StandardOutput.ReadToEnd());
                p.WaitForExit();
                wr.Close();
                if (p.ExitCode != 0)
                    throw new ApplicationException("run result is " + p.ExitCode.ToString());
                p.Close();
            }
            catch(Exception ex)
            {
                log.Error("Error while "+md+":"+ex.GetType().ToString()+":"+ex.Message);
                File.Delete(fname+".dump");
                return;
            }
            md = Options.get().Path7Z;
            if (md == "")
                log.Warn("7z not specified");
            else
                try
                {//
                    ProcessStartInfo inf = new ProcessStartInfo(md, " a -mx9 -p" + PASSWORD + " \"" + fname + ".7z\" \"" + fname + ".dump\"");
                    inf.CreateNoWindow = true;
                    inf.RedirectStandardOutput = true;
                    inf.UseShellExecute = false;
                    Process p = Process.Start(inf);
                    p.WaitForExit();
                    if (p.ExitCode!=0)
                        throw new ApplicationException("run result is "+p.ExitCode.ToString());
                    File.Delete(fname + ".dump");
                }
                catch (Exception ex)
                {
                    log.Error("Error while "+md+":"+ex.GetType().ToString()+":"+ex.Message);
                    return;
                }
        }
        public void run()
        {
            log.Debug("Making dump for "+j.Name);
            if (j.DB == DataBase.AllDataBases)
                foreach (DataBase db in Options.get().Databases)
                    dumpdb(db);
            else
                dumpdb(j.DB);
            jobber.onEndJob();
        }

        public void onEndJob()
        {
            j.busy = false;
            j.lastWork = DateTime.Now;
            log.Debug("End of making dump "+j.Name);
        }

        public static void MakeJob(ArchiveJob j)
        {
            ArchiveJobThread jt = new ArchiveJobThread(j);
            j.busy = true;
            Thread t = new Thread(jt.run);
            t.Start();
        }
    }
}
