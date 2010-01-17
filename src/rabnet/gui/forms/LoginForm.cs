using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;
using System.Configuration;

namespace rabnet
{
    public partial class LoginForm : Form
    {
        public static bool stop = true;
        protected static readonly ILog log = LogManager.GetLogger(typeof(LoginForm));
        public LoginForm()
        {
            InitializeComponent();
            log.Debug("inited");
        }

        public void readConfig()
        {
            ConfigurationManager.GetSection("rabnetds");
            comboBox1.Items.Clear();
            foreach (RabnetConfigHandler.dataSource ds in RabnetConfigHandler.ds)
            {
                comboBox1.Items.Add(ds.name);
                if (ds.def)
                {
                    comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                    comboBox1_SelectedIndexChanged(null, null);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
                return;
            comboBox2.Focus();
            try
            {
                RabnetConfigHandler.dataSource xs = RabnetConfigHandler.ds[comboBox1.SelectedIndex];
                Engine.get().initEngine(xs.type, xs.param);
                comboBox2.Items.Clear();
                comboBox2.Enabled = false;
                textBox1.Enabled = false;
                List<String> usrs = Engine.db().getUsers(false,0);
                if (usrs != null)
                {
                    comboBox2.Enabled = true;
                    textBox1.Enabled = true;
                    foreach (String s in usrs)
                    {
                        comboBox2.Items.Add(s);
                        if (xs.defuser != "" && xs.defuser == s)
                        {
                            comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
                            if (xs.defpassword != "")
                            {
                                textBox1.Text = xs.defpassword;
                            }
                            textBox1.Focus();
                            textBox1.SelectAll();
                        }
                    }
                }
                else
                {
                    button2.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                comboBox2.SelectedIndex = comboBox1.SelectedIndex = -1;
                comboBox1.Text=comboBox2.Text=textBox1.Text = "";
                comboBox1.Focus();
                MessageBox.Show("Ошибка подключения " + ex.GetType().ToString() + ": " + ex.Message,"Ошибка подключения");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled=(comboBox2.Text!="");
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int uid=Engine.get().setUid(comboBox2.Text, textBox1.Text,comboBox1.Text);
            if (uid != 0)
            {
                DialogResult = DialogResult.OK;
                return;
            }
            MessageBox.Show("Неверное имя пользователя или пароль");

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            readConfig();
            textBox1.SelectAll();
            textBox1.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button2.PerformClick();
        }
    }
}
