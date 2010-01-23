using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class ZootehFilter : FilterPanel
    {
        public ZootehFilter(RabStatusBar sb):base(sb,"zooteh",Options.OPT_ID.ZOO_FILTER)
        {
            //InitializeComponent();
        }

    #region FilterPanel overrides

        public override Filters getFilters()
        {
            Filters f = new Filters();
            if (!cbCount.Checked || !cbOkrol.Checked || !cbVudvor.Checked || !cbPreokrol.Checked)
            {
                f["act"] = "" + (cbOkrol.Checked ? "O" : "") + (cbVudvor.Checked ? "V" : "") + (cbCount.Checked ? "C" : "") + (cbPreokrol.Checked ? "P" : "")+
                    (cbReplace.Checked ? "R" : "");
                if (f["act"] == "") f.Remove("act");
            }
            return f;
        }

        public override void setFilters(Filters f)
        {
            cbOkrol.Checked = f.safeValue("act", "O").Contains("O");
            cbVudvor.Checked = f.safeValue("act", "V").Contains("V");
            cbCount.Checked = f.safeValue("act", "C").Contains("C");
            cbPreokrol.Checked = f.safeValue("act", "P").Contains("P");
            cbReplace.Checked = f.safeValue("act", "R").Contains("R");
        }

        public override void clearFilters()
        {
            cbOkrol.Checked = cbVudvor.Checked = cbCount.Checked = cbPreokrol.Checked = true;
            cbReplace.Checked = true;
        }
    #endregion

    }
}
