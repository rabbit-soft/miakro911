namespace rabnet
{
	partial class RabbitPair
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
			this.MaleRabbit = new rabnet.RabbitBar();
			this.FemaleRabbit = new rabnet.RabbitBar();
			this.SuspendLayout();
			// 
			// MaleRabbit
			// 
			this.MaleRabbit.BackColor = System.Drawing.SystemColors.Control;
			this.MaleRabbit.Gender = 1;
			this.MaleRabbit.Genom = "0";
			this.MaleRabbit.Location = new System.Drawing.Point(0, 0);
			this.MaleRabbit.Name = "MaleRabbit";
			this.MaleRabbit.OrderedGenom = false;
			this.MaleRabbit.PlodK = 0;
			this.MaleRabbit.RodK = 0;
			this.MaleRabbit.Size = new System.Drawing.Size(150, 40);
			this.MaleRabbit.TabIndex = 0;
			// 
			// FemaleRabbit
			// 
			this.FemaleRabbit.BackColor = System.Drawing.SystemColors.Control;
			this.FemaleRabbit.Gender = 2;
			this.FemaleRabbit.Genom = "0";
			this.FemaleRabbit.Location = new System.Drawing.Point(0, 43);
			this.FemaleRabbit.Name = "FemaleRabbit";
			this.FemaleRabbit.OrderedGenom = false;
			this.FemaleRabbit.PlodK = 0;
			this.FemaleRabbit.RodK = 0;
			this.FemaleRabbit.Size = new System.Drawing.Size(150, 40);
			this.FemaleRabbit.TabIndex = 1;
			// 
			// RabbitPair
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.FemaleRabbit);
			this.Controls.Add(this.MaleRabbit);
			this.Name = "RabbitPair";
			this.Size = new System.Drawing.Size(150, 83);
			this.LocationChanged += new System.EventHandler(this.RabbitPair_LocationChanged);
			this.ResumeLayout(false);

		}

		#endregion

		private RabbitBar MaleRabbit;
		private RabbitBar FemaleRabbit;
	}
}
