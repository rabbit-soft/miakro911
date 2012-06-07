namespace rabdump
{
    partial class DongleUpdater
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbOrgName = new System.Windows.Forms.TextBox();
            this.tbFarms = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbEndDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbStartDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbFlags = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btUpdate = new System.Windows.Forms.Button();
            this.tbMoney = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btPayments = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(282, 348);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "ОК";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(201, 348);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Название организации:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbOrgName
            // 
            this.tbOrgName.Enabled = false;
            this.tbOrgName.Location = new System.Drawing.Point(153, 9);
            this.tbOrgName.Name = "tbOrgName";
            this.tbOrgName.Size = new System.Drawing.Size(200, 20);
            this.tbOrgName.TabIndex = 3;
            // 
            // tbFarms
            // 
            this.tbFarms.Enabled = false;
            this.tbFarms.Location = new System.Drawing.Point(273, 35);
            this.tbFarms.Name = "tbFarms";
            this.tbFarms.Size = new System.Drawing.Size(80, 20);
            this.tbFarms.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(187, 35);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Кол-во ферм:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbEndDate);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbStartDate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 53);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Период действия ключа";
            // 
            // tbEndDate
            // 
            this.tbEndDate.Enabled = false;
            this.tbEndDate.Location = new System.Drawing.Point(224, 20);
            this.tbEndDate.Name = "tbEndDate";
            this.tbEndDate.Size = new System.Drawing.Size(100, 20);
            this.tbEndDate.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(168, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "По дату:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbStartDate
            // 
            this.tbStartDate.Enabled = false;
            this.tbStartDate.Location = new System.Drawing.Point(62, 20);
            this.tbStartDate.Name = "tbStartDate";
            this.tbStartDate.Size = new System.Drawing.Size(100, 20);
            this.tbStartDate.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "С даты:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbFlags
            // 
            this.lbFlags.FormattingEnabled = true;
            this.lbFlags.Location = new System.Drawing.Point(34, 146);
            this.lbFlags.Name = "lbFlags";
            this.lbFlags.Size = new System.Drawing.Size(300, 134);
            this.lbFlags.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(79, 120);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(210, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Доступный функционал";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btUpdate
            // 
            this.btUpdate.Location = new System.Drawing.Point(227, 285);
            this.btUpdate.Name = "btUpdate";
            this.btUpdate.Size = new System.Drawing.Size(101, 23);
            this.btUpdate.TabIndex = 11;
            this.btUpdate.Text = "Обновить ключ";
            this.btUpdate.UseVisualStyleBackColor = true;
            this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
            // 
            // tbMoney
            // 
            this.tbMoney.Enabled = false;
            this.tbMoney.Location = new System.Drawing.Point(141, 286);
            this.tbMoney.Name = "tbMoney";
            this.tbMoney.Size = new System.Drawing.Size(80, 20);
            this.tbMoney.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(40, 286);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Денег на счету:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btPayments
            // 
            this.btPayments.Location = new System.Drawing.Point(12, 348);
            this.btPayments.Name = "btPayments";
            this.btPayments.Size = new System.Drawing.Size(114, 23);
            this.btPayments.TabIndex = 14;
            this.btPayments.Text = "Операции по счету";
            this.btPayments.UseVisualStyleBackColor = true;
            this.btPayments.Click += new System.EventHandler(this.btPayments_Click);
            // 
            // DongleUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 383);
            this.Controls.Add(this.btPayments);
            this.Controls.Add(this.tbMoney);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btUpdate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbFlags);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbFarms);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbOrgName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DongleUpdater";
            this.Text = "Управление  ключами защиты";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbOrgName;
        private System.Windows.Forms.TextBox tbFarms;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbEndDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbStartDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbFlags;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btUpdate;
        private System.Windows.Forms.TextBox tbMoney;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btPayments;
    }
}