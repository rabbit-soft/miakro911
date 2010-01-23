namespace rabnet
{
    partial class ZootehFilter
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
        protected override void InitializeComponent()
        {
            this.cbFilters = new System.Windows.Forms.ComboBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.cbOkrol = new System.Windows.Forms.CheckBox();
            this.cbVudvor = new System.Windows.Forms.CheckBox();
            this.cbCount = new System.Windows.Forms.CheckBox();
            this.cbPreokrol = new System.Windows.Forms.CheckBox();
            this.cbReplace = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbFilters
            // 
            this.cbFilters.FormattingEnabled = true;
            this.cbFilters.Location = new System.Drawing.Point(239, 255);
            this.cbFilters.Name = "cbFilters";
            this.cbFilters.Size = new System.Drawing.Size(121, 21);
            this.cbFilters.TabIndex = 0;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(366, 253);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 1;
            this.saveBtn.Text = "Сохранить";
            this.saveBtn.UseVisualStyleBackColor = true;
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(29, 255);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 2;
            this.okBtn.Text = "Готово";
            this.okBtn.UseVisualStyleBackColor = true;
            // 
            // cbOkrol
            // 
            this.cbOkrol.AutoSize = true;
            this.cbOkrol.Location = new System.Drawing.Point(21, 16);
            this.cbOkrol.Name = "cbOkrol";
            this.cbOkrol.Size = new System.Drawing.Size(58, 17);
            this.cbOkrol.TabIndex = 3;
            this.cbOkrol.Text = "Окрол";
            this.cbOkrol.UseVisualStyleBackColor = true;
            // 
            // cbVudvor
            // 
            this.cbVudvor.AutoSize = true;
            this.cbVudvor.Location = new System.Drawing.Point(21, 39);
            this.cbVudvor.Name = "cbVudvor";
            this.cbVudvor.Size = new System.Drawing.Size(89, 17);
            this.cbVudvor.TabIndex = 4;
            this.cbVudvor.Text = "Выдворение";
            this.cbVudvor.UseVisualStyleBackColor = true;
            // 
            // cbCount
            // 
            this.cbCount.AutoSize = true;
            this.cbCount.Location = new System.Drawing.Point(21, 62);
            this.cbCount.Name = "cbCount";
            this.cbCount.Size = new System.Drawing.Size(125, 17);
            this.cbCount.TabIndex = 5;
            this.cbCount.Text = "Подсчет гнездовых";
            this.cbCount.UseVisualStyleBackColor = true;
            // 
            // cbPreokrol
            // 
            this.cbPreokrol.AutoSize = true;
            this.cbPreokrol.Location = new System.Drawing.Point(21, 85);
            this.cbPreokrol.Name = "cbPreokrol";
            this.cbPreokrol.Size = new System.Drawing.Size(148, 17);
            this.cbPreokrol.TabIndex = 6;
            this.cbPreokrol.Text = "Предокрольный осмотр";
            this.cbPreokrol.UseVisualStyleBackColor = true;
            // 
            // cbReplace
            // 
            this.cbReplace.AutoSize = true;
            this.cbReplace.Location = new System.Drawing.Point(21, 108);
            this.cbReplace.Name = "cbReplace";
            this.cbReplace.Size = new System.Drawing.Size(67, 17);
            this.cbReplace.TabIndex = 7;
            this.cbReplace.Text = "отсадки";
            this.cbReplace.UseVisualStyleBackColor = true;
            // 
            // ZootehFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbReplace);
            this.Controls.Add(this.cbPreokrol);
            this.Controls.Add(this.cbCount);
            this.Controls.Add(this.cbVudvor);
            this.Controls.Add(this.cbOkrol);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.cbFilters);
            this.FilterCombo = this.cbFilters;
            this.HideBtn = this.okBtn;
            this.Name = "ZootehFilter";
            this.SaveButton = this.saveBtn;
            this.Size = new System.Drawing.Size(454, 299);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbFilters;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.CheckBox cbOkrol;
        private System.Windows.Forms.CheckBox cbVudvor;
        private System.Windows.Forms.CheckBox cbCount;
        private System.Windows.Forms.CheckBox cbPreokrol;
        private System.Windows.Forms.CheckBox cbReplace;
    }
}
