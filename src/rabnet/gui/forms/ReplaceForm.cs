﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class ReplaceForm : Form
    {
        public enum Action { NONE, CHANGE, BOYSOUT, SET_NEST, ONE_GIRL_OUT }
        class RP
        {
            /// <summary>
            /// Переселяется ли мать вместе с детьми
            /// </summary>
            public bool WithChild = false;
            /// <summary>
            /// Уникальный номер в базе
            /// </summary>
            public int ID;
            public bool Saved = false;
            public string SName;
            /// <summary>
            /// ID кормилицы
            /// </summary>
            public int Parent = 0;
            /// <summary>
            /// ID новой кормилицы
            /// </summary>
            public int NewParent = 0;
            /// <summary>
            /// Количество кроликов в группе
            /// </summary>
            public int Count;
            /// <summary>
            /// Новое количество в группе
            /// </summary>
            public int NewCount;
            /// <summary>
            /// Клонирован из
            /// </summary>
            public RP MadeFrom;
            /// <summary>
            /// Новый пол кролика
            /// </summary>
            public Rabbit.SexType NewSex;
            /// <summary>
            /// Пол кролика
            /// </summary>
            public Rabbit.SexType Sex;
            public int Age = 0;
            /// <summary>
            /// Текущий адрес кролика
            /// </summary>
            public string Address;
            public string BreedName;
            /// <summary>
            /// Новый адрес кролика
            /// </summary>
            public string NewAddress = "";
            public string Action = "остается";
            /// <summary>
            /// Является ли группа молодняком
            /// </summary>
            public bool Younger = false;
            /// <summary>
            /// Может ли быть установлено гнездовье
            /// </summary>
            public bool CanHaveNest = false;
            /// <summary>
            /// Установить ли гнездовье
            /// </summary>
            public bool SetNest = false;
            public RP PlaceTo = null;
            public RP PlaceWith = null;
            public RPList List = null;
            public RPList Clones = new RPList();

            public RP(RPList list, int id, string name, string address, int count, Rabbit.SexType sex, int age, string breedName)
            {
                this.List = list;
                this.ID = id;
                SName = name;
                this.Address = address;
                this.Count = count;
                NewCount = count;
                this.Sex = sex;
                NewSex = sex;
                this.Age = age;
                this.CanHaveNest = canTierHaveNest(address);
                this.BreedName = breedName;
            }

            public static bool canTierHaveNest(string place)
            {
                if (place.Contains(BuildingType.DualFemale_Rus) || place.Contains(BuildingType.Female_Rus)) return true;
                if (place.Contains(BuildingType.Jurta_Rus))
                {
                    place = place.Remove(place.LastIndexOf(" ["));
                    if (place.Contains("а"))
                        return true;
                }
                return false;
            }

            public RP(RPList list, int id, string name, string address, int count, Rabbit.SexType sex, int age, string breedName, bool nest)
                : this(list, id, name, address, count, sex, age, breedName)
            {
                this.SetNest = nest;
            }

            public RP(RPList list, int id, string name, string address, int count, Rabbit.SexType sex, int age, string breedName, int parent)
                : this(list, id, name, address, count, sex, age, breedName)
            {
                Younger = true;
                this.Parent = parent;
                NewParent = parent;
            }
            public RP(RP fromrp, int count)
                : this(fromrp.List, 0, fromrp.Name, fromrp.CurAddress, count, fromrp.Sex, fromrp.Age, fromrp.BreedName, fromrp.Parent)
            {
                MadeFrom = fromrp;
                fromrp.NewCount -= count;
                NewSex = fromrp.NewSex;
                int idx = List.IndexOf(fromrp);
                List.Insert(idx + 1, this);
                fromrp.Clones.Add(this);
                Younger = false;
            }

            public void clear()
            {
                NewAddress = "";
                PlaceTo = null;
                PlaceWith = null;
                foreach (RP r in List)
                    if (r.Parent == ID)
                        r.Address = Address;
                while (Clones.Count > 0)
                {
                    RP f = Clones[0];
                    f.clear();
                    Clones.RemoveAt(0);
                    List.Remove(f);
                }
                NewCount = Count;
                NewSex = Sex;
                NewParent = Parent;
            }

            /// <summary>
            /// Устанавливает адрес куда пересадить.
            /// </summary>
            /// <param name="adr">Адрес</param>
            /// <param name="wchildren">Пересадить с детьми</param>
            public void SetCurAddress(string adr, bool wchildren)
            {
                WithChild = wchildren;
                CurAddress = adr;
                WithChild = false;
            }

            public string CurAddress
            {
                get { return NewAddress == "" ? Address : NewAddress; }
                set
                {
                    if (value == this.Address || (value == "" || value == "бомж"))
                        NewAddress = "";
                    else this.NewAddress = value;
                    if (this.ID == 0) return;

                    foreach (RP r in List)//далее ищет детей и присваивает их адресу - новый адрес матери.
                        if (r.Parent == this.ID && r != this)
                        {
                            if (r.CurAddress != this.Address) continue;//если детям назначили адрес раньше матери
                            String cur = r.Address;
                            r.Address = this.CurAddress;
                            if (!this.WithChild)
                                r.CurAddress = cur;
                        }
                }
            }

            public bool Replaced { get { return CurAddress != Address; } }
            public string Name { get { return (Younger ? " - " : "") + SName; } set { SName = value; } }
            public string Status
            {
                get
                {
                    string res = NewCount > 1 ? "остаются" : "остается";
                    if (CurAddress != Address)
                    {
                        if (Younger) res = "отсадка";
                        else res = "пересадка";
                        if (PlaceTo != null) res = "подсадка";
                    }
                    if (PlaceWith != null) res = ",объединение";
                    if (NewCount < Count) res += ",разбиение";
                    if (Sex != NewSex) res += ",пол-" + Rabbit.SexToRU(NewSex);
                    return res;
                }
            }

            public RP SplitGroup(int num)
            {
                if (ID == 0) return null;
                if (num < 1 || num >= NewCount) return null;
                return new RP(this, num);
            }
        }

        public const int FIELD_NAME = 0;
        public const int FIELD_BREED = 1;
        public const int FIELD_AGE = 2;
        public const int FIELD_SEX = 3;
        public const int FIELD_COUNT = 4;
        public const int FIELD_OLDPLACE = 5;
        /// <summary>
        /// Номер колонки, в которой содержится адрес, по которому переселят кролика
        /// (номер считается от 0)
        /// </summary>
        public const int FIELD_NEWPLACE = 6;
        public const int FIELD_STATE = 7;
        /// <summary>
        /// Номер колонки, в которой содержится чекбокс с установкой гнездовья
        /// </summary>
        public const int FIELD_NEST = 8;
        class RPList : List<RP> { }
      
        private bool _manual = false;
        private int girlout = 0;
        private List<RabNetEngRabbit> rbs = new List<RabNetEngRabbit>();
        private RPList _replaceList = new RPList();
        private DataTable _dataSet = new DataTable();
        /// <summary>
        /// Комбобокс с новыми адресами, который добавляется к новой строке
        /// </summary>
        private DataGridViewComboBoxColumn dgcbNewAddress = new DataGridViewComboBoxColumn();        
        private Building[] bs = null;       
        private Action _action = Action.NONE;
        private bool globalError=false;
        private bool noboys = false;
        private int combineage = 0;

        public ReplaceForm()
        {
            InitializeComponent();
            initialHints();
            _dataSet.Columns.Add("Имя", typeof(string));
            _dataSet.Columns.Add("Порода", typeof(string));
            _dataSet.Columns.Add("Возраст", typeof(int));
            _dataSet.Columns.Add("Пол", typeof(string));
            _dataSet.Columns.Add("Количество", typeof(int));
            _dataSet.Columns.Add("Старый адрес", typeof(string));
            _dataSet.Columns.Add("Новый адрес", typeof(string));//FIELD_PLACE
            _dataSet.Columns.Add("Статус", typeof(string));
            _dataSet.Columns.Add("Гнездовье", typeof(bool));    //FIELD_NEST           
            dataGridView1.DataSource = _dataSet;
            dataGridView1.Columns[FIELD_NAME].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            cbFilter.Tag = 1;
            cbFilter.SelectedIndex = 0;
            cbFilter.Tag = 0;
            combineage = Engine.get().options().getIntOption(Options.OPT_ID.COMBINE_AGE);
            FormSizeSaver.Append(this);
            _manual = true;
        }

        /// <summary>
        /// Добавляет в форму пересадок кролика с указанным Id.
        /// </summary>
        /// <param name="id"></param>
        public void AddRabbit(int id)
        {
            AddRabbit(Engine.get().getRabbit(id));
        }
        /// <summary>
        /// Добавление нового кролика в список, если есть подсосные то и их
        /// </summary>
        /// <param name="r">Объект символизирующий кролика или группу</param>
        public void AddRabbit(RabNetEngRabbit r)
        {
            rbs.Add(r);
            clear();
        }
        /// <summary>
        /// Обновление списка кроликов в RPList
        /// </summary>
        public void clear()
        {
            _replaceList.Clear();
            foreach (RabNetEngRabbit r in rbs)
            {
                _replaceList.Add(new RP(_replaceList, r.RabID, r.FullName, r.medAddress, r.Group, r.Sex, r.Age, r.BreedName, r.SetNest));
                //Engine.get().db().getBuilding(r.Address);
                foreach (YoungRabbit y in r.Youngers)
                    _replaceList.Add(new RP(_replaceList, y.ID, y.NameFull, r.medAddress, y.Group, y.Sex, DateTime.Now.Subtract(y.BirthDay).Days, r.BreedName, r.RabID));
            }
        }
        public void setAction(Action act)
        {
            _action = act;
        }

        public bool myrab(int i)
        {
            foreach (RabNetEngRabbit r in rbs)
                if (r.RabID == i)return true;
            return false;
        }

        private bool busy(RP id)
        {
            bool res = false;
            if (id.CurAddress == Rabbit.NULL_ADDRESS) return false;
            foreach (RP r in _replaceList)
            {
                if (r.CurAddress == id.CurAddress && r != id && !res)
                {
                    if (!(id.Younger && id.Parent == r.ID) && !(r.Younger && r.Parent == id.ID))
                        res = true;
                    if (res)
                    {
                        if (r.PlaceWith == id || id.PlaceWith == r) res = false;
                        if (r.PlaceTo == id || id.PlaceTo == r) res = false;
                        if (r.PlaceTo != null && (id.Younger && r.PlaceTo.ID == id.Parent))
                            res = false;
                        if (id.PlaceTo != null && (r.Younger && id.PlaceTo.ID == r.Parent))
                            res = false;
                        if (r.Parent == id.Parent && r.Parent != 0)
                            res = false;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Заполняет ComboBox с новыми адресами
        /// </summary>
        public void getBuildings()
        {
            Filters f = new Filters();
            f["rcnt"] = rbs.Count.ToString();
            for (int i = 0; i < rbs.Count; i++) 
                f["r" + i.ToString()] = rbs[i].RabID.ToString();
            String tp = "";
            cbFilter.Tag = 1;
            //if (_action == Action.SET_NEST && cbFilter.SelectedIndex != 4 && cbFilter.SelectedIndex != 2 && cbFilter.SelectedIndex !=1)
            //    cbFilter.SelectedIndex = 4;
            cbFilter.Tag = 0;
            if (_action != Action.SET_NEST)
            {
                if (cbFilter.SelectedIndex != 0)//если не выбрано Все
                {
                    switch (cbFilter.SelectedIndex)
                    {
                        case 1: tp = BuildingType.Female; break;
                        case 2: tp = BuildingType.DualFemale; break;
                        case 3: tp = BuildingType.Complex; break;
                        case 4: tp = BuildingType.Jurta; break;
                        case 5: tp = BuildingType.Quarta; break;
                        case 6: tp = BuildingType.Vertep; break;
                        case 7: tp = BuildingType.Barin; break;
                        case 8: tp = BuildingType.Cabin; break;
                    }
                    f["tp"] = tp;
                }
            }
            else
            {
                f["nest"] = "1";
            }
            bs = Engine.db().getFreeBuilding(f);
            dgcbNewAddress.Items.Clear();
            dgcbNewAddress.Items.Add(Rabbit.NULL_ADDRESS);
            foreach (Building b in bs)
            {
                for (int i = 0; i < b.secs(); i++)
                {
                    if (_action == Action.SET_NEST && b.ftype == BuildingType.Jurta && i == 1)
                        continue;
                    if (b.busy(i)==0 || myrab(b.busy(i))) 
                        dgcbNewAddress.Items.Add(b.medname[i]);
                }
            }
            foreach (RP r in _replaceList)
            {
                if (!dgcbNewAddress.Items.Contains(r.CurAddress))
                    dgcbNewAddress.Items.Add(r.CurAddress);
            }
        }

        public void updateB() { update(true); }
        public void update() { update(false); }

        /// <summary>
        /// Обновляет информацию dataGrid
        /// </summary>
        /// <param name="reget">Заполнять ли данными DataGridView</param>
        public void update(bool reget)
        {
            int selectedRow = -1;
            if (dataGridView1.SelectedRows.Count != 0)
                selectedRow = dataGridView1.SelectedRows[0].Index; 
            globalError = false;
            _dataSet.Rows.Clear();
            if (reget)
            {
                getBuildings();
                dataGridView1.Columns.RemoveAt(FIELD_NEWPLACE);
                dgcbNewAddress.MaxDropDownItems = 20;
                dgcbNewAddress.HeaderText = "Новый адрес";
                dgcbNewAddress.DataPropertyName = "Новый адрес";
                dgcbNewAddress.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns.Insert(FIELD_NEWPLACE, dgcbNewAddress);
            }
            foreach (RP r in _replaceList)
            {
                string stat = r.Status;
                if (r.PlaceTo != null)
                    r.CurAddress = r.PlaceTo.CurAddress;
                if (r.PlaceWith != null) 
                    r.CurAddress = r.PlaceWith.CurAddress;
                bool b = busy(r);
                if (b)
                {
                    globalError = true;
                    stat += ",ЗАНЯТО";
                }
                DataRow rw = _dataSet.Rows.Add(r.Name,r.BreedName, r.Age, Rabbit.SexToRU(r.NewSex), r.NewCount, r.Address, r.CurAddress, stat, r.SetNest);
                if (_action == Action.CHANGE && dataGridView1.SelectedRows.Count < 2 && r.Parent==0)
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                if (_action == Action.BOYSOUT)
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = r.Younger;
                }
                if (b)
                    rw.RowError = "занято";
            }
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            if (_action == Action.CHANGE)
            {
                _action = Action.NONE;
                if (dataGridView1.SelectedRows.Count == 2)
                    btChangeAddresses.PerformClick();
            }
            if (_action == Action.BOYSOUT)
            {
//                action = Action.NONE;
                btSeparate.Enabled = btChangeAddresses.Enabled = btSeparateByOne.Enabled=noboys;
            }
            if (selectedRow != -1 && selectedRow <= dataGridView1.Rows.Count-1)            
                dataGridView1.CurrentCell = dataGridView1[0, selectedRow];
            
        }

        private RP rp(int idx)
        {
            return _replaceList[idx];
        }

        private RP rp()
        {
            return rp(dataGridView1.SelectedRows[0].Index);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((int)cbFilter.Tag == 1)
                return;
            updateB();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex != FIELD_NEST ||(e.ColumnIndex == FIELD_NEST && !canHaveNest(e.RowIndex)))
                e.Cancel = (e.ColumnIndex != FIELD_NEWPLACE);
        }
        private void ReplaceForm_Load(object sender, EventArgs e)
        {
            _manual = false;
            if (_action == Action.SET_NEST)
            {
                cbFilter.Enabled = false;
                this.Text += " - Установка гнездовья";
            }
            updateB();
            //loadColumnsWidths();
            _manual = true;
        }

        /// <summary>
        /// Вызывается при окончании редактирования ячейки с адресом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex != FIELD_NEWPLACE && e.ColumnIndex != FIELD_NEST) || _dataSet.Rows.Count == 0)
                return;
            DataRow rw = _dataSet.Rows[e.RowIndex];
            RP r = _replaceList[e.RowIndex];
            if (e.ColumnIndex == FIELD_NEWPLACE) 
                r.CurAddress = (string)rw[FIELD_NEWPLACE];
            if (e.ColumnIndex == FIELD_NEWPLACE && r.Address == r.CurAddress)
                return;
            r.CanHaveNest = canHaveNest(e.RowIndex);
            if (!r.CanHaveNest)
                r.SetNest = false;
            else r.SetNest = (bool)rw[FIELD_NEST];
            update();
        }

        /// <summary>
        /// Можно ли установить гнездовье
        /// </summary>
        /// <param name="rowIndex">Номер строки</param>
        private bool canHaveNest(int rowIndex)
        {
            string place = dataGridView1.Rows[rowIndex].Cells[FIELD_NEWPLACE].Value.ToString();
            return RP.canTierHaveNest(place);
        }

        private void btClearAll_Click(object sender, EventArgs e)
        {
            clear();
            btSeparate.Enabled = btSeparateByOne.Enabled = (_action != Action.BOYSOUT);
            noboys = false;
            update();
        }

        private void btChangeAddresses_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 2)
            {
                MessageBox.Show("Выберите две строки");
                return;
            }
            RP r1 = rp(dataGridView1.SelectedRows[0].Index);
            RP r2 = rp(dataGridView1.SelectedRows[1].Index);
            string ca = r1.CurAddress;
            r1.SetCurAddress(r2.CurAddress,true);
            r2.SetCurAddress(ca,true);
            update();
        }

        private void dataGridView1_MultiSelectChanged(object sender, EventArgs e)
        {
            MainForm.StillWorking();
            btChangeAddresses.Enabled = (dataGridView1.SelectedRows.Count == 2 && _action!=Action.ONE_GIRL_OUT);
            btClear.Enabled = (dataGridView1.SelectedRows.Count >0);
            groupBox1.Enabled = groupBox2.Enabled = false;
            if (dataGridView1.SelectedRows.Count == 2 && _action!=Action.ONE_GIRL_OUT)
            {
                groupBox2.Enabled = true;
                btCombine.Enabled = btUniteUp.Enabled = btUniteDown.Enabled=false;
                RP r1 = rp(dataGridView1.SelectedRows[0].Index);
                RP r2 = rp(dataGridView1.SelectedRows[1].Index);
                if ((r1.Count == 1 && r1.Sex == Rabbit.SexType.FEMALE && r1.Age>r2.Age) || (r2.Count == 1 && r2.Sex == Rabbit.SexType.FEMALE && r2.Age>r1.Age))
                {
                    btCombine.Enabled = true;
                }
                if (r1.Sex == r2.Sex && Math.Abs(r1.Age-r2.Age)<=combineage)
                {
                    btUniteUp.Enabled = btUniteDown.Enabled=true;
                }
                btUniteUp.Enabled = btUniteDown.Enabled = true;//(r1.ID != 0 && r2.ID != 0);
            }
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int cnt = rp(dataGridView1.SelectedRows[0].Index).NewCount;
                groupBox1.Enabled = (cnt > 1);
                btSeparateBoys.Enabled = btSetAllGirls.Enabled = btSetAllBoys.Enabled = (rp().NewSex == Rabbit.SexType.VOID);
                if (cnt > 1)           
                    numericUpDown1.Maximum = cnt - 1;
                numericUpDown1.Enabled = btSeparate.Enabled = btSeparateBoys.Enabled = btSeparateByOne.Enabled = (cnt > 1) ;                
                groupBox1.Enabled = (rp().ID != 0);
                if (_action == Action.ONE_GIRL_OUT)
                {
                    numericUpDown1.Value = numericUpDown1.Maximum = numericUpDown1.Minimum = 1;
                    btSeparateBoys.Enabled = btSetAllGirls.Enabled = btSetAllBoys.Enabled = btChangeAddresses.Enabled = false;
                    groupBox2.Enabled = false;
                }
            }
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.SelectedRows.Count;i++)
                rp(dataGridView1.SelectedRows[i].Index).clear();
            btSeparate.Enabled=btSeparateByOne.Enabled=(_action!=Action.BOYSOUT);
            noboys = false;
            update();
        }

        private void btSeparate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            rp().SplitGroup((int)numericUpDown1.Value);
            update();
        }
        /// <summary>
        /// Преобразует строку адреса типа  555а[Кварта] в масссив цифр
        /// </summary>
        /// <param name="s">Название клетки</param>
        /// <returns>Массив (Номер фермы, Ярус, Клетка)</returns>
        private int[] getAddress(String s)
        {
            if (s == Rabbit.NULL_ADDRESS)
                return new int[] { 0, 0, 0 };
            for (int i = 0; i < bs.Length; i++)
                for (int j = 0; j < bs[i].secs();j++ )
                    if (bs[i].medname[j] == s)
                    {
                        return new int[]{(int)bs[i].farm(),bs[i].tier_id(),j};
                    }
            return null;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException=false;
            dataGridView1.Rows[e.RowIndex].ErrorText = e.ColumnIndex.ToString()+":"+e.Exception.Message;
        }

        private void btSeparateByOne_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            for (int i = 0; i < rp().Count-1;i++)
                rp().SplitGroup(1);
            update();

        }

        private void btSeparateBoys_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            RP nrp = rp().SplitGroup((int)numericUpDown1.Value);
            rp().NewSex = Rabbit.SexType.FEMALE;
            nrp.NewSex = Rabbit.SexType.MALE;
            noboys = true;
            btSeparate.Enabled = btSeparateByOne.Enabled = true;
            update();
        }
        
        /// <summary>
        /// Сохраняет операции, по пересадке кролика
        /// warning full vachanaliation and analizing troubles
        /// </summary>
        /// <param name="r">Строка кролика</param>
        /// <param name="id"></param>
        /// <param name="allowReplace">Уже перемещен</param>
        private void commitRabbit(RP rp,int id,bool allowReplace)
        {
            if (rp.Saved) return;

            if (rp.ID == 0)
            {
                int[] a = getAddress(rp.CurAddress);
                rbs[_replaceList.IndexOf(rp)].ReplaceRabbit(a[0], a[1], a[2], rp.CurAddress);
                rp.Saved = true;
                return;
            }
            RabNetEngRabbit rb = Engine.get().getRabbit(id == 0 ? rp.ID : id);
            RabNetEngRabbit par = null;

            //if (rp.Clones.Count > 0)
                //foreach (RP c in rp.Clones)
            while (rp.Clones.Count > 0)
            {
                RP c = rp.Clones[0];
                int[] a = getAddress(c.CurAddress);
                int cid = rb.Clone(c.Count, a[0], a[1], a[2]);
                c.ID = cid;
                commitRabbit(c, cid, allowReplace);
                if (_action == Action.ONE_GIRL_OUT && girlout == 0 && c.Sex == Rabbit.SexType.FEMALE && c.Count == 1)
                    girlout = cid;
                rp.Clones.RemoveAt(0);
            }

            if (rp.Younger)
                par = Engine.get().getRabbit(rp.Parent);
            if (rp.PlaceTo != null || rp.PlaceWith != null)
            {
                if (!allowReplace) return;

                if (rp.PlaceWith != null)
                {
                    rb.CombineWidth(rp.PlaceWith.ID);
                    rp.Saved = true;
                    return;
                }
                if (rp.PlaceTo != null)
                {
                    rb.PlaceSuckerTo(rp.PlaceTo.ID);
                }
            }

            if (rp.Replaced && !allowReplace)
            {
                int[] a = getAddress(rp.CurAddress);
                if (rp.Younger)
                    par.ReplaceYounger(rb.RabID, a[0], a[1], a[2], rp.CurAddress);
                else
                    rb.ReplaceRabbit(a[0], a[1], a[2], rp.CurAddress);               
            }

            if (rp.CanHaveNest)
            {
                if (!rp.Younger || (rp.Younger && rp.Replaced))
                {
                    RabNetEngRabbit rr = Engine.get().getRabbit(rb.RabID);
                    RabNetEngBuilding rbe = Engine.get().getBuilding(rr.JustAddress);
                    string[] vals = rr.JustAddress.Split(',');
                    if (vals[3] == BuildingType.Jurta || vals[3] == BuildingType.Female)
                        rbe.setNest(rp.SetNest);
                    else if (vals[3] == BuildingType.DualFemale)
                    {                        
                        if (vals[2] == "0")
                            rbe.setNest(rp.SetNest);
                        else if (vals[2] == "1")
                            rbe.setNest2(rp.SetNest);
                    }
                }
            }

            if (rp.NewSex != rp.Sex)
                rb.SetSex(rp.NewSex);
            //if (rp.Clones.Count > 0)
            //    foreach (RP c in rp.Clones)
            //    {
            //        int[] a=getAddress(c.CurAddress);
            //        int cid = rb.Clone(c.Count, a[0],a[1],a[2]);
            //        c.ID = cid;
            //        commitRabbit(c, cid,allowReplace);
            //        if (_action == Action.ONE_GIRL_OUT && girlout == 0 && c.Sex == Rabbit.SexType.FEMALE && c.Count == 1)
            //            girlout = cid;
            //    }
            rp.Saved = true;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (Engine.opt().getIntOption(Options.OPT_ID.CONFIRM_REPLACE) == 1)
            {
                if (MessageBox.Show("Пересадить?", "", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            if (_action == Action.SET_NEST && !chechOnNest())
            {
                this.DialogResult = DialogResult.None;
                return;
            }
            try
            {
                if (globalError)
                    throw new ApplicationException("Не всем кроликам назначены уникальные клетки для пересадки.");
                foreach (RP r in _replaceList)
                    commitRabbit(r,0,false);
                foreach (RP r in _replaceList)
                    commitRabbit(r, 0, true);
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message,"Нельзя продолжить",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
            }
        }
        /// <summary>
        /// Проверяет установлена ли галочка на гнездовье
        /// </summary>
        /// <returns>Продолжать ли выполнение</returns>
        private bool chechOnNest()
        {
            bool error = false;
            foreach (RP rp in this._replaceList)
            {
                if (!rp.CanHaveNest && !rp.SetNest)
                {
                    error = true;
                    break;
                }

            }
            if (error)
            {
                DialogResult dr = MessageBox.Show("Не во всех клетках установлено гнездовье"+Environment.NewLine+"Продолжить выполнение?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes) return true;
                else return false;
            }
            else return true;
        }

        private void btSetAllGirls_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            rp().NewSex = Rabbit.SexType.FEMALE;
            update();
        }

        private void btAutoReplace_Click(object sender, EventArgs e)
        {
                for (int j = _replaceList.Count - 1; j > 0;j-- )
                {
                    RP r = _replaceList[j];
                    if (busy(r))
                    {
                        for (int i = 1; i < dgcbNewAddress.Items.Count && busy(r); i++)
                            if ((string)dgcbNewAddress.Items[i] != r.Address)
                                r.CurAddress = (string)dgcbNewAddress.Items[i];
                    }
                }
            update();
        }

        private void btCombine_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 2) return;
            RP mother = rp(dataGridView1.SelectedRows[0].Index);
            RP child = rp(dataGridView1.SelectedRows[1].Index);
            if (child.Age > mother.Age)
            {
                RP ex = mother; mother = child; child = ex;
            }
            child.PlaceTo = mother;
            child.CurAddress = mother.CurAddress;
            update();
        }

        private void btUniteUp_Click(object sender, EventArgs e)
        {
            не должно собираться тут еще не int правильно все работает
            if (dataGridView1.SelectedRows.Count != 2) return;
            RP rWith = rp(dataGridView1.SelectedRows[(sender == btUniteUp ? 1 : 0)].Index); //BUG selectedIndex меньше у того, кого раньше выделили, он не привязан к позиции в DGLV
            RP r2 = rp(dataGridView1.SelectedRows[(sender == btUniteUp ? 0 : 1)].Index);
            r2.PlaceWith = rWith;
            rWith.PlaceWith = null;
            r2.CurAddress = rWith.CurAddress;
            update();
        }

        public int getGirlOut()
        {
            return girlout;
        }

        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(btOK, "Выполнить план пересадок");
            toolTip.SetToolTip(btCancel, "Закрыть окно.Не применять пересадки");
            toolTip.SetToolTip(btClearAll, "Отменить изменения для всех записей");
            toolTip.SetToolTip(btChangeAddresses, "Поменять адресами 2 выбранные записи");
            toolTip.SetToolTip(btClear, "Отменить изменения для выделенной записи");
            toolTip.SetToolTip(btSeparateByOne, "Разбить выделенную группу по одному");
            toolTip.SetToolTip(btSeparate, "Отделить от выбранной группы N-количество");
            toolTip.SetToolTip(btSeparateBoys, "Отделить из выбранной группы безполых, N-количество Мальчиков,\nоставшаяся группа будет помечена как Девочки");
            toolTip.SetToolTip(btSetAllGirls, "Пометить группу безполых как Девочки");
                       
        }

        private void btSetAllBoys_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            rp().NewSex = Rabbit.SexType.MALE;
            update();
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!_manual || dataGridView1.CurrentCell.ColumnIndex != FIELD_NEWPLACE) return;
            _manual = false;
            //int cr = dataGridView1.CurrentCell.RowIndex;
            //int cc = dataGridView1.CurrentCell.ColumnIndex;
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //dataGridView1.Rows[cr].Cells[cc].Selected = true;
            _manual = true;
        }

        //private void loadColumnsWidths()
        //{
        //    _manual = false;
        //    string widths = Engine.opt().getOption(Options.OPT_ID.REPLACE_LIST);
        //    string[] arr = widths.Split(new string[]{","},StringSplitOptions.RemoveEmptyEntries);
        //    int w = 10;
        //    for (int i = 0; i < arr.Length; i++)
        //    {
        //        if (int.TryParse(arr[i], out w) && w >0)
        //            dataGridView1.Columns[i].Width = w;       
        //    }
        //    _manual = true;
        //}

        //private string convIntToStr(int v)
        //{
        //    return v.ToString();
        //}

        //private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        //{
        //    if (!_manual) return;
        //    _manual = false;
        //    List<int> widths = new List<int>();
        //    foreach (DataGridViewColumn c in dataGridView1.Columns)
        //        widths.Add(c.Width);
        //    string res = String.Join(",", Array.ConvertAll<int, string>(widths.ToArray(), new Converter<int, string>(convIntToStr)));
        //    Engine.opt().setOption(Options.OPT_ID.REPLACE_LIST, res);
        //    _manual = true;
        //}       
    }
}
