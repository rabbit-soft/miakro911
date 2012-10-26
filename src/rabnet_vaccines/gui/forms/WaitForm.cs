using System.Windows.Forms;

namespace rabnet
{
    /// <summary>
    /// Форма - ПрограсБар
    /// </summary>
    public partial class WaitForm : Form
    {
        public WaitForm()
        {
            InitializeComponent();
        }

        public bool isFull
        {
            get
            {
                return progressBar1.Value == progressBar1.Maximum;
            }
        }

        public void SetName(string name)
        {
            Text = name;
        }

        public int MinValue
        {
            get { return progressBar1.Minimum; }
            set { progressBar1.Minimum = value; }
        }

        public int MaxValue
        {
            get { return progressBar1.Maximum; }
            set { progressBar1.Maximum = value; }
        }

        public void Inc()
        {
            progressBar1.Value++;
        }

        public void Flush()
        {
            progressBar1.Value=0;
        }

        public ProgressBarStyle Style
        {
            get { return progressBar1.Style; }
            set { progressBar1.Style = value; }
        }
    }
}
