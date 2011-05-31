using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace butcher
{
    public partial class FarmListForm : Form
    {
        public FarmListForm()
        {
            InitializeComponent();
            updateList();
        }

        public void updateList()
        {
            listView1.Tag = 1;
            listView1.Items.Clear();
            foreach (RabnetConfig.rabDataSource ds in RabnetConfig.DataSources)
            {
                ListViewItem li = listView1.Items.Add(ds.Name);
                li.Checked = !ds.Hidden;
                li.SubItems.Add(ds.SavePassword ? "ДА" : "нет");
                li.SubItems.Add(ds.Params.ToString());
                li.Tag = ds;
            }
            listView1.Tag = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listView1.Items)
                ((RabnetConfig.rabDataSource)li.Tag).Hidden = !li.Checked;
            RabnetConfig.SaveDataSources();
            Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button4.Enabled=button3.Enabled = listView1.SelectedItems.Count == 1;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            button2.PerformClick();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            new FarmChangeForm((RabnetConfig.rabDataSource)listView1.SelectedItems[0].Tag).ShowDialog();
            updateList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new FarmChangeForm(null).ShowDialog();
            updateList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            RabnetConfig.DataSources.Remove((RabnetConfig.rabDataSource)listView1.SelectedItems[0].Tag);
            updateList();
        }
    }
}
