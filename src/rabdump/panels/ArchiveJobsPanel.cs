using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using rabnet.RNC;
using gamlib;
#if PROTECTED
    using RabGRD;
#endif

namespace rabdump
{
    public partial class ArchiveJobsPanel : UserControl
    {
        private RabnetConfig _rnc;
        private Dictionary<int, DataSource> _ds_dict;
        private Dictionary<int, ArchiveJob> _aj_dict;
        private bool _manual = false;
        private int _sInd = -1;

        public ArchiveJobsPanel()
        {
            InitializeComponent();
            foreach (ArchiveType at in Enum.GetValues(typeof(ArchiveType)))
                cbArcType.Items.Add(at);
        }

        public void Init(RabnetConfig rnc)
        {
            btCancel_Click(null, null);

            _rnc = rnc;
            init();
        }

        private void init()
        {
            _manual = false;
            _ds_dict = new Dictionary<int, DataSource>();
            _aj_dict = new Dictionary<int, ArchiveJob>();
            cbName.Items.Clear();
            cbDataBase.Items.Clear();
            for (int i = 0; i < _rnc.ArchiveJobs.Count; i++) {
                _aj_dict.Add(i, _rnc.ArchiveJobs[i]);
                cbName.Items.Add(_rnc.ArchiveJobs[i].Name);
            }

            for (int i = 0; i < _rnc.DataSources.Count; i++) {
                _ds_dict.Add(i, _rnc.DataSources[i]);
                cbDataBase.Items.Add(_rnc.DataSources[i].Name);
            }


            if (cbName.Items.Count > 0) {
                cbName.SelectedIndex = 0;
                btEdit.Enabled = btDelete.Enabled = true;
            }
            _manual = true;
        }

        private void btDumpPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                tbDumpPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void cbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateSelectedAJ();
        }

        private void updateSelectedAJ()
        {
            ArchiveJob aj = _aj_dict[cbName.SelectedIndex];
            for (int i = 0; i < _ds_dict.Count; i++)
                if (aj.DataSrc == _ds_dict[i])
                    cbDataBase.SelectedIndex = i;
            tbDumpPath.Text = aj.DumpPath;
            nudCountLimit.Value = aj.CountLimit;
            nudSizeLimit.Value = aj.SizeLimit;
            cbArcType.SelectedIndex = aj.ArcType;
            dtpDate.Value =
                dtpTime.Value = aj.StartTime == DateTime.MinValue ? DateTime.Now : aj.StartTime;
            cbWeekDay.SelectedIndex = (int)aj.StartTime.DayOfWeek == 0 ? 6 : (int)aj.StartTime.DayOfWeek - 1;
            aj.SendToServ = chServerSend.Checked;
        }

