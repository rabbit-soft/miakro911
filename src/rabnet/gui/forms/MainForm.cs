//#define PROTECTED
using System;
using System.Windows.Forms;
using log4net;
using System.Xml;
using rabnet.panels;

#if PROTECTED
using RabGRD;
#endif

namespace rabnet.forms
{
    internal delegate void WorkingHandler();

    public partial class MainForm : Form
    {
        protected static readonly ILog _logger = LogManager.GetLogger(typeof(MainForm));
        private bool _manual = false;
        private RabNetPanel[] panels =null;
        /// <summary>
        /// Панель, активная в данныймомент
        /// </summary>
        private RabNetPanel curpanel=null;
        private static MainForm me = null;
        private static bool _mustclose = false;
#if PROTECTED
        private DateTime _lastPTest = DateTime.MinValue;
        private const int PTEST_DELAY_SEC =5*60;       
#endif

        public static bool MustClose { get { return _mustclose; } }

        public MainForm()
        {
            InitializeComponent(); 
            me = this;
            _logger.Debug("Program started");
            panels=new RabNetPanel[]
            {
                new RabbitsPanel(rabStatusBar1),
                new YoungsPanel(rabStatusBar1),
                new BuildingsPanel(rabStatusBar1),
                new WorksPanel(rabStatusBar1),
                new ButcherPanel(rabStatusBar1)
            };
            //curpanel = panels[0];
            //tabControl1.SelectedIndex = 0;
            //tabControl1_SelectedIndexChanged(null, null);
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
            //CAS.ScaleForm.SummarySaving += new CAS.AddPLUSummaryHandler(AddPluSummary);
            if (
    #if PROTECTED
                GRD.Instance.GetFlag(GRD.FlagType.Butcher) && 
    #endif
                Engine.opt().getIntOption(Options.OPT_ID.BUCHER_TYPE)==1)
            {
                
                //CAS.ScaleForm.StartMonitoring();
            }
#endif
            _mustclose = false;
            usersMenuItem.Visible = Engine.get().isAdmin();
            _manual = true;

            DateTime srvNow = Engine.db().now();
            TimeSpan timeDiff = DateTime.Now.Subtract(srvNow);
            bool curDate = Math.Round(timeDiff.TotalDays) > 0;
            if (curDate)
            {
                _logger.WarnFormat("serv and local date mismatch s:{0:s} l:{1:s}", srvNow.ToShortDateString(), DateTime.Now.ToShortDateString());
                MessageBox.Show(String.Format(@"Дата на сервере не совпадает с датой на данном компьютере.
Это может повлечь за собой непоправимые ошибки.
Рекомендуется установить корректную дату.
Дата на сервере: {0:s}
Дата локальная:  {1:s}", srvNow.ToShortDateString(), DateTime.Now.ToShortDateString()), "Даты не совпадают", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            rabStatusBar1.SetText(0, srvNow.ToShortDateString(),curDate);    
        
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

            _manual = false;
#if !DEMO
            EPasportForm.CheckSelfCidInDb();
            Engine.db().ArchLogs();
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

            tabControl1.SelectedIndex = 0;//раньше было в конструкторе
            tabControl1_SelectedIndexChanged(null, null);
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
            try
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
            catch (Exception exc)
            {
                _logger.Error(exc);
                MessageBox.Show(exc.Message);
            }
        }
#endif
        private void showTierTMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (_manual) return;

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
            if (curpanel != null)
            {
                curpanel.deactivate();
                panel1.Controls.Remove(curpanel);
            }
            for (int i = 1; i < 5; i++)
                rabStatusBar1.SetText(i, "");
            curpanel = panels[tabControl1.SelectedIndex];
            panel1.Controls.Add(curpanel);
            tsmiActions.DropDown = curpanel.getMenu();           
            curpanel.activate();
            Working();
            ProtectTest();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //DataThread.Get().Stop();
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
            if (
    #if PROTECTED
                GRD.Instance.GetFlag(GRD.FlagType.Butcher) &&
    #endif
                Engine.opt().getIntOption(Options.OPT_ID.BUCHER_TYPE) == 1)
            {
                //CAS.ScaleForm.StopMonitoring();
            }
#endif
            if(!rabStatusBar1.IsDisposed && rabStatusBar1.Working)
                rabStatusBar1.Stop();
            if (Engine.opt().getIntOption(Options.OPT_ID.CONFIRM_EXIT) == 0 || _mustclose ) return;

            DialogResult dlr = MessageBox.Show("Вы уверены что хотите Выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlr == DialogResult.No)
            {
                if (LoginForm.stop == false) LoginForm.stop = true;
                e.Cancel = true;
            }

            _logger.Debug("CloseReason: " + e.CloseReason.ToString());
        }

        private void usersMenuItem_Click(object sender, EventArgs e)
        {
            if (!Engine.get().isAdmin()) return;
            new UserForm().ShowDialog();
        }

        /// <summary>
        /// При срабатывании таймера, происходит выход в меню выбора юзера
        /// </summary>
        private void tNoWorking_Tick(object sender, EventArgs e)
        {
            LoginForm.stop = false;
            _logger.Debug("closing by wait timeout");
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
            if (timeToPTest() && (DateTime.Today < GRD.Instance.GetDateStart() || DateTime.Today > GRD.Instance.GetDateEnd()))
            {
                MessageBox.Show("Срок дейсвия лицензии истек!" + Environment.NewLine +
                                "Ваша лицензия позволяла работать с " + GRD.Instance.GetDateStart().ToShortDateString() + " по " + GRD.Instance.GetDateEnd().ToShortDateString() + Environment.NewLine +
                                "Для продления обратитесь к продавцу у которого вы приобрели программу." + Environment.NewLine + "Программа будет закрыта.", "Истекла лицензия", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(101);
            }

        }

        private bool timeToPTest()
        {
            TimeSpan testDelay = DateTime.Now.Subtract(_lastPTest);           
            bool res = testDelay.TotalSeconds >= PTEST_DELAY_SEC; 
            if(res)
                _lastPTest = DateTime.Now;
            return res;
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
        public static void ProtectTest(int farmsCount)
        {
            me.ptest(farmsCount);
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
                Environment.Exit(100);
            }
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
            rabStatusBar1.FilterSwitch();
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
            if (rabStatusBar1.Working) return;

            new DeadForm().ShowDialog();
            if(!_mustclose)
                rabStatusBar1.Run();
        }

        private void namesMenuItem_Click(object sender, EventArgs e)
        {
            if (rabStatusBar1.Working) return;
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

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
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
                    Engine.get().db().makeReport(myReportType.FUCKER, f),dlg.getXml()})).Show();
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
                f[Filters.DATE_PERIOD] = dlg.PeriodChar;
                f[Filters.DATE_VALUE] = dlg.PeriodValue;
                (new ReportViewForm(myReportType.DEADREASONS, new XmlDocument[]
                {
                    Engine.db().makeReport(myReportType.DEADREASONS,f),
                    dlg.GetXml()
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
                f[Filters.DATE_PERIOD] = dlg.PeriodChar;
                f[Filters.DATE_VALUE] = dlg.PeriodValue;
                (new ReportViewForm(myReportType.DEAD,
                    new XmlDocument[]
                    {
                        Engine.db().makeReport(myReportType.DEAD,f),
                        dlg.GetXml()
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
                f[Filters.DATE_PERIOD] = dlg.PeriodChar;
                f[Filters.DATE_VALUE] = dlg.DateValue;
                (new ReportViewForm(myReportType.USER_OKROLS, 
                    new XmlDocument[]
                    {
                        Engine.get().db().makeReport(myReportType.USER_OKROLS,f),
                        dlg.getXml()
                    }
                    )).Show();

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
                f[Filters.DATE_PERIOD] = dlg.PeriodChar;
                f[Filters.DATE_VALUE] = dlg.PeriodValue;
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
            if (GRD.Instance.GetFlag(GRD.FlagType.Butcher))
            {
#endif
#if !DEMO
                ButcherReportDate brd = new ButcherReportDate();
                if (brd.ShowDialog() == DialogResult.OK)
                {
                    Filters f = new Filters();
                    f[Filters.DATE_PERIOD] = brd.PeriodChar;
                    f[Filters.DATE_VALUE] = brd.DateValue;
                    (new ReportViewForm("Отчет по стерильному цеху", "butcher", new XmlDocument[]
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
            else
                MessageBox.Show("Текущая лицензия не распространяется на данный отчет.");
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
#if !DEMO
            (new MealForm()).ShowDialog();
#else
            DemoErr.DemoNoModuleMsg();
#endif
        }

        private void miScale_Click(object sender, EventArgs e)
        {
#if !DEMO
            tNoWorking.Stop();
            //(new CAS.ScaleForm()).ShowDialog();
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

        //private void AddPluSummary(int pluID, string pluPN1, int pluTSell, int TSumm, int TWeight, DateTime LastClear)
        //{
        //    Engine.db2().addPLUSummary(pluID,pluPN1,pluTSell,TSumm,TWeight,LastClear);
        //}

        private void miLogs_Click(object sender, EventArgs e)
        {
#if !DEMO
            (new LogsForm()).Show();
#else
            DemoErr.DemoNoModuleMsg();
#endif

        }

        private void miRabExport_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (new EPasportForm(true).ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                rabStatusBar1.Run();
#else
            DemoErr.DemoNoModuleMsg();
#endif
        }

        private void miChangeLog_Click(object sender, EventArgs e)
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"changeLog.html");
            if(System.IO.File.Exists(path))
                System.Diagnostics.Process.Start(path);
        }
    }
}
