using System;
using System.Collections.Generic;
using System.Text;

namespace CAS
{
    /// <summary>
    /// Текущее состояние весов
    /// </summary>
    public class State
    {
        private readonly byte[] _stateByte = new byte[Info.Sizes.State.STATE_BYTE_LENGHT];
        private readonly byte[] _absWeight = new byte[Info.Sizes.State.ABSOLUTE_WEIGHT_LENGHT];
        private readonly byte[] _priceRate = new byte[Info.Sizes.State.PRICE_LENGHT];
        private readonly byte[] _value = new byte[Info.Sizes.State.VALUE_LENGHT];
        private readonly byte[] _checkPlu = new byte[Info.Sizes.State.CHECKED_PLU_LENGHT];

        public State(byte[] bts)
        {
            if (bts.Length < Info.Sizes.STATE_LENGTH) return;
            Array.Copy(bts, Info.Sizes.State.STATE_BYTE_ADDRESS, _stateByte, 0, Info.Sizes.State.STATE_BYTE_LENGHT);
            Array.Copy(bts, Info.Sizes.State.ABSOLUTE_WEIGHT_ADDRESS, _absWeight, 0, Info.Sizes.State.ABSOLUTE_WEIGHT_LENGHT);
            Array.Copy(bts, Info.Sizes.State.PRICE_ADDRESS, _priceRate, 0, Info.Sizes.State.PRICE_LENGHT);
            Array.Copy(bts, Info.Sizes.State.VALUE_ADDRESS, _value, 0, Info.Sizes.State.VALUE_LENGHT);
            Array.Copy(bts, Info.Sizes.State.CHECKED_PLU_ADDRESS, _checkPlu, 0, Info.Sizes.State.CHECKED_PLU_LENGHT);
        }

        public bool Overload { get { return (_stateByte[0] & Convert.ToByte("00000001", 2)) > 0 ? true : false; } }
        public bool TaraSelection { get { return (_stateByte[0] & Convert.ToByte("00000100", 2)) > 0 ? true : false; } }
        public bool ZeroWeight { get { return (_stateByte[0] & Convert.ToByte("00001000", 2)) > 0 ? true : false; } }
        public bool TwoRange { get { return (_stateByte[0] & Convert.ToByte("00100000", 2)) > 0 ? true : false; } }
        public bool StableWeight { get { return (_stateByte[0] & Convert.ToByte("01000000", 2)) > 0 ? true : false; } }
        public char Sign { get { return (_stateByte[0] & Convert.ToByte("10000000", 2)) > 0 ? '-' : '+'; } }
        public int Weight { get { return (int)BitConverter.ToInt16(_absWeight, 0); } }
        /// <summary>
        /// Цена товара (коп/кг)
        /// </summary>
        public int PriceRate { get { return BitConverter.ToInt32(_priceRate, 0); } }
        public int Value { get { return BitConverter.ToInt32(_value, 0); } }
        public int CheckedPLU { get { return BitConverter.ToInt32(_checkPlu, 0); } }
    }
}
