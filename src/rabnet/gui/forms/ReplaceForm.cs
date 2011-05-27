using System;
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
        /// <summary>
        /// Номер колонки, в которой содержится адрес, по которому переселят кролика
        /// (номер считается от 0)
        /// </summary>
        public const int PLACEFIELD = 5;
        /// <summary>
        /// Номер колонки, в которой содержится чекбокс с установкой гнездовья
        /// </summary>
        public const int NESTFIELD = 7;
        class RPList : List<RP>
        {
        }

        class RP
        {
            public bool wchild = false;
            public int id;
            public bool saved = false;
            public string sname;
            /// <summary>
            /// ID кормилицы
            /// </summary>
            public int parent=0;
            /// <summary>
            /// ID новой кормилицы
            /// </summary>
            public int nuparent=0;
            /// <summary>
            /// Количество кроликов в группе
            /// </summary>
            public int count;
            public int nucount;
            public RP madefrom;
            /// <summary>
            /// Новый пол кролика
            /// </summary>
            public OneRabbit.RabbitSex nusex;
            /// <summary>
            /// Пол кролика
            /// </summary>
            public OneRabbit.RabbitSex sex;
            public int age=0;
            /// <summary>
            /// Текущий адрес кролика
            /// </summary>
            public string address;
            /// <summary>
            /// Новый адрес кролика
            /// </summary>
            public string nuaddress="";
            public string action="остается";
            /// <summary>
            /// Является ли группа молодняком
            /// </summary>
            public bool younger = false;
            /// <summary>
            /// Может ли быть установлено гнездовье
            /// </summary>
            public bool canHaveNest = false;
            /// <summary>
            /// Установить ли гнездовье
            /// </summary>
            public bool setNest = false;
            public RP placeto = null;
            public RP placewith = null;
            public RPList list = null;
            public RPList children = new RPList();
            public RP(RPList list,int id, string name, string address, int count, OneRabbit.RabbitSex sex,int age)
            {
                this.list = list;
                this.id = id;
                sname = name;
                this.address = address;
                this.count = count;
                nucount = count;
                this.sex = sex;
                nusex = sex;
                this.age = age;
                this.canHaveNest = canTierHaveNest(address);
            }

            public static bool canTierHaveNest(string place)
            {              
                if (place.Contains(myBuildingType.DualFemale_Rus) || place.Contains(myBuildingType.Female_Rus)) return true;
                if (place.Contains(myBuildingType.Jurta_Rus))
                {
                    place = place.Remove(place.LastIndexOf(" ["));
                    if (place.Contains("а"))
                        return true;
                }
                return false;
            }

            public RP(RPList list, int id, string name, string address, int count, OneRabbit.RabbitSex sex, int age, bool nest):
                this(list, id, name, address, count, sex, age)
            {
                this.setNest = nest;
            }
            public RP(RPList list,int id, string name, string address, int count, OneRabbit.RabbitSex sex,int age,int parent):
                this(list,id,name,address,count,sex,age)
            {
                younger = true;
                this.parent = parent;
                nuparent = parent;
            }
            public RP(RP fromrp, int count):this(fromrp.list,fromrp.id,fromrp.name,fromrp.curaddress,count,fromrp.sex,fromrp.age,fromrp.parent)
            {
                madefrom = fromrp;
                fromrp.nucount -= count;
                nusex = fromrp.nusex;
                int idx = list.IndexOf(fromrp);
                list.Insert(idx + 1, this);
                fromrp.children.Add(this);
                younger = false;
            }
            public void clear()
            {
                nuaddress = "";
                placeto = null;
                placewith = null;
                foreach (RP r in list)
                    if (r.parent == id)
                        r.address = address;
                while (children.Count > 0)
                {
                    RP f = children[0];
                    f.clear();
                    children.RemoveAt(0);
                    list.Remove(f);
                }
                nucount = count;
                nusex = sex;
                nuparent = parent;
            }

            public void setCurAddress(string adr, bool wchildren)
            {
                wchild = wchildren;
                curaddress = adr;
                wchild = false;
            }

            public string curaddress
            {
                get { return nuaddress == "" ? address : nuaddress; } 
                set
                {
                    if (value == address || (value == "" || value == "бомж"))
                        nuaddress = "";
                    else nuaddress = value;
                    foreach (RP r in list)
                        if (r.parent == id && r != this)
                        {
                            String cur = r.address;
                            r.address = curaddress;
                            if (!wchild)
                                r.curaddress = cur;
                        }
                }
            }
            public bool replaced { get { return curaddress != address; } }
            public string name { get { return (younger?" - ":"")+sname; } set { sname = value; } }
            public string status
            {
                get
                {
                    string res = nucount > 1 ? "остаются" : "остается";
                    if (curaddress != address)
                    {
                        if (younger)res = "отсадка";                      
                        else res = "пересадка";
                        if (placeto != null) res = "подсадка";
                    }
                    if (placewith != null) res = ",объединение";
                    if (nucount < count) res += ",разбиение";
                    if (sex != nusex) res += ",пол-" + OneRabbit.SexToRU(nusex);
                    return res;
                }
            }

            public RP splitGroup(int num)
            {
                if (id == 0) return null;
                if (num<1 || num>=nucount) return null;
                return new RP(this, num);
            }
        }

        private int girlout = 0;
        private List<RabNetEngRabbit> rbs = new List<RabNetEngRabbit>();
        private RPList _replaceList = new RPList();
        private DataTable _dataSet = new DataTable();
        /// <summary>
        /// Комбобокс с новыми адресами, который добавляется к новой строке
        /// </summary>
        private DataGridViewComboBoxColumn dgcbNewAddress = new DataGridViewComboBoxColumn();        
        private Building[] bs = null;
        public enum Action { NONE,CHANGE,BOYSOUT,SET_NEST,ONE_GIRL_OUT}
        private Action _action = Action.NONE;
        private bool globalError=false;
        private bool noboys = false;
        private int combineage = 0;

        public ReplaceForm()
        {
            InitializeComponent();
            initialHints();
            _dataSet.Columns.Add("Имя", typeof(string));
            _dataSet.Columns.Add("Возраст", typeof(int));
            _dataSet.Columns.Add("Пол", typeof(string));
            _dataSet.Columns.Add("Количество", typeof(int));
            _dataSet.Columns.Add("Старый адрес", typeof(string));
            _dataSet.Columns.Add("Новый адрес", typeof(string));
            _dataSet.Columns.Add("Статус", typeof(string));
            _dataSet.Columns.Add("Гнездовье", typeof(bool));
            dataGridView1.DataSource = _dataSet;
            //dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            cbFilter.Tag = 1;
            cbFilter.SelectedIndex = 0;
            cbFilter.Tag = 0;
            combineage = Engine.get().options().getIntOption(Options.OPT_ID.COMBINE_AGE);
        }

        /// <summary>
        /// Добавляет из вне Кролика, которого надо пересадить в форму.
        /// </summary>
        /// <param name="id"></param>
        public void addRabbit(int id)
        {
            addRabbit(Engine.get().getRabbit(id));
        }
        /// <summary>
        /// Добавление нового кролика в список, если есть подсосные то и их
        /// </summary>
        /// <param name="r">Объект символизирующий кролика или группу</param>
        public void addRabbit(RabNetEngRabbit r)
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
                _replaceList.Add(new RP(_replaceList,r.RabID, r.FullName, r.medAddress, r.Group, r.Sex, r.age,r.SetNest));
                //Engine.get().db().getBuilding(r.Address);
                foreach (OneRabbit y in r.Youngers)
                    _replaceList.Add(new RP(_replaceList,y.id, y.fullname, r.medAddress, y.group, y.sex,(DateTime.Now-y.born).Days, r.RabID));
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
            if (id.curaddress == OneRabbit.NullAddress) return false;
            foreach (RP r in _replaceList)
            {
                if (r.curaddress == id.curaddress && r != id && !res)
                {
                    if (!(id.younger && id.parent==r.id) && !(r.younger && r.parent==id.id))
                        res=true;
                    if (res)
                    {
                        if (r.placewith == id || id.placewith == r) res=false;
                        if(r.placeto == id || id.placeto == r) res=false;
                        if (r.placeto!=null && (id.younger && r.placeto.id == id.parent))
                            res = false;
                        if (id.placeto != null && (r.younger && id.placeto.id == r.parent))
                            res = false;
                        if (r.parent == id.parent && r.parent != 0)
                            res = false;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Заволняет ComboBox с новыми адресами
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
                        case 1: tp = myBuildingType.Female; break;
                        case 2: tp = myBuildingType.DualFemale; break;
                        case 3: tp = myBuildingType.Complex; break;
                        case 4: tp = myBuildingType.Jurta; break;
                        case 5: tp = myBuildingType.Quarta; break;
                        case 6: tp = myBuildingType.Vertep; break;
                        case 7: tp = myBuildingType.Barin; break;
                        case 8: tp = myBuildingType.Cabin; break;
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
            dgcbNewAddress.Items.Add(OneRabbit.NullAddress);
            foreach (Building b in bs)
            {
                for (int i = 0; i < b.secs(); i++)
                {
                    if (_action == Action.SET_NEST && b.ftype == myBuildingType.Jurta && i == 1)
                        continue;
                    if (b.busy(i)==0 || myrab(b.busy(i))) 
                        dgcbNewAddress.Items.Add(b.medname[i]);
                }
            }
            foreach (RP r in _replaceList)
            {
                if (!dgcbNewAddress.Items.Contains(r.curaddress))
                    dgcbNewAddress.Items.Add(r.curaddress);
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
                dataGridView1.Columns.RemoveAt(PLACEFIELD);
                dgcbNewAddress.MaxDropDownItems = 20;
                dgcbNewAddress.HeaderText = "Новый адрес";
                dgcbNewAddress.DataPropertyName = "Новый адрес";
                dgcbNewAddress.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns.Insert(PLACEFIELD, dgcbNewAddress);
            }
            foreach (RP r in _replaceList)
            {
                string stat = r.status;
                if (r.placeto != null)
                    r.curaddress = r.placeto.curaddress;
                if (r.placewith != null) 
                    r.curaddress = r.placewith.curaddress;
                bool b = busy(r);
                if (b)
                {
                    globalError = true;
                    stat += ",ЗАНЯТО";
                }
                DataRow rw = _dataSet.Rows.Add(r.name, r.age, OneRabbit.SexToRU(r.nusex), r.nucount, r.address, r.curaddress, stat, r.setNest);
                if (_action == Action.CHANGE && dataGridView1.SelectedRows.Count < 2 && r.parent==0)
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                if (_action == Action.BOYSOUT)
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = r.younger;
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
            if (selectedRow != -1)            
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
            if (e.ColumnIndex != NESTFIELD ||(e.ColumnIndex == NESTFIELD && !canHaveNest(e.RowIndex)))
                e.Cancel = (e.ColumnIndex != PLACEFIELD);
        }
        private void ReplaceForm_Load(object sender, EventArgs e)
        {
            if (_action == Action.SET_NEST)
            {
                cbFilter.Enabled = false;
                this.Text += " - Установка гнездовья";
            }
            updateB();
        }
        /// <summary>
        /// Вызывается при окончании редактирования ячейки с адресом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex != PLACEFIELD && e.ColumnIndex != NESTFIELD) || _dataSet.Rows.Count == 0)
                return;
            DataRow rw = _dataSet.Rows[e.RowIndex];
            RP r = _replaceList[e.RowIndex];
            r.curaddress = (string)rw[PLACEFIELD];
            if (e.ColumnIndex == PLACEFIELD && r.address == r.curaddress)
                return;
            r.canHaveNest = canHaveNest(e.RowIndex);
            if (!r.canHaveNest)
                r.setNest = false;
            else r.setNest = (bool)rw[NESTFIELD];
            update();
        }
        /// <summary>
        /// Можно ли установить гнездовье
        /// </summary>
        /// <param name="rowIndex">Номер строки</param>
        private bool canHaveNest(int rowIndex)
        {
            string place = dataGridView1.Rows[rowIndex].Cells[PLACEFIELD].Value.ToString();
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
            string ca = r1.curaddress;
            r1.setCurAddress(r2.curaddress,true);
            r2.setCurAddress(ca,true);
            update();
        }

        private void dataGridView1_MultiSelectChanged(object sender, EventArgs e)
        {
            btChangeAddresses.Enabled = (dataGridView1.SelectedRows.Count == 2 && _action!=Action.ONE_GIRL_OUT);
            btClear.Enabled = (dataGridView1.SelectedRows.Count >0);
            groupBox1.Enabled = groupBox2.Enabled = false;
            if (dataGridView1.SelectedRows.Count == 2 && _action!=Action.ONE_GIRL_OUT)
            {
                groupBox2.Enabled = true;
                btCombine.Enabled = btUniteUp.Enabled = btUniteDown.Enabled=false;
                RP r1 = rp(dataGridView1.SelectedRows[0].Index);
                RP r2 = rp(dataGridView1.SelectedRows[1].Index);
                if ((r1.count == 1 && r1.sex == OneRabbit.RabbitSex.FEMALE && r1.age>r2.age) || (r2.count == 1 && r2.sex == OneRabbit.RabbitSex.FEMALE && r2.age>r1.age))
                {
                    btCombine.Enabled = true;
                }
                if (r1.sex == r2.sex && Math.Abs(r1.age-r2.age)<=combineage)
                {
                    btUniteUp.Enabled = btUniteDown.Enabled=true;
                }
                btUniteUp.Enabled = btUniteDown.Enabled = (r1.id != 0 && r2.id != 0);
            }
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int cnt = rp(dataGridView1.SelectedRows[0].Index).nucount;
                groupBox1.Enabled = (cnt > 1);
                btSeparateBoys.Enabled = btSetAllGirls.Enabled = btSetAllBoys.Enabled = (rp().nusex == OneRabbit.RabbitSex.VOID);
                if (cnt > 1)           
                    numericUpDown1.Maximum = cnt - 1;
                numericUpDown1.Enabled = btSeparate.Enabled = btSeparateBoys.Enabled = btSeparateByOne.Enabled = (cnt > 1) ;                
                groupBox1.Enabled = (rp().id != 0);
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
            rp().splitGroup((int)numericUpDown1.Value);
            update();
        }
        /// <summary>
        /// Преобразует строку адреса типа  555а[Кварта] в масссив цифр
        /// </summary>
        /// <param name="s">Название клетки</param>
        /// <returns>Массив (Номер фермы, Ярус, Клетка)</returns>
        private int[] getAddress(String s)
        {
            if (s == OneRabbit.NullAddress)
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
            for (int i = 0; i < rp().count-1;i++)
                rp().splitGroup(1);
            update();

        }

        private void btSeparateBoys_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            RP nrp = rp().splitGroup((int)numericUpDown1.Value);
            rp().nusex = OneRabbit.RabbitSex.FEMALE;
            nrp.nusex = OneRabbit.RabbitSex.MALE;
            noboys = true;
            btSeparate.Enabled = btSeparateByOne.Enabled = true;
            update();
        }

        private void commitRabbit(RP r,int id,bool replaced)
        {
            if (r.saved) return;
            if (r.id == 0)
            {
                int[] a = getAddress(r.curaddress);
                rbs[_replaceList.IndexOf(r)].replaceRabbit(a[0], a[1], a[2], r.curaddress);
                r.saved = true;
                return;
            }
            RabNetEngRabbit rb = Engine.get().getRabbit(id == 0 ? r.id : id);
            RabNetEngRabbit par = null;
            if (r.younger)
                par = Engine.get().getRabbit(r.parent);
            if (r.placeto != null || r.placewith != null)
            {
                if (!replaced) return;
                if (r.placewith != null)
                {
                    rb.combineWidth(r.placewith.id);
                    r.saved = true;
                    return;
                }
                if (r.placeto != null)
                {
                    rb.placeSuckerTo(r.placeto.id);
                }
            }
            if (r.replaced && !replaced)
            {
                int[] a = getAddress(r.curaddress);
                if (r.younger)
                    par.ReplaceYounger(rb.RabID, a[0], a[1], a[2], r.curaddress);
                else
                    rb.replaceRabbit(a[0], a[1], a[2], r.curaddress);
                
            }
            if (r.canHaveNest)
            {
                if (!r.younger || (r.younger && r.replaced))
                {
                    RabNetEngRabbit rr = Engine.get().getRabbit(rb.RabID);
                    RabNetEngBuilding rbe = Engine.get().getBuilding(rr.JustAddress);
                    string[] vals = rr.JustAddress.Split(',');
                    if (vals[3] == myBuildingType.Jurta || vals[3] == myBuildingType.Female)
                        rbe.setNest(r.setNest);
                    else if (vals[3] == myBuildingType.DualFemale)
                    {                        
                        if (vals[2] == "0")
                            rbe.setNest(r.setNest);
                        else if (vals[2] == "1")
                            rbe.setNest2(r.setNest);
                    }
                }
            }

            if (r.nusex != r.sex)
                rb.setSex(r.nusex);
            if (r.children.Count > 0)
                foreach (RP c in r.children)
                {
                    int[] a=getAddress(c.curaddress);
                    int cid = rb.clone(c.count, a[0],a[1],a[2]);
                    commitRabbit(c, cid,replaced);
                    if (_action == Action.ONE_GIRL_OUT && girlout == 0 && c.sex == OneRabbit.RabbitSex.FEMALE && c.count == 1)
                        girlout = cid;
                }
            r.saved = true;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
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
                if (!rp.canHaveNest && !rp.setNest)
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
            rp().nusex = OneRabbit.RabbitSex.FEMALE;
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
                            if ((string)dgcbNewAddress.Items[i] != r.address)
                                r.curaddress = (string)dgcbNewAddress.Items[i];
                    }
                }
            update();
        }

        private void btCombine_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 2) return;
            RP mother = rp(dataGridView1.SelectedRows[0].Index);
            RP child = rp(dataGridView1.SelectedRows[1].Index);
            if (child.age > mother.age)
            {
                RP ex = mother; mother = child; child = ex;
            }
            child.placeto = mother;
            child.curaddress = mother.curaddress;
            update();
        }

        private void btUniteUp_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 2) return;
            RP r1 = rp(dataGridView1.SelectedRows[(sender == btUniteUp ? 1 : 0)].Index);
            RP r2 = rp(dataGridView1.SelectedRows[(sender == btUniteUp ? 0 : 1)].Index);
            r2.placewith = r1;
            r1.placewith = null;
            r2.curaddress = r1.curaddress;
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
            rp().nusex = OneRabbit.RabbitSex.MALE;
            update();
        }
    }
}
