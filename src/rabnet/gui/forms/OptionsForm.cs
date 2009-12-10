using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void load()
        {
            nudOkrol.Value = Engine.opt().getIntOption(Options.OPT_ID.OKROL);
        }

        private void save()
        {
            Engine.opt().setOption(Options.OPT_ID.OKROL, (int)nudOkrol.Value);
        }
            

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            load();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            save();
            Close();
        }
    }
}
