namespace rabnet
{
    partial class RabbitsPanel
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
            this.actMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.passportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newRab = new System.Windows.Forms.ToolStripMenuItem();
            this.makeBon = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.KillMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.placeChMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fuckMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.proholostMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.okrolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countKidsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.genTree = new System.Windows.Forms.TreeView();
            this.boysoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.actMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.genTree);
            this.splitContainer1.Size = new System.Drawing.Size(853, 550);
            this.splitContainer1.SplitterDistance = 709;
            this.splitContainer1.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13});
            this.listView1.ContextMenuStrip = this.actMenu;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(703, 544);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Имя";
            this.columnHeader1.Width = 122;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Пол";
            this.columnHeader2.Width = 41;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Возраст";
            this.columnHeader3.Width = 47;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Порода";
            this.columnHeader4.Width = 48;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Вес";
            this.columnHeader5.Width = 38;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Статус";
            this.columnHeader6.Width = 50;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Б/Г";
            this.columnHeader7.Width = 35;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "N";
            this.columnHeader8.Width = 31;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "СрВ";
            this.columnHeader9.Width = 36;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Рейтинг";
            this.columnHeader10.Width = 46;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Класс";
            this.columnHeader11.Width = 41;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Адрес";
            this.columnHeader12.Width = 90;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Заметки";
            this.columnHeader13.Width = 64;
            // 
            // actMenu
            // 
            this.actMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.passportMenuItem,
            this.newRab,
            this.makeBon,
            this.toolStripMenuItem1,
            this.KillMenuItem,
            this.replaceMenuItem,
            this.boysoutMenuItem,
            this.placeChMenuItem,
            this.fuckMenuItem,
            this.proholostMenuItem,
            this.okrolMenuItem,
            this.countKidsMenuItem,
            this.toolStripMenuItem2,
            this.SelectAllMenuItem});
            this.actMenu.Name = "actMenu";
            this.actMenu.Size = new System.Drawing.Size(201, 302);
            // 
            // passportMenuItem
            // 
            this.passportMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.passportMenuItem.Name = "passportMenuItem";
            this.passportMenuItem.Size = new System.Drawing.Size(200, 22);
            this.passportMenuItem.Text = "Паспорт";
            this.passportMenuItem.Click += new System.EventHandler(this.passportMenuItem_Click);
            // 
            // newRab
            // 
            this.newRab.Name = "newRab";
            this.newRab.Size = new System.Drawing.Size(200, 22);
            this.newRab.Text = "Привоз";
            this.newRab.Click += new System.EventHandler(this.newRab_Click);
            // 
            // makeBon
            // 
            this.makeBon.Name = "makeBon";
            this.makeBon.Size = new System.Drawing.Size(200, 22);
            this.makeBon.Text = "Бонитировка";
            this.makeBon.Click += new System.EventHandler(this.makeBon_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(197, 6);
            // 
            // KillMenuItem
            // 
            this.KillMenuItem.Name = "KillMenuItem";
            this.KillMenuItem.Size = new System.Drawing.Size(200, 22);
            this.KillMenuItem.Text = "Списание";
            this.KillMenuItem.Click += new System.EventHandler(this.KillMenuItem_Click);
            // 
            // replaceMenuItem
            // 
            this.replaceMenuItem.Name = "replaceMenuItem";
            this.replaceMenuItem.Size = new System.Drawing.Size(200, 22);
            this.replaceMenuItem.Text = "Пересадить/Отсадить";
            this.replaceMenuItem.Click += new System.EventHandler(this.replaceMenuItem_Click);
            // 
            // placeChMenuItem
            // 
            this.placeChMenuItem.Name = "placeChMenuItem";
            this.placeChMenuItem.Size = new System.Drawing.Size(200, 22);
            this.placeChMenuItem.Text = "Жилобмен";
            this.placeChMenuItem.Click += new System.EventHandler(this.placeChMenuItem_Click);
            // 
            // fuckMenuItem
            // 
            this.fuckMenuItem.Name = "fuckMenuItem";
            this.fuckMenuItem.Size = new System.Drawing.Size(200, 22);
            this.fuckMenuItem.Text = "Случка";
            this.fuckMenuItem.Click += new System.EventHandler(this.fuckMenuItem_Click);
            // 
            // proholostMenuItem
            // 
            this.proholostMenuItem.Name = "proholostMenuItem";
            this.proholostMenuItem.Size = new System.Drawing.Size(200, 22);
            this.proholostMenuItem.Text = "Прохолостание";
            this.proholostMenuItem.Click += new System.EventHandler(this.proholostMenuItem_Click);
            // 
            // okrolMenuItem
            // 
            this.okrolMenuItem.Name = "okrolMenuItem";
            this.okrolMenuItem.Size = new System.Drawing.Size(200, 22);
            this.okrolMenuItem.Text = "Окрол";
            this.okrolMenuItem.Click += new System.EventHandler(this.okrolMenuItem_Click);
            // 
            // countKidsMenuItem
            // 
            this.countKidsMenuItem.Name = "countKidsMenuItem";
            this.countKidsMenuItem.Size = new System.Drawing.Size(200, 22);
            this.countKidsMenuItem.Text = "Подсчет гнездовых";
            this.countKidsMenuItem.Click += new System.EventHandler(this.countKidsMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(197, 6);
            // 
            // SelectAllMenuItem
            // 
            this.SelectAllMenuItem.Name = "SelectAllMenuItem";
            this.SelectAllMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.SelectAllMenuItem.Size = new System.Drawing.Size(200, 22);
            this.SelectAllMenuItem.Text = "Выбрать всех";
            this.SelectAllMenuItem.Click += new System.EventHandler(this.SelectAllMenuItem_Click);
            // 
            // genTree
            // 
            this.genTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.genTree.Location = new System.Drawing.Point(3, 3);
            this.genTree.Name = "genTree";
            this.genTree.Size = new System.Drawing.Size(134, 544);
            this.genTree.TabIndex = 0;
            // 
            // boysoutMenuItem
            // 
            this.boysoutMenuItem.Name = "boysoutMenuItem";
            this.boysoutMenuItem.Size = new System.Drawing.Size(200, 22);
            this.boysoutMenuItem.Text = "Отсадить мальчиков";
            this.boysoutMenuItem.Click += new System.EventHandler(this.boysoutMenuItem_Click);
            // 
            // RabbitsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "RabbitsPanel";
            this.Size = new System.Drawing.Size(853, 550);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.actMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.TreeView genTree;
        private System.Windows.Forms.ContextMenuStrip actMenu;
        private System.Windows.Forms.ToolStripMenuItem passportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newRab;
        private System.Windows.Forms.ToolStripMenuItem makeBon;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem proholostMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem SelectAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem placeChMenuItem;
        private System.Windows.Forms.ToolStripMenuItem KillMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countKidsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fuckMenuItem;
        private System.Windows.Forms.ToolStripMenuItem okrolMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boysoutMenuItem;
    }
}
