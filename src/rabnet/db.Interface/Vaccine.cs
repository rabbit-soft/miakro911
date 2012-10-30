using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class Vaccine
    {
        public readonly int ID=0;
        public string Name;
        public int Duration = 0;
        public int Age = 0;
        public int After = 0;
        public bool Zoo = true;

        public Vaccine(int id,string name,int duration,int age,int after,bool zoo)
        {
            this.ID = id;
            this.Name = name;
            this.Duration = duration;
            this.Age = age;
            this.After = after;
            this.Zoo = zoo;
        }
    }
}
