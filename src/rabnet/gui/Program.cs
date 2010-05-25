//#define GLOBCATCH
//#define PROTECTED

using System;
using System.Windows.Forms;
using log4net;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace rabnet
{
    static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd,int mode);


        static void SwitchRabWindow()
        {
            Process cp = Process.GetCurrentProcess();
            foreach(Process p in Process.GetProcessesByName(cp.ProcessName))
                if (p.Id != cp.Id)
                {
                    SetForegroundWindow(p.MainWindowHandle);
                    ShowWindow(p.MainWindowHandle, 3);
                }
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool new_instance;
            using(System.Threading.Mutex mutex=new System.Threading.Mutex(true,"RabNetApplication",out new_instance))
            {
                if (new_instance)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    ILog log = LogManager.GetLogger(typeof(Program));
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
#if PROTECTED
            bool exit = true;
            do
            {
                exit = true;
                int hkey = 0;
                while (PClient.get().farms() == -1 && hkey == 0)
                    if (MessageBox.Show(null, "Ключ защиты не найден!\nВставьте ключ защиты в компьютер с БД и нажмите кнопку повтор.",
                            "Ключ защиты", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Cancel) hkey = -1;
                if (PClient.get().farms() == -1)
                    return;
                //MessageBox.Show(String.Format("HAS {0:d} FARMS",PClient.get().farms()));
#endif
#if (GLOBCATCH)
            try
            {
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
                    if (PClient.get().farms() == -1)
                        LoginForm.stop = true;
#endif
                    }
#if (GLOBCATCH)
            }
                        catch (Exception ex)
                        {
                            log.Fatal("General fault exception", ex);
                            throw ex;
                        }
#endif
#if PROTECTED
                if (PClient.get().farms() == -1)
                    exit = false;
            } while (!exit);
#endif
                }//new_instance
                else
                {
                    SwitchRabWindow();
                }
        }//using
         }//main()
    }//class
}//namespace
