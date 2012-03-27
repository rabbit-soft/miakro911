using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
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
                //label1.Text = GRDEndUser.Instance.GetKeyID();
                label2.Text = GRDEndUser.Instance.GetFarmsCnt().ToString();
                label3.Text = GRDEndUser.Instance.GetOrganizationName();
                label4.Text = GRDEndUser.Instance.GetDateStart().ToString();
                label5.Text = GRDEndUser.Instance.GetDateEnd().ToString();
                label6.Text = GRDEndUser.Instance.GetFlag(GRDEndUser.FlagType.RabNet).ToString();
                label7.Text = GRDEndUser.Instance.GetFlag(GRDEndUser.FlagType.Genetics).ToString();
                label8.Text = GRDEndUser.Instance.GetFlag(GRDEndUser.FlagType.RabDump).ToString();
                label9.Text = GRDEndUser.Instance.GetFlag(GRDEndUser.FlagType.Butcher).ToString();
                label10.Text = GRDEndUser.Instance.GetFlag(GRDEndUser.FlagType.ReportPlugIns).ToString();
                label22.Text = GRDEndUser.Instance.ValidKey().ToString();
                label24.Text = GRDEndUser.Instance.GetCustomerID().ToString("X");
            } 
            else
            {
                label22.Text = GRDEndUser.Instance.ValidKey().ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string base64data = GRDEndUser.Instance.GetTRUQuestion();
            TextWriter tw = new StreamWriter("question.txt");
            tw.WriteLine("############################################################################");
            tw.WriteLine("##                                                                        ##");
            tw.WriteLine("##     Key update question data                                           ##");
            tw.WriteLine("##                                                                        ##");
            tw.WriteLine("############################################################################");
            tw.WriteLine(base64data);
            tw.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TextReader tr = new StreamReader("answer.txt");
            string st = tr.ReadToEnd();
            Regex test = new Regex(@"^##.*$", RegexOptions.Multiline);
            st = test.Replace(st, string.Empty);            
            tr.Close();
            GRDEndUser.Instance.SetTRUAnswer(st);
        }
    }
}
