using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet.forms
{
    public partial class VaccinesCatalogForm : Form
    {
        private const int FIELD_ID = 0;
        private const int FIELD_NAME = 1;
        private const int FIELD_AGE = 2;
        private const int FIELD_DURA = 3;       
        private const int FIELD_AFTER = 4;
        private const int FIELD_ZOO = 5;
        private const int FIELD_TIMES = 6;

        private const int WIDTH_ADD = 210;
        /// <summary>
        /// Вносятся ли изменения пользователем
        /// </summary>
        private bool _manual = true;

        public VaccinesCatalogForm()
        {
            InitializeComponent();
            if (nudAge.Value > nudLustDuration.Value)
                nudAge.Value = nudLustDuration.Value;
            nudAge.Maximum = nudLustDuration.Value;
            dataGridView1.Anchor = dataGridView1.Anchor | AnchorStyles.Right;
            //gbLust.Anchor = dataGridView1.Anchor | AnchorStyles.Right;
            fillTable();
        }

        /// <summary>
        /// Заполнение каталога данными
        /// </summary>
        /// <param name="update"></param>
        private void fillTable()
        {
            _manual = false;
            List<Vaccine> vaccs = Engine.get().db().GetVaccines(true);
            //int colorExist=-1,imageExist = -1, boolExist=-1; //чтобы удалить слово #color#

            chAfter.Items.Clear();
            chAfter.Items.Add("Рождения");
            foreach (Vaccine v in vaccs)
            {
                if (v.ID < 0) continue;
                chAfter.Items.Add(v.ID + ":" + v.Name);
            }

            dataGridView1.Rows.Clear();
            foreach (Vaccine v in vaccs)
            {
                if (v.ID == Vaccine.V_ID_LUST)
                {
                    nudLustDuration.Value = v.Duration;
                    nudAge.Value = v.Age;
                    continue;
                }

                int rowNumber = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowNumber].Cells[FIELD_ID].Value = v.ID;
                dataGridView1.Rows[rowNumber].Cells[FIELD_NAME].Value = v.Name;
                dataGridView1.Rows[rowNumber].Cells[FIELD_AGE].Value = v.Age;
                dataGridView1.Rows[rowNumber].Cells[FIELD_DURA].Value = v.Duration;
                dataGridView1.Rows[rowNumber].Cells[FIELD_AFTER].Value = chAfter.Items[v.After];
                dataGridView1.Rows[rowNumber].Cells[FIELD_ZOO].Value = v.Zoo;
                dataGridView1.Rows[rowNumber].Cells[FIELD_TIMES].Value = v.DoTimes == 1;
                dataGridView1.Rows[rowNumber].Tag = v;
            }

            _manual = true;
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == FIELD_ZOO||dataGridView1.CurrentCell.ColumnIndex ==FIELD_TIMES)
            {
                _manual = false;
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                _manual = true;
                dataGridView1_CellEndEdit(sender, new DataGridViewCellEventArgs(dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.RowIndex));
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!_manual) return;
            DataGridViewRow editRow = dataGridView1.Rows[e.RowIndex];

            string name = "";
            int duration = 0, age = 0, after = 0,times=0;
            bool zoo = false;

            if (editRow.Cells[FIELD_NAME].Value != null)
                name = editRow.Cells[FIELD_NAME].Value.ToString();
            if (name == "") return;
            duration = getIntVal(editRow.Cells[FIELD_DURA].Value);///todo если введено не число, то сразу исправлять
            age = getIntVal(editRow.Cells[FIELD_AGE].Value);
            if (editRow.Cells[FIELD_AFTER].Value != null && editRow.Cells[FIELD_AFTER].Value.ToString().IndexOf(':') != -1)
            {
                string tmp = editRow.Cells[FIELD_AFTER].Value.ToString().Split(':')[0];
                if (tmp != editRow.Cells[FIELD_ID].Value.ToString())//не является ли после себя самого
                    after = int.Parse(tmp);
                else
                    editRow.Cells[FIELD_AFTER].Value = chAfter.Items[0];
            }

            if (editRow.Cells[FIELD_ZOO].Value != null)
                zoo = (bool)editRow.Cells[FIELD_ZOO].Value;
            if (editRow.Cells[FIELD_TIMES].Value != null)
                times = (bool)editRow.Cells[FIELD_TIMES].Value ?1:0;

            if (editRow.Cells[FIELD_ID].Value == null)
            {
                editRow.Cells[FIELD_ID].Value = Engine.db().AddVaccine(name, duration, age, after, zoo,times);
                //fillTable();
            }
            else
            {
                Engine.db().EditVaccine(Convert.ToInt32(editRow.Cells[FIELD_ID].Value), name, duration, age, after, zoo,times);
                dataGridView1.Refresh();
            }
        }

        protected int getIntVal(object cellValue)
        {
            int intVal = 0;
            string result = cellValue == null ? "0" : cellValue.ToString();
            int.TryParse(result, out intVal);
            if (intVal < 0)
                intVal = 0;
            return intVal;
        }

        private void btAddRow_Click(object sender, EventArgs e)
        {
            //надо быть отчаянным вакцинатором чтобы вывести это сообщение
            if (dataGridView1.Rows.Count > Vaccine.MAX_VACS_COUNT)
            {
                MessageBox.Show("Достигнуто максимальное количествово прививок");
                return;
            }
            _manual = false;
            int rind = dataGridView1.Rows.Add();
            dataGridView1.Rows[rind].Cells[FIELD_DURA].Value = 180;
            dataGridView1.Rows[rind].Cells[FIELD_AGE].Value = 45;
            dataGridView1.Rows[rind].Cells[FIELD_AFTER].Value = chAfter.Items[0];
            _manual = true;
        }

        private void btSpetial_Click(object sender, EventArgs e)
        {
            dataGridView1.Anchor -= AnchorStyles.Right;
            btSpetial.Anchor -= AnchorStyles.Right;
            btSpetial.Anchor = btSpetial.Anchor | AnchorStyles.Left;
            gbLust.Anchor -= AnchorStyles.Right;
            gbLust.Anchor = gbLust.Anchor | AnchorStyles.Left;

            if (btSpetial.Tag.ToString()== "0")
            {                            
                this.Width += WIDTH_ADD;
                btSpetial.Text = "<< Cкрыть";
                btSpetial.Tag = "1";
            }
            else
            {
                this.Width -= WIDTH_ADD;
                btSpetial.Text = "Спец. уколы >>";
                btSpetial.Tag = "0";
            }

            dataGridView1.Anchor = dataGridView1.Anchor | AnchorStyles.Right;
            btSpetial.Anchor -= AnchorStyles.Left;
            btSpetial.Anchor = btSpetial.Anchor | AnchorStyles.Right;
            gbLust.Anchor -= AnchorStyles.Left;
            gbLust.Anchor = gbLust.Anchor | AnchorStyles.Right;
        }

        private void btLustSave_Click(object sender, EventArgs e)
        {
            Engine.db().EditVaccine(Vaccine.V_ID_LUST, "Стимуляция", (int)nudLustDuration.Value, (int)nudAge.Value, 0, false, 0);
        }

        private void nudLustDuration_ValueChanged(object sender, EventArgs e)
        {
            nudAge.Maximum = nudLustDuration.Value;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!_manual) return;

            if (e.ColumnIndex == FIELD_DURA || e.ColumnIndex == FIELD_AGE)
            {
                int o = 0;
                if (!int.TryParse(e.FormattedValue.ToString(), out o) || o<0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Значение должно быть положительным числом");
                }
            }
        }
    }
}
