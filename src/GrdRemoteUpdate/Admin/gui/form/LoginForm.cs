using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using pEngine;

namespace AdminGRD
{
    public partial class LoginFrom : Form
    {
        private const int SERV_H = 40;
        private const int PASS_H = 100;

        private List<string> _usrList = null;
        public LoginFrom()
        {
            InitializeComponent();
            showOwnServer(false);
            showPassChange(false);
            toolTip1.SetToolTip(btPassChange, "Сменить пароль");
#if DEBUG
            this.BackColor = System.Drawing.Color.BurlyWood;
#endif          
        }

        public string SelectedFile
        {
            get 
            {
                return comboBox1.Text;
            }
        }
        public string Password { get { return tbPass.Text; } }

        public void FocusPassword()
        {
            tbPass.Focus();
        }

        public string NewPassword
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
            string defU = Engine.Options.GetStringOption(optType.DefaultUser)+".psk";
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
            fillUsers();
        }

        private void btOwnServer_Click(object sender, EventArgs e)
        {
            showOwnServer(!gbServ.Visible);
        }

        private void showOwnServer(bool show)
        {
            tbServ.Clear();
            gbServ.Visible = show;
            this.Height += show ? SERV_H : -SERV_H;
            textBox1_TextChanged(null, null);
        }

        private void showPassChange(bool show)
        {

            tbNewPass.Clear();
            tbNewPassConf.Clear();
            lbError.Visible = false;

            gbNewPass.Visible = show;
            this.Height += show ? PASS_H : -PASS_H;
            textBox1_TextChanged(null, null);
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
        }

        private void btPassChange_Click(object sender, EventArgs e)
        {
            showPassChange(!tbNewPass.Visible);
        }
    }
}
