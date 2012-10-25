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
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btChangeBreed = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvSuckers
            // 
            this.lvSuckers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSuckers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14});
            this.lvSuckers.FullRowSelect = true;
            this.lvSuckers.GridLines = true;
            this.lvSuckers.Location = new System.Drawing.Point(3, 3);
            this.lvSuckers.MultiSelect = false;
            this.lvSuckers.Name = "lvSuckers";
            this.lvSuckers.Size = new System.Drawing.Size(514, 277);
            this.lvSuckers.TabIndex = 2;
            this.lvSuckers.UseCompatibleStateImageBehavior = false;
            this.lvSuckers.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Имя";
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Количество";
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Возраст";
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Пол";
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Порода";
            // 
            // btChangeBreed
            // 
            this.btChangeBreed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btChangeBreed.Location = new System.Drawing.Point(3, 286);
            this.btChangeBreed.Name = "btChangeBreed";
            this.btChangeBreed.Size = new System.Drawing.Size(114, 23);
            this.btChangeBreed.TabIndex = 3;
            this.btChangeBreed.Text = "Изменить породу";
            this.btChangeBreed.UseVisualStyleBackColor = true;
            this.btChangeBreed.Click += new System.EventHandler(this.btChangeBreed_Click);
            // 
            // RISuckersPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btChangeBreed);
            this.Controls.Add(this.lvSuckers);
            this.Name = "RISuckersPanel";
            this.Size = new System.Drawing.Size(520, 312);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvSuckers;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.Button btChangeBreed;
    }
}
