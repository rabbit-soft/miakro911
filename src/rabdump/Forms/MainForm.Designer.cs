namespace rabdump
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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
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
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tDumper = new System.Windows.Forms.Timer(this.components);
            this.tUpdater = new System.Windows.Forms.Timer(this.components);
            this.miCheckForUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Резервные копии";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
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
            this.restoreMenuItem.Click += new System.EventHandler(this.restoreMenuItem_Click);
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
            this.RunRabnetToolStripMenuItem.Click += new System.EventHandler(this.RunRabnetToolStripMenuItem_Click);
            // 
            // miNewFarm
            // 
            this.miNewFarm.Name = "miNewFarm";
            this.miNewFarm.Size = new System.Drawing.Size(210, 22);
            this.miNewFarm.Text = "Новая Ферма";
            this.miNewFarm.Click += new System.EventHandler(this.newFarm_Click);
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
            this.miServDump.Click += new System.EventHandler(this.miServDump_Click);
            // 
            // jobnowMenuItem
            // 
            this.jobnowMenuItem.Name = "jobnowMenuItem";
            this.jobnowMenuItem.Size = new System.Drawing.Size(210, 22);
            this.jobnowMenuItem.Text = "Резервировать все";
            this.jobnowMenuItem.Visible = false;
            this.jobnowMenuItem.Click += new System.EventHandler(this.jobnowMenuItem_Click);
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
            this.restMenuItem.Click += new System.EventHandler(this.restMenuItem_Click);
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
            this.miSendGlobRep.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // miManage
            // 
            this.miManage.Name = "miManage";
            this.miManage.Size = new System.Drawing.Size(210, 22);
            this.miManage.Text = "Управление лецензиями";
            this.miManage.Click += new System.EventHandler(this.miManage_Click);
            // 
            // updateKeyMenuItem
            // 
            this.updateKeyMenuItem.Name = "updateKeyMenuItem";
            this.updateKeyMenuItem.Size = new System.Drawing.Size(210, 22);
            this.updateKeyMenuItem.Text = "Обновить лицензию";
            this.updateKeyMenuItem.Click += new System.EventHandler(this.updateKeyMenuItem_Click);
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
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
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
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Location = new System.Drawing.Point(459, 362);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 2;
            this.btOk.Text = "Применить";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(378, 362);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(6, -1);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(528, 357);
            this.propertyGrid1.TabIndex = 0;
            // 
            // tDumper
            // 
            this.tDumper.Interval = 60000;
            this.tDumper.Tick += new System.EventHandler(this.tDumper_Tick);
            // 
            // tUpdater
            // 
            this.tUpdater.Interval = 10800000;
            // 
            // miCheckForUpdate
            // 
            this.miCheckForUpdate.Name = "miCheckForUpdate";
            this.miCheckForUpdate.Size = new System.Drawing.Size(210, 22);
            this.miCheckForUpdate.Text = "Проверить обновление";
            this.miCheckForUpdate.Click += new System.EventHandler(this.miCheckForUpdate_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(541, 393);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(549, 427);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Резервное копирование";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem restoreMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem jobnowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        /// <summary>
        /// Проверяет каждую минуту нужно ли делать резервирование
        /// </summary>
        private System.Windows.Forms.Timer tDumper;
        private System.Windows.Forms.ToolStripMenuItem restMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RunRabnetToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem miNewFarm;
        private System.Windows.Forms.Timer tUpdater;
        private System.Windows.Forms.ToolStripMenuItem updateKeyMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miServDump;
        private System.Windows.Forms.ToolStripMenuItem miSendGlobRep;
        private System.Windows.Forms.ToolStripMenuItem miManage;
        private System.Windows.Forms.ToolStripMenuItem miCheckForUpdate;
    }
}

