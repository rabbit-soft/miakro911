namespace rabnet
{
    partial class LogsCheckBoxList
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
            this.lbLogs = new System.Windows.Forms.CheckedListBox();
            this.btAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbLogs
            // 
            this.lbLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLogs.CheckOnClick = true;
            this.lbLogs.FormattingEnabled = true;
            this.lbLogs.Location = new System.Drawing.Point(3, 3);
            this.lbLogs.Name = "lbLogs";
            this.lbLogs.Size = new System.Drawing.Size(194, 244);
            this.lbLogs.TabIndex = 14;
            // 
            // btAll
            // 
            this.btAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btAll.Location = new System.Drawing.Point(63, 251);
            this.btAll.Name = "btAll";
            this.btAll.Size = new System.Drawing.Size(75, 23);
            this.btAll.TabIndex = 15;
            this.btAll.Text = "Все";
            this.btAll.UseVisualStyleBackColor = true;
            this.btAll.Click += new System.EventHandler(this.btAll_Click);
            // 
            // LogsCheckBoxList
            // 
            this.Controls.Add(this.btAll);
            this.Controls.Add(this.lbLogs);
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "LogsCheckBoxList";
            this.Size = new System.Drawing.Size(200, 277);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox lbLogs;
        private System.Windows.Forms.Button btAll;
    }
}
