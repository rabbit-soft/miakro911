using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using log4net;
using SeasideResearch.LibCurlNet;
#if PROTECTED
using RabGRD;
#endif

namespace rabdump
{
    static class RabServWorker
    {
        struct ServData
        {
            internal string farmname;
            internal string dumpPath;
            internal string dumpMD5;
            internal bool uploaded;
        }

        private static string NL = Environment.NewLine;
        private static string URL = "localhost";
        private static string lastDumpURI = URL + "/" + "dumplist.php";
        private static string loadDumpURI = URL + "/" + "uploader.php";
        private static ILog _logger = LogManager.GetLogger(typeof(RabServWorker));
        private static ServData _data;
        private static ArchiveJobThread _ajt;

        public static event MessageSenderCallbackDelegate OnMessage;

        public static void MakeDump(ArchiveJob j)
        {
            _data = new ServData();
#if PROTECTED
            _data.farmname = GRD.Instance.GetOrgName();           
#elif DEBUG
            _data.farmname = "testing";
#endif                
            _ajt = new ArchiveJobThread(j);           
            Thread t = new Thread(makeDump);
            t.Start();
        }

        public static void Undump(ArchiveJob j)
        {
            
        }

        private static void lastDump()
        {
#if !NOCATCH
            try
            {
#endif
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                MultiPartForm mf = new MultiPartForm();
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "farm",
                    CURLformoption.CURLFORM_COPYCONTENTS, defendString(_data.farmname),
                    CURLformoption.CURLFORM_END);

                Easy easy = new Easy();

                Easy.WriteFunction wf = new Easy.WriteFunction(OnWriteData);
                easy.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);
                //easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);

                //Easy.ProgressFunction pf = new Easy.ProgressFunction(OnProgress);

                //easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);

                easy.SetOpt(CURLoption.CURLOPT_URL, lastDumpURI);
                easy.SetOpt(CURLoption.CURLOPT_HTTPPOST, mf);
                easy.Perform();
                easy.Cleanup();
                mf.Free();

                Curl.GlobalCleanup();
#if !NOCATCH
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
#endif
        }

        private static void makeDump()      
        {
            while(_ajt.JobIsBusy)           
                Thread.Sleep(1000);
            OnMessage("Начало отсылки" + NL + _ajt.JobName, "Отправка", 1, false);
            _data.dumpPath = _ajt.GetLatestDump(out _data.dumpMD5);
            sendFile();
        }

        private static void sendFile()
        {
#if !NOCATCH
            try
            {
#endif
                _logger.Info("sending dump on server");
                Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);

                // <form action="http://mybox/cgi-bin/myscript.cgi
                //  method="post" enctype="multipart/form-data">
                MultiPartForm mf = new MultiPartForm();

                // <input name="frmUsername">
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "farm",
                    CURLformoption.CURLFORM_COPYCONTENTS, _data.farmname,
                    CURLformoption.CURLFORM_END);

                // <input name="frmPassword">
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "type",
                    CURLformoption.CURLFORM_COPYCONTENTS, "dump",
                    CURLformoption.CURLFORM_END);

                // <input type="File" name="f1">
                mf.AddSection(CURLformoption.CURLFORM_COPYNAME, "uploadedfile",
                    CURLformoption.CURLFORM_FILE, _data.dumpPath,
                    CURLformoption.CURLFORM_CONTENTTYPE, "application/binary",
                    CURLformoption.CURLFORM_END);

                Easy easy = new Easy();

                Easy.DebugFunction df = new Easy.DebugFunction(onDebug);
                easy.SetOpt(CURLoption.CURLOPT_DEBUGFUNCTION, df);
                easy.SetOpt(CURLoption.CURLOPT_VERBOSE, true);

                Easy.ProgressFunction pf = new Easy.ProgressFunction(onProgress);
                easy.SetOpt(CURLoption.CURLOPT_PROGRESSFUNCTION, pf);

                easy.SetOpt(CURLoption.CURLOPT_URL, loadDumpURI);
                easy.SetOpt(CURLoption.CURLOPT_HTTPPOST, mf);

                easy.Perform();
                easy.Cleanup();
                mf.Free();

                Curl.GlobalCleanup();

#if !NOCATCH
            }
            catch (Exception ex)
            {
                _logger.Error("sendfile",ex);
            }
#endif
        }

        public static Int32 OnWriteData(Byte[] buf, Int32 size, Int32 nmemb, Object extraData)
        {
            _logger.DebugFormat("Curl WriteData: {0:s}", System.Text.Encoding.UTF8.GetString(buf));
            return size * nmemb;
        }

        private static Int32 onProgress(Object extraData, Double dlTotal, Double dlNow, Double ulTotal, Double ulNow)
        {
            _logger.DebugFormat("Curl Progress: {0} {1} {2} {3}", dlTotal, dlNow, ulTotal, ulNow);
            _data.uploaded =  ulTotal == ulNow;
            return 0;
        }

        private static void onDebug(CURLINFOTYPE infoType, String msg, Object extraData)
        {
            // print out received data only
            if (infoType == CURLINFOTYPE.CURLINFO_DATA_IN)
                _logger.DebugFormat("Curl Debug: {0}", msg);
            if(infoType ==CURLINFOTYPE.CURLINFO_DATA_IN || infoType == CURLINFOTYPE.CURLINFO_TEXT)
            {
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
            string title = "";
            int type=1;
            bool send = false;
            switch (msg.Replace("п»ї",""))
            {
                case "couldn't connect to host\n":
                    msg = "Неудалось подключиться к серверу";
                    title = "Ошибка";
                    type = 3;
                    send = true;
                    break;
                case "[File Upload Successful]":
                    msg = "Файл отправлен успешно";
                    title = "Успех";
                    type = 1;
                    send = true;
                    break;
                case "[File Upload Error]":
                    msg = "Ошибка при отправлении файла";
                    title = "Внимание";
                    type = 2;
                    send = true;
                    break;
            }
            if(send)
                OnMessage(msg, title, type, false);
            
        }

    }
}
