using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                LoginForm lf = new LoginForm();
                if (lf.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm());
                }
            }
            catch (Exception ex)
            {
                log.Fatal("General fault exception", ex);
                throw ex;
            }
        }
    }
}
