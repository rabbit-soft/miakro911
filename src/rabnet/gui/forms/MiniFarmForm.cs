using System;
using System.Windows.Forms;
using gamlib;

namespace rabnet.forms
{
    public partial class MiniFarmForm : Form
    {
        /// <summary>
        /// Если новая то '0'
        /// </summary>
        private int _id = 0;
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
#if DEMO
            cbNum.DropDownStyle = ComboBoxStyle.DropDownList;
#endif
        }

        public MiniFarmForm(int parent, int[] ids)
            : this()
        {
            _manual = false;
            this.parent = parent;
            for (int i = 0; i < ids.Length; i++) {
                cbNum.Items.Add(ids[i].ToString());
            }
            cbNum.SelectedIndex = cbNum.Items.Count - 1;
            _manual = true;
        }

        public MiniFarmForm(int id)
            : this()
        {
            this._id = id;
            cbNum.Items.Add(id.ToString());
            cbNum.SelectedIndex = 0;
            cbNum.Enabled = false;
            tiers = Engine.db().getTiers(id);
            b1 = Engine.db().getBuilding(tiers[0]);
            cbUpper.SelectedIndex = idFromType(b1.Type) - 1;
            if (tiers[1] != 0) {
                b2 = Engine.db().getBuilding(tiers[1]);
                cbLower.SelectedIndex = this.idFromType(b2.Type);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private int idFromType(BuildingType type)
        {
            switch (type) {
                case BuildingType.Jurta: return 1;
                case BuildingType.Quarta: return 2;
                case BuildingType.Vertep: return 3;
                case BuildingType.Barin: return 4;
                case BuildingType.Female: return 5;
                case BuildingType.DualFemale: return 6;
                case BuildingType.Complex: return 7;
                case BuildingType.Cabin: return 8;
            }
            return 0;
        }

        private BuildingType getType(int id)
        {
            switch (id) {
                case 1: return BuildingType.Jurta;
                case 2: return BuildingType.Quarta;
                case 3: return BuildingType.Vertep;
                case 4: return BuildingType.Barin;
                case 5: return BuildingType.Female;
                case 6: return BuildingType.DualFemale;
                case 7: return BuildingType.Complex;
                case 8: return BuildingType.Cabin;
            }
            return BuildingType.None;
        }

        /// <summary>
        /// Выбраный пользователем Верхний ярус
        /// </summary>
        private BuildingType upperType
        {
            get { return getType(cbUpper.SelectedIndex + 1); }
        }

        /// <summary>
        /// Выбраный пользователем Нижний ярус
        /// </summary>
        private BuildingType lowerType
        {
            get { return getType(cbLower.SelectedIndex); }
        }

        /// <summary>
        /// Сидят ли в строении кролики
        /// </summary>
        private bool hasRabbits(Building b)
        {
            if (b == null) return false;

            for (int i = 0; i < b.Sections; i++) {
                if (b.Busy[i].ID > 0) {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Проверяет на занятость
        /// </summary>
        /// <param name="b"></param>
        /// <returns>0-свободна; 1-занята</returns>
        private int checkBuilding(Building b)
        {
            if (!hasRabbits(b)) {
                return 0;
            }
            MessageBox.Show("Перед изменением типа яруса расселите его.");
            return 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_id == 0) {
                int fid = int.Parse(cbNum.Text);
                if (Engine.db().FarmExists(fid)) {
                    MessageBox.Show("Ферма с таким номером уже существует");
                    this.DialogResult = DialogResult.None;
                    return;
                }
                Engine.db().addFarm(parent, upperType, lowerType, "", fid);
                this.Close();
            } else {
                int change = -1;
                if (upperType != b1.Type) {
                    change = checkBuilding(b1);
                }

                if ((b2 != null && lowerType != b2.Type) || (b2 == null && lowerType != BuildingType.None)) {
                    if (change < 1) {
                        change = checkBuilding(b2);
                    }
                }

                if (change == 0) {
                    Engine.db().ChangeFarm(_id, upperType, lowerType);
                    this.Close();
                } else {
                    this.DialogResult = DialogResult.None;
                }
            }
        }

        private void cbNum_TextChanged(object sender, EventArgs e)
        {
            if (!_manual) {
                return;
            }
            _manual = false;
            TextBox tb = new TextBox();
            tb.Text = cbNum.Text;
            tb.SelectionStart = cbNum.SelectionStart;
            Helper.checkIntNumber(tb, e);
            cbNum.Text = tb.Text;
            cbNum.SelectionStart = tb.SelectionStart;

            button1.Enabled = (cbNum.Text != "") && (cbNum.Text != "0");
            _manual = true;
        }
    }
}
