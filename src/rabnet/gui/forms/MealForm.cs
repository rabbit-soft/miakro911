﻿using System;
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
        private const string rusIN = "Привоз";
        private const string rusOUT = "Продажа";

        public MealForm()
        {
            InitializeComponent();
            fillPeriods();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            X_Tools.XTools.checkIntNumber(sender, e);
        }

        private void fillPeriods()
        {
            dataGridView1.Rows.Clear();
            List<sMeal> per = Engine.get().db().getMealPeriods();
            foreach (sMeal m in per)
            {
                string type = m.Type == sMeal.MoveType.In ? rusIN : rusOUT;
                string end = m.Type == sMeal.MoveType.In ? m.EndDate == DateTime.MinValue ? " - " : m.EndDate.ToShortDateString() : "   --|||--    ";
                string rate = m.Type == sMeal.MoveType.In ? m.Rate.ToString() : "   --|||--    ";         
                dataGridView1.Rows.Add(new string[] { m.StartDate.ToShortDateString(), end, m.Amount.ToString(),type, rate });
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = m;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Style.ForeColor = m.Type == sMeal.MoveType.In ? Color.Green : Color.Crimson;
            }
            if (this.dataGridView1.Rows.Count!=0) 
                dataGridView1.CurrentCell = this.dataGridView1[0, this.dataGridView1.Rows.Count - 1];
            dtpStartDate.MaxDate = DateTime.Now;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if (!(rbIn.Checked || rbSell.Checked)) { MessageBox.Show("Выберите \"Привоз\" или \"Продажа\""); return; }
            if (tbAmount.Text == "") { MessageBox.Show("Заполните данными поле \"Объем\""); return; }
            if (rbIn.Checked)
            {
                
                Engine.get().db().addMealIn(dtpStartDate.Value, int.Parse(tbAmount.Text));
                tbAmount.Clear();
                fillPeriods();
                dtpStartDate.Value = dtpStartDate.MaxDate;
            }
            else
            {
                int exclude = int.Parse(tbAmount.Text);
                if (!canAddMealOut()) { MessageBox.Show("Корма не достаточно чтобы продать такое количество."); return; }
                Engine.get().db().addMealOut(dtpStartDate.Value, exclude);
                tbAmount.Clear();
                fillPeriods();
                dtpStartDate.Value = dtpStartDate.MaxDate;
            }
            rbIn.Checked = rbSell.Checked = false;

        }

        private bool canAddMealOut()
        {
            if (rbIn.Checked) return false;
            DateTime excDate = dtpStartDate.Value;
            int excAmount = int.Parse(tbAmount.Text);
            int indS=-1, indE = -1;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                sMeal meal = (dataGridView1.Rows[i].Tag as sMeal);
                if (meal.Type == sMeal.MoveType.In)
                {
                    if (meal.StartDate <= excDate)
                        indS = i;
                    if (indS != -1 && indE == -1 && meal.StartDate > excDate)                   
                        indE = i-1;                  
                }

            }
            if (indS == -1)//если перед первым завозом
                return true;

            int start = dataGridView1.Rows.Count-1;
            if (indE != -1)
                start=indE;
            int remain = -excAmount;
            for (int i = start ; i >= indS; i--)
            {
                sMeal meal = (dataGridView1.Rows[i].Tag as sMeal);
                if (meal.Type == sMeal.MoveType.In)
                {
                    remain += meal.Amount;
                    break;
                }
                else               
                    remain -= meal.Amount;
                
            }
            return !(remain < 0);
        }

    }
}
