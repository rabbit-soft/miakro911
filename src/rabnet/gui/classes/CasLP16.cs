#if DEBUG
//#define NOCATCH
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
//using System.Linq;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace CAS
{
    public sealed partial class CasLP16
    {
        private static class BitHelper
        {
            /// <summary>
            /// Заполнен ли массив только нулями
            /// </summary>
            /// <param name="bts"></param>
            /// <returns></returns>
            internal static bool arrayIsEmpty(byte[] bts)
            {
                for (int i = 0; i < bts.Length; i++)
                    if (bts[i] != 0)
                        return false;
                return true;         
            }

            internal static int fromBinDecimal(byte[] bytes)
            {
                string result = "";
                for (int i = bytes.Length - 1; i >= 0; i--)
                    result += bytes[i].ToString();
                return int.Parse(result);
            }

            internal static byte[] toBinDecimal(int val)
            {
                string vstr = val.ToString();
                byte[] result = new byte[vstr.Length];

                return result;
            }

            internal static byte[] parseGroupBinDec(byte[] bindec)
            {
                byte[] result = new byte[bindec.Length * 2];
                int i = 0;
                foreach (byte bt in bindec)//начинаем первого байта
                {
                    string str = Convert.ToString(bt, 2);
                    while (str.Length < 8)
                        str = "0" + str;
                    result[i++] = Convert.ToByte(str.Substring(0, 4), 2);//4старших бита вперед
                    result[i++] = Convert.ToByte(str.Substring(4, 4), 2);//4младших бита за ним
                }
                return result;
            }

            internal static byte[] makeGroupBinDec(byte[] bin)
            {
                byte[] result = new byte[bin.Length / 2];
                if (bin.Length != 6) return result;
                int i = 0;
                for (int j = 0; j < result.Length; j++)
                {
                    string str1 = Convert.ToString(bin[i++], 2);
                    while (str1.Length < 4)
                        str1 = "0" + str1;
                    string str2 = Convert.ToString(bin[i++], 2);
                    while (str2.Length < 4)
                        str2 = "0" + str2;
                    result[j] = Convert.ToByte(str1 + str2, 2);
                }
                return result;
            }

            internal static int getLiveTimeDays(byte[] bts)
            {
                if (bts.Length > 6) return 0;
                int result = 0;
                int pos = 1;
                for (int i = 5; i > -1; i--)
                {
                    result += bts[i] * pos;
                    pos *= 10;
                }
                return result;
            }

            internal static byte[] getLiveTimeBytes(int val)
            {
                byte[] result = new byte[6];
                int i = 5;
                while (i > -1)
                {
                    result[i] = (byte)(val % 10);
                    val = val / 10;
                    i--;
                }
                return result;
            }

            internal static DateTime getLastClear(byte[] bts)
            {

                DateTime result = new DateTime(
                    2000 + bts[10] * 10 + bts[11],
                    bts[8] * 10 + bts[9],
                    bts[6] * 10 + bts[7],
                    bts[4] * 10 + bts[5],
                    bts[2] * 10 + bts[3],
                    bts[0] * 10 + bts[1]);
                return result;
            }

            internal static byte[] concat(params byte[][] matrix)
            {
                int totalLength = 0;
                for (int i = 0; i < matrix.Length; i++)
                    totalLength += matrix[i].Length;
                byte[] result = new byte[totalLength];
                int k = 0;
                for (int i = 0; i < matrix.Length; i++)
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        result[k++] = matrix[i][j];
                    }
                return result;
            }
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
        }
        public class MSGList : List<MSG>
        {
            internal MSG GetMSG(int id)
            {
                foreach (MSG msg in this)
                    if (msg.ID == id)
                        return msg;
                return null;
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
        }

        private static class Info
        {
            public static class Sizes
            {
                public static class PLU
                {
                    public const byte ID_ADDRESS = 0x00;
                    public const byte ID_LENGTH = 4;

                    public const byte CODE_ADDRESS = 0x04;
                    public const byte CODE_LENGTH = 6;

                    public const byte PRODUCT_NAME_1_ADDRESS = 0x0A;
                    public const byte PRODUCT_NAME_2_ADDRESS = 0x26;
                    public const byte PRODUCT_NAME_LENGTH = 28;

                    public const byte PRICE_ADDRESS = 0x42;
                    public const byte PRICE_LENGTH = 4;

                    public const byte LIVETIME_ADDRESS = 0x46;
                    public const byte LIVETIME_LENGTH = 3;

                    public const byte TARA_WEIGHT_ADDRESS = 0x49;
                    public const byte TARA_WEIGHT_LENGHT = 2;

                    public const byte GROUP_CODE_ADDRESS = 0x4B;
                    public const byte GROUP_CODE_LENGTH = 6;

                    public const byte MESSAGE_ID_ADDRESS = 0x51;
                    public const byte MESSAGE_ID_LENGTH = 2;

                    public const byte LAST_CLEAR_ADDRESS = 0x53;
                    public const byte LAST_CLEAR_LENGTH = 6;

                    public const byte TOTAL_SUMM_ADDRESS = 0x59;
                    public const byte TOTAL_SUMM_LENGTH = 4;

                    public const byte TOTAL_WEIGHT_ADDRESS = 0x5D;
                    public const byte TOTAL_WEIGHT_LENGTH = 4;

                    public const byte TOTAL_SELL_ADDRESS = 0x61;
                    public const byte TOTAL_SELL_LENGTH = 3;
                }
                public static class Summary
                {
                    public const byte ROLL_ADDRESS = 0x00;
                    public const byte ROLL_LENGTH = 4;

                    public const byte STICKER_ADDRESS = 0x04;
                    public const byte STICKER_LENGTH = 4;

                    public const byte SUMM_ADDRESS = 0x08;
                    public const byte SUMM_LENGTH = 4;

                    public const byte SELL_ADDRESS = 0x0C;
                    public const byte SELL_LENGTH = 3;

                    public const byte WEIGHT_ADDRESS = 0x0F;
                    public const byte WEIGHT_LENGTH = 4;

                    public const byte ALL_PLU_SUMM_ADDRESS = 0x13;
                    public const byte ALL_PLU_SUMM_LENGTH = 4;

                    public const byte ALL_PLU_SELL_ADDRESS = 0x17;
                    public const byte ALL_PLU_SELL_LENGTH = 4;

                    public const byte ALL_PLU_WEIGHT_ADDRESS = 0x1A;
                    public const byte ALL_PLU_WEIGHT_LENGTH = 3;

                    public const byte LAST_CLEAR_ADDRESS = 0x1E;
                    public const byte LAST_CLEAR_LENGTH = 6;

                    public const byte FREE_PLU_ADDRESS = 0x24;
                    public const byte FREE_PLU_LENGTH = 2;

                    public const byte FREE_MSG_ADDRESS = 0x26;
                    public const byte FREE_MSG_LENGTH = 2;
                }
                public static class Commands
                {
                    public const int PLU_SET = 1+83;
                    public const int MSG_SET = 1 + 2 + 400;
                    public const int PLU_CLEAR = 1 + 4;
                    public const int MSG_CLEAR = 1 + 2;
                }
                /// <summary>
                /// Максимальный номер ID сообщения. ID=0 допустим 
                /// </summary>
                public const int PLU_MAX_INDEX = 4000;
                /// <summary>
                /// Размер в байтах одной записи PLU = 100
                /// </summary>
                public const int PLU_LENGTH = 100;
                /// <summary>
                /// Максимально допустимое количество символов ASCII в одном сообщении.
                /// </summary>
                public const int MSG_LENGTH = 400;
                /// <summary>
                /// Максимальный номер ID сообщения
                /// </summary>
                public const int MSG_MAX_INDEX = 1000;
                /// <summary>
                /// Максимальное количество строчек в сообщении = 8
                /// </summary>
                public const int MSG_MAX_STRINGS_COUNT = 8;
                /// <summary>
                /// Размер одной строки в поле сообщения = 50
                /// </summary>
                public const int MSG_MAX_STRING_LENGTH = 50;
                public const int FACTORY_CONF_ANS_SIZE = 13;
                public const int SUMMARY_LENGTH = 40;
                public const int STATE_LENGTH = 13;
                public const int LIVE_TIME_MAX_DAYS = 999;
            }

            public static class Ansvers
            {
                public const byte READY = 0x80;
                public const byte SUCCESS = 0xAA;
                public const byte EMERGENCY_REQUEST_PLU = 0xDD;
                public const byte ERROR = 0xEE;
            }

            /// <summary>
            /// Команды Обмена
            /// </summary>
            public static class Commands
            {
                public const byte PLU_GET = 0x81;
                public const byte PLU_SET = 0x82;
                public const byte MSG_GET = 0x83;
                public const byte MSG_SET = 0x84;
                public const byte SUMMARY_SELL_GET = 0x85;
                public const byte SUMMARY_SELL_CLEAR = 0x86;
                public const byte BORDERS_SET = 0x87;
                public const byte BORDERS_CLEAR = 0x88;
                public const byte STATE_GET = 0x89;
                public const byte USER_SET = 0x8A;
                public const byte PRICE_BUTTON_SET_PLU = 0x8B;
                public const byte LOGO_SET = 0x8C;
                public const byte PLU_CLEAR = 0x8D;
                public const byte MSG_CLEAR = 0x8E;
                //0x8f  //0x90  //0x91
                public const byte PLU_SUMMARY_CLEAR = 0x92;
                //0x93
                public const byte SHOP_SET = 0x94;
                public const byte USER_GET = 0x95;
                public const byte PRICE_BUTTON_GET_PLU = 0x96;
                public const byte LOGO_GET = 0x97;
                public const byte SHOP_GET = 0x98;
                public const byte DATE_SET = 0x99;
                public const byte TIME_SET = 0x9A;
                public const byte FACTORY_CONF_GET = 0x9B;
            }
        }

        public static class ReturnCode
        {
            public static string getDescription(int code)
            {
                switch (code)
                {
                    case SUCCESS: return "Операция прошла успешно";
                    case ERROR: return "Произошла ошибка";
                    case SCALE_ERROR: return "Весы сообщили об ошибке";
                    case BAD_HOST: return "Неверно указан адрес весов в сети";
                    case CONNECTION_NOT_SET: return "Соединение с весами не установлено.";
                    case WRONG_PLU_ID: return "Неверный номер записи о товаре";
                    case PLU_ID_ALREADY_EXISTS: return "Запись о товаре с таким номером уже существует";
                    
                    case CONNECTION_FAIL: return "Разрыв соединения.";
                    default: return "";
                }                
            }

            public const byte SUCCESS = 0;
            public const byte ERROR = 1;
            public const byte SCALE_ERROR = 2;
            public const byte BAD_HOST = 3;
            public const byte CONNECTION_NOT_SET = 4;
            public const byte WRONG_PLU_ID = 5;
            public const byte PLU_ID_ALREADY_EXISTS = 6;
            public const byte BAD_PARAMS = 7;
            public const byte WRONG_MSG_ID = 8;
            public const byte MSG_ID_ALREADY_EXIST = 9;
            public const byte PLU_IS_NOT_EXISTS = 10;
            public const byte MSG_IS_NOT_EXISTS = 11;
            public const byte CONNECTION_FAIL = 12;
            public const byte METHOD_NOT_SUPPORTED = 13;
        }


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
                    return BitHelper.concat(_id,_code, _prodName1, _prodName2, _price, _liveTime, _taraWeight, _groupCode, _msgId);
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

            public int Code
            {
                get { return BitHelper.fromBinDecimal(_code); }
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

                }
            }
            public int LiveTime
            {
                get
                {
                    byte[] bts = BitHelper.parseGroupBinDec(_liveTime);
                    return BitHelper.getLiveTimeDays(bts);
                }
                set
                {
                    if (value < 0 || value > 999) return;
                    byte[] bts = BitHelper.getLiveTimeBytes(value);
                    Array.Copy(BitHelper.makeGroupBinDec(bts), _liveTime, 3);
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
                get { return BitHelper.fromBinDecimal(_groupCode); }
                set
                {

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
                    byte[] bts = BitHelper.parseGroupBinDec(_lastClear);
                    return BitHelper.getLastClear(bts);
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
                //_id = getBytes(Info.Sizes.PLU.ID_ADDRESS, Info.Sizes.PLU.ID_LENGTH);
                //_code = getBytes(Info.Sizes.PLU.CODE_ADDRESS, Info.Sizes.PLU.CODE_LENGTH);
                //_prodName1 = getBytes(Info.Sizes.PLU.PRODUCT_NAME_1_ADDRESS, Info.Sizes.PLU.PRODUCT_NAME_LENGTH); 
                //_prodName2 = getBytes(Info.Sizes.PLU.PRODUCT_NAME_2_ADDRESS, Info.Sizes.PLU.PRODUCT_NAME_LENGTH);
                //_price = getBytes(Info.Sizes.PLU.PRICE_ADDRESS, Info.Sizes.PLU.PRICE_LENGTH); 
                //_liveTime = getBytes(Info.Sizes.PLU.LIVETIME_ADDRESS, Info.Sizes.PLU.LIVETIME_LENGTH); ;
                //_taraWeight = getBytes(Info.Sizes.PLU.TARA_WEIGHT_ADDRESS, Info.Sizes.PLU.TARA_WEIGHT_LENGHT);
                //_groupCode = getBytes(Info.Sizes.PLU.GROUP_CODE_ADDRESS, Info.Sizes.PLU.GROUP_CODE_LENGTH);
                //_msgId = getBytes(Info.Sizes.PLU.MESSAGE_ID_ADDRESS, Info.Sizes.PLU.MESSAGE_ID_LENGTH);
                //_lastClear = getBytes(Info.Sizes.PLU.LAST_CLEAR_ADDRESS, Info.Sizes.PLU.LAST_CLEAR_LENGTH);
                // _totalSumm = getBytes(Info.Sizes.PLU.TOTAL_SUMM_ADDRESS, Info.Sizes.PLU.TOTAL_SUMM_LENGTH);
                //_totalWeight = getBytes(Info.Sizes.PLU.TOTAL_WEIGHT_ADDRESS, Info.Sizes.PLU.TOTAL_WEIGHT_LENGTH);
                //_totalSell = getBytes(Info.Sizes.PLU.TOTAL_SELL_ADDRESS, Info.Sizes.PLU.TOTAL_SELL_LENGTH);

            }
            public PLU() { }

            /*private byte[] getBytes(byte startByte, byte lenght)
            {
                byte[] result = new byte[lenght];
                Array.Copy(_bytes, startByte, result, 0, lenght);
                return result;
            }*/


        }

        public class MSG
        {
            private readonly byte[] _msg = new byte[Info.Sizes.MSG_LENGTH];
            private readonly byte[] _id = new byte[2];

            public string Text
            {
                get { return ToString(); }
                set { setMessage(value); }
            }
            public int ID
            {
                get { return (int)BitConverter.ToInt16(_id, 0); }
                set 
                {
                    if (value < 1 || value > 1000) return;
                    byte[] bid = BitConverter.GetBytes(value);
                    Array.Copy(bid, _id, 2);
                }
            }

            public byte[] Bytes
            {
                get { return _msg; }
            }

            #region strings
            public string String1
            {
                get { return getRowString(0); }
                set { setRowString(0, value); }
            }

            public string String2
            {
                get { return getRowString(1); }
                set { setRowString(1, value); }
            }

            public string String3
            {
                get { return getRowString(2); }
                set { setRowString(2, value); }
            }

            public string String4
            {
                get { return getRowString(3); }
                set { setRowString(3, value); }
            }
            public string String5
            {
                get { return getRowString(4); }
                set { setRowString(4, value); }
            }
            public string String6
            {
                get { return getRowString(5); }
                set { setRowString(5, value); }
            }
            public string String7
            {
                get { return getRowString(6); }
                set { setRowString(6, value); }
            }
            public string String8
            {
                get { return getRowString(7); }
                set { setRowString(7, value); }
            }
            #endregion strings
            public MSG(int id,string Message)
            {
                ID = id;
                Text = Message;
            }
            public MSG(int id,byte[] bytes)
            {
                if (bytes.Length < 400) return;
                _id = BitConverter.GetBytes(id);
                Array.Copy(bytes, _msg, _msg.Length);
            }

            public override string ToString()
            {
                string result = "";
                for (int row = 0; row < Info.Sizes.MSG_MAX_STRINGS_COUNT; row++)
                {
                    byte[] btF = new byte[Info.Sizes.MSG_MAX_STRING_LENGTH];
                    int from = row * Info.Sizes.MSG_MAX_STRING_LENGTH;
                    int until = (row + 1) * Info.Sizes.MSG_MAX_STRING_LENGTH;
                    /*int empStart = -1;
                    //Вычисялем заполнен ли конец пробелами чтобы заменить переходом на новую строку
                    for (int i = from; i < until; i++)
                    {                     
                        if (_msg[i] == 0x20 ||_msg[i]==0)
                            empStart = i;
                        else empStart = -1;
                    }
                    if (empStart != -1)
                        btF = _msg.Skip(from).Take(empStart - from).ToArray();
                    else*/
                    //btF = _msg.Skip(from).Take(Info.Sizes.MSG_MAX_STRING_LENGTH).ToArray();
                    Array.Copy(_msg, from, btF, 0, Info.Sizes.MSG_MAX_STRING_LENGTH);
                    result += Encoding.GetEncoding(866).GetString(btF).TrimEnd(new char[]{' '}).Replace("\0","") ;
                    if (!BitHelper.arrayIsEmpty(btF))
                        result += Environment.NewLine;
                }
                return result;
            }

            /// <summary>
            /// Возвращает одну строку по индексу. 
            /// </summary>
            /// <param name="row">Индекс от 0 до 7</param>
            /// <returns></returns>
            private string getRowString(int row)
            {
                if (row < 0 && row > 7) return "";
                byte[] bts = new byte[Info.Sizes.MSG_MAX_STRING_LENGTH];
                Array.Copy(_msg, row * Info.Sizes.MSG_MAX_STRING_LENGTH, bts, 0, Info.Sizes.MSG_MAX_STRING_LENGTH);
                //_msg.Skip(row * Info.Sizes.MSG_MAX_STRING_LENGTH).Take(Info.Sizes.MSG_MAX_STRING_LENGTH).ToArray();          
                return Encoding.GetEncoding(866).GetString(bts).Replace("\0","");
            }

            /// <summary>
            /// Записывает строку в Массив байтов
            /// </summary>
            /// <param name="row">Индекс от 0 до 7</param>
            /// <param name="rowText">Текст строки</param>
            private void setRowString(int row, string rowText)
            {
                if ((row < 0 && row > 7 ) || rowText == null) return;
                rowText = rowText.TrimEnd(new char[]{' '}).Replace(Environment.NewLine, "");
                if (rowText.Length > 50)
                    rowText = rowText.Substring(0, 50);
                byte[] bts = Encoding.GetEncoding(866).GetBytes(rowText);
                int j = 0;
                int from = row * Info.Sizes.MSG_MAX_STRING_LENGTH;
                int until = (row + 1) * Info.Sizes.MSG_MAX_STRING_LENGTH;
                for (int i = from; i < until; i++)
                {
                    if (bts.Length > (i-from))
                    {
                        _msg[i] = bts[j];
                        j++;
                    }
                    else 
                        _msg[i] = 0;//ноль
                }
            }

            private void setMessage(string text)
            {
                Array.Clear(_msg, 0, _msg.Length);
                text = text.Trim();
                string[] strings = text.Split(new string[] { Environment.NewLine }, 8, StringSplitOptions.RemoveEmptyEntries);
                if (strings.Length > 1)
                {
                    for (int row = 0; row < strings.Length; row++)
                        setRowString(row, strings[row]);
                }
                else
                {
                    if(text.Length>400)
                        text = text.Substring(0, 400);
                    string[] strs = new string[8];
                    for (int row = 0; row < strs.Length; row++)
                    {
                        if (text.Length > 50)
                        {
                            strs[row] = text.Substring(0, 50).Trim();
                            text =text.Remove(0, 50);
                        }
                        else
                        {
                            strs[row] = text.Substring(0, text.Length).Trim();
                            break;
                        }
                    }
                    for (int row = 0; row < strs.Length; row++)
                        setRowString(row, strs[row]);
                }
            }

        }

        public class Summary
        {
            private readonly byte[] _bytes = new byte[Info.Sizes.SUMMARY_LENGTH];
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
                if (bts.Length < 100) return;
                Array.Copy(bts, _bytes, Info.Sizes.SUMMARY_LENGTH);

                Array.Copy(_bytes, Info.Sizes.Summary.ROLL_ADDRESS, _roll, 0, Info.Sizes.Summary.ROLL_LENGTH);
                Array.Copy(_bytes, Info.Sizes.Summary.STICKER_ADDRESS, _sticker, 0, Info.Sizes.Summary.STICKER_LENGTH);
                Array.Copy(_bytes, Info.Sizes.Summary.SUMM_ADDRESS, _summ, 0, Info.Sizes.Summary.SUMM_LENGTH);
                Array.Copy(_bytes, Info.Sizes.Summary.SELL_ADDRESS, _sell, 0, Info.Sizes.Summary.SELL_LENGTH);
                Array.Copy(_bytes, Info.Sizes.Summary.WEIGHT_ADDRESS, _weight, 0, Info.Sizes.Summary.WEIGHT_LENGTH);
                Array.Copy(_bytes, Info.Sizes.Summary.ALL_PLU_SUMM_ADDRESS, _allPluSumm, 0, Info.Sizes.Summary.ALL_PLU_SUMM_LENGTH);
                Array.Copy(_bytes, Info.Sizes.Summary.ALL_PLU_SELL_ADDRESS, _allPluSell, 0, Info.Sizes.Summary.ALL_PLU_SELL_LENGTH);
                Array.Copy(_bytes, Info.Sizes.Summary.ALL_PLU_WEIGHT_ADDRESS, _allPluWeight, 0, Info.Sizes.Summary.ALL_PLU_WEIGHT_LENGTH);
                Array.Copy(_bytes, Info.Sizes.Summary.LAST_CLEAR_ADDRESS, _lastClear, 0, Info.Sizes.Summary.LAST_CLEAR_LENGTH);
                Array.Copy(_bytes, Info.Sizes.Summary.FREE_PLU_ADDRESS, _freePlu, 0, Info.Sizes.Summary.FREE_PLU_LENGTH);
                Array.Copy(_bytes, Info.Sizes.Summary.FREE_MSG_ADDRESS, _freeMsg, 0, Info.Sizes.Summary.FREE_MSG_LENGTH);
            }
        }
    }


    //Реализация самого класса
    public sealed partial class CasLP16
    {
        private static CasLP16 _instance = null;
        /// <summary>
        /// Должен быть создан лишь один класс CasLP16
        /// </summary>
        public static CasLP16 Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CasLP16();
                return _instance;
            }
        }

        private int _id = 1;
        private string _host = "192.168.0.5";
        private int _port = 8111;
        private int _loadProgress = -1;
        private bool _loading = false;
        private Thread _timer = new Thread(timerStart);
        private NetworkStream _nwStream;
        private TcpClient _tcpClient;
        //private byte[] _bytesForRead;
        private readonly PLUList _pluList = new PLUList();
        private readonly MSGList _msgList = new MSGList();

        public int ScaleNumber
        {
            get { return _id; }
            set
            {
                if (value < 0 || value > 99) return;
                _id = value;
            }
        }
        public string Host
        {
            get { return _host; }
            set { SetScaleAddress(value, _port); }
        }
        public int Port
        {
            get { return _port; }
            set { SetScaleAddress(_host, value); }
        }
        public int LoadProgress
        {
            get { return _loadProgress; }
        }
        /// <summary>
        /// Идет ли загрузка данных из весов.
        /// </summary>
        public bool Loading
        {
            get { return _loading; }
            set { _loading = value; }
        }
        public int PLUCount
        {
            get { return _pluList.Count; }
        }
        public int MSGCount
        {
            get { return _msgList.Count; }
        }
        //public PLUList PluList{get { return _pluList; }}
        //public MSGList MsgList{get { return _msgList; }}
        public bool Connected
        {
            get 
            {
                if (_tcpClient == null)
                    return false;
                else return _tcpClient.Connected;
            }
        }
        public CasLP16() { }
        ~CasLP16()
        {
            this.Disconnect();
        }

        public int[] getIDsOfPLUs()
        {
            return _pluList.getIds();
        }

        public int[] getIDsOfMSGs()
        {
            return _msgList.getIds();
        }

        private void runTimer()
        {
            _timer = new Thread(timerStart);
            _timer.IsBackground = true;
            _timer.Start();
        }

        private static void timerStart()
        {
            Thread.Sleep(220);//положено 200
        }

        private void connectWaiter(IAsyncResult res)
        {
            TcpClient clnt = (TcpClient)res.AsyncState;
            if (clnt.Client != null)
            {
                try
                {
                    clnt.EndConnect(res);
                    _nwStream = clnt.GetStream();
                    _nwStream.ReadTimeout = 10000;
                }
                catch(SocketException ex)
                {
                    this.Disconnect();
                }
            }
        }

        private int beginSession()
        {
#if !NOCATCH
            try
            {
#endif
            while (_timer.IsAlive)
                    Thread.Sleep(20);
                if (_tcpClient == null || !_tcpClient.Connected) return ReturnCode.CONNECTION_NOT_SET;
                _nwStream.WriteByte((byte)_id);
                byte[] ans = new byte[2];
                _nwStream.Read(ans, 0, ans.Length);
                runTimer();
                if (ans[1] == Info.Ansvers.ERROR)
                    return ReturnCode.SCALE_ERROR;
                else return ReturnCode.SUCCESS;
            #if !NOCATCH
            }
            catch (IOException)
            {
                return ReturnCode.CONNECTION_FAIL;
            }
#endif
        }

        /// <summary>
        /// Выполняет команду, на которую весы не возвращают данные
        /// </summary>
        private int execCommand(byte[] bts)
        {
            byte[] doodle = new byte[1];
            return execCommand(bts, ref doodle, 0);
        }
        /// <summary>
        /// Выполняет команду весов
        /// </summary>
        /// <param name="bts">Массив байтов соманды.</param>
        /// <param name="answer">Информация, полученнаяф от весов</param>
        /// <param name="offset">Смещение. С какого байта начинаются данные.</param>
        /// <returns>Код состояния</returns>
        private int execCommand(byte[] bts, ref byte[] answer, int offset)
        {
#if !NOCATCH
            try
            {
#endif
                int result = beginSession();
                if (result != ReturnCode.SUCCESS) return result;
                _nwStream.Write(bts, 0, bts.Length);
                byte[] ans = new byte[answer.Length + offset];
                int a = 0;
                int i = 0;
                while (i < ans.Length && a != -1)//альтернатива Read, т.к он не читал полностью месседж
                {
                    a = _nwStream.ReadByte();
                    if (a != -1)
                        ans[i] = (byte)a;
                    if (i == 0 && a == Info.Ansvers.ERROR) break;
                    i++;
                }
                //_nwStream.Read(ans, 0,401);
                //System.Diagnostics.Debug.WriteLine("Scale ans:"+ans[0],"execComand");
                if (ans[0] == Info.Ansvers.ERROR)
                    return ReturnCode.SCALE_ERROR;
                else //if (ans[0] == Info.Ansvers.SUCCESS)
                {
                    Array.Copy(ans, offset, answer, 0, answer.Length);
                    return ReturnCode.SUCCESS;
                }
#if !NOCATCH
            }
            catch (IOException)
            {
                return ReturnCode.CONNECTION_FAIL;
            }
#endif
        }

        private int getPLUbyID(int id, out PLU plu)
        {
            byte[] bid = BitConverter.GetBytes(id);
            byte[] bplu = new byte[Info.Sizes.PLU_LENGTH];
            byte[] cmd = new byte[5];
            cmd[0] = Info.Commands.PLU_GET;
            Array.Copy(bid, 0, cmd, 1, 4);
            int result = execCommand(cmd, ref bplu, 1);
            if (result != ReturnCode.SUCCESS)
            {
                plu = null;
                return result;
            }
            else
            {
                plu = new PLU(bplu);
                return result;
            }
        }

        private int getMSGbyID(int id, out MSG msg)
        {
            msg = null;
            if (id < 1 || id > 1000) return ReturnCode.BAD_PARAMS;
            byte[] bid = BitConverter.GetBytes(id);
            byte[] bmsg = new byte[Info.Sizes.MSG_LENGTH];
            byte[] cmd = new byte[3];
            cmd[0] = Info.Commands.MSG_GET;
            Array.Copy(bid, 0, cmd, 1, 2);
            int result = execCommand(cmd, ref bmsg, 1);          
            if (result != ReturnCode.SUCCESS)                           
                return result;           
            else
            {
                if(!BitHelper.arrayIsEmpty(bmsg))
                    msg = new MSG(id,bmsg);
                return result;
            }
        }

        private int savePLU(PLU plu)
        {
            byte[] cmd = new byte[Info.Sizes.Commands.PLU_SET];
            cmd[0] = Info.Commands.PLU_SET;
            Array.Copy(plu.Bytes, 0, cmd, 1, plu.Bytes.Length);
            return execCommand(cmd);
        }

        private int saveMSG(MSG msg)
        {
            byte[] cmd = new byte[Info.Sizes.Commands.MSG_SET];
            cmd[0] = Info.Commands.MSG_SET;
            Array.Copy(BitConverter.GetBytes(msg.ID), 0, cmd, 1, 2);
            Array.Copy(msg.Bytes, 0, cmd, 3, 400);
            return execCommand(cmd);
        }

        private int deletePLU(PLU plu)
        {
            byte[] cmd = new byte[Info.Sizes.Commands.PLU_CLEAR];
            cmd[0] = Info.Commands.PLU_CLEAR;
            Array.Copy(BitConverter.GetBytes(plu.ID), 0, cmd, 1, 4);
            return execCommand(cmd);
        }

        private int deleteMSG(MSG msg)
        {
            byte[] cmd = new byte[Info.Sizes.Commands.MSG_CLEAR];
            cmd[0] = Info.Commands.MSG_CLEAR;
            Array.Copy(BitConverter.GetBytes(msg.ID), 0, cmd, 1, 2);
            return execCommand(cmd);
        }

        public int SetScaleAddress(string host, int port)
        {
#if !NOCATCH
            try
            {
#endif
            IPAddress.Parse(host);
            _host = host;
            _port = port;
            return ReturnCode.SUCCESS;
#if !NOCATCH
            }
            catch(Exception)
            {
                return ReturnCode.ERROR;
            }
#endif
        }

        /// <summary>
        /// Устанавливает соединение с весами
        /// </summary>
        /// <returns>Код ошибки</returns>
        public int Connect()
        {
#if !NOCATCH
            try
            {
#endif
            IPEndPoint pnt = new IPEndPoint(IPAddress.Parse(_host), _port);
            _tcpClient = new TcpClient();
            //_tcpClient.BeginConnect(pnt.Address, pnt.Port, new AsyncCallback(connectWaiter), _tcpClient);
            _tcpClient.Connect(pnt);
            _nwStream = _tcpClient.GetStream();
            return ReturnCode.SUCCESS;
#if !NOCATCH
            }
            catch
            {
                return ReturnCode.ERROR;
            }
#endif
        }
        public int Connect(string host, int port)
        {
            SetScaleAddress(host, port);
            return Connect();
        }

        public int Disconnect()
        {
#if !NOCATCH
            try
            {
#endif
            if (_nwStream != null)
            {
                _nwStream.Close();
                _nwStream = null;
            }
            if (_tcpClient != null)
            {
                _tcpClient.Close();
                _tcpClient = null;
            }
            return ReturnCode.SUCCESS;
#if !NOCATCH
            }
            catch (Exception)
            {
                return ReturnCode.ERROR;
            }
#endif
        }

        public int LoadPLUs() { return LoadPLUs(0, Info.Sizes.PLU_MAX_INDEX); }
        public int LoadPLUs(int from, int until)
        {
            if (from >= until) return ReturnCode.BAD_PARAMS;
            int[]ids = new int[until-from+1];
            for (int i = from; i <= until; i++)
                ids[i - from] = i;
            return LoadPLUs(ids);
            /*_pluList.Clear();
            int result = beginSession();
            if (result != ReturnCode.SUCCESS) return result;
            _loadProgress = 0;
            _loading = true;
            for (int i = from; i <= until; i++)
            {
#if !NOCATCH
                try
                {
#endif
                if (!_loading) break;
                PLU plu;
                getPLUbyID(i, out plu);
                if (plu != null)
                    _pluList.Add(plu);
                _loadProgress = (until - from) / 100 * (i - from);
#if !NOCATCH
                }
                catch(Exception ex) 
                {
 
                }
#endif
            }
            _loading = false;
            _loadProgress = -1;
            return ReturnCode.SUCCESS;*/
        }
        public int LoadPLUs(params int[] IDs)
        {
            _pluList.Clear();
            int result = beginSession();
            if (result != ReturnCode.SUCCESS) return result;
            //_loadProgress = 0;
            _loading = true;
            for (int i = 0; i < IDs.Length; i++)
            {
                if (IDs[i] < 0 || IDs[i] > Info.Sizes.PLU_MAX_INDEX) continue;
#if !NOCATCH
                try
                {
#endif
                if (!_loading) break;
                PLU plu;
                getPLUbyID(IDs[i], out plu);
                if (plu != null)
                    _pluList.Add(plu);
                //_loadProgress = ;
#if !NOCATCH
                }
                catch(Exception ex) 
                {
 
                }
#endif
            }
            _loading = false;
            //_loadProgress = -1;
            return ReturnCode.SUCCESS;
        }

        /// <summary>
        /// Проверяет есть ли в списке PLU c данным ID.
        /// Если нет, то пытается загрузить с весов.
        /// Если в весах есть - добавлет в список, если нет - возвращает  null
        /// </summary>
        public PLU GetPLUbyID(int id)
        {
            foreach (PLU plu in _pluList)
            {
                if (plu.ID == id)
                    return plu;
            }
            PLU newplu;
            getPLUbyID(id, out newplu);
            if (newplu != null)
                _pluList.Add(newplu);
            return newplu;
        }

        public int SavePLUbyID(int id)
        {
            PLU save = _pluList.GetPLU(id);
            if (save == null) return ReturnCode.WRONG_PLU_ID;
            return savePLU(save);
        }

        public int AddPLU(PLU newplu)
        {
            if (GetPLUbyID(newplu.ID) != null) return ReturnCode.PLU_ID_ALREADY_EXISTS;
            _pluList.Add(newplu);
            return savePLU(newplu);
        }

        public int DeletePLUbyID(int id)
        {
            PLU plu = GetPLUbyID(id);
            if ( plu == null) return ReturnCode.PLU_IS_NOT_EXISTS;

            _pluList.Remove(plu);
            return deletePLU(plu);         
        }

        public int LoadMSGs() { return LoadMSGs(0, Info.Sizes.MSG_MAX_INDEX); }
        public int LoadMSGs(int from, int until)
        {
            if (from >= until) return ReturnCode.BAD_PARAMS;
            int[] ids = new int[until - from + 1];
            for (int i = from; i <= until; i++)
                ids[i - from] = i;
            return LoadMSGs(ids);
            /*_msgList.Clear();
            int result = beginSession();
            if (result != ReturnCode.SUCCESS) return result;
            _loadProgress = 0;
            _loading = true;
            for (int i = from; i <= until; i++)
            {
#if !NOCATCH
                try
                {
#endif
                if (!_loading) break;
                MSG msg;
                getMSGbyID(i, out msg);
                if (msg != null && msg.Text !="")
                    _msgList.Add(msg);
                _loadProgress = (until - from) / 100 * (i - from);
#if !NOCATCH
                }
                catch(Exception ex) 
                {
 
                }
#endif
            }
            _loading = false;
            _loadProgress = -1;
            return ReturnCode.SUCCESS;*/
        }
        public int LoadMSGs(params int[] IDs)
        {
            _msgList.Clear();
            int result = beginSession();
            if (result != ReturnCode.SUCCESS) return result;
            //_loadProgress = 0;
            _loading = true;
            for (int i = 0; i < IDs.Length; i++)
            {
                if (IDs[i] < 0 || IDs[i] > Info.Sizes.MSG_MAX_INDEX) continue;
#if !NOCATCH
                try
                {
#endif
                if (!_loading) break;
                MSG msg;
                getMSGbyID(IDs[i], out msg);
                if (msg != null)
                    _msgList.Add(msg);
                //_loadProgress = ;
#if !NOCATCH
                }
                catch(Exception ex) 
                {
 
                }
#endif
            }
            _loading = false;
            //_loadProgress = -1;
            return ReturnCode.SUCCESS;
        }
        public MSG GetMSGbyID(int id)
        {
            if (id < 1 || id > 1000) return null;
            foreach (MSG msg in _msgList)
            {
                if (msg.ID == id)
                    return msg;
            }
            MSG newmsg;
            getMSGbyID(id, out newmsg);
            if (newmsg != null)
                _msgList.Add(newmsg);
            return newmsg;
        }

        public int SaveMSGbyID(int id)
        {
            MSG save = _msgList.GetMSG(id);
            if (save == null) return ReturnCode.WRONG_PLU_ID;
            return saveMSG(save);
        }

        public int AddMSG(MSG newmsg)
        {
            if (GetMSGbyID(newmsg.ID) != null) return ReturnCode.MSG_ID_ALREADY_EXIST;
            _msgList.Add(newmsg);
            return saveMSG(newmsg);
        }

        public int DeleteMSGbyID(int id)
        {
            return;// НА весах пишется "POC",вводишь 2симвОла, пишется лого.При этом данные не отдает
            MSG msg = GetMSGbyID(id);
            if (msg == null) return ReturnCode.MSG_IS_NOT_EXISTS;
            _msgList.Remove(msg);
            return deleteMSG(msg);
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
    }

}

