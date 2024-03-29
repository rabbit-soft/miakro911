﻿using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;
using System.IO;
using log4net;
using rabnet;

namespace mia_conv
{
    partial class MDCreator
    {
        private enum Tables { Breeds, DeadReasons, Names, Zones }
        private ILog log = LogManager.GetLogger("MDCreator");
        private MySqlConnection _sql = null;
        private MySqlCommand _cmd = null;
        public bool OldID = false;
        public MiaFile Mia = null;
        private int _maxbreed = 0;

        public MDCreator() { }

        public void Debug(String str)
        {
            log.Debug(str);
        }

        /// <summary>
        /// Удаляет базу данных
        /// </summary>
        public static void DropDb(String root, String rpswd, String db, String host)
        {
            MySqlConnection sql = new MySqlConnection("server=" + host + ";userId=" + root + ";password=" + rpswd + ";database=mysql");
            try {
                sql.Open();
                MySqlCommand cmd = new MySqlCommand("DROP DATABASE IF EXISTS " + db + ";", sql);
                cmd.ExecuteNonQuery();
            } catch (Exception) {
            }
        }

        /// <summary>
        /// Существует ли БазаДанных
        /// </summary>
        public static bool HasDB(String root, String rpswd, String db, String host)
        {
            MySqlConnection sql = new MySqlConnection("server=" + host + ";userId=" + root + ";password=" + rpswd + ";database=" + db);
            try {
                sql.Open();
            } catch {
                return false;
            }
            sql.Close();
            return true;
        }

        /// <summary>
        /// Создает пустую базу данных, без заполнений таблицами.
        /// </summary>
        /// <returns>miaExitCode</returns>
        public int CreateDB(String root, String rpswd, String db, String host, String user, String pswd, bool throwing, bool quiet)
        {
            Debug("Creating database " + db);
            _sql = new MySqlConnection("server=" + host + ";userId=" + root + ";password=" + rpswd + ";database=mysql");
            //#if !NOCATCH
            try {
                //#endif
                if (HasDB(root, rpswd, db, host) && !quiet) {
                    if (MessageBox.Show(null, "База данных " + db + " уже существует.\nЗаменить на новую?\nДанные старой фермы будут утеряны!",
                        "БД существует", MessageBoxButtons.YesNoCancel) != DialogResult.Yes) {
                        return miaExitCode.ABORTED_BY_USER;
                    }
                }
                _sql.Open();
                MySqlCommand cmd = new MySqlCommand("DROP DATABASE IF EXISTS " + db + ";", _sql);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("CREATE DATABASE {0:s} DEFAULT CHARACTER SET UTF8;", db);
                cmd.ExecuteNonQuery();
                Debug("database created\r\nMaking db user");
                cmd.CommandText = "GRANT ALL ON " + db + ".* TO " + user + " IDENTIFIED BY '" + pswd + "';";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "GRANT ALL ON " + db + ".* TO " + user + "@'localhost' IDENTIFIED BY '" + pswd + "';";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SET GLOBAL log_bin_trust_function_creators=1;";
                cmd.ExecuteNonQuery();
#if !NOCATCH
            } catch (Exception ex) {
                if (throwing) {
                    throw ex;
                }
                return checkOnMyEx(ex);
                //MessageBox.Show(ex.Message);
                //return miaExitCode.ERROR;    

#endif
            } finally {
                 if (_sql != null) {
                     _sql.Close();
                 }
                _sql = null;
            }
            return miaExitCode.OK;
        }

        //public int Prepare(bool nudb, String host, String user, String password, String db, String root, String rpswd, bool throwing)
        //{
        //    return Prepare(nudb, host, user, password, db, root, rpswd, throwing, false);
        //}

        /// <summary>
        /// Создание Таблиц и Функций для Кроличьей базы данных
        /// </summary>
        /// <returns>miaExitCode</returns>
        public int Prepare(bool nudb, String host, String user, String password, String db, String root, String rpswd, bool throwing, bool quiet)
        {
            if (host == "" || user == "" || db == "") {
                return miaExitCode.NOT_ENOUGH_ARGS;
            }
            if (nudb) {
                int code = CreateDB(root, rpswd, db, host, user, password, throwing, quiet);
                if (code != miaExitCode.OK) {
                    return code;
                }
            } else {
                //if (!HasDB(user, password, db, host))
                //  return miaExitCode.DB_NOT_EXISTS;
                if (MessageBox.Show(null, "Данные старой фермы будут утеряны! Продолжить?",
                    "БД существует", MessageBoxButtons.YesNo) != DialogResult.Yes) {
                    return miaExitCode.ABORTED_BY_USER;
                }
            }

            _sql = new MySqlConnection("host=" + host + ";uid=" + user + ";pwd=" + password + ";database=" + db + ";charset=utf8");
#if !NOCATCH
            try {
#endif
                _sql.Open();
#if !NOCATCH
            } catch (Exception ex) {
                if (throwing) {
                    throw ex;
                }
                return checkOnMyEx(ex);
                //MessageBox.Show(ex.Message);
                //return miaExitCode.ERROR;
            }
#endif
            _cmd = new MySqlCommand("SET CHARACTER SET utf8;", _sql);
            _cmd.ExecuteNonQuery();
            StreamReader stm = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("mia_conv.rabnet_db_fmt.sql"), Encoding.UTF8);
            String cmd = stm.ReadToEnd();
            stm.Close();

