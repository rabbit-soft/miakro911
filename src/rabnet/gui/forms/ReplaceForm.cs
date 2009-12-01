using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class ReplaceForm : Form
    {
        private List<RabNetEngRabbit> rbs = new List<RabNetEngRabbit>();
        public ReplaceForm()
        {
            InitializeComponent();
        }
        public void addRabbit(int id)
        {
            rbs.Add(Engine.get().getRabbit(id));
        }
    }
}
