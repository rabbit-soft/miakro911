#if DEBUG
//#define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;
using System.IO;
using log4net;

namespace pEngine
{
    public static class Engine
    {
        private static RequestSender _reqPack = null;
        private static Options _opt = null;
        private static User _curUser = null;
        private static ILog _logger = LogManager.GetLogger(typeof(Engine));

        /// <summary>
        /// Получить экземпляр IRequestPack,
        /// который уже полностью настроен
        /// </summary>
        /// <returns></returns>
        public static IRequestSender NewReqPack()
        {
            RequestSender rp = new RequestSender(_opt.ServersGetDefault());
            rp.UserID = _curUser.Id;
            rp.Key = _curUser.Key;
            return rp;
        }

        public static IOptions Options
        {
            get
            {
                if (_opt == null)
                    _opt = new Options();
                return _opt;
            }
        }

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
        /// Пользователь, который работает с программой
        /// </summary>
        public static User CurUser
        {
            get
            {
                return _curUser;
            }
        }

        private static RequestSender Pack
        {
            get
            {
                if (_reqPack == null)
                {
                    _reqPack = new RequestSender();
                    _reqPack.UserID = _curUser.Id;
                    _reqPack.Key = _curUser.Key;
                    _reqPack.Url = _opt.ServersGetDefault();
                }
                return _reqPack;
            }
        }

        /// <summary>
        /// Перед началом использования движка
        /// нужно указать пользователя иначе нельзя будет работать
        /// </summary>
        /// <param name="user">Пользователь который запрашивает данные</param>
        /// <param name="server">использовать сервер  Если пользователь не указал то равно ""</param>
        /// <param name"newpass">при подключении поменять паролью Если пользователь не указал, то равно ""</param>
        /// <returns>Возвращает пустую строку если логин прошел успешно,
        /// иначе возвращает текст ошибки</returns>
        public static string LogIn(string keyfile, string password, string server,string newpass)
        {
            string ans = "Произошла ошибка";
#if !NOCATCH
            try
            {
#endif
                _curUser = extractKeyFileData(keyfile, password);
                if (_curUser != null) 
                    ans = "";
#if !NOCATCH
            }
            catch(Exception ex)
            {
                _logger.Error("init", ex);
                ans = ex.Message;
                if(ex.InnerException !=null)
                    ans+=Environment.NewLine+ex.InnerException.Message;
            }
#endif
            if (ans == "")
            {
                if (_opt == null)
                    _opt = new Options(_curUser);
                else _opt.BindUser(_curUser);
                Options.SetOption(optType.DefaultUser, _curUser.Name);
                if (server != "")
                    _opt.ServersAdd(new sServer("Сервер", server), true);

                int i = 0;
                bool succesConnect = false;
                while (i <= _opt.ServersCount())
                {
                    try
                    {
                        ResponceItem resp;
                        if (newpass != "")
                        {
                            resp = Pack.ExecuteMethod(MethodName.UserGenerateKey, MethodParamName.userId, _curUser.Id.ToString());
                            MakeUserFile(_curUser.Id, _curUser.Name, newpass, resp.Value as string);
                            LogOut();
                            return LogIn(keyfile, newpass, server, "");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn("LogIn:", ex);
                        if (ex.InnerException != null && ex.InnerException is XmlRpcFaultException )
                        {
                            //LogOut();
                            //return ex.InnerException.Message;
                            succesConnect = true;
                            ans = ex.InnerException.Message;
                            break;
                        }
                        Pack.Url = _opt.ServersGetNext();
                        i++;
                    }

                }//while END
                if (!succesConnect)
                    ans = String.Format("Не удалось подключиться ни к одному из серверов ({0:d}){1:s}Задайте новый сервер либо свяжитесь с поставщиком.", _opt.ServersCount(), Environment.NewLine);
            }

            if (ans != "")
                LogOut();                    
            return ans;
        }

        /// <summary>
        /// Прекращает работу пользователя с движком
        /// </summary>
        public static void LogOut()
        {
            _opt = null;
            _reqPack = null;
            _curUser = null;
        }

        #region mayBee_replace 
        //TODO возможно работу с ключами нужно перенести в отдельный модуль

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
        public static void MakeUserFile(int uid,string name,string pass,byte[] key)
        {
            const int NAME_START = 8;
            byte[] nm = Encoding.UTF8.GetBytes(name);
            byte[] falseKey = org.phprpc.util.XXTEA.Encrypt(key, Encoding.UTF8.GetBytes(pass));//falsekey
#if DEBUG
            string s = Encoding.UTF8.GetString(key);
#endif 
            byte[] buffer = new byte[NAME_START + nm.Length + falseKey.Length];
            buffer[0] = 0xAB; buffer[1] = 0x61; buffer[2] = 0xFF; 
            buffer[3] = (byte)nm.Length;
            Array.Copy(BitConverter.GetBytes((short)falseKey.Length), 0, buffer, 4, 2);
            Array.Copy(BitConverter.GetBytes((short)uid), 0, buffer, 6, 2);
            Array.Copy(nm, 0, buffer, NAME_START, nm.Length);
            Array.Copy(falseKey, 0, buffer, NAME_START+nm.Length, falseKey.Length);

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
            byte[] key = new byte[buff.Length-4];
            byte[] buid = new byte[4];
            Buffer.BlockCopy(buff, 0, buid, 0, 4);
            Buffer.BlockCopy(buff, 4, key, 0, buff.Length - 4);
            int uid = BitConverter.ToInt32(buid, 0);
            MakeUserFile(uid,name, pass, key);
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
        /// </remarks>
        private static User extractKeyFileData(string keyfile,string password)
        {
            const int NAME_START = 8;
            FileStream fstream = new FileStream(Path.Combine(KeysFolder, keyfile),FileMode.Open,FileAccess.Read);
            byte[] buffer = new byte[fstream.Length];
            fstream.Read(buffer, 0, buffer.Length);
            fstream.Close();
            if(buffer[0]!=0xAB || buffer[1]!=0x61 || buffer[2]!=0xFF)
                throw new Exception("Не корректный файл");
            byte[] trueKey = null;
            byte[] falseKey = new byte[BitConverter.ToInt16(buffer,4)];
            int nameLen = buffer[3];
            Array.Copy(buffer, NAME_START + nameLen, falseKey, 0, falseKey.Length);
            string s = Encoding.UTF8.GetString(falseKey);
            try
            {
                trueKey = org.phprpc.util.XXTEA.Decrypt(falseKey, Encoding.UTF8.GetBytes(password));
                if (trueKey == null)
                    throw new DecryptionException();
            }
            catch 
            { 
                throw new DecryptionException("Не верный пароль");
            }

            //if(trueKey==null)
                //throw new Exception("Не корректный файл");
            //string str2 = Encoding.UTF8.GetString(trueKey);
            //byte[] h2 = md5.ComputeHash(trueKey);
            //if (trueKey != null /*&& Helper.ArraysEquals(md5.ComputeHash(trueKey), ref hash)*/)
                return new User((int)BitConverter.ToInt16(buffer,6), Encoding.UTF8.GetString(buffer, NAME_START, nameLen).TrimEnd(new char[] { '\0' }), trueKey, password);
            //else throw new Exception("Не верный пароль");
        }

        #endregion mayBee_replace
    }

}
