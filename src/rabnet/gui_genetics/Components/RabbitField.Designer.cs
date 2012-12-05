namespace rabnet.components
{
	partial class RabbitField
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
			this.RabbitsHolder = new System.Windows.Forms.Panel();
			this.ProgressPanel = new System.Windows.Forms.Panel();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.ProgressPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// RabbitsHolder
			// 
			this.RabbitsHolder.Location = new System.Drawing.Point(277, 53);
			this.RabbitsHolder.Name = "RabbitsHolder";
			this.RabbitsHolder.Size = new System.Drawing.Size(307, 228);
			this.RabbitsHolder.TabIndex = 5;
			this.RabbitsHolder.Visible = false;
			// 
			// ProgressPanel
			// 
			this.ProgressPanel.BackColor = System.Drawing.SystemColors.Control;
			this.ProgressPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ProgressPanel.Controls.Add(this.progressBar1);
			this.ProgressPanel.Location = new System.Drawing.Point(140, 139);
			this.ProgressPanel.Name = "ProgressPanel";
			this.ProgressPanel.Size = new System.Drawing.Size(186, 53);
			this.ProgressPanel.TabIndex = 6;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(16, 17);
			this.progressBar1.MarqueeAnimationSpeed = 50;
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(149, 17);
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBar1.TabIndex = 0;
			// 
			// RabbitField
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.ProgressPanel);
			this.Controls.Add(this.RabbitsHolder);
			this.DoubleBuffered = true;
			this.Name = "RabbitField";
			this.Size = new System.Drawing.Size(636, 407);
			this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.RabbitField_Scroll);
			this.Resize += new System.EventHandler(this.RabbitField_Resize);
			this.SizeChanged += new System.EventHandler(this.RabbitField_SizeChanged);
			this.ProgressPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel RabbitsHolder;
		private System.Windows.Forms.Panel ProgressPanel;
		private System.Windows.Forms.ProgressBar progressBar1;
	}
}
