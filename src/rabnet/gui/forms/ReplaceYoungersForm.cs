using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class ReplaceYoungersForm : Form
    {
        private RabNetEngRabbit r = null;
        public ReplaceYoungersForm()
        {
            InitializeComponent();
        }

        public ReplaceYoungersForm(int rid):this()
        {
            r = Engine.get().getRabbit(rid);
            label1.Text = r.fullName;
            label2.Text = "Возраст: " + r.age.ToString();
            label3.Text = "Количество:" + r.group.ToString();
            label5.Text = "Порода: " + r.breedName;
        }

        public void updateMothers()
        {
            int ad=Engine.opt().getIntOption(Options.OPT_ID.COMBINE_AGE);
            listView1.Items.Clear();
            foreach(Rabbit rb in Engine.db().getMothers(r.age, ad))
            {
                ListViewItem li = listView1.Items.Add(rb.fname);
                li.SubItems.Add(rb.fage.ToString());
                li.SubItems.Add(rb.fbreed);
                li.SubItems.Add(rb.fstatus);
                li.SubItems.Add(rb.frate.ToString());
                li.SubItems.Add(rb.fN);
                li.SubItems.Add(rb.faverage.ToString());
                li.Tag=rb.fid;
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            try
            {
                r.placeSuckerTo((int)listView1.SelectedItems[0].Tag);
                Close();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReplaceYoungersForm_Load(object sender, EventArgs e)
        {
            updateMothers();
        }
    }
}
