using System;
using System.Collections.Generic;
using System.Text;

namespace pEngine
{
    static class Helper
    {
        /// <summary>
        /// Временная переменная для хранения промежуточных результатов.
        /// Нужна для быстроты выполнения функции N3Ppath_Compare, 
        /// т.к. она постоянно вызывается при сортировке
        /// </summary>
        private static int temp;

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

       /* public static int N3Ppath_Compare(ref string p1, ref string p2)
        {
            temp = countOccurencesOfChar(ref p1, '.').CompareTo(countOccurencesOfChar(ref p2, '.'));
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

        private static int countOccurencesOfChar(ref string instance, char c)
        {
            temp = 0;
            foreach (char curChar in instance)            
                if (c == curChar)               
                    ++temp;
            return temp;
        }
        


        /*public static T Cast<T>(object o)
        {
            return (T)o;
        }

        public static object foo(Type t,string param)
        {
            System.Reflection.MemberInfo mi = (typeof(Helper)).GetMethod("Cast");
            System.Reflection.MethodInfo castMethod = (typeof(Helper)).GetMethod("Cast").MakeGenericMethod(t);
            object castedObject = castMethod.Invoke(null, new object[] { param });
            return castedObject;
        }*/
    }
}
