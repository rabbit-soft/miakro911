namespace rabnet
{
    partial class RISuckersPanel
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
            this.lvSuckers = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBreed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvSuckers
            // 
            this.lvSuckers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSuckers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chCount,
            this.chAge,
            this.chSex,
            this.chBreed});
            this.lvSuckers.FullRowSelect = true;
            this.lvSuckers.GridLines = true;
            this.lvSuckers.Location = new System.Drawing.Point(3, 3);
            this.lvSuckers.MultiSelect = false;
            this.lvSuckers.Name = "lvSuckers";
            this.lvSuckers.Size = new System.Drawing.Size(514, 277);
            this.lvSuckers.TabIndex = 2;
            this.lvSuckers.UseCompatibleStateImageBehavior = false;
            this.lvSuckers.View = System.Windows.Forms.View.Details;
            this.lvSuckers.DoubleClick += new System.EventHandler(this.lvSuckers_DoubleClick);
            // 
            // chName
            // 
            this.chName.Text = "Имя";
            this.chName.Width = 150;
            // 
            // chCount
            // 
            this.chCount.Text = "Количество";
            this.chCount.Width = 80;
            // 
            // chAge
            // 
            this.chAge.Text = "Возраст";
            // 
            // chSex
            // 
            this.chSex.Text = "Пол";
            this.chSex.Width = 40;
            // 
            // chBreed
            // 
            this.chBreed.Text = "Порода";
            this.chBreed.Width = 130;
            // 
            // RISuckersPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvSuckers);
            this.Name = "RISuckersPanel";
            this.Size = new System.Drawing.Size(520, 312);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvSuckers;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chCount;
        private System.Windows.Forms.ColumnHeader chAge;
        private System.Windows.Forms.ColumnHeader chSex;
        private System.Windows.Forms.ColumnHeader chBreed;
    }
}
