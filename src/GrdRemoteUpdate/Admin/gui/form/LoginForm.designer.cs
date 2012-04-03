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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btOwnServer = new System.Windows.Forms.Button();
            this.gbServ = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbServ = new System.Windows.Forms.TextBox();
            this.btPassChange = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbNewPass = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbNewPassConf = new System.Windows.Forms.TextBox();
            this.lbError = new System.Windows.Forms.Label();
            this.lbNewPass = new System.Windows.Forms.Label();
            this.tbNewPass = new System.Windows.Forms.TextBox();
            this.gbServ.SuspendLayout();
            this.gbNewPass.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(134, 71);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(172, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(134, 113);
            this.tbPass.Name = "tbPass";
            this.tbPass.PasswordChar = '*';
            this.tbPass.Size = new System.Drawing.Size(141, 20);
            this.tbPass.TabIndex = 1;
            this.tbPass.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выберите ключ и введите пароль";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Пользователь:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(80, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Пароль:";
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
            this.gbServ.Location = new System.Drawing.Point(56, 239);
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
            // btPassChange
            // 
            this.btPassChange.Location = new System.Drawing.Point(281, 111);
            this.btPassChange.Name = "btPassChange";
            this.btPassChange.Size = new System.Drawing.Size(25, 23);
            this.btPassChange.TabIndex = 7;
            this.btPassChange.Text = "...";
            this.btPassChange.UseVisualStyleBackColor = true;
            this.btPassChange.Click += new System.EventHandler(this.btPassChange_Click);
            // 
            // gbNewPass
            // 
            this.gbNewPass.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.gbNewPass.Controls.Add(this.label5);
            this.gbNewPass.Controls.Add(this.tbNewPassConf);
            this.gbNewPass.Controls.Add(this.lbError);
            this.gbNewPass.Controls.Add(this.lbNewPass);
            this.gbNewPass.Controls.Add(this.tbNewPass);
            this.gbNewPass.Location = new System.Drawing.Point(56, 142);
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
            // LoginFrom
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(360, 332);
            this.Controls.Add(this.gbNewPass);
            this.Controls.Add(this.btPassChange);
            this.Controls.Add(this.gbServ);
            this.Controls.Add(this.btOwnServer);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPass);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginFrom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Вход в систем GuardantUpdate";
            this.Load += new System.EventHandler(this.LoginFrom_Load);
            this.gbServ.ResumeLayout(false);
            this.gbServ.PerformLayout();
            this.gbNewPass.ResumeLayout(false);
            this.gbNewPass.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btOwnServer;
        private System.Windows.Forms.GroupBox gbServ;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbServ;
        private System.Windows.Forms.Button btPassChange;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox gbNewPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbNewPassConf;
        private System.Windows.Forms.Label lbError;
        private System.Windows.Forms.Label lbNewPass;
        private System.Windows.Forms.TextBox tbNewPass;
    }
}