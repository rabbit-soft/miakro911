using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class YoungsPanel : RabNetPanel
    {
        public YoungsPanel():base()
        {
        }
        public YoungsPanel(RabStatusBar sb)
            : base(sb, null)
        {
        }
    }
}
