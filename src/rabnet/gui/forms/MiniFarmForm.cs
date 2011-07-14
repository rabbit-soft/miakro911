using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using X_Tools;

namespace rabnet
{
    public partial class MiniFarmForm : Form
    {
        /// <summary>
        /// Если новая то '0'
        /// </summary>
        private int id = 0;
        private int parent = 0;
        int[] tiers = null;
        Building b1 = null;
        Building b2 = null;
        private bool _manual = true;

        public MiniFarmForm()
        {
            InitializeComponent();
            cbUpper.SelectedIndex = 0;
            cbLower.SelectedIndex = 0;
        }

        public MiniFarmForm(int parent,int[] ids):this()
        {
            _manual = false;
            this.parent = parent;
            for (int i = 0; i < ids.Length; i++)
                cbNum.Items.Add(ids[i].ToString());
            cbNum.SelectedIndex = cbNum.Items.Count - 1;
            _manual = true;
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
                case myBuildingType.Jurta: return 1;
                case myBuildingType.Quarta: return 2;
                case myBuildingType.Vertep: return 3;
                case myBuildingType.Barin: return 4;
                case myBuildingType.Female: return 5;
                case myBuildingType.DualFemale: return 6;
                case myBuildingType.Complex: return 7;                
                case myBuildingType.Cabin: return 8;
            }
            return 0;
        }

        private String getType(int id)
        {
            switch(id)
            {
                case 1: return myBuildingType.Jurta;
                case 2: return myBuildingType.Quarta;
                case 3: return myBuildingType.Vertep;
                case 4: return myBuildingType.Barin;
                case 5: return myBuildingType.Female;
                case 6: return myBuildingType.DualFemale;
                case 7: return myBuildingType.Complex;
                case 8: return myBuildingType.Cabin;
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
        /// <summary>
        /// Проверяет на занятость
        /// </summary>
        /// <param name="b"></param>
        /// <returns>0-свободна; 1-занята</returns>
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
                if (Engine.db().FarmExists(fid))
                {
                    MessageBox.Show("Ферма с таким номером уже существует");
                    this.DialogResult = DialogResult.None;
                    return;
                }
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

                if (change == 0)
                {
                    Engine.db().changeFarm(id, getUpperType(), getLowerType());
                    Close();
                }
                else this.DialogResult = DialogResult.None;
            }
        }

        private void cbNum_TextChanged(object sender, EventArgs e)
        {
            if (!_manual) return;
            _manual = false;
            TextBox tb = new TextBox();
            tb.Text = cbNum.Text;
            tb.SelectionStart = cbNum.SelectionStart;
            XTools.checkIntNumber(tb,e);
            cbNum.Text = tb.Text;
            cbNum.SelectionStart = tb.SelectionStart;

            button1.Enabled = (cbNum.Text != "") && (cbNum.Text != "0");
            _manual = true;
        }
    }
}
