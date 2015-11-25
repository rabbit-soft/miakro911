using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace rabnet.forms
{
    public partial class FuckForm : Form
    {
        private ILog _logger = LogManager.GetLogger(typeof(FuckForm));

        private const int IND_NAME = 0;
        private const int IND_STATE = 1;
        private const int IND_BREED = 2;
        private const int IND_FUCKS = 3;
        private const int IND_CHILDREN = 4;
        private const int IND_INBR = 5;

        private RabNetEngRabbit _rabFemale = null;
        private Catalog _breeds;
        private int _rabMaleId = 0;
        bool _manual = true;
        int _opt_malewait = 0;
        int _opt_makeCand = 0;
        ListViewColumnSorter cs;
        Catalog names = null;
        bool _needCommit = true;

        private FuckForm()
        {
            InitializeComponent();
            initialHints();
            dateDays1.DateValue = DateTime.Now;
            _breeds = Engine.db().catalogs().getBreeds();

            _manual = false;
            chCandidates.Checked = Engine.opt().getBoolOption(Options.OPT_ID.SHOW_CANDIDATES);
            chHetererosis.Checked = Engine.opt().getBoolOption(Options.OPT_ID.GETEROSIS);
            chInbreed.Checked = Engine.opt().getBoolOption(Options.OPT_ID.INBREEDING);
            _opt_malewait = Engine.opt().getIntOption(Options.OPT_ID.MALE_WAIT);
            _opt_makeCand = Engine.opt().getIntOption(Options.OPT_ID.MAKE_CANDIDATE);
            _manual = true;

            cs = new ListViewColumnSorter(listView1, new int[] { IND_FUCKS, IND_CHILDREN }, Options.OPT_ID.MAKE_FUCK_LIST);
            listView1.ListViewItemSorter = cs;
            FormSizeSaver.Append(this);
        }
        public FuckForm(int r1)
            : this(r1, 0)
        { }

        public FuckForm(int r1, int r2)
            : this()
        {
            _rabFemale = Engine.get().getRabbit(r1);
            _rabMaleId = r2;
            init();
        }
        public FuckForm(RabNetEngRabbit rab)
            : this()
        {
            _rabFemale = rab;
            init();
        }

        public FuckForm(RabNetEngRabbit rab, int r2, bool needCommit)
            : this()
        {
            _rabFemale = rab;
            _rabMaleId = r2;
            _needCommit = needCommit;
            init();
        }

        private void init()
        {
            label1.Text = _rabFemale.FullName;
            label2.Text = _rabFemale.BreedName;
            this.dateDays1.MinDate = _rabFemale.LastFuckOkrol;
            if (_rabFemale.EventDate > _rabFemale.LastFuckOkrol) {
                this.dateDays1.DateValue = _rabFemale.EventDate;
            }

            fillTable();
            if (_rabFemale.Status > 0) {
                this.Text = btOk.Text = "Вязать";
            }
            fillNames();
        }

        private void initialHints()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 1000;
            toolTip.SetToolTip(btGens, "Показать гены выбранного самца");
        }

        private void fillNames()
        {
            cbName.Items.Clear();
            names = Engine.db().catalogs().getFreeNames(2, _rabFemale.NameID);
            cbName.Items.Add("");
            cbName.SelectedIndex = 0;
            foreach (int key in names.Keys) {
                cbName.Items.Add(names[key]);
                if (key == _rabFemale.NameID) {
                    cbName.SelectedIndex = cbName.Items.Count - 1;
                }
            }
            cbName.Enabled = btNames.Enabled = _rabFemale.NameID == 0;
        }

        private void fillTable()
        {
            cs.PrepareForUpdate();
            Filters flt = new Filters();
            flt[Filters.RAB_ID] = _rabFemale.ID.ToString();
            flt[Filters.HETEROSIS] = chHetererosis.Checked ? "1" : "0";
            flt[Filters.INBREEDING] = chInbreed.Checked ? "1" : "0";
            flt[Filters.MALE_REST] = _opt_malewait.ToString();
            flt[Filters.MAKE_CANDIDATE] = _opt_makeCand.ToString();
            flt[Filters.SHOW_CANDIDATE] = chCandidates.Checked ? "1" : "0";
            flt[Filters.SHOW_REST] = chRest.Checked ? "1" : "0";
            //TODO здесь трахатели идеалогически неверно передаются через объекты Трахов
            FuckPartner[] fs = Engine.db().GetAllFuckers(flt);
            listView1.BeginUpdate();

            foreach (FuckPartner fP in fs) {
                bool heter = (fP.BreedId != _rabFemale.BreedID);
                bool inbr = RabNetEngHelper.inbreeding(_rabFemale.Genoms, fP.OldGenoms);

                ListViewItem li = listView1.Items.Add(fP.FullName);
                li.UseItemStyleForSubItems = false;
                if (fP.LastFuck != DateTime.MinValue && DateTime.Now < fP.LastFuck.Date.AddDays(_opt_malewait)) {
                    li.SubItems[IND_NAME].ForeColor = chRest.ForeColor;
                }
                li.Tag = fP;
                li.SubItems.Add("Мальчик");
                if (fP.Status == 1 || (fP.Status == 0 && fP.Age >= _opt_makeCand)) {
                    li.SubItems[IND_STATE].Text = "Кандидат";
                    li.SubItems[IND_STATE].ForeColor = chCandidates.ForeColor;
                }
                if (fP.Status == 2) {
                    li.SubItems[IND_STATE].Text = "Производитель";
                }
                li.SubItems.Add(_breeds[fP.BreedId]);
                li.SubItems.Add(fP.Fucks.ToString());
                li.SubItems.Add(fP.MutualChildren.ToString());
                li.SubItems.Add(inbr ? "ДА" : "-");

                int inbrLevel = 0;
                if (RabbitGen.DetectInbreeding(_rabFemale.RabGenoms, fP.RabGenoms, ref inbrLevel)) {
                    li.SubItems.Add(inbrLevel.ToString() + " поколение");
                } else {
                    li.SubItems.Add("-");
                }

                if (heter) {
                    li.SubItems[IND_BREED].ForeColor = chHetererosis.ForeColor;
                }
                if (inbr) {
                    li.SubItems[IND_INBR].ForeColor = chInbreed.ForeColor;
                }
                if (_rabMaleId == fP.Id) {
                    li.Selected = true;
                    li.EnsureVisible();
                }
            }
            // если был передан партнер, но его нет в списке, то обнуляем его, т.к. он не был выбран
            if (listView1.SelectedItems.Count == 0) {
                _rabMaleId = 0;
            } else {
                listView1_SelectedIndexChanged(null, null);
            }

            listView1.EndUpdate();
            cs.RestoreAfterUpdate();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btOk.Enabled = btGens.Enabled = (listView1.SelectedItems.Count == 1);
            MainForm.StillWorking();
        }

        private void btGens_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) {
                MessageBox.Show("Партнер не выбран");
                return;
            }
            MainForm.StillWorking();
            int r2 = (listView1.SelectedItems[0].Tag as FuckPartner).Id;
            (new GenomViewForm(_rabFemale.ID, r2)).ShowDialog();
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
            try {
                if (listView1.SelectedItems.Count != 1) {
                    throw new RabNetException("Выберите самца");
                }

                if (_rabFemale.NameID == 0 && cbName.SelectedIndex != 0) {
                    foreach (int k in names.Keys) {
                        if (cbName.Text == names[k]) {
                            _rabFemale.NameID = k;
                        }
                    }
                    _rabFemale.Commit();
                }

                _rabMaleId = (listView1.SelectedItems[0].Tag as FuckPartner).Id;
                if (_needCommit) {
                    _rabFemale.FuckIt(_rabMaleId, dateDays1.DaysValue, syntetic);
                }

                this.DialogResult = DialogResult.OK;
                Close();
            } catch (Exception ex) {
                DialogResult = DialogResult.None;
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            if (!_manual) {
                return;
            }

            if (sender == chCandidates) {
                Engine.opt().setOption(Options.OPT_ID.SHOW_CANDIDATES, chCandidates.Checked ? 1 : 0);
            }
            fillTable();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1) {
                btGens.PerformClick();
            }
        }

        private void btNames_Click(object sender, EventArgs e)
        {
            new NamesForm(1).ShowDialog();
            fillNames();
        }

        public int SelectedFucker { get { return _rabMaleId; } }

        public DateTime FuckDate { get { return dateDays1.DateValue; } }

    }
}
