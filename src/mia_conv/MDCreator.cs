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
        public void debug(Exception ex)
        {
            debug("Error:"+ex.GetType().ToString()+":"+ex.Message);
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
            fillZones();
            fillBuildings();
            fillTransfers();
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
                c.CommandText=String.Format("INSERT INTO names(n_sex,n_name,n_surname,n_use,n_block_date) VALUES('{0:s}','{1:s}','{2:s}',{3:s},{4:s});",
                    sex?"male":"female",nm.name.value(),nm.surname.value(),use,xdt);
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

        public void fillZones()
        {
            debug("fill zones");
            List<MFString> st = mia.zone_list.strings;
            for (int i = 0; i < st.Count / 2; i++)
            {
                String[] idnm = st[i * 2].value().Split(':');
                c.CommandText = String.Format("INSERT INTO zones(z_id,z_name,z_short_name) VALUES({0:d},'{1:s}','{2:s}')",
                    int.Parse(idnm[0]),idnm[1],st[i*2+1].value());
                try
                {
                    if (int.Parse(idnm[0])!=0)
                        c.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    debug(ex);
                }
            }
        }

        public int savetier(Tier tr)
        {
            String tp = "unk";
            int b1=0,b2=0,b3=0,b4=0;
            int heater=0;
            int nest=0;
            int delims=0;
            switch (tr.type)
            {
                case 0: tp = "none";
                    break;
                case 1: tp = "female";
                    b1 = tr.busies[0];
                    heater = tr.heaters[0];
                    nest = tr.nests[0];
                    break;
                case 2: tp = "dfemale";
                    b1 = tr.busies[0];
                    b2 = tr.busies[1];
                    heater = (tr.heaters[0] & 0x01) | ((tr.heaters[1]&0x01) << 1);
                    nest = (tr.nests[0] & 0x01) | ((tr.nests[1] & 0x01) << 1);
                    break;
                case 3: tp = "complex";
                    b1 = tr.busies[0];
                    b2 = tr.busies[1];
                    b3 = tr.busies[2];
                    heater = tr.heaters[0];
                    nest = tr.nests[0];
                    break;
                case 4: tp = "jurta";
                    b1 = tr.busies[0];
                    b2 = tr.busies[1];
                    heater = tr.heaters[0];
                    nest = tr.nests[0];
                    delims = tr.nest_wbig;
                    break;
                case 5: tp = "quarta";
                    b1 = tr.busies[0];
                    b2 = tr.busies[1];
                    b3 = tr.busies[2];
                    b4 = tr.busies[3];
                    delims = (tr.delims[0] & 0x01) | ((tr.delims[1] & 0x01) << 1) | ((tr.delims[2] & 0x01) << 2);
                    break;
                case 6: tp = "vertep";
                    b1 = tr.busies[0];
                    b2 = tr.busies[1];
                    break;
                case 7: tp = "barin";
                    b1 = tr.busies[0];
                    b2 = tr.busies[1];
                    delims = tr.delims[0];
                    break;
                case 8: tp = "cabin";
                    b1 = tr.busies[0];
                    b2 = tr.busies[1];
                    heater = tr.heaters[0];
                    nest = tr.nests[0];
                    break;
            }
            c.CommandText=String.Format("INSERT INTO tiers(t_type,t_repair,t_notes,t_busy1,t_busy2,t_busy3,t_busy4,t_heater,t_nest,t_delims) "+
                "VALUES('{0:s}',{1:d},'{2:s}',{3:d},{4:d},{5:d},{6:d},{7:d},{8:d},{9:d});",
                tp,tr.repair,tr.notes.value(),b1,b2,b3,b4,heater,nest,delims);
            c.ExecuteNonQuery();
            return (int)c.LastInsertedId;
        }

        public void procTreeNode(TreeNode nd,int level,int parent)
        {
            for (int i = 0; i < nd.Nodes.Count; i++)
            {
                String nm = nd.Nodes[i].Text;
                int fid = 0;
                try
                {
                    fid = int.Parse(nm);
                    nm = "farm "+fid.ToString();
                }
                catch(FormatException ex)
                {
                    fid = 0;
                }
                c.CommandText=String.Format("INSERT INTO buildings(b_name,b_parent,b_level,b_farm) "+
                    "VALUES('{0:s}',{1:d},{2:d},{3:d});",nm,parent,level,fid);
                c.ExecuteNonQuery();
                int tpar = (int)c.LastInsertedId;
                procTreeNode(nd.Nodes[i], level + 1, tpar);
            }
        }

        public void fillBuildings()
        {
            debug("fill buildings");
            int maxid = 0;
            foreach (MiniFarm fm in mia.builds.minifarms)
            {
                int upp=savetier(fm.upper);
                int low = 0;
                if (fm.id > maxid)
                    maxid = fm.id;
                if (fm.haslower==1)
                    low = savetier(fm.lower);
                c.CommandText = String.Format("INSERT INTO minifarms(m_id,m_upper,m_lower) VALUES({0:d},{1:d},{2:d});",
                    fm.id,upp,low);
                c.ExecuteNonQuery();

            }
            debug("buildings="+maxid.ToString()+"\r\nfill buildings tree");
            procTreeNode(mia.buildPlan.value(),0,0);
        }

        public String convdt(String dt)
        {
            String[] dmy=dt.Split('.');
            return String.Format("'{0:S4}-{1:S2}-{2:S2}'",dmy[2],dmy[1],dmy[0]);
        }
        public String convdt(DateTime dt)
        {
            return convdt(String.Format("{0:D2}.{1:D2}.{2:D2}",dt.Day,dt.Month,dt.Year));
        }

        public int getCatalogValue(String type,Char flag,String value)
        {
            c.CommandText = "SELECT c_id,c_flags FROM catalogs WHERE c_type='"+type+"' AND c_value='"+value+"'";
            MySqlDataReader rd = c.ExecuteReader();
            int res = 0;
            if (rd.HasRows)
            {
                rd.Read();
                res = rd.GetInt32(0);
                String flgs = rd.GetString(1);
                rd.Close();
                for (int i = 0; (i < flgs.Length) && flag != '\0'; i++)
                    if (flgs[i]==flag) flag='\0';
                if (flag != '\0')
                {
                    c.CommandText = "UPDATE catalogs SET c_flags=c_flags+'" + flag + "' WHERE c_id=" + res.ToString() + ";";
                    c.ExecuteNonQuery();
                }

            }
            else
            {
                rd.Close();
                c.CommandText = String.Format("INSERT INTO catalogs(c_type,c_flags,c_value) VALUES('{0:s}','{1:s}','{2:s}');",
                    type,""+flag,value);
                c.ExecuteNonQuery();
                res = (int)c.LastInsertedId;
            }
            return res;
        }

        public uint findname(String name,ref String addNm)
        {
            if (name == "") return 0;
            c.CommandText = "SELECT n_id FROM names WHERE n_name='" + name + "';";
            MySqlDataReader rd = c.ExecuteReader();
            if (!rd.HasRows)
            {
                rd.Close();
                addNm = name;
                return 0;
            }
            rd.Read();
            uint res= rd.GetUInt32(0);
            rd.Close();
            return res;
        }
        public int findbreed(int breed)
        {
            return breed + 1;
        }

        public void fillTransfers()
        {
            debug("fill transfers");
            foreach (Trans t in mia.trans_table.transes)
            {
                String add = "";
                String cmd= "INSERT INTO transfers(t_notes,t_date,t_units,t_type";
                String vals = String.Format("VALUES('{0:s}',{1:s},{2:d},",t.notes.value(),convdt(t.when.value()),t.units.value());
                switch (t.transferType)
                {
                    case 0:
                        cmd+=",t_sold,t_age,t_weight,t_partner,t_price";
                        vals += String.Format("'meat',1,{0:d},{1:d},{2:d},{3:s}", t.age.value(), t.lweight.value(), getCatalogValue("partner", 'm', t.partner.value()), t.price.value().Replace(',', '.'));
                        break;
                    case 1:
                        cmd+=",t_sold,t_age,t_kind,t_partner,t_price";
                        vals += String.Format("'skin',1,{0:d},{1:d},{2:d},{3:s}", t.age.value(), t.skintype.value(), getCatalogValue("partner", 's', t.partner.value()), t.price.value().Replace(',', '.'));
                        break;
                    case 2:
                        cmd += ",t_sold,t_age,t_name,t_breed,t_weight,t_partner,t_price,t_str";
                        vals += String.Format("'rabbits',{0:d},{1:d},{2:d},{3:d},{4:d},{5:d},{6:F2},'{7:s}'",
                            t.issold.value(),t.age.value(),findname(t.name.value(),ref add),findbreed((int)t.breed.value()),
                            t.sweight.value(),getCatalogValue("partner",(t.issold.value()==1?'r':'R'),t.partner.value()),
                            t.price.value().Replace(',','.'),add);
                        break;
                    case 3:
                        cmd += ",t_age,t_name,t_weight,t_kind,t_partner,t_price";
                        vals += String.Format("'feed',{0:d},{1:d},{2:d},{3:d},{4:d},{5:s}",t.age.value(),
                            getCatalogValue("name",'f',t.name.value()),t.lweight.value(),getCatalogValue("kind",'f',t.kind.value()),
                            getCatalogValue("partner", 'f', t.partner.value()), t.price.value().Replace(',', '.'));
                        break;
                    case 4:
                        cmd += ",t_sold,t_age,t_name,t_weight,t_kind,t_partner,t_price";
                        bool issold = t.issold.value() == 1;
                        vals += String.Format("'other',{0:d},{1:d},{2:d},{3:d},{4:d},{5:d},{6:s}", t.issold.value(),t.age.value(),
                            getCatalogValue("name", issold ? 'o' : 'O', t.name.value()), t.lweight.value(), getCatalogValue("kind", issold ? 'o' : 'O', t.kind.value()),
                            getCatalogValue("partner", issold ? 'o' : 'O', t.partner.value()), t.price.value().Replace(',', '.'));
                        break;
                    case 5:
                        cmd += ",t_sold,t_age,t_mdate,t_weight,t_weight2,t_name,t_str";
                        vals += String.Format("'meat',0,{0:d},{1:s},{2:d},{3:d},{4:d},'{5:s}'",
                            t.age.value(),convdt(t.murder.value()),t.brutto.value(),t.netto.value(),
                            findname(t.name.value(),ref add),add+" "+t.address.value());
                        break;
                    case 6:
                        cmd += ",t_sold,t_age,t_mdate,t_sex,t_breed,t_kind,t_name,t_str";
                        vals += String.Format("'skin',0,{0:d},{1:s},{2:d},{3:d},{4:d},{5:d},'{6:s}'",t.age.value(),
                            convdt(t.murder.value()),t.sex.value(),t.breed.value(),t.skintype.value(),
                            findname(t.name.value(),ref add),add+" "+t.address.value());
                        break;
                    case 7:
                        cmd += ",t_age,t_name,t_weight,t_kind";
                        vals += String.Format("'feed_use',{0:d},{1:d},{2:d},{3:d}",t.age.value(),
                            getCatalogValue("name",'f',t.name.value()),t.lweight.value(),
                            getCatalogValue("kind",'f',t.name.value()));
                        break;
                    case 8:
                        cmd += ",t_sold,t_age,t_weight,t_partner,t_kind,t_price";
                        vals += String.Format("'otsev',{0:d},{1:d},{2:d},{3:d},{4:d},{5:s}",t.issold.value(),t.age.value(),
                            t.lweight.value(),getCatalogValue("partner",'x',t.partner.value()),
                            getCatalogValue("kind", 'x', t.kind.value()), t.price.value().Replace(',', '.'));
                        break;
                }
                c.CommandText = cmd + ") " + vals + ");";
                c.ExecuteNonQuery();
            }
        }

    }
}
