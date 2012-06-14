#if DEBUG
    //#define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using log4net;
using pEngine;

namespace AdminGRD
{
    static class Program
    {
        static ILog _log;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log = LogManager.GetLogger("AdminGrd");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (alredyRun())
            {
                MessageBox.Show("Программа уже запущена");
                return;
            }
            while (true)
            {
                LoginFrom lf = new LoginFrom();
                if (lf.ShowDialog() == DialogResult.OK)
                {                
                        MainForm mf = new MainForm();
                        Application.Run(mf);
                        Engine.LogOut();
                        if (!mf.Retry)
                            break;
                }
                else break;
            }
        }

        private static bool alredyRun()
        {
            System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName("pAdmin");
            return p.Length > 1;
        }

#if !NOCATCH
        static void Excepted(Exception ex)
        {
            if (ex.Source == "MySql.Data")
            {
                MessageBox.Show("Соединение с сервером было разорвано.\n\rПрграмма будет закрыта");
            }
            else
            {
                MessageBox.Show("Произошла ошибка. Программа будет закрыта.\n\r" + ex.Message);
            }
            _log.Fatal(ex.Message, ex);
        }

        static void Threaded(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                Excepted(e.Exception);
            }
            finally
            {
                //Application.Exit();
                Environment.Exit(0);
            }
        }

        static void Unhandled(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Excepted((Exception)e.ExceptionObject);
            }
            finally
            {
                //Application.Exit();
                Environment.Exit(0);
            }
        }
#endif


    }

    public class MyException : Exception
    {
        public MyException(string message) : base(message) { }
    }
}
