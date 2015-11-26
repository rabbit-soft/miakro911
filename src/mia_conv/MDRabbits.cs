using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace mia_conv
{
    partial class MDCreator
    {
        //private int _curRabbit=1;

        public int GetNameId(ushort key, ushort sex, bool name)
        {
            if (sex == 0)
                return 0;
            if (key == 0) return 0;
            int res = 0;
            MFRabNames rn = (sex == 1 ? Mia.MaleNames : Mia.FemaleNames);
            for (int i = 0; (i < rn.Rabnames.Count) && (res == 0); i++) {
                if (name) {
                    if (rn.Rabnames[i].Key.value() == key)
                        res = rn.Rabnames[i].Key.tag;
                } else {
                    if (rn.Rabnames[i].Surkey.value() == key)
                        res = rn.Rabnames[i].Key.tag;
                }
            }
            return res;
        }

        public int Rabbyname(int nid)
        {
            _cmd.CommandText = "SELECT r_id FROM rabbits WHERE r_name=" + nid.ToString() + ";";
            MySqlDataReader rd = _cmd.ExecuteReader();
            int res = 0;
            if (rd.Read())
                res = rd.GetInt32(0);
            rd.Close();
            return res;
        }

        public int GetTier(int farm, int level)
        {
            _cmd.CommandText = "SELECT " + (level == 1 ? "m_lower" : "m_upper") + " FROM minifarms WHERE m_id=" + farm.ToString() + ";";
            MySqlDataReader rd = _cmd.ExecuteReader();
            rd.Read();
            int res = rd.GetInt32(0);
            rd.Close();
            return res;
        }

        /// <summary>
        /// Назначет Секции в ярусе, ID кролика, который там сидит.
        /// </summary>
        /// <param name="tier">Номер яруса</param>
        /// <param name="busy">Номер секции</param>
        /// <param name="rabbit">Сквозной ID кролика, который присваивается во время конвертации из .mia-файла</param>
        public void SetTierUser(int tier, int busy, int rabbit)
        {
            _cmd.CommandText = String.Format("UPDATE tiers SET t_busy{0:d}={1:d} WHERE t_id={2:d};", (busy + 1), rabbit, tier.ToString());
            _cmd.ExecuteNonQuery();
        }

        public void SetNameUser(int nameid, int rabbit)
        {
            _cmd.CommandText = "UPDATE names SET n_use=" + rabbit.ToString() + " WHERE n_id=" + nameid.ToString() + ";";
            _cmd.ExecuteNonQuery();
        }

        public int MakeGenesis(List<ushort> gen)
        {
            String s = "";
            for (int i = 0; i < gen.Count; i++) {
                s += gen[i].ToString();
                if (i < gen.Count - 1) s += " ";
            }
            _cmd.CommandText = "SELECT g_id FROM genesis WHERE g_key=MD5('" + s + "');";
            MySqlDataReader rd = _cmd.ExecuteReader();
            int res = 0;
            if (rd.HasRows) {
                rd.Read();
                res = rd.GetInt32(0);
                rd.Close();
            } else {
                rd.Close();
                _cmd.CommandText = "INSERT INTO genesis(g_notes) VALUES('');";
                _cmd.ExecuteNonQuery();
                res = (int)_cmd.LastInsertedId;
                foreach (ushort g in gen) {
                    _cmd.CommandText = "INSERT INTO genoms(g_id,g_genom) VALUES(" + res.ToString() + "," + g.ToString() + ");";
                    _cmd.ExecuteNonQuery();
                }
                _cmd.CommandText = "UPDATE genesis SET g_key=(SELECT MD5(GROUP_CONCAT(g_genom ORDER BY g_genom ASC SEPARATOR ' ')) FROM genoms WHERE g_id=" + res.ToString() + ") WHERE g_id=" + res.ToString() + ";";
                _cmd.ExecuteNonQuery();
            }
            return res;
        }

        public int MakeWorker(String wname)
        {
            if (wname == "") return 0;
            _cmd.CommandText = "SELECT w_id FROM workers WHERE w_name='" + wname + "';";
            MySqlDataReader rd = _cmd.ExecuteReader();
            int res = 0;
            if (rd.HasRows) {
                rd.Read();
                res = rd.GetInt32(0);
                rd.Close();
            } else {
                rd.Close();
                _cmd.CommandText = "INSERT INTO workers(w_name) VALUES('" + wname + "');";
                _cmd.ExecuteNonQuery();
                res = (int)_cmd.LastInsertedId;
            }
            return res;
        }

        public void FillRabWeight(Rabbit r, int crab)
        {
            _cmd.CommandText = "ALTER TABLE `weights` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();
            for (int i = 0; i < r.weights.Count; i++) {
                ushort weight = (ushort)(r.weights[i] & 0xFFFF);
                ushort dt = (ushort)((r.weights[i] >> 16) & 0xFFFF);
                DateTime sdt = (new DateTime(1899, 12, 30)).AddDays(dt);
                _cmd.CommandText = String.Format("INSERT INTO weights(w_rabid,w_date,w_weight) VALUES({0:d},{1:s},{2:d});", crab, Convdt(sdt), weight);
                _cmd.ExecuteNonQuery();
            }
            _cmd.CommandText = "ALTER TABLE `weights` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
        }

        private int FindDead(String name, int breed, List<ushort> genesis)
        {
            _cmd.CommandText = "SELECT r_id FROM dead,names WHERE r_name=n_id AND n_name='" + name + "';";
            MySqlDataReader rd = _cmd.ExecuteReader();
            int res = 0;
            if (rd.Read()) {
                res = rd.GetInt32(0);
                rd.Close();
                int gen = MakeGenesis(genesis);
                _cmd.CommandText = String.Format("UPDATE dead SET r_breed={0:d},r_genesis={1:d} WHERE r_id={2:d};", FindBreed(breed), gen, res);
                _cmd.ExecuteNonQuery();
            } else {
                rd.Close();
            }
            return res;

        }

        public void FillRabFuckers(Rabbit r, int crab, bool sukrol)
        {
            _cmd.CommandText = "ALTER TABLE `fucks` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();
            foreach (Fucker f in r.female.fuckers) {
                if (f.fucks.value() > 0 || f.my_fuck_is_last.value() == 1) {
                    int link = 0;
                    int dead = 0;
                    if (f.live.value() == 1) {
                        link = GetNameId((ushort)f.name_key.value(), 1, true);
                        //link = rabbyname(link);
                        dead = 1;
                    } else {
                        link = FindDead(f.name.value(), (int)f.breed.value(), f.genesis);
                    }
                    String state = "okrol";
                    if (f.my_fuck_is_last.value() == 1 && sukrol) {
                        state = "sukrol";
                    }
                    String type = "vyazka";
                    if ((r.female.borns.value() == 0) || (r.female.borns.value() == 1 && state != "sukrol")) {
                        type = "sluchka";
                    }
                    _cmd.CommandText = String.Format(@"INSERT INTO fucks(f_rabid,f_partner,f_times,f_children,f_dead,f_state,f_last,f_type) 
VALUES({0:d},{1:d},{2:d},{3:d},{4:d},'{5:s}',{6:d},'{7:d}');",
                      crab, link, (int)f.fucks.value() + (sukrol ? 1 : 0), f.children.value(), dead, state,
                      f.my_fuck_is_last.value(), type);
                    _cmd.ExecuteNonQuery();
                }
            }
            _cmd.CommandText = "ALTER TABLE `fucks` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();

        }

        /// <summary>
        /// Вносит живого кролика в БД.
        /// </summary>
        /// <param name="r">Кролик-объект</param>
        /// <param name="parent">Родитель, с которым сидит</param>
        /// <param name="dead">Мертвый, да?</param>
        public void FillRabbit(Rabbit r, int parent, bool dead)
        {
            //Application.DoEvents();
            String query = "INSERT INTO " + (dead ? "dead" : "rabbits") + @"(r_sex,r_parent,r_bon,r_name,r_surname,
                r_secname,r_notes,r_okrol,r_farm,r_tier_id,r_tier,r_area,r_rate,r_group,r_breed,r_flags,r_zone,
                r_born,r_status,r_last_fuck_okrol,r_genesis";
            String sex = "void";
            if (r.sex.value() == 1) {
                sex = "male";
            }
            if (r.sex.value() == 2) {
                sex = "female";
            }
            String bon = String.Format("{0:D1}{1:D1}{2:D1}{3:D1}{4:D1}", r.bon.manual.value(), r.bon.weight.value(), r.bon.body.value(), r.bon.hair.value(), r.bon.color.value());
            String vals = String.Format("VALUES('{0:s}',{1:d},'{2:s}'", sex, parent, bon);//,{3:d},{4:d}  ,r.number.value(),r.unique.value());
            int name = GetNameId((ushort)r.namekey.value(), (ushort)r.sex.value(), true);
            int surname = GetNameId((ushort)r.surkey.value(), 2, false);
            int secname = GetNameId((ushort)r.pathkey.value(), 1, false);
            vals += String.Format(",{0:d},{1:d},{2:d},'{3:s}',{4:d}", name, surname, secname, r.notes.value(), r.okrol_num.value());

            String flags = String.Format("{0:D1}{1:D1}{2:D1}", r.butcher.value(), r.risk.value(), r.multi.value());
            if (r.sex.value() == 2) {
                flags += String.Format("{0:D1}{1:D1}", r.female.no_kuk.value(), r.female.no_lact.value());
            } else {
                flags += "00";
            }

            int tier = GetTier((int)r.where.value(), (int)r.tier_id.value());
            if (parent == 0) {
                int tier_id = r.tier_id.value() == 1 ? 2 : 1; //здесь перепутаны местами нежели в строениях
                vals += String.Format(",{0:d},{1:d},{2:d},{3:d}", r.where.value(), tier_id, tier, r.area.value());
            } else {
                vals += String.Format(",{0:d},{1:d},{2:d},{3:d}", 0, 0, 0, 0);
            }
            vals += String.Format(",{0:d},{1:d},{2:d},'{3:s}',{4:d}", 0/*r.rate.value()*/, r.group.value(),
                FindBreed((int)r.breed.value()), flags, r.zone.value());

            int status = 0;
            String lfo = "NULL";
            if (r.sex.value() == 1) {
                status = (int)r.status.value();
                lfo = Convdt(r.lastfuck.value());
            }
            if (r.sex.value() == 2) {
                status = (int)r.female.borns.value();
                lfo = Convdt(r.female.last_okrol.value());
            }
            bool sukrol = false;
            vals += String.Format(",{0:s},{1:d},{2:s},{3:d}", Convdt(r.borndate.value()), status, lfo, MakeGenesis(r.genesis));
            if (r.sex.value() == 2) {
                query += ",r_event,r_event_date,r_lost_babies,r_overall_babies";//,r_worker";,r_children
                String ev = "none";
                switch (r.female.ev_type.value()) {
                    case 1: ev = "sluchka"; break;
                    case 2: ev = "vyazka"; break;
                    case 3: ev = "kuk"; break;
                }
                String edt = Convdt(r.female.ev_date.value());
                sukrol = (edt != "NULL");
                vals += String.Format(",'{0:s}',{1:s},{2:d},{3:d}", ev,
                    edt, r.female.lost_babies.value(), r.female.overall_babies.value());//,  ,{5:d},{0:d}
                //makeWorker(r.female.worker.value()));  //r.female.child_count.value(),
            }
            _cmd.CommandText = query + ") " + vals + ");";
            _cmd.ExecuteNonQuery();

            int crab = (int)_cmd.LastInsertedId;
            r.notes.tag = crab;
            //_curRabbit = crab + 1;
            if (dead) return;


            if (parent == 0) {
                SetTierUser(tier, (int)r.area.value(), crab);
            }
            if (name != 0) {
                SetNameUser(name, crab);
            }

            FillRabWeight(r, crab);
            if (r.sex.value() == 2) {
                FillRabFuckers(r, crab, sukrol);
                foreach (Rabbit xr in r.female.suckers.rabbits) {
                    FillRabbit(xr, crab, false);
                }
            }
        }

        public void NormalizeFuckers()
        {

            _cmd.CommandText = "UPDATE fucks SET f_partner=(SELECT n_use FROM names WHERE n_id=fucks.f_partner),f_dead=0 WHERE f_dead=1;";
            _cmd.ExecuteNonQuery();
            //_cmd.CommandText = "UPDATE rabbits SET r_vaccine_end=NOW()+interval 365 day WHERE r_flags LIKE '__2__' OR r_flags LIKE '__3__';";
            //_cmd.ExecuteNonQuery();
            //_cmd.CommandText = "UPDATE rabbits SET r_vaccine_end=NOW() WHERE r_flags LIKE '__0__';";
            //_cmd.ExecuteNonQuery();
            _cmd.CommandText = "UPDATE rabbits SET r_flags=CONCAT(SUBSTR(r_flags,1,1),SUBSTR(r_flags,2,1),'0',SUBSTR(r_flags,4,1),SUBSTR(r_flags,5,1)) where r_flags LIKE '__2__';";
            _cmd.ExecuteNonQuery();
            _cmd.CommandText = "UPDATE rabbits SET r_flags=CONCAT(SUBSTR(r_flags,1,1),SUBSTR(r_flags,2,1),'1',SUBSTR(r_flags,4,1),SUBSTR(r_flags,5,1)) where r_flags LIKE '__3__'";
            _cmd.ExecuteNonQuery();
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
            int cnt = Mia.Rabbits.rabbits.Count;
            int i = 0;
            _cmd.CommandText = "ALTER TABLE `rabbits` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();
            foreach (Rabbit r in Mia.Rabbits.rabbits) {
                i++;
                Mia.Setpb(i, cnt);
                FillRabbit(r, 0, false);
            }
            _cmd.CommandText = "ALTER TABLE `rabbits` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
            Debug("normalizing fuckers");
            NormalizeFuckers();
        }

        //public void FillDead()
        //{
        //    _cmd.CommandText = "ALTER TABLE `dead` DISABLE KEYS;";
        //    _cmd.ExecuteNonQuery();
        //    Debug("fill dead");
        //    foreach (Rabbit r in Mia.Arcform.dead.rabbits)
        //    {
        //        FillRabbit(r, 0, true);
        //    }
        //    _cmd.CommandText = "ALTER TABLE `dead` ENABLE KEYS;";
        //    _cmd.ExecuteNonQuery();
        //}

    }
}
