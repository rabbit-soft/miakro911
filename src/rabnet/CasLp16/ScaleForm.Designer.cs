namespace CAS
{
    partial class ScaleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScaleForm));
            this.listView1 = new System.Windows.Forms.ListView();
            this.chPLUID = new System.Windows.Forms.ColumnHeader();
            this.chPLUCode = new System.Windows.Forms.ColumnHeader();
            this.chPLUProdName1 = new System.Windows.Forms.ColumnHeader();
            this.chPLUProdName2 = new System.Windows.Forms.ColumnHeader();
            this.срPLUPrice = new System.Windows.Forms.ColumnHeader();
            this.chPLULiveTime = new System.Windows.Forms.ColumnHeader();
            this.chPLUTara = new System.Windows.Forms.ColumnHeader();
            this.chPLUGroupCode = new System.Windows.Forms.ColumnHeader();
            this.chPLUMessage = new System.Windows.Forms.ColumnHeader();
            this.cmPLU = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miPLUChange = new System.Windows.Forms.ToolStripMenuItem();
            this.miPLUadd = new System.Windows.Forms.ToolStripMenuItem();
            this.miPLUdel = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lvMSG = new System.Windows.Forms.ListView();
            this.chMSGid = new System.Windows.Forms.ColumnHeader();
            this.chMSGtext = new System.Windows.Forms.ColumnHeader();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tbSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.tslbScaleMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.tScaleMessageClear = new System.Windows.Forms.Timer(this.components);
            this.tLoadFromScaleChecker = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tpOptions = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbMSGsu = new System.Windows.Forms.TextBox();
            this.tbMSGsf = new System.Windows.Forms.TextBox();
            this.btTest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chMonitor = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.nudScanFreq = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.gbPLUScan = new System.Windows.Forms.GroupBox();
            this.btAllPLUn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbSU = new System.Windows.Forms.TextBox();
            this.tbSF = new System.Windows.Forms.TextBox();
            this.btOptionsSave = new System.Windows.Forms.Button();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmMSG = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miMsgAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.miMsgDel = new System.Windows.Forms.ToolStripMenuItem();
            this.cmPLU.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tpOptions.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScanFreq)).BeginInit();
            this.gbPLUScan.SuspendLayout();
            this.gbConnection.SuspendLayout();
            this.cmMSG.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPLUID,
            this.chPLUCode,
            this.chPLUProdName1,
            this.chPLUProdName2,
            this.срPLUPrice,
            this.chPLULiveTime,
            this.chPLUTara,
            this.chPLUGroupCode,
            this.chPLUMessage});
            this.listView1.ContextMenuStrip = this.cmPLU;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(786, 185);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // chPLUID
            // 
            this.chPLUID.Text = "№";
            this.chPLUID.Width = 54;
            // 
            // chPLUCode
            // 
            this.chPLUCode.Text = "Код";
            this.chPLUCode.Width = 66;
            // 
            // chPLUProdName1
            // 
            this.chPLUProdName1.Text = "Название1";
            this.chPLUProdName1.Width = 108;
            // 
            // chPLUProdName2
            // 
            this.chPLUProdName2.Text = "Название2";
            this.chPLUProdName2.Width = 111;
            // 
            // срPLUPrice
            // 
            this.срPLUPrice.Text = "Цена (за КГ)";
            this.срPLUPrice.Width = 79;
            // 
            // chPLULiveTime
            // 
            this.chPLULiveTime.Text = "Срок годности";
            this.chPLULiveTime.Width = 96;
            // 
            // chPLUTara
            // 
            this.chPLUTara.Text = "Тара (гр)";
            this.chPLUTara.Width = 64;
            // 
            // chPLUGroupCode
            // 
            this.chPLUGroupCode.Text = "Групповой Код";
            this.chPLUGroupCode.Width = 99;
            // 
            // chPLUMessage
            // 
            this.chPLUMessage.Text = "Сообщение (№)";
            this.chPLUMessage.Width = 96;
            // 
            // cmPLU
            // 
            this.cmPLU.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miPLUChange,
            this.miPLUadd,
            this.miPLUdel});
            this.cmPLU.Name = "cmPLU";
            this.cmPLU.Size = new System.Drawing.Size(136, 70);
            this.cmPLU.Opening += new System.ComponentModel.CancelEventHandler(this.cmPLU_Opening);
            // 
            // miPLUChange
            // 
            this.miPLUChange.Name = "miPLUChange";
            this.miPLUChange.Size = new System.Drawing.Size(135, 22);
            this.miPLUChange.Text = "Изменить";
            this.miPLUChange.Click += new System.EventHandler(this.pluModification);
            // 
            // miPLUadd
            // 
            this.miPLUadd.Name = "miPLUadd";
            this.miPLUadd.Size = new System.Drawing.Size(135, 22);
            this.miPLUadd.Text = "Добавить";
            this.miPLUadd.Click += new System.EventHandler(this.pluModification);
            // 
            // miPLUdel
            // 
            this.miPLUdel.Name = "miPLUdel";
            this.miPLUdel.Size = new System.Drawing.Size(135, 22);
            this.miPLUdel.Text = "Удалить";
            this.miPLUdel.Click += new System.EventHandler(this.pluModification);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(786, 453);
            this.splitContainer1.SplitterDistance = 208;
            this.splitContainer1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(786, 185);
            this.panel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(786, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Записи продукции в весах";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 21);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lvMSG);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tbMessage);
            this.splitContainer2.Panel2MinSize = 0;
            this.splitContainer2.Size = new System.Drawing.Size(786, 220);
            this.splitContainer2.SplitterDistance = 466;
            this.splitContainer2.TabIndex = 2;
            // 
            // lvMSG
            // 
            this.lvMSG.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chMSGid,
            this.chMSGtext});
            this.lvMSG.ContextMenuStrip = this.cmMSG;
            this.lvMSG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMSG.FullRowSelect = true;
            this.lvMSG.GridLines = true;
            this.lvMSG.Location = new System.Drawing.Point(0, 0);
            this.lvMSG.MultiSelect = false;
            this.lvMSG.Name = "lvMSG";
            this.lvMSG.Size = new System.Drawing.Size(466, 220);
            this.lvMSG.TabIndex = 0;
            this.lvMSG.UseCompatibleStateImageBehavior = false;
            this.lvMSG.View = System.Windows.Forms.View.Details;
            this.lvMSG.SelectedIndexChanged += new System.EventHandler(this.lvMSG_SelectedIndexChanged);
            // 
            // chMSGid
            // 
            this.chMSGid.Text = "№";
            // 
            // chMSGtext
            // 
            this.chMSGtext.Text = "Текст Сообщения";
            this.chMSGtext.Width = 400;
            // 
            // tbMessage
            // 
            this.tbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMessage.Location = new System.Drawing.Point(0, 0);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(316, 220);
            this.tbMessage.TabIndex = 0;
            this.tbMessage.TextChanged += new System.EventHandler(this.tbMessage_TextChanged);
            this.tbMessage.Leave += new System.EventHandler(this.tbMessage_Leave);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(786, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Сообщения";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbRefresh,
            this.tbSave,
            this.toolStripProgressBar1,
            this.tslbScaleMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 485);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tbRefresh
            // 
            this.tbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tbRefresh.Image")));
            this.tbRefresh.Name = "tbRefresh";
            this.tbRefresh.Size = new System.Drawing.Size(77, 20);
            this.tbRefresh.Text = "Обновить";
            this.tbRefresh.Click += new System.EventHandler(this.RefreshData);
            // 
            // tbSave
            // 
            this.tbSave.Image = ((System.Drawing.Image)(resources.GetObject("tbSave.Image")));
            this.tbSave.Name = "tbSave";
            this.tbSave.Size = new System.Drawing.Size(82, 20);
            this.tbSave.Text = "Сохранить";
            this.tbSave.Click += new System.EventHandler(this.tbSave_Click);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBar1.Visible = false;
            // 
            // tslbScaleMessage
            // 
            this.tslbScaleMessage.Name = "tslbScaleMessage";
            this.tslbScaleMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // tScaleMessageClear
            // 
            this.tScaleMessageClear.Interval = 15000;
            this.tScaleMessageClear.Tick += new System.EventHandler(this.tScaleMessageClear_Tick);
            // 
            // tLoadFromScaleChecker
            // 
            this.tLoadFromScaleChecker.Interval = 1000;
            this.tLoadFromScaleChecker.Tick += new System.EventHandler(this.tLoadFromScaleChecker_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tpOptions);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 485);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 459);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Основное";
            // 
            // tpOptions
            // 
            this.tpOptions.Controls.Add(this.groupBox2);
            this.tpOptions.Controls.Add(this.btTest);
            this.tpOptions.Controls.Add(this.groupBox1);
            this.tpOptions.Controls.Add(this.gbPLUScan);
            this.tpOptions.Controls.Add(this.btOptionsSave);
            this.tpOptions.Controls.Add(this.gbConnection);
            this.tpOptions.Location = new System.Drawing.Point(4, 22);
            this.tpOptions.Name = "tpOptions";
            this.tpOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tpOptions.Size = new System.Drawing.Size(792, 459);
            this.tpOptions.TabIndex = 1;
            this.tpOptions.Text = "Настройки";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.tbMSGsu);
            this.groupBox2.Controls.Add(this.tbMSGsf);
            this.groupBox2.Location = new System.Drawing.Point(8, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(273, 63);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Загружать Сообщения";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(200, 25);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(58, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "ВСЕ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(101, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "По";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "С";
            // 
            // tbMSGsu
            // 
            this.tbMSGsu.Location = new System.Drawing.Point(128, 27);
            this.tbMSGsu.MaxLength = 4;
            this.tbMSGsu.Name = "tbMSGsu";
            this.tbMSGsu.Size = new System.Drawing.Size(62, 20);
            this.tbMSGsu.TabIndex = 0;
            this.tbMSGsu.TextChanged += new System.EventHandler(this.checkOptions);
            // 
            // tbMSGsf
            // 
            this.tbMSGsf.Location = new System.Drawing.Point(26, 27);
            this.tbMSGsf.MaxLength = 4;
            this.tbMSGsf.Name = "tbMSGsf";
            this.tbMSGsf.Size = new System.Drawing.Size(62, 20);
            this.tbMSGsf.TabIndex = 0;
            this.tbMSGsf.TextChanged += new System.EventHandler(this.checkOptions);
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(287, 29);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(75, 23);
            this.btTest.TabIndex = 4;
            this.btTest.Text = "Не жать";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chMonitor);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.nudScanFreq);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(8, 208);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 100);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Интервал между сканированиями";
            // 
            // chMonitor
            // 
            this.chMonitor.AutoSize = true;
            this.chMonitor.Location = new System.Drawing.Point(9, 25);
            this.chMonitor.Name = "chMonitor";
            this.chMonitor.Size = new System.Drawing.Size(45, 17);
            this.chMonitor.TabIndex = 4;
            this.chMonitor.Text = "Вкл";
            this.chMonitor.UseVisualStyleBackColor = true;
            this.chMonitor.CheckedChanged += new System.EventHandler(this.chMonitor_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(239, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "сек.";
            // 
            // nudScanFreq
            // 
            this.nudScanFreq.Location = new System.Drawing.Point(178, 24);
            this.nudScanFreq.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudScanFreq.Name = "nudScanFreq";
            this.nudScanFreq.Size = new System.Drawing.Size(55, 20);
            this.nudScanFreq.TabIndex = 2;
            this.nudScanFreq.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(76, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "новая продукция:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gbPLUScan
            // 
            this.gbPLUScan.Controls.Add(this.btAllPLUn);
            this.gbPLUScan.Controls.Add(this.label6);
            this.gbPLUScan.Controls.Add(this.label5);
            this.gbPLUScan.Controls.Add(this.tbSU);
            this.gbPLUScan.Controls.Add(this.tbSF);
            this.gbPLUScan.Location = new System.Drawing.Point(8, 70);
            this.gbPLUScan.Name = "gbPLUScan";
            this.gbPLUScan.Size = new System.Drawing.Size(273, 63);
            this.gbPLUScan.TabIndex = 2;
            this.gbPLUScan.TabStop = false;
            this.gbPLUScan.Text = "Загружать продукцию";
            // 
            // btAllPLUn
            // 
            this.btAllPLUn.Location = new System.Drawing.Point(200, 25);
            this.btAllPLUn.Name = "btAllPLUn";
            this.btAllPLUn.Size = new System.Drawing.Size(58, 23);
            this.btAllPLUn.TabIndex = 2;
            this.btAllPLUn.Text = "ВСЕ";
            this.btAllPLUn.UseVisualStyleBackColor = true;
            this.btAllPLUn.Click += new System.EventHandler(this.btAllPLUn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(101, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "По";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "С";
            // 
            // tbSU
            // 
            this.tbSU.Location = new System.Drawing.Point(128, 27);
            this.tbSU.MaxLength = 4;
            this.tbSU.Name = "tbSU";
            this.tbSU.Size = new System.Drawing.Size(62, 20);
            this.tbSU.TabIndex = 0;
            this.tbSU.TextChanged += new System.EventHandler(this.checkOptions);
            // 
            // tbSF
            // 
            this.tbSF.Location = new System.Drawing.Point(26, 27);
            this.tbSF.MaxLength = 4;
            this.tbSF.Name = "tbSF";
            this.tbSF.Size = new System.Drawing.Size(62, 20);
            this.tbSF.TabIndex = 0;
            this.tbSF.TextChanged += new System.EventHandler(this.checkOptions);
            // 
            // btOptionsSave
            // 
            this.btOptionsSave.Location = new System.Drawing.Point(101, 430);
            this.btOptionsSave.Name = "btOptionsSave";
            this.btOptionsSave.Size = new System.Drawing.Size(75, 23);
            this.btOptionsSave.TabIndex = 1;
            this.btOptionsSave.Text = "Сохранить";
            this.btOptionsSave.UseVisualStyleBackColor = true;
            this.btOptionsSave.Click += new System.EventHandler(this.btOptionsSave_Click);
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.tbPort);
            this.gbConnection.Controls.Add(this.tbAddress);
            this.gbConnection.Controls.Add(this.label4);
            this.gbConnection.Controls.Add(this.label3);
            this.gbConnection.Location = new System.Drawing.Point(8, 6);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Size = new System.Drawing.Size(273, 58);
            this.gbConnection.TabIndex = 0;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Подключение";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(200, 25);
            this.tbPort.MaxLength = 6;
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(58, 20);
            this.tbPort.TabIndex = 1;
            this.tbPort.TextChanged += new System.EventHandler(this.checkOptions);
            // 
            // tbAddress
            // 
            this.tbAddress.Location = new System.Drawing.Point(53, 25);
            this.tbAddress.MaxLength = 20;
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(100, 20);
            this.tbAddress.TabIndex = 1;
            this.tbAddress.TextChanged += new System.EventHandler(this.checkOptions);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Адрес:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(159, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Порт:";
            // 
            // cmMSG
            // 
            this.cmMSG.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMsgAdd,
            this.miMsgDel});
            this.cmMSG.Name = "cmMSG";
            this.cmMSG.Size = new System.Drawing.Size(136, 48);
            this.cmMSG.Opening += new System.ComponentModel.CancelEventHandler(this.cmMSG_Opening);
            // 
            // miMsgAdd
            // 
            this.miMsgAdd.Name = "miMsgAdd";
            this.miMsgAdd.Size = new System.Drawing.Size(152, 22);
            this.miMsgAdd.Text = "Добавить";
            this.miMsgAdd.Click += new System.EventHandler(this.miMsgAdd_Click);
            // 
            // miMsgDel
            // 
            this.miMsgDel.Name = "miMsgDel";
            this.miMsgDel.Size = new System.Drawing.Size(152, 22);
            this.miMsgDel.Text = "Удалить";
            this.miMsgDel.Visible = false;
            // 
            // ScaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 507);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScaleForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Продукция Весов";
            this.Load += new System.EventHandler(this.ScaleForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ScaleForm_FormClosed);
            this.cmPLU.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tpOptions.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScanFreq)).EndInit();
            this.gbPLUScan.ResumeLayout(false);
            this.gbPLUScan.PerformLayout();
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            this.cmMSG.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader chPLUID;
        private System.Windows.Forms.ColumnHeader chPLUCode;
        private System.Windows.Forms.ColumnHeader chPLUProdName1;
        private System.Windows.Forms.ColumnHeader chPLUProdName2;
        private System.Windows.Forms.ColumnHeader срPLUPrice;
        private System.Windows.Forms.ColumnHeader chPLULiveTime;
        private System.Windows.Forms.ColumnHeader chPLUTara;
        private System.Windows.Forms.ColumnHeader chPLUGroupCode;
        private System.Windows.Forms.ColumnHeader chPLUMessage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView lvMSG;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader chMSGid;
        private System.Windows.Forms.ColumnHeader chMSGtext;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton tbRefresh;
        private System.Windows.Forms.ToolStripStatusLabel tslbScaleMessage;
        private System.Windows.Forms.ToolStripButton tbSave;
        private System.Windows.Forms.Timer tScaleMessageClear;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Timer tLoadFromScaleChecker;
        private System.Windows.Forms.ContextMenuStrip cmPLU;
        private System.Windows.Forms.ToolStripMenuItem miPLUChange;
        private System.Windows.Forms.ToolStripMenuItem miPLUadd;
        private System.Windows.Forms.ToolStripMenuItem miPLUdel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tpOptions;
        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btOptionsSave;
        private System.Windows.Forms.GroupBox gbPLUScan;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbSF;
        private System.Windows.Forms.Button btAllPLUn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbSU;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudScanFreq;
        private System.Windows.Forms.Button btTest;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbMSGsu;
        private System.Windows.Forms.TextBox tbMSGsf;
        private System.Windows.Forms.CheckBox chMonitor;
        private System.Windows.Forms.ContextMenuStrip cmMSG;
        private System.Windows.Forms.ToolStripMenuItem miMsgAdd;
        private System.Windows.Forms.ToolStripMenuItem miMsgDel;
    }
}