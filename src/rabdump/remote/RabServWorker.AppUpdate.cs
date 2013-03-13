using System;
using System.Collections.Generic;
using System.Text;
using pEngine;
using System.Reflection;
using gamlib;

namespace rabdump
{

    public delegate void UpdateCheckedHandler(UpdateInfo up);
    public delegate void UpdateFinishedHandler();

    partial class RabServWorker
    {
        public static event UpdateCheckedHandler OnUpdateChecked;
        public static event UpdateFinishedHandler OnUpdateFinished;

        private static pAppUpdater _app = null;

        internal static void CheckForUpdate(bool download)
        {
            _dlUpdate = download;
            if (_app == null)
            {
                _app = new pAppUpdater(ReqSender);
                _app.FilesChecked += new FilesCheckedHandler(app_FilesChecked);
                _app.OnError += new UpdateFailHandler(onUpdateFail);
                _app.UpdateFinish += new UpdateFinishHandler(app_UpdateFinish);
            }
            if (download)
                _app.Download();
            else
                _app.Check();
        }

        static void app_UpdateFinish()
        {
            if (OnUpdateFinished != null)
                OnUpdateFinished();
        }

        static bool app_FilesChecked(List<UpdateFile> ufiles)
        {
            string curVer = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            UpdateInfo up = new UpdateInfo(curVer, true);
            up.UpdateRequired = false;
            try
            {              
                if (ufiles.Count == 0) throw new Exception("На сервере нет новых файлов для обновления");
                foreach (sUpdateFile uf in ufiles)
                    if (uf.Name == "rabnet.exe" || uf.Name== "rabdump.exe")
                    {
                        up = new UpdateInfo(uf.Version, true);
                        up.UpdateRequired = Helper.VersionCompare(uf.Version, curVer) == 1;
                        break;
                    }
                if (OnUpdateChecked != null)
                    OnUpdateChecked(up);
            }
            catch (Exception exc)
            {
                onUpdateFail(exc);           
            }          
            return _dlUpdate;
        }

        private static void onUpdateFail(Exception exc)
        {
            if (OnUpdateCheckFail != null)
                OnUpdateCheckFail(exc);
        }
    }
}
