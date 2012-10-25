﻿using System;
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
        private int _rabId = 0;
        private Catalog breeds = null;
        private Catalog zones = null;
        private Catalog names = null;
        private Catalog surnames = null;
        private Catalog secnames = null;
        private RabNetEngRabbit _rab = null;
        /*private TabPage _tpMale;
        private TabPage _tpFemale;
        private TabPage _tpOkrol;
        private TabPage _tpSuckers;
        private TabPage _tpWeight;*/
        private int _curzone = 0;
        private int _mkbrides = 122;
        private int _mkcandidate = 120;
        //private int makesuck = 50; ваще не понятно зачем это тут надо
        private bool _can_commit;
        bool _manual = true;

        public RabbitInfo()
        {
            InitializeComponent();
            initialHints();            
            /*_tpMale = tabControl1.TabPages[1];
            _tpFemale = tabControl1.TabPages[2];
            _tpOkrol = tabControl1.TabPages[3];
            _tpSuckers = tabControl1.TabPages[4];
            _tpWeight = tabControl1.TabPages[5];
            while(tabControl1.TabPages.Count>1)
                tabControl1.TabPages.RemoveAt(1);*/
            _mkbrides = Engine.get().brideAge();
            _mkcandidate = Engine.opt().getIntOption(Options.OPT_ID.MAKE_CANDIDATE);
            //makesuck = Engine.opt().getIntOption(Options.OPT_ID.COUNT_SUCKERS);
            dateWeight.Value = DateTime.Now.Date;
        }

        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(btGens,"Показать генетику выделенного самца");
            toolTip.SetToolTip(btFuckHer,"Выбрать партнера для соития");
            toolTip.SetToolTip(overallBab,"Сколько родила живых крольчат вообщем");
            toolTip.SetToolTip(btBon,"Определение классности");
            toolTip.SetToolTip(group,"Количество кроликов в клетке");
            toolTip.SetToolTip(button16,"Показать все имена");
            toolTip.SetToolTip(button14,"Показать все породы");
            toolTip.SetToolTip(button15,"Показать все местности");
            toolTip.SetToolTip(btReplace,"Показать окно пересадок");
            toolTip.SetToolTip(button4,"Удалить выбранный номер гена");
            toolTip.SetToolTip(button3,"Добавить номер гена");
            toolTip.SetToolTip(checkBox5,"Изменить данные вручную");
            toolTip.SetToolTip(button9,"Принять окрол");
            toolTip.SetToolTip(gp,"Готовая продукция");
            //toolTip.SetToolTip(spec,"Привит ли кролик или группа");
            //toolTip.SetToolTip(dtp_vacEnd,"Дата окончания прививки");
            toolTip.SetToolTip(sex, "Пол одного или группы кроликов");
            toolTip.SetToolTip(rate, "Рейтинг кролика");
            toolTip.SetToolTip(btChangeWorker, "Изменить работника, который случал");
        }

        public RabbitInfo(int id)
            : this()
        {
            _rabId = id;
        }
        public RabbitInfo(RabNetEngRabbit r)
            : this()
        {
            _rabId = 0;
            _rab = r;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btAccept.PerformClick();
            if (_can_commit)
            {
                this.DialogResult = btOk.DialogResult;
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
            defect.Checked = _rab.Defect;
            gp.Checked = _rab.Production;
            cbRealization.Checked = _rab.Realization;
            rate.Value = _rab.Rate;
            group.Value = _rab.Group;
            lbName.Text = "Имя:" + name.Text;
            lbSecname.Text = "Ж.Фам:" + surname.Text;
            lbSurname.Text = "М.Фам:" + secname.Text;
            lbAddress.Text = "Адрес:" + _rab.Address;
            bdate.DateValue = _rab.Born.Date;
            notes.Text = _rab.Notes;
            String[] gns = _rab.Genom.Split(' ');
            gens.Items.Clear();
            foreach (string s in gns)
                if (s!="")
                    addgen(int.Parse(s));
            label11.Text = "Вес:" + getbon(_rab.Bon[1]);
            label12.Text = "Телосложение:" + getbon(_rab.Bon[2]);
            label18.Text = "Шкура:" + getbon(_rab.Bon[3]);
            label17.Text = "Окраска:" + getbon(_rab.Bon[4]);
            weightList.Items.Clear();
            String[] wgh = Engine.db().getWeights(_rab.RabID);
            for (int i = 0; i < wgh.Length / 2; i++)
                weightList.Items.Add(wgh[i * 2]).SubItems.Add(wgh[i*2+1]);
            //spec.Checked  = _rab.Spec;//+gambit
            riVaccinePanel1.SetRabbit(_rab);
            //dtp_vacEnd.Value = rab.VaccineEnd; //+gambit
        }

        private void updateMale()
        {
            setSex(1);        
            if (_rab.Status == 2) maleStatus.SelectedIndex = 2;
            else if (_rab.Status == 1 || _rab.Age >= _mkcandidate) maleStatus.SelectedIndex = 1;
            else maleStatus.SelectedIndex = 0;
            maleStatus.Enabled = groupBox4.Enabled = _rab.Age > _mkcandidate;
            //maleStatus.SelectedIndex = rab.status;
            //maleStatus.Enabled = rab.age > 100 && rab.name != 0;
            lbState.Text = "Статус: " + maleStatus.Text;
            if (_rab.Group != 1) return;
            tabControl1.TabPages.Remove(tpFemale);
            tabControl1.TabPages.Remove(tpYoungers);
            tabControl1.TabPages.Remove(tpFucks);
            /*tabControl1.TabPages.Add(_tpMale);
            tabControl1.TabPages.Add(_tpWeight);*/         
            lastFuckNever.Checked = _rab.Last_Fuck_Okrol == DateTime.MinValue;
            lastFuckNever_CheckedChanged(null, null);
            if (!lastFuckNever.Checked)
            {
                lastFuck.Value = _rab.Last_Fuck_Okrol;
            }
            double[] d = Engine.db().getMaleChildrenProd(_rab.RabID);
            maleKids.Text = String.Format("Количество крольчат: {0:f0}",d[0]);
            maleProd.Text = String.Format("Продуктивность соития: {0:f5}",d[1]);
        }

        /// <summary>
        /// Обновляет информацию для самки
        /// </summary>
        private void updateFemale()
        {
            tabControl1.TabPages.Remove(tpMale);
            setSex(2);
            lbState.Text = "Статус: Девочка";
            if (bdate.DaysValue >= _mkbrides)
                lbState.Text = "Статус: Невеста";
            if ((_rab.Status == 0 && _rab.EventDate != DateTime.MinValue )||(_rab.Status==1 && _rab.EventDate==DateTime.MinValue))
                lbState.Text = "Статус: Первокролка";
            if (_rab.Status > 1 || (_rab.Status==1 && _rab.EventDate!=DateTime.MinValue))
                lbState.Text = "Статус: Штатная";
            if (_rab.Status > 0)
            {
                button8.Text = btFuckHer.Text = "Вязать";
                if (_rab.Status>1) 
                    tpFucks.Text = "Вязки/Окролы";
            }            
            if (_rab.Group != 1) return;
            /*tabControl1.TabPages.Add(_tpOkrol);
            tabControl1.TabPages.Add(_tpFemale);           
            tabControl1.TabPages.Add(_tpSuckers);
            tabControl1.TabPages.Add(_tpWeight);*/
            nokuk.Checked = _rab.NoKuk;
            nolact.Checked = _rab.NoLact;
            okrolCount.Value = _rab.Status;
            if (_rab.Status < 1 || _rab.Last_Fuck_Okrol == DateTime.MinValue)
            {
                okrolDd.Enabled = false;
            }
            else
                okrolDd.DateValue = _rab.Last_Fuck_Okrol;
            sukr.Checked=(_rab.EventDate != DateTime.MinValue);
            sukrType.SelectedIndex = 0;
            button8.Enabled = button9.Enabled = button10.Enabled = false;
            if (sukr.Checked)
            {
                sukrType.SelectedIndex = _rab.EventType;
                sukrDd.DateValue = _rab.EventDate.Date;
                button9.Enabled = button10.Enabled=true;
            }
            else
                button8.Enabled = true;
            overallBab.Value = _rab.Babies;
            deadBab.Value = _rab.Lost;
            okrolCount.Value = _rab.Status;
            ///Заполнение списка случек
            fucks.Items.Clear();
            if (_rabId>0)
            foreach (Fucks.Fuck f in Engine.db().getFucks(_rab.RabID).fucks)
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
                li.SubItems.Add(f.breed == _rab.Breed ? "-" : "Да");
                li.SubItems.Add(RabNetEngHelper.inbreeding(f.rgenom, _rab.Genom) ? "Да" : "-");
                li.SubItems.Add(f.worker);
                li.Tag = f;
            }
            changeFucker.Enabled = false;
            riSuckersPanel1.Fill(_rabId,_rab);            
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
            FillList(breed,breeds,_rab.Breed);
            zones = cts.getZones();
            FillList(zone, zones, _rab.Zone);
            int sx=0;
            String end="";
            if (_rab.Sex==Rabbit.SexType.MALE)
                sx=1;
            if (_rab.Sex==Rabbit.SexType.FEMALE)
            {
                end="а";
                sx=2;
            }
            if (_rab.Group>1)
                end="ы";
            surnames = cts.getSurNames(2, end);
            secnames = cts.getSurNames(1, end);
            FillList(surname, surnames, _rab.Surname);
            FillList(secname, secnames, _rab.SecondName);
            fillNames(sx);
        }

        private void fillNames(int sx)
        {
            if (sx != 0 && _rab.Group == 1)
            {
                names = Engine.db().catalogs().getFreeNames(sx, _rab.WasName);
                FillList(name, names, _rab.Name);
                if (_rab.Name == 0)
                    name.Enabled = button16.Enabled=true;
            }
        }

        private void UpdateNew()
        {
            _manual = false;
            checkBox5.Checked = true;
            checkBox5.Enabled = false; 
            groupBox2.Enabled = true;
            button8.Enabled = false;
            groupBox5.Enabled = groupBox6.Enabled = true;
            _manual = true;
        }

        private void updateData()
        {
            if (_rab != null && _rab.RabID == 0)
            {
                btBon.Enabled = 
                btReplace.Enabled = false;
            }
            int idx = tabControl1.SelectedIndex;
            //while (tabControl1.TabPages.Count > 1)
                //tabControl1.TabPages.RemoveAt(1);
            if (_rabId!=0)
                _rab=Engine.get().getRabbit(_rabId);
            fillCatalogs(0);
            updateStd();
            if (_rab.Sex == Rabbit.SexType.VOID)
            {
                setSex(0);
                //label7.Text = "Статус:" + (rab.age < makesuck ? "Гнездовые" : "Подсосные");
                lbState.Text = "Статус: Бесполые";
            }
            if (_rab.Sex == Rabbit.SexType.MALE)
                updateMale();
            if (_rab.Sex == Rabbit.SexType.FEMALE)
                updateFemale();
            tabControl1.SelectedIndex = idx;
            sex.Enabled = (_rab.Name == 0 && _rab.Last_Fuck_Okrol == DateTime.MinValue);
            if (_rabId == 0)
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
            _rab.Production = gp.Checked;
            _rab.Defect = defect.Checked;
            _rab.Realization = cbRealization.Checked;
            _rab.Rate = (int)rate.Value;
            _rab.Name = getCatValue(names, name.Text);
            _rab.Surname = getCatValue(surnames, surname.Text);
            _rab.SecondName = getCatValue(secnames, secname.Text);
            _rab.Breed = getCatValue(breeds, breed.Text);
            _rab.Zone = getCatValue(zones, zone.Text);
            _curzone = _rab.Zone;
            _rab.Born = bdate.DateValue.Date;
            _rab.Group = (int)group.Value;
            _rab.Notes = notes.Text;
            String gns = "";
            for (int i = 0; i < gens.Items.Count;i++ )
                gns += ((int)gens.Items[i]).ToString() + " ";
            _rab.Genom=gns.Trim();
            if (_rab.Sex == Rabbit.SexType.MALE)
            {
                _rab.Status = maleStatus.SelectedIndex;
                if (lastFuckNever.Checked)
                    _rab.Last_Fuck_Okrol = DateTime.MinValue;
                else
                    _rab.Last_Fuck_Okrol = lastFuck.Value;
            }
            if (_rab.Sex == Rabbit.SexType.FEMALE)
            {
                _rab.Status = (int)okrolCount.Value;
                if (_rab.Status<1)
                    _rab.Last_Fuck_Okrol = DateTime.MinValue;
                else
                    _rab.Last_Fuck_Okrol = okrolDd.DateValue;
                _rab.NoKuk = nokuk.Checked;
                _rab.NoLact = nolact.Checked;
                _rab.Babies = (int)overallBab.Value;
                _rab.Lost = (int)deadBab.Value;
            }
            //rab.VaccineEnd = dtp_vacEnd.Value.Date;
            _rab.Commit();
        }

        private bool warnme()
        {
            String CHANGE_ERR = @"Вы пытаетесь изменить статичные данные.
Обычно их не нужно изменять вручную - они изменяются программно.
Изменить?";
            if (!_manual)
                return true;
            return MessageBox.Show(CHANGE_ERR, "Изменить данные?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (_manual)
            if (checkBox5.Checked)
                if (!warnme())
                {
                    checkBox5.Checked=false;
                    return;
                }
            name.Enabled=groupBox2.Enabled = checkBox5.Checked;
            if (_rab.Group > 1 || _rab.Sex==Rabbit.SexType.VOID)
                name.Enabled = false;
            if (!checkBox5.Checked && _rab.Group==1 && _rab.Sex!=Rabbit.SexType.VOID && _rab.Name==0)
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
                _can_commit = false;
                MessageBox.Show(this, ex, "Невозможно продолжить", MessageBoxButtons.OK, MessageBoxIcon.Warning);                
            }
            else
            {
                _can_commit = true;
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
            /*gens.Items.Remove(curzone);
            curzone = getCatValue(zones, zone.Text);
            if (curzone != 0)
                addgen(curzone);*/
        }

        private void fucks_SelectedIndexChanged(object sender, EventArgs e)
        {
            btChangeWorker.Enabled = changeFucker.Enabled = false;
            if (fucks.SelectedItems.Count == 1)
            {
                bool dead = (fucks.SelectedItems[0].SubItems[3].Text == RABDEAD);
                btGens.Enabled = true;
                btFuckHer.Enabled = !sukr.Checked && !dead;
                changeFucker.Enabled=fucks.SelectedItems[0].SubItems[3].Text=="сукрольна";
                btChangeWorker.Enabled = true;
            }
            else
                 btGens.Enabled = btFuckHer.Enabled = false;
        }

        private void btGens_Click(object sender, EventArgs e)
        {
            Fucks.Fuck f=fucks.SelectedItems[0].Tag as Fucks.Fuck;
            String nm=lbName.Text.Split(':')[1];
            if (lbSecname.Text!="")
                nm += " " + lbSecname.Text.Split(':')[1];
            if (lbSurname.Text!="")
                nm += "-" + lbSurname.Text.Split(':')[1];
            (new GenomView(_rab.Breed, f.breed, _rab.Genom, f.rgenom, nm, f.partner)).ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (_rab.RabID == 0) return;
            if((new BonForm(_rab.RabID)).ShowDialog() != DialogResult.Abort)
                btCancel.Enabled = false;
            updateData();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if((new Proholost(_rab.RabID)).ShowDialog() != DialogResult.Abort)
                btCancel.Enabled = false;
            updateData();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if((new OkrolForm(_rab.RabID)).ShowDialog() != DialogResult.Abort)
                btCancel.Enabled = false;
            updateData();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            (new MakeFuckForm(_rab.RabID)).ShowDialog();
            updateData();
        }

        private void btFuckHer_Click(object sender, EventArgs e)
        {
            Fucks.Fuck f = fucks.SelectedItems[0].Tag as Fucks.Fuck;
            (new MakeFuckForm(_rab.RabID,f.partnerid)).ShowDialog();
            updateData();
        }

        private void fucks_DoubleClick(object sender, EventArgs e)
        {
            if (fucks.SelectedItems.Count == 1)
                btGens.PerformClick();
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
                _manual = false;
                checkBox5_CheckedChanged(null, null);
                _manual = true;
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
            rpf.AddRabbit(_rab);
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
            Engine.db().addWeight(_rab.RabID, (int)nudWeight.Value, dateWeight.Value.Date);
            updateData();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (weightList.SelectedItems.Count != 1) return;
            DateTime dt = DateTime.Parse(weightList.SelectedItems[0].SubItems[0].Text);
            Engine.db().deleteWeight(_rab.RabID, dt.Date);
            updateData();
        }

        private void sex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_manual) return;
            if (MessageBox.Show(this, "Сменить пол?", "Смена пола", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Rabbit.SexType sx=Rabbit.SexType.VOID;
                if (sex.SelectedIndex==1) sx=Rabbit.SexType.MALE;
                if (sex.SelectedIndex==2) sx=Rabbit.SexType.FEMALE;
                _rab.SetSex(sx);
                updateData();
            }
            else
            {
                if (_rab.Sex == Rabbit.SexType.VOID) setSex(0);
                if (_rab.Sex == Rabbit.SexType.MALE) setSex(1);
                if (_rab.Sex == Rabbit.SexType.FEMALE) setSex(2);
            }
        }
        void setSex(int s)
        {
            _manual = false;
            sex.SelectedIndex = s;
            _manual = true;
        }

        private void changeFucker_Click(object sender, EventArgs e)
        {
            if (fucks.SelectedItems.Count!=1) return;
            Fucks.Fuck f = fucks.SelectedItems[0].Tag as Fucks.Fuck;
            MakeFuckForm mf = new MakeFuckForm(_rab.RabID, f.partnerid, 1);
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

        /*private void spec_CheckedChanged(object sender, EventArgs e)
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
        }*/

        /*private void dtp_vacEnd_CloseUp(object sender, EventArgs e)
        {
            if (dtp_vacEnd.Value.Date <= DateTime.Now.Date)
            {
                dtp_vacEnd.Enabled = spec.Checked = false;
                dtp_vacEnd.Value = DateTime.Now.Date;
            }
        }*/

        private void maleStatus_TextChanged(object sender, EventArgs e)
        { 
            if (_rab.Name == 0 && maleStatus.SelectedIndex == 2)
            {               
                MessageBox.Show("У Производителя должно быть имя");
                if (_rab.Status == 1 || _rab.Age >= _mkcandidate) maleStatus.SelectedIndex = 1;
                    else maleStatus.SelectedIndex = 0;
            }
        }

        private void miIsNotAProholost_Click(object sender, EventArgs e)
        {
            if (fucks.SelectedItems.Count == 1)
            {
                Fucks.Fuck f = (fucks.SelectedItems[0].Tag as Fucks.Fuck);
                if (!isLastEvent(f))
                {
                    MessageBox.Show("Отменить прохолостание можно лишь последней записи");
                    return;
                }
                if (MessageBox.Show("Данная функция отменит прохолост текущей крольчихи и восстановит сукрольность." + Environment.NewLine +
                    "Продолжить?", "Отмена прохолоста", MessageBoxButtons.YesNo) == DialogResult.No) return;
                Engine.db().cancelFuckEnd(f.id);
                this.updateData();
                //(new OkrolForm(this.rid)).ShowDialog();
            }
        }

        private bool isLastEvent(Fucks.Fuck f)
        {
            foreach (ListViewItem lvi in fucks.Items)
            {
                if ((lvi.Tag as Fucks.Fuck).id > f.id)
                    return false;
            }
            return true;
        }

        private void msFucks_Opening(object sender, CancelEventArgs e)
        {
            if (fucks.SelectedItems.Count != 1 || fucks.SelectedItems[0].SubItems[3].Text != Fucks.Type.Proholost_rus)          
                e.Cancel = true;                       
        }

    }
}
