using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet.RNC
{
   
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class DataSource
    {
        /// <summary>
        /// Уникальный номер Настройки. Нужен чтобы определять папку в реестре
        /// </summary>
        public string Guid = "";
        /// <summary>
        /// Отображаемое имя Базы Данных
        /// </summary>
        public String Name;
        /// <summary>
        /// Тип базы данных.Постоянно 'db.mysql'.
        /// </summary>
        public String Type = "db.mysql";
        /// <summary>
        /// Параметры подключения
        /// </summary>
        public sParams Params;
        /// <summary>
        /// Отображать ли БД в выпадающем списке
        /// </summary>
        public bool Hidden = false;
        /// <summary>
        /// Выбирать ли базу данных по умолчанию
        /// </summary>
        public bool Default = false;
        public String DefUser = "";
        public String DefPassword = "";
        public bool SavePassword = false;
        /// <summary>
        /// отправлять ли ВебСтатистику
        /// </summary>
        public bool WebReport = false;

        protected DataSource() { }
        public DataSource(string guid, string name, string type, string param)
        {
            this.Guid = guid;
            this.Name = name;
            this.Type = type;
            this.Params = new sParams(param);
        }
        public DataSource(string guid, string name, string p_host, string p_db, string p_user, string p_password)
        {
            this.Guid = guid;
            this.Name = name;
            this.Params = new sParams(p_host, p_db, p_user, p_password);
        }

        public override string ToString()
        {
            return String.Format("name={0};hid={1};def={2};du={3};dp={4};sp={5}; params={6}",
                Name,
                Hidden ? "1" : "0",
                Default ? "1" : "0",
                DefUser,
                DefPassword,
                SavePassword ? "1" : "0",
                Params.ToString());
        }
    }
}
