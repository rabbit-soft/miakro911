#if DEBUG
    //#define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using System.IO;
using gamlib;

namespace rabnet
{
    public class RabLanException : Exception
    {
        public RabLanException(string message) : base(message) { }
    }
    
    public delegate void RabLanExceptionHandler(Exception exc);
    public delegate void RabLanHandler();   

    abstract class RabLan:IDisposable
    {
        protected enum TcpStreamDataType { Message,ErrorMessage,Serialize,File}

        protected static readonly ILog _logger = LogManager.GetLogger(typeof(RabLan));
        protected const string WHERE_IS_RD = "who_is_rabdump";
        protected const string RD_IS_HERE = "rabdump_is_here";
        protected const string GET_UP_FILES_LIST = "GET_UP_FILES_LIST";
        protected const string GET_UP_FILE = "GET_UP_FILE";
        protected const char MSG_DELIMITER = '|';
        protected const int UPDATE_FILES_PORT = 9052;     
        /// <summary>
        /// Сколько первых байт TCP посылки хранят размер передаваеммых данных.
        /// <remarks>При получении TCP посылки нелбхдимо прочитать первые NET_STREAM_PACK_LENGHT байт. 
        /// Полученное Int64 число указывает на кол-во байт посылки, которые надо получить.
        /// </remarks>
        /// </summary>
        protected const int NET_STREAM_PACK_LENGHT = 8;
        protected const int NET_STREAM_DATATYPE_LENGHT = 1;
        protected const int DATATYPE_OFFSET = NET_STREAM_PACK_LENGHT + NET_STREAM_DATATYPE_LENGHT; 

        private Thread _udpListenThread;
        UdpClient _udpListener;

        public const int MSG_PORT = 10283;

        public event RabLanExceptionHandler OnException;

        protected abstract void onUdpMessage(string message,IPEndPoint ep);

        protected void startUdpListen()
        {
            _udpListenThread = new Thread(udpListenThread);
            _udpListenThread.IsBackground = true;
            _udpListenThread.Start();
        }

        protected void sendUdpMessage(string msg,IPEndPoint addr)
        {
            try
            {
                byte[] sendBuff = Encoding.UTF8.GetBytes(msg);
                UdpClient udpSender = new UdpClient();
                addr.Port = MSG_PORT;
                udpSender.Send(sendBuff, sendBuff.Length, addr);
            }
            catch (Exception exc)
            {
                _logger.Error(String.Format("fail to send msg:'{0:s}' to {1:s}",msg,addr.ToString()),exc);
            }
        }

        protected void sendUdpMessage(string msg)
        {
            IPEndPoint addr = new IPEndPoint(IPAddress.Broadcast, MSG_PORT);
            sendUdpMessage(msg, addr);
        }

        protected void udpListenThread()
        {
            if (Helper.CheckUdpPortBinded(MSG_PORT))
            {
                _logger.InfoFormat("UDP Port {0:d} already listen", MSG_PORT);
                return;
            }
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, MSG_PORT);            
            _udpListener = new UdpClient(ep);
            while (true)
            {               
                IPEndPoint epSender = null;
                byte[] recBytes = _udpListener.Receive(ref epSender);
                string msg = Encoding.UTF8.GetString(recBytes);
                _logger.Debug("income message: " + msg);
                if (isMyIp(epSender)) continue;
                onUdpMessage(msg,epSender);
            }
        }
       
        private bool isMyIp(IPEndPoint ep)
        {
            string hostName = Dns.GetHostName();
            IPAddress[] adrs = Dns.GetHostAddresses(hostName);
            foreach (IPAddress a in adrs)
                if (a.Equals(ep.Address))
                    return true;
            return false;
        }

        public virtual void Dispose()
        {
            if(_udpListener!=null)
                _udpListener.Close();
            if (_udpListenThread != null)
                _udpListenThread.Abort();
        }


