using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace rabnet
{
    public partial class MainForm : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        private bool manflag=false;
        private RabNetPanel[] panels =null;
        private RabNetPanel curpanel=null;
        public MainForm()
        {
            InitializeComponent();
            log.Debug("Program started");
            panels=new RabNetPanel[]{new RabbitsPanel(rabStatusBar1),
                        new YoungsPanel(rabStatusBar1),
                        new BuildingsPanel(rabStatusBar1),
                        new WorksPanel(rabStatusBar1)};
            curpanel = panels[0];
            tabControl1.SelectedIndex = 0;
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChangeFarmMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm.stop = false;
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            manflag = true;
            rabStatusBar1.setText(0, Engine.db().now().ToShortDateString());
            Text = Engine.get().farmName();
            Options op = Engine.opt();
            showTierTMenuItem.Checked = (op.getIntOption(Options.OPT_ID.SHOW_TIER_TYPE) == 1);
            showTierSMenuItem.Checked = (op.getIntOption(Options.OPT_ID.SHOW_TIER_SEC) == 1);
            shortNamesMenuItem.Checked = (op.getIntOption(Options.OPT_ID.SHORT_NAMES) == 1);
            dblSurMenuItem.Checked = (op.getIntOption(Options.OPT_ID.DBL_SURNAME) == 1);
            geterosisMenuItem.Checked = (op.getIntOption(Options.OPT_ID.GETEROSIS) == 1);
            inbreedingMenuItem.Checked = (op.getIntOption(Options.OPT_ID.INBREEDING) == 1);
            shNumMenuItem.Checked = (op.getIntOption(Options.OPT_ID.SHOW_NUMBERS) == 1);
            rabStatusBar1.run();
            manflag = false;
        }

        private void фильтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rabStatusBar1.filterSwitch();
        }

        private void showTierTMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (manflag)
                return;
            bool reshow = true;
            Options.OPT_ID id=Options.OPT_ID.SHOW_TIER_TYPE;
            if (sender == showTierSMenuItem)id = Options.OPT_ID.SHOW_TIER_SEC;
            if (sender == dblSurMenuItem) id = Options.OPT_ID.DBL_SURNAME;
            if (sender == shortNamesMenuItem) id = Options.OPT_ID.SHORT_NAMES;
            if (sender == geterosisMenuItem) { id = Options.OPT_ID.GETEROSIS; reshow = false; }
            if (sender == inbreedingMenuItem) { id = Options.OPT_ID.INBREEDING; reshow = false; }
            if (sender == shNumMenuItem) id = Options.OPT_ID.SHOW_NUMBERS;
            Engine.opt().setOption(id, ((sender as ToolStripMenuItem).Checked ? 1 : 0));
            if (reshow)
                rabStatusBar1.run();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataThread.get().stop();
            curpanel.deactivate();
            panel1.Controls.Remove(curpanel);
            curpanel = panels[tabControl1.SelectedIndex];
            panel1.Controls.Add(curpanel);
            curpanel.activate();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = 0; i < panels.Length; i++)
                panels[i].close();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void breedsMenuItem_Click(object sender, EventArgs e)
        {
            new CatalogForm(CatalogForm.CatalogType.BREEDS).ShowDialog();
        }

        private void namesMenuItem_Click(object sender, EventArgs e)
        {
            new NamesForm().ShowDialog();
        }

    }
}
