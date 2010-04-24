//#define PROTECTED
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;
using System.Xml;

namespace rabnet
{
    public partial class MainForm : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        private bool manflag=false;
        private RabNetPanel[] panels =null;
        private RabNetPanel curpanel=null;
        private static MainForm me = null;
        private bool mustclose = false;
        public MainForm()
        {
            InitializeComponent();
            me = this;
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
            mustclose = true;
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            usersMenuItem.Visible = Engine.get().isAdmin();
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
            shortZooMenuItem.Checked = (op.safeIntOption(Options.OPT_ID.SHORT_ZOO,1) == 1);
            //rabStatusBar1.run();
            manflag = false;
#if PROTECTED
            protest(getmax(Engine.db().buildingsTree(), 0));
        }

        private int getmax(TreeData td,int mx)
        {
            int tx=0;
            String[] st = td.caption.Split(':');
            if (st.Length == 3)
                tx = int.Parse(st[1]);
            if (tx > mx) mx=tx;
            if (td.items!=null)
            for (int i = 0; i < td.items.Length; i++)
            {
                tx = getmax(td.items[i], mx);
                if (tx > mx)
                    mx = tx;
            }
            return mx;
#endif
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
            if (sender == shortZooMenuItem) id = Options.OPT_ID.SHORT_ZOO;
            Engine.opt().setOption(id, ((sender as ToolStripMenuItem).Checked ? 1 : 0));
            if (reshow)
                rabStatusBar1.run();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataThread.get().stop();
            curpanel.deactivate();
            for (int i = 1; i < 5;i++ )
                rabStatusBar1.setText(i, "");
            panel1.Controls.Remove(curpanel);
            curpanel = panels[tabControl1.SelectedIndex];
            panel1.Controls.Add(curpanel);
            actMenuItem.DropDown = curpanel.getMenu();
            curpanel.activate();
            working();
            protest();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataThread.get().stop();
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

        private void зоныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CatalogForm(CatalogForm.CatalogType.ZONES).ShowDialog();
        }

        private void paramsMenuItem1_Click(object sender, EventArgs e)
        {
            if ((new OptionsForm()).ShowDialog() == DialogResult.OK)
                rabStatusBar1.run();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Engine.opt().getIntOption(Options.OPT_ID.CONFIRM_EXIT) == 0)
                return;
            if (mustclose) return;
            DialogResult dlr = MessageBox.Show("Вы уверены что хотите Выйти?","Выход",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dlr == DialogResult.No)
            {
                if (LoginForm.stop == false) LoginForm.stop = true;
                e.Cancel = true;
            }   
        }

        private void usersMenuItem_Click(object sender, EventArgs e)
        {
            if (!Engine.get().isAdmin()) return;
            new UserForm().ShowDialog();
        }

        private void забоиПривесыСписанияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DeadForm().ShowDialog();
            rabStatusBar1.run();
        }

        private void тестовыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new ReportViewForm("Тестовый отчет","test", 
                Engine.get().db().makeReport(ReportType.Type.TEST, null))).ShowDialog();
        }

        private void породыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters f = new Filters();
            f["brd"] = Engine.get().brideAge().ToString();
            (new ReportViewForm("Отчет по породам", "breeds", 
                Engine.get().db().makeReport(ReportType.Type.BREEDS, f))).ShowDialog();
        }

        private void возрастИКоличествоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new ReportViewForm("Статистика возрастного поголовья", "age", 
                Engine.get().db().makeReport(ReportType.Type.AGE, null))).ShowDialog();
        }

        private void продуктивностьСоитияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FuckerForm dlg=new FuckerForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Filters f = new Filters();
                f["prt"] = dlg.getFucker().ToString();
                f["dfr"] = dlg.getFromDate();
                f["dto"] = dlg.getToDate();
                (new ReportViewForm("Статистика продуктивности", "fucker",new XmlDocument[]{
                    Engine.get().db().makeReport(ReportType.Type.FUCKER, f),dlg.getXml()})).ShowDialog();
            }
        }

        private void причиныСпичанияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CatalogForm(CatalogForm.CatalogType.DEAD).ShowDialog();
        }

        private void списанияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters f=new Filters();
            XmlDocument dt=null;
            if (PeriodForm.Run(f, PeriodForm.Preset.CUR_MONTH, ref dt) == DialogResult.OK)
                (new ReportViewForm("Причины списаний", "deadreason", new XmlDocument[]{
                    Engine.db().makeReport(ReportType.Type.DEADREASONS,f),dt})).ShowDialog();
        }

        private void списанияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters f = new Filters();
            XmlDocument dt = null;
            if (PeriodForm.Run(f, PeriodForm.Preset.CUR_MONTH, ref dt) == DialogResult.OK)
                (new ReportViewForm("Списания", "dead", new XmlDocument[]{
                    Engine.db().makeReport(ReportType.Type.DEAD,f),dt})).ShowDialog();
		}

        private void окролыПоПользователямToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OkrolUser dlg = new OkrolUser();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Filters f = new Filters();
                f["user"] = dlg.getUser().ToString();
                f["dfr"] = dlg.getFromDate();
                f["dto"] = dlg.getToDate();
                (new ReportViewForm("Окролы по пользователям", "okrol_user", new XmlDocument[]{
                    Engine.get().db().makeReport(ReportType.Type.USER_OKROLS, f),dlg.getXml()})).ShowDialog();
            }
        }

        private void количествоПоМесяцамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters f = new Filters();
            (new ReportViewForm("Количество по месяцам", "by_month", new XmlDocument[]{
                    Engine.get().db().makeReport(ReportType.Type.BY_MONTH,f)})).ShowDialog();
        }

        private void fucksByDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OkrolUser dlg = new OkrolUser();            
            Filters f = new Filters();
            (new ReportViewForm("Список случек/окролов", "fucks_by_date", new XmlDocument[]{
                    Engine.get().db().makeReport(ReportType.Type.FUCKS_BY_DATE,f)})).ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoginForm.stop = false;
            mustclose = true;
            Close();
        }
        public void working()
        {
            timer1.Stop();
            timer1.Start();
        }
        public static void still_working()
        {
            me.working();
        }
        public static void protest()
        {
            me.ptest(0);
        }
        public static void protest(int farms)
        {
            me.ptest(farms);
        }
        public void ptest(int farms)
        {
#if PROTECTED
            String msg = "";
            if (!PClient.get().canwork())
                msg = "Ключ защиты не найден.";
            if (farms > 0 && farms > PClient.get().farms())
                msg = "Превышено количество разрешенных ферм.";
            if (msg != "")
            {
                MessageBox.Show(this,msg+"\nПрограмма будет закрыта.","Ошибка защиты",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                LoginForm.stop = true;
                mustclose = true;
                Close();
            }
#endif
        }
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            working();
        }

    }
}
