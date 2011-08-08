#if DEBUG
#define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using log4net;
using SeasideResearch.LibCurlNet;
using System.Xml;
using System.IO;
#if PROTECTED
using RabGRD;
#endif

namespace rabdump
{
    static class RabServWorker
    {
#if !DEMO
        public enum State
        {
            /// <summary>
            /// Класс не выполняет никакой работы
            /// </summary>
            Free,
            Conection_Failed,
            UploadDump_Loading,
            /// <summary>
            /// Успешная загрузка на сервер
            /// </summary>
            UploadDump_Loaded,
            /// <summary>
            /// Ошибка при загрузке на сервер
            /// </summary>
            UploadDump_LoadErr,
            /// <summary>
            /// Ожидание списка Дампов от сервера
            /// </summary>
            DumpList_Wait,
            /// <summary>
            /// Список дампов получен. На сервере имеются дампы
            /// </summary>
            DumpList_NodesYes,
            /// <summary>
            /// Список дампов получен. На сервере дампов нет.
            /// </summary>
            DumpList_NodesNo,
            /// <summary>
            /// Процесс скачивания дампа
            /// </summary>
            DownloadDump_Loading,
            /// <summary>
            /// Дамп скачан
            /// </summary>
            DownloadDump_Loaded,
            /// <summary>
            /// Ошибка при скачке дампа
            /// </summary>
            DownloadDump_LoadErr,
        }

        struct ServData
        {
            internal string farmname;
            internal string dumpPath;
            internal XmlDocument dumpList;
            internal FileStream uploadFS;
            internal BinaryWriter uploadBW;
            internal System.Diagnostics.Stopwatch watcher;
            internal Double lastUploadedBytes;
        }

        private const string dumpListNodeName ="dumplist";
        private const string dumpListItemName = "dump";
        private static string _tmpPath = Path.GetTempPath();
        private static string NL = Environment.NewLine;
        private static string _url = "localhost";
        private static string lastDumpURI = _url + "/" + "dumplist.php";
        private static string uploadDumpURI = _url + "/" + "uploader.php";
        private static string downloadDumpURI = _url + "/" + "getdump.php";
        private static ILog _logger = LogManager.GetLogger(typeof(RabServWorker));
        private static ServData _crossData;
        private static ArchiveJobThread _ajt;
        /// <summary>
        /// В каком состоянии находися Класс в данный момент
        /// </summary>
        private static State _state = State.Free;

        public static State CurState { get { return _state; } }

        public static event MessageSenderCallbackDelegate OnMessage;

        public static string ServerUrl{get{ return _url;}}

        public static void SetServerUrl(string url)
        {
            if (_state != State.Free) return;
            _url = url == "" ? "vm1" : url;
            lastDumpURI = _url + "/" + "dumplist.php";
            uploadDumpURI = _url + "/" + "uploader.php";
            downloadDumpURI = _url + "/" + "getdump.php";
        }

        public static void MakeDump(ArchiveJob j)
        {
            _crossData = new ServData();
#if PROTECTED
            _crossData.farmname = GRD.Instance.GetOrgName();           
#elif DEBUG
            _crossData.farmname = "testing";
#endif
            _ajt = new ArchiveJobThread(j);
            Thread t = new Thread(makeDump);
            t.Start();
        }

        /// <summary>
        /// Скачивает Дам с сервера
        /// </summary>
        /// <param name="filename">Название файла</param>
        /// <returns>Путь к скачанному файлу</returns>
        public static string DownloadDump(string filename)
        {
#if !NOCATCH
            try
            {
#endif
                callOnMessage("Скачиваем с сервера Резервную копию", "Начало загрузки", 1);
#if PROTECTED
                _crossData.farmname = GRD.Instance.GetOrgName();           
#elif DEBUG
                _crossData.farmname = "testing";
#endif
                _crossData.dumpPath = filename;
                downloadDump();
                if (_state == State.DownloadDump_Loaded)
                {
                    _state = State.Free;
                    callOnMessage(String.Format("Файл успешно скачан{0:s}Переходим к востановлению.", Environment.NewLine), "Успех", 1);
                    return _tmpPath + filename;
                }
                
                _state = State.Free;
                return "";
#if !NOCATCH
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("DownloadDump", ex);
                return "";
            }
#endif
        }

