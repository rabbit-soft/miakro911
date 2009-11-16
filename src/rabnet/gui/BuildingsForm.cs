using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class BuildingsForm : Form
    {
        public BuildingsForm()
        {
            InitializeComponent();
        }

        private IDataGetter rabStatusBar1_prepareGet(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView1.Hide();
            return Engine.get().db().getBuildings("");
        }

        private void rabStatusBar1_itemGet(object sender, RabStatusBar.RSBItemEvent e)
        {
            if (e.data == null)
            {
                listView1.Show();
                return;
            }
            IBuilding b = e.data as IBuilding;
            ListViewItem li = listView1.Items.Add(b.id().ToString());
            li.SubItems.Add(b.name());
            li.SubItems.Add(b.type());
        }

        private void BuildingsForm_Activated(object sender, EventArgs e)
        {
            rabStatusBar1.run();
        }

        private void BuildingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel=true;
            Hide();
        }
    }
}
