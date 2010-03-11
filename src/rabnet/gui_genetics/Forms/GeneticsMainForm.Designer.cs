namespace rabnet
{
	partial class GeneticsMainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneticsMainForm));
			this.panel1 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.rabbitField1 = new rabnet.Components.RabbitField();
			this.rabbitBar1 = new rabnet.RabbitBar();
			this.rabbitPair1 = new rabnet.RabbitPair();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button1);
			this.panel1.Controls.Add(this.checkBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1004, 25);
			this.panel1.TabIndex = 7;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(885, 1);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(116, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Закрыть все окна";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(12, 5);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(119, 17);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "Сортировать гены";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// rabbitField1
			// 
			this.rabbitField1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rabbitField1.AutoScroll = true;
			this.rabbitField1.BackColor = System.Drawing.Color.White;
			this.rabbitField1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.rabbitField1.Location = new System.Drawing.Point(0, 25);
			this.rabbitField1.Name = "rabbitField1";
			this.rabbitField1.OrderedGenom = false;
			this.rabbitField1.Size = new System.Drawing.Size(1004, 493);
			this.rabbitField1.TabIndex = 6;
			// 
			// rabbitBar1
			// 
			this.rabbitBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rabbitBar1.BackColor = System.Drawing.SystemColors.Control;
			this.rabbitBar1.FgColor = System.Drawing.SystemColors.Control;
			this.rabbitBar1.ForcedGender = 0;
			this.rabbitBar1.Genom = "0";
			this.rabbitBar1.Location = new System.Drawing.Point(196, 466);
			this.rabbitBar1.Name = "rabbitBar1";
			this.rabbitBar1.OrderedGenom = false;
			this.rabbitBar1.Size = new System.Drawing.Size(150, 40);
			this.rabbitBar1.TabIndex = 3;
			this.rabbitBar1.TabStop = false;
			// 
			// rabbitPair1
			// 
			this.rabbitPair1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rabbitPair1.BackColor = System.Drawing.SystemColors.Control;
			this.rabbitPair1.FemaleGenom = "";
			this.rabbitPair1.Location = new System.Drawing.Point(408, 438);
			this.rabbitPair1.MaleGenom = "";
			this.rabbitPair1.Name = "rabbitPair1";
			this.rabbitPair1.OrderedGenom = false;
			this.rabbitPair1.Size = new System.Drawing.Size(150, 83);
			this.rabbitPair1.TabIndex = 2;
			// 
			// GeneticsMainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1004, 518);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.rabbitField1);
			this.Controls.Add(this.rabbitBar1);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "GeneticsMainForm";
			this.Text = "Родословная";
			this.Shown += new System.EventHandler(this.GeneticsMainForm_Shown);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GeneticsMainForm_FormClosing);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private RabbitPair rabbitPair1;
		private RabbitBar rabbitBar1;
		private rabnet.Components.RabbitField rabbitField1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button button1;
	}
}