        public static List<string> GetDumpList(string farmname,string dbName)
        {
            if (_crossData.dumpList == null)
            {
                getDumpList(farmname);
            }

            List<string> result = new List<string>();
            if (_crossData.dumpList!= null && _crossData.dumpList.FirstChild.ChildNodes.Count>0)
            {          
                foreach (XmlNode nd in _crossData.dumpList.FirstChild.ChildNodes)
                {
                    if (nd.Attributes["file"].Value.Contains(dbName.Replace(' ','_')))
                    {
                        if(!result.Contains(nd.Attributes["file"].Value))
                            result.Add( nd.Attributes["file"].Value);                      
                    }
                }
            }
            _state = State.Free;
            return result;
        }

        private static void makeDump()      
        {
            while(_ajt.JobIsBusy)           
                Thread.Sleep(2000);
            if (_state != State.Free)
            {
                callOnMessage("Пока нельзя отправить","",2);
                return;
            }        
            _logger.Debug("making Server Dump");
            callOnMessage("Начало отсылки" + NL + _ajt.JobName, "Отправка", 1, false);
            getDumpList();
            if (_state == State.Conection_Failed)
            {
                callOnMessage("Не удалось подключиться к серверу", "ошибка", 3);
                return;
            }
            if (_state == State.DumpList_NodesYes)
            {
                if (!System.IO.File.Exists(Options.Get().Path7Z))
                {
                    _logger.Warn("7z not specified");
                    _state = State.DumpList_NodesNo;
                }
                else if (_crossData.dumpList != null)
                {
                    _logger.Debug("we have a DumpList");
                    bool coincidence = false;
                    foreach (XmlNode nd in _crossData.dumpList.FirstChild.ChildNodes)
                    {
                        if (_ajt.FileExists(nd.Attributes["file"].Value, nd.Attributes["md5dump"].Value))
                        {
                            coincidence = true;
                            string srcFile = nd.Attributes["file"].Value;
                            string trgFile = _ajt.GetLatestDump();
                            if (srcFile != Path.GetFileName(trgFile))
                            {
                                srcFile = Path.GetDirectoryName(trgFile) + "\\" + srcFile;
                                srcFile = ArchiveJobThread.ExtractDump(srcFile);
                                trgFile = ArchiveJobThread.ExtractDump(trgFile);
                                string diffPath = makeDiff(srcFile, trgFile);
                                diffPath = ArchiveJobThread.ZipFile(diffPath, true);
                                File.Delete(srcFile);File.Delete(trgFile);
                                _crossData.dumpPath = diffPath;
                                uploadDump();
                                File.Delete(diffPath);
                                break;
                            }
                            else
                            {
                                callOnMessage("На сервере сама последняя версия БД", "загрузка отменена", 0);
                                break;
                            }
                        }                     
                    }
                    if(!coincidence)
                        _state = State.DumpList_NodesNo;
                }
            }           
            if (_state == State.DumpList_NodesNo)
            {
                _crossData.dumpPath = _ajt.GetLatestDump();
                if (_crossData.dumpPath != "")
                {
                    uploadDump();
                    if (_state == State.UploadDump_Loaded)
                        callOnMessage("Файл отправлен успешно", "Успех", 1);
                    else callOnMessage("Ошибка при отправлении файла", "Внимание", 2);
                }
                else OnMessage("Нет резервных копий для отсылки", "Отправка файла", 2, false);
            }
            _state = State.Free;
        }

        /// <summary>
        /// Файл должен делать патч
        /// </summary>
        /// <param name="srcFile">Старый файл</param>
        /// <param name="trgFile">Новый Файл</param>
        /// <returns>Путь к файлу с Дифом</returns>
        [Obsolete("Недописан")]
        private static string makeDiff(string srcFile, string trgFile)
        {
            
            /*FileStream srcFS = new FileStream(srcFile, FileMode.Open, FileAccess.Read);
            StreamReader srcSR = new StreamReader(srcFile);
            FileStream trgFS = new FileStream(trgFile, FileMode.Open, FileAccess.Read);
            StreamReader trgSR = new StreamReader(trgFile);
            srcSR.Close(); srcFS.Close();
            trgSR.Close(); trgFS.Close();*/
            return trgFile;       
        }

