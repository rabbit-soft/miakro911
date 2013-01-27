using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetException : Exception
    {
        public RabNetException(string message) : base(message) { }
    }

    public class FarmCountOverdarwException : RabNetException
    {
        public FarmCountOverdarwException() : base("Достигнуту максимально-допустимое количество ферм.") { }
    }

    public class DBDriverNotFoudException : RabNetException
    {
        public DBDriverNotFoudException(String driver) : base("Database Driver " + driver + " not found!") { }
    }
    public class DBBadVersionException : RabNetException
    {
        public readonly int Has;
        public readonly int Need;

        public DBBadVersionException(int need, int has) 
            :base(need > has ? String.Format(@"Не верная версия Базы Данных {0:d}.{2:s}Требуется версия {1:d}.", has, need, Environment.NewLine)
                : String.Format(@"Необходимо обновить программу для работы с версией Базы Данных {0:d}.", has)) 
        {
            this.Has = has;
            this.Need = need;
            
        }

        public bool NeedDbUpdate
        {
            get { return Need > Has; }
        }
    }
}
