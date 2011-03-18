﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Xml;

namespace butcher
{
    public partial class MainForm : Form
    {
        private LoginPanel logPan = new LoginPanel();
        public MainForm()
        {
            InitializeComponent();            
#if RELEASE
            this.WindowState = FormWindowState.Maximized;
#endif         
            logPan.Location = logPan.mustLocation;
            this.Controls.Add(logPan);
            npButcher.AddControl(tbAmount);
            logPan.BringToFront();
            logPan.dSuccessfulLogin = this.enterMain;
            npButcher.dOk = FreshMeat;
            
        }

        public void enterMain()
        {
            FillProducts();
            updateLogs();
        }

        public void FillProducts()
        {
            dataGridView1.Rows.Clear();
            List<sProductType> products = DBproc.GetProducts();
            for (int i = 0; i < products.Count; i++)
            {
                int cellInt =0;
                if (i % 2 != 0) 
                    cellInt = 1;

                if (cellInt == 0)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    dataGridView1.Rows.Add(row);
                }

                if (products[i].Image.Length != 0)
                {
                    Image img = getImage(products[i].Image);
                    dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[cellInt].Value = img;
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[cellInt].Tag = products[i].Id;
                    if (cellInt==0)
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Height = img.Height;
                }
                else
                {
                    var bmp = new Bitmap(300,200);
                    Graphics gr = Graphics.FromImage(bmp);
                    var fnt = new Font("Arial", 20);
                    int pointH = bmp.Height / 2 - fnt.Height;
                    int pointW = (int)((float)bmp.Width - fnt.Size * (float)products[i].Name.Length) / 2;
                    gr.DrawString(products[i].Name, fnt, new SolidBrush(Color.Black),pointW, pointH);
                    
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[cellInt].Value = bmp;
                    if (cellInt == 0)
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Height = bmp.Height;
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[cellInt].Tag = products[i].Id;
                }
            }
            if (dataGridView1.Rows.Count > 4)
                btDown.Enabled = true;
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            logPan.Show();
        }

        private Image getImage(byte[] bts)
        {
            var ms = new MemoryStream(bts);
            var img = Image.FromStream(ms);
            return img;
        }

        private void btDown_Click(object sender, EventArgs e)
        {
            dataGridView1.FirstDisplayedScrollingRowIndex++;
            btUp.Enabled = true;
            if (dataGridView1.FirstDisplayedScrollingRowIndex == dataGridView1.Rows.Count-1-3)
            {
                btDown.Enabled = false;
            }
        }

        private void btUp_Click(object sender, EventArgs e)
        {
            dataGridView1.FirstDisplayedScrollingRowIndex--;
            btDown.Enabled = true;
            if (dataGridView1.FirstDisplayedScrollingRowIndex == 0)
            {
                btUp.Enabled = false;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells[0].Tag == null)
            {
                lbProductName.Text = lbUnit.Text= "";
                dataGridView1.SelectedCells[0].Selected = false;
                tbAmount.Enabled = npButcher.Enabled = false;
                return;
            }
            var prod = DBproc.GetProduct((int)dataGridView1.SelectedCells[0].Tag);
            lbProductName.Text = prod.Name;
            lbUnit.Text = prod.Units;
            tbAmount.Enabled = npButcher.Enabled = true;
        }

        private void FreshMeat()
        {
            DBproc.AddMeat((int)dataGridView1.SelectedCells[0].Tag, float.Parse(tbAmount.Text));
            tbAmount.Clear();
            updateLogs();
            //dataGridView1.SelectedCells[0].Selected = false;
            //tbAmount.Enabled = npButcher.Enabled = false;
            
        }

        private void updateLogs()
        {
            lvLogs.Items.Clear();
            List<sMeat> logs = DBproc.GetMeats();
            foreach (sMeat l in logs)
            {
                ListViewItem lvi = new ListViewItem(l.Date.ToString());
                if (l.Today)
                    lvi.SubItems[0].Font = new Font("Arial", lvLogs.Font.Size, FontStyle.Bold);
                lvi.Tag = l.Id;
                lvi.SubItems.Add(l.ProductType);
                lvi.SubItems.Add(l.Amount.ToString()+" "+l.Units );
                lvi.SubItems.Add(l.User);
                lvLogs.Items.Add(lvi);
            }
        }

        private void tbAmount_TextChanged(object sender, EventArgs e)
        {
            List<char> numbers = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            TextBox tb = (sender as TextBox);
            try
            {
                ulong.Parse(tb.Text);
            }
            catch (FormatException)
            {               
                if (tb.Text != "")
                {
                    for (int i = 0; i < tb.Text.Length; i++)
                    {
                        if (!numbers.Contains(tb.Text[i]))
                        {
                            tb.Text = tb.Text.Remove(i, 1);
                            tb.Select(i, 0);
                            break;
                        }
                    }
                }
            }

            npButcher.OkButtonEnable = (tbAmount.Text != "");
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!logPan.Visible) return;
            if (e.KeyChar == 'd')
            {
                (new FarmListForm()).ShowDialog();
            }
        }

    }


    
}
