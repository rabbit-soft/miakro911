﻿namespace rabnet.panels
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
            this.lvZooTech = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.actMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.okrolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vudvorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countChangedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preokrolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boysOutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.girlsOutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fuckMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miLust = new System.Windows.Forms.ToolStripMenuItem();
            this.vaccMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setNestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miBoysByOne = new System.Windows.Forms.ToolStripMenuItem();
            this.printMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.lvLogs = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.miSpermTake = new System.Windows.Forms.ToolStripMenuItem();
            this.actMenu.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvZooTech
            // 
            this.lvZooTech.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvZooTech.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader6});
            this.lvZooTech.ContextMenuStrip = this.actMenu;
            this.lvZooTech.FullRowSelect = true;
            this.lvZooTech.GridLines = true;
            this.lvZooTech.HideSelection = false;
            this.lvZooTech.Location = new System.Drawing.Point(3, 3);
            this.lvZooTech.MultiSelect = false;
            this.lvZooTech.Name = "lvZooTech";
            this.lvZooTech.OwnerDraw = true;
            this.lvZooTech.Size = new System.Drawing.Size(677, 266);
            this.lvZooTech.TabIndex = 0;
            this.lvZooTech.UseCompatibleStateImageBehavior = false;
            this.lvZooTech.View = System.Windows.Forms.View.Details;
            this.lvZooTech.SelectedIndexChanged += new System.EventHandler(this.lvZooTech_SelectedIndexChanged);
            this.lvZooTech.DoubleClick += new System.EventHandler(this.MenuItem_Click);
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
            this.columnHeader8.Text = "Доп.Инф.";
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
            this.miLust,
            this.vaccMenuItem,
            this.setNestMenuItem,
            this.miBoysByOne,
            this.miSpermTake,
            this.printMenuItem});
            this.actMenu.Name = "actMenu";
            this.actMenu.Size = new System.Drawing.Size(265, 334);
            this.actMenu.Opening += new System.ComponentModel.CancelEventHandler(this.actMenu_Opening);
            // 
            // okrolMenuItem
            // 
            this.okrolMenuItem.Name = "okrolMenuItem";
            this.okrolMenuItem.Size = new System.Drawing.Size(264, 22);
            this.okrolMenuItem.Text = "Принять окрол";
            this.okrolMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // vudvorMenuItem
            // 
            this.vudvorMenuItem.Name = "vudvorMenuItem";
            this.vudvorMenuItem.Size = new System.Drawing.Size(264, 22);
            this.vudvorMenuItem.Text = "Выдворение";
            this.vudvorMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // countsMenuItem
            // 
            this.countsMenuItem.Name = "countsMenuItem";
            this.countsMenuItem.Size = new System.Drawing.Size(264, 22);
            this.countsMenuItem.Text = "Подсчет гнездовых/подсосных";
            this.countsMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // countChangedMenuItem
            // 
            this.countChangedMenuItem.Name = "countChangedMenuItem";
            this.countChangedMenuItem.Size = new System.Drawing.Size(264, 22);
            this.countChangedMenuItem.Text = "Изменилось количество крольчат";
            this.countChangedMenuItem.Click += new System.EventHandler(this.countChangedMenuItem_Click);
            // 
            // preokrolMenuItem
            // 
            this.preokrolMenuItem.Name = "preokrolMenuItem";
            this.preokrolMenuItem.Size = new System.Drawing.Size(264, 22);
            this.preokrolMenuItem.Text = "Предокрольный осмотр";
            this.preokrolMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // boysOutMenuItem
            // 
            this.boysOutMenuItem.Name = "boysOutMenuItem";
            this.boysOutMenuItem.Size = new System.Drawing.Size(264, 22);
            this.boysOutMenuItem.Text = "Отсадить мальчиков";
            this.boysOutMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // girlsOutMenuItem
            // 
            this.girlsOutMenuItem.Name = "girlsOutMenuItem";
            this.girlsOutMenuItem.Size = new System.Drawing.Size(264, 22);
            this.girlsOutMenuItem.Text = "Отсадить девочек";
            this.girlsOutMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // fuckMenuItem
            // 
            this.fuckMenuItem.Name = "fuckMenuItem";
            this.fuckMenuItem.Size = new System.Drawing.Size(264, 22);
            this.fuckMenuItem.Text = "Случить";
            this.fuckMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // miLust
            // 
            this.miLust.Name = "miLust";
            this.miLust.Size = new System.Drawing.Size(264, 22);
            this.miLust.Text = "Стимуляция";
            this.miLust.Click += new System.EventHandler(this.miLust_Click);
            // 
            // vaccMenuItem
            // 
            this.vaccMenuItem.Name = "vaccMenuItem";
            this.vaccMenuItem.Size = new System.Drawing.Size(264, 22);
            this.vaccMenuItem.Text = "Привить";
            this.vaccMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // setNestMenuItem
            // 
            this.setNestMenuItem.Name = "setNestMenuItem";
            this.setNestMenuItem.Size = new System.Drawing.Size(264, 22);
            this.setNestMenuItem.Text = "Установить гнездовье";
            this.setNestMenuItem.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // miBoysByOne
            // 
            this.miBoysByOne.Name = "miBoysByOne";
            this.miBoysByOne.Size = new System.Drawing.Size(264, 22);
            this.miBoysByOne.Text = "Рассадка по одному";
            this.miBoysByOne.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // printMenuItem
            // 
            this.printMenuItem.Name = "printMenuItem";
            this.printMenuItem.Size = new System.Drawing.Size(264, 22);
            this.printMenuItem.Text = "Печать";
            this.printMenuItem.Click += new System.EventHandler(this.printMenuItem_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.lvZooTech);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.lvLogs);
            this.splitContainer1.Size = new System.Drawing.Size(683, 459);
            this.splitContainer1.SplitterDistance = 272;
            this.splitContainer1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(683, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "События (Логи)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lvLogs
            // 
            this.lvLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader15,
            this.columnHeader14});
            this.lvLogs.ForeColor = System.Drawing.Color.Black;
            this.lvLogs.FullRowSelect = true;
            this.lvLogs.GridLines = true;
            this.lvLogs.HideSelection = false;
            this.lvLogs.Location = new System.Drawing.Point(3, 22);
            this.lvLogs.Name = "lvLogs";
            this.lvLogs.Size = new System.Drawing.Size(677, 158);
            this.lvLogs.TabIndex = 0;
            this.lvLogs.UseCompatibleStateImageBehavior = false;
            this.lvLogs.View = System.Windows.Forms.View.Details;
            this.lvLogs.SelectedIndexChanged += new System.EventHandler(this.lvZooTech_SelectedIndexChanged);
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
            // miSpermTake
            // 
            this.miSpermTake.Name = "miSpermTake";
            this.miSpermTake.Size = new System.Drawing.Size(264, 22);
            this.miSpermTake.Text = "Забор материала у самца";
            this.miSpermTake.Click += new System.EventHandler(this.MenuItem_Click);
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

        private System.Windows.Forms.ListView lvZooTech;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvLogs;
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
        private System.Windows.Forms.ToolStripMenuItem printMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vaccMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ToolStripMenuItem setNestMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countChangedMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem miBoysByOne;
        private System.Windows.Forms.ToolStripMenuItem miLust;
        private System.Windows.Forms.ToolStripMenuItem miSpermTake;

    }
}
