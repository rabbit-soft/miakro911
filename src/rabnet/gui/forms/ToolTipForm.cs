using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class ToolTipForm : Form
    {
        private static ToolTipForm ttfrm = null;
        public static ToolTipForm getForm()
        {
            if (ttfrm == null)
                ttfrm = new ToolTipForm();
            return ttfrm;
        }

        public ToolTipForm()
        {
            InitializeComponent();
        }

        private void ToolTipForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ttfrm = null;
        }
    }
}
