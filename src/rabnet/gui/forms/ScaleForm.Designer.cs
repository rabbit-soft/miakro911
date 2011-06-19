namespace rabnet
{
    partial class ScaleForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScaleForm));
            this.listView1 = new System.Windows.Forms.ListView();
            this.chPLUID = new System.Windows.Forms.ColumnHeader();
            this.chPLUCode = new System.Windows.Forms.ColumnHeader();
            this.chPLUProdName1 = new System.Windows.Forms.ColumnHeader();
            this.chPLUProdName2 = new System.Windows.Forms.ColumnHeader();
            this.срPLUPrice = new System.Windows.Forms.ColumnHeader();
            this.chPLULiveTime = new System.Windows.Forms.ColumnHeader();
            this.chPLUTara = new System.Windows.Forms.ColumnHeader();
            this.chPLUGroupCode = new System.Windows.Forms.ColumnHeader();
            this.chPLUMessage = new System.Windows.Forms.ColumnHeader();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lvMSG = new System.Windows.Forms.ListView();
            this.chMSGid = new System.Windows.Forms.ColumnHeader();
            this.chMSGtext = new System.Windows.Forms.ColumnHeader();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tbSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.tslbScaleMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.tScaleMessageClear = new System.Windows.Forms.Timer(this.components);
            this.tLoadFromScaleChecker = new System.Windows.Forms.Timer(this.components);
            this.cmPLU = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miPLUChange = new System.Windows.Forms.ToolStripMenuItem();
            this.miPLUadd = new System.Windows.Forms.ToolStripMenuItem();
            this.miPLUdel = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.cmPLU.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPLUID,
            this.chPLUCode,
            this.chPLUProdName1,
            this.chPLUProdName2,
            this.срPLUPrice,
            this.chPLULiveTime,
            this.chPLUTara,
            this.chPLUGroupCode,
            this.chPLUMessage});
            this.listView1.ContextMenuStrip = this.cmPLU;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(769, 184);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // chPLUID
            // 
            this.chPLUID.Text = "№";
            this.chPLUID.Width = 54;
            // 
            // chPLUCode
            // 
            this.chPLUCode.Text = "Код";
            this.chPLUCode.Width = 46;
            // 
            // chPLUProdName1
            // 
            this.chPLUProdName1.Text = "Название1";
            this.chPLUProdName1.Width = 108;
            // 
            // chPLUProdName2
            // 
            this.chPLUProdName2.Text = "Название2";
            this.chPLUProdName2.Width = 111;
            // 
            // срPLUPrice
            // 
            this.срPLUPrice.Text = "Цена";
            // 
            // chPLULiveTime
            // 
            this.chPLULiveTime.Text = "Срок годности";
            this.chPLULiveTime.Width = 96;
            // 
            // chPLUTara
            // 
            this.chPLUTara.Text = "Тара (КГ)";
            this.chPLUTara.Width = 64;
            // 
            // chPLUGroupCode
            // 
            this.chPLUGroupCode.Text = "Групповой Код";
            this.chPLUGroupCode.Width = 99;
            // 
            // chPLUMessage
            // 
            this.chPLUMessage.Text = "Сообщение (№)";
            this.chPLUMessage.Width = 96;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(769, 449);
            this.splitContainer1.SplitterDistance = 207;
            this.splitContainer1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(769, 184);
            this.panel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(769, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Записи продукции в весах";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 21);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lvMSG);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.textBox1);
            this.splitContainer2.Size = new System.Drawing.Size(769, 217);
            this.splitContainer2.SplitterDistance = 465;
            this.splitContainer2.TabIndex = 2;
            // 
            // lvMSG
            // 
            this.lvMSG.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chMSGid,
            this.chMSGtext});
            this.lvMSG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMSG.FullRowSelect = true;
            this.lvMSG.GridLines = true;
            this.lvMSG.Location = new System.Drawing.Point(0, 0);
            this.lvMSG.MultiSelect = false;
            this.lvMSG.Name = "lvMSG";
            this.lvMSG.Size = new System.Drawing.Size(465, 217);
            this.lvMSG.TabIndex = 0;
            this.lvMSG.UseCompatibleStateImageBehavior = false;
            this.lvMSG.View = System.Windows.Forms.View.Details;
            this.lvMSG.SelectedIndexChanged += new System.EventHandler(this.lvMSG_SelectedIndexChanged);
            // 
            // chMSGid
            // 
            this.chMSGid.Text = "№";
            // 
            // chMSGtext
            // 
            this.chMSGtext.Text = "Текст Сообщения";
            this.chMSGtext.Width = 400;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(300, 217);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(769, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Сообщения";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbRefresh,
            this.tbSave,
            this.toolStripProgressBar1,
            this.tslbScaleMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 452);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(769, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tbRefresh
            // 
            this.tbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tbRefresh.Image")));
            this.tbRefresh.Name = "tbRefresh";
            this.tbRefresh.Size = new System.Drawing.Size(77, 20);
            this.tbRefresh.Text = "Обновить";
            this.tbRefresh.Click += new System.EventHandler(this.tbRefresh_Click);
            // 
            // tbSave
            // 
            this.tbSave.Image = ((System.Drawing.Image)(resources.GetObject("tbSave.Image")));
            this.tbSave.Name = "tbSave";
            this.tbSave.Size = new System.Drawing.Size(82, 20);
            this.tbSave.Text = "Сохранить";
            this.tbSave.Click += new System.EventHandler(this.tbRefresh_Click);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBar1.Visible = false;
            // 
            // tslbScaleMessage
            // 
            this.tslbScaleMessage.Name = "tslbScaleMessage";
            this.tslbScaleMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // tScaleMessageClear
            // 
            this.tScaleMessageClear.Interval = 15000;
            this.tScaleMessageClear.Tick += new System.EventHandler(this.tScaleMessageClear_Tick);
            // 
            // tLoadFromScaleChecker
            // 
            this.tLoadFromScaleChecker.Interval = 1000;
            this.tLoadFromScaleChecker.Tick += new System.EventHandler(this.tLoadFromScaleChecker_Tick);
            // 
            // cmPLU
            // 
            this.cmPLU.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miPLUChange,
            this.miPLUadd,
            this.miPLUdel});
            this.cmPLU.Name = "cmPLU";
            this.cmPLU.Size = new System.Drawing.Size(153, 92);
            this.cmPLU.Opening += new System.ComponentModel.CancelEventHandler(this.cmPLU_Opening);
            // 
            // miPLUChange
            // 
            this.miPLUChange.Name = "miPLUChange";
            this.miPLUChange.Size = new System.Drawing.Size(152, 22);
            this.miPLUChange.Text = "Изменить";
            // 
            // miPLUadd
            // 
            this.miPLUadd.Name = "miPLUadd";
            this.miPLUadd.Size = new System.Drawing.Size(152, 22);
            this.miPLUadd.Text = "Добавить";
            // 
            // miPLUdel
            // 
            this.miPLUdel.Name = "miPLUdel";
            this.miPLUdel.Size = new System.Drawing.Size(152, 22);
            this.miPLUdel.Text = "Удалить";
            // 
            // ScaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 474);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ScaleForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Продукция Весов";
            this.Load += new System.EventHandler(this.ScaleForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmPLU.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader chPLUID;
        private System.Windows.Forms.ColumnHeader chPLUCode;
        private System.Windows.Forms.ColumnHeader chPLUProdName1;
        private System.Windows.Forms.ColumnHeader chPLUProdName2;
        private System.Windows.Forms.ColumnHeader срPLUPrice;
        private System.Windows.Forms.ColumnHeader chPLULiveTime;
        private System.Windows.Forms.ColumnHeader chPLUTara;
        private System.Windows.Forms.ColumnHeader chPLUGroupCode;
        private System.Windows.Forms.ColumnHeader chPLUMessage;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView lvMSG;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader chMSGid;
        private System.Windows.Forms.ColumnHeader chMSGtext;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton tbRefresh;
        private System.Windows.Forms.ToolStripStatusLabel tslbScaleMessage;
        private System.Windows.Forms.ToolStripButton tbSave;
        private System.Windows.Forms.Timer tScaleMessageClear;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Timer tLoadFromScaleChecker;
        private System.Windows.Forms.ContextMenuStrip cmPLU;
        private System.Windows.Forms.ToolStripMenuItem miPLUChange;
        private System.Windows.Forms.ToolStripMenuItem miPLUadd;
        private System.Windows.Forms.ToolStripMenuItem miPLUdel;
    }
}