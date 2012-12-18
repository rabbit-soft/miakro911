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
        public DBBadVersionException(int need, int has)
            : base(String.Format(@"Не верная версия базы данных {0:d}.
Требуется версия {1:d}.
Обновите программу и базу данных до последних версий.", has, need)) { }
    }
}
