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
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.scUsers = new System.Windows.Forms.SplitContainer();
            this.lbUsers = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btAddUser = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.chID = new System.Windows.Forms.ColumnHeader();
            this.chIdhex = new System.Windows.Forms.ColumnHeader();
            this.chLabel = new System.Windows.Forms.ColumnHeader();
            this.chFarms = new System.Windows.Forms.ColumnHeader();
            this.btAddKey = new System.Windows.Forms.Button();
            this.btEditKey = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.scUsers.Panel1.SuspendLayout();
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
            this.scUsers.Panel1.Controls.Add(this.btAddUser);
            this.scUsers.Panel1.Controls.Add(this.label1);
            this.scUsers.Panel1.Controls.Add(this.lbUsers);
            this.scUsers.Size = new System.Drawing.Size(750, 250);
            this.scUsers.SplitterDistance = 250;
            this.scUsers.TabIndex = 0;
            // 
            // lbUsers
            // 
            this.lbUsers.FormattingEnabled = true;
            this.lbUsers.Location = new System.Drawing.Point(6, 21);
            this.lbUsers.Name = "lbUsers";
            this.lbUsers.Size = new System.Drawing.Size(244, 199);
            this.lbUsers.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Пользователи";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            // 
            // btAddUser
            // 
            this.btAddUser.Location = new System.Drawing.Point(12, 224);
            this.btAddUser.Name = "btAddUser";
            this.btAddUser.Size = new System.Drawing.Size(75, 23);
            this.btAddUser.TabIndex = 2;
            this.btAddUser.Text = "Добавить";
            this.btAddUser.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chID,
            this.chIdhex,
            this.chLabel,
            this.chFarms});
            this.listView1.Location = new System.Drawing.Point(3, 26);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(744, 185);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
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
            this.chFarms.Width = 128;
            // 
            // btAddKey
            // 
            this.btAddKey.Location = new System.Drawing.Point(12, 218);
            this.btAddKey.Name = "btAddKey";
            this.btAddKey.Size = new System.Drawing.Size(151, 23);
            this.btAddKey.TabIndex = 2;
            this.btAddKey.Text = "Прошить новый ключ";
            this.btAddKey.UseVisualStyleBackColor = true;
            // 
            // btEditKey
            // 
            this.btEditKey.Location = new System.Drawing.Point(175, 218);
            this.btEditKey.Name = "btEditKey";
            this.btEditKey.Size = new System.Drawing.Size(182, 23);
            this.btEditKey.TabIndex = 3;
            this.btEditKey.Text = "Перепрошить ключ";
            this.btEditKey.UseVisualStyleBackColor = true;
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.SplitContainer scUsers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbUsers;
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
    }
}