        private static void downloadDump(string farmname,string dumpFile)
        {
            _logger.Info("start Downloading Dump");
            _state = State.DownloadDump_Loading;
            _crossData.farmname = farmname;
            _crossData.dumpPath = dumpFile;
            long offset = unDownloaded(dumpFile);

            try
            {
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                MultiPartForm mf = new MultiPartForm();
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "farm",
                    CURLformoption.CURLFORM_COPYCONTENTS, defendString(farmname),
                    CURLformoption.CURLFORM_END);
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "file",
                    CURLformoption.CURLFORM_COPYCONTENTS, dumpFile,
                    CURLformoption.CURLFORM_END);
                if (offset != -1)               
                    mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "offset",
                        CURLformoption.CURLFORM_COPYCONTENTS, offset.ToString(),
                        CURLformoption.CURLFORM_END);
                

                Easy easy = new Easy();
                easy.SetOpt(CURLoption.CURLOPT_HTTPPOST, mf);
                easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);
                easy.SetOpt(CURLoption.CURLOPT_URL, downloadDumpURI);
                easy.SetOpt(CURLoption.CURLOPT_LOW_SPEED_LIMIT, 10);
                easy.SetOpt(CURLoption.CURLOPT_LOW_SPEED_TIME, 30);

                Easy.WriteFunction wf = new Easy.WriteFunction(OnWriteData);
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);

                Easy.DebugFunction df = new Easy.DebugFunction(onDebug);
                easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, df);
              
                Easy.ProgressFunction pf = new Easy.ProgressFunction(onProgress);
                easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);            
               
                easy.Perform();
                easy.Cleanup();
                mf.Free();

                Curl.GlobalCleanup();

                if (_state != State.DownloadDump_Loaded)
                {
                    _state = State.DownloadDump_LoadErr;
                    if (_crossData.uploadBW != null)
                    {
                        _crossData.uploadBW.Close();
                        _crossData.uploadBW = null;
                    }
                    if (_crossData.uploadFS != null)
                    {
                        _crossData.uploadFS.Close();
                        _crossData.uploadFS = null;
                    }
                }
                if (_crossData.watcher != null)
                {
                    _crossData.watcher.Stop();
                    _crossData.watcher = null;
                }
            }
            catch (NullReferenceException ex)
            {
                callOnMessage("Возможно отсутствует сетевое подключение", "Ошибка при отправке", 3);
                _logger.Error("sendfile", ex);
                if (_crossData.watcher != null)
                {
                    _crossData.watcher.Stop();
                    _crossData.watcher = null;
                }
                _state = State.Free;
            }
#if !NOCATCH          
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                _state = State.Free;
            }
#endif
        }
        private static void downloadDump()
        {
            downloadDump(_crossData.farmname,_crossData.dumpPath);
        }



        private static void getDumpList(string farmname)
        {
            try
            {
                _state = State.DumpList_Wait;
                _crossData.farmname = farmname;
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                MultiPartForm mf = new MultiPartForm();
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "farm",
                    CURLformoption.CURLFORM_COPYCONTENTS, defendString(farmname),
                    CURLformoption.CURLFORM_END);

                Easy easy = new Easy();
                easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);
                easy.SetOpt(CURLoption.CURLOPT_URL, lastDumpURI);
                easy.SetOpt(CURLoption.CURLOPT_HTTPPOST, mf);
                easy.SetOpt(CURLoption.CURLOPT_CONNECTTIMEOUT, 10);

                Easy.WriteFunction wf = new Easy.WriteFunction(OnWriteData);
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);

                Easy.DebugFunction df = new Easy.DebugFunction(onDebug);
                easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, df);
                
                Easy.ProgressFunction pf = new Easy.ProgressFunction(onProgress);
                easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);


                easy.Perform();
                easy.Cleanup();
                mf.Free();

                Curl.GlobalCleanup();
                if (_state == State.DumpList_Wait)
                    _state = State.DumpList_NodesNo;
            }
            catch (NullReferenceException ex)
            {
                callOnMessage("Возможно отсутствует сетевое подключение", "Ошибка при отправке", 3);
                _logger.Error("sendfile", ex);
                _state = State.Free;
            }
