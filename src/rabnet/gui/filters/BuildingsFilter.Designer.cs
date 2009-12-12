namespace rabnet
{
    partial class BuildingsFilter
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        protected override void InitializeComponent()
        {
            this.farmGroup = new System.Windows.Forms.GroupBox();
            this.kvartaBox = new System.Windows.Forms.CheckBox();
            this.urtaBox = new System.Windows.Forms.CheckBox();
            this.vertepBox = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.barinBox = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFarm = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.farmGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // farmGroup
            // 
            this.farmGroup.Controls.Add(this.button1);
            this.farmGroup.Controls.Add(this.checkBox5);
            this.farmGroup.Controls.Add(this.barinBox);
            this.farmGroup.Controls.Add(this.checkBox3);
            this.farmGroup.Controls.Add(this.checkBox2);
            this.farmGroup.Controls.Add(this.checkBox1);
            this.farmGroup.Controls.Add(this.kvartaBox);
            this.farmGroup.Controls.Add(this.urtaBox);
            this.farmGroup.Controls.Add(this.vertepBox);
            this.farmGroup.Location = new System.Drawing.Point(130, 3);
            this.farmGroup.Name = "farmGroup";
            this.farmGroup.Size = new System.Drawing.Size(176, 141);
            this.farmGroup.TabIndex = 2;
            this.farmGroup.TabStop = false;
            this.farmGroup.Text = "Тип Фермы";
            this.farmGroup.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // kvartaBox
            // 
            this.kvartaBox.AutoSize = true;
            this.kvartaBox.Location = new System.Drawing.Point(6, 65);
            this.kvartaBox.Name = "kvartaBox";
            this.kvartaBox.Size = new System.Drawing.Size(62, 17);
            this.kvartaBox.TabIndex = 3;
            this.kvartaBox.Text = "Кварта";
            this.kvartaBox.UseVisualStyleBackColor = true;
            this.kvartaBox.CheckedChanged += new System.EventHandler(this.kvartaBox_CheckedChanged);
            // 
            // urtaBox
            // 
            this.urtaBox.AutoSize = true;
            this.urtaBox.Location = new System.Drawing.Point(6, 42);
            this.urtaBox.Name = "urtaBox";
            this.urtaBox.Size = new System.Drawing.Size(52, 17);
            this.urtaBox.TabIndex = 1;
            this.urtaBox.Text = "Юрта";
            this.urtaBox.UseVisualStyleBackColor = true;
            // 
            // vertepBox
            // 
            this.vertepBox.AutoSize = true;
            this.vertepBox.Location = new System.Drawing.Point(6, 19);
            this.vertepBox.Name = "vertepBox";
            this.vertepBox.Size = new System.Drawing.Size(62, 17);
            this.vertepBox.TabIndex = 0;
            this.vertepBox.Text = "Вертеп";
            this.vertepBox.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(74, 88);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(65, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Хижина";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(74, 65);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(97, 17);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "Комплексный";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(74, 42);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(104, 17);
            this.checkBox3.TabIndex = 6;
            this.checkBox3.Text = "Двукрольчихин";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // barinBox
            // 
            this.barinBox.AutoSize = true;
            this.barinBox.Location = new System.Drawing.Point(6, 88);
            this.barinBox.Name = "barinBox";
            this.barinBox.Size = new System.Drawing.Size(57, 17);
            this.barinBox.TabIndex = 7;
            this.barinBox.Text = "Барин";
            this.barinBox.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(74, 19);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(85, 17);
            this.checkBox5.TabIndex = 8;
            this.checkBox5.Text = "Крольчихин";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 111);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Выбрать все";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Фермы";
            // 
            // cbFarm
            // 
            this.cbFarm.FormattingEnabled = true;
            this.cbFarm.Items.AddRange(new object[] {
            "Все",
            "Занятые",
            "Свободные"});
            this.cbFarm.Location = new System.Drawing.Point(3, 18);
            this.cbFarm.Name = "cbFarm";
            this.cbFarm.Size = new System.Drawing.Size(121, 21);
            this.cbFarm.TabIndex = 11;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Неважно",
            "Отсутствует",
            "Установлена"});
            this.comboBox1.Location = new System.Drawing.Point(3, 64);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "Гнездовье";
            // 
            // BuildingsFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.cbFarm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.farmGroup);
            this.Name = "BuildingsFilter";
            this.Size = new System.Drawing.Size(320, 190);
            this.farmGroup.ResumeLayout(false);
            this.farmGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox farmGroup;
        private System.Windows.Forms.CheckBox kvartaBox;
        private System.Windows.Forms.CheckBox urtaBox;
        private System.Windows.Forms.CheckBox vertepBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox barinBox;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbFarm;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
    }
}
