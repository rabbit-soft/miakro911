using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class Dead : IData
    {
        public int id;
        public string name;
        public string address;
        public int age;
        public DateTime deadDate;
        public string reason;
        public string notes;
        public string breed;
        public int group;
        public Dead(int id, string nm, string ad, int ag, DateTime dd, string rsn, string nts, string brd, int grp)
        {
            this.id = id; name = nm;
            address = Building.FullPlaceName(ad);
            age = ag; deadDate = dd;
            reason = rsn;
            notes = nts; breed = brd;
            group = grp;
        }
    }

    public static class DeadReason_Static
    {
        /// <summary>
        /// Списан из старой программы
        /// </summary>
        public static int DeadFromOldApp = 1;

        /// <summary>
        /// Объединение
        /// </summary>
        public static int CombineGroups = 2;
        
        /// <summary>
        /// На убой
        /// </summary>
        public static int Killed = 3;
        
        /// <summary>
        /// Продажа племенного поголовья
        /// </summary>
        public static int Selled = 4;
        
        /// <summary>
        /// Падеж при подсчете
        /// </summary>
        public static int Dead_KidsCount = 5;

        /// <summary>
        /// Падеж
        /// </summary>
        public static int Dead = 6;
    }
}
