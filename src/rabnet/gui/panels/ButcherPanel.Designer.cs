namespace rabnet
{
    partial class ButcherPanel
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.lvVictims = new System.Windows.Forms.ListView();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chAge = new System.Windows.Forms.ColumnHeader();
            this.chGroup = new System.Windows.Forms.ColumnHeader();
            this.chBreed = new System.Windows.Forms.ColumnHeader();
            this.label3 = new System.Windows.Forms.Label();
            this.lvMeat = new System.Windows.Forms.ListView();
            this.chProduct = new System.Windows.Forms.ColumnHeader();
            this.chAmount = new System.Windows.Forms.ColumnHeader();
            this.chUser = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.lvButcherDates = new System.Windows.Forms.ListView();
            this.chDate = new System.Windows.Forms.ColumnHeader();
            this.chCount = new System.Windows.Forms.ColumnHeader();
            this.actMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.lvButcherDates);
            this.splitContainer1.Size = new System.Drawing.Size(662, 455);
            this.splitContainer1.SplitterDistance = 476;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.lvVictims);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Panel2.Controls.Add(this.lvMeat);
            this.splitContainer2.Size = new System.Drawing.Size(476, 455);
            this.splitContainer2.SplitterDistance = 221;
            this.splitContainer2.SplitterWidth = 10;
            this.splitContainer2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(476, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Забитые кролики";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lvVictims
            // 
            this.lvVictims.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvVictims.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chAge,
            this.chGroup,
            this.chBreed});
            this.lvVictims.FullRowSelect = true;
            this.lvVictims.GridLines = true;
            this.lvVictims.Location = new System.Drawing.Point(0, 23);
            this.lvVictims.Name = "lvVictims";
            this.lvVictims.Size = new System.Drawing.Size(476, 198);
            this.lvVictims.TabIndex = 0;
            this.lvVictims.UseCompatibleStateImageBehavior = false;
            this.lvVictims.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "Имя";
            this.chName.Width = 126;
            // 
            // chAge
            // 
            this.chAge.Text = "Возраст";
            // 
            // chGroup
            // 
            this.chGroup.Text = "Количество";
            this.chGroup.Width = 85;
            // 
            // chBreed
            // 
            this.chBreed.Text = "Порода";
            this.chBreed.Width = 107;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(476, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Выходная продукция";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lvMeat
            // 
            this.lvMeat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMeat.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chProduct,
            this.chAmount,
            this.chUser});
            this.lvMeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvMeat.FullRowSelect = true;
            this.lvMeat.GridLines = true;
            this.lvMeat.Location = new System.Drawing.Point(0, 23);
            this.lvMeat.Name = "lvMeat";
            this.lvMeat.Size = new System.Drawing.Size(476, 201);
            this.lvMeat.TabIndex = 0;
            this.lvMeat.UseCompatibleStateImageBehavior = false;
            this.lvMeat.View = System.Windows.Forms.View.Details;
            // 
            // chProduct
            // 
            this.chProduct.Text = "Продукция";
            this.chProduct.Width = 88;
            // 
            // chAmount
            // 
            this.chAmount.Text = "Количество";
            this.chAmount.Width = 85;
            // 
            // chUser
            // 
            this.chUser.Text = "Пользователь";
            this.chUser.Width = 120;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Забои";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lvButcherDates
            // 
            this.lvButcherDates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvButcherDates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDate,
            this.chCount});
            this.lvButcherDates.FullRowSelect = true;
            this.lvButcherDates.GridLines = true;
            this.lvButcherDates.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvButcherDates.Location = new System.Drawing.Point(0, 23);
            this.lvButcherDates.MultiSelect = false;
            this.lvButcherDates.Name = "lvButcherDates";
            this.lvButcherDates.Size = new System.Drawing.Size(176, 432);
            this.lvButcherDates.TabIndex = 0;
            this.lvButcherDates.UseCompatibleStateImageBehavior = false;
            this.lvButcherDates.View = System.Windows.Forms.View.Details;
            this.lvButcherDates.SelectedIndexChanged += new System.EventHandler(this.lvButcherDates_SelectedIndexChanged);
            // 
            // chDate
            // 
            this.chDate.Text = "Дата забоя";
            this.chDate.Width = 79;
            // 
            // chCount
            // 
            this.chCount.Text = "Забито";
            this.chCount.Width = 114;
            // 
            // actMenu
            // 
            this.actMenu.Name = "actMenu";
            this.actMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // ButcherPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ButcherPanel";
            this.Size = new System.Drawing.Size(662, 455);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView lvButcherDates;
        private System.Windows.Forms.ColumnHeader chDate;
        private System.Windows.Forms.ColumnHeader chCount;
        private System.Windows.Forms.ListView lvVictims;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chAge;
        private System.Windows.Forms.ColumnHeader chGroup;
        private System.Windows.Forms.ColumnHeader chBreed;
        private System.Windows.Forms.ContextMenuStrip actMenu;
        private System.Windows.Forms.ListView lvMeat;
        private System.Windows.Forms.ColumnHeader chProduct;
        private System.Windows.Forms.ColumnHeader chAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader chUser;
    }
}
