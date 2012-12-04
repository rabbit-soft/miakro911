using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

namespace gamlib
{
    public static partial class Helper
    {
        public static void checkIntNumber(object sender, EventArgs e)
        {
            (sender as TextBox).Text = (sender as TextBox).Text.Replace(",", "");
            checkFloatNumber(sender, e);
        }

        public static void checkFloatNumber(object sender, EventArgs e)
        {
            if (!(sender is TextBox)) return;
            List<char> numbers = new List<char>();
            numbers.Add('0');
            numbers.Add('1');
            numbers.Add('2');
            numbers.Add('3');
            numbers.Add('4');
            numbers.Add('5');
            numbers.Add('6');
            numbers.Add('7');
            numbers.Add('8');
            numbers.Add('9');
            numbers.Add(',');
            TextBox tb = (sender as TextBox);
            if (tb.Text.Length != 0 && tb.Text[0] == ',')
            {
                tb.Text = tb.Text.Insert(0, "0");
                tb.Select(tb.Text.Length, 0);
            }
            bool haveComma = false;
            if (tb.Text != "")
            {
                int i = 0;
                while (i < tb.Text.Length)
                {
                    if (tb.Text[i] == ',')
                    {
                        if (haveComma)
                        {
                            tb.Text = tb.Text.Remove(i);
                            continue;
                        }
                        else haveComma = true;
                    }
                    if (tb.Text.Length > 1 && i == 0 && tb.Text[i] == '0' && tb.Text[i + 1] != ',')
                    {
                        tb.Text = tb.Text.Remove(i, 1);
                        tb.Select(i, 0);
                        continue;
                    }
                    if (!numbers.Contains(tb.Text[i]))//удал€ем недопустимые символы
                    {
                        tb.Text = tb.Text.Remove(i, 1);
                        tb.Select(i, 0);//если символ кос€чный,значит курсор стоит на нем
                        continue;
                    }

                    i++;
                }
            }
        }


    }
}