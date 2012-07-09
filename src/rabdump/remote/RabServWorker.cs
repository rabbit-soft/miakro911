#if DEBUG
//#define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using log4net;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Globalization;
#if PROTECTED
    using RabGRD;
    using pEngine;   
#endif

namespace rabdump
{
    public class UploadFile
    {
        public UploadFile()
        {
            ContentType = "application/octet-stream";
        }
        public string Name;
        public string Filename;
        public string ContentType;
        public Stream Stream;
    }

    /// <summary>
    /// Нужен для отправки сообщения из классов,
    /// работающих с сервером, основной программе
    /// </summary>
    /// <param name="msg">Сообщение</param>
    /// <param name="ttl">Заголовок</param>
    /// <param name="type">Error (3);Warning (2);Info (1);None (0)</param>
    /// <param name="hide">Скрыть ли значек из трея</param>
    public delegate void MessageSenderCallbackDelegate(string msg, string ttl, int type, bool hide);
    public delegate void CloseCallbackDelegate();

    /// <summary>
    /// Класс отсылающий РК и Статистику на сервер
    /// </summary>
    static class RabServWorker
    {
        private static string _url = "http://192.168.0.95/rabServ/";
        private static ILog _logger = LogManager.GetLogger(typeof(RabServWorker));
        private static ArchiveJobThread _ajt;
        private static RequestSender _reqSend = null;
        private static bool _busy = false;

        internal static RequestSender ReqSender
        {
            get
            {
                if (_reqSend == null)
                {
                    _reqSend = new RequestSender();
                    _reqSend.UserID = GRD.Instance.GetClientID();
                    _reqSend.Key = GRD.Instance.GetKeyCode();
                    _reqSend.Url = _url;
                }
                return _reqSend;
            }
        }
        public static event MessageSenderCallbackDelegate OnMessage;     

        /// <summary>
        /// Адрес удаленного сервера
        /// </summary>
        public static string Url
        {
            get { return _url; }
            set 
            {
                if (value == null || value == "")
                    return;
                _url = value; 
            }
        }

        public static void MakeDump(ArchiveJob j)
        {
            //_crossData = new ServData();
            _ajt = new ArchiveJobThread(j);
            _ajt.Run();
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
                string filePath = Path.GetTempPath() + filename + ".tmp";
                callOnMessage("Скачиваем с сервера Резервную копию", "Начало загрузки", 1);
                long offset = unDownloaded(filePath);
                downloadDump(filename, filePath, offset);

                callOnMessage(String.Format("Файл успешно скачан{0:s}Переходим к востановлению.", Environment.NewLine), "Успех", 1);
                return filePath;
#if !NOCATCH
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("DownloadDump", ex);
                return "";
            }
#endif
        }

        public static void SendWebReport()
        {           
            Thread t = new Thread(sendWebReport);
            t.Start();
        }

        /// <summary>
        /// Отправляет Веб-отчет
        /// </summary>
        private static void sendWebReport()
        {
            if (_busy) return;
            _busy = true;
            try
            {
                const int MAX_DAYS = 200;
                RabnetConfig.rabDataSource[] dataSources = new RabnetConfig.rabDataSource[RabnetConfig.DataSources.Count];
                RabnetConfig.DataSources.CopyTo(dataSources);
                sWebRepOneDay[] reps = null;
                foreach (RabnetConfig.rabDataSource rds in dataSources)
                {
                    if (!rds.WebReport) continue;
                    string webRepLD = RabServWorker.ReqSender.ExecuteMethod(MethodName.WebRep_GetLastDate,
                        MPN.db, rds.Params.DataBase).Value as string;

                    rabnet.Engine.get().initEngine(rds.Params.ToString());


                    DateTime stDate = DateTime.MaxValue;
                    if (webRepLD == "")
                    {
                        stDate = rabnet.Engine.db().GetFarmStartTime();
                        if (stDate == DateTime.MaxValue) continue;
                        if (DateTime.Now.Date.Subtract(stDate.Date).Days > MAX_DAYS)
                            stDate = DateTime.Now.AddDays(-MAX_DAYS);
                    }
                    else
                    {
                        DateTime.TryParse(webRepLD, out stDate);
                        if (stDate == DateTime.MaxValue)
                        {
                            _logger.ErrorFormat("LAST DATE ERROR: {0:s}", webRepLD);
                            continue;
                        }                       
                    }
                    if (stDate.Date == DateTime.Now.AddDays(-1))
                    {
                        _logger.Info("На сервере самый свежий отчет по БД: "+rds.Name);
                        continue;
                    }
                    ///далее Прибавляем все нужные отчеты
                    reps = appendWR_Global(rds.Params.DataBase, stDate.AddDays(1), DateTime.Now.AddDays(-1));
                    if (reps != null && reps.Length > 0)
                    {
                        RabServWorker.ReqSender.ExecuteMethod(MethodName.WebRep_SendGlobal,
                            MPN.db, rds.Params.DataBase,
                            MPN.value, reps);
                        callOnMessage("Статистика отослана", " ", 1);
                    }
                    else
                        callOnMessage("На сервере самая свежая информация", "Отправка статистики отменена", 1);
                }
            }
            catch (Exception exc)
            {
                _logger.Error(exc);
                callOnMessage(exc.Message, "Ошибка", 3);
            }
            finally
            {
                _busy = false;
            }
        }