            //cmd=cmd.Remove(cmd.IndexOf("##TEST_DATA"));
            String[] cmds = cmd.Split(new string[] { "#DELIMITER |" }, StringSplitOptions.RemoveEmptyEntries);
            _cmd.CommandText = cmds[0];//выполнение команд по созданию таблиц
            _cmd.ExecuteNonQuery();
            MySqlScript scr = new MySqlScript(_sql, cmds[1]);
            scr.Delimiter = "|";
            scr.Execute();//Создает функции
            return miaExitCode.OK;
        }

        /// <summary>
        /// Запускает SQL-скрипт
        /// </summary>
        public void Finish(String sqlfile)
        {
            if (sqlfile != "") {
                StreamReader rd = new StreamReader(new FileStream(sqlfile, FileMode.Open), Encoding.ASCII);
                String sq = rd.ReadToEnd();
                rd.Close();
                if (sq.Trim() != "") {
                    String[] cmds = sq.Split(new string[] { "#DELIMITER |" }, StringSplitOptions.RemoveEmptyEntries);
                    _cmd.CommandText = cmds[0];
                    _cmd.ExecuteNonQuery();
                    if (cmds.Length > 1) {
                        MySqlScript scr = new MySqlScript(_sql, cmds[1]);
                        scr.Delimiter = "|";
                        scr.Execute();
                    }
                }
            }
            _sql.Close();
        }

