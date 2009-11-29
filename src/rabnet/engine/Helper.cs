using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetEngHelper
    {
        public static bool geterosis(String g1,String g2)
        {
            string[] g1s=g1.Split(' ');
            string[] g2s=g2.Split(' ');
            foreach (String s in g1s)
            {
                for (int i = 0; i < g2s.Length; i++)
                    if (s == g2s[i])
                        return true;
            }
            return false;
        }
    }
}
