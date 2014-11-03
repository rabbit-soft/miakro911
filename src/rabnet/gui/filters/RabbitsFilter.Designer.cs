namespace rabnet.filters
{
    partial class RabbitsFilter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nudCountTo = new System.Windows.Forms.NumericUpDown();
            this.cbCount = new System.Windows.Forms.ComboBox();
            this.nudCountFrom = new System.Windows.Forms.NumericUpDown();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.cobBreeds = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.nudPregFrom = new System.Windows.Forms.NumericUpDown();
            this.dtpPregTo = new System.Windows.Forms.DateTimePicker();
            this.cbPregTo = new System.Windows.Forms.CheckBox();
            this.nudPregTo = new System.Windows.Forms.NumericUpDown();
            this.dtpPregFrom = new System.Windows.Forms.DateTimePicker();
            this.cbPregFrom = new System.Windows.Forms.CheckBox();
            this.cobPregnant = new System.Windows.Forms.ComboBox();
            this.cbFemaleState = new System.Windows.Forms.CheckBox();
            this.cbFemaleFirst = new System.Windows.Forms.CheckBox();
            this.cbFemaleBride = new System.Windows.Forms.CheckBox();
            this.cbFemaleGirl = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbMaleProducer = new System.Windows.Forms.CheckBox();
            this.cbMaleCandidate = new System.Windows.Forms.CheckBox();
            this.cbMaleBoy = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nudWeightTo = new System.Windows.Forms.NumericUpDown();
            this.nudWeightFrom = new System.Windows.Forms.NumericUpDown();
            this.cbWeightTo = new System.Windows.Forms.CheckBox();
            this.cbWeightFrom = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nudDateFrom = new System.Windows.Forms.NumericUpDown();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.cbDateFrom = new System.Windows.Forms.CheckBox();
            this.nudDateTo = new System.Windows.Forms.NumericUpDown();
            this.cbDateTo = new System.Windows.Forms.CheckBox();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSexVoid = new System.Windows.Forms.CheckBox();
            this.cbSexFemale = new System.Windows.Forms.CheckBox();
            this.cbSexMale = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCountTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCountFrom)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPregFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPregTo)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWeightTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWeightFrom)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDateFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDateTo)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.nudCountTo);
            this.groupBox4.Controls.Add(this.cbCount);
            this.groupBox4.Controls.Add(this.nudCountFrom);
            this.groupBox4.Location = new System.Drawing.Point(395, 32);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(115, 74);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Количество";
            // 
            // nudCountTo
            // 
            this.nudCountTo.Enabled = false;
            this.nudCountTo.Location = new System.Drawing.Point(67, 39);
            this.nudCountTo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCountTo.Name = "nudCountTo";
            this.nudCountTo.Size = new System.Drawing.Size(42, 20);
            this.nudCountTo.TabIndex = 2;
            this.nudCountTo.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // cbCount
            // 
            this.cbCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCount.FormattingEnabled = true;
            this.cbCount.Items.AddRange(new object[] {
            ">=",
            "=",
            "<=",
            "<",
            "A..Z"});
            this.cbCount.Location = new System.Drawing.Point(6, 13);
            this.cbCount.Name = "cbCount";
            this.cbCount.Size = new System.Drawing.Size(55, 21);
            this.cbCount.TabIndex = 1;
            this.cbCount.SelectedIndexChanged += new System.EventHandler(this.cbCount_SelectedIndexChanged);
            // 
            // nudCountFrom
            // 
            this.nudCountFrom.Location = new System.Drawing.Point(67, 14);
            this.nudCountFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCountFrom.Name = "nudCountFrom";
            this.nudCountFrom.Size = new System.Drawing.Size(42, 20);
            this.nudCountFrom.TabIndex = 0;
            this.nudCountFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label6);
            this.groupBox9.Controls.Add(this.tbName);
            this.groupBox9.Location = new System.Drawing.Point(240, -2);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(230, 34);
            this.groupBox9.TabIndex = 20;
            this.groupBox9.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(6, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Содержит строку";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(110, 10);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(110, 20);
            this.tbName.TabIndex = 12;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.cobBreeds);
            this.groupBox8.Controls.Add(this.label7);
            this.groupBox8.Location = new System.Drawing.Point(3, -2);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(223, 34);
            this.groupBox8.TabIndex = 20;
            this.groupBox8.TabStop = false;
            // 
            // cobBreeds
            // 
            this.cobBreeds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobBreeds.FormattingEnabled = true;
            this.cobBreeds.Items.AddRange(new object[] {
            "-=ВСЕ=-"});
            this.cobBreeds.Location = new System.Drawing.Point(60, 10);
            this.cobBreeds.MaxDropDownItems = 12;
            this.cobBreeds.Name = "cobBreeds";
            this.cobBreeds.Size = new System.Drawing.Size(150, 21);
            this.cobBreeds.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(8, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Порода";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(482, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(30, 23);
            this.button3.TabIndex = 18;
            this.button3.Text = "X";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(429, 217);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // cbFilter
            // 
            this.cbFilter.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cbFilter.FormattingEnabled = true;
            this.cbFilter.Location = new System.Drawing.Point(323, 218);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(100, 21);
            this.cbFilter.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(270, 221);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Фильтр";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.Controls.Add(this.cbFemaleState);
            this.groupBox6.Controls.Add(this.cbFemaleFirst);
            this.groupBox6.Controls.Add(this.cbFemaleBride);
            this.groupBox6.Controls.Add(this.cbFemaleGirl);
            this.groupBox6.Location = new System.Drawing.Point(144, 112);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(366, 100);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Статус самки";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.nudPregFrom);
            this.groupBox7.Controls.Add(this.dtpPregTo);
            this.groupBox7.Controls.Add(this.cbPregTo);
            this.groupBox7.Controls.Add(this.nudPregTo);
            this.groupBox7.Controls.Add(this.dtpPregFrom);
            this.groupBox7.Controls.Add(this.cbPregFrom);
            this.groupBox7.Controls.Add(this.cobPregnant);
            this.groupBox7.Location = new System.Drawing.Point(108, 12);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(254, 78);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Сукрольность";
            // 
            // nudPregFrom
            // 
            this.nudPregFrom.Enabled = false;
            this.nudPregFrom.Location = new System.Drawing.Point(210, 42);
            this.nudPregFrom.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nudPregFrom.Name = "nudPregFrom";
            this.nudPregFrom.Size = new System.Drawing.Size(38, 20);
            this.nudPregFrom.TabIndex = 10;
            this.nudPregFrom.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudPregFrom.ValueChanged += new System.EventHandler(this.nudPregFrom_ValueChanged);
            // 
            // dtpPregTo
            // 
            this.dtpPregTo.Enabled = false;
            this.dtpPregTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPregTo.Location = new System.Drawing.Point(126, 16);
            this.dtpPregTo.Name = "dtpPregTo";
            this.dtpPregTo.Size = new System.Drawing.Size(79, 20);
            this.dtpPregTo.TabIndex = 9;
            this.dtpPregTo.ValueChanged += new System.EventHandler(this.dtpPregTo_ValueChanged);
            // 
            // cbPregTo
            // 
            this.cbPregTo.AutoSize = true;
            this.cbPregTo.Location = new System.Drawing.Point(79, 19);
            this.cbPregTo.Name = "cbPregTo";
            this.cbPregTo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbPregTo.Size = new System.Drawing.Size(41, 17);
            this.cbPregTo.TabIndex = 8;
            this.cbPregTo.Text = "До";
            this.cbPregTo.UseVisualStyleBackColor = true;
            this.cbPregTo.CheckedChanged += new System.EventHandler(this.cbPregTo_CheckedChanged);
            // 
            // nudPregTo
            // 
            this.nudPregTo.Enabled = false;
            this.nudPregTo.Location = new System.Drawing.Point(210, 16);
            this.nudPregTo.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nudPregTo.Name = "nudPregTo";
            this.nudPregTo.Size = new System.Drawing.Size(38, 20);
            this.nudPregTo.TabIndex = 5;
            this.nudPregTo.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudPregTo.ValueChanged += new System.EventHandler(this.nudPregTo_ValueChanged);
            // 
            // dtpPregFrom
            // 
            this.dtpPregFrom.Enabled = false;
            this.dtpPregFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPregFrom.Location = new System.Drawing.Point(126, 42);
            this.dtpPregFrom.Name = "dtpPregFrom";
            this.dtpPregFrom.Size = new System.Drawing.Size(79, 20);
            this.dtpPregFrom.TabIndex = 3;
            this.dtpPregFrom.ValueChanged += new System.EventHandler(this.dtpPregFrom_ValueChanged);
            // 
            // cbPregFrom
            // 
            this.cbPregFrom.AutoSize = true;
            this.cbPregFrom.Location = new System.Drawing.Point(81, 44);
            this.cbPregFrom.Name = "cbPregFrom";
            this.cbPregFrom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbPregFrom.Size = new System.Drawing.Size(39, 17);
            this.cbPregFrom.TabIndex = 1;
            this.cbPregFrom.Tag = "";
            this.cbPregFrom.Text = "От";
            this.cbPregFrom.UseVisualStyleBackColor = true;
            this.cbPregFrom.CheckedChanged += new System.EventHandler(this.cbPregFrom_CheckedChanged);
            // 
            // cobPregnant
            // 
            this.cobPregnant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobPregnant.FormattingEnabled = true;
            this.cobPregnant.Items.AddRange(new object[] {
            "Неважно",
            "Нет",
            "Да"});
            this.cobPregnant.Location = new System.Drawing.Point(6, 30);
            this.cobPregnant.Name = "cobPregnant";
            this.cobPregnant.Size = new System.Drawing.Size(70, 21);
            this.cobPregnant.TabIndex = 0;
            this.cobPregnant.SelectedIndexChanged += new System.EventHandler(this.cobPregnant_SelectedIndexChanged);
            // 
            // cbFemaleState
            // 
            this.cbFemaleState.AutoSize = true;
            this.cbFemaleState.Location = new System.Drawing.Point(6, 73);
            this.cbFemaleState.Name = "cbFemaleState";
            this.cbFemaleState.Size = new System.Drawing.Size(71, 17);
            this.cbFemaleState.TabIndex = 3;
            this.cbFemaleState.Text = "Штатные";
            this.cbFemaleState.UseVisualStyleBackColor = true;
            this.cbFemaleState.CheckedChanged += new System.EventHandler(this.cbFemaleState_CheckedChanged);
            // 
            // cbFemaleFirst
            // 
            this.cbFemaleFirst.AutoSize = true;
            this.cbFemaleFirst.Location = new System.Drawing.Point(6, 55);
            this.cbFemaleFirst.Name = "cbFemaleFirst";
            this.cbFemaleFirst.Size = new System.Drawing.Size(94, 17);
            this.cbFemaleFirst.TabIndex = 2;
            this.cbFemaleFirst.Text = "Первокролки";
            this.cbFemaleFirst.UseVisualStyleBackColor = true;
            this.cbFemaleFirst.CheckedChanged += new System.EventHandler(this.cbFemaleFirst_CheckedChanged);
            // 
            // cbFemaleBride
            // 
            this.cbFemaleBride.AutoSize = true;
            this.cbFemaleBride.Location = new System.Drawing.Point(6, 37);
            this.cbFemaleBride.Name = "cbFemaleBride";
            this.cbFemaleBride.Size = new System.Drawing.Size(71, 17);
            this.cbFemaleBride.TabIndex = 1;
            this.cbFemaleBride.Text = "Невесты";
            this.cbFemaleBride.UseVisualStyleBackColor = true;
            this.cbFemaleBride.CheckedChanged += new System.EventHandler(this.cbFemaleBride_CheckedChanged);
            // 
            // cbFemaleGirl
            // 
            this.cbFemaleGirl.AutoSize = true;
            this.cbFemaleGirl.Location = new System.Drawing.Point(6, 19);
            this.cbFemaleGirl.Name = "cbFemaleGirl";
            this.cbFemaleGirl.Size = new System.Drawing.Size(70, 17);
            this.cbFemaleGirl.TabIndex = 0;
            this.cbFemaleGirl.Text = "Девочки";
            this.cbFemaleGirl.UseVisualStyleBackColor = true;
            this.cbFemaleGirl.CheckedChanged += new System.EventHandler(this.cbFemaleGirl_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbMaleProducer);
            this.groupBox5.Controls.Add(this.cbMaleCandidate);
            this.groupBox5.Controls.Add(this.cbMaleBoy);
            this.groupBox5.Location = new System.Drawing.Point(3, 112);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(139, 100);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Статус самца";
            // 
            // cbMaleProducer
            // 
            this.cbMaleProducer.AutoSize = true;
            this.cbMaleProducer.Location = new System.Drawing.Point(11, 55);
            this.cbMaleProducer.Name = "cbMaleProducer";
            this.cbMaleProducer.Size = new System.Drawing.Size(105, 17);
            this.cbMaleProducer.TabIndex = 2;
            this.cbMaleProducer.Text = "Производители";
            this.cbMaleProducer.UseVisualStyleBackColor = true;
            this.cbMaleProducer.CheckedChanged += new System.EventHandler(this.cbMaleProducer_CheckedChanged);
            // 
            // cbMaleCandidate
            // 
            this.cbMaleCandidate.AutoSize = true;
            this.cbMaleCandidate.Location = new System.Drawing.Point(11, 37);
            this.cbMaleCandidate.Name = "cbMaleCandidate";
            this.cbMaleCandidate.Size = new System.Drawing.Size(82, 17);
            this.cbMaleCandidate.TabIndex = 1;
            this.cbMaleCandidate.Text = "Кандидаты";
            this.cbMaleCandidate.UseVisualStyleBackColor = true;
            this.cbMaleCandidate.CheckedChanged += new System.EventHandler(this.cbMaleCandidate_CheckedChanged);
            // 
            // cbMaleBoy
            // 
            this.cbMaleBoy.AutoSize = true;
            this.cbMaleBoy.Location = new System.Drawing.Point(11, 19);
            this.cbMaleBoy.Name = "cbMaleBoy";
            this.cbMaleBoy.Size = new System.Drawing.Size(76, 17);
            this.cbMaleBoy.TabIndex = 0;
            this.cbMaleBoy.Text = "Мальчики";
            this.cbMaleBoy.UseVisualStyleBackColor = true;
            this.cbMaleBoy.CheckedChanged += new System.EventHandler(this.cbMaleBoy_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudWeightTo);
            this.groupBox3.Controls.Add(this.nudWeightFrom);
            this.groupBox3.Controls.Add(this.cbWeightTo);
            this.groupBox3.Controls.Add(this.cbWeightFrom);
            this.groupBox3.Location = new System.Drawing.Point(283, 31);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(106, 75);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Вес";
            // 
            // nudWeightTo
            // 
            this.nudWeightTo.Enabled = false;
            this.nudWeightTo.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudWeightTo.Location = new System.Drawing.Point(50, 15);
            this.nudWeightTo.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.nudWeightTo.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudWeightTo.Name = "nudWeightTo";
            this.nudWeightTo.Size = new System.Drawing.Size(50, 20);
            this.nudWeightTo.TabIndex = 3;
            this.nudWeightTo.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudWeightTo.ValueChanged += new System.EventHandler(this.nudWeightTo_ValueChanged);
            // 
            // nudWeightFrom
            // 
            this.nudWeightFrom.Enabled = false;
            this.nudWeightFrom.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudWeightFrom.Location = new System.Drawing.Point(50, 40);
            this.nudWeightFrom.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.nudWeightFrom.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudWeightFrom.Name = "nudWeightFrom";
            this.nudWeightFrom.Size = new System.Drawing.Size(50, 20);
            this.nudWeightFrom.TabIndex = 2;
            this.nudWeightFrom.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudWeightFrom.ValueChanged += new System.EventHandler(this.nudWeightFrom_ValueChanged);
            // 
            // cbWeightTo
            // 
            this.cbWeightTo.AutoSize = true;
            this.cbWeightTo.Location = new System.Drawing.Point(3, 18);
            this.cbWeightTo.Name = "cbWeightTo";
            this.cbWeightTo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbWeightTo.Size = new System.Drawing.Size(41, 17);
            this.cbWeightTo.TabIndex = 1;
            this.cbWeightTo.Text = "До";
            this.cbWeightTo.UseVisualStyleBackColor = true;
            this.cbWeightTo.CheckedChanged += new System.EventHandler(this.cbWeightTo_CheckedChanged);
            // 
            // cbWeightFrom
            // 
            this.cbWeightFrom.AutoSize = true;
            this.cbWeightFrom.Location = new System.Drawing.Point(5, 43);
            this.cbWeightFrom.Name = "cbWeightFrom";
            this.cbWeightFrom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbWeightFrom.Size = new System.Drawing.Size(39, 17);
            this.cbWeightFrom.TabIndex = 0;
            this.cbWeightFrom.Text = "От";
            this.cbWeightFrom.UseVisualStyleBackColor = true;
            this.cbWeightFrom.CheckedChanged += new System.EventHandler(this.cbWeightFrom_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nudDateFrom);
            this.groupBox2.Controls.Add(this.dtpDateFrom);
            this.groupBox2.Controls.Add(this.cbDateFrom);
            this.groupBox2.Controls.Add(this.nudDateTo);
            this.groupBox2.Controls.Add(this.cbDateTo);
            this.groupBox2.Controls.Add(this.dtpDateTo);
            this.groupBox2.Location = new System.Drawing.Point(97, 31);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(185, 75);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Дата рождения/Возраст";
            // 
            // nudDateFrom
            // 
            this.nudDateFrom.Enabled = false;
            this.nudDateFrom.Location = new System.Drawing.Point(135, 40);
            this.nudDateFrom.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudDateFrom.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudDateFrom.Name = "nudDateFrom";
            this.nudDateFrom.Size = new System.Drawing.Size(44, 20);
            this.nudDateFrom.TabIndex = 2;
            this.toolTip1.SetToolTip(this.nudDateFrom, "Старше ... дней");
            this.nudDateFrom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudDateFrom.ValueChanged += new System.EventHandler(this.nudDateFrom_ValueChanged);
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.Enabled = false;
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateFrom.Location = new System.Drawing.Point(50, 40);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(79, 20);
            this.dtpDateFrom.TabIndex = 4;
            this.toolTip1.SetToolTip(this.dtpDateFrom, "Дата рождения не раньше чем...");
            this.dtpDateFrom.ValueChanged += new System.EventHandler(this.dtpDateFrom_ValueChanged);
            // 
            // cbDateFrom
            // 
            this.cbDateFrom.AutoSize = true;
            this.cbDateFrom.Location = new System.Drawing.Point(5, 43);
            this.cbDateFrom.Name = "cbDateFrom";
            this.cbDateFrom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbDateFrom.Size = new System.Drawing.Size(39, 17);
            this.cbDateFrom.TabIndex = 3;
            this.cbDateFrom.Text = "От";
            this.toolTip1.SetToolTip(this.cbDateFrom, "Дата рождения не раньше чем...");
            this.cbDateFrom.UseVisualStyleBackColor = true;
            this.cbDateFrom.CheckedChanged += new System.EventHandler(this.cbDateFrom_CheckedChanged);
            // 
            // nudDateTo
            // 
            this.nudDateTo.Enabled = false;
            this.nudDateTo.Location = new System.Drawing.Point(135, 15);
            this.nudDateTo.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudDateTo.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudDateTo.Name = "nudDateTo";
            this.nudDateTo.Size = new System.Drawing.Size(44, 20);
            this.nudDateTo.TabIndex = 5;
            this.toolTip1.SetToolTip(this.nudDateTo, "Младше ... дней");
            this.nudDateTo.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudDateTo.ValueChanged += new System.EventHandler(this.nudDateTo_ValueChanged);
            // 
            // cbDateTo
            // 
            this.cbDateTo.AutoSize = true;
            this.cbDateTo.Location = new System.Drawing.Point(3, 18);
            this.cbDateTo.Name = "cbDateTo";
            this.cbDateTo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbDateTo.Size = new System.Drawing.Size(41, 17);
            this.cbDateTo.TabIndex = 1;
            this.cbDateTo.Text = "До";
            this.cbDateTo.UseVisualStyleBackColor = true;
            this.cbDateTo.CheckedChanged += new System.EventHandler(this.cbDateTo_CheckedChanged);
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Enabled = false;
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateTo.Location = new System.Drawing.Point(50, 15);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(79, 20);
            this.dtpDateTo.TabIndex = 0;
            this.toolTip1.SetToolTip(this.dtpDateTo, "Дата рождения не позже чем...");
            this.dtpDateTo.ValueChanged += new System.EventHandler(this.dtpDateTo_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbSexVoid);
            this.groupBox1.Controls.Add(this.cbSexFemale);
            this.groupBox1.Controls.Add(this.cbSexMale);
            this.groupBox1.Location = new System.Drawing.Point(3, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(93, 75);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Пол";
            // 
            // cbSexVoid
            // 
            this.cbSexVoid.AutoSize = true;
            this.cbSexVoid.Location = new System.Drawing.Point(11, 52);
            this.cbSexVoid.Name = "cbSexVoid";
            this.cbSexVoid.Size = new System.Drawing.Size(77, 17);
            this.cbSexVoid.TabIndex = 2;
            this.cbSexVoid.Text = "Бесполые";
            this.cbSexVoid.UseVisualStyleBackColor = true;
            this.cbSexVoid.CheckedChanged += new System.EventHandler(this.cbSexVoid_CheckedChanged);
            // 
            // cbSexFemale
            // 
            this.cbSexFemale.AutoSize = true;
            this.cbSexFemale.Location = new System.Drawing.Point(11, 35);
            this.cbSexFemale.Name = "cbSexFemale";
            this.cbSexFemale.Size = new System.Drawing.Size(59, 17);
            this.cbSexFemale.TabIndex = 1;
            this.cbSexFemale.Text = "Самки";
            this.cbSexFemale.UseVisualStyleBackColor = true;
            this.cbSexFemale.CheckedChanged += new System.EventHandler(this.cbSexFemale_CheckedChanged);
            // 
            // cbSexMale
            // 
            this.cbSexMale.AutoSize = true;
            this.cbSexMale.Location = new System.Drawing.Point(11, 18);
            this.cbSexMale.Name = "cbSexMale";
            this.cbSexMale.Size = new System.Drawing.Size(61, 17);
            this.cbSexMale.TabIndex = 0;
            this.cbSexMale.Text = "Самцы";
            this.cbSexMale.UseVisualStyleBackColor = true;
            this.cbSexMale.CheckedChanged += new System.EventHandler(this.cbSexMale_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.Location = new System.Drawing.Point(3, 217);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(57, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Готово";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // RabbitsFilter
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbFilter);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.FilterCombo = this.cbFilter;
            this.HideBtn = this.button2;
            this.Name = "RabbitsFilter";
            this.SaveButton = this.button1;
            this.Size = new System.Drawing.Size(515, 247);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudCountTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCountFrom)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPregFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPregTo)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWeightTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWeightFrom)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDateFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDateTo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.NumericUpDown nudPregTo;
        private System.Windows.Forms.DateTimePicker dtpPregFrom;
        private System.Windows.Forms.CheckBox cbPregFrom;
        private System.Windows.Forms.ComboBox cobPregnant;
        private System.Windows.Forms.CheckBox cbFemaleState;
        private System.Windows.Forms.CheckBox cbFemaleFirst;
        private System.Windows.Forms.CheckBox cbFemaleBride;
        private System.Windows.Forms.CheckBox cbFemaleGirl;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cbMaleProducer;
        private System.Windows.Forms.CheckBox cbMaleCandidate;
        private System.Windows.Forms.CheckBox cbMaleBoy;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nudWeightTo;
        private System.Windows.Forms.NumericUpDown nudWeightFrom;
        private System.Windows.Forms.CheckBox cbWeightTo;
        private System.Windows.Forms.CheckBox cbWeightFrom;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nudDateFrom;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.CheckBox cbDateFrom;
        private System.Windows.Forms.NumericUpDown nudDateTo;
        private System.Windows.Forms.CheckBox cbDateTo;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbSexVoid;
        private System.Windows.Forms.CheckBox cbSexFemale;
        private System.Windows.Forms.CheckBox cbSexMale;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown nudPregFrom;
        private System.Windows.Forms.DateTimePicker dtpPregTo;
        private System.Windows.Forms.CheckBox cbPregTo;
        private System.Windows.Forms.ComboBox cobBreeds;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown nudCountTo;
        private System.Windows.Forms.ComboBox cbCount;
        private System.Windows.Forms.NumericUpDown nudCountFrom;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}
