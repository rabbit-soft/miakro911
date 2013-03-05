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
            if (temp == 1)//p1 ����� ������ �����, ��� p2
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
        /// ���������� ������ � ���������� ��������� ���������
        /// </summary>
        /// <param name="ver1">������ ������� �����</param>
        /// <param name="ver2">������ ������� �����</param>
        /// <returns>1- ������ ������ �������; 0 - �����; -1-������ ������ �������</returns>
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
                    case 1: result += "������ "; break;
                    case 2: result += "������� "; break;
                    case 3: result += "���� "; break;
                    case 4: result += "������ "; break;
                    case 5: result += "��� "; break;
                    case 6: result += "���� "; break;
                    case 7: result += "���� "; break;
                    case 8: result += "������ "; break;
                    case 9: result += "�������� "; break;
                    case 10: result += "������� "; break;
                    case 11: result += "������ "; break;
                    case 12: result += "������� "; break;
                }
                return result;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// ��������� ���� �� ��������� ����, ���� ���� �� ���������� ��� '���� (1)'
        /// </summary>
        /// <param name="s">������ ����</param>
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