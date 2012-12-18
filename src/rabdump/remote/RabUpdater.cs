//using System;
//using System.Net;
//using System.Xml;
//using System.IO;
//using System.Windows.Forms;
//using System.Threading;
//using System.ComponentModel;
//using log4net;
//using X_Tools;
//using System.Diagnostics;


//namespace rabdump
//{
//    class RabUpdateInfo
//    {
//        public string Version = "";
//        public string Info = "";
//        public string FileUri = "";
//        public string FileName = "";
//        public string FileMD5 = "";
//        public bool RequireClientRestart = false;

//        public int XmlInfoErr = 0;

//        //public RabUpdateInfo()
//        //{
//        //}

//        public bool Check()
//        {
//            try
//            {
//                Version v = new Version(Version);
//                Uri u = new Uri(FileUri);
//            }
//            catch
//            {
//                return false;
//            }
//            if (XTools.IsFilenameValid(FileName))
//            {
//                return (XmlInfoErr == 0);
//            }
//            else
//            {
//                return false;
//            }
//        }
//    }
    
    
    
//    class RabUpdater
//    {
//        public static readonly ILog logger = LogManager.GetLogger(typeof(RabUpdater));

//        public const int ErrOk = 0;
//        public const int ErrTransportFail = 10;
//        public const int ErrBadXml = 100;
//        public const int ErrUnknown = 10000;

//        /// <summary>
//        /// Для измерения скорости
//        /// </summary>
//        private readonly Stopwatch _stpw = new Stopwatch();

//        public MessageSenderCallbackDelegate MessageSenderCallback;
//        public CloseCallbackDelegate CloseCallback;

//        private Thread _thrUpdate;

//        private RabUpdateInfo _upInfo;

//        private int _lastPercents;
//        private long _lastBytes;


////        private Downloader dldr;

//        //public RabUpdater()
//        //{
//        //}

//        public static ILog log()
//        {
//            return logger;
//        }

//        private void MessageShow(string txt, string ttl)
//        {
//            if (MessageSenderCallback != null)
//            {
//                MessageSenderCallback(txt, ttl, 1,false);
//            }
//        }

//        private void ErrorShow(string txt, string ttl)
//        {
//            if (MessageSenderCallback != null)
//            {
//                MessageSenderCallback(txt, ttl, 3,false);
//            }
//        }

//        private void HideBubble()
//        {
//            if (MessageSenderCallback != null)
//            {
//                MessageSenderCallback("", "", 3, true);
//            }
//        }

//        public static RabUpdateInfo ReadUpdateInfo(string file)
//        {
//            return ReadUpdateInfo(file, "no save");
//        }

//        public static RabUpdateInfo ReadUpdateInfo(string file, string savetofile)
//        {
//            RabUpdateInfo res = new RabUpdateInfo();
//            res.XmlInfoErr = ErrUnknown;

//            WebClient client = new WebClient();
//            Stream xmlStream;

//            try
//            {
//                xmlStream = client.OpenRead(file);
//            }
//            catch (Exception e)
//            {
//                res.XmlInfoErr = ErrTransportFail;
//                log().Error("Failed to get update xml. Err: " + e.Message);
//                return res;
//            }

//            if (xmlStream != null)
//            {
//                log().Debug("Got update xml.");

//                XmlReader reader = XmlReader.Create(xmlStream);

//                XmlDocument xd = new XmlDocument();

//                xd.Load(reader);

//                if (xd.DocumentElement != null)
//                {
//                    foreach (XmlNode nd in xd.DocumentElement.ChildNodes)
//                    {
//                        if (nd.Name == "bundle")
//                        {
//                            foreach (XmlNode bn in nd.ChildNodes)
//                            {
//                                switch (bn.Name)
//                                {
//                                    case "version":
//                                        try
//                                        {
//                                            res.Version = bn.Attributes["number"].Value;
//                                        }
//                                        catch
//                                        {
//                                            res.XmlInfoErr = ErrBadXml;
//                                            log().Error("XML parse error in '" + bn.Name + "' node.");
//                                            return res;
//                                        }
//                                        log().Debug("Update version: " + res.Version.ToString());
//                                        break;
//                                    case "file":
//                                        try
//                                        {
//                                            res.FileUri = bn.Attributes["uri"].Value;
//                                            res.FileName = bn.Attributes["name"].Value;
//                                            res.FileMD5 = bn.Attributes["md5"].Value;
//                                        }
//                                        catch
//                                        {
//                                            res.XmlInfoErr = ErrBadXml;
//                                            log().Error("XML parse error in '" + bn.Name + "' node.");
//                                            return res;
//                                        }
//                                        log().Debug("Update file uri: " + res.FileUri);
//                                        log().Debug("Update file name: " + res.FileName);
//                                        log().Debug("Update file MD5: " + res.FileMD5);
//                                        break;
//                                    case "info":
//                                        res.Info = bn.InnerText;
//                                        log().Debug("Update info: " + res.Info);
//                                        break;
//                                    case "req_client_restart":
//                                        try
//                                        {
//                                            res.RequireClientRestart = (bn.Attributes["value"].Value.ToLower() == "true");
//                                        }
//                                        catch
//                                        {
//                                            res.RequireClientRestart = false;
//                                        }
//                                        log().Debug("Update needs restart: " + res.RequireClientRestart.ToString());
//                                        break;
//                                }
//                            }
//                        }
//                        else
//                        {
//                            log().Debug("Null update xml.");
//                            res.XmlInfoErr = ErrBadXml;
//                            return res;
//                        }
//                    }
//                } else
//                {
//                    log().Debug("Bad update xml.");
//                    res.XmlInfoErr = ErrBadXml;
//                    return res;
//                }
//                if (savetofile != "no save")
//                {
//                    xd.Save(savetofile);
//                }
//            }
//            res.XmlInfoErr = ErrOk;
//            xmlStream.Close();
//            return res;
//        }

