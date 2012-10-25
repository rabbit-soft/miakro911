using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class RISuckersPanel : UserControl
    {
        RabNetEngRabbit _rab = null;

        public RISuckersPanel()
        {
            InitializeComponent();
        }

        public void Fill(int rabId, RabNetEngRabbit rab)
        {
            _rab = rab;
            if (rabId <= 0) return;
            fill();
        }

        private void fill()
        {
            lvSuckers.Items.Clear();
            foreach (YoungRabbit y in Engine.db().GetYoungers(_rab.RabID))
            {
                ListViewItem li = lvSuckers.Items.Add(y.NameFull);
                li.SubItems.Add(y.Group.ToString());
                li.SubItems.Add(y.Age.ToString());
                li.SubItems.Add(y.FSex());
                li.SubItems.Add(y.BreedName);
                li.Tag = y.ID;
            }
        }

        private void btChangeBreed_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvSuckers.SelectedItems.Count == 0) throw new Exception("Не выбрано ни одной строчки");
                if (lvSuckers.SelectedItems.Count > 1) throw new Exception("Выберите одну строчку");

                int yId = (int)lvSuckers.SelectedItems[0].Tag;
                RabNetEngRabbit yng = Engine.get().getRabbit(yId);

                //todo смена породы
                //yng.Breed = 2;
                //yng.Commit();
                fill();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
