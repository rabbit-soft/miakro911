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
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBreed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWeight = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFlags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAverAge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chNotes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.tvGens = new rabnet.RabGenTreeView();
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
            this.chName,
            this.chSex,
            this.chAge,
            this.chBreed,
            this.chWeight,
            this.chStatus,
            this.chFlags,
            this.chCount,
            this.chAverAge,
            this.chRate,
            this.chClass,
            this.chAddress,
            this.chNotes});
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
            // chName
            // 
            this.chName.Text = "Имя";
            this.chName.Width = 122;
            // 
            // chSex
            // 
            this.chSex.Text = "Пол";
            this.chSex.Width = 41;
            // 
            // chAge
            // 
            this.chAge.Text = "Возраст";
            this.chAge.Width = 47;
            // 
            // chBreed
            // 
            this.chBreed.Text = "Порода";
            this.chBreed.Width = 48;
            // 
            // chWeight
            // 
            this.chWeight.Text = "Вес";
            this.chWeight.Width = 38;
            // 
            // chStatus
            // 
            this.chStatus.Text = "Статус";
            this.chStatus.Width = 50;
            // 
            // chFlags
            // 
            this.chFlags.Text = "Пометки";
            this.chFlags.Width = 35;
            // 
            // chCount
            // 
            this.chCount.Text = "N";
            this.chCount.Width = 31;
            // 
            // chAverAge
            // 
            this.chAverAge.Text = "СрВ";
            this.chAverAge.Width = 36;
            // 
            // chRate
            // 
            this.chRate.Text = "Рейтинг";
            this.chRate.Width = 46;
            // 
            // chClass
            // 
            this.chClass.Text = "Класс";
            this.chClass.Width = 41;
            // 
            // chAddress
            // 
            this.chAddress.Text = "Адрес";
            this.chAddress.Width = 90;
            // 
            // chNotes
            // 
            this.chNotes.Text = "Заметки";
            this.chNotes.Width = 64;
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
            this.tvGens.MaxNodesCount = 1;
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
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chSex;
        private System.Windows.Forms.ColumnHeader chAge;
        private System.Windows.Forms.ColumnHeader chBreed;
        private System.Windows.Forms.ColumnHeader chWeight;
        private System.Windows.Forms.ColumnHeader chStatus;
        private System.Windows.Forms.ColumnHeader chFlags;
        private System.Windows.Forms.ColumnHeader chCount;
        private System.Windows.Forms.ColumnHeader chAverAge;
        private System.Windows.Forms.ColumnHeader chRate;
        private System.Windows.Forms.ColumnHeader chClass;
        private System.Windows.Forms.ColumnHeader chAddress;
        private System.Windows.Forms.ColumnHeader chNotes;
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
