namespace rabnet.panels.RabInfoPanels
{
    partial class RIVaccinePanel
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
            this.lvVaccine = new System.Windows.Forms.ListView();
            this.chDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDuration = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUnable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btAddVac = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvVaccine
            // 
            this.lvVaccine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvVaccine.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDate,
            this.chName,
            this.chDuration,
            this.chUnable});
            this.lvVaccine.FullRowSelect = true;
            this.lvVaccine.GridLines = true;
            this.lvVaccine.Location = new System.Drawing.Point(3, 3);
            this.lvVaccine.MultiSelect = false;
            this.lvVaccine.Name = "lvVaccine";
            this.lvVaccine.Size = new System.Drawing.Size(570, 282);
            this.lvVaccine.TabIndex = 1;
            this.lvVaccine.UseCompatibleStateImageBehavior = false;
            this.lvVaccine.View = System.Windows.Forms.View.Details;
            // 
            // chDate
            // 
            this.chDate.Text = "Дата прививки";
            this.chDate.Width = 126;
            // 
            // chName
            // 
            this.chName.Text = "Название";
            this.chName.Width = 100;
            // 
            // chDuration
            // 
            this.chDuration.Text = "Действительна (дней)";
            this.chDuration.Width = 145;
            // 
            // chUnable
            // 
            this.chUnable.Text = "Отменена";
            this.chUnable.Width = 75;
            // 
            // btAddVac
            // 
            this.btAddVac.Location = new System.Drawing.Point(3, 291);
            this.btAddVac.Name = "btAddVac";
            this.btAddVac.Size = new System.Drawing.Size(75, 23);
            this.btAddVac.TabIndex = 2;
            this.btAddVac.Text = "Привить";
            this.btAddVac.UseVisualStyleBackColor = true;
            // 
            // RIVaccinePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btAddVac);
            this.Controls.Add(this.lvVaccine);
            this.Name = "RIVaccinePanel";
            this.Size = new System.Drawing.Size(576, 317);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvVaccine;
        private System.Windows.Forms.ColumnHeader chDate;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chDuration;
        private System.Windows.Forms.ColumnHeader chUnable;
        private System.Windows.Forms.Button btAddVac;
    }
}
