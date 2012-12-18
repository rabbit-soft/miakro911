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

namespace gamlib
{
    public class AppUpdaterException : Exception
    {
        public AppUpdaterException(string message) : base(message) { }
    }


    public delegate void UpdateFinishHandler();
    public delegate void UpdateFailHandler(Exception exc);
    /// <returns>Продолжать ли обновление</returns>
    public delegate bool FilesCheckedHandler(UpdateFile[] ufiles);

    /// <summary>
    /// Занимается обновлением программы
    /// </summary>
    public class AppUpdater:IDisposable
    {        
        protected const string DEL = ".del";
        protected const string NEW = ".new";

        protected const string GET_FILE_URI = "getfile.php";

        protected ILog _log = LogManager.GetLogger("AppUpdater");

        public event UpdateFinishHandler UpdateFinish;
        public event FilesCheckedHandler FilesChecked;
        public event UpdateFailHandler OnError;

        protected string _updatePath = "";

        /// <summary>
        /// Проверяет наличие обновлений и скачивает их.
        /// </summary>       
        public virtual void CheckUpdate()
        {
            _log.Debug("UPDATE DIR: " + _updatePath);
            ParameterizedThreadStart d = new ParameterizedThreadStart(checkForUpdate);
            Thread t = new Thread(d);
            t.Start(null);
        }

        /// <summary>
        /// Проверяет наличие обновлений и скачивает их.
        /// </summary>       
        public void Download()
        {
            _log.Debug("UPDATE DIR: " + _updatePath);
            ParameterizedThreadStart d = new ParameterizedThreadStart(checkForUpdate);
            Thread t = new Thread(d);
            t.Start(new object());
        }