        /// <summary>
        /// Удалаяет данные из таблицы с Причинами списаний
        /// <remarks>
        /// При создании новой БД в справочники добавляется информация.
        /// При конверте из .mia файла нужно ее стереть справочники чтобы заполнить информацией из конвертируемой Фермы
        /// </remarks>
        /// </summary>
        private void deleteTableContent(Tables tb)
        {
            Debug("deleting " + tb.ToString().ToLower());
            _cmd.CommandText = String.Format("DELETE FROM {0:s};", tb.ToString().ToLower());
            _cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Проверяет является ли Исключение Мускульным. Если нет, то возвращает miaExitCode.ERROR
        /// </summary>
        /// <returns>miaExitCode</returns>
        private int checkOnMyEx(Exception ex)
        {
            if (ex is MySqlException) {
                MySqlException mex = (ex as MySqlException);
                switch (mex.Number) {
                    case 1042: return miaExitCode.CANT_CONNECT_TO_MYSQL;
                    case 1044: return miaExitCode.DB_NOT_EXISTS;
                    case 1045: return miaExitCode.DB_ACCESS_DENIED;
                    default: return miaExitCode.ERROR;
                }
            } else {
                return miaExitCode.ERROR;
            }
        }

        public String Decode(String data)
        {
            return new String(Encoding.Unicode.GetChars(Encoding.Convert(Encoding.UTF8, Encoding.Unicode, Encoding.UTF8.GetBytes(data))));
        }

        public void SetUsers(DataTable usrs)
        {
            Debug("adding users");
            foreach (DataRow rw in usrs.Rows) {
                Debug("adding user " + rw.ItemArray[0]);
                _cmd.CommandText = String.Format("INSERT INTO users(u_name, u_password) VALUES('{0:s}', MD5('{1:s}'));", rw.ItemArray[0] as String, rw.ItemArray[1]);
                _cmd.ExecuteNonQuery();
            }
        }

        public int SetUsers(String[] usrs)
        {
            try {
                for (int i = 0; i < usrs.Length / 2; i++) {
                    Debug("adding user " + usrs[i * 2]);
                    _cmd.CommandText = String.Format("INSERT INTO users(u_name, u_password) VALUES('{0:s}', MD5('{1:s}'));", usrs[i * 2], usrs[i * 2 + 1]);
                    _cmd.ExecuteNonQuery();
                }
            } catch (Exception exc) {
                log.Error(exc);
                return checkOnMyEx(exc);
            }
            return miaExitCode.OK;
        }

        #region fill_data

        /// <summary>
        /// Заполняет базу данными из mia-файла
        /// </summary>
        public void FillAll()
        {
            const int of = 8;
            int pgs = 0;
            Mia.Setpbpart(pgs++, of);

            _cmd.CommandText = "SET foreign_key_checks = 0;";
            _cmd.ExecuteNonQuery();

            Mia.SetLabelName("Породы");
            this.FillBreeds();
            Mia.SetLabelName("Имена");
            this.FillNames();
            Mia.Setpbpart(pgs++, of);

            Mia.SetLabelName("Зоны");
            this.FillZones();
            Mia.Setpbpart(pgs++, of);

            Mia.SetLabelName("Строения");
            this.FillBuildings();
            Mia.Setpbpart(pgs++, of);

            //Mia.SetLabelName("Переводы");
            //FillTransfers();
            //Mia.Setpbpart(pgs++, of);

            Mia.SetLabelName("Мертвые Кролики");
            this.FillDeadFromLost();
            Mia.Setpbpart(pgs++, of);

            Mia.SetLabelName("Живые Кролики");
            this.FillRabbits();
            Mia.Setpbpart(pgs++, of);

            //Mia.SetLabelName("ПереФормы");
            //FillTransForm();
            Mia.SetLabelName("Настройки");
            this.FillOptions();
            Mia.Setpbpart(pgs++, of);

            //fillZooForm();
            //Mia.SetLabelName("ГрафФормы");
            //FillGraphForm();
            Mia.SetLabelName("АркФормы");
            this.FillArcForm();
            Mia.Setpbpart(pgs++, of);

            Mia.SetLabelName("Наладка связей");
            miaRepair.Go(_cmd);
            Mia.Setpbpart(pgs++, of);
            Mia.SetLabelName("");

            _cmd.CommandText = "SET foreign_key_checks = 1;";
            _cmd.ExecuteNonQuery();
        }

        public void FillNames()
        {
            deleteTableContent(Tables.Names);
            Debug("fill names");
            _cmd.CommandText = "ALTER TABLE `names` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();

            int cnt = Mia.MaleNames.Rabnames.Count + Mia.FemaleNames.Rabnames.Count;
            int c = 0;
            foreach (RabName nm in Mia.MaleNames.Rabnames) {
                this.InsertName(nm, true);
                c++;
                Mia.Setpb(c, cnt);
            }
            foreach (RabName nm in Mia.FemaleNames.Rabnames) {
                this.InsertName(nm, false);
                c++;
                Mia.Setpb(c, cnt);
            }
            _cmd.CommandText = "ALTER TABLE `names` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
        }

        public void FillZones()
        {
            deleteTableContent(Tables.Zones);
            Debug("fill zones");
            List<MFString> st = Mia.ZoneList.strings;
            _cmd.CommandText = "ALTER TABLE `zones` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();

            for (int i = 0; i < st.Count / 2; i++) {
                String[] idnm = st[i * 2].value().Split(':');
                _cmd.CommandText = String.Format("INSERT INTO zones(z_id, z_name, z_short_name) VALUES({0:d}, '{1:s}', '{2:s}')", int.Parse(idnm[0]), idnm[1], st[i * 2 + 1].value());
                Mia.Setpb(i, st.Count);
                try {
                    if (int.Parse(idnm[0]) != 0) {
                        _cmd.ExecuteNonQuery();
                    }
                } catch (Exception ex) {
                    log.Error(String.Format("Error while filling Zones. ({0:s}, {1:s}){2:s}", idnm[1], st[i * 2 + 1].value(), (Environment.NewLine + ex.Message)));
                }
            }
            _cmd.CommandText = "ALTER TABLE `zones` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
        }

        public void FillBreeds()
        {
            deleteTableContent(Tables.Breeds);
            Debug("filling breeds");
            List<MFString> ls = Mia.BreedList.strings;
            _maxbreed = ls.Count / 3;
            _cmd.CommandText = "ALTER TABLE `breeds` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();

            for (int i = 0; i < _maxbreed; i++) {
                try {
                    _cmd.CommandText = String.Format("INSERT INTO breeds(b_id, b_name, b_short_name) VALUES({2:d}, '{0:s}', '{1:s}');", ls[i * 3].value(), ls[i * 3 + 1].value(), i + 1);
                    _cmd.ExecuteNonQuery();
                    ls[i * 3 + 2].tag = i + 1;// (int)c.LastInsertedId;
                    Mia.Setpb(i, _maxbreed);
                } catch (Exception exc) {
                    log.Error(String.Format("Error while insertin Breed ({0:s}, {1:s})\n\t{2:s}", ls[i * 3].value(), ls[i * 3 + 1].value(), exc.Message));
                }
            }
            _cmd.CommandText = "ALTER TABLE `breeds` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
        }

        public void FillBuildings()
        {
            Debug("fill buildings");
            int maxid = 0;
            _cmd.CommandText = "ALTER TABLE `minifarms` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();
            _cmd.CommandText = "ALTER TABLE `tiers` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();

            int cnt = Mia.Builds.Minifarms.Count;
            int c = 0;
            foreach (MiniFarm fm in Mia.Builds.Minifarms) {
                //Application.DoEvents();
                try {
                    c++;
                    int upp = saveTier(fm.Upper);
                    int low = 0;
                    if (fm.ID > maxid) {
                        maxid = fm.ID;
                    }
                    if (fm.Haslower == 1) {
                        low = saveTier(fm.Lower);
                    }
                    _cmd.CommandText = String.Format("INSERT INTO minifarms(m_id, m_upper, m_lower) VALUES({0:d}, {1:d}, {2:d});", fm.ID, (upp == 0 ? "NULL" : upp.ToString()), (low == 0 ? "NULL" : low.ToString()));
                    _cmd.ExecuteNonQuery();
                    Mia.Setpb(c, cnt);
                } catch (Exception exc) {
                    log.Error(exc.Message);
                }

            }
            _cmd.CommandText = "ALTER TABLE `tiers` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
            _cmd.CommandText = "ALTER TABLE `minifarms` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
            Debug("buildings=" + maxid.ToString() + "\r\nfill buildings tree");
            ProcTreeNode(Mia.BuildPlan.Value(), 0, 0);
        }

        public void FillZooForm()
        {
            Debug("fill ZooForm");
            DateTime dt = Mia.Zooform.zoodate.value();
            String memo = "";
            for (int i = 0; i < Mia.Zooform.strings.Count; i++) {
                memo += Mia.Zooform.strings[i].value() + "\r\n";
            }
            _cmd.CommandText = "ALTER TABLE `zooplans` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();
            _cmd.CommandText = "ALTER TABLE `zooacceptors` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();
            _cmd.CommandText = "INSERT INTO zooplans(z_date, z_memo) VALUES(" + Convdt(dt) + ",'" + MySqlHelper.EscapeString(memo) + "');";
            _cmd.ExecuteNonQuery();

            for (int i = 0; i < Mia.Zooform.donors.Count; i++) {
                //Application.DoEvents();
                Donor d = Mia.Zooform.donors[i];
                _cmd.CommandText = String.Format("INSERT INTO zooplans(z_date, z_job, z_rabbit, z_address, z_address2) VALUES({0:s}, 666, {1:d}, {2:d}, {3:d});",
                    Convdt(dt), GetUniqueRabbit((int)d.unique.value()), d.surplus.value(), d.immediate.value());
                _cmd.ExecuteNonQuery();
                int did = (int)_cmd.LastInsertedId;
                for (int j = 0; j < d.acc.Count; j++) {
                    Acceptor a = d.acc[j];
                    _cmd.CommandText = String.Format(@"INSERT INTO zooacceptors(z_id, z_rabbit, z_lack, z_hybrid, z_new_group, z_gendiff, z_distance, z_best_donor, z_best_acceptor) 
VALUES({0:d}, {1:d}, {2:d}, {3:d}, {4:d}, {5:d}, {6:d}, {7:d}, {8:d});", did, GetUniqueRabbit((int)a.unique.value()),
                        a.lack.value(), a.hybrid.value(), a.newgroup.value(), a.gendiff.value(), a.distance.value(),
                        GetUniqueRabbit((int)a.donor_best.value()), GetUniqueRabbit((int)a.acceptor_best.value()));
                    _cmd.ExecuteNonQuery();
                }
            }
            _cmd.CommandText = "ALTER TABLE `zooplans` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
            _cmd.CommandText = "ALTER TABLE `zooplans` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();

            for (int i = 0; i < Mia.Zooform.zoojobs.Count; i++) {
                //Application.DoEvents();
                ZooJob j = Mia.Zooform.zoojobs[i];
                _cmd.CommandText = String.Format(@"INSERT INTO zooplans(z_date, z_job, z_level, z_rabbit, z_notes) VALUES({0:s}, {1:d}, {2:s}, {3:d}, '{4:s}');", Convdt(dt), j.type.value() + 1, j.caption.value(),
                    GetUniqueRabbit(j.uniques[0]), ((j.subcount.value() >= 7) ? j.subs[6].value() : ""));
                _cmd.ExecuteNonQuery();
                int jid = (int)_cmd.LastInsertedId;
                for (int k = 1; k < (int)j.uniquescnt.value(); k++) {
                    String field = "z_rabbit2";
                    if (k == 2) {
                        field = "z_address";
                    }
                    if (k == 3) {
                        field = "z_address2";
                    }
                    _cmd.CommandText = String.Format("UPDATE zooplans SET {1:s} = {2:d} WHERE z_id = {0:d};", jid, field, j.uniques[k]);
                    _cmd.ExecuteNonQuery();
                }
            }
            _cmd.CommandText = "ALTER TABLE `zooplans` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
            _cmd.CommandText = "ALTER TABLE `zooacceptors` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
        }

        public void FillDeadFromLost()
        {
            //deleteTableContent(Tables.DeadReasons);
            Debug("filling Dead Rabbits From Lost List");

            fillDeadReasons();

            int cnt = (int)Mia.Graphform.lost.size.value();
            _cmd.CommandText = "ALTER TABLE `rabbits` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();

            for (int i = 0; i < Mia.Graphform.lost.size.value(); i++) {
                Mia.Setpb(i, cnt);
                MFListItem li = Mia.Graphform.lost.items[i];
                String sex = "void";
                if (li.subs[2].value() == "м") {
                    sex = "male";
                }
                if (li.subs[2].value() == "ж") {
                    sex = "female";
                }
                int weight = 0;
                if (!int.TryParse(li.subs[5].value(), out weight)) {
                    weight = 0;
                }
                String ddt = li.caption.value();
                String[] names = li.subs[0].value().Split(' ');
                String address = li.subs[1].value();
                String stat = li.subs[3].value();
                int age = int.Parse(li.subs[4].value());
                int group = 1;
                String notes = "";
                if (li.subitems.value() >= 9) {
                    notes = li.subs[8].value();
                }
                int reason = GetReason(li.subs[6].value());
                if (names[names.Length - 1].Length > 0)
                    if (names[names.Length - 1].Trim()[0] == '[') {
                        String grp = names[names.Length - 1].Trim();
                        grp = grp.Replace("[", "").Replace("]", "");
                        group = int.Parse(grp);
                        names[names.Length - 1] = "";
                    }
                String nm = names[0];
                String xx = "";
                uint nid = Findname(nm, ref xx);
                if (nid != 0) {
                    nm = "";
                    if (names.Length > 1) {
                        nm = names[1];
                    }
                }

                uint suid = 0, seid = 0;
                String[] nms = nm.Split('-');
                if (nms.Length > 0) {
                    if (nms[0].Trim() != "") {
                        suid = FindSurname(nms[0].Trim(), sex, group, 2);
                    }
                }
                if (nms.Length > 1 && nms[1].Trim() != "") {
                    seid = FindSurname(nms[1].Trim(), sex, group, 1);
                }

                int farm = 0;
                int tier = 0;
                int tierID = 0;
                int area = 0;
                string sa = "";
                int j = 0;
                while (j < address.Length && address[j] >= '0' && address[j] <= '9') {
                    sa += address[j];
                    j++;
                }
                if (sa != "") {
                    farm = int.Parse(sa);
                    if (j < address.Length) {
                        switch (address[j]) {
                            case '^': tierID = 1; j++; break;
                            case '-': tierID = 2; j++; break;
                            case 'б': area = 1; break;
                            case 'в': area = 2; break;
                            case 'г': area = 3; break;
                        }
                    }
                }
                _cmd.CommandText = String.Format(@"INSERT INTO rabbits(r_sex, r_name, r_surname, r_secname, r_notes, r_group, r_born, r_farm, r_tier_id, r_tier, r_area, r_breed) 
VALUES('{0:s}', {1:d}, {2:d}, {3:d}, '{4:s}', {5:d}, {6:s}-INTERVAL {7:d} DAY, {8:d}, {9:d}, {10:d}, {11:d}, 1);",
                                  sex, nid, suid, seid, notes, group, Convdt(ddt), age, farm, tierID, tier, area);
                _cmd.ExecuteNonQuery();
                long lid = _cmd.LastInsertedId;
                _cmd.CommandText = String.Format("CALL killRabbitDate({0:d},1,'',{1:s});", lid, Convdt(ddt));
                _cmd.ExecuteNonQuery();
            }
            _cmd.CommandText = "ALTER TABLE `rabbits` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
        }

        private void fillDeadReasons()
        {
            _cmd.CommandText = "ALTER TABLE `deadreasons` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();

            for (int i = 0; i < Mia.Graphform.reasons.size.value(); i++) {
                //Application.DoEvents();
                MFListItem li = Mia.Graphform.reasons.items[i];
                _cmd.CommandText = String.Format("INSERT INTO deadreasons(d_name,d_rate) VALUES('{0:s}', {1:s});", li.caption.value(), li.subs[0].value());
                _cmd.ExecuteNonQuery();
            }
            _cmd.CommandText = "ALTER TABLE `deadreasons` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();
        }


        public void FillJobs(MFStringList arc, DateTime date)
        {
            string adr = arc.strings[2].value();
            string nm = arc.strings[3].value();
            int jid = Jobid(arc.strings[1].value());
            if (jid == 0) return;
            MySqlCommand cmd = new MySqlCommand(@"SELECT r_id FROM rabbits,names WHERE r_name=n_id AND n_name='" + nm + "';", _sql);
            int r = 0;
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read()) {
                r = rd.GetInt32(0);
            }
            rd.Close();

            if (r == 0) {
                cmd.CommandText = "SELECT r_id FROM dead, names WHERE r_name=n_id AND n_name='" + nm + "';";
            }
            rd = cmd.ExecuteReader();
            if (rd.Read()) {
                r = rd.GetInt32(0);
            }
            rd.Close();

            cmd.CommandText = String.Format(@"INSERT INTO logs(l_date, l_type, l_user, l_rabbit, l_address) VALUES({0:s}, {1:d}, 0, {2:d}, '{3:s}');", Convdt(date), jid, r, adr);
            cmd.ExecuteNonQuery();
        }

