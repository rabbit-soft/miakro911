//#define PROTECTED
#if DEBUG
    #define NOCATCH
#endif
using System;
using System.Windows.Forms;
using log4net;
#if PROTECTED
using RabGRD;
#endif

namespace rabdump
{
    static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program).Name);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU", false);
            bool new_instance;
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, "RabDumpApplication", out new_instance))
            {
                if (new_instance)
                {
#if !NOCATCH
                    try
                    {
#endif
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
#if !NOCATCH
                        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Unhandled);
                        Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Threaded);
#endif
#if PROTECTED
                    bool exit;
                    do
                    {
                        exit = true;
                        //                            int end = 0;
                        if (!GRD.Instance.ValidKey())
                        {
                            MessageBox.Show(null, "Ключ защиты не найден!\nРабота программы будет завершена!",
                                            "Ключ защиты", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                            //                                    end = 1;
                        }
                        //                            if (!GRD.Instance.ValidKey())
                        //{
                        //return;
                        //}
                        if (!GRD.Instance.GetFlag(GRD.FlagType.RabDump))
                        {
                            MessageBox.Show(null, "Данный ключ защиты не позволяет запуск приложения!\n",
                                            "Ключ защиты", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        //                pserver svr = new pserver();
#endif
                        Application.Run(new MainFormNew());
#if PROTECTED
                        //svr.release();
                        if (!GRD.Instance.ValidKey())
                        {
                            exit = false;
                        }
                    }
                    while (!exit);
#endif
#if !NOCATCH
                    }
                    catch (Exception e)
                    {
                        log.Error("<exp>", e);
                        MessageBox.Show(e.Message + e.StackTrace);
                    }
#endif
                }
                //else MessageBox.Show("Программа уже запущена");
            }
        }
#if !NOCATCH
        private static void Excepted(Exception ex)
        {
            log.Fatal(ex);
            string msg ="Произошла ошибка. Программа будет закрыта.\n\r" + ex.Message;
            if (ex.Source == "MySql.Data")            
                msg="Соединение с сервером было разорвано.\n\rПрграмма будет закрыта";
            MessageBox.Show(msg, "Серьезная ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);           
        }

        private static void Threaded(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                Excepted(e.Exception);
            }
            finally
            {
                //Application.Exit();
                Environment.Exit(1);
            }
        }

        private static void Unhandled(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Excepted((Exception)e.ExceptionObject);
            }
            finally
            {
                //Application.Exit();
                Environment.Exit(1);
            }
        }
#endif
    }
}
