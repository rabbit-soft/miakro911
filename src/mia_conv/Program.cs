using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mia_conv
{
    static class Program
    {

        static String usage()
        {
            return @"usage: mia_conv file.mia dbparams users
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
            bool auto = false;
            if (args.Length >= 1)
            {
                if (args.Length<3)
                {
                    MessageBox.Show(usage());
                    return;
                }
                file = args[0];
                if (!System.IO.File.Exists(file))
                {
                    MessageBox.Show("Mia File Not exists "+file+"\r\n"+usage());
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
                    return;
                }
                users = args[2];
                String[] us=users.Split(';');
                {
                    if (us.Length < 2)
                    {
                        MessageBox.Show("expected one user\r\n"+usage());
                        return;
                    }
                    if (us.Length % 2 != 0)
                    {
                        MessageBox.Show("Every user must have password\r\n" + usage());
                        return;
                    }
                }
                auto = true;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(auto,file,host,db,user,pswd,root,rpswd,users));
        }
    }
}