using System;
using System.Windows.Forms;
using System.IO;

namespace rabdump
{
    public partial class RestoreForm : Form
    {
        private ArchiveJob jj = null;
        public RestoreForm()
        {
            InitializeComponent();
            setMode(true);
            foreach (ArchiveJob j in Options.get().Jobs)
                comboBox1.Items.Add(j.Name);
            if (comboBox1.Items.Count>0)
                comboBox1.SelectedIndex = 0;
        }

        public RestoreForm(String place):this()
        {
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if ((string)comboBox1.Items[i] == place)
                    comboBox1.SelectedIndex = i;
            }
        }

        public void setMode(bool small)
        {
            Height = (small ? 410 : 550);
            button3.Text="Расширенный режим "+(small?">>":"<<");
            groupBox1.Visible = !small;
        }

        private void button3_Click(object sender, EventArgs e)
        {
           setMode(Height==550);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            listView1.Items.Clear();
            foreach (ArchiveJob j in Options.get().Jobs)
                if (j.Name == comboBox1.Text)
                {
                    jj = j;
                    if (j.DB == DataBase.AllDataBases)
                    {
                        foreach (DataBase db in Options.get().Databases)
                            comboBox2.Items.Add(db.Name);
                    }
                    else
                        comboBox2.Items.Add(j.DB.Name);
                }
            comboBox2.SelectedIndex = 0;
        }

        private void fillList(ArchiveJob j, String db)
        {
            listView1.Items.Clear();
            DirectoryInfo di = new DirectoryInfo(j.BackupPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                String[] nm = Path.GetFileName(fi.FullName).Split('_');
                if (nm.Length == ArchiveJobThread.SPLIT_NAMES && nm[0]==j.Name && nm[1]==db)
                {
                    string[] hms = new string[3] { nm[5].Substring(0, 2), nm[5].Substring(2, 2), nm[5].Substring(4, 2) };
                    DateTime dtm = new DateTime(int.Parse(nm[2]), int.Parse(nm[3]), int.Parse(nm[4]), int.Parse(hms[0]), int.Parse(hms[1]), int.Parse(hms[2]));
                    int idx=0;
                    for (int i = 0; i < listView1.Items.Count;i++ )
                    {
                        if (DateTime.Parse(listView1.Items[i].SubItems[0].Text) > dtm)
                            idx++;
                    }
                    ListViewItem li=listView1.Items.Insert(idx,dtm.ToShortDateString()+" "+dtm.ToLongTimeString());
                    li.SubItems.Add(Path.GetFileName(fi.FullName));
                }
            }
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBase db = jj.DB;
            if (db == DataBase.AllDataBases)
                foreach (DataBase d in Options.get().Databases)
                    if (d.Name == comboBox2.Text) db = d;
            tbHost.Text = db.Host;
            tbDB.Text = db.DBName;
            tbUser.Text = db.User;
            tbPassword.Text = db.Password;
            fillList(jj, comboBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbFile.Text = openFileDialog1.FileName;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count!=1) return;
            tbFile.Text=jj.BackupPath+"\\"+listView1.SelectedItems[0].SubItems[1].Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ArchiveJobThread.undumpdb(tbHost.Text, tbDB.Text, tbUser.Text, tbPassword.Text, tbFile.Text);
                MessageBox.Show("Восстановление завершено");
                Close();
            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.None;
                MessageBox.Show(ex.Message);
            }
        }
    }
}
