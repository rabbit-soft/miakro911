using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet.RNC
{
    /// <summary>
    /// Представлет собой Параметры с подключениями к БД
    /// </summary>
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class sParams
    {
        /// <summary>
        /// Адресс БД
        /// </summary>
        public readonly string Host;
        /// <summary>
        /// Имя Базы Данных
        /// </summary>
        public readonly string DataBase;
        /// <summary>
        /// Пользователь, который подключается к БД
        /// </summary>
        public readonly string User;
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public readonly string Password;
        public readonly string Charset = "utf8";

        public sParams(string host, string database, string user, string pwd)
        {
            this.Host = host;
            this.DataBase = database;
            this.User = user;
            this.Password = pwd;
        }

        public sParams(string connectionString)
        {
            string[] prms = connectionString.Split(';');
            foreach (string pair in prms)
            {
                switch (pair.Split('=')[0])
                {
                    case "host": this.Host = pair.Split('=')[1]; break;
                    case "database": this.DataBase = pair.Split('=')[1]; break;
                    case "uid": this.User = pair.Split('=')[1]; break;
                    case "pwd": this.Password = pair.Split('=')[1]; break;
                }
            }
            /*this.Host = str[0].Split('=')[1];
            this.User = str[1].Split('=')[1];
            this.Password = str[2].Split('=')[1];
            this.DataBase = str[3].Split('=')[1];  
            host=localhost;database=krol;uid=kroliki3;pwd=kroliki   */
        }

        public override string ToString()
        {
            return String.Format("host={0};database={1};uid={2};pwd={3};charset={4}", 
                Host, 
                DataBase, 
                User, 
                string.IsNullOrEmpty(Password) ? "''": Password, 
                Charset);
        }

    }
}
