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
        private RabNetEngRabbit _rab;

        public AddRabVacForm(RabNetEngRabbit rab,bool withChildrens)
        {
            InitializeComponent();
            chWithChildren.Enabled = withChildrens;
            _rab = rab;
            _vacc = Engine.db().getVaccines().Get();
            foreach (CatalogData.Row r in _vacc.Rows)
            {
                cbVaccineType.Items.Add(r.data[0]);
            }
        }
        public AddRabVacForm(RabNetEngRabbit rab):this(rab,true) { }

        public int VacID { get { return _vacc.Rows[cbVaccineType.SelectedIndex].key; } }
        public bool VacChildren { get { return chWithChildren.Checked; } }

        private void cbVaccineType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btOk.Visible = true;
        }

        public DateTime VacDate { get { return dateDays1.DateValue; } }

        private void btOk_Click(object sender, EventArgs e)
        {
            foreach (RabVac rv in _rab.Vaccines)
            {
                if (rv.vid == this.VacID && !rv.unabled && rv.remains != 0)
                {
                    MessageBox.Show("Данный кролик уже привит данной прививкой", "", MessageBoxButtons.OK);
                    this.DialogResult = DialogResult.None;
                }
            }
        }
    }
}
