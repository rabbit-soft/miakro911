﻿using System;
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

        public static bool DetectInbreeding(String rabGenom1, String rabGenom2)
        {
            if (rabGenom1.Length > rabGenom2.Length)
                DetectInbreeding(rabGenom2,rabGenom1);
            if (rabGenom2.Contains(rabGenom1))
                return true;
            else
            {
                bool res=false;
                string mGens,fGens;
                ParceGenoms(rabGenom1, out mGens, out fGens);
                if (!String.IsNullOrEmpty(mGens))
                {
                    res = DetectInbreeding(mGens,rabGenom2);
                    if (res) return true;
                }
                if (!String.IsNullOrEmpty(fGens))
                {
                    res = DetectInbreeding(fGens, rabGenom2);
                    if (res) return true;
                }
                return false;
            }
        }

        public static void ParceGenoms(string rabGenoms, out string mGens, out string fGens)
        {
            mGens = "";
            fGens = "";
            if (String.IsNullOrEmpty(rabGenoms) || !rabGenoms.Contains("{")) return;

            rabGenoms = rabGenoms.Substring(rabGenoms.IndexOf('{') + 1);
            rabGenoms = rabGenoms.Remove(rabGenoms.Length-1);
            int i = rabGenoms.IndexOf('{');
            if (i > 0)
            {
                i = 0;
                bool canP = false, ff = false;
                int serc = 0;
                ///определяем указан один родитель или 2е
                while (i < rabGenoms.Length)
                {
                    if (rabGenoms[i] == '{')
                    {
                        ff = true;
                        serc++;
                    }
                    else if (rabGenoms[i] == '}')
                        serc--;
                    if (rabGenoms[i] == ',' && !ff)
                    {
                        i--;
                        canP = true;
                        break;
                    }
                    if (ff && serc == 0)
                    {
                        canP = true;
                        break;
                    }
                    i++;
                }
                if (canP)
                {
                    mGens = rabGenoms.Substring(1, i);
                    fGens = rabGenoms.Substring(i + 1);
                }
                else
                    mGens = rabGenoms;
            }
            else
            {
                i = rabGenoms.IndexOf(',');
                if (i > 0)
                {
                    mGens = rabGenoms.Substring(1, i - 1);
                    fGens = rabGenoms.Substring(i + 1);
                }
                else
                    mGens = rabGenoms;
            }
        }
       
    }
}
