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
            this.фермаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сменитьФермуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.фильтрToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.параметрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.забоиПривесыСписанияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.namesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breedsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.зоныToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.причиныСпичанияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчетыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.тестовыйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.породыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.продуктивностьСоитияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.возрастИКоличествоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списанияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списанияToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.окролыПоПользователямToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.paramsMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rabStatusBar1 = new rabnet.RabStatusBar();
            this.количествоПоМесяцамToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.фермаToolStripMenuItem,
            this.видToolStripMenuItem,
            this.actMenuItem,
            this.отчетыToolStripMenuItem,
            this.настройкиToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(914, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // фермаToolStripMenuItem
            // 
            this.фермаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сменитьФермуToolStripMenuItem,
            this.usersMenuItem,
            this.toolStripMenuItem1,
            this.выходToolStripMenuItem});
            this.фермаToolStripMenuItem.Name = "фермаToolStripMenuItem";
            this.фермаToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.фермаToolStripMenuItem.Text = "Ферма";
            // 
            // сменитьФермуToolStripMenuItem
            // 
            this.сменитьФермуToolStripMenuItem.Name = "сменитьФермуToolStripMenuItem";
            this.сменитьФермуToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.сменитьФермуToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.сменитьФермуToolStripMenuItem.Text = "Сменить ферму/пользователя ...";
            this.сменитьФермуToolStripMenuItem.Click += new System.EventHandler(this.ChangeFarmMenuItem_Click);
            // 
            // usersMenuItem
            // 
            this.usersMenuItem.Name = "usersMenuItem";
            this.usersMenuItem.Size = new System.Drawing.Size(293, 22);
            this.usersMenuItem.Text = "Пользователи";
            this.usersMenuItem.Click += new System.EventHandler(this.usersMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(290, 6);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // видToolStripMenuItem
            // 
            this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.фильтрToolStripMenuItem,
            this.параметрыToolStripMenuItem,
            this.забоиПривесыСписанияToolStripMenuItem,
            this.toolStripMenuItem2,
            this.namesMenuItem,
            this.breedsMenuItem,
            this.зоныToolStripMenuItem,
            this.причиныСпичанияToolStripMenuItem});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.видToolStripMenuItem.Text = "Вид";
            // 
            // фильтрToolStripMenuItem
            // 
            this.фильтрToolStripMenuItem.Name = "фильтрToolStripMenuItem";
            this.фильтрToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.фильтрToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.фильтрToolStripMenuItem.Text = "Фильтр";
            this.фильтрToolStripMenuItem.Click += new System.EventHandler(this.фильтрToolStripMenuItem_Click);
            // 
            // параметрыToolStripMenuItem
            // 
            this.параметрыToolStripMenuItem.Name = "параметрыToolStripMenuItem";
            this.параметрыToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.параметрыToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.параметрыToolStripMenuItem.Text = "Параметры";
            this.параметрыToolStripMenuItem.Click += new System.EventHandler(this.paramsMenuItem1_Click);
            // 
            // забоиПривесыСписанияToolStripMenuItem
            // 
            this.забоиПривесыСписанияToolStripMenuItem.Name = "забоиПривесыСписанияToolStripMenuItem";
            this.забоиПривесыСписанияToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.забоиПривесыСписанияToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.забоиПривесыСписанияToolStripMenuItem.Text = "Списания";
            this.забоиПривесыСписанияToolStripMenuItem.Click += new System.EventHandler(this.забоиПривесыСписанияToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // namesMenuItem
            // 
            this.namesMenuItem.Name = "namesMenuItem";
            this.namesMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.namesMenuItem.Size = new System.Drawing.Size(180, 22);
            this.namesMenuItem.Text = "Имена";
            this.namesMenuItem.Click += new System.EventHandler(this.namesMenuItem_Click);
            // 
            // breedsMenuItem
            // 
            this.breedsMenuItem.Name = "breedsMenuItem";
            this.breedsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.breedsMenuItem.Size = new System.Drawing.Size(180, 22);
            this.breedsMenuItem.Text = "Породы";
            this.breedsMenuItem.Click += new System.EventHandler(this.breedsMenuItem_Click);
            // 
            // зоныToolStripMenuItem
            // 
            this.зоныToolStripMenuItem.Name = "зоныToolStripMenuItem";
            this.зоныToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.зоныToolStripMenuItem.Text = "Зоны";
            this.зоныToolStripMenuItem.Click += new System.EventHandler(this.зоныToolStripMenuItem_Click);
            // 
            // причиныСпичанияToolStripMenuItem
            // 
            this.причиныСпичанияToolStripMenuItem.Name = "причиныСпичанияToolStripMenuItem";
            this.причиныСпичанияToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.причиныСпичанияToolStripMenuItem.Text = "Причины списания";
            this.причиныСпичанияToolStripMenuItem.Click += new System.EventHandler(this.причиныСпичанияToolStripMenuItem_Click);
            // 
            // actMenuItem
            // 
            this.actMenuItem.Name = "actMenuItem";
            this.actMenuItem.Size = new System.Drawing.Size(68, 20);
            this.actMenuItem.Text = "Действия";
            // 
            // отчетыToolStripMenuItem
            // 
            this.отчетыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.тестовыйToolStripMenuItem,
            this.породыToolStripMenuItem,
            this.продуктивностьСоитияToolStripMenuItem,
            this.возрастИКоличествоToolStripMenuItem,
            this.количествоПоМесяцамToolStripMenuItem,
            this.списанияToolStripMenuItem,
            this.списанияToolStripMenuItem1,
            this.окролыПоПользователямToolStripMenuItem});
            this.отчетыToolStripMenuItem.Name = "отчетыToolStripMenuItem";
            this.отчетыToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.отчетыToolStripMenuItem.Text = "Отчеты";
            // 
            // тестовыйToolStripMenuItem
            // 
            this.тестовыйToolStripMenuItem.Name = "тестовыйToolStripMenuItem";
            this.тестовыйToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.тестовыйToolStripMenuItem.Text = "Тестовый";
            this.тестовыйToolStripMenuItem.Visible = false;
            this.тестовыйToolStripMenuItem.Click += new System.EventHandler(this.тестовыйToolStripMenuItem_Click);
            // 
            // породыToolStripMenuItem
            // 
            this.породыToolStripMenuItem.Name = "породыToolStripMenuItem";
            this.породыToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.породыToolStripMenuItem.Text = "Состав пород";
            this.породыToolStripMenuItem.Click += new System.EventHandler(this.породыToolStripMenuItem_Click);
            // 
            // продуктивностьСоитияToolStripMenuItem
            // 
            this.продуктивностьСоитияToolStripMenuItem.Name = "продуктивностьСоитияToolStripMenuItem";
            this.продуктивностьСоитияToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.продуктивностьСоитияToolStripMenuItem.Text = "Продуктивность соития";
            this.продуктивностьСоитияToolStripMenuItem.Click += new System.EventHandler(this.продуктивностьСоитияToolStripMenuItem_Click);
            // 
            // возрастИКоличествоToolStripMenuItem
            // 
            this.возрастИКоличествоToolStripMenuItem.Name = "возрастИКоличествоToolStripMenuItem";
            this.возрастИКоличествоToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.возрастИКоличествоToolStripMenuItem.Text = "Возраст и количество";
            this.возрастИКоличествоToolStripMenuItem.Click += new System.EventHandler(this.возрастИКоличествоToolStripMenuItem_Click);
            // 
            // списанияToolStripMenuItem
            // 
            this.списанияToolStripMenuItem.Name = "списанияToolStripMenuItem";
            this.списанияToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.списанияToolStripMenuItem.Text = "Причины списания";
            this.списанияToolStripMenuItem.Click += new System.EventHandler(this.списанияToolStripMenuItem_Click);
            // 
            // списанияToolStripMenuItem1
            // 
            this.списанияToolStripMenuItem1.Name = "списанияToolStripMenuItem1";
            this.списанияToolStripMenuItem1.Size = new System.Drawing.Size(220, 22);
            this.списанияToolStripMenuItem1.Text = "Списания";
            this.списанияToolStripMenuItem1.Click += new System.EventHandler(this.списанияToolStripMenuItem1_Click);
            // 
            // окролыПоПользователямToolStripMenuItem
            // 
            this.окролыПоПользователямToolStripMenuItem.Name = "окролыПоПользователямToolStripMenuItem";
            this.окролыПоПользователямToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.окролыПоПользователямToolStripMenuItem.Text = "Окролы по пользователям";
            this.окролыПоПользователямToolStripMenuItem.Click += new System.EventHandler(this.окролыПоПользователямToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.paramsMenuItem1});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // showTierTMenuItem
            // 
            this.showTierTMenuItem.CheckOnClick = true;
            this.showTierTMenuItem.Name = "showTierTMenuItem";
            this.showTierTMenuItem.Size = new System.Drawing.Size(228, 22);
            this.showTierTMenuItem.Text = "Показывать типы ярусов";
            this.showTierTMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // showTierSMenuItem
            // 
            this.showTierSMenuItem.CheckOnClick = true;
            this.showTierSMenuItem.Name = "showTierSMenuItem";
            this.showTierSMenuItem.Size = new System.Drawing.Size(228, 22);
            this.showTierSMenuItem.Text = "Пказывать типы отделений";
            this.showTierSMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // shortNamesMenuItem
            // 
            this.shortNamesMenuItem.CheckOnClick = true;
            this.shortNamesMenuItem.Name = "shortNamesMenuItem";
            this.shortNamesMenuItem.Size = new System.Drawing.Size(228, 22);
            this.shortNamesMenuItem.Text = "Сокращения в таблицах";
            this.shortNamesMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // shortZooMenuItem
            // 
            this.shortZooMenuItem.CheckOnClick = true;
            this.shortZooMenuItem.Name = "shortZooMenuItem";
            this.shortZooMenuItem.Size = new System.Drawing.Size(228, 22);
            this.shortZooMenuItem.Text = "Сокращения в зоотехплане";
            this.shortZooMenuItem.Click += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // dblSurMenuItem
            // 
            this.dblSurMenuItem.CheckOnClick = true;
            this.dblSurMenuItem.Name = "dblSurMenuItem";
            this.dblSurMenuItem.Size = new System.Drawing.Size(228, 22);
            this.dblSurMenuItem.Text = "Двойные фамилии";
            this.dblSurMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(225, 6);
            // 
            // shNumMenuItem
            // 
            this.shNumMenuItem.CheckOnClick = true;
            this.shNumMenuItem.Name = "shNumMenuItem";
            this.shNumMenuItem.Size = new System.Drawing.Size(228, 22);
            this.shNumMenuItem.Text = "Показывать номера";
            this.shNumMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(225, 6);
            // 
            // geterosisMenuItem
            // 
            this.geterosisMenuItem.CheckOnClick = true;
            this.geterosisMenuItem.Name = "geterosisMenuItem";
            this.geterosisMenuItem.Size = new System.Drawing.Size(228, 22);
            this.geterosisMenuItem.Text = "Разрешен гетерозис";
            this.geterosisMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // inbreedingMenuItem
            // 
            this.inbreedingMenuItem.CheckOnClick = true;
            this.inbreedingMenuItem.Name = "inbreedingMenuItem";
            this.inbreedingMenuItem.Size = new System.Drawing.Size(228, 22);
            this.inbreedingMenuItem.Text = "Разрешен инбридинг";
            this.inbreedingMenuItem.CheckedChanged += new System.EventHandler(this.showTierTMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(225, 6);
            // 
            // paramsMenuItem1
            // 
            this.paramsMenuItem1.Name = "paramsMenuItem1";
            this.paramsMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.paramsMenuItem1.Size = new System.Drawing.Size(228, 22);
            this.paramsMenuItem1.Text = "Параметры ...";
            this.paramsMenuItem1.Click += new System.EventHandler(this.paramsMenuItem1_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе ...";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(914, 24);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(906, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Поголовье";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(906, 0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Молодняк";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(906, 0);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Строения";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(906, 0);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Зоотехплан";
            this.tabPage4.UseVisualStyleBackColor = true;
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
            this.rabStatusBar1.filterPanel = null;
            this.rabStatusBar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.rabStatusBar1.Location = new System.Drawing.Point(0, 481);
            this.rabStatusBar1.Name = "rabStatusBar1";
            this.rabStatusBar1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.rabStatusBar1.Size = new System.Drawing.Size(914, 23);
            this.rabStatusBar1.TabIndex = 5;
            this.rabStatusBar1.Text = "rabStatusBar1";
            // 
            // количествоПоМесяцамToolStripMenuItem
            // 
            this.количествоПоМесяцамToolStripMenuItem.Name = "количествоПоМесяцамToolStripMenuItem";
            this.количествоПоМесяцамToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.количествоПоМесяцамToolStripMenuItem.Text = "Количество по месяцам";
            this.количествоПоМесяцамToolStripMenuItem.Click += new System.EventHandler(this.количествоПоМесяцамToolStripMenuItem_Click);
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
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem фермаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сменитьФермуToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem фильтрToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem параметрыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem забоиПривесыСписанияToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem namesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem breedsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showTierTMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showTierSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shortNamesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dblSurMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem shNumMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem geterosisMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inbreedingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private RabStatusBar rabStatusBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem actMenuItem;
        private System.Windows.Forms.ToolStripMenuItem зоныToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem paramsMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem usersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отчетыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem тестовыйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem породыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem списанияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem продуктивностьСоитияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem возрастИКоличествоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem причиныСпичанияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem списанияToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem shortZooMenuItem;
        private System.Windows.Forms.ToolStripMenuItem окролыПоПользователямToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem количествоПоМесяцамToolStripMenuItem;
    }
}

