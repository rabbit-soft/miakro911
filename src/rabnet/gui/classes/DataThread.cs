using System;
using System.Threading;
using System.Windows.Forms;
using rabnet.components;

namespace rabnet
{
    class DataThread
    {
        delegate void initCallBack();
        delegate void progressCallBack(int i);

        /// <summary>
        /// Объект для Критических секций. Ибо в интернетах говорят что lock(this) использовать не хорошо.
        /// Предпосылкой данного использования является зависание у Землеведа по не понятным причинам.
        /// </summary>
        private object _locker = new object();
        private static DataThread _instance=null;
        private int _status = 0;
        private RabStatusBar _rabStatusBar = null;
        private IDataGetter _dataGetter = null;
        private Thread _t = null;
        private EventHandler _onItem = null;
        private EventHandler _stopEvent = null;
        private bool _isStop = false;

        private DataThread()
        {
            _stopEvent = new EventHandler(this.stopClick);
        }

        public static DataThread Get()
        {
            if (_instance == null)
                _instance = new DataThread();
            return _instance;
        }

        public static DataThread Get4run()
        {
            DataThread th = DataThread.Get();
            th.Stop();
            return th;
        }

        public static IRabNetDataLayer Db()
        {
            return Engine.db2();
        }       
       
        public void SetInit()
        {
            if (_rabStatusBar.InvokeRequired)
            {
                initCallBack d = new initCallBack(SetInit);
                _rabStatusBar.Invoke(d);
            }
            else
            {
                _rabStatusBar.InitProgress(_dataGetter.getCount());
                _rabStatusBar.StopClick += _stopEvent;
            }
        }

        private void setProgress(int progress)
        {
            if (_rabStatusBar.InvokeRequired)
            {
                progressCallBack d = new progressCallBack(setProgress);
                _rabStatusBar.Invoke(d, new object[] { progress });
            }
            else
            {
                _rabStatusBar.Progress(progress);
                IData it = _dataGetter.GetNextItem();
                _onItem(it, null);
                if (it == null) setStop(true);
            }
        }

        private void setRelease()
        {
            if (_rabStatusBar.InvokeRequired)
            {
                initCallBack d = new initCallBack(setRelease);
                _rabStatusBar.Invoke(d);
            }
            else
            {
                _dataGetter.stop();
                if (getStop())
                    _rabStatusBar.EmergencyStop();
                else
                    _rabStatusBar.EndProgress();
                _rabStatusBar.StopClick -= _stopEvent;
                _onItem(null, null);
            }
        }

        public void Run(IDataGetter getter,RabStatusBar sb,EventHandler onItem)
        {
            while (getStatus() != 0) 
            { 
                Stop(); 
                Thread.Sleep(100); 
            }
            _dataGetter = getter;
            if (_dataGetter==null) return;

            _rabStatusBar = sb;
            _onItem = onItem;
            _isStop = false;
            _t = new Thread(new ThreadStart(threadProc));
            _t.Start();
        }

        public bool IsWorking()
        {
            return (getStatus() != 0);
        }

        public void Stop()
        {
            if (getStatus() == 0) return;

            setStop(true);
            while (getStatus() != 0)
                Application.DoEvents();
        }

        private void threadProc()
        {
            setStatus(1);
            int count = getCount();
            SetInit();
            for (int i = 0; (i < count) && (!getStop()); i++)
            {
                setProgress(i);
            }
            setRelease();
            setStatus(0);
        }

        private void stopClick(object sender, EventArgs e)
        {
            Stop();
        }
       

        private bool getStop()
        {
            lock (_locker)
            {
                return _isStop;
            }
        }

        private void setStop(bool val) 
        {
            lock (_locker)
            {
                _isStop = val;
            }
        }
        private int getStatus()
        {
            lock (_locker)
            {
                return _status;
            }
        }

        private void setStatus(int stat)
        {
            lock (_locker)
            {
                _status = stat;
            }
        }

        private int getCount()
        {
            lock (_locker)
            {
                return _dataGetter.getCount();
            }
        }        
    }
}
