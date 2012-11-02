using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace rabnet
{
    public class RabbitGen
    {
        public Rabbit.SexType Sex;
        public int ID;
        public int MotherId;
        public int FatherId;
        public string Name;
        public string Surname;
        public string Secname;
        public string BreedColorName;
        public Color BreedColor;
        public int BreedId;
        public string BreedName;
        public Boolean IsDead;
        //public string t;
        public float PriplodK;
        public float RodK;

        public string Fullname
        {
            get
            {
                string n = Name;
                string surn = Surname;
                string secn = Secname;
                if (Sex == Rabbit.SexType.FEMALE)
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
