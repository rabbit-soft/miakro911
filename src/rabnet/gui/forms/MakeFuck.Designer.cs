namespace rabnet
{
    partial class MakeFuck
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
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.cbHeter = new System.Windows.Forms.CheckBox();
            this.cbInbreed = new System.Windows.Forms.CheckBox();
            this.cbCand = new System.Windows.Forms.CheckBox();
            this.dateDays1 = new rabnet.components.DateDays();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 37);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(610, 285);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Имя";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Статус";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Порода";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Случек";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Детей";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Гетерозис";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Инбридинг";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(308, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(547, 385);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Случить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(466, 385);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(15, 385);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Гены";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // cbHeter
            // 
            this.cbHeter.AutoSize = true;
            this.cbHeter.Location = new System.Drawing.Point(386, 328);
            this.cbHeter.Name = "cbHeter";
            this.cbHeter.Size = new System.Drawing.Size(78, 17);
            this.cbHeter.TabIndex = 7;
            this.cbHeter.Text = "гетерозис";
            this.cbHeter.UseVisualStyleBackColor = true;
            this.cbHeter.CheckedChanged += new System.EventHandler(this.cbCand_CheckedChanged);
            // 
            // cbInbreed
            // 
            this.cbInbreed.AutoSize = true;
            this.cbInbreed.Location = new System.Drawing.Point(386, 351);
            this.cbInbreed.Name = "cbInbreed";
            this.cbInbreed.Size = new System.Drawing.Size(79, 17);
            this.cbInbreed.TabIndex = 8;
            this.cbInbreed.Text = "инбридинг";
            this.cbInbreed.UseVisualStyleBackColor = true;
            this.cbInbreed.CheckedChanged += new System.EventHandler(this.cbCand_CheckedChanged);
            // 
            // cbCand
            // 
            this.cbCand.AutoSize = true;
            this.cbCand.Checked = true;
            this.cbCand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCand.Location = new System.Drawing.Point(286, 328);
            this.cbCand.Name = "cbCand";
            this.cbCand.Size = new System.Drawing.Size(81, 17);
            this.cbCand.TabIndex = 9;
            this.cbCand.Text = "кандидаты";
            this.cbCand.UseVisualStyleBackColor = true;
            this.cbCand.CheckedChanged += new System.EventHandler(this.cbCand_CheckedChanged);
            // 
            // dateDays1
            // 
            this.dateDays1.AutoSize = true;
            this.dateDays1.DateText = "Дата";
            this.dateDays1.DateValue = new System.DateTime(2010, 1, 24, 0, 0, 0, 0);
            this.dateDays1.DaysText = "Дней";
            this.dateDays1.DaysValue = 0;
            this.dateDays1.Location = new System.Drawing.Point(485, 328);
            this.dateDays1.Maximum = 100000;
            this.dateDays1.Name = "dateDays1";
            this.dateDays1.position = rabnet.components.DateDays.DDPosition.LABELS_LR;
            this.dateDays1.Size = new System.Drawing.Size(137, 46);
            this.dateDays1.Step = 1;
            this.dateDays1.TabIndex = 6;
            // 
            // MakeFuck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 420);
            this.Controls.Add(this.cbCand);
            this.Controls.Add(this.cbInbreed);
            this.Controls.Add(this.cbHeter);
            this.Controls.Add(this.dateDays1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MakeFuck";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Случить";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private rabnet.components.DateDays dateDays1;
        private System.Windows.Forms.CheckBox cbHeter;
        private System.Windows.Forms.CheckBox cbInbreed;
        private System.Windows.Forms.CheckBox cbCand;
    }
}