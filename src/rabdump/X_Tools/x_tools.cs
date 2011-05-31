using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace X_Tools
{
    static class XTools
    {
        static public bool IsFilenameValid(string inputFileName)
        {
            Match m = Regex.Match(inputFileName, @"[\\\/\:\*\?\" + Convert.ToChar(34) + @"\<\>\|]");
            return !(m.Success);
        }

        static public string FormatBytes(double bytes)
        {
            string[] suffix = { "B", "KB", "MB", "GB", "TB", "PB" };
            int i;
            double dblSByte = 0;
            for (i = 0; (int)(bytes / 1024) > 0; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }
            return String.Format("{0:0.00} {1}", dblSByte, suffix[i]);
        }

        public static string SafeFileName(string fileName, string replacer)
        {
            return Regex.Replace(fileName, "[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]", replacer, RegexOptions.Compiled);
        }

        public static string SafeFileName(string fileName)
        {
            return SafeFileName(fileName, string.Empty);
        }

        public static bool VerifyMD5(string file, string hash)
        {
            if (File.Exists(file))
            {
                string computed = GetFileMD5(file);
                if (computed == hash)
                    return true;
                else return false;
            }
            else return false;           
        }

        public static string GetFileMD5(string file)
        {
            MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider();
            if (File.Exists(file))
            {
                FileStream fs = File.OpenRead(file);
                byte[] fileHash = csp.ComputeHash(fs);
                fs.Close();

                return BitConverter.ToString(fileHash).Replace("-", "").ToLower();
            }
            else return "nofile";
            
        }

        public static string RusMonthTo(string mon)
        {
            for (int i = 1; i <= 12; i++)
            {
                if (mon == toRusMonth(i.ToString()))
                    return i.ToString();            
            }
            return "0";
        }

        public static string toRusMonth(string dt)
        {
            try
            {
                string result = "";
                switch (int.Parse(dt))
                {
                    case 1: result += "Январь "; break;
                    case 2: result += "Февраль "; break;
                    case 3: result += "Март "; break;
                    case 4: result += "Апрель "; break;
                    case 5: result += "Май "; break;
                    case 6: result += "Июнь "; break;
                    case 7: result += "Июль "; break;
                    case 8: result += "Август "; break;
                    case 9: result += "Сентябрь "; break;
                    case 10: result += "Октябрь "; break;
                    case 11: result += "Ноябрь "; break;
                    case 12: result += "Декабрь "; break;
                }
                return result;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Проверяет есть ли указанный файл, если есть то возвращает имя 'Файл (1)'
        /// </summary>
        /// <param name="s">Полный путь</param>
        public static string DuplicateName(string s)
        {
            int i = 1;
            string path = s.Remove(s.LastIndexOf('.'));
            string ext = s.Substring(s.LastIndexOf('.'));
            string append = "";
            while(true)
            {
                if (File.Exists(path + append + ext))
                {
                    append = " (" + i.ToString() + ")";
                    i++;
                }
                else return path + append + ext;
            }
        }

        public static void checkFloatNumber(object sender,EventArgs e)
        {
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
            try
            {
                if (tb.Text.Length != 0 && tb.Text[0] == ',')
                {
                    tb.Text = tb.Text.Insert(0, "0");
                    tb.Select(tb.Text.Length , 0);
                }
                float.Parse(tb.Text);              
            }
            catch (FormatException)
            {
                bool haveComma = false;
                if (tb.Text != "")
                {
                    for (int i = 0; i < tb.Text.Length; i++)
                    {
                        if (tb.Text[i] == ',')
                        {
                            if (haveComma)
                            {
                                tb.Text = tb.Text.Remove(i);
                                break;
                            }
                            else haveComma = true;
                        }

                        if (!numbers.Contains(tb.Text[i]) )
                        {
                            tb.Text = tb.Text.Remove(i, 1);
                            tb.Select(i, 0);
                            break;
                        }
                    }
                }
            }
        }

        // readStream is the stream you need to read
        // writeStream is the stream you want to write to
/*
        static public void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            const int length = 256;
            Byte[] buffer = new Byte[length];
            int bytesRead = readStream.Read(buffer, 0, length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, length);
            }
            readStream.Close();
            writeStream.Close();
        }
*/
        
    }
}
