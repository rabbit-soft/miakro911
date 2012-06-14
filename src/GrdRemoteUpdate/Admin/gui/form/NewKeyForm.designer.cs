namespace AdminGRD
{
    partial class NewKeyForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudUID = new System.Windows.Forms.NumericUpDown();
            this.btFromFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbHexKey = new System.Windows.Forms.TextBox();
            this.btFromHex = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudUID)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Пароль:";
            // 
            // tbPass
            // 
            this.tbPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPass.Location = new System.Drawing.Point(66, 37);
            this.tbPass.Name = "tbPass";
            this.tbPass.Size = new System.Drawing.Size(231, 20);
            this.tbPass.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "UserID:";
            // 
            // nudUID
            // 
            this.nudUID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nudUID.Location = new System.Drawing.Point(66, 63);
            this.nudUID.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudUID.Name = "nudUID";
            this.nudUID.Size = new System.Drawing.Size(231, 20);
            this.nudUID.TabIndex = 2;
            // 
            // btFromFile
            // 
            this.btFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btFromFile.Location = new System.Drawing.Point(199, 193);
            this.btFromFile.Name = "btFromFile";
            this.btFromFile.Size = new System.Drawing.Size(97, 23);
            this.btFromFile.TabIndex = 8;
            this.btFromFile.Text = "Из файла";
            this.btFromFile.UseVisualStyleBackColor = true;
            this.btFromFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Имя:";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(66, 11);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(231, 20);
            this.tbName.TabIndex = 0;
            // 
            // tbHexKey
            // 
            this.tbHexKey.Location = new System.Drawing.Point(66, 89);
            this.tbHexKey.Multiline = true;
            this.tbHexKey.Name = "tbHexKey";
            this.tbHexKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbHexKey.Size = new System.Drawing.Size(230, 98);
            this.tbHexKey.TabIndex = 3;
            // 
            // btFromHex
            // 
            this.btFromHex.Location = new System.Drawing.Point(66, 193);
            this.btFromHex.Name = "btFromHex";
            this.btFromHex.Size = new System.Drawing.Size(75, 23);
            this.btFromHex.TabIndex = 12;
            this.btFromHex.Text = "Из HexStr";
            this.btFromHex.UseVisualStyleBackColor = true;
            this.btFromHex.Click += new System.EventHandler(this.btFromHex_Click);
            // 
            // NewKeyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 228);
            this.Controls.Add(this.btFromHex);
            this.Controls.Add(this.tbHexKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.btFromFile);
            this.Controls.Add(this.nudUID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbPass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewKeyForm";
            this.Text = "ЗАмутить новый ключ";
            ((System.ComponentModel.ISupportInitialize)(this.nudUID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudUID;
        private System.Windows.Forms.Button btFromFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbHexKey;
        private System.Windows.Forms.Button btFromHex;

    }
}