//        private static RabUpdateInfo GetUpdateInfo()
//        {
//            Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\");
//            return ReadUpdateInfo("http://update.rabbit-soft.ru/rab/updates.xml", Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\updates.xml");
//        }

//        public void CheckUpdate()
//        {
//            if (_thrUpdate == null)
//            {
//                _thrUpdate = new Thread(CheckUpdateThr);
//                _thrUpdate.Start();
//            }
//            else
//            {
//                _thrUpdate.Abort();
//                _thrUpdate = new Thread(CheckUpdateThr);
//                _thrUpdate.Start();
//            }
//        }

//        private void CheckUpdateThr()
//        {
//            RabUpdateInfo r = GetUpdateInfo();

//            if (!r.Check())
//            {
//                log().Error("XML data verification failed!");
//                return;
//            }

//            bool needUpdate = false;

//            if (r.XmlInfoErr == 0)
//            {
//                string ver = Application.ProductVersion;

//                Version vUp = new Version(r.Version);
//                Version vSelf = new Version(ver);


//                if (vUp > vSelf)
//                {
//                    needUpdate = true;
//                }

//            }

//            if (needUpdate)
//            {
//                _upInfo = r;
//                log().Debug("Starting download of " + r.FileUri + "...");
//                RunUpdate(r);
//            }
//            else
//            {
//                log().Debug("No need to update!");
//                MessageShow("Обновление текущей версии не требуется!", "Обновление");
//            }
//        }

//        private void RunUpdate(RabUpdateInfo nfo)
//        {
//            _lastPercents = -1;
//            _lastBytes = 0;

//            Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath)+"\\updates\\");

//            WebClient webClient = new WebClient();
//            webClient.DownloadFileCompleted += Completed;
//            webClient.DownloadProgressChanged += ProgressChanged;
//            _stpw.Reset();
//            _stpw.Start();
//            try
//            {
//                File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + nfo.FileName + ".tmp");
//            }
//            catch (Exception e)
//            {
//                ErrorShow("Ошибка при загрузке!" + Environment.NewLine + e.Message.Replace(" | ", Environment.NewLine), "Обновление");
//                log().Error("Update download failed. " + e.Message);
//                return;
//            }
//            webClient.DownloadFileAsync(new Uri(nfo.FileUri), Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + nfo.FileName+".tmp");          
//        }

//        private void Completed(object sender, AsyncCompletedEventArgs e)
//        {
//            _stpw.Stop();
//            _stpw.Reset();
//            if (e.Cancelled)
//            {
//                log().Debug("Canceled!!!");
//            }
//            else
//            {
//                if (e.Error != null)
//                {
//                    string errMsg = e.Error.Message;
//                    if (e.Error.InnerException != null)
//                    {
//                        errMsg += " | " + e.Error.InnerException.Message;
//                    }
//                    ErrorShow("Ошибка при загрузке!" + Environment.NewLine + errMsg.Replace(" | ", Environment.NewLine), "Обновление");
//                    log().Error("Update download failed. " + errMsg);
//                }
//                else
//                {
//                    if (XTools.VerifyMD5(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + _upInfo.FileName + ".tmp", _upInfo.FileMD5))
//                    {
//                        try
//                        {
//                            File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + _upInfo.FileName);
//                        }
//                        catch (Exception e2)
//                        {
//                            ErrorShow("Ошибка при загрузке!" + Environment.NewLine + e2.Message.Replace(" | ", Environment.NewLine), "Обновление");
//                            log().Error("Update download failed. " + e2.Message);
//                            return;
//                        }
//                        try
//                        {
//                            File.Move(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + _upInfo.FileName + ".tmp", Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + _upInfo.FileName);
//                        }
//                        catch (Exception e3)
//                        {
//                            ErrorShow("Ошибка при загрузке!" + Environment.NewLine + e3.Message.Replace(" | ", Environment.NewLine), "Обновление");
//                            log().Error("Update download failed. " + e3.Message);
//                            return;
//                        }
//                        MessageShow("Обновление загружено!", "Обновление");
//                        log().Debug("Update download completed.");

//                        MessageBox.Show("Новая версия готова к установке", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                        HideBubble();

//                        Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\");

//                        Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + _upInfo.FileName, "/S"); //Batch mode
//                        if (CloseCallback != null)
//                        {
//                            CloseCallback();
//                        }
//                    }
//                    else
//                    {
//                        MessageShow("Файл обновлений поврежден! Повторите загрузку обновления.", "Обновление");
//                        log().Debug("Update download completed. MD5 check failed...");
//                    }
//                }
//            }
//        }

//        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
//        {
//            if (e.ProgressPercentage % 5 == 0)
//            {
//                if (_lastPercents != e.ProgressPercentage)
//                {
//                    _stpw.Stop();
//                    long ms = _stpw.ElapsedMilliseconds;
//                    _stpw.Reset();
//                    _stpw.Start();

//                    long bs = e.BytesReceived - _lastBytes;
//                    _lastBytes = e.BytesReceived;

//                    double speed = bs * 1000.0 / ms;

//                    _lastPercents = e.ProgressPercentage;
//                    MessageShow("Новая версия программы " + _upInfo.Version + Environment.NewLine + "Загрузка: " + e.ProgressPercentage.ToString() + "% (" + XTools.FormatBytes(e.BytesReceived) + ", " + XTools.FormatBytes(speed) + "/s)", "Обновление");
//                    log().Debug("Update download progress: " + e.ProgressPercentage + "% (" + XTools.FormatBytes(e.BytesReceived) + ", " + XTools.FormatBytes(speed) + "/s)");
//                }
//            }
//        }


//    }
//}
