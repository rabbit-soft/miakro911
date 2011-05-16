//#define PROTECTED
using System;
using System.Windows.Forms;
using log4net;
using X_Classes;
using System.Diagnostics;
using System.IO;
#if PROTECTED
using RabGRD;
#endif
using rabnet;

namespace rabdump
{
    public partial class MainForm : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainForm));
        bool _canclose = false;
        bool _manual = true;

        readonly RabUpdater _rupd;

        readonly SocketServer _socksrv;

        long _updDelayCnt = 0;

        public MainForm()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            _rupd = new RabUpdater();
            _rupd.MessageSenderCallback = MessageCb;
            _rupd.CloseCallback = CloseCb;
            _socksrv = new SocketServer();
        }

        public static ILog log()
        {
            return logger;
        }
        private void restoreMenuItem_Click(object sender, EventArgs e)
        {
            _manual = false;
            Show();
//            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            TopMost = true;
            Focus();
            BringToFront();
            TopMost = false;            
            _manual = true;
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            _canclose = (MessageBox.Show(this,"Выйти из программы?","RabDump",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes);
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                _canclose = true;
            }
            e.Cancel = !_canclose;
            if (_canclose)
            {
                log().Debug("Program finished");
                _socksrv.Close();
            }
            _manual = false;
            WindowState = FormWindowState.Minimized;
            Hide();
            //ShowInTaskbar = false;
            _manual = true;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            restoreMenuItem.PerformClick();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = Icon;
            propertyGrid1.SelectedObject = Options.Get();
            log().Debug("Program started");
            Options.Get().Load();
            ReinitTimer(true);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (!_manual) return;
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                //ShowInTaskbar = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Options.Get().Load();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Options.Get().Save();
            Close();
            ReinitTimer(false);
        }

        private void ReinitTimer(bool onStart)
        {
            timer1.Stop();
            timer1.Start();
            jobsMenuItem.DropDownItems.Clear();
            restMenuItem.DropDownItems.Clear();
            foreach (ArchiveJob j in Options.Get().Jobs)
            {
                jobsMenuItem.DropDownItems.Add(j.Name, null, jobnowMenuItem_Click);
                restMenuItem.DropDownItems.Add(j.Name,null,restMenuItem_Click);
            }
            ProcessTiming(onStart);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ProcessTiming(false);
            if ((_updDelayCnt >= 900000) && (!timer_up.Enabled))
            {
                _rupd.CheckUpdate();
                timer_up.Enabled = true;
            }
            if ((_updDelayCnt < 900000) && (!timer_up.Enabled))
            {
                _updDelayCnt++;
            }
#if PROTECTED
            if (!GRD.Instance.ValidKey())
            {
                _canclose = true;
                Close();
            }
            if (!GRD.Instance.GetFlag(GRD.FlagType.RabDump))
            {
                _canclose = true;
                Close();
            }
#endif
        }

        private void ProcessTiming(bool onstart)
        {
            log().Debug("processing timer " + (onstart ? "OnStart" : ""));
            foreach (ArchiveJob j in Options.Get().Jobs)
                if (j.NeedDump(onstart))
                    DoDump(j);
        }

        private void jobnowMenuItem_Click(object sender, EventArgs e)
        {
            foreach(ArchiveJob j in Options.Get().Jobs)
                if (sender == jobnowMenuItem || j.Name == ((ToolStripMenuItem) sender).Text)
                {
                    if (!j.Busy)
                        DoDump(j);
                }
        }

        private void DoDump(ArchiveJob j)
        {
            notifyIcon1.ShowBalloonTip(5000,"Резервирование",j.Name,ToolTipIcon.Info);
            ArchiveJobThread.MakeJob(j);

        }

        private void restMenuItem_Click(object sender, EventArgs e)
        {
            RestoreForm rest;
            if (sender == restMenuItem)
            {
                if (!restMenuItem.HasDropDownItems)
                {
                    rest = new RestoreForm();
                    rest.ShowDialog();
                }
            }
            else
            {
                    rest = new RestoreForm(((ToolStripMenuItem) sender).Text);
                    rest.ShowDialog();
            }
        }

        private void RunRabnetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArchiveJobThread.RunRabnet("");
        }

        private void новаяФермаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FarmChangeForm().ShowDialog();
        }

        private void MessageCb(string txt, string ttl,ToolTipIcon ico, bool hide)
        {
            if (InvokeRequired)
            {
                MessageSenderCallbackDelegate d = MessageCb;
                Invoke(d,new object[] {txt,ttl,ico,hide});
            }
            else
            {
                if (hide)
                {
                    notifyIcon1.Visible = false;
                    notifyIcon1.Visible = true;
                }
                else
                {
                    notifyIcon1.ShowBalloonTip(5, ttl, txt, ico);
                }
            }
        }

        private void CloseCb()
        {
            if (InvokeRequired)
            {
                CloseCallbackDelegate d = CloseCb;
                Invoke(d);
            }
            else
            {
                _canclose = true;
                Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _rupd.CheckUpdate();
        }

        private void updateKeyMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + @"\..\Guardant\GrdTRU.exe");
            } catch
            {

            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aFrm = new AboutForm();
            //aFrm
            aFrm.Show();
        }

        private void jobsMenuItem_Click(object sender, EventArgs e)
        {

        }

    }
}
