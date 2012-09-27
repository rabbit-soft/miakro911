using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace rabnet.panels
{
    public partial class FarmsPanel : UserControl
    {
        private const int gbConn_DEF_Y = 128;
        private const int btCancelOK_DEF_Y = 274;
        private const int DEF_Y_SUB = 128 - 39;

        private bool _manual = false;
        private Dictionary<int, RabnetConfig.rabDataSource> _ds;
        private int _sInd = -1;

        public FarmsPanel()
        {
            InitializeComponent();
            showGBoxes(false);
            _manual = true;
        }

        public void UpdateList()
        {
            _manual = false;
            _ds = new Dictionary<int, RabnetConfig.rabDataSource>();
            cbName.Items.Clear();
            foreach (RabnetConfig.rabDataSource ds in RabnetConfig.DataSources)
            {
                cbName.Items.Add(ds.Name);
                _ds.Add(cbName.Items.Count - 1, ds);
            }
            if (cbName.Items.Count > 0)
            {
                cbName.SelectedIndex = 0;
                btEdit.Enabled = btDelete.Enabled = true;
            }
            _manual = true;
        }

        private void cbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_manual) return;
            updateSelectedItem();
        }

        private void updateSelectedItem()
        {            
            RabnetConfig.rabDataSource ds = _ds[cbName.SelectedIndex];
            tbHost.Text = ds.Params.Host;
            tbDB.Text = ds.Params.DataBase;
            tbUser.Text = ds.Params.User;
            tbPass.Text = ds.Params.Password;
        }

        private void btAdd_CheckedChanged(object sender, EventArgs e)
        {
            _sInd = -1;
            CheckBox cb = sender as CheckBox;
            cbName.DropDownStyle = cb.Checked ? ComboBoxStyle.Simple : ComboBoxStyle.DropDownList;
            showGBoxes(cb.Checked && cb == btAdd);
            setConnFieldsRO(!cb.Checked);
            showButtons(cb.Checked);
            if (cb == btAdd)
            {
                if (!cb.Checked)
                {
                    cbName.SelectedIndex = 0;
                    updateSelectedItem();
                }
                else cbName.Text = "";
                btEdit.Enabled = btDelete.Enabled = !btAdd.Checked;
            }
            else if (cb == btEdit)
            {
                btAdd.Enabled = btDelete.Enabled = !btEdit.Checked;
                _sInd = cbName.SelectedIndex;
            }
        }

        private void btEdit_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void showButtons(bool p)
        {
            btOk.Visible =
                btCancel.Visible = p;
        }

        private void showGBoxes(bool addStyle)
        {
            gbAdmin.Visible = addStyle;
            gbConn.Top = addStyle ? gbConn_DEF_Y : gbConn_DEF_Y - DEF_Y_SUB;
            btCancel.Top =
                btOk.Top = addStyle ? btCancelOK_DEF_Y : btCancelOK_DEF_Y - DEF_Y_SUB; ;
        }

        private void setConnFieldsRO(bool readOnly)
        {
            tbHost.ReadOnly =
                tbDB.ReadOnly =
                tbUser.ReadOnly =
                tbPass.ReadOnly = readOnly;
        }

        private void chCreate_CheckedChanged(object sender, EventArgs e)
        {
            tbAdmin.Enabled =  tbAdminPass.Enabled = chCreate.Checked;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (btAdd.Checked)
                btAdd.Checked = false;
            else if (btEdit.Checked)
                btEdit.Checked = false;
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format("Вы уверены что хотите удалить подключение '{0:s}' ?", _ds[cbName.SelectedIndex].Name), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            RabnetConfig.DataSources.Remove(_ds[cbName.SelectedIndex]);
            RabnetConfig.SaveDataSources();
            UpdateList();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            RabnetConfig.rabDataSource ds;
            if (btAdd.Checked)
            {
                if (chCreate.Checked)
                    runmia("nudb");
                if(isNameExists()) return;
                ds = new RabnetConfig.rabDataSource(Guid.NewGuid().ToString(), cbName.Text, tbHost.Text, tbDB.Text, tbUser.Text, tbPass.Text);
                RabnetConfig.SaveDataSource(ds);
            }
            else if (btEdit.Checked)
            {
                ds = _ds[_sInd];
                ds.Name = cbName.Text;
                ds.Params = new RabnetConfig.sParams(tbHost.Text, tbDB.Text, tbUser.Text, tbPass.Text);
            }
            RabnetConfig.SaveDataSources();
            btCancel_Click(null,null);
            UpdateList();
        }

        private bool isNameExists()
        {
            bool exists = false;
            foreach (RabnetConfig.rabDataSource ds in _ds.Values)
            {
                if (cbName.Text == ds.Name)
                {
                    exists = true;
                    break;
                }
            }
            return exists && MessageBox.Show("Подключение с таким именем уже существует\nПродолжить?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes;
        }

        /// <summary>
        /// Запустить mia_conv
        /// </summary>
        private bool runmia(String prm)
        {
            String prms = "\"" + prm + "\" " + tbHost.Text + ';' + tbDB.Text + ';' + tbUser.Text + ';' + tbPass.Text + ';' + tbAdmin.Text + ';' + tbAdminPass.Text;
            prms += " зоотехник;";
            try
            {
                String prg = Path.GetDirectoryName(Application.ExecutablePath) + @"\..\Tools\mia_conv.exe";
#if DEBUG
                if (!File.Exists(prg))//нужно для того чтобы из под дебага можно было запустить Mia_Conv
                {
                    string path = Path.GetFullPath(Application.ExecutablePath);
                    bool recurs = true;
                    string[] drives = Directory.GetLogicalDrives();
                    while (recurs)
                    {
                        foreach (string d in drives)
                        {
                            if (d.ToLower() == path)
                                recurs = false;
                        }
                        if (!recurs) break;
                        path = Directory.GetParent(path).FullName;
                        string[] dirs = Directory.GetDirectories(path);
                        if (Directory.Exists(path + @"\bin\protected\Tools"))
                        {
                            prg = path + @"\bin\protected\Tools\mia_conv.exe";
                            recurs = false;
                            break;
                        }
                    }
                }
#endif
                Process p = Process.Start(prg, prms);
                p.WaitForExit();
                if (p.ExitCode != 0)
                    throw new ApplicationException("Ошибка создания БД. " + miaExitCode.GetText(p.ExitCode));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        
    }
}
