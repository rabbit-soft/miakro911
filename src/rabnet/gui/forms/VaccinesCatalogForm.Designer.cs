namespace rabnet.forms
{
    partial class VaccinesCatalogForm
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chAfter = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.chZoo = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.chDoOnce = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btAddRow = new System.Windows.Forms.Button();
            this.btSpetial = new System.Windows.Forms.Button();
            this.gbLust = new System.Windows.Forms.GroupBox();
            this.btLustSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudAge = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudLustDuration = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.gbLust.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLustDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chID,
            this.chName,
            this.chAge,
            this.chDuration,
            this.chAfter,
            this.chZoo,
            this.chDoOnce});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(590, 309);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            // 
            // chID
            // 
            this.chID.HeaderText = "№";
            this.chID.Name = "chID";
            this.chID.ReadOnly = true;
            this.chID.Width = 40;
            // 
            // chName
            // 
            this.chName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.chName.HeaderText = "Название";
            this.chName.Name = "chName";
            // 
            // chAge
            // 
            this.chAge.HeaderText = "Назначать с...(дней)";
            this.chAge.Name = "chAge";
            this.chAge.Width = 80;
            // 
            // chDuration
            // 
            this.chDuration.HeaderText = "Срок действия (дней)";
            this.chDuration.Name = "chDuration";
            this.chDuration.Width = 110;
            // 
            // chAfter
            // 
            this.chAfter.HeaderText = "После";
            this.chAfter.Name = "chAfter";
            this.chAfter.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.chAfter.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // chZoo
            // 
            this.chZoo.HeaderText = "Назначать в ЗооТехПлане";
            this.chZoo.Name = "chZoo";
            this.chZoo.Width = 80;
            // 
            // chDoOnce
            // 
            this.chDoOnce.HeaderText = "Назначать единожды";
            this.chDoOnce.Name = "chDoOnce";
            this.chDoOnce.Width = 80;
            // 
            // btAddRow
            // 
            this.btAddRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btAddRow.Location = new System.Drawing.Point(12, 327);
            this.btAddRow.Name = "btAddRow";
            this.btAddRow.Size = new System.Drawing.Size(115, 23);
            this.btAddRow.TabIndex = 2;
            this.btAddRow.Text = "Добавить новую";
            this.btAddRow.UseVisualStyleBackColor = true;
            this.btAddRow.Click += new System.EventHandler(this.btAddRow_Click);
            // 
            // btSpetial
            // 
            this.btSpetial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSpetial.Location = new System.Drawing.Point(507, 327);
            this.btSpetial.Name = "btSpetial";
            this.btSpetial.Size = new System.Drawing.Size(95, 23);
            this.btSpetial.TabIndex = 3;
            this.btSpetial.Tag = "0";
            this.btSpetial.Text = "Спец. уколы >>";
            this.btSpetial.UseVisualStyleBackColor = true;
            this.btSpetial.Click += new System.EventHandler(this.btSpetial_Click);
            // 
            // gbLust
            // 
            this.gbLust.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbLust.Controls.Add(this.btLustSave);
            this.gbLust.Controls.Add(this.label3);
            this.gbLust.Controls.Add(this.label4);
            this.gbLust.Controls.Add(this.nudAge);
            this.gbLust.Controls.Add(this.label2);
            this.gbLust.Controls.Add(this.nudLustDuration);
            this.gbLust.Controls.Add(this.label1);
            this.gbLust.Location = new System.Drawing.Point(618, 12);
            this.gbLust.Name = "gbLust";
            this.gbLust.Size = new System.Drawing.Size(197, 100);
            this.gbLust.TabIndex = 4;
            this.gbLust.TabStop = false;
            this.gbLust.Text = "Стимуляция крольчихи";
            // 
            // btLustSave
            // 
            this.btLustSave.Location = new System.Drawing.Point(61, 71);
            this.btLustSave.Name = "btLustSave";
            this.btLustSave.Size = new System.Drawing.Size(75, 23);
            this.btLustSave.TabIndex = 6;
            this.btLustSave.Text = "Сохранить";
            this.btLustSave.UseVisualStyleBackColor = true;
            this.btLustSave.Click += new System.EventHandler(this.btLustSave_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(155, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "дней";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(155, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "день";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudAge
            // 
            this.nudAge.Location = new System.Drawing.Point(99, 42);
            this.nudAge.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAge.Name = "nudAge";
            this.nudAge.Size = new System.Drawing.Size(50, 20);
            this.nudAge.TabIndex = 3;
            this.toolTip1.SetToolTip(this.nudAge, "Назначать случку\\вызку самки на день после стимуляции");
            this.nudAge.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Назначать на";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudLustDuration
            // 
            this.nudLustDuration.Location = new System.Drawing.Point(99, 16);
            this.nudLustDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLustDuration.Name = "nudLustDuration";
            this.nudLustDuration.Size = new System.Drawing.Size(50, 20);
            this.nudLustDuration.TabIndex = 1;
            this.toolTip1.SetToolTip(this.nudLustDuration, "Сколько дней действует  стимуляция");
            this.nudLustDuration.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudLustDuration.ValueChanged += new System.EventHandler(this.nudLustDuration_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Действительна";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "№";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Название";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Назначать с...(дней)";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Срок действия (дней)";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 110;
            // 
            // VaccinesCatalogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 362);
            this.Controls.Add(this.gbLust);
            this.Controls.Add(this.btSpetial);
            this.Controls.Add(this.btAddRow);
            this.Controls.Add(this.dataGridView1);
            this.Name = "VaccinesCatalogForm";
            this.ShowIcon = false;
            this.Text = "Список прививок";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.gbLust.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudAge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLustDuration)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Button btAddRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn chID;
        private System.Windows.Forms.DataGridViewTextBoxColumn chName;
        private System.Windows.Forms.DataGridViewTextBoxColumn chAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn chDuration;
        private System.Windows.Forms.DataGridViewComboBoxColumn chAfter;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chZoo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chDoOnce;
        private System.Windows.Forms.Button btSpetial;
        private System.Windows.Forms.GroupBox gbLust;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudAge;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudLustDuration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btLustSave;
    }
}