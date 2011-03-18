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
#if PROTECTED
using RabGRD;
#endif

namespace rabnet
{
    public partial class MainForm : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        private bool manflag=false;
        private RabNetPanel[] panels =null;
        /// <summary>
        /// Панель, активная в данныймомент
        /// </summary>
        private RabNetPanel curpanel=null;
        private static MainForm me = null;
        private bool mustclose = false;
        public MainForm()
        {
            InitializeComponent();
            me = this;
            log.Debug("Program started");
            panels=new RabNetPanel[]
            {
                new RabbitsPanel(rabStatusBar1),
                new YoungsPanel(rabStatusBar1),
                new BuildingsPanel(rabStatusBar1),
                new WorksPanel(rabStatusBar1),
                new ButcherPanel(rabStatusBar1)
            };
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
            this.Text = Engine.get().farmName();
#if DEMO
            Text += " Демонстрационная версия";
#endif
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
#if PROTECTED || DEMO
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
            tsmiActions.DropDown = curpanel.getMenu();
            curpanel.activate();
            working();
            protectTest();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataThread.get().stop();
            for (int i = 0; i < panels.Length; i++)
                panels[i].close();
        }

        private void tsmiAboutPO_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (Engine.opt().getIntOption(Options.OPT_ID.CONFIRM_EXIT) == 0)
                return;
            if (mustclose) return;
            DialogResult dlr = MessageBox.Show("Вы уверены что хотите Выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr == DialogResult.No)
            {
                if (LoginForm.stop == false) LoginForm.stop = true;
                e.Cancel = true;
            }

            log.Debug("CloseReason: " + e.CloseReason.ToString());
        }

        private void usersMenuItem_Click(object sender, EventArgs e)
        {
            if (!Engine.get().isAdmin()) return;
            new UserForm().ShowDialog();
        }

        /// <summary>
        /// При срабатывании таймера, происходит выход в меню выбора юзера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tNoWorking_Tick(object sender, EventArgs e)
        {
            LoginForm.stop = false;
            mustclose = true;
            Close();
        }
        /// <summary>
        /// Сбрасывает таймер Простоя
        /// </summary>
        public void working()
        {
            tNoWorking.Stop();
            tNoWorking.Start();
#if PROTECTED
            if ((DateTime.Today < GRD.Instance.GetDateStart()) || (DateTime.Today > GRD.Instance.GetDateEnd()))
            {
                MessageBox.Show("Срок дейсвия лицензии истек!" + Environment.NewLine +
                                "Ваша лицензия позволяла работать с " + GRD.Instance.GetDateStart().ToShortDateString() + " по " + GRD.Instance.GetDateEnd().ToShortDateString() + Environment.NewLine +
                                "Для продления обратитесь к продавцу у которого вы приобрели программу." + Environment.NewLine + "Программа будет закрыта.", "Истекла лицензия", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(101);
            }
#endif
        }
        /// <summary>
        /// Сбрасывает таймер простоя. Тем самым обозначает что с программой работают.
        /// </summary>
        public static void still_working()
        {
            me.working();
        }
        public static void protectTest()
        {
            me.ptest(0);
        }
        /// <summary>
        /// Проверяет наличие ключа
        /// </summary>
        /// <param name="farms">Количество ферм</param>
        public static void protectTest(int farms)
        {
            me.ptest(farms);
        }
        public void ptest(int farms)
        {
#if PROTECTED
            String msg = "";
//            GRD.Instance.ValidKey();
            if (!GRD.Instance.ValidKey())
                msg = "Ключ защиты не найден.";
            if (farms > 0 && farms > GRD.Instance.GetFarmsCnt())
                msg = "Превышено количество разрешенных ферм.";
            if (msg != "")
            {
                MessageBox.Show(this, msg + "\nПрограмма будет закрыта.", "Ошибка защиты", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                LoginForm.stop = true;
                mustclose = true;
                Close();
            }
/*            if (!PClient.get().canwork())
                msg = "Ключ защиты не найден.";
            if (farms > 0 && farms > PClient.get().farms())
                msg = "Превышено количество разрешенных ферм.";
            if (msg != "")
            {
                MessageBox.Show(this, msg + "\nПрограмма будет закрыта.", "Ошибка защиты", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                // Надо сделать выход более доброжелательным
//                LoginForm.stop = true;
//                mustclose = true;
//                Close();
                Environment.Exit(100);
            }*/
#endif
#if DEMO
            if (farms > 100)
            {
                MessageBox.Show(this, "Превышено количество разрешенных ферм." + Environment.NewLine + "Программа будет закрыта.", "Демонстрационная версия", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                // Надо сделать выход более доброжелательным
//                LoginForm.stop = true;
//                mustclose = true;
//                Close();
                Environment.Exit(100);
            }
#endif
        }
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            working();
        }

        private void tsmiHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "rabHelp.chm");
        }

#region Views

        private void tsmiFilter_Click(object sender, EventArgs e)
        {
            rabStatusBar1.filterSwitch();
        }

