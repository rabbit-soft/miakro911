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
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.rabbitField1 = new rabnet.Components.RabbitField();
			this.rabbitBar1 = new rabnet.RabbitBar();
			this.rabbitPair1 = new rabnet.RabbitPair();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 503);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(94, 498);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button2.Location = new System.Drawing.Point(615, 467);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// rabbitField1
			// 
			this.rabbitField1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.rabbitField1.AutoScroll = true;
			this.rabbitField1.BackColor = System.Drawing.Color.White;
			this.rabbitField1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.rabbitField1.Location = new System.Drawing.Point(6, 12);
			this.rabbitField1.Name = "rabbitField1";
			this.rabbitField1.Size = new System.Drawing.Size(1178, 396);
			this.rabbitField1.TabIndex = 4;
			// 
			// rabbitBar1
			// 
			this.rabbitBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rabbitBar1.BackColor = System.Drawing.SystemColors.Control;
			this.rabbitBar1.Exists = false;
			this.rabbitBar1.FgColor = System.Drawing.SystemColors.Control;
			this.rabbitBar1.Gender = 0;
			this.rabbitBar1.Genom = "0";
			this.rabbitBar1.Location = new System.Drawing.Point(196, 481);
			this.rabbitBar1.Name = "rabbitBar1";
			this.rabbitBar1.OrderedGenom = false;
			this.rabbitBar1.PlodK = 0F;
			this.rabbitBar1.RabbitDad = 0;
			this.rabbitBar1.RabbitID = 0;
			this.rabbitBar1.RabbitMom = 0;
			this.rabbitBar1.RodK = 0F;
			this.rabbitBar1.Size = new System.Drawing.Size(150, 40);
			this.rabbitBar1.TabIndex = 3;
			this.rabbitBar1.TabStop = false;
			// 
			// rabbitPair1
			// 
			this.rabbitPair1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rabbitPair1.BackColor = System.Drawing.SystemColors.Control;
			this.rabbitPair1.FemaleGenom = "";
			this.rabbitPair1.FemalePlodK = 0F;
			this.rabbitPair1.FemaleRodK = 0F;
			this.rabbitPair1.Location = new System.Drawing.Point(408, 438);
			this.rabbitPair1.MaleGenom = "";
			this.rabbitPair1.MalePlodK = 0F;
			this.rabbitPair1.MaleRodK = 0F;
			this.rabbitPair1.Name = "rabbitPair1";
			this.rabbitPair1.OrderedGenom = false;
			this.rabbitPair1.Size = new System.Drawing.Size(150, 83);
			this.rabbitPair1.TabIndex = 2;
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.Location = new System.Drawing.Point(859, 468);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 6;
			this.button3.Text = "button3";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// GeneticsMainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1196, 533);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.rabbitField1);
			this.Controls.Add(this.rabbitBar1);
			this.Controls.Add(this.rabbitPair1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "GeneticsMainForm";
			this.Text = "Родословная";
			this.Load += new System.EventHandler(this.GeneticsMainForm_Load);
			this.Shown += new System.EventHandler(this.GeneticsMainForm_Shown);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GeneticsMainForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private RabbitPair rabbitPair1;
		private RabbitBar rabbitBar1;
		private rabnet.Components.RabbitField rabbitField1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
	}
}