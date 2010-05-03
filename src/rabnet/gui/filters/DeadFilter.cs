using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class DeadFilter : FilterPanel
    {
        public DeadFilter(RabStatusBar sb):base(sb,"deads",Options.OPT_ID.DEAD_FILTER)
        {
            //InitializeComponent();
        }

    #region FilterPanelFuncs

        public override Filters getFilters()
        {
            Filters f = new Filters();
            f["max"] = nudMax.Value.ToString();
            f["nm"] = tbName.Text;
            return f;
        }

        public override void setFilters(Filters f)
        {
            nudMax.Value = f.safeInt("max", 1000);
            tbName.Text = f.safeValue("nm");
        }

        public override void clearFilters()
        {
            nudMax.Value = 1000;
            tbName.Text = "";
        }

    #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
        }


    }
}
