using System;
using System.Windows.Forms;


namespace rabnet.forms
{
    public partial class ReplaceBrideForm : Form
    {
        RabNetEngRabbit r = null;
        BuildingList bs;
        int girlOut = 0;
        public ReplaceBrideForm()
        {
            InitializeComponent();
        }

        public ReplaceBrideForm(int rid):this()
        {
            r = Engine.get().getRabbit(rid);          
        }

        public void FillAddresses()
        {
            comboBox1.Items.Clear();
            Filters f = new Filters();
            f[Filters.FREE] = "1";
            bs=Engine.db().getBuildings(f);
            if (bs.Count == 0)
            {
                MessageBox.Show(@"Не возможно пересадить, т.к. все клетки заняты.
Освободите одну или несколько клеток и попробуйте снова.","Нет свободных клеток",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }
            foreach(Building b in bs)
            {
                for (int i = 0; i < b.Sections; i++)
                    if (b.Busy[i].ID==0)
                    comboBox1.Items.Add(b.MedName(i));
            }
            comboBox1.Sorted = true;
            comboBox1.SelectedIndex = 0;
        }

        public int getGirlOut()
        {
            return girlOut;
        }

        private int[] getAddress(String s)
        {
            if (s == Rabbit.NULL_ADDRESS)
                return new int[] { 0, 0, 0 };
            for (int i = 0; i < bs.Count; i++)
                for (int j = 0; j < bs[i].Sections; j++)
                    if (bs[i].MedName(j) == s)
                    {
                        return new int[] { (int)bs[i].Farm, bs[i].TierID, j };
                    }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex<0)
            {
                DialogResult=DialogResult.Cancel;
                Close();
                return;
            }
            int[] adr=getAddress(comboBox1.Text);
            if (adr[0]==0)
            {
                DialogResult=DialogResult.Cancel;
                Close();
                return;
            }
            girlOut = r.Clone(1, adr[0], adr[1], adr[2]);
        }

        private void ReplaceBride_Load(object sender, EventArgs e)
        {
            FillAddresses();
        }
    }
}
