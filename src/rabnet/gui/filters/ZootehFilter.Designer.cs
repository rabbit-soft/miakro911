namespace rabnet.filters
{
    partial class ZootehFilter
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
        protected override void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.nudLogLim = new System.Windows.Forms.NumericUpDown();
            this.lbLogs = new System.Windows.Forms.CheckedListBox();
            this.lbZoo = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.cbFilters = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLim)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(430, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(262, 248);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Количество строк";
            // 
            // nudLogLim
            // 
            this.nudLogLim.Location = new System.Drawing.Point(366, 246);
            this.nudLogLim.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudLogLim.Name = "nudLogLim";
            this.nudLogLim.Size = new System.Drawing.Size(75, 20);
            this.nudLogLim.TabIndex = 14;
            this.nudLogLim.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lbLogs
            // 
            this.lbLogs.CheckOnClick = true;
            this.lbLogs.FormattingEnabled = true;
            this.lbLogs.Location = new System.Drawing.Point(239, 26);
            this.lbLogs.Name = "lbLogs";
            this.lbLogs.Size = new System.Drawing.Size(202, 214);
            this.lbLogs.TabIndex = 13;
            // 
            // lbZoo
            // 
            this.lbZoo.CheckOnClick = true;
            this.lbZoo.FormattingEnabled = true;
            this.lbZoo.Items.AddRange(new object[] {
            "Окрол",
            "Выдворение",
            "Подсчет гнездовых",
            "Предокрольный осмотр",
            "Отсадки",
            "Случки",
            "Вязки",
            "Прививка",
            "Установка гнездовья",
            "Рассадка мальчиков по одному",
            "Забор спермы"});
            this.lbZoo.Location = new System.Drawing.Point(12, 26);
            this.lbZoo.Name = "lbZoo";
            this.lbZoo.Size = new System.Drawing.Size(191, 214);
            this.lbZoo.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(28, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Зоотехплан";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(302, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "События";
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(12, 273);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 2;
            this.okBtn.Text = "Готово";
            this.okBtn.UseVisualStyleBackColor = true;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(366, 273);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 1;
            this.saveBtn.Text = "Сохранить";
            this.saveBtn.UseVisualStyleBackColor = true;
            // 
            // cbFilters
            // 
            this.cbFilters.FormattingEnabled = true;
            this.cbFilters.Location = new System.Drawing.Point(239, 273);
            this.cbFilters.Name = "cbFilters";
            this.cbFilters.Size = new System.Drawing.Size(121, 21);
            this.cbFilters.TabIndex = 0;
            // 
            // ZootehFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudLogLim);
            this.Controls.Add(this.lbLogs);
            this.Controls.Add(this.lbZoo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.cbFilters);
            this.FilterCombo = this.cbFilters;
            this.HideBtn = this.okBtn;
            this.Name = "ZootehFilter";
            this.SaveButton = this.saveBtn;
            this.Size = new System.Drawing.Size(454, 299);
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLim)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbFilters;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox lbZoo;
        private System.Windows.Forms.CheckedListBox lbLogs;
        private System.Windows.Forms.NumericUpDown nudLogLim;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}
