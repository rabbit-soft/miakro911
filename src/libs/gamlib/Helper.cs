using System.IO;
using System.Text;
using System;

namespace gamlib
{
    static class Helper
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
    }
}

	
	