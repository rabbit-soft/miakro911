#if DEBUG
#define NOCATCH
#define ONLYONE
#endif

using System;
using System.Windows.Forms;
using log4net;
using System.Diagnostics;
using System.Runtime.InteropServices;
using rabnet.forms;

namespace rabnet
{
    static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int mode);

        static ILog _logger = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            _logger = LogManager.GetLogger(typeof(Program));
            _logger.Info("----- Application Starts -----");
#if !ONLYONE
            bool new_instance;
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, "RabNetApplication", out new_instance)) {

                if (new_instance) {
#endif
#if !NOCATCH
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Unhandled);
                    Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Threaded);
#endif
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                        LoginForm lf = new LoginForm();
                        LoginForm.stop = false;
                        while (!LoginForm.stop) {
                            LoginForm.stop = true;
                            if (lf.ShowDialog() == DialogResult.OK) {
                                Application.Run(new MainForm());
                            }
                        }
#if !ONLYONE
                } else {   //new_instance
                    SwitchRabWindow();
                }
            }//using
#endif
        }

#if !NOCATCH
        static void Excepted(Exception ex)
        {
            _logger.Fatal(ex.Message, ex);
            if (ex.Source == "MySql.Data") {
                MessageBox.Show("Ошибка БД: " + ex.Message + Environment.NewLine + "Программа будет закрыта");
                Environment.Exit(0);
            } else {
                MessageBox.Show("Произошла необработанная ошибка." + Environment.NewLine + ex.Message);
            }
        }

        static void Threaded(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try {
                Excepted(e.Exception);
            } finally {
                //Environment.Exit(0);
            }
        }

        static void Unhandled(object sender, UnhandledExceptionEventArgs e)
        {
            try {
                Excepted((Exception)e.ExceptionObject);
            } finally {
                //Environment.Exit(0);
            }
        }
#endif
        static void SwitchRabWindow()
        {
            Process cp = Process.GetCurrentProcess();
            foreach (Process p in Process.GetProcessesByName(cp.ProcessName))
                if (p.Id != cp.Id) {
                    SetForegroundWindow(p.MainWindowHandle);
                    ShowWindow(p.MainWindowHandle, 5);
                }
        }

    }//class
}//namespace
