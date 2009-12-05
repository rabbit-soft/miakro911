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
            this.enableBox = new System.Windows.Forms.CheckBox();
            this.statusGroup = new System.Windows.Forms.GroupBox();
            this.busyBox = new System.Windows.Forms.CheckBox();
            this.freeBox = new System.Windows.Forms.CheckBox();
            this.farmGroup = new System.Windows.Forms.GroupBox();
            this.kvartaBox = new System.Windows.Forms.CheckBox();
            this.urtaBox = new System.Windows.Forms.CheckBox();
            this.vertepBox = new System.Windows.Forms.CheckBox();
            this.filterGroup = new System.Windows.Forms.GroupBox();
            this.gnezdoGroup = new System.Windows.Forms.GroupBox();
            this.grelkaGroup = new System.Windows.Forms.GroupBox();
            this.radioGnezdo1 = new System.Windows.Forms.RadioButton();
            this.radioGnezdo2 = new System.Windows.Forms.RadioButton();
            this.radioGnezdo3 = new System.Windows.Forms.RadioButton();
            this.radioGrelka1 = new System.Windows.Forms.RadioButton();
            this.radioGrelka2 = new System.Windows.Forms.RadioButton();
            this.radioGrelka3 = new System.Windows.Forms.RadioButton();
            this.radioGrelka4 = new System.Windows.Forms.RadioButton();
            this.radioGrelka5 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.checkAllButton = new System.Windows.Forms.Button();
            this.statusGroup.SuspendLayout();
            this.farmGroup.SuspendLayout();
            this.filterGroup.SuspendLayout();
            this.gnezdoGroup.SuspendLayout();
            this.grelkaGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // enableBox
            // 
            this.enableBox.AutoSize = true;
            this.enableBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.enableBox.Location = new System.Drawing.Point(15, 15);
            this.enableBox.Name = "enableBox";
            this.enableBox.Size = new System.Drawing.Size(92, 20);
            this.enableBox.TabIndex = 0;
            this.enableBox.Text = "Включен";
            this.enableBox.UseVisualStyleBackColor = true;
            // 
            // statusGroup
            // 
            this.statusGroup.Controls.Add(this.busyBox);
            this.statusGroup.Controls.Add(this.freeBox);
            this.statusGroup.Location = new System.Drawing.Point(5, 20);
            this.statusGroup.Name = "statusGroup";
            this.statusGroup.Size = new System.Drawing.Size(102, 68);
            this.statusGroup.TabIndex = 1;
            this.statusGroup.TabStop = false;
            this.statusGroup.Text = "Статус";
            // 
            // busyBox
            // 
            this.busyBox.AutoSize = true;
            this.busyBox.Location = new System.Drawing.Point(6, 42);
            this.busyBox.Name = "busyBox";
            this.busyBox.Size = new System.Drawing.Size(70, 17);
            this.busyBox.TabIndex = 1;
            this.busyBox.Text = "Занятые";
            this.busyBox.UseVisualStyleBackColor = true;
            // 
            // freeBox
            // 
            this.freeBox.AutoSize = true;
            this.freeBox.Location = new System.Drawing.Point(6, 19);
            this.freeBox.Name = "freeBox";
            this.freeBox.Size = new System.Drawing.Size(83, 17);
            this.freeBox.TabIndex = 0;
            this.freeBox.Text = "Свободные";
            this.freeBox.UseVisualStyleBackColor = true;
            // 
            // farmGroup
            // 
            this.farmGroup.Controls.Add(this.kvartaBox);
            this.farmGroup.Controls.Add(this.urtaBox);
            this.farmGroup.Controls.Add(this.vertepBox);
            this.farmGroup.Location = new System.Drawing.Point(5, 94);
            this.farmGroup.Name = "farmGroup";
            this.farmGroup.Size = new System.Drawing.Size(102, 94);
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
            // filterGroup
            // 
            this.filterGroup.Controls.Add(this.checkAllButton);
            this.filterGroup.Controls.Add(this.button1);
            this.filterGroup.Controls.Add(this.grelkaGroup);
            this.filterGroup.Controls.Add(this.gnezdoGroup);
            this.filterGroup.Controls.Add(this.statusGroup);
            this.filterGroup.Controls.Add(this.farmGroup);
            this.filterGroup.Location = new System.Drawing.Point(15, 40);
            this.filterGroup.Name = "filterGroup";
            this.filterGroup.Size = new System.Drawing.Size(250, 270);
            this.filterGroup.TabIndex = 2;
            this.filterGroup.TabStop = false;
            // 
            // gnezdoGroup
            // 
            this.gnezdoGroup.Controls.Add(this.radioGnezdo3);
            this.gnezdoGroup.Controls.Add(this.radioGnezdo2);
            this.gnezdoGroup.Controls.Add(this.radioGnezdo1);
            this.gnezdoGroup.Location = new System.Drawing.Point(113, 20);
            this.gnezdoGroup.Name = "gnezdoGroup";
            this.gnezdoGroup.Size = new System.Drawing.Size(129, 94);
            this.gnezdoGroup.TabIndex = 3;
            this.gnezdoGroup.TabStop = false;
            this.gnezdoGroup.Text = "Гнездовья";
            // 
            // grelkaGroup
            // 
            this.grelkaGroup.Controls.Add(this.radioGrelka5);
            this.grelkaGroup.Controls.Add(this.radioGrelka4);
            this.grelkaGroup.Controls.Add(this.radioGrelka3);
            this.grelkaGroup.Controls.Add(this.radioGrelka2);
            this.grelkaGroup.Controls.Add(this.radioGrelka1);
            this.grelkaGroup.Location = new System.Drawing.Point(113, 120);
            this.grelkaGroup.Name = "grelkaGroup";
            this.grelkaGroup.Size = new System.Drawing.Size(129, 142);
            this.grelkaGroup.TabIndex = 0;
            this.grelkaGroup.TabStop = false;
            this.grelkaGroup.Text = "Грелка";
            // 
            // radioGnezdo1
            // 
            this.radioGnezdo1.AutoSize = true;
            this.radioGnezdo1.Location = new System.Drawing.Point(6, 19);
            this.radioGnezdo1.Name = "radioGnezdo1";
            this.radioGnezdo1.Size = new System.Drawing.Size(71, 17);
            this.radioGnezdo1.TabIndex = 0;
            this.radioGnezdo1.TabStop = true;
            this.radioGnezdo1.Text = "Неважно";
            this.radioGnezdo1.UseVisualStyleBackColor = true;
            // 
            // radioGnezdo2
            // 
            this.radioGnezdo2.AutoSize = true;
            this.radioGnezdo2.Location = new System.Drawing.Point(6, 42);
            this.radioGnezdo2.Name = "radioGnezdo2";
            this.radioGnezdo2.Size = new System.Drawing.Size(94, 17);
            this.radioGnezdo2.TabIndex = 1;
            this.radioGnezdo2.TabStop = true;
            this.radioGnezdo2.Text = "Установлены";
            this.radioGnezdo2.UseVisualStyleBackColor = true;
            // 
            // radioGnezdo3
            // 
            this.radioGnezdo3.AutoSize = true;
            this.radioGnezdo3.Location = new System.Drawing.Point(6, 65);
            this.radioGnezdo3.Name = "radioGnezdo3";
            this.radioGnezdo3.Size = new System.Drawing.Size(108, 17);
            this.radioGnezdo3.TabIndex = 2;
            this.radioGnezdo3.TabStop = true;
            this.radioGnezdo3.Text = "Не установлены";
            this.radioGnezdo3.UseVisualStyleBackColor = true;
            // 
            // radioGrelka1
            // 
            this.radioGrelka1.AutoSize = true;
            this.radioGrelka1.Location = new System.Drawing.Point(7, 20);
            this.radioGrelka1.Name = "radioGrelka1";
            this.radioGrelka1.Size = new System.Drawing.Size(71, 17);
            this.radioGrelka1.TabIndex = 0;
            this.radioGrelka1.TabStop = true;
            this.radioGrelka1.Text = "Неважно";
            this.radioGrelka1.UseVisualStyleBackColor = true;
            // 
            // radioGrelka2
            // 
            this.radioGrelka2.AutoSize = true;
            this.radioGrelka2.Location = new System.Drawing.Point(7, 43);
            this.radioGrelka2.Name = "radioGrelka2";
            this.radioGrelka2.Size = new System.Drawing.Size(87, 17);
            this.radioGrelka2.TabIndex = 1;
            this.radioGrelka2.TabStop = true;
            this.radioGrelka2.Text = "Отсутствует";
            this.radioGrelka2.UseVisualStyleBackColor = true;
            // 
            // radioGrelka3
            // 
            this.radioGrelka3.AutoSize = true;
            this.radioGrelka3.Location = new System.Drawing.Point(6, 66);
            this.radioGrelka3.Name = "radioGrelka3";
            this.radioGrelka3.Size = new System.Drawing.Size(92, 17);
            this.radioGrelka3.TabIndex = 2;
            this.radioGrelka3.TabStop = true;
            this.radioGrelka3.Text = "Установлена";
            this.radioGrelka3.UseVisualStyleBackColor = true;
            // 
            // radioGrelka4
            // 
            this.radioGrelka4.AutoSize = true;
            this.radioGrelka4.Location = new System.Drawing.Point(6, 89);
            this.radioGrelka4.Name = "radioGrelka4";
            this.radioGrelka4.Size = new System.Drawing.Size(83, 17);
            this.radioGrelka4.TabIndex = 3;
            this.radioGrelka4.TabStop = true;
            this.radioGrelka4.Text = "Выключена";
            this.radioGrelka4.UseVisualStyleBackColor = true;
            // 
            // radioGrelka5
            // 
            this.radioGrelka5.AutoSize = true;
            this.radioGrelka5.Location = new System.Drawing.Point(6, 112);
            this.radioGrelka5.Name = "radioGrelka5";
            this.radioGrelka5.Size = new System.Drawing.Size(75, 17);
            this.radioGrelka5.TabIndex = 4;
            this.radioGrelka5.TabStop = true;
            this.radioGrelka5.Text = "Включена";
            this.radioGrelka5.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 239);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Применить";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // checkAllButton
            // 
            this.checkAllButton.Location = new System.Drawing.Point(5, 194);
            this.checkAllButton.Name = "checkAllButton";
            this.checkAllButton.Size = new System.Drawing.Size(102, 23);
            this.checkAllButton.TabIndex = 5;
            this.checkAllButton.Text = "Выбрать все";
            this.checkAllButton.UseVisualStyleBackColor = true;
            // 
            // BuildingsFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.filterGroup);
            this.Controls.Add(this.enableBox);
            this.Name = "BuildingsFilter";
            this.Size = new System.Drawing.Size(270, 320);
            this.statusGroup.ResumeLayout(false);
            this.statusGroup.PerformLayout();
            this.farmGroup.ResumeLayout(false);
            this.farmGroup.PerformLayout();
            this.filterGroup.ResumeLayout(false);
            this.gnezdoGroup.ResumeLayout(false);
            this.gnezdoGroup.PerformLayout();
            this.grelkaGroup.ResumeLayout(false);
            this.grelkaGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox enableBox;
        private System.Windows.Forms.GroupBox statusGroup;
        private System.Windows.Forms.CheckBox busyBox;
        private System.Windows.Forms.CheckBox freeBox;
        private System.Windows.Forms.GroupBox farmGroup;
        private System.Windows.Forms.CheckBox kvartaBox;
        private System.Windows.Forms.CheckBox urtaBox;
        private System.Windows.Forms.CheckBox vertepBox;
        private System.Windows.Forms.GroupBox filterGroup;
        private System.Windows.Forms.GroupBox grelkaGroup;
        private System.Windows.Forms.GroupBox gnezdoGroup;
        private System.Windows.Forms.RadioButton radioGnezdo3;
        private System.Windows.Forms.RadioButton radioGnezdo2;
        private System.Windows.Forms.RadioButton radioGnezdo1;
        private System.Windows.Forms.RadioButton radioGrelka5;
        private System.Windows.Forms.RadioButton radioGrelka4;
        private System.Windows.Forms.RadioButton radioGrelka3;
        private System.Windows.Forms.RadioButton radioGrelka2;
        private System.Windows.Forms.RadioButton radioGrelka1;
        private System.Windows.Forms.Button checkAllButton;
        private System.Windows.Forms.Button button1;
    }
}
