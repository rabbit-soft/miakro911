using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet.components
{
    public partial class DateDays : UserControl
    {
        private bool _manual = true;

        public enum DDPosition{ALL_UD,LABELS_LR,ALL_LR}

        public DateDays()
        {
            InitializeComponent();
            dateTimePicker1.MaxDate = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now.Date;
        }

        private DDPosition pos=DDPosition.LABELS_LR;
        public DDPosition position{
            get { return pos; }
            set
            {
                pos = value;
                reposition();
            }
        }

        private void reposition()
        {
            Rectangle r1 = new Rectangle(0, 0, label1.Width, label1.Height);
            Rectangle r2 = new Rectangle(0, 0, dateTimePicker1.Width, dateTimePicker1.Height);
            Rectangle r3 = new Rectangle(0, 0, label2.Width, label2.Height);
            Rectangle r4 = new Rectangle(0, 0, numericUpDown1.Width, numericUpDown1.Height);
            switch (pos)
            {
                case DDPosition.ALL_LR:
                    r2.Offset(10 + r1.Right, 0);
                    r3.Offset(10 + r2.Right, 0);
                    r4.Offset(10 + r3.Right, 0);
                    break;
                case DDPosition.ALL_UD:
                    r2.Offset(0, 10+r1.Bottom);
                    r3.Offset(0, 10 + r2.Bottom);
                    r4.Offset(0, 10 + r3.Bottom);
                    break;
                case DDPosition.LABELS_LR:
                    if (r1.Width > r3.Width)
                        r3.Width = r1.Width;
                    if (r1.Width<r3.Width)
                        r1.Width = r3.Width;
                    r2.Offset(10+r1.Right, 0);
                    r3.Offset(0, 10 + r1.Bottom);
                    r4.Offset(10+r3.Right, r3.Top);
                    break;
            }
            label1.Top = r1.Top; label1.Left = r1.Left;
            dateTimePicker1.Left = r2.Left; dateTimePicker1.Top = r2.Top;
            label2.Top = r3.Top; label2.Left = r3.Left;
            numericUpDown1.Top = r4.Top; numericUpDown1.Left = r4.Left;

        }

        public String DateText
        {
            get { return label1.Text; }
            set { label1.Text = value; reposition(); }
        }
        public String DaysText
        {
            get { return label2.Text; }
            set { label2.Text = value; reposition(); }
        }
        public DateTime DateValue
        {
            get { return dateTimePicker1.Value; }
            set {
                    if (value.Date > DateTime.Now.Date)
                    {
                        MessageBox.Show("Дата не может быть в будущем.");
                        value = DateTime.Now.Date;
                    }
                    dateTimePicker1.Value = value.Date;
                    dateTimePicker1_ValueChanged(null, null);
                }
        }
        public int DaysValue
        {
            get { return (int)numericUpDown1.Value; }
            set {
                if (value < 0)
                {
                    MessageBox.Show("Дата не может быть в будущем.");
                    value = 0;
                }
                numericUpDown1.Value = value;
                numericUpDown1_ValueChanged(null, null);
            }
        }
        public int Maximum
        {
            get { return (int)numericUpDown1.Maximum; }
            set 
            { 
                numericUpDown1.Maximum = value;
                dateTimePicker1.MinDate = DateTime.Now.Date.AddDays(-value);
            }
        }
        public int Step
        {
            get { return (int)numericUpDown1.Increment; }
            set { numericUpDown1.Increment = value; }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (!_manual) return;

            _manual = false;
            numericUpDown1.Value = (DateTime.Now - dateTimePicker1.Value).Days;
            _manual = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!_manual) return;

            _manual = false;
            TimeSpan ts = new TimeSpan((int)numericUpDown1.Value, 0, 0, 0);
            dateTimePicker1.Value = (DateTime.Now.Subtract(ts)).Date;
            _manual = true;
        }
    }
}
