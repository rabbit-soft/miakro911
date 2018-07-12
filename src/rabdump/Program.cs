//#define PROTECTED
#if DEBUG
//#define NOCATCH
#endif
using System;
using System.Windows.Forms;
using System.Threading;
using log4net;

namespace rabdump
{
    static class Program
    {
        private const string MUTEX_NAME = "RabDumpApplication";
        private static ILog _logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            _logger = LogManager.GetLogger(typeof(Program).Name);
            _logger.Info("----- Application Starts -----");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU", false);
            bool new_instance;
            using (Mutex mutex = new Mutex(true, MUTEX_NAME, out new_instance)) {
                if (new_instance) {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

#if !NOCATCH
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Unhandled);
                    Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Threaded);
                    try {
#endif
                            Application.Run(new MainForm());
#if !NOCATCH
                    } catch (Exception e) {
                        _logger.Error("<exp>", e);
                        MessageBox.Show(e.Message + e.StackTrace);
                    }
#endif
                }
                //else MessageBox.Show("Программа уже запущена");
                Environment.Exit(0);
            }
        }

        internal static void ReleaseMutex()
        {
            Mutex m = Mutex.OpenExisting(MUTEX_NAME);
            m.ReleaseMutex();
        }


#if !NOCATCH
        private static void Excepted(Exception ex)
        {
            _logger.Fatal(ex);
            string msg = "Произошла ошибка. Программа будет закрыта.\n\r" + ex.Message;
            if (ex.Source == "MySql.Data") {
                msg = "Соединение с MySQL-сервером было разорвано.\n\rПрграмма будет закрыта";
            } else if (ex is UnauthorizedAccessException) {
                msg = "Произошла ошибка доступа" + Environment.NewLine +
                    "Программу необходимо запустить от Имени администратора";
            }
            MessageBox.Show(msg, "Серьезная ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void Threaded(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try {
                Excepted(e.Exception);
            } finally {
                //Application.Exit();
                //Environment.Exit(1);
            }
        }

        private static void Unhandled(object sender, UnhandledExceptionEventArgs e)
        {
            try {
                Excepted((Exception)e.ExceptionObject);
            } finally {
                //Application.Exit();
                //Environment.Exit(1);
            }
        }
#endif
    }
}
