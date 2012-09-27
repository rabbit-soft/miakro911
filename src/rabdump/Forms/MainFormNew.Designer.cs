namespace rabdump
{
    partial class MainFormNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormNew));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chStartUp = new System.Windows.Forms.CheckBox();
            this.tb7zPath = new System.Windows.Forms.TextBox();
            this.bt7ZipPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMysqlPath = new System.Windows.Forms.TextBox();
            this.btMysqlPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tpDataSources = new System.Windows.Forms.TabPage();
            this.farmsPanel1 = new rabnet.panels.FarmsPanel();
            this.Расписания = new System.Windows.Forms.TabPage();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.tDumper = new System.Windows.Forms.Timer(this.components);
            this.tUpdater = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.RunRabnetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewFarm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.miServDump = new System.Windows.Forms.ToolStripMenuItem();
            this.jobnowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miSendGlobRep = new System.Windows.Forms.ToolStripMenuItem();
            this.miManage = new System.Windows.Forms.ToolStripMenuItem();
            this.updateKeyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miCheckForUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpDataSources.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpGeneral);
            this.tabControl1.Controls.Add(this.tpDataSources);
            this.tabControl1.Controls.Add(this.Расписания);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(361, 351);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.groupBox1);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(353, 325);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "Основное";
            this.tpGeneral.Click += new System.EventHandler(this.tpGeneral_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chStartUp);
            this.groupBox1.Controls.Add(this.tb7zPath);
            this.groupBox1.Controls.Add(this.bt7ZipPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbMysqlPath);
            this.groupBox1.Controls.Add(this.btMysqlPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 169);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // chStartUp
            // 
            this.chStartUp.AutoSize = true;
            this.chStartUp.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chStartUp.Location = new System.Drawing.Point(9, 100);
            this.chStartUp.Name = "chStartUp";
            this.chStartUp.Size = new System.Drawing.Size(196, 17);
            this.chStartUp.TabIndex = 8;
            this.chStartUp.Text = "Запускать при загрузке Windows";
            this.chStartUp.UseVisualStyleBackColor = true;
            // 
            // tb7zPath
            // 
            this.tb7zPath.Location = new System.Drawing.Point(33, 74);
            this.tb7zPath.Name = "tb7zPath";
            this.tb7zPath.Size = new System.Drawing.Size(270, 20);
            this.tb7zPath.TabIndex = 6;
            // 
            // bt7ZipPath
            // 
            this.bt7ZipPath.Location = new System.Drawing.Point(309, 72);
            this.bt7ZipPath.Name = "bt7ZipPath";
            this.bt7ZipPath.Size = new System.Drawing.Size(28, 23);
            this.bt7ZipPath.TabIndex = 7;
            this.bt7ZipPath.Text = "...";
            this.bt7ZipPath.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Путь к архиватороу 7Zip:";
            // 
            // tbMysqlPath
            // 
            this.tbMysqlPath.Location = new System.Drawing.Point(33, 32);
            this.tbMysqlPath.Name = "tbMysqlPath";
            this.tbMysqlPath.Size = new System.Drawing.Size(270, 20);
            this.tbMysqlPath.TabIndex = 3;
            this.tbMysqlPath.TextChanged += new System.EventHandler(this.tbMysqlPath_TextChanged);
            // 
            // btMysqlPath
            // 
            this.btMysqlPath.Location = new System.Drawing.Point(309, 30);
            this.btMysqlPath.Name = "btMysqlPath";
            this.btMysqlPath.Size = new System.Drawing.Size(28, 23);
            this.btMysqlPath.TabIndex = 4;
            this.btMysqlPath.Text = "...";
            this.btMysqlPath.UseVisualStyleBackColor = true;
            this.btMysqlPath.Click += new System.EventHandler(this.btMysqlPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Путь к папке с MySQL сервером:";
            // 
            // tpDataSources
            // 
            this.tpDataSources.Controls.Add(this.farmsPanel1);
            this.tpDataSources.Location = new System.Drawing.Point(4, 22);
            this.tpDataSources.Name = "tpDataSources";
            this.tpDataSources.Padding = new System.Windows.Forms.Padding(3);
            this.tpDataSources.Size = new System.Drawing.Size(353, 325);
            this.tpDataSources.TabIndex = 1;
            this.tpDataSources.Text = "Подключения";
            // 
            // farmsPanel1
            // 
            this.farmsPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.farmsPanel1.Location = new System.Drawing.Point(3, 3);
            this.farmsPanel1.MaximumSize = new System.Drawing.Size(0, 315);
            this.farmsPanel1.MinimumSize = new System.Drawing.Size(340, 315);
            this.farmsPanel1.Name = "farmsPanel1";
            this.farmsPanel1.Size = new System.Drawing.Size(347, 315);
            this.farmsPanel1.TabIndex = 0;
            // 
            // Расписания
            // 
            this.Расписания.Location = new System.Drawing.Point(4, 22);
            this.Расписания.Name = "Расписания";
            this.Расписания.Size = new System.Drawing.Size(353, 378);
            this.Расписания.TabIndex = 2;
            this.Расписания.Text = "Расписания";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = ".exe|Исполняемый";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = "MySQL Server 5.1";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Резервные копии";
            this.notifyIcon1.Visible = true;
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(102, 363);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(183, 363);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "Сохранить";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // tDumper
            // 
            this.tDumper.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tUpdater
            // 
            this.tUpdater.Interval = 10800000;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreMenuItem,
            this.toolStripMenuItem1,
            this.RunRabnetToolStripMenuItem,
            this.miNewFarm,
            this.toolStripMenuItem3,
            this.miServDump,
            this.jobnowMenuItem,
            this.jobsMenuItem,
            this.restMenuItem,
            this.toolStripMenuItem2,
            this.miSendGlobRep,
            this.miManage,
            this.updateKeyMenuItem,
            this.miCheckForUpdate,
            this.toolStripSeparator1,
            this.AboutToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 342);
            // 
            // restoreMenuItem
            // 
            this.restoreMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.restoreMenuItem.Name = "restoreMenuItem";
            this.restoreMenuItem.Size = new System.Drawing.Size(210, 22);
            this.restoreMenuItem.Text = "Настройки";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(207, 6);
            // 
            // RunRabnetToolStripMenuItem
            // 
            this.RunRabnetToolStripMenuItem.Name = "RunRabnetToolStripMenuItem";
            this.RunRabnetToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.RunRabnetToolStripMenuItem.Text = "Запустить rabnet";
            this.RunRabnetToolStripMenuItem.Visible = false;
            // 
            // miNewFarm
            // 
            this.miNewFarm.Name = "miNewFarm";
            this.miNewFarm.Size = new System.Drawing.Size(210, 22);
            this.miNewFarm.Text = "Новая Ферма";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(207, 6);
            // 
            // miServDump
            // 
            this.miServDump.Name = "miServDump";
            this.miServDump.Size = new System.Drawing.Size(210, 22);
            this.miServDump.Text = "Отправить на сервер";
            // 
            // jobnowMenuItem
            // 
            this.jobnowMenuItem.Name = "jobnowMenuItem";
            this.jobnowMenuItem.Size = new System.Drawing.Size(210, 22);
            this.jobnowMenuItem.Text = "Резервировать все";
            this.jobnowMenuItem.Visible = false;
            // 
            // jobsMenuItem
            // 
            this.jobsMenuItem.Name = "jobsMenuItem";
            this.jobsMenuItem.Size = new System.Drawing.Size(210, 22);
            this.jobsMenuItem.Text = "Резервировать";
            // 
            // restMenuItem
            // 
            this.restMenuItem.Name = "restMenuItem";
            this.restMenuItem.Size = new System.Drawing.Size(210, 22);
            this.restMenuItem.Text = "Восстановить";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(207, 6);
            // 
            // miSendGlobRep
            // 
            this.miSendGlobRep.Name = "miSendGlobRep";
            this.miSendGlobRep.Size = new System.Drawing.Size(210, 22);
            this.miSendGlobRep.Text = "Послать отчет на сервер";
            // 
            // miManage
            // 
            this.miManage.Name = "miManage";
            this.miManage.Size = new System.Drawing.Size(210, 22);
            this.miManage.Text = "Управление лецензиями";
            // 
            // updateKeyMenuItem
            // 
            this.updateKeyMenuItem.Name = "updateKeyMenuItem";
            this.updateKeyMenuItem.Size = new System.Drawing.Size(210, 22);
            this.updateKeyMenuItem.Text = "Обновить лицензию";
            // 
            // miCheckForUpdate
            // 
            this.miCheckForUpdate.Name = "miCheckForUpdate";
            this.miCheckForUpdate.Size = new System.Drawing.Size(210, 22);
            this.miCheckForUpdate.Text = "Проверить обновление";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(207, 6);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.AboutToolStripMenuItem.Text = "О программе";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(207, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(210, 22);
            this.exitMenuItem.Text = "Выход";
            // 
            // MainFormNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 398);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainFormNew";
            this.Text = "RabDump";
            this.Load += new System.EventHandler(this.MainFormNew_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tpDataSources.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpDataSources;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chStartUp;
        private System.Windows.Forms.TextBox tb7zPath;
        private System.Windows.Forms.Button bt7ZipPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMysqlPath;
        private System.Windows.Forms.Button btMysqlPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage Расписания;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btOK;
        private rabnet.panels.FarmsPanel farmsPanel1;
        private System.Windows.Forms.Timer tDumper;
        private System.Windows.Forms.Timer tUpdater;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem restoreMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem RunRabnetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miNewFarm;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem miServDump;
        private System.Windows.Forms.ToolStripMenuItem jobnowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem miSendGlobRep;
        private System.Windows.Forms.ToolStripMenuItem miManage;
        private System.Windows.Forms.ToolStripMenuItem updateKeyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miCheckForUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
    }
}