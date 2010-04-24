//#define PROTECTED
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace rabdump
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if PROTECTED
            bool exit = true;
            do
            {
                exit = true;
                int end = 0;
                while (!pserver.haskey() && end == 0)
                {
                    if (MessageBox.Show(null, "Ключ защиты не найден!\nВставьте ключ защиты и нажмите кнопку повтор.",
                        "Ключ защиты", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                        end = 1;
                }
                if (!pserver.haskey())
                    return;
                pserver svr = new pserver();
#endif
                Application.Run(new MainForm());
#if PROTECTED
                svr.release();
                if (!pserver.haskey())
                    exit = false;
            } while (!exit);
#endif
        }
    }
}
