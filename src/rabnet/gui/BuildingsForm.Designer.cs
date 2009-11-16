namespace rabnet
{
    partial class BuildingsForm
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.rabStatusBar1 = new rabnet.RabStatusBar();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Location = new System.Drawing.Point(125, 56);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(455, 329);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // rabStatusBar1
            // 
            this.rabStatusBar1.filterPanel = null;
            this.rabStatusBar1.Location = new System.Drawing.Point(0, 470);
            this.rabStatusBar1.Name = "rabStatusBar1";
            this.rabStatusBar1.Size = new System.Drawing.Size(785, 23);
            this.rabStatusBar1.TabIndex = 0;
            this.rabStatusBar1.Text = "rabStatusBar1";
            this.rabStatusBar1.itemGet += new rabnet.RabStatusBar.RSBItemEventHandler(this.rabStatusBar1_itemGet);
            this.rabStatusBar1.prepareGet += new rabnet.RabStatusBar.RSBPrepareEventHandler(this.rabStatusBar1_prepareGet);
            // 
            // BuildingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 493);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.rabStatusBar1);
            this.Name = "BuildingsForm";
            this.Text = "Постройки";
            this.Activated += new System.EventHandler(this.BuildingsForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BuildingsForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RabStatusBar rabStatusBar1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}