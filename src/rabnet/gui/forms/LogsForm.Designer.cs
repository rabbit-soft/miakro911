namespace rabnet
{
    partial class LogsForm
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
            this.lvLogs = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudLogLim = new System.Windows.Forms.NumericUpDown();
            this.btSearch = new System.Windows.Forms.Button();
            this.tbRabID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btShowTypes = new System.Windows.Forms.Button();
            this.cbAddress = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.chPeriod = new System.Windows.Forms.CheckBox();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lgTypes = new rabnet.LogsCheckBoxList();
            this.gbSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLim)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvLogs
            // 
            this.lvLogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader15,
            this.columnHeader14});
            this.lvLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLogs.ForeColor = System.Drawing.Color.Black;
            this.lvLogs.FullRowSelect = true;
            this.lvLogs.GridLines = true;
            this.lvLogs.HideSelection = false;
            this.lvLogs.Location = new System.Drawing.Point(0, 0);
            this.lvLogs.Name = "lvLogs";
            this.lvLogs.Size = new System.Drawing.Size(866, 476);
            this.lvLogs.TabIndex = 1;
            this.lvLogs.UseCompatibleStateImageBehavior = false;
            this.lvLogs.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Дата";
            this.columnHeader11.Width = 73;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Событие";
            this.columnHeader12.Width = 105;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Адрес";
            this.columnHeader13.Width = 75;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Параметры";
            this.columnHeader15.Width = 203;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Пользователь";
            this.columnHeader14.Width = 120;
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.label5);
            this.gbSearch.Controls.Add(this.label4);
            this.gbSearch.Controls.Add(this.dtpDateTo);
            this.gbSearch.Controls.Add(this.chPeriod);
            this.gbSearch.Controls.Add(this.dtpDateFrom);
            this.gbSearch.Controls.Add(this.label3);
            this.gbSearch.Controls.Add(this.cbAddress);
            this.gbSearch.Controls.Add(this.label2);
            this.gbSearch.Controls.Add(this.nudLogLim);
            this.gbSearch.Controls.Add(this.btSearch);
            this.gbSearch.Controls.Add(this.tbRabID);
            this.gbSearch.Controls.Add(this.label1);
            this.gbSearch.Location = new System.Drawing.Point(12, 12);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(730, 68);
            this.gbSearch.TabIndex = 2;
            this.gbSearch.TabStop = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(540, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "Показать записей";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudLogLim
            // 
            this.nudLogLim.Location = new System.Drawing.Point(649, 16);
            this.nudLogLim.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudLogLim.Name = "nudLogLim";
            this.nudLogLim.Size = new System.Drawing.Size(75, 20);
            this.nudLogLim.TabIndex = 16;
            this.nudLogLim.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // btSearch
            // 
            this.btSearch.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btSearch.Location = new System.Drawing.Point(649, 41);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(75, 21);
            this.btSearch.TabIndex = 3;
            this.btSearch.Text = "Искать";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // tbRabID
            // 
            this.tbRabID.Location = new System.Drawing.Point(75, 16);
            this.tbRabID.MaxLength = 10;
            this.tbRabID.Name = "tbRabID";
            this.tbRabID.Size = new System.Drawing.Size(77, 20);
            this.tbRabID.TabIndex = 3;
            this.tbRabID.TextChanged += new System.EventHandler(this.tbID_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "ID кролика";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 86);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvLogs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lgTypes);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(866, 476);
            this.splitContainer1.SplitterDistance = 656;
            this.splitContainer1.TabIndex = 4;
            // 
            // btShowTypes
            // 
            this.btShowTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btShowTypes.Location = new System.Drawing.Point(748, 57);
            this.btShowTypes.Name = "btShowTypes";
            this.btShowTypes.Size = new System.Drawing.Size(130, 23);
            this.btShowTypes.TabIndex = 6;
            this.btShowTypes.Tag = "0";
            this.btShowTypes.Text = "Показать типы лого";
            this.btShowTypes.UseVisualStyleBackColor = true;
            this.btShowTypes.Click += new System.EventHandler(this.btShowTypes_Click);
            // 
            // cbAddress
            // 
            this.cbAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAddress.FormattingEnabled = true;
            this.cbAddress.Location = new System.Drawing.Point(209, 16);
            this.cbAddress.Name = "cbAddress";
            this.cbAddress.Size = new System.Drawing.Size(93, 21);
            this.cbAddress.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(158, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 20);
            this.label3.TabIndex = 19;
            this.label3.Text = "Адрес";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.dtpDateFrom.Location = new System.Drawing.Point(130, 42);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(141, 20);
            this.dtpDateFrom.TabIndex = 20;
            // 
            // chPeriod
            // 
            this.chPeriod.AutoSize = true;
            this.chPeriod.Checked = true;
            this.chPeriod.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chPeriod.Location = new System.Drawing.Point(9, 44);
            this.chPeriod.Name = "chPeriod";
            this.chPeriod.Size = new System.Drawing.Size(64, 17);
            this.chPeriod.TabIndex = 22;
            this.chPeriod.Text = "Период";
            this.chPeriod.UseVisualStyleBackColor = true;
            this.chPeriod.CheckedChanged += new System.EventHandler(this.chPeriod_CheckedChanged);
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.dtpDateTo.Location = new System.Drawing.Point(328, 42);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(141, 20);
            this.dtpDateTo.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(79, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 20);
            this.label4.TabIndex = 24;
            this.label4.Text = "с даты";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(277, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 20);
            this.label5.TabIndex = 25;
            this.label5.Text = "по дату";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lgTypes
            // 
            this.lgTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lgTypes.Location = new System.Drawing.Point(0, 0);
            this.lgTypes.Margin = new System.Windows.Forms.Padding(0);
            this.lgTypes.MinimumSize = new System.Drawing.Size(100, 100);
            this.lgTypes.Name = "lgTypes";
            this.lgTypes.Size = new System.Drawing.Size(100, 100);
            this.lgTypes.TabIndex = 0;
            // 
            // LogsForm
            // 
            this.AcceptButton = this.btSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 574);
            this.Controls.Add(this.btShowTypes);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.gbSearch);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Поиск в Логах";
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLim)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvLogs;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.TextBox tbRabID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private rabnet.LogsCheckBoxList lgTypes;
        private System.Windows.Forms.NumericUpDown nudLogLim;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btShowTypes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbAddress;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.CheckBox chPeriod;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
    }
}