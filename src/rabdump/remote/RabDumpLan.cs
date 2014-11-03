using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using gamlib;
using rabnet;
using System.Diagnostics;
using System.Xml.Serialization;

namespace rabdump
{
    public delegate void RabDumpAlreadyInLanHandle(string hostName);

    class RabDumpLan:rabnet.RabLan
    {
        private const string RD_IS_STARTED = "rabdump_is_started";
        private const string RD_ALREADY_HERE = "shut_down_please_mr_rabdump";          

        public event RabDumpAlreadyInLanHandle RabDumpAlreadyInLan;

        private Thread _updaterThread;
        private TcpListener _tcpListener;

        public void Start()
        {
            startUdpListen();
            sendUdpMessage(RD_IS_STARTED);
            startUpdateListen();
        }      

        protected override void onUdpMessage(string message,IPEndPoint ep)
        {
            switch(message)
            {
                case RD_IS_STARTED:
                    sendUdpMessage(RD_ALREADY_HERE, ep);
                    break;
                case RD_ALREADY_HERE:
                    // todo тупит на компах с мобильным интернетом
                    //if (RabDumpAlreadyInLan != null)
                    //{                        
                    //    IPHostEntry ent = Dns.GetHostEntry(ep.Address);
                    //    RabDumpAlreadyInLan(ent.HostName);
                    //}
                    break;
                case WHERE_IS_RD:
                        sendUdpMessage(RD_IS_HERE,ep);
                    break;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _updaterThread.Abort();
            _tcpListener.Stop();            
        }

        private void startUpdateListen()
        {
            _updaterThread = new Thread(upFilesThread);
            _updaterThread.IsBackground = true;
            _updaterThread.Start();
        }

        private void upFilesThread()
        {
            try
            {
                _tcpListener = new TcpListener(IPAddress.Any, UPDATE_FILES_PORT);
                _tcpListener.Start();
                while (true)
                {
                    TcpClient client = _tcpListener.AcceptTcpClient();
                    Thread t = new Thread(new ParameterizedThreadStart(readWaiter));
                    t.Start(client);
                    //NetworkStream netStream = client.GetStream();
                    //byte[] buffer = new byte[4096];
                    //netStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(readWaiter), new MyAsyncState(netStream,buffer));
                }
            }
            catch (Exception exc)
            {
                _logger.Error(exc);
            }
        }

        protected override void sendAnswer(NetworkStream netStream,byte[] inBuff)
        {
            string BASE_PATH = AppDomain.CurrentDomain.BaseDirectory;

            string request = Encoding.UTF8.GetString(inBuff);
            string[] args = request.Split(MSG_DELIMITER);
            switch (args[0])
            {
                case GET_UP_FILES_LIST:                    
                    List<UpdateFile> files = getAnswerUpdateFiles(BASE_PATH);
                    ///абсолютный путь в относительный
                    for(int i=0;i<files.Count;i++)
                        files[i].Path = files[i].Path.Replace(BASE_PATH.TrimEnd('\\'), "");
                    byte[] result = makeSendBuffer(serializeUF(files), TcpStreamDataType.Serialize);
                    netStream.Write(result, 0, result.Length);
                    break;

                case GET_UP_FILE:
                    string path = Path.Combine(BASE_PATH, args[1].TrimStart('\\'));
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    byte[] len = BitConverter.GetBytes(fs.Length);
                    netStream.Write(len, 0, len.Length);
                    byte b = (byte)TcpStreamDataType.File;
                    netStream.WriteByte(b);
                    Helper.CopyStream(fs, netStream);
                    fs.Close();
                    netStream.Flush();
                    break; 
            }
        }

        private byte[] serializeUF(List<UpdateFile> files)
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(files.GetType());
            xs.Serialize(ms, files);
            byte[] buffer = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        private List<UpdateFile> getAnswerUpdateFiles(string path)
        {
            List<UpdateFile> result = new List<UpdateFile>();
            DirectoryInfo dir = new DirectoryInfo(path);
            string[] patterns = new string[] { "*.exe","*.dll","*.rdl" };
            foreach(string p in patterns)
                foreach (FileInfo fi in dir.GetFiles(p))
                {
                    string ver = FileVersionInfo.GetVersionInfo(fi.FullName).FileVersion;
                    string md5 = Helper.GetMD5FromFile(fi.FullName);
                    result.Add(new UpdateFile(fi.Name, fi.DirectoryName,ver,md5,fi.Length));
                }
            foreach (DirectoryInfo di in dir.GetDirectories())            
                result.AddRange(getAnswerUpdateFiles(di.FullName));
            
            return result;
        }


        private byte[] readStream(NetworkStream stream)
        {
            List<byte> decBuff = new List<byte>();
            int bt = 0; //узнаем длинну ибо иначе никак
            while ((bt = stream.ReadByte()) != -1)
                decBuff.Add((byte)bt);
            return decBuff.ToArray();
        }

       
    }

    
}
