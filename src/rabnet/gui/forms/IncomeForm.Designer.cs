namespace rabnet
{
    partial class IncomeForm
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
            this.zones = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.btMale = new System.Windows.Forms.Button();
            this.btFemale = new System.Windows.Forms.Button();
            this.btNoSex = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.btPassport = new System.Windows.Forms.Button();
            this.btReplace = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // zones
            // 
            this.zones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zones.FormattingEnabled = true;
            this.zones.Location = new System.Drawing.Point(64, 12);
            this.zones.Name = "zones";
            this.zones.Size = new System.Drawing.Size(121, 21);
            this.zones.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Родина";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(19, 39);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(469, 286);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.btPassport_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Пол";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Количество";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Имя";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Порода";
            this.columnHeader4.Width = 94;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Адрес";
            this.columnHeader5.Width = 76;
            // 
            // btMale
            // 
            this.btMale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btMale.Location = new System.Drawing.Point(251, 10);
            this.btMale.Name = "btMale";
            this.btMale.Size = new System.Drawing.Size(75, 23);
            this.btMale.TabIndex = 3;
            this.btMale.Text = "Самцы";
            this.btMale.UseVisualStyleBackColor = true;
            this.btMale.Click += new System.EventHandler(this.btMale_Click);
            // 
            // btFemale
            // 
            this.btFemale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btFemale.Location = new System.Drawing.Point(332, 10);
            this.btFemale.Name = "btFemale";
            this.btFemale.Size = new System.Drawing.Size(75, 23);
            this.btFemale.TabIndex = 4;
            this.btFemale.Text = "Самки";
            this.btFemale.UseVisualStyleBackColor = true;
            this.btFemale.Click += new System.EventHandler(this.btFemale_Click);
            // 
            // btNoSex
            // 
            this.btNoSex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btNoSex.Location = new System.Drawing.Point(413, 10);
            this.btNoSex.Name = "btNoSex";
            this.btNoSex.Size = new System.Drawing.Size(75, 23);
            this.btNoSex.TabIndex = 5;
            this.btNoSex.Text = "Бесполые";
            this.btNoSex.UseVisualStyleBackColor = true;
            this.btNoSex.Click += new System.EventHandler(this.btNoSex_Click);
            // 
            // btDelete
            // 
            this.btDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDelete.Enabled = false;
            this.btDelete.Location = new System.Drawing.Point(494, 117);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(75, 23);
            this.btDelete.TabIndex = 6;
            this.btDelete.Text = "Удалить";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(191, 12);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(27, 21);
            this.button5.TabIndex = 7;
            this.button5.Text = "...";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.button6.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button6.Location = new System.Drawing.Point(487, 331);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 8;
            this.button6.Text = "Привоз";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button7.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button7.Location = new System.Drawing.Point(406, 331);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 9;
            this.button7.Text = "Отмена";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // btPassport
            // 
            this.btPassport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btPassport.Enabled = false;
            this.btPassport.Location = new System.Drawing.Point(494, 59);
            this.btPassport.Name = "btPassport";
            this.btPassport.Size = new System.Drawing.Size(75, 23);
            this.btPassport.TabIndex = 10;
            this.btPassport.Text = "Паспорт";
            this.btPassport.UseVisualStyleBackColor = true;
            this.btPassport.Click += new System.EventHandler(this.btPassport_Click);
            // 
            // btReplace
            // 
            this.btReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btReplace.Enabled = false;
            this.btReplace.Location = new System.Drawing.Point(494, 88);
            this.btReplace.Name = "btReplace";
            this.btReplace.Size = new System.Drawing.Size(75, 23);
            this.btReplace.TabIndex = 11;
            this.btReplace.Text = "Поселить";
            this.btReplace.UseVisualStyleBackColor = true;
            this.btReplace.Click += new System.EventHandler(this.btReplace_Click);
            // 
            // IncomeForm
            // 
            this.AcceptButton = this.button6;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button7;
            this.ClientSize = new System.Drawing.Size(574, 366);
            this.Controls.Add(this.btReplace);
            this.Controls.Add(this.btPassport);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.btNoSex);
            this.Controls.Add(this.btFemale);
            this.Controls.Add(this.btMale);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.zones);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(560, 230);
            this.Name = "IncomeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Привоз";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox zones;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btMale;
        private System.Windows.Forms.Button btFemale;
        private System.Windows.Forms.Button btNoSex;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btPassport;
        private System.Windows.Forms.Button btReplace;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}