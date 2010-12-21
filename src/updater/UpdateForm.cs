using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using System.Xml;

namespace updater
{
    public partial class UpdateForm : Form
    {
        bool _batch = false;
        public int Result = 0;
        private int _curver = 0;
        private string _filename="";
        private Dictionary<int, String> scr = new Dictionary<int, string>();
        private MySqlConnection _sql = null;
        public enum UpdateStatus { Before, Procs, After }
        public UpdateForm()
        {
            InitializeComponent();
        }
        public UpdateForm(String fl,bool bt):this()
        {
            _batch = bt;
            _filename = fl;
        }
        public int GetScripts()
        {
            int i = 2;
            string prefix = "";
            try
            {
                new StreamReader(GetType().Assembly.GetManifestResourceStream("2.sql"));
            }catch(Exception)
            {
                prefix="updater.sql.";
            }
            try{
                while (true)
                {
                    StreamReader stm = new StreamReader(GetType().Assembly.GetManifestResourceStream(prefix + i.ToString()+".sql"), Encoding.UTF8);
                    String cmd = stm.ReadToEnd();
                    stm.Close();
                    scr[i] = cmd;
                    i++;
                }
            }catch(Exception)
            {
                i--;
            }
            return i;
        }

        public void Status(String txt)
        {
            label2.Text = txt;
            label2.Update();
        }

        public void UpdateList()
        {
			button1.Enabled = false;
			int needcount = 0;
            button1.Enabled = false;
            Status("Проверка версий баз данных");
            lv.Items.Clear();
            XmlDocument xml = new XmlDocument();
            xml.Load(_filename);
            foreach (XmlNode n in xml.DocumentElement.ChildNodes)
            if (n.Name=="rabdumpOptions")
                foreach(XmlNode ds in n.ChildNodes)
                    if (ds.Name == "db")
                    {
                        String nm = "";
                        String prm = "";
                        foreach (XmlNode nd in ds.ChildNodes)
                            switch (nd.Name)
                            {
                                case "name": nm = nd.FirstChild != null ? nd.FirstChild.Value : ""; break;
                                case "host": prm += "host="+(nd.FirstChild != null ? nd.FirstChild.Value : "")+";"; break;
                                case "db": prm += "database=" + (nd.FirstChild != null ? nd.FirstChild.Value : "") + ";"; break;
                                case "user": prm += "uid=" + (nd.FirstChild != null ? nd.FirstChild.Value : "") + ";"; break;
                                case "password": prm += "pwd=" + (nd.FirstChild != null ? nd.FirstChild.Value : "") + ";"; break;
                            }
                        prm += "charset=utf8";
                        ListViewItem li = lv.Items.Add(nm);
                        li.SubItems.Add(prm);
                        li.Tag=0;
                        _sql = new MySqlConnection(prm);
                        int hasver=0;
                        lv.Update();
                        try
                        {
                            _sql.Open();
                            MySqlCommand cmd = new MySqlCommand("SELECT o_value FROM options WHERE o_name='db' AND o_subname='version';",_sql);
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
                            li.Tag = 3;
                            li.ForeColor = Color.Red;
                            li.SubItems.Add("нет доступа");
                        }
                        li.SubItems.Add(_curver.ToString());
                    }
            if (needcount>0)
            {
                Status("Требуется обновить " + needcount.ToString() + " БД");
                button1.Enabled = true;
            }else{
                Status("Обновления не требуются");
                //button2.Enabled = true;
            }
			button1.Enabled = true;
        }

        private void UpdateForm_Shown(object sender, EventArgs e)
        {
            label1.Update();
            button1.Update();
            button2.Update();
            _curver = GetScripts();
            UpdateList();
            if (_batch)
                button1.PerformClick();
            if (_batch)
                button2.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
			button1.Enabled = false;
            button2.Enabled = !_batch;
            foreach (ListViewItem li in lv.Items)
            if ((int)li.Tag == 0)
            {
                int prever = int.Parse(li.SubItems[2].Text);
                String prm = li.SubItems[1].Text;
                String nm = li.SubItems[0].Text;
                Status("Обновляется БД "+nm);
                while (prever < _curver)
                {
                    prever++;
                    foreach(int k in scr.Keys)
                        if (k == prever)
                        {
                            try
                            {
                                Status(String.Format("Обновление {0:s} {1:d}->{2:d}",nm,prever-1,prever));
                                _sql = new MySqlConnection(prm);
                                _sql.Open();
                                MySqlCommand c = new MySqlCommand("", _sql);
                                OnUpdate(prever, _sql,UpdateStatus.Before);
                                String[] cmds = scr[k].Split(new string[] { "#DELIMITER |" }, StringSplitOptions.RemoveEmptyEntries);
                                c.CommandText = cmds[0];
                                c.ExecuteNonQuery();
                                if (cmds.Length > 1)
                                {
                                    OnUpdate(prever, _sql, UpdateStatus.Procs);
                                    MySqlScript sc = new MySqlScript(_sql, cmds[1]);
                                    sc.Delimiter = "|";
                                    sc.Execute();
                                }
                                OnUpdate(prever, _sql,UpdateStatus.After);
                                _sql.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Во время обновления БД произошла ошибка:"+ex.Message);
                                _batch = false;
                            }
                        }
                }
            }
            UpdateList();
			button1.Enabled = true;
            button2.Enabled = true;
        }
        private static void OnUpdate(int tover,MySqlConnection con,UpdateStatus status)
        {
			Application.DoEvents();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Result = 0;
            Close();
        }

        private void UpdateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !button1.Enabled;
        }

    }
}
