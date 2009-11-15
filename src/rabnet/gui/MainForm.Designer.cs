namespace rabnet
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.фермаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сменитьФермуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.постройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.молоднякToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.учетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.зоотехпланToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.блокнотToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.фильтрToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.параметрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.забоиПривесыСписанияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.внеплановыеПересадкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.архивToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.генеалогическоеДеревоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.именаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.породыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показыватьТипыЯрусовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пказыватьОтделенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сокращенияВТаблицахToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.двойныеФамилииToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.номерПередИменемToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показыватьНомераToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.разрешенГетерозисToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.разрешенИнбридингToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rabStatusBar1 = new rabnet.RabStatusBar();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.фермаToolStripMenuItem,
            this.видToolStripMenuItem,
            this.настройкиToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(812, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // фермаToolStripMenuItem
            // 
            this.фермаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сменитьФермуToolStripMenuItem,
            this.toolStripMenuItem1,
            this.выходToolStripMenuItem});
            this.фермаToolStripMenuItem.Name = "фермаToolStripMenuItem";
            this.фермаToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.фермаToolStripMenuItem.Text = "Ферма";
            // 
            // сменитьФермуToolStripMenuItem
            // 
            this.сменитьФермуToolStripMenuItem.Name = "сменитьФермуToolStripMenuItem";
            this.сменитьФермуToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.сменитьФермуToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.сменитьФермуToolStripMenuItem.Text = "Сменить ферму/пользователя ...";
            this.сменитьФермуToolStripMenuItem.Click += new System.EventHandler(this.ChangeFarmMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(290, 6);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // видToolStripMenuItem
            // 
            this.видToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.постройкиToolStripMenuItem,
            this.молоднякToolStripMenuItem,
            this.учетToolStripMenuItem,
            this.зоотехпланToolStripMenuItem,
            this.блокнотToolStripMenuItem,
            this.фильтрToolStripMenuItem,
            this.параметрыToolStripMenuItem,
            this.забоиПривесыСписанияToolStripMenuItem,
            this.внеплановыеПересадкиToolStripMenuItem,
            this.архивToolStripMenuItem,
            this.toolStripMenuItem2,
            this.генеалогическоеДеревоToolStripMenuItem,
            this.toolStripMenuItem3,
            this.именаToolStripMenuItem,
            this.породыToolStripMenuItem});
            this.видToolStripMenuItem.Name = "видToolStripMenuItem";
            this.видToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.видToolStripMenuItem.Text = "Вид";
            // 
            // постройкиToolStripMenuItem
            // 
            this.постройкиToolStripMenuItem.Name = "постройкиToolStripMenuItem";
            this.постройкиToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.постройкиToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.постройкиToolStripMenuItem.Text = "Постройки";
            // 
            // молоднякToolStripMenuItem
            // 
            this.молоднякToolStripMenuItem.Name = "молоднякToolStripMenuItem";
            this.молоднякToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.молоднякToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.молоднякToolStripMenuItem.Text = "Молодняк";
            // 
            // учетToolStripMenuItem
            // 
            this.учетToolStripMenuItem.Name = "учетToolStripMenuItem";
            this.учетToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.учетToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.учетToolStripMenuItem.Text = "Учет";
            // 
            // зоотехпланToolStripMenuItem
            // 
            this.зоотехпланToolStripMenuItem.Name = "зоотехпланToolStripMenuItem";
            this.зоотехпланToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.зоотехпланToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.зоотехпланToolStripMenuItem.Text = "Зоотехплан";
            // 
            // блокнотToolStripMenuItem
            // 
            this.блокнотToolStripMenuItem.Name = "блокнотToolStripMenuItem";
            this.блокнотToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.блокнотToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.блокнотToolStripMenuItem.Text = "Блокнот";
            // 
            // фильтрToolStripMenuItem
            // 
            this.фильтрToolStripMenuItem.Name = "фильтрToolStripMenuItem";
            this.фильтрToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.фильтрToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.фильтрToolStripMenuItem.Text = "Фильтр";
            // 
            // параметрыToolStripMenuItem
            // 
            this.параметрыToolStripMenuItem.Name = "параметрыToolStripMenuItem";
            this.параметрыToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.параметрыToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.параметрыToolStripMenuItem.Text = "Параметры";
            // 
            // забоиПривесыСписанияToolStripMenuItem
            // 
            this.забоиПривесыСписанияToolStripMenuItem.Name = "забоиПривесыСписанияToolStripMenuItem";
            this.забоиПривесыСписанияToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.забоиПривесыСписанияToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.забоиПривесыСписанияToolStripMenuItem.Text = "Забои, привесы, списания";
            // 
            // внеплановыеПересадкиToolStripMenuItem
            // 
            this.внеплановыеПересадкиToolStripMenuItem.Name = "внеплановыеПересадкиToolStripMenuItem";
            this.внеплановыеПересадкиToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.внеплановыеПересадкиToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.внеплановыеПересадкиToolStripMenuItem.Text = "Внеплановые пересадки";
            // 
            // архивToolStripMenuItem
            // 
            this.архивToolStripMenuItem.Name = "архивToolStripMenuItem";
            this.архивToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F3)));
            this.архивToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.архивToolStripMenuItem.Text = "Архив";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(255, 6);
            // 
            // генеалогическоеДеревоToolStripMenuItem
            // 
            this.генеалогическоеДеревоToolStripMenuItem.Name = "генеалогическоеДеревоToolStripMenuItem";
            this.генеалогическоеДеревоToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.генеалогическоеДеревоToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.генеалогическоеДеревоToolStripMenuItem.Text = "Генеалогическое дерево";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(255, 6);
            // 
            // именаToolStripMenuItem
            // 
            this.именаToolStripMenuItem.Name = "именаToolStripMenuItem";
            this.именаToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.именаToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.именаToolStripMenuItem.Text = "Имена";
            // 
            // породыToolStripMenuItem
            // 
            this.породыToolStripMenuItem.Name = "породыToolStripMenuItem";
            this.породыToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.породыToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.породыToolStripMenuItem.Text = "Породы";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.показыватьТипыЯрусовToolStripMenuItem,
            this.пказыватьОтделенияToolStripMenuItem,
            this.сокращенияВТаблицахToolStripMenuItem,
            this.двойныеФамилииToolStripMenuItem,
            this.toolStripMenuItem4,
            this.номерПередИменемToolStripMenuItem,
            this.показыватьНомераToolStripMenuItem,
            this.toolStripMenuItem5,
            this.разрешенГетерозисToolStripMenuItem,
            this.разрешенИнбридингToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // показыватьТипыЯрусовToolStripMenuItem
            // 
            this.показыватьТипыЯрусовToolStripMenuItem.Name = "показыватьТипыЯрусовToolStripMenuItem";
            this.показыватьТипыЯрусовToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.показыватьТипыЯрусовToolStripMenuItem.Text = "Показывать типы ярусов";
            // 
            // пказыватьОтделенияToolStripMenuItem
            // 
            this.пказыватьОтделенияToolStripMenuItem.Name = "пказыватьОтделенияToolStripMenuItem";
            this.пказыватьОтделенияToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.пказыватьОтделенияToolStripMenuItem.Text = "Пказывать типы отделений";
            // 
            // сокращенияВТаблицахToolStripMenuItem
            // 
            this.сокращенияВТаблицахToolStripMenuItem.Name = "сокращенияВТаблицахToolStripMenuItem";
            this.сокращенияВТаблицахToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.сокращенияВТаблицахToolStripMenuItem.Text = "Сокращения в таблицах";
            // 
            // двойныеФамилииToolStripMenuItem
            // 
            this.двойныеФамилииToolStripMenuItem.Name = "двойныеФамилииToolStripMenuItem";
            this.двойныеФамилииToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.двойныеФамилииToolStripMenuItem.Text = "Двойные фамилии";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(225, 6);
            // 
            // номерПередИменемToolStripMenuItem
            // 
            this.номерПередИменемToolStripMenuItem.Name = "номерПередИменемToolStripMenuItem";
            this.номерПередИменемToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.номерПередИменемToolStripMenuItem.Text = "Номер перед именем";
            // 
            // показыватьНомераToolStripMenuItem
            // 
            this.показыватьНомераToolStripMenuItem.Name = "показыватьНомераToolStripMenuItem";
            this.показыватьНомераToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.показыватьНомераToolStripMenuItem.Text = "Показывать номера";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(225, 6);
            // 
            // разрешенГетерозисToolStripMenuItem
            // 
            this.разрешенГетерозисToolStripMenuItem.Name = "разрешенГетерозисToolStripMenuItem";
            this.разрешенГетерозисToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.разрешенГетерозисToolStripMenuItem.Text = "Разрешен гетерозис";
            // 
            // разрешенИнбридингToolStripMenuItem
            // 
            this.разрешенИнбридингToolStripMenuItem.Name = "разрешенИнбридингToolStripMenuItem";
            this.разрешенИнбридингToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.разрешенИнбридингToolStripMenuItem.Text = "Разрешен инбридинг";
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе ...";
            // 
            // rabStatusBar1
            // 
            this.rabStatusBar1.Location = new System.Drawing.Point(0, 507);
            this.rabStatusBar1.Name = "rabStatusBar1";
            this.rabStatusBar1.Size = new System.Drawing.Size(812, 23);
            this.rabStatusBar1.TabIndex = 5;
            this.rabStatusBar1.Text = "rabStatusBar1";
            this.rabStatusBar1.itemGet += new rabnet.RabStatusBar.RSBItemEventHandler(this.rabStatusBar1_itemGet);
            this.rabStatusBar1.prepareGet += new rabnet.RabStatusBar.RSBPrepareEventHandler(this.rabStatusBar1_prepareGet);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.Location = new System.Drawing.Point(38, 42);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(688, 408);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(725, 481);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 530);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.rabStatusBar1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem фермаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сменитьФермуToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem видToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem постройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem молоднякToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem учетToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem зоотехпланToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem блокнотToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem фильтрToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem параметрыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem забоиПривесыСписанияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem внеплановыеПересадкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem архивToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem генеалогическоеДеревоToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem именаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem породыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показыватьТипыЯрусовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пказыватьОтделенияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сокращенияВТаблицахToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem двойныеФамилииToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem номерПередИменемToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показыватьНомераToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem разрешенГетерозисToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem разрешенИнбридингToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private RabStatusBar rabStatusBar1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button button1;
    }
}

