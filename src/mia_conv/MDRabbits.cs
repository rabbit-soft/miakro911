using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace mia_conv
{
    partial class MDCreator
    {
        private int _curRabbit=1;

        public int GetNameId(ushort key, ushort sex,bool name)
        {
            if (sex==0)
                return 0;
            if (key == 0) return 0;
            int res = 0;
            MFRabNames rn = (sex == 1 ? Mia.MaleNames : Mia.FemaleNames);
            for (int i=0;(i<rn.Rabnames.Count) && (res==0);i++)
            {
                if (name)
                {
                    if (rn.Rabnames[i].Key.value() == key)
                        res = rn.Rabnames[i].Key.tag;
                }
                else
                {
                    if (rn.Rabnames[i].Surkey.value() == key)
                        res = rn.Rabnames[i].Key.tag;
                }
            }
            return res;
        }
        
        public int Rabbyname(int nid)
        {
            C.CommandText="SELECT r_id FROM rabbits WHERE r_name="+nid.ToString()+";";
            MySqlDataReader rd = C.ExecuteReader();
            int res = 0;
            if (rd.Read())
                res = rd.GetInt32(0);
            rd.Close();
            return res;
        }

        public int GetTier(int farm, int level)
        {

            C.CommandText = "SELECT " + (level == 1 ? "m_lower" : "m_upper") + " FROM minifarms WHERE m_id=" + farm.ToString() + ";";
            MySqlDataReader rd = C.ExecuteReader();
            rd.Read();
            int res = rd.GetInt32(0);
            rd.Close();
            return res;
        }

        public void Settieruser(int tier,int busy,int rabbit)
        {
            C.CommandText="UPDATE tiers SET t_busy"+(busy+1).ToString()+"="+rabbit.ToString()+
                " WHERE t_id="+tier.ToString()+";";
            C.ExecuteNonQuery();
        }

        public void Setnameuser(int nameid,int rabbit)
        {
            C.CommandText="UPDATE names SET n_use="+rabbit.ToString()+" WHERE n_id="+nameid.ToString()+";";
            C.ExecuteNonQuery();
        }

        public int MakeGenesis(List<ushort> gen)
        {
            String s = "";
            for (int i = 0; i < gen.Count;i++ )
            {
                s += gen[i].ToString();
                if (i < gen.Count - 1) s += " ";
            }
            C.CommandText = "SELECT g_id FROM genesis WHERE g_key=MD5('"+s+"');";
            MySqlDataReader rd = C.ExecuteReader();
            int res = 0;
            if (rd.HasRows)
            {
                rd.Read();
                res = rd.GetInt32(0);
                rd.Close();
            }
            else
            {
                rd.Close();
                C.CommandText = "INSERT INTO genesis(g_notes) VALUES('');";
                C.ExecuteNonQuery();
                res = (int)C.LastInsertedId;
                foreach (ushort g in gen)
                {
                    C.CommandText = "INSERT INTO genoms(g_id,g_genom) VALUES("+res.ToString()+","+g.ToString()+");";
                    C.ExecuteNonQuery();
                }
                C.CommandText = "UPDATE genesis SET g_key=(SELECT MD5(GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ')) FROM genoms WHERE g_id="+res.ToString()+") WHERE g_id=" + res.ToString() + ";";
                C.ExecuteNonQuery();
            }
            return res;
        }

        public int MakeWorker(String wname)
        {
            if (wname == "") return 0;
            C.CommandText = "SELECT w_id FROM workers WHERE w_name='" + wname + "';";
            MySqlDataReader rd = C.ExecuteReader();
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
                C.CommandText = "INSERT INTO workers(w_name) VALUES('"+wname+"');";
                C.ExecuteNonQuery();
                res = (int)C.LastInsertedId;
            }
            return res;
        }

        public void FillRabWeight(Rabbit r,int crab)
        {
            C.CommandText = "ALTER TABLE `weights` DISABLE KEYS;";
            C.ExecuteNonQuery();
            for (int i = 0; i < r.weights.Count; i++)
            {
                ushort weight=(ushort)(r.weights[i] & 0xFFFF);
                ushort dt=(ushort)((r.weights[i] >> 16)& 0xFFFF);
                DateTime sdt = (new DateTime(1899, 12, 30)).AddDays(dt);
                C.CommandText = String.Format("INSERT INTO weights(w_rabid,w_date,w_weight) VALUES({0:d},{1:s},{2:d});",crab,Convdt(sdt),weight);
                C.ExecuteNonQuery();
            }
            C.CommandText = "ALTER TABLE `weights` ENABLE KEYS;";
            C.ExecuteNonQuery();
        }

        private int FindDead(String name,int breed,List<ushort> genesis)
        {
            C.CommandText = "SELECT r_id FROM dead,names WHERE r_name=n_id AND n_name='"+name+"';";
            MySqlDataReader rd = C.ExecuteReader();
            int res = 0;
            if (rd.Read())
            {
                res = rd.GetInt32(0);
                rd.Close();
                int gen=MakeGenesis(genesis);
                C.CommandText = String.Format("UPDATE dead SET r_breed={0:d},r_genesis={1:d} WHERE r_id={2:d};",
                    Findbreed(breed), gen, res);
                C.ExecuteNonQuery();
            }
            else
                rd.Close();
            return res;

        }

        public void FillRabFuckers(Rabbit r, int crab,bool sukrol)
        {
            C.CommandText = "ALTER TABLE `fucks` DISABLE KEYS;";
            C.ExecuteNonQuery();
            foreach (Fucker f in r.female.fuckers)
            {
                if (f.fucks.value() > 0 || f.my_fuck_is_last.value()==1)
                {
                    int link = 0;
                    int dead = 0;
                    if (f.live.value() == 1)
                    {
                        link = GetNameId((ushort)f.name_key.value(), 1,true);
                        //link = rabbyname(link);
                        dead = 1;
                    }
                    else
                    {
                        link = FindDead(f.name.value(), (int)f.breed.value(), f.genesis);
                    }
                    String state = "okrol";
                    if (f.my_fuck_is_last.value() == 1 && sukrol)
                        state = "sukrol";
                    String type = "vyazka";
                    if ((r.female.borns.value() == 0)||(r.female.borns.value()==1 && state!="sukrol") )
                        type = "sluchka";
                    C.CommandText = String.Format(@"INSERT INTO fucks(f_rabid,f_partner,f_times,f_children,f_dead,f_state,f_last,f_type) 
VALUES({0:d},{1:d},{2:d},{3:d},{4:d},'{5:s}',{6:d},'{7:d}');", 
                      crab, link, (int)f.fucks.value()+(sukrol?1:0),f.children.value(),dead,state,
                      f.my_fuck_is_last.value(),type);
                    C.ExecuteNonQuery();
                }
            }
            C.CommandText = "ALTER TABLE `fucks` ENABLE KEYS;";
            C.ExecuteNonQuery();
            
        }

        public void FillRabbit(Rabbit r,int parent,bool dead)
        {
            //Application.DoEvents();
            String cmd = "INSERT INTO " + (dead ? "dead" : "rabbits") + "(r_sex,r_parent,r_bon,r_name,r_surname," + //r_number,r_unique,
                "r_secname,r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,"+
                "r_born,r_status,r_last_fuck_okrol,r_genesis";
            String sex = "void";
            if (r.sex.value() == 1) sex = "male"; if (r.sex.value() == 2) sex = "female";
            String bon = String.Format("{0:D1}{1:D1}{2:D1}{3:D1}{4:D1}", r.bon.manual.value(), r.bon.weight.value(), r.bon.body.value(), r.bon.hair.value(), r.bon.color.value());
            String vals = String.Format("VALUES('{0:s}',{1:d},'{2:s}'", sex, parent, bon);//,{3:d},{4:d}  ,r.number.value(),r.unique.value());
            int name = GetNameId((ushort)r.namekey.value(), (ushort)r.sex.value(),true);
            int surname = GetNameId((ushort)r.surkey.value(), 2,false);
            int secname = GetNameId((ushort)r.pathkey.value(), 1,false);
            vals += String.Format(",{0:d},{1:d},{2:d},'{3:s}',{4:d}", name, surname, secname,r.notes.value(),r.okrol_num.value());
            int tier=GetTier((int)r.where.value(),(int)r.tier_id.value());
            if (parent == 0)
                Settieruser(tier, (int)r.area.value(), _curRabbit);
            if (name != 0)
                Setnameuser(name, _curRabbit);
            String flags = String.Format("{0:D1}{1:D1}{2:D1}",r.butcher.value(),r.risk.value(),r.multi.value());
            if (r.sex.value() == 2)
                flags += String.Format("{0:D1}{1:D1}",r.female.no_kuk.value(),r.female.no_lact.value());
            else
                flags += "00";
            if (parent==0)
                vals += String.Format(",{0:d},{1:d},{2:d},{3:d}",r.where.value(),r.tier_id.value(),tier,r.area.value());
            else
                vals += String.Format(",{0:d},{1:d},{2:d},{3:d}", 0, 0, 0, 0);
            vals += String.Format(",{0:d},{1:d},{2:d},'{3:s}',{4:d}", 0/*r.rate.value()*/, r.group.value(),
                Findbreed((int)r.breed.value()),flags,r.zone.value());
            int status=0;
            String lfo="NULL";
            if (r.sex.value()==1) {
                status=(int)r.status.value();
                lfo=Convdt(r.lastfuck.value());
            }
            if (r.sex.value()==2) {
                status=(int)r.female.borns.value();
                lfo=Convdt(r.female.last_okrol.value());
            }
            bool sukrol = false;
            vals += String.Format(",{0:s},{1:d},{2:s},{3:d}", Convdt(r.borndate.value()), status, lfo,MakeGenesis(r.genesis));
            if (r.sex.value() == 2)
            {
                cmd += ",r_event,r_event_date,r_lost_babies,r_overall_babies";//,r_worker";,r_children
                String ev="none";
                switch (r.female.ev_type.value())
                {
                    case 1:ev="sluchka";break;
                    case 2:ev="vyazka";break;
                    case 3:ev="kuk";break;
                }
                String edt=Convdt(r.female.ev_date.value());
                sukrol = (edt != "NULL");
                vals+=String.Format(",'{0:s}',{1:s},{2:d},{3:d}",ev,
                    edt, r.female.lost_babies.value(), r.female.overall_babies.value());//,  ,{5:d},{0:d}
                //makeWorker(r.female.worker.value()));  //r.female.child_count.value(),
            }
            C.CommandText = cmd + ") " + vals + ");";
            C.ExecuteNonQuery();
            int crab = (int)C.LastInsertedId;
            r.notes.tag = crab;
            _curRabbit = crab + 1;
            if (dead)
                return;
            FillRabWeight(r, crab);
            if (r.sex.value() == 2)
            {
                FillRabFuckers(r, crab, sukrol);
                foreach (Rabbit xr in r.female.suckers.rabbits)
                {
                    FillRabbit(xr, crab, false);
                }
            }
        }

        public void NormalizeFuckers()
        {
            
            C.CommandText = "UPDATE fucks SET f_partner=(SELECT n_use FROM names WHERE n_id=fucks.f_partner),f_dead=0 WHERE f_dead=1;";
            C.ExecuteNonQuery();
            C.CommandText = "UPDATE rabbits SET r_vaccine_end=NOW()+interval 365 day WHERE r_flags LIKE '__2__' OR r_flags LIKE '__3__';";
            C.ExecuteNonQuery();
            C.CommandText = "UPDATE rabbits SET r_vaccine_end=NOW() WHERE r_flags LIKE '__0__';";
            C.ExecuteNonQuery();
            C.CommandText = "UPDATE rabbits SET r_flags=CONCAT(SUBSTR(r_flags,1,1),SUBSTR(r_flags,2,1),'0',SUBSTR(r_flags,4,1),SUBSTR(r_flags,5,1)) where r_flags LIKE '__2__';";
            C.ExecuteNonQuery();
            C.CommandText = "UPDATE rabbits SET r_flags=CONCAT(SUBSTR(r_flags,1,1),SUBSTR(r_flags,2,1),'1',SUBSTR(r_flags,4,1),SUBSTR(r_flags,5,1)) where r_flags LIKE '__3__'";
            C.ExecuteNonQuery();
            /*
            c.CommandText = "UPDATE rabbits SET r_mother=(SELECT COALESCE(n_use,0) FROM names WHERE n_id=rabbits.r_surname AND n_sex='female') WHERE r_mother=0;";
            c.ExecuteNonQuery();
            c.CommandText = "UPDATE rabbits SET r_father=(SELECT COALESCE(n_use,0) FROM names WHERE n_id=rabbits.r_secname AND n_sex='male') WHERE r_father=0;";
            c.ExecuteNonQuery();
             * */
            /*
            c.CommandText = "UPDATE rabbits SET r_mother=(SELECT COALESCE(r_id,0) FROM dead WHERE dead.r_name=rabbits.r_surname AND dead.r_sex='female' LIMIT 1) WHERE r_mother=0;";
            c.ExecuteNonQuery();
            c.CommandText = "UPDATE rabbits SET r_father=(SELECT COALESCE(r_id,0) FROM dead WHERE dead.r_name=rabbits.r_secname AND dead.r_sex='male' LIMIT 1) WHERE r_father=0;";
            c.ExecuteNonQuery();
            */
        }

        public void FillRabbits()
        {
            Debug("fill rabbits");
            int cnt=Mia.Rabbits.rabbits.Count;
            int i=0;
            C.CommandText = "ALTER TABLE `rabbits` DISABLE KEYS;";
            C.ExecuteNonQuery();
            foreach (Rabbit r in Mia.Rabbits.rabbits)
            {
                i++;
                Mia.Setpb(i,cnt);
                FillRabbit(r, 0, false);
            }
            C.CommandText = "ALTER TABLE `rabbits` ENABLE KEYS;";
            C.ExecuteNonQuery();
            Debug("normalizing fuckers");
            NormalizeFuckers();
        }

        public void FillDead()
        {
            C.CommandText = "ALTER TABLE `dead` DISABLE KEYS;";
            C.ExecuteNonQuery();
            Debug("fill dead");
            foreach (Rabbit r in Mia.Arcform.dead.rabbits)
            {
                FillRabbit(r, 0, true);
            }
            C.CommandText = "ALTER TABLE `dead` ENABLE KEYS;";
            C.ExecuteNonQuery();
        }

    }
}
