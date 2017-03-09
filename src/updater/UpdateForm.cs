using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using System.Xml;
using log4net;
using rabnet.RNC;

namespace updater
{
    public partial class UpdateForm : Form
    {
        private ILog _logger = LogManager.GetLogger(typeof(UpdateForm));
        bool _batch = false;
        public int Result = 0;
        /// <summary>
        /// Версия до которой надо обновиться
        /// </summary>
        private int _curver = 0;
        private bool _needUpdateSomebody = true;
        private Dictionary<int, String> _scripts = new Dictionary<int, string>();
        private MySqlConnection _sql = null;

        public UpdateForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Загружает список скриптов Обновлений
        /// </summary>
        /// <returns>Количество скриптов</returns>
        private int GetScripts()
        {
            int i = 2;
            string prefix = "";

            try {
                new StreamReader(GetType().Assembly.GetManifestResourceStream("2.sql"));
            } catch (Exception) {
                prefix = "updater.sql.";
            }
            try {
                while (true) {
                    StreamReader stm = new StreamReader(GetType().Assembly.GetManifestResourceStream(prefix + i.ToString() + ".sql"), Encoding.UTF8);
                    String cmd = stm.ReadToEnd();
                    stm.Close();
                    _scripts[i] = cmd;
                    i++;
                }            
            } catch (Exception exc) {
                _logger.Warn(exc);
                i--;
            }

            _logger.InfoFormat("get {0:d} scripts", i);
            return i;
        }

        private void Status(String txt)
        {
            label2.Text = txt;
            label2.Update();
        }


        private void UpdateForm_Shown(object sender, EventArgs e)
        {
            label1.Update();
            btUpdate.Update();
            btClose.Update();
            _curver = GetScripts();
            UpdateList();
            if (_batch) {
                btUpdate.PerformClick();
            }
            if (_batch) {
                btClose.PerformClick();
            }
        }

        /// <summary>
        /// Заполняет список данными Настроек подключения
        /// </summary>
        private void UpdateList()
        {
            _logger.Info("updating list");
            btUpdate.Enabled = false;
            _needUpdateSomebody = true;
            lv.Items.Clear();
            int needcount = 0;
            Program.RNC.LoadDataSources();
            lv.Update();
            foreach (DataSource rds in Program.RNC.DataSources) {
                ListViewItem li = lv.Items.Add(rds.Name);
                li.SubItems.Add(rds.Params.ToString());
                li.Tag = 0;
                _sql = new MySqlConnection(rds.Params.ToString());
                int hasver = 0;
                try {
                    _sql.Open();
                    _logger.DebugFormat("connecting success: {0}|params:{1}", rds.Name, rds.Params.ToString());
                    MySqlCommand cmd = new MySqlCommand("SELECT o_value FROM options WHERE o_name='db' AND o_subname='version';", _sql);
                    MySqlDataReader rd = cmd.ExecuteReader();
                    if (rd.Read())
                        hasver = rd.GetInt32(0);
                    rd.Close();
                    _sql.Close();
                    li.SubItems.Add(hasver.ToString());
                    if (hasver == _curver) {
                        li.ForeColor = Color.Green;
                        li.Tag = 1;
                    } else if (hasver > _curver) {
                        li.ForeColor = Color.YellowGreen;
                        li.Tag = 2;
                    } else needcount++;
                } catch (Exception) {
                    _sql.Close();
                    _logger.DebugFormat("connecting fail: {0}|params:{1}", rds.Name, rds.Params.ToString());
                    li.Tag = 3;
                    li.ForeColor = Color.Red;
                    li.SubItems.Add("нет доступа");
                }
                li.SubItems.Add(_curver.ToString());
            }
            lv.Update();
            if (needcount > 0) {
                Status("Требуется обновить " + needcount.ToString() + " БД");
                btUpdate.Enabled = true;

            } else {
                Status("Обновления не требуются");
                _needUpdateSomebody = false;
                //button2.Enabled = true;
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            _logger.Info("start to Updating");
            btUpdate.Enabled = false;
            progressBar1.Show();
            btClose.Enabled = !_batch;
            List<UpRow> uprows = new List<UpRow>();
            foreach (ListViewItem li in lv.Items) {
                if ((int)li.Tag != 0) { 
                    continue; 
                }
                uprows.Add(new UpRow(int.Parse(li.SubItems[2].Text), li.SubItems[0].Text, li.SubItems[1].Text));
            }
            UpdateThread ut = new UpdateThread(uprows, _curver, _scripts);
            ut.Error += new ErrorEventHandler(upError);
            ut.Progress += new ProgressEventHandler(upProgress);
            ut.EndUpdate += new EndUpdateEventHandler(upEnd);
            ut.StartUpdate();
        }

        private void upProgress(string Name, int PreVer, int ver)
        {
            if (this.InvokeRequired) {
                this.Invoke(new ProgressEventHandler(upProgress), new object[] { Name, PreVer, ver });
            } else {
                Status(String.Format("Обновление '{0:s}' {1:d}->{2:d}", Name, PreVer, ver));
            }
        }

        private void upError(string db, Exception exc)
        {
            MessageBox.Show(String.Format("Произошла ошибка при обновлении базы '{0:s}'. {1:s}", db, exc.Message));
            //upEnd();
        }

        private void upEnd()
        {
            if (this.InvokeRequired) {
                this.Invoke(new EndUpdateEventHandler(upEnd));
            } else {
                UpdateList();
                progressBar1.Hide();
                btUpdate.Enabled = true;
                btClose.Enabled = true;
            }
        }

        //private static void OnUpdate(int tover,MySqlConnection con,UpdateStatus status)
        //{
        //    Application.DoEvents();
        //}

        private void btClose_Click(object sender, EventArgs e)
        {
            Result = 0;
            Close();
        }

        private void UpdateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_needUpdateSomebody) {
                if (MessageBox.Show("Перед выходом необходимо обновить БазыДанных" + Environment.NewLine
                    + "Вы точно хотите выйти?", "", MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }
    }
}
