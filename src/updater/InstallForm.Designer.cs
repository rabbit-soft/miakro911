﻿namespace updater
{
    partial class InstallForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallForm));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbJustExit = new System.Windows.Forms.RadioButton();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.btFile = new System.Windows.Forms.Button();
            this.tbComp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbImportFromMia = new System.Windows.Forms.RadioButton();
            this.rbRemoteDb = new System.Windows.Forms.RadioButton();
            this.rbMakeNew = new System.Windows.Forms.RadioButton();
            this.btCheck = new System.Windows.Forms.Button();
            this.btExtended = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbRPwd = new System.Windows.Forms.TextBox();
            this.tbRoot = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbPwd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbDb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(91, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(341, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Для завершения установки необходимо создать ферму";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbJustExit);
            this.groupBox1.Controls.Add(this.tbFile);
            this.groupBox1.Controls.Add(this.btFile);
            this.groupBox1.Controls.Add(this.tbComp);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.rbImportFromMia);
            this.groupBox1.Controls.Add(this.rbRemoteDb);
            this.groupBox1.Controls.Add(this.rbMakeNew);
            this.groupBox1.Location = new System.Drawing.Point(12, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(488, 172);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Создание фермы";
            // 
            // rbJustExit
            // 
            this.rbJustExit.AutoSize = true;
            this.rbJustExit.Location = new System.Drawing.Point(38, 146);
            this.rbJustExit.Name = "rbJustExit";
            this.rbJustExit.Size = new System.Drawing.Size(227, 17);
            this.rbJustExit.TabIndex = 7;
            this.rbJustExit.TabStop = true;
            this.rbJustExit.Text = "Просто завершить работу установщика";
            this.rbJustExit.UseVisualStyleBackColor = true;
            // 
            // tbFile
            // 
            this.tbFile.Enabled = false;
            this.tbFile.Location = new System.Drawing.Point(61, 120);
            this.tbFile.Name = "tbFile";
            this.tbFile.ReadOnly = true;
            this.tbFile.Size = new System.Drawing.Size(298, 20);
            this.tbFile.TabIndex = 6;
            this.tbFile.TextChanged += new System.EventHandler(this.tbFile_TextChanged);
            // 
            // btFile
            // 
            this.btFile.Enabled = false;
            this.btFile.Location = new System.Drawing.Point(365, 118);
            this.btFile.Name = "btFile";
            this.btFile.Size = new System.Drawing.Size(101, 23);
            this.btFile.TabIndex = 5;
            this.btFile.Text = "Выбрать файл";
            this.btFile.UseVisualStyleBackColor = true;
            this.btFile.Click += new System.EventHandler(this.btFile_Click);
            // 
            // tbComp
            // 
            this.tbComp.Enabled = false;
            this.tbComp.Location = new System.Drawing.Point(168, 72);
            this.tbComp.Name = "tbComp";
            this.tbComp.Size = new System.Drawing.Size(124, 20);
            this.tbComp.TabIndex = 4;
            this.tbComp.TextChanged += new System.EventHandler(this.tbComp_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Адрес компьютера";
            // 
            // rbImportFromMia
            // 
            this.rbImportFromMia.AutoSize = true;
            this.rbImportFromMia.Location = new System.Drawing.Point(38, 98);
            this.rbImportFromMia.Name = "rbImportFromMia";
            this.rbImportFromMia.Size = new System.Drawing.Size(191, 17);
            this.rbImportFromMia.TabIndex = 2;
            this.rbImportFromMia.Text = "Импортировать ферму из файла";
            this.rbImportFromMia.UseVisualStyleBackColor = true;
            this.rbImportFromMia.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rbRemoteDb
            // 
            this.rbRemoteDb.AutoSize = true;
            this.rbRemoteDb.Location = new System.Drawing.Point(38, 49);
            this.rbRemoteDb.Name = "rbRemoteDb";
            this.rbRemoteDb.Size = new System.Drawing.Size(254, 17);
            this.rbRemoteDb.TabIndex = 1;
            this.rbRemoteDb.Text = "Использовать ферму на другом компьютере";
            this.rbRemoteDb.UseVisualStyleBackColor = true;
            this.rbRemoteDb.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rbMakeNew
            // 
            this.rbMakeNew.AutoSize = true;
            this.rbMakeNew.Checked = true;
            this.rbMakeNew.Location = new System.Drawing.Point(38, 26);
            this.rbMakeNew.Name = "rbMakeNew";
            this.rbMakeNew.Size = new System.Drawing.Size(250, 17);
            this.rbMakeNew.TabIndex = 0;
            this.rbMakeNew.TabStop = true;
            this.rbMakeNew.Text = "Создать пустую ферму на этом компьютере";
            this.rbMakeNew.UseVisualStyleBackColor = true;
            this.rbMakeNew.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // btCheck
            // 
            this.btCheck.Location = new System.Drawing.Point(425, 248);
            this.btCheck.Name = "btCheck";
            this.btCheck.Size = new System.Drawing.Size(75, 23);
            this.btCheck.TabIndex = 2;
            this.btCheck.Text = "Выбрать";
            this.btCheck.UseVisualStyleBackColor = true;
            this.btCheck.Click += new System.EventHandler(this.btCheck_Click);
            // 
            // btExtended
            // 
            this.btExtended.Location = new System.Drawing.Point(12, 248);
            this.btExtended.Name = "btExtended";
            this.btExtended.Size = new System.Drawing.Size(148, 23);
            this.btExtended.TabIndex = 3;
            this.btExtended.Text = "Расширенный режим >>";
            this.btExtended.UseVisualStyleBackColor = true;
            this.btExtended.Click += new System.EventHandler(this.btExtended_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.tbRPwd);
            this.groupBox2.Controls.Add(this.tbRoot);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tbPwd);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.tbUser);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbDb);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbHost);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 277);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(488, 131);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройки Базы Данных";
            this.groupBox2.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(239, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(132, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Пароль администратора";
            // 
            // tbRPwd
            // 
            this.tbRPwd.Location = new System.Drawing.Point(377, 51);
            this.tbRPwd.Name = "tbRPwd";
            this.tbRPwd.Size = new System.Drawing.Size(100, 20);
            this.tbRPwd.TabIndex = 10;
            // 
            // tbRoot
            // 
            this.tbRoot.Location = new System.Drawing.Point(377, 25);
            this.tbRoot.Name = "tbRoot";
            this.tbRoot.Size = new System.Drawing.Size(100, 20);
            this.tbRoot.TabIndex = 9;
            this.tbRoot.Text = "root";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(266, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Администратор БД";
            // 
            // tbPwd
            // 
            this.tbPwd.Location = new System.Drawing.Point(92, 103);
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.Size = new System.Drawing.Size(100, 20);
            this.tbPwd.TabIndex = 7;
            this.tbPwd.Text = "krol";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Пароль";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(92, 77);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(100, 20);
            this.tbUser.TabIndex = 5;
            this.tbUser.Text = "kroliki";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Пользователь";
            // 
            // tbDb
            // 
            this.tbDb.Location = new System.Drawing.Point(92, 51);
            this.tbDb.Name = "tbDb";
            this.tbDb.Size = new System.Drawing.Size(100, 20);
            this.tbDb.TabIndex = 3;
            this.tbDb.Text = "kroliki";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "БД";
            // 
            // tbHost
            // 
            this.tbHost.Location = new System.Drawing.Point(92, 25);
            this.tbHost.Name = "tbHost";
            this.tbHost.Size = new System.Drawing.Size(100, 20);
            this.tbHost.TabIndex = 1;
            this.tbHost.Text = "localhost";
            this.tbHost.TextChanged += new System.EventHandler(this.tbHost_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Хост";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Введите название фермы";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(169, 44);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(320, 20);
            this.tbName.TabIndex = 6;
            this.tbName.Text = "Новая Ферма";
            // 
            // ofd
            // 
            this.ofd.Filter = "Файл фермы (*.mia)|*.mia";
            // 
            // InstallForm
            // 
            this.AcceptButton = this.btCheck;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 278);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btExtended);
            this.Controls.Add(this.btCheck);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Установка фермы";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbImportFromMia;
        private System.Windows.Forms.RadioButton rbRemoteDb;
        private System.Windows.Forms.RadioButton rbMakeNew;
        private System.Windows.Forms.Button btCheck;
        private System.Windows.Forms.Button btFile;
        private System.Windows.Forms.TextBox tbComp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.Button btExtended;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbHost;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbRPwd;
        private System.Windows.Forms.TextBox tbRoot;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbDb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.RadioButton rbJustExit;
    }
}