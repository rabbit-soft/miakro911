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
using rabnet;

#if PROTECTED
using RabGRD;
using System.Reflection;
#endif

namespace rabdump
{
    public partial class MainForm : Form
    {
        const int PUP_INFO = 1;
        const int PUP_WARN = 2;
        const int PUP_ERROR = 3;


        private static readonly ILog _logger = LogManager.GetLogger(typeof(MainForm));
        //private RabServWorker _rabSW;
        private bool _canclose = false;
        private bool _manual = true;
        long _updDelayCnt = 0;
        RabDumpLan _rdl;

        public MainForm()
        {
            InitializeComponent();
            //RabServWorker.Url = Options.Inst.ServerUrl;
            _rdl = new RabDumpLan();
            _rdl.RabDumpAlreadyInLan += new RabDumpAlreadyInLanHandle(_rdl_RabDumpAlreadyInLan);
            _rdl.Start();
            pAppUpdater.DeleteOldFiles(AppDomain.CurrentDomain.BaseDirectory);

            RabServWorker.OnMessage += new MessageSenderCallbackDelegate(messageCb);
            RabServWorker.OnUpdateChecked += new UpdateCheckedHandler(RabServWorker_OnUpdateChecked);
            RabServWorker.OnUpdateCheckFail += new ErrorHandler(RabServWorker_OnUpdateCheckFail);
            RabServWorker.OnUpdateFinished += new UpdateFinishedHandler(RabServWorker_OnUpdateFinished);

            ArchiveJobThread.OnMessage += new MessageSenderCallbackDelegate(messageCb);
#if PROTECTED
            //miSendGlobRep.Enabled = GRD.Instance.GetFlag(GRD_Base.FlagType.WebReports);
            miSendGlobRep.Visible =
                //miCheckForUpdate.Visible =
                miManage.Visible =
                //miUpdateKey.Visible = 
                miRemoteSeparator.Visible = false;
#endif
            //_rupd = new RabUpdater();
            //_rupd.MessageSenderCallback = MessageCb;          
            //_rupd.CloseCallback = CloseCb;
            //_socksrv = new SocketServer(); 
        }


        private void MainFormNew_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = Icon;
            _logger.Debug("Program started");
            //Options.Get().Load();
            reinitTimer(true);
            _manual = true;
            tabControl1_SelectedIndexChanged(null, null);
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
            _logger.Debug("processing timer " + (onStart ? "OnStart" : ""));
            foreach (ArchiveJob j in Options.Inst.Jobs)
            {
                if (needDump(j, onStart))
                    doDump(j);
            }
        }

        /// <summary>
        /// Нужно ли делать Резервирование по переданному расписанию
        /// </summary>
        /// <param name="start">Программа только что запустилась</param>
        private bool needDump(ArchiveJob aj, bool start)
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
#if PROTECTED
            //if (GRD.Instance.GetFlag(GRD.FlagType.ServerDump))
            //{
#endif
            if (j.SendToServ)
                RabServWorker.SendDump(j);
#if PROTECTED
            //}
#endif
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
        private void messageCb(string txt, string ttl, int type)
        {
            messageCb(txt, ttl, type, false);
        }

