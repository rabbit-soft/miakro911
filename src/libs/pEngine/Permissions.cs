using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

namespace pEngine
{
    public class Permissions
    {
        private List<sPermission> _permissions ;

        public Permissions(sPermission[] permiss)
        {
            Array.Sort(permiss);
            _permissions = new List<sPermission>(permiss);
        }
        public Permissions():this(new sPermission[0]){}

        public bool Contains(int pid)
        {
            return contains(pid.ToString());
        }
        public bool Contains(string nick)
        {
            foreach (sPermission p in _permissions)
                if (p.Nick == nick)
                    return true && contains(p.Parent);
            return false;
        }

        /// <summary>
        /// Возвращает массив имеющихся разрешений
        /// </summary>
        /// <remarks>Массив разрешений клонируется лдя того, 
        /// чтобы нельзя было изменить права пользователя извне</remarks>
        /// <returns></returns>
        public sPermission[] ToArray()
        {
            return _permissions.ToArray();   
            /*sPermission[] result = new sPermission[_permissions.Length];
            for(int i=0;i<_permissions.Length;i++)
                result[i] = (sPermission)_permissions[i].Clone();
            return result;*/
        }

        private bool contains(string pid)
        {
            if (pid == "0") return true;
            foreach (sPermission p in _permissions)
                if (p.PermID == pid)
                    return true && contains(p.Parent);
            return false; 
        }
        /// <summary>
        /// Добавляет разрешение в список доступных, если такое не имеется
        /// </summary>
        /// <param name="pms"></param>
        public void Add(sPermission pms)
        {
            if (!_permissions.Contains(pms))
                _permissions.Add(pms);
        }
        /// <summary>
        /// Удаляет разрешение из списка доступных, если такое имеется
        /// </summary>
        /// <param name="pms"></param>
        public void Delete(sPermission pms)
        {
            if (_permissions.Contains(pms))
                _permissions.Remove(pms);
        }

        public int Count { get { return _permissions.Count; } }
    }
}
