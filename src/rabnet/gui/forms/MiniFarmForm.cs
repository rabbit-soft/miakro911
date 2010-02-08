using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class MiniFarmForm : Form
    {
        private int id=0;
        private int parent = 0;
        int[] tiers = null;
        Building b1 = null;
        Building b2 = null;
        public MiniFarmForm()
        {
            InitializeComponent();
            cbUpper.SelectedIndex = 0;
            cbLower.SelectedIndex = 0;
        }

        public MiniFarmForm(int parent,int[] ids):this()
        {
            this.parent = parent;
            for (int i = 0; i < ids.Length; i++)
#if TRIAL && !CRACKED
                if (ids[i]<256)
#endif
                cbNum.Items.Add(ids[i].ToString());
            cbNum.SelectedIndex = cbNum.Items.Count - 1;
#if TRIAL  && !CRACKED
            if (cbNum.Items.Count == 0)
                cbNum.Enabled=button1.Enabled=false;
#endif
        }
        public MiniFarmForm(int id):this()
        {
            this.id = id;
            cbNum.Items.Add(id.ToString());
            cbNum.SelectedIndex = 0;
            cbNum.Enabled = false;
            tiers = Engine.db().getTiers(id);
            b1 = Engine.db().getBuilding(tiers[0]);
            cbUpper.SelectedIndex = idFromType(b1.ftype) - 1;
            if (tiers[1] != 0)
            {
                b2 = Engine.db().getBuilding(tiers[1]);
                cbLower.SelectedIndex = idFromType(b2.ftype);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private int idFromType(String type)
        {
            switch(type)
            {
                case "female": return 1;
                case "dfemale": return 2;
                case "complex": return 3;
                case "jurta": return 4;
                case "quarta": return 5;
                case "vertep": return 6;
                case "barin": return 7;
                case "cabin": return 8;
            }
            return 0;
        }

        private String getType(int id)
        {
            switch(id)
            {
                case 1: return "female";
                case 2: return "dfemale";
                case 3: return "complex";
                case 4: return "jurta";
                case 5: return "quarta";
                case 6: return "vertep";
                case 7: return "barin";
                case 8: return "cabin";
            }
            return "none";
        }

        private String getUpperType()
        {
            return getType(cbUpper.SelectedIndex + 1);
        }

        private String getLowerType()
        {
            return getType(cbLower.SelectedIndex);
        }

        private bool hasRabbits(Building b)
        {
            if (b==null)
                return false;
            bool res = false;
            for (int i = 0; i < b.secs(); i++)
                if (b.busy(i) != 0)
                    res = true;
            return res;
        }

        private int checkBuilding(Building b)
        {
            if (!hasRabbits(b)) return 0;
            MessageBox.Show("Перед изменением типа яруса расселите его.");
            return 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                int fid = int.Parse(cbNum.Text);
#if TRIAL && !CRACKED
                if (fid > 255)
                {
                    MessageBox.Show("В Демо версии нельзя сделать больше 255 ферм");
                    return;
                }
#endif
                Engine.db().addFarm(parent, getUpperType(), getLowerType(), "", fid);
                Close();
            }
            else
            {
                int change = -1;
                if (getUpperType() != b1.ftype)
                    change = checkBuilding(b1);
                if ((b2 != null && getLowerType() != b2.ftype) || (b2 == null && getLowerType() != "none"))
                {
                    if (change<1)
                        change = checkBuilding(b2);
                }
                if (change==0)
                {
                    Engine.db().changeFarm(id, getUpperType(), getLowerType());
                }
                Close();
            }
        }
    }
}
