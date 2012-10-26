using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabName : IData
    {
        public int id;
        public string name;
        public string surname;
        public int use;
        public DateTime td;
        public String sex;
        public RabName(int id, String name, String surname, String sex, int use, DateTime dt)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            //this.sex = sex;
            if (sex == "male")
                this.sex = "м";
            else
                this.sex = "ж";
            this.use = use;
            this.td = dt;
        }
    }
}
