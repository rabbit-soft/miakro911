using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace updater
{
    public partial class InstallForm : Form
    {
        public int result = 0;
        public string filename = "";
        public InstallForm()
        {
            InitializeComponent();
        }
        public InstallForm(String fl):this()
        {
            filename = fl;
        }
    }
}
