namespace rabnet.forms
{
    partial class FarmChangeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FarmChangeForm));
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.fname = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rpswd = new System.Windows.Forms.TextBox();
            this.ruser = new System.Windows.Forms.TextBox();
            this.btDeleteDB = new System.Windows.Forms.Button();
            this.btImportDB = new System.Windows.Forms.Button();
            this.btCreateDB = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.fsavepswd = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fpswd = new System.Windows.Forms.TextBox();
            this.fhost = new System.Windows.Forms.TextBox();
            this.fdb = new System.Windows.Forms.TextBox();
            this.fuser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.fhostm = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btOk.Location = new System.Drawing.Point(232, 363);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 11;
            this.btOk.Text = "Сохранить";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(151, 363);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 12;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Название";
            // 
            // fname
            // 
            this.fname.Location = new System.Drawing.Point(75, 18);
            this.fname.Name = "fname";
            this.fname.Size = new System.Drawing.Size(232, 20);
            this.fname.TabIndex = 0;
            this.fname.Text = "Название фермы";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Файл фермы(*.mia)|*.mia";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.textBox6);
            this.panel1.Controls.Add(this.fsavepswd);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(14, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(296, 320);
            this.panel1.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rpswd);
            this.groupBox2.Controls.Add(this.ruser);
            this.groupBox2.Controls.Add(this.btDeleteDB);
            this.groupBox2.Controls.Add(this.btImportDB);
            this.groupBox2.Controls.Add(this.btCreateDB);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(0, 201);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(292, 112);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Создание БД";
            // 
            // rpswd
            // 
            this.rpswd.Location = new System.Drawing.Point(115, 47);
            this.rpswd.Name = "rpswd";
            this.rpswd.PasswordChar = '*';
            this.rpswd.Size = new System.Drawing.Size(166, 20);
            this.rpswd.TabIndex = 7;
            // 
            // ruser
            // 
            this.ruser.Location = new System.Drawing.Point(115, 23);
            this.ruser.Name = "ruser";
            this.ruser.Size = new System.Drawing.Size(166, 20);
            this.ruser.TabIndex = 6;
            this.ruser.Tag = "";
            this.ruser.Text = "root";
            // 
            // btDeleteDB
            // 
            this.btDeleteDB.Enabled = false;
            this.btDeleteDB.Location = new System.Drawing.Point(207, 78);
            this.btDeleteDB.Name = "btDeleteDB";
            this.btDeleteDB.Size = new System.Drawing.Size(75, 23);
            this.btDeleteDB.TabIndex = 10;
            this.btDeleteDB.Text = "Удалить";
            this.btDeleteDB.UseVisualStyleBackColor = true;
            this.btDeleteDB.Visible = false;
            // 
            // btImportDB
            // 
            this.btImportDB.Location = new System.Drawing.Point(86, 78);
            this.btImportDB.Name = "btImportDB";
            this.btImportDB.Size = new System.Drawing.Size(115, 23);
            this.btImportDB.TabIndex = 9;
            this.btImportDB.Text = "Импортировать";
            this.btImportDB.UseVisualStyleBackColor = true;
            this.btImportDB.Click += new System.EventHandler(this.btImportDB_Click);
            // 
            // btCreateDB
            // 
            this.btCreateDB.Location = new System.Drawing.Point(13, 78);
            this.btCreateDB.Name = "btCreateDB";
            this.btCreateDB.Size = new System.Drawing.Size(67, 23);
            this.btCreateDB.TabIndex = 8;
            this.btCreateDB.Text = "Создать";
            this.btCreateDB.UseVisualStyleBackColor = true;
            this.btCreateDB.Click += new System.EventHandler(this.btCreateDB_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(64, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Пароль";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Администратор";
            // 
            // textBox6
            // 
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox6.Enabled = false;
            this.textBox6.ForeColor = System.Drawing.Color.Red;
            this.textBox6.Location = new System.Drawing.Point(0, 158);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(289, 37);
            this.textBox6.TabIndex = 10;
            this.textBox6.Text = "Внимание! Пароль сохраняется на компьютере в открытом виде.";
            // 
            // fsavepswd
            // 
            this.fsavepswd.AutoSize = true;
            this.fsavepswd.Location = new System.Drawing.Point(0, 135);
            this.fsavepswd.Name = "fsavepswd";
            this.fsavepswd.Size = new System.Drawing.Size(192, 17);
            this.fsavepswd.TabIndex = 9;
            this.fsavepswd.Text = "Сохранять пароль пользователя";
            this.fsavepswd.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fpswd);
            this.groupBox1.Controls.Add(this.fhost);
            this.groupBox1.Controls.Add(this.fdb);
            this.groupBox1.Controls.Add(this.fuser);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 129);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "База данных";
            // 
            // fpswd
            // 
            this.fpswd.Location = new System.Drawing.Point(115, 99);
            this.fpswd.Name = "fpswd";
            this.fpswd.Size = new System.Drawing.Size(166, 20);
            this.fpswd.TabIndex = 4;
            this.fpswd.Text = "krol";
            // 
            // fhost
            // 
            this.fhost.Location = new System.Drawing.Point(115, 21);
            this.fhost.Name = "fhost";
            this.fhost.Size = new System.Drawing.Size(166, 20);
            this.fhost.TabIndex = 1;
            this.fhost.Text = "localhost";
            // 
            // fdb
            // 
            this.fdb.Location = new System.Drawing.Point(115, 47);
            this.fdb.Name = "fdb";
            this.fdb.Size = new System.Drawing.Size(166, 20);
            this.fdb.TabIndex = 2;
            this.fdb.Text = "kroliki";
            // 
            // fuser
            // 
            this.fuser.Location = new System.Drawing.Point(115, 73);
            this.fuser.Name = "fuser";
            this.fuser.Size = new System.Drawing.Size(166, 20);
            this.fuser.TabIndex = 3;
            this.fuser.Text = "kroliki";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Пароль БД";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Пользователь БД";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "База данных";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(78, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Хост";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.fhostm);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Location = new System.Drawing.Point(329, 44);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(296, 37);
            this.panel2.TabIndex = 14;
            // 
            // fhostm
            // 
            this.fhostm.Location = new System.Drawing.Point(61, 7);
            this.fhostm.Name = "fhostm";
            this.fhostm.Size = new System.Drawing.Size(231, 20);
            this.fhostm.TabIndex = 3;
            this.fhostm.Text = "localhost";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(-2, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Хост";
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button6.Location = new System.Drawing.Point(15, 363);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 15;
            this.button6.Text = "Подробнее";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // FarmChangeForm
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(675, 393);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.fname);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FarmChangeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Описание фермы";
            this.Shown += new System.EventHandler(this.FarmChangeForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fname;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox rpswd;
        private System.Windows.Forms.TextBox ruser;
        private System.Windows.Forms.Button btDeleteDB;
        private System.Windows.Forms.Button btImportDB;
        private System.Windows.Forms.Button btCreateDB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.CheckBox fsavepswd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox fpswd;
        private System.Windows.Forms.TextBox fhost;
        private System.Windows.Forms.TextBox fdb;
        private System.Windows.Forms.TextBox fuser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox fhostm;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button6;
    }
}