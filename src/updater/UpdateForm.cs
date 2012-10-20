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
        //private string _filenameRabDump = "";
        //private string _filenameRabNet = "";
        private bool _needUpdateSomebody = true;
        private Dictionary<int, String> _scripts = new Dictionary<int, string>();
        private MySqlConnection _sql = null;
        public enum UpdateStatus { Before, Procs, After }

        public UpdateForm()
        {
            InitializeComponent();
        }

        /*public UpdateForm(String flRabDump,string flRabNet,bool bt):this()
        {
            _batch = bt;
            _filenameRabDump = flRabDump;
            _filenameRabNet = flRabNet;
        }*/

        /// <summary>
        /// Загружает список скриптов Обновлений
        /// </summary>
        /// <returns>Количество скриптов</returns>
        private int GetScripts()
        {
            int i = 2;
            string prefix = "";
            try
            {
                new StreamReader(GetType().Assembly.GetManifestResourceStream("2.sql"));
            }
            catch(Exception)
            {
                prefix = "updater.sql.";
            }
            try
            {
                while (true)
                {
                    StreamReader stm = new StreamReader(GetType().Assembly.GetManifestResourceStream(prefix + i.ToString() + ".sql"), Encoding.UTF8);
                    String cmd = stm.ReadToEnd();
                    stm.Close();
                    _scripts[i] = cmd;
                    i++;
                }
            }
            catch (Exception)
            {
                i--;
            }
            _logger.InfoFormat("get {0:d} scripts",i);
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
            if (_batch)
                btUpdate.PerformClick();
            if (_batch)
                btClose.PerformClick();
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
            foreach (DataSource rds in Program.RNC.DataSources)      
            {
                ListViewItem li = lv.Items.Add(rds.Name);
                li.SubItems.Add(rds.Params.ToString());
                li.Tag = 0;
                _sql = new MySqlConnection(rds.Params.ToString());
                int hasver = 0;
                try
                {
                    _sql.Open();
                    _logger.DebugFormat("connecting success: {0}|params:{1}",rds.Name,rds.Params.ToString());
                    MySqlCommand cmd = new MySqlCommand("SELECT o_value FROM options WHERE o_name='db' AND o_subname='version';", _sql);
                    MySqlDataReader rd = cmd.ExecuteReader();
                    if (rd.Read())
                        hasver = rd.GetInt32(0);
                    rd.Close();
                    _sql.Close();
                    li.SubItems.Add(hasver.ToString());
                    if (hasver == _curver)
                    {
                        li.ForeColor = Color.Green;
                        li.Tag = 1;
                    }
                    else if (hasver > _curver)
                    {
                        li.ForeColor = Color.YellowGreen;
                        li.Tag = 2;
                    }
                    else needcount++;
                }
                catch (Exception)
                {
                    _sql.Close();
                    _logger.DebugFormat("connecting fail: {0}|params:{1}", rds.Name, rds.Params.ToString());
                    li.Tag = 3;
                    li.ForeColor = Color.Red;
                    li.SubItems.Add("нет доступа");
                }
                li.SubItems.Add(_curver.ToString());
            }
            lv.Update();
            if (needcount > 0)
            {
                Status("Требуется обновить " + needcount.ToString() + " БД");
                btUpdate.Enabled = true;
                
            }
            else
            {
                Status("Обновления не требуются");
                _needUpdateSomebody = false;
                //button2.Enabled = true;
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            _logger.Info("start to Updating");
			btUpdate.Enabled = false;
            btClose.Enabled = !_batch;
            foreach (ListViewItem li in lv.Items)
            if ((int)li.Tag == 0)
            {
                int preVer = int.Parse(li.SubItems[2].Text);
                String prm = li.SubItems[1].Text + ";Allow User Variables=True";
                String nm = li.SubItems[0].Text;
                Status("Обновляется БД "+nm);
                while (preVer < _curver)
                {
                    preVer++;
                    foreach(int k in _scripts.Keys)
                        if (k == preVer)
                        {
                            try
                            {
                                Status(String.Format("Обновление {0:s} {1:d}->{2:d}",nm,preVer-1,preVer));
                                _sql = new MySqlConnection(prm);
                                _sql.Open();
                                MySqlCommand c = new MySqlCommand("", _sql);
                                OnUpdate(preVer, _sql,UpdateStatus.Before);
                                String[] cmds = _scripts[k].Split(new string[] { "#DELIMITER |" }, StringSplitOptions.RemoveEmptyEntries);
                                c.CommandText = cmds[0];
                                c.ExecuteNonQuery();
                                if (cmds.Length > 1)
                                {
                                    OnUpdate(preVer, _sql, UpdateStatus.Procs);
                                    MySqlScript sc = new MySqlScript(_sql, cmds[1]);
                                    sc.Delimiter = "|";
                                    sc.Execute();
                                }
                                OnUpdate(preVer, _sql,UpdateStatus.After);
                                _sql.Close();
                                _logger.DebugFormat("update success db:{0:s}|script:{1:#}|",li.SubItems[0].Text ,k);
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(ex);
                                MessageBox.Show("Во время обновления БД произошла ошибка: "+ex.Message);
                                _batch = false;
                            }
                        }
                }
            }
            UpdateList();
			btUpdate.Enabled = true;
            btClose.Enabled = true;
        }
        
        private static void OnUpdate(int tover,MySqlConnection con,UpdateStatus status)
        {
			Application.DoEvents();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Result = 0;
            Close();
        }

        private void UpdateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_needUpdateSomebody)
            {              
                if(MessageBox.Show("Перед выходом необходимо обновить БазыДанных"+Environment.NewLine
                    +"Вы точно хотите выйти?","",MessageBoxButtons.YesNo) == DialogResult.No)
                e.Cancel = true;
            }
        }

        //private void UpdateList()
        //{
        //    btUpdate.Enabled = false;
        //    int needcount = 0;
        //    btUpdate.Enabled = false;
        //    Status("Проверка версий баз данных");
        //    lv.Items.Clear();
        //    XmlDocument xml = new XmlDocument();
        //    try
        //    {
        //        //загружаем данные из файла rabDump.exe.config
        //        xml.Load(_filenameRabDump);
        //        foreach (XmlNode n in xml.DocumentElement.ChildNodes)
        //        {
        //            if (n.Name == "rabdumpOptions")
        //            {
        //                foreach (XmlNode ds in n.ChildNodes)
        //                {
        //                    if (ds.Name == "db")
        //                    {
        //                        String nm = "";
        //                        String prm = "";
        //                        foreach (XmlNode nd in ds.ChildNodes)
        //                            switch (nd.Name)
        //                            {
        //                                case "name":
        //                                    nm = nd.FirstChild != null ? nd.FirstChild.Value : "";
        //                                    break;
        //                                case "host":
        //                                    prm += "host=" + (nd.FirstChild != null ? nd.FirstChild.Value : "") + ";";
        //                                    break;
        //                                case "db":
        //                                    prm += "database=" + (nd.FirstChild != null ? nd.FirstChild.Value : "") + ";";
        //                                    break;
        //                                case "user":
        //                                    prm += "uid=" + (nd.FirstChild != null ? nd.FirstChild.Value : "") + ";";
        //                                    break;
        //                                case "password":
        //                                    prm += "pwd=" + (nd.FirstChild != null ? nd.FirstChild.Value : "") + ";";
        //                                    break;
        //                            }
        //                        prm += "charset=utf8";
          
        //                        ListViewItem li = lv.Items.Add(nm);
        //                        li.SubItems.Add(prm);
        //                        li.Tag = 0;
        //                        _sql = new MySqlConnection(prm);
        //                        int hasver = 0;
        //                        lv.Update();
        //                        try
        //                        {
        //                            _sql.Open();
        //                            MySqlCommand cmd = new MySqlCommand("SELECT o_value FROM options WHERE o_name='db' AND o_subname='version';", _sql);
        //                            MySqlDataReader rd = cmd.ExecuteReader();
        //                            if (rd.Read())
        //                                hasver = rd.GetInt32(0);
        //                            rd.Close();
        //                            _sql.Close();
        //                            li.SubItems.Add(hasver.ToString());
        //                            if (hasver == _curver)
        //                            {
        //                                li.ForeColor = Color.Green;
        //                                li.Tag = 1;
        //                            }
        //                            else if (hasver > _curver)
        //                            {
        //                                li.ForeColor = Color.YellowGreen;
        //                                li.Tag = 2;
        //                            }
        //                            else needcount++;
        //                        }
        //                        catch (Exception)
        //                        {
        //                            _sql.Close();
        //                            li.Tag = 3;
        //                            li.ForeColor = Color.Red;
        //                            li.SubItems.Add("нет доступа");
        //                        }
        //                        li.SubItems.Add(_curver.ToString());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        //загружаем данные из файла rabNet.exe.config
        //        xml.Load(_filenameRabNet);
        //        foreach (XmlNode n in xml.DocumentElement.ChildNodes)
        //        {
        //            if (n.Name == "rabnetds")
        //            {
        //                foreach (XmlNode ds in n.ChildNodes)
        //                {
        //                    if (ds.Name == "dataSource")
        //                    {
        //                        String nm = ds.Attributes["name"].Value;
        //                        String prm = ds.Attributes["param"].Value; 
        //                        ListViewItem li = lv.Items.Add(nm);
        //                        li.SubItems.Add(prm);
        //                        li.Tag = 0;
        //                        _sql = new MySqlConnection(prm);
        //                        int hasver = 0;
        //                        lv.Update();
        //                        try
        //                        {
        //                            _sql.Open();
        //                            MySqlCommand cmd = new MySqlCommand("SELECT o_value FROM options WHERE o_name='db' AND o_subname='version';", _sql);
        //                            MySqlDataReader rd = cmd.ExecuteReader();
        //                            if (rd.Read())
        //                                hasver = rd.GetInt32(0);
        //                            rd.Close();
        //                            _sql.Close();
        //                            li.SubItems.Add(hasver.ToString());
        //                            if (hasver == _curver)
        //                            {
        //                                li.ForeColor = Color.Green;
        //                                li.Tag = 1;
        //                            }
        //                            else if (hasver > _curver)
        //                            {
        //                                li.ForeColor = Color.YellowGreen;
        //                                li.Tag = 2;
        //                            }
        //                            else needcount++;
        //                        }
        //                        catch (Exception)
        //                        {
        //                            _sql.Close();
        //                            li.Tag = 3;
        //                            li.ForeColor = Color.Red;
        //                            li.SubItems.Add("нет доступа");
        //                        }
        //                        li.SubItems.Add(_curver.ToString());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    if (needcount>0)
        //    {
        //        Status("Требуется обновить " + needcount.ToString() + " БД");
        //        btUpdate.Enabled = true;
        //    }
        //    else
        //    {
        //        Status("Обновления не требуются");
        //        //button2.Enabled = true;
        //    }
        //    btUpdate.Enabled = true;
        //}
    }
}
