#if DEBUG
#define NOCATCH
#endif

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet.forms
{
    public partial class KillForm : Form
    {
        private const int IND_AGE = 4;
        private const int IND_COUNT = 5;
        private const int IND_KILL_COUNT = 6;

        private List<RabNetEngRabbit> rbs = new List<RabNetEngRabbit>();
        private List<int> youngers = new List<int>();
        bool confirm = true;
        private ListViewColumnSorter cs = null;
        public KillForm()
        {
            InitializeComponent();
            initialHints();
            dateDays1.DateValue = DateTime.Now;
            confirm = Engine.opt().getIntOption(Options.OPT_ID.CONFIRM_KILL) == 1;
            Catalog c = Engine.db().catalogs().getDeadReasons();
            cbDeadReason.Items.Add("");
            for (int i = 0; i < c.Count; i++) {
                cbDeadReason.Items.Add(c[i + 3]);
            }
            cs = new ListViewColumnSorter(listView1, new int[] { IND_AGE, IND_COUNT, IND_KILL_COUNT }, Options.OPT_ID.KILL_LIST);
            update();
            FormSizeSaver.Append(this);
        }
        /// <summary>
        /// Установка подсказок на компоненты
        /// </summary>
        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(numericUpDown1, "Количество списываемых кроликов из выделенной записи");
            toolTip.SetToolTip(button1, "Убрать выделенную запись из плана списаний");
            toolTip.SetToolTip(button2, "Списать кроликов");
            toolTip.SetToolTip(dateDays1, "Дата списания");
            toolTip.SetToolTip(cbDeadReason, "Причина списания кроликов");
            toolTip.SetToolTip(textBox1, "Здесь можно оставить любой коментарий по данному списанию");
        }
        public void addRabbit(int id)
        {
            addRabbit(Engine.get().getRabbit(id));

        }

        public void addRabbit(RabNetEngRabbit r)
        {
            rbs.Add(r);
            int id = rbs.Count - 1;
            r.Tag = r.Address;
            foreach (YoungRabbit or in r.Youngers) {
                addRabbit(or.ID);
                rbs[rbs.Count - 1].Tag = r.Address;
            }
            //update();
        }

        public void update()
        {
            cs.PrepareForUpdate();
            listView1.Items.Clear();
            foreach (RabNetEngRabbit r in rbs) {
                ListViewItem li = listView1.Items.Add(r.FullName);
                li.SubItems.Add(r.BreedName);
                li.Tag = r.ID;
                li.SubItems.Add(r.Tag);
                String sex = "?";
                if (r.Sex == Rabbit.SexType.FEMALE) sex = "Ж";
                if (r.Sex == Rabbit.SexType.MALE) sex = "М";
                li.SubItems.Add(sex);
                li.SubItems.Add(r.Age.ToString());
                li.SubItems.Add(r.Group.ToString());
                li.SubItems.Add(r.Group.ToString());
            }
            //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            cs.RestoreAfterUpdate();
            updateLabels();
        }

        private void KillForm_Load(object sender, EventArgs e)
        {
            update();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = false;
            button1.Enabled = false;
            if (listView1.SelectedItems.Count == 1) {
                numericUpDown1.Maximum = int.Parse(listView1.SelectedItems[0].SubItems[IND_COUNT].Text);
                numericUpDown1.Value = int.Parse(listView1.SelectedItems[0].SubItems[IND_KILL_COUNT].Text);
                numericUpDown1.Enabled = true;
                button1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool doConfirm()
        {
            if (!confirm) return true;
            return MessageBox.Show(this, "Списать " + updateLabels().ToString() + " кроликов?", "Подтверждение",
                MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        private void button2_Click(object sender, EventArgs e)
        {
#if !NOCATCH
            try {
#endif
                if (cbDeadReason.SelectedIndex == -1) {
                    MessageBox.Show("Вам необходимо указать причину списания для продолжения", "Не указана причина списания");
                    this.DialogResult = DialogResult.None;
                    cbDeadReason.Focus();
                    return;
                }
                int i = 0;
                if (!doConfirm()) {
                    DialogResult = DialogResult.None;
                    return;
                }
                foreach (ListViewItem li in listView1.Items) {
                    RabNetEngRabbit r = Engine.get().getRabbit((int)li.Tag);
                    int cnt = int.Parse(listView1.Items[i].SubItems[IND_KILL_COUNT].Text);
                    if (cnt != 0) {
                        int reason = 0;
                        if (cbDeadReason.SelectedIndex > 0) {
                            reason = cbDeadReason.SelectedIndex + 2;//1- списан из старой программы; 2- Обьединение
                        }
                        r.KillIt(dateDays1.DaysValue, reason, r.AddressSmall + " " + (textBox1.Text != "" ? textBox1.Text : cbDeadReason.Text), cnt);
                    }
                    i++;
                }
                this.DialogResult = DialogResult.OK;
                Close();
#if !NOCATCH
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
#endif
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            listView1.SelectedItems[0].SubItems[IND_KILL_COUNT].Text = numericUpDown1.Value.ToString();
            updateLabels();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1) return;
            int id = (int)listView1.SelectedItems[0].Tag;
            foreach (RabNetEngRabbit r in rbs) {
                if (r.ID == id) {
                    rbs.Remove(r);
                    update();
                    return;
                }
            }
        }

        private int updateLabels()
        {
            int str = 0;
            int cnt = 0;
            for (int i = 0; i < listView1.Items.Count; i++) {
                int c = int.Parse(listView1.Items[i].SubItems[IND_KILL_COUNT].Text);
                cnt += c;
                str += (c != 0 ? 1 : 0);
            }
            label4.Text = "Строк: " + str.ToString();
            label5.Text = "Кроликов: " + cnt.ToString();
            return cnt;
        }
    }
}
