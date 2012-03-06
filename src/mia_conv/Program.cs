using System;
using System.Windows.Forms;

namespace mia_conv
{
    
    static class Program
    {
        static String usage()
        {
            return @"usage: mia_conv file.mia dbparams users [script_file]
dbparams: host;database;user;password;[root;root_password] - use root;root_password to create new database
users: user1;password1[;user2;password2[;user3;passowrd3...]] - create users
";
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            String file ="";
            String host = "";
            String user = "";
            String pswd = "";
            String db = "";
            String root = "";
            String rpswd = "";
            String users = "";
            String scr = "";
            int auto = 0;
            Environment.ExitCode = miaExitCode.OK;

            if (args.Length >= 1)
            {
                if (args.Length<3)
                {
                    MessageBox.Show(usage());
                    Environment.ExitCode = miaExitCode.ERROR;
                    return;
                }
                file = args[0];
                if (file == "nudb")
                {
                    Environment.ExitCode = nudb(args);
                    return;
                }
                if (file == "dropdb")
                {
                    Environment.ExitCode = dropdb(args);
                    return;
                }
                if (!System.IO.File.Exists(file))
                {
                    MessageBox.Show("Mia File Not exists "+file+"\r\n"+usage());
                    Environment.ExitCode = miaExitCode.FILE_NOT_EXISTS;
                    return;
                }
                String[] dbpar = args[1].Split(';');
                try
                {
                    host = dbpar[0];
                    db = dbpar[1];
                    user = dbpar[2];
                    pswd = dbpar[3];
                    if (dbpar.Length > 4)
                    {
                        root = dbpar[4];
                        rpswd = dbpar[5];
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error:" + ex.ToString() + ":" + ex.Message + "\r\n" + usage());
                    Environment.ExitCode = miaExitCode.ERROR;
                    return;
                }
                users = args[2];
                String[] us = users.Split(';');
                {
                    if (us.Length < 2)
                    {
                        MessageBox.Show("expected one user\r\n"+usage());
                        Environment.ExitCode = miaExitCode.EXPECTED_ONE_USER;
                        return;
                    }
                    if (us.Length % 2 != 0)
                    {
                        MessageBox.Show("Every user must have password\r\n" + usage());
                        Environment.ExitCode = miaExitCode.USER_MUST_HAVE_PASSWORD;
                        return;
                    }
                }
                if (args.Length>3)
                    scr=args[3];
                auto = 1;
                if (args.Length > 4 && args[4] == "quiet")
                    auto = 2;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(auto,file,host,db,user,pswd,root,rpswd,users,scr));
        }

        /// <summary>
        /// Новая База данных
        /// </summary>
        /// <returns>miaExitCode</returns>
        static int nudb(string[] args)
        {
            String[] dbpar = args[1].Split(';');
                string host = dbpar[0];
                string db = dbpar[1];
                string user = dbpar[2];
                string pswd = dbpar[3];
                string root = dbpar[4];
                string rpswd = dbpar[5];
            Environment.ExitCode = miaExitCode.OK;
            String[] us = args[2].Split(';');
            if (us.Length < 2)           
                return miaExitCode.EXPECTED_ONE_USER;            
            if (us.Length % 2 != 0)           
                return miaExitCode.USER_MUST_HAVE_PASSWORD;          
            if (MDCreator.HasDB(root, rpswd, db, host))           
                return miaExitCode.DB_ALREADY_EXISTS;            

            MDCreator cr = new MDCreator();
            try
            {
                cr.Prepare(true, host, user, pswd, db, root, rpswd, true);
                cr.SetUsers(us);
            }
            catch (Exception)
            {
                return miaExitCode.ERROR;
            }
            return miaExitCode.OK;
        }

        /// <summary>
        /// Удалить БД
        /// </summary>
        /// <returns>miaExitCode</returns>
        static int dropdb(string[] args)
        {
            Environment.ExitCode = miaExitCode.OK;
            String[] dbpar = args[1].Split(';');
            string host = dbpar[0];
            string db = dbpar[1];
            string root = dbpar[4];
            string rpswd = dbpar[5];
            try
            {
                MDCreator.DropDb(root, rpswd, db, host);
            }
            catch(Exception)
            {
                return miaExitCode.ERROR;
            }
            return miaExitCode.OK;
        }
    }
}