using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using rabnet.forms;

namespace rabnet
{
    public partial class RIFucksPanel : UserControl
    {
        private const int IND_PARTNER = 2;
        const String RABDEAD = "Списан";
        internal event RIHandler UpdateRequire;
        internal event RIHandler UncanceledEvent;
        private RabNetEngRabbit _rab = null;

        public RIFucksPanel()
        {
            InitializeComponent();
        }

        public void SetRabbit(RabNetEngRabbit rab)
        {
            _rab = rab;
            UpdateData();
        }

        public void UpdateData()
        {
            if (_rab == null) return;
            ///Заполнение списка случек
            lvFucks.Items.Clear();
            if (_rab.ID > 0)
                foreach (Fuck f in Engine.db().GetFucks(new Filters(Filters.RAB_ID + "=" + _rab.ID)))
                {
                    ListViewItem li = lvFucks.Items.Add(f.EventDate == DateTime.MinValue ? "-" : f.EventDate.ToShortDateString());
                    li.SubItems.Add(Fuck.GetFuckTypeStr(f.FType,false));
                    li.SubItems.Add(string.IsNullOrEmpty(f.PartnerName) ? "-" : f.PartnerName);
                    
                    li.SubItems.Add(Fuck.GetFuckEndTypeStr(f.FEndType,false));
                    if (f.IsPartnerDead)
                    {
                        li.UseItemStyleForSubItems = false;
                        li.SubItems[IND_PARTNER].ForeColor = Color.Brown;
                    }
                    
                    li.SubItems.Add(f.EndDate == DateTime.MinValue ? "-" : f.EndDate.ToShortDateString());
                    li.SubItems.Add(f.Children.ToString());
                    li.SubItems.Add(f.Dead.ToString());
                    li.SubItems.Add(f.Killed.ToString());
                    li.SubItems.Add(f.Added.ToString());
                    li.SubItems.Add(f.Breed == _rab.BreedID ? "-" : "Да");
                    li.SubItems.Add(RabNetEngHelper.inbreeding(f.rGenom, _rab.Genoms) ? "Да" : "-");
                    li.SubItems.Add(f.Worker);
                    li.Tag = f;
                }
            changeFucker.Enabled = false;
        }

        private void cancelFuckEnd_Click(object sender, EventArgs e)
        {
            if (lvFucks.SelectedItems.Count != 1) return;
            try
            {
                Fuck f = (lvFucks.SelectedItems[0].Tag as Fuck);
                if (!isLastEvent(f))                
                    throw new Exception("Отменить прохолостание можно лишь последней записи");                    
                
                if (MessageBox.Show(String.Format("Данная функция отменит {0:s} текущей крольчихи и восстановит сукрольность.{1:s}Продолжить?",Fuck.GetFuckEndTypeStr(f.FEndType,false),Environment.NewLine),
                     "Отмена прохолоста", MessageBoxButtons.YesNo) == DialogResult.No) return;
                _rab.CancelFuckEnd(f);               
                cancelUnable();
                this.updateData();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btGens_Click(object sender, EventArgs e)
        {
            Fuck f = lvFucks.SelectedItems[0].Tag as Fuck;
            //String nm = lbName.Text.Split(':')[1];
            //if (lbSecname.Text != "")
            //    nm += " " + lbSecname.Text.Split(':')[1];
            //if (lbSurname.Text != "")
            //    nm += "-" + lbSurname.Text.Split(':')[1];
            //(new GenomView(_rab.Breed, f.breed, _rab.Genom, f.rgenom, nm, f.partner)).ShowDialog();
            (new GenomViewForm(_rab.ID, f.PartnerId)).ShowDialog();
        }

        private void btFuckHer_Click(object sender, EventArgs e)
        {
            Fuck f = lvFucks.SelectedItems[0].Tag as Fuck;
            MakeFuckForm dlg = new MakeFuckForm(_rab.ID, f.PartnerId);
            if (dlg.ShowDialog() == DialogResult.OK)
                cancelUnable();
            cancelUnable();
            updateData();
        }

        private void changeFucker_Click(object sender, EventArgs e)
        {
            if (lvFucks.SelectedItems.Count != 1) return;
            Fuck f = lvFucks.SelectedItems[0].Tag as Fuck;
            MakeFuckForm mf = new MakeFuckForm(_rab.ID, f.PartnerId, 1);
            if (mf.ShowDialog() == DialogResult.OK && mf.SelectedFucker != f.Id)
                Engine.db().changeFucker(f.Id, mf.SelectedFucker);
            cancelUnable();
            updateData();
        }

        private void changeWorker_Click(object sender, EventArgs e)
        {
            if (lvFucks.SelectedItems.Count != 1) return;
            Fuck f = lvFucks.SelectedItems[0].Tag as Fuck;
            SelectUserForm sf = new SelectUserForm(f.Worker);
            if (sf.ShowDialog() == DialogResult.OK && sf.SelectedUser != 0 && sf.SelectedUserName != f.Worker)
                Engine.db().changeWorker(f.Id, sf.SelectedUser);
            cancelUnable();
            updateData();
        }

        private void fucks_SelectedIndexChanged(object sender, EventArgs e)
        {
            btChangeWorker.Enabled = changeFucker.Enabled = btGens.Enabled = btFuckHer.Enabled = false;
            if (lvFucks.SelectedItems.Count != 1) return;

            bool dead = (lvFucks.SelectedItems[0].SubItems[3].Text == RABDEAD);
            btGens.Enabled = true;
            btFuckHer.Enabled = (_rab.EventDate != DateTime.MinValue) && !dead;            
            changeFucker.Enabled = lvFucks.SelectedItems[0].SubItems[3].Text == Fuck.GetFuckEndTypeStr(FuckEndType.Sukrol);//todo не правильно
            btCancelFuckEnd.Enabled = lvFucks.SelectedItems[0].SubItems[3].Text != Fuck.GetFuckEndTypeStr(FuckEndType.Sukrol,false) && (lvFucks.SelectedItems[0].Index == lvFucks.Items.Count - 1);
            btChangeWorker.Enabled = true;
        }

        private void updateData()
        {
            if(UpdateRequire!=null)
                UpdateRequire();
        }

        private void cancelUnable()
        {
            if (UncanceledEvent != null)
                UncanceledEvent();
        }

        private bool isLastEvent(Fuck f)
        {
            foreach (ListViewItem lvi in lvFucks.Items)
            {
                if ((lvi.Tag as Fuck).Id > f.Id)
                    return false;
            }
            return true;
        }

        private void fucks_DoubleClick(object sender, EventArgs e)
        {
            if (lvFucks.SelectedItems.Count == 1)
                btGens.PerformClick();
        }
    }  
}
