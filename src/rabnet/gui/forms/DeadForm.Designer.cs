namespace rabnet
{
    partial class DeadForm
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.miChangeReason = new System.Windows.Forms.ToolStripMenuItem();
            this.rsb = new rabnet.RabStatusBar();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.OwnerDraw = true;
            this.listView1.Size = new System.Drawing.Size(754, 448);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Имя";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Дата Списания";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Возраст";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Группа";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Порода";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Адрес";
            this.columnHeader6.Width = 104;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Причина списания";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Заметки";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRestore,
            this.miChangeReason});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // miRestore
            // 
            this.miRestore.Name = "miRestore";
            this.miRestore.Size = new System.Drawing.Size(172, 22);
            this.miRestore.Text = "Восстановить";
            this.miRestore.Click += new System.EventHandler(this.miRestore_Click);
            // 
            // miChangeReason
            // 
            this.miChangeReason.Name = "miChangeReason";
            this.miChangeReason.Size = new System.Drawing.Size(172, 22);
            this.miChangeReason.Text = "Изменит причину";
            this.miChangeReason.Click += new System.EventHandler(this.miChangeReason_Click);
            // 
            // rsb
            // 
            this.rsb.ExcelButtonClick = null;
            this.rsb.filterPanel = null;
            this.rsb.Location = new System.Drawing.Point(0, 463);
            this.rsb.Name = "rsb";
            this.rsb.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.rsb.Size = new System.Drawing.Size(778, 23);
            this.rsb.TabIndex = 0;
            this.rsb.Text = "rabStatusBar1";
            this.rsb.itemGet += new rabnet.RabStatusBar.RSBItemEventHandler(this.rsb_itemGet);
            this.rsb.PrepareGet += new rabnet.RabStatusBar.RSBPrepareEventHandler(this.rsb_prepareGet);
            // 
            // DeadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 486);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.rsb);
            this.MinimizeBox = false;
            this.Name = "DeadForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Архив списаний";
            this.Activated += new System.EventHandler(this.DeadForm_Activated);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RabStatusBar rsb;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miRestore;
        private System.Windows.Forms.ToolStripMenuItem miChangeReason;
    }
}