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
        public const int PLACEFIELD = 5;
        class RPList : List<RP>
        {
        }
        class RP
        {
            public int id;
            public bool saved = false;
            public string sname;
            public int parent=0;
            public int nuparent=0;
            public int count;
            public int nucount;
            public RP madefrom;
            public OneRabbit.RabbitSex nusex;
            public OneRabbit.RabbitSex sex;
            public int age=0;
            public string address;
            public string nuaddress="";
            public string action="остается";
            public bool younger = false;
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
            public string curaddress
            { 
                get { return nuaddress==""?address:nuaddress;} 
                set{ if (value==address || value=="") nuaddress=""; else nuaddress=value;
                    foreach (RP r in list)
                        if (r.parent == id && r != this)
                        {
                            String cur = r.address;
                            r.address = curaddress;
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
                    string res = nucount>1?"остаются":"остается";
                    if (curaddress != address)
                    {
                        if (younger)
                        {
                            res = "отсадка";
                        }
                        else
                            res = "пересадка";
                        if (placeto != null)
                            res = "подсадка";
                    }
                    if (placewith != null)
                        res = ",объединение";
                    if (nucount < count)
                        res += ",разбиение";
                    if (sex != nusex)
                        res += ",пол-" + OneRabbit.SexToRU(nusex);
                    return res;
                }
            }

            public RP splitGroup(int num)
            {
                if (id == 0) return null;
                if (num<1 || num>=nucount) return null;
                return new RP(this, num);
            }
        };

        private int girlout = 0;
        private List<RabNetEngRabbit> rbs = new List<RabNetEngRabbit>();
        private RPList rs = new RPList();
        private DataTable ds = new DataTable();
        private DataGridViewComboBoxColumn cbc = new DataGridViewComboBoxColumn();        
        private Building[] bs = null;
        public enum Action { NONE,CHANGE,BOYSOUT,SET_NEST,ONE_GIRL_OUT}
        private Action action = Action.NONE;
        private bool globalError=false;
        private bool noboys = false;
        private int combineage = 0;
        public ReplaceForm()
        {
            InitializeComponent();
            ds.Columns.Add("Имя", typeof(string));
            ds.Columns.Add("Возраст", typeof(int));
            ds.Columns.Add("Пол", typeof(string));
            ds.Columns.Add("Количество", typeof(int));
            ds.Columns.Add("Старый адрес", typeof(string));
            ds.Columns.Add("Новый адрес", typeof(string));
            ds.Columns.Add("Статус", typeof(string));
            dataGridView1.DataSource = ds;
            comboBox1.Tag = 1;
            comboBox1.SelectedIndex = 0;
            comboBox1.Tag = 0;
            combineage = Engine.get().options().getIntOption(Options.OPT_ID.COMBINE_AGE);
        }
        public void addRabbit(int id)
        {
            addRabbit(Engine.get().getRabbit(id));
        }
        public void addRabbit(RabNetEngRabbit r)
        {
            rbs.Add(r);
            clear();
        }
        public void clear()
        {
            rs.Clear();
            foreach (RabNetEngRabbit r in rbs)
            {
                rs.Add(new RP(rs,r.rid, r.fullName, r.address, r.group, r.sex,r.age));
                foreach (OneRabbit y in r.youngers)
                    rs.Add(new RP(rs,y.id, y.fullname, r.address, y.group, y.sex,(DateTime.Now-y.born).Days, r.rid));
            }
        }
        public void setAction(Action act)
        {
            action = act;
        }

        public bool myrab(int i)
        {
            foreach (RabNetEngRabbit r in rbs)
                if (r.rid == i)
                    return true;
            return false;
        }

        private bool busy(RP id)
        {
            bool res=false;
            if (id.curaddress == OneRabbit.NullAddress) return false;
            foreach (RP r in rs)
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

        public void getBuildings()
        {
            Filters f = new Filters();
            f["rcnt"] = rbs.Count.ToString();
            for (int i=0;i<rbs.Count;i++)
                f["r" + i.ToString()] = rbs[i].rid.ToString();
            String tp = "";
            comboBox1.Tag = 1;
            if (action == Action.SET_NEST)
                comboBox1.SelectedIndex = 4;
            comboBox1.Tag = 0;
            if (comboBox1.SelectedIndex != 0)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 1: tp = "female"; break;
                    case 2: tp = "dfemale"; break;
                    case 3: tp = "complex"; break;
                    case 4: tp = "jurta"; break;
                    case 5: tp = "quarta"; break;
                    case 6: tp = "vertep"; break;
                    case 7: tp = "barin"; break;
                    case 8: tp = "cabin"; break;
                }
                f["tp"] = tp;
            }
            bs = Engine.db().getFreeBuilding(f);
            cbc.Items.Clear();
            cbc.Items.Add(OneRabbit.NullAddress);
            foreach (Building b in bs)
            {
                for (int i = 0; i < b.secs(); i++)
                {
                    if (b.busy(i)==0 || myrab(b.busy(i)))
                        cbc.Items.Add(b.fullname[i]);
                }
            }
            foreach (RP r in rs)
            {
                if (!cbc.Items.Contains(r.curaddress))
                    cbc.Items.Add(r.curaddress);
            }
        }

        public void updateB() { update(true); }
        public void update(){update(false);}
        public void update(bool reget)
        {
            globalError = false;
            ds.Rows.Clear();
            if (reget)
            {
                getBuildings();
                dataGridView1.Columns.RemoveAt(PLACEFIELD);
                cbc.MaxDropDownItems = 20;
                cbc.HeaderText = "Новый адрес";
                cbc.DataPropertyName = "Новый адрес";
                cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns.Insert(PLACEFIELD, cbc);
            }
            foreach (RP r in rs)
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
                DataRow rw = ds.Rows.Add(r.name, r.age,OneRabbit.SexToRU(r.nusex) ,r.nucount, r.address, r.curaddress, stat);
                if (action == Action.CHANGE && dataGridView1.SelectedRows.Count < 2)
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                if (action == Action.BOYSOUT)
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = r.younger;
                }
                if (b)
                    rw.RowError = "занято";
            }
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            if (action == Action.CHANGE)
            {
                action = Action.NONE;
                if (dataGridView1.SelectedRows.Count == 2)
                    button4.PerformClick();
            }
            if (action == Action.BOYSOUT)
            {
//                action = Action.NONE;
                button7.Enabled = button4.Enabled = button6.Enabled=noboys;
            }
        }

        private RP rp(int idx)
        {
            return rs[idx];
        }
        private RP rp()
        {
            return rp(dataGridView1.SelectedRows[0].Index);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((int)comboBox1.Tag == 1)
                return;
            updateB();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = (e.ColumnIndex != PLACEFIELD);
        }

        private void ReplaceForm_Load(object sender, EventArgs e)
        {
            updateB();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != PLACEFIELD)
                return;
            DataRow rw = ds.Rows[e.RowIndex];
            RP r = rs[e.RowIndex];
            r.curaddress = (string)rw[PLACEFIELD];
            update();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
            button7.Enabled = button6.Enabled = (action != Action.BOYSOUT);
            noboys = false;
            update();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 2)
            {
                MessageBox.Show("Выберите две строки");
                return;
            }
            RP r1= rp(dataGridView1.SelectedRows[0].Index);
            RP r2 = rp(dataGridView1.SelectedRows[1].Index);
            string ca = r1.curaddress;
            r1.curaddress = r2.curaddress;
            r2.curaddress = ca;
            update();
        }

        private void dataGridView1_MultiSelectChanged(object sender, EventArgs e)
        {
            button4.Enabled = (dataGridView1.SelectedRows.Count == 2 && action!=Action.ONE_GIRL_OUT);
            button5.Enabled = (dataGridView1.SelectedRows.Count >0);
            groupBox1.Enabled = groupBox2.Enabled = false;
            if (dataGridView1.SelectedRows.Count == 2 && action!=Action.ONE_GIRL_OUT)
            {
                groupBox2.Enabled = true;
                button8.Enabled = button9.Enabled = button13.Enabled=false;
                RP r1 = rp(dataGridView1.SelectedRows[0].Index);
                RP r2 = rp(dataGridView1.SelectedRows[1].Index);
                if ((r1.count == 1 && r1.sex == OneRabbit.RabbitSex.FEMALE && r1.age>r2.age) || (r2.count == 1 && r2.sex == OneRabbit.RabbitSex.FEMALE && r2.age>r1.age))
                {
                    button8.Enabled = true;
                }
                if (r1.sex == r2.sex && Math.Abs(r1.age-r2.age)<=combineage)
                {
                    button9.Enabled = button13.Enabled=true;
                }
                button9.Enabled = button13.Enabled = (r1.id != 0 && r2.id != 0);
            }
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int cnt = rp(dataGridView1.SelectedRows[0].Index).nucount;
                groupBox1.Enabled = (cnt > 1);
                if (cnt > 1)
                    numericUpDown1.Maximum = cnt-1;
                button10.Enabled = button11.Enabled = rp().nusex == OneRabbit.RabbitSex.VOID;
                groupBox1.Enabled = (rp().id != 0);
                if (action == Action.ONE_GIRL_OUT)
                {
                    numericUpDown1.Value = numericUpDown1.Maximum = numericUpDown1.Minimum = 1;
                    button10.Enabled = button11.Enabled = button4.Enabled = false;
                    groupBox2.Enabled = false;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.SelectedRows.Count;i++)
                rp(dataGridView1.SelectedRows[i].Index).clear();
            button7.Enabled=button6.Enabled=(action!=Action.BOYSOUT);
            noboys = false;
            update();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            rp().splitGroup((int)numericUpDown1.Value);
            update();
        }

        private int[] getAddress(String s)
        {
            if (s == OneRabbit.NullAddress)
                return new int[] { 0, 0, 0 };
            for (int i = 0; i < bs.Length; i++)
                for (int j = 0; j < bs[i].secs();j++ )
                    if (bs[i].fullname[j] == s)
                    {
                        return new int[]{bs[i].farm(),bs[i].tier_id(),j};
                    }
            return null;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException=false;
            dataGridView1.Rows[e.RowIndex].ErrorText = e.ColumnIndex.ToString()+":"+e.Exception.Message;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            for (int i = 0; i < rp().count-1;i++)
                rp().splitGroup(1);
            update();

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            RP nrp=rp().splitGroup((int)numericUpDown1.Value);
            rp().nusex = OneRabbit.RabbitSex.FEMALE;
            nrp.nusex = OneRabbit.RabbitSex.MALE;
            noboys = true;
            button7.Enabled = button6.Enabled = true;
            update();
        }

        private void commitRabbit(RP r,int id,bool replaced)
        {
            if (r.saved)
                return;
            if (r.id == 0)
            {
                int[] a = getAddress(r.curaddress);
                rbs[rs.IndexOf(r)].replaceRabbit(a[0], a[1], a[2], r.curaddress);
                r.saved = true;
                return;
            }
            RabNetEngRabbit rb = Engine.get().getRabbit(id==0?r.id:id);
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
                    par.ReplaceYounger(rb.rid, a[0], a[1], a[2]);
                else
                    rb.replaceRabbit(a[0], a[1], a[2], r.curaddress);
            }
            if (r.nusex != r.sex)
                rb.setSex(r.nusex);
            if (r.children.Count > 0)
                foreach (RP c in r.children)
                {
                    int[] a=getAddress(r.curaddress);
                    int cid = rb.clone(c.count, a[0],a[1],a[2]);
                    commitRabbit(c, cid,replaced);
                    if (action == Action.ONE_GIRL_OUT && girlout == 0 && c.sex == OneRabbit.RabbitSex.FEMALE && c.count == 1)
                        girlout = cid;
                }
            r.saved = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (globalError)
                    throw new ApplicationException("Секция для пересадки занята");
                foreach (RP r in rs)
                    commitRabbit(r,0,false);
                foreach (RP r in rs)
                    commitRabbit(r, 0, true);
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("Ошибка " + ex.GetType().ToString() + ":" + ex.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            rp().nusex = OneRabbit.RabbitSex.FEMALE;
            update();
        }

        private void button12_Click(object sender, EventArgs e)
        {
                for (int j = rs.Count - 1; j > 0;j-- )
                {
                    RP r = rs[j];
                    if (busy(r))
                    {
                        for (int i = 1; i < cbc.Items.Count && busy(r); i++)
                            if ((string)cbc.Items[i] != r.address)
                                r.curaddress = (string)cbc.Items[i];
                    }
                }
            update();
        }

        private void button8_Click(object sender, EventArgs e)
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

        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 2) return;
            RP r1 = rp(dataGridView1.SelectedRows[(sender == button9 ? 1 : 0)].Index);
            RP r2 = rp(dataGridView1.SelectedRows[(sender == button9 ? 0 : 1)].Index);
            r2.placewith = r1;
            r1.placewith = null;
            r2.curaddress = r1.curaddress;
            update();
        }

        public int getGirlOut()
        {
            return girlout;
        }

    }
}
