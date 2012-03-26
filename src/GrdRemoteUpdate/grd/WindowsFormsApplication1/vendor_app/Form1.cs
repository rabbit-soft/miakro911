using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using RabGRD;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        private List<KeyInfo> _keys=new List<KeyInfo>();
        private GRDVendorKey key;

        private string truKeyStr = "главрыба";
        private byte[] truKey;


        public Form1()
        {
            InitializeComponent();
            button8.Enabled = false;
            button9.Enabled = false;
            groupBox1.Enabled = false;

            truKey = Encoding.GetEncoding(1251).GetBytes(truKeyStr);


        }


        private const int grdMarkerOffset = 0;
        private const int grdMarkerLength = 32;
        private const int grdNameOffset = 32;
        private const int grdNameLength = 100;



        private void button7_Click(object sender, EventArgs e)
        {
            _keys.Clear();

            _keys=new GRDEnumerator().KeyList;

//            GRDVendor.Instance.ListKeys(out _keys);

            listBox1.Items.Clear();

            if (_keys.Count==0)
            {
                return;
            }

            for (int i=0; i<_keys.Count; i++)
            {
                listBox1.Items.Add(_keys[i].ID.ToString("X") + "h ");
//                listBox1.Items.Add(_keys[i].ID.ToString("X") + "h " + key.GrdModelName(_keys[i].Model));
            }
            listBox1.SelectedIndex = 0;
            button8.Enabled = true;



        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
//            GRDVendor.Instance.Connect(_keys[listBox1.SelectedIndex].ID);
            key = new GRDVendorKey(_keys[listBox1.SelectedIndex].ID, truKey);
            this.Enabled = true;
            CheckActive();

//            label8.Text = key.UAMOffset.ToString()+" - "+key.ProgID.ToString();
            label8.Text = key.ReadStringCp1251(key.UAMOffset + grdMarkerOffset, grdMarkerLength);
            textBox6.Text = "";

        }

        private void CheckActive()
        {
//            if (GRDVendor.Instance.Active)

            bool act;
            try
            {
                act = key.Active;
            } catch
            {
                act = false;
            }

            if (act)
            {
                listBox1.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = true;
                groupBox1.Enabled = true;
            }else
            {
                listBox1.Enabled = true;
                button7.Enabled = true;
                button8.Enabled = true;
                button9.Enabled = false;
                groupBox1.Enabled = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            key.Dispose();
            CheckActive();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox6.Text = key.ReadStringCp1251(key.UAMOffset + grdNameOffset, grdNameLength);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            key.WriteToUserBuf(textBox6.Text, grdMarkerOffset, grdMarkerLength);
            key.WriteMask();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string st = key.get_question();

            TextWriter tw = new StreamWriter("question.txt");
            tw.WriteLine("############################################################################");
            tw.WriteLine("##                                                                        ##");
            tw.WriteLine("##     Key update question data                                           ##");
            tw.WriteLine("##                                                                        ##");
            tw.WriteLine("############################################################################");
            tw.WriteLine(st);
            tw.Close();
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            key.WriteToUserBuf(textBox6.Text, grdMarkerOffset, grdMarkerLength);

            TextReader tr = new StreamReader("question.txt");

            string st = tr.ReadToEnd();

            Regex test = new Regex(@"^##.*$", RegexOptions.Multiline);
            st = test.Replace(st, string.Empty).Trim();
            MessageBox.Show(st);
            tr.Close();

            st = key.GetAnswer(st);

            MessageBox.Show(st);
            TextWriter tw = new StreamWriter("answer.txt");

            tw.WriteLine("############################################################################");
            tw.WriteLine("##                                                                        ##");
            tw.WriteLine("##     Key update answer data                                             ##");
            tw.WriteLine("##                                                                        ##");
            tw.WriteLine("############################################################################");
            tw.WriteLine(st);
            tw.Close();

        }

        private void button14_Click(object sender, EventArgs e)
        {
            TextReader tr = new StreamReader("answer.txt");
            string st = tr.ReadToEnd();
            Regex test = new Regex(@"^##.*$", RegexOptions.Multiline);
            st = test.Replace(st, string.Empty);
            tr.Close();
            key.SetTRUAnswer(st);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            key.WriteToUserBuf(new DateTime(2011, 10, 5), 100);
        }



    }
}