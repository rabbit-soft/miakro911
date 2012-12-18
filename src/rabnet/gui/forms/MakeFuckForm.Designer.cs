namespace rabnet.forms
{
    partial class MakeFuckForm
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
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBreed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFucksCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chChildren = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chHeterosis = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chInbr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chtInbrRG = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btGens = new System.Windows.Forms.Button();
            this.chHetererosis = new System.Windows.Forms.CheckBox();
            this.chInbreed = new System.Windows.Forms.CheckBox();
            this.chCandidates = new System.Windows.Forms.CheckBox();
            this.dateDays1 = new rabnet.components.DateDays();
            this.cbName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btNames = new System.Windows.Forms.Button();
            this.btOk = new ExoticControls.SplitButton();
            this.cmsOk = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSyntetic = new System.Windows.Forms.ToolStripMenuItem();
            this.chRest = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmsOk.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chState,
            this.chBreed,
            this.chFucksCount,
            this.chChildren,
            this.chHeterosis,
            this.chInbr,
            this.chtInbrRG});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 37);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.OwnerDraw = true;
            this.listView1.Size = new System.Drawing.Size(526, 231);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // chName
            // 
            this.chName.Text = "Имя";
            // 
            // chState
            // 
            this.chState.Text = "Статус";
            // 
            // chBreed
            // 
            this.chBreed.Text = "Порода";
            // 
            // chFucksCount
            // 
            this.chFucksCount.Text = "Случек";
            // 
            // chChildren
            // 
            this.chChildren.Text = "Детей";
            // 
            // chHeterosis
            // 
            this.chHeterosis.Text = "Гетерозис";
            // 
            // chInbr
            // 
            this.chInbr.Text = "Инбридинг NГ";
            this.chInbr.Width = 90;
            // 
            // chtInbrRG
            // 
            this.chtInbrRG.Text = "Инбридинг Род.Древ.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(334, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(382, 331);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btGens
            // 
            this.btGens.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btGens.Location = new System.Drawing.Point(15, 331);
            this.btGens.Name = "btGens";
            this.btGens.Size = new System.Drawing.Size(75, 23);
            this.btGens.TabIndex = 5;
            this.btGens.Text = "Гены";
            this.btGens.UseVisualStyleBackColor = true;
            this.btGens.Click += new System.EventHandler(this.btGens_Click);
            // 
            // chHetererosis
            // 
            this.chHetererosis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chHetererosis.AutoSize = true;
            this.chHetererosis.ForeColor = System.Drawing.Color.SlateBlue;
            this.chHetererosis.Location = new System.Drawing.Point(115, 274);
            this.chHetererosis.Name = "chHetererosis";
            this.chHetererosis.Size = new System.Drawing.Size(79, 17);
            this.chHetererosis.TabIndex = 7;
            this.chHetererosis.Text = "Гетерозис";
            this.chHetererosis.UseVisualStyleBackColor = true;
            this.chHetererosis.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // chInbreed
            // 
            this.chInbreed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chInbreed.AutoSize = true;
            this.chInbreed.ForeColor = System.Drawing.Color.Crimson;
            this.chInbreed.Location = new System.Drawing.Point(115, 297);
            this.chInbreed.Name = "chInbreed";
            this.chInbreed.Size = new System.Drawing.Size(81, 17);
            this.chInbreed.TabIndex = 8;
            this.chInbreed.Text = "Инбридинг";
            this.chInbreed.UseVisualStyleBackColor = true;
            this.chInbreed.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // chCandidates
            // 
            this.chCandidates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chCandidates.AutoSize = true;
            this.chCandidates.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.chCandidates.Location = new System.Drawing.Point(15, 274);
            this.chCandidates.Name = "chCandidates";
            this.chCandidates.Size = new System.Drawing.Size(82, 17);
            this.chCandidates.TabIndex = 9;
            this.chCandidates.Text = "Кандидаты";
            this.chCandidates.UseVisualStyleBackColor = true;
            this.chCandidates.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // dateDays1
            // 
            this.dateDays1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dateDays1.AutoSize = true;
            this.dateDays1.DateText = "Дата";
            this.dateDays1.DateValue = new System.DateTime(2012, 12, 18, 0, 0, 0, 0);
            this.dateDays1.DaysText = "Дней";
            this.dateDays1.DaysValue = 0;
            this.dateDays1.Location = new System.Drawing.Point(401, 274);
            this.dateDays1.Maximum = 10000;
            this.dateDays1.Name = "dateDays1";
            this.dateDays1.position = rabnet.components.DateDays.DDPosition.LABELS_LR;
            this.dateDays1.Size = new System.Drawing.Size(137, 46);
            this.dateDays1.Step = 1;
            this.dateDays1.TabIndex = 6;
            // 
            // cbName
            // 
            this.cbName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbName.FormattingEnabled = true;
            this.cbName.Location = new System.Drawing.Point(207, 6);
            this.cbName.MaxDropDownItems = 20;
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(121, 21);
            this.cbName.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(172, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Имя";
            // 
            // btNames
            // 
            this.btNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btNames.Location = new System.Drawing.Point(335, 6);
            this.btNames.Name = "btNames";
            this.btNames.Size = new System.Drawing.Size(25, 20);
            this.btNames.TabIndex = 12;
            this.btNames.Text = "...";
            this.btNames.UseVisualStyleBackColor = true;
            this.btNames.Click += new System.EventHandler(this.btNames_Click);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.ContextMenuStrip = this.cmsOk;
            this.btOk.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btOk.ImageKey = "Normal";
            this.btOk.Location = new System.Drawing.Point(463, 331);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 13;
            this.btOk.Text = "Случка";
            this.btOk.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.ButtonClick += new System.EventHandler(this.btOk_Click);
            // 
            // cmsOk
            // 
            this.cmsOk.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSyntetic});
            this.cmsOk.Name = "cmsOk";
            this.cmsOk.Size = new System.Drawing.Size(230, 26);
            // 
            // miSyntetic
            // 
            this.miSyntetic.Name = "miSyntetic";
            this.miSyntetic.Size = new System.Drawing.Size(229, 22);
            this.miSyntetic.Text = "Искусственное Осеменение";
            this.miSyntetic.Click += new System.EventHandler(this.miSyntetic_Click);
            // 
            // chRest
            // 
            this.chRest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chRest.AutoSize = true;
            this.chRest.ForeColor = System.Drawing.Color.RoyalBlue;
            this.chRest.Location = new System.Drawing.Point(15, 297);
            this.chRest.Name = "chRest";
            this.chRest.Size = new System.Drawing.Size(93, 17);
            this.chRest.TabIndex = 14;
            this.chRest.Text = "Отдыхающие";
            this.chRest.UseVisualStyleBackColor = true;
            this.chRest.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // MakeFuckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(550, 366);
            this.Controls.Add(this.chRest);
            this.Controls.Add(this.btNames);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbName);
            this.Controls.Add(this.chCandidates);
            this.Controls.Add(this.chInbreed);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.chHetererosis);
            this.Controls.Add(this.dateDays1);
            this.Controls.Add(this.btGens);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 250);
            this.Name = "MakeFuckForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Случить";
            this.cmsOk.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chState;
        private System.Windows.Forms.ColumnHeader chBreed;
        private System.Windows.Forms.ColumnHeader chFucksCount;
        private System.Windows.Forms.ColumnHeader chChildren;
        private System.Windows.Forms.ColumnHeader chHeterosis;
        private System.Windows.Forms.ColumnHeader chInbr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btGens;
        private rabnet.components.DateDays dateDays1;
        private System.Windows.Forms.CheckBox chHetererosis;
        private System.Windows.Forms.CheckBox chInbreed;
        private System.Windows.Forms.CheckBox chCandidates;
        private System.Windows.Forms.ComboBox cbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btNames;
        private ExoticControls.SplitButton btOk;
        private System.Windows.Forms.ContextMenuStrip cmsOk;
        private System.Windows.Forms.ToolStripMenuItem miSyntetic;
        private System.Windows.Forms.CheckBox chRest;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ColumnHeader chtInbrRG;
    }
}