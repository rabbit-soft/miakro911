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
        const String RABDEAD = "Списан";
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
        private int mkcandidate = 120;
        private int makesuck = 50;
        private bool can_commit;
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
            mkcandidate = Engine.opt().getIntOption(Options.OPT_ID.MAKE_CANDIDATE);
            makesuck = Engine.opt().getIntOption(Options.OPT_ID.SUCKERS);
            dateWeight.Value = DateTime.Now.Date;
        }

        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(button11,"Показать генетику выделенного самца");
            toolTip.SetToolTip(button12,"Выбрать партнера для соития");
            toolTip.SetToolTip(overallBab,"Сколько родила живых крольчат вообщем");
            toolTip.SetToolTip(button13,"Определение классности");
            toolTip.SetToolTip(group,"Количество кроликов в клетке");
            toolTip.SetToolTip(button16,"Показать все имена");
            toolTip.SetToolTip(button14,"Показать все породы");
            toolTip.SetToolTip(button15,"Показать все местности");
            toolTip.SetToolTip(button17,"Показать окно пересадок");
            toolTip.SetToolTip(button4,"Удалить выбранный номер гена");
            toolTip.SetToolTip(button3,"Добавить номер гена");
            toolTip.SetToolTip(checkBox5,"Изменить данные вручную");
            toolTip.SetToolTip(button9,"Принять окрол");
            toolTip.SetToolTip(gp,"Готовая продукция");
            toolTip.SetToolTip(spec,"Привит ли кролик или группа");
            toolTip.SetToolTip(dtp_vacEnd,"Дата окончания прививки");
            toolTip.SetToolTip(sex, "Пол одного или группы кроликов");
            toolTip.SetToolTip(rate, "Рейтинг кролика");
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
            if (can_commit)
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
            defect.Checked = rab.Defect;
            gp.Checked = rab.Production;
            spec.Checked = rab.Spec;
            rate.Value = rab.Rate;
            group.Value = rab.Group;
            label2.Text = "Имя:" + name.Text;
            label3.Text = "Ж.Фам:" + surname.Text;
            label4.Text = "М.Фам:" + secname.Text;
            label5.Text = "Адрес:" + rab.Address;
            bdate.DateValue = rab.Born.Date;
            notes.Text = rab.Notes;
            String[] gns = rab.Genom.Split(' ');
            gens.Items.Clear();
            foreach (string s in gns)
                if (s!="")
                    addgen(int.Parse(s));
            label11.Text = "Вес:" + getbon(rab.Bon[1]);
            label12.Text = "Телосложение:" + getbon(rab.Bon[2]);
            label18.Text = "Шкура:" + getbon(rab.Bon[3]);
            label17.Text = "Окраска:" + getbon(rab.Bon[4]);
            weightList.Items.Clear();
            String[] wgh = Engine.db().getWeights(rab.RabID);
            for (int i = 0; i < wgh.Length / 2; i++)
                weightList.Items.Add(wgh[i * 2]).SubItems.Add(wgh[i*2+1]);
            spec.Checked = dtp_vacEnd.Enabled = rab.Spec;//+gambit
            dtp_vacEnd.Value = rab.VaccineEnd; //+gambit
        }

        private void updateMale()
        {
            setSex(1);        
            if (rab.Status == 2) maleStatus.SelectedIndex = 2;
                else if (rab.Status == 1 || rab.age >= mkcandidate) maleStatus.SelectedIndex = 1;
                else maleStatus.SelectedIndex = 0;
            maleStatus.Enabled = groupBox4.Enabled = rab.age > mkcandidate;
            //maleStatus.SelectedIndex = rab.status;
            //maleStatus.Enabled = rab.age > 100 && rab.name != 0;
            label7.Text = "Статус: " + maleStatus.Text;
            if (rab.Group != 1) return;
            tabControl1.TabPages.Add(malePage);
            tabControl1.TabPages.Add(weightPage);           
            lastFuckNever.Checked = rab.Last_Fuck_Okrol == DateTime.MinValue;
            lastFuckNever_CheckedChanged(null, null);
            if (!lastFuckNever.Checked)
            {
                lastFuck.Value = rab.Last_Fuck_Okrol;
            }
            double[] d = Engine.db().getMaleChildrenProd(rab.RabID);
            maleKids.Text = String.Format("Количество крольчат: {0:f0}",d[0]);
            maleProd.Text = String.Format("Продуктивность соития: {0:f5}",d[1]);
        }

        private void updateFemale()
        {
            setSex(2);
            label7.Text = "Статус: Девочка";
            if (bdate.DaysValue >= mkbrides)
                label7.Text = "Статус: Невеста";
            if ((rab.Status == 0 && rab.EventDate != DateTime.MinValue )||(rab.Status==1 && rab.EventDate==DateTime.MinValue))
                label7.Text = "Статус: Первокролка";
            if (rab.Status > 1 || (rab.Status==1 && rab.EventDate!=DateTime.MinValue))
                label7.Text = "Статус: Штатная";
            if (rab.Status > 0)
            {
                button8.Text = button12.Text = "Вязать";
                if (rab.Status>1)
                    okrolPage.Text = "Вязки/Окролы";
            }            
            if (rab.Group != 1) return;
            tabControl1.TabPages.Add(okrolPage);
            tabControl1.TabPages.Add(femalePage);           
            tabControl1.TabPages.Add(suckersPage);
            tabControl1.TabPages.Add(weightPage);
            nokuk.Checked = rab.NoKuk;
            nolact.Checked = rab.NoLact;
            okrolCount.Value = rab.Status;
            if (rab.Status<1)
            {
                okrolDd.Enabled = false;
            }
            else
                okrolDd.DateValue = rab.Last_Fuck_Okrol;
            sukr.Checked=(rab.EventDate != DateTime.MinValue);
            sukrType.SelectedIndex = 0;
            button8.Enabled = button9.Enabled = button10.Enabled = false;
            if (sukr.Checked)
            {
                sukrType.SelectedIndex = rab.EventType;
                sukrDd.DateValue = rab.EventDate.Date;
                button9.Enabled = button10.Enabled=true;
            }
            else
                button8.Enabled = true;
            overallBab.Value = rab.Babies;
            deadBab.Value = rab.Lost;
            okrolCount.Value = rab.Status;
            fucks.Items.Clear();
            if (rid>0)
            foreach (Fucks.Fuck f in Engine.db().getFucks(rab.RabID).fucks)
            {
                ListViewItem li = fucks.Items.Add(f.when == DateTime.MinValue ? "" : f.when.ToShortDateString());
                li.SubItems.Add(f.type);
                li.SubItems.Add(f.partner);
                if (f.isDead)
                    li.SubItems.Add(RABDEAD);
                else
                    li.SubItems.Add(f.status);
                li.SubItems.Add(f.enddate == DateTime.MinValue ? "" : f.enddate.ToShortDateString());
                li.SubItems.Add(f.children.ToString());
                li.SubItems.Add(f.dead.ToString());
                li.SubItems.Add(f.killed.ToString());
                li.SubItems.Add(f.added.ToString());
                li.SubItems.Add(f.breed == rab.Breed ? "-" : "Да");
                li.SubItems.Add(RabNetEngHelper.inbreeding(f.rgenom, rab.Genom) ? "Да" : "-");
                li.SubItems.Add(f.worker);
                li.Tag = f;
            }
            changeFucker.Enabled = false;
            fucks.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            suckers.Items.Clear();
            if (rid>0)
            foreach (Younger y in Engine.db().getSuckers(rab.RabID))
            {
                ListViewItem li=suckers.Items.Add(y.fname);
                li.SubItems.Add(y.fcount.ToString());
                li.SubItems.Add(y.fage.ToString());
                li.SubItems.Add(y.fsex);
                li.SubItems.Add(y.fbreed);
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
            FillList(breed,breeds,rab.Breed);
            zones = cts.getZones();
            FillList(zone, zones, rab.Zone);
            int sx=0;
            String end="";
            if (rab.Sex==OneRabbit.RabbitSex.MALE)
                sx=1;
            if (rab.Sex==OneRabbit.RabbitSex.FEMALE)
            {
                end="а";
                sx=2;
            }
            if (rab.Group>1)
                end="ы";
            surnames = cts.getSurNames(2, end);
            secnames = cts.getSurNames(1, end);
            FillList(surname, surnames, rab.Surname);
            FillList(secname, secnames, rab.SecondName);
            fillNames(sx);
        }

        private void fillNames(int sx)
        {
            if (sx != 0 && rab.Group == 1)
            {
                names = Engine.db().catalogs().getFreeNames(sx, rab.WasName);
                FillList(name, names, rab.Name);
                if (rab.Name == 0)
                    name.Enabled = button16.Enabled=true;
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
            if (rab.RabID == 0)
            {
                button13.Enabled = false;
            }
            int idx = tabControl1.SelectedIndex;
            while (tabControl1.TabPages.Count > 1)
                tabControl1.TabPages.RemoveAt(1);
            if (rid!=0)
                rab=Engine.get().getRabbit(rid);
            fillCatalogs(0);
            updateStd();
            if (rab.Sex == OneRabbit.RabbitSex.VOID)
            {
                setSex(0);
                label7.Text = "Статус:" + (rab.age < makesuck ? "Гнездовые" : "Подсосные");
            }
            if (rab.Sex == OneRabbit.RabbitSex.MALE)
                updateMale();
            if (rab.Sex == OneRabbit.RabbitSex.FEMALE)
                updateFemale();
            tabControl1.SelectedIndex = idx;
            sex.Enabled = (rab.Name == 0 && rab.Last_Fuck_Okrol == DateTime.MinValue);
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
            rab.Production = gp.Checked;
            rab.Defect = defect.Checked;
            rab.Spec = spec.Checked;
            rab.Rate = (int)rate.Value;
            rab.Name = getCatValue(names, name.Text);
            rab.Surname = getCatValue(surnames, surname.Text);
            rab.SecondName = getCatValue(secnames, secname.Text);
            rab.Breed = getCatValue(breeds, breed.Text);
            rab.Zone = getCatValue(zones, zone.Text);
            curzone = rab.Zone;
            rab.Born = bdate.DateValue.Date;
            rab.Group = (int)group.Value;
            rab.Notes = notes.Text;
            String gns = "";
            for (int i = 0; i < gens.Items.Count;i++ )
                gns += ((int)gens.Items[i]).ToString() + " ";
            rab.Genom=gns.Trim();
            if (rab.Sex == OneRabbit.RabbitSex.MALE)
            {
                rab.Status = maleStatus.SelectedIndex;
                if (lastFuckNever.Checked)
                    rab.Last_Fuck_Okrol = DateTime.MinValue;
                else
                    rab.Last_Fuck_Okrol = lastFuck.Value;
            }
            if (rab.Sex == OneRabbit.RabbitSex.FEMALE)
            {
                rab.Status = (int)okrolCount.Value;
                if (rab.Status<1)
                    rab.Last_Fuck_Okrol = DateTime.MinValue;
                else
                    rab.Last_Fuck_Okrol = okrolDd.DateValue;
                rab.NoKuk = nokuk.Checked;
                rab.NoLact = nolact.Checked;
                rab.Babies = (int)overallBab.Value;
                rab.Lost = (int)deadBab.Value;
            }
            rab.VaccineEnd = dtp_vacEnd.Value.Date;
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
            if (rab.Group > 1 || rab.Sex==OneRabbit.RabbitSex.VOID)
                name.Enabled = false;
            if (!checkBox5.Checked && rab.Group==1 && rab.Sex!=OneRabbit.RabbitSex.VOID && rab.Name==0)
                name.Enabled = true;
            button16.Enabled = name.Enabled;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String ex = "";
            if (name.Text == "" && surname.Text == "" && secname.Text == "") ex = "У кролика нет имени!\n";
            //if (rab.address == OneRabbit.NullAddress) ex += "У кролика нет места жительства!\n";
            if (gens.Items.Count == 0) ex += "У кролика нет ни одного Номера Гена!\n"; 
            if (ex != "")
            {
                can_commit = false;
                MessageBox.Show(this, ex, "Невозможно продолжить", MessageBoxButtons.OK, MessageBoxIcon.Warning);                
            }
            else
            {
                can_commit = true;
                applyData();
                updateData();
            }
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
            changeFucker.Enabled = false;
            if (fucks.SelectedItems.Count == 1)
            {
                bool dead = (fucks.SelectedItems[0].SubItems[3].Text == RABDEAD);
                button11.Enabled = true;
                button12.Enabled = !sukr.Checked && !dead;
                changeFucker.Enabled=fucks.SelectedItems[0].SubItems[3].Text=="сукрольна";
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
            (new GenomView(rab.Breed, f.breed, rab.Genom, f.rgenom, nm, f.partner)).ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (rab.RabID == 0) return;
            if((new BonForm(rab.RabID)).ShowDialog() != DialogResult.Abort)
                button2.Enabled = false;
            updateData();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if((new Proholost(rab.RabID)).ShowDialog() != DialogResult.Abort)
                button2.Enabled = false;
            updateData();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if((new OkrolForm(rab.RabID)).ShowDialog() != DialogResult.Abort)
                button2.Enabled = false;
            updateData();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            (new MakeFuck(rab.RabID)).ShowDialog();
            updateData();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Fucks.Fuck f = fucks.SelectedItems[0].Tag as Fucks.Fuck;
            (new MakeFuck(rab.RabID,f.partnerid)).ShowDialog();
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
            button16.Enabled = name.Enabled;
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button19.Enabled = weightList.SelectedItems.Count == 1;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem li in weightList.Items)
                if (DateTime.Parse(li.SubItems[0].Text).Date == dateWeight.Value.Date)
                {
                    MessageBox.Show("Кролик уже взсешен "+li.SubItems[0].Text);
                    return;
                }
            Engine.db().addWeight(rab.RabID, (int)nudWeight.Value, dateWeight.Value.Date);
            updateData();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (weightList.SelectedItems.Count != 1) return;
            DateTime dt = DateTime.Parse(weightList.SelectedItems[0].SubItems[0].Text);
            Engine.db().deleteWeight(rab.RabID, dt.Date);
            updateData();
        }

        private void sex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!manual) return;
            if (MessageBox.Show(this, "Сменить пол?", "Смена пола", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OneRabbit.RabbitSex sx=OneRabbit.RabbitSex.VOID;
                if (sex.SelectedIndex==1) sx=OneRabbit.RabbitSex.MALE;
                if (sex.SelectedIndex==2) sx=OneRabbit.RabbitSex.FEMALE;
                rab.setSex(sx);
                updateData();
            }
            else
            {
                if (rab.Sex == OneRabbit.RabbitSex.VOID) setSex(0);
                if (rab.Sex == OneRabbit.RabbitSex.MALE) setSex(1);
                if (rab.Sex == OneRabbit.RabbitSex.FEMALE) setSex(2);
            }
        }
        void setSex(int s)
        {
            manual = false;
            sex.SelectedIndex = s;
            manual = true;
        }

        private void changeFucker_Click(object sender, EventArgs e)
        {
            if (fucks.SelectedItems.Count!=1) return;
            Fucks.Fuck f = fucks.SelectedItems[0].Tag as Fucks.Fuck;
            MakeFuck mf = new MakeFuck(rab.RabID, f.partnerid, 1);
            if (mf.ShowDialog() == DialogResult.OK && mf.SelectedFucker!=f.id)
                Engine.db().changeFucker(f.id, mf.SelectedFucker);
            updateData();
        }

        private void changeWorker_Click(object sender, EventArgs e)
        {
            if (fucks.SelectedItems.Count != 1) return;
            Fucks.Fuck f = fucks.SelectedItems[0].Tag as Fucks.Fuck;
            SelectUserForm sf = new SelectUserForm(f.worker);
            if (sf.ShowDialog() == DialogResult.OK && sf.SelectedUser!=0 && sf.SelectedUserName!=f.worker)
                Engine.db().changeWorker(f.id, sf.SelectedUser);
            updateData();
        }

        private void spec_CheckedChanged(object sender, EventArgs e)
        {
            if (spec.Checked)
            {
                dtp_vacEnd.Enabled = true;
                dtp_vacEnd.Value = DateTime.Now.AddDays(Engine.opt().getIntOption(Options.OPT_ID.VACCINE_TIME));
            }
            else
            {
                dtp_vacEnd.Enabled = false;
                dtp_vacEnd.Value = DateTime.Now;
            }
        }

        private void dtp_vacEnd_CloseUp(object sender, EventArgs e)
        {
            if (dtp_vacEnd.Value.Date <= DateTime.Now.Date)
            {
                dtp_vacEnd.Enabled = spec.Checked = false;
                dtp_vacEnd.Value = DateTime.Now.Date;
            }
        }

        private void maleStatus_TextChanged(object sender, EventArgs e)
        { 
            if (rab.Name == 0 && maleStatus.SelectedIndex == 2)
            {               
                MessageBox.Show("У Производителя должно быть имя");
                if (rab.Status == 1 || rab.age >= mkcandidate) maleStatus.SelectedIndex = 1;
                    else maleStatus.SelectedIndex = 0;
            }
        }

    }
}
