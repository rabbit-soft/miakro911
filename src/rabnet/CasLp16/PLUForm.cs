using System;
using System.Windows.Forms;

namespace CAS
{
    internal partial class PLUForm : Form
    {
        private CasLP16.PLU _plu;
        private int[] _IDs;
        private bool _manual = true;

        public PLUForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Добавление новой записи
        /// </summary>
        /// <param name="plu"></param>
        public PLUForm(CasLP16.PLU plu,int[] messageIDs,int[]existIDs): this()
        {
            _plu = plu;
            tbID.Text = plu.ID.ToString();
            mIdsFill(messageIDs);
            _IDs = existIDs;
        }

        /// <summary>
        /// Изменение имеющейся
        /// </summary>
        public PLUForm(CasLP16.PLU plu, int[] messageIDs):this()
        {
            _plu = plu;
            mIdsFill(messageIDs);
            _manual = false;
            tbID.Text = plu.ID.ToString();
            tbCode.Text = plu.Code.ToString();
            tbGroupcode.Text = plu.GroupCode.ToString();
            tbName1.Text = plu.ProductName1;
            tbName2.Text = plu.ProductName2;
            tbLiveTime.Text = plu.LiveTime.ToString();
            tbPrice.Text = plu.Price.ToString();
            tbTara.Text = plu.TaraWeight.ToString();
            tbID.Enabled = false;
            _manual = true;
        }

        private void mIdsFill(int[] messageIDs)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("0");
            int ind = 0;
            for (int i = 0; i < messageIDs.Length; i++)
            {
                comboBox1.Items.Add(messageIDs[i].ToString());
                if(_plu!= null)                
                    if (messageIDs[i] == _plu.MessageID)
                        ind = i+1;
            }
            comboBox1.SelectedIndex = ind;
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            if (!(sender is TextBox)) return;
            if (!_manual) return;
            _manual = false;
            TextBox tb = sender as TextBox;
            string res = tb.Text;
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] < '0' || res[i] > '9')
                    res.Remove(i, 1);
            }
            if(tb.Text.Length != res.Length)
                tb.Text = res;
            _manual = true;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (tbID.Text == "" || !idIsValid(tbID.Text))
            {
                MessageBox.Show("Неверный номер товара");
                this.DialogResult = DialogResult.None;
                return;
            }
            _plu.ID = int.Parse(tbID.Text);
            _plu.Code = int.Parse(tbCode.Text);
            _plu.GroupCode = int.Parse(tbGroupcode.Text == "" ? "0" : tbGroupcode.Text);
            _plu.ProductName1 = tbName1.Text;
            _plu.ProductName2 = tbName2.Text;
            _plu.LiveTime = int.Parse(tbLiveTime.Text == "" ? "0" : tbLiveTime.Text);
            _plu.Price = int.Parse(tbPrice.Text == "" ? "0" : tbPrice.Text);
            _plu.TaraWeight = int.Parse(tbTara.Text == "" ? "0" : tbTara.Text);
            _plu.MessageID = int.Parse(comboBox1.Text);
        }

        private bool idIsValid(string id)
        {
            if (_IDs==null) return true;
            try
            {
                int newID =int.Parse(id);
                foreach (int oldID in _IDs)
                {
                    if (oldID == newID) return false;
                }
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


    }
}
