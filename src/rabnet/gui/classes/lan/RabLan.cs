using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using gamlib;

namespace rabnet
{
    public delegate void RabLanExceptionHandler(Exception exc);

    abstract class RabLan:AppUpdater
    {
        protected static readonly ILog _logger = LogManager.GetLogger(typeof(RabLan));
        protected const string WHERE_IS_RD = "who_is_rabdump";
        protected const string RD_IS_HERE = "rabdump_is_here";
        protected const string GET_UP_FILES_LIST = "GET_UP_FILES_LIST";
        protected const string GET_UP_FILE = "GET_UP_FILE";
        protected const int UPDATE_FILES_PORT = 9052;
        public const int MSG_PORT = 10283;
        public const int NET_STREAM_PACK_LENGHT = 4;

        public event RabLanExceptionHandler OnException;

        protected abstract void onUdpMessage(string message,IPEndPoint ep);

        protected void startUdpListen()
        {
            Thread t = new Thread(udpListenThread);
            t.IsBackground=true;
            t.Start();
        }

        protected void sendUdpMessage(string msg,IPEndPoint addr)
        {
            byte[] sendBuff = Encoding.UTF8.GetBytes(msg);
            UdpClient udpSender = new UdpClient();
            addr.Port = MSG_PORT;
            udpSender.Send(sendBuff, sendBuff.Length, addr);
        }

        protected void sendUdpMessage(string msg)
        {
            IPEndPoint addr = new IPEndPoint(IPAddress.Broadcast, MSG_PORT);
            sendUdpMessage(msg, addr);
        }

        protected void udpListenThread()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, MSG_PORT);
            UdpClient udpListener = new UdpClient(ep);
            while (true)
            {               
                IPEndPoint epSender = null;
                byte[] recBytes = udpListener.Receive(ref epSender);
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

        protected void readWaiter(IAsyncResult res)
        {
            try
            {
                MyAsyncState astate = (MyAsyncState)res.AsyncState;
                NetworkStream netStream = astate.Stream;
                byte[] buffer = astate.Buffer;

                int receivedBytesCount = netStream.EndRead(res);
                if (receivedBytesCount < NET_STREAM_PACK_LENGHT)
                    throw new NotImplementedException();
                receivedBytesCount = receivedBytesCount - NET_STREAM_PACK_LENGHT;
                int lenght = BitConverter.ToInt32(buffer, 0);
                byte[] tmp = new byte[lenght];
                Array.Copy(buffer, NET_STREAM_PACK_LENGHT, tmp, 0, receivedBytesCount);
                buffer = tmp;

                while (lenght != receivedBytesCount)
                {
                    tmp = new byte[4096];
                    int len = tmp.Length < (lenght - receivedBytesCount) ? tmp.Length : lenght - receivedBytesCount;
                    IAsyncResult asyncResult = netStream.BeginRead(tmp, 0, len, null, null);

                    WaitHandle waiter = asyncResult.AsyncWaitHandle;
                    bool good = waiter.WaitOne(10000, true);
                    if (good)
                    {
                        int recBytesCount = netStream.EndRead(asyncResult);
                        Array.Copy(tmp, 0, buffer, receivedBytesCount, recBytesCount);
                        receivedBytesCount += receivedBytesCount;
                    }
                }
                byte[] answer = getAnswer(buffer);
                answer = makeSendBuffer(answer);
                if (answer != null)
                    netStream.Write(answer, 0, answer.Length);
                else
                    netStream.Close();
            }
            catch (Exception exc)
            {
                if (OnException != null)
                    OnException(exc);
            }
        }

        protected virtual byte[] getAnswer(byte[] inBuff)
        {
            return null;
        }

        protected byte[] makeSendBuffer(string message)
        {
            byte[] buff = Encoding.UTF8.GetBytes(message);
            return makeSendBuffer(buff);
        }

        protected byte[] makeSendBuffer(byte[] buff)
        {
            byte[] result = new byte[buff.Length + NET_STREAM_PACK_LENGHT];
            byte[] length = BitConverter.GetBytes(buff.Length);
            Array.Copy(length, result, NET_STREAM_PACK_LENGHT);
            Array.Copy(buff, 0, result, NET_STREAM_PACK_LENGHT, buff.Length);
            return result;
        }        

        protected byte[] parseSendBuffer(byte[] buffer, out int lenght)
        {
            lenght = BitConverter.ToInt32(buffer, 0);
            byte[] result = new byte[lenght];
            Array.Copy(buffer, NET_STREAM_PACK_LENGHT, result, 0, lenght);
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
