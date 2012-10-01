using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using log4net;
using pEngine;
using rabnet.RNC;
using X_Tools;

#if PROTECTED
using RabGRD;
#endif

namespace rabdump
{
    public partial class MainFormNew : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainFormNew));
        //private RabServWorker _rabSW;
        private bool _canclose = false;
        private bool _manual = true;
        long _updDelayCnt = 0;

        public MainFormNew()
        {
            InitializeComponent();
            RabServWorker.Url = Options.Inst.ServerUrl;
            RabServWorker.OnMessage += new MessageSenderCallbackDelegate(messageCb);
#if PROTECTED
            //miSendGlobRep.Enabled = GRD.Instance.GetFlag(GRD_Base.FlagType.WebReports);
            miSendGlobRep.Visible =
                miCheckForUpdate.Visible =
                miManage.Visible =
                miUpdateKey.Visible = 
                miRemoteSeparator.Visible = false;
#endif
            //log4net.Config.XmlConfigurator.Configure();
            //_rupd = new RabUpdater();
            //_rupd.MessageSenderCallback = MessageCb;          
            //_rupd.CloseCallback = CloseCb;
            //_socksrv = new SocketServer(); 
        }

        private void MainFormNew_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = Icon;
            logger.Debug("Program started");
            //Options.Get().Load();
            reinitTimer(true);
            _manual = true;
            tabControl1_SelectedIndexChanged(null,null);
