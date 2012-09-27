namespace rabnet.panels
{
    partial class FarmsPanel
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FarmsPanel));
            this.cbName = new System.Windows.Forms.ComboBox();
            this.btAdd = new System.Windows.Forms.CheckBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btEdit = new System.Windows.Forms.CheckBox();
            this.btDelete = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbAdmin = new System.Windows.Forms.GroupBox();
            this.chCreate = new System.Windows.Forms.CheckBox();
            this.tbAdminPass = new System.Windows.Forms.TextBox();
            this.tbAdmin = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.gbConn = new System.Windows.Forms.GroupBox();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.tbDB = new System.Windows.Forms.TextBox();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.gbAdmin.SuspendLayout();
            this.gbConn.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbName
            // 
            this.cbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbName.FormattingEnabled = true;
            this.cbName.Location = new System.Drawing.Point(80, 9);
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(149, 21);
            this.cbName.TabIndex = 0;
            this.cbName.SelectedIndexChanged += new System.EventHandler(this.cbName_SelectedIndexChanged);
            // 
            // btAdd
            // 
            this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAdd.Appearance = System.Windows.Forms.Appearance.Button;
            this.btAdd.ImageIndex = 0;
            this.btAdd.ImageList = this.imageList1;
            this.btAdd.Location = new System.Drawing.Point(235, 3);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(30, 30);
            this.btAdd.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btAdd, "Добавить новое");
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.CheckedChanged += new System.EventHandler(this.btAdd_CheckedChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "round_plus.png");
            this.imageList1.Images.SetKeyName(1, "cog_icon.png");
            this.imageList1.Images.SetKeyName(2, "delete.png");
            this.imageList1.Images.SetKeyName(3, "round_checkmark.png");
            this.imageList1.Images.SetKeyName(4, "round_delete.png");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Имя фермы:";
            // 
            // btEdit
            // 
            this.btEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btEdit.Appearance = System.Windows.Forms.Appearance.Button;
            this.btEdit.Enabled = false;
            this.btEdit.ImageIndex = 1;
            this.btEdit.ImageList = this.imageList1;
            this.btEdit.Location = new System.Drawing.Point(271, 3);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(30, 30);
            this.btEdit.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btEdit, "Редактировать БД");
            this.btEdit.UseVisualStyleBackColor = true;
            this.btEdit.CheckedChanged += new System.EventHandler(this.btAdd_CheckedChanged);
            // 
            // btDelete
            // 
            this.btDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDelete.Enabled = false;
            this.btDelete.ImageIndex = 2;
            this.btDelete.ImageList = this.imageList1;
            this.btDelete.Location = new System.Drawing.Point(307, 3);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(30, 30);
            this.btDelete.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btDelete, "Удалить");
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 1000;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // gbAdmin
            // 
            this.gbAdmin.Controls.Add(this.chCreate);
            this.gbAdmin.Controls.Add(this.tbAdminPass);
            this.gbAdmin.Controls.Add(this.tbAdmin);
            this.gbAdmin.Controls.Add(this.label7);
            this.gbAdmin.Controls.Add(this.label6);
            this.gbAdmin.Location = new System.Drawing.Point(40, 39);
            this.gbAdmin.Name = "gbAdmin";
            this.gbAdmin.Size = new System.Drawing.Size(260, 83);
            this.gbAdmin.TabIndex = 7;
            this.gbAdmin.TabStop = false;
            this.gbAdmin.Text = "Создание БД";
            // 
            // chCreate
            // 
            this.chCreate.AutoSize = true;
            this.chCreate.Checked = true;
            this.chCreate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chCreate.Location = new System.Drawing.Point(6, 0);
            this.chCreate.Name = "chCreate";
            this.chCreate.Size = new System.Drawing.Size(87, 17);
            this.chCreate.TabIndex = 12;
            this.chCreate.Text = "Создать БД";
            this.chCreate.UseVisualStyleBackColor = true;
            this.chCreate.CheckedChanged += new System.EventHandler(this.chCreate_CheckedChanged);
            // 
            // tbAdminPass
            // 
            this.tbAdminPass.Location = new System.Drawing.Point(106, 43);
            this.tbAdminPass.Name = "tbAdminPass";
            this.tbAdminPass.PasswordChar = '*';
            this.tbAdminPass.Size = new System.Drawing.Size(140, 20);
            this.tbAdminPass.TabIndex = 11;
            // 
            // tbAdmin
            // 
            this.tbAdmin.Location = new System.Drawing.Point(106, 19);
            this.tbAdmin.Name = "tbAdmin";
            this.tbAdmin.Size = new System.Drawing.Size(140, 20);
            this.tbAdmin.TabIndex = 10;
            this.tbAdmin.Tag = "";
            this.tbAdmin.Text = "root";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(55, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Пароль";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Администратор";
            // 
            // gbConn
            // 
            this.gbConn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.gbConn.Controls.Add(this.tbPass);
            this.gbConn.Controls.Add(this.tbHost);
            this.gbConn.Controls.Add(this.tbDB);
            this.gbConn.Controls.Add(this.tbUser);
            this.gbConn.Controls.Add(this.label5);
            this.gbConn.Controls.Add(this.label4);
            this.gbConn.Controls.Add(this.label3);
            this.gbConn.Controls.Add(this.label2);
            this.gbConn.Location = new System.Drawing.Point(40, 128);
            this.gbConn.Name = "gbConn";
            this.gbConn.Size = new System.Drawing.Size(260, 140);
            this.gbConn.TabIndex = 8;
            this.gbConn.TabStop = false;
            this.gbConn.Text = "Параметры подключения";
            // 
            // tbPass
            // 
            this.tbPass.Location = new System.Drawing.Point(106, 94);
            this.tbPass.Name = "tbPass";
            this.tbPass.ReadOnly = true;
            this.tbPass.Size = new System.Drawing.Size(140, 20);
            this.tbPass.TabIndex = 12;
            this.tbPass.Text = "krol";
            // 
            // tbHost
            // 
            this.tbHost.Location = new System.Drawing.Point(106, 16);
            this.tbHost.Name = "tbHost";
            this.tbHost.ReadOnly = true;
            this.tbHost.Size = new System.Drawing.Size(140, 20);
            this.tbHost.TabIndex = 7;
            this.tbHost.Text = "localhost";
            // 
            // tbDB
            // 
            this.tbDB.Location = new System.Drawing.Point(106, 42);
            this.tbDB.Name = "tbDB";
            this.tbDB.ReadOnly = true;
            this.tbDB.Size = new System.Drawing.Size(140, 20);
            this.tbDB.TabIndex = 9;
            this.tbDB.Text = "kroliki";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(106, 68);
            this.tbUser.Name = "tbUser";
            this.tbUser.ReadOnly = true;
            this.tbUser.Size = new System.Drawing.Size(140, 20);
            this.tbUser.TabIndex = 10;
            this.tbUser.Text = "kroliki";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Пароль БД";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Пользователь";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "База данных";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Хост";
            // 
            // btOk
            // 
            this.btOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOk.ImageIndex = 3;
            this.btOk.ImageList = this.imageList1;
            this.btOk.Location = new System.Drawing.Point(173, 274);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 25);
            this.btOk.TabIndex = 9;
            this.btOk.Text = "Готово";
            this.btOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Visible = false;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btCancel.ImageKey = "round_delete.png";
            this.btCancel.ImageList = this.imageList1;
            this.btCancel.Location = new System.Drawing.Point(92, 274);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 25);
            this.btCancel.TabIndex = 10;
            this.btCancel.Text = "Отмена";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Visible = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // FarmsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.gbConn);
            this.Controls.Add(this.gbAdmin);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.btEdit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.cbName);
            this.MaximumSize = new System.Drawing.Size(0, 315);
            this.MinimumSize = new System.Drawing.Size(340, 315);
            this.Name = "FarmsPanel";
            this.Size = new System.Drawing.Size(340, 315);
            this.gbAdmin.ResumeLayout(false);
            this.gbAdmin.PerformLayout();
            this.gbConn.ResumeLayout(false);
            this.gbConn.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbName;
        private System.Windows.Forms.CheckBox btAdd;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox btEdit;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox gbAdmin;
        private System.Windows.Forms.TextBox tbAdminPass;
        private System.Windows.Forms.TextBox tbAdmin;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox gbConn;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.TextBox tbHost;
        private System.Windows.Forms.TextBox tbDB;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.CheckBox chCreate;
    }
}