        public void FillArcForm()
        {
            Debug("fill ArcForm");
            _cmd.CommandText = "ALTER TABLE `logs` DISABLE KEYS;";
            _cmd.ExecuteNonQuery();

            int cnt = Mia.Arcform.plans.Count;
            int c = 0;
            foreach (ArcPlan p in Mia.Arcform.plans) {
                c++;
                Mia.Setpb(c, cnt);
                DateTime dt = p.date.value();
                foreach (MFStringList sl in p.works) {
                    this.FillJobs(sl, dt);
                }
            }
            _cmd.CommandText = "ALTER TABLE `logs` ENABLE KEYS;";
            _cmd.ExecuteNonQuery();            
        }

        #endregion fill

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nm"></param>
        /// <param name="sex"></param>
        public void InsertName(RabName nm, bool sex)
        {
            String xdt = "NULL";
            if (nm.Key.value() <= 365 && nm.Key.value() > 0) {
                xdt = String.Format("DATE(NOW()) + INTERVAL {0:d} DAY", nm.Key.value());
            }
            String use = "NULL";
            if (nm.Key.value() > 365) {
                use = String.Format("{0:d}", nm.Key.value());
            }
            _cmd.CommandText = String.Format("INSERT INTO names(n_sex, n_name, n_surname, n_use, n_block_date) VALUES('{0:s}', '{1:s}', '{2:s}', {3:s}, {4:s});",
                (sex ? "male" : "female"), nm.Name.value(), nm.Surname.value(), use, xdt);

            try {
                _cmd.ExecuteNonQuery();
                nm.Key.tag = (int)_cmd.LastInsertedId;
            } catch (Exception ex) {
                log.Error("MYSQL Exception on name " + nm.Name.value() + "(" + nm.Key.value().ToString() + "): " + ex.Message);
                _cmd.CommandText = "SELECT n_id FROM names WHERE n_name='" + nm.Name.value() + "';";
                MySqlDataReader rd = _cmd.ExecuteReader();
                rd.Read();
                nm.Key.tag = (int)rd.GetInt32(0);
                rd.Close();
            }
        }

