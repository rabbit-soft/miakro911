using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AdminGRD
{
    public partial class AddDongleForm : Form
    {
        bool _manual = false;
        public AddDongleForm(string organization,uint dongleId)
        {
            InitializeComponent();
            this.Text += organization;
            tbDongleId.Text = dongleId.ToString();
            nudFlags.Minimum = 0;
            nudFlags.Maximum = 0;
            int i = 0;            
            foreach (RabGRD.GRD_Base.FlagType s in Enum.GetValues(typeof(RabGRD.GRD_Base.FlagType)))
            {
                nudFlags.Maximum += 1 << i++;
                ch.Items.Add(s.ToString());             
            }
            //updateFlags((int)nudFlags.Value);
            _manual = true;
            //dtpTimeFlags.Value = DateTime.Now;
        }

        private void ch_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void updateFlags(int flags)
        {
            if (!_manual) return;
            nudFlags.Value = flags;
            int cnt = Enum.GetValues(typeof(RabGRD.GRD_Base.FlagType)).Length;
            for (int i = 0; i < cnt;i++ )
                ch.SetItemChecked(i,(flags & 1 << i)!=0);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            updateFlags((int)nudFlags.Value);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dtpEndDate.MinDate = dtpStartDate.Value;
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            dtpStartDate.MaxDate = dtpEndDate.Value;
        }

        public int Farms { get { return (int)nudFarms.Value; } }
        public int Flags { get { return (int)nudFlags.Value; } }
        public DateTime StartDate { get { return dtpStartDate.Value; } }
        public DateTime EndDate { get { return dtpEndDate.Value; } }
        public int TimeFlags { get { return (int)nudTimeFlags.Value; } }
        public string TimeFlagsEnd { get { return dtpTimeFlags.Value.ToString("yyyy-MM-dd"); } }

        private void ch_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_manual) return;
            _manual = false;
            int result = 0;
            ch.SetItemChecked(e.Index, (e.NewValue == CheckState.Checked));
            int cnt = Enum.GetValues(typeof(RabGRD.GRD_Base.FlagType)).Length;
            for (int i = 0; i < cnt; i++)
                result += (ch.GetItemChecked(i))? 1 << i : 0;
            _manual = true;
            updateFlags(result);

            
        }
    }
}
