using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace mia_conv
{
    partial class MDCreator
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
            log.Select(log.Text.Length, 0);
            log.ScrollToCaret();
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
            cmd.CommandText = "GRANT ALL ON " + db + ".* TO " + user + " IDENTIFIED BY '" + pswd + "';";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SET GLOBAL log_bin_trust_function_creators=1;";
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
            StreamReader stm=new StreamReader(this.GetType().Assembly.GetManifestResourceStream("mia_conv.rabnet_db_fmt.sql"),Encoding.UTF8);
            String cmd = stm.ReadToEnd();
            stm.Close();
            cmd=cmd.Remove(cmd.IndexOf("##TEST_DATA"));
            String[] cmds = cmd.Split(new string[] { "#DELIMITER |" }, StringSplitOptions.RemoveEmptyEntries);
            c.CommandText = cmds[0];
            c.ExecuteNonQuery();
            MySqlScript scr=new MySqlScript(sql,cmds[1]);
            scr.Delimiter="|";
            scr.Execute();
            return true;
        }

        public void finish(String sqlfile)
        {
            if (sqlfile != "")
            {
                StreamReader rd = new StreamReader(new FileStream(sqlfile, FileMode.Open), Encoding.ASCII);
                String sq = rd.ReadToEnd();
                rd.Close();
                if (sq.Trim() != "")
                {
                    MySqlCommand cmd = new MySqlCommand(sq, sql);
                    cmd.ExecuteNonQuery();
                }
            }
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
            mia.setpbpart(0, 8);
            fillBreeds();
            fillNames();
            mia.setpbpart(1, 8);
            fillZones();
            mia.setpbpart(2, 8);
            fillBuildings();
            mia.setpbpart(3, 8);
            fillTransfers();
            mia.setpbpart(4, 8);
            fillDeadFromLost();
            mia.setpbpart(5, 8);
            fillRabbits();
            mia.setpbpart(6, 8);
            fillTransForm();
            fillOptions();
            mia.setpbpart(7, 8);
            fillZooForm();
            fillGraphForm();
            fillArcForm();
            mia.setpbpart(8, 8);
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
            Application.DoEvents();
            String tp = "unk";
            String b1="NULL",b2="NULL",b3="NULL",b4="NULL";
            String heater="00";
            String nest="00";
            String delims="000";
            String d = "D1";
            switch (tr.type)
            {
                case 0: tp = "none";
                    break;
                case 1: tp = "female";
                    b1 = "'"+tr.busies[0].ToString()+"'";
                    heater = tr.heaters[0].ToString(d);
                    nest = tr.nests[0].ToString(d);
                    break;
                case 2: tp = "dfemale";
                    b1 = "'" + tr.busies[0].ToString() + "'";
                    b2 = "'" + tr.busies[1].ToString() + "'";
                    heater = tr.heaters[0].ToString(d)+tr.heaters[1].ToString(d);
                    nest = tr.nests[0].ToString(d)+tr.nests[1].ToString(d);
                    break;
                case 3: tp = "complex";
                    b1 = "'" + tr.busies[0].ToString() + "'";
                    b2 = "'" + tr.busies[1].ToString() + "'";
                    b3 = "'" + tr.busies[2].ToString() + "'";
                    heater = tr.heaters[0].ToString(d);
                    nest = tr.nests[0].ToString(d);
                    break;
                case 4: tp = "jurta";
                    b1 = "'" + tr.busies[0].ToString() + "'";
                    b2 = "'" + tr.busies[1].ToString() + "'";
                    heater = tr.heaters[0].ToString(d);
                    nest = tr.nests[0].ToString(d);
                    delims = tr.nest_wbig.ToString(d);
                    break;
                case 5: tp = "quarta";
                    b1 = "'" + tr.busies[0].ToString() + "'";
                    b2 = "'" + tr.busies[1].ToString() + "'";
                    b3 = "'" + tr.busies[2].ToString() + "'";
                    b4 = "'" + tr.busies[3].ToString() + "'";
                    delims = tr.delims[0].ToString(d)+tr.delims[1].ToString(d)+tr.delims[2].ToString(d);
                    break;
                case 6: tp = "vertep";
                    b1 = "'" + tr.busies[0].ToString() + "'";
                    b2 = "'" + tr.busies[1].ToString() + "'";
                    break;
                case 7: tp = "barin";
                    b1 = "'" + tr.busies[0].ToString() + "'";
                    b2 = "'" + tr.busies[1].ToString() + "'";
                    delims = tr.delims[0].ToString(d);
                    break;
                case 8: tp = "cabin";
                    b1 = "'" + tr.busies[0].ToString() + "'";
                    b2 = "'" + tr.busies[1].ToString() + "'";
                    heater = tr.heaters[0].ToString(d);
                    nest = tr.nests[0].ToString(d);
                    break;
            }
            c.CommandText = String.Format("INSERT INTO tiers(t_type,t_repair,t_notes,t_heater,t_nest,t_delims,t_busy1,t_busy2,t_busy3,t_busy4) " + 
                "VALUES('{0:s}',{1:d},'{2:s}','{3:s}','{4:s}','{5:s}',{6:s},{7:s},{8:s},{9:s});", //{3:d},{4:d},{5:d},{6:d},
                tp, tr.repair, tr.notes.value(), heater, nest, delims,b1,b2,b3,b4); //b1,b2,b3,b4,
            c.ExecuteNonQuery();
            return (int)c.LastInsertedId;
        }

        public void procTreeNode(TreeNode nd,int level,int parent)
        {
            for (int i = 0; i < nd.Nodes.Count; i++)
            {
                String nm = nd.Nodes[i].Text;
                int fid=0;
                if (int.TryParse(nm, out fid))
                {
                    nm = "farm " + fid.ToString();
                }
                else
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
                Application.DoEvents();
                int upp = savetier(fm.upper);
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
            if (dmy[2] == "1899" && dmy[1] == "12" && dmy[0] == "30")
                return "NULL";
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

        public uint findsurname(string sur, String sex, int cnt, int tp)
        {
            if (sur == "") return 0;
            if (cnt > 1) sur=sur.TrimEnd('ы');
            if (cnt == 1 && sex == "female") sur = sur.TrimEnd('а');
            String sx = "male";
            if (tp==2) sx="female";
            c.CommandText = "SELECT n_id FROM names WHERE n_surname='" + sur + "' AND n_sex='" + sx + "';";
            MySqlDataReader rd = c.ExecuteReader();
            if (!rd.Read())
            {
                rd.Close();
                return 0;
            }
            uint res = rd.GetUInt32(0);
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
                Application.DoEvents();
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

        public void setOption(String name, String subname, String value)
        {
            c.CommandText = "INSERT INTO options(o_name,o_subname,o_uid,o_value) VALUES('" + name + "','" + subname + "',0,'" + value + "');";
            c.ExecuteNonQuery();
        }
        public void setOption(String name,String subname,int value)
        {
            setOption(name, subname, value.ToString());
        }
        public void setOption(String name, String subname, float value)
        {
            setOption(name,subname,value.ToString());
        }

        public void setCatList(MFStringList lst, String type, String flag)
        {
            for (int i = 0; i < lst.strings.Count; i++)
            {
                for (int j = 0; j < flag.Length;j++ )
                    getCatalogValue(type, flag[j], lst.strings[i].value());
            }
        }

        public void fillTransForm()
        {
            debug("fill transform");
            for (int i=0;i<mia.transform.skinnames.Count;i++)
                setOption("price","skin"+i.ToString(),mia.transform.skinnames[i].value());
            setOption("price", "meat", mia.transform.pricePerKilo.value());
            setOption("price", "feed", mia.transform.feedPrice.value());
            setCatList(mia.transform.skinBuyers, "partner", "s");
            setCatList(mia.transform.bodyBuyers, "partner", "m");
            setCatList(mia.transform.rabbitPartner, "partner", "rR");
            setCatList(mia.transform.feedPartner, "partner", "f");
            setCatList(mia.transform.kind, "kind", "x");
            setCatList(mia.transform.otherPartner, "partner", "oO");
            setCatList(mia.transform.feedType, "name", "f");
            setCatList(mia.transform.otherKind, "kind", "oO");
            setCatList(mia.transform.otherProduct, "name", "oO");
            setCatList(mia.transform.usedFeedType, "name", "f");
            setCatList(mia.transform.usedFeedSpec, "kind", "f");
            setCatList(mia.transform.otsevBuyer, "partner", "x");
        }

        public int getUniqueRabbit(int unique)
        {
            if (unique == 0) return 0;
            /*c.CommandText = "SELECT r_id FROM rabbits WHERE r_unique=" + unique.ToString() + ";";
            MySqlDataReader rd = c.ExecuteReader();
            rd.Read();
            int res = rd.GetInt32(0);
            rd.Close();
            return res;
             * */
            foreach (Rabbit r in mia.rabbits.rabbits)
            {
                if ((int)r.unique.value() == unique)
                    return r.notes.tag;
            }
            return 0;
        }

        public void fillZooForm()
        {
            debug("fill ZooForm");
            DateTime dt = mia.zooform.zoodate.value();
            String memo="";
            for (int i = 0; i < mia.zooform.strings.Count; i++)
            {
                memo += mia.zooform.strings[i].value() + "\r\n";
            }
            c.CommandText = "INSERT INTO zooplans(z_date,z_memo) VALUES("+convdt(dt)+",'"+MySqlHelper.EscapeString(memo)+"');";
            c.ExecuteNonQuery();
            for (int i = 0; i < mia.zooform.donors.Count; i++)
            {
                Application.DoEvents();
                Donor d = mia.zooform.donors[i];
                c.CommandText=String.Format("INSERT INTO zooplans(z_date,z_job,z_rabbit,z_address,z_address2) VALUES({0:s},666,{1:d},{2:d},{3:d});",
                    convdt(dt),getUniqueRabbit((int)d.unique.value()),d.surplus.value(),d.immediate.value());
                c.ExecuteNonQuery();
                int did=(int)c.LastInsertedId;
                for (int j = 0; j < d.acc.Count; j++)
                {
                    Acceptor a = d.acc[j];
                    c.CommandText=String.Format("INSERT INTO zooacceptors(z_id,z_rabbit,z_lack,z_hybrid,z_new_group,z_gendiff,z_distance,z_best_donor,z_best_acceptor) "+
                        "VALUES({0:d},{1:d},{2:d},{3:d},{4:d},{5:d},{6:d},{7:d},{8:d});",did,getUniqueRabbit((int)a.unique.value()),
                        a.lack.value(),a.hybrid.value(),a.newgroup.value(),a.gendiff.value(),a.distance.value(),
                        getUniqueRabbit((int)a.donor_best.value()),getUniqueRabbit((int)a.acceptor_best.value()));
                    c.ExecuteNonQuery();
                }
            }
            for (int i = 0; i < mia.zooform.zoojobs.Count; i++)
            {
                Application.DoEvents();
                ZooJob j = mia.zooform.zoojobs[i];
                c.CommandText = String.Format("INSERT INTO zooplans(z_date,z_job,z_level,z_rabbit,z_notes) "+
                    "VALUES({0:s},{1:d},{2:s},{3:d},'{4:s}');",convdt(dt),j.type.value()+1,j.caption.value(),
                    getUniqueRabbit(j.uniques[0]),((j.subcount.value()>=7)?j.subs[6].value():""));
                c.ExecuteNonQuery();
                int jid = (int)c.LastInsertedId;
                for (int k = 1; k < (int)j.uniquescnt.value(); k++)
                {
                    String field="z_rabbit2";
                    if (k==2) field="z_address";
                    if (k==3) field="z_address2";
                    c.CommandText = String.Format("UPDATE zooplans SET {1:s}={2:d} WHERE z_id={0:d};",jid,
                        field,j.uniques[k]);
                    c.ExecuteNonQuery();
                }
            }

        }

        public int getWorker(String name,bool insert)
        {
            if (name == "") name = "undefined";
            c.CommandText = "SELECT w_id FROM workers WHERE w_name='"+name+"';";
            MySqlDataReader rd = c.ExecuteReader();
            int res=0;
            if (rd.HasRows)
            {
                rd.Read();
                res = rd.GetInt32(0);
                rd.Close();
            }
            else
            {
                rd.Close();
                if (!insert)
                    return 0;
                c.CommandText = "INSERT INTO workers(w_name) VALUES('" + name + "');";
                c.ExecuteNonQuery();
                res = (int)c.LastInsertedId;
            }
            return res;
        }
        public int getReason(String name)
        {
            if (name == "") return 0;
            c.CommandText = "SELECT d_id FROM deadreasons WHERE d_name='"+name+"'";
            MySqlDataReader rd = c.ExecuteReader();
            if (!rd.HasRows)
            {
                rd.Close();
                return 0;
            }
            rd.Read();
            int res = rd.GetInt32(0);
            rd.Close();
            return res;
        }

        public void fillDeadFromLost()
        {
            debug("filling Dead Rabbits From Lost List");
            for (int i = 0; i < mia.graphform.reasons.size.value(); i++)
            {
                Application.DoEvents();
                MFListItem li = mia.graphform.reasons.items[i];
                c.CommandText = String.Format("INSERT INTO deadreasons(d_name,d_rate) VALUES('{0:s}',{1:s});",
                    li.caption.value(),li.subs[0].value());
                c.ExecuteNonQuery();
            }
            int cnt = (int)mia.graphform.lost.size.value();
            for (int i = 0; i < mia.graphform.lost.size.value(); i++)
            {
                mia.setpb(100 *i/ cnt);
                Application.DoEvents();
                MFListItem li = mia.graphform.lost.items[i];
                String sex = "void";
                if (li.subs[2].value() == "м") sex = "male";
                if (li.subs[2].value() == "ж") sex = "female";
                int weight = 0;
                if (!int.TryParse(li.subs[5].value(),out weight))
                {
                    weight=0;
                }
                String ddt = li.caption.value();
                String[] names = li.subs[0].value().Split(' ');
                String address = li.subs[1].value();
                String stat = li.subs[3].value();
                int age = int.Parse(li.subs[4].value());
                int group = 1;
                String notes = "";
                if (li.subitems.value() >= 9)
                    notes = li.subs[8].value();
                int reason = getReason(li.subs[6].value());
                if (names[names.Length - 1].Length>0)
                if (names[names.Length - 1].Trim()[0] == '[')
                {
                    String grp = names[names.Length - 1].Trim();
                    grp=grp.Replace("[", "").Replace("]","");
                    group = int.Parse(grp);
                    names[names.Length - 1] = "";
                }
                String nm = names[0];
                String xx = "";
                uint nid = findname(nm, ref xx);
                if (nid != 0)
                {
                    nm = "";
                    if (names.Length>1)
                        nm = names[1];
                }

                uint suid = 0;uint seid = 0;
                String[] nms = nm.Split('-');
                if (nms.Length>0)
                if (nms[0].Trim()!="")
                    suid = findsurname(nms[0].Trim(), sex, group, 2);
                if (nms.Length > 1 && nms[1].Trim() != "")
                    seid = findsurname(nms[1].Trim(), sex, group, 1);
                int farm = 0;
                int tier = 0;
                int tier_id = 0;
                int area = 0;
                string sa = "";
                int j=0;
                while (address[j] >= '0' && address[j] <= '9'){sa += address[j];j++;}
                if (sa != "")
                {
                    farm = int.Parse(sa);
                    if (address[j] == '^') { tier_id = 1; j++; }
                    if (address[j] == '-') { tier_id = 2; j++; }
                    if (address[j] == 'б') area = 1;
                    if (address[j] == 'в') area = 2;
                    if (address[j] == 'г') area = 3;
                }
                c.CommandText = String.Format(@"INSERT INTO rabbits(r_sex,r_name,r_surname,r_secname,r_notes,r_group,r_born,r_farm,r_tier_id,r_tier,r_area) 
VALUES('{0:s}',{1:d},{2:d},{3:d},'{4:s}',{5:d},{6:s}-INTERVAL {7:d} DAY,{8:d},{9:d},{10:d},{11:d});",
                                  sex,nid,suid,seid,notes,group,convdt(ddt),age,farm,tier_id,tier,area);
                c.ExecuteNonQuery();
                uint lid = (uint)c.LastInsertedId;
                c.CommandText = "CALL killRabbitDate("+lid.ToString()+","+reason.ToString()+",'списан из старой программы',"+convdt(ddt)+");";
                c.ExecuteNonQuery();
            }
        }

        public void fillGraphForm()
        {
            debug("fill GraphForm");
            for (int i = 0; i < mia.graphform.workers.size.value(); i++)
            {
                Application.DoEvents();
                MFListItem li = mia.graphform.workers.items[i];
                int wid = getWorker(li.caption.value(),true);
                int rate = int.Parse(li.subs[0].value());
                c.CommandText = "UPDATE workers SET w_rate=" + rate.ToString() + " WHERE w_id=" + wid.ToString() + ";";
                c.ExecuteNonQuery();
            }
            /*
            int cnt = (int)mia.graphform.lost.size.value();
            for (int i = 0; i < mia.graphform.lost.size.value(); i++)
            {
                mia.setpb(100 *i/ cnt);
                Application.DoEvents();
                MFListItem li = mia.graphform.lost.items[i];
                String sex = "void";
                if (li.subs[2].value() == "м") sex = "male";
                if (li.subs[2].value() == "ж") sex = "female";
                int weight = 0;
                if (!int.TryParse(li.subs[5].value(),out weight))
                {
                    weight=0;
                }                c.CommandText = String.Format("INSERT INTO drops(d_date,d_name,d_address,d_sex,d_state,d_age,d_weight,d_notes,d_reason,d_worker) "+
                    "VALUES({0:s},'{1:s}','{2:s}','{3:s}','{4:s}',{5:d},{6:d},'{7:s}',{8:d},{9:d});",
                    convdt(li.caption.value()),li.subs[0].value(),li.subs[1].value(),sex,li.subs[3].value(),
                    int.Parse(li.subs[4].value()),weight,(li.subitems.value()>=9?li.subs[8].value():""),
                    getReason(li.subs[6].value()),getWorker(li.subs[7].value(),false)
                    );
                c.ExecuteNonQuery();
            }
             * */ 
        }

        public void fillArcForm()
        {
            debug("fill ArcForm");
            foreach (ArcPlan p in mia.arcform.plans)
            {
                DateTime dt=p.date.value();
                foreach (MFStringList sl in p.works)
                {
                    String cmd = "INSERT INTO archive(a_date,a_level,a_job,a_address,a_name,a_age";
                    String vals=String.Format("VALUES({0:s},{1:d},'{2:s}','{3:s}','{4:s}',{5:d}",
                        convdt(dt),int.Parse(sl.strings[0].value()),sl.strings[1].value(),sl.strings[2].value(),
                        sl.strings[3].value(),int.Parse(sl.strings[4].value()));
                    if (sl.count.value() > 5)
                    {cmd += ",a_partners";vals += ",'" + sl.strings[5].value()+"'";}
                    if (sl.count.value() > 6)
                    { cmd += ",a_addresses"; vals += ",'" + sl.strings[6].value() + "'"; }
                    if (sl.count.value() > 7)
                    { cmd += ",a_notes"; vals += ",'" + sl.strings[7].value() + "'"; }
                    c.CommandText = cmd + ") " + vals + ");";
                    c.ExecuteNonQuery();
                }
            }
            fillDead();
        }

    }
}
