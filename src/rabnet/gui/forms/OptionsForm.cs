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
            Options o=Engine.opt();
            nudOkrol.Value = o.getIntOption(Options.OPT_ID.OKROL);
            nudVudvor.Value = o.getIntOption(Options.OPT_ID.VUDVOR);
            nudCount1.Value = o.getIntOption(Options.OPT_ID.COUNT1);
            nudCount2.Value = o.getIntOption(Options.OPT_ID.COUNT2);
            nudCount3.Value = o.getIntOption(Options.OPT_ID.COUNT3);
            nudBrides.Value = o.getIntOption(Options.OPT_ID.MAKE_BRIDE);
            nudPreokrol.Value = o.getIntOption(Options.OPT_ID.PRE_OKROL);
        }

        private void save()
        {
            Options o = Engine.opt();
            o.setOption(Options.OPT_ID.OKROL, (int)nudOkrol.Value);
            o.setOption(Options.OPT_ID.VUDVOR, (int)nudVudvor.Value);
            o.setOption(Options.OPT_ID.COUNT1, (int)nudCount1.Value);
            o.setOption(Options.OPT_ID.COUNT2, (int)nudCount2.Value);
            o.setOption(Options.OPT_ID.COUNT3, (int)nudCount3.Value);
            o.setOption(Options.OPT_ID.MAKE_BRIDE, (int)nudBrides.Value);
            o.setOption(Options.OPT_ID.PRE_OKROL, (int)nudPreokrol.Value);
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
