using System;
using System.Windows.Forms;


namespace rabnet
{
    public partial class ReplaceBride : Form
    {
        RabNetEngRabbit r = null;
        Building[] bs;
        int girlOut = 0;
        public ReplaceBride()
        {
            InitializeComponent();
        }

        public ReplaceBride(int rid):this()
        {
            r = Engine.get().getRabbit(rid);
            FillAddresses();
        }

        public void FillAddresses()
        {
            comboBox1.Items.Clear();
            bs=Engine.db().getFreeBuilding(new Filters());
            foreach(Building b in bs)
            {
                for (int i = 0; i < b.secs();i++ )
                    if (b.busy(i)==0)
                    comboBox1.Items.Add(b.medname[i]);
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
            if (s == OneRabbit.NullAddress)
                return new int[] { 0, 0, 0 };
            for (int i = 0; i < bs.Length; i++)
                for (int j = 0; j < bs[i].secs(); j++)
                    if (bs[i].medname[j] == s)
                    {
                        return new int[] { (int)bs[i].farm(), bs[i].tier_id(), j };
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
    }
}
