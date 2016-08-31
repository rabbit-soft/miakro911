using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using log4net;
using gamlib;
using CookComputing.XmlRpc;

namespace pEngine
{
    /// <summary>
    /// Занимается обновлением программы
    /// </summary>
    public class pAppUpdater:AppUpdater
    {        
        private RequestSender _reqSender = null;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="rs">Реквест сендер</param>
        /// <param name="upPath">Где искать обновляемые файлы</param>
        public pAppUpdater(RequestSender rs, string upPath)
        {
            _reqSender = rs;
            _updatePath = upPath;
        }
        public pAppUpdater(RequestSender rs) : this(rs, AppDomain.CurrentDomain.BaseDirectory) { }
        
        protected override string getRemoteUrl()
        {
            return _reqSender.Url;
        }

        protected override List<UpdateFile> getUpdateFiles()
        {
            return new List<UpdateFile>((_reqSender.ExecuteMethod(MethodName.GetUpdateFiles).Value as UpdateFile[]));
        }

        //protected override Stream decompress(Stream dlFileStream)
        //{
            //List<byte> decBuff = new List<byte>();
            //using (MemoryStream defMS = new MemoryStream(responseArray))
            //using (DeflateStream deflafe = new DeflateStream(defMS, System.IO.Compression.CompressionMode.Decompress, true))
            //{
            //    int bt = 0; //узнаем длинну ибо иначе никак
            //    while ((bt = deflafe.ReadByte()) != -1)
            //        decBuff.Add((byte)bt);
            //}
            //return decBuff.ToArray();
            //return new DeflateStream(dlFileStream, System.IO.Compression.CompressionMode.Decompress, true);
        //}

        //protected override byte[] downloadFile(string pathname)
        //{
        //    Uri gfUri = new Uri(getRemoteUrl());
        //    gfUri = new Uri(gfUri,GET_FILE_URI);

        //    WebClient wc = new WebClient();           
        //    wc.Encoding = Encoding.UTF8;
        //    NameValueCollection myNameValueCollection = new NameValueCollection();
        //    myNameValueCollection.Add("getfile", pathname);
        //    return wc.UploadValues(gfUri.AbsoluteUri, myNameValueCollection);        
        //}
    }

    public class sUpdateFile:UpdateFile
    {
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public override string PathName
        {
            get
            {
                return base.PathName;
            }
        }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public override string LocalFilePath
        {
            get
            {
                return base.LocalFilePath;
            }
            set
            {
                base.LocalFilePath = value;
            }
        }

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public override string LocalFileMD5
        {
            get
            {
                return base.LocalFileMD5;
            }
            set
            {
                base.LocalFileMD5 = value;
            }
        }
    }
}
