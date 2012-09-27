namespace rabnet
{
    partial class FarmNewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FarmNewForm));
            this.farmsPanel1 = new rabnet.panels.FarmsPanel();
            this.SuspendLayout();
            // 
            // farmsPanel1
            // 
            resources.ApplyResources(this.farmsPanel1, "farmsPanel1");
            this.farmsPanel1.Name = "farmsPanel1";
            // 
            // FarmNewForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.farmsPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FarmNewForm";
            this.ResumeLayout(false);

        }

        #endregion

        private panels.FarmsPanel farmsPanel1;
    }
}