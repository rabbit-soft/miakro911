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
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.actMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.passportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newRab = new System.Windows.Forms.ToolStripMenuItem();
            this.makeBon = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.KillMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boysoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceYoungersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.placeChMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fuckMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.proholostMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.okrolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countKidsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.svidMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plemMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.realizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replacePlanMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miGenetic = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.показатьНомерToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tvGens = new RabGenTreeView();
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
            this.splitContainer1.Panel2.Controls.Add(this.tvGens);
            this.splitContainer1.Size = new System.Drawing.Size(853, 550);
            this.splitContainer1.SplitterDistance = 709;
            this.splitContainer1.TabIndex = 2;
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
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.OwnerDraw = true;
            this.listView1.Size = new System.Drawing.Size(703, 544);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
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
            this.toolStripSeparator1,
            this.KillMenuItem,
            this.replaceMenuItem,
            this.boysoutMenuItem,
            this.replaceYoungersMenuItem,
            this.placeChMenuItem,
            this.fuckMenuItem,
            this.proholostMenuItem,
            this.okrolMenuItem,
            this.countKidsMenuItem,
            this.toolStripSeparator2,
            this.svidMenuItem,
            this.plemMenuItem,
            this.realizeMenuItem,
            this.replacePlanMenuItem,
            this.toolStripSeparator3,
            this.miGenetic,
            this.toolStripSeparator4,
            this.показатьНомерToolStripMenuItem});
            this.actMenu.Name = "actMenu";
            this.actMenu.Size = new System.Drawing.Size(237, 446);
            this.actMenu.Opening += new System.ComponentModel.CancelEventHandler(this.actMenu_Opening);
            // 
            // passportMenuItem
            // 
            this.passportMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.passportMenuItem.Name = "passportMenuItem";
            this.passportMenuItem.Size = new System.Drawing.Size(236, 22);
            this.passportMenuItem.Text = "Паспорт";
            this.passportMenuItem.Click += new System.EventHandler(this.passportMenuItem_Click);
            // 
            // newRab
            // 
            this.newRab.Name = "newRab";
            this.newRab.Size = new System.Drawing.Size(236, 22);
            this.newRab.Text = "Привоз";
            this.newRab.Click += new System.EventHandler(this.newRab_Click);
            // 
            // makeBon
            // 
            this.makeBon.Name = "makeBon";
            this.makeBon.Size = new System.Drawing.Size(236, 22);
            this.makeBon.Text = "Бонитировка";
            this.makeBon.Click += new System.EventHandler(this.makeBon_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(233, 6);
            // 
            // KillMenuItem
            // 
            this.KillMenuItem.Name = "KillMenuItem";
            this.KillMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.KillMenuItem.Size = new System.Drawing.Size(236, 22);
            this.KillMenuItem.Text = "Списание";
            this.KillMenuItem.Click += new System.EventHandler(this.KillMenuItem_Click);
            // 
            // replaceMenuItem
            // 
            this.replaceMenuItem.Name = "replaceMenuItem";
            this.replaceMenuItem.Size = new System.Drawing.Size(236, 22);
            this.replaceMenuItem.Text = "Пересадить";
            this.replaceMenuItem.Click += new System.EventHandler(this.replaceMenuItem_Click);
            // 
            // boysoutMenuItem
            // 
            this.boysoutMenuItem.Name = "boysoutMenuItem";
            this.boysoutMenuItem.Size = new System.Drawing.Size(236, 22);
            this.boysoutMenuItem.Text = "Отсадить мальчиков";
            this.boysoutMenuItem.Visible = false;
            this.boysoutMenuItem.Click += new System.EventHandler(this.boysoutMenuItem_Click);
            // 
            // replaceYoungersMenuItem
            // 
            this.replaceYoungersMenuItem.Name = "replaceYoungersMenuItem";
            this.replaceYoungersMenuItem.Size = new System.Drawing.Size(236, 22);
            this.replaceYoungersMenuItem.Text = "Отсадить молодняк";
            this.replaceYoungersMenuItem.Click += new System.EventHandler(this.replaceYoungersMenuItem_Click);
            // 
            // placeChMenuItem
            // 
            this.placeChMenuItem.Name = "placeChMenuItem";
            this.placeChMenuItem.Size = new System.Drawing.Size(236, 22);
            this.placeChMenuItem.Text = "Жилобмен";
            this.placeChMenuItem.Click += new System.EventHandler(this.placeChMenuItem_Click);
            // 
            // fuckMenuItem
            // 
            this.fuckMenuItem.Name = "fuckMenuItem";
            this.fuckMenuItem.Size = new System.Drawing.Size(236, 22);
            this.fuckMenuItem.Text = "Случка";
            this.fuckMenuItem.Click += new System.EventHandler(this.fuckMenuItem_Click);
            // 
            // proholostMenuItem
            // 
            this.proholostMenuItem.Name = "proholostMenuItem";
            this.proholostMenuItem.Size = new System.Drawing.Size(236, 22);
            this.proholostMenuItem.Text = "Прохолостание";
            this.proholostMenuItem.Click += new System.EventHandler(this.proholostMenuItem_Click);
            // 
            // okrolMenuItem
            // 
            this.okrolMenuItem.Name = "okrolMenuItem";
            this.okrolMenuItem.Size = new System.Drawing.Size(236, 22);
            this.okrolMenuItem.Text = "Принять окрол";
            this.okrolMenuItem.Click += new System.EventHandler(this.okrolMenuItem_Click);
            // 
            // countKidsMenuItem
            // 
            this.countKidsMenuItem.Name = "countKidsMenuItem";
            this.countKidsMenuItem.Size = new System.Drawing.Size(236, 22);
            this.countKidsMenuItem.Text = "Подсчет гнездовых";
            this.countKidsMenuItem.Click += new System.EventHandler(this.countKidsMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(233, 6);
            // 
            // svidMenuItem
            // 
            this.svidMenuItem.Name = "svidMenuItem";
            this.svidMenuItem.Size = new System.Drawing.Size(236, 22);
            this.svidMenuItem.Text = "Племенное свидетельство";
            this.svidMenuItem.Click += new System.EventHandler(this.svidMenuItem_Click);
            // 
            // plemMenuItem
            // 
            this.plemMenuItem.Name = "plemMenuItem";
            this.plemMenuItem.Size = new System.Drawing.Size(236, 22);
            this.plemMenuItem.Text = "Племенной список";
            this.plemMenuItem.Click += new System.EventHandler(this.plemMenuItem_Click);
            // 
            // realizeMenuItem
            // 
            this.realizeMenuItem.Name = "realizeMenuItem";
            this.realizeMenuItem.Size = new System.Drawing.Size(236, 22);
            this.realizeMenuItem.Text = "Кандидаты на реализацию";
            this.realizeMenuItem.Click += new System.EventHandler(this.realizeMenuItem_Click);
            // 
            // replacePlanMenuItem
            // 
            this.replacePlanMenuItem.Name = "replacePlanMenuItem";
            this.replacePlanMenuItem.Size = new System.Drawing.Size(236, 22);
            this.replacePlanMenuItem.Text = "План пересадок";
            this.replacePlanMenuItem.Click += new System.EventHandler(this.replacePlanMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(233, 6);
            // 
            // miGenetic
            // 
            this.miGenetic.Name = "miGenetic";
            this.miGenetic.Size = new System.Drawing.Size(236, 22);
            this.miGenetic.Text = "Показать родословную";
            this.miGenetic.Click += new System.EventHandler(this.GeneticsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(233, 6);
            this.toolStripSeparator4.Visible = false;
            // 
            // показатьНомерToolStripMenuItem
            // 
            this.показатьНомерToolStripMenuItem.Name = "показатьНомерToolStripMenuItem";
            this.показатьНомерToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.Z)));
            this.показатьНомерToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.показатьНомерToolStripMenuItem.Text = "Показать номер";
            this.показатьНомерToolStripMenuItem.Visible = false;
            this.показатьНомерToolStripMenuItem.Click += new System.EventHandler(this.tsmiIDshow_Click);
            // 
            // tvGens
            // 
            this.tvGens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvGens.Location = new System.Drawing.Point(3, 3);
            this.tvGens.Name = "tvGens";
            this.tvGens.Size = new System.Drawing.Size(134, 544);
            this.tvGens.TabIndex = 1;
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
        private RabGenTreeView tvGens;
        private System.Windows.Forms.ContextMenuStrip actMenu;
        private System.Windows.Forms.ToolStripMenuItem passportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newRab;
        private System.Windows.Forms.ToolStripMenuItem makeBon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem proholostMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem placeChMenuItem;
        private System.Windows.Forms.ToolStripMenuItem KillMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countKidsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fuckMenuItem;
        private System.Windows.Forms.ToolStripMenuItem okrolMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boysoutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceYoungersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem svidMenuItem;
        private System.Windows.Forms.ToolStripMenuItem realizeMenuItem;
		private System.Windows.Forms.ToolStripMenuItem miGenetic;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem plemMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replacePlanMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem показатьНомерToolStripMenuItem;
    }
}
