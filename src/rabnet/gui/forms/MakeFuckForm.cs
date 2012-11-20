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

        private const int IND_NAME = 0;
        private const int IND_STATE = 1;
        private const int IND_BREED = 2;
        private const int IND_FUCKS = 3;
        private const int IND_CHILDREN = 4;
        private const int IND_INBR = 6;

        private RabNetEngRabbit rab1 = null;
        private Catalog brds;
        private int rtosel=0;
        bool _manual = true;
        int _malewait = 0;
        int _makeCand = 0;
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

            _manual = false;
            chCandidates.Checked = Engine.opt().getBoolOption(Options.OPT_ID.SHOW_CANDIDATES);
            chHetererosis.Checked = Engine.opt().getBoolOption(Options.OPT_ID.GETEROSIS);
            chInbreed.Checked = Engine.opt().getBoolOption(Options.OPT_ID.INBREEDING);
            _malewait = Engine.opt().getIntOption(Options.OPT_ID.MALE_WAIT);
            _makeCand = Engine.opt().getIntOption(Options.OPT_ID.MAKE_CANDIDATE);
            _manual = true;

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
            Filters flt = new Filters();
            flt[Filters.RAB_ID] = rab1.RabID.ToString();
            flt[Filters.HETEROSIS] = chHetererosis.Checked ? "1" : "0";
            flt[Filters.INBREEDING] = chInbreed.Checked ? "1" : "0";
            flt[Filters.MALE_WAIT] = _malewait.ToString();
            flt[Filters.MAKE_CANDIDATE] = _makeCand.ToString();
            flt[Filters.SHOW_CANDIDATE] = chCandidates.Checked ? "1" : "0";
            flt[Filters.SHOW_REST] = chRest.Checked ? "1" : "0";
            //TODO здесь трахатели идеалогически неверно передаются через объекты Трыхов
            Fucks fs = Engine.db().GetAllFuckers(flt);
            listView1.BeginUpdate();
            foreach (Fucks.Fuck f in fs.fucks)
            {
                bool heter=(f.Breed != rab1.Breed);
                bool inbr = RabNetEngHelper.inbreeding(rab1.Genom, f.rGenom);

                ListViewItem li = listView1.Items.Add(f.PartnerName);
                li.UseItemStyleForSubItems = false;
                if (f.When != DateTime.MinValue && f.When.Date.AddDays(_malewait) >= DateTime.Now.Date)
                    li.SubItems[IND_NAME].ForeColor = chRest.ForeColor;
                li.Tag = f;
                li.SubItems.Add("Мальчик");
                if (f.Dead == 1 || (f.Dead == 0 && f.Killed >= _makeCand))
                {
                    li.SubItems[IND_STATE].Text = "Кандидат";
                    li.SubItems[IND_STATE].ForeColor = chCandidates.ForeColor;
                }
                if (f.Dead==2)
                    li.SubItems[IND_STATE].Text = "Производитель";
                li.SubItems.Add(brds[f.Breed]);
                li.SubItems.Add(f.Times.ToString());
                li.SubItems.Add(f.Children.ToString());
                li.SubItems.Add(heter? "ДА" : "-");
                li.SubItems.Add(inbr ? "ДА" : "-");
                if (heter)
                    li.SubItems[IND_BREED].ForeColor = chHetererosis.ForeColor;
                if (inbr)
                    li.SubItems[IND_INBR].ForeColor = chInbreed.ForeColor;
                if (rtosel == f.PartnerId)
                 {
                    li.Selected = true;
                    li.EnsureVisible();
                 }
            }
            listView1.EndUpdate();
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
            int r2 = (listView1.SelectedItems[0].Tag as Fucks.Fuck).PartnerId;
            (new GenomViewForm(rab1.RabID, r2)).ShowDialog();
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

                int r2 = (listView1.SelectedItems[0].Tag as Fucks.Fuck).PartnerId;
                selected = r2;
                if (action == 0)
                    rab1.FuckIt(r2, dateDays1.DaysValue,syntetic);

                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (ApplicationException ex)
            {
                DialogResult = DialogResult.None;
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            if (!_manual) return;

            if(sender == chCandidates)
                Engine.opt().setOption(Options.OPT_ID.SHOW_CANDIDATES, chCandidates.Checked?1:0);
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
