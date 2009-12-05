using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class BuildingsFilter : FilterPanel
    {
        public BuildingsFilter(RabStatusBar sb):base(sb,"buildings",Options.OPT_ID.BUILD_FILTER)
        {
            //InitializeComponent();
        }

        public BuildingsFilter() : base() { }

        public override Filters getFilters()
        {
            Filters f = new Filters();
            return f;
        }

        public override void setFilters(Filters f)
        {
            //
        }
        public override void clearFilters()
        {
            //
        }

        public override void loadFilters()
        {
            //
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void kvartaBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
