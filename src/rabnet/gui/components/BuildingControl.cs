using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class BuildingControl : UserControl
    {
        public BuildingControl()
        {
            InitializeComponent();
            cbVigul.SelectedIndex = 0;
            cbHeater.SelectedIndex = 0;
            cbHeater2.SelectedIndex = 0;
        }
        public bool repair
        {
            get { return cbRepair.Checked; }
            set { cbRepair.Checked = value; }
        }
        public bool nest
        {
            get { return cbNest.Checked; }
            set { cbNest.Checked = value; }
        }
        public bool nest2
        {
            get { return cbNest2.Checked; }
            set { cbNest2.Checked = value; }
        }
        public int heater
        {
            get { return cbHeater.SelectedIndex; }
            set { cbHeater.SelectedIndex = value; }
        }
        public int heater2
        {
            get { return cbHeater2.SelectedIndex; }
            set { cbHeater2.SelectedIndex = value; }
        }
        public bool delim
        {
            get { return cbDelim.Checked; }
            set { cbDelim.Checked = value; }
        }
        public bool delim1
        {
            get { return cbDelim1.Checked; }
            set { cbDelim1.Checked = value; }
        }
        public bool delim2
        {
            get { return cbDelim2.Checked; }
            set { cbDelim2.Checked = value; }
        }
        public bool delim3
        {
            get { return cbDelim3.Checked; }
            set { cbDelim3.Checked = value; }
        }
        public bool vigul
        {
            get { return cbVigul.SelectedIndex == 1; }
            set { cbVigul.SelectedIndex = (value ? 1 : 0); }
        }

        public void setType(String type)
        {
            grDelim.Visible = grDelims.Visible = grNest.Visible = grNest2.Visible = grVigul.Visible = false;
            switch (type)
            {
                case "female":
                case "complex": 
                case "jurta":
                case "cabin":
                case "dfemale": grNest.Visible = true;
                    if (type == "jurta") grVigul.Visible = true;
                    if (type == "dfemale") grNest2.Visible = true;
                    break;
                case "quarta": grDelims.Visible = true; break;
                case "barin": grDelim.Visible = true;  break;
            }
        }

    }
}
