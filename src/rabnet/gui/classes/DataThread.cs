using System;
using System.Threading;
using System.Windows.Forms;
using rabnet.components;

namespace rabnet
{
    public delegate void DTProgressHandler(int progress);

    public delegate void DTItemsHandler(IDataGetter dataGetter);

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
        public event DTItemsHandler OnItems;
        public event DTProgressHandler InitMaxProgress;
        //public event DTProgressHandler Progress;

        /// <summary>
        /// Запускает отдельный поток, который получает данные.
        /// </summary>
        /// <param name="getter"></param>
        public void Run(IDataGetter getter/*, RabStatusBar sb, RSBItemEventHandler onItem*/)
        {
            if (_thr != null && _thr.IsAlive) {
                Stop();
                Thread.Sleep(100);
            }
            _dataGetter = getter;
            if (_dataGetter == null) {
                onFinish();
                return;
            }

            //_rabStatusBar = sb;
            //_onItem = onItem;
            _stopRequired = false;
            _thr = new Thread(threadProc);
            _thr.Start();
        }

        public void Stop()
        {
            _stopRequired = true;
            if (_thr != null) {
                _thr.Abort();
                _thr = null;
            }
            if (_dataGetter != null) {
                _dataGetter.Close();
            }
        }

        private void threadProc()
        {
            if (_dataGetter == null) {
                return;
            }

            if (this.InitMaxProgress != null) {
                this.InitMaxProgress(_dataGetter.getCount());
            }

            if (this.OnItems != null) {
                this.OnItems(_dataGetter);
            }
            
            _dataGetter.Close();

            onFinish();
        }

        private void onFinish()
        {
            if (OnFinish != null) {
                OnFinish();
            }
        }
    }
}
