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
        class RPList : List<RP>
        {
        }
        class RP
        {
            public int id;
            public bool saved = false;
            public string name;
            public int parent=0;
            public int nuparent=0;
            public int count;
            public int nucount;
            public RP madefrom;
            public OneRabbit.RabbitSex nusex;
            public OneRabbit.RabbitSex sex;
            public string address;
            public string nuaddress="";
            public string action="остается";
            public bool younger = false;
            public RPList list = null;
            public RPList children = new RPList();
            public RP(RPList list,int id, string name, string address, int count, OneRabbit.RabbitSex sex)
            {
                this.list = list;
                this.id = id;
                this.name = name;
                this.address = address;
                this.count = count;
                nucount = count;
                this.sex = sex;
                nusex = sex;
            }
            public RP(RPList list,int id, string name, string address, int count, OneRabbit.RabbitSex sex,int parent):
                this(list,id,name,address,count,sex)
            {
                younger = true;
                this.parent = parent;
                nuparent = parent;
            }
            public RP(RP fromrp, int count):this(fromrp.list,fromrp.id,fromrp.name,fromrp.curaddress,count,fromrp.sex,fromrp.parent)
            {
                madefrom = fromrp;
                fromrp.nucount -= count;
                nusex = fromrp.nusex;
                int idx = list.IndexOf(fromrp);
                list.Insert(idx + 1, this);
                fromrp.children.Add(this);
            }
            public void clear()
            {
                nuaddress = "";
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
                        if (r.parent == id)
                            r.address = curaddress;
                }
            }
            public bool replaced { get { return curaddress == address; } }
            public string status
            {
                get
                {
                    string res = "остается";
                    if (curaddress != address)
                    {
                        if (younger)
                        {
                            res = "отсадка";
                            if (nuparent != parent)
                                res = "подсадка";
                        }
                        else
                            res = "пересадка";
                    }
                    if (nucount < count)
                        res += ",разбиение";
                    if (nucount > count)
                        res += ",объединение";
                    if (sex != nusex)
                        res += ",пол-" + OneRabbit.SexToRU(nusex);
                    return res;
                }
            }

            public RP splitGroup(int num)
            {
                if (num<1 || num>=nucount) return null;
                return new RP(this, num);
            }
        };
        private List<RabNetEngRabbit> rbs = new List<RabNetEngRabbit>();
        private RPList rs = new RPList();
        private DataTable ds = new DataTable();
        private DataGridViewComboBoxColumn cbc = new DataGridViewComboBoxColumn();
        private Building[] bs = null;
        public enum Action { NONE,CHANGE}
        private Action action = Action.NONE;
        public ReplaceForm()
        {
            InitializeComponent();
            ds.Columns.Add("Имя", typeof(string));
            ds.Columns.Add("Пол", typeof(string));
            ds.Columns.Add("Количество", typeof(int));
            ds.Columns.Add("Старый адрес", typeof(string));
            ds.Columns.Add("Новый адрес", typeof(string));
            ds.Columns.Add("Статус", typeof(string));
            dataGridView1.DataSource = ds;
            comboBox1.Tag = 1;
            comboBox1.SelectedIndex = 0;
            comboBox1.Tag = 0;
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
                rs.Add(new RP(rs,r.rid, r.fullName, r.address, r.group, r.sex));
                foreach (OneRabbit y in r.youngers)
                    rs.Add(new RP(rs,y.id, " - "+y.fullname, r.address, y.group, y.sex, r.rid));
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
            foreach (RP r in rs)
            {
                if (r.curaddress == id.curaddress && r != id)
                {
                    if (!(id.younger && id.parent==r.id) && !(r.younger && r.parent==id.id))
                        res=true;
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
            ds.Rows.Clear();
            if (reget)
            {
                getBuildings();
                dataGridView1.Columns.RemoveAt(4);
                cbc.HeaderText = "Новый адрес";
                cbc.DataPropertyName = "Новый адрес";
                cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns.Insert(4, cbc);
            }
            foreach (RP r in rs)
            {
                string stat = r.status;
                bool b = busy(r);
                if (b)
                    stat += ",ЗАНЯТО";
                DataRow rw = ds.Rows.Add(r.name, OneRabbit.SexToRU(r.nusex) ,r.nucount, r.address, r.curaddress, stat);
                if (action == Action.CHANGE && dataGridView1.SelectedRows.Count < 2)
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
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
            e.Cancel = (e.ColumnIndex != 4);
        }

        private void ReplaceForm_Load(object sender, EventArgs e)
        {
            updateB();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 4)
                return;
            DataRow rw = ds.Rows[e.RowIndex];
            RP r = rs[e.RowIndex];
            r.curaddress = (string)rw[4];
            update();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
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
            button4.Enabled = (dataGridView1.SelectedRows.Count == 2);
            button5.Enabled = (dataGridView1.SelectedRows.Count >0);
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int cnt = rp(dataGridView1.SelectedRows[0].Index).nucount;
                groupBox1.Enabled = (cnt > 1);
                if (cnt > 1)
                    numericUpDown1.Maximum = cnt-1;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.SelectedRows.Count;i++)
                rp(dataGridView1.SelectedRows[i].Index).clear();
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
            update();
        }

        private void commitRabbit(RP r,int id)
        {
            if (r.saved)
                return;
            RabNetEngRabbit rb = Engine.get().getRabbit(id==0?r.id:id);
            RabNetEngRabbit par = null;
            if (r.younger)
                par = Engine.get().getRabbit(r.parent);
            if (r.replaced)
            {
                int[] a = getAddress(r.curaddress);
                if (r.younger)
                    par.ReplaceYounger(rb.rid, a[0], a[1], a[2]);
                else
                    rb.replaceRabbit(a[0], a[1], a[2], r.address);
            }
            if (r.nusex != r.sex)
                rb.setSex(r.nusex);
            if (r.children.Count > 0)
                foreach (RP c in r.children)
                {
                    int[] a=getAddress(r.curaddress);
                    int cid = rb.clone(c.count, a[0],a[1],a[2]);
                    commitRabbit(c, cid);
                }
            r.saved = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RP r in rs)
                    commitRabbit(r,0);
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show("Ошибка " + ex.GetType().ToString() + ":" + ex.Message);
            }
        }

    }
}
