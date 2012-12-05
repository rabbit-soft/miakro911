using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet.forms
{
    public partial class Proholost : Form
    {
        private RabNetEngRabbit r1 = null;
        public Proholost()
        {
            InitializeComponent();
            initialHints();
        }

        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(button1,"Отметить прохолостание крольчихи");
            toolTip.SetToolTip(button2, "Закрыть окно");
        }

        public Proholost(int r):this()
        {
            r1 = Engine.get().getRabbit(r);
            label1.Text = r1.FullName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                r1.ProholostIt(dateDays1.DaysValue);
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
