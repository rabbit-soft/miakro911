namespace rabnet
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                for (int i = 0; i < panels.Length; i++)
                    panels[i] = null;
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiFarm = new System.Windows.Forms.ToolStripMenuItem();
            this.сменитьФермуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiView = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.параметрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeadsArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.miMeal = new System.Windows.Forms.ToolStripMenuItem();
            this.miLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.namesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breedsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAreas = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeadReasonsView = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiProductTypesView = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiVaccines = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiActions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReports = new System.Windows.Forms.ToolStripMenuItem();
            this.тестовыйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBreeds = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFuckProductivity = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAgeAndCount = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCountByMonths = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeadReasons = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeadsReaport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFucksByUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.fucksByDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miButcher = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.showTierTMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTierSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortNamesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortZooMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dblSurMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.shNumMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.geterosisMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inbreedingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.miScale = new System.Windows.Forms.ToolStripMenuItem();
            this.paramsMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAboutPO = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tNoWorking = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tpButcher = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rabStatusBar1 = new rabnet.RabStatusBar();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFarm,
            this.tsmiView,
            this.tsmiActions,
            this.tsmiReports,
            this.tsmiOptions,
            this.tsmiAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(914, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFarm
            // 
            this.tsmiFarm.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сменитьФермуToolStripMenuItem,
            this.usersMenuItem,
            this.toolStripMenuItem1,
            this.выходToolStripMenuItem});
            this.tsmiFarm.Name = "tsmiFarm";
            this.tsmiFarm.Size = new System.Drawing.Size(56, 20);
            this.tsmiFarm.Text = "Ферма";
            // 
            // сменитьФермуToolStripMenuItem
            // 
            this.сменитьФермуToolStripMenuItem.Name = "сменитьФермуToolStripMenuItem";
            this.сменитьФермуToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.сменитьФермуToolStripMenuItem.Size = new System.Drawing.Size(297, 22);
            this.сменитьФермуToolStripMenuItem.Text = "Сменить ферму/пользователя ...";
            this.сменитьФермуToolStripMenuItem.Click += new System.EventHandler(this.ChangeFarmMenuItem_Click);
            // 
            // usersMenuItem
            // 
            this.usersMenuItem.Name = "usersMenuItem";
            this.usersMenuItem.Size = new System.Drawing.Size(297, 22);
            this.usersMenuItem.Text = "Пользователи";
            this.usersMenuItem.Click += new System.EventHandler(this.usersMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(294, 6);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(297, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // tsmiView
            // 
            this.tsmiView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFilter,
            this.параметрыToolStripMenuItem,
            this.tsmiDeadsArchive,
            this.miMeal,
            this.miLogs,
            this.toolStripMenuItem2,
            this.namesMenuItem,
            this.breedsMenuItem,
            this.tsmiAreas,
            this.tsmiDeadReasonsView,
            this.tsmiProductTypesView,
            this.tsmiVaccines});
            this.tsmiView.Name = "tsmiView";
            this.tsmiView.Size = new System.Drawing.Size(39, 20);
            this.tsmiView.Text = "Вид";
            // 
            // tsmiFilter
            // 
            this.tsmiFilter.Name = "tsmiFilter";
            this.tsmiFilter.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.tsmiFilter.Size = new System.Drawing.Size(208, 22);
            this.tsmiFilter.Text = "Фильтр";
            this.tsmiFilter.Click += new System.EventHandler(this.tsmiFilter_Click);
            // 
            // параметрыToolStripMenuItem
            // 
            this.параметрыToolStripMenuItem.Name = "параметрыToolStripMenuItem";
            this.параметрыToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.параметрыToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.параметрыToolStripMenuItem.Text = "Параметры";
            this.параметрыToolStripMenuItem.Click += new System.EventHandler(this.paramsMenuItem1_Click);
            // 
            // tsmiDeadsArchive
            // 
            this.tsmiDeadsArchive.Name = "tsmiDeadsArchive";
            this.tsmiDeadsArchive.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.tsmiDeadsArchive.Size = new System.Drawing.Size(208, 22);
            this.tsmiDeadsArchive.Text = "Архив Списаний";
            this.tsmiDeadsArchive.Click += new System.EventHandler(this.tsmiDeadsArchive_Click);
            // 
            // miMeal
            // 
            this.miMeal.Name = "miMeal";
            this.miMeal.Size = new System.Drawing.Size(208, 22);
            this.miMeal.Text = "Учет кормов";
            this.miMeal.Click += new System.EventHandler(this.miMeal_Click);
            // 
            // miLogs
            // 
            this.miLogs.Name = "miLogs";
            this.miLogs.Size = new System.Drawing.Size(208, 22);
            this.miLogs.Text = "Логи";
            this.miLogs.Click += new System.EventHandler(this.miLogs_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(205, 6);
            // 
            // namesMenuItem
            // 
            this.namesMenuItem.Name = "namesMenuItem";
            this.namesMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.namesMenuItem.Size = new System.Drawing.Size(208, 22);
            this.namesMenuItem.Text = "Имена";
            this.namesMenuItem.Click += new System.EventHandler(this.namesMenuItem_Click);
            // 
            // breedsMenuItem
            // 
            this.breedsMenuItem.Name = "breedsMenuItem";
            this.breedsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.breedsMenuItem.Size = new System.Drawing.Size(208, 22);
            this.breedsMenuItem.Text = "Породы";
            this.breedsMenuItem.Click += new System.EventHandler(this.breedsMenuItem_Click);
            // 
            // tsmiAreas
            // 
            this.tsmiAreas.Name = "tsmiAreas";
            this.tsmiAreas.Size = new System.Drawing.Size(208, 22);
            this.tsmiAreas.Text = "Зоны";
            this.tsmiAreas.Click += new System.EventHandler(this.tsmiAreas_Click);
            // 
            // tsmiDeadReasonsView
            // 
            this.tsmiDeadReasonsView.Name = "tsmiDeadReasonsView";
            this.tsmiDeadReasonsView.Size = new System.Drawing.Size(208, 22);
            this.tsmiDeadReasonsView.Text = "Причины списания";
            this.tsmiDeadReasonsView.Click += new System.EventHandler(this.tsmiDeadReasonsView_Click);
            // 
            // tsmiProductTypesView
            // 
            this.tsmiProductTypesView.Name = "tsmiProductTypesView";
            this.tsmiProductTypesView.Size = new System.Drawing.Size(208, 22);
            this.tsmiProductTypesView.Text = "Виды продукции";
            this.tsmiProductTypesView.Click += new System.EventHandler(this.tsmiProductTypesView_Click);
            // 
            // tsmiVaccines
            // 
            this.tsmiVaccines.Name = "tsmiVaccines";
            this.tsmiVaccines.Size = new System.Drawing.Size(208, 22);
            this.tsmiVaccines.Text = "Вакцины";
            this.tsmiVaccines.Click += new System.EventHandler(this.tsmiVaccines_Click);
            // 
            // tsmiActions
            // 
            this.tsmiActions.Name = "tsmiActions";
            this.tsmiActions.Size = new System.Drawing.Size(70, 20);
            this.tsmiActions.Text = "Действия";
            // 
            // tsmiReports
            // 
            this.tsmiReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.тестовыйToolStripMenuItem,
            this.tsmiBreeds,
            this.tsmiFuckProductivity,
            this.tsmiAgeAndCount,
            this.tsmiCountByMonths,
            this.tsmiDeadReasons,
            this.tsmiDeadsReaport,
            this.tsmiFucksByUsers,
            this.fucksByDateToolStripMenuItem,
            this.miButcher});
            this.tsmiReports.Name = "tsmiReports";
            this.tsmiReports.Size = new System.Drawing.Size(60, 20);
            this.tsmiReports.Text = "Отчеты";
            this.tsmiReports.DropDownOpening += new System.EventHandler(this.tsmiReports_DropDownOpening);
            // 
            // тестовыйToolStripMenuItem
            // 
            this.тестовыйToolStripMenuItem.Name = "тестовыйToolStripMenuItem";
            this.тестовыйToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.тестовыйToolStripMenuItem.Text = "Тестовый";
            this.тестовыйToolStripMenuItem.Visible = false;
            this.тестовыйToolStripMenuItem.Click += new System.EventHandler(this.тестовыйToolStripMenuItem_Click);
            // 
            // tsmiBreeds
            // 
            this.tsmiBreeds.Name = "tsmiBreeds";
            this.tsmiBreeds.Size = new System.Drawing.Size(223, 22);
            this.tsmiBreeds.Text = "Состав пород";
            this.tsmiBreeds.Click += new System.EventHandler(this.tsmiBreeds_Click);
            // 
            // tsmiFuckProductivity
            // 
            this.tsmiFuckProductivity.Name = "tsmiFuckProductivity";
            this.tsmiFuckProductivity.Size = new System.Drawing.Size(223, 22);
            this.tsmiFuckProductivity.Text = "Продуктивность соития";
            this.tsmiFuckProductivity.Click += new System.EventHandler(this.tsmiFuckProductivity_Click);
            // 
            // tsmiAgeAndCount
            // 
            this.tsmiAgeAndCount.Name = "tsmiAgeAndCount";
            this.tsmiAgeAndCount.Size = new System.Drawing.Size(223, 22);
            this.tsmiAgeAndCount.Text = "Возраст и количество";
            this.tsmiAgeAndCount.Click += new System.EventHandler(this.tsmiAgeAndCount_Click);
            // 
            // tsmiCountByMonths
            // 
            this.tsmiCountByMonths.Name = "tsmiCountByMonths";
            this.tsmiCountByMonths.Size = new System.Drawing.Size(223, 22);
            this.tsmiCountByMonths.Text = "Количество по месяцам";
            this.tsmiCountByMonths.Click += new System.EventHandler(this.tsmiCountByMonth_Click);
            // 
            // tsmiDeadReasons
            // 
            this.tsmiDeadReasons.Name = "tsmiDeadReasons";
            this.tsmiDeadReasons.Size = new System.Drawing.Size(223, 22);
            this.tsmiDeadReasons.Text = "Причины списания";
            this.tsmiDeadReasons.Click += new System.EventHandler(this.tsmiDeads_Click);
            // 
            // tsmiDeadsReaport
            // 
            this.tsmiDeadsReaport.Name = "tsmiDeadsReaport";
            this.tsmiDeadsReaport.Size = new System.Drawing.Size(223, 22);
            this.tsmiDeadsReaport.Text = "Списания";
            this.tsmiDeadsReaport.Click += new System.EventHandler(this.tsmiDeadsReaport_Click);
            // 
            // tsmiFucksByUsers
            // 
            this.tsmiFucksByUsers.Name = "tsmiFucksByUsers";
            this.tsmiFucksByUsers.Size = new System.Drawing.Size(223, 22);
            this.tsmiFucksByUsers.Text = "Окролы по пользователям";
            this.tsmiFucksByUsers.Click += new System.EventHandler(this.tsmiFucksByUsers_Click);
            // 
            // fucksByDateToolStripMenuItem
            // 
            this.fucksByDateToolStripMenuItem.Name = "fucksByDateToolStripMenuItem";
            this.fucksByDateToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.fucksByDateToolStripMenuItem.Text = "Список случек\\вязок";
            this.fucksByDateToolStripMenuItem.Click += new System.EventHandler(this.fucksByDateToolStripMenuItem_Click);
            // 
            // miButcher
            // 
            this.miButcher.Name = "miButcher";
            this.miButcher.Size = new System.Drawing.Size(223, 22);
            this.miButcher.Text = "Стерильный цех";
            this.miButcher.Click += new System.EventHandler(this.miButcher_Click);
            // 
            // tsmiOptions
            // 
            this.tsmiOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showTierTMenuItem,
            this.showTierSMenuItem,
            this.shortNamesMenuItem,
            this.shortZooMenuItem,
            this.dblSurMenuItem,
            this.toolStripMenuItem4,
            this.shNumMenuItem,
            this.toolStripMenuItem5,
            this.geterosisMenuItem,
            this.inbreedingMenuItem,
            this.toolStripMenuItem3,
            this.miScale,
            this.paramsMenuItem1});
            this.tsmiOptions.Name = "tsmiOptions";
            this.tsmiOptions.Size = new System.Drawing.Size(79, 20);
            this.tsmiOptions.Text = "Настройки";
            this.tsmiOptions.DropDownOpening += new System.EventHandler(this.tsmiOptions_DropDownOpening);
            // 
            // showTierTMenuItem
            // 
            this.showTierTMenuItem.CheckOnClick = true;
            this.showTierTMenuItem.Name = "showTierTMenuItem";
            this.showTierTMenuItem.Size = new System.Drawing.Size(231, 22);
            this.showTierTMenuItem.Text = "Показывать типы ярусов";
            this.showTierTMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // showTierSMenuItem
            // 
            this.showTierSMenuItem.CheckOnClick = true;
            this.showTierSMenuItem.Name = "showTierSMenuItem";
            this.showTierSMenuItem.Size = new System.Drawing.Size(231, 22);
            this.showTierSMenuItem.Text = "Показывать типы отделений";
            this.showTierSMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // shortNamesMenuItem
            // 
            this.shortNamesMenuItem.CheckOnClick = true;
            this.shortNamesMenuItem.Name = "shortNamesMenuItem";
            this.shortNamesMenuItem.Size = new System.Drawing.Size(231, 22);
            this.shortNamesMenuItem.Text = "Сокращения в таблицах";
            this.shortNamesMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // shortZooMenuItem
            // 
            this.shortZooMenuItem.CheckOnClick = true;
            this.shortZooMenuItem.Name = "shortZooMenuItem";
            this.shortZooMenuItem.Size = new System.Drawing.Size(231, 22);
            this.shortZooMenuItem.Text = "Сокращения в зоотехплане";
            this.shortZooMenuItem.Click += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // dblSurMenuItem
            // 
            this.dblSurMenuItem.CheckOnClick = true;
            this.dblSurMenuItem.Name = "dblSurMenuItem";
            this.dblSurMenuItem.Size = new System.Drawing.Size(231, 22);
            this.dblSurMenuItem.Text = "Двойные фамилии";
            this.dblSurMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(228, 6);
            // 
            // shNumMenuItem
            // 
            this.shNumMenuItem.CheckOnClick = true;
            this.shNumMenuItem.Name = "shNumMenuItem";
            this.shNumMenuItem.Size = new System.Drawing.Size(231, 22);
            this.shNumMenuItem.Text = "Показывать номера";
            this.shNumMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(228, 6);
            // 
            // geterosisMenuItem
            // 
            this.geterosisMenuItem.CheckOnClick = true;
            this.geterosisMenuItem.Name = "geterosisMenuItem";
            this.geterosisMenuItem.Size = new System.Drawing.Size(231, 22);
            this.geterosisMenuItem.Text = "Разрешен гетерозис";
            this.geterosisMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // inbreedingMenuItem
            // 
            this.inbreedingMenuItem.CheckOnClick = true;
            this.inbreedingMenuItem.Name = "inbreedingMenuItem";
            this.inbreedingMenuItem.Size = new System.Drawing.Size(231, 22);
            this.inbreedingMenuItem.Text = "Разрешен инбридинг";
            this.inbreedingMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(228, 6);
            // 
            // miScale
            // 
            this.miScale.Name = "miScale";
            this.miScale.Size = new System.Drawing.Size(231, 22);
            this.miScale.Text = "Весы";
            this.miScale.Click += new System.EventHandler(this.miScale_Click);
            // 
            // paramsMenuItem1
            // 
            this.paramsMenuItem1.Name = "paramsMenuItem1";
            this.paramsMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.paramsMenuItem1.Size = new System.Drawing.Size(231, 22);
            this.paramsMenuItem1.Text = "Параметры ...";
            this.paramsMenuItem1.Click += new System.EventHandler(this.paramsMenuItem1_Click);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAboutPO,
            this.tsmiHelp});
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(65, 20);
            this.tsmiAbout.Text = "Справка";
            // 
            // tsmiAboutPO
            // 
            this.tsmiAboutPO.Name = "tsmiAboutPO";
            this.tsmiAboutPO.Size = new System.Drawing.Size(161, 22);
            this.tsmiAboutPO.Text = "О программе ...";
            this.tsmiAboutPO.Click += new System.EventHandler(this.tsmiAboutPO_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.tsmiHelp.Size = new System.Drawing.Size(161, 22);
            this.tsmiHelp.Text = "Помощь";
            this.tsmiHelp.Click += new System.EventHandler(this.tsmiHelp_Click);
            // 
            // tNoWorking
            // 
            this.tNoWorking.Interval = 480000;
            this.tNoWorking.Tick += new System.EventHandler(this.tNoWorking_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tpButcher);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(914, 24);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(906, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Поголовье";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(906, 0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Молодняк";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(906, 0);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Строения";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 24);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(906, 0);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Зоотехплан";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tpButcher
            // 
            this.tpButcher.Location = new System.Drawing.Point(4, 24);
            this.tpButcher.Name = "tpButcher";
            this.tpButcher.Size = new System.Drawing.Size(906, 0);
            this.tpButcher.TabIndex = 4;
            this.tpButcher.Text = "Стерильный цех";
            this.tpButcher.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(4, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(906, 423);
            this.panel1.TabIndex = 7;
            // 
            // rabStatusBar1
            // 
            this.rabStatusBar1.ExcelButtonClick = null;
            this.rabStatusBar1.filterPanel = null;
            this.rabStatusBar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.rabStatusBar1.Location = new System.Drawing.Point(0, 481);
            this.rabStatusBar1.Name = "rabStatusBar1";
            this.rabStatusBar1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.rabStatusBar1.Size = new System.Drawing.Size(914, 23);
            this.rabStatusBar1.TabIndex = 5;
            this.rabStatusBar1.Text = "rabStatusBar1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 504);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.rabStatusBar1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFarm;
        private System.Windows.Forms.ToolStripMenuItem сменитьФермуToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiView;
        private System.Windows.Forms.ToolStripMenuItem tsmiFilter;
        private System.Windows.Forms.ToolStripMenuItem параметрыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeadsArchive;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem namesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem breedsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiOptions;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStripMenuItem showTierTMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showTierSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shortNamesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dblSurMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem shNumMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem geterosisMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inbreedingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiAboutPO;
        private RabStatusBar rabStatusBar1;
        private System.Windows.Forms.Timer tNoWorking;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem tsmiActions;
        private System.Windows.Forms.ToolStripMenuItem tsmiAreas;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem paramsMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem usersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiReports;
        private System.Windows.Forms.ToolStripMenuItem тестовыйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiBreeds;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeadReasons;
        private System.Windows.Forms.ToolStripMenuItem tsmiFuckProductivity;
        private System.Windows.Forms.ToolStripMenuItem tsmiAgeAndCount;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeadReasonsView;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeadsReaport;
        private System.Windows.Forms.ToolStripMenuItem shortZooMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiFucksByUsers;
        private System.Windows.Forms.ToolStripMenuItem tsmiCountByMonths;
        private System.Windows.Forms.ToolStripMenuItem fucksByDateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiProductTypesView;
        private System.Windows.Forms.TabPage tpButcher;
        private System.Windows.Forms.ToolStripMenuItem miButcher;
        private System.Windows.Forms.ToolStripMenuItem miMeal;
        private System.Windows.Forms.ToolStripMenuItem miScale;
        private System.Windows.Forms.ToolStripMenuItem tsmiVaccines;
        private System.Windows.Forms.ToolStripMenuItem miLogs;
    }
}

