using System;
using System.Collections.Generic;
using System.Text;

namespace pEngine
{
    public class User
    {
        public readonly string Name;      
        public readonly int Id;
        public readonly string Parent;
        public bool Block;
        //internal bool Default;
        internal byte[] Key;
        //private Permissions _permis = null;
        private string _password;

        internal User(int id, string name, byte[] key) //TODO с ключем еще ничего не понятно
        {
            this.Id = id;
            this.Name = name;
            this.Key = key;
        }
        internal User(int id, string name, byte[] key,string password):this(id,name,key)
        {
            _password = password;
        }
        public User(string id, string name,string parent,bool block/*, sPermission[] perms*/)
        {
            this.Id = int.Parse(id);
            this.Name = name;
            this.Parent = parent;
            this.Block = block;
            //SetPermissions(perms);
        }

        /// <summary>
        /// Разрещения Данного пользователя
        /// </summary>
        /*public Permissions Permiss
        {
            get
            {
                if (_permis == null)
                    _permis = new Permissions();
                return _permis;
            }
        }*/

        /// <summary>
        /// Устанавливает разрешения для данного пользователя
        /// </summary>
        /*public void SetPermissions(sPermission[] per)
        {            
            _permis = new Permissions(per);
        }*/

        /// <summary>
        /// Проверяет правильный ли пароль
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns>Правильный ли пароль</returns>
        public bool CheckPassword(string password)
        {
            return password == _password;
        }

        public void SetKey(byte[] key)
        {
            this.Key = key;
        }

        public void SetPassword(string pass)
        {
            _password = pass;
        }
    } 

}
