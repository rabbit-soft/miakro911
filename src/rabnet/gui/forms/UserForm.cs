﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
            update();
        }

        public void update()
        {
            if (!Engine.get().isAdmin())
                return;
            listView1.Items.Clear();
            List<String> usr = Engine.db().getUsers(true,0);
            for (int i = 0; i < usr.Count / 3; i++)
            {
                ListViewItem li = listView1.Items.Add(usr[i * 3]);
                li.SubItems.Add(usr[i * 3 + 1]);
                li.Tag = int.Parse(usr[i * 3 + 2]);
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
            if ((int)listView1.SelectedItems[0].Tag != Engine.get().uId())
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
