using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using rabnet.components;

namespace rabnet.filters
{
    public partial class ZootehFilter : FilterPanel
    {
        const String ITEM_FLAGS = "OVCPRFfvNBS";
        public ZootehFilter()
            : base("zooteh", Options.OPT_ID.ZOO_FILTER)
        {
            //InitializeComponent();
        }

        protected override void initAgain()
        {
#if !DEMO
            lbLogs.Items.Clear();
            String[] lg = Engine.db().logNames();
            for (int i = 0; i < lg.Length; i++) {
                lbLogs.Items.Add(lg[i]);
            }
#endif
        }

        #region FilterPanel overrides

        public override Filters getFilters()
        {
            Filters f = new Filters();
            f["act"] = "";
            bool ac = true; //не понятно что за переменная
            for (int i = 0; i < lbZoo.Items.Count; i++) {
                if (lbZoo.GetItemChecked(i)) {
                    f["act"] += ITEM_FLAGS[i];
                } else {
                    ac = false;
                }
            }
            if (f["act"] == "" || ac) f.Remove("act");

            ac = true;
            f["lgs"] = "";
            for (int i = 0; i < lbLogs.Items.Count; i++) {
                if (lbLogs.GetItemChecked(i)) {
                    f["lgs"] += "," + (i + 1).ToString();
                } else {
                    ac = false;
                }
            }
            f["lgs"] = f["lgs"].Trim(',');
            if (f["lgs"] == "" || ac) {
                f.Remove("lgs");
            }

            if (nudLogLim.Value != 100) {
                f["lim"] = nudLogLim.Value.ToString();
            }
            return f;
        }

        private bool hasnum(string[] nums, int num)
        {
            if (nums.Length == 1 && nums[0] == "0") {
                return true;
            }
            foreach (String nm in nums) {
                if (int.Parse(nm) == num) {
                    return true;
                }
            }
            return false;
        }

        public override void setFilters(Filters f)
        {
            for (int i = 0; i < lbZoo.Items.Count; i++) {
                lbZoo.SetItemChecked(i, f.safeValue("act", ITEM_FLAGS).Contains("" + ITEM_FLAGS[i]));
            }
            String[] nums = f.safeValue("lgs", "0").Split(',');
            for (int i = 0; i < lbLogs.Items.Count; i++) {
                lbLogs.SetItemChecked(i, hasnum(nums, i + 1));
            }
            nudLogLim.Value = f.safeInt("lim", 100);
        }

        public override void clearFilters()
        {
            for (int i = 0; i < lbZoo.Items.Count; i++) {
                lbZoo.SetItemChecked(i, true);
            }
            for (int i = 0; i < lbLogs.Items.Count; i++) {
                lbLogs.SetItemChecked(i, true);
            }
            nudLogLim.Value = 100;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
        }

    }
}
