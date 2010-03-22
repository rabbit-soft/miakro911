using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabdump
{
    public partial class MainForm : Form
    {
        bool canclose = false;
        bool manual = true;
        public MainForm()
        {
            InitializeComponent();
        }

        private void restoreMenuItem_Click(object sender, EventArgs e)
        {
            manual = false;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            manual = true;
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            canclose = (MessageBox.Show(this,"Выйти из программы?","rabdump",MessageBoxButtons.YesNo)==DialogResult.Yes);
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !canclose;
            manual = false;
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            manual = true;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            restoreMenuItem.PerformClick();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = Icon;
            propertyGrid1.SelectedObject = Options.get();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (!manual) return;
            if (WindowState == FormWindowState.Minimized)
                ShowInTaskbar = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Options.get().load();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Options.get().save();
            Close();
        }
    }
}
