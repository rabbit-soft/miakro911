namespace rabnet
{
    partial class ReportViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportViewForm));
            this.rdlViewer1 = new fyiReporting.RdlViewer.RdlViewer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.scaleBtn = new System.Windows.Forms.ToolStripSplitButton();
            this.widthScaleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageScaleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.отчетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.печатьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиПечатиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.сохранитьКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.масштабToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.поШиринеЛистаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.одинЛистToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdlViewer1
            // 
            this.rdlViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rdlViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.rdlViewer1.Folder = null;
            this.rdlViewer1.HighlightAll = false;
            this.rdlViewer1.HighlightAllColor = System.Drawing.Color.Fuchsia;
            this.rdlViewer1.HighlightCaseSensitive = false;
            this.rdlViewer1.HighlightItemColor = System.Drawing.Color.Aqua;
            this.rdlViewer1.HighlightPageItem = null;
            this.rdlViewer1.HighlightText = null;
            this.rdlViewer1.Location = new System.Drawing.Point(0, 52);
            this.rdlViewer1.Name = "rdlViewer1";
            this.rdlViewer1.PageCurrent = 1;
            this.rdlViewer1.Parameters = null;
            this.rdlViewer1.ReportName = null;
            this.rdlViewer1.ScrollMode = fyiReporting.RdlViewer.ScrollModeEnum.Continuous;
            this.rdlViewer1.SelectTool = false;
            this.rdlViewer1.ShowFindPanel = false;
            this.rdlViewer1.ShowParameterPanel = false;
            this.rdlViewer1.ShowWaitDialog = true;
            this.rdlViewer1.Size = new System.Drawing.Size(787, 418);
            this.rdlViewer1.SourceFile = null;
            this.rdlViewer1.SourceRdl = null;
            this.rdlViewer1.TabIndex = 0;
            this.rdlViewer1.Text = "rdlViewer1";
            this.rdlViewer1.UseTrueMargins = true;
            this.rdlViewer1.Zoom = 0.3704159F;
            this.rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitPage;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.scaleBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(787, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Сохранить";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Печать";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Настройки печати";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel1.Text = "Масштаб:";
            // 
            // scaleBtn
            // 
            this.scaleBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.scaleBtn.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.widthScaleMenuItem,
            this.pageScaleMenuItem,
            this.toolStripMenuItem2,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem4,
            this.toolStripMenuItem7});
            this.scaleBtn.Image = ((System.Drawing.Image)(resources.GetObject("scaleBtn.Image")));
            this.scaleBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scaleBtn.Name = "scaleBtn";
            this.scaleBtn.Size = new System.Drawing.Size(76, 22);
            this.scaleBtn.Text = "Один лист";
            this.scaleBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            // 
            // widthScaleMenuItem
            // 
            this.widthScaleMenuItem.Name = "widthScaleMenuItem";
            this.widthScaleMenuItem.Size = new System.Drawing.Size(191, 22);
            this.widthScaleMenuItem.Text = "По ширине страницы";
            this.widthScaleMenuItem.Click += new System.EventHandler(this.widthScaleMenuItem_Click);
            // 
            // pageScaleMenuItem
            // 
            this.pageScaleMenuItem.Name = "pageScaleMenuItem";
            this.pageScaleMenuItem.Size = new System.Drawing.Size(191, 22);
            this.pageScaleMenuItem.Text = "Один лист";
            this.pageScaleMenuItem.Click += new System.EventHandler(this.pageScaleMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(188, 6);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(191, 22);
            this.toolStripMenuItem5.Text = "50%";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem10_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(191, 22);
            this.toolStripMenuItem6.Text = "100%";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(191, 22);
            this.toolStripMenuItem4.Text = "150%";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(191, 22);
            this.toolStripMenuItem7.Text = "200%";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem12_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.отчетToolStripMenuItem,
            this.масштабToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(787, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // отчетToolStripMenuItem
            // 
            this.отчетToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.печатьToolStripMenuItem,
            this.настройкиПечатиToolStripMenuItem,
            this.toolStripMenuItem1,
            this.сохранитьКакToolStripMenuItem});
            this.отчетToolStripMenuItem.Name = "отчетToolStripMenuItem";
            this.отчетToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.отчетToolStripMenuItem.Text = "Отчет";
            // 
            // печатьToolStripMenuItem
            // 
            this.печатьToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("печатьToolStripMenuItem.Image")));
            this.печатьToolStripMenuItem.Name = "печатьToolStripMenuItem";
            this.печатьToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.печатьToolStripMenuItem.Text = "Печать";
            this.печатьToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // настройкиПечатиToolStripMenuItem
            // 
            this.настройкиПечатиToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("настройкиПечатиToolStripMenuItem.Image")));
            this.настройкиПечатиToolStripMenuItem.Name = "настройкиПечатиToolStripMenuItem";
            this.настройкиПечатиToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.настройкиПечатиToolStripMenuItem.Text = "Настройки принтера...";
            this.настройкиПечатиToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(199, 6);
            // 
            // сохранитьКакToolStripMenuItem
            // 
            this.сохранитьКакToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("сохранитьКакToolStripMenuItem.Image")));
            this.сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            this.сохранитьКакToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.сохранитьКакToolStripMenuItem.Text = "Сохранить как...";
            this.сохранитьКакToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // масштабToolStripMenuItem
            // 
            this.масштабToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.поШиринеЛистаToolStripMenuItem,
            this.одинЛистToolStripMenuItem,
            this.toolStripMenuItem3,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.toolStripMenuItem9,
            this.toolStripMenuItem12});
            this.масштабToolStripMenuItem.Name = "масштабToolStripMenuItem";
            this.масштабToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.масштабToolStripMenuItem.Text = "Масштаб";
            // 
            // поШиринеЛистаToolStripMenuItem
            // 
            this.поШиринеЛистаToolStripMenuItem.Name = "поШиринеЛистаToolStripMenuItem";
            this.поШиринеЛистаToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.поШиринеЛистаToolStripMenuItem.Text = "По ширине страницы";
            this.поШиринеЛистаToolStripMenuItem.Click += new System.EventHandler(this.widthScaleMenuItem_Click);
            // 
            // одинЛистToolStripMenuItem
            // 
            this.одинЛистToolStripMenuItem.Name = "одинЛистToolStripMenuItem";
            this.одинЛистToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.одинЛистToolStripMenuItem.Text = "Один лист";
            this.одинЛистToolStripMenuItem.Click += new System.EventHandler(this.pageScaleMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(188, 6);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(191, 22);
            this.toolStripMenuItem10.Text = "50%";
            this.toolStripMenuItem10.Click += new System.EventHandler(this.toolStripMenuItem10_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(191, 22);
            this.toolStripMenuItem11.Text = "100%";
            this.toolStripMenuItem11.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(191, 22);
            this.toolStripMenuItem9.Text = "150%";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(191, 22);
            this.toolStripMenuItem12.Text = "200%";
            this.toolStripMenuItem12.Click += new System.EventHandler(this.toolStripMenuItem12_Click);
            // 
            // sfd
            // 
            this.sfd.Filter = "Файл PDF (*.pdf)|*.pdf|Файл XML (*.xml)|*.xml|Файл HTML (*.html)|*.html|Файл CSV " +
                "(*.csv)|*.csv|Файл RTF (*.rtf)|*.rtf|Файл TIF (*.tif)|*.tif|Файл Excel (*.xlsx)|" +
                "*.xlsx|Файл MHT (*.mht)|*.mht";
            // 
            // ReportViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 482);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.rdlViewer1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimizeBox = false;
            this.Name = "ReportViewForm";
            this.ShowIcon = false;
            this.Text = "Отчет";
            this.Load += new System.EventHandler(this.ReportViewForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private fyiReporting.RdlViewer.RdlViewer rdlViewer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem отчетToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem печатьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиПечатиToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSplitButton scaleBtn;
        private System.Windows.Forms.ToolStripMenuItem widthScaleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pageScaleMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem масштабToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem поШиринеЛистаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem одинЛистToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}