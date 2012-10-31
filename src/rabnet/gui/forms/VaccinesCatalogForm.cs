using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class VaccinesCatalogForm : Form
    {
        private const int FIELD_ID = 0;
        private const int FIELD_NAME = 1;
        private const int FIELD_DURA = 2;
        private const int FIELD_AGE = 3;
        private const int FIELD_AFTER = 4;
        private const int FIELD_ZOO = 5;
        /// <summary>
        /// Вносятся ли изменения пользователем
        /// </summary>
        private bool _manual = true;

        public VaccinesCatalogForm()
        {
            InitializeComponent();
            fillTable();
        }

        /// <summary>
        /// Заполнение каталога данными
        /// </summary>
        /// <param name="update"></param>
        private void fillTable()
        {
            _manual = false;
            List<Vaccine> vaccs = Engine.get().db().GetVaccines();
            //int colorExist=-1,imageExist = -1, boolExist=-1; //чтобы удалить слово #color#

            chAfter.Items.Clear();
            chAfter.Items.Add("Рождения");
            foreach (Vaccine v in vaccs)
                chAfter.Items.Add(v.ID + ":" + v.Name);
            
            dataGridView1.Rows.Clear();
            foreach (Vaccine v in vaccs)
            {
                int rowNumber = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowNumber].Cells[FIELD_ID].Value = v.ID;
                dataGridView1.Rows[rowNumber].Cells[FIELD_NAME].Value = v.Name;
                dataGridView1.Rows[rowNumber].Cells[FIELD_DURA].Value = v.Duration;
                dataGridView1.Rows[rowNumber].Cells[FIELD_AGE].Value = v.Age;
                dataGridView1.Rows[rowNumber].Cells[FIELD_AFTER].Value = chAfter.Items[v.After];
                dataGridView1.Rows[rowNumber].Cells[FIELD_ZOO].Value = v.Zoo;
                dataGridView1.Rows[rowNumber].Tag = v;
            }

            _manual = true;
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == FIELD_ZOO)
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
            int duration = 0, age = 0, after = 0;
            bool zoo = false;

            if (editRow.Cells[FIELD_NAME].Value != null)
                name = editRow.Cells[FIELD_NAME].Value.ToString();
            if (name == "") return;
            duration = getIntVal(editRow.Cells[FIELD_DURA].Value);
            age = getIntVal(editRow.Cells[FIELD_AGE].Value);
            if (editRow.Cells[FIELD_AFTER].Value != null && editRow.Cells[FIELD_AFTER].Value.ToString().IndexOf(':') != -1)
            {
                string tmp = editRow.Cells[FIELD_AFTER].Value.ToString().Split(':')[0];
                if (tmp != editRow.Cells[FIELD_ID].Value.ToString())//не является ли после себя самого
                    after = int.Parse(tmp);
            }

            if (editRow.Cells[FIELD_ZOO].Value != null)
                zoo = (bool)editRow.Cells[FIELD_ZOO].Value;

            if (editRow.Cells[FIELD_ID].Value == null)
            {
                editRow.Cells[FIELD_ID].Value = Engine.db().AddVaccine(name, duration, age, after, zoo);
                fillTable();
            }
            else
            {
                Engine.db().EditVaccine(Convert.ToInt32(editRow.Cells[FIELD_ID].Value), name, duration, age, after, zoo);
                dataGridView1.Refresh();
            }
        }

        protected int getIntVal(object cellValue)
        {
            int intVal = 0;
            string result = cellValue == null ? "0" : cellValue.ToString();
            int.TryParse(result, out intVal);
            return intVal;
        }

        private void btAddRow_Click(object sender, EventArgs e)
        {
#if !DEMO
            _manual = false;
            int rind = dataGridView1.Rows.Add();
            dataGridView1.Rows[rind].Cells[FIELD_DURA].Value = 180;
            dataGridView1.Rows[rind].Cells[FIELD_AGE].Value = 45;
            dataGridView1.Rows[rind].Cells[FIELD_AFTER].Value = chAfter.Items[0];
            _manual = true;
#else
            DemoErr.DemoNoModuleMsg();
#endif
        }
    }
}
