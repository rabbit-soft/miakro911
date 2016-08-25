/**
 * Чтобы добавить новый вид работ нужно:
 * - Добавить enum к JobType
 * - Увеличить переменную ZOOTEH_ITEMS класса ZooTehNullGetter
 * - Добавить название работы в список ZootechFilter
 * - В ZootechFilter добавить символ в переменную ITEM_FLAGS
 * - В engine.RabNetZooTech обновляем метод makeZooTehPlan
 * - db.mysql.ZooTech добавить название работы в методе getRusJobName
 * - там же обновляем метод getQuery и создаем соответствующий метод с запросом
 * - там же обновляем метод fillData
 * - в WorkPanel добавляем пункт меню и правило для его отображения в методе setMenu
 * - добавляем обработчик для нового типа в методе makeJob
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public enum JobType
    {
        None,
        Okrol,
        /// <summary> Выдворение </summary>
        NestOut,
        /// <summary>Подсчет гнездовых/подсосных</summary>
        CountKids,
        /// <summary>Предокрольный осмотр</summary>
        PreOkrol,
        BoysOut,
        GirlsOut,
        Fuck,
        Vaccine,
        NestSet,
        BoysByOne,
        SpermTake
    }

    public class ZooTehNullItem : IData
    {
        public int id = 0;
        public ZooTehNullItem(int id) { this.id = id; }
    }

    public class ZootehJob : IData
    {
        public JobType Type = JobType.Okrol;
        public int Days = 0;
        public string JobName = "";
        public string Rabplace = "";
        public string Address = "";
        public string RabName = "";
        public int RabAge = 0;
        public string RabBreed = "";
        public string Comment = "";
        public string Partners = "";
        public int ID = 0;
        public int ID2;
        public int Flag = 0;
        public int Flag2 = 0;
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
        const int ZOOTEH_ITEMS = 11;

        public int getCount()
        {
            val = -1;
            return ZOOTEH_ITEMS;
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

        public void Close()
        {

        }
        /// <summary>
        /// Возвращает объект класса ZooTehNullItem, который содержит только параметр "id"
        /// </summary>
        /// <returns></returns>
        public IData GetNextItem()
        {
            val++;
            if (val >= ZOOTEH_ITEMS) {
                return null;
            }
            return new ZooTehNullItem(val);
        }

        #endregion
    }
}
