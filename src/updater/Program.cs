using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace updater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool batch = false;
            if (args.Length>0)
                batch = args[0] == "batch";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            String fl = Path.GetDirectoryName(Application.ExecutablePath) + "\\rabdump.exe.config";
            bool update = File.Exists(fl);
            //update = true;
            int res=0;
            if (update)
            {
                UpdateForm uf = new UpdateForm(fl,batch);
                Application.Run(uf);
                res = uf.result;
            }
            else
            {
                InstallForm ifr=new InstallForm(fl,batch);
                Application.Run(ifr);
                res = ifr.result;
            }
            Environment.ExitCode = res;
        }
    }
}
