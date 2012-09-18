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
        private static DataThread thread=null;
        public static DataThread get()
        {
            if (thread == null)
                thread = new DataThread();
            return thread;
        }
        public static DataThread get4run()
        {
            DataThread th = DataThread.get();
            th.stop();
            return th;
        }
        public static IRabNetDataLayer db()
        {
            return Engine.db2();
        }
        private int status=0;
        private RabStatusBar sb = null;
        private IDataGetter gt = null;
        private Thread t = null;
        private EventHandler onitem = null;
        private EventHandler stopev = null;
        private bool is_stop = false;
        delegate void initCallBack();
        delegate void progressCallBack(int i);

        public DataThread()
        {
            stopev=new EventHandler(this.StopClick);
        }

        private void threadProc()
        {
            setStatus(1);
            int count=getCount();
            setInit();
            for (int i = 0; (i < count) && (!getStop()); i++)
            {
                setProgress(i);
            }
            setRelease();
            setStatus(0);
        }

        public void setInit()
        {
            if (sb.InvokeRequired)
            {
                initCallBack d = new initCallBack(setInit);
                sb.Invoke(d);
            }
            else
            {
                sb.initProgress(gt.getCount());
                sb.stopClick += stopev;
            }
        }

        public void setProgress(int progress)
        {
            if (sb.InvokeRequired)
            {
                progressCallBack d = new progressCallBack(setProgress);
                sb.Invoke(d, new object[] { progress });
            }
            else
            {
                sb.progress(progress);
                IData it = gt.getNextItem();
                onitem(it, null);
                if (it == null) setStop(true);
            }
        }

        public void setRelease()
        {
            if (sb.InvokeRequired)
            {
                initCallBack d = new initCallBack(setRelease);
                sb.Invoke(d);
            }
            else
            {
                gt.stop();
                if (getStop())
                    sb.emergencyStop();
                else
                    sb.endProgress();
                sb.stopClick -= stopev;
                onitem(null, null);
            }
        }

        public void Run(IDataGetter getter,RabStatusBar sb,EventHandler onItem)
        {
            while (getStatus() != 0) 
            { 
                stop(); 
                Thread.Sleep(100); 
            }
            gt = getter;
            if (gt==null)
                return;
            this.sb = sb;
            this.onitem = onItem;
            is_stop = false;
            t = new Thread(new ThreadStart(threadProc));
            t.Start();
        }

        private void StopClick(object sender, EventArgs e)
        {
            stop();
        }

        public void stop()
        {
            if (getStatus() == 0)
                return;
            setStop(true);
            while (getStatus() != 0)
                Application.DoEvents();
        }

        private bool getStop()
        {
            lock (this)
            {
                return is_stop;
            }
        }

        private void setStop(bool val)
        {
            lock (this)
            {
                is_stop = val;
            }
        }
        private int getStatus()
        {
            lock (this)
            {
                return status;
            }
        }

        private void setStatus(int stat)
        {
            lock (this)
            {
                status = stat;
            }
        }

        private int getCount()
        {
            lock (this)
            {
                return gt.getCount();
            }
        }

        public bool IsWorking()
        {
            return (getStatus() != 0);
        }
    }
}