#if !NOCATCH          
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                _state = State.Free;
            }
#endif
        }
        private static void getDumpList()
        {
            getDumpList(_crossData.farmname);
        }


        private static void uploadDump(string farmname,string dumpPath)
        {
            try
            {
                _state = State.UploadDump_Loading;
                _crossData.farmname = farmname;
                _crossData.dumpPath = dumpPath;

                _logger.Info("sending dump on server");
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                // <form action="http://mybox/cgi-bin/myscript.cgi
                //  method="post" enctype="multipart/form-data">
                MultiPartForm mform = new MultiPartForm();

                // <input name="frmUsername">
                mform.AddSection(CURLformoption.CURLFORM_COPYNAME, "farm",
                    CURLformoption.CURLFORM_COPYCONTENTS, farmname,
                    CURLformoption.CURLFORM_END);

                // <input name="frmPassword">
                mform.AddSection(CURLformoption.CURLFORM_COPYNAME, "type",
                    CURLformoption.CURLFORM_COPYCONTENTS, "dump",
                    CURLformoption.CURLFORM_END);

                // <input type="File" name="f1">
                mform.AddSection(CURLformoption.CURLFORM_COPYNAME, "uploadedfile",
                    CURLformoption.CURLFORM_FILE, dumpPath,
                    CURLformoption.CURLFORM_CONTENTTYPE, "application/binary",
                    CURLformoption.CURLFORM_END);

                Easy easy = new Easy();
                easy.SetOpt(CURLoption.CURLOPT_CONNECTTIMEOUT, 10);
                easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);
                easy.SetOpt(CURLoption.CURLOPT_URL, uploadDumpURI);
                easy.SetOpt(CURLoption.CURLOPT_HTTPPOST, mform);

                Easy.DebugFunction df = new Easy.DebugFunction(onDebug);
                easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, df);
               
                Easy.ProgressFunction pf = new Easy.ProgressFunction(onProgress);
                easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);

                easy.Perform();
                easy.Cleanup();
                mform.Free();

                Curl.GlobalCleanup();
                if (_crossData.watcher != null)
                {
                    _crossData.watcher.Stop();
                    _crossData.watcher = null;
                } 
            }
            catch (NullReferenceException ex)
            {
                callOnMessage("Возможно отсутствует сетевое подключение", "Ошибка при отправке", 3);
                _logger.Error("sendfile", ex);
                _state = State.Free;
            }
#if !NOCATCH
            /*catch (ApplicationException ex)
            {
                callOnMessage(ex.Message, "Ошибка при отправке", 3);
                _logger.Error("sendfile", ex);
                _state = State.Free;
            }*/
            catch (Exception ex)
            {
                _logger.Fatal("sendfile", ex);
                _state = State.Free;
            }
