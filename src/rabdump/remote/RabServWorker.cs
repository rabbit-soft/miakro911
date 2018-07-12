#if DEBUG
    #define NOCATCH
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
using rabnet.RNC;
using pEngine;
using gamlib;
#if PROTECTED
	using System.Reflection;  
#endif

namespace rabdump
{
    class RabReqSender : RequestSender
    {
        public RabReqSender()
        {
            _ServUriAppend = "";
            _RPCfile = "forrpc.php";
        }
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
    //public delegate void UpdateCheckedHandler(UpdateInfo info);
    public delegate void ErrorHandler(Exception exc);

    /// <summary>
    /// Класс отсылающий РК и Статистику на сервер
    /// </summary>
    partial class RabServWorker
    {
        private const string DEF_PWD ="user_with_old_key_10578vnr/* ekei";
        private static string _url = "http://trunk.rab_srv.wd2.9-bits.ru/";
        private static ILog _logger = LogManager.GetLogger(typeof(RabServWorker));
        //private static ArchiveJobThread _ajt;
        private static RabReqSender _reqSend = null;
        private static bool _busy = false;
        private static bool _dlUpdate = false;

        private static object _sendDumpLoacker = new object();

        public static event ErrorHandler OnUpdateCheckFail;
        public static event MessageSenderCallbackDelegate OnMessage;

        internal static RabReqSender ReqSender
        {
            get
            {
                if (_reqSend == null)
                {
                    _reqSend = new RabReqSender();
                    _reqSend.Url = _url;

                    _reqSend.Key = new byte[262];//GRD.KEY_CODE_LENGHT
                    byte[] defPass = Encoding.UTF8.GetBytes("user_with_old_key");
                    Array.Copy(defPass, _reqSend.Key, defPass.Length);
                }
                return _reqSend;
            }
        }
             
        /// <summary>
        /// Адрес удаленного сервера
        /// </summary>
        public static string Url
        {
            get { return _url; }
            set 
            {
                Uri uri;
                if (Uri.TryCreate(value, UriKind.Absolute, out uri))
                {
                    _url = uri.AbsoluteUri;
                    if (_reqSend != null) ///todo ГОВНОКОД
                        _reqSend.Url = uri.AbsolutePath;
                }
            }
        }

        public static void SendDump(ArchiveJob j)
        {
            lock (_sendDumpLoacker)
            {
                Thread t = new Thread(new ParameterizedThreadStart(sendDump));
                t.Name = "DumpSender";
                t.Start(j);
            }
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
                DataSource[] dataSources = Options.Inst.DataSource.ToArray();
                // new RabnetConfig.rabDataSource[RabnetConfig.DataSources.Count];
                //RabnetConfig.DataSources.CopyTo(dataSources);
                sWebRepOneDay[] reps = null;
                foreach (DataSource rds in dataSources)
                {
                    if (!rds.WebReport)
                    {
                        _logger.Debug("Data source no need to WebReport");
                        continue;
                    }

                    string webRepLD = RabServWorker.ReqSender.ExecuteMethod(MethodName.WebRep_GetLastDate,
                        MPN.db, rds.Params.DataBase).Value as string;

                    rabnet.Engine.get().initEngine(rds.Params.ToString());


                    DateTime stDate = DateTime.MaxValue;
                    if (String.IsNullOrEmpty(webRepLD))
                    {
                        stDate = rabnet.Engine.db().GetFarmStartTime();
                        if (stDate == DateTime.MaxValue)
                        {
                            _logger.Debug("No Farm start Time");
                            continue;
                        }
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
                if (exc.InnerException != null)
                    exc = exc.InnerException;
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

        private static void sendDump(object archJob)
        {
            if (!(archJob is ArchiveJob)) return;
            
            ArchiveJobThread ajt = new ArchiveJobThread(archJob as ArchiveJob);
            while (ajt.Job.Busy)///todo говнокод, нужно делать на ивентах
                Thread.Sleep(2000);
            try
            {
                if (!System.IO.File.Exists(Options.Inst.Path7Z))
                    throw new Exception("Путь к 7zip не корректен");

                string dumpPath = ajt.GetLatestDump();
                if (dumpPath == "")
                {
                    _logger.Info("There is no local dump to send.");
                    return;
                }

                DateTime lastReservDate = ArchiveJobThread.ParseDumpDate(Path.GetFileName(dumpPath));
                sDump[] dmps = RabServWorker.ReqSender.ExecuteMethod(MethodName.GetDumpList).Value as sDump[];
                if (dmps != null && dmps.Length > 0)
                {
                    DateTime lastSendDate = DateTime.Parse(dmps[dmps.Length - 1].Datetime);
                    if (lastReservDate.Subtract(lastSendDate).TotalDays < ajt.Job.SendDayDelay)
                    {
                        _logger.Info("Servdump canseled. Because DayDelay not reached.");
                        return;
                    }
                }

                _logger.Debug("ServerDump start");
                callOnMessage("Начало отсылки" + Environment.NewLine + ajt.Job.Name, "Отправка", 1, false);


                    string dump = ArchiveJobThread.ExtractDump(dumpPath);
                    string md5Dump = Helper.GetMD5FromFile(dump);
                    File.Delete(dump);
                    uploadDump(dumpPath, md5Dump);
                    callOnMessage("Файл отправлен успешно", "Успех", 1);
                }           
            catch (Exception exc)
            {
                _logger.Error(exc);
                if (exc.InnerException != null)
                    exc = exc.InnerException;
                callOnMessage(exc.Message, "Внимание", 2);                
            }
            _logger.Debug("ServerDump end");
        }

        /// <summary>
        /// Скачивает РКБД с сервера
        /// </summary>
        /// <param name="filename">Название файла, который надо получить с сервера</param>
        /// <param name="downloadTo">По какому пути поместить файл после загрузки</param>
        /// <param name="offset">Если не равен -1, то будет скачан файл начиная с щааыуе байта и прибавлен к существующему, 
        /// который находится по пути downloadTo</param>
        private static void downloadDump(string filename,string downloadTo, long offset)
        {
            string address = Helper.UriCombine(ReqSender.Url, "getdump.php");
            byte[] responce;
            
            WebClient wc = new WebClient();

            wc.Encoding = Encoding.UTF8;
            NameValueCollection values = new NameValueCollection();

            values.Add("clientId", "1");

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

        #region OnMessage
        /// <summary>
        /// Запускает событие
        /// </summary>
        /// <param name="msg">Сообщение</param>
        /// <param name="ttl">Заголовок</param>
        /// <param name="type">Тип(none,info,warning,error)</param>
        /// <param name="hide">Спрятать Значок в трее</param>
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
        #endregion OnMessage

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

        private static void uploadDump(string dumpPath, string md5dump)
        {
            string address = Helper.UriCombine(ReqSender.Url,"uploader.php");

            using (FileStream stream = File.Open(dumpPath, FileMode.Open))
            {
                UploadFile fl = new UploadFile();
                fl.Name = "file";
                fl.Filename = Path.GetFileName(dumpPath);
                fl.ContentType = "text/plain";
                fl.Stream = stream;

                UploadFile[] files = new UploadFile[] { fl };

                NameValueCollection values = new NameValueCollection();
                values.Add("clientId", "1");
                values.Add("md5dump", md5dump);

                byte[] result = Helper.UploadFiles(address, files, values);
            }
        }        
    }

    
}
