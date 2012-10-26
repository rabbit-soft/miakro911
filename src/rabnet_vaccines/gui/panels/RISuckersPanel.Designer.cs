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
            this.btChangeBreed = new System.Windows.Forms.Button();
            this.cbBreeds = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
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
            this.lvSuckers.SelectedIndexChanged += new System.EventHandler(this.lvSuckers_SelectedIndexChanged);
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
            // btChangeBreed
            // 
            this.btChangeBreed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btChangeBreed.Enabled = false;
            this.btChangeBreed.Location = new System.Drawing.Point(249, 286);
            this.btChangeBreed.Name = "btChangeBreed";
            this.btChangeBreed.Size = new System.Drawing.Size(74, 21);
            this.btChangeBreed.TabIndex = 3;
            this.btChangeBreed.Text = "Изменить";
            this.btChangeBreed.UseVisualStyleBackColor = true;
            this.btChangeBreed.Click += new System.EventHandler(this.btChangeBreed_Click);
            // 
            // cbBreeds
            // 
            this.cbBreeds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbBreeds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBreeds.Enabled = false;
            this.cbBreeds.FormattingEnabled = true;
            this.cbBreeds.Location = new System.Drawing.Point(122, 286);
            this.cbBreeds.Name = "cbBreeds";
            this.cbBreeds.Size = new System.Drawing.Size(121, 21);
            this.cbBreeds.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(3, 289);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Изменить породу на:";
            // 
            // RISuckersPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbBreeds);
            this.Controls.Add(this.btChangeBreed);
            this.Controls.Add(this.lvSuckers);
            this.Name = "RISuckersPanel";
            this.Size = new System.Drawing.Size(520, 312);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvSuckers;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chCount;
        private System.Windows.Forms.ColumnHeader chAge;
        private System.Windows.Forms.ColumnHeader chSex;
        private System.Windows.Forms.ColumnHeader chBreed;
        private System.Windows.Forms.Button btChangeBreed;
        private System.Windows.Forms.ComboBox cbBreeds;
        private System.Windows.Forms.Label label1;
    }
}
