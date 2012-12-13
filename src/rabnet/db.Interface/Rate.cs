using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public static class Rate
    {
        public const int NORMAL_OKROL_CHILDRENS = 8;
        public const int AUTUMN_FUCK_RATE = 2;
        public const int PROHOLOST_RATE = -2;

        public static int CalcRate(int born, int dead, bool female)
        {
            int rate = 0;
            if (born <= NORMAL_OKROL_CHILDRENS)
                rate = born  / 2;
            else           
                rate = (NORMAL_OKROL_CHILDRENS / 2) + (born - NORMAL_OKROL_CHILDRENS);
            if (female && dead > 0)
                rate -= dead / 2;
            return rate;
        }

        public static int CalcChildrenRate(int femaleRate,int maleRate)
        {
            return (femaleRate + maleRate) / 10;
        }

    }
}
