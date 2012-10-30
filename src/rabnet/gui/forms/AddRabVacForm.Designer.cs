namespace rabnet
{
    partial class AddRabVacForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.cbVaccineType = new System.Windows.Forms.ComboBox();
            this.chWithChildren = new System.Windows.Forms.CheckBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.dateDays1 = new rabnet.components.DateDays();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Прививка:";
            // 
            // cbVaccineType
            // 
            this.cbVaccineType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbVaccineType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVaccineType.FormattingEnabled = true;
            this.cbVaccineType.Location = new System.Drawing.Point(78, 72);
            this.cbVaccineType.Name = "cbVaccineType";
            this.cbVaccineType.Size = new System.Drawing.Size(158, 21);
            this.cbVaccineType.TabIndex = 2;
            this.cbVaccineType.SelectedIndexChanged += new System.EventHandler(this.cbVaccineType_SelectedIndexChanged);
            // 
            // chWithChildren
            // 
            this.chWithChildren.AutoSize = true;
            this.chWithChildren.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chWithChildren.Location = new System.Drawing.Point(78, 99);
            this.chWithChildren.Name = "chWithChildren";
            this.chWithChildren.Size = new System.Drawing.Size(127, 17);
            this.chWithChildren.TabIndex = 4;
            this.chWithChildren.Text = "Привить подсосных";
            this.chWithChildren.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(78, 128);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Location = new System.Drawing.Point(161, 128);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 5;
            this.btOk.Text = "Готово";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Visible = false;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // dateDays1
            // 
            this.dateDays1.AutoSize = true;
            this.dateDays1.DateText = "Дата";
            this.dateDays1.DateValue = new System.DateTime(2012, 10, 30, 0, 0, 0, 0);
            this.dateDays1.DaysText = "Дней";
            this.dateDays1.DaysValue = 0;
            this.dateDays1.Location = new System.Drawing.Point(45, 12);
            this.dateDays1.Maximum = 10000;
            this.dateDays1.Name = "dateDays1";
            this.dateDays1.position = rabnet.components.DateDays.DDPosition.LABELS_LR;
            this.dateDays1.Size = new System.Drawing.Size(158, 54);
            this.dateDays1.Step = 1;
            this.dateDays1.TabIndex = 7;
            // 
            // AddRabVacForm
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(248, 163);
            this.Controls.Add(this.dateDays1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.chWithChildren);
            this.Controls.Add(this.cbVaccineType);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddRabVacForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Привить";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbVaccineType;
        private System.Windows.Forms.CheckBox chWithChildren;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private components.DateDays dateDays1;
    }
}