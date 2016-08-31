#if DEBUG
    //#define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;
using log4net;
using CookComputing.XmlRpc;
using System.IO;
using System.IO.Compression;

namespace pEngine
{
    public delegate void RSStartEventHandler();
    public delegate void RSErrorEventHandler(Exception excpt);
    public delegate void RSSuccessEventHandler(ResponseList responces); 

    /// <summary>
    /// Класс, выполняющий на удаленном сервере методы, вызванные пользователем.
    /// </summary>
    public class RequestSender //: IRequestSender
    {
#if DEBUG
        private const int MAX_LOG_LENGHT = 1000000;       
#endif
        protected string _ServUriAppend =
#if !DEBUG
            "/_padmin01/";
#else
            "";
#endif
        protected string _RPCfile = "index.php";

        private IServerProxy _serverProxy = null;
        private ILog _logger = LogManager.GetLogger("ServerMethods");
        private int _uid = 0;
        private byte[] _key;

        public string Url
        {
            get { return _serverProxy.Url; }
            set 
            {
                Uri u = new Uri(value.TrimEnd('/')+'/');
                u = new Uri(u, _ServUriAppend);
                u = new Uri(u, _RPCfile);
                _serverProxy.Url = u.AbsoluteUri; 
            }
        }

        public int UserID
        {
            get { return _uid; }
            set { _uid = value; }
        }

        public byte[] Key
        {
            get { return _key; }
            set { _key = value; }
        }

        internal RequestSender()
        {
            _serverProxy = XmlRpcProxyGen.Create<IServerProxy>();
            _serverProxy.XmlEncoding = Encoding.UTF8;

            _serverProxy.EncryptStream += new XmlRpcEncryptEventHandler(encryptStream);
            _serverProxy.DecryptStream += new XmlRpcEncryptEventHandler(decryptStream);
#if DEBUG
            _serverProxy.RequestEvent += new XmlRpcRequestEventHandler(reqHandler);
            _serverProxy.ResponseEvent += new XmlRpcResponseEventHandler(respHandler);
#endif
        }
        internal RequestSender(string url)
            : this()
        {
            this.Url = url;
        }

        #region IRequestPack Members

        public event RSStartEventHandler OnStart;
        public event RSErrorEventHandler OnError;
        public event RSSuccessEventHandler OnSuccess;

        public void ExecuteAsync(RequestList list)
        {
            Thread t = new Thread(new ParameterizedThreadStart(executeAsync));
            t.IsBackground = true;
            t.Start(list);
        }

        public void ExecuteAsync(RequestItem item)
        {
            RequestList list = new RequestList(item);
            ExecuteAsync(list);
        }

        /// <summary>
        /// Выполняет функцию на сервере(см. ServerMethod) и возвращает данные ответа.
        /// </summary>
        /// <param name="mn">Название функции</param>
        /// <param name="vars">Параметры функции</param>
        /// <returns>Объект с данными ответа</returns>
        public ResponceItem ExecuteMethod(MethodName mn, MethodParams vars)
        {
            return executeMethod(mn, vars);
        }
        public ResponceItem ExecuteMethod(MethodName mn, params object[] prms)
        {
            if (prms.Length % 2 != 0)
                throw new DevelException("Количество параметров нечетно");
            MethodParams vars = new MethodParams();
            for (int i = 0; i < prms.Length; i += 2)
                vars.Add(prms[i].ToString(), prms[i + 1]);

            return executeMethod(mn, vars);
        }
        public ResponceItem ExecuteMethod(MethodName mn)
        {
            return executeMethod(mn, null);
        }

        #endregion

        private void callOnStart()
        {
            if (OnStart != null)
                OnStart();
        }

        private void callOnError(Exception ex)
        {
            if (OnError != null)
                OnError(ex);
        }

        private void callOnSuccess(ResponseList responce)
        {
            if (OnSuccess != null)
                OnSuccess(responce);
        }

        /// <summary>
        /// Выполняет последовательно методы, переданные через RequestList
        /// расчитан на вызов в отдельном параметризированном потоке. 
        /// </summary>
        /// <param name="o">RequestList</param>
        private void executeAsync(object o)
        {
#if !NOCATCH
            try
            {
#endif
                callOnStart();
                RequestList data = (o as RequestList);
                ResponseList result = new ResponseList();
                foreach (RequestItem pair in data)
                {
                    result.Add(executeMethod(pair.Name, pair.Params));
                }
                callOnSuccess(result);
#if !NOCATCH
            }
            catch (Exception ex)
            {
                _logger.Error("error: ", ex);
                callOnError(ex);
            }
#endif
        }

