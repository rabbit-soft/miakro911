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
        private List<RabNetEngRabbit> rbs = new List<RabNetEngRabbit>();
        private List<RabNetEngRabbit> nrbs = new List<RabNetEngRabbit>();
        private DataTable ds = new DataTable();
        private DataGridViewComboBoxColumn cbc = new DataGridViewComboBoxColumn();
        private Building[] bs = null;
        public ReplaceForm()
        {
            InitializeComponent();
            ds.Columns.Add("Имя", typeof(string));
            ds.Columns.Add("Группа", typeof(int));
            ds.Columns.Add("Старый адрес", typeof(string));
            ds.Columns.Add("Новый адрес", typeof(string));
            ds.Columns.Add("Статус", typeof(string));
            ds.Columns.Add("rabid", typeof(int));
            ds.Columns.Add("yid", typeof(int));
            dataGridView1.DataSource = ds;
            comboBox1.Tag = 1;
            comboBox1.SelectedIndex = 0;
            comboBox1.Tag = 0;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
        }
        public void addRabbit(int id)
        {
            rbs.Add(Engine.get().getRabbit(id));
        }

        public bool myrab(int i)
        {
            foreach (RabNetEngRabbit r in rbs)
                if (r.rid == i)
                    return true;
            return false;
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
            cbc.Items.Add("бомж");
            foreach (Building b in bs)
            {
                for (int i = 0; i < b.secs(); i++)
                {
                    if (b.busy(i)==0 || myrab(b.busy(i)))
                        cbc.Items.Add(b.fullname[i]);
                }
            }
        }

        public int checkEmpty(string address,int rid)
        {
            int state=0;
            for (int i = 0; i < rbs.Count && state==0; i++)
                if (i != rid)
                {
                    if (rbs[i].address == address)
                    {
                        state = 1;
                        if (rbs[i].tag == "")
                            state = 2;
                    }
                    if (address == rbs[i].tag)
                        state = 2;
                    foreach (OneRabbit y in rbs[i].youngers)
                        if (address == y.tag)
                            state = 2;
                }
            return state;
        }

        public void update(bool reget)
        {
            ds.Rows.Clear();
            if (reget)
            {
                getBuildings();
                dataGridView1.Columns.RemoveAt(3);
                cbc.HeaderText = "Новый адрес";
                cbc.DataPropertyName = "Новый адрес";
                cbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns.Insert(3, cbc);
            }
            dataGridView1.ForeColor = Color.Black;
            foreach (RabNetEngRabbit r in rbs)
            {
                bool err = false;
                int rid = rbs.IndexOf(r);
                String val = r.address;
                String status = "остается";
                if (r.tag != "")
                {
                    val = r.tag;
                    status = "пересадка";
                }
                int st = checkEmpty(val, rid);
                if (st == 1) status = "жилобмен";
                if (st == 2) { err = true; status = "занято"; }
                DataRow rw=ds.Rows.Add(r.fullName, r.group, r.address, val, status,rid,-1,0);
                if (err)   rw.RowError = "занято";
                int yid = 0;
                foreach (OneRabbit y in r.youngers)
                {
                    err = false;
                    if (y.tag != "")
                    {
                        val = y.tag;
                        status = "отсадка";
                    }
                    st = checkEmpty(val, rid);
                    if (st == 1) status = "жилобмен";
                    if (st == 2) { err = true; status = "занято"; }
                    rw=ds.Rows.Add(y.fullname, y.group, r.address, val, status, rid, yid,0);
                    if (err)
                        rw.RowError = "занято";
                    yid++;
                }
            }
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((int)comboBox1.Tag == 1)
                return;
            button3_Click(null, null);
            update(true);
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = (e.ColumnIndex != 3);
        }

        private void ReplaceForm_Load(object sender, EventArgs e)
        {
            update(true);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 3)
                return;
            DataRow rw = ds.Rows[e.RowIndex];
            int id = (int)rw.ItemArray[5];
            int yid = (int)rw.ItemArray[6];
            setCurAddr(id, yid, (string)rw[3]);
            update(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (RabNetEngRabbit r in rbs)
            {
                r.tag = "";
                foreach (OneRabbit y in r.youngers)
                    y.tag = "";
            }
            update(false);
        }

        private string getCurAddr(int idx)
        {
            DataRow rw = ds.Rows[idx];
            return getCurAddr((int)rw[5], (int)rw[6]);
        }
        private string getCurAddr(int r, int y)
        {
            String res = rbs[r].tag==""?rbs[r].address:rbs[r].tag;
            if (y != -1 && rbs[r].youngers[y].tag != "")
                res = rbs[r].youngers[y].tag;
            return res;
        }

        private String setCurAddr(int idx,string addr)
        {
            DataRow rw = ds.Rows[idx];
            return setCurAddr((int)rw[5], (int)rw[6],addr);
        }
        private string setCurAddr(int r,int y,string address)
        {
            String res = getCurAddr(r, y);
            if (y < 0)
            {
                if (address == rbs[r].address || address=="")
                    rbs[r].tag = "";
                else
                    rbs[r].tag = address;
            }
            else
            {
                if (address == getCurAddr(r, -1) || address=="")
                    rbs[r].youngers[y].tag = "";
                else
                    rbs[r].youngers[y].tag = address;
            }
            return res;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 2)
            {
                MessageBox.Show("Выберите две строки");
                return;
            }
            String a1 = setCurAddr(dataGridView1.SelectedRows[0].Index, getCurAddr(dataGridView1.SelectedRows[1].Index));
            setCurAddr(dataGridView1.SelectedRows[1].Index, a1);
            update(false);
        }

        private int getCount(int idx)
        {
            DataRow rw = ds.Rows[idx];
            return getCount((int)rw[5], (int)rw[6]);
        }
        private int getCount(int r,int y)
        {
            if (y == -1)
                return rbs[r].group;
            return rbs[r].youngers[y].group;
        }

        private void dataGridView1_MultiSelectChanged(object sender, EventArgs e)
        {
            button4.Enabled = (dataGridView1.SelectedRows.Count == 2);
            button5.Enabled = (dataGridView1.SelectedRows.Count >0);
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int cnt=getCount(dataGridView1.SelectedRows[0].Index);
                groupBox1.Enabled = (cnt > 1);
                if (cnt > 1)
                    numericUpDown1.Maximum = cnt-1;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow rw in dataGridView1.SelectedRows)
                setCurAddr(rw.Index, "");
            update(false);
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

    }
}
