using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Globalization;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace gamlib
{
    public static partial class Helper
    {
        public static byte[] UploadFiles(string address, IEnumerable<UploadFile> files, NameValueCollection values)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address); 
            request.Timeout = 5000; //TODO сообщить если не удалось подключиться
            request.Method = "POST";
            request.UserAgent = "Gamlib-Helper";

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (Stream requestStream = request.GetRequestStream())
            {

                ///Write the values
                foreach (string name in values.Keys)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                /// Write the files
                foreach (UploadFile file in files)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    Helper.CopyStream(file.Stream, requestStream);
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
                Helper.CopyStream(responseStream, stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Записывает POST переменные в поток.
        /// </summary>
        /// <param name="requestStream">Поток запроса к удаленному серверу</param>
        /// <param name="values">Список переменная=значение</param>
        /// <param name="boundary">Строка-Разграничитель</param>
        /// <param name="closeBoundary">Закрывать ли Разграничитель</param>
        public static void WritePostValues(Stream requestStream, NameValueCollection values, string boundary, bool closeBoundary)
        {
            foreach (string name in values.Keys)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                requestStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                requestStream.Write(buffer, 0, buffer.Length);
            }
            if (closeBoundary)
            {
                byte[] boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--"+Environment.NewLine);
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }
        }

        public static long CopyStream(Stream source, Stream target)
        {
            try
            {
                const int bufSize = 0x1000;
                byte[] buf = new byte[bufSize];

                long totalBytes = 0;
                int bytesRead = 0;

                while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                {
                    target.Write(buf, 0, bytesRead);
                    totalBytes += bytesRead;
                }
                return totalBytes;
            }
            catch (IOException exc)
            {
                if (exc.InnerException != null)
                    throw exc.InnerException;
                else
                    throw exc;
            }
        }

        public static bool CheckUdpPortBinded(int port)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] points = ipGlobalProperties.GetActiveUdpListeners();
            foreach (IPEndPoint ep in points)
            {
                if (ep.Port == port)
                    return true;
            }
            return false;
        }

        public static string UriCombine(string host, string file)
        {
            Uri u = new Uri(host);
            u = new Uri(u, file);
            return u.AbsoluteUri;
        }

        public static string UriNormalize(string host)
        {            
            if(!host.StartsWith("http://"))
                host = "http://" + host;      
            if(!host.EndsWith("/"))
                host += "/";      
            return host;
        }
    }

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


}