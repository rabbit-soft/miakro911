//#define PROTECTED
#if DEBUG
    //#define NOCATCH
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
                            Application.Run(new MainForm());
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
            }
        }
    }
}
