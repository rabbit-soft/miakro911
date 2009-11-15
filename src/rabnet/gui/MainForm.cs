using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace rabnet
{
    public partial class MainForm : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        public MainForm()
        {
            InitializeComponent();
            log.Debug("Program started");
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChangeFarmMenuItem_Click(object sender, EventArgs e)
        {
            (new LoginForm()).ShowDialog();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            Text = Engine.get().farmName();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new BuildingsForm()).Show();
        }

        private IDataGetter rabStatusBar1_prepareGet(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView1.Hide();
            return Engine.get().db().getRabbits("");
        }

        private void rabStatusBar1_itemGet(object sender, RabStatusBar.RSBItemEvent e)
        {
            if (e.data==null)
            {
                listView1.Show();
                return;
            }
            IRabbit rab = (e.data as IRabbit);
            ListViewItem li = listView1.Items.Add(rab.id().ToString());
            li.SubItems.Add(rab.name());
            li.SubItems.Add(rab.surname());
            li.SubItems.Add(rab.secname());
            li.SubItems.Add(rab.sex());
        }


    }
}