        protected void readWaiter(object obj) ///todo этот метод надо слить с RabNetLan.sendTcpMessage
        {
            if(!(obj is TcpClient))return;

            NetworkStream netStream = (obj as TcpClient).GetStream();
            try
            {
                Stream result = new MemoryStream();
                byte[] buffer = new byte[1];

                int totalRecBytesCount = 0;
                long lenght = -1;
                TcpStreamDataType dtype;
                int offset = 0;

                do
                {
                    if (offset == 0)
                        buffer = new byte[65536];
                    int len = (lenght < 0 || buffer.Length < (lenght - totalRecBytesCount)) ? buffer.Length : (int)(lenght - totalRecBytesCount);
                    IAsyncResult asyncResult = netStream.BeginRead(buffer, offset, (len - offset), null, null);

                    WaitHandle waiter = asyncResult.AsyncWaitHandle;
                    bool good = waiter.WaitOne(10000, true);
                    if (!good) continue;

                    int recBytesCount = netStream.EndRead(asyncResult) + offset;
                    if (lenght == -1)
                    {
                        if (recBytesCount < DATATYPE_OFFSET)
                        {
                            offset = recBytesCount;
                            continue;
                        }
                        offset = 0;
                        buffer = parseSendBuffer(buffer, ref recBytesCount, out lenght, out dtype);
                        ///
                    }

                    //Array.Copy(buffer, 0, resultBuff, recBytesCount, recBytesCount);
                    result.Write(buffer, 0, recBytesCount);
                    totalRecBytesCount += recBytesCount;
                }
                while (lenght != totalRecBytesCount);

                buffer = new byte[result.Length];
                result.Position = 0;
                result.Read(buffer, 0, buffer.Length);
                sendAnswer(netStream, buffer);
            }
#if !NOCATCH
            catch (Exception exc)
            {
                _logger.Error(exc);
                if (OnException != null)
                    OnException(exc);
                if (!(exc is SocketException || exc.InnerException is SocketException))
                {
                    byte[] errMsg = makeSendBuffer(exc.Message, true);
                    netStream.Write(errMsg, 0, errMsg.Length);
                }
            }
#endif
            finally
            {
                netStream.Close();
            }
        }

        protected virtual void sendAnswer(NetworkStream netStream, byte[] buffer) { }

        protected byte[] makeSendBuffer(string message,bool error)
        {
            byte[] buff = Encoding.UTF8.GetBytes(message);
            return makeSendBuffer(buff, error? TcpStreamDataType.ErrorMessage:TcpStreamDataType.Message);
        }
        protected byte[] makeSendBuffer(string message) 
        {
            return makeSendBuffer( message, false);
        }

        protected byte[] makeSendBuffer(byte[] buff, TcpStreamDataType dtype)
        {
            byte[] result = new byte[buff.Length + NET_STREAM_PACK_LENGHT+NET_STREAM_DATATYPE_LENGHT];
            byte[] length = BitConverter.GetBytes((long)buff.Length);
            ///записываем длинну данных
            Array.Copy(length, result, NET_STREAM_PACK_LENGHT);
            ///после длинны пишем тип Данных посылки
            result[NET_STREAM_PACK_LENGHT] = (byte)dtype;
            ///за типом пишем посылку
            Array.Copy(buff, 0, result, DATATYPE_OFFSET, buff.Length);
            return result;
        }

        protected byte[] parseSendBuffer(byte[] buffer,ref int recCount, out long lenght,out TcpStreamDataType dtp)
        {           
            lenght = BitConverter.ToInt64(buffer, 0);
            byte[] result = new byte[lenght];
            dtp = (TcpStreamDataType)buffer[NET_STREAM_PACK_LENGHT];
            recCount = recCount - DATATYPE_OFFSET;
            Array.Copy(buffer, DATATYPE_OFFSET, result, 0, recCount);
            return result;
        }
    }

    class MyAsyncState
    {
        public readonly NetworkStream Stream;
        public readonly byte[] Buffer;

        public MyAsyncState(NetworkStream stream, byte[] buffer)
        {
            this.Stream = stream;
            this.Buffer = buffer;
        }
    }
}
