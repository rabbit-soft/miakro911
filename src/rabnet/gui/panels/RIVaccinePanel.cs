using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet.panels.RabInfoPanels
{
    public partial class RIVaccinePanel : UserControl
    {
        private RabNetEngRabbit _rab;

        public RIVaccinePanel()
        {
            InitializeComponent();
        }

        public void SetRabbit(RabNetEngRabbit rab)
        {
            _rab = rab;
            updateRabVac();
        }

        private void updateRabVac()
        {
            lvVaccine.Items.Clear();
            foreach (RabVac rv in _rab.Vaccines)
            {
                ListViewItem lvi = lvVaccine.Items.Add(rv.date.ToShortDateString());
                if(rv.vid>0)
                    lvi.SubItems.Add(String.Format("{0:d}:{1:s}",rv.vid,rv.name));
                else
                    lvi.SubItems.Add(rv.name);
                lvi.SubItems.Add(rv.remains.ToString());
                lvi.SubItems.Add(rv.unabled?"ДА":"-");
            }
        }

        private void btAddVac_Click(object sender, EventArgs e)
        {
            AddRabVacForm dlg = new AddRabVacForm(_rab);
            if (dlg.ShowDialog() == DialogResult.OK)
            {               
                _rab.SetVaccine(dlg.VacID, dlg.VacDate, dlg.VacChildren);
                updateRabVac();
            }
        }
    }
}
