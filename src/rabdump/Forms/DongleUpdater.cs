using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RabGRD;
using pEngine;

namespace rabdump
{
    partial class DongleUpdater : Form
    {
#if PROTECTED
        private GRD grd = GRD.Instance;

        private RabReqSender _reqSend;
        private sClient _client;

        public DongleUpdater(RabReqSender reqSend,sClient client)
        {            
            InitializeComponent();
            _reqSend = reqSend;
            _client = client;
            updateData();
        }

        private void updateData()
        {
            tbOrgName.Text = grd.GetOrganizationName();
            tbFarms.Text = grd.GetFarmsCnt().ToString();
            tbStartDate.Text = grd.GetDateStart().ToString("yyyy-MM-dd");
            tbEndDate.Text = grd.GetDateEnd().ToString("yyyy-MM-dd");
            tbMoney.Text = _client.Money;
            fillFlags();
        }

        private void fillFlags()
        {
            foreach (GRD_Base.FlagType f in Enum.GetValues(typeof(GRD_Base.FlagType)))
            {
                if (grd.GetFlag(f))
                    lbFlags.Items.Add(getRusName(f));
            }
        }

        private string getRusName(GRD_Base.FlagType f)
        {
            switch (f)
            {
                case GRD_Base.FlagType.RabNet: return "Программа Миакро 9.11";
                case GRD_Base.FlagType.Butcher: return "Стерильный цех";
                case GRD_Base.FlagType.Genetics: return "Генетика";
                case GRD_Base.FlagType.RabDump: return "Программа администрирования";
                case GRD_Base.FlagType.ReportPlugIns: return "Отчеты на заказ";
                case GRD_Base.FlagType.SAASVersion: return "SAAS версия поддержки";
                case GRD_Base.FlagType.ServerDump: return "Резервное копирование на удаленный сервер";
                case GRD_Base.FlagType.WebReports: return "Веб-отчеты";
                default: return "";
            }
        }
#endif

        private void btUpdate_Click(object sender, EventArgs e)
        {
#if PROTECTED
            this.Enabled = false;
            foreach(sDongle d in _client.Dongles)
                if (uint.Parse(d.Id) == grd.ID)
                {
                    ResponceItem resp = _reqSend.ExecuteMethod(MethodName.GetCosts);
                    AdminGRD.AddDongleForm dlg = new AdminGRD.AddDongleForm(_client, d,false);
                    dlg.BOX_Farm_Cost = int.Parse((resp.Value as string[])[0]);
                    dlg.SAAS_Farm_Cost = int.Parse((resp.Value as string[])[1]);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        string q;
                        try
                        {
                            int retCode = grd.GetTRUQuestion(out q);

                            resp = _reqSend.ExecuteMethod(MethodName.VendorUpdateDongle, //MName.VendorSheduleDongle,
                                MPN.base64_question, q,
                                MPN.clientId, _client.Id,
                                MPN.farms, dlg.Farms.ToString(),
                                MPN.flags, dlg.Flags.ToString(),
                                MPN.startDate, d.StartDate,
                                MPN.endDate, dlg.EndDate.ToString("yyyy-MM-dd"),
                                MPN.dongleId, grd.ID.ToString());
                            if (resp.Value == null)
                                throw new Exception("Пустое число вопрос");
                            retCode = grd.SetTRUAnswer(resp.Value as string);
                            if (retCode != 0)
                                throw new Exception("Ошибка обновления ключа");
                            _reqSend.ExecuteMethod(MethodName.SuccessUpdate,
                                MPN.dongleId, grd.ID.ToString());
                            MessageBox.Show("Прошивка прошла успешно");
                            this.Close();
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message);
                        }
                    }
                    break;
                }
            this.Enabled = true;
#endif
        }

        private void btPayments_Click(object sender, EventArgs e)
        {
#if PROTECTED
            ResponceItem s = _reqSend.ExecuteMethod(MethodName.GetPayments, MPN.clientId, grd.GetClientID().ToString());
            (new PaymentForm(s.Value as sPayment[])).ShowDialog();
#endif
        }

    }
}
