using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class OkrolForm : Form
    {
        private RabNetEngRabbit r = null;
        public OkrolForm()
        {
            InitializeComponent();
        }

        public OkrolForm(int r1):this()
        {
            r = Engine.get().getRabbit(r1);
            label1.Text = r.fullName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                r.ProholostIt(dateDays1.DateValue);
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                r.OkrolIt(dateDays1.DateValue, (int)numericUpDown1.Value, (int)numericUpDown2.Value);
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
