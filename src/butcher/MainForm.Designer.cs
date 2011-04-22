namespace butcher
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.lbUnit = new System.Windows.Forms.Label();
            this.chLogDate = new System.Windows.Forms.ColumnHeader();
            this.chLogProdType = new System.Windows.Forms.ColumnHeader();
            this.chLogAmount = new System.Windows.Forms.ColumnHeader();
            this.chLogUser = new System.Windows.Forms.ColumnHeader();
            this.lbProductName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btExit = new System.Windows.Forms.Button();
            this.btDown = new System.Windows.Forms.Button();
            this.tbAmount = new System.Windows.Forms.TextBox();
            this.btUp = new System.Windows.Forms.Button();
            this.lvLogs = new System.Windows.Forms.ListView();
            this.chLogsDate = new System.Windows.Forms.ColumnHeader();
            this.chLogsProdType = new System.Windows.Forms.ColumnHeader();
            this.chLogsAmount = new System.Windows.Forms.ColumnHeader();
            this.chLogsUser = new System.Windows.Forms.ColumnHeader();
            this.npButcher = new butcher.NumPad();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView1.Location = new System.Drawing.Point(57, 117);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(20, 3, 50, 10);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView1.Size = new System.Drawing.Size(600, 800);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // lbUnit
            // 
            this.lbUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbUnit.Location = new System.Drawing.Point(1092, 117);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(145, 49);
            this.lbUnit.TabIndex = 21;
            // 
            // chLogDate
            // 
            this.chLogDate.Text = "Дата";
            this.chLogDate.Width = 350;
            // 
            // chLogProdType
            // 
            this.chLogProdType.Text = "Продукция";
            this.chLogProdType.Width = 183;
            // 
            // chLogAmount
            // 
            this.chLogAmount.Text = "Количество";
            this.chLogAmount.Width = 193;
            // 
            // chLogUser
            // 
            this.chLogUser.Text = "Пользователь";
            this.chLogUser.Width = 216;
            // 
            // lbProductName
            // 
            this.lbProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbProductName.Location = new System.Drawing.Point(738, 62);
            this.lbProductName.Name = "lbProductName";
            this.lbProductName.Size = new System.Drawing.Size(384, 52);
            this.lbProductName.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(57, 65);
            this.label1.Margin = new System.Windows.Forms.Padding(20, 20, 20, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(600, 49);
            this.label1.TabIndex = 16;
            this.label1.Text = "Виды продукции";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btExit
            // 
            this.btExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btExit.Location = new System.Drawing.Point(1060, 960);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(177, 52);
            this.btExit.TabIndex = 20;
            this.btExit.Text = "ВЫХОД";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // btDown
            // 
            this.btDown.Enabled = false;
            this.btDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btDown.Image = ((System.Drawing.Image)(resources.GetObject("btDown.Image")));
            this.btDown.Location = new System.Drawing.Point(368, 937);
            this.btDown.Margin = new System.Windows.Forms.Padding(10);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(250, 52);
            this.btDown.TabIndex = 18;
            this.btDown.Text = "Вниз";
            this.btDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btDown.UseVisualStyleBackColor = true;
            this.btDown.Click += new System.EventHandler(this.btDown_Click);
            // 
            // tbAmount
            // 
            this.tbAmount.Enabled = false;
            this.tbAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbAmount.Location = new System.Drawing.Point(736, 117);
            this.tbAmount.Name = "tbAmount";
            this.tbAmount.Size = new System.Drawing.Size(350, 49);
            this.tbAmount.TabIndex = 15;
            this.tbAmount.TextChanged += new System.EventHandler(this.tbAmount_TextChanged);
            // 
            // btUp
            // 
            this.btUp.Enabled = false;
            this.btUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btUp.Image = ((System.Drawing.Image)(resources.GetObject("btUp.Image")));
            this.btUp.Location = new System.Drawing.Point(98, 937);
            this.btUp.Margin = new System.Windows.Forms.Padding(10);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(250, 52);
            this.btUp.TabIndex = 19;
            this.btUp.Text = "Вверх";
            this.btUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btUp.UseVisualStyleBackColor = true;
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            // 
            // lvLogs
            // 
            this.lvLogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chLogsDate,
            this.chLogsProdType,
            this.chLogsAmount,
            this.chLogsUser});
            this.lvLogs.GridLines = true;
            this.lvLogs.Location = new System.Drawing.Point(721, 749);
            this.lvLogs.Name = "lvLogs";
            this.lvLogs.Size = new System.Drawing.Size(516, 168);
            this.lvLogs.TabIndex = 22;
            this.lvLogs.UseCompatibleStateImageBehavior = false;
            this.lvLogs.View = System.Windows.Forms.View.Details;
            // 
            // chLogsDate
            // 
            this.chLogsDate.Text = "Дата";
            this.chLogsDate.Width = 131;
            // 
            // chLogsProdType
            // 
            this.chLogsProdType.Text = "Продукция";
            this.chLogsProdType.Width = 104;
            // 
            // chLogsAmount
            // 
            this.chLogsAmount.Text = "Количество";
            this.chLogsAmount.Width = 128;
            // 
            // chLogsUser
            // 
            this.chLogsUser.Text = "Пользователь";
            this.chLogsUser.Width = 107;
            // 
            // npButcher
            // 
            this.npButcher.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.npButcher.Enabled = false;
            this.npButcher.Location = new System.Drawing.Point(745, 172);
            this.npButcher.MaximumSize = new System.Drawing.Size(502, 670);
            this.npButcher.MinimumSize = new System.Drawing.Size(192, 256);
            this.npButcher.Name = "npButcher";
            this.npButcher.OkButtonEnable = false;
            this.npButcher.OkButtonVisible = true;
            this.npButcher.OnlyDigits = false;
            this.npButcher.Size = new System.Drawing.Size(326, 551);
            this.npButcher.TabIndex = 14;
            this.npButcher.OkButtonClick += new System.EventHandler(this.npButcher_OkButtonClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.Controls.Add(this.lvLogs);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lbUnit);
            this.Controls.Add(this.npButcher);
            this.Controls.Add(this.lbProductName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.btDown);
            this.Controls.Add(this.tbAmount);
            this.Controls.Add(this.btUp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Стерильный цех";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
        private System.Windows.Forms.DataGridViewImageColumn Column2;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.ColumnHeader chLogDate;
        private System.Windows.Forms.ColumnHeader chLogProdType;
        private System.Windows.Forms.ColumnHeader chLogAmount;
        private System.Windows.Forms.ColumnHeader chLogUser;
        private NumPad npButcher;
        private System.Windows.Forms.Label lbProductName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.TextBox tbAmount;
        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.ListView lvLogs;
        private System.Windows.Forms.ColumnHeader chLogsDate;
        private System.Windows.Forms.ColumnHeader chLogsProdType;
        private System.Windows.Forms.ColumnHeader chLogsAmount;
        private System.Windows.Forms.ColumnHeader chLogsUser;

    }
}

