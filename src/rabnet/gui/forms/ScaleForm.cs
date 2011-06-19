using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using CAS;
namespace rabnet
{
    public partial class ScaleForm : Form
    {
        private bool _manual = true;
        private Thread _loader;
        private static int _lastAnswer = 0;

        public ScaleForm()
        {
            InitializeComponent();
        }
        ~ScaleForm()
        {
            CasLP16.Instance.Loading = false;
            CasLP16.Instance.Disconnect();
        }
        private void tbRefresh_Click(object sender, EventArgs e)
        {
            _loader = new Thread(loadfromscale);
            _loader.IsBackground = true;
            _loader.Start();
            toolStripProgressBar1.Visible = true;
            tLoadFromScaleChecker.Start();
            /*toolStripProgressBar1.Visible = true;
            tScaleMessageClear_Tick(null, null);
            int ans = CasLP16.Instance.Connect();
            //RabnetConfig          
            ans = CasLP16.Instance.LoadPLUs(0, 20);
            if (ans != CasLP16.ReturnCode.SUCCESS)
            {
                tslbScaleMessage.Text = CasLP16.ReturnCode.getDescription(ans);
                tScaleMessageClear.Start();
                toolStripProgressBar1.Visible = false;
                return;
            }
            ans = CasLP16.Instance.LoadMSGs(1, 20);
            fillLists();
            ans = CasLP16.Instance.Disconnect();
            toolStripProgressBar1.Visible = false;*/
        }

        private static void loadfromscale()
        {
            if(!CasLP16.Instance.Connected)
                _lastAnswer = CasLP16.Instance.Connect();
            _lastAnswer = CasLP16.Instance.LoadPLUs(0, 20);
            if (_lastAnswer != CasLP16.ReturnCode.SUCCESS) return;
            _lastAnswer = CasLP16.Instance.LoadMSGs(1, 20);
            if (_lastAnswer != CasLP16.ReturnCode.SUCCESS) return;
        }

        private void ScaleForm_Load(object sender, EventArgs e)
        {
            tbRefresh.PerformClick();
        }

        private void fillLists()
        {
            listView1.Items.Clear();
            foreach (int id in CasLP16.Instance.getIDsOfPLUs())
            {
                CasLP16.PLU plu = CasLP16.Instance.GetPLUbyID(id);
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
            lvMSG.Items.Clear();
            foreach (int id in CasLP16.Instance.getIDsOfMSGs())
            {
                CasLP16.MSG msg = CasLP16.Instance.GetMSGbyID(id);
                ListViewItem lvi = lvMSG.Items.Add(msg.ID.ToString());
                lvi.SubItems.Add(msg.Text);
                lvi.Tag = msg;
            }
            Application.DoEvents();
            saveSummarys();
        }

        private void saveSummarys()
        {
            foreach (int id in CasLP16.Instance.getIDsOfPLUs())
            {
                if (id == 0) continue;
                CasLP16.PLU plu = CasLP16.Instance.GetPLUbyID(id);
                Engine.db().addPLUSummary(plu.ID, plu.ProductName1, plu.TotalSell, plu.TotalSumm, plu.TotalWeight, plu.LastClear);
            }
        }

        private void lvMSG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMSG.SelectedItems.Count != 0)
            {
                textBox1.Enabled = true;
                textBox1.Text = lvMSG.SelectedItems[0].SubItems[1].Text;
            }
            else
            {
                textBox1.Clear();
                textBox1.Enabled = false;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            (lvMSG.SelectedItems[0].Tag as CasLP16.MSG).Text = textBox1.Text;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
                (new PLUForm((listView1.SelectedItems[0].Tag as CasLP16.PLU), CasLP16.Instance.getIDsOfMSGs())).ShowDialog();
            else
            {
                (new PLUForm()).ShowDialog();
            }
        }

        private void tScaleMessageClear_Tick(object sender, EventArgs e)
        {
            tslbScaleMessage.Text = "";
            tScaleMessageClear.Stop();    
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!_manual) return;
            if (textBox1.Text.Length < 50) return;
            else if (textBox1.Text.Length > 400) textBox1.Text = textBox1.Text.Substring(0, 400);
            _manual = false;
            List<string> result = new List<string>();
            string[] lines = textBox1.Lines;
            for (int i = 0; i < lines.Length; i++)
            {
                if(lines[i].Length>50)
                {
                    result.Add(lines[i].Substring(0,50));
                    result.Add(lines[i].Substring(50,lines[i].Length-50));
                }
                else result.Add(lines[i]);
            }
            textBox1.Clear();
            for (int i = 0; i < result.Count; i++)
                textBox1.Text+= result[i]+Environment.NewLine;
            _manual = true;
        }

        private void tLoadFromScaleChecker_Tick(object sender, EventArgs e)
        {
            if (!_loader.IsAlive)
            {
                toolStripProgressBar1.Visible = false;
                tLoadFromScaleChecker.Stop();
                if (_lastAnswer != 0)
                {
                    tslbScaleMessage.Text = CasLP16.ReturnCode.getDescription(_lastAnswer);
                    tScaleMessageClear.Start();
                    return;
                }
                fillLists();
            }
        }

        private void cmPLU_Opening(object sender, CancelEventArgs e)
        {
            miPLUChange.Visible = (listView1.SelectedItems.Count != 0);
        }
    }
}
