using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;

namespace mia_conv
{
    class MDCreator
    {
        private TextBox log=null;
        public MySqlConnection sql = null;
        public MySqlCommand c=null;
        public bool oldid=false;
        public MiaFile mia=null;
        public MDCreator(TextBox logger)
        {
            log=logger;
        }

        public void debug(String str)
        {
            log.Text += str + "\r\n";
        }

        public bool createDB(String root,String rpswd,String db,String host,String user,String pswd)
        {
            debug("Creating database "+db);
            sql = new MySqlConnection("server=" + host + ";userId=" + root + ";password=" + rpswd + ";database=mysql");
            sql.Open();
            MySqlCommand cmd = new MySqlCommand("DROP DATABASE IF EXISTS "+db+";", sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "CREATE DATABASE " + db + " DEFAULT CHARACTER SET CP1251;";
            cmd.ExecuteNonQuery();
            debug("database created\r\nMaking db user");
            cmd.CommandText = "GRANT ALL ON " + db + ".* TO " + user + "@localhost IDENTIFIED BY '" + pswd + "';";
            cmd.ExecuteNonQuery();
            sql.Close();
            sql = null;
            return true;
        }

        public bool prepare(bool nudb, String host, String user, String password, String db,String root, String rpswd)
        {
            if (nudb)
                if (!createDB(root,rpswd,db,host,user,password))
                    return false;
            sql = new MySqlConnection("host=" + host + ";uid=" + user + ";pwd=" + password + ";database=" + db+";charset=utf8");
            sql.Open();
            c = new MySqlCommand("SET CHARACTER SET utf8;", sql);
            c.ExecuteNonQuery();
            StreamReader stm=new StreamReader(this.GetType().Assembly.GetManifestResourceStream("mia_conv.rabnet_db_fmt.sql"),Encoding.ASCII);
            String cmd = stm.ReadToEnd();
            stm.Close();
            cmd=cmd.Remove(cmd.IndexOf("##TEST_DATA"));
            c.CommandText = cmd;
            c.ExecuteNonQuery();
            return true;
        }

        public void finish()
        {
            sql.Close();
        }


        public String decode(String data)
        {
            return new String(Encoding.Unicode.GetChars(Encoding.Convert(Encoding.UTF8, Encoding.Unicode, Encoding.UTF8.GetBytes(data))));
        }

        public void setUsers(DataTable usrs)
        {
            debug("adding users");
            foreach (DataRow rw in usrs.Rows)
            {
                debug("adding user " + rw.ItemArray[0]);
                c.CommandText = String.Format("INSERT INTO users(u_name,u_password) VALUES('{0:s}',MD5('{1:s}'));",rw.ItemArray[0] as String,rw.ItemArray[1]);
                c.ExecuteNonQuery();
            }
        }

        public void fillAll()
        {
            fillBreeds();
            fillNames();
        }

        public void fillBreeds()
        {
            debug("filling breeds");
            List<MFString> ls=mia.breed_list.strings;
            for (int i = 0; i < ls.Count / 3;i++ )
            {
                c.CommandText = String.Format("INSERT INTO breeds(b_id,b_name,b_short_name) VALUES({2:d},'{0:s}','{1:s}');",
                    ls[i * 3].value(), ls[i * 3 + 1].value(), int.Parse(ls[i * 3 + 2].value())+1);
                c.ExecuteNonQuery();
                ls[i * 3].tag = (int)c.LastInsertedId;
            }
        }

        public void insName(RabName nm,bool sex)
        {

                String xdt="NULL";
                if (nm.key.value()<=365 && nm.key.value()>0)
                    xdt=String.Format("DATE(NOW())+INTERVAL {0:d} DAY",nm.key.value());
                String use="0";
                if (nm.key.value()>365)
                    use=String.Format("{0:d}",nm.key.value());
                c.CommandText=String.Format("INSERT INTO names(n_sex,n_name,n_surname,n_use,n_block_date) VALUES({0:d},'{1:s}','{2:s}',{3:s},{4:s});",
                    sex?1:0,nm.name.value(),nm.surname.value(),use,xdt);
                c.ExecuteNonQuery();
                nm.key.tag = (int)c.LastInsertedId;
        }

        public void fillNames()
        {
            debug("fill names");
            foreach(RabName nm in mia.male_names.rabnames)
                insName(nm, true);
            foreach (RabName nm in mia.female_names.rabnames)
                insName(nm, false);
        }

    }
}
