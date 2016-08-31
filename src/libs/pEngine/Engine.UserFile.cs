using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace pEngine
{
    public static partial class Engine
    {
        /// <summary>
        /// Папка в которой лежат ключи юзеров
        /// </summary>
        public static String KeysFolder
        {
            get
            {
                string path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "keys");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        /// <summary>
        /// Сканирует папку с ключами на наличие таких
        /// </summary>
        /// <returns></returns>
        public static List<string> GetUserKeys()
        {
            List<string> result = new List<string>();
            DirectoryInfo dinf = new DirectoryInfo(KeysFolder);
            foreach (FileInfo fi in dinf.GetFiles("*.psk"))
            {
                result.Add(fi.Name);
            }
            return result;
        }

        /// <summary>
        /// Создает файл нового пользователя в надлежащей папкепапке
        /// </summary>
        /// <param name="name">имя пользователя</param>
        /// <param name="pass">пароль пользователя</param>
        /// <param name="key">ключ</param>
        public static void MakeUserFile(int uid, string name, string pass, byte[] key)
        {
            const int NAME_START = 8;
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] nm = Encoding.UTF8.GetBytes(name);
            //byte[] hash = md5.ComputeHash(key);
            byte[] falseKey = org.phprpc.util.XXTEA.Encrypt(key, Encoding.UTF8.GetBytes(pass));//falsekey
#if DEBUG
            string s = Encoding.UTF8.GetString(key);
#endif
            byte[] buffer = new byte[NAME_START + nm.Length + falseKey.Length /*+ hash.Length*/];
            buffer[0] = _fileMarker[0]; buffer[1] = _fileMarker[1]; buffer[2] = _fileMarker[2];
            buffer[3] = (byte)nm.Length;
            Array.Copy(BitConverter.GetBytes((short)falseKey.Length), 0, buffer, 4, 2);
            Array.Copy(BitConverter.GetBytes((short)uid), 0, buffer, 6, 2);
            Array.Copy(nm, 0, buffer, NAME_START, nm.Length);
            Array.Copy(falseKey, 0, buffer, NAME_START + nm.Length, falseKey.Length);
            //Array.Copy(hash, 0, buffer, NAME_START + nm.Length + falseKey.Length, hash.Length);

            FileStream fstream = new FileStream(Path.Combine(KeysFolder, name + ".psk"), FileMode.Create, FileAccess.Write);
            fstream.Write(buffer, 0, buffer.Length);
            fstream.Close();
        }

        /// <summary>
        /// Первые 4 байта элемент посылки - UID
        /// </summary>
        public static void MakeNewUserFile(string name, string pass, string base64str)
        {
            byte[] buff = Convert.FromBase64String(base64str);
            byte[] key = new byte[buff.Length - 4];
            byte[] buid = new byte[4];
            Buffer.BlockCopy(buff, 0, buid, 0, 4);
            Buffer.BlockCopy(buff, 4, key, 0, buff.Length - 4);
            int uid = BitConverter.ToInt32(buid, 0);
            MakeUserFile(uid, name, pass, key);
        }

        public static void MakeUserFile(int uid, string name, string pass, string base64str)
        {
            //byte[] buff = new byte[key.Length * 4];
            //Buffer.BlockCopy(key, 0, buff, 0, key.Length * 4);
            byte[] buff = Convert.FromBase64String(base64str);
            MakeUserFile(uid, name, pass, buff);
        }
        /*public static void MakeUserFile(int uid, string name, string pass, string key)
        {
            MakeUserFile(uid,name, pass, Encoding.UTF8.GetBytes(key));
        }*/

        /// <summary>
        /// Преобразует ключ-файл в запись типа User
        /// </summary>
        /// <param name="keyfile">Название файла ключа</param>
        /// <param name="password">Пароль</param>
        /// <returns>User</returns>
        /// <remarks>
        /// <para>0 - 2: контрольные биты (0c,0b,58)</para>
        /// <para>3: Сколько бит имя пользователя [N]</para>
        /// <para>4 - 5: Сколько бит занимает Ключ[K]</para>
        /// <para>6 - 7: Uid</para>
        /// <para>8 - 8+N</para>
        /// <para>8+N - 8+N+K</para>
        /// <para>8+N+K +16: Контрольная сумма для проверки правильности пароля (md5)</para>
        /// </remarks>
        private static User extractKeyFileData(string keyfile, string password)
        {
            const int NAME_START = 8;
            //System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            FileStream fstream = new FileStream(Path.Combine(KeysFolder, keyfile), FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fstream.Length];
            fstream.Read(buffer, 0, buffer.Length);
            fstream.Close();
            if (buffer[0] != _fileMarker[0] || buffer[1] != _fileMarker[1] || buffer[2] != _fileMarker[2])
                throw new pException(pException.InvalidUserFile);
            byte[] trueKey = null;
            byte[] falseKey = new byte[BitConverter.ToInt16(buffer, 4)];
            //byte[] hash = new byte[16];
            int nameLen = buffer[3];
            //Array.Copy(buffer, buffer.Length-16, hash, 0, 16);
            Array.Copy(buffer, NAME_START + nameLen, falseKey, 0, falseKey.Length);
            string s = Encoding.UTF8.GetString(falseKey);
            try
            {
                trueKey = org.phprpc.util.XXTEA.Decrypt(falseKey, Encoding.UTF8.GetBytes(password));
                if (trueKey == null)
                    throw new Exception();
            }
            catch
            {
                System.Threading.Thread.Sleep(2000);
                throw new pException(pException.InvalidUserPassword);
            }
            

            //if(trueKey==null)
            //throw new Exception("Не корректный файл");
            //string str2 = Encoding.UTF8.GetString(trueKey);
            //byte[] h2 = md5.ComputeHash(trueKey);
            //if (trueKey != null /*&& Helper.ArraysEquals(md5.ComputeHash(trueKey), ref hash)*/)
            return new User((int)BitConverter.ToInt16(buffer, 6), Encoding.UTF8.GetString(buffer, NAME_START, nameLen).TrimEnd(new char[] { '\0' }), trueKey, password);
            //else throw new Exception("Не верный пароль");
        }
    
    }
}
