using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class OkrolUser : Form
    {
        List<int> ids = new List<int>();
        public OkrolUser()
        {
            InitializeComponent();
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            List<String> usrs=Engine.db().getUsers(true, 0);
            for (int i = 0; i < usrs.Count / 3; i++)
            {
                comboBox1.Items.Add(usrs[i * 3]);
                ids.Add(int.Parse(usrs[i*3+2]));
            }
            comboBox1.SelectedIndex = 0;
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            if (dtpFrom.Value > dtpTo.Value)
                dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            if (dtpTo.Value < dtpFrom.Value)
                dtpFrom.Value = dtpTo.Value.AddMonths(-1).AddDays(1);
        }

        public int getUser()
        {
            if (comboBox1.SelectedIndex < 0) return 0;
            return ids[comboBox1.SelectedIndex];
        }

        public String getFromDate()
        {
            return dtpFrom.Value.ToShortDateString();
        }
        public String getToDate()
        {
            return dtpTo.Value.ToShortDateString();
        }

    }
}
