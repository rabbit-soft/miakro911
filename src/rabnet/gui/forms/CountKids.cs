using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet.forms
{
    public partial class CountKids : Form
    {
        private class OneCount
        {
            public YoungRabbit Younger;
            public int Dead;
            public int Killed;
            public int Added;

            public OneCount(YoungRabbit y, int dead, int killed, int added)
            {
                this.Younger =y;
                this.Dead = dead;
                this.Killed = killed;
                this.Added = added;
            }

            public int Atall
            {
                get { return Younger.Group - (Dead + Killed) + Added; }
            }
        }

        private class OneCountList : List<OneCount>
        {
            public OneCount GetByYID(int id)
            {
                foreach (OneCount oc in this)
                    if (oc.Younger.ID == id)
                        return oc;
                return null;
            }
        }

        private RabNetEngRabbit _rab = null;
        private int _grp=0;
        private OneCountList _ocList = new OneCountList();
        private bool _counted=false;

        public CountKids()
        {
            InitializeComponent();
        }

        public CountKids(int id):this(Engine.get().getRabbit(id)/*,suckers*/){}
        public CountKids(RabNetEngRabbit r):this()
        {
            this._rab = r;
            foreach (YoungRabbit y in _rab.Youngers)
                _ocList.Add(new OneCount(y, 0, 0, 0));
        }
        public void SetGroup(int grp)
        {
            this._grp=grp;
        }

        public void MakeCount()
        {
            if (_counted) return;

            foreach (OneCount oc in _ocList)
            {
                _rab.CountKids(oc.Dead, oc.Killed, oc.Added, oc.Atall, oc.Younger.Age, oc.Younger.ID);
            }
            _counted = true;
        }

        private void CountKids_Load(object sender, EventArgs e)
        {
            lParent.Text = _rab.FullName;
            comboBox1.Items.Clear();
            for (int i = 0; i < _rab.Youngers.Count; i++)
            {
                comboBox1.Items.Add(_rab.Youngers[i].NameFull + " (" + _rab.Youngers[i].Group + ")");                
            }
            comboBox1.SelectedIndex = _grp;
        }

        private void nudDead_ValueChanged(object sender, EventArgs e)
        {
            int s = comboBox1.SelectedIndex;
            OneCount oc = _ocList[s];

            oc.Dead = (int)nudDead.Value;
            oc.Killed = (int)nudKilled.Value;
            oc.Added = (int)nudAdd.Value;

            tbAlive.Text = oc.Atall.ToString();
            nudKilled.Maximum = oc.Younger.Group - oc.Dead;
            nudDead.Maximum = oc.Younger.Group - oc.Killed;            
            //int c = _rab.Youngers[s].Group;
            //int x = c - (int)(nudDead.Value + nudKilled.Value)+(int)nudAdd.Value;
            //tbAlive.Text = x.ToString();
            //nudKilled.Maximum = c - nudDead.Value;
            //nudDead.Maximum = c - nudKilled.Value;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MakeCount();
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboBox1.SelectedIndex;
            OneCount oc = _ocList[i];

            lAge.Text = String.Format("Возраст: {0:d}{1:s}Порода:{2:s}", + oc.Younger.Age,Environment.NewLine, oc.Younger.BreedName);
            tbAlive.Text = oc.Younger.Group.ToString();
            //nudKilled.Value = 
            //    nudDead.Value =
            //    nudAdd.Value = 0;
            nudKilled.Maximum = 
                nudDead.Maximum = oc.Younger.Group;

            nudDead.Value = oc.Dead;
            nudKilled.Value = oc.Killed;
            nudAdd.Value = oc.Added;
        }

    }

}