        private void btAdd_CheckedChanged(object sender, EventArgs e)
        {
            //_sInd = -1;
            CheckBox cb = sender as CheckBox;
            cbName.DropDownStyle = cb.Checked ? ComboBoxStyle.Simple : ComboBoxStyle.DropDownList;
            gbAJProperties.Enabled = cb.Checked;
            showButtons(cb.Checked);
            if (cb == btAdd) {
                if (!cb.Checked && cbName.Items.Count > 0) {
                    cbName.SelectedIndex = 0;
                    updateSelectedAJ();
                } else {
                    cbName.Text = "";
                    tbDumpPath.Text = getDefaultDumpPath();
                    folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyDocuments;
                    cbArcType.SelectedIndex = 0;
                    nudCountLimit.Value = nudSizeLimit.Value = 0;
                }
                btEdit.Enabled = btDelete.Enabled = !btAdd.Checked;
            } else if (cb == btEdit) {
                btAdd.Enabled = btDelete.Enabled = !btEdit.Checked;
                if (cb.Checked) {
                    _sInd = cbName.SelectedIndex;
                } else {
                    cbName.SelectedIndex = _sInd;
                }
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            try {
                cbName.Text = cbName.Text.Replace(ArchiveJobThread.SPACE_REPLACE, '\0');
                if (cbName.Text == "") {
                    throw new Exception("Название не должно быть пустым");
                }
                if (cbDataBase.SelectedIndex == -1) {
                    throw new Exception("Не выбрана \"Ферма\"");
                }
                ArchiveJob aj;
                if (btAdd.Checked) {
                    if (isNameExists()) {
                        throw new Exception("Расписание с таким именем уже существует");
                    }

                    aj = new ArchiveJob(Guid.NewGuid().ToString(), cbName.Text, _ds_dict[cbDataBase.SelectedIndex], tbDumpPath.Text, getDT().ToLongDateString(),
                        cbArcType.SelectedIndex, (int)nudCountLimit.Value, (int)nudSizeLimit.Value, chServerSend.Checked, (int)nudSrvDump.Value);
                    _rnc.ArchiveJobs.Add(aj);
                } else if (btEdit.Checked) {
                    aj = _aj_dict[_sInd];
                    aj.Name = cbName.Text;
                    aj.DataSrc = _ds_dict[cbDataBase.SelectedIndex];
                    aj.DumpPath = tbDumpPath.Text;
                    aj.StartTime = getDT();
                    aj.ArcType = cbArcType.SelectedIndex;
                    aj.CountLimit = (int)nudCountLimit.Value;
                    aj.SizeLimit = (int)nudSizeLimit.Value;
                    aj.SendToServ = chServerSend.Checked;
                }
                //RabnetConfig.SaveDataSources();
                btCancel_Click(null, null);
                init();
            } catch (Exception exc) {
                MessageBox.Show(exc.Message);
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format("Вы уверены что хотите удалить расписание '{0:s}' ?", _aj_dict[cbName.SelectedIndex].Name), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }

            _rnc.ArchiveJobs.Remove(_aj_dict[cbName.SelectedIndex]);
            //RabnetConfig.SaveDataSources();
            init();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (btAdd.Checked) {
                btAdd.Checked = false;
            } else if (btEdit.Checked) {
                btEdit.Checked = false;
            }
        }

        private DateTime getDT()
        {
            DateTime dt = dtpDate.Value;
            TimeSpan tm = dtpTime.Value.TimeOfDay;
            return new DateTime(dt.Year, dt.Month, dt.Day, tm.Hours, tm.Minutes, tm.Seconds);
        }

        private bool isNameExists()
        {
            bool exists = false;
            foreach (ArchiveJob aj in _aj_dict.Values) {
                if (cbName.Text == aj.Name) {
                    exists = true;
                    break;
                }
            }
            return exists;
        }

        private void showButtons(bool p)
        {
            btOk.Visible =
                btCancel.Visible = p;
        }

        private void cbArcType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbWeekDay.Visible = cbArcType.SelectedIndex == (int)ArchiveType.Еженедельно;
            dtpDate.Visible = cbArcType.SelectedIndex != (int)ArchiveType.Никогда &&
                cbArcType.SelectedIndex != (int)ArchiveType.При_Запуске &&
                cbArcType.SelectedIndex != (int)ArchiveType.Еженедельно &&
                cbArcType.SelectedIndex != (int)ArchiveType.Ежедневно;
            dtpTime.Visible = cbArcType.SelectedIndex != (int)ArchiveType.Никогда &&
                cbArcType.SelectedIndex != (int)ArchiveType.При_Запуске;

            if (cbArcType.SelectedIndex == (int)ArchiveType.Ежемесячно) {
                dtpDate.Format = DateTimePickerFormat.Custom;
                dtpDate.CustomFormat = "dd";
                dtpDate.ShowUpDown = true;
            } else {
                dtpDate.ShowUpDown = false;
                dtpDate.Format = DateTimePickerFormat.Short;
            }
        }

        private void cbWeekDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_manual) {
                return;
            }

            int ind = cbWeekDay.SelectedIndex == 6 ? 0 : cbWeekDay.SelectedIndex + 1;
            //int add = (int)dtpDate.Value.DayOfWeek > ind ? 7 - (int)dtpDate.Value.DayOfWeek +ind : ind - (int)dtpDate.Value.DayOfWeek;
            int add = ind - (int)dtpDate.Value.DayOfWeek;
            dtpDate.Value = dtpDate.Value.AddDays(add);
        }

        private string getDefaultDumpPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Helper.PathCombine(path, RabnetConfig.MY_DOCUMENTS_APP_FOLDER, RabnetConfig.MY_DUMPS_FOLDER);
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        private void chServerSend_CheckedChanged(object sender, EventArgs e)
        {
            nudSrvDump.Enabled = chServerSend.Checked;
        }
    }
}
