using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using rabnet.RNC;

namespace rabnet
{
    public partial class FarmNewForm : Form
    {
        RabnetConfig _rnc;

        public FarmNewForm()
        {
            _rnc = new RabnetConfig();
            _rnc.LoadDataSources();
            InitializeComponent();
            farmsPanel1.Init(_rnc);
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            _rnc.SaveDataSources();
        }       
    }
}
