using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class FarmCountOverdarwException : Exception
    {
        public FarmCountOverdarwException() : base("Достигнуту максимально-допустимое количество ферм.") { }
    }

    public class DBDriverNotFoudException : Exception
    {
        public DBDriverNotFoudException(String driver) : base("Database Driver " + driver + " not found!") { }
    }
    public class DBBadVersionException : Exception
    {
        public DBBadVersionException(int need, int has)
            : base(String.Format(@"Не верная версия базы данных {0:d}.
Требуется версия {1:d}.
Обновите программу и базу данных до последних версий.", has, need)) { }
    }
}
