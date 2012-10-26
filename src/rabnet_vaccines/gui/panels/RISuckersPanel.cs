using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class RISuckersPanel : UserControl
    {
        private RabNetEngRabbit _rab = null;
        private Catalog _breeds = null;

        public RISuckersPanel()
        {
            InitializeComponent();
        }

        public void Fill(RabNetEngRabbit rab)
        {
            _rab = rab;
            fill(false);
        }

        private void fill(bool reget)
        {
            if (reget)
                _rab.YoungersUpdate();
            lvSuckers.Items.Clear();
            foreach (YoungRabbit y in _rab.Youngers)
            {
                ListViewItem li = lvSuckers.Items.Add(y.NameFull);
                li.SubItems.Add(y.Group.ToString());
                li.SubItems.Add(y.Age.ToString());
                li.SubItems.Add(y.FSex());
                li.SubItems.Add(y.BreedName);
                li.Tag = y.ID;
            }
        }

        public void SetBreeds(Catalog breeds)
        {
            _breeds = breeds;
            foreach (KeyValuePair<int,string> kvp in _breeds)            
                cbBreeds.Items.Add(kvp.Value);
            
        }

        private void btChangeBreed_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvSuckers.SelectedItems.Count == 0) throw new Exception("Не выбрано ни одной строчки");
                if (lvSuckers.SelectedItems.Count > 1) throw new Exception("Выберите одну строчку");
                if (cbBreeds.SelectedIndex < 0) throw new Exception("Выберите породу");

                int yId = (int)lvSuckers.SelectedItems[0].Tag;
                RabNetEngRabbit yng = Engine.get().getRabbit(yId);

                if (MessageBox.Show(String.Format("Вы действительно хотите назначить{2:s}породу \"{0:s}\"{2:s}группе детей:{1:s}", cbBreeds.Text, yng.FullName,Environment.NewLine), 
                    "Подтверждение",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No) return;

                yng.Breed = getBreedID(cbBreeds.Text);
                yng.Commit();
                fill(true);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private int getBreedID(string breedName)
        {
            foreach (KeyValuePair<int, string> kvp in _breeds)
                if (kvp.Value == breedName)
                    return kvp.Key;
            throw new Exception("Порода не найдена");
        }

        private void lvSuckers_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Enabled = 
                cbBreeds.Enabled = 
                btChangeBreed.Enabled = lvSuckers.SelectedItems.Count > 0; 
        }
    }
}
