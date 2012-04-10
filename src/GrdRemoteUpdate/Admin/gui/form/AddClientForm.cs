using System.Windows.Forms;
using pEngine;

namespace AdminGRD
{
    public partial class AddClientForm : Form
    {
        public AddClientForm()
        {
            InitializeComponent();
        }

        public AddClientForm(sClient client):this()
        {
            tbAddress.Text = client.Address;
            tbContact.Text = client.ContactMan;
            tbOrgName.Text = client.Organization;
        }

        public string Address { get { return tbAddress.Text; } }
        public string Contact { get { return tbContact.Text; } }
        public string Organization { get { return tbOrgName.Text; } }
        public bool SAAS { get { return chSAAS.Checked; } }
    }
}
