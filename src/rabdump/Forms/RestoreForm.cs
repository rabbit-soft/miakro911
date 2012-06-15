#if DEBUG
    #define NOCATCH
#endif
using System;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using log4net;
using System.Collections.Generic;
#if PROTECTED
using pEngine;
using RabGRD;
#endif

namespace rabdump
{
    public partial class RestoreForm : Form
    {
        private ArchiveJob _jj = null;

        private Thread _thrRestore;

        private readonly WaitForm _wtFrm;
        private bool _smallMode = true;
        //private bool _manual = true;

        private static readonly ILog log = LogManager.GetLogger(typeof(RestoreForm));

        public RestoreForm()
        {
            InitializeComponent();
            SetMode(true);
            foreach (ArchiveJob j in Options.Get().Jobs)
            {
                cbJobName.Items.Add(j.Name);
            }
            if (cbJobName.Items.Count > 0)
            {
                cbJobName.SelectedIndex = 0;
            }
            _wtFrm = new WaitForm();
            _wtFrm.SetName("Восстановление БД...");

        }

        public RestoreForm(String place):this()
        {
            for (int i = 0; i < cbJobName.Items.Count; i++)
            {
                if ((string)cbJobName.Items[i] == place)
                    cbJobName.SelectedIndex = i;
            }
        }

        /// <summary>
        /// Установить режим
        /// </summary>
        /// <param name="small">Обычный/Расширенный</param>
        public void SetMode(bool small)
        {
            Height = (small ? 410 : 550);
            btExtMode.Text="Расширенный режим "+(small?">>":"<<");
            groupBox1.Visible = !small;
            _smallMode = small;
        }

        private void btExtMode_Click(object sender, EventArgs e)
        {
           SetMode(Height==550);
        }

        private void cbJobName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDataBase.Items.Clear();
            listView1.Items.Clear();

            foreach (ArchiveJob j in Options.Get().Jobs)
                if (j.Name == cbJobName.Text)
                {
                    _jj = j;
                    if (j.DB == DataBase.AllDataBases)
                    {
                        foreach (DataBase db in Options.Get().Databases)
                            cbDataBase.Items.Add(db.Name);
                    }
                    else
                        cbDataBase.Items.Add(j.DB.Name);
                }
            if (cbDataBase.Items.Count!=0)
                cbDataBase.SelectedIndex = 0;
            
        }