        /// <summary>
        /// Составляет отчет за каждый день в диапазоне дат от startDate до endDate
        /// </summary>
        private static sWebRepOneDay[] appendWR_Global(string DBname, DateTime startDate, DateTime endDate)
        {
            if (startDate.Date == endDate.Date) return null;
            List<sWebRepOneDay> reps = new List<sWebRepOneDay>();
            while (startDate.Date <= endDate.Date)
            {
                reps.Add(new sWebRepOneDay(rabnet.Engine.db().WebReportGlobal(startDate)));
                startDate = startDate.AddDays(1);
            }
            return reps.ToArray();
        }

        private static void makeDump()
        {
            try
            {
                if (!System.IO.File.Exists(Options.Get().Path7Z))
                    throw new Exception("7z not specified");
                while (_ajt.JobIsBusy)
                    Thread.Sleep(2000);
                _logger.Debug("making Server Dump");
                callOnMessage("Начало отсылки" + Environment.NewLine + _ajt.JobName, "Отправка", 1, false);
                //RequestSender reqSend = MainForm.newReqSender();
                //sDump[] dumpList = reqSend.ExecuteMethod(MethodName.GetDumpList).Value as sDump[];
                //if (dumpList.Length > 0) //todo если что можно сделать diff
                //{
                    /*_logger.Debug("we have a DumpList"); 
                    bool coincidence = false;
                    foreach (sDump nd in dumpList)
                    {
                        if (_ajt.FileExists(nd.FileName, nd.MD5))
                        {
                            string srcFile = nd.FileName;
                            string trgFile = _ajt.GetLatestDump();
                            if (srcFile != Path.GetFileName(trgFile))
                            {
                                srcFile = Path.GetDirectoryName(trgFile) + "\\" + srcFile;
                                srcFile = ArchiveJobThread.ExtractDump(srcFile);
                                trgFile = ArchiveJobThread.ExtractDump(trgFile);
                                string diffPath = makeDiff(srcFile, trgFile);
                                diffPath = ArchiveJobThread.ZipFile(diffPath, true);
                                File.Delete(srcFile);
                                File.Delete(trgFile);
                                uploadDump(diffPath);
                                File.Delete(diffPath);
                                break;
                            }
                            else
                                callOnMessage("На сервере сама последняя версия БД", "загрузка отменена", 0);
                        }
                    }*/
                //}
                //else
                //{
                    string dumpPath = _ajt.GetLatestDump();
                    if (dumpPath != "")
                    {
                        string dump = ArchiveJobThread.ExtractDump(dumpPath);
                        string md5Dump = Helper.GetMD5FromFile(dump);
                        File.Delete(dump);
                        uploadDump(dumpPath, md5Dump);
                        callOnMessage("Файл отправлен успешно", "Успех", 1);
                    }
                //}               
            }
            catch (Exception)
            {
                callOnMessage("Ошибка при отправлении файла", "Внимание", 2);
            }

        }