        private int saveTier(Tier tr)
        {
            //Application.DoEvents();
            String tp = "unk";
            String b1 = "NULL", b2 = "NULL", b3 = "NULL", b4 = "NULL";
            String heater = "00";
            String nest = "00";
            String delims = "000";
            String d = "D1";
            switch (tr.Type) {
                case 0: tp = "none";
                    break;
                case 1: tp = "female";
                    b1 = tierBusyWrap(tr, 0);//"'" + tr.Busies[0].ToString() + "'";
                    heater = tr.Heaters[0].ToString(d);
                    nest = tr.Nests[0].ToString(d);
                    break;
                case 2: tp = "dfemale";
                    b1 = tierBusyWrap(tr, 0);//"'" + tr.Busies[0].ToString() + "'";
                    b2 = tierBusyWrap(tr, 1);//"'" + tr.Busies[1].ToString() + "'";
                    heater = tr.Heaters[0].ToString(d) + tr.Heaters[1].ToString(d);
                    nest = tr.Nests[0].ToString(d) + tr.Nests[1].ToString(d);
                    break;
                case 3: tp = "complex";
                    b1 = tierBusyWrap(tr, 0);//"'" + tr.Busies[0].ToString() + "'";
                    b2 = tierBusyWrap(tr, 1);//"'" + tr.Busies[1].ToString() + "'";
                    b3 = tierBusyWrap(tr, 2);//"'" + tr.Busies[2].ToString() + "'";
                    heater = tr.Heaters[0].ToString(d);
                    nest = tr.Nests[0].ToString(d);
                    break;
                case 4: tp = "jurta";
                    b1 = tierBusyWrap(tr, 0);//"'" + tr.Busies[0].ToString() + "'";
                    b2 = tierBusyWrap(tr, 1);//"'" + tr.Busies[1].ToString() + "'";
                    heater = tr.Heaters[0].ToString(d);
                    nest = tr.Nests[0].ToString(d);
                    delims = tr.NestWbig.ToString(d);
                    break;
                case 5: tp = "quarta";
                    delims = tr.Delims[0].ToString(d) + tr.Delims[1].ToString(d) + tr.Delims[2].ToString(d);
                    b1 = tierBusyWrap(tr, 0);//"'" + tr.Busies[0].ToString() + "'";
                    if (delims[0] == '1')
                        b2 = tierBusyWrap(tr, 1);//"'" + tr.Busies[1].ToString() + "'";
                    if (delims[1] == '1')
                        b3 = tierBusyWrap(tr, 2);//"'" + tr.Busies[2].ToString() + "'";
                    if (delims[2] == '1')
                        b4 = tierBusyWrap(tr, 3);//"'" + tr.Busies[3].ToString() + "'";                    
                    break;
                case 6: tp = "vertep";
                    b1 = tierBusyWrap(tr, 0);//"'" + tr.Busies[0].ToString() + "'";
                    b2 = tierBusyWrap(tr, 1);//"'" + tr.Busies[1].ToString() + "'";
                    break;
                case 7: tp = "barin";
                    b1 = tierBusyWrap(tr, 0);//"'" + tr.Busies[0].ToString() + "'";
                    b2 = tierBusyWrap(tr, 1);//"'" + tr.Busies[1].ToString() + "'";
                    delims = tr.Delims[0].ToString(d);
                    break;
                case 8: tp = "cabin";
                    b1 = tierBusyWrap(tr, 0);//"'" + tr.Busies[0].ToString() + "'";
                    b2 = tierBusyWrap(tr, 1);//"'" + tr.Busies[1].ToString() + "'";
                    heater = tr.Heaters[0].ToString(d);
                    nest = tr.Nests[0].ToString(d);
                    break;
            }
            _cmd.CommandText = String.Format(@"INSERT INTO tiers(t_type, t_repair, t_notes, t_heater, t_nest, t_delims, t_busy1, t_busy2, t_busy3, t_busy4) 
VALUES('{0:s}', {1:d}, '{2:s}', '{3:s}', '{4:s}', '{5:s}', {6:s}, {7:s}, {8:s}, {9:s});", //{3:d},{4:d},{5:d},{6:d},
                tp, tr.Repair, tr.Notes.value(), heater, nest, delims, b1, b2, b3, b4); //b1,b2,b3,b4,
            _cmd.ExecuteNonQuery();
            return (int)_cmd.LastInsertedId;
        }

