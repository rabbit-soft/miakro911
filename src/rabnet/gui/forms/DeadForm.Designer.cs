namespace rabnet
{
    partial class DeadForm
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
            this.rabStatusBar1 = new rabnet.RabStatusBar();
            this.SuspendLayout();
            // 
            // rabStatusBar1
            // 
            this.rabStatusBar1.filterPanel = null;
            this.rabStatusBar1.Location = new System.Drawing.Point(0, 404);
            this.rabStatusBar1.Name = "rabStatusBar1";
            this.rabStatusBar1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.rabStatusBar1.Size = new System.Drawing.Size(725, 23);
            this.rabStatusBar1.TabIndex = 0;
            this.rabStatusBar1.Text = "rabStatusBar1";
            // 
            // DeadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 427);
            this.Controls.Add(this.rabStatusBar1);
            this.MinimizeBox = false;
            this.Name = "DeadForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Списания";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RabStatusBar rabStatusBar1;
    }
}