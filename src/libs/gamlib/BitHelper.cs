using System;	
	
namespace gamlib
{		
	/// <summary>
	/// Класс дополняющий функционал BitConverter'а
	/// </summary>
	public static class BitHelper
	{
		/// <summary>
		/// Заполнен ли массив только нулями
		/// </summary>
		public static bool ArrayIsEmpty(byte[] bts)
		{
			for (int i = 0; i < bts.Length; i++)
				if (bts[i] != 0)
					return false;
			return true;         
		}

		/// <summary>
		/// Переводит из Двоично-десятичного формата в число
		/// </summary>
		public static int FromBinDecimal(byte[] bytes)
		{
			string result = "";
			for (int i = bytes.Length - 1; i >= 0; i--)
				result += bytes[i].ToString();
			return int.Parse(result);
		}

		/// <summary>
		/// переводит число в массив байтов двоично-десятичного формата
		/// </summary>
		public static byte[] ToBinDecimal(int val)
		{
			string vstr = val.ToString("000000");
			byte[] result = new byte[vstr.Length];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = byte.Parse(vstr[5 - i].ToString());
			}
			return result;
		}

		/// <summary>
		/// Разбирает массив байтов, данные в котором представленны в Упакованном двоичном формате.
		/// (Старшие 4бита и Младшие 4бита содержат разные значения)
		/// </summary>
		public static byte[] ParseGroupBinDec(byte[] bindec)
		{
			byte[] result = new byte[bindec.Length * 2];
			int i = 0;
			foreach (byte bt in bindec)//начинаем первого байта
			{
				string str = Convert.ToString(bt, 2);
				while (str.Length < 8)
					str = "0" + str;
				result[i++] = Convert.ToByte(str.Substring(0, 4), 2);//4старших бита вперед
				result[i++] = Convert.ToByte(str.Substring(4, 4), 2);//4младших бита за ним
			}
			return result;
		}

		public static byte[] MakeGroupBinDec(byte[] bin)
		{
			byte[] result = new byte[bin.Length / 2];
			if (bin.Length != 6) return result;
			int i = 0;
			for (int j = 0; j < result.Length; j++)
			{
				string str1 = Convert.ToString(bin[i++], 2);
				while (str1.Length < 4)
					str1 = "0" + str1;
				string str2 = Convert.ToString(bin[i++], 2);
				while (str2.Length < 4)
					str2 = "0" + str2;
				result[j] = Convert.ToByte(str1 + str2, 2);
			}
			return result;
		}

		public static int GetLiveTimeDays(byte[] bts)
		{
			if (bts.Length > 6) return 0;
			int result = 0;
			int pos = 1;
			for (int i = 5; i > -1; i--)
			{
				result += bts[i] * pos;
				pos *= 10;
			}
			return result;
		}

		public static byte[] GetLiveTimeBytes(int val)
		{
			byte[] result = new byte[6];
			int i = 5;
			while (i > -1)
			{
				result[i] = (byte)(val % 10);
				val = val / 10;
				i--;
			}
			return result;
		}

		public static DateTime GetLastClear(byte[] bts)
		{

			int milenium = bts[10] * 10 + bts[11] > 80 ? 1900 : 2000;
			DateTime result = new DateTime(
				milenium + bts[10] * 10 + bts[11],
				bts[8] * 10 + bts[9],
				bts[6] * 10 + bts[7],
				bts[4] * 10 + bts[5],
				bts[2] * 10 + bts[3],
				bts[0] * 10 + bts[1]);
			return result;

		}

		/// <summary>
		/// Соединяет массивы байтов в один
		/// </summary>
		public static byte[] Concat(params byte[][] matrix)
		{
			int totalLength = 0;
			for (int i = 0; i < matrix.Length; i++)
				totalLength += matrix[i].Length;
			byte[] result = new byte[totalLength];
			int k = 0;
			for (int i = 0; i < matrix.Length; i++)
				for (int j = 0; j < matrix[i].Length; j++)
				{
					result[k++] = matrix[i][j];
				}
			return result;
		}
	}
}