namespace rabnet.forms
{
    partial class CountKids
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
            this.lParent = new System.Windows.Forms.Label();
            this.lAge = new System.Windows.Forms.Label();
            this.nudDead = new System.Windows.Forms.NumericUpDown();
            this.tbAlive = new System.Windows.Forms.TextBox();
            this.nudKilled = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.nudAdd = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudDead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudKilled)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAdd)).BeginInit();
            this.SuspendLayout();
            // 
            // lParent
            // 
            this.lParent.AutoSize = true;
            this.lParent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lParent.Location = new System.Drawing.Point(12, 9);
            this.lParent.Name = "lParent";
            this.lParent.Size = new System.Drawing.Size(41, 13);
            this.lParent.TabIndex = 0;
            this.lParent.Text = "label1";
            // 
            // lAge
            // 
            this.lAge.AutoSize = true;
            this.lAge.Location = new System.Drawing.Point(24, 65);
            this.lAge.Name = "lAge";
            this.lAge.Size = new System.Drawing.Size(49, 13);
            this.lAge.TabIndex = 1;
            this.lAge.Text = "Возраст";
            // 
            // nudDead
            // 
            this.nudDead.Location = new System.Drawing.Point(118, 127);
            this.nudDead.Name = "nudDead";
            this.nudDead.Size = new System.Drawing.Size(70, 20);
            this.nudDead.TabIndex = 3;
            this.nudDead.ValueChanged += new System.EventHandler(this.nudDead_ValueChanged);
            // 
            // tbAlive
            // 
            this.tbAlive.Enabled = false;
            this.tbAlive.Location = new System.Drawing.Point(118, 101);
            this.tbAlive.Name = "tbAlive";
            this.tbAlive.Size = new System.Drawing.Size(70, 20);
            this.tbAlive.TabIndex = 2;
            // 
            // nudKilled
            // 
            this.nudKilled.Location = new System.Drawing.Point(118, 153);
            this.nudKilled.Name = "nudKilled";
            this.nudKilled.Size = new System.Drawing.Size(70, 20);
            this.nudKilled.TabIndex = 4;
            this.nudKilled.ValueChanged += new System.EventHandler(this.nudDead_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Живых";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Умерли";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Притоптано";
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Location = new System.Drawing.Point(127, 234);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 6;
            this.btOk.Text = "Подсчет";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(46, 234);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 7;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // nudAdd
            // 
            this.nudAdd.Location = new System.Drawing.Point(118, 195);
            this.nudAdd.Name = "nudAdd";
            this.nudAdd.Size = new System.Drawing.Size(70, 20);
            this.nudAdd.TabIndex = 5;
            this.nudAdd.ValueChanged += new System.EventHandler(this.nudDead_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Прибавилось";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(14, 34);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(188, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // CountKids
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(214, 269);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudAdd);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudKilled);
            this.Controls.Add(this.tbAlive);
            this.Controls.Add(this.nudDead);
            this.Controls.Add(this.lAge);
            this.Controls.Add(this.lParent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(400, 200);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CountKids";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Подсчет гнездовых";
            this.Load += new System.EventHandler(this.CountKids_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudDead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudKilled)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAdd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lParent;
        private System.Windows.Forms.Label lAge;
        private System.Windows.Forms.NumericUpDown nudDead;
        private System.Windows.Forms.TextBox tbAlive;
        private System.Windows.Forms.NumericUpDown nudKilled;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.NumericUpDown nudAdd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}