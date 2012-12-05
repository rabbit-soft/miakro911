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
            this.cbRepair = new System.Windows.Forms.CheckBox();
            this.cbNest = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbHeater = new System.Windows.Forms.ComboBox();
            this.cbNest2 = new System.Windows.Forms.CheckBox();
            this.cbHeater2 = new System.Windows.Forms.ComboBox();
            this.grNest = new System.Windows.Forms.GroupBox();
            this.grVigul = new System.Windows.Forms.GroupBox();
            this.cbVigul = new System.Windows.Forms.ComboBox();
            this.grNest2 = new System.Windows.Forms.GroupBox();
            this.grDelims = new System.Windows.Forms.GroupBox();
            this.grDelim = new System.Windows.Forms.GroupBox();
            this.cbDelim = new System.Windows.Forms.CheckBox();
            this.cbDelim3 = new System.Windows.Forms.CheckBox();
            this.cbDelim2 = new System.Windows.Forms.CheckBox();
            this.cbDelim1 = new System.Windows.Forms.CheckBox();
            this.grNest.SuspendLayout();
            this.grVigul.SuspendLayout();
            this.grNest2.SuspendLayout();
            this.grDelims.SuspendLayout();
            this.grDelim.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbRepair
            // 
            this.cbRepair.AutoSize = true;
            this.cbRepair.Location = new System.Drawing.Point(161, 3);
            this.cbRepair.Name = "cbRepair";
            this.cbRepair.Size = new System.Drawing.Size(63, 17);
            this.cbRepair.TabIndex = 0;
            this.cbRepair.Text = "ремонт";
            this.cbRepair.UseVisualStyleBackColor = true;
            this.cbRepair.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // cbNest
            // 
            this.cbNest.AutoSize = true;
            this.cbNest.Location = new System.Drawing.Point(9, 13);
            this.cbNest.Name = "cbNest";
            this.cbNest.Size = new System.Drawing.Size(79, 17);
            this.cbNest.TabIndex = 1;
            this.cbNest.Text = "гнездовье";
            this.cbNest.UseVisualStyleBackColor = true;
            this.cbNest.CheckedChanged += new System.EventHandler(this.makeCBEvent);
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
            // cbNest2
            // 
            this.cbNest2.AutoSize = true;
            this.cbNest2.Location = new System.Drawing.Point(6, 15);
            this.cbNest2.Name = "cbNest2";
            this.cbNest2.Size = new System.Drawing.Size(79, 17);
            this.cbNest2.TabIndex = 4;
            this.cbNest2.Text = "гнездовье";
            this.cbNest2.UseVisualStyleBackColor = true;
            this.cbNest2.CheckedChanged += new System.EventHandler(this.makeCBEvent);
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
            this.grNest.Controls.Add(this.cbNest);
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
            // grNest2
            // 
            this.grNest2.Controls.Add(this.cbNest2);
            this.grNest2.Controls.Add(this.cbHeater2);
            this.grNest2.Location = new System.Drawing.Point(130, 21);
            this.grNest2.Name = "grNest2";
            this.grNest2.Size = new System.Drawing.Size(94, 59);
            this.grNest2.TabIndex = 8;
            this.grNest2.TabStop = false;
            this.grNest2.Text = "гнездовье-Б";
            this.grNest2.Visible = false;
            // 
            // grDelims
            // 
            this.grDelims.Controls.Add(this.grDelim);
            this.grDelims.Controls.Add(this.cbDelim3);
            this.grDelims.Controls.Add(this.cbDelim2);
            this.grDelims.Controls.Add(this.cbDelim1);
            this.grDelims.Location = new System.Drawing.Point(3, 21);
            this.grDelims.Name = "grDelims";
            this.grDelims.Size = new System.Drawing.Size(221, 59);
            this.grDelims.TabIndex = 9;
            this.grDelims.TabStop = false;
            this.grDelims.Text = "перегородки";
            this.grDelims.Visible = false;
            // 
            // grDelim
            // 
            this.grDelim.Controls.Add(this.cbDelim);
            this.grDelim.Location = new System.Drawing.Point(0, 0);
            this.grDelim.Name = "grDelim";
            this.grDelim.Size = new System.Drawing.Size(221, 59);
            this.grDelim.TabIndex = 10;
            this.grDelim.TabStop = false;
            this.grDelim.Text = "перегородка";
            this.grDelim.Visible = false;
            // 
            // cbDelim
            // 
            this.cbDelim.AutoSize = true;
            this.cbDelim.Location = new System.Drawing.Point(14, 25);
            this.cbDelim.Name = "cbDelim";
            this.cbDelim.Size = new System.Drawing.Size(91, 17);
            this.cbDelim.TabIndex = 0;
            this.cbDelim.Text = "перегородка";
            this.cbDelim.UseVisualStyleBackColor = true;
            this.cbDelim.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // cbDelim3
            // 
            this.cbDelim3.AutoSize = true;
            this.cbDelim3.Location = new System.Drawing.Point(156, 25);
            this.cbDelim3.Name = "cbDelim3";
            this.cbDelim3.Size = new System.Drawing.Size(41, 17);
            this.cbDelim3.TabIndex = 2;
            this.cbDelim3.Text = "В|Г";
            this.cbDelim3.UseVisualStyleBackColor = true;
            this.cbDelim3.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // cbDelim2
            // 
            this.cbDelim2.AutoSize = true;
            this.cbDelim2.Location = new System.Drawing.Point(91, 25);
            this.cbDelim2.Name = "cbDelim2";
            this.cbDelim2.Size = new System.Drawing.Size(42, 17);
            this.cbDelim2.TabIndex = 1;
            this.cbDelim2.Text = "Б|В";
            this.cbDelim2.UseVisualStyleBackColor = true;
            this.cbDelim2.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // cbDelim1
            // 
            this.cbDelim1.AutoSize = true;
            this.cbDelim1.Location = new System.Drawing.Point(23, 25);
            this.cbDelim1.Name = "cbDelim1";
            this.cbDelim1.Size = new System.Drawing.Size(42, 17);
            this.cbDelim1.TabIndex = 0;
            this.cbDelim1.Text = "А|Б";
            this.cbDelim1.UseVisualStyleBackColor = true;
            this.cbDelim1.CheckedChanged += new System.EventHandler(this.makeCBEvent);
            // 
            // BuildingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.grDelims);
            this.Controls.Add(this.grNest);
            this.Controls.Add(this.cbRepair);
            this.Controls.Add(this.grVigul);
            this.Controls.Add(this.grNest2);
            this.DoubleBuffered = true;
            this.Name = "BuildingControl";
            this.Size = new System.Drawing.Size(227, 82);
            this.grNest.ResumeLayout(false);
            this.grNest.PerformLayout();
            this.grVigul.ResumeLayout(false);
            this.grNest2.ResumeLayout(false);
            this.grNest2.PerformLayout();
            this.grDelims.ResumeLayout(false);
            this.grDelims.PerformLayout();
            this.grDelim.ResumeLayout(false);
            this.grDelim.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbRepair;
        private System.Windows.Forms.CheckBox cbNest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbHeater;
        private System.Windows.Forms.CheckBox cbNest2;
        private System.Windows.Forms.ComboBox cbHeater2;
        private System.Windows.Forms.GroupBox grNest;
        private System.Windows.Forms.GroupBox grVigul;
        private System.Windows.Forms.ComboBox cbVigul;
        private System.Windows.Forms.GroupBox grNest2;
        private System.Windows.Forms.GroupBox grDelims;
        private System.Windows.Forms.CheckBox cbDelim3;
        private System.Windows.Forms.CheckBox cbDelim2;
        private System.Windows.Forms.CheckBox cbDelim1;
        private System.Windows.Forms.GroupBox grDelim;
        private System.Windows.Forms.CheckBox cbDelim;
    }
}
