using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using pEngine;

namespace AdminGRD
{
    public partial class NewKeyForm : Form
    {
        public NewKeyForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read);
                byte[] key = new byte[fs.Length];
                fs.Read(key, 0, (int)fs.Length);
                fs.Close();
                Engine.MakeUserFile((int)nudUID.Value, tbName.Text, tbPass.Text, Convert.ToBase64String(key));
                this.Close();
            }
        }

        private void btFromHex_Click(object sender, EventArgs e)
        {
            string hexKey = tbHexKey.Text.Trim();
            byte[] key = new byte[hexKey.Length / 2];
            string ch;
            for (int i = 0; i < hexKey.Length; i += 2)
            {
                ch = hexKey[i].ToString() + hexKey[i + 1].ToString();
                key[i / 2] = Convert.ToByte(ch, 16);
            }
            Engine.MakeUserFile((int)nudUID.Value, tbName.Text, tbPass.Text, Convert.ToBase64String(key));
            this.Close();
        }
    }
}
