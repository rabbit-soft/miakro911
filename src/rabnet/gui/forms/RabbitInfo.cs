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
        private Catalog zones = null;
        private Catalog names = null;
        private Catalog surnames = null;
        private Catalog secnames = null;
        private RabNetEngRabbit rab = null;
        private TabPage malePage;
        private TabPage femalePage;
        private TabPage okrolPage;
        private TabPage suckersPage;
        private TabPage weightPage;
        public RabbitInfo()
        {
            InitializeComponent();
            malePage = tabControl1.TabPages[1];
            femalePage = tabControl1.TabPages[2];
            okrolPage = tabControl1.TabPages[3];
            suckersPage = tabControl1.TabPages[4];
            weightPage = tabControl1.TabPages[5];
            for (int i = 0; i < 5;i++ )
                tabControl1.TabPages.RemoveAt(1);
        }
        public RabbitInfo(int id)
            : this()
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
            label5.Text = "Адрес:" + rab.address;
            label2.Text = "Имя:" + name.Text;
            label3.Text = "Ж.Фам:" + surname.Text;
            label4.Text = "М.Фам:" + secname.Text;
            bdate.Value = rab.born.Date;
            bdate_ValueChanged(null, null);
            notes.Text = rab.notes;
        }

        private void updateMale()
        {
            tabControl1.TabPages.Add(malePage);
            tabControl1.TabPages.Add(weightPage);
        }

        private void updateFemale()
        {
            tabControl1.TabPages.Add(femalePage);
            tabControl1.TabPages.Add(okrolPage);
            tabControl1.TabPages.Add(suckersPage);
            tabControl1.TabPages.Add(weightPage);
        }

        private void FillList(ComboBox cb,Catalog c,int key)
        {
            cb.Items.Clear();
            cb.Items.Add("");
            cb.SelectedIndex = 0;
            foreach (int k in c.Keys)
            {
                int id = cb.Items.Add(c[k]);
                if (key == k)
                    cb.SelectedIndex = id;
            }
        }

        private void fillCatalogs(int what)
        {
            ICatalogs cts = Engine.db().catalogs();
            breeds = cts.getBreeds();
            FillList(breed,breeds,rab.breed);
            zones = cts.getZones();
            FillList(zone, zones, rab.zone);
            int sx=0;
            String end="";
            if (rab.sex==OneRabbit.RabbitSex.MALE)
                sx=1;
            if (rab.sex==OneRabbit.RabbitSex.FEMALE)
            {
                end="а";
                sx=2;
            }
            if (rab.group>1)
                end="ы";
            surnames = cts.getSurNames(2, end);
            secnames = cts.getSurNames(1, end);
            FillList(surname, surnames, rab.surname);
            FillList(secname, secnames, rab.secname);
            if (sx!=0 && rab.group==1)
            {
                names = cts.getFreeNames(sx, rab.name);
                FillList(name, names, rab.name);
                if (rab.name == 0)
                    name.Enabled = true;
            }
        }

        private void updateData()
        {
            if (rid == 0)
                return;
            rab=Engine.get().getRabbit(rid);
            fillCatalogs(0);
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
            name.Enabled=groupBox2.Enabled = checkBox5.Checked;
            if (rab.group > 1 || rab.sex==OneRabbit.RabbitSex.VOID)
                name.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            applyData();
            updateData();
        }

        private void RabbitInfo_Load(object sender, EventArgs e)
        {
            updateData();
        }

        private void bdate_ValueChanged(object sender, EventArgs e)
        {
            age.Value = (DateTime.Now.Date - bdate.Value.Date).Days;
        }

        private void age_ValueChanged(object sender, EventArgs e)
        {
            bdate.Value = DateTime.Today.Subtract(new TimeSpan((int)age.Value, 0, 0, 0));
        }
    }
}
