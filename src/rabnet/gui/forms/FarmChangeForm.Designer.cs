namespace rabnet
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.fname = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.fpswd = new System.Windows.Forms.TextBox();
            this.fhost = new System.Windows.Forms.TextBox();
            this.fdb = new System.Windows.Forms.TextBox();
            this.fuser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fsavepswd = new System.Windows.Forms.CheckBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rpswd = new System.Windows.Forms.TextBox();
            this.ruser = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(232, 363);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(151, 363);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            this.fname.TabIndex = 3;
            this.fname.Text = "Название фермы";
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
            this.groupBox1.Location = new System.Drawing.Point(15, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 129);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "База данных";
            // 
            // fpswd
            // 
            this.fpswd.Location = new System.Drawing.Point(115, 99);
            this.fpswd.Name = "fpswd";
            this.fpswd.Size = new System.Drawing.Size(166, 20);
            this.fpswd.TabIndex = 7;
            this.fpswd.Text = "krol";
            // 
            // fhost
            // 
            this.fhost.Location = new System.Drawing.Point(115, 21);
            this.fhost.Name = "fhost";
            this.fhost.Size = new System.Drawing.Size(166, 20);
            this.fhost.TabIndex = 6;
            this.fhost.Text = "localhost";
            // 
            // fdb
            // 
            this.fdb.Location = new System.Drawing.Point(115, 47);
            this.fdb.Name = "fdb";
            this.fdb.Size = new System.Drawing.Size(166, 20);
            this.fdb.TabIndex = 5;
            this.fdb.Text = "kroldb";
            // 
            // fuser
            // 
            this.fuser.Location = new System.Drawing.Point(115, 73);
            this.fuser.Name = "fuser";
            this.fuser.Size = new System.Drawing.Size(166, 20);
            this.fuser.TabIndex = 4;
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
            // fsavepswd
            // 
            this.fsavepswd.AutoSize = true;
            this.fsavepswd.Location = new System.Drawing.Point(15, 179);
            this.fsavepswd.Name = "fsavepswd";
            this.fsavepswd.Size = new System.Drawing.Size(192, 17);
            this.fsavepswd.TabIndex = 5;
            this.fsavepswd.Text = "Сохранять пароль пользователя";
            this.fsavepswd.UseVisualStyleBackColor = true;
            // 
            // textBox6
            // 
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox6.Enabled = false;
            this.textBox6.ForeColor = System.Drawing.Color.Red;
            this.textBox6.Location = new System.Drawing.Point(15, 202);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(289, 37);
            this.textBox6.TabIndex = 6;
            this.textBox6.Text = "Внимание! Пароль сохраняется на компьютере в открытом виде.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rpswd);
            this.groupBox2.Controls.Add(this.ruser);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(15, 245);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(292, 112);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Создание БД";
            // 
            // rpswd
            // 
            this.rpswd.Location = new System.Drawing.Point(115, 47);
            this.rpswd.Name = "rpswd";
            this.rpswd.Size = new System.Drawing.Size(166, 20);
            this.rpswd.TabIndex = 6;
            // 
            // ruser
            // 
            this.ruser.Location = new System.Drawing.Point(115, 23);
            this.ruser.Name = "ruser";
            this.ruser.Size = new System.Drawing.Size(166, 20);
            this.ruser.TabIndex = 5;
            this.ruser.Tag = "";
            this.ruser.Text = "root";
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(207, 78);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "Удалить";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(86, 78);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(115, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Импортировать";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 78);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(67, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Создать";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
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
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Файл фермы(*.mia)|*.mia";
            // 
            // FarmChangeForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(316, 393);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.fsavepswd);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.fname);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FarmChangeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Описание фермы";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fname;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox fpswd;
        private System.Windows.Forms.TextBox fhost;
        private System.Windows.Forms.TextBox fdb;
        private System.Windows.Forms.TextBox fuser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox fsavepswd;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox rpswd;
        private System.Windows.Forms.TextBox ruser;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}