        /// <summary>
        /// Файл должен делать патч
        /// </summary>
        /// <param name="srcFile">Старый файл</param>
        /// <param name="trgFile">Новый Файл</param>
        /// <returns>Путь к файлу с Дифом</returns>
        private static string makeDiff(string srcFile, string trgFile)
        {
            //TODO реализовать diff
            /*FileStream srcFS = new FileStream(srcFile, FileMode.Open, FileAccess.Read);
            StreamReader srcSR = new StreamReader(srcFile);
            FileStream trgFS = new FileStream(trgFile, FileMode.Open, FileAccess.Read);
            StreamReader trgSR = new StreamReader(trgFile);
            srcSR.Close(); srcFS.Close();
            trgSR.Close(); trgFS.Close();*/
            return trgFile;
        }

#region curl_usage

        /// <summary>
        /// Скачивает РКБД с сервера
        /// </summary>
        /// <param name="filename">Название файла, который надо получить с сервера</param>
        /// <param name="downloadTo">По какому пути поместить файл после загрузки</param>
        /// <param name="offset">Если не равен -1, то будет скачан файл начиная с щааыуе байта и прибавлен к существующему, 
        /// который находится по пути downloadTo</param>
        private static void downloadDump(string filename,string downloadTo, long offset)
        {
            string address = Path.Combine(ReqSender.Url, "getdump.php");
            byte[] responce;
            
            WebClient wc = new WebClient();

            wc.Encoding = Encoding.UTF8;
            NameValueCollection values = new NameValueCollection();
            values.Add("clientId", GRD.Instance.GetClientID().ToString());
            values.Add("file", filename);
            if (offset != -1)
                values.Add("offset", offset.ToString());
            responce = wc.UploadValues(address, values);
            if (responce.Length == 0)
                throw new Exception("Не удалось получить файл от удаленного сервера");
            FileStream fs = new FileStream(downloadTo, FileMode.OpenOrCreate, FileAccess.Write);
            if (offset == -1)
                offset = 0;  
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Write(responce, 0, responce.Length);
            fs.Close();
            string md5 = Helper.GetMD5FromFile(downloadTo);
            if (wc.ResponseHeaders["Content-MD5"] != "" && md5 != wc.ResponseHeaders["Content-MD5"])
            {
                File.Delete(downloadTo);
                throw new Exception("Контрольная сумма скачанного файла не совпадает");
            }
        }

        private static void uploadDump(string dumpPath, string md5dump)
        {
            string address = Path.Combine(ReqSender.Url, "uploader.php");
            
            /*using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                NameValueCollection values = new NameValueCollection();
                values.Add("clientId", GRD.Instance.GetClientID().ToString());
                values.Add("md5dump", md5dump); 
                wc.QueryString = values;
                byte[] buff = wc.UploadFile(address, dumpPath);
                address = Encoding.UTF8.GetString(buff);
            }*/
            
            using (FileStream stream = File.Open(dumpPath, FileMode.Open))
            {
                UploadFile fl = new UploadFile();                   
                fl.Name = "file";
                fl.Filename = Path.GetFileName(dumpPath);
                fl.ContentType = "text/plain";
                fl.Stream = stream;

                UploadFile[] files = new UploadFile[] { fl };

                NameValueCollection values = new NameValueCollection();
                values.Add( "clientId", GRD.Instance.GetClientID().ToString());
                values.Add( "md5dump", md5dump );             

                byte[] result = uploadFiles(address, files, values);
            }
        }

#endregion curl_usage
        
        /// <summary>
        /// Запускает событие
        /// </summary>
        /// <param name="msg">Сообщение</param>
        /// <param name="ttl">Заголовок</param>
        /// <param name="type">Тип(none,info,warning,error)</param>
        /// <param name="hide">Спрятать Значек в трее</param>
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
        private static void callOnMessage(string msg)
        {
            callOnMessage(msg, "", 0, false);
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
        private static long unDownloaded(string filePath)
        {            
            if(File.Exists(filePath))
            {
                FileInfo fi = new FileInfo(filePath);
                return fi.Length;
            }
            else return -1;
        }

        private static byte[] uploadFiles(string address, IEnumerable<UploadFile> files, NameValueCollection values)
        {
            WebRequest request = WebRequest.Create(address);
            request.Method = "POST";
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (Stream requestStream = request.GetRequestStream())
            {
                // Write the values
                foreach (string name in values.Keys)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                // Write the files
                foreach (UploadFile file in files)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    CopyStream(file.Stream,requestStream);
                    buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                byte[] boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }

            using (WebResponse response = request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (MemoryStream stream = new MemoryStream())
            {
                CopyStream(responseStream,stream);
                return stream.ToArray();
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }
    }
}
