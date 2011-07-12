using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using X_Tools;
using CAS;

namespace rabnet
{
    public partial class PLUForm : Form
    {
        public PLUForm()
        {
            InitializeComponent();
        }

        public PLUForm(CasLP16.PLU plu, int[] messageIDs):this()
        {
            mIdsFill(messageIDs);
            tbID.Text = plu.ID.ToString();
            tbCode.Text = plu.Code.ToString();
            tbGroupcode.Text = plu.GroupCode.ToString();
            tbName1.Text = plu.ProductName1;
            tbName2.Text = plu.ProductName2;
            tbPrice.Text = plu.Price.ToString();
            tbTara.Text = plu.TaraWeight.ToString();
        }

        private void mIdsFill(int[] messageIDs)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("");
            for (int i = 0; i < messageIDs.Length; i++)
            {
                comboBox1.Items.Add(messageIDs[i].ToString());
            }
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            XTools.checkIntNumber(sender, e);
        }
    }
}
