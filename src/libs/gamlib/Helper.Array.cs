using System;
using System.IO;
using System.Text;

namespace gamlib
{
    public static partial class Helper
    {

        public static bool ArraysEquals(byte[] arr1,byte[] arr2)
        {
            if (arr1.Length != arr2.Length) return false;
            for(int i=0;i<arr1.Length;i++)
                if(arr1[i] != arr2[i]) 
                    return false;
            return true;
        }

        public static void ReverseArrayEllements(ref byte[] arr)
        {
            for (int i = 0; i < arr.Length; i++)           
                arr[i] =(byte)(byte.MaxValue-arr[i]);           
        }

        public static bool Array_IsEmpty(byte[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] != 0)
                {
                    return false;
                }
            return true;
        }
    }
}
