using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet.forms
{
    public partial class OkrolForm : Form
    {
        private RabNetEngRabbit _rabbit = null;

        public OkrolForm(int r1)           
        {
            InitializeComponent();
            initialHint();
            _rabbit = Engine.get().getRabbit(r1);
            label1.Text = _rabbit.FullName;
            TimeSpan days = DateTime.Now.Subtract(Engine.db().getFucks(_rabbit.ID).LastFuck.When);
            dateDays1.Maximum = days.Days;
        }

        private void initialHint()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(dateDays1, "Дата принятия окрола");
            toolTip.SetToolTip(button1, "Отметить принятие окрола");
            toolTip.SetToolTip(button2, "Отметить прохолостание крольчихи");
            toolTip.SetToolTip(button3, "Отменить принятие окрола. Закрыть окно");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                _rabbit.ProholostIt(dateDays1.DaysValue);
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
                _rabbit.OkrolIt(dateDays1.DaysValue, (int)numericUpDown1.Value, (int)numericUpDown2.Value);
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
