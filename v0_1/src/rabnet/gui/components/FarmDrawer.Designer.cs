namespace rabnet
{
    partial class FarmDrawer
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bc1 = new rabnet.BuildingControl();
            this.bc2 = new rabnet.BuildingControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(225, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(499, 216);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // bc1
            // 
            this.bc1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bc1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bc1.delim = false;
            this.bc1.delim1 = false;
            this.bc1.delim2 = false;
            this.bc1.delim3 = false;
            this.bc1.heater = -1;
            this.bc1.heater2 = -1;
            this.bc1.Location = new System.Drawing.Point(-1, 45);
            this.bc1.Name = "bc1";
            this.bc1.nest = false;
            this.bc1.nest2 = false;
            this.bc1.repair = false;
            this.bc1.Size = new System.Drawing.Size(227, 82);
            this.bc1.TabIndex = 1;
            this.bc1.vigul = false;
            this.bc1.Visible = false;
            this.bc1.ValueChanged += new rabnet.BuildingControl.BCEventHandler(this.bc_ValueChanged);
            // 
            // bc2
            // 
            this.bc2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bc2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bc2.delim = false;
            this.bc2.delim1 = false;
            this.bc2.delim2 = false;
            this.bc2.delim3 = false;
            this.bc2.heater = -1;
            this.bc2.heater2 = -1;
            this.bc2.Location = new System.Drawing.Point(-1, 133);
            this.bc2.Name = "bc2";
            this.bc2.nest = false;
            this.bc2.nest2 = false;
            this.bc2.repair = false;
            this.bc2.Size = new System.Drawing.Size(227, 82);
            this.bc2.TabIndex = 2;
            this.bc2.vigul = false;
            this.bc2.Visible = false;
            this.bc2.ValueChanged += new rabnet.BuildingControl.BCEventHandler(this.bc_ValueChanged);
            // 
            // FarmDrawer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.bc1);
            this.Controls.Add(this.bc2);
            this.Name = "FarmDrawer";
            this.Size = new System.Drawing.Size(727, 222);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FarmDrawer_Paint);
            this.Resize += new System.EventHandler(this.FarmDrawer_Resize);
            this.Move += new System.EventHandler(this.FarmDrawer_Move);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private BuildingControl bc1;
        private BuildingControl bc2;
    }
}
