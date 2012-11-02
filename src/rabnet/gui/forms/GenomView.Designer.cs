namespace rabnet
{
    partial class GenomView
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
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbFemaleName = new System.Windows.Forms.Label();
            this.lbFemaleBreed = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbMaleName = new System.Windows.Forms.Label();
            this.lbMaleBreed = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbChildName = new System.Windows.Forms.Label();
            this.lbChildBreed = new System.Windows.Forms.Label();
            this.tvFemale = new rabnet.RabGenTreeView();
            this.tvMale = new rabnet.RabGenTreeView();
            this.tvChildren = new rabnet.RabGenTreeView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(582, 501);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Закрыть";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 506);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Гетерозис: нет";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(124, 506);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Инбридинг: нет";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(645, 483);
            this.splitContainer1.SplitterDistance = 206;
            this.splitContainer1.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tvFemale);
            this.groupBox1.Controls.Add(this.lbFemaleName);
            this.groupBox1.Controls.Add(this.lbFemaleBreed);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 483);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Самка";
            // 
            // lbFemaleName
            // 
            this.lbFemaleName.AutoSize = true;
            this.lbFemaleName.Location = new System.Drawing.Point(6, 17);
            this.lbFemaleName.Name = "lbFemaleName";
            this.lbFemaleName.Size = new System.Drawing.Size(35, 13);
            this.lbFemaleName.TabIndex = 2;
            this.lbFemaleName.Text = "Имя: ";
            // 
            // lbFemaleBreed
            // 
            this.lbFemaleBreed.AutoSize = true;
            this.lbFemaleBreed.Location = new System.Drawing.Point(6, 41);
            this.lbFemaleBreed.Name = "lbFemaleBreed";
            this.lbFemaleBreed.Size = new System.Drawing.Size(51, 13);
            this.lbFemaleBreed.TabIndex = 2;
            this.lbFemaleBreed.Text = "Порода: ";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer2.Size = new System.Drawing.Size(435, 483);
            this.splitContainer2.SplitterDistance = 213;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tvMale);
            this.groupBox2.Controls.Add(this.lbMaleName);
            this.groupBox2.Controls.Add(this.lbMaleBreed);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 483);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Самец";
            // 
            // lbMaleName
            // 
            this.lbMaleName.AutoSize = true;
            this.lbMaleName.Location = new System.Drawing.Point(6, 17);
            this.lbMaleName.Name = "lbMaleName";
            this.lbMaleName.Size = new System.Drawing.Size(35, 13);
            this.lbMaleName.TabIndex = 2;
            this.lbMaleName.Text = "Имя: ";
            // 
            // lbMaleBreed
            // 
            this.lbMaleBreed.AutoSize = true;
            this.lbMaleBreed.Location = new System.Drawing.Point(6, 41);
            this.lbMaleBreed.Name = "lbMaleBreed";
            this.lbMaleBreed.Size = new System.Drawing.Size(51, 13);
            this.lbMaleBreed.TabIndex = 2;
            this.lbMaleBreed.Text = "Порода: ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tvChildren);
            this.groupBox3.Controls.Add(this.lbChildName);
            this.groupBox3.Controls.Add(this.lbChildBreed);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(218, 483);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Потомство";
            // 
            // lbChildName
            // 
            this.lbChildName.AutoSize = true;
            this.lbChildName.Location = new System.Drawing.Point(6, 17);
            this.lbChildName.Name = "lbChildName";
            this.lbChildName.Size = new System.Drawing.Size(35, 13);
            this.lbChildName.TabIndex = 2;
            this.lbChildName.Text = "Имя: ";
            // 
            // lbChildBreed
            // 
            this.lbChildBreed.AutoSize = true;
            this.lbChildBreed.Location = new System.Drawing.Point(6, 41);
            this.lbChildBreed.Name = "lbChildBreed";
            this.lbChildBreed.Size = new System.Drawing.Size(51, 13);
            this.lbChildBreed.TabIndex = 2;
            this.lbChildBreed.Text = "Порода: ";
            // 
            // tvFemale
            // 
            this.tvFemale.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvFemale.Location = new System.Drawing.Point(6, 82);
            this.tvFemale.Name = "tvFemale";
            this.tvFemale.Size = new System.Drawing.Size(194, 395);
            this.tvFemale.TabIndex = 3;
            // 
            // tvMale
            // 
            this.tvMale.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvMale.Location = new System.Drawing.Point(6, 82);
            this.tvMale.Name = "tvMale";
            this.tvMale.Size = new System.Drawing.Size(201, 395);
            this.tvMale.TabIndex = 4;
            // 
            // tvChildren
            // 
            this.tvChildren.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvChildren.Location = new System.Drawing.Point(6, 82);
            this.tvChildren.Name = "tvChildren";
            this.tvChildren.Size = new System.Drawing.Size(206, 395);
            this.tvChildren.TabIndex = 5;
            // 
            // GenomView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(671, 532);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenomView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Гены";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbFemaleName;
        private System.Windows.Forms.Label lbFemaleBreed;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbMaleName;
        private System.Windows.Forms.Label lbMaleBreed;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbChildName;
        private System.Windows.Forms.Label lbChildBreed;
        private RabGenTreeView tvFemale;
        private RabGenTreeView tvMale;
        private RabGenTreeView tvChildren;
    }
}