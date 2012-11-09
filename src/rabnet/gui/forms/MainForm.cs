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
using X_Tools;
using System.IO;
using System.Reflection;
#if PROTECTED
using RabGRD;
#endif

namespace rabnet
{
    public partial class MainForm : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        private bool manual = false;
        private RabNetPanel[] panels =null;
        /// <summary>
        /// Панель, активная в данныймомент
        /// </summary>
        private RabNetPanel curpanel=null;
        private static MainForm me = null;
        private static bool _mustclose = false;

        public static bool MustClose { get { return _mustclose; } }

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
            _mustclose = true;
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
#if !DEMO
            CAS.ScaleForm.SummarySaving += new CAS.AddPLUSummaryHandler(AddPluSummary);
            if (
    #if PROTECTED
                GRD.Instance.GetFlag(GRD.FlagType.Butcher) && 
    #endif
                Engine.opt().getIntOption(Options.OPT_ID.BUCHER_TYPE)==1)
            {
                
                CAS.ScaleForm.StartMonitoring();
            }
#endif
            usersMenuItem.Visible = Engine.get().isAdmin();
            manual = true;
            rabStatusBar1.setText(0, Engine.db().now().ToShortDateString());
            this.Text = Engine.get().farmName();
#if DEMO
            this.Text += " Демонстрационная версия";
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
            Building.SetDefFmt(op.getIntOption(Options.OPT_ID.BUILD_FILL_ZERO) == 1 ? '0' : ' ');
            //rabStatusBar1.run();
            manual = false;
#if !DEMO
            checkPlugins();
    #if PROTECTED
            uint elapsed =(uint) GRD.Instance.GetDateEnd().Subtract(DateTime.Now).Days;
            if (elapsed <= 10)
                MessageBox.Show(String.Format("Срок лицензии истекает через {0:d} дней", elapsed));
    #endif
#endif
#if PROTECTED || DEMO
            MainForm.ProtectTest(BuildingsPanel.GetFarmsCount(Engine.db().buildingsTree()));
#endif
        }

#if !DEMO 
        private void checkPlugins()
        {
#if PROTECTED
            if (GRD.Instance.GetFlag(GRD.FlagType.ReportPlugIns))
            {
#endif          
                if (ReportBase.CheckPlugins() != 0)
                {
                    tsmiReports.DropDownItems.Add(new ToolStripSeparator());
                    foreach (ReportBase p in ReportBase.Plugins)
                    {
                        ToolStripMenuItem menu = new ToolStripMenuItem(p.MenuText);
                        menu.Tag = p.UniqueName;
                        menu.Click += new EventHandler(reportPluginMenu_Click);
                        tsmiReports.DropDownItems.Add(menu);
                    }
                }
#if PROTECTED
            }
#endif
        }

        private void reportPluginMenu_Click(object sender, EventArgs e)
        {
#if PROTECTED
            if (GRD.Instance.GetFlag(GRD.FlagType.ReportPlugIns))
            {
#endif
                ReportBase p = ReportBase.GetPluginByName((sender as ToolStripMenuItem).Tag.ToString());
                if (p != null)
                    p.MakeReport();
#if PROTECTED
            }
#endif
        }
#endif
        private void showTierTMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (manual)
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
                rabStatusBar1.Run();
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
            rabStatusBar1.dExcelButtonClick = curpanel.MakeExcel;
            curpanel.activate();
            Working();
            ProtectTest();
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
#if !DEMO
            if(
    #if PROTECTED
                GRD.Instance.GetFlag(GRD.FlagType.Butcher) && 
    #endif
                Engine.opt().getIntOption(Options.OPT_ID.BUCHER_TYPE)==1)
                CAS.ScaleForm.StopMonitoring();
#endif
            if (Engine.opt().getIntOption(Options.OPT_ID.CONFIRM_EXIT) == 0)
                return;
            if (_mustclose) return;

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
            _mustclose = true;
            Close();
        }
        /// <summary>
        /// Сбрасывает таймер Простоя
        /// </summary>
        public void Working()
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
        public static void StillWorking()
        {
            me.Working();
        }
        public static void ProtectTest()
        {
            me.ptest(0);
        }

        /// <summary>
        /// Проверяет допустимо ли количество Ферм
        /// </summary>
        /// <param name="farms">Количество ферм</param>
        public static void ProtectTest(int farms)
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
                _mustclose = true;
                Close();//Environment.Exit(100);
            }
