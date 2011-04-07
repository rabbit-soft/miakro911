using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

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
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
            else
            {
                return "nofile";
            }
        }

        public static string RusMonthTo(string mon)
        {
            for (int i = 1; i <= 12; i++)
            {
                if (mon == toRusMonth(i.ToString()))
                {
                    return i.ToString();
                }
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
