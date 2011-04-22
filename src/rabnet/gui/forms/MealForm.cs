using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class MealForm : Form
    {
        public MealForm()
        {
            InitializeComponent();
            fillPeriods();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            X_Tools.XTools.checkFloatNumber(sender, e);
        }

        private void fillPeriods()
        {
            dataGridView1.Rows.Clear();
            List<sMeal> per = Engine.get().db().getMealPeriods();
            foreach (sMeal m in per)
            {
                dataGridView1.Rows.Add(new string[] { m.StartDate, m.EndDate, m.Amount.ToString(), m.Rate.ToString() });
                dtpStartDate.MinDate = DateTime.Parse(m.StartDate);
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            Engine.get().db().addMealPeriod(dtpStartDate.Value,float.Parse(tbAmount.Text));
            tbAmount.Clear();
            fillPeriods();
            dtpStartDate.Value = DateTime.Now;
        }

    }
}
