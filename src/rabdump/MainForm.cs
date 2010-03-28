using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;


namespace rabdump
{
    public partial class MainForm : Form
    {
        public static readonly ILog logger = LogManager.GetLogger(typeof(MainForm));
        bool canclose = false;
        bool manual = true;
        public MainForm()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        public static ILog log()
        {
            return logger;
        }
        private void restoreMenuItem_Click(object sender, EventArgs e)
        {
            manual = false;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            manual = true;
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            canclose = (MessageBox.Show(this,"Выйти из программы?","rabdump",MessageBoxButtons.YesNo)==DialogResult.Yes);
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !canclose;
            if (canclose)
                log().Debug("Program finished");
            manual = false;
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            manual = true;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            restoreMenuItem.PerformClick();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = Icon;
            propertyGrid1.SelectedObject = Options.get();
            log().Debug("Program started");
            Options.get().load();
            ReinitTimer(true);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (!manual) return;
            if (WindowState == FormWindowState.Minimized)
                ShowInTaskbar = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Options.get().load();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Options.get().save();
            Close();
            ReinitTimer(false);
        }

        private void ReinitTimer(bool OnStart)
        {
            timer1.Stop();
            timer1.Start();
            jobsMenuItem.DropDownItems.Clear();
            restMenuItem.DropDownItems.Clear();
            foreach (ArchiveJob j in Options.get().Jobs)
            {
                jobsMenuItem.DropDownItems.Add(j.Name, null, jobnowMenuItem_Click);
                restMenuItem.DropDownItems.Add(j.Name,null,restMenuItem_Click);
            }
            processTiming(OnStart);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            processTiming(false);
        }

        private void processTiming(bool onstart)
        {
            log().Debug("processing timer "+(onstart?"OnStart":""));
            foreach (ArchiveJob j in Options.get().Jobs)
                if (j.needDump(onstart))
                    doDump(j);
        }

        private void jobnowMenuItem_Click(object sender, EventArgs e)
        {
            foreach(ArchiveJob j in Options.get().Jobs)
                if (sender == jobnowMenuItem || j.Name == (sender as ToolStripMenuItem).Text)
                {
                    if (!j.busy)
                        doDump(j);
                }
        }

        private void doDump(ArchiveJob j)
        {
            notifyIcon1.ShowBalloonTip(5000,"Резервирование",j.Name,ToolTipIcon.Info);
            ArchiveJobThread.MakeJob(j);

        }

        private void restMenuItem_Click(object sender, EventArgs e)
        {
            if (sender == restMenuItem)
                return;
        }

    }
}
