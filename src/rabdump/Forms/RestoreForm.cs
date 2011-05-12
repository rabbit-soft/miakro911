using System;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using log4net;

namespace rabdump
{
    public partial class RestoreForm : Form
    {
        private ArchiveJob _jj = null;

        private Thread _thrRestore;

        private readonly WaitForm _wtFrm;

        private static readonly ILog log = LogManager.GetLogger(typeof(RestoreForm));

        public RestoreForm()
        {
            InitializeComponent();
            SetMode(true);
            foreach (ArchiveJob j in Options.Get().Jobs)
            {
                comboBox1.Items.Add(j.Name);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            _wtFrm = new WaitForm();
            _wtFrm.SetName("Восстановление БД...");

        }

        public RestoreForm(String place):this()
        {
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if ((string)comboBox1.Items[i] == place)
                    comboBox1.SelectedIndex = i;
            }
        }

        public void SetMode(bool small)
        {
            Height = (small ? 410 : 550);
            button3.Text="Расширенный режим "+(small?">>":"<<");
            groupBox1.Visible = !small;
        }

        private void button3_Click(object sender, EventArgs e)
        {
           SetMode(Height==550);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            listView1.Items.Clear();
            foreach (ArchiveJob j in Options.Get().Jobs)
                if (j.Name == comboBox1.Text)
                {
                    _jj = j;
                    if (j.DB == DataBase.AllDataBases)
                    {
                        foreach (DataBase db in Options.Get().Databases)
                            comboBox2.Items.Add(db.Name);
                    }
                    else
                        comboBox2.Items.Add(j.DB.Name);
                }
            comboBox2.SelectedIndex = 0;
        }

        private void FillList(ArchiveJob j, String db)
        {
            listView1.Items.Clear();
            DirectoryInfo di = new DirectoryInfo(j.BackupPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                String[] nm = Path.GetFileName(fi.FullName).Split('_');
                string fls = X_Tools.XTools.SafeFileName(j.Name,"_") + "_" + X_Tools.XTools.SafeFileName(db,"_");
                fls = fls.Replace(" ", "_");
                if (Path.GetFileName(fi.FullName).StartsWith(fls))
                //                    if (nm.Length == ArchiveJobThread.SplitNames && nm[0] == j.Name && nm[1] == db)
                {
                    string[] hms = new string[3] { nm[nm.Length - 1].Substring(0, 2), nm[nm.Length - 1].Substring(2, 2), nm[nm.Length - 1].Substring(4, 2) };
                    DateTime dtm = new DateTime(int.Parse(nm[nm.Length - 4]), int.Parse(nm[nm.Length - 3]), int.Parse(nm[nm.Length - 2]), int.Parse(hms[0]), int.Parse(hms[1]), int.Parse(hms[2]));
                    int idx = 0;
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        if (DateTime.Parse(listView1.Items[i].SubItems[0].Text) > dtm)
                            idx++;
                    }
                    ListViewItem li = listView1.Items.Insert(idx, dtm.ToShortDateString() + " " + dtm.ToLongTimeString());
                    li.SubItems.Add(Path.GetFileName(fi.FullName));
                }
            }
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBase db = _jj.DB;
            if (db == DataBase.AllDataBases)
                foreach (DataBase d in Options.Get().Databases)
                    if (d.Name == comboBox2.Text) db = d;
            tbHost.Text = db.Host;
            tbDB.Text = db.DBName;
            tbUser.Text = db.User;
            tbPassword.Text = db.Password;
            tbFile_TextChanged(null,null);
            FillList(_jj, comboBox2.Text);
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
            tbFile.Text=_jj.BackupPath+"\\"+listView1.SelectedItems[0].SubItems[1].Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            _thrRestore = new Thread(new ParameterizedThreadStart(RestoreThr));
            RestoreRarams p = new RestoreRarams();

            p.Host=tbHost.Text;
            p.Db = tbDB.Text;
            p.User = tbUser.Text;
            p.Password = tbPassword.Text;
            p.File = tbFile.Text;

            Enabled = false;

            _wtFrm.Show();

            _thrRestore.Start(p);




//            try
//            {

//                ArchiveJobThread.UndumpDB(tbHost.Text, tbDB.Text, tbUser.Text, tbPassword.Text, tbFile.Text);
//                MessageBox.Show("Восстановление завершено");
//                Close();
//            }
//            catch (Exception ex)
//            {
//                DialogResult = DialogResult.None;
//                MessageBox.Show(ex.Message);
//            }
        }

        public delegate void RestoreThrCbDelegate(bool success, Exception ex);


        private void RestoreThrCb(bool success, Exception ex)
        {
            if (InvokeRequired)
            {
                RestoreThrCbDelegate d = RestoreThrCb;
                Invoke(d, new object[] { success, ex });
                //Invoke(d);
            }
            else
            {
           
                _wtFrm.Hide();
                if (success)
                {
                    MessageBox.Show("Восстановление базы данных прошло успешно!", "Восстановление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                } else
                {
                    BringToFront();
                    MessageBox.Show(ex.Message,"Ошибка при восстановлении",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    log.Error(ex.Message);
//                    log.Error(ex.Message);
                    Enabled = true;
                    BringToFront();
                }
            }

        }

        private void RestoreThr(object prms)
        {
            try
            {
                RestoreRarams p = (RestoreRarams) prms;
                //MessageBox.Show(p.Db + Environment.NewLine + p.File);
                ArchiveJobThread.UndumpDB(p.Host, p.Db, p.User, p.Password, p.File);
                //MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
//                DialogResult = DialogResult.None;
//                MessageBox.Show(ex.Message);
                RestoreThrCb(false, ex);
                return;
            }
            RestoreThrCb(true,null);
        }

        private void tbFile_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = !((tbFile.Text == "") || (tbDB.Text == "") || (tbHost.Text == "") || (tbUser.Text == "") || (tbPassword.Text == ""));
        }
    }
    public class RestoreRarams
    {
        public string Host;
        public string Db;
        public string User;
        public string Password;
        public string File;
    }
}
