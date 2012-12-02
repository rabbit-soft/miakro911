using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class IncomeForm : Form
    {
        private Catalog _zones = null;
        private Catalog _breeds = null;
        private List<RabNetEngRabbit> rbs = new List<RabNetEngRabbit>();
        public IncomeForm()
        {
            InitializeComponent();
            initialHint();
            _zones = Engine.db().catalogs().getZones();
            _breeds = Engine.db().catalogs().getBreeds();
            if (_breeds.Count == 0)
                throw new ApplicationException(@"Справочник пород пуст
Добавьте породу в форме Пород (Вид->Породы)
и попытайтесь снова");
            fillZones();
        }

        private void initialHint()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(btMale,"Добавить привезенного самца");
            toolTip.SetToolTip(btFemale,"Добавить привезенную самку");
            toolTip.SetToolTip(btNoSex,"Добавить привезенного кролика с неопределенным полом");
            toolTip.SetToolTip(btDelete,"Удалить выделенную запись");
            toolTip.SetToolTip(button5,"Показать справочник зон");
            toolTip.SetToolTip(button6,"Добавить привезенных кроликов на ферму");
            toolTip.SetToolTip(button7,"Отменить привоз кроликов. Закрыть окно");
            toolTip.SetToolTip(btPassport,"Показать паспорт выделенного кролика");
            toolTip.SetToolTip(btReplace,"Назначить адрес выделенному кролику");
            toolTip.SetToolTip(zones,"Выбрать родину привезенных кроликов");
        }

        private void fillZones()
        {
            String tx = zones.Text;
            zones.Items.Clear();
            zones.Items.Add("");
            foreach (int k in _zones.Keys)
                zones.Items.Add(_zones[k]);
            zones.Text = tx;
        }

        private int getZone()
        {
            if (zones.Text=="") return 0;
            foreach (int k in _zones.Keys)
                if (zones.Text == _zones[k])
                    return k;
            return 0;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (new CatalogForm(CatalogForm.CatalogType.ZONES)).ShowDialog();
            fillZones();
        }

        private void update(bool plast)
        {
            listView1.Items.Clear();
            for (int i = 0; i < rbs.Count; i++)
            {
                String name = "Бесполые";
                if (rbs[i].Sex == Rabbit.SexType.MALE) name = rbs[i].Group==1 ? "Самец" : "Самцы";
                if (rbs[i].Sex == Rabbit.SexType.FEMALE) name = rbs[i].Group == 1 ? "Самка" : "Самки";
                ListViewItem li = listView1.Items.Add(name);
                li.SubItems.Add(rbs[i].Group.ToString());
                li.Tag=rbs[i];
                li.SubItems.Add(rbs[i].FullName);
                li.SubItems.Add(_breeds.ContainsKey(rbs[i].BreedID) ? _breeds[rbs[i].BreedID] : _breeds[1]);//ГИБРИД
                li.SubItems.Add(rbs[i].Address);
            }
            if (plast)
            {
                listView1.SelectedItems.Clear();
                ListViewItem it = listView1.Items[listView1.Items.Count - 1];
                it.Selected = true;
                (it.Tag as RabNetEngRabbit).Zone = getZone();
                btPassport.PerformClick();
            }
        }

        private void btMale_Click(object sender, EventArgs e)
        {
            rbs.Add(new RabNetEngRabbit(Engine.get(),Rabbit.SexType.MALE));
            update(true);
        }

        private void btFemale_Click(object sender, EventArgs e)
        {
            rbs.Add(new RabNetEngRabbit(Engine.get(), Rabbit.SexType.FEMALE));
            update(true);
        }

        private void btNoSex_Click(object sender, EventArgs e)
        {
            rbs.Add(new RabNetEngRabbit(Engine.get(), Rabbit.SexType.VOID));
            update(true);
        }

        private void btPassport_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            RabbitInfo dlg = new RabbitInfo(listView1.SelectedItems[0].Tag as RabNetEngRabbit);
            if (dlg.ShowDialog() == DialogResult.Cancel && (listView1.SelectedItems[0].Tag as RabNetEngRabbit).NameID==0)
                rbs.RemoveAt(rbs.Count-1);
            update(false);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btDelete.Enabled=btPassport.Enabled=btReplace.Enabled=(listView1.SelectedItems.Count==1);
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            int i=0;
            while (i < rbs.Count)
            {
                if (rbs[i] == (RabNetEngRabbit)listView1.SelectedItems[0].Tag)
                    rbs.RemoveAt(i);
                else i++;
            }
//            rbs.Remove(listView1.SelectedItems[0].Tag as RabNetEngRabbit);
            update(false);
        }

        private void btReplace_Click(object sender, EventArgs e)
        {
            ReplaceForm rpf = new ReplaceForm();
            foreach (RabNetEngRabbit r in rbs)
                rpf.AddRabbit(r);
            rpf.ShowDialog();
            update(false);
        }

        private bool isMom(RabNetEngRabbit r1)
        {
            return (r1.Sex == Rabbit.SexType.FEMALE && r1.Status > 0);
        }

        private void commit(RabNetEngRabbit r1)
        {
            if (r1.Tag != "done")
                r1.CommitNew();
            r1.Tag = "done";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                bool no_names = false;
                bool no_addresses = false;
                bool no_gens = false;
                bool same_names = false;
                foreach (RabNetEngRabbit r in rbs)
                {
                    if (r.NameID == 0 && r.SurnameID==0) no_names = true;
                    if (r.Address == Rabbit.NULL_ADDRESS) no_addresses = true;
                    if (r.Genoms == "") no_gens = true;
                    if (!same_names)
                    {
                        foreach (RabNetEngRabbit r2 in rbs)
                        {
                            if (r != r2 && r.NameID == r2.NameID)
                                same_names = true;
                        }
                    }
                }

                String msg="";
                if (no_names) msg = "У некоторых кроликов нет имени." + Environment.NewLine;
                if (no_addresses) msg += "У некоторых кроликов нет адреса." + Environment.NewLine;
                if (no_gens) msg += "У некоторых кроликов нет ни одного Номера Гена." + Environment.NewLine;
                if (same_names) msg += "Двум или более кроликам назначено одно имя." + Environment.NewLine;

                if (msg != "")
                {
                    MessageBox.Show(this, msg, "Нельзя продолжить", MessageBoxButtons.OK);
                    DialogResult = DialogResult.None;
                    return;
                    /*msg += "Продолжить?";
                    if (MessageBox.Show(this, msg, "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        DialogResult = DialogResult.None;
                        return;
                    }*/
                }
                foreach (RabNetEngRabbit r in rbs)
                {
                    foreach (RabNetEngRabbit r2 in rbs)
                    {
                        if (r2 != r && r2.NewAddress == r.NewAddress && r.NewAddress!="")
                        {
                            RabNetEngRabbit mom=r;
                            RabNetEngRabbit chl = r2;
                            if (isMom(r2)) { chl = r; mom = r2; }
                            commit(mom);
                            chl.Mom=mom.ID;
                            commit(chl);
                        }
                    }
                    commit(r);
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка " + ex.ToString());
            }
  
  }


    }

}
