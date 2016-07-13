using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
#if !DEMO
    public class sMeal
    {
        public enum MoveType { In, Out };

        public readonly int Id;

        public readonly MoveType Type = MoveType.In;
        /// <summary>
        /// Дата завоза корма
        /// </summary>
        public readonly DateTime StartDate = DateTime.MinValue;
        /// <summary>
        /// Объем кормов (Указывается в КилоГраммах)
        /// </summary>
        public readonly int Amount = 0;
        /// <summary>
        /// Среднее потребление кролика в день(Измеряется в КилоГраммах)
        /// </summary>
        public readonly float Rate = 0;

        public int rabDays = 0;
        public DateTime endDate = DateTime.MaxValue;
        public int totalAmount = 0;

        public sMeal(int id, DateTime start, int amount, float rate, string type)
        {
            this.Id = id;
            if (start != null) {
                this.StartDate = start;
            }
            this.Amount = amount;
            this.Rate = rate;
            this.Type = type == "in" ? MoveType.In : MoveType.Out;
        }

        public sMeal(int id, DateTime start, int amount, string type)
        {
            this.Id = id;
            this.StartDate = start;
            this.Amount = amount;
            this.Type = type == "in" ? MoveType.In : MoveType.Out;
        }

    }
#endif
}
