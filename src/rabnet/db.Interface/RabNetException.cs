using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class FarmCountOverdarwException : Exception
    {
        public FarmCountOverdarwException() : base("Достигнуту максимально-допустимое количество ферм.") { }
    }
}
