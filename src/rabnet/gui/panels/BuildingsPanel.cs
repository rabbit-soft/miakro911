using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class BuildingsPanel : RabNetPanel
    {
        public BuildingsPanel(): base(){}
        public BuildingsPanel(RabStatusBar sb):base(sb,null)
        {
        }
    }
}
