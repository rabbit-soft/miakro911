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
        DateTime _from = DateTime.Now;
        int _flags =0;
        int _timeFlags =0;

        public int SAAS_Farm_Cost = 5;
        public int BOX_Farm_Cost = 100;

#if PROTECTED
        public AddDongleForm(sClient client,uint dongleId,bool flags)
        {
            InitializeComponent();
            this.Text += client.Organization;
            _client = client;
            tbDongleId.Text = dongleId.ToString();
            int i = 0;            
            foreach (GRD_Base.FlagType s in Enum.GetValues(typeof(GRD_Base.FlagType)))
            {
                chbFlags.Items.Add(s.ToString());
                chbTimeFlags.Items.Add(s.ToString());
                if (_client.SAAS && s == GRD_Base.FlagType.SAASVersion)
                {
                    _flags += 1 << i;
                    chbFlags.SetItemChecked(i, true);
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

            if (!flags)
            {
                label6.Visible =
                    label7.Visible =
                    chbTimeFlags.Visible =
                    dtpTimeFlags.Visible = false;
                this.Height -= 170;

                label4.Anchor =
                    label3.Anchor =
                    label9.Anchor =
                    nudMonths.Anchor =
                    dtpStartDate.Anchor =
                    dtpEndDate.Anchor = AnchorStyles.Bottom;

                label2.Visible =
                    chbFlags.Visible = false;
                this.Height -= 140;               
            }

            _manual = true;
            calcMoney();
        }

        public AddDongleForm(sClient client, sDongle dongle, bool flags)
            : this(client, uint.Parse(dongle.Id), flags)
        {
            _manual = false;
            if(_client.SAAS)
                _from = DateTime.Parse(dongle.EndDate);
            dtpStartDate.Value = DateTime.Parse(dongle.StartDate);
            dtpEndDate.Value = DateTime.Parse(dongle.EndDate);                       
            dtpStartDate.Enabled = false;
            nudMonths.Enabled =
            dtpEndDate.Enabled = true;
            nudFarms.Value = int.Parse(dongle.Farms);
            
            _manual = true;
        }
#endif
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
        public int Flags { get { return _flags; } }
        public DateTime StartDate { get { return dtpStartDate.Value; } }
        public DateTime EndDate { get { return dtpEndDate.Value; } }
        public int TimeFlags { get { return _timeFlags; } }
        public string TimeFlagsEnd { get { return dtpTimeFlags.Value.ToString("yyyy-MM-dd"); } }

        /// <summary>
        /// Событие происходит, а только потом устанавливает галочку
        /// </summary>
        private void ch_ItemCheck(object sender, ItemCheckEventArgs e)
        {
#if PROTECTED
            if (!_manual) return;
            if (1 << e.Index == (int)GRD_Base.FlagType.SAASVersion)
                e.NewValue = _client.SAAS ? CheckState.Checked : CheckState.Unchecked;          
            _manual = false;
            int result = 0;
            (sender as CheckedListBox).SetItemChecked(e.Index, (e.NewValue == CheckState.Checked)); //ставим быстрей галочку
            int cnt = Enum.GetValues(typeof(RabGRD.GRD_Base.FlagType)).Length;
            for (int i = 0; i < cnt; i++)
                result += ((sender as CheckedListBox).GetItemChecked(i)) ? 1 << i : 0;
            _manual = true;
            if (sender == chbFlags)
                _flags = result;
            else if (sender == chbTimeFlags)
                _timeFlags = result;
#endif
        }

        private void calcMoney()
        {
            if(!_manual) return;
            _manual = false;
            if (_client.SAAS)
            {
                DateTime from = _client.SAAS ? _from : dtpStartDate.Value;
                int months = (dtpEndDate.Value.Year - from.Year) * 12;
                months += dtpEndDate.Value.Month - from.Month;
                dtpEndDate.Value = _client.SAAS ? _from.AddMonths(months) : dtpStartDate.Value.AddMonths(months);
                nudMonths.Value = months;
                tbPrice.Text = (months * SAAS_Farm_Cost * Farms).ToString(); //TODO 5- рублей за миниферму в месяц
            }
            else
            {
                tbPrice.Text = (Farms * BOX_Farm_Cost).ToString(); //TODO 100 рублей за миниферму в коробочной версии           
            }

            _manual = true;
        }

        private void nudMonths_ValueChanged(object sender, EventArgs e)
        {
            if (!_manual) return;
            _manual = false;
            dtpEndDate.Value = _client.SAAS ? _from.AddMonths((int)nudMonths.Value) 
                                            : dtpStartDate.Value.AddMonths((int)nudMonths.Value);           
            _manual = true;
            calcMoney();
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
            nudFarms.Value -= (nudFarms.Value % nudFarms.Increment);
            calcMoney();
        }

    }
}
