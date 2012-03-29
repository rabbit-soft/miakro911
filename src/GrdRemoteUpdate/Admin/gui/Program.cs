using System;
using System.Collections.Generic;
using System.Windows.Forms;
using pEngine;

namespace AdminGRD
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
                string msg = "1";
                if (lf.ShowDialog() == DialogResult.OK)
                {
                    msg = Engine.LogIn(lf.SelectedFile, lf.Password, lf.Server, lf.NewPassword);
                    if (msg == "")
                    {
                        MainForm mf = new MainForm();
                        Application.Run(mf);
                        Engine.LogOut();
                        if (!mf.Retry)
                            break;
                    }
                    else
                    {
                        MessageBox.Show(msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        lf.FocusPassword();
                    }
                }
                else break;
            }
        }

        private static bool alredyRun()
        {
            System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName("pAdmin");
            return p.Length > 1;
        }

        //TODO Глобальный отлов Эксепшнов
    }

    public class MyException : Exception
    {
        public MyException(string message) : base(message) { }
    }
}
