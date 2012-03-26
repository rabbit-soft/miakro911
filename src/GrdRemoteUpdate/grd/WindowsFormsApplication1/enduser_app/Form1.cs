using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RabGRD;

namespace enduser_app
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GRDEndUser.Instance.ValidKey())
            {
                label1.Text = GRDEndUser.Instance.GetKeyID();
                label2.Text = GRDEndUser.Instance.GetFarmsCnt().ToString();
                label3.Text = GRDEndUser.Instance.GetOrgName();
                label4.Text = GRDEndUser.Instance.GetDateStart().ToString();
                label5.Text = GRDEndUser.Instance.GetDateEnd().ToString();
                label6.Text = GRDEndUser.Instance.GetFlag(GRDEndUser.FlagType.RabNet).ToString();
                label7.Text = GRDEndUser.Instance.GetFlag(GRDEndUser.FlagType.Genetics).ToString();
                label8.Text = GRDEndUser.Instance.GetFlag(GRDEndUser.FlagType.RabDump).ToString();
                label9.Text = GRDEndUser.Instance.GetFlag(GRDEndUser.FlagType.Butcher).ToString();
                label10.Text = GRDEndUser.Instance.GetFlag(GRDEndUser.FlagType.ReportPlugIns).ToString();
                label22.Text = GRDEndUser.Instance.ValidKey().ToString();
                label24.Text = GRDEndUser.Instance.GetCustomerID().ToString("X");
            } else
            {
                label22.Text = GRDEndUser.Instance.ValidKey().ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GRDEndUser.Instance.GetTRUQuestion();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GRDEndUser.Instance.SetTRUAnswer();
        }
    }
}