#endif
        }
        private static void uploadDump()
        {
            uploadDump(_crossData.farmname, _crossData.dumpPath);
        }

        /// <summary>
        /// При получении данных от сервера
        /// </summary>
        /// <returns>size * nmemb</returns>
        public static Int32 OnWriteData(Byte[] buf, Int32 size, Int32 nmemb, Object extraData)
        {
            string msg = ""; 
            if (_state != State.DownloadDump_Loading)
            {
                msg = System.Text.Encoding.UTF8.GetString(buf);
                _logger.DebugFormat("Curl WriteData: {0:s}", msg);
            }
            if (_state == State.DumpList_Wait)
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.LoadXml(msg);
                    if (doc.FirstChild.Name == dumpListNodeName)
                        if (doc.FirstChild.ChildNodes.Count > 0)
                        {
                            _crossData.dumpList = dumpListDateDSort(doc);
                            _state = State.DumpList_NodesYes;                       
                        }
                        else _state = State.DumpList_NodesNo;
                }
                catch (XmlException) { }
            }
            else if (_state == State.DownloadDump_Loading)
            {
                if (_crossData.uploadFS == null)
                {
                    _crossData.uploadFS = new FileStream(_tmpPath + Path.GetFileName(_crossData.dumpPath) + ".tmp", FileMode.OpenOrCreate, FileAccess.Write);
                    _crossData.uploadBW = new BinaryWriter(_crossData.uploadFS);
                    _crossData.uploadBW.Seek(0, SeekOrigin.End);
                }
                _crossData.uploadBW.Write(buf);
            }
            return size * nmemb;
        }

        private static Int32 onProgress(Object extraData, Double dlTotal, Double dlNow, Double ulTotal, Double ulNow)
        {
            _logger.DebugFormat("Curl Progress: {0} {1} {2} {3}", dlTotal, dlNow, ulTotal, ulNow);
            if (_state == State.DownloadDump_Loading && dlNow != 0 && dlTotal == dlNow)
            {
                _state = State.DownloadDump_Loaded;
                _crossData.uploadBW.Close(); 
                _crossData.uploadFS.Close();
                File.Move(_crossData.uploadFS.Name, _crossData.uploadFS.Name.Replace(".tmp",""));
                _crossData.dumpPath = _crossData.uploadFS.Name;
                _crossData.uploadFS = null;            
            }
            else if (_state == State.UploadDump_Loading || _state == State.DownloadDump_Loading)
            {
                Double Total = 0, Now = 0;
                if (_state == State.UploadDump_Loading) { Total = ulTotal; Now = ulNow; }
                if (_state == State.DownloadDump_Loading)
                {
                    if (dlTotal == 0) return 0;//Пока идет отправка запроса, и не получен размер файла
                    Total = dlTotal;
                    Now = dlNow;
                }
                if (Total == Now)
                {
                    if (_state == State.UploadDump_Loading) _state = State.UploadDump_Loaded;
                    if (_state == State.DownloadDump_Loading) _state = State.DownloadDump_Loaded;
                    _crossData.watcher.Stop();
                    _crossData.watcher = null;
                }
                else
                {
                    if (_crossData.watcher == null)
                    {
                        _crossData.watcher = new System.Diagnostics.Stopwatch();
                        _crossData.lastUploadedBytes = Now;
                        _crossData.watcher.Start();
                    }
                    else
                    {
                        if (_crossData.watcher.Elapsed.Seconds > 10)
                        {
                            _crossData.watcher.Stop();
                            double speed = (Now - _crossData.lastUploadedBytes) / _crossData.watcher.Elapsed.Seconds;

                            _crossData.watcher.Reset();

                            if (_state == State.UploadDump_Loading)
                                callOnMessage(String.Format("Отправленно: {0:s}{1:s}Скорость: {2:s}/сек", getSize(Now), Environment.NewLine, getSize(speed)), "отправка", 1);
                            if (_state == State.DownloadDump_Loading)
                                callOnMessage(String.Format("Скачано: {0:s}{1:s}Скорость: {2:s}/сек", getSize(_crossData.uploadFS.Length), Environment.NewLine, getSize(speed)), "Загрузка", 1);

                            _crossData.lastUploadedBytes = Now;
                            _crossData.watcher.Start();
                        }
                    }
                }
            }
            return 0;
        }

        private static void onDebug(CURLINFOTYPE infoType, String msg, Object extraData)
        {
            if((infoType ==CURLINFOTYPE.CURLINFO_DATA_IN && msg.Length<1000) || infoType == CURLINFOTYPE.CURLINFO_TEXT
#if DEBUG
                || infoType == CURLINFOTYPE.CURLINFO_HEADER_OUT || infoType == CURLINFOTYPE.CURLINFO_HEADER_IN
#endif
                )
            {
                _logger.DebugFormat("Curl Debug: [{1:s}] {0}", msg,infoType.ToString());
                servMsg(msg);
            }
        }

        private static string defendString(string str)
        {
            return str.Replace("\"", "").Replace("\\", "").Replace("/", "");
        }

        /// <summary>
        /// Вылавливание информации, которую нужно показать в ToolTip
        /// </summary>
        /// <param name="msg"></param>
        private static void servMsg(string msg)
        {
            if(msg.Contains("couldn't connect to host"))
            {
                callOnMessage("Не удалось подключиться к серверу", "Ошибка", 3);
            }
            else if(msg.Contains("[File Upload Successful]"))
            {
                _state = State.UploadDump_Loaded;
                //callOnMessage("Файл отправлен успешно", "Успех", 1);
            }
            else if(msg.Contains("[File Upload Error]"))
            {
                _state = State.UploadDump_LoadErr;
                //callOnMessage("Ошибка при отправлении файла", "Внимание", 2);
            }
            else if (msg.StartsWith("transfer closed with") && msg.EndsWith("bytes remaining to read\n"))
            {
                _state = State.DownloadDump_LoadErr;
            }
            else if (msg == "connect() timed out!\n")          
                _state = State.Conection_Failed;
            
        }

        private static void callOnMessage(string msg, string ttl, int type, bool hide)
        {
            if (OnMessage != null)
            {
                OnMessage(msg, ttl, type, hide);
            }
        }
        private static void callOnMessage(string msg, string ttl, int type)
        {
            callOnMessage(msg, ttl, type, false);
        }
        private static void callOnMessage(string msg, string ttl)
        {
            callOnMessage(msg, ttl, 0, false);
        }

        /// <summary>
        /// Сортирует Ноды дампов по убыванию Дат.
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns>Отсортированная XML по датам</returns>
        private static XmlDocument dumpListDateDSort(XmlDocument srcDocument)
        {
            XmlDocument sortedDoc = new XmlDocument();
            XmlElement rootNode = sortedDoc.CreateElement(srcDocument.FirstChild.Name);
            sortedDoc.AppendChild(rootNode);
            foreach (XmlNode srcND in srcDocument.FirstChild.ChildNodes)
            {
                if (srcND.Name != dumpListItemName) continue;
                if (rootNode.ChildNodes.Count == 0)
                    rootNode.AppendChild(sortedDoc.ImportNode(srcND,true));
                else
                {
                    DateTime srcDT = DateTime.Parse(srcND.Attributes["datetime"].Value);
                    foreach (XmlNode destND in rootNode.ChildNodes)
                    {                      
                        DateTime destDT = DateTime.Parse(destND.Attributes["datetime"].Value);
                        if (srcDT > destDT)
                        {
                            rootNode.InsertBefore(sortedDoc.ImportNode(srcND, true), destND);
                            break;
                        }
                        if (destND == rootNode.LastChild)
                        {
                            rootNode.AppendChild(sortedDoc.ImportNode(srcND, true));
                            break;
                        }
                    }
                }
            }
#if DEBUG
            sortedDoc.Save("sortedDoc.xml");
#endif
            return sortedDoc;
        }

        /// <summary>
        /// Округляет число байтов до Кбайт,Мбайт,ГБайт
        /// </summary>
        /// <param name="bytes">Число байт</param>
        /// <returns>Строку типа "56,98 КБ"</returns>
        private static string getSize(Double bytes)
        {
            string units = "байт";
            double amount = bytes;
            if (amount / 1000 > 1)
            {
                units = "КБ";
                amount = bytes / 1000;
            }
            if (amount / 1000 > 1)
            {
                units = "МБ";
                amount = amount / 1000;
            }
            if (amount / 1000 > 1)
            {
                units = "ГБ";
                amount = amount / 1000;
            }
            return String.Format("{0:0.00} {1:s}",amount,units);
        }

        /// <summary>
        /// Возвращает размер (offset) недокачанного файла
        /// Если нет недокачанного, то возвращает  "-1"
        /// </summary>
        /// <param name="filename">Название файла</param>
        /// <returns></returns>
        private static long unDownloaded(string filename)
        {
            filename = _tmpPath+Path.GetFileName(filename)+".tmp";
            if(File.Exists(filename))
            {
                FileInfo fi = new FileInfo(filename);
                return fi.Length;
            }
            else return -1;
        }
#endif
    }
}
