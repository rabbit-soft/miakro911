namespace AdminGRD
{
    partial class LoginFrom
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
            this.lbInfo = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btOwnServer = new System.Windows.Forms.Button();
            this.gbServ = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbServ = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbNewPass = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbNewPassConf = new System.Windows.Forms.TextBox();
            this.lbError = new System.Windows.Forms.Label();
            this.lbNewPass = new System.Windows.Forms.Label();
            this.tbNewPass = new System.Windows.Forms.TextBox();
            this.btKey = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btPassChange = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.gbServ.SuspendLayout();
            this.gbNewPass.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbInfo
            // 
            this.lbInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbInfo.Location = new System.Drawing.Point(0, 0);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(360, 46);
            this.lbInfo.TabIndex = 2;
            this.lbInfo.Text = "Выберите ключ и введите пароль";
            this.lbInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Location = new System.Drawing.Point(273, 297);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 4;
            this.btOk.Text = "Войти";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Location = new System.Drawing.Point(192, 297);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 23);
            this.btExit.TabIndex = 4;
            this.btExit.Text = "Выход";
            this.btExit.UseVisualStyleBackColor = true;
            // 
            // btOwnServer
            // 
            this.btOwnServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btOwnServer.Location = new System.Drawing.Point(12, 297);
            this.btOwnServer.Name = "btOwnServer";
            this.btOwnServer.Size = new System.Drawing.Size(93, 23);
            this.btOwnServer.TabIndex = 5;
            this.btOwnServer.Text = "Задать сервер";
            this.btOwnServer.UseVisualStyleBackColor = true;
            this.btOwnServer.Click += new System.EventHandler(this.btOwnServer_Click);
            // 
            // gbServ
            // 
            this.gbServ.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbServ.Controls.Add(this.label4);
            this.gbServ.Controls.Add(this.tbServ);
            this.gbServ.Location = new System.Drawing.Point(50, 239);
            this.gbServ.Name = "gbServ";
            this.gbServ.Size = new System.Drawing.Size(260, 46);
            this.gbServ.TabIndex = 6;
            this.gbServ.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Адрес:";
            // 
            // tbServ
            // 
            this.tbServ.Location = new System.Drawing.Point(64, 15);
            this.tbServ.Name = "tbServ";
            this.tbServ.Size = new System.Drawing.Size(181, 20);
            this.tbServ.TabIndex = 4;
            this.tbServ.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // gbNewPass
            // 
            this.gbNewPass.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbNewPass.Controls.Add(this.label5);
            this.gbNewPass.Controls.Add(this.tbNewPassConf);
            this.gbNewPass.Controls.Add(this.lbError);
            this.gbNewPass.Controls.Add(this.lbNewPass);
            this.gbNewPass.Controls.Add(this.tbNewPass);
            this.gbNewPass.Location = new System.Drawing.Point(50, 142);
            this.gbNewPass.Name = "gbNewPass";
            this.gbNewPass.Size = new System.Drawing.Size(260, 91);
            this.gbNewPass.TabIndex = 11;
            this.gbNewPass.TabStop = false;
            this.gbNewPass.Text = "Установить новый пароль";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Подтвердить:";
            // 
            // tbNewPassConf
            // 
            this.tbNewPassConf.Location = new System.Drawing.Point(104, 45);
            this.tbNewPassConf.Name = "tbNewPassConf";
            this.tbNewPassConf.PasswordChar = '*';
            this.tbNewPassConf.Size = new System.Drawing.Size(141, 20);
            this.tbNewPassConf.TabIndex = 16;
            this.tbNewPassConf.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lbError
            // 
            this.lbError.AutoSize = true;
            this.lbError.ForeColor = System.Drawing.Color.Maroon;
            this.lbError.Location = new System.Drawing.Point(118, 68);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(118, 13);
            this.lbError.TabIndex = 13;
            this.lbError.Text = "Пароли не совпадают";
            this.lbError.Visible = false;
            // 
            // lbNewPass
            // 
            this.lbNewPass.AutoSize = true;
            this.lbNewPass.Location = new System.Drawing.Point(15, 22);
            this.lbNewPass.Name = "lbNewPass";
            this.lbNewPass.Size = new System.Drawing.Size(83, 13);
            this.lbNewPass.TabIndex = 12;
            this.lbNewPass.Text = "Новый пароль:";
            // 
            // tbNewPass
            // 
            this.tbNewPass.Location = new System.Drawing.Point(104, 19);
            this.tbNewPass.Name = "tbNewPass";
            this.tbNewPass.PasswordChar = '*';
            this.tbNewPass.Size = new System.Drawing.Size(141, 20);
            this.tbNewPass.TabIndex = 11;
            this.tbNewPass.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btKey
            // 
            this.btKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btKey.Location = new System.Drawing.Point(111, 297);
            this.btKey.Name = "btKey";
            this.btKey.Size = new System.Drawing.Size(75, 23);
            this.btKey.TabIndex = 13;
            this.btKey.Text = "Key_Make";
            this.btKey.UseVisualStyleBackColor = true;
            this.btKey.Visible = false;
            this.btKey.Click += new System.EventHandler(this.btKey_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btPassChange);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbPass);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Location = new System.Drawing.Point(30, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 87);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // btPassChange
            // 
            this.btPassChange.Location = new System.Drawing.Point(256, 49);
            this.btPassChange.Name = "btPassChange";
            this.btPassChange.Size = new System.Drawing.Size(25, 25);
            this.btPassChange.TabIndex = 12;
            this.btPassChange.UseVisualStyleBackColor = true;
            this.btPassChange.Click += new System.EventHandler(this.btPassChange_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Пароль:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Пользователь:";
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(109, 52);
            this.tbPass.Name = "tbPass";
            this.tbPass.PasswordChar = '*';
            this.tbPass.Size = new System.Drawing.Size(141, 20);
            this.tbPass.TabIndex = 9;
            this.tbPass.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(109, 22);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(172, 21);
            this.comboBox1.TabIndex = 8;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // LoginFrom
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(360, 332);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btKey);
            this.Controls.Add(this.gbNewPass);
            this.Controls.Add(this.gbServ);
            this.Controls.Add(this.btOwnServer);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.lbInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginFrom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Вход в систем pAdmin";
            this.Load += new System.EventHandler(this.LoginFrom_Load);
            this.gbServ.ResumeLayout(false);
            this.gbServ.PerformLayout();
            this.gbNewPass.ResumeLayout(false);
            this.gbNewPass.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btOwnServer;
        private System.Windows.Forms.GroupBox gbServ;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbServ;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox gbNewPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbNewPassConf;
        private System.Windows.Forms.Label lbError;
        private System.Windows.Forms.Label lbNewPass;
        private System.Windows.Forms.TextBox tbNewPass;
        private System.Windows.Forms.Button btKey;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btPassChange;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}