using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class Client
    {
        public readonly int ID;
        public readonly string Name;
        public readonly string Address;

        public Client(int id,string name,string adr)
        {
            this.ID = id;
            this.Name = name;
            this.Address = adr;
        }
    }

    public class ClientsList:List<Client>
    {
        public bool ContainsID(int cId)
        {
            foreach(Client c in this)
            {
                if(c.ID ==cId)
                    return true;
            }
            return false;
        }

        public string GetName(int cId)
        {
            foreach (Client c in this)
            {
                if (c.ID == cId)
                    return c.Name;
            }
            return "";
        }
    }
}
