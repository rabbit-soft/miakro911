﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public int result = 0;
        private int curver = 0;
        private string filename="";
        private Dictionary<int, String> scr = new Dictionary<int, string>();
        private MySqlConnection sql = null;
        public UpdateForm()
        {
            InitializeComponent();
        }
        public UpdateForm(String fl):this()
        {
            filename = fl;
        }
        public int getScripts()
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

        public void status(String txt)
        {
            label2.Text = txt;
            label2.Update();
        }

        public void UpdateList()
        {
            int needcount=0;
            button1.Enabled = button2.Enabled = false;
            status("Проверка версий баз данных");
            lv.Items.Clear();
            XmlDocument xml = new XmlDocument();
            xml.Load(filename);
            foreach (XmlNode n in xml.DocumentElement.ChildNodes)
            if (n.Name=="rabnetds")
                foreach(XmlNode ds in n.ChildNodes)
                    if (ds.Name == "dataSource")
                    {
                        String nm=ds.Attributes["name"].Value;
                        String prm = ds.Attributes["param"].Value;
                        ListViewItem li = lv.Items.Add(nm);
                        li.SubItems.Add(prm);
                        li.Tag=0;
                        sql = new MySqlConnection(prm);
                        int hasver=0;
                        lv.Update();
                        try
                        {
                            sql.Open();
                            MySqlCommand cmd = new MySqlCommand("SELECT o_value FROM options WHERE o_name='db' AND o_subname='version';",sql);
                            MySqlDataReader rd = cmd.ExecuteReader();
                            if (rd.Read())
                                hasver = rd.GetInt32(0);
                            rd.Close();
                            sql.Close();
                            li.SubItems.Add(hasver.ToString());
                            if (hasver == curver)
                            {
                                li.ForeColor = Color.Green;
                                li.Tag = 1;
                            }
                            else if (hasver > curver)
                            {
                                li.ForeColor = Color.YellowGreen;
                                li.Tag = 2;
                            }
                            else needcount++;
                        }
                        catch (Exception)
                        {
                            sql.Close();
                            li.Tag = 3;
                            li.ForeColor = Color.Red;
                            li.SubItems.Add("нет доступа");
                        }
                        li.SubItems.Add(curver.ToString());
                    }
            if (needcount>0)
            {
                status("Требуется обновить " + needcount.ToString() + " БД");
                button1.Enabled = true;
            }else{
                status("Обновления не требуются");
                button2.Enabled = true;
            }
        }

        private void UpdateForm_Shown(object sender, EventArgs e)
        {
            label1.Update();
            button1.Update();
            button2.Update();
            curver = getScripts();
            UpdateList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem li in lv.Items)
            if ((int)li.Tag == 0)
            {
                int prever = int.Parse(li.SubItems[2].Text);
                String prm = li.SubItems[1].Text;
                String nm = li.SubItems[0].Text;
                status("Обновляется БД "+nm);
                while (prever < curver)
                {
                    prever++;
                    foreach(int k in scr.Keys)
                        if (k == prever)
                        {
                            try
                            {
                                status(String.Format("Обновление {0:s} {1:d}->{2:d}",nm,prever-1,prever));
                                sql = new MySqlConnection(prm);
                                sql.Open();
                                MySqlCommand c = new MySqlCommand("", sql);
                                OnUpdate(prever, sql,UpdateStatus.BEFORE);
                                String[] cmds = scr[k].Split(new string[] { "#DELIMITER |" }, StringSplitOptions.RemoveEmptyEntries);
                                c.CommandText = cmds[0];
                                c.ExecuteNonQuery();
                                if (cmds.Length > 1)
                                {
                                    OnUpdate(prever, sql, UpdateStatus.PROCS);
                                    MySqlScript sc = new MySqlScript(sql, cmds[1]);
                                    sc.Delimiter = "|";
                                    sc.Execute();
                                }
                                OnUpdate(prever, sql,UpdateStatus.AFTER);
                                sql.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Во время обновления БД произошла ошибка:"+ex.Message);
                            }
                        }
                }
            }
            UpdateList();
        }
        public enum UpdateStatus {BEFORE,PROCS,AFTER}
        private void OnUpdate(int tover,MySqlConnection con,UpdateStatus status)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            result = 0;
            Close();
        }

    }
}