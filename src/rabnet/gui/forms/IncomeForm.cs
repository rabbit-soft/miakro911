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
        private Catalog zns = null;
        private Catalog brd = null;
        private List<RabNetEngRabbit> rbs = new List<RabNetEngRabbit>();
        public IncomeForm()
        {
            InitializeComponent();
            initialHint();
            zns = Engine.db().catalogs().getZones();
            brd = Engine.db().catalogs().getBreeds();
            if (brd.Count == 0)
                throw new ApplicationException(@"Справочник пород пуст
Добавьте породу в форме Пород (Вид->Породы)
и попытайтесь снова");
            fillZones();
        }

        private void initialHint()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(button1,"Добавить привезенного самца");
            toolTip.SetToolTip(button2,"Добавить привезенную самку");
            toolTip.SetToolTip(button3,"Добавить привезенного кролика с неопределенным полом");
            toolTip.SetToolTip(button4,"Удалить выделенную запись");
            toolTip.SetToolTip(button5,"Показать справочник зон");
            toolTip.SetToolTip(button6,"Добавить привезенных кроликов на ферму");
            toolTip.SetToolTip(button7,"Отменить привоз кроликов. Закрыть окно");
            toolTip.SetToolTip(button8,"Показать паспорт выделенного кролика");
            toolTip.SetToolTip(button9,"Назначить адрес выделенному кролику");
            toolTip.SetToolTip(zones,"Выбрать родину привезенных кроликов");
        }

        private void fillZones()
        {
            String tx = zones.Text;
            zones.Items.Clear();
            zones.Items.Add("");
            foreach (int k in zns.Keys)
                zones.Items.Add(zns[k]);
            zones.Text = tx;
        }

        private int getZone()
        {
            if (zones.Text=="") return 0;
            foreach (int k in zns.Keys)
                if (zones.Text == zns[k])
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
                if (rbs[i].Sex == OneRabbit.RabbitSex.MALE) name = rbs[i].Group==1 ? "Самец" : "Самцы";
                if (rbs[i].Sex == OneRabbit.RabbitSex.FEMALE) name = rbs[i].Group == 1 ? "Самка" : "Самки";
                ListViewItem li = listView1.Items.Add(name);
                li.SubItems.Add(rbs[i].Group.ToString());
                li.Tag=rbs[i];
                li.SubItems.Add(rbs[i].FullName);
                li.SubItems.Add(brd[rbs[i].Breed]);
                li.SubItems.Add(rbs[i].Address);
            }
            if (plast)
            {
                listView1.SelectedItems.Clear();
                ListViewItem it = listView1.Items[listView1.Items.Count - 1];
                it.Selected = true;
                (it.Tag as RabNetEngRabbit).Zone = getZone();
                button8.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rbs.Add(new RabNetEngRabbit(Engine.get(),OneRabbit.RabbitSex.MALE));
            update(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rbs.Add(new RabNetEngRabbit(Engine.get(), OneRabbit.RabbitSex.FEMALE));
            update(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rbs.Add(new RabNetEngRabbit(Engine.get(), OneRabbit.RabbitSex.VOID));
            update(true);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            (new RabbitInfo(listView1.SelectedItems[0].Tag as RabNetEngRabbit)).ShowDialog();
            update(false);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button4.Enabled=button8.Enabled=button9.Enabled=(listView1.SelectedItems.Count==1);
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button9_Click(object sender, EventArgs e)
        {
            ReplaceForm rpf = new ReplaceForm();
            foreach (RabNetEngRabbit r in rbs)
                rpf.addRabbit(r);
            rpf.ShowDialog();
            update(false);
        }

        private bool isMom(RabNetEngRabbit r1)
        {
            return (r1.Sex == OneRabbit.RabbitSex.FEMALE && r1.Status > 0);
        }

        private void commit(RabNetEngRabbit r1)
        {
            if (r1.Tag != "done")
                r1.newCommit();
            r1.Tag = "done";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                bool no_names = false;
                bool no_addresses = false;
                bool no_gens = false;
                foreach (RabNetEngRabbit r in rbs)
                {
                    if (r.Name == 0 && r.Surname==0) no_names = true;
                    if (r.Address == OneRabbit.NullAddress) no_addresses = true;
                    if (r.Genom == "") no_gens = true;
                }
                String msg="";
                if (no_names) msg = "У некоторых кроликов нет имени.\n";
                if (no_addresses) msg += "У некоторых кроликов нет адреса.\n";
                if (no_gens) msg += "У некоторых кроликов нет ни одного Номера Гена";
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
                            chl.mom=mom.RabID;
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
