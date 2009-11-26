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
        private int curzone = 0;
        public RabbitInfo()
        {
            InitializeComponent();
            malePage = tabControl1.TabPages[1];
            femalePage = tabControl1.TabPages[2];
            okrolPage = tabControl1.TabPages[3];
            suckersPage = tabControl1.TabPages[4];
            weightPage = tabControl1.TabPages[5];
            while(tabControl1.TabPages.Count>1)
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
            String[] gns = rab.genom.Split(' ');
            gens.Items.Clear();
            foreach (string s in gns)
                addgen(int.Parse(s));
        }

        private void updateMale()
        {
            tabControl1.TabPages.Add(malePage);
            tabControl1.TabPages.Add(weightPage);
            maleStatus.SelectedIndex = rab.status;
            lastFuckNever.Checked = rab.last_fuck_okrol == DateTime.MinValue;
            lastFuckNever_CheckedChanged(null, null);
            if (!lastFuckNever.Checked)
            {
                lastFuck.Value = rab.last_fuck_okrol;
            }
            label7.Text = "Статус:" + maleStatus.Text;
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
            fillNames(sx);
        }

        private void fillNames(int sx)
        {
            if (sx != 0 && rab.group == 1)
            {
                names = Engine.db().catalogs().getFreeNames(sx, rab.wasname);
                FillList(name, names, rab.name);
                if (rab.name == 0)
                    name.Enabled = true;
            }
        }

        private void updateData()
        {
            while (tabControl1.TabPages.Count > 1)
                tabControl1.TabPages.RemoveAt(1);
            if (rid == 0)
                return;
            rab=Engine.get().getRabbit(rid);
            fillCatalogs(0);
            updateStd();
            if (rab.sex == OneRabbit.RabbitSex.VOID)
                label7.Text = "Статус:" + (rab.status == 1 ? "Гнездовые" : "Подсосные");
            if (rab.sex == OneRabbit.RabbitSex.MALE)
                updateMale();
            if (rab.sex == OneRabbit.RabbitSex.FEMALE)
                updateFemale();
        }

        private int getCatValue(Catalog c,string value)
        {
            if (value == "")
                return 0;
            foreach (int k in c.Keys)
                if (c[k] == value)
                    return k;
            return 0;
        }

        private void applyData()
        {
            rab.production = gp.Checked;
            rab.realization = gr.Checked;
            rab.defect = defect.Checked;
            rab.spec = spec.Checked;
            rab.rate = (int)rate.Value;
            rab.name = getCatValue(names, name.Text);
            rab.surname = getCatValue(surnames, surname.Text);
            rab.secname = getCatValue(secnames, secname.Text);
            rab.breed = getCatValue(breeds, breed.Text);
            rab.zone = getCatValue(zones, zone.Text);
            curzone = rab.zone;
            rab.born = bdate.Value.Date;
            rab.group = (int)group.Value;
            String gns = "";
            for (int i = 0; i < gens.Items.Count;i++ )
                gns += ((int)gens.Items[i]).ToString() + " ";
            rab.genom=gns.Trim();
            if (rab.sex == OneRabbit.RabbitSex.MALE)
            {
                rab.status = maleStatus.SelectedIndex;
                if (lastFuckNever.Checked)
                    rab.last_fuck_okrol = DateTime.MinValue;
                else
                    rab.last_fuck_okrol = lastFuck.Value;
            }
            if (rab.sex == OneRabbit.RabbitSex.FEMALE)
            {
            }
            rab.commit();
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
            if (!checkBox5.Checked && rab.group==1 && rab.sex!=OneRabbit.RabbitSex.VOID)
                name.Enabled = true;
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

        private void lastFuckNever_CheckedChanged(object sender, EventArgs e)
        {
            lastFuck.Enabled = !lastFuckNever.Checked;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lastFuckNever.Checked = false;
            lastFuck.Enabled = true;
            lastFuck.Value = DateTime.Today;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            lastFuckNever.Checked = false;
            lastFuck.Enabled = true;
            lastFuck.Value = DateTime.Today.Subtract(new TimeSpan(1,0,0,0));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (gens.SelectedIndex > -1)
                gens.Items.RemoveAt(gens.SelectedIndex);
        }

        private void addgen(int gen)
        {
            int pos = 0;
            for (int i = 0; i < gens.Items.Count; i++)
            {
                if ((int)gens.Items[i] == gen)
                    return;
                if ((int)gens.Items[i] < gen)
                    pos++;
            }
            gens.Items.Insert(pos, gen);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            addgen((int)numericUpDown3.Value);
        }

        private void zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            gens.Items.Remove(curzone);
            curzone = getCatValue(zones, zone.Text);
            if (curzone != 0)
                addgen(curzone);
        }
    }
}
