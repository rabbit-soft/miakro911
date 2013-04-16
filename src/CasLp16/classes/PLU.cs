using System;
using System.Collections.Generic;
using System.Text;
using gamlib;

namespace CAS
{
    /// <summary>
    /// Запись о товаре
    /// </summary>
    public class PLU
    {
        //private readonly byte[] _bytes = new byte[Info.Sizes.PLU_LENGTH];
        private readonly byte[] _id = new byte[Info.Sizes.PLU.ID_LENGTH];
        private readonly byte[] _code = new byte[Info.Sizes.PLU.CODE_LENGTH];
        private readonly byte[] _prodName1 = new byte[Info.Sizes.PLU.PRODUCT_NAME_LENGTH];
        private readonly byte[] _prodName2 = new byte[Info.Sizes.PLU.PRODUCT_NAME_LENGTH];
        private readonly byte[] _price = new byte[Info.Sizes.PLU.PRICE_LENGTH];
        private readonly byte[] _liveTime = new byte[Info.Sizes.PLU.LIVETIME_LENGTH];
        private readonly byte[] _taraWeight = new byte[Info.Sizes.PLU.TARA_WEIGHT_LENGHT];
        private readonly byte[] _groupCode = new byte[Info.Sizes.PLU.GROUP_CODE_LENGTH];
        private readonly byte[] _msgId = new byte[Info.Sizes.PLU.MESSAGE_ID_LENGTH];
        private readonly byte[] _lastClear = new byte[Info.Sizes.PLU.LAST_CLEAR_LENGTH];
        private readonly byte[] _totalSumm = new byte[Info.Sizes.PLU.TOTAL_SUMM_LENGTH];
        private readonly byte[] _totalWeight = new byte[Info.Sizes.PLU.TOTAL_WEIGHT_LENGTH];
        private readonly byte[] _totalSell = new byte[Info.Sizes.PLU.TOTAL_SELL_LENGTH];

        public byte[] Bytes
        {
            get
            {
                return BitHelper.Concat(_id, _code, _prodName1, _prodName2, _price, _liveTime, _taraWeight, _groupCode, _msgId);
                /*return _id.Concat(_code).
                            Concat(_prodName1).
                            Concat(_prodName2).
                            Concat(_price).
                            Concat(_liveTime).
                            Concat(_taraWeight).
                            Concat(_groupCode).
                            Concat(_msgId).
                    Concat(_lastClear).
                    Concat(_totalSumm).
                    Concat(_totalWeight).
                    Concat(_totalSell).ToArray();*/

            }
        }

        public int ID
        {
            get { return BitConverter.ToInt32(_id, 0); }
            set
            {
                if (value < 0 || value > Info.Sizes.PLU_MAX_INDEX) return;
                byte[] newval = BitConverter.GetBytes(value);
                Array.Clear(_id, 0, _id.Length);
                Array.Copy(newval, _id, newval.Length);
            }
        }
        //todo "Должен быть строкой"
        public int Code
        {
            get { return BitHelper.FromBinDecimal(_code); }
            set
            {
                int val = value % 1000000;
                int i = 0;
                Array.Clear(_code, 0, 6);
                while (i < 6)
                {
                    _code[i] = (byte)(val % 10);
                    val = val / 10;
                    i++;
                }
            }
        }