        /// <summary>
        /// Выполняет метод на сервере
        /// </summary>
        /// <param name="name">Имя метода</param>
        /// <param name="vars">Параметры необходимые для выполнения метода</param>
        /// <returns>Может вернуть null при неверном запросе</returns>
        /// <exception cref="">NotHandle</exception>
        private ResponceItem executeMethod(MethodName name, MethodParams vars)
        {
            Type t = typeof(IServerProxy);
            ResponceItem result = new ResponceItem();
            result.Key = name;
            if (vars == null)
            {
                MethodInfo mi = t.GetMethod(name.ToString(), new Type[] { });
                if (mi == null)
                //{
                    throw new NotImplementedException(String.Format("[IServerProxy not contains the method '{0:s}']", name.ToString()));
                  //  result.Value = null;
                //}
                result.Value = mi.Invoke(_serverProxy, new object[] { });
            }
            else
            {
                MethodInfo[] mis = t.GetMethods();
                MethodInfo candMI = null;
                ParameterInfo[] candPIs = null;
                string needName = name.ToString();
                int matches = 0;
                foreach (MethodInfo mi in mis)
                    if (mi.Name == needName)
                    {
                        int mc = 0;
                        ParameterInfo[] pis = mi.GetParameters();
                        foreach (ParameterInfo pi in pis)
                        {
                            if (vars.ContainsKey(pi.Name)) mc++;//todo возможно нужна регистра-независимость
                        }
                        if (mc > matches)
                        {
                            candMI = mi;
                            candPIs = pis;
                            matches = mc;
                        }
                    }
                if (candMI != null)
                {
                    if (candPIs.Length == matches)
                    {
                        result.Value = candMI.Invoke(_serverProxy, invokeParams(candPIs, vars));
                    }
                    else
                    {
                        _logger.Warn("executeMethod: MISSING PARAMETERS");
#if DEBUG
                        throw new DevelException("Не достаточно параметров для вызова метода: " + candMI.Name);
#else
                        result.Value = null;
#endif
                    }
                }
                else
                {
                    _logger.Warn("executeMethod: REQUIRED METHOD NOT FOUND");
#if DEBUG
                    throw new DevelException("Запрашиваемый метод не найден: " + candMI.Name);
#else
                    result.Value = null;
#endif
                }
            }
            return result;
        }

        /// <summary>
        /// Получает массив значений из параметров,
        /// которые требуются для функции, описанные в pis
        /// </summary>
        /// <param name="pis"></param>
        /// <param name="vars">Параметры вызова функции</param>
        /// <returns></returns>
        private object[] invokeParams(ParameterInfo[] pis, MethodParams vars)
        {
            object[] result = new object[pis.Length];
            int i = 0;
            foreach (ParameterInfo pi in pis)
            {
                result[i++] = vars[pi.Name];
            }
            return result;
        }

#if DEBUG
        private void reqHandler(object sender, XmlRpcRequestEventArgs args)
        {
            traceStream(args.RequestStream, false);
        }

        private void respHandler(object sender, XmlRpcResponseEventArgs args)
        {
            traceStream(args.ResponseStream, true);
        }
        /// <summary>
        /// Добавляет данные отправленного\принятого потока в логи
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="responce"></param>
        private void traceStream(Stream stream, bool responce)
        {
            stream.Position = 0;
            TextReader trdr = new StreamReader(stream);
            String s = trdr.ReadToEnd();
            string tp = responce ? "responce" : "request";
            if (s.Length > MAX_LOG_LENGHT)
                _logger.DebugFormat("{1:s}: Traced {0:d} chars", s.Length, tp);
            else
                _logger.DebugFormat("{0:s}: {1:s}{2:s}", tp, Environment.NewLine, s);
        }
#endif

        private Stream encryptStream(Stream income)
        {
            try
            {
                income.Position = 0;
                byte[] buffer = new byte[income.Length];
                income.Read(buffer, 0, buffer.Length);
                //архивируем
                using (MemoryStream defMS = new MemoryStream())
                {
                    using (DeflateStream deflafe = new DeflateStream(defMS, CompressionMode.Compress,true))
                    {
                        deflafe.Write(buffer, 0, buffer.Length);
                    }
                    buffer = new byte[defMS.Length];
                    defMS.Position = 0;
                    defMS.Read(buffer, 0, buffer.Length);
                }
                
                byte[] encBuff = org.phprpc.util.XXTEA.Encrypt(buffer, _key);

                ///подставляем в начало id
                buffer = new byte[encBuff.Length + 4];//используем переменную buffer дважды для возвращения значения
                Array.Copy(BitConverter.GetBytes(_uid), buffer, 4); //добавляем в начало UID пользователя
                Array.Copy(encBuff, 0, buffer, 4, encBuff.Length);

                MemoryStream result = new MemoryStream(buffer.Length);
                result.Write(buffer, 0, buffer.Length);
                result.Position = 0;

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                pException pex = new pException(pException.EncryptException,ex);               
                throw pex;
            }
        }
    