        private void paramsMenuItem1_Click(object sender, EventArgs e)
        {
            if ((new OptionsForm()).ShowDialog() == DialogResult.OK)
                rabStatusBar1.run();
        }

        private void tsmiDeadsArchive_Click(object sender, EventArgs e)
        {
            new DeadForm().ShowDialog();
            rabStatusBar1.run();
        }

        private void namesMenuItem_Click(object sender, EventArgs e)
        {
            new NamesForm().ShowDialog();
        }

        private void breedsMenuItem_Click(object sender, EventArgs e)
        {
            new CatalogForm(CatalogForm.CatalogType.BREEDS).ShowDialog();
        }

        private void tsmiAreas_Click(object sender, EventArgs e)
        {
            new CatalogForm(CatalogForm.CatalogType.ZONES).ShowDialog();
        }

        private void tsmiDeadReasonsView_Click(object sender, EventArgs e)
        {
#if !DEMO
            new CatalogForm(CatalogForm.CatalogType.DEAD).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void tsmiProductTypesView_Click(object sender, EventArgs e)
        {
#if !DEMO
            new CatalogForm(CatalogForm.CatalogType.PRODUCTS).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

#endregion Views 


#region reports

        private void тестовыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            (new ReportViewForm("Тестовый отчет", "test",
                Engine.get().db().makeReport(myReportType.TEST, null))).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }


        private void tsmiBreeds_Click(object sender, EventArgs e)
        {
#if !DEMO
            Filters f = new Filters();
            f["brd"] = Engine.get().brideAge().ToString();
            (new ReportViewForm("Отчет по породам", "breeds",Engine.get().db().makeReport(myReportType.BREEDS, f)) ).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void tsmiFuckProductivity_Click(object sender, EventArgs e)
        {
#if !DEMO
            FuckerForm dlg = new FuckerForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Filters f = new Filters();
                f["prt"] = dlg.getFucker().ToString();
                f["dfr"] = dlg.getFromDate();
                f["dto"] = dlg.getToDate();
                (new ReportViewForm("Статистика продуктивности", "fucker", new XmlDocument[]{
                    Engine.get().db().makeReport(myReportType.FUCKER, f),dlg.getXml()})).ShowDialog();
            }
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void tsmiAgeAndCount_Click(object sender, EventArgs e)
        {
#if !DEMO
            (new ReportViewForm("Статистика возрастного поголовья", "age",
                Engine.get().db().makeReport(myReportType.AGE, null))).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void tsmiCountByMonth_Click(object sender, EventArgs e)
        {
#if !DEMO
            Filters f = new Filters();
            (new ReportViewForm("Количество по месяцам", "by_month", new XmlDocument[]{
                    Engine.get().db().makeReport(myReportType.BY_MONTH,f)})).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void tsmiDeads_Click(object sender, EventArgs e)
        {
#if !DEMO
            Filters f = new Filters();
            XmlDocument dt = null;
            if (PeriodForm.Run(f, PeriodForm.Preset.CUR_MONTH, ref dt) == DialogResult.OK)
                (new ReportViewForm("Причины списаний", "deadreason", new XmlDocument[]{
                    Engine.db().makeReport(myReportType.DEADREASONS,f),dt})).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void tsmiDeadsReaport_Click(object sender, EventArgs e)
        {
#if !DEMO
            Filters f = new Filters();
            XmlDocument dt = null;
            if (PeriodForm.Run(f, PeriodForm.Preset.CUR_MONTH, ref dt) == DialogResult.OK)
                (new ReportViewForm("Списания", "dead", new XmlDocument[]{
                    Engine.db().makeReport(myReportType.DEAD,f),dt})).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void tsmiFucksByUsers_Click(object sender, EventArgs e)
        {
#if !DEMO
            OkrolUser dlg = new OkrolUser();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Filters f = new Filters();
                f["user"] = dlg.getUser().ToString();
                f["dfr"] = dlg.getFromDate();
                f["dto"] = dlg.getToDate(); 
                (new ReportViewForm("Окролы по пользователям", "okrol_user", 
                    new XmlDocument[]
                    {
                        Engine.get().db().makeReport(myReportType.USER_OKROLS,f),
                        dlg.getXml()
                    }
                    )).ShowDialog();
            }
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void fucksByDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            OkrolUser dlg = new OkrolUser();
            Filters f = new Filters();
            (new ReportViewForm("Список случек/окролов", "fucks_by_date", new XmlDocument[]{
                    Engine.get().db().makeReport(myReportType.FUCKS_BY_DATE,f)})).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

#endregion reports

        private void мяснойЦехToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var brd = new ButcherReportDate();
            if (brd.ShowDialog() == DialogResult.OK)
            {
                Filters f = new Filters();
                f["per"] = brd.Period == ButcherReportDate.myDatePeriod.Month ? "0" : "1";
                (new ReportViewForm("Отчет по завесам", "butcher", Engine.get().db().makeReport(myReportType.BUTCHER_PERIOD, f))).ShowDialog();
            }
        }

    }
}
