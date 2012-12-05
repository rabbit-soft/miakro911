namespace rabnet.forms
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.farmsPanel1 = new rabnet.FarmsPanel();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Name = "btCancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            resources.ApplyResources(this.btOk, "btOk");
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Name = "btOk";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // farmsPanel1
            // 
            resources.ApplyResources(this.farmsPanel1, "farmsPanel1");
            this.farmsPanel1.MaximumSize = new System.Drawing.Size(0, 315);
            this.farmsPanel1.MinimumSize = new System.Drawing.Size(340, 315);
            this.farmsPanel1.Name = "farmsPanel1";
            // 
            // FarmNewForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.farmsPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FarmNewForm";
            this.ResumeLayout(false);

        }

        #endregion

        private FarmsPanel farmsPanel1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
    }
}