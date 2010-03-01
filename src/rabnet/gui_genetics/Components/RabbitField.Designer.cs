namespace rabnet.Components
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
			this.label1 = new System.Windows.Forms.Label();
			this.RabbitsHolder = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// RabbitsHolder
			// 
			this.RabbitsHolder.Location = new System.Drawing.Point(184, 76);
			this.RabbitsHolder.Name = "RabbitsHolder";
			this.RabbitsHolder.Size = new System.Drawing.Size(307, 228);
			this.RabbitsHolder.TabIndex = 5;
			this.RabbitsHolder.Visible = false;
			// 
			// RabbitField
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.RabbitsHolder);
			this.Controls.Add(this.label1);
			this.DoubleBuffered = true;
			this.Name = "RabbitField";
			this.Size = new System.Drawing.Size(636, 407);
			this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.RabbitField_Scroll);
			this.Resize += new System.EventHandler(this.RabbitField_Resize);
			this.SizeChanged += new System.EventHandler(this.RabbitField_SizeChanged);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel RabbitsHolder;
	}
}
