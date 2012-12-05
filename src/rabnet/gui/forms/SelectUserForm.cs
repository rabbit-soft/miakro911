using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet.forms
{
    public partial class SelectUserForm : Form
    {
        List<int> ids = new List<int>();
        int selected = 0;
        String selectednm = "";
        public SelectUserForm()
        {
            InitializeComponent();
            comboBox1.Items.Add("");
            ids.Add(0);
            List<sUser> us = Engine.db().GetUsers();
            for (int i = 0; i < us.Count; i++)
            {
                comboBox1.Items.Add(us[i].Name);
                ids.Add(us[i].Id);
            }
            comboBox1.SelectedIndex = 0;
        }

        public SelectUserForm(int uid):this()
        {
            for (int i = 0; i < ids.Count; i++)
                if (ids[i] == uid)
                    comboBox1.SelectedIndex = i;
        }
        public SelectUserForm(String uname):this()
        {
            for (int i = 0; i < ids.Count; i++)
            {
                String s=comboBox1.Items[i] as String;
                if (s == uname)
                    comboBox1.SelectedIndex = i;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selected = ids[comboBox1.SelectedIndex];
            selectednm = comboBox1.Text;
            Close();
        }

        public int SelectedUser { get { return selected; } }
        public string SelectedUserName { get { return selectednm; } }

    }
}
