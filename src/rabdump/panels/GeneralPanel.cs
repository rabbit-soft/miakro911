using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabdump
{
    partial class GeneralPanel : UserControl
    {
        private Options _options;
        private bool _manual = false;

        public GeneralPanel()
        {
            InitializeComponent();
#if !DEBUG
            gbRemote.Visible = false; //пока нет смысла это показывать
#endif
        }

        public void Init(Options opts)
        {
            _manual = false;
            _options = opts;
            tbMysqlPath.Text = folderBrowserDialog1.SelectedPath = opts.MySqlPath;
            tb7zPath.Text =  opts.Path7Z;
            tbServerUrl.Text = opts.ServerUrl;
            if (tb7zPath.Text!="")
                openFileDialog1.InitialDirectory = System.IO.Path.GetDirectoryName(tb7zPath.Text);
            chStartUp.Checked = opts.StartAtStart;
            _manual = true;
        }

        private void btMysqlPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                tbMysqlPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void bt7ZipPath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tb7zPath.Text = openFileDialog1.FileName;
        }

        private void optValue_Changed(object sender, EventArgs e)
        {
            if (!_manual) return;
            if (sender == tbMysqlPath)
                _options.MySqlPath = tbMysqlPath.Text;
            else if (sender == tb7zPath)
                _options.Path7Z = tb7zPath.Text;
            else if (sender == chStartUp)
                _options.StartAtStart = chStartUp.Checked;
            else if (sender == tbServerUrl) //TODO лучше сделать по окончанию редактирования
                _options.ServerUrl = tbServerUrl.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
