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
        private List<Vaccine> _vacc;
        private RabNetEngRabbit _rab;
        //private int _forceVacId=-1;

        public AddRabVacForm(RabNetEngRabbit rab,bool withChildrens,int forceId)
        {
            InitializeComponent();
            dateDays1.Maximum = rab.Age;
            chWithChildren.Enabled = withChildrens;
            _rab = rab;
            _vacc = Engine.db().GetVaccines();
            int ind=-1;
            foreach (Vaccine v in _vacc)
            {              
                cbVaccineType.Items.Add(v.Name);
                if (forceId != -1 && v.ID == forceId)
                    ind=cbVaccineType.Items.Count-1;
            }
            if (forceId != -1)
            {
                cbVaccineType.SelectedIndex = ind;
                cbVaccineType.Enabled = false;
            }
        }
        public AddRabVacForm(RabNetEngRabbit rab):this(rab,true,-1) { }

        public int VacID { get { return _vacc[cbVaccineType.SelectedIndex].ID; } }
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
