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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.scUsers = new System.Windows.Forms.SplitContainer();
            this.lbClients = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.btAddUser = new System.Windows.Forms.Button();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbContact = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbOrgName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btEditKey = new System.Windows.Forms.Button();
            this.btAddKey = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chID = new System.Windows.Forms.ColumnHeader();
            this.chIdhex = new System.Windows.Forms.ColumnHeader();
            this.chLabel = new System.Windows.Forms.ColumnHeader();
            this.chFarms = new System.Windows.Forms.ColumnHeader();
            this.chStartDate = new System.Windows.Forms.ColumnHeader();
            this.chEndDate = new System.Windows.Forms.ColumnHeader();
            this.chFlags = new System.Windows.Forms.ColumnHeader();
            this.chTimeFlags = new System.Windows.Forms.ColumnHeader();
            this.chTimeFlagsEnd = new System.Windows.Forms.ColumnHeader();
            this.menuStrip1.SuspendLayout();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.scUsers.Panel1.SuspendLayout();
            this.scUsers.Panel2.SuspendLayout();
            this.scUsers.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(750, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.выходToolStripMenuItem.Text = "Выход";
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
            this.scMain.Panel2.Controls.Add(this.btEditKey);
            this.scMain.Panel2.Controls.Add(this.btAddKey);
            this.scMain.Panel2.Controls.Add(this.label2);
            this.scMain.Panel2.Controls.Add(this.listView1);
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
            this.scUsers.Panel1.Controls.Add(this.lbClients);
            this.scUsers.Panel1.Controls.Add(this.btAddUser);
            // 
            // scUsers.Panel2
            // 
            this.scUsers.Panel2.Controls.Add(this.tbAddress);
            this.scUsers.Panel2.Controls.Add(this.label3);
            this.scUsers.Panel2.Controls.Add(this.tbContact);
            this.scUsers.Panel2.Controls.Add(this.label4);
            this.scUsers.Panel2.Controls.Add(this.tbOrgName);
            this.scUsers.Panel2.Controls.Add(this.label5);
            this.scUsers.Size = new System.Drawing.Size(750, 250);
            this.scUsers.SplitterDistance = 250;
            this.scUsers.TabIndex = 0;
            // 
            // lbClients
            // 
            this.lbClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lbClients.FullRowSelect = true;
            this.lbClients.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lbClients.Location = new System.Drawing.Point(3, 3);
            this.lbClients.Name = "lbClients";
            this.lbClients.Size = new System.Drawing.Size(244, 215);
            this.lbClients.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lbClients.TabIndex = 3;
            this.lbClients.UseCompatibleStateImageBehavior = false;
            this.lbClients.View = System.Windows.Forms.View.Details;
            this.lbClients.SelectedIndexChanged += new System.EventHandler(this.lbClients_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Клиенты";
            this.columnHeader1.Width = 200;
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
            // tbAddress
            // 
            this.tbAddress.Enabled = false;
            this.tbAddress.Location = new System.Drawing.Point(58, 74);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(265, 20);
            this.tbAddress.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Адрес:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbContact
            // 
            this.tbContact.Enabled = false;
            this.tbContact.Location = new System.Drawing.Point(121, 47);
            this.tbContact.Name = "tbContact";
            this.tbContact.Size = new System.Drawing.Size(202, 20);
            this.tbContact.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Контактное лицо:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbOrgName
            // 
            this.tbOrgName.Enabled = false;
            this.tbOrgName.Location = new System.Drawing.Point(159, 21);
            this.tbOrgName.Name = "tbOrgName";
            this.tbOrgName.Size = new System.Drawing.Size(164, 20);
            this.tbOrgName.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Название организации:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btEditKey
            // 
            this.btEditKey.Location = new System.Drawing.Point(175, 218);
            this.btEditKey.Name = "btEditKey";
            this.btEditKey.Size = new System.Drawing.Size(182, 23);
            this.btEditKey.TabIndex = 3;
            this.btEditKey.Text = "Перепрошить ключ";
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
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chID,
            this.chIdhex,
            this.chLabel,
            this.chFarms,
            this.chStartDate,
            this.chEndDate,
            this.chFlags,
            this.chTimeFlags,
            this.chTimeFlagsEnd});
            this.listView1.Location = new System.Drawing.Point(3, 26);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(744, 185);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
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
            // 
            // chEndDate
            // 
            this.chEndDate.Text = "Дата Окончания";
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
            this.scUsers.Panel2.ResumeLayout(false);
            this.scUsers.Panel2.PerformLayout();
            this.scUsers.ResumeLayout(false);
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
        private System.Windows.Forms.ListView listView1;
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
        private System.Windows.Forms.ListView lbClients;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbContact;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbOrgName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}

