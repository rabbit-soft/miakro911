using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace rabnet
{
    public class RabbitGen
    {
        public Rabbit.SexType sex;
        public int rid;
        public int r_mother;
        public int r_father;
        public string name;
        public string surname;
        public string secname;
        public string breed_color_name;
        public Color breed_color;
        public int breed;
        public string breed_name;
        public Boolean IsDead;
        public string t;
        public float PriplodK;
        public float RodK;
        public string fullname
        {
            get
            {
                string n = name;
                string surn = surname;
                string secn = secname;
                if (sex == Rabbit.SexType.FEMALE)
                {
                    if (surn != "")
                    {
                        surn += "a";
                    }
                    if (secn != "")
                    {
                        secn += "a";
                    }
                }

                if ((secn != "") && (surn != ""))
                {
                    n += " " + surn + "-" + secn;
                }
                else
                {
                    if (secn != "")
                    {
                        n += " " + secn;
                    }
                    if (surn != "")
                    {
                        n += " " + surn;
                    }
                }
                return n;
            }
        }
    }
}
