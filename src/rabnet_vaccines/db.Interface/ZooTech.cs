using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public enum JobType
    {
        NONE,
        OKROL,
        VUDVOR,
        COUNT_KIDS,
        PRE_OKROL,
        BOYS_OUT,
        GIRLS_OUT,
        FUCK,
        VACC,
        SET_NEST,
        BOYS_BY_ONE
    }

    public class ZooTehNullItem : IData
    {
        public int id = 0;
        public ZooTehNullItem(int id) { this.id = id; }
    }

    public class ZootehJob : IData
    {
        public JobType Type = JobType.OKROL;
        public int Days = 0;
        public string JobName = "";
        public string Address = "";
        public string RabName = "";
        public int RabAge = 0;
        public string RabBreed = "";
        public string Comment = "";
        public string Partners = "";
        public int ID = 0;
        public int ID2;
        public int Flag = 0;
        public ZootehJob() { }
    }

    /// <summary>
    /// В зоотехплане надо сделать 9 разных запросов.
    /// В ответ на onPrepare возвращается этот класс, сожержащий 9 элементов,
    /// на onItem каждой из них заполняется список конкретной работой.
    /// </summary>
    public class ZooTehNullGetter : IDataGetter
    {
        #region IDataGetter Members
        private int val;
        /// <summary>
        /// Количество зоотех работ.
        /// </summary>
        const int ZOOTEHITEMS = 10;

        public int getCount()
        {
            val = -1;
            return ZOOTEHITEMS;
        }

        public int getCount2()
        {
            return 0;
        }

        public int getCount3()
        {
            return 0;
        }

        public float getCount4()
        {
            return 0;
        }

        public void stop()
        {

        }
        /// <summary>
        /// Возвращает объект класса ZooTehNullItem, который содержит только параметр "id"
        /// </summary>
        /// <returns></returns>
        public IData getNextItem()
        {
            val++;
            if (val > ZOOTEHITEMS) return null;
            return new ZooTehNullItem(val);
        }

        #endregion
    }
}
