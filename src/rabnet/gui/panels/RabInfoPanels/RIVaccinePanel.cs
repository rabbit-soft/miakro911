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
        public RIVaccinePanel()
        {
            InitializeComponent();
        }

        public void FillVaccines(int rabId)
        {
            String[][] vacc = Engine.db().GetRabVac(rabId);
            foreach (string[] s in vacc)
            {
                ListViewItem lvi = lvVaccine.Items.Add(s[0]);
                lvi.SubItems.Add(s[1]);
                lvi.SubItems.Add(s[2]);
                lvi.SubItems.Add(s[3]);
            }
        }
    }
}
