using System;
using System.Threading;
using System.Windows.Forms;
using rabnet.components;

namespace rabnet
{
    public delegate void DTProgressHandler(int progress);

    class DataThread
    {
        delegate void initCallBack();
        
        /// <summary>
        /// Объект для Критических секций. Ибо в интернетах говорят что lock(this) использовать не хорошо.
        /// Предпосылкой данного использования является зависание у Землеведа по не понятным причинам.
        /// </summary>
        private object _locker = new object();
        //private static DataThread _instance = null;       
        private IDataGetter _dataGetter = null;
        private Thread _thr = null;        
        private bool _stopRequired = false;
        public event RSBEventHandler OnFinish;
        public event RSBItemEventHandler OnItem;
        public event DTProgressHandler InitMaxProgress;
        public event DTProgressHandler Progress;
       
        /// <summary>
        /// Запускает отдельный поток, который получает данные.
        /// </summary>
        /// <param name="getter"></param>
        public void Run(IDataGetter getter/*, RabStatusBar sb, RSBItemEventHandler onItem*/)
        {
            if(_thr !=null && _thr.IsAlive)
            {
                Stop();
                Thread.Sleep(100);
            }
            _dataGetter = getter;
            if (_dataGetter == null) return;

            //_rabStatusBar = sb;
            //_onItem = onItem;
            _stopRequired = false;
            _thr = new Thread(threadProc);
            _thr.Start();
        }

        public void Stop()
        {
            _stopRequired=true;
            _thr.Abort();
            _thr = null;
            _dataGetter.Close();
        }

        private void threadProc()
        {
            if (_dataGetter == null) return;

            int count = _dataGetter.getCount();
            if(InitMaxProgress!=null)
                InitMaxProgress(_dataGetter.getCount());

            for (int i = 0; (i < count) && (!_stopRequired); i++)
            {
                if (Progress!=null)
                    Progress(i);
                IData it = _dataGetter.GetNextItem();
                if(OnItem!=null)
                    OnItem(it);
                if (it == null)
                    break;
            }
            _dataGetter.Close();

            if (OnFinish != null)
                OnFinish();
        }

        //private int getCount()
        //{
        //    lock (_locker)
        //    {
        //        return _dataGetter.getCount();
        //    }
        //}

        //public static DataThread Get()
        //{
        //    if (_instance == null)
        //        _instance = new DataThread();
        //    return _instance;
        //}

        //private int _status = 0;
        //private RabStatusBar _rabStatusBar = null;
        //private RSBItemEventHandler _onItem = null;
        //private RSBEventHandler _stopEvent = null;

        //public DataThread()
        //{
        //    _stopEvent = new RSBEventHandler(this.stopClick);
        //}

        //public static DataThread Get4run()
        //{
        //    DataThread th = DataThread.Get();
        //    th.Stop();
        //    return th;
        //}

        //public static IRabNetDataLayer Db()
        //{
        //    return Engine.db2();
        //}

        //public void SetInit()
        //{
        //    if (_rabStatusBar.InvokeRequired)
        //    {
        //        initCallBack d = new initCallBack(SetInit);
        //        _rabStatusBar.Invoke(d);
        //    }
        //    else
        //    {
        //        _rabStatusBar.InitProgress(_dataGetter.getCount());
        //        _rabStatusBar.StopClick += _stopEvent;
        //    }
        //}

        //private void setProgress(int progress)
        //{
        //    if (_rabStatusBar.InvokeRequired)
        //    {
        //        progressCallBack d = new progressCallBack(setProgress);
        //        _rabStatusBar.Invoke(d, new object[] { progress });
        //    }
        //    else
        //    {
        //        _rabStatusBar.Progress(progress);
        //        IData it = _dataGetter.GetNextItem();
        //        _onItem(it);
        //        if (it == null) setStop(true);
        //    }
        //}

        //private void setRelease()
        //{
        //if (_rabStatusBar.InvokeRequired)
        //{
        //    initCallBack d = new initCallBack(setRelease);
        //    _rabStatusBar.Invoke(d);
        //}
        //else
        //{
        //_dataGetter.Close();          
        //if (_stopRequired)
        //_rabStatusBar.EmergencyStop();
        //else
        //_rabStatusBar.EndProgress();
        //_rabStatusBar.StopClick -= _stopEvent;
        //_onItem(null);

        //}
        //}

        //public bool IsWorking()
        //{
        //    return (getStatus() != 0);
        //}

        //private void stopClick()
        //{
        //    Stop();
        //}


        //private bool getStop()
        //{
        //    lock (_locker)
        //    {
        //        return _isStop;
        //    }
        //}

        //private void setStop(bool val)
        //{
        //    lock (_locker)
        //    {
        //        _isStop = val;
        //    }
        //}

        //private int getStatus()
        //{
        //    lock (_locker)
        //    {
        //        return _status;
        //    }
        //}

        //private void setStatus(int stat)
        //{
        //    lock (_locker)
        //    {
        //        _status = stat;
        //    }
        //}
    }
}
