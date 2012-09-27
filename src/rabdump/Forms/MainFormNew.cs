using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace rabdump
{
    public partial class MainFormNew : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainFormNew));
        bool _canclose = false;
        bool _manual = true;
        long _updDelayCnt = 0;

        public MainFormNew()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            RabnetConfig.LoadDataSources();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tpDataSources)
                farmsPanel1.UpdateList();
        }

        private void btMysqlPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                tbMysqlPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tb7zPath.Text = openFileDialog1.FileName;
        }

        private void tbMysqlPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void tpGeneral_Click(object sender, EventArgs e)
        {

        }

        private void MainFormNew_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = Icon;
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
#if PROTECTED
                if(GRD.Instance.GetFlag(GRD_Base.FlagType.ServerDump))
#endif
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

        private void timer1_Tick(object sender, EventArgs e)
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
        /// Делает резервирование одного расписания
        /// </summary>
        /// <param name="j"></param>
        private void DoDump(ArchiveJob j)
        {
            notifyIcon1.ShowBalloonTip(5000, "Резервирование", j.Name, ToolTipIcon.Info);
            ArchiveJobThread.MakeJob(j);
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
            foreach (ArchiveJob j in Options.Get().Jobs)
                if (sender == jobnowMenuItem || j.Name == ((ToolStripMenuItem)sender).Text)
                {
                    if (!j.Busy)
                        DoDump(j);
                }
        }
        #region mi_click
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
        #endregion mi_click

        private void MessageCb(string txt, string ttl, int type, bool hide)
        {
            if (type < 0) type = 0;
            if (type > 3) type = 3;
            if (InvokeRequired)
            {
                MessageSenderCallbackDelegate d = MessageCb;
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
    }
}
