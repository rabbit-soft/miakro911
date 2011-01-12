using System;
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
            //            try
            //            {
            bool batch = false;
            if (args.Length > 0)
            {
                batch = args[0] == "batch";
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            String flRabDump = Path.GetDirectoryName(Application.ExecutablePath) + @"\..\RabDump\rabdump.exe.config";
            String flRabNet = Path.GetDirectoryName(Application.ExecutablePath) + @"\..\RabNet\rabnet.exe.config";

            bool hasRabNet = File.Exists(flRabNet);
            bool hasRabDump = File.Exists(flRabDump);
            if (hasRabNet)
            {
                hasRabNet = InstallForm.TestRabNetConfig(flRabNet);
            }
            bool update = hasRabNet || hasRabDump;
            if (hasRabNet && !hasRabDump)
            {
                if(Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\..\RabNet"))
                {
                    update = false;
                }
            }
            int res = 0;
            if (update)
            {
                UpdateForm uf = new UpdateForm(flRabDump, flRabNet, batch);
                Application.Run(uf);
                res = uf.Result;
            }
            else
            {
                InstallForm ifr = new InstallForm(flRabDump, flRabNet, batch);
                Application.Run(ifr);
                res = ifr.Result;
            }
            Environment.ExitCode = res;
            //            }
            //            catch (Exception e)
            //            {
            //                MessageBox.Show(e.Message + e.StackTrace);
            //            }
        }
    }
}
