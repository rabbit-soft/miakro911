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
    public static partial class Engine
    {
        /// <summary>
        /// Первые 3 байта файла-пользователя данным приложением
        /// </summary>
        private static byte[] _fileMarker = new byte[] {0xAB,0x61,0xff };
        private static RequestSender _reqPack = null;
        private static Options _opt = null;
        private static User _curUser = null;
        private static ILog _logger = LogManager.GetLogger(typeof(Engine));

        /// <summary>
        /// Получить экземпляр IRequestPack,
        /// который уже полностью настроен
        /// </summary>
        /// <returns></returns>
        public static RequestSender NewReqSender()
        {
            RequestSender rp = new RequestSender(_opt.ServersGetDefault());
            rp.UserID = _curUser.Id;
            rp.Key = _curUser.Key;
            return rp;
        }

        public static Options Opt
        {
            get
            {
                if (_opt == null)
                    _opt = new Options();
                return _opt;
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
        /// <returns>Возвращает код ощибки</returns> //todo сделать номер ошибки
        public static void LogIn(string keyfile, string password, string server, string newpass)
        {
            LogOut();
            try
            {
                _curUser = extractKeyFileData(keyfile, password);
                if (_opt == null)
                    _opt = new Options();
                _opt.SetOption(optType.DefaultUser, keyfile);
                _opt.BindUser(_curUser);

                //Options.SetOption(optType.DefaultUser, _curUser.Name);
                if (server != "")
                    _opt.ServersAdd(new sServer("Сервер", server), true);
                if (_opt.ServersCount == 0)
                {
                    LogOut();
                    throw new pException(pException.AdressesNotFound);
                }

#if DEBUG
                _opt.ServersSave();
#endif
                int i = 0;
                bool succesConnect = false;
                while (i <= _opt.ServersCount)
                {
                    try
                    {
                        ResponceItem resp;
                        if (newpass != "")
                        {
                            resp = Pack.ExecuteMethod(MethodName.UserGenerateKey, MPN.userId, _curUser.Id.ToString());
                            MakeUserFile(_curUser.Id, _curUser.Name, newpass, resp.Value as string);
                            LogIn(keyfile, newpass, server, "");
                            return;
                        }
                        Pack.ExecuteMethod(MethodName.Ping);
                        succesConnect = true;
                        break; //обязалельно иначе бесконечный цикл
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn("LogIn: "+ ex.Message);
                        if (ex.InnerException != null && ex.InnerException is XmlRpcFaultException)
                        {
                            _opt.ServersSave();
                            throw new pException(50 + (ex.InnerException as XmlRpcFaultException).FaultCode, ex.InnerException.Message);
                        }
                        Pack.Url = _opt.ServersGetNext();
                        i++;
                    }

                }//while END
                if (!succesConnect)
                    throw new pException(pException.FailToConnect, String.Format("Не удалось подключиться ни к одному из серверов ({0:d}){1:s}Задайте новый сервер либо свяжитесь с поставщиком.", _opt.ServersCount, Environment.NewLine));

            }
            catch (pException pexc)
            {
                LogOut();
                _logger.Error(pexc.Message);
                throw pexc;
            }
            catch (Exception exc)
            {
                LogOut();
                _logger.Error(exc);
                throw new pException(1, exc.Message);
            }
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


    }
}
