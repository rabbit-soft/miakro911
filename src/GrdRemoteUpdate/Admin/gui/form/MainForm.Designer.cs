namespace AdminGRD
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обновитьСписокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miDongles = new System.Windows.Forms.ToolStripMenuItem();
            this.miTRUHostDongle = new System.Windows.Forms.ToolStripMenuItem();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.scUsers = new System.Windows.Forms.SplitContainer();
            this.lvClients = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьДенегToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btAddUser = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btEditKey = new System.Windows.Forms.Button();
            this.btAddKey = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lvDongles = new System.Windows.Forms.ListView();
            this.chID = new System.Windows.Forms.ColumnHeader();
            this.chIdhex = new System.Windows.Forms.ColumnHeader();
            this.chLabel = new System.Windows.Forms.ColumnHeader();
            this.chFarms = new System.Windows.Forms.ColumnHeader();
            this.chStartDate = new System.Windows.Forms.ColumnHeader();
            this.chEndDate = new System.Windows.Forms.ColumnHeader();
            this.chFlags = new System.Windows.Forms.ColumnHeader();
            this.chTimeFlags = new System.Windows.Forms.ColumnHeader();
            this.chTimeFlagsEnd = new System.Windows.Forms.ColumnHeader();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.показатьПрошивкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.scUsers.Panel1.SuspendLayout();
            this.scUsers.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.miDongles});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(750, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.обновитьСписокToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // обновитьСписокToolStripMenuItem
            // 
            this.обновитьСписокToolStripMenuItem.Name = "обновитьСписокToolStripMenuItem";
            this.обновитьСписокToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.обновитьСписокToolStripMenuItem.Text = "Обновить список";
            this.обновитьСписокToolStripMenuItem.Click += new System.EventHandler(this.обновитьСписокToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // miDongles
            // 
            this.miDongles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miTRUHostDongle});
            this.miDongles.Name = "miDongles";
            this.miDongles.Size = new System.Drawing.Size(121, 20);
            this.miDongles.Text = "Работа с ключами";
            // 
            // miTRUHostDongle
            // 
            this.miTRUHostDongle.Name = "miTRUHostDongle";
            this.miTRUHostDongle.Size = new System.Drawing.Size(184, 22);
            this.miTRUHostDongle.Text = "Прошить ключ TRU";
            this.miTRUHostDongle.Click += new System.EventHandler(this.miTRUHostDongle_Click);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 24);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.scUsers);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.button1);
            this.scMain.Panel2.Controls.Add(this.btEditKey);
            this.scMain.Panel2.Controls.Add(this.btAddKey);
            this.scMain.Panel2.Controls.Add(this.label2);
            this.scMain.Panel2.Controls.Add(this.lvDongles);
            this.scMain.Size = new System.Drawing.Size(750, 507);
            this.scMain.SplitterDistance = 250;
            this.scMain.TabIndex = 1;
            // 
            // scUsers
            // 
            this.scUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scUsers.Location = new System.Drawing.Point(0, 0);
            this.scUsers.Name = "scUsers";
            // 
            // scUsers.Panel1
            // 
            this.scUsers.Panel1.Controls.Add(this.lvClients);
            this.scUsers.Panel1.Controls.Add(this.btAddUser);
            this.scUsers.Panel2Collapsed = true;
            this.scUsers.Size = new System.Drawing.Size(750, 250);
            this.scUsers.SplitterDistance = 250;
            this.scUsers.TabIndex = 0;
            // 
            // lvClients
            // 
            this.lvClients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lvClients.ContextMenuStrip = this.contextMenuStrip1;
            this.lvClients.FullRowSelect = true;
            this.lvClients.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvClients.Location = new System.Drawing.Point(3, 3);
            this.lvClients.Name = "lvClients";
            this.lvClients.Size = new System.Drawing.Size(744, 215);
            this.lvClients.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvClients.TabIndex = 3;
            this.lvClients.UseCompatibleStateImageBehavior = false;
            this.lvClients.View = System.Windows.Forms.View.Details;
            this.lvClients.SelectedIndexChanged += new System.EventHandler(this.lbClients_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Клиенты";
            this.columnHeader1.Width = 129;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Контактное лицо";
            this.columnHeader2.Width = 116;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Деньги";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Адрес";
            this.columnHeader4.Width = 299;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Версия";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьДенегToolStripMenuItem,
            this.показатьПрошивкиToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(186, 48);
            // 
            // добавитьДенегToolStripMenuItem
            // 
            this.добавитьДенегToolStripMenuItem.Name = "добавитьДенегToolStripMenuItem";
            this.добавитьДенегToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.добавитьДенегToolStripMenuItem.Text = "Добавить денег";
            this.добавитьДенегToolStripMenuItem.Click += new System.EventHandler(this.btAddMoney_Click);
            // 
            // btAddUser
            // 
            this.btAddUser.Location = new System.Drawing.Point(12, 224);
            this.btAddUser.Name = "btAddUser";
            this.btAddUser.Size = new System.Drawing.Size(75, 23);
            this.btAddUser.TabIndex = 2;
            this.btAddUser.Text = "Добавить";
            this.btAddUser.UseVisualStyleBackColor = true;
            this.btAddUser.Click += new System.EventHandler(this.btAddUser_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(514, 217);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btEditKey
            // 
            this.btEditKey.Location = new System.Drawing.Point(175, 218);
            this.btEditKey.Name = "btEditKey";
            this.btEditKey.Size = new System.Drawing.Size(182, 23);
            this.btEditKey.TabIndex = 3;
            this.btEditKey.Text = "Назначить прошивку";
            this.btEditKey.UseVisualStyleBackColor = true;
            this.btEditKey.Click += new System.EventHandler(this.btEditKey_Click);
            // 
            // btAddKey
            // 
            this.btAddKey.Location = new System.Drawing.Point(12, 218);
            this.btAddKey.Name = "btAddKey";
            this.btAddKey.Size = new System.Drawing.Size(151, 23);
            this.btAddKey.TabIndex = 2;
            this.btAddKey.Text = "Прошить новый ключ";
            this.btAddKey.UseVisualStyleBackColor = true;
            this.btAddKey.Click += new System.EventHandler(this.btAddKey_Click);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(750, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ключи пользователя";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lvDongles
            // 
            this.lvDongles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDongles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chID,
            this.chIdhex,
            this.chLabel,
            this.chFarms,
            this.chStartDate,
            this.chEndDate,
            this.chFlags,
            this.chTimeFlags,
            this.chTimeFlagsEnd});
            this.lvDongles.FullRowSelect = true;
            this.lvDongles.Location = new System.Drawing.Point(3, 26);
            this.lvDongles.Name = "lvDongles";
            this.lvDongles.Size = new System.Drawing.Size(744, 185);
            this.lvDongles.TabIndex = 0;
            this.lvDongles.UseCompatibleStateImageBehavior = false;
            this.lvDongles.View = System.Windows.Forms.View.Details;
            // 
            // chID
            // 
            this.chID.Text = "Номер";
            this.chID.Width = 91;
            // 
            // chIdhex
            // 
            this.chIdhex.Text = "Номер(hex)";
            this.chIdhex.Width = 113;
            // 
            // chLabel
            // 
            this.chLabel.Text = "Владелец";
            this.chLabel.Width = 112;
            // 
            // chFarms
            // 
            this.chFarms.Text = "Кол-во ферм";
            this.chFarms.Width = 95;
            // 
            // chStartDate
            // 
            this.chStartDate.Text = "ДатаНачала ";
            this.chStartDate.Width = 120;
            // 
            // chEndDate
            // 
            this.chEndDate.Text = "Дата Окончания";
            this.chEndDate.Width = 120;
            // 
            // chFlags
            // 
            this.chFlags.Text = "Флаги";
            // 
            // chTimeFlags
            // 
            this.chTimeFlags.Text = "Временные флаги";
            // 
            // chTimeFlagsEnd
            // 
            this.chTimeFlagsEnd.Text = "Окончание временных флагов";
            // 
            // показатьПрошивкиToolStripMenuItem
            // 
            this.показатьПрошивкиToolStripMenuItem.Name = "показатьПрошивкиToolStripMenuItem";
            this.показатьПрошивкиToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.показатьПрошивкиToolStripMenuItem.Text = "Показать прошивки";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 531);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            this.scMain.ResumeLayout(false);
            this.scUsers.Panel1.ResumeLayout(false);
            this.scUsers.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.SplitContainer scUsers;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.Button btAddUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lvDongles;
        private System.Windows.Forms.ColumnHeader chID;
        private System.Windows.Forms.ColumnHeader chIdhex;
        private System.Windows.Forms.ColumnHeader chLabel;
        private System.Windows.Forms.ColumnHeader chFarms;
        private System.Windows.Forms.Button btEditKey;
        private System.Windows.Forms.Button btAddKey;
        private System.Windows.Forms.ColumnHeader chStartDate;
        private System.Windows.Forms.ColumnHeader chEndDate;
        private System.Windows.Forms.ColumnHeader chFlags;
        private System.Windows.Forms.ColumnHeader chTimeFlags;
        private System.Windows.Forms.ColumnHeader chTimeFlagsEnd;
        private System.Windows.Forms.ListView lvClients;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem добавитьДенегToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripMenuItem обновитьСписокToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem miDongles;
        private System.Windows.Forms.ToolStripMenuItem miTRUHostDongle;
        private System.Windows.Forms.ToolStripMenuItem показатьПрошивкиToolStripMenuItem;
    }
}