        /// <summary>
        /// Проверяет наличие обновлений
        /// </summary>
        protected void checkForUpdate(object checkObj)
        {
            bool check = checkObj == null;
            try
            {
                //RequestSender rs = _reqSender;
                //List<sUpdateFile> ufiles = new List<sUpdateFile>( (rs.ExecuteMethod(MethodName.GetUpdateFiles).Value as sUpdateFile[]) );
                List<UpdateFile> ufiles = getUpdateFiles();
                if (ufiles == null)
                    throw new AppUpdaterException("update files list is null");
                FileVersionInfo curFI = null;
                int fail = 0;
                for (int i = 0; i < ufiles.Count; )
                {
                    ufiles[i].LocalFilePath = Path.Combine(_updatePath, ufiles[i].PathName.Replace('/', '\\'));

                    if (File.Exists(ufiles[i].LocalFilePath))
                    {
                        curFI = FileVersionInfo.GetVersionInfo(ufiles[i].LocalFilePath);
                        ufiles[i].LocalFileMD5 = Helper.GetMD5FromFile(ufiles[i].LocalFilePath);
                        int versComp = Helper.VersionCompare(curFI.FileVersion, ufiles[i].Version);
                        if (versComp > 0 || (versComp == 0 && ufiles[i].LocalFileMD5 == ufiles[i].MD5)) //если версия текущего файла больше или равна версии файла на сервере
                        {
                            _log.DebugFormat("update no required for file {0:s} SrvVer: {1:s} CurVer: {2:s}", ufiles[i].RelativePath + ufiles[i].Name, ufiles[i].Version, curFI.FileVersion);
                            ufiles.RemoveAt(i);
                            continue;
                        }
                        else
                            _log.DebugFormat("need to update file {0:s}  versions: {1:s}=>{2:s} md5: {3:s}=>{4:s}",
                                ufiles[i].Name, curFI.FileVersion, ufiles[i].Version, ufiles[i].LocalFileMD5, ufiles[i].MD5);
                    }
                    i++;
                }
                if (check)
                {
                    onFilesChecked(ufiles);
                    return;
                }
                //начинаем обновление файлов
                for (int i = 0; i < ufiles.Count; )
                {
                    //скачиваем файл
                    //string address = Path.Combine(getRemoteUrl(), getFileUri);
                    byte[] responseArray = downloadFile(ufiles[i].PathName);                   
                    //разпаковываем файл
                    byte[] decBuff = decompress(responseArray);
                    if (!Directory.Exists(Path.GetDirectoryName(ufiles[i].LocalFilePath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(ufiles[i].LocalFilePath));
                    using (FileStream fileSave = new FileStream(ufiles[i].LocalFilePath + NEW, FileMode.Create, FileAccess.Write))
                    {
                        fileSave.Write(decBuff, 0, decBuff.Length);
                        _log.DebugFormat("saving new file to [{0:s}] version: {1:s}", ufiles[i].LocalFilePath, ufiles[i].Version);
                    }
                    //проверяем скачанный файл на правильность
                    FileVersionInfo newFI = FileVersionInfo.GetVersionInfo(ufiles[i].LocalFilePath + NEW);
                    string newMD5 = Helper.GetMD5FromFile(ufiles[i].LocalFilePath + NEW);
                    if ((newFI.FileVersion == null ? "" : newFI.FileVersion) != ufiles[i].Version || newMD5 != ufiles[i].MD5)
                    {
                        _log.DebugFormat("downloaded updateFile '{0:s}' is corrupt, try again to download ", ufiles[i].PathName);
                        File.Delete(ufiles[i].LocalFilePath + NEW);
                        fail++;
                        if (fail == 5)
                        {
                            _log.ErrorFormat("File " + ufiles[i].PathName + " is corrupt");
                            break;
                        }
                        continue;
                    }
                    _log.DebugFormat("rename file {0:s} to {1:s}  version: {2:s}", ufiles[i].LocalFilePath, ufiles[i].PathName + DEL, curFI != null ? curFI.FileVersion : "null");
                    if (File.Exists(ufiles[i].LocalFilePath))
                    {
                        if (File.Exists(ufiles[i].LocalFilePath + DEL))
                            File.Delete(ufiles[i].LocalFilePath + DEL);
                        File.Move(ufiles[i].LocalFilePath, ufiles[i].LocalFilePath + DEL);
                    }
                    File.Move(ufiles[i].LocalFilePath + NEW, ufiles[i].LocalFilePath);
                    i++;
                }
                onUpdateFinish();
            }
            catch (Exception exc)
            {
                _log.Error(exc);
                if (OnError != null)
                    OnError(exc);
            }
        }

        /// <summary>
        /// Получить адрес удаленного сервера
        /// </summary>
        /// <returns></returns>
        protected virtual string getRemoteUrl()
        {
            return "";
        }

        /// <summary>
        /// Получить список файлов для обновления
        /// </summary>
        /// <returns></returns>
        protected virtual List<UpdateFile> getUpdateFiles()
        {
            return null;
        }

        /// <summary>
        /// Рапаковывает файлы
        /// </summary>
        /// <param name="responseArray">Запакованный массив байт</param>
        /// <returns>Распакованный массив байт</returns>
        protected virtual byte[] decompress(byte[] responseArray)
        {
            return responseArray;
        }

        protected virtual byte[] downloadFile(string pathname)
        {
            string address = getRemoteUrl();
            Uri u = new Uri(address);
            u = new Uri(u, GET_FILE_URI);

            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            NameValueCollection myNameValueCollection = new NameValueCollection();
            myNameValueCollection.Add("getfile", pathname);
            return wc.UploadValues(u.AbsoluteUri, myNameValueCollection);
        }     

        public static void DeleteOldFiles(string path)
        {
            DirectoryInfo rootDir = new DirectoryInfo(path);
            foreach (FileInfo fi in rootDir.GetFiles("*"+DEL))
                File.Delete(fi.FullName);
            foreach (DirectoryInfo di in rootDir.GetDirectories())
                DeleteOldFiles(di.FullName);
        }
        public void DeleteOldFiles()
        {
            DeleteOldFiles(_updatePath);
        }

        protected void onUpdateFinish()
        {
            if (UpdateFinish != null)
                UpdateFinish();
        }

        /// <summary>
        /// Вызывает Событие, о том что получены файлы для обновления
        /// </summary>
        /// <param name="files">Список оригинальных файлов</param>
        protected void onFilesChecked(List<UpdateFile> files)
        {
            if (FilesChecked != null)
                FilesChecked(files.ToArray());
        }       

        public virtual void Dispose()
        {
            
        }

    }

    public class UpdateFile
    {
        public string Name;
        /// <summary>
        /// Каталог в папке обновления, в котором находится файл.
        /// <remarks>
        /// Не должен начинаться прямой косой черты(/).
        /// Должен заканчиваться на прямую косую черту(/).</remarks>
        /// </summary>
        public string RelativePath;
        public string MD5;
        public string Version;

        protected string _localFilePath;
        protected string _localFileMD5;

        public UpdateFile() { }
        public UpdateFile(string name,string path,string version,string md5)
        {
            this.Name = name;
            this.RelativePath = path;
            this.Version = version;
            this.MD5 = md5;
        }

        public virtual string PathName
        {
            get { return Path.Combine(RelativePath , Name); }
        }

        public virtual string LocalFilePath
        {
            get { return _localFilePath; }
            set { _localFilePath = value; }
        }
        
        public virtual string LocalFileMD5
        {
            get { return _localFileMD5; }
            set { _localFileMD5 = value; }
        }
    }
}
