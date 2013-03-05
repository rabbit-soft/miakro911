using System;
using System.IO;
using System.Text;

namespace gamlib
{
    public static partial class Helper
    {
        public static string GetMD5FromFile(string filepath)
        {
            if (filepath == "") return "0";
            FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            return ByteArrayToHexString(retVal);
        }

        public static byte[] GetMD5(byte[] buff)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            return md5.ComputeHash(buff);
        }

        public static bool IsInteger(string str)
        {
            try
            {
                int.Parse(str);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static int N3Ppath_Compare(ref string p1, ref string p2)
        {
            int temp = countOccurencesOfChar(ref p1, '.').CompareTo(countOccurencesOfChar(ref p2, '.'));
            if (temp == 1)//p1 имеет больше точек, чем p2
                return -1;
            else if (temp == -1)
                return 1;
            else return 0;
        }

        private static int countOccurencesOfChar(ref string instance, char c)
        {
            int temp = 0;
            foreach (char curChar in instance)
                if (c == curChar)
                    ++temp;
            return temp;
        }

        public static string ByteArrayToHexString(byte[] arr)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                sb.Append(arr[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Сравнивает версии и возвращает результат сравнения
        /// </summary>
        /// <param name="ver1">Версия первого файла</param>
        /// <param name="ver2">Версия второго файла</param>
        /// <returns>1- Первый больше второго; 0 - Равны; -1-первый меньше второго</returns>
        public static int VersionCompare(string ver1, string ver2)
        {
            if (ver1 == null)
                ver1 = "";
            if (ver2 == null)
                ver2 = "";
            string[] arr1 = ver1.Split('.');
            string[] arr2 = ver2.Split('.');
            int comp = 0;
            for (int i = 0; i < arr1.Length; i++)
            {
                comp = arr1[i].CompareTo(arr2[i]);
                if (comp != 0)
                    return comp;
            }
            return comp;
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
            while (true)
            {
                if (File.Exists(path + append + ext))
                {
                    append = " (" + i.ToString() + ")";
                    i++;
                }
                else return path + append + ext;
            }
        }
    }
}