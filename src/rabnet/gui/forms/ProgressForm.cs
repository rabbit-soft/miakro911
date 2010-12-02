using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

#if !DEMO
        public void progressUp(int p)
        {
            if (this.InvokeRequired)
            {
                RabUpdaterClient.progressUpCB d = new RabUpdaterClient.progressUpCB(progressUp);
                this.Invoke(d, new object[] { p });
            }
            else
            {
                progressBar1.Value = p;
            }
        }
#endif

    }
}
