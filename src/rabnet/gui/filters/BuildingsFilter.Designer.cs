namespace rabnet.filters
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
            this.btnAll = new System.Windows.Forms.Button();
            this.krolBox = new System.Windows.Forms.CheckBox();
            this.barinBox = new System.Windows.Forms.CheckBox();
            this.dvukrolBox = new System.Windows.Forms.CheckBox();
            this.komplexBox = new System.Windows.Forms.CheckBox();
            this.hizhinaBox = new System.Windows.Forms.CheckBox();
            this.kvartaBox = new System.Windows.Forms.CheckBox();
            this.urtaBox = new System.Windows.Forms.CheckBox();
            this.vertepBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFarm = new System.Windows.Forms.ComboBox();
            this.cbGnezdo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbGrelka = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.saveBox = new System.Windows.Forms.ComboBox();
            this.gotovo = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.farmGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // farmGroup
            // 
            this.farmGroup.Controls.Add(this.btnAll);
            this.farmGroup.Controls.Add(this.krolBox);
            this.farmGroup.Controls.Add(this.barinBox);
            this.farmGroup.Controls.Add(this.dvukrolBox);
            this.farmGroup.Controls.Add(this.komplexBox);
            this.farmGroup.Controls.Add(this.hizhinaBox);
            this.farmGroup.Controls.Add(this.kvartaBox);
            this.farmGroup.Controls.Add(this.urtaBox);
            this.farmGroup.Controls.Add(this.vertepBox);
            this.farmGroup.Location = new System.Drawing.Point(130, 3);
            this.farmGroup.Name = "farmGroup";
            this.farmGroup.Size = new System.Drawing.Size(176, 141);
            this.farmGroup.TabIndex = 2;
            this.farmGroup.TabStop = false;
            this.farmGroup.Text = "Тип Фермы";
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(25, 111);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(114, 23);
            this.btnAll.TabIndex = 11;
            this.btnAll.Text = "Выбрать все";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // krolBox
            // 
            this.krolBox.AutoSize = true;
            this.krolBox.Location = new System.Drawing.Point(74, 19);
            this.krolBox.Name = "krolBox";
            this.krolBox.Size = new System.Drawing.Size(85, 17);
            this.krolBox.TabIndex = 7;
            this.krolBox.Text = "Крольчихин";
            this.krolBox.UseVisualStyleBackColor = true;
            // 
            // barinBox
            // 
            this.barinBox.AutoSize = true;
            this.barinBox.Location = new System.Drawing.Point(6, 88);
            this.barinBox.Name = "barinBox";
            this.barinBox.Size = new System.Drawing.Size(57, 17);
            this.barinBox.TabIndex = 6;
            this.barinBox.Text = "Барин";
            this.barinBox.UseVisualStyleBackColor = true;
            // 
            // dvukrolBox
            // 
            this.dvukrolBox.AutoSize = true;
            this.dvukrolBox.Location = new System.Drawing.Point(74, 42);
            this.dvukrolBox.Name = "dvukrolBox";
            this.dvukrolBox.Size = new System.Drawing.Size(104, 17);
            this.dvukrolBox.TabIndex = 8;
            this.dvukrolBox.Text = "Двукрольчихин";
            this.dvukrolBox.UseVisualStyleBackColor = true;
            // 
            // komplexBox
            // 
            this.komplexBox.AutoSize = true;
            this.komplexBox.Location = new System.Drawing.Point(74, 65);
            this.komplexBox.Name = "komplexBox";
            this.komplexBox.Size = new System.Drawing.Size(97, 17);
            this.komplexBox.TabIndex = 9;
            this.komplexBox.Text = "Комплексный";
            this.komplexBox.UseVisualStyleBackColor = true;
            // 
            // hizhinaBox
            // 
            this.hizhinaBox.AutoSize = true;
            this.hizhinaBox.Location = new System.Drawing.Point(74, 88);
            this.hizhinaBox.Name = "hizhinaBox";
            this.hizhinaBox.Size = new System.Drawing.Size(65, 17);
            this.hizhinaBox.TabIndex = 10;
            this.hizhinaBox.Text = "Хижина";
            this.hizhinaBox.UseVisualStyleBackColor = true;
            // 
            // kvartaBox
            // 
            this.kvartaBox.AutoSize = true;
            this.kvartaBox.Location = new System.Drawing.Point(6, 65);
            this.kvartaBox.Name = "kvartaBox";
            this.kvartaBox.Size = new System.Drawing.Size(62, 17);
            this.kvartaBox.TabIndex = 5;
            this.kvartaBox.Text = "Кварта";
            this.kvartaBox.UseVisualStyleBackColor = true;
            // 
            // urtaBox
            // 
            this.urtaBox.AutoSize = true;
            this.urtaBox.Location = new System.Drawing.Point(6, 42);
            this.urtaBox.Name = "urtaBox";
            this.urtaBox.Size = new System.Drawing.Size(52, 17);
            this.urtaBox.TabIndex = 4;
            this.urtaBox.Text = "Юрта";
            this.urtaBox.UseVisualStyleBackColor = true;
            // 
            // vertepBox
            // 
            this.vertepBox.AutoSize = true;
            this.vertepBox.Location = new System.Drawing.Point(6, 19);
            this.vertepBox.Name = "vertepBox";
            this.vertepBox.Size = new System.Drawing.Size(62, 17);
            this.vertepBox.TabIndex = 3;
            this.vertepBox.Text = "Вертеп";
            this.vertepBox.UseVisualStyleBackColor = true;
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
            this.cbFarm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFarm.FormattingEnabled = true;
            this.cbFarm.Items.AddRange(new object[] {
            "Все",
            "Занятые",
            "Свободные"});
            this.cbFarm.Location = new System.Drawing.Point(3, 22);
            this.cbFarm.MaxDropDownItems = 3;
            this.cbFarm.Name = "cbFarm";
            this.cbFarm.Size = new System.Drawing.Size(121, 21);
            this.cbFarm.TabIndex = 0;
            // 
            // cbGnezdo
            // 
            this.cbGnezdo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGnezdo.FormattingEnabled = true;
            this.cbGnezdo.Items.AddRange(new object[] {
            "Неважно",
            "Отсутствуют",
            "Установлены"});
            this.cbGnezdo.Location = new System.Drawing.Point(3, 65);
            this.cbGnezdo.MaxDropDownItems = 3;
            this.cbGnezdo.Name = "cbGnezdo";
            this.cbGnezdo.Size = new System.Drawing.Size(121, 21);
            this.cbGnezdo.TabIndex = 1;
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(3, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Грелка";
            // 
            // cbGrelka
            // 
            this.cbGrelka.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGrelka.FormattingEnabled = true;
            this.cbGrelka.Items.AddRange(new object[] {
            "Неважно",
            "Отсутствуют",
            "Установлены",
            "Выключены",
            "Включены"});
            this.cbGrelka.Location = new System.Drawing.Point(3, 108);
            this.cbGrelka.MaxDropDownItems = 5;
            this.cbGrelka.Name = "cbGrelka";
            this.cbGrelka.Size = new System.Drawing.Size(121, 21);
            this.cbGrelka.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(231, 150);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // saveBox
            // 
            this.saveBox.FormattingEnabled = true;
            this.saveBox.Location = new System.Drawing.Point(104, 152);
            this.saveBox.Name = "saveBox";
            this.saveBox.Size = new System.Drawing.Size(121, 21);
            this.saveBox.TabIndex = 13;
            // 
            // gotovo
            // 
            this.gotovo.Location = new System.Drawing.Point(3, 150);
            this.gotovo.Name = "gotovo";
            this.gotovo.Size = new System.Drawing.Size(30, 23);
            this.gotovo.TabIndex = 12;
            this.gotovo.Text = "OK";
            this.gotovo.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(39, 150);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // BuildingsFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.gotovo);
            this.Controls.Add(this.saveBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbGrelka);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbGnezdo);
            this.Controls.Add(this.cbFarm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.farmGroup);
            this.FilterCombo = this.saveBox;
            this.HideBtn = this.gotovo;
            this.Name = "BuildingsFilter";
            this.SaveButton = this.button1;
            this.Size = new System.Drawing.Size(310, 178);
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
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.CheckBox krolBox;
        private System.Windows.Forms.CheckBox barinBox;
        private System.Windows.Forms.CheckBox dvukrolBox;
        private System.Windows.Forms.CheckBox komplexBox;
        private System.Windows.Forms.CheckBox hizhinaBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbFarm;
        private System.Windows.Forms.ComboBox cbGnezdo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbGrelka;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox saveBox;
        private System.Windows.Forms.Button gotovo;
        private System.Windows.Forms.Button button2;
    }
}
