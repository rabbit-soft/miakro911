namespace rabnet.panels
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
            this.miMeal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.lvVictims = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBreed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chGroup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.lvMeat = new System.Windows.Forms.ListView();
            this.chProduct = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTSell = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTSumm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTWeight = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.lvButcherDates = new System.Windows.Forms.ListView();
            this.chDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chProd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.miMeal.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // miMeal
            // 
            this.miMeal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDelete});
            this.miMeal.Name = "actMenu";
            this.miMeal.Size = new System.Drawing.Size(119, 26);
            // 
            // miDelete
            // 
            this.miDelete.Name = "miDelete";
            this.miDelete.Size = new System.Drawing.Size(118, 22);
            this.miDelete.Text = "Удалить";
            this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
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
            this.splitContainer1.Size = new System.Drawing.Size(909, 614);
            this.splitContainer1.SplitterDistance = 612;
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
            this.splitContainer2.Size = new System.Drawing.Size(612, 614);
            this.splitContainer2.SplitterDistance = 327;
            this.splitContainer2.SplitterWidth = 10;
            this.splitContainer2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(612, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Убитые кролики";
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
            this.chBreed,
            this.chGroup,
            this.chAddress});
            this.lvVictims.FullRowSelect = true;
            this.lvVictims.GridLines = true;
            this.lvVictims.Location = new System.Drawing.Point(0, 23);
            this.lvVictims.Name = "lvVictims";
            this.lvVictims.Size = new System.Drawing.Size(612, 304);
            this.lvVictims.TabIndex = 0;
            this.lvVictims.UseCompatibleStateImageBehavior = false;
            this.lvVictims.View = System.Windows.Forms.View.Details;
            this.lvVictims.SelectedIndexChanged += new System.EventHandler(this.lvVictims_SelectedIndexChanged);
            // 
            // chName
            // 
            this.chName.Text = "Имя";
            this.chName.Width = 164;
            // 
            // chAge
            // 
            this.chAge.Text = "Возраст";
            this.chAge.Width = 67;
            // 
            // chBreed
            // 
            this.chBreed.Text = "Порода";
            this.chBreed.Width = 165;
            // 
            // chGroup
            // 
            this.chGroup.Text = "Количество";
            this.chGroup.Width = 85;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(612, 20);
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
            this.chTSell,
            this.chTSumm,
            this.chTWeight,
            this.chUser});
            this.lvMeat.ContextMenuStrip = this.miMeal;
            this.lvMeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvMeat.FullRowSelect = true;
            this.lvMeat.GridLines = true;
            this.lvMeat.Location = new System.Drawing.Point(0, 23);
            this.lvMeat.Name = "lvMeat";
            this.lvMeat.Size = new System.Drawing.Size(612, 254);
            this.lvMeat.TabIndex = 0;
            this.lvMeat.UseCompatibleStateImageBehavior = false;
            this.lvMeat.View = System.Windows.Forms.View.Details;
            // 
            // chProduct
            // 
            this.chProduct.Text = "Продукция";
            this.chProduct.Width = 118;
            // 
            // chTSell
            // 
            this.chTSell.Text = "Продано";
            this.chTSell.Width = 96;
            // 
            // chTSumm
            // 
            this.chTSumm.Text = "На сумму";
            this.chTSumm.Width = 96;
            // 
            // chTWeight
            // 
            this.chTWeight.Text = "Вес";
            this.chTWeight.Width = 93;
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
            this.label1.Size = new System.Drawing.Size(287, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Убои";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lvButcherDates
            // 
            this.lvButcherDates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvButcherDates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDate,
            this.chCount,
            this.chProd});
            this.lvButcherDates.FullRowSelect = true;
            this.lvButcherDates.GridLines = true;
            this.lvButcherDates.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvButcherDates.Location = new System.Drawing.Point(0, 23);
            this.lvButcherDates.MultiSelect = false;
            this.lvButcherDates.Name = "lvButcherDates";
            this.lvButcherDates.Size = new System.Drawing.Size(287, 591);
            this.lvButcherDates.TabIndex = 0;
            this.lvButcherDates.UseCompatibleStateImageBehavior = false;
            this.lvButcherDates.View = System.Windows.Forms.View.Details;
            this.lvButcherDates.SelectedIndexChanged += new System.EventHandler(this.lvButcherDates_SelectedIndexChanged);
            // 
            // chDate
            // 
            this.chDate.Text = "Дата забоя";
            this.chDate.Width = 84;
            // 
            // chCount
            // 
            this.chCount.Text = "Убито";
            this.chCount.Width = 63;
            // 
            // chProd
            // 
            this.chProd.Text = "Продукция";
            this.chProd.Width = 87;
            // 
            // chAddress
            // 
            this.chAddress.Text = "Адрес";
            // 
            // ButcherPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ButcherPanel";
            this.Size = new System.Drawing.Size(909, 614);
            this.Load += new System.EventHandler(this.ButcherPanel_Load);
            this.miMeal.ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip miMeal;
        private System.Windows.Forms.ListView lvMeat;
        private System.Windows.Forms.ColumnHeader chProduct;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader chUser;
        private System.Windows.Forms.ColumnHeader chProd;
        private System.Windows.Forms.ColumnHeader chTSell;
        private System.Windows.Forms.ColumnHeader chTSumm;
        private System.Windows.Forms.ColumnHeader chTWeight;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
        private System.Windows.Forms.ColumnHeader chAddress;
    }
}
