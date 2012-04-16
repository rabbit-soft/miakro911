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
                if (_opt.ServersCount == 0)
                {
                    return "Не найдено не одного сервера. Зайдайте собственный";
                }
                int i = 0;
                bool succesConnect = false;
                while (i < _opt.ServersCount)
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
                        Pack.ExecuteMethod(MethodName.Ping);
                        succesConnect = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn("LogIn:", ex);
                        if (ex.InnerException != null && ex.InnerException is XmlRpcFaultException )
                        {
                            //LogOut();
                            //return ex.InnerException.Message;
                            succesConnect = true;
                            if(server!="")
                                _opt.ServersSave();
                            ans = ex.InnerException.Message;
                            break;
                        }
                        Pack.Url = _opt.ServersGetNext();
                        i++;
                    }

                }//while END
                if (!succesConnect)
                    ans = String.Format("Не удалось подключиться ни к одному из серверов ({0:d}){1:s}Задайте новый сервер либо свяжитесь с поставщиком.", _opt.ServersCount, Environment.NewLine);
            }

            if (ans != "")
                LogOut();
            if (server != "")
                _opt.ServersSave();
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

    }

}
