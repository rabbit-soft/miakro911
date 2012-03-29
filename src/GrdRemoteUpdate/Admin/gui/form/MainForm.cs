using System.Windows.Forms;

namespace AdminGRD
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            pEngine.RequestSender p = new pEngine.RequestSender();
        }

        public bool Retry = false;
    }
}