        public string ProductName1
        {
            get { return Encoding.GetEncoding(866).GetString(_prodName1).Replace("\0", ""); }
            set
            {
                if (value.Length > Info.Sizes.PLU.PRODUCT_NAME_LENGTH)
                    value = value.Substring(0, Info.Sizes.PLU.PRODUCT_NAME_LENGTH);
                byte[] newval = Encoding.GetEncoding(866).GetBytes(value);
                Array.Clear(_prodName1, 0, _prodName1.Length);
                Array.Copy(newval, _prodName1, newval.Length);
            }
        }
        public string ProductName2
        {
            get { return Encoding.GetEncoding(866).GetString(_prodName2).Replace("\0", ""); }
            set
            {
                if (value.Length > Info.Sizes.PLU.PRODUCT_NAME_LENGTH)
                    value = value.Substring(0, Info.Sizes.PLU.PRODUCT_NAME_LENGTH);
                byte[] newval = Encoding.GetEncoding(866).GetBytes(value);
                Array.Clear(_prodName2, 0, _prodName2.Length);
                Array.Copy(newval, _prodName2, newval.Length);
            }
        }
        public int Price
        {
            get { return BitConverter.ToInt32(_price, 0); }
            set
            {
                if (value > 999999) return;
                byte[] bts = BitConverter.GetBytes(value);
                Array.Copy(bts, _price, bts.Length);
            }
        }
        public int LiveTime
        {
            get
            {
                byte[] bts = BitHelper.ParseGroupBinDec(_liveTime);
                return BitHelper.GetLiveTimeDays(bts);
            }
            set
            {
                if (value < 0 || value > 999) return;
                byte[] bts = BitHelper.GetLiveTimeBytes(value);
                Array.Copy(BitHelper.MakeGroupBinDec(bts), _liveTime, 3);
            }
        }
        /// <summary>
        /// Вес тары в КилоГраммах
        /// </summary>
        public int TaraWeight
        {
            get { return (int)BitConverter.ToInt16(_taraWeight, 0); }
            set
            {
                Array.Copy(BitConverter.GetBytes(value), _taraWeight, 2);
            }
        }
        public int GroupCode
        {
            get { return BitHelper.FromBinDecimal(_groupCode); }
            set
            {
                if (value > 999999) return;
                byte[] bts = BitHelper.ToBinDecimal(value);
                Array.Copy(bts, _groupCode, bts.Length);
            }
        }
        public int MessageID
        {
            get { return (int)BitConverter.ToInt16(_msgId, 0); }
            set
            {
                if (value < 0 || value > Info.Sizes.MSG_LENGTH) return;
                Array.Clear(_msgId, 0, _msgId.Length);
                if (value > Info.Sizes.MSG_MAX_INDEX)
                    value = Info.Sizes.MSG_MAX_INDEX;
                byte[] newval = BitConverter.GetBytes(value);
                Array.Copy(newval, _msgId, 2);

            }
        }
        public DateTime LastClear
        {
            get
            {
                byte[] bts = BitHelper.ParseGroupBinDec(_lastClear);
                return BitHelper.GetLastClear(bts);
            }
        }
        public int TotalSumm
        {
            get { return BitConverter.ToInt32(_totalSumm, 0); }
        }
        /// <summary>
        /// Общий вес взвешенной продукции.
        /// Указывается в КилоГраммах
        /// </summary>
        public int TotalWeight
        {
            get { return BitConverter.ToInt32(_totalWeight, 0); }
        }
        public int TotalSell
        {
            get
            {
                byte[] result = new byte[4];
                Array.Copy(_totalSell, result, 3);
                return BitConverter.ToInt32(result, 0);
            }
        }
        /// <summary>
        /// Удалить ли данную запись при сохранении
        /// </summary>
        public bool Delete = false;
        /// <summary>
        /// Создает объект PLU из хранящихся данных в весах
        /// </summary>
        /// <param name="bytes">100 байт информации</param>
        public PLU(byte[] bytes)
        {
            if (bytes.Length < 100) return;
            //Array.Copy(bytes, _bytes, Info.Sizes.PLU_LENGTH);

            Array.Copy(bytes, Info.Sizes.PLU.ID_ADDRESS, _id, 0, Info.Sizes.PLU.ID_LENGTH);
            Array.Copy(bytes, Info.Sizes.PLU.CODE_ADDRESS, _code, 0, Info.Sizes.PLU.CODE_LENGTH);
            Array.Copy(bytes, Info.Sizes.PLU.PRODUCT_NAME_1_ADDRESS, _prodName1, 0, Info.Sizes.PLU.PRODUCT_NAME_LENGTH);
            Array.Copy(bytes, Info.Sizes.PLU.PRODUCT_NAME_2_ADDRESS, _prodName2, 0, Info.Sizes.PLU.PRODUCT_NAME_LENGTH);
            Array.Copy(bytes, Info.Sizes.PLU.PRICE_ADDRESS, _price, 0, Info.Sizes.PLU.PRICE_LENGTH);
            Array.Copy(bytes, Info.Sizes.PLU.LIVETIME_ADDRESS, _liveTime, 0, Info.Sizes.PLU.LIVETIME_LENGTH);
            Array.Copy(bytes, Info.Sizes.PLU.TARA_WEIGHT_ADDRESS, _taraWeight, 0, Info.Sizes.PLU.TARA_WEIGHT_LENGHT);
            Array.Copy(bytes, Info.Sizes.PLU.GROUP_CODE_ADDRESS, _groupCode, 0, Info.Sizes.PLU.GROUP_CODE_LENGTH);
            Array.Copy(bytes, Info.Sizes.PLU.MESSAGE_ID_ADDRESS, _msgId, 0, Info.Sizes.PLU.MESSAGE_ID_LENGTH);
            Array.Copy(bytes, Info.Sizes.PLU.LAST_CLEAR_ADDRESS, _lastClear, 0, Info.Sizes.PLU.LAST_CLEAR_LENGTH);
            Array.Copy(bytes, Info.Sizes.PLU.TOTAL_SUMM_ADDRESS, _totalSumm, 0, Info.Sizes.PLU.TOTAL_SUMM_LENGTH);
            Array.Copy(bytes, Info.Sizes.PLU.TOTAL_WEIGHT_ADDRESS, _totalWeight, 0, Info.Sizes.PLU.TOTAL_WEIGHT_LENGTH);
            Array.Copy(bytes, Info.Sizes.PLU.TOTAL_SELL_ADDRESS, _totalSell, 0, Info.Sizes.PLU.TOTAL_SELL_LENGTH);

        }
        public PLU() { }

        /*private byte[] getBytes(byte startByte, byte lenght)
        {
            byte[] result = new byte[lenght];
            Array.Copy(_bytes, startByte, result, 0, lenght);
            return result;
        }*/
    }

    public class PLUList : List<PLU>
    {
        internal PLU GetPLU(int id)
        {
            foreach (PLU plu in this)
                if (plu.ID == id)
                    return plu;
            return null;
        }

        internal void DeletPLU(int id)
        {
            for (int i = 0; i < this.Count; i++)
                if (this[i].ID == id)
                {
                    this.RemoveAt(i);
                    return;
                }
        }

        internal int[] getIds()
        {
            int[] result = new int[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                result[i] = this[i].ID;
            }
            return result;
        }

        internal bool ContainsID(int id)
        {
            foreach (PLU plu in this)
            {
                if (plu.ID == id) return true;
            }
            return false;
        }
    }

}
