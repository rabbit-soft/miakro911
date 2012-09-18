using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class AddRabVacForm : Form
    {
        private CatalogData _vacc;

        public AddRabVacForm(int rabId)
        {
            InitializeComponent();
            _vacc = Engine.db().getVaccines().Get();
            foreach (CatalogData.Row r in _vacc.Rows)
            {
                cbVaccineType.Items.Add(r.data[1]);
            }
        }

        public int VacID { get { return _vacc.Rows[cbVaccineType.SelectedIndex].key; } }
        public bool VacChildren { get { return chWithChildren.Checked; } }

        private void cbVaccineType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btOk.Visible = true;
        }
    }
}
