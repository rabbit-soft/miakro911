using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet.forms
{
    public partial class BuildingNotesEdit : Form
    {
        public BuildingNotesEdit(string address,string notes)
        {
            InitializeComponent();
            lAddress.Text += address;
            tbNotes.Text = notes;
        }

        public string Notes
        {
            get { return tbNotes.Text; }
        }
    }
}
