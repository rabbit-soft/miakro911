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

        public void setType(BuildingType type)
        {
            grDelim.Visible = grDelims.Visible = grNest.Visible = grNest2.Visible = grVigul.Visible = false;
            switch (type)
            {
                case BuildingType.Female:
                case BuildingType.Complex: 
                case BuildingType.Jurta:               
                case BuildingType.DualFemale: grNest.Visible = true;
                    if (type == BuildingType.Jurta) grVigul.Visible = true;
                    if (type == BuildingType.DualFemale) grNest2.Visible = true;
                    break;
                case BuildingType.Cabin:
                case BuildingType.Quarta:
                case BuildingType.Barin: 
                case BuildingType.Vertep:
                    //grDelim.Visible = true;  
                    break;
            }
        }

        public class BCEvent : EventArgs
        {
            public enum EVTYPE{REPAIR,NEST,NEST2,HEATER,HEATER2,DELIM,DELIM1,DELIM2,DELIM3,VIGUL};
            public int value;
            public EVTYPE type;
            public int farm;
            public int tier;
            public BCEvent(EVTYPE type,int val)
            {
                this.type=type;
                value=val;
            }
            public BCEvent(EVTYPE type,bool val):this(type,val?1:0){}
            public bool val()
            {
                return value==1;
            }
        }
        public delegate void BCEventHandler(object sender, BCEvent e);
        public event BCEventHandler ValueChanged=null;

        private void makeCBEvent(object sender, EventArgs e)
        {
            if (ValueChanged == null) return;
            CheckBox cb=sender as CheckBox;
            BCEvent.EVTYPE type=BCEvent.EVTYPE.REPAIR;
            if (cb == cbRepair) type=BCEvent.EVTYPE.REPAIR;
            if (cb == cbNest) type = BCEvent.EVTYPE.NEST;
            if (cb == cbNest2) type = BCEvent.EVTYPE.NEST2;
            if (cb == cbDelim) type = BCEvent.EVTYPE.DELIM;
            if (cb == cbDelim1) type = BCEvent.EVTYPE.DELIM1;
            if (cb == cbDelim2) type = BCEvent.EVTYPE.DELIM2;
            if (cb == cbDelim3) type = BCEvent.EVTYPE.DELIM3;
            ValueChanged(this,new BCEvent(type,cb.Checked));
        }

        private void makeComboEvent(object sender, EventArgs e)
        {
            if (ValueChanged == null) return;
            ComboBox cb = sender as ComboBox;
            BCEvent.EVTYPE type = BCEvent.EVTYPE.HEATER;
            if (cb == cbHeater) type = BCEvent.EVTYPE.HEATER;
            if (cb == cbHeater2) type = BCEvent.EVTYPE.HEATER2;
            if (cb == cbVigul) type = BCEvent.EVTYPE.VIGUL;
            ValueChanged(this, new BCEvent(type, cb.SelectedIndex));
        }

    }
}
