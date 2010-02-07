namespace rabnet
{
    partial class BuildingsPanel
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
            this.farmDrawer1 = new rabnet.FarmDrawer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.actMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.replaceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.killMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFarmMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addBuildingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeFarmMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteBuildingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
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
            this.splitContainer1.Panel1.Controls.Add(this.farmDrawer1);
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.treeView1);
            this.splitContainer1.Size = new System.Drawing.Size(842, 539);
            this.splitContainer1.SplitterDistance = 654;
            this.splitContainer1.TabIndex = 0;
            // 
            // farmDrawer1
            // 
            this.farmDrawer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.farmDrawer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.farmDrawer1.Location = new System.Drawing.Point(3, 0);
            this.farmDrawer1.Name = "farmDrawer1";
            this.farmDrawer1.Size = new System.Drawing.Size(648, 215);
            this.farmDrawer1.TabIndex = 4;
            this.farmDrawer1.ValueChanged += new rabnet.FarmDrawer.FDEventHandler(this.farmDrawer1_ValueChanged);
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
            this.columnHeader8});
            this.listView1.ContextMenuStrip = this.actMenu;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 221);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(648, 315);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.Enter += new System.EventHandler(this.listView1_Enter);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "№";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Ярус";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Отделения";
            this.columnHeader3.Width = 76;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Статус";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Гнездо";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Грелка";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Адрес";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Заметки";
            // 
            // actMenu
            // 
            this.actMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.replaceMenuItem,
            this.killMenuItem,
            this.addFarmMenuItem,
            this.addBuildingMenuItem,
            this.changeFarmMenuItem,
            this.deleteBuildingMenuItem});
            this.actMenu.Name = "actMenu";
            this.actMenu.Size = new System.Drawing.Size(195, 158);
            // 
            // replaceMenuItem
            // 
            this.replaceMenuItem.Name = "replaceMenuItem";
            this.replaceMenuItem.Size = new System.Drawing.Size(194, 22);
            this.replaceMenuItem.Text = "Расселить";
            this.replaceMenuItem.Click += new System.EventHandler(this.replaceMenuItem_Click);
            // 
            // killMenuItem
            // 
            this.killMenuItem.Name = "killMenuItem";
            this.killMenuItem.Size = new System.Drawing.Size(194, 22);
            this.killMenuItem.Text = "Списать";
            this.killMenuItem.Click += new System.EventHandler(this.killMenuItem_Click);
            // 
            // addFarmMenuItem
            // 
            this.addFarmMenuItem.Name = "addFarmMenuItem";
            this.addFarmMenuItem.Size = new System.Drawing.Size(194, 22);
            this.addFarmMenuItem.Text = "Добавить миниферму";
            this.addFarmMenuItem.Click += new System.EventHandler(this.addFarmMenuItem_Click);
            // 
            // addBuildingMenuItem
            // 
            this.addBuildingMenuItem.Name = "addBuildingMenuItem";
            this.addBuildingMenuItem.Size = new System.Drawing.Size(194, 22);
            this.addBuildingMenuItem.Text = "Добавить блок(шед)";
            this.addBuildingMenuItem.Click += new System.EventHandler(this.addBuildingMenuItem_Click);
            // 
            // changeFarmMenuItem
            // 
            this.changeFarmMenuItem.Name = "changeFarmMenuItem";
            this.changeFarmMenuItem.Size = new System.Drawing.Size(194, 22);
            this.changeFarmMenuItem.Text = "Изменить миниферму";
            this.changeFarmMenuItem.Click += new System.EventHandler(this.changeFarmMenuItem_Click);
            // 
            // deleteBuildingMenuItem
            // 
            this.deleteBuildingMenuItem.Name = "deleteBuildingMenuItem";
            this.deleteBuildingMenuItem.Size = new System.Drawing.Size(194, 22);
            this.deleteBuildingMenuItem.Text = "Удалить строение";
            this.deleteBuildingMenuItem.Click += new System.EventHandler(this.deleteBuildingMenuItem_Click);
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.ContextMenuStrip = this.actMenu;
            this.treeView1.HideSelection = false;
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(178, 533);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.Enter += new System.EventHandler(this.treeView1_Enter);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver_1);
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BuildingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "BuildingsPanel";
            this.Size = new System.Drawing.Size(842, 539);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.actMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private FarmDrawer farmDrawer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ContextMenuStrip actMenu;
        private System.Windows.Forms.ToolStripMenuItem replaceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem killMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFarmMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addBuildingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeFarmMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteBuildingMenuItem;
        private System.Windows.Forms.Timer timer1;



    }
}
