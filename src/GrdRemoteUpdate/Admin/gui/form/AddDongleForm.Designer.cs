namespace AdminGRD
{
    partial class AddDongleForm
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.tbDongleId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudFarms = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.chbFlags = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpTimeFlags = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.tbPrice = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.nudMonths = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chbTimeFlags = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudFarms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonths)).BeginInit();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(126, 497);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Location = new System.Drawing.Point(207, 497);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 2;
            this.btOk.Text = "Готово";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // tbDongleId
            // 
            this.tbDongleId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDongleId.Enabled = false;
            this.tbDongleId.Location = new System.Drawing.Point(116, 9);
            this.tbDongleId.Name = "tbDongleId";
            this.tbDongleId.Size = new System.Drawing.Size(167, 20);
            this.tbDongleId.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "ID ключа";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Кол-во ферм";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudFarms
            // 
            this.nudFarms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudFarms.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudFarms.Location = new System.Drawing.Point(116, 35);
            this.nudFarms.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudFarms.Name = "nudFarms";
            this.nudFarms.Size = new System.Drawing.Size(166, 20);
            this.nudFarms.TabIndex = 13;
            this.nudFarms.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudFarms.ValueChanged += new System.EventHandler(this.nudFarms_ValueChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 22);
            this.label2.TabIndex = 14;
            this.label2.Text = "Флаги";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chbFlags
            // 
            this.chbFlags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbFlags.FormattingEnabled = true;
            this.chbFlags.Location = new System.Drawing.Point(116, 61);
            this.chbFlags.Name = "chbFlags";
            this.chbFlags.Size = new System.Drawing.Size(166, 139);
            this.chbFlags.TabIndex = 16;
            this.chbFlags.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ch_ItemCheck);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "С даты";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpStartDate.Location = new System.Drawing.Point(115, 206);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(167, 20);
            this.dtpStartDate.TabIndex = 18;
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEndDate.Location = new System.Drawing.Point(115, 230);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(167, 20);
            this.dtpEndDate.TabIndex = 20;
            this.dtpEndDate.ValueChanged += new System.EventHandler(this.dtpEndDate_ValueChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 20);
            this.label4.TabIndex = 19;
            this.label4.Text = "По дату";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(9, 284);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 20);
            this.label6.TabIndex = 21;
            this.label6.Text = "Врем. флаги";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpTimeFlags
            // 
            this.dtpTimeFlags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpTimeFlags.Location = new System.Drawing.Point(115, 429);
            this.dtpTimeFlags.Name = "dtpTimeFlags";
            this.dtpTimeFlags.Size = new System.Drawing.Size(166, 20);
            this.dtpTimeFlags.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(10, 431);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 20);
            this.label7.TabIndex = 24;
            this.label7.Text = "Врем. флаги до";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbPrice
            // 
            this.tbPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPrice.Enabled = false;
            this.tbPrice.Location = new System.Drawing.Point(115, 462);
            this.tbPrice.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.tbPrice.Name = "tbPrice";
            this.tbPrice.Size = new System.Drawing.Size(166, 20);
            this.tbPrice.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Location = new System.Drawing.Point(12, 462);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 20);
            this.label8.TabIndex = 26;
            this.label8.Text = "Сколько стоит";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudMonths
            // 
            this.nudMonths.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudMonths.Location = new System.Drawing.Point(197, 256);
            this.nudMonths.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMonths.Name = "nudMonths";
            this.nudMonths.Size = new System.Drawing.Size(84, 20);
            this.nudMonths.TabIndex = 27;
            this.nudMonths.ValueChanged += new System.EventHandler(this.nudMonths_ValueChanged);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(115, 256);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 20);
            this.label9.TabIndex = 28;
            this.label9.Text = "На месяцев";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chbTimeFlags
            // 
            this.chbTimeFlags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chbTimeFlags.FormattingEnabled = true;
            this.chbTimeFlags.Location = new System.Drawing.Point(115, 284);
            this.chbTimeFlags.Name = "chbTimeFlags";
            this.chbTimeFlags.Size = new System.Drawing.Size(166, 139);
            this.chbTimeFlags.TabIndex = 29;
            this.chbTimeFlags.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ch_ItemCheck);
            // 
            // AddDongleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 532);
            this.Controls.Add(this.chbTimeFlags);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.nudMonths);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbPrice);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dtpTimeFlags);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chbFlags);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudFarms);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDongleId);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddDongleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Прошить ключ для: ";
            ((System.ComponentModel.ISupportInitialize)(this.nudFarms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonths)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.TextBox tbDongleId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudFarms;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox chbFlags;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpTimeFlags;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbPrice;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudMonths;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckedListBox chbTimeFlags;
    }
}