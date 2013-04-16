using System;
using System.Collections.Generic;
using System.Text;
using gamlib;

namespace CAS
{
    /// <summary>
    /// Общие итоги по продажам
    /// </summary>
    public class Summary
    {
        //private readonly byte[] _bytes = new byte[Info.Sizes.SUMMARY_LENGTH];
        private readonly byte[] _roll = new byte[Info.Sizes.Summary.ROLL_LENGTH];
        private readonly byte[] _sticker = new byte[Info.Sizes.Summary.STICKER_LENGTH];
        private readonly byte[] _summ = new byte[Info.Sizes.Summary.SUMM_LENGTH];
        private readonly byte[] _sell = new byte[Info.Sizes.Summary.SELL_LENGTH];
        private readonly byte[] _weight = new byte[Info.Sizes.Summary.WEIGHT_LENGTH];
        private readonly byte[] _allPluSumm = new byte[Info.Sizes.Summary.ALL_PLU_SUMM_LENGTH];
        private readonly byte[] _allPluSell = new byte[Info.Sizes.Summary.ALL_PLU_SELL_LENGTH];
        private readonly byte[] _allPluWeight = new byte[Info.Sizes.Summary.ALL_PLU_WEIGHT_LENGTH];
        private readonly byte[] _lastClear = new byte[Info.Sizes.Summary.LAST_CLEAR_LENGTH];
        private readonly byte[] _freePlu = new byte[Info.Sizes.Summary.FREE_PLU_LENGTH];
        private readonly byte[] _freeMsg = new byte[Info.Sizes.Summary.FREE_MSG_LENGTH];

        public Summary(byte[] bts)
        {
            if (bts.Length < Info.Sizes.SUMMARY_LENGTH) return;
            //Array.Copy(bts, _bytes, Info.Sizes.SUMMARY_LENGTH);

            Array.Copy(bts, Info.Sizes.Summary.ROLL_ADDRESS, _roll, 0, Info.Sizes.Summary.ROLL_LENGTH);
            Array.Copy(bts, Info.Sizes.Summary.STICKER_ADDRESS, _sticker, 0, Info.Sizes.Summary.STICKER_LENGTH);
            Array.Copy(bts, Info.Sizes.Summary.SUMM_ADDRESS, _summ, 0, Info.Sizes.Summary.SUMM_LENGTH);
            Array.Copy(bts, Info.Sizes.Summary.SELL_ADDRESS, _sell, 0, Info.Sizes.Summary.SELL_LENGTH);
            Array.Copy(bts, Info.Sizes.Summary.WEIGHT_ADDRESS, _weight, 0, Info.Sizes.Summary.WEIGHT_LENGTH);
            Array.Copy(bts, Info.Sizes.Summary.ALL_PLU_SUMM_ADDRESS, _allPluSumm, 0, Info.Sizes.Summary.ALL_PLU_SUMM_LENGTH);
            Array.Copy(bts, Info.Sizes.Summary.ALL_PLU_SELL_ADDRESS, _allPluSell, 0, Info.Sizes.Summary.ALL_PLU_SELL_LENGTH);
            Array.Copy(bts, Info.Sizes.Summary.ALL_PLU_WEIGHT_ADDRESS, _allPluWeight, 0, Info.Sizes.Summary.ALL_PLU_WEIGHT_LENGTH);
            Array.Copy(bts, Info.Sizes.Summary.LAST_CLEAR_ADDRESS, _lastClear, 0, Info.Sizes.Summary.LAST_CLEAR_LENGTH);
            Array.Copy(bts, Info.Sizes.Summary.FREE_PLU_ADDRESS, _freePlu, 0, Info.Sizes.Summary.FREE_PLU_LENGTH);
            Array.Copy(bts, Info.Sizes.Summary.FREE_MSG_ADDRESS, _freeMsg, 0, Info.Sizes.Summary.FREE_MSG_LENGTH);
        }

        /// <summary>
        /// Cчётчик пробега (мм)
        /// </summary>
        public int RollOut { get { return BitConverter.ToInt32(_roll, 0); } }
        /// <summary>
        /// Счетчик этикеток
        /// </summary>
        public int Sticker { get { return BitConverter.ToInt32(_sticker, 0); } }
        public int Summ { get { return BitConverter.ToInt32(_summ, 0); } }
        public int Sell
        {
            get
            {
                byte[] result = new byte[4];
                Array.Copy(_sell, result, _sell.Length);
                return BitConverter.ToInt32(result, 0);
            }
        }
        public int Weight { get { return BitConverter.ToInt32(_summ, 0); } }
        public int TotalSumm { get { return BitConverter.ToInt32(_summ, 0); } }
        public int TotalSell
        {
            get
            {
                //далее лечение от несоответствия байтов
                byte[] result = new byte[4];
                Array.Copy(_allPluSell, result, _allPluSell.Length);
                return BitConverter.ToInt32(result, 0);
            }
        }
        public int TotalWeight { get { return BitConverter.ToInt32(_weight, 0); } }
        public DateTime LastClear
        {
            get { return BitHelper.GetLastClear(BitHelper.ParseGroupBinDec(_lastClear)); }
        }
        public int FreePLU { get { return (int)BitConverter.ToUInt16(_freePlu, 0); } }
        public int FreeMSG { get { return (int)BitConverter.ToUInt16(_freeMsg, 0); } }
    }
}
