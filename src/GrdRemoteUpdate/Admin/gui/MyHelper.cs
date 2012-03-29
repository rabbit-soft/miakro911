using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AdminGRD
{
    public static class MyHelper
    {
        /// <summary>
        /// Нужна для востановления позиции.
        /// <remarks>Сделана статической в целях оптимизации</remarks>
        /// </summary>
        private static int _position;
        private static bool _block = false;

        public static Color ErrorColor { get { return Color.DarkSalmon; } }
        public static Color WarningColor { get { return Color.Yellow; } }
        public static Color SuccessColor { get { return Color.PaleGreen; } }
        public static Color NormalColor { get { return SystemColors.Window; } }

        public static Brush ErrorBrush { get { return Brushes.DarkSalmon; } }
        public static Brush SuccessBrush { get { return Brushes.PaleGreen; } }
        public static Brush WarningBrush { get { return Brushes.Yellow; } }
        public static Brush NormalBrush { get { return SystemBrushes.Window; } }

        public static Color GroupColor { get { return Color.LightSteelBlue; } }
        public static Color UnregistredStation { get { return Color.BurlyWood; } }

        public static Color Darker(Color clr) 
        {
            if (clr == Color.Empty)
                return Color.LightGray;
            return ControlPaint.Dark(clr); 
        }
        public static Color Lighter(Color clr) 
        {
            if (clr == Color.Empty)
                return clr;
            return ControlPaint.Light(clr); 
        }

        public static bool IsValidNumeric(string value)
        {
            foreach (char c in value)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Проверяет текст Контрола. Только ли цифры. Если нет - то удаляет недопустимые символы
        /// </summary>
        /// <param name="sender">Control</param>
        public static void OnlyDecimals(object sender)
        {
            if (_block) return;//Блокировка от рекурсии
            _block = true;

            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                try
                {
                    _position = tb.SelectionStart;
                    tb.Text = int.Parse(tb.Text).ToString();
                    if (_position <= tb.Text.Length)
                        tb.Select(_position, 0);
                    else
                        tb.Select(tb.Text.Length, 0);
                }
                catch (FormatException)
                {
                    if (tb.Text != "")
                    {
                        for (int i = 0; i < tb.Text.Length; i++)
                        {
                            if (tb.Text[i] < '0' || tb.Text[i] > '9')
                            {
                                tb.Text = tb.Text.Remove(i, 1);
                                tb.Select(i, 0);
                                break;
                            }
                        }
                    }
                    else
                    {
                        tb.Text = "0";
                        tb.Select(1, 0);
                    }
                }
            }

            _block = false;
        }

        public static string InsertDelimiter(string str, string del, int each)
        {
            int pos = each;
            while (str.Length > pos + 1)
            {
                str=str.Insert(pos, del);
                pos += 1 + each;
            }
            return str;
        }

    }
}