        private Stream decryptStream(Stream income)
        {
            byte[] decBuff;
            Stream result = null;                       
#if DEBUG
            income.Position = 0;
            TextReader trdr = new StreamReader(income);
            String s = trdr.ReadToEnd();
            _logger.Debug("From:"+this.Url);
            _logger.Debug(s);
#endif
            income.Position = 0;
			byte[] buffer = new byte[income.Length];
            income.Read(buffer, 0, buffer.Length);
            try
            {
                decBuff = org.phprpc.util.XXTEA.Decrypt(buffer, _key);
                if (decBuff == null)
                    throw new Exception();
                //разархивируем
                using (MemoryStream defMS = new MemoryStream(decBuff))
                using (DeflateStream deflafe = new DeflateStream(defMS, CompressionMode.Decompress,true))
                using (StreamReader reader = new StreamReader(deflafe))
                {
                    string xml =reader.ReadToEnd();
                    decBuff = Encoding.UTF8.GetBytes(xml);
                }
            }
            catch (Exception ex)
            {
                income.Position = 0;
                TextReader tr = new StreamReader(income);
                string str = tr.ReadToEnd(); //to see what in buffer
                if (str.Contains("<br />\n"))
                    _logger.Debug("Readable error message: " + Environment.NewLine + str.Remove(str.LastIndexOf("<br />\n")));
                _logger.Error(ex.Message);
                if(str == "fuck you")
                    throw new pException(pException.ServerCannotDecrypt);
                throw new pException(pException.DecryptException);
            }
            result = new MemoryStream(decBuff.Length);
            result.Write(decBuff, 0, decBuff.Length);
            result.Position = 0;
            return result;
        }
    }

    /// <summary>
    /// Данные для одного метода, который нужно выполнить на сервере.
    /// </summary>
    public struct RequestItem
    {
        public MethodName Name;
        public MethodParams Params;
        public RequestItem(MethodName name,MethodParams prms)
        {
            this.Name = name;
            this.Params = prms;
        }
        /// <summary>
        /// Данные для одного запроса
        /// </summary>
        /// <param name="name">Название метода</param>
        /// <param name="prms">Параметры функци, Должны представлять пару (название параметра, значение)</param>
        public RequestItem(MethodName name, params object[] prms)
        {
            this.Name = name;
            //if()
            MethodParams temp = new MethodParams();
            for (int i = 0; i < prms.Length; i += 2)
            {
                temp.Add(prms[i].ToString(), prms[i + 1]);
            }
            this.Params = temp;
        }
        public RequestItem(MethodName name)
        {
            this.Name = name;
            this.Params = null;
        }
    }

    /// <summary>
    /// Ответные данные на запрашиваемый метод
    /// </summary>
    public class ResponceItem 
    {
        /// <summary>
        /// Название метода
        /// </summary>
        public MethodName Key;
        /// <summary>
        /// Данные, возвращенные выполнениым методом
        /// </summary>
        public Object Value;
    }
    
    /// <summary>
    /// Представляет список функций (sRequest), которые нужно вуполнить на сервере.
    /// </summary>
    public class RequestList :List<RequestItem> 
    {
        public RequestList(RequestList parentRequest) : base(parentRequest) { }
        public RequestList(RequestItem req){ this.Add(req);}
        public RequestList() { }
    }
    public class ResponseList : List<ResponceItem>
    {
        public ResponceItem GetItemByMName(MethodName methodName)
        {
            foreach (ResponceItem i in this)
            {
                if (i.Key == methodName)
                    return i;
            }
            return null;
        }
    };

    /// <summary>
    /// "Словарь" параметров для (sRequest) выполнения метода на сервере.
    /// </summary>
    public class MethodParams : Dictionary<string, object> {}
   
    /// <summary>
    /// Происходит если разработчик допустил ошибку
    /// </summary>
    class DevelException : Exception
    {
        internal DevelException(string message) : base(message) { }
        internal DevelException() { }
    }
}
