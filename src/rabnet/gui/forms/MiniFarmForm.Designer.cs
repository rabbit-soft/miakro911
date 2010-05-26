﻿namespace rabnet
{
    partial class MiniFarmForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbUpper = new System.Windows.Forms.ComboBox();
            this.cbLower = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbNum = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Верхний ярус";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Нижний ярус";
            // 
            // cbUpper
            // 
            this.cbUpper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUpper.FormattingEnabled = true;
            this.cbUpper.Items.AddRange(new object[] {
            "Юрта",
            "Кварта",
            "Вертеп",
            "Барин",
            "Крольчихин",
            "Двукрольчихин",
            "Комплексный",
            "Хижина"});
            this.cbUpper.Location = new System.Drawing.Point(125, 37);
            this.cbUpper.Name = "cbUpper";
            this.cbUpper.Size = new System.Drawing.Size(121, 21);
            this.cbUpper.TabIndex = 2;
            // 
            // cbLower
            // 
            this.cbLower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLower.FormattingEnabled = true;
            this.cbLower.Items.AddRange(new object[] {
            "Нет",
            "Юрта",
            "Кварта",
            "Вертеп",
            "Барин",
            "Крольчихин",
            "Двукрольчихин",
            "Комплексный",
            "Хижина"});
            this.cbLower.Location = new System.Drawing.Point(125, 69);
            this.cbLower.Name = "cbLower";
            this.cbLower.Size = new System.Drawing.Size(121, 21);
            this.cbLower.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.button1.Location = new System.Drawing.Point(171, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Готово";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(90, 101);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(65, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Номер";
            // 
            // cbNum
            // 
            this.cbNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNum.FormattingEnabled = true;
            this.cbNum.Location = new System.Drawing.Point(112, 6);
            this.cbNum.Name = "cbNum";
            this.cbNum.Size = new System.Drawing.Size(66, 21);
            this.cbNum.TabIndex = 7;
            // 
            // MiniFarmForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(271, 136);
            this.Controls.Add(this.cbNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbLower);
            this.Controls.Add(this.cbUpper);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MiniFarmForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Миниферма";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbUpper;
        private System.Windows.Forms.ComboBox cbLower;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbNum;
    }
}