/*            if (!PClient.get().canwork())
                msg = "Ключ защиты не найден.";
            if (farms > 0 && farms > PClient.get().farms())
                msg = "Превышено количество разрешенных ферм.";
            if (msg != "")
            {
                MessageBox.Show(this, msg + "\nПрограмма будет закрыта.", "Ошибка защиты", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //TODO Надо сделать выход более доброжелательным
//                LoginForm.stop = true;
//                mustclose = true;
//                Close();
                Environment.Exit(100);
            }*/
#endif
#if DEMO
            if (farms > BuildingsPanel.DEMO_MAX_FARMS)
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
            Working();
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
            {
                Options op = Engine.opt();
                Building.SetDefFmt(op.getIntOption(Options.OPT_ID.BUILD_FILL_ZERO) == 1 ? '0' : ' ');
                rabStatusBar1.Run();
            }
        }

        private void tsmiDeadsArchive_Click(object sender, EventArgs e)
        {
            new DeadForm().ShowDialog();
            if(!_mustclose)
                rabStatusBar1.Run();
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
//#if !DEMO
            new CatalogForm(CatalogForm.CatalogType.DEAD).ShowDialog();
//#else
//            DemoErr.DemoNoReportMsg();
//#endif
        }

        private void tsmiProductTypesView_Click(object sender, EventArgs e)
        {
#if !DEMO
            new CatalogForm(CatalogForm.CatalogType.PRODUCTS).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void tsmiVaccines_Click(object sender, EventArgs e)
        {
#if !DEMO
            //new CatalogForm(CatalogForm.CatalogType.VACCINES).ShowDialog();
            new VaccinesCatalogForm().ShowDialog();
            if (!_mustclose)
                rabStatusBar1.Run();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

#endregion Views 


#region reports

        private void тестовыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            (new ReportViewForm(myReportType.TEST,Engine.get().db().makeReport(myReportType.TEST, null))).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }


        private void tsmiBreeds_Click(object sender, EventArgs e)
        {
#if !DEMO
            Filters f = new Filters();
            f["brd"] = Engine.get().brideAge().ToString();
            f["cnd"] = Engine.get().candidateAge().ToString();
            (new ReportViewForm(myReportType.BREEDS,Engine.get().db().makeReport(myReportType.BREEDS, f)) ).ShowDialog();
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
                (new ReportViewForm(myReportType.FUCKER, new XmlDocument[]{
                    Engine.get().db().makeReport(myReportType.FUCKER, f),dlg.getXml()})).ShowDialog();
            }
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void tsmiAgeAndCount_Click(object sender, EventArgs e)
        {
#if !DEMO
            (new ReportViewForm(myReportType.AGE,
                Engine.get().db().makeReport(myReportType.AGE, null))).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void tsmiCountByMonth_Click(object sender, EventArgs e)
        {
#if !DEMO
            Filters f = new Filters();
            (new ReportViewForm(myReportType.BY_MONTH, new XmlDocument[]{
                    Engine.get().db().makeReport(myReportType.BY_MONTH,f)})).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void tsmiDeads_Click(object sender, EventArgs e)
        {
#if !DEMO
            Filters f = new Filters();
            //XmlDocument dt = null;
            PeriodForm dlg = new PeriodForm(myReportType.DEADREASONS);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                f["dttp"] = dlg.PeriodChar;
                f["dtval"] = dlg.DateValue;
                (new ReportViewForm(myReportType.DEADREASONS, new XmlDocument[]
                {
                    Engine.db().makeReport(myReportType.DEADREASONS,f),
                    dlg.getXml()
                })).ShowDialog();
            }
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        /// <summary>
        /// Отчет "Списания"
        /// </summary>
        private void tsmiDeadsReaport_Click(object sender, EventArgs e)
        {
#if !DEMO
            Filters f = new Filters();
            //XmlDocument dt = null;
            PeriodForm dlg = new PeriodForm(myReportType.DEAD);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                f["dttp"] = dlg.PeriodChar;
                f["dtval"] = dlg.DateValue;
                (new ReportViewForm(myReportType.DEAD,
                    new XmlDocument[]
                    {
                        Engine.db().makeReport(myReportType.DEAD,f),
                        dlg.getXml()
                    }
                 )).ShowDialog();
            }
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
                f["dttp"] = dlg.PeriodChar;
                f["dtval"] = dlg.DateValue;
                (new ReportViewForm(myReportType.USER_OKROLS, 
                    new XmlDocument[]
                    {
                        Engine.get().db().makeReport(myReportType.USER_OKROLS,f),
                        dlg.getXml()
                    }
#if DEBUG
                    )).Show();
#else
                    )).ShowDialog();
#endif
            }
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void fucksByDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            Filters f = new Filters();
            PeriodForm dlg = new PeriodForm(myReportType.FUCKS_BY_DATE);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                f["dttp"] = dlg.PeriodChar;
                f["dtval"] = dlg.DateValue;
                (new ReportViewForm(myReportType.FUCKS_BY_DATE, new XmlDocument[]
                {
                    Engine.get().db().makeReport(myReportType.FUCKS_BY_DATE,f)
                })).ShowDialog();
            }
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void miButcher_Click(object sender, EventArgs e)
        {            
#if PROTECTED
            if (!GRD.Instance.GetFlag(GRD.FlagType.Butcher))
            {
#endif
#if !DEMO
                ButcherReportDate brd = new ButcherReportDate();
                if (brd.ShowDialog() == DialogResult.OK)
                {
                    Filters f = new Filters();
                    f["dttp"] = brd.PeriodChar;
                    f["dtval"] = brd.DateValue;
                    (new ReportViewForm("Отчет по стерильному цеху", "butcher",new XmlDocument[]
                    { 
                        Engine.get().db().makeReport(myReportType.BUTCHER_PERIOD, f),
                        //brd.getXml()
                    }
                    )).ShowDialog();
                }   
#else
            DemoErr.DemoNoReportMsg();
#endif
#if PROTECTED
            }
#endif
        }

