using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class UserInfoForm : Form
    {
        /*
         * Тип юзеров берется не из базы, они находятся в проге
         * при редактировании надо внести изменения в 
         * db.mysql/Users.getUser
         * 
         * по идее надо сделать чтобы тип юзеров брался из базы
         */
        /// <summary>
        /// Действие. 1-сменить пароль
        /// </summary>
        private int _action = 0;
        private int _uid=0;
        public UserInfoForm()
        {
            InitializeComponent();
            cbUserType.SelectedIndex = 0;
        }

        public UserInfoForm(int id,int action):this()
        {
            this._uid = id;
            this._action = action;
            if (action == 1)
                tbUserName.Enabled = cbUserType.Enabled = false;
            checkBox1.Checked = groupBox1.Enabled = (id == 0 || action == 1);
            checkBox1.Enabled = !groupBox1.Enabled;
            if (_uid != 0)
            {
                sUser ui = Engine.db().getUser(_uid);
                tbUserName.Text = ui.Name;
                switch (ui.Group)
                {
                    case sUser.Admin: cbUserType.SelectedIndex = 0; break;
                    case sUser.Zootech: cbUserType.SelectedIndex = 1; break;
                    case sUser.Butcher: cbUserType.SelectedIndex = 2; break;
                }
            }
            if (_uid == Engine.get().uId())
                cbUserType.Enabled = false;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                    if (tbPass1.Text != tbPass2.Text)
                        throw new ApplicationException("Пароли не совпадают.");
                if (_uid == 0)
                    Engine.get().addUser(tbUserName.Text, cbUserType.SelectedIndex, tbPass1.Text);
                else 
                    Engine.get().updateUser(_uid, tbUserName.Text, cbUserType.SelectedIndex, tbPass1.Text, checkBox1.Checked);
                Close();
            }
            catch(ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = checkBox1.Checked;
        }

        private void tbPass1_TextChanged(object sender, EventArgs e)
        {
            if (cbUserType.SelectedIndex != 2) return;
            List<char> numbers = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            TextBox tb = (sender as TextBox);
            try
            {
                ulong.Parse(tb.Text);
            }
            catch (FormatException)
            {
                if (tb.Text != "")
                {
                    for (int i = 0; i < tb.Text.Length; i++)
                    {
                        if (!numbers.Contains(tb.Text[i]))
                        {
                            tb.Text = tb.Text.Remove(i, 1);
                            tb.Select(i, 0);
                            break;
                        }
                    }
                }
            }

        }

        private void cbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbUserType.SelectedIndex == 2)
            {
                lbWarning.Text = "Пароль должен состоять только из цифр";
                lbWarning.Show();
            }
            else lbWarning.Hide();
            
        }
    }
}
