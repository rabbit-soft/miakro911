using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using rabnet.RNC;

namespace rabnet
{
    public partial class FarmsPanel : UserControl
    {
        private const int gbConn_DEF_Y = 128;
        private const int btCancelOK_DEF_Y = 274;
        private const int DEF_Y_SUB = 128 - 39;

        private bool _manual = false;
        private RabnetConfig _rnc;
        private Dictionary<int, DataSource> _ds_dict;
        private int _sInd = -1;

        public FarmsPanel()
        {
            InitializeComponent();
            showGBoxes(false);
            _manual = true;
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
            int def = -1;
            cbName.Items.Clear();
            _ds_dict = new Dictionary<int, DataSource>();
            for (int i = 0; i < _rnc.DataSources.Count; i++)
            {
                _ds_dict.Add(i, _rnc.DataSources[i]);
                cbName.Items.Add(_rnc.DataSources[i].Name);
                if (_rnc.DataSources[i].Default)
                    def = i;
            }
         
            if (cbName.Items.Count > 0)
            {
                cbName.SelectedIndex = 0;
                btEdit.Enabled = btDelete.Enabled = true;
                if (def != -1)
                    cbName.SelectedIndex = def;
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
            DataSource ds = _ds_dict[cbName.SelectedIndex];
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
                if (!cb.Checked && cbName.Items.Count > 0)
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
            if (MessageBox.Show(String.Format("Вы уверены что хотите удалить подключение '{0:s}' ?", _ds_dict[cbName.SelectedIndex].Name), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            _rnc.DataSources.Remove(_ds_dict[cbName.SelectedIndex]);
            //RabnetConfig.SaveDataSources();
            init();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            cbName.Text = cbName.Text.Replace("+","");
            if (cbName.Text == "")
            {
                MessageBox.Show("Название не должно быть пустым");
                return;
            }
            DataSource ds;
            if (btAdd.Checked)
            {
                if (chCreate.Checked)
                    runmia("nudb");
                if(isNameExists())
                {
                    MessageBox.Show("Подключение с таким именем уже существует");
                    return;
                }
                ds = new DataSource(Guid.NewGuid().ToString(), cbName.Text, tbHost.Text, tbDB.Text, tbUser.Text, tbPass.Text);
                _rnc.SaveDataSource(ds);
            }
            else if (btEdit.Checked)
            {
                ds = _ds_dict[_sInd];
                ds.Name = cbName.Text;
                ds.Params = new sParams(tbHost.Text, tbDB.Text, tbUser.Text, tbPass.Text);
            }
            //RabnetConfig.SaveDataSources();
            btCancel_Click(null,null);
            init();
        }

        private bool isNameExists()
        {
            bool exists = false;
            foreach (DataSource ds in _ds_dict.Values)
            {
                if (cbName.Text == ds.Name)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }

        private bool runmia(String prm)
        {
            try
            {
                Run.DBCreate(prm, tbHost.Text, tbDB.Text, tbUser.Text, tbPass.Text, tbAdmin.Text, tbAdminPass.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return false;
            }
            return true;
        }
    }
}
