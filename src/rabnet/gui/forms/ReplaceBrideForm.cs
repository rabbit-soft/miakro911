using System;
using System.Windows.Forms;


namespace rabnet.forms
{
    public partial class ReplaceBrideForm : Form
    {
        RabNetEngRabbit r = null;
        BuildingList _buildings;
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
            _buildings=Engine.db().getBuildings(f);
            if (_buildings.Count == 0)
            {
                MessageBox.Show(@"Не возможно пересадить, т.к. все клетки заняты.
Освободите одну или несколько клеток и попробуйте снова.","Нет свободных клеток",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }
            foreach(Building b in _buildings)
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

        //private int[] getAddress(String s)
        //{
        //    if (s == Rabbit.NULL_ADDRESS)
        //        return new int[] { 0, 0, 0 };
        //    for (int i = 0; i < _buildings.Count; i++)
        //        for (int j = 0; j < _buildings[i].Sections; j++)
        //            if (_buildings[i].MedName(j) == s)
        //            {
        //                return new int[] { (int)_buildings[i].Farm, _buildings[i].TierID, j };
        //            }
        //    return null;
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex<0)
            {
                DialogResult=DialogResult.Cancel;
                Close();
                return;
            }
            Address adr=_buildings.SearchByMedName(comboBox1.Text);
            if (adr.Farm==0)
            {
                DialogResult=DialogResult.Cancel;
                Close();
                return;
            }
            girlOut = r.Clone(1, adr.Farm, adr.Floor, adr.Section);
        }

        private void ReplaceBride_Load(object sender, EventArgs e)
        {
            FillAddresses();
        }
    }
}
