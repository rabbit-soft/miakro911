using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using log4net;
using X_Tools;
using System.Diagnostics;



namespace X_Classes
{
    class RabUpdateInfo
    {
        public string version = "";
        public string info = "";
        public string file_uri = "";
        public string file_name = "";
        public bool require_client_restart = false;

        public int xml_info_err = 0;

        public RabUpdateInfo()
        {
        }

        public bool Check()
        {
            try
            {
                Version v = new Version(version);
                Uri u = new Uri(file_uri);
            }
            catch
            {
                return false;
            }
            if (x_tools.IsFilenameValid(file_name)){
                return (xml_info_err == 0);
            } else {
                return false;
            }
        }
    }

    public delegate void MessageSenderCallbackDelegate(string msg, string ttl, ToolTipIcon ico, bool hide);
    public delegate void CloseCallbackDelegate();
    
    class RabUpdater
    {
        public static readonly ILog logger = LogManager.GetLogger(typeof(RabUpdater));

        public const int ERR_OK = 0;
        public const int ERR_TRANSPORT_FAIL = 10;
        public const int ERR_BAD_XML = 100;

        private Stopwatch stpw = new Stopwatch();

        public MessageSenderCallbackDelegate MessageSenderCallback;
        public CloseCallbackDelegate CloseCallback;

        private Thread thrUpdate;

        private RabUpdateInfo UpInfo;

        private int lastPercents;
        private long lastBytes;


//        private Downloader dldr;

        public RabUpdater()
        {
        }

        public static ILog log()
        {
            return logger;
        }

        private void MessageShow(string txt, string ttl)
        {
            if (MessageSenderCallback != null)
            {
                MessageSenderCallback(txt, ttl, ToolTipIcon.Info,false);
            }
        }

        private void ErrorShow(string txt, string ttl)
        {
            if (MessageSenderCallback != null)
            {
                MessageSenderCallback(txt, ttl, ToolTipIcon.Error,false);
            }
        }

        private void HideBubble()
        {
            if (MessageSenderCallback != null)
            {
                MessageSenderCallback("", "", ToolTipIcon.Error, true);
            }
        }

        public static RabUpdateInfo ReadUpdateInfo(string file)
        {
            return ReadUpdateInfo(file, "no save");
        }

        public static RabUpdateInfo ReadUpdateInfo(string file, string savetofile)
        {
            RabUpdateInfo res = new RabUpdateInfo();

            WebClient client = new WebClient();
            Stream XmlStream;

            try
            {
                XmlStream = client.OpenRead(file);
            }
            catch (Exception e)
            {
                res.xml_info_err = ERR_TRANSPORT_FAIL;
                log().Error("Failed to get update xml. Err: " + e.Message);
                return res;
            }
            log().Debug("Got update xml.");

            XmlReader reader = XmlReader.Create(XmlStream);

            XmlDocument xd = new XmlDocument();

            xd.Load(reader);

            foreach (XmlNode nd in xd.DocumentElement.ChildNodes)
            {
                if (nd.Name == "bundle")
                {
                    foreach (XmlNode bn in nd.ChildNodes)
                    {
                        switch (bn.Name)
                        {
                            case "version":
                                try
                                {
                                    res.version = bn.Attributes["number"].Value;
                                }
                                catch
                                {
                                    res.xml_info_err = ERR_BAD_XML;
                                    log().Error("XML parse error in '" + bn.Name + "' node.");
                                    return res;
                                }
                                log().Debug("Update version: " + res.version.ToString());
                                break;
                            case "file":
                                try
                                {
                                    res.file_uri = bn.Attributes["uri"].Value;
                                    res.file_name = bn.Attributes["name"].Value;
                                }
                                catch
                                {
                                    res.xml_info_err = ERR_BAD_XML;
                                    log().Error("XML parse error in '" + bn.Name + "' node.");
                                    return res;
                                }
                                log().Debug("Update file uri: " + res.file_uri);
                                log().Debug("Update file name: " + res.file_name);
                                break;
                            case "info":
                                res.info = bn.InnerText;
                                log().Debug("Update info: " + res.info);
                                break;
                            case "req_client_restart":
                                try
                                {
                                    res.require_client_restart = (bn.Attributes["value"].Value.ToLower() == "true");
                                }
                                catch
                                {
                                    res.require_client_restart = false;
                                }
                                log().Debug("Update needs restart: " + res.require_client_restart.ToString());
                                break;
                        }
                    }
                }
            }
            if (savetofile != "no save")
            {
                xd.Save(savetofile);
            }
            return res;
        }