//        private void showRepport(myReportType type)
//        {
//            Filters f = new Filters();
//            ReportViewForm dlg = null;
//            switch (type)
//            {
//                case myReportType.AGE: break;
//            }
//            if (dlg == null) return;
//#if DEBUG
//            dlg.Show();
//#else
//            dlg.ShowDialog();
//#endif
//        }

#endregion reports

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {

            if (tabControl1.SelectedTab == tpButcher)
            {
#if PROTECTED
                if (!GRD.Instance.GetFlag(GRD.FlagType.Butcher))
                {
                    MessageBox.Show("Текущая лицензия не распространяется на данный модуль");
                    e.Cancel = true;
                }
#elif DEMO
                DemoErr.DemoNoModuleMsg();
                e.Cancel = true;
#endif
            }
        }

        private void tsmiReports_DropDownOpening(object sender, EventArgs e)
        {
#if PROTECTED
            miButcher.Visible =  GRD.Instance.GetFlag(GRD.FlagType.Butcher) && Engine.opt().getIntOption(Options.OPT_ID.BUCHER_TYPE)==0 && GRD.Instance.GetFlag(GRD.FlagType.Butcher);           
#elif DEMO
            miButcher.Visible = false;        
#endif
        }

        private void miMeal_Click(object sender, EventArgs e)
        {
            (new MealForm()).ShowDialog();
        }

        private void miScale_Click(object sender, EventArgs e)
        {
#if !DEMO
            tNoWorking.Stop();
            (new CAS.ScaleForm()).ShowDialog();
            tNoWorking.Start();
#endif
        }

        private void tsmiOptions_DropDownOpening(object sender, EventArgs e)
        {
#if !DEMO
            miScale.Visible = 
#if PROTECTED
                GRD.Instance.GetFlag(GRD.FlagType.Butcher) && 
#endif
                Engine.opt().getIntOption(Options.OPT_ID.BUCHER_TYPE)==1;
#else
            miScale.Visible = false;
#endif
        }

        private void AddPluSummary(int pluID, string pluPN1, int pluTSell, int TSumm, int TWeight, DateTime LastClear)
        {
            Engine.db2().addPLUSummary(pluID,pluPN1,pluTSell,TSumm,TWeight,LastClear);
        }
    }
}
