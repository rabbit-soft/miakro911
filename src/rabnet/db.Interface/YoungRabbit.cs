using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class YoungRabbit : Rabbit, IData
    {
        public readonly int ParentId;
        public readonly string ParentName;
        public readonly int Neighbours;

        public YoungRabbit(int id, string name, string sex, DateTime born, String breedname, int group, String bon, string rawAddress, String notes, int pid, string pname, int nbrs)
            : base(id, name, sex, born, breedname, group, bon, rawAddress, notes)
        {
            ParentId = pid;
            ParentName = pname;
            Neighbours = nbrs;
        }
    }
}
