using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using log4net;

namespace CAS
{
    public delegate void AddPLUSummaryHandler(int pluID,string pluPN1,int pluTSell,int TSumm,int TWeight,DateTime LastClear);

    public partial class ScaleForm : Form
    {
        class sumSave
        {
            internal int id;
            internal int sell;
            internal int summ;
            internal int weight;

            internal sumSave(int id, int sell, int summ, int weight)
            {
                this.id = id;
                this.sell = sell;
                this.summ = summ;
                this.weight = weight;
            }
        }

        private class sumSaved : List<sumSave>
        {
            internal bool needSend(int id, int sell, int summ, int weight)
            {
                sumSave newSS = new sumSave(id, sell,summ,weight);
                for (int i = 0; i < this.Count;i++ )
                {
                    if (this[i].id == id)
                    {
                        if (this[i].sell != sell || this[i].summ != summ || this[i].weight != weight)
                        {
                            this[i].sell = sell;
                            this[i].summ = summ;
                            this[i].weight = weight;
                            return true;
                        }
                        else return false;
                    }
                }
                this.Add(newSS);
                return true;
            }
        }

        private static ILog _logger = LogManager.GetLogger(typeof(ScaleForm));

        private static sumSaved sumSaveded = new sumSaved();
        private bool _manual = true;
        private Thread _loader;
        private static int _lastAnswer = 0;
        /// <summary>
        /// Сканировать ли на общие итоги
        /// </summary>
        private static bool _scanSmrs = false;
        private static bool _contSS = false;
        private static bool _scaleFormActive = false;

        public ScaleForm()
        {
            InitializeComponent();
        }
        ~ScaleForm()
        {
            CasLP16.Instance.Loading = false;
            CasLP16.Instance.Disconnect();
        }
        #region static

        /// <summary>
        /// Имеется запись для сохранени
        /// </summary>
        public static event AddPLUSummaryHandler SummarySaving;

        /// <summary>
        /// Запускает поток, проверяющий внесение новых данных
        /// </summary>
        public static void StartMonitoring()
        {
            if (!ScaleOpt.GetBoolOpt(ScaleOpt.OptType.Monitoring)) return;
            _scaleFormActive = false;
            _scanSmrs = true;
            Thread scaner = new Thread(listener);
            scaner.IsBackground = true;
            scaner.Start();
        }
        /// <summary>
        /// Останавливает поток,проверяющий внесение новых данных
        /// </summary>
        public static void StopMonitoring(bool disconnect)
        {
            if (!disconnect) _scaleFormActive = true;
            _scanSmrs = false;
            Thread.Sleep(1000);
        }
        public static void StopMonitoring()
        {
            StopMonitoring(true);
        }

        public static void listener()
        {
            while (_scanSmrs)
            {
                if (!CasLP16.Instance.Loading)
                {
                    loadfromscale(false);
                    saveSummarys();
                }
                Thread.Sleep(ScaleOpt.GetIntOpt(ScaleOpt.OptType.ScanDelay) * 1000);
            }
            if(!_scaleFormActive)CasLP16.Instance.Disconnect();
        }

        /// <summary>
        /// Отдельный поток.Загружает данные с весов
        /// Также устанавливает соединение если нет такового
        /// <param name="loadMsg">Загружать ли сообщения</param>
        /// </summary>
        private static void loadfromscale(bool loadMsg)
        {
            _logger.DebugFormat("loadFromScale");
            //Если с весами работает другой поток(StartListen), то надо подождать.
            while (CasLP16.Instance.Loading)
                Thread.Sleep(1000);
            if (!CasLP16.Instance.Connected)
            {
                CasLP16.Instance.SetScaleAddress(ScaleOpt.GetStrOpt(ScaleOpt.OptType.ScaleAddres), ScaleOpt.GetIntOpt(ScaleOpt.OptType.ScalePort));
                _lastAnswer = CasLP16.Instance.Connect();
                _logger.DebugFormat("Connection: {0:d}",_lastAnswer);
                if (_lastAnswer != ReturnCode.SUCCESS) return;
            }
            _lastAnswer = CasLP16.Instance.LoadPLUs(ScaleOpt.GetIntOpt(ScaleOpt.OptType.ScanPLUFrom), ScaleOpt.GetIntOpt(ScaleOpt.OptType.ScanPLUUntil));
             _logger.DebugFormat("loadPLU: {0:d}",_lastAnswer);
            if (_lastAnswer != ReturnCode.SUCCESS) return;
            if (loadMsg)
            {
                _lastAnswer = CasLP16.Instance.LoadMSGs(ScaleOpt.GetIntOpt(ScaleOpt.OptType.ScanMSGFrom), ScaleOpt.GetIntOpt(ScaleOpt.OptType.ScanMSGUntil));
                _logger.DebugFormat("loadMSG: {0:d}",_lastAnswer);
                if (_lastAnswer != ReturnCode.SUCCESS) return;
            }
        }
        private static void loadfromscale()
        {
            loadfromscale(true);
        }

        private static void saveSummarys()
        {
            if (SummarySaving == null) return;
            foreach (int id in CasLP16.Instance.getIDsOfPLUs())
            {
                if (id == 0) continue;
                PLU plu = CasLP16.Instance.GetPLUbyID(id);
                _logger.DebugFormat("plu: {0:d},clear: {1:yyyy-MM-dd hh:mm:ss}", plu.ID, plu.LastClear);
                if (plu.TotalSell == 0 && plu.TotalSumm == 0 && plu.TotalWeight == 0)continue; 
                if (!sumSaveded.needSend(plu.ID,plu.TotalSell, plu.TotalSumm, plu.TotalWeight)) continue;
                SummarySaving(plu.ID, plu.ProductName1, plu.TotalSell, plu.TotalSumm, plu.TotalWeight, plu.LastClear);
            }
        }

        #endregion

        /// <summary>
        /// Запускает обновление данных с весов
        /// </summary>
        private void RefreshData(object sender, EventArgs e)
        {
            //CasLP16.Instance.Disconnect();
            _loader = new Thread(loadfromscale);
            _loader.IsBackground = true;
            _loader.Start();
            toolStripProgressBar1.Visible = true;
            tabControl1.Enabled = false;
            tLoadFromScaleChecker.Start();
        }

        /// <summary>
        /// Таймер проверяет,загрузились ли данные из весов (Жив ли обрабатывающий поток).
        /// </summary>
        private void tLoadFromScaleChecker_Tick(object sender, EventArgs e)
        {
            if (!_loader.IsAlive)
            {
                toolStripProgressBar1.Visible = false;
                tabControl1.Enabled = true;
                tLoadFromScaleChecker.Stop();
                if (_lastAnswer != 0)
                {
                    writeMessage(ReturnCode.getDescription(_lastAnswer));
                    return;
                }
                updateLists();
            }
        }

        /// <summary>
        /// Обновить списки
        /// </summary>
        private void updateLists()
        {
            
            listView1.Items.Clear();
            foreach (int id in CasLP16.Instance.getIDsOfPLUs())
            {
                PLU plu = CasLP16.Instance.GetPLUbyID(id);
                ListViewItem lvi = listView1.Items.Add(plu.ID.ToString());
                lvi.SubItems.Add(plu.Code.ToString());
                lvi.SubItems.Add(plu.ProductName1);
                lvi.SubItems.Add(plu.ProductName2.ToString());
                lvi.SubItems.Add(plu.Price.ToString());
                lvi.SubItems.Add(plu.LiveTime.ToString());
                lvi.SubItems.Add(plu.TaraWeight.ToString());
                lvi.SubItems.Add(plu.GroupCode.ToString());
                lvi.SubItems.Add(plu.MessageID.ToString());
                lvi.Tag = plu;
            }
            ///Обновляем сообщения
            lvMSG.Items.Clear();
            foreach (int id in CasLP16.Instance.getIDsOfMSGs())
            {
                MSG msg = CasLP16.Instance.GetMSGbyID(id);
                ListViewItem lvi = lvMSG.Items.Add(msg.ID.ToString());
                lvi.SubItems.Add(msg.Text);
                lvi.Tag = msg;
            }
            tbMessage.Text = "";
            tbMessage.Enabled = false;
            Application.DoEvents();
            if(!ScaleOpt.GetBoolOpt(ScaleOpt.OptType.Monitoring))
                saveSummarys();
        }

        private void lvMSG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMSG.SelectedItems.Count != 0)
            {
                tbMessage.Enabled = true;
                tbMessage.Text = lvMSG.SelectedItems[0].SubItems[1].Text;
            }
            else
            {
                tbMessage.Clear();
                tbMessage.Enabled = false;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {       
            if (listView1.SelectedItems.Count > 0)
                (new PLUForm((listView1.SelectedItems[0].Tag as PLU), CasLP16.Instance.getIDsOfMSGs())).ShowDialog();
            else
            {
                PLU newPlu = new PLU();
                newPlu.ID = (listView1.Items[listView1.Items.Count - 1].Tag as PLU).ID + 1;
                if ((new PLUForm(newPlu,CasLP16.Instance.getIDsOfMSGs(), CasLP16.Instance.getIDsOfPLUs())).ShowDialog() == DialogResult.OK)
                {
                    CasLP16.Instance.AddPLU(newPlu);
                }
            }
            updateLists();
        }

        private void tScaleMessageClear_Tick(object sender, EventArgs e)
        {
            tslbScaleMessage.Text = "";
            tScaleMessageClear.Stop();    
        }

        private void tbMessage_TextChanged(object sender, EventArgs e)
        {
            if (!_manual) return;
            string message = tbMessage.Text.TrimEnd(new char[] { '\r', '\n' });
            if (message.Length <= 50) return;
            else if (message.Length > 400)
            {
                message = message.Substring(0, 400);
            }
        }

        private void cmPLU_Opening(object sender, CancelEventArgs e)
        {
            miPLUChange.Visible = (listView1.SelectedItems.Count != 0);
        }

        private void tbMessage_Leave(object sender, EventArgs e)
        {
            try
            {
                if (lvMSG.SelectedItems.Count == 0) return;
                string s = tbMessage.Text.TrimEnd(new char[] { '\r', '\n' });
                (lvMSG.SelectedItems[0].Tag as MSG).Text = s;
                tbMessage.Clear();
                tbMessage.Enabled = false;
                updateLists();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка.");
                _logger.Debug("tbMessage_Leave", ex);
            }
        }

        private void pluModification(object sender, EventArgs e)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            if (mi != miPLUdel)
                listView1_DoubleClick(sender, e);
            else
            {
                (listView1.SelectedItems[0].Tag as PLU).Delete = true;
                listView1.Items.Remove(listView1.SelectedItems[0]);            
            }

        }

        #region options
        private void btOptionsSave_Click(object sender, EventArgs e)
        {
            ScaleOpt.SaveStrOpt(ScaleOpt.OptType.ScaleAddres, tbAddress.Text);
            ScaleOpt.SaveStrOpt(ScaleOpt.OptType.ScalePort, tbPort.Text);
            ScaleOpt.SaveStrOpt(ScaleOpt.OptType.ScanPLUFrom, tbSF.Text);
            ScaleOpt.SaveStrOpt(ScaleOpt.OptType.ScanPLUUntil, tbSU.Text);
            ScaleOpt.SaveIntOpt(ScaleOpt.OptType.ScanDelay, (int)nudScanFreq.Value);
            ScaleOpt.SaveStrOpt(ScaleOpt.OptType.ScanMSGFrom, tbMSGsf.Text);
            ScaleOpt.SaveStrOpt(ScaleOpt.OptType.ScanMSGUntil, tbMSGsu.Text);
            ScaleOpt.SaveBoolOpt(ScaleOpt.OptType.Monitoring, chMonitor.Checked);
            if (chMonitor.Checked)
                _contSS = true;
            tabControl1.SelectedTab = tabPage1;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab == tpOptions)
            {
                tbAddress.Text = ScaleOpt.GetStrOpt(ScaleOpt.OptType.ScaleAddres);
                tbPort.Text = ScaleOpt.GetStrOpt(ScaleOpt.OptType.ScalePort);
                tbSF.Text = ScaleOpt.GetStrOpt(ScaleOpt.OptType.ScanPLUFrom);
                tbSU.Text = ScaleOpt.GetStrOpt(ScaleOpt.OptType.ScanPLUUntil);
                nudScanFreq.Value = ScaleOpt.GetIntOpt(ScaleOpt.OptType.ScanDelay);
                tbMSGsf.Text = ScaleOpt.GetStrOpt(ScaleOpt.OptType.ScanMSGFrom);
                tbMSGsu.Text = ScaleOpt.GetStrOpt(ScaleOpt.OptType.ScanMSGUntil);
                chMonitor.Checked = nudScanFreq.Enabled = ScaleOpt.GetBoolOpt(ScaleOpt.OptType.Monitoring);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tbMSGsf.Text = "1";
            tbMSGsu.Text = "1000";
        }

