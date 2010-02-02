using System;
using System.Collections.Generic;
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
            String file ="";
            String host = "";
            String user = "";
            String pswd = "";
            String db = "";
            String root = "";
            String rpswd = "";
            String users = "";
            String scr = "";
            bool auto = false;
            Environment.ExitCode = 0;
            if (args.Length >= 1)
            {
                if (args.Length<3)
                {
                    MessageBox.Show(usage());
                    Environment.ExitCode = 1;
                    return;
                }
                file = args[0];
                if (file == "nudb")
                {
                    nudb(args);
                    return;
                }
                if (file == "dropdb")
                {
                    dropdb(args);
                    return;
                }
                if (!System.IO.File.Exists(file))
                {
                    MessageBox.Show("Mia File Not exists "+file+"\r\n"+usage());
                    Environment.ExitCode = 1;
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
                }catch(Exception ex)
                {
                    MessageBox.Show("Error:" + ex.ToString() + ":" + ex.Message + "\r\n" + usage());
                    Environment.ExitCode = 1;
                    return;
                }
                users = args[2];
                String[] us=users.Split(';');
                {
                    if (us.Length < 2)
                    {
                        MessageBox.Show("expected one user\r\n"+usage());
                        Environment.ExitCode = 1;
                        return;
                    }
                    if (us.Length % 2 != 0)
                    {
                        MessageBox.Show("Every user must have password\r\n" + usage());
                        Environment.ExitCode = 2;
                        return;
                    }
                }
                if (args.Length>3)
                    scr=args[3];
                auto = true;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(auto,file,host,db,user,pswd,root,rpswd,users,scr));
        }

        static void nudb(string[] args)
        {
            String[] dbpar = args[1].Split(';');
                string host = dbpar[0];
                string db = dbpar[1];
                string user = dbpar[2];
                string pswd = dbpar[3];
                string root = dbpar[4];
                string rpswd = dbpar[5];
            Environment.ExitCode = 0;
            String[] us = args[2].Split(';');
            if (us.Length < 2 || us.Length % 2 != 0)
            {
                Environment.ExitCode = 1;
                return;
            }
            if (MDCreator.hasDB(root, rpswd, db, host))
                Environment.ExitCode = 1;
            MDCreator cr = new MDCreator(null);
            try
            {
                cr.prepare(true, host, user, pswd, db, root, rpswd, true);
                cr.setUsers(us);
            }
            catch (Exception)
            {
                Environment.ExitCode = 1;
            }
            return;
        }

        static void dropdb(string[] args)
        {
            Environment.ExitCode=0;
            String[] dbpar = args[1].Split(';');
            string host = dbpar[0];
            string db = dbpar[1];
            string root = dbpar[4];
            string rpswd = dbpar[5];
            try
            {
                MDCreator.DropDb(root, rpswd, db, host);
            }catch(Exception)
            {
                Environment.ExitCode=1;
            }
            return;
        }
    }
}