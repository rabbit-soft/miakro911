using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using pEngine;

namespace AdminGRD
{
    public partial class LoginFrom : Form
    {
        private const string DEF_INFO = "Выберите ключ и введите пароль";
        private const int SERV_H = 40;
        private const int PASS_H = 100;

        private bool _sos = true;
        private bool _spc = true;
        private List<string> _usrList = null;

        public LoginFrom()
        {
            InitializeComponent();
            toolTip1.SetToolTip(btPassChange, "Сменить пароль");
            ShowOwnServer = false;
            ShowPassChange = false;
#if DEBUG
            this.BackColor = System.Drawing.Color.BurlyWood;
            this.btKey.Visible = true;
#endif         
        }

        private string SelectedFile
        {
            get 
            {
                return comboBox1.Text;
            }
        }
        private string Password { get { return tbPass.Text; } }

        public void FocusPassword()
        {
            tbPass.Focus();
        }

        private string NewPassword
        {
            get
            {
                return tbNewPass.Text;
            }
        }

        private void fillUsers()
        {
            comboBox1.Items.Clear();
            _usrList = Engine.GetUserKeys();
            string defU = Engine.Opt.GetStringOption(optType.DefaultUser);
            int index = 0;
            foreach (string u in _usrList)
            {
                if (defU == u)
                    index = comboBox1.Items.Count;
                comboBox1.Items.Add(u);               
            }
            if (comboBox1.Items.Count == 0)
            {
                comboBox1.Text = "Ключей не найдено";
                comboBox1.Enabled = false;
            }
            else
                comboBox1.SelectedIndex = index;
        }

        private void LoginFrom_Load(object sender, EventArgs e)
        {
            tbPass.Select();
            fillUsers();
        }

        private void btOwnServer_Click(object sender, EventArgs e)
        {
            ShowOwnServer = !gbServ.Visible;
        }

        private bool ShowOwnServer
        {
            get { return _sos; }
            set
            {
                if (_sos == value) return;
                _sos = value;
                tbServ.Clear();
                gbServ.Visible = value;
                this.Height += value ? SERV_H : -SERV_H;
                textBox1_TextChanged(null, null);
            }
        }

        /// <summary>
        /// Ввод пароля
        /// </summary>
        private bool ShowPassChange
        {
            get { return _spc; }
            set
            {
                if (_spc == value) return;
                _spc = value;
                if (value)
                    lbInfo.Text = @"Введите текущий пароль в поле 'Пароль'
Новый пароль укажите в поле 'Установить новый пароль'";
                else lbInfo.Text = DEF_INFO;
                tbNewPass.Clear();
                tbNewPassConf.Clear();
                lbError.Visible = false;

                gbNewPass.Visible = value;
                this.Height += value ? PASS_H : -PASS_H;
                textBox1_TextChanged(null, null);
            }
        }
               

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            tbPass.BackColor = tbPass.Text.Length == 0 ? MyHelper.ErrorColor : MyHelper.NormalColor;
            if(gbServ.Visible)
                tbServ.BackColor = tbServ.Text.Length == 0 ? MyHelper.ErrorColor : MyHelper.NormalColor;
            if (gbNewPass.Visible)
            {
                tbNewPass.BackColor = tbNewPass.Text.Length == 0 ? MyHelper.ErrorColor : MyHelper.NormalColor;
                tbNewPassConf.BackColor = tbNewPassConf.Text.Length == 0 ? MyHelper.ErrorColor : MyHelper.NormalColor;
                lbError.Visible = tbNewPass.Text != tbNewPassConf.Text;
            }
            
            btOk.Enabled = tbPass.Text.Length > 0 && (!gbServ.Visible || (gbServ.Visible && tbServ.Text.Length > 0))
                && (!gbNewPass.Visible || (gbNewPass.Visible && tbNewPass.Text!="" && !lbError.Visible));
        }

        public string Server { get { return tbServ.Text; } }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbPass.Text = "";
            ShowPassChange = false;
        }

        private void btPassChange_Click(object sender, EventArgs e)
        {
            ShowPassChange = !tbNewPass.Visible;
        }

        private void btKey_Click(object sender, EventArgs e)
        {
#if DEBUG
            NewKeyForm dlg = new NewKeyForm();
            dlg.ShowDialog();
            fillUsers();
#endif
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Engine.LogIn(SelectedFile, Password, Server, NewPassword);
            }
            catch (pException pex)
            {
                string title = "Ошибка";
                MessageBoxIcon icon = MessageBoxIcon.Stop;
                if (pex.Code == pException.NeedChangeUserPass)
                {
                    ShowPassChange = (pex.Code == pException.NeedChangeUserPass);
                    title = "";
                    icon = MessageBoxIcon.Information;
                }
                MessageBox.Show(pex.Message, title, MessageBoxButtons.OK, icon);
                this.DialogResult = DialogResult.None;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                tbPass.Clear();
            }
        }
    }
}
