namespace mia_conv
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tb1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.log = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.clb1 = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textHost = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textRootPswd = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textRoot = new System.Windows.Forms.TextBox();
            this.dbnew = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.textUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textDB = new System.Windows.Forms.TextBox();
            this.ofd2 = new System.Windows.Forms.OpenFileDialog();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb1
            // 
            this.tb1.Location = new System.Drawing.Point(89, 9);
            this.tb1.Name = "tb1";
            this.tb1.Size = new System.Drawing.Size(438, 20);
            this.tb1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(533, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ofd
            // 
            this.ofd.DefaultExt = "mia";
            this.ofd.FileName = "openFileDialog1";
            this.ofd.Filter = "Mia files(*.mia)|*.mia";
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(61, 212);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.log.Size = new System.Drawing.Size(96, 23);
            this.log.TabIndex = 2;
            this.log.Visible = false;
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(570, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "read";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // clb1
            // 
            this.clb1.FormattingEnabled = true;
            this.clb1.Items.AddRange(new object[] {
            "Даты",
            "План строений",
            "Строения",
            "муж имена",
            "жен имена",
            "покупки-продажи",
            "Список зон",
            "Список пород",
            "Кролики",
            "Форма фильтров",
            "Форма трансферов",
            "Форма параметров",
            "Форма зоотехплана",
            "Форма графов",
            "Архивная форма",
            "Строки названий фермы",
            "График взвешиваний"});
            this.clb1.Location = new System.Drawing.Point(61, 187);
            this.clb1.Name = "clb1";
            this.clb1.Size = new System.Drawing.Size(63, 19);
            this.clb1.TabIndex = 5;
            this.clb1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Логи";
            this.label1.Visible = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(61, 138);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(62, 20);
            this.dateTimePicker1.TabIndex = 7;
            this.dateTimePicker1.Visible = false;
            this.dateTimePicker1.CloseUp += new System.EventHandler(this.dateTimePicker1_CloseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textHost);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.dbnew);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textPassword);
            this.groupBox1.Controls.Add(this.textUser);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textDB);
            this.groupBox1.Location = new System.Drawing.Point(12, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(603, 302);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "База Данных";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 277);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Скрипт";
            this.label9.Visible = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(475, 273);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(26, 22);
            this.button4.TabIndex = 11;
            this.button4.Text = "...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(58, 274);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(413, 20);
            this.textBox1.TabIndex = 9;
            this.textBox1.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(55, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Хост";
            // 
            // textHost
            // 
            this.textHost.Location = new System.Drawing.Point(92, 16);
            this.textHost.Name = "textHost";
            this.textHost.Size = new System.Drawing.Size(100, 20);
            this.textHost.TabIndex = 12;
            this.textHost.Text = "localhost";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(404, 32);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(192, 235);
            this.dataGridView1.TabIndex = 11;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.log);
            this.groupBox3.Controls.Add(this.clb1);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.dateTimePicker1);
            this.groupBox3.Location = new System.Drawing.Point(198, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 251);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Параметры";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(455, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Пользователи программы";
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(521, 272);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Перенести";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textRootPswd);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textRoot);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(9, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 110);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Создать новую";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Пароль";
            // 
            // textRootPswd
            // 
            this.textRootPswd.Location = new System.Drawing.Point(95, 56);
            this.textRootPswd.Name = "textRootPswd";
            this.textRootPswd.PasswordChar = '*';
            this.textRootPswd.Size = new System.Drawing.Size(82, 20);
            this.textRootPswd.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Администратор";
            // 
            // textRoot
            // 
            this.textRoot.Location = new System.Drawing.Point(95, 30);
            this.textRoot.Name = "textRoot";
            this.textRoot.Size = new System.Drawing.Size(82, 20);
            this.textRoot.TabIndex = 0;
            this.textRoot.Text = "root";
            // 
            // dbnew
            // 
            this.dbnew.AutoSize = true;
            this.dbnew.Location = new System.Drawing.Point(6, 135);
            this.dbnew.Name = "dbnew";
            this.dbnew.Size = new System.Drawing.Size(121, 17);
            this.dbnew.TabIndex = 1;
            this.dbnew.Text = "Создать новую БД";
            this.dbnew.UseVisualStyleBackColor = true;
            this.dbnew.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Пароль";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Пользователь";
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(92, 95);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(100, 20);
            this.textPassword.TabIndex = 3;
            this.textPassword.Text = "krol";
            // 
            // textUser
            // 
            this.textUser.Location = new System.Drawing.Point(92, 69);
            this.textUser.Name = "textUser";
            this.textUser.Size = new System.Drawing.Size(100, 20);
            this.textUser.TabIndex = 2;
            this.textUser.Text = "kroliki";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Имя БД";
            // 
            // textDB
            // 
            this.textDB.Location = new System.Drawing.Point(92, 43);
            this.textDB.Name = "textDB";
            this.textDB.Size = new System.Drawing.Size(100, 20);
            this.textDB.TabIndex = 0;
            this.textDB.Text = "kroliki";
            // 
            // ofd2
            // 
            this.ofd2.FileName = "openFileDialog1";
            this.ofd2.Filter = "SQL files(*.sql)|*.sql";
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(12, 345);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(603, 16);
            this.pb.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Файл фермы";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 370);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Импорт фермы";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckedListBox clb1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textDB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textRootPswd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textRoot;
        private System.Windows.Forms.CheckBox dbnew;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.TextBox textUser;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textHost;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.OpenFileDialog ofd2;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.Label label10;
    }
}

