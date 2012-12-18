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
                    if (RabDumpAlreadyInLan != null)
                    {
                        IPHostEntry ent = Dns.GetHostEntry(ep.Address);
                        RabDumpAlreadyInLan(ent.HostName);
                    }
                    break;
                case WHERE_IS_RD:
                        sendUdpMessage(RD_IS_HERE,ep);
                    break;

            }
        }

        private void startUpdateListen()
        {
            Thread t = new Thread(upFilesThread);
            t.IsBackground = true;
            t.Start();
        }

        private void upFilesThread()
        {
            TcpListener listener = new TcpListener(IPAddress.Any,UPDATE_FILES_PORT);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream netStream = client.GetStream();
                byte[] buffer = new byte[4096];
                netStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(readWaiter), new MyAsyncState(netStream,buffer));
            }
        }

        protected override byte[] getAnswer(byte[] inBuff)
        {
            string request = Encoding.UTF8.GetString(inBuff);
            switch (request)
            {
                case GET_UP_FILES_LIST:
                    string basePath = AppDomain.CurrentDomain.BaseDirectory;
                    List<UpdateFile> files = getAnswerUpdateFiles(basePath);
                    for(int i=0;i<files.Count;i++)
                        files[i].RelativePath = files[i].RelativePath.Replace(basePath.TrimEnd('\\'), "");
                    return serializeUF(files);
                    //return Encoding.UTF8.GetBytes(result);
                    //break;
            }
            return Encoding.UTF8.GetBytes("hellow guy");
        }

        private byte[] serializeUF(List<UpdateFile> files)
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(files.GetType());
            xs.Serialize(ms, files);
            byte[] buffer = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buffer, 0, buffer.Length);
            //string result = Encoding.UTF8.GetString(buffer);
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
                    result.Add(new UpdateFile(fi.Name, fi.DirectoryName,ver,md5));
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
