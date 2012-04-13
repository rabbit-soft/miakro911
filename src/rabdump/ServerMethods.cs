using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;
using log4net;
using System.IO;

namespace rabdump
{
    interface IServerProxy : IXmlRpcProxy
    {
        [XmlRpcMethod("client.get.update")]
        void ClientGetUpdate();
    }

    class RequestSender
    {
        private IServerProxy _serverProxy = null;
        private ILog _logger = LogManager.GetLogger("ServerMethods");

        private int _uid;
        private byte[] _key;

        public string Url
        {
            get { return _serverProxy.Url; }
            set { _serverProxy.Url = value; }
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

        public RequestSender()
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
        public RequestSender(string url)
            : this()
        {
            this.Url = url;
        }

        private Stream encryptStream(Stream income)
        {
            try
            {
                income.Position = 0;
                byte[] buffer = new byte[income.Length];
                income.Read(buffer, 0, buffer.Length);

                byte[] encBuff = org.phprpc.util.XXTEA.Encrypt(buffer, _key);

                ///подставляем в начало id
                buffer = new byte[encBuff.Length + 4];//используем переменную buffer дважды для возвращения значения
                Array.Copy(BitConverter.GetBytes(_uid), buffer, 4); //добавляем в начало UID пользователя
                Array.Copy(encBuff, 0, buffer, 4, encBuff.Length);

                var result = new MemoryStream(buffer.Length);
                result.Write(buffer, 0, buffer.Length);
                result.Position = 0;

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error("encrypt exception", ex);
                throw ex;
            }
        }

        private Stream decryptStream(Stream income)
        {
            byte[] decBuff;
            Stream result = null;
            income.Position = 0;
            byte[] buffer = new byte[income.Length];

            income.Read(buffer, 0, buffer.Length);
            try
            {
                decBuff = org.phprpc.util.XXTEA.Decrypt(buffer, _key);
                if (decBuff == null)
                    throw new Exception("Ощибка при расшифровке посылки");
            }
            catch (Exception ex)
            {
                string str = Encoding.UTF8.GetString(buffer); //to see what in buffer
                if (str.Contains("<br />\n"))
                    _logger.Debug("Readable error message: " + Environment.NewLine + str.Remove(str.LastIndexOf("<br />\n")));

                _logger.Error("!DECRIPT EXCEPTION!", ex);
                throw new Exception("Ощибка при расшифровке посылки");
            }
            result = new MemoryStream(decBuff.Length);
            result.Write(decBuff, 0, decBuff.Length);
            result.Position = 0;

            return result;
        }
    }
}
