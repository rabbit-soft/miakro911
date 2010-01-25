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
        private int mkbrides = 122;
        bool manual = true;
        public RabbitInfo()
        {
            InitializeComponent();
            initialHints();            
            malePage = tabControl1.TabPages[1];
            femalePage = tabControl1.TabPages[2];
            okrolPage = tabControl1.TabPages[3];
            suckersPage = tabControl1.TabPages[4];
            weightPage = tabControl1.TabPages[5];
            while(tabControl1.TabPages.Count>1)
                tabControl1.TabPages.RemoveAt(1);
            mkbrides = Engine.get().brideAge();
        }

        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(button13, "Определение классности");
            toolTip.SetToolTip(group, "Количество кроликов в клетке");
            toolTip.SetToolTip(button16,"Показать все имена");
            toolTip.SetToolTip(button14,"Показать все породы");
            toolTip.SetToolTip(button15,"Показать все местности");
            toolTip.SetToolTip(button17,"Показать окно пересадок");
            toolTip.SetToolTip(button4,"Удалить выбранный номер гена");
            toolTip.SetToolTip(button3,"Добавить номер гена");
            toolTip.SetToolTip(checkBox5,"Изменить данные вручную");
            toolTip.SetToolTip(button9, "Принять окрол");
            toolTip.SetToolTip(gr, "Готов к реализации");
            toolTip.SetToolTip(gp, "Готовая продукция");
        }

        public RabbitInfo(int id)
            : this()
        {
            rid = id;
        }
        public RabbitInfo(RabNetEngRabbit r)
            : this()
        {
            rid = 0;
            rab = r;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button5.PerformClick();
            String ex = "";
            if (name.Text == "" && surname.Text == "" && secname.Text == "")
                ex = "У кролика нет имени!\n";
            if (rab.address == OneRabbit.NullAddress)
                ex += "У кролика нет места жительства!\n";
            if (ex != "")
            {
                ex += "Продолжить?";
                if (MessageBox.Show(this, ex, "Не введены данные", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Close();
            }
            else
            {
                this.DialogResult = button1.DialogResult;
                Close();
            }
            
        }

        private String getbon(char bon)
        {
            if (bon == '1') return "III";
            if (bon == '2') return "II";
            if (bon == '3') return "I";
            if (bon == '4') return "Элита";
            return "Нет";
        }

        private void updateStd()
        {
            defect.Checked = rab.defect;
            gp.Checked = rab.production;
            gr.Checked = rab.realization;
            spec.Checked = rab.spec;
            rate.Value = rab.rate;
            group.Value = rab.group;
            label5.Text = label22.Text="Адрес:" + rab.address;
            label2.Text = "Имя:" + name.Text;
            label3.Text = "Ж.Фам:" + surname.Text;
            label4.Text = "М.Фам:" + secname.Text;
            bdate.DateValue = rab.born.Date;
            notes.Text = rab.notes;
            String[] gns = rab.genom.Split(' ');
            gens.Items.Clear();
            foreach (string s in gns)
                if (s!="")
                    addgen(int.Parse(s));
            label11.Text = "Вес:" + getbon(rab.bon[1]);
            label12.Text = "Телосложение:" + getbon(rab.bon[2]);
            label18.Text = "Шкура:" + getbon(rab.bon[3]);
            label17.Text = "Окраска:" + getbon(rab.bon[4]);
        }

        private void updateMale()
        {
            maleStatus.SelectedIndex = rab.status;
            label7.Text = "Статус: " + maleStatus.Text;
            if (rab.group != 1) return;
            tabControl1.TabPages.Add(malePage);
            tabControl1.TabPages.Add(weightPage);           
            lastFuckNever.Checked = rab.last_fuck_okrol == DateTime.MinValue;
            lastFuckNever_CheckedChanged(null, null);
            if (!lastFuckNever.Checked)
            {
                lastFuck.Value = rab.last_fuck_okrol;
            }
        }

        private void updateFemale()
        {
            label7.Text = "Статус: Девочка";
            if (bdate.DaysValue >= mkbrides)
                label7.Text = "Статус: Невеста";
            if (rab.status == 1)
                label7.Text = "Статус: Первокролка";
            if (rab.status > 1)
                label7.Text = "Статус: Штатная";
            tabControl1.TabPages.Add(okrolPage);
            if (rab.group != 1) return;
            tabControl1.TabPages.Add(femalePage);           
            tabControl1.TabPages.Add(suckersPage);
            tabControl1.TabPages.Add(weightPage);
            nokuk.Checked = rab.nokuk;
            nolact.Checked = rab.nolact;
            okrolCount.Value = rab.status;
            if (rab.status<1)
            {
                okrolDd.Enabled = false;
            }
            else
                okrolDd.DateValue = rab.last_fuck_okrol;
            sukr.Checked=(rab.evdate != DateTime.MinValue);
            sukrType.SelectedIndex = 0;
            button8.Enabled = button9.Enabled = button10.Enabled = false;
            if (sukr.Checked)
            {
                sukrType.SelectedIndex = rab.evtype;
                sukrDd.DateValue = rab.evdate.Date;
                button9.Enabled = button10.Enabled=true;
            }
            else
                button8.Enabled = true;
            overallBab.Value = rab.babies;
            deadBab.Value = rab.lost;
            okrolCount.Value = rab.status;
            fucks.Items.Clear();
            if (rid>0)
            foreach (Fucks.Fuck f in Engine.db().getFucks(rab.rid).fucks)
            {
                ListViewItem li = fucks.Items.Add(f.when == DateTime.MinValue ? "" : f.when.ToShortDateString());
                li.SubItems.Add(f.type);
                li.SubItems.Add(f.partner);
                li.SubItems.Add(f.status);
                li.SubItems.Add(f.enddate == DateTime.MinValue ? "" : f.enddate.ToShortDateString());
                li.SubItems.Add(f.children.ToString());
                li.SubItems.Add(f.dead.ToString());
                li.SubItems.Add(f.killed.ToString());
                li.SubItems.Add(f.added.ToString());
                li.SubItems.Add(f.breed == rab.breed ? "-" : "Да");
                li.SubItems.Add(RabNetEngHelper.inbreeding(f.rgenom, rab.genom) ? "Да" : "-");
                li.Tag = f;
            }
            fucks.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            suckers.Items.Clear();
            if (rid>0)
            foreach (Younger y in Engine.db().getSuckers(rab.rid))
            {
                ListViewItem li=suckers.Items.Add(y.name());
                li.SubItems.Add(y.N());
                li.SubItems.Add(y.age().ToString());
                li.SubItems.Add(y.sex());
                li.SubItems.Add(y.breed());
            }
            suckers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void FillList(ComboBox cb,Catalog c,int key)
        {
            cb.Items.Clear();
            if (cb != breed)
            {
                cb.Items.Add("");
                cb.SelectedIndex = 0;
            }
            foreach (int k in c.Keys)
            {
                int id = cb.Items.Add(c[k]);
                if (key == k)
                    cb.SelectedIndex = id;
            }
            if (cb.SelectedIndex < 0) cb.SelectedIndex = 0;
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

        private void UpdateNew()
        {
            manual = false;
            checkBox5.Checked = true;
            checkBox5.Enabled = false; 
            groupBox2.Enabled = true;
            button8.Enabled = false;
            groupBox5.Enabled = groupBox6.Enabled = true;
            manual = true;
        }

        private void updateData()
        {
            int idx = tabControl1.SelectedIndex;
            while (tabControl1.TabPages.Count > 1)
                tabControl1.TabPages.RemoveAt(1);
            if (rid!=0)
                rab=Engine.get().getRabbit(rid);
            fillCatalogs(0);
            updateStd();
            if (rab.sex == OneRabbit.RabbitSex.VOID)
                label7.Text = "Статус:" + (rab.status == 1 ? "Гнездовые" : "Подсосные");
            if (rab.sex == OneRabbit.RabbitSex.MALE)
                updateMale();
            if (rab.sex == OneRabbit.RabbitSex.FEMALE)
                updateFemale();
            tabControl1.SelectedIndex = idx;
            if (rid == 0)
                UpdateNew();
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
            rab.born = bdate.DateValue.Date;
            rab.group = (int)group.Value;
            rab.notes = notes.Text;
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
                rab.status = (int)okrolCount.Value;
                if (rab.status<1)
                    rab.last_fuck_okrol = DateTime.MinValue;
                else
                    rab.last_fuck_okrol = okrolDd.DateValue;
                rab.nokuk = nokuk.Checked;
                rab.nolact = nolact.Checked;
                rab.babies = (int)overallBab.Value;
                rab.lost = (int)deadBab.Value;
            }
            rab.commit();
        }

        private bool warnme()
        {
            String CHANGE_ERR = @"Вы пытаетесь изменить статичные данные.
Обычно их не нужно изменять вручную - они изменяются программно.
Изменить?";
            if (!manual)
                return true;
            return MessageBox.Show(CHANGE_ERR, "Изменить данные?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (manual)
            if (checkBox5.Checked)
                if (!warnme())
                {
                    checkBox5.Checked=false;
                    return;
                }
            name.Enabled=groupBox2.Enabled = checkBox5.Checked;
            if (rab.group > 1 || rab.sex==OneRabbit.RabbitSex.VOID)
                name.Enabled = false;
            if (!checkBox5.Checked && rab.group==1 && rab.sex!=OneRabbit.RabbitSex.VOID && rab.name==0)
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

        private void fucks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fucks.SelectedItems.Count == 1)
            {
                button11.Enabled = true;
                button12.Enabled = !sukr.Checked;
            }
            else
                button11.Enabled = button12.Enabled = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Fucks.Fuck f=fucks.SelectedItems[0].Tag as Fucks.Fuck;
            String nm=label2.Text.Split(':')[1];
            if (label3.Text!="")
                nm += " " + label3.Text.Split(':')[1];
            if (label4.Text!="")
                nm += "-" + label4.Text.Split(':')[1];
            (new GenomView(rab.breed, f.breed, rab.genom, f.rgenom, nm, f.partner)).ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if((new BonForm(rab.rid)).ShowDialog() != DialogResult.Abort)
                button2.Enabled = false;
            updateData();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if((new Proholost(rab.rid)).ShowDialog() != DialogResult.Abort)
                button2.Enabled = false;
            updateData();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if((new OkrolForm(rab.rid)).ShowDialog() != DialogResult.Abort)
                button2.Enabled = false;
            updateData();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            (new MakeFuck(rab.rid)).ShowDialog();
            updateData();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Fucks.Fuck f = fucks.SelectedItems[0].Tag as Fucks.Fuck;
            (new MakeFuck(rab.rid,f.partnerid)).ShowDialog();
            updateData();
        }

        private void fucks_DoubleClick(object sender, EventArgs e)
        {
            if (fucks.SelectedItems.Count == 1)
                button11.PerformClick();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            (new CatalogForm(CatalogForm.CatalogType.BREEDS)).ShowDialog();
            fillCatalogs(0);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            (new CatalogForm(CatalogForm.CatalogType.ZONES)).ShowDialog();
            fillCatalogs(0);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            byte sex;
            if (tabControl1.TabPages[1].Text == "Самец") sex = 0; else sex = 1; 
            (new NamesForm(sex)).ShowDialog();
            fillCatalogs(0);
        }

        private void group_ValueChanged(object sender, EventArgs e)
        {
            if (group.Value == 1)
            {
                name.Enabled = false;
                fillCatalogs(0);
                manual = false;
                checkBox5_CheckedChanged(null, null);
                manual = true;
            }
            else
            {
                name.Enabled = false;
                name.Text = "";
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            ReplaceForm rpf = new ReplaceForm();
            rpf.addRabbit(rab);
            rpf.ShowDialog();
            updateData();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            groupBox6.Enabled = false;
            if (checkBox4.Checked)
                if (!warnme())
                {
                    checkBox4.Checked = false;
                    return;
                }
            groupBox6.Enabled = checkBox4.Checked;
        }

        private void okrolCount_ValueChanged(object sender, EventArgs e)
        {
            okrolDd.Enabled = (okrolCount.Value!=0);
        }

    }
}
