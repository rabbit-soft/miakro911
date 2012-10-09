namespace rabdump
{
    partial class ArchiveJobsPanel
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArchiveJobsPanel));
            this.btDelete = new System.Windows.Forms.Button();
            this.btEdit = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btAdd = new System.Windows.Forms.CheckBox();
            this.cbName = new System.Windows.Forms.ComboBox();
            this.gbAJProperties = new System.Windows.Forms.GroupBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chServerSend = new System.Windows.Forms.CheckBox();
            this.cbWeekDay = new System.Windows.Forms.ComboBox();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.cbArcType = new System.Windows.Forms.ComboBox();
            this.tbDumpPath = new System.Windows.Forms.TextBox();
            this.btDumpPath = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudSizeLimit = new System.Windows.Forms.NumericUpDown();
            this.nudCountLimit = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.cbDataBase = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gbAJProperties.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSizeLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCountLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // btDelete
            // 
            this.btDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDelete.Enabled = false;
            this.btDelete.Image = ((System.Drawing.Image)(resources.GetObject("btDelete.Image")));
            this.btDelete.Location = new System.Drawing.Point(303, 3);
            this.btDelete.Margin = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(30, 30);
            this.btDelete.TabIndex = 11;
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btEdit
            // 
            this.btEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btEdit.Appearance = System.Windows.Forms.Appearance.Button;
            this.btEdit.Enabled = false;
            this.btEdit.Image = ((System.Drawing.Image)(resources.GetObject("btEdit.Image")));
            this.btEdit.Location = new System.Drawing.Point(267, 3);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(30, 30);
            this.btEdit.TabIndex = 10;
            this.btEdit.UseVisualStyleBackColor = true;
            this.btEdit.CheckedChanged += new System.EventHandler(this.btAdd_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Расписание:";
            // 
            // btAdd
            // 
            this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAdd.Appearance = System.Windows.Forms.Appearance.Button;
            this.btAdd.Image = ((System.Drawing.Image)(resources.GetObject("btAdd.Image")));
            this.btAdd.Location = new System.Drawing.Point(231, 3);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(30, 30);
            this.btAdd.TabIndex = 8;
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.CheckedChanged += new System.EventHandler(this.btAdd_CheckedChanged);
            // 
            // cbName
            // 
            this.cbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbName.FormattingEnabled = true;
            this.cbName.Location = new System.Drawing.Point(80, 9);
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(145, 21);
            this.cbName.TabIndex = 7;
            this.cbName.SelectedIndexChanged += new System.EventHandler(this.cbName_SelectedIndexChanged);
            // 
            // gbAJProperties
            // 
            this.gbAJProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gbAJProperties.Controls.Add(this.btCancel);
            this.gbAJProperties.Controls.Add(this.btOk);
            this.gbAJProperties.Controls.Add(this.groupBox1);
            this.gbAJProperties.Controls.Add(this.tbDumpPath);
            this.gbAJProperties.Controls.Add(this.btDumpPath);
            this.gbAJProperties.Controls.Add(this.label7);
            this.gbAJProperties.Controls.Add(this.label5);
            this.gbAJProperties.Controls.Add(this.label4);
            this.gbAJProperties.Controls.Add(this.label3);
            this.gbAJProperties.Controls.Add(this.nudSizeLimit);
            this.gbAJProperties.Controls.Add(this.nudCountLimit);
            this.gbAJProperties.Controls.Add(this.label2);
            this.gbAJProperties.Controls.Add(this.cbDataBase);
            this.gbAJProperties.Controls.Add(this.label6);
            this.gbAJProperties.Enabled = false;
            this.gbAJProperties.Location = new System.Drawing.Point(7, 39);
            this.gbAJProperties.Name = "gbAJProperties";
            this.gbAJProperties.Size = new System.Drawing.Size(327, 299);
            this.gbAJProperties.TabIndex = 12;
            this.gbAJProperties.TabStop = false;
            this.gbAJProperties.Text = "Локальное резервирование";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btCancel.Image = ((System.Drawing.Image)(resources.GetObject("btCancel.Image")));
            this.btCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btCancel.Location = new System.Drawing.Point(85, 268);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 25);
            this.btCancel.TabIndex = 29;
            this.btCancel.Text = "Отмена";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Visible = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOk
            // 
            this.btOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOk.Image = ((System.Drawing.Image)(resources.GetObject("btOk.Image")));
            this.btOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOk.Location = new System.Drawing.Point(166, 268);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 25);
            this.btOk.TabIndex = 28;
            this.btOk.Text = "Готово";
            this.btOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Visible = false;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chServerSend);
            this.groupBox1.Controls.Add(this.cbWeekDay);
            this.groupBox1.Controls.Add(this.dtpTime);
            this.groupBox1.Controls.Add(this.dtpDate);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.cbArcType);
            this.groupBox1.Location = new System.Drawing.Point(6, 147);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 99);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Рассписание";
            // 
            // chServerSend
            // 
            this.chServerSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chServerSend.AutoSize = true;
            this.chServerSend.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chServerSend.Location = new System.Drawing.Point(169, 77);
            this.chServerSend.Name = "chServerSend";
            this.chServerSend.Size = new System.Drawing.Size(140, 17);
            this.chServerSend.TabIndex = 34;
            this.chServerSend.Text = "Отправлять на сервер";
            this.chServerSend.UseVisualStyleBackColor = true;
            // 
            // cbWeekDay
            // 
            this.cbWeekDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWeekDay.FormattingEnabled = true;
            this.cbWeekDay.Items.AddRange(new object[] {
            "Понедельник",
            "Вторник",
            "Среда",
            "Четверг",
            "Пятница",
            "Суббота",
            "Воскресение"});
            this.cbWeekDay.Location = new System.Drawing.Point(6, 45);
            this.cbWeekDay.Name = "cbWeekDay";
            this.cbWeekDay.Size = new System.Drawing.Size(100, 21);
            this.cbWeekDay.TabIndex = 33;
            this.toolTip1.SetToolTip(this.cbWeekDay, "День недели");
            this.cbWeekDay.SelectedIndexChanged += new System.EventHandler(this.cbWeekDay_SelectedIndexChanged);
            // 
            // dtpTime
            // 
            this.dtpTime.CustomFormat = "HH:mm";
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTime.Location = new System.Drawing.Point(255, 46);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            this.dtpTime.Size = new System.Drawing.Size(54, 20);
            this.dtpTime.TabIndex = 32;
            this.toolTip1.SetToolTip(this.dtpTime, "Время резервирования");
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(134, 46);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(101, 20);
            this.dtpDate.TabIndex = 28;
            this.toolTip1.SetToolTip(this.dtpDate, "Дата резервирования");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Запускать:";
            // 
            // cbArcType
            // 
            this.cbArcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbArcType.FormattingEnabled = true;
            this.cbArcType.Location = new System.Drawing.Point(86, 19);
            this.cbArcType.Name = "cbArcType";
            this.cbArcType.Size = new System.Drawing.Size(127, 21);
            this.cbArcType.TabIndex = 27;
            this.cbArcType.SelectedIndexChanged += new System.EventHandler(this.cbArcType_SelectedIndexChanged);
            // 
            // tbDumpPath
            // 
            this.tbDumpPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDumpPath.Location = new System.Drawing.Point(17, 69);
            this.tbDumpPath.Name = "tbDumpPath";
            this.tbDumpPath.Size = new System.Drawing.Size(269, 20);
            this.tbDumpPath.TabIndex = 20;
            // 
            // btDumpPath
            // 
            this.btDumpPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDumpPath.Location = new System.Drawing.Point(293, 67);
            this.btDumpPath.Name = "btDumpPath";
            this.btDumpPath.Size = new System.Drawing.Size(28, 23);
            this.btDumpPath.TabIndex = 21;
            this.btDumpPath.Text = "...";
            this.btDumpPath.UseVisualStyleBackColor = true;
            this.btDumpPath.Click += new System.EventHandler(this.btDumpPath_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(138, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Сохранять РКБД в папке:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(281, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "МБайт";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(277, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "файлов";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Ограничение общего размера РКБД:";
            // 
            // nudSizeLimit
            // 
            this.nudSizeLimit.Location = new System.Drawing.Point(220, 121);
            this.nudSizeLimit.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.nudSizeLimit.Name = "nudSizeLimit";
            this.nudSizeLimit.Size = new System.Drawing.Size(50, 20);
            this.nudSizeLimit.TabIndex = 15;
            this.toolTip1.SetToolTip(this.nudSizeLimit, "Общий размер всех РКБД в папке не должен превышать указанного размера\r\n");
            // 
            // nudCountLimit
            // 
            this.nudCountLimit.Location = new System.Drawing.Point(220, 95);
            this.nudCountLimit.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.nudCountLimit.Name = "nudCountLimit";
            this.nudCountLimit.Size = new System.Drawing.Size(50, 20);
            this.nudCountLimit.TabIndex = 14;
            this.toolTip1.SetToolTip(this.nudCountLimit, "Общее количество всех РКБД в папке не должен превышать указанного количества");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Ограничение общ. количества РКБД:";
            // 
            // cbDataBase
            // 
            this.cbDataBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDataBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataBase.FormattingEnabled = true;
            this.cbDataBase.Location = new System.Drawing.Point(67, 19);
            this.cbDataBase.Name = "cbDataBase";
            this.cbDataBase.Size = new System.Drawing.Size(200, 21);
            this.cbDataBase.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Ферма:";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 5000;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // ArchiveJobsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbAJProperties);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.btEdit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.cbName);
            this.MinimumSize = new System.Drawing.Size(340, 315);
            this.Name = "ArchiveJobsPanel";
            this.Size = new System.Drawing.Size(340, 341);
            this.gbAJProperties.ResumeLayout(false);
            this.gbAJProperties.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSizeLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCountLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.CheckBox btEdit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox btAdd;
        private System.Windows.Forms.ComboBox cbName;
        private System.Windows.Forms.GroupBox gbAJProperties;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudSizeLimit;
        private System.Windows.Forms.NumericUpDown nudCountLimit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbDataBase;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox tbDumpPath;
        private System.Windows.Forms.Button btDumpPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cbArcType;
        private System.Windows.Forms.ComboBox cbWeekDay;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.CheckBox chServerSend;
        private System.Windows.Forms.Button btCancel;
    }
}
