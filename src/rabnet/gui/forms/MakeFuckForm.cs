using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace rabnet
{
    public partial class MakeFuckForm : Form
    {
        private ILog _logger = LogManager.GetLogger(typeof(MakeFuckForm));

        private const int IND_FUCKS = 3;
        private const int IND_CHILDREN = 4;

        private RabNetEngRabbit rab1 = null;
        private Catalog brds;
        private int rtosel=0;
        bool manual = true;
        int malewait = 0;
        ListViewColumnSorter cs;
        Catalog names = null;
        int selected = 0;
        int action = 0;

        public MakeFuckForm()
        {
            InitializeComponent();
            initialHints();
            dateDays1.DateValue = DateTime.Now;
            brds = Engine.db().catalogs().getBreeds();
            manual = false;
            cbHeter.Checked=(Engine.opt().getIntOption(Options.OPT_ID.GETEROSIS)==1);
            cbInbreed.Checked = (Engine.opt().getIntOption(Options.OPT_ID.INBREEDING) == 1);
            malewait = Engine.opt().getIntOption(Options.OPT_ID.MALE_WAIT);
            manual = true;
            cs = new ListViewColumnSorter(listView1, new int[] { IND_FUCKS, IND_CHILDREN }, Options.OPT_ID.MAKE_FUCK_LIST);
            listView1.ListViewItemSorter = cs;
            FormSizeSaver.Append(this);
        }
        public MakeFuckForm(int r1): this(r1,0) { }

        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(btGens, "Показать гены выбранного самца");
        }

        private void fillNames()
        {
            cbName.Items.Clear();
            names = Engine.db().catalogs().getFreeNames(2, rab1.Name);
            cbName.Items.Add("");
            cbName.SelectedIndex = 0;
            foreach (int key in names.Keys)
            {
                cbName.Items.Add(names[key]);
                if (key == rab1.Name)
                    cbName.SelectedIndex = cbName.Items.Count - 1;
            }
            cbName.Enabled = btNames.Enabled = rab1.Name == 0;
        }

        public MakeFuckForm(int r1, int r2):this()
        {
            rab1 = Engine.get().getRabbit(r1);
            label1.Text = rab1.FullName;
            label2.Text = rab1.BreedName;
            rtosel = r2;
            fillTable();
            if (rab1.Status > 0)
                Text = btOk.Text = "Вязать";
            fillNames();
        }
        public MakeFuckForm(int r1, int r2, int action):this(r1,r2)
        {
            this.action = action;
        }

        private void fillTable()
        {
            cs.Prepare();
            Fucks fs = Engine.db().GetAllFuckers(rab1.RabID,cbHeter.Checked,cbInbreed.Checked,malewait);
#if DEBUG
            _logger.Debug("Starting to fill fucker list");
#endif
            listView1.BeginUpdate();
            foreach (Fucks.Fuck f in fs.fucks)
            {
                bool heter=(f.breed != rab1.Breed);
                bool inbr=RabNetEngHelper.inbreeding(rab1.Genom,f.rgenom);
                /*
                if ((!inbr || cbInbreed.Checked) && (!heter || cbHeter.Checked) &&
                    (f.dead>1 || cbCand.Checked) || f.partnerid==rtosel)
                {
                 */
                ListViewItem li = listView1.Items.Add(f.partner);
                li.Tag = f;
                String stat="Мальчик";
                if (f.dead==1)
                    stat="Кандидат";
                if (f.dead==2)
                    stat="Производитель";
                li.SubItems.Add(stat);
                li.SubItems.Add(brds[f.breed]);
                li.SubItems.Add(f.times.ToString());
                li.SubItems.Add(f.children.ToString());
                li.SubItems.Add(heter? "ДА" : "-");
                li.SubItems.Add(inbr ? "ДА" : "-");
                if (rtosel == f.partnerid)
                 {
                    li.Selected = true;
                    li.EnsureVisible();
                 }
                //}
            }
            listView1.EndUpdate();
#if DEBUG
            _logger.Debug("end to fill fucker list");
#endif
            cs.Restore();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btOk.Enabled=btGens.Enabled=(listView1.SelectedItems.Count==1);
        }

        private void btGens_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Партнер не выбран");
                return;
            }
            int r2 = (listView1.SelectedItems[0].Tag as Fucks.Fuck).partnerid;
            (new GenomView(rab1.RabID, r2)).ShowDialog();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            makeFuck(false);
        }

        private void miSyntetic_Click(object sender, EventArgs e)
        {
            makeFuck(true);
        }

        private void makeFuck(bool syntetic)
        {
            try
            {
                if (listView1.SelectedItems.Count != 1)
                    throw new ApplicationException("Выберите самца");
                if (rab1.Name == 0 && cbName.SelectedIndex != 0)
                {
                    foreach (int k in names.Keys)
                        if (cbName.Text == names[k])
                            rab1.Name = k;
                    rab1.Commit();
                }

                int r2 = (listView1.SelectedItems[0].Tag as Fucks.Fuck).partnerid;
                selected = r2;
                if (action == 0)
                    rab1.FuckIt(r2, dateDays1.DateValue,syntetic);

                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (ApplicationException ex)
            {
                DialogResult = DialogResult.None;
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void cbCand_CheckedChanged(object sender, EventArgs e)
        {
            if (!manual)
                return;
            fillTable();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count==1)
                btGens.PerformClick();
        }

        private void btNames_Click(object sender, EventArgs e)
        {
            new NamesForm(1).ShowDialog();
            fillNames();
        }

        public int SelectedFucker { get { return selected; } }
    }
}
