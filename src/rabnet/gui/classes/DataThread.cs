using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

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
        private RabStatusBar _sb = null;
        private IDataGetter _gt = null;
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
            if (_sb.InvokeRequired)
            {
                initCallBack d = new initCallBack(SetInit);
                _sb.Invoke(d);
            }
            else
            {
                _sb.initProgress(_gt.getCount());
                _sb.stopClick += _stopEvent;
            }
        }

        public void SetProgress(int progress)
        {
            if (_sb.InvokeRequired)
            {
                progressCallBack d = new progressCallBack(SetProgress);
                _sb.Invoke(d, new object[] { progress });
            }
            else
            {
                _sb.progress(progress);
                IData it = _gt.GetNextItem();
                _onItem(it, null);
                if (it == null) setStop(true);
            }
        }

        public void SetRelease()
        {
            if (_sb.InvokeRequired)
            {
                initCallBack d = new initCallBack(SetRelease);
                _sb.Invoke(d);
            }
            else
            {
                _gt.stop();
                if (getStop())
                    _sb.emergencyStop();
                else
                    _sb.endProgress();
                _sb.stopClick -= _stopEvent;
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
            _gt = getter;
            if (_gt==null)
                return;
            this._sb = sb;
            this._onItem = onItem;
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
            if (getStatus() == 0)
                return;
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
                SetProgress(i);
            }
            SetRelease();
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
                return _gt.getCount();
            }
        }        
    }
}
