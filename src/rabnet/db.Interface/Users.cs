using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class sUser
    {
        public int Id;
        public string Name;
        public string Group;

        public sUser() { }

        public sUser(int UserID, string UserName, string UserGroup)
        {
            this.Id = UserID;
            this.Name = UserName;
            this.Group = UserGroup;
        }

        public string GetRusUserName()
        {
            switch (Group)
            {
                case Admin: return "Администратор";
                case Zootech: return "Зоотехник";
                case Butcher: return "Упаковщик";
                default: return "Рабочий";
            }
        }

        /*
         * Список типов юзеров, которые могут быть в базе.
         * 
         * Если хочешь добавить новый тип пользователя,
         * то в начале нужно внести коррективы в таблицу БД 
         * Users - поле u_group, т.к. оно ENUM
         */
        public const string Worker = "worker";

        public const string Admin = "admin";//0
        public const string Zootech = "zootech";//1    
        public const string Butcher = "butcher";//2
    }
}
