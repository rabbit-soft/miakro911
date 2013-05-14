namespace rabnet.components
{
    partial class BuildingControl
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
            this.chRepair = new System.Windows.Forms.CheckBox();
            this.chNest = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbHeater = new System.Windows.Forms.ComboBox();
            this.chNest2 = new System.Windows.Forms.CheckBox();
            this.cbHeater2 = new System.Windows.Forms.ComboBox();
            this.grNest = new System.Windows.Forms.GroupBox();
            this.grVigul = new System.Windows.Forms.GroupBox();
            this.cbVigul = new System.Windows.Forms.ComboBox();
            this.gbNest2 = new System.Windows.Forms.GroupBox();
            this.gbDelims = new System.Windows.Forms.GroupBox();
            this.gbOneDelim = new System.Windows.Forms.GroupBox();
            this.chDelim = new System.Windows.Forms.CheckBox();
            this.chDelim3 = new System.Windows.Forms.CheckBox();
            this.chDelim2 = new System.Windows.Forms.CheckBox();
            this.chDelim1 = new System.Windows.Forms.CheckBox();
            this.grNest.SuspendLayout();
            this.grVigul.SuspendLayout();
            this.gbNest2.SuspendLayout();
            this.gbDelims.SuspendLayout();
            this.gbOneDelim.SuspendLayout();
            this.SuspendLayout();
            // 
            // chRepair
            // 
            this.chRepair.AutoSize = true;
            this.chRepair.Location = new System.Drawing.Point(161, 3);
            this.chRepair.Name = "chRepair";
            this.chRepair.Size = new System.Drawing.Size(63, 17);
            this.chRepair.TabIndex = 0;
            this.chRepair.Text = "ремонт";
            this.chRepair.UseVisualStyleBackColor = true;
            this.chRepair.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // chNest
            // 
            this.chNest.AutoSize = true;
            this.chNest.Location = new System.Drawing.Point(9, 13);
            this.chNest.Name = "chNest";
            this.chNest.Size = new System.Drawing.Size(79, 17);
            this.chNest.TabIndex = 1;
            this.chNest.Text = "гнездовье";
            this.chNest.UseVisualStyleBackColor = true;
            this.chNest.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "грелка";
            // 
            // cbHeater
            // 
            this.cbHeater.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHeater.FormattingEnabled = true;
            this.cbHeater.Items.AddRange(new object[] {
            "Нет",
            "Выкл",
            "Вкл"});
            this.cbHeater.Location = new System.Drawing.Point(54, 33);
            this.cbHeater.Name = "cbHeater";
            this.cbHeater.Size = new System.Drawing.Size(61, 21);
            this.cbHeater.TabIndex = 3;
            this.cbHeater.SelectedIndexChanged += new System.EventHandler(this.makeComboEvent);
            // 
            // chNest2
            // 
            this.chNest2.AutoSize = true;
            this.chNest2.Location = new System.Drawing.Point(6, 15);
            this.chNest2.Name = "chNest2";
            this.chNest2.Size = new System.Drawing.Size(79, 17);
            this.chNest2.TabIndex = 4;
            this.chNest2.Text = "гнездовье";
            this.chNest2.UseVisualStyleBackColor = true;
            this.chNest2.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // cbHeater2
            // 
            this.cbHeater2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHeater2.FormattingEnabled = true;
            this.cbHeater2.Items.AddRange(new object[] {
            "Нет",
            "Выкл",
            "Вкл"});
            this.cbHeater2.Location = new System.Drawing.Point(6, 32);
            this.cbHeater2.Name = "cbHeater2";
            this.cbHeater2.Size = new System.Drawing.Size(61, 21);
            this.cbHeater2.TabIndex = 5;
            this.cbHeater2.SelectedIndexChanged += new System.EventHandler(this.makeComboEvent);
            // 
            // grNest
            // 
            this.grNest.Controls.Add(this.chNest);
            this.grNest.Controls.Add(this.label1);
            this.grNest.Controls.Add(this.cbHeater);
            this.grNest.Location = new System.Drawing.Point(3, 21);
            this.grNest.Name = "grNest";
            this.grNest.Size = new System.Drawing.Size(121, 59);
            this.grNest.TabIndex = 6;
            this.grNest.TabStop = false;
            this.grNest.Text = "Гнездовье";
            this.grNest.Visible = false;
            // 
            // grVigul
            // 
            this.grVigul.Controls.Add(this.cbVigul);
            this.grVigul.Location = new System.Drawing.Point(130, 21);
            this.grVigul.Name = "grVigul";
            this.grVigul.Size = new System.Drawing.Size(94, 59);
            this.grVigul.TabIndex = 7;
            this.grVigul.TabStop = false;
            this.grVigul.Text = "выгул";
            this.grVigul.Visible = false;
            // 
            // cbVigul
            // 
            this.cbVigul.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVigul.FormattingEnabled = true;
            this.cbVigul.Items.AddRange(new object[] {
            "малый",
            "большой"});
            this.cbVigul.Location = new System.Drawing.Point(6, 24);
            this.cbVigul.Name = "cbVigul";
            this.cbVigul.Size = new System.Drawing.Size(82, 21);
            this.cbVigul.TabIndex = 0;
            this.cbVigul.SelectedIndexChanged += new System.EventHandler(this.makeComboEvent);
            // 
            // gbNest2
            // 
            this.gbNest2.Controls.Add(this.chNest2);
            this.gbNest2.Controls.Add(this.cbHeater2);
            this.gbNest2.Location = new System.Drawing.Point(130, 21);
            this.gbNest2.Name = "gbNest2";
            this.gbNest2.Size = new System.Drawing.Size(94, 59);
            this.gbNest2.TabIndex = 8;
            this.gbNest2.TabStop = false;
            this.gbNest2.Text = "гнездовье-Б";
            this.gbNest2.Visible = false;
            // 
            // gbDelims
            // 
            this.gbDelims.Controls.Add(this.chDelim3);
            this.gbDelims.Controls.Add(this.chDelim2);
            this.gbDelims.Controls.Add(this.chDelim1);
            this.gbDelims.Controls.Add(this.gbOneDelim);
            this.gbDelims.Location = new System.Drawing.Point(3, 21);
            this.gbDelims.Name = "gbDelims";
            this.gbDelims.Size = new System.Drawing.Size(221, 59);
            this.gbDelims.TabIndex = 9;
            this.gbDelims.TabStop = false;
            this.gbDelims.Text = "перегородки";
            this.gbDelims.Visible = false;
            // 
            // gbOneDelim
            // 
            this.gbOneDelim.Controls.Add(this.chDelim);
            this.gbOneDelim.Location = new System.Drawing.Point(0, 0);
            this.gbOneDelim.Name = "gbOneDelim";
            this.gbOneDelim.Size = new System.Drawing.Size(221, 59);
            this.gbOneDelim.TabIndex = 10;
            this.gbOneDelim.TabStop = false;
            this.gbOneDelim.Text = "перегородка";
            this.gbOneDelim.Visible = false;
            // 
            // chDelim
            // 
            this.chDelim.AutoSize = true;
            this.chDelim.Location = new System.Drawing.Point(14, 25);
            this.chDelim.Name = "chDelim";
            this.chDelim.Size = new System.Drawing.Size(91, 17);
            this.chDelim.TabIndex = 0;
            this.chDelim.Text = "перегородка";
            this.chDelim.UseVisualStyleBackColor = true;
            this.chDelim.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // chDelim3
            // 
            this.chDelim3.AutoSize = true;
            this.chDelim3.Location = new System.Drawing.Point(156, 25);
            this.chDelim3.Name = "chDelim3";
            this.chDelim3.Size = new System.Drawing.Size(41, 17);
            this.chDelim3.TabIndex = 2;
            this.chDelim3.Text = "В|Г";
            this.chDelim3.UseVisualStyleBackColor = true;
            this.chDelim3.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // chDelim2
            // 
            this.chDelim2.AutoSize = true;
            this.chDelim2.Location = new System.Drawing.Point(91, 25);
            this.chDelim2.Name = "chDelim2";
            this.chDelim2.Size = new System.Drawing.Size(42, 17);
            this.chDelim2.TabIndex = 1;
            this.chDelim2.Text = "Б|В";
            this.chDelim2.UseVisualStyleBackColor = true;
            this.chDelim2.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // chDelim1
            // 
            this.chDelim1.AutoSize = true;
            this.chDelim1.Location = new System.Drawing.Point(23, 25);
            this.chDelim1.Name = "chDelim1";
            this.chDelim1.Size = new System.Drawing.Size(42, 17);
            this.chDelim1.TabIndex = 0;
            this.chDelim1.Text = "А|Б";
            this.chDelim1.UseVisualStyleBackColor = true;
            this.chDelim1.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // BuildingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.gbDelims);
            this.Controls.Add(this.grNest);
            this.Controls.Add(this.chRepair);
            this.Controls.Add(this.grVigul);
            this.Controls.Add(this.gbNest2);
            this.DoubleBuffered = true;
            this.Name = "BuildingControl";
            this.Size = new System.Drawing.Size(227, 82);
            this.grNest.ResumeLayout(false);
            this.grNest.PerformLayout();
            this.grVigul.ResumeLayout(false);
            this.gbNest2.ResumeLayout(false);
            this.gbNest2.PerformLayout();
            this.gbDelims.ResumeLayout(false);
            this.gbDelims.PerformLayout();
            this.gbOneDelim.ResumeLayout(false);
            this.gbOneDelim.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chRepair;
        private System.Windows.Forms.CheckBox chNest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbHeater;
        private System.Windows.Forms.CheckBox chNest2;
        private System.Windows.Forms.ComboBox cbHeater2;
        private System.Windows.Forms.GroupBox grNest;
        private System.Windows.Forms.GroupBox grVigul;
        private System.Windows.Forms.ComboBox cbVigul;
        private System.Windows.Forms.GroupBox gbNest2;
        private System.Windows.Forms.GroupBox gbDelims;
        private System.Windows.Forms.CheckBox chDelim3;
        private System.Windows.Forms.CheckBox chDelim2;
        private System.Windows.Forms.CheckBox chDelim1;
        private System.Windows.Forms.GroupBox gbOneDelim;
        private System.Windows.Forms.CheckBox chDelim;
    }
}