        public void ProcTreeNode(TreeNode nd, int level, int parent)
        {
            for (int i = 0; i < nd.Nodes.Count; i++) {
                String nm = nd.Nodes[i].Text;
                int fid = 0;
                if (int.TryParse(nm, out fid)) {
                    nm = "№" + fid.ToString();
                }
                _cmd.CommandText = String.Format("INSERT INTO buildings(b_name, b_parent, b_level, b_farm) VALUES('{0:s}', {1:d}, {2:d}, {3:d});", nm, parent, level, (fid > 0 ? fid.ToString() : "NULL"));
                _cmd.ExecuteNonQuery();
                int tpar = (int)_cmd.LastInsertedId;
                ProcTreeNode(nd.Nodes[i], level + 1, tpar);
            }
        }

        public String Convdt(String dt)
        {
            String[] dmy = dt.Split('.');
            if (dmy[2] == "1899" && dmy[1] == "12" && dmy[0] == "30") {
                return "NULL";
            }
            return String.Format("'{0:S4}-{1:S2}-{2:S2}'", dmy[2], dmy[1], dmy[0]);
        }

        public String Convdt(DateTime dt)
        {
            return Convdt(String.Format("{0:D2}.{1:D2}.{2:D2}", dt.Day, dt.Month, dt.Year));
        }

