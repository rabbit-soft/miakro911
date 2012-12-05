using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet.forms
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
            initialHints();
            update();
        }

        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;

            toolTip.SetToolTip(button1,"Изменить настройки выделенного пользователя");
            toolTip.SetToolTip(button2,"Добавить нового пользователя");
            toolTip.SetToolTip(button3,"Закрыть окно");
            toolTip.SetToolTip(button4,"Сменить пароль выделенного пользователя");
            toolTip.SetToolTip(button5,"Удалить выделенного пользователя");
        }
        public void update()
        {
            if (!Engine.get().isAdmin())
                return;
            listView1.Items.Clear();
            List<sUser> usr = Engine.db().GetUsers();
            for (int i = 0; i < usr.Count; i++)
            {
                ListViewItem li = listView1.Items.Add(usr[i].Name);
                li.SubItems.Add(usr[i].GetRusUserName());
                li.Tag = usr[i].Id;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (new UserInfoForm(0, 0)).ShowDialog();
            update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            (new UserInfoForm((int)listView1.SelectedItems[0].Tag,0)).ShowDialog();
            update();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = button4.Enabled=button5.Enabled=false;
            if (listView1.SelectedItems.Count != 1) return;
            button1.Enabled = button4.Enabled =true;
            if ((int)listView1.SelectedItems[0].Tag != Engine.get().userId)
                button5.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            (new UserInfoForm((int)listView1.SelectedItems[0].Tag, 1)).ShowDialog();
            update();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            try
            {
                Engine.get().delUser((int)listView1.SelectedItems[0].Tag);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            update();
        }
    }
}