        #region mi_click
        private void miRunRabnet_Click(object sender, EventArgs e)
        {
            try
            {
                rabnet.Run.Rabnet();
            }
            catch (Exception exc)
            {
                _logger.Warn(exc.Message);
                MessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

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

        private void miUpdateKey_Click(object sender, EventArgs e)
        {
            try
            {
#if PROTECTED
                string q;
                if (GRD.Instance.GetTRUQuestion(out q) != 0)
                    throw new Exception("Не удалось сгенерировать число-вопрос для локального ключа защиты");
                ResponceItem ri = RabServWorker.ReqSender.ExecuteMethod(MethodName.ClientGetUpdate,
                    MPN.question, q,
                    MPN.dongleId, GRD.Instance.ID.ToString());
                GRD.Instance.SetTRUAnswer(ri.Value as String);
                messageCb("Обновлено","", PUP_INFO);
#elif DEBUG
                throw new Exception("Как бэ нельзя под дебагом обновлять ключи");
#endif
            }
            catch (Exception exc)
            {
                if (exc.InnerException != null)
                    exc = exc.InnerException;
                _logger.Error(exc);
                messageCb(exc.Message, "Обновление лицензии", PUP_ERROR);
            }

        }

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
                _logger.Debug("Program finished");
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
            miCheckForUpdate.Enabled = false;
            if (miCheckForUpdate.Tag.ToString() == "0")
            {
                btCheckUpdate_Click(sender, e);
            }
            else
            {
                btDloadUpdate_Click(sender, e);
            }
        }

        #region event_handlers
        void RabServWorker_OnUpdateCheckFail(Exception exc)
        {
            if (this.InvokeRequired)
            {
                ErrorHandler d = RabServWorker_OnUpdateCheckFail;
                Invoke(d, new object[] { exc });
            }
            else
            {
                pbUpdate.Visible = false;
                miCheckForUpdate.Enabled = true;
                _logger.Error(exc);
                if (exc.InnerException != null)
                    exc = exc.InnerException;
                tbUpdateInfo.Text = "Ошибка: " + exc.Message;
                messageCb(exc.Message, miCheckForUpdate.Tag.ToString() == "0" ? "Проверка обновления" : "Установка обновления", PUP_WARN, false);
            }
        }

        void RabServWorker_OnUpdateChecked(UpdateInfo info)
        {
            if (this.InvokeRequired)
            {
                UpdateCheckedHandler d = RabServWorker_OnUpdateChecked;
                Invoke(d, new object[] { info });
            }
            else
            {
                pbUpdate.Visible = false;
                btDloadUpdate.Visible = info.UpdateRequired;
                string message = String.Format(@"Версия на сервере: {0:s}{1:s}", info.Version.ToString(),
                    Environment.NewLine + (!info.UpdateRequired ? "Обновление не требуется" : "Требуется обновление"));
                tbUpdateInfo.Text = message;
                tbUpdateInfo.Visible = 
                    miCheckForUpdate.Enabled = true;
                messageCb(message, "Проверка обновления", PUP_INFO);

                miCheckForUpdate.Text = "Установить обновление";
                miCheckForUpdate.Tag = "1";
            }
        }

        void RabServWorker_OnUpdateFinished()
        {
            if (this.InvokeRequired)
            {
                UpdateFinishedHandler d = RabServWorker_OnUpdateFinished;
                Invoke(d);
            }
            else
            {
                messageCb("Необходима перезагрузка", "Программа обновлена", 1);
                Run.Updater();
                _logger.Info("Updating Finished");               
                Program.ReleaseMutex();
                Run.RabDump();
                _canclose = true;
                this.Close();
            }
        }

        void _rdl_RabDumpAlreadyInLan(string hostName)
        {
            if (this.InvokeRequired)
            {
                RabDumpAlreadyInLanHandle d = _rdl_RabDumpAlreadyInLan;
                Invoke(d, new object[] { hostName });
            }
            else
            {
                MessageBox.Show(String.Format("В данной локальной сети уже запущено приложение RabDump на компьютере '{0:s}'.", hostName),
                    "Приложение будет закрыто", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _logger.Info("RabDump already in Lan");
                Options.Inst.StartAtStart = false;
                Options.Inst.Save();
                _canclose = true;
                this.Close();
            }
        }
        #endregion event_handlers

        private void btDloadUpdate_Click(object sender, EventArgs e)
        {
            pbUpdate.Left = btCheckUpdate.Left;
            pbUpdate.Visible = true;
            RabServWorker.CheckForUpdate(true);
        }

        private void btCheckUpdate_Click(object sender, EventArgs e)
        {
            pbUpdate.Left = btDloadUpdate.Left;
            pbUpdate.Visible = true;            
            RabServWorker.CheckForUpdate(false);
        }
    }
}