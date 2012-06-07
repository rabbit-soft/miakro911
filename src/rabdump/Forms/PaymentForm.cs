using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using pEngine;

namespace rabdump
{
    public partial class PaymentForm : Form
    {
        sPayment[] _pays;

        public PaymentForm(sPayment[] pays)
        {
            InitializeComponent();
            _pays = pays;
            fillData();
        }

        private void fillData()
        {
            listView1.Items.Clear();
            int deb = 0;
            int cred = 0;
            ListViewItem lvi;
            foreach(sPayment p in _pays)
            {
                lvi = listView1.Items.Add(p.Date);
                lvi.SubItems.Add(p.Debet);
                lvi.SubItems.Add(p.Credit);
                lvi.SubItems.Add(p.Comment);
                deb += int.Parse(p.Debet);
                cred += int.Parse(p.Credit);
            }
            lvi = listView1.Items.Add("Итого");
            lvi.SubItems.Add(deb.ToString());
            lvi.SubItems.Add(cred.ToString());
        }
    }
}
