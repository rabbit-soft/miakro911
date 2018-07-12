#if DEBUG
#define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using gamlib;
using System.Xml.Serialization;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;

namespace rabnet
{
#if !DEMO
    public delegate void RabDumpFindedHandler(string address);
    public delegate void RabLanBoolHandler(bool val);

    class RabNetLan:RabLan
    {
        public event RabDumpFindedHandler OnRabDumpFinded;
        public event RabLanBoolHandler OnUpdateRequired;
        public event UpdateFinishHandler OnUpdateFinished;
        public event UpdateFailHandler OnUpdateError;

        private string _rdAddress;
        RabNetUpdater _appUp;

        public RabNetLan(string rabDumpAddress)
        {
            _rdAddress = rabDumpAddress;
        }

        public void Start()
        {
            startUdpListen();
        }

        public void CheckUpdate()
        {
            RabNetUpdater.DeleteOldFiles(AppDomain.CurrentDomain.BaseDirectory);

            IPAddress rdAddress;
            if (!IPAddress.TryParse(_rdAddress, out rdAddress))
            {
                searchRD();
                return;
            }
            if (_appUp == null)
            {
                _appUp = new RabNetUpdater(new RabNetUpdater.GetRemUrlHandler(getRemoteUrl), 
                    new RabNetUpdater.GetUpFilesHandler(getUpdateFiles),
                    new RabNetUpdater.DLFileHandler(downloadFile));
                _appUp.FilesChecked += new FilesCheckedHandler(au_FilesChecked);
                _appUp.UpdateFinish += new UpdateFinishHandler(au_UpdateFinish);
                //_appUp.OnError += new UpdateFailHandler(_appUp_OnError);
            }
            _appUp.Check();
        }

        //void _appUp_OnError(Exception exc)
        //{
            
        //}

        private void searchRD()
        {
            this.OnRabDumpFinded += new RabDumpFindedHandler(RabNetLan_OnRabDumpFinded);
            sendUdpMessage(WHERE_IS_RD);
        }

        bool au_FilesChecked(List<UpdateFile> ufiles)
        {
            bool update = false;
            string curVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            try
            {
                if (ufiles.Count == 0) throw new Exception("На сервере нет новых файлов для обновления");

                for (int i = 0; i < ufiles.Count; )
                {
                    if (ufiles[i].Name == "CodeStorage64_grdnetagent.exe")
                    {
                        ufiles.RemoveAt(i);
                        continue;
                    }
                    if (ufiles[i].Name == "rabnet.exe") {
                        update = Helper.VersionCompare(ufiles[i].Version, curVer) == 1;
                    }
                    i++;
                }
            }
            catch (Exception exc)
            {
                _logger.Error(exc);
            }
            if(OnUpdateRequired != null)
                OnUpdateRequired(update);
            return update;
        }

        void au_UpdateFinish()
        {
            if (OnUpdateFinished != null)
                OnUpdateFinished();
        }

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


        #region update
        private Stream sendTcpMessage(string message)
        {
            TcpClient client = new TcpClient();
            client.Connect(_rdAddress, UPDATE_FILES_PORT);
            NetworkStream netStream = client.GetStream();
            Stream result = null;
#if !NOCATCH
            try
            {
#endif
                byte[] buffer = makeSendBuffer(message);
                netStream.Write(buffer, 0, buffer.Length);

                long totalRecBytesCount = 0;
                long lenght = -1;
                TcpStreamDataType dtype;
                int offset=0;

                do
                {
                    if (offset == 0)
                        buffer = new byte[65536];
                    int len = (lenght < 0 || buffer.Length < (lenght - totalRecBytesCount)) ? buffer.Length : (int)(lenght - totalRecBytesCount);
                    IAsyncResult asyncResult = netStream.BeginRead(buffer, offset, (len - offset), null, null);

                    WaitHandle waiter = asyncResult.AsyncWaitHandle;
                    bool good = waiter.WaitOne(10000, true);
                    if (!good) continue;

                    int recBytesCount = netStream.EndRead(asyncResult)+offset;
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
                        switch (dtype)
                        {                            
                            case TcpStreamDataType.ErrorMessage: throw new RabLanException(Encoding.UTF8.GetString(buffer));
                            case TcpStreamDataType.File:
                                string tmpFile = Path.Combine(Path.GetTempPath(), String.Format("rabnet_{0:s}.tmp", Guid.NewGuid().ToString()));
                                result = new FileStream(tmpFile, FileMode.CreateNew, FileAccess.ReadWrite);
                                break;
                            default: result = new MemoryStream(); break;
                        }

                    }

                    result.Write(buffer, 0, recBytesCount);
                    totalRecBytesCount += recBytesCount;
                }
                while (lenght != totalRecBytesCount);
#if !NOCATCH
            }
            catch (Exception exc)
            {
                _logger.Error(exc);
            }
            finally
            {
#endif
                netStream.Close(); 
#if !NOCATCH 
            }
#endif
            result.Position = 0;
            return result;
        }

        protected string getRemoteUrl()
        {
            return _rdAddress;
        }

        protected List<UpdateFile> getUpdateFiles()
        {
            try
            {
                MemoryStream ms = sendTcpMessage(GET_UP_FILES_LIST) as MemoryStream;
                ms.Position = 0;
                byte[] buffer = new byte[ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                return unserializeUF(buffer);
            }
            catch(Exception exc)
            {
                if (exc is SocketException)
                {
                    _rdAddress = "";
                    searchRD();
                }
                return null;
            }
        }

        protected virtual Stream downloadFile(string pathname,long offset)
        {
            FileStream fs = sendTcpMessage(GET_UP_FILE + MSG_DELIMITER + pathname) as FileStream;
            
            return fs;
        }

        private List<UpdateFile> unserializeUF(byte[] buff)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<UpdateFile>));
            MemoryStream ms = new MemoryStream(buff, 0, buff.Length);
            return (xs.Deserialize(ms) as List<UpdateFile>);
        }
        #endregion update
        
    }

    class RabNetUpdater : AppUpdater
    {
        internal delegate string GetRemUrlHandler();
        internal delegate List<UpdateFile> GetUpFilesHandler();
        internal delegate Stream DLFileHandler(string file,long offset);

        private GetRemUrlHandler _getRemoteUrl;
        private GetUpFilesHandler _getUpdateFiles;
        private DLFileHandler _dlFile;

        public RabNetUpdater(GetRemUrlHandler h1, GetUpFilesHandler h2,DLFileHandler h3)
        {
            _getRemoteUrl = h1;
            _getUpdateFiles = h2;
            _dlFile = h3;
        }

        protected override string getRemoteUrl()
        {
            return _getRemoteUrl();
        }

        protected override List<UpdateFile> getUpdateFiles()
        {
            return _getUpdateFiles();
        }
    }
#endif
}
