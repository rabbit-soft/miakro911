namespace rabdump
{
    partial class GeneralPanel
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chStartUp = new System.Windows.Forms.CheckBox();
            this.tb7zPath = new System.Windows.Forms.TextBox();
            this.bt7ZipPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMysqlPath = new System.Windows.Forms.TextBox();
            this.btMysqlPath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbRemote = new System.Windows.Forms.GroupBox();
            this.tbServerUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gbRemote.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chStartUp);
            this.groupBox1.Controls.Add(this.tb7zPath);
            this.groupBox1.Controls.Add(this.bt7ZipPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbMysqlPath);
            this.groupBox1.Controls.Add(this.btMysqlPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 133);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // chStartUp
            // 
            this.chStartUp.AutoSize = true;
            this.chStartUp.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chStartUp.Location = new System.Drawing.Point(9, 100);
            this.chStartUp.Name = "chStartUp";
            this.chStartUp.Size = new System.Drawing.Size(196, 17);
            this.chStartUp.TabIndex = 8;
            this.chStartUp.Text = "Запускать при загрузке Windows";
            this.chStartUp.UseVisualStyleBackColor = true;
            this.chStartUp.CheckedChanged += new System.EventHandler(this.optValue_Changed);
            // 
            // tb7zPath
            // 
            this.tb7zPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb7zPath.Location = new System.Drawing.Point(33, 74);
            this.tb7zPath.Name = "tb7zPath";
            this.tb7zPath.Size = new System.Drawing.Size(261, 20);
            this.tb7zPath.TabIndex = 6;
            this.tb7zPath.TabStop = false;
            this.tb7zPath.TextChanged += new System.EventHandler(this.optValue_Changed);
            // 
            // bt7ZipPath
            // 
            this.bt7ZipPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt7ZipPath.Location = new System.Drawing.Point(300, 72);
            this.bt7ZipPath.Name = "bt7ZipPath";
            this.bt7ZipPath.Size = new System.Drawing.Size(28, 23);
            this.bt7ZipPath.TabIndex = 7;
            this.bt7ZipPath.Text = "...";
            this.bt7ZipPath.UseVisualStyleBackColor = true;
            this.bt7ZipPath.Click += new System.EventHandler(this.bt7ZipPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Путь к архиватороу 7Zip:";
            // 
            // tbMysqlPath
            // 
            this.tbMysqlPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMysqlPath.Location = new System.Drawing.Point(33, 32);
            this.tbMysqlPath.Name = "tbMysqlPath";
            this.tbMysqlPath.Size = new System.Drawing.Size(261, 20);
            this.tbMysqlPath.TabIndex = 3;
            this.tbMysqlPath.TabStop = false;
            this.tbMysqlPath.TextChanged += new System.EventHandler(this.optValue_Changed);
            // 
            // btMysqlPath
            // 
            this.btMysqlPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btMysqlPath.Location = new System.Drawing.Point(300, 30);
            this.btMysqlPath.Name = "btMysqlPath";
            this.btMysqlPath.Size = new System.Drawing.Size(28, 23);
            this.btMysqlPath.TabIndex = 4;
            this.btMysqlPath.Text = "...";
            this.btMysqlPath.UseVisualStyleBackColor = true;
            this.btMysqlPath.Click += new System.EventHandler(this.btMysqlPath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Путь к папке с MySQL сервером:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = "MySQL Server 5.1";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "7za";
            this.openFileDialog1.Filter = "Исполняемый|*.exe";
            // 
            // gbRemote
            // 
            this.gbRemote.Controls.Add(this.tbServerUrl);
            this.gbRemote.Controls.Add(this.label3);
            this.gbRemote.Location = new System.Drawing.Point(3, 142);
            this.gbRemote.Name = "gbRemote";
            this.gbRemote.Size = new System.Drawing.Size(334, 67);
            this.gbRemote.TabIndex = 7;
            this.gbRemote.TabStop = false;
            // 
            // tbServerUrl
            // 
            this.tbServerUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServerUrl.Location = new System.Drawing.Point(33, 32);
            this.tbServerUrl.Name = "tbServerUrl";
            this.tbServerUrl.Size = new System.Drawing.Size(261, 20);
            this.tbServerUrl.TabIndex = 5;
            this.tbServerUrl.TabStop = false;
            this.tbServerUrl.TextChanged += new System.EventHandler(this.optValue_Changed);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Адрес удаленного сервера:";
            // 
            // GeneralPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbRemote);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(340, 315);
            this.Name = "GeneralPanel";
            this.Size = new System.Drawing.Size(340, 315);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbRemote.ResumeLayout(false);
            this.gbRemote.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chStartUp;
        private System.Windows.Forms.TextBox tb7zPath;
        private System.Windows.Forms.Button bt7ZipPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMysqlPath;
        private System.Windows.Forms.Button btMysqlPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox gbRemote;
        private System.Windows.Forms.TextBox tbServerUrl;
        private System.Windows.Forms.Label label3;
    }
}
