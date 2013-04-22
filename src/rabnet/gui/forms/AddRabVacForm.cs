using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet.forms
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
            if (rab.YoungCount > 0)
                chWithChildren.Enabled = withChildrens;
            else
                chWithChildren.Visible = false;
            _rab = rab;
            _vacc = Engine.db().GetVaccines(true);
            int ind=-1;
            for (int i = 0; i < _vacc.Count; )
            {
                if (_vacc[i].ID == Vaccine.V_ID_LUST && _rab.Sex != Rabbit.SexType.FEMALE)
                {
                    _vacc.Remove(_vacc[i]);
                    continue;
                }
                cbVaccineType.Items.Add(_vacc[i].Name);
                if (forceId != 0 && _vacc[i].ID == forceId)
                    ind = cbVaccineType.Items.Count - 1;
                i++;
            }
            if (forceId != 0)
            {
                cbVaccineType.SelectedIndex = ind;
                cbVaccineType.Enabled = false;
            }
        }
        public AddRabVacForm(RabNetEngRabbit rab):this(rab,true,0) { }

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
                    if(MessageBox.Show("Данный кролик уже привит данной прививкой. Желаете сделать ревакцинацию", "Вакцинация", MessageBoxButtons.YesNo)!=DialogResult.Yes)
                        this.DialogResult = DialogResult.None;
                }
            }
        }
    }
}
