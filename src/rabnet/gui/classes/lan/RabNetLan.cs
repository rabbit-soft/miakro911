using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using gamlib;
using System.Xml.Serialization;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace rabnet
{
    public delegate void RabDumpFindedHandler(string address);
    public delegate void UpdateCheckedHandler(bool needUpdate);

    class RabNetLan:RabLan
    {
        public event RabDumpFindedHandler OnRabDumpFinded;
        public event UpdateCheckedHandler OnUpdateRequired;

        private string _rdAddress;

        public RabNetLan(string rabDumpAddress)
        {
            _rdAddress = rabDumpAddress;
        }

        public void Start()
        {
            startUdpListen();
        }

        public override void CheckUpdate()
        {
            DeleteOldFiles(AppDomain.CurrentDomain.BaseDirectory);

            IPAddress rdAddress;
            if (!IPAddress.TryParse(_rdAddress, out rdAddress))
            {
                this.OnRabDumpFinded += new RabDumpFindedHandler(RabNetLan_OnRabDumpFinded);
                sendUdpMessage(WHERE_IS_RD);
                return;
            }
            //FilesChecked += new FilesCheckedHandler(au_FilesChecked);
            //UpdateFinish += new UpdateFinishHandler(au_UpdateFinish);
            base.CheckUpdate();
        }

        //bool au_FilesChecked(UpdateFile[] ufiles)
        //{
        //    throw new NotImplementedException();
        //}

        //void au_UpdateFinish()
        //{
        //    throw new NotImplementedException();
        //}

        void RabNetLan_OnRabDumpFinded(string address)
        {
            this.OnRabDumpFinded -= new RabDumpFindedHandler(RabNetLan_OnRabDumpFinded);
            _rdAddress = address;
            CheckUpdate();
        }

        protected override void onUdpMessage(string message, IPEndPoint ep)
        {
            switch(message)
            {
                case RD_IS_HERE:

                    if (OnRabDumpFinded != null)
                        OnRabDumpFinded(ep.Address.ToString());
                    break;
            }

        }


        #region updater

        protected override string getRemoteUrl()
        {
            return _rdAddress;
        }

        protected override List<UpdateFile> getUpdateFiles()
        {
            byte[] buffer = sendTcpMessage(GET_UP_FILES_LIST);
            return unserializeUF(buffer);
        }

        private List<UpdateFile> unserializeUF(byte[] buff)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<UpdateFile>));
            MemoryStream ms = new MemoryStream(buff, 0, buff.Length);
            return (xs.Deserialize(ms) as List<UpdateFile>);
        }

        protected byte[] sendTcpMessage(string message)
        {
            TcpClient client = new TcpClient();
            client.Connect(_rdAddress, UPDATE_FILES_PORT);
            NetworkStream netStream = client.GetStream();
            byte[] buffer = makeSendBuffer(message);
            netStream.Write(buffer, 0, buffer.Length);

            int receivedBytesCount = 0;
            int lenght = -1;
            buffer = new byte[4096];

            do
            {
                byte[] tmp = new byte[4096];
                int len = (lenght < 0 || tmp.Length < (lenght - receivedBytesCount)) ? tmp.Length : lenght - receivedBytesCount;
                IAsyncResult asyncResult = netStream.BeginRead(tmp, 0, len, null, null);

                WaitHandle waiter = asyncResult.AsyncWaitHandle;
                bool good = waiter.WaitOne(10000, true);
                if (good)
                {
                    int recBytesCount = netStream.EndRead(asyncResult);
                    if (lenght == -1)
                    {
                        buffer = parseSendBuffer(tmp, out lenght);
                        receivedBytesCount = recBytesCount - NET_STREAM_PACK_LENGHT;
                    }
                    else
                    {
                        Array.Copy(tmp, 0, buffer, receivedBytesCount, recBytesCount);
                        receivedBytesCount += recBytesCount;
                    }
                }
            }
            while (lenght != receivedBytesCount);
            return buffer;
        }
        #endregion updater
    }
}