        private RabUpdateInfo GetUpdateInfo()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\");

            return ReadUpdateInfo("http://updates.trustbox.ru/rab/updates.xml", Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\updates.xml");
        }

        public void CheckUpdate()
        {
            if (thrUpdate == null)
            {
                thrUpdate = new Thread(new ThreadStart(CheckUpdateThr));
                thrUpdate.Start();
            }
            else
            {
                thrUpdate.Abort();
                thrUpdate = new Thread(new ThreadStart(CheckUpdateThr));
                thrUpdate.Start();
            }
        }

        private void CheckUpdateThr()
        {
            RabUpdateInfo r = GetUpdateInfo();

            if (!r.Check())
            {
                log().Error("XML data verification failed!");
                return;
            }

            bool needUpdate = false;

            if (r.xml_info_err == 0)
            {
                string ver = Application.ProductVersion;

                Version v_up = new Version(r.version);
                Version v_self = new Version(ver);


                if (v_up > v_self)
                {
                    needUpdate = true;
                }

            }

            if (needUpdate)
            {
                UpInfo = r;
                log().Debug("Starting download of " + r.file_uri + "...");
                RunUpdate(r);
            }
            else
            {
                log().Debug("No need to update!");
                MessageShow("Обновление текущей версии не требуется!", "Обновление");
            }



        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {



            if (e.ProgressPercentage % 5 == 0)
            {

                if (lastPercents != e.ProgressPercentage)
                {
                    stpw.Stop();
                    long ms = stpw.ElapsedMilliseconds;
                    stpw.Reset();
                    stpw.Start();

                    long bs = e.BytesReceived-lastBytes;
                    lastBytes = e.BytesReceived;

                    double speed = (float)bs / (float)ms * 1000.0;

                    lastPercents = e.ProgressPercentage;
                    MessageShow("Новая версия программы " + UpInfo.version + Environment.NewLine + "Загрузка: " + e.ProgressPercentage.ToString() + "% (" + x_tools.formatBytes(e.BytesReceived) + ", " + x_tools.formatBytes(speed) + "/s)", "Обновление");
                    log().Debug("Update download progress: " + e.ProgressPercentage + "% (" + x_tools.formatBytes(e.BytesReceived) + ", " + x_tools.formatBytes(speed) + "/s)");
                }
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            stpw.Stop();
            stpw.Reset();
            if (e.Cancelled)
            {
                log().Debug("Canceled!!!");
            }
            else
            {
                if (e.Error != null)
                {
                    string errMsg = e.Error.Message;
                    if (e.Error.InnerException != null)
                    {
                        errMsg += " | " + e.Error.InnerException.Message;
                    }
                    ErrorShow("Ошибка при загрузке!"+Environment.NewLine+errMsg.Replace(" | ",Environment.NewLine), "Обновление");
                    log().Error("Update download failed. " + errMsg);
                }
                else
                {
                    MessageShow("Обновление загружено!", "Обновление");
                    log().Debug("Update download completed.");

                    MessageBox.Show("Новая версия готова к установке", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HideBubble();
                    
                    Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\");

                    Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + UpInfo.file_name, "test");
                    if (CloseCallback != null)
                    {
                        CloseCallback();
                    }

                }
            }
            
        }

        private void RunUpdate(RabUpdateInfo nfo)
        {

            lastPercents = -1;
            lastBytes = 0;

            Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath)+"\\updates\\");


            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            stpw.Reset();
            stpw.Start();
            webClient.DownloadFileAsync(new Uri(nfo.file_uri), Path.GetDirectoryName(Application.ExecutablePath) + "\\updates\\" + nfo.file_name);
            




        }


    }
}
