using System;
using System.Collections.Generic;
using System.Text;

namespace CAS
{
    /// <summary>
    /// Заводские настройки весов
    /// </summary>
    public class FactoryConfig
    {
        private readonly byte[] _weightLimit = new byte[Info.Sizes.FactoryConfig.WEIGHT_LIMIT_LENGHT];
        private readonly byte[] _dotPlace = new byte[Info.Sizes.FactoryConfig.DOT_PLACE_LENGHT];
        private readonly byte[] _doubleRange = new byte[Info.Sizes.FactoryConfig.DRW_MODE_LENGHT];
        private readonly byte[] _shit1 = new byte[Info.Sizes.FactoryConfig.SHIT1_LENGHT];
        private readonly byte[] _shit2 = new byte[Info.Sizes.FactoryConfig.SHIT2_LENGHT];
        private readonly byte[] _weightFor = new byte[Info.Sizes.FactoryConfig.WEIGHT_FOR_PRICE_LENGHT];
        private readonly byte[] _round = new byte[Info.Sizes.FactoryConfig.ROUND_VALUE_LENGHT];
        private readonly byte[] _taraLimit = new byte[Info.Sizes.FactoryConfig.TARA_LIMIT_LENGHT];

        public FactoryConfig(byte[] bts)
        {
            if (bts.Length < Info.Sizes.FACTORY_CONF_LENGTH) return;
            Array.Copy(bts, Info.Sizes.FactoryConfig.WEIGHT_LIMIT_ADDRESS, _weightLimit, 0, Info.Sizes.FactoryConfig.WEIGHT_LIMIT_LENGHT);
            Array.Copy(bts, Info.Sizes.FactoryConfig.DOT_PLACE_ADDRESS, _dotPlace, 0, Info.Sizes.FactoryConfig.DOT_PLACE_LENGHT);
            Array.Copy(bts, Info.Sizes.FactoryConfig.DRW_MODE_ADDRESS, _doubleRange, 0, Info.Sizes.FactoryConfig.DRW_MODE_LENGHT);
            Array.Copy(bts, Info.Sizes.FactoryConfig.SHIT1_ADDRESS, _shit1, 0, Info.Sizes.FactoryConfig.SHIT1_LENGHT);
            Array.Copy(bts, Info.Sizes.FactoryConfig.SHIT2_ADDRESS, _shit2, 0, Info.Sizes.FactoryConfig.SHIT2_LENGHT);
            Array.Copy(bts, Info.Sizes.FactoryConfig.WEIGHT_FOR_PRICE_ADDRESS, _weightFor, 0, Info.Sizes.FactoryConfig.WEIGHT_FOR_PRICE_LENGHT);
            Array.Copy(bts, Info.Sizes.FactoryConfig.ROUND_VALUE_ADDRESS, _round, 0, Info.Sizes.FactoryConfig.ROUND_VALUE_LENGHT);
            Array.Copy(bts, Info.Sizes.FactoryConfig.TARA_LIMIT_ADDRESS, _taraLimit, 0, Info.Sizes.FactoryConfig.TARA_LIMIT_LENGHT);
        }

        public int WeightLimit { get { return (int)BitConverter.ToInt16(_weightLimit, 0); } }
        public int DotPlace_Weight { get { return (int)_dotPlace[0]; } }
        public int DotPlace_Price { get { return (int)_dotPlace[1]; } }
        public int DotPlace_Value { get { return (int)_dotPlace[2]; } }
        public bool DoubleRange { get { return _dotPlace[0] == 0 ? false : true; } }
        /// <summary>
        ///  Дискретность индикации веса во всем диапазоне или в
        ///  верхнем диапазоне при включенном двухдиапазонном режиме.
        /// </summary>
        public bool Shit1 { get { return _shit1[0] == 0 ? false : true; } }
        /// <summary>
        /// Дискретность индикации веса в нижнем диапазоне
        /// при включенном двухдиапазонном режиме.
        /// </summary>
        public bool Shit2 { get { return _shit2[0] == 0 ? false : true; } }
        public int WeightForPrice { get { return (int)BitConverter.ToInt16(_weightFor, 0); } }
        public int RoundValue { get { return (int)_round[0]; } }
        public int TaraLimit { get { return (int)BitConverter.ToInt16(_taraLimit, 0); } }
    }
}
