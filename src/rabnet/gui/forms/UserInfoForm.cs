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
        private int action = 0;
        private int uid=0;
        public UserInfoForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        public UserInfoForm(int id,int action):this()
        {
            this.uid = id;
            this.action = action;
            if (action == 1)
                textBox1.Enabled = comboBox1.Enabled = false;
            checkBox1.Checked = groupBox1.Enabled = (id == 0 || action == 1);
            checkBox1.Enabled = !groupBox1.Enabled;
            if (uid != 0)
            {
                List<String> ui = Engine.db().getUsers(true, uid);
                textBox1.Text = ui[0];
                comboBox1.SelectedIndex = (ui[1] == "admin" ? 0 : 1);
            }
            if (uid == Engine.get().uId())
                comboBox1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                    if (textBox2.Text != textBox3.Text)
                        throw new ApplicationException("Пароли не совпадают.");
                if (uid == 0)
                    Engine.get().addUser(textBox1.Text, comboBox1.SelectedIndex, textBox2.Text);
                else
                    Engine.get().updateUser(uid, textBox1.Text, comboBox1.SelectedIndex, textBox2.Text, checkBox1.Checked);
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
    }
}
