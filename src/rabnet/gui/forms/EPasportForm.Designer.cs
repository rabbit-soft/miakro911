namespace rabnet.forms
{
    partial class EPasportForm
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
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lFile = new System.Windows.Forms.Label();
            this.tbFileFrom = new System.Windows.Forms.TextBox();
            this.btOpenFile = new System.Windows.Forms.Button();
            this.lvExportRabbits = new System.Windows.Forms.ListView();
            this.chrName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chrBreed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chrSex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chrAge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chrGroup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chrExportCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chrNewRabName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chrNewRabAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cbFreeBuildings = new System.Windows.Forms.ComboBox();
            this.lReplace = new System.Windows.Forms.Label();
            this.cbNewName = new System.Windows.Forms.ComboBox();
            this.lNewName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lCount = new System.Windows.Forms.Label();
            this.nudExportCnt = new System.Windows.Forms.NumericUpDown();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lvAscendants = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.lvNames = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.cbBreedAnalog = new System.Windows.Forms.ComboBox();
            this.lBreedAnalog = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lvBreeds = new System.Windows.Forms.ListView();
            this.chrBreedName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chrLocalBreedAnalog = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lExists = new System.Windows.Forms.Label();
            this.lExistIDNotMatch = new System.Windows.Forms.Label();
            this.lNotMatchInUse = new System.Windows.Forms.Label();
            this.lNotExists = new System.Windows.Forms.Label();
            this.lMatchInUse = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExportCnt)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Location = new System.Drawing.Point(728, 727);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "ОК";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(647, 727);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Электронный паспорт кролика| *.erabp";
            // 
            // lFile
            // 
            this.lFile.Location = new System.Drawing.Point(12, 11);
            this.lFile.Name = "lFile";
            this.lFile.Size = new System.Drawing.Size(100, 23);
            this.lFile.TabIndex = 2;
            this.lFile.Text = "Файл кролика:";
            this.lFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFileFrom
            // 
            this.tbFileFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFileFrom.Location = new System.Drawing.Point(118, 14);
            this.tbFileFrom.Name = "tbFileFrom";
            this.tbFileFrom.Size = new System.Drawing.Size(650, 20);
            this.tbFileFrom.TabIndex = 3;
            // 
            // btOpenFile
            // 
            this.btOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOpenFile.Location = new System.Drawing.Point(774, 12);
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(29, 23);
            this.btOpenFile.TabIndex = 4;
            this.btOpenFile.Text = "...";
            this.btOpenFile.UseVisualStyleBackColor = true;
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // lvExportRabbits
            // 
            this.lvExportRabbits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvExportRabbits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chrName,
            this.chrBreed,
            this.chrSex,
            this.chrAge,
            this.chrGroup,
            this.chrExportCount,
            this.chrNewRabName,
            this.chrNewRabAddress});
            this.lvExportRabbits.FullRowSelect = true;
            this.lvExportRabbits.GridLines = true;
            this.lvExportRabbits.Location = new System.Drawing.Point(6, 26);
            this.lvExportRabbits.MultiSelect = false;
            this.lvExportRabbits.Name = "lvExportRabbits";
            this.lvExportRabbits.Size = new System.Drawing.Size(693, 218);
            this.lvExportRabbits.TabIndex = 5;
            this.lvExportRabbits.UseCompatibleStateImageBehavior = false;
            this.lvExportRabbits.View = System.Windows.Forms.View.Details;
            this.lvExportRabbits.SelectedIndexChanged += new System.EventHandler(this.lvExportRabbits_SelectedIndexChanged);
            // 
            // chrName
            // 
            this.chrName.Text = "Имя";
            this.chrName.Width = 133;
            // 
            // chrBreed
            // 
            this.chrBreed.Text = "Порода";
            this.chrBreed.Width = 113;
            // 
            // chrSex
            // 
            this.chrSex.Text = "Пол";
            // 
            // chrAge
            // 
            this.chrAge.Text = "Возраст";
            this.chrAge.Width = 61;
            // 
            // chrGroup
            // 
            this.chrGroup.Text = "Количество";
            this.chrGroup.Width = 85;
            // 
            // chrExportCount
            // 
            this.chrExportCount.Text = "На экспорт";
            this.chrExportCount.Width = 81;
            // 
            // chrNewRabName
            // 
            this.chrNewRabName.Text = "Назначить имя";
            this.chrNewRabName.Width = 101;
            // 
            // chrNewRabAddress
            // 
            this.chrNewRabAddress.Text = "Адрес";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 40);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cbFreeBuildings);
            this.splitContainer1.Panel1.Controls.Add(this.lReplace);
            this.splitContainer1.Panel1.Controls.Add(this.cbNewName);
            this.splitContainer1.Panel1.Controls.Add(this.lNewName);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.lCount);
            this.splitContainer1.Panel1.Controls.Add(this.nudExportCnt);
            this.splitContainer1.Panel1.Controls.Add(this.lvExportRabbits);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(791, 681);
            this.splitContainer1.SplitterDistance = 249;
            this.splitContainer1.TabIndex = 6;
            // 
            // cbFreeBuildings
            // 
            this.cbFreeBuildings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFreeBuildings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFreeBuildings.Enabled = false;
            this.cbFreeBuildings.FormattingEnabled = true;
            this.cbFreeBuildings.Location = new System.Drawing.Point(705, 133);
            this.cbFreeBuildings.Name = "cbFreeBuildings";
            this.cbFreeBuildings.Size = new System.Drawing.Size(83, 21);
            this.cbFreeBuildings.TabIndex = 12;
            this.cbFreeBuildings.SelectedIndexChanged += new System.EventHandler(this.cbFreeBuildings_SelectedIndexChanged);
            // 
            // lReplace
            // 
            this.lReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lReplace.AutoSize = true;
            this.lReplace.Location = new System.Drawing.Point(702, 117);
            this.lReplace.Name = "lReplace";
            this.lReplace.Size = new System.Drawing.Size(68, 13);
            this.lReplace.TabIndex = 11;
            this.lReplace.Text = "Поселить в:";
            this.lReplace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbNewName
            // 
            this.cbNewName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbNewName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNewName.Enabled = false;
            this.cbNewName.FormattingEnabled = true;
            this.cbNewName.Location = new System.Drawing.Point(705, 93);
            this.cbNewName.Name = "cbNewName";
            this.cbNewName.Size = new System.Drawing.Size(83, 21);
            this.cbNewName.TabIndex = 10;
            this.cbNewName.SelectedIndexChanged += new System.EventHandler(this.cbNewName_SelectedIndexChanged);
            // 
            // lNewName
            // 
            this.lNewName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lNewName.AutoSize = true;
            this.lNewName.Location = new System.Drawing.Point(702, 77);
            this.lNewName.Name = "lNewName";
            this.lNewName.Size = new System.Drawing.Size(87, 13);
            this.lNewName.TabIndex = 9;
            this.lNewName.Text = "Назначить имя:";
            this.lNewName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Экспорта в Поголовье";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lCount
            // 
            this.lCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lCount.AutoSize = true;
            this.lCount.Location = new System.Drawing.Point(702, 28);
            this.lCount.Name = "lCount";
            this.lCount.Size = new System.Drawing.Size(68, 13);
            this.lCount.TabIndex = 7;
            this.lCount.Text = "На экспорт:";
            this.lCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudExportCnt
            // 
            this.nudExportCnt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudExportCnt.Enabled = false;
            this.nudExportCnt.Location = new System.Drawing.Point(718, 44);
            this.nudExportCnt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudExportCnt.Name = "nudExportCnt";
            this.nudExportCnt.Size = new System.Drawing.Size(70, 20);
            this.nudExportCnt.TabIndex = 6;
            this.nudExportCnt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudExportCnt.ValueChanged += new System.EventHandler(this.nudExportCnt_ValueChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lvAscendants);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(791, 428);
            this.splitContainer2.SplitterDistance = 419;
            this.splitContainer2.TabIndex = 10;
            // 
            // lvAscendants
            // 
            this.lvAscendants.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAscendants.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvAscendants.FullRowSelect = true;
            this.lvAscendants.GridLines = true;
            this.lvAscendants.Location = new System.Drawing.Point(3, 26);
            this.lvAscendants.MultiSelect = false;
            this.lvAscendants.Name = "lvAscendants";
            this.lvAscendants.Size = new System.Drawing.Size(413, 399);
            this.lvAscendants.TabIndex = 6;
            this.lvAscendants.UseCompatibleStateImageBehavior = false;
            this.lvAscendants.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Имя";
            this.columnHeader1.Width = 203;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Порода";
            this.columnHeader2.Width = 119;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Пол";
            this.columnHeader3.Width = 42;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(196, 23);
            this.label4.TabIndex = 9;
            this.label4.Text = "Экспорта в Информацию о предках";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.lvNames);
            this.splitContainer3.Panel1.Controls.Add(this.label5);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.cbBreedAnalog);
            this.splitContainer3.Panel2.Controls.Add(this.lBreedAnalog);
            this.splitContainer3.Panel2.Controls.Add(this.label1);
            this.splitContainer3.Panel2.Controls.Add(this.lvBreeds);
            this.splitContainer3.Size = new System.Drawing.Size(368, 428);
            this.splitContainer3.SplitterDistance = 193;
            this.splitContainer3.TabIndex = 16;
            // 
            // lvNames
            // 
            this.lvNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvNames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader7});
            this.lvNames.FullRowSelect = true;
            this.lvNames.GridLines = true;
            this.lvNames.Location = new System.Drawing.Point(0, 26);
            this.lvNames.MultiSelect = false;
            this.lvNames.Name = "lvNames";
            this.lvNames.Size = new System.Drawing.Size(365, 164);
            this.lvNames.TabIndex = 17;
            this.lvNames.UseCompatibleStateImageBehavior = false;
            this.lvNames.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Имя";
            this.columnHeader4.Width = 144;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Фамилия";
            this.columnHeader5.Width = 133;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Пол";
            this.columnHeader7.Width = 43;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(-3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(196, 23);
            this.label5.TabIndex = 16;
            this.label5.Text = "Экспорт в Имена";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbBreedAnalog
            // 
            this.cbBreedAnalog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbBreedAnalog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBreedAnalog.Enabled = false;
            this.cbBreedAnalog.FormattingEnabled = true;
            this.cbBreedAnalog.Location = new System.Drawing.Point(117, 207);
            this.cbBreedAnalog.Name = "cbBreedAnalog";
            this.cbBreedAnalog.Size = new System.Drawing.Size(170, 21);
            this.cbBreedAnalog.TabIndex = 13;
            this.cbBreedAnalog.SelectedIndexChanged += new System.EventHandler(this.cbBreedAnalog_SelectedIndexChanged);
            // 
            // lBreedAnalog
            // 
            this.lBreedAnalog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lBreedAnalog.Location = new System.Drawing.Point(0, 207);
            this.lBreedAnalog.Name = "lBreedAnalog";
            this.lBreedAnalog.Size = new System.Drawing.Size(111, 21);
            this.lBreedAnalog.TabIndex = 12;
            this.lBreedAnalog.Text = "Локальный аналог:";
            this.lBreedAnalog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "Экспорт в Породы";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvBreeds
            // 
            this.lvBreeds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvBreeds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chrBreedName,
            this.chrLocalBreedAnalog});
            this.lvBreeds.FullRowSelect = true;
            this.lvBreeds.GridLines = true;
            this.lvBreeds.Location = new System.Drawing.Point(0, 26);
            this.lvBreeds.MultiSelect = false;
            this.lvBreeds.Name = "lvBreeds";
            this.lvBreeds.Size = new System.Drawing.Size(365, 172);
            this.lvBreeds.TabIndex = 11;
            this.lvBreeds.UseCompatibleStateImageBehavior = false;
            this.lvBreeds.View = System.Windows.Forms.View.Details;
            this.lvBreeds.SelectedIndexChanged += new System.EventHandler(this.lvBreeds_SelectedIndexChanged);
            // 
            // chrBreedName
            // 
            this.chrBreedName.Text = "Название породы";
            this.chrBreedName.Width = 203;
            // 
            // chrLocalBreedAnalog
            // 
            this.chrLocalBreedAnalog.Text = "Локальный аналог";
            this.chrLocalBreedAnalog.Width = 153;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Электронный паспорт кролика| *.erabp";
            // 
            // lExists
            // 
            this.lExists.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lExists.AutoSize = true;
            this.lExists.Location = new System.Drawing.Point(12, 724);
            this.lExists.Name = "lExists";
            this.lExists.Size = new System.Drawing.Size(160, 13);
            this.lExists.TabIndex = 7;
            this.lExists.Text = "Имеется в базе ID совпадают";
            // 
            // lExistIDNotMatch
            // 
            this.lExistIDNotMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lExistIDNotMatch.AutoSize = true;
            this.lExistIDNotMatch.Location = new System.Drawing.Point(12, 740);
            this.lExistIDNotMatch.Name = "lExistIDNotMatch";
            this.lExistIDNotMatch.Size = new System.Drawing.Size(175, 13);
            this.lExistIDNotMatch.TabIndex = 8;
            this.lExistIDNotMatch.Text = "Имеется в базе ID не совпадают";
            // 
            // lNotMatchInUse
            // 
            this.lNotMatchInUse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lNotMatchInUse.AutoSize = true;
            this.lNotMatchInUse.Location = new System.Drawing.Point(193, 740);
            this.lNotMatchInUse.Name = "lNotMatchInUse";
            this.lNotMatchInUse.Size = new System.Drawing.Size(253, 13);
            this.lNotMatchInUse.TabIndex = 9;
            this.lNotMatchInUse.Text = "Имеется в базе ID не совпадают, Используется";
            // 
            // lNotExists
            // 
            this.lNotExists.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lNotExists.AutoSize = true;
            this.lNotExists.Location = new System.Drawing.Point(437, 724);
            this.lNotExists.Name = "lNotExists";
            this.lNotExists.Size = new System.Drawing.Size(119, 13);
            this.lNotExists.TabIndex = 10;
            this.lNotExists.Text = "Не существует в базе";
            // 
            // lMatchInUse
            // 
            this.lMatchInUse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lMatchInUse.AutoSize = true;
            this.lMatchInUse.Location = new System.Drawing.Point(193, 724);
            this.lMatchInUse.Name = "lMatchInUse";
            this.lMatchInUse.Size = new System.Drawing.Size(238, 13);
            this.lMatchInUse.TabIndex = 11;
            this.lMatchInUse.Text = "Имеется в базе ID совпадают, Используется";
            // 
            // EPasportForm
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(815, 762);
            this.Controls.Add(this.lMatchInUse);
            this.Controls.Add(this.lNotExists);
            this.Controls.Add(this.lNotMatchInUse);
            this.Controls.Add(this.lExistIDNotMatch);
            this.Controls.Add(this.lExists);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btOpenFile);
            this.Controls.Add(this.tbFileFrom);
            this.Controls.Add(this.lFile);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EPasportForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Электронный паспорт кролика";
            this.Load += new System.EventHandler(this.EPasportForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudExportCnt)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lFile;
        private System.Windows.Forms.TextBox tbFileFrom;
        private System.Windows.Forms.Button btOpenFile;
        private System.Windows.Forms.ListView lvExportRabbits;
        private System.Windows.Forms.ColumnHeader chrName;
        private System.Windows.Forms.ColumnHeader chrBreed;
        private System.Windows.Forms.ColumnHeader chrAge;
        private System.Windows.Forms.ColumnHeader chrGroup;
        private System.Windows.Forms.ColumnHeader chrExportCount;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.NumericUpDown nudExportCnt;
        private System.Windows.Forms.Label lCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView lvAscendants;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColumnHeader chrSex;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView lvBreeds;
        private System.Windows.Forms.ColumnHeader chrBreedName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ListView lvNames;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox cbBreedAnalog;
        private System.Windows.Forms.Label lBreedAnalog;
        private System.Windows.Forms.Label lExists;
        private System.Windows.Forms.Label lExistIDNotMatch;
        private System.Windows.Forms.Label lNotMatchInUse;
        private System.Windows.Forms.Label lNotExists;
        private System.Windows.Forms.Label lMatchInUse;
        private System.Windows.Forms.ComboBox cbNewName;
        private System.Windows.Forms.Label lNewName;
        private System.Windows.Forms.ComboBox cbFreeBuildings;
        private System.Windows.Forms.Label lReplace;
        private System.Windows.Forms.ColumnHeader chrNewRabName;
        private System.Windows.Forms.ColumnHeader chrNewRabAddress;
        private System.Windows.Forms.ColumnHeader chrLocalBreedAnalog;
    }
}