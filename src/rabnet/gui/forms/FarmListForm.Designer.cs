namespace rabnet
{
    partial class FarmListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FarmListForm));
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.button1 = new System.Windows.Forms.Button();
            this.btAddFarm = new System.Windows.Forms.Button();
            this.btChangeFarm = new System.Windows.Forms.Button();
            this.btDelFarm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(558, 229);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Название";
            this.columnHeader1.Width = 157;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Сохранять пароль";
            this.columnHeader4.Width = 49;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "БД";
            this.columnHeader5.Width = 195;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(495, 252);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Закрыть";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btAddFarm
            // 
            this.btAddFarm.Location = new System.Drawing.Point(12, 252);
            this.btAddFarm.Name = "btAddFarm";
            this.btAddFarm.Size = new System.Drawing.Size(75, 23);
            this.btAddFarm.TabIndex = 2;
            this.btAddFarm.Text = "Добавить";
            this.btAddFarm.UseVisualStyleBackColor = true;
            this.btAddFarm.Click += new System.EventHandler(this.button2_Click);
            // 
            // btChangeFarm
            // 
            this.btChangeFarm.Enabled = false;
            this.btChangeFarm.Location = new System.Drawing.Point(93, 252);
            this.btChangeFarm.Name = "btChangeFarm";
            this.btChangeFarm.Size = new System.Drawing.Size(75, 23);
            this.btChangeFarm.TabIndex = 3;
            this.btChangeFarm.Text = "Изменить";
            this.btChangeFarm.UseVisualStyleBackColor = true;
            this.btChangeFarm.Click += new System.EventHandler(this.button3_Click);
            // 
            // btDelFarm
            // 
            this.btDelFarm.Enabled = false;
            this.btDelFarm.Location = new System.Drawing.Point(174, 252);
            this.btDelFarm.Name = "btDelFarm";
            this.btDelFarm.Size = new System.Drawing.Size(75, 23);
            this.btDelFarm.TabIndex = 4;
            this.btDelFarm.Text = "Удалить";
            this.btDelFarm.UseVisualStyleBackColor = true;
            this.btDelFarm.Click += new System.EventHandler(this.button4_Click);
            // 
            // FarmListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(578, 287);
            this.Controls.Add(this.btDelFarm);
            this.Controls.Add(this.btChangeFarm);
            this.Controls.Add(this.btAddFarm);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FarmListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Фермы";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btAddFarm;
        private System.Windows.Forms.Button btChangeFarm;
        private System.Windows.Forms.Button btDelFarm;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}