using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabVac
    {
        public readonly int vid;
        public readonly DateTime date;
        public readonly string name;
        public readonly int remains;
        public bool unabled;

        public RabVac(int vid, DateTime dateTime, string nm, int remains, bool un)
        {
            this.vid = vid;
            this.date = dateTime;
            this.name = nm;
            this.remains = remains;
            this.unabled = un;
        }
    }
}
