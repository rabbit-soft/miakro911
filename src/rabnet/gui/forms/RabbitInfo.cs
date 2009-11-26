using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class RabbitInfo : Form
    {
        private int rid = 0;
        private Catalog breeds = null;
        private RabNetEngRabbit rab = null;
        public RabbitInfo()
        {
            InitializeComponent();
        }
        public RabbitInfo(int id)
            : base()
        {
            rid = id;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button5.PerformClick();
            Close();
        }

        private void updateStd()
        {
            defect.Checked = rab.defect;
            gp.Checked = rab.production;
            gr.Checked = rab.realization;
            spec.Checked = rab.spec;
            rate.Value = rab.rate;
            group.Value = rab.group;

        }

        private void updateMale()
        {
        }

        private void updateFemale()
        {
        }

        private void fillCatalogs(int what)
        {
            ICatalogs cts = Engine.db().catalogs();
            breeds = cts.getBreeds();
            breed.Items.Clear();
            breed.Items.Add("");
            foreach (int key in breeds.Keys)
                breed.Items.Add(breeds[key]);
        }

        private void updateData()
        {
            if (rid == 0)
                return;
            rab=Engine.get().getRabbit(rid);
            updateStd();
            if (rab.sex == OneRabbit.RabbitSex.MALE)
                updateMale();
            if (rab.sex == OneRabbit.RabbitSex.FEMALE)
                updateFemale();
        }

        private void applyData()
        {
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            String CHANGE_ERR=@"Вы пытаетесь изменить статичные данные.
эти типа плохо... и тд... и тп... 
Изменить?";
            if (checkBox5.Checked)
                if (MessageBox.Show(CHANGE_ERR,"Изменить данные?",
                    MessageBoxButtons.YesNo,MessageBoxIcon.Warning)!=DialogResult.Yes)
                {
                    checkBox5.Checked=false;
                    return;
                }
            groupBox2.Enabled = checkBox5.Checked;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            applyData();
            updateData();
        }

        private void RabbitInfo_Load(object sender, EventArgs e)
        {
            fillCatalogs(0);
            updateData();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