//#if PROTECTED 
//            if(GRD.Instance.GetFlag(GRD.FlagType.WebReports))
//            {
//#endif
//                RabServWorker.SendWebReport();
//#if PROTECTED
//            }
//#endif
        }     

        /// <summary>
        /// Перезапускает таймер Дампов. Обновляет список расписаний
        /// </summary>
        /// <param name="onStart"></param>
        private void reinitTimer(bool onStart)
        {
            tDumper.Stop();
            tDumper.Start();
            miJobStart.DropDownItems.Clear();
            miRestore.DropDownItems.Clear();
            //miServDump.DropDownItems.Clear();
            foreach (ArchiveJob j in Options.Inst.Jobs)
            {
                miJobStart.DropDownItems.Add(j.JobName, null, miJobDo_Click);
                miRestore.DropDownItems.Add(j.JobName, null, miRestore_Click);
//#if PROTECTED
//                if(GRD.Instance.GetFlag(GRD_Base.FlagType.ServerDump))
//#endif
                //miServDump.DropDownItems.Add(j.JobName, null, miServDump_Click);
            }
            processTiming(onStart);
        }

        private void tDumper_Tick(object sender, EventArgs e)
        {
            processTiming(false);
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
        /// <param name="onStart">Делать ли дамп при старте</param>
        private void processTiming(bool onStart)
        {
            logger.Debug("processing timer " + (onStart ? "OnStart" : ""));
            foreach (ArchiveJob j in Options.Inst.Jobs)
            {
                if (needDump(j,onStart))
                    doDump(j);

//#if PROTECTED
//                if(GRD.Instance.GetFlag(GRD.FlagType.ServerDump))
//                {
//#endif
//                if (j.NeedServDump(onstart))
//                    ServDump(j);
//#if PROTECTED               
//                }
//#endif
            }
        }

        /// <summary>
        /// Нужно ли делать Резервирование по переданному расписанию
        /// </summary>
        /// <param name="start">Программа только что запустилась</param>
        private bool needDump(ArchiveJob aj,bool start)
        {
            if (aj.Busy) return false;
            if (aj.ArcAType == ArchiveType.Никогда)
                return false;
            if (start && aj.ArcAType == ArchiveType.При_Запуске)
                return true;
            //if (Repeat > 0 && DateCmpNoSec(LastWork.AddHours(Repeat)))
            //    return true;
            if (aj.ArcAType == ArchiveType.Единожды && XTools.DateCmpNoSec(aj.StartTime))
                return true;
            if (aj.ArcAType == ArchiveType.Ежедневно && XTools.DateCmpTime(aj.StartTime))
                return true;
            if (aj.ArcAType == ArchiveType.Еженедельно && ((DateTime.Now - aj.StartTime).Days % 7) == 0 && XTools.DateCmpTime(aj.StartTime))
                return true;
            if (aj.ArcAType == ArchiveType.Ежемесячно && aj.StartTime.Day == DateTime.Now.Day && XTools.DateCmpTime(aj.StartTime))
                return true;
            return false;
        }

        /// <summary>
        /// Делает резервирование одного расписания
        /// </summary>
        /// <param name="j"></param>
        private void doDump(ArchiveJob j)
        {
            notifyIcon1.ShowBalloonTip(5000, "Резервирование", j.JobName, ToolTipIcon.Info);
            ArchiveJobThread.MakeJob(j);
        }

        private void messageCb(string txt, string ttl, int type, bool hide)
        {
            if (txt == "") return;
            if (type < 0) type = 0;
            if (type > 3) type = 3;
            if (InvokeRequired)
            {
                MessageSenderCallbackDelegate d = messageCb;
                Invoke(d, new object[] { txt, ttl, (ToolTipIcon)type, hide });
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


        #region mi_click
        private void miShowMainForm_Click(object sender, EventArgs e)
        {
            _manual = false;
            Show();
            //            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            TopMost = true;
            Focus();
            this.BringToFront();
            TopMost = false;
            _manual = true;
        }

        private void miJobDo_Click(object sender, EventArgs e)
        {
            foreach (ArchiveJob j in Options.Inst.Jobs)
                if (/*sender == jobnowMenuItem ||*/ j.JobName == ((ToolStripMenuItem)sender).Text)
                {
                    if (!j.Busy)
                        doDump(j);
                }
        }

        private void miRestore_Click(object sender, EventArgs e)
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
            if (sender == miRestore)
            {
                if (!miRestore.HasDropDownItems)
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

        private void exitMenuItem_Click(object sender, EventArgs e)
        {

#if !DEBUG
            _canclose = (MessageBox.Show(this,"Выйти из программы?","RabDump",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes);
#else
            _canclose = true;
#endif
            this.Close();
        }

        //private void miServDump_Click(object sender, EventArgs e)
        //{
        //    foreach (ArchiveJob j in Options.Inst.Jobs)
        //    {
        //        if (j.JobName == ((ToolStripMenuItem)sender).Text)
        //            if (!j.Busy)
        //                ServDump(j);
        //            else MessageCb("Выполняется другой процесс", "Отправка отменена", 2, false);
        //    }
        //}
        #endregion mi_click

        #region handlers       

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tpGeneral)
                generalPanel1.Init(Options.Inst);
            else if (tabControl1.SelectedTab == tpDataSources)
                farmsPanel1.Init(Options.Inst.GetRabnetConfig());
            else if (tabControl1.SelectedTab == tpArchiveJobs)
                archiveJobsPanel1.Init(Options.Inst.GetRabnetConfig());
        }

        private void MainFormNew_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btOK_Click(object sender, EventArgs e)
        {
            Options.Inst.Save();
            Close();
            tabControl1.SelectedIndex = 0;
            reinitTimer(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Options.Inst.Load();
            tabControl1.SelectedIndex = 0;
            Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            miShowMainForm.PerformClick();
        }

        

        private void MainFormNew_FormClosed(object sender, FormClosedEventArgs e)
        {
            tUpdater.Stop();
            tDumper.Stop();
        }
        #endregion handlers

        private string getUpdatePath()
        {
            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            return di.Parent.FullName;
        }

        private void miCheckForUpdate_Click(object sender, EventArgs e)
        {
            AppUpdater au = new AppUpdater(RabServWorker.ReqSender, getUpdatePath());
            au.DeleteOldFiles();
            au.UpdateFinish += new AppUpdater.UpdateFinishDelegate(au_UpdateFinish);          
            au.Check();
        }

        void au_UpdateFinish(bool success, string msg)
        {
            if (success)
            {
                messageCb("Обновление прошло успешно. Программа должна перезагрузится", "", 0, false);
                _canclose = true;
                Close();
            }
            else
            {

                messageCb(msg, "Ошибка", 2, false);
            }
        }

        //public static void RunRabnet(string param)
        //{
        //    try
        //    {
        //        Process p = Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + @"\..\RabNet\rabnet.exe", param);
        //        if (param == "dbedit")
        //        {
        //            p.WaitForExit();
        //            Options.Inst.Load();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ошибка: "+ex.Message);
        //    }
        //}  
    }
}
