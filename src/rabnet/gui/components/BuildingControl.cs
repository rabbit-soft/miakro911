using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet.components
{
    public partial class BuildingControl : UserControl
    {
        public delegate void BCEventHandler(object sender, BCEvent e);

        public class BCEvent : EventArgs
        {
            public enum EVTYPE { REPAIR, NEST, NEST2, HEATER, HEATER2, DELIM, DELIM1, DELIM2, DELIM3, VIGUL };            
            public int value;
            public EVTYPE type;
            public int farm;
            public int tier;

            public BCEvent(EVTYPE type, int val)
            {
                this.type = type;
                value = val;
            }

            public BCEvent(EVTYPE type, bool val) : this(type, val ? 1 : 0) 
            { 
            }

            public bool val()
            {
                return value == 1;
            }
        }

        bool _manual = true;

        public event BCEventHandler ValueChanged = null;

        public BuildingControl()
        {
            InitializeComponent();
            cbVigul.SelectedIndex = 0;
            cbHeater.SelectedIndex = 0;
            cbHeater2.SelectedIndex = 0;
        }
        public bool repair
        {
            get { return chRepair.Checked; }
            set { chRepair.Checked = value; }
        }
        public bool nest
        {
            get { return chNest.Checked; }
            set { chNest.Checked = value; }
        }
        public bool nest2
        {
            get { return chNest2.Checked; }
            set { chNest2.Checked = value; }
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
            get { return chDelim.Checked; }
            set { chDelim.Checked = value; }
        }
        public bool delim1
        {
            get { return chDelim1.Checked; }
            set { chDelim1.Checked = value; }
        }
        public bool delim2
        {
            get { return chDelim2.Checked; }
            set { chDelim2.Checked = value; }
        }
        public bool delim3
        {
            get { return chDelim3.Checked; }
            set { chDelim3.Checked = value; }
        }
        public bool vigul
        {
            get { return cbVigul.SelectedIndex == 1; }
            set { cbVigul.SelectedIndex = (value ? 1 : 0); }
        }

        public void SetType(BuildingType type)
        {
            gbOneDelim.Visible = gbDelims.Visible = grNest.Visible = gbNest2.Visible = grVigul.Visible = false;
            switch (type)
            {
                case BuildingType.Female:
                    grNest.Visible = true;
                    break;                
                case BuildingType.Jurta:
                    grNest.Visible = grVigul.Visible = true;
                    break;
                case BuildingType.DualFemale: 
                    grNest.Visible = gbNest2.Visible = true;
                    break;
                case BuildingType.Complex:
                    grNest.Visible = true;                    
                    break;
                case BuildingType.Cabin:            
                case BuildingType.Barin: 
                case BuildingType.Vertep:
                    break;
                case BuildingType.Quarta:
                    gbDelims.Visible = true;
                    break;
            }
        }        

        private void makeCBEvent(object sender, EventArgs e)
        {
            if (!_manual || ValueChanged == null) return;

            CheckBox cb=sender as CheckBox;

            BCEvent.EVTYPE type = BCEvent.EVTYPE.REPAIR;
            if (cb == chRepair) type=BCEvent.EVTYPE.REPAIR;
            if (cb == chNest)   type = BCEvent.EVTYPE.NEST;
            if (cb == chNest2)  type = BCEvent.EVTYPE.NEST2;
            if (cb == chDelim)  type = BCEvent.EVTYPE.DELIM;
            if (cb == chDelim1) type = BCEvent.EVTYPE.DELIM1;
            if (cb == chDelim2) type = BCEvent.EVTYPE.DELIM2;
            if (cb == chDelim3) type = BCEvent.EVTYPE.DELIM3;

            try
            {
                ValueChanged(this, new BCEvent(type, cb.Checked));
            }
            catch (Exception exc)
            {                
                MessageBox.Show(exc.Message,"Действие невозможно",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                _manual = false;
                cb.Checked = !cb.Checked;
                _manual = true;
            }
        }

        private void makeComboEvent(object sender, EventArgs e)
        {
            if (ValueChanged == null) {
                return;
            }

            ComboBox cb = sender as ComboBox;
            BCEvent.EVTYPE type = BCEvent.EVTYPE.HEATER;
            if (cb == cbHeater) {
                type = BCEvent.EVTYPE.HEATER;
            }
            if (cb == cbHeater2) {
                type = BCEvent.EVTYPE.HEATER2;
            }
            if (cb == cbVigul) {
                type = BCEvent.EVTYPE.VIGUL;
            }
            ValueChanged(this, new BCEvent(type, cb.SelectedIndex));
        }

    }
}
