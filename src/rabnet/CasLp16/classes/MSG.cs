using System;
using System.Collections.Generic;
using System.Text;
using gamlib;

namespace CAS
{
    /// <summary>
    /// Сообщение
    /// </summary>
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
        public bool Delete = false;

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
        public MSG(int id, string Message)
        {
            ID = id;
            Text = Message;
        }
        public MSG(int id, byte[] bytes)
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
                if (!BitHelper.ArrayIsEmpty(btF))
                {
                    if (row != 0) result += Environment.NewLine;
                    result += Encoding.GetEncoding(866).GetString(btF).TrimEnd(new char[] { ' ' }).Replace("\0", "");
                }
            }
            return result;
            //return result.Remove(result.LastIndexOf(Environment.NewLine));
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
            return Encoding.GetEncoding(866).GetString(bts).Replace("\0", "");
        }

        /// <summary>
        /// Записывает строку в Массив байтов
        /// </summary>
        /// <param name="row">Индекс от 0 до 7</param>
        /// <param name="rowText">Текст строки</param>
        private void setRowString(int row, string rowText)
        {
            if ((row < 0 && row > 7) || rowText == null) return;
            rowText = rowText.TrimEnd(new char[] { ' ' }).Replace(Environment.NewLine, "");
            if (rowText.Length > 50)
                rowText = rowText.Substring(0, 50);
            byte[] bts = Encoding.GetEncoding(866).GetBytes(rowText);
            int j = 0;
            int from = row * Info.Sizes.MSG_MAX_STRING_LENGTH;
            int until = (row + 1) * Info.Sizes.MSG_MAX_STRING_LENGTH;
            for (int i = from; i < until; i++)
            {
                if (bts.Length > (i - from))
                {
                    _msg[i] = bts[j];
                    j++;
                }
                else
                    _msg[i] = 0;//ноль
            }
        }

        /// <summary>
        /// Устанавливает текст сообщения
        /// </summary>
        private void setMessage(string text)
        {
            Array.Clear(_msg, 0, _msg.Length);
            text = text.Trim();
            string[] lines = text.Split(new string[] { Environment.NewLine }, 8, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 1)
            {
                for (int row = 0; row < lines.Length; row++)
                    setRowString(row, lines[row].Substring(0, lines[row].Length > 50 ? 50 : lines[row].Length));
            }
            else
            {
                if (text.Length <= 50)
                    setRowString(0, text);
                else
                {
                    if (text.Length > 400)
                        text = text.Substring(0, 400);
                    string[] strs = new string[8];
                    for (int row = 0; row < strs.Length; row++)
                    {
                        if (text.Length > 50)
                        {
                            strs[row] = text.Substring(0, 50).Trim();
                            text = text.Remove(0, 50);
                        }
                        else
                        {
                            strs[row] = text.Trim();
                            break;
                        }
                    }
                    for (int row = 0; row < strs.Length; row++)
                        setRowString(row, strs[row]);
                }
            }
        }

    }

    public class MSGList : List<MSG>
    {
        /// <summary>
        /// Возвращает сообщение с конкретным ID
        /// </summary>
        /// <param name="id">ID сообщения</param>
        internal MSG GetMSG(int id)
        {
            foreach (MSG msg in this)
                if (msg.ID == id)
                    return msg;
            return null;
        }

        /// <summary>
        /// Получить массив из ID сообщений
        /// </summary>
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
}
