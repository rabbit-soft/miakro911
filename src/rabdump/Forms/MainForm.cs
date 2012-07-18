//#define PROTECTED
using System;
using System.Windows.Forms;
using log4net;
//using X_Classes;
using System.Diagnostics;
using System.IO;
using pEngine;
#if PROTECTED
    using RabGRD;
#endif
//using rabnet;

namespace rabdump
{
    public partial class MainForm : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainForm));
        bool _canclose = false;
        bool _manual = true;

        //readonly RabUpdater _rupd;

        //readonly SocketServer _socksrv;

        long _updDelayCnt = 0;

        public MainForm()
        {
            InitializeComponent();
            AppUpdater au = new AppUpdater(RabServWorker.ReqSender, getUpdatePath());
            au.DeleteOldFiles();
            
#if PROTECTED
            miServDump.Enabled = GRD.Instance.GetFlag(GRD.FlagType.ServerDump);
            miSendGlobRep.Enabled = GRD.Instance.GetFlag(GRD_Base.FlagType.WebReports);
#else
            miServDump.Enabled =
                miSendGlobRep.Enabled= true;
#endif
            log4net.Config.XmlConfigurator.Configure();
            //_rupd = new RabUpdater();
            //_rupd.MessageSenderCallback = MessageCb;          
            //_rupd.CloseCallback = CloseCb;
            //_socksrv = new SocketServer(); 
#if PROTECTED
            if(RabGRD.GRD.Instance.GetFlag(GRD.FlagType.ServerDump))
            {
#endif
                RabServWorker.Url = RabnetConfig.GetOption(RabnetConfig.OptionType.serverUrl);
                RabServWorker.OnMessage += new MessageSenderCallbackDelegate(MessageCb);
                miServDump.Visible = true;
#if PROTECTED
            }
#endif
        }

        public void UpdateFinishHandler()
        {
            notifyIcon1.ShowBalloonTip(5, "Обновление завершено", "Программа требует перезапуска",ToolTipIcon.Info);
            Environment.Exit(1);
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
#if !DEBUG
            _canclose = (MessageBox.Show(this,"Выйти из программы?","RabDump",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes);
#else
            _canclose = true;
#endif
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
                logger.Debug("Program finished");
                //_socksrv.Close();
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
            logger.Debug("Program started");
            Options.Get().Load();
            ReinitTimer(true);
#if PROTECTED 
            if(GRD.Instance.GetFlag(GRD.FlagType.WebReports))
            {
#endif
                RabServWorker.SendWebReport();
#if PROTECTED
            }
#endif
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
            tDumper.Stop();
            tDumper.Start();
            jobsMenuItem.DropDownItems.Clear();
            restMenuItem.DropDownItems.Clear();
            miServDump.DropDownItems.Clear();
            foreach (ArchiveJob j in Options.Get().Jobs)
            {
                jobsMenuItem.DropDownItems.Add(j.Name, null, jobnowMenuItem_Click);
                restMenuItem.DropDownItems.Add(j.Name, null, restMenuItem_Click);
                if(GRD.Instance.GetFlag(GRD_Base.FlagType.ServerDump))
                    miServDump.DropDownItems.Add(j.Name, null, miServDump_Click);
            }
            ProcessTiming(onStart);
        }

        private void tDumper_Tick(object sender, EventArgs e)
        {
            ProcessTiming(false);
            if ((_updDelayCnt >= 900000) && (!tUpdater.Enabled))
            {
                //_rupd.CheckUpdate();
                tUpdater.Enabled = true;
            }
            if ((_updDelayCnt < 900000) && (!tUpdater.Enabled))
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

        /// <summary>
        /// Проверяет все расписания на необходимость обновления
        /// </summary>
        /// <param name="onstart">Делать ли дамп при старте</param>
        private void ProcessTiming(bool onstart)
        {
            logger.Debug("processing timer " + (onstart ? "OnStart" : ""));
            foreach (ArchiveJob j in Options.Get().Jobs)
            {
                if (j.NeedDump(onstart))
                    DoDump(j);

#if PROTECTED
                if(GRD.Instance.GetFlag(GRD.FlagType.ServerDump))
                {
#endif
                    if (j.NeedServDump(onstart))
                        ServDump(j);
#if PROTECTED               
                }
#endif


            }
        }

        private void ServDump(ArchiveJob j)
        {
#if PROTECTED
            if(RabGRD.GRD.Instance.GetFlag(GRD.FlagType.ServerDump))   
#endif
                RabServWorker.MakeDump(j);
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
        
        /// <summary>
        /// Делает резервирование одного расписания
        /// </summary>
        /// <param name="j"></param>
        private void DoDump(ArchiveJob j)
        {
            notifyIcon1.ShowBalloonTip(5000,"Резервирование",j.Name,ToolTipIcon.Info);
            ArchiveJobThread.MakeJob(j);
        }

        private void restMenuItem_Click(object sender, EventArgs e)
        {
            ///Чтобы не отображалось 2 формы востановления
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == typeof(RestoreForm))
                {
                    OpenForm.BringToFront();
                    return;
                }
            }

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
                rest = new RestoreForm(((ToolStripMenuItem)sender).Text);
                rest.ShowDialog();
            }
        }

        private void RunRabnetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArchiveJobThread.RunRabnet("");
        }

        private void newFarm_Click(object sender, EventArgs e)
        {
            new rabnet.FarmChangeForm().ShowDialog();
        }

        private void MessageCb(string txt, string ttl,int type, bool hide)
        {
            if (type < 0) type = 0;
            if (type > 3) type = 3;
            if (InvokeRequired)
            {
                MessageSenderCallbackDelegate d = MessageCb;
                Invoke(d,new object[] {txt,ttl,(ToolTipIcon)type,hide});
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
                    notifyIcon1.ShowBalloonTip(10, ttl, txt, (ToolTipIcon)type);//10secs is min
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
            //_rupd.CheckUpdate();
        }

        private void updateKeyMenuItem_Click(object sender, EventArgs e)
        {
#if PROTECTED
            try
            {
                //Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + @"\..\Guardant\GrdTRU.exe");
                string q;
                if(GRD.Instance.GetTRUQuestion(out q)!=0)
                    throw new Exception("не удалось сгенерировать число вопрос");
                ResponceItem ri = RabServWorker.ReqSender.ExecuteMethod(MethodName.ClientGetUpdate,
                    MPN.question,q,
                    MPN.dongleId,GRD.Instance.ID.ToString());
                GRD.Instance.SetTRUAnswer(ri.Value as String);
                notifyIcon1.ShowBalloonTip(5, "", "Обновлено", ToolTipIcon.Info);               
            } 
            catch(Exception exc)
            {
                if (exc.InnerException != null)
                    exc = exc.InnerException;
                logger.Error(exc);
                notifyIcon1.ShowBalloonTip(5, "Ошибка обновления", exc.Message,ToolTipIcon.Error);
            }
#endif
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rabnet.AboutForm aFrm = new rabnet.AboutForm();
            //aFrm
            aFrm.Show();
        }

        private void miServDump_Click(object sender, EventArgs e)
        {
            foreach (ArchiveJob j in Options.Get().Jobs)
            {
                if (j.Name == ((ToolStripMenuItem)sender).Text)
                    if (!j.Busy)
                        ServDump(j);
                    else MessageCb("Выполняется другой процесс", "Отправка отменена", 2, false);
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RabServWorker.SendWebReport();
        }

        private void miManage_Click(object sender, EventArgs e)
        {
#if PROTECTED
            ResponceItem resp = RabServWorker.ReqSender.ExecuteMethod(MethodName.GetClient, MPN.clientId, GRD.Instance.GetClientID().ToString());
            
            DongleUpdater dlg = new DongleUpdater((resp.Value as sClient[])[0]);
            dlg.ShowDialog();
#endif
        }

        private void miCheckForUpdate_Click(object sender, EventArgs e)
        {
            AppUpdater au = new AppUpdater(RabServWorker.ReqSender, getUpdatePath());          
            au.OnUpdateFinish += UpdateFinishHandler;
            au.Check();   
#if !DEBUG
            miCheckForUpdate.Visible = false;
#endif
        }

        private string getUpdatePath()
        {
            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            return di.Parent.FullName;
        }
    }
}
