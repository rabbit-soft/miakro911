using System.Windows.Forms;

namespace rabdump
{
    public partial class WaitForm : Form
    {
        public WaitForm()
        {
            InitializeComponent();
        }
        public void SetName(string name)
        {
            Text = name;
        }
    }
}