        public uint Findname(String name, ref String addNm)
        {
            if (name == "") {
                return 0;
            }
            _cmd.CommandText = "SELECT n_id FROM names WHERE n_name='" + name + "';";
            MySqlDataReader rd = _cmd.ExecuteReader();
            if (!rd.HasRows) {
                rd.Close();
                addNm = name;
                return 0;
            }
            rd.Read();
            uint res = rd.GetUInt32(0);
            rd.Close();
            return res;
        }

        public uint FindSurname(string sur, String sex, int cnt, int tp)
        {
            if (sur == "") {
                return 0;
            }
            if (cnt > 1) {
                sur = sur.TrimEnd('ы');
            }
            if (cnt == 1 && sex == "female") {
                sur = sur.TrimEnd('а');
            }
            String sx = "male";
            if (tp == 2) {
                sx = "female";
            }
            _cmd.CommandText = "SELECT n_id FROM names WHERE n_surname='" + sur + "' AND n_sex='" + sx + "';";
            MySqlDataReader rd = _cmd.ExecuteReader();
            if (!rd.Read()) {
                rd.Close();
                return 0;
            }
            uint res = rd.GetUInt32(0);
            rd.Close();
            return res;
        }

        public int FindBreed(int breed)
        {
            if (breed > _maxbreed) {
                breed = 0;
            }
            return breed + 1;
            /* 
            List<MFString> ls = mia.breed_list.strings;
            for (int i = 0; i < ls.Count / 3; i++)
                if (int.Parse(ls[i * 3 + 2].value()) == breed)
                    return i+1;
            return 1;
             * */
        }

