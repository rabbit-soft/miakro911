//#define GLOBCATCH

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using log4net;
using log4net.Config;


namespace rabnet
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger(typeof(Program));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if (GLOBCATCH)
            try
            {
#endif
                LoginForm lf = new LoginForm();
                LoginForm.stop = false;
                while (!LoginForm.stop)
                {
                    if (lf.ShowDialog() == DialogResult.OK)
                    {
                        LoginForm.stop = true;
                        Application.Run(new MainForm());
                    }
                }
#if (GLOBCATCH)
            }
                        catch (Exception ex)
                        {
                            log.Fatal("General fault exception", ex);
                            throw ex;
                        }
#endif
         }
    }
}
