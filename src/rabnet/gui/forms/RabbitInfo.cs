using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet.forms
{
    internal delegate void RIHandler();

    public partial class RabbitInfo : Form
    {        
        private int _rabId = 0;
        private Catalog _breeds = null;
        private Catalog _zones = null;
        private Catalog _names = null;
        private Catalog _surnames = null;
        private Catalog _secnames = null;
        private RabNetEngRabbit _rab = null;
        private int _curzone = 0;
        private int _mkbrides = 122;
        private int _mkcandidate = 120;
        private bool _can_commit;
        bool _manual = true;

        internal event WorkingHandler Working;

        public RabbitInfo()
        {
            InitializeComponent();
            initialHints();
            removeTabsForOneRabbit();
            _mkbrides = Engine.get().brideAge();
            _mkcandidate = Engine.opt().getIntOption(Options.OPT_ID.MAKE_CANDIDATE);
            //makesuck = Engine.opt().getIntOption(Options.OPT_ID.COUNT_SUCKERS);
            dateWeight.Value = DateTime.Now.Date;

            riFucksPanel1.UncanceledEvent+=new RIHandler(cancelUnable);
            riFucksPanel1.UpdateRequire += new RIHandler(updateData);
        }

        private void cancelUnable()
        {
            btCancel.Enabled = false;
        }

        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            
            toolTip.InitialDelay = 1000;
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

        private void updateData()
        {
            if (_rab != null && _rab.ID == 0)
            {
                btBon.Enabled =
                btReplace.Enabled = false;
            }
            int idx = tabControl1.SelectedIndex;
            //while (tabControl1.TabPages.Count > 1)
            //tabControl1.TabPages.RemoveAt(1);
            if (_rabId != 0)
                _rab = Engine.get().getRabbit(_rabId);
            fillCatalogs(0);
            updateStd();
            if (_rab.Sex == Rabbit.SexType.VOID)
            {
                setSex(0);
                //label7.Text = "Статус:" + (rab.age < makesuck ? "Гнездовые" : "Подсосные");
                lbState.Text = "Статус: Бесполые";
            }
            else if (_rab.Sex == Rabbit.SexType.MALE)
                updateMale();
            else if (_rab.Sex == Rabbit.SexType.FEMALE)
                updateFemale();
            tabControl1.SelectedIndex = idx;
            sex.Enabled = (_rab.NameID == 0 && _rab.LastFuckOkrol == DateTime.MinValue);
            if (_rabId == 0)
                UpdateNew();
        }

        private void updateStd()
        {
            defect.Checked = _rab.Defect;
            gp.Checked = _rab.Production;
            cbRealization.Checked = _rab.RealizeReady;
            rate.Value = _rab.Rate;
            group.Value = _rab.Group == 0 ? 1 : _rab.Group;//защита на всякий случай
            lbName.Text = "Имя:" + name.Text;
            lbSecname.Text = "Ж.Фам:" + surname.Text;
            lbSurname.Text = "М.Фам:" + secname.Text;
            lbAddress.Text = "Адрес:" + _rab.Address;
            bdate.DateValue = _rab.BirthDay.Date;
            notes.Text = _rab.Notes;
            String[] gns = _rab.Genoms.Split(' ');
            gens.Items.Clear();
            foreach (string s in gns)
                if (s!="")
                    addgen(int.Parse(s));
            label11.Text = "Вес:" + getbon(_rab.Bon[1]);
            label12.Text = "Телосложение:" + getbon(_rab.Bon[2]);
            label18.Text = "Шкура:" + getbon(_rab.Bon[3]);
            label17.Text = "Окраска:" + getbon(_rab.Bon[4]);
            weightList.Items.Clear();
            String[] wgh = Engine.db().getWeights(_rab.ID);
            for (int i = 0; i < wgh.Length / 2; i++)
                weightList.Items.Add(wgh[i * 2]).SubItems.Add(wgh[i*2+1]);
            riVaccinePanel1.SetRabbit(_rab);
        }

        private void updateMale()
        {
            setSex(1);
            _manual = false;
            if (_rab.Status == 2) 
                maleStatus.SelectedIndex = 2;
            else if (_rab.Status == 1 || _rab.Age >= _mkcandidate) 
                maleStatus.SelectedIndex = 1;
            else 
                maleStatus.SelectedIndex = 0;
            maleStatus.Enabled = groupBox4.Enabled = _rab.Age > _mkcandidate;
            lbState.Text = "Статус: " + maleStatus.Text;
            if (_rab.Group != 1) return;

            addMaleTabs();     
            lastFuckNever.Checked = _rab.LastFuckOkrol == DateTime.MinValue;
            lastFuckNever_CheckedChanged(null, null);
            if (!lastFuckNever.Checked)
            {
                lastFuck.Value = _rab.LastFuckOkrol;
            }
            double[] d = Engine.db().getMaleChildrenProd(_rab.ID);
            maleKids.Text = String.Format("Количество крольчат: {0:f0}",d[0]);
            maleProd.Text = String.Format("Продуктивность соития: {0:f5}",d[1]);
            _manual = true;
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
                button8.Text = "Вязать";
                if (_rab.Status>1) 
                    tpFucks.Text = "Вязки/Окролы";
            }            
            if (_rab.Group != 1) return;
            
            addFemaleTabs();
            nokuk.Checked = _rab.NoKuk;
            nolact.Checked = _rab.NoLact;
            okrolCount.Value = _rab.Status;
            if (_rab.Status < 1 || _rab.LastFuckOkrol == DateTime.MinValue)
            {
                okrolDd.Enabled = false;
            }
            else
                okrolDd.DateValue = _rab.LastFuckOkrol;
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
            overallBab.Value = _rab.KidsOverAll;
            deadBab.Value = _rab.KidsLost;
            okrolCount.Value = _rab.Status;            

            if (_rabId > 0)
            {
                //riSuckersPanel1.SetBreeds(_breeds);
                riSuckersPanel1.Fill(_rab);
                riFucksPanel1.SetRabbit(_rab);
            }
        }

        #region tabs_modify
        private void removeTabsForOneRabbit()
        {
            tabControl1.TabPages.Remove(tpMale);
            tabControl1.TabPages.Remove(tpFucks);
            tabControl1.TabPages.Remove(tpFemale);
            tabControl1.TabPages.Remove(tpYoungers);
            tabControl1.TabPages.Remove(tpWeight);
        }

        private void addMaleTabs()
        {
            if (!tabControl1.TabPages.Contains(tpMale))  
                tabControl1.TabPages.Insert(1,tpMale);
            if (!tabControl1.TabPages.Contains(tpWeight)) 
                tabControl1.TabPages.Insert(2,tpWeight); 
        }

        private void addFemaleTabs()
        {
            if (!tabControl1.TabPages.Contains(tpFemale))
                tabControl1.TabPages.Insert(1,tpFemale);
            if (!tabControl1.TabPages.Contains(tpFucks))
                tabControl1.TabPages.Insert(2,tpFucks);            
            if (!tabControl1.TabPages.Contains(tpYoungers))
                tabControl1.TabPages.Insert(3,tpYoungers);
            if (!tabControl1.TabPages.Contains(tpWeight))
                tabControl1.TabPages.Insert(4,tpWeight);           
        }
        #endregion tabs_modify

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
            _breeds = cts.getBreeds();
            FillList(breed,_breeds,_rab.BreedID);
            _zones = cts.getZones();
            FillList(zone, _zones, _rab.Zone);
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
            _surnames = cts.getSurNames(2, end);
            _secnames = cts.getSurNames(1, end);
            FillList(surname, _surnames, _rab.SurnameID);
            FillList(secname, _secnames, _rab.SecnameID);
            fillNames(sx);
        }

        private void fillNames(int sx)
        {
            if (sx != 0 && _rab.Group == 1)
            {
                _names = Engine.db().catalogs().getFreeNames(sx, _rab.WasNameID);
                FillList(name, _names, _rab.NameID);
                if (_rab.NameID == 0)
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
            _rab.RealizeReady = cbRealization.Checked;
            _rab.Rate = (int)rate.Value;
            _rab.NameID = getCatValue(_names, name.Text);
            _rab.SurnameID = getCatValue(_surnames, surname.Text);
            _rab.SecnameID = getCatValue(_secnames, secname.Text);
            _rab.BreedID = getCatValue(_breeds, breed.Text);
            _rab.BreedName = breed.Text;
            _rab.Zone = getCatValue(_zones, zone.Text);
            _curzone = _rab.Zone;
            _rab.BirthDay = bdate.DateValue.Date;
            _rab.Group = (int)group.Value;
            _rab.Notes = notes.Text;
            String gns = "";
            for (int i = 0; i < gens.Items.Count;i++ )
                gns += ((int)gens.Items[i]).ToString() + " ";
            _rab.Genoms=gns.Trim();
            if (_rab.Sex == Rabbit.SexType.MALE)
            {
                _rab.Status = maleStatus.SelectedIndex;
                if (lastFuckNever.Checked)
                    _rab.LastFuckOkrol = DateTime.MinValue;
                else
                    _rab.LastFuckOkrol = lastFuck.Value;
            }
            if (_rab.Sex == Rabbit.SexType.FEMALE)
            {
                _rab.Status = (int)okrolCount.Value;
                if (_rab.Status<1)
                    _rab.LastFuckOkrol = DateTime.MinValue;
                else
                    _rab.LastFuckOkrol = okrolDd.DateValue;
                _rab.NoKuk = nokuk.Checked;
                _rab.NoLact = nolact.Checked;
                _rab.KidsOverAll = (int)overallBab.Value;
                _rab.KidsLost = (int)deadBab.Value;
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
            if (!checkBox5.Checked && _rab.Group==1 && _rab.Sex!=Rabbit.SexType.VOID && _rab.NameID==0)
                name.Enabled = true;
            button16.Enabled = name.Enabled;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String ex = "";
            if (name.Text == "" && surname.Text == "" && secname.Text == "") ex = "У кролика нет имени!\n";
            //if (rab.address == OneRabbit.NullAddress) ex += "У кролика нет места жительства!\n";
            if (gens.Items.Count == 0)
            {
                int level=0;
                int rab_gen_depth = Engine.opt().getIntOption(Options.OPT_ID.RAB_GEN_DEPTH);
                RabbitGen.GetFullGenLevels(_rab.RabGenoms,ref level);
                if(rab_gen_depth>level)
                    ex += "У кролика нет ни одного Номера Гена!\n";
            }
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

        

        

        private void button13_Click(object sender, EventArgs e)
        {
            if (_rab.ID == 0) return;
            if((new BonForm(_rab.ID)).ShowDialog() != DialogResult.Abort)
                btCancel.Enabled = false;
            updateData();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if((new Proholost(_rab.ID)).ShowDialog() != DialogResult.Abort)
                btCancel.Enabled = false;
            updateData();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OkrolForm dlg = new OkrolForm(_rab.ID);
            if (dlg.ShowDialog() != DialogResult.Abort)
                btCancel.Enabled = false;
            updateData();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MakeFuckForm dlg = new MakeFuckForm(_rab.ID);
            if (dlg.ShowDialog() == DialogResult.OK)
                btCancel.Enabled = false;
            updateData();
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
            working();
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
            Engine.db().addWeight(_rab.ID, (int)nudWeight.Value, dateWeight.Value.Date);
            updateData();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (weightList.SelectedItems.Count != 1) return;
            DateTime dt = DateTime.Parse(weightList.SelectedItems[0].SubItems[0].Text);
            Engine.db().deleteWeight(_rab.ID, dt.Date);
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

        private void maleStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_manual) return;

            _manual = false;
            if (_rab.NameID == 0 && maleStatus.SelectedIndex == 2)
            {               
                MessageBox.Show("У Производителя должно быть имя");
                if (_rab.Status == 1 || _rab.Age >= _mkcandidate) maleStatus.SelectedIndex = 1;
                    else maleStatus.SelectedIndex = 0;
            }
            _manual = true;
        }

        private void working()
        {
            if (Working != null)
                Working();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            working();
        }

    }
}