        public void SetOption(String name, String subname, String value)
        {
            _cmd.CommandText = "UPDATE options SET o_value='" + value + "' WHERE o_name='" + name + "' AND o_subname='" + subname + "';";
            _cmd.ExecuteNonQuery();
        }

        public void SetOption(String name, String subname, int value)
        {
            SetOption(name, subname, value.ToString());
        }

        public void SetOption(String name, String subname, float value)
        {
            SetOption(name, subname, value.ToString());
        }

        public int GetUniqueRabbit(int unique)
        {
            if (unique == 0) {
                return 0;
            }

            foreach (Rabbit r in Mia.Rabbits.rabbits) {
                if ((int)r.unique.value() == unique) {
                    return r.notes.tag;
                }
            }
            return 0;
        }

        public int GetReason(String name)
        {
            if (name == "") {
                return 0;
            }
            _cmd.CommandText = "SELECT d_id FROM deadreasons WHERE d_name='" + name + "'";
            MySqlDataReader rd = _cmd.ExecuteReader();
            if (!rd.HasRows) {
                rd.Close();
                return 0;
            }
            rd.Read();
            int res = rd.GetInt32(0);
            rd.Close();
            return res;
        }

        public int Jobid(string name)
        {
            switch (name.ToLower()) {
                case "вселить":
                case "всел": return 1;
                case "случить":
                case "сл": return 5;
                case "вязать":
                case "вязк": return 5;
                case "кук": return 0;
                case "устан. гнезда":
                case "гнзд": return 9;
                case "уст. гнезда.грелки":
                case "гнгр": return 9;
                case "отсадка девочек":
                case "отде": return 2;
                case "включить грелку":
                case "вкгр": return 12;
                case "предокросмотр":
                case "прок": return 21;
                case "принять окрол":
                case "окрл": return 6;
                case "подсчет гнездовых":
                case "подсчёт гнездовых":
                case "счгн": return 17;
                case "подсчет подсосных":
                case "подсчёт подсосных":
                case "счпс": return 17;
                case "выдворение":
                case "выдв": return 10;
                case "отсадка мальчиков":
                case "отма": return 2;
                case "рассел. мальчиков":
                case "рсма": return 2;
                case "чистка гнезда":
                case "чигн": return 0;
                case "пересадка крольчат":
                case "пекр": return 2;
                case "рассадка":
                case "расс": return 2;
            }
            return 0;
        }

        private string tierBusyWrap(Tier tr, int bInd)
        {
            if (bInd > tr.Busies.Length) {
                throw new Exception("incorrect tierBusyWrap index");
            }
            return String.Format("'{0:d}'", tr.Busies[bInd]);
        }

    }
}
