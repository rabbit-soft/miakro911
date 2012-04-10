using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using pEngine;
using RabGRD;

namespace AdminGRD
{
    public partial class AddDongleForm : Form
    {
        bool _manual = false;
        sClient _client;

        public AddDongleForm(sClient client,uint dongleId)
        {
            InitializeComponent();
            this.Text += client.Organization;
            _client = client;
            tbDongleId.Text = dongleId.ToString();
            nudFlags.Minimum = 0;
            nudFlags.Maximum = 0;
            int i = 0;            
            foreach (GRD_Base.FlagType s in Enum.GetValues(typeof(GRD_Base.FlagType)))
            {
                nudFlags.Maximum += 1 << i;
                ch.Items.Add(s.ToString());
                if (_client.SAAS && s == GRD_Base.FlagType.SAASVersion)
                {
                    nudFlags.Value += 1 << i;
                    ch.SetItemChecked(i, true);
                }
                i++;
            }

            if (!_client.SAAS)
            {
                const int YEARS = 20;
                nudMonths.Enabled =
                dtpStartDate.Enabled =
                dtpEndDate.Enabled = false;
                dtpEndDate.Value = dtpStartDate.Value.AddYears(YEARS);
                nudMonths.Value = YEARS * 12;
            }

            _manual = true;
            calcMoney();
        }

        public AddDongleForm(sClient client, sDongle dongle)
            :this(client,uint.Parse(dongle.Id))
        {
            _manual = false;
            if (client.SAAS)
                dtpStartDate.Value = DateTime.Parse(dongle.EndDate);
            else
            {
                dtpStartDate.Value = DateTime.Parse(dongle.StartDate);
                dtpStartDate.Enabled = true;
            }
            dtpEndDate.Value = DateTime.Parse(dongle.EndDate);                       
            dtpStartDate.Enabled = false;
            nudMonths.Enabled =
            dtpEndDate.Enabled = true;
            nudFarms.Value = int.Parse(dongle.Farms);
            nudFlags.Value = int.Parse(dongle.Flags);
            _manual = true;
        }

        private void ch_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void updateFlags(int flags)
        {
            if (!_manual) return;
            nudFlags.Value = flags;
            int cnt = Enum.GetValues(typeof(GRD_Base.FlagType)).Length;
            for (int i = 0; i < cnt;i++ )
                ch.SetItemChecked(i,(flags & 1 << i)!=0);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            updateFlags((int)nudFlags.Value);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (!_manual) return;
            dtpEndDate.MinDate = dtpStartDate.Value;
            calcMoney();
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            if (!_manual) return;
            dtpStartDate.MaxDate = dtpEndDate.Value;
            calcMoney();
        }

        public int Farms { get { return (int)nudFarms.Value; } }
        public int Flags { get { return (int)nudFlags.Value; } }
        public DateTime StartDate { get { return dtpStartDate.Value; } }
        public DateTime EndDate { get { return dtpEndDate.Value; } }
        public int TimeFlags { get { return (int)nudTimeFlags.Value; } }
        public string TimeFlagsEnd { get { return dtpTimeFlags.Value.ToString("yyyy-MM-dd"); } }

        /// <summary>
        /// Событие происходит, а только потом устанавливает галочку
        /// </summary>
        private void ch_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_manual) return;
            if (1 << e.Index == (int)GRD_Base.FlagType.SAASVersion)
                e.NewValue = _client.SAAS ? CheckState.Checked : CheckState.Unchecked;          
            _manual = false;
            int result = 0;
            ch.SetItemChecked(e.Index, (e.NewValue == CheckState.Checked)); //ставим быстрей галочку
            int cnt = Enum.GetValues(typeof(RabGRD.GRD_Base.FlagType)).Length;
            for (int i = 0; i < cnt; i++)
                result += (ch.GetItemChecked(i))? 1 << i : 0;
            _manual = true;
            updateFlags(result);           
        }

        private void calcMoney()
        {
            if(!_manual) return;
            _manual = false;
            if (_client.SAAS)
            {
                int months = calcMonths();
                dtpEndDate.Value = dtpStartDate.Value.AddMonths(months);
                nudMonths.Value = months;
                tbPrice.Text = (months * 5 * Farms).ToString(); //TODO 5- рублей за миниферму в месяц
            }
            else
            {
                tbPrice.Text = (Farms * 100).ToString(); //TODO 100 рублей за миниферму в коробочной версии           
            }

            _manual = true;
        }

        private int calcMonths()
        {
            int result = (dtpEndDate.Value.Year - dtpStartDate.Value.Year) * 12;
            result += dtpEndDate.Value.Month - dtpStartDate.Value.Month;
            return result;
        }

        private void nudMonths_ValueChanged(object sender, EventArgs e)
        {
            dtpEndDate.Value = dtpStartDate.Value.AddMonths((int)nudMonths.Value);
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(tbPrice.Text) > int.Parse(_client.Money))               
                    throw new Exception("Не достаточно денег");                                
                if (dtpStartDate.Value.Date == dtpEndDate.Value.Date)
                    throw new Exception("Дата начала и окончания совпадают");                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                this.DialogResult = DialogResult.None;
            }
        }

        private void nudFarms_ValueChanged(object sender, EventArgs e)
        {
            calcMoney();
        }
    }
}