        private void btAllPLUn_Click(object sender, EventArgs e)
        {
            tbSF.Text = "0";
            tbSU.Text = "5000";
        }

        /// <summary>
        /// Выполняется проверка на удовлетворительность номера.
        /// </summary>
        private void checkOptions(object sender, EventArgs e)
        {
            btOptionsSave.Enabled = true;
            bool error = false;
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                tb.BackColor = Color.White;

                if (tb == tbAddress)
                {
                    string[] octets = tbAddress.Text.Split('.');
                    if (octets.Length < 4)
                    {
                        error = true;
                    }
                    foreach (string s in octets)
                    {
                        try
                        {
                            int.Parse(s);
                        }
                        catch (FormatException)
                        {
                            error = true;
                        }
                    }
                }
                else if (tb == tbPort)
                {
                    try
                    {
                        int.Parse(tbPort.Text);
                    }
                    catch (FormatException)
                    {
                        error = true;
                    }
                }
                else if (tb == tbSF || tb == tbSU)
                {
                    try
                    {
                        int i = int.Parse(tb.Text);
                        if (i < 1) tb.Text = "1";
                        if (i > 5000) tb.Text = "5000";
                    }
                    catch (FormatException)
                    {
                        error = true;
                    }
                }
                else if (tb == tbMSGsf || tb == tbMSGsu)
                {
                    try
                    {
                        int i = int.Parse(tb.Text);
                        if (i < 1) tb.Text = "1";
                        if (i > 1000) tb.Text = "1000";
                    }
                    catch (FormatException)
                    {
                        error = true;
                    }
                }
                if (error)
                {
                    btOptionsSave.Enabled = false;
                    tb.BackColor = Color.Crimson;
                }
            }

        }

        private void chMonitor_CheckedChanged(object sender, EventArgs e)
        {
            nudScanFreq.Enabled = chMonitor.Checked;
        }
        #endregion 

        private void btTest_Click(object sender, EventArgs e)
        {
            CasLP16.Instance.ClearSumarys();
            Summary sm = CasLP16.Instance.GetSummary();
            FactoryConfig fc = CasLP16.Instance.GetFactoryConfig();
            State st = CasLP16.Instance.GetState();
        }

        private void ScaleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CasLP16.Instance.Disconnect();
            _scaleFormActive = false;
            if (_contSS)
                StartMonitoring();
        }

        private void ScaleForm_Load(object sender, EventArgs e)
        {
            _scaleFormActive = true;
            _contSS = _scanSmrs;
            if (_scanSmrs)
            {
                StopMonitoring();
                //Thread.Sleep(1000);
            }
            RefreshData(sender,e);
        }

        private void tbSave_Click(object sender, EventArgs e)
        {
            CasLP16.Instance.SavePLUs();
            CasLP16.Instance.SaveMSGs();
            writeMessage("Сохранено");
        }

        private void writeMessage(string text)
        {
            tslbScaleMessage.Text = text;
            tScaleMessageClear.Start();
        }

        private void cmMSG_Opening(object sender, CancelEventArgs e)
        {
            miMsgAdd.Visible = lvMSG.SelectedItems.Count == 0;                   
        }

        private void miMsgAdd_Click(object sender, EventArgs e)
        {
            const string newTXT = "Текст сообщения";
            try
            {
                int newID = int.Parse(lvMSG.Items[lvMSG.Items.Count - 1].SubItems[0].Text) + 1;
                ListViewItem lvi = lvMSG.Items.Add(newID.ToString());
                lvi.SubItems.Add(newTXT);
                CasLP16.Instance.AddMSG(new MSG(newID, newTXT));
                lvi.Tag = CasLP16.Instance.GetMSGbyID(newID);
            }
            catch(Exception ex)
            {
                _logger.Error("miMsgAdd",ex);
            }
        }
    }
}
