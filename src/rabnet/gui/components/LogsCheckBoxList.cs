using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace rabnet.components
{
    public partial class LogsCheckBoxList : UserControl
    {
        public LogsCheckBoxList()
        {
            InitializeComponent();
        }

        public void UpdateList()
        {
#if !DEMO
            lbLogs.Items.Clear();
            String[] lg = Engine.db().logNames();
            for (int i = 0; i < lg.Length; i++)
                lbLogs.Items.Add(lg[i]);
#endif
        }

        public String GetChecked()
        {
            string res = "";
            res = "";
            for (int i = 0; i < lbLogs.Items.Count; i++)
            {
                if (lbLogs.GetItemChecked(i))
                    res += (i + 1).ToString()+',';
            }
            res = res.Trim(',');
            return res;
        }

        private void btAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lbLogs.Items.Count; i++)            
                lbLogs.SetItemChecked(i,true);           
        }
    }
}
