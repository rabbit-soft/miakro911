#if DEBUG
#define NOCATCH
#endif

using System;
using System.Windows.Forms;
using log4net;
using System.Diagnostics;
using System.Runtime.InteropServices;
#if PROTECTED
using RabGRD;
#endif

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

        static ILog log = null;

        static void SwitchRabWindow()
        {
            Process cp = Process.GetCurrentProcess();
            foreach (Process p in Process.GetProcessesByName(cp.ProcessName))
                if (p.Id != cp.Id)
                {
                    SetForegroundWindow(p.MainWindowHandle);
                    ShowWindow(p.MainWindowHandle, 5);
                }
        }



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool new_instance;
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, "RabNetApplication", out new_instance))
            {
                if (new_instance)
                {
#if !NOCATCH
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Unhandled);
                    Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Threaded);
#endif
                    log4net.Config.XmlConfigurator.Configure();
                    log = LogManager.GetLogger(typeof(Program));
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
#if PROTECTED
                    bool exit = true;
                    do
                    {
                        exit = true;
                        int hkey = 0;

                        //while (PClient.get().farms() == -1 && hkey == 0)
                        while (GRD_Base.Instance.GetFarmsCnt() == -1 && hkey == 0)
                        {
                            if (MessageBox.Show(null, "Ключ защиты не найден!\nВставьте ключ защиты в компьютер и нажмите кнопку повтор.",
                                    "Ключ защиты", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                            {
                                hkey = -1;
                            }
                        }
                        //              if (PClient.get().farms() == -1)
                        if (GRD_Base.Instance.GetFarmsCnt() == -1)
                        {
                            return;
                        }
                        if (!GRD_Base.Instance.GetFlag(GRD_Base.FlagType.RabNet))
                        {
                            MessageBox.Show("Программа не может работать с данным ключом защиты");
                            return;
                        }
                        //MessageBox.Show(String.Format("HAS {0:d} FARMS",PClient.get().farms()));
#endif
                        bool dbedit = false;
                        if (args.Length > 0 && args[0] == "dbedit")
                            dbedit = true;
                        LoginForm lf = new LoginForm(dbedit);
                        LoginForm.stop = false;
                        while (!LoginForm.stop)
                        {
                            LoginForm.stop = true;
                            if (lf.ShowDialog() == DialogResult.OK)
                            {
                                Application.Run(new MainForm());
                            }
#if PROTECTED
                            //                        if (PClient.get().farms() == -1)
                            if (GRD_Base.Instance.GetFarmsCnt() == -1)
                            {
                                LoginForm.stop = true;
                            }
#endif
                        }
#if PROTECTED
                        //                    if (PClient.get().farms() == -1)
                        if (GRD_Base.Instance.GetFarmsCnt() == -1)
                        {
                            exit = false;
                        }
                    }
                    while (!exit);
                    
#endif
                }//new_instance
                else
                {
                    SwitchRabWindow();
                }
            }//using
        }

#if !NOCATCH
        static void Excepted(Exception ex)
        {
            if (log != null)
                log.Fatal("General fault exception", ex);
            if (ex.Source == "MySql.Data")
            {
                MessageBox.Show("Соединение с сервером было разорвано.\n\rПрграмма будет закрыта");
            }
            else
            {
                MessageBox.Show("Произошла ошибка. Программа будет закрыта.\n\r" + ex.Message);
            }
            log.Debug(ex.Message, ex);
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

    }//class
}//namespace
