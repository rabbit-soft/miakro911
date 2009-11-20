using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class WorksPanel : RabNetPanel
    {
        public WorksPanel():base(){}
        public WorksPanel(RabStatusBar sb)
            : base(sb, null)
        {
        }
    }
}