        /// <summary>
        /// Заполняет таблицу имеющимися Дампами
        /// </summary>
        /// <param name="j"></param>
        /// <param name="db">имя Бд</param>
        private void FillList(ArchiveJob j, String db)
        {
            listView1.Items.Clear();
            DirectoryInfo di = new DirectoryInfo(j.BackupPath);
            string searchName = j.Name.Replace(' ', '+') + "_" + db.Replace(' ', '+');
            List<sDump> servDumps = null;
#if PROTECTED           
            if (GRD.Instance.GetFlag(GRD.FlagType.ServerDump))
            {
#endif
#if PROTECTED ||DEBUG
                sDump[] dmps= RabServWorker.ReqSender.ExecuteMethod(MethodName.GetDumpList).Value as sDump[];
                servDumps = new List<sDump>(dmps);
#endif
#if PROTECTED               
            }
#endif

            int idx;
            DateTime dtm;
            foreach (FileInfo fi in di.GetFiles())
            {
                if (Path.GetFileName(fi.FullName).StartsWith(searchName))
                {
                    String[] nm = ArchiveJobThread.ParseDumpName(fi.Name);
                    dtm = DateTime.Parse(String.Format("{0}-{1}-{2} {3}:{4}:{5}", nm[2], nm[3], nm[4], nm[5].Substring(0, 2), nm[5].Substring(2, 2), nm[5].Substring(4, 2)));
                    idx = 0;
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        if (DateTime.Parse(listView1.Items[i].SubItems[0].Text) > dtm)
                            idx++;
                    }
                    ListViewItem li = listView1.Items.Insert(idx, dtm.ToShortDateString() + " " + dtm.ToLongTimeString());
                    li.SubItems.Add(fi.Name);

                    ///Если такой дамп имеется на сервере, то окрашиваем строку в зеленый                    
                    if (servDumps != null && servDumps.Count > 0)
                        for (int i = 0; i < servDumps.Count; i++)
                            if (servDumps[i].FileName == fi.Name)
                            {
                                li.ForeColor = System.Drawing.Color.Green;
                                servDumps.RemoveAt(i);
                                break;
                            }

                }
            }
            ///Добавляем файлы которые имеются только на сервере
            if (servDumps != null && servDumps.Count > 0 && j.DB != DataBase.AllDataBases)
            {
                foreach (sDump dmp in servDumps)
                {
                    String[] nm = ArchiveJobThread.ParseDumpName(dmp.FileName);
                    dtm = DateTime.Parse(String.Format("{0}-{1}-{2} {3}:{4}:{5}", nm[2], nm[3], nm[4], nm[5].Substring(0, 2), nm[5].Substring(2, 2), nm[5].Substring(4, 2)));
                    idx = 0;
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        if (DateTime.Parse(listView1.Items[i].SubItems[0].Text) > dtm)
                            idx++;                        
                    }
                    ListViewItem li = listView1.Items.Insert(idx, dtm.ToShortDateString() + " " + dtm.ToLongTimeString());
                    li.SubItems.Add(dmp.FileName);
                    li.ForeColor = System.Drawing.Color.BlueViolet;
                }
            }
        }

        private void cbDataBase_SelectedIndexChanged(object sender, EventArgs e)
        {         
            DataBase db = _jj.DB;
            if (db == DataBase.AllDataBases)
                foreach (DataBase d in Options.Get().Databases)
                    if (d.Name == cbDataBase.Text) db = d;
            tbHost.Text = db.Host;
            tbDB.Text = db.DBName;
            tbUser.Text = db.User;
            tbPassword.Text = db.Password;
            tbFile_TextChanged(null,null);
            FillList(_jj, cbDataBase.Text);
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

        private void btRestore_Click(object sender, EventArgs e)
        {
            if (_smallMode)
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Выберите Резервную Копию для востановления");
                    return;
                }
                if (listView1.Items.Count!=0 && listView1.SelectedItems[0].Index != 0)
                    if (MessageBox.Show("Выбранная Резервная копия не является самой поздней. Продолжить?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
            }           
            _thrRestore = new Thread(new ParameterizedThreadStart(RestoreThr));
            RestoreRarams p = new RestoreRarams();

            p.Host=tbHost.Text;
            p.Db = tbDB.Text;
            p.User = tbUser.Text;
            p.Password = tbPassword.Text;
            p.File = tbFile.Text;

            p.fromServer = _smallMode && listView1.SelectedItems.Count != 0 && (listView1.SelectedItems[0].ForeColor == System.Drawing.Color.BlueViolet);


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
                } 
                else
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
#if !NOCATCH
            try
            {
#endif
                RestoreRarams p = (RestoreRarams) prms;

    #if PROTECTED
                if (GRD.Instance.GetFlag(GRD.FlagType.ServerDump))
                {
    #endif
                    if (p.fromServer)
                    {
                        string filepath = RabServWorker.DownloadDump(Path.GetFileName(p.File));
                        if (filepath == "")
                        {
                            RestoreThrCb(false, new ApplicationException("Проблема при скачки файла." + Environment.NewLine + "Востановление отменено"));
                            return;
                        }
                        else
                        {
                            File.Move(filepath, p.File);
                        }
                    }
    #if PROTECTED
                }
    #endif

                ArchiveJobThread.UndumpDB(p.Host, p.Db, p.User, p.Password, p.File);
#if !NOCATCH
            }
            catch (Exception ex)
            {
//                DialogResult = DialogResult.None;
//                MessageBox.Show(ex.Message);
                RestoreThrCb(false, ex);
                return;
            }
#endif
                RestoreThrCb(true,null);
        }

        private void tbFile_TextChanged(object sender, EventArgs e)
        {
            btRestore.Enabled = !((tbFile.Text == "") || (tbDB.Text == "") || (tbHost.Text == "") || (tbUser.Text == "") || (tbPassword.Text == ""));
        }
    }

    /// <summary>
    /// Набор параметров для востановления Базы Данных
    /// </summary>
    public class RestoreRarams
    {

        public bool fromServer;
        public string Host;
        public string Db;
        public string User;
        public string Password;
        public string File;
    }

}
