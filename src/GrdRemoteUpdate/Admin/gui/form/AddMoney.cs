using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AdminGRD
{
    public partial class AddMoneyForm : Form
    {
        public AddMoneyForm()
        {
            InitializeComponent();
            numericUpDown1.Focus();
        }

        public int Value { get { return (int)numericUpDown1.Value; } }
    }
}
