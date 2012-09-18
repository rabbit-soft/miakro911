using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class DeadReasonChangeForm : Form
    {
        private CatalogData dr = null;

        public DeadReasonChangeForm(int id,string name,string curReason)
        {
            InitializeComponent();
            label1.Text = name;
            cbOldReason.Items.Add(curReason);
            cbOldReason.SelectedIndex = 0;
            dr = Engine.get().db().getDeadReasons().Get();
            foreach (CatalogData.Row row in dr.Rows)
            {
                if(row.data[0] != curReason)
                    cbNewReason.Items.Add(row.data[0]);
            }
        }

        private void cbNewReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            btOk.Enabled = cbNewReason.SelectedIndex != -1;
        }

        public int NewReason
        {
            get
            {
                foreach (CatalogData.Row row in dr.Rows)
                {
                    if (row.data[0] == cbNewReason.Text)
                        return row.key;
                }
                return 0;
            }
        }
    }
}
