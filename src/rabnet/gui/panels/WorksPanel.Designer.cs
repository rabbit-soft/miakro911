﻿namespace rabnet
{
    partial class WorksPanel
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.actMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.okrolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vudvorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.countsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.countChangedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.preokrolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.boysOutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.girlsOutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fuckMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vaccMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setNestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.печатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listView2 = new System.Windows.Forms.ListView();
			this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
			this.actMenu.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
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
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader6});
			this.listView1.ContextMenuStrip = this.actMenu;
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(3, 3);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(677, 266);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "!";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Название работ";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Адрес";
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Имя";
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Возраст";
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Порода";
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Заметки";
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Партнеры";
			// 
			// actMenu
			// 
			this.actMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.okrolMenuItem,
            this.vudvorMenuItem,
            this.countsMenuItem,
            this.countChangedMenuItem,
            this.preokrolMenuItem,
            this.boysOutMenuItem,
            this.girlsOutMenuItem,
            this.fuckMenuItem,
            this.vaccMenuItem,
            this.setNestMenuItem,
            this.печатьToolStripMenuItem});
			this.actMenu.Name = "actMenu";
			this.actMenu.Size = new System.Drawing.Size(258, 246);
			// 
			// okrolMenuItem
			// 
			this.okrolMenuItem.Name = "okrolMenuItem";
			this.okrolMenuItem.Size = new System.Drawing.Size(257, 22);
			this.okrolMenuItem.Text = "Принять окрол";
			this.okrolMenuItem.Click += new System.EventHandler(this.okrolMenuItem_Click);
			// 
			// vudvorMenuItem
			// 
			this.vudvorMenuItem.Name = "vudvorMenuItem";
			this.vudvorMenuItem.Size = new System.Drawing.Size(257, 22);
			this.vudvorMenuItem.Text = "Выдворение";
			this.vudvorMenuItem.Click += new System.EventHandler(this.okrolMenuItem_Click);
			// 
			// countsMenuItem
			// 
			this.countsMenuItem.Name = "countsMenuItem";
			this.countsMenuItem.Size = new System.Drawing.Size(257, 22);
			this.countsMenuItem.Text = "Подсчет гнездовых/подсосных";
			this.countsMenuItem.Click += new System.EventHandler(this.okrolMenuItem_Click);
			// 
			// countChangedMenuItem
			// 
			this.countChangedMenuItem.Name = "countChangedMenuItem";
			this.countChangedMenuItem.Size = new System.Drawing.Size(257, 22);
			this.countChangedMenuItem.Text = "Изменилось количество крольчат";
			this.countChangedMenuItem.Click += new System.EventHandler(this.countChangedMenuItem_Click);
			// 
			// preokrolMenuItem
			// 
			this.preokrolMenuItem.Name = "preokrolMenuItem";
			this.preokrolMenuItem.Size = new System.Drawing.Size(257, 22);
			this.preokrolMenuItem.Text = "Предокрольный осмотр";
			this.preokrolMenuItem.Click += new System.EventHandler(this.okrolMenuItem_Click);
			// 
			// boysOutMenuItem
			// 
			this.boysOutMenuItem.Name = "boysOutMenuItem";
			this.boysOutMenuItem.Size = new System.Drawing.Size(257, 22);
			this.boysOutMenuItem.Text = "Отсадить мальчиков";
			this.boysOutMenuItem.Click += new System.EventHandler(this.okrolMenuItem_Click);
			// 
			// girlsOutMenuItem
			// 
			this.girlsOutMenuItem.Name = "girlsOutMenuItem";
			this.girlsOutMenuItem.Size = new System.Drawing.Size(257, 22);
			this.girlsOutMenuItem.Text = "Отсадить девочек";
			this.girlsOutMenuItem.Click += new System.EventHandler(this.okrolMenuItem_Click);
			// 
			// fuckMenuItem
			// 
			this.fuckMenuItem.Name = "fuckMenuItem";
			this.fuckMenuItem.Size = new System.Drawing.Size(257, 22);
			this.fuckMenuItem.Text = "Случить";
			this.fuckMenuItem.Click += new System.EventHandler(this.okrolMenuItem_Click);
			// 
			// vaccMenuItem
			// 
			this.vaccMenuItem.Name = "vaccMenuItem";
			this.vaccMenuItem.Size = new System.Drawing.Size(257, 22);
			this.vaccMenuItem.Text = "Привить";
			// 
			// setNestMenuItem
			// 
			this.setNestMenuItem.Name = "setNestMenuItem";
			this.setNestMenuItem.Size = new System.Drawing.Size(257, 22);
			this.setNestMenuItem.Text = "Установить гнездовье";
			this.setNestMenuItem.Click += new System.EventHandler(this.okrolMenuItem_Click);
			// 
			// печатьToolStripMenuItem
			// 
			this.печатьToolStripMenuItem.Name = "печатьToolStripMenuItem";
			this.печатьToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
			this.печатьToolStripMenuItem.Text = "Печать";
			this.печатьToolStripMenuItem.Click += new System.EventHandler(this.печатьToolStripMenuItem_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.listView2);
			this.splitContainer1.Size = new System.Drawing.Size(683, 459);
			this.splitContainer1.SplitterDistance = 272;
			this.splitContainer1.TabIndex = 1;
			// 
			// listView2
			// 
			this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader15,
            this.columnHeader14});
			this.listView2.ForeColor = System.Drawing.Color.Gray;
			this.listView2.FullRowSelect = true;
			this.listView2.GridLines = true;
			this.listView2.HideSelection = false;
			this.listView2.Location = new System.Drawing.Point(3, 3);
			this.listView2.Name = "listView2";
			this.listView2.Size = new System.Drawing.Size(677, 177);
			this.listView2.TabIndex = 0;
			this.listView2.UseCompatibleStateImageBehavior = false;
			this.listView2.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader11
			// 
			this.columnHeader11.Text = "Дата";
			// 
			// columnHeader12
			// 
			this.columnHeader12.Text = "Событие";
			// 
			// columnHeader13
			// 
			this.columnHeader13.Text = "Адрес";
			this.columnHeader13.Width = 75;
			// 
			// columnHeader15
			// 
			this.columnHeader15.Text = "Параметры";
			this.columnHeader15.Width = 342;
			// 
			// columnHeader14
			// 
			this.columnHeader14.Text = "Пользователь";
			// 
			// WorksPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "WorksPanel";
			this.Size = new System.Drawing.Size(689, 465);
			this.actMenu.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ContextMenuStrip actMenu;
        private System.Windows.Forms.ToolStripMenuItem okrolMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vudvorMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preokrolMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boysOutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem girlsOutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fuckMenuItem;
        private System.Windows.Forms.ToolStripMenuItem печатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vaccMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ToolStripMenuItem setNestMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countChangedMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader7;

    }
}
