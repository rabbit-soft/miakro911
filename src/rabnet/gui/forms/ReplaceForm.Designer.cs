namespace rabnet.forms
{
    partial class ReplaceForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.cbFilter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btClearAll = new System.Windows.Forms.Button();
            this.btChangeAddresses = new System.Windows.Forms.Button();
            this.btClear = new System.Windows.Forms.Button();
            this.btSeparateByOne = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btSetAllBoys = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btSetAllGirls = new System.Windows.Forms.Button();
            this.btSeparateBoys = new System.Windows.Forms.Button();
            this.btSeparate = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btUniteDown = new System.Windows.Forms.Button();
            this.btUniteUp = new System.Windows.Forms.Button();
            this.btCombine = new System.Windows.Forms.Button();
            this.btAutoReplace = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 39);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(747, 437);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_MultiSelectChanged);
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(674, 482);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(85, 23);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "Пересадить";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btCancel.Location = new System.Drawing.Point(593, 482);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // cbFilter
            // 
            this.cbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFilter.FormattingEnabled = true;
            this.cbFilter.Items.AddRange(new object[] {
            "Все",
            "Крольчихин",
            "Двукрольчихин",
            "Комплексный",
            "Юрта",
            "Кварта",
            "Вертеп",
            "Барин",
            "Хижина"});
            this.cbFilter.Location = new System.Drawing.Point(65, 12);
            this.cbFilter.Name = "cbFilter";
            this.cbFilter.Size = new System.Drawing.Size(140, 21);
            this.cbFilter.TabIndex = 3;
            this.cbFilter.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Фильтр";
            // 
            // btClearAll
            // 
            this.btClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btClearAll.Location = new System.Drawing.Point(641, 10);
            this.btClearAll.Name = "btClearAll";
            this.btClearAll.Size = new System.Drawing.Size(118, 23);
            this.btClearAll.TabIndex = 5;
            this.btClearAll.Text = "Очистить все";
            this.btClearAll.UseVisualStyleBackColor = true;
            this.btClearAll.Click += new System.EventHandler(this.btClearAll_Click);
            // 
            // btChangeAddresses
            // 
            this.btChangeAddresses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btChangeAddresses.Enabled = false;
            this.btChangeAddresses.Location = new System.Drawing.Point(765, 108);
            this.btChangeAddresses.Name = "btChangeAddresses";
            this.btChangeAddresses.Size = new System.Drawing.Size(120, 23);
            this.btChangeAddresses.TabIndex = 6;
            this.btChangeAddresses.Text = "Жилобмен";
            this.btChangeAddresses.UseVisualStyleBackColor = true;
            this.btChangeAddresses.Click += new System.EventHandler(this.btChangeAddresses_Click);
            // 
            // btClear
            // 
            this.btClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btClear.Location = new System.Drawing.Point(764, 39);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(120, 23);
            this.btClear.TabIndex = 7;
            this.btClear.Text = "Очистить";
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // btSeparateByOne
            // 
            this.btSeparateByOne.Location = new System.Drawing.Point(6, 74);
            this.btSeparateByOne.Name = "btSeparateByOne";
            this.btSeparateByOne.Size = new System.Drawing.Size(107, 23);
            this.btSeparateByOne.TabIndex = 8;
            this.btSeparateByOne.Text = "По одному";
            this.btSeparateByOne.UseVisualStyleBackColor = true;
            this.btSeparateByOne.Click += new System.EventHandler(this.btSeparateByOne_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btSetAllBoys);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btSetAllGirls);
            this.groupBox1.Controls.Add(this.btSeparateBoys);
            this.groupBox1.Controls.Add(this.btSeparate);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.btSeparateByOne);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(766, 137);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(119, 206);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Рассадить группу";
            // 
            // btSetAllBoys
            // 
            this.btSetAllBoys.Location = new System.Drawing.Point(6, 177);
            this.btSetAllBoys.Name = "btSetAllBoys";
            this.btSetAllBoys.Size = new System.Drawing.Size(107, 23);
            this.btSetAllBoys.TabIndex = 14;
            this.btSetAllBoys.Text = "Все мальчики";
            this.btSetAllBoys.UseVisualStyleBackColor = true;
            this.btSetAllBoys.Click += new System.EventHandler(this.btSetAllBoys_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(8, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "N";
            // 
            // btSetAllGirls
            // 
            this.btSetAllGirls.Location = new System.Drawing.Point(6, 152);
            this.btSetAllGirls.Name = "btSetAllGirls";
            this.btSetAllGirls.Size = new System.Drawing.Size(107, 23);
            this.btSetAllGirls.TabIndex = 12;
            this.btSetAllGirls.Text = "Все девочки";
            this.btSetAllGirls.UseVisualStyleBackColor = true;
            this.btSetAllGirls.Click += new System.EventHandler(this.btSetAllGirls_Click);
            // 
            // btSeparateBoys
            // 
            this.btSeparateBoys.Location = new System.Drawing.Point(6, 102);
            this.btSeparateBoys.Name = "btSeparateBoys";
            this.btSeparateBoys.Size = new System.Drawing.Size(107, 44);
            this.btSeparateBoys.TabIndex = 11;
            this.btSeparateBoys.Text = "Отделить Мальчиков";
            this.btSeparateBoys.UseVisualStyleBackColor = true;
            this.btSeparateBoys.Click += new System.EventHandler(this.btSeparateBoys_Click);
            // 
            // btSeparate
            // 
            this.btSeparate.Location = new System.Drawing.Point(6, 46);
            this.btSeparate.Name = "btSeparate";
            this.btSeparate.Size = new System.Drawing.Size(107, 23);
            this.btSeparate.TabIndex = 10;
            this.btSeparate.Text = "Отделить";
            this.btSeparate.UseVisualStyleBackColor = true;
            this.btSeparate.Click += new System.EventHandler(this.btSeparate_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(31, 19);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(80, 20);
            this.numericUpDown1.TabIndex = 9;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btUniteDown);
            this.groupBox2.Controls.Add(this.btUniteUp);
            this.groupBox2.Controls.Add(this.btCombine);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(766, 349);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(119, 104);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Объединить";
            // 
            // btUniteDown
            // 
            this.btUniteDown.Location = new System.Drawing.Point(6, 75);
            this.btUniteDown.Name = "btUniteDown";
            this.btUniteDown.Size = new System.Drawing.Size(107, 23);
            this.btUniteDown.TabIndex = 2;
            this.btUniteDown.Text = "Объединить низ";
            this.btUniteDown.UseVisualStyleBackColor = true;
            this.btUniteDown.Visible = false;
            this.btUniteDown.Click += new System.EventHandler(this.btUniteUp_Click);
            // 
            // btUniteUp
            // 
            this.btUniteUp.Location = new System.Drawing.Point(6, 48);
            this.btUniteUp.Name = "btUniteUp";
            this.btUniteUp.Size = new System.Drawing.Size(107, 23);
            this.btUniteUp.TabIndex = 1;
            this.btUniteUp.Text = "Объединить верх";
            this.btUniteUp.UseVisualStyleBackColor = true;
            this.btUniteUp.Visible = false;
            this.btUniteUp.Click += new System.EventHandler(this.btUniteUp_Click);
            // 
            // btCombine
            // 
            this.btCombine.Location = new System.Drawing.Point(6, 19);
            this.btCombine.Name = "btCombine";
            this.btCombine.Size = new System.Drawing.Size(107, 23);
            this.btCombine.TabIndex = 0;
            this.btCombine.Text = "Подсадить";
            this.btCombine.UseVisualStyleBackColor = true;
            this.btCombine.Click += new System.EventHandler(this.btCombine_Click);
            // 
            // btAutoReplace
            // 
            this.btAutoReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAutoReplace.Location = new System.Drawing.Point(765, 68);
            this.btAutoReplace.Name = "btAutoReplace";
            this.btAutoReplace.Size = new System.Drawing.Size(120, 23);
            this.btAutoReplace.TabIndex = 11;
            this.btAutoReplace.Text = "Автоматически";
            this.btAutoReplace.UseVisualStyleBackColor = true;
            this.btAutoReplace.Click += new System.EventHandler(this.btAutoReplace_Click);
            // 
            // ReplaceForm
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(892, 516);
            this.Controls.Add(this.btAutoReplace);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.btChangeAddresses);
            this.Controls.Add(this.btClearAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbFilter);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.dataGridView1);
            this.Location = new System.Drawing.Point(100, 100);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(480, 500);
            this.Name = "ReplaceForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Пересадки";
            this.Load += new System.EventHandler(this.ReplaceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.ComboBox cbFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btClearAll;
        private System.Windows.Forms.Button btChangeAddresses;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.Button btSeparateByOne;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btSeparate;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btUniteUp;
        private System.Windows.Forms.Button btCombine;
        private System.Windows.Forms.Button btSeparateBoys;
        private System.Windows.Forms.Button btSetAllGirls;
        private System.Windows.Forms.Button btAutoReplace;
        private System.Windows.Forms.Button btUniteDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btSetAllBoys;

    }
}