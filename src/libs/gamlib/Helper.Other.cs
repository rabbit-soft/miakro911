using System.IO;
using System.Text;
using System;

namespace gamlib
{
    static partial class Helper
    {
        public static bool isInteger(string str)
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

        public static bool ArraysEquals(ref byte[] arr1,ref byte[] arr2)
        {
            if (arr1.Length != arr2.Length) return false;
            for(int i=0;i<arr1.Length;i++)
                if(arr1[i] != arr2[i]) 
                    return false;
            return true;
        }
        public static bool ArraysEquals(byte[] arr1, ref byte[] arr2) { return ArraysEquals(ref arr1, ref arr2); }

        public static void ReverseArrayEllements(ref byte[] arr)
        {
            for (int i = 0; i < arr.Length; i++)           
                arr[i] =(byte)(byte.MaxValue-arr[i]);           
        }

        public static string GetMD5FromFile(string filepath)
        {
            if (filepath == "") return "0";
            FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            return ByteArrayToHexString(retVal);
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

        private static int countOccurencesOfChar(ref string instance, char c)
        {
            int temp = 0;
            foreach (char curChar in instance)            
                if (c == curChar)               
                    ++temp;
            return temp;
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

	
	