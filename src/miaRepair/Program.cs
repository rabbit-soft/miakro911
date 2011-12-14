#if DEBUG
    #define NOCATCH
    #define NOBREAK
#endif
    
using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using MySql.Data.MySqlClient;
using System.IO;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace miaRepair
{
    class Program
    {
        private const string LOGFILE = "miaRepair.log";

        private static ILog _logger = LogManager.GetLogger("miaRepair");

        static string _host = "localhost";
        static string _database = "kroliki";
        static string _uid = "kroliki";
        static string _pwd = "krol";
        static bool _yestoall = false;

        private static MySqlConnection _sql;
        private static MySqlCommand _cmd;
        private static RabbitList _rabbits = new RabbitList();
        private static DeadList _deads = new DeadList();
        private static NameList _names = new NameList();
        private static FuckList _fucks = new FuckList();
        private static TiersList _tiers = new TiersList();
        

        static void Main(string[] args)
        {
            if(!setOptions(args))return;

            if(File.Exists(LOGFILE))
                File.Delete(LOGFILE);
            try
            {
                _sql = new MySqlConnection(conString());
                _sql.Open();
            }
            catch (Exception exc)
            {
                log(exc.Message);
                return;
            }
#if !NOCATCH
            try
            { 
#endif
            
            _cmd = new MySqlCommand("", _sql);
            _rabbits.LoadContent(_cmd);
            _names.LoadNames(_cmd);
            _deads.LoadContent(_cmd);
            _fucks.LoadFucks(_cmd);
            _tiers.LoadTiers(_cmd);
            repairNames();
            //searchYoungerMother_bySurname();
            //searchYoungerFathers_bySecname();
            repairMotherAndFather_bySurnameAndSecname();
            repairFucksEndDate_byYoungers();             
            repairFucksStartDate_byRabEventDate();
            repairFucksIfChildrenThenOkrol();
            repairTiers_InAreaUndefinedRabbit();
                 
            while(true)
            {
                string ans;
                if (!_yestoall)
                {
                    log("Do You Want Save Changes in DataBase? [y/n]");
                    ans = Console.ReadLine().ToLower();
                }
                else ans = "y";

                if(ans == "y")
                {
                    saveRabbits();
                    saveFucks();
                    saveTiers();
                    break;
                }
                else if(ans !="n")
                    log("   Are You Idiot? You must type 'y' or 'n'. Lets Ask Again.");
                else break;
            }

            _sql.Close();
#if !NOCATCH
            }
            catch (Exception ex)
            {
                _sql.Close();
                _logger.ErrorFormat("Exception: {0}",ex.Message);
                log("---Произошла ошибка при востановлении связей.");
                log("---Это не очень критичная ошибка");
                log("---Но все таки советуем обратится к разработчику");
            }
#endif
        }

        private static string conString()
        {
            return string.Format("host={0:s};database={1:s};uid={2:s};pwd={3:d};charset=utf8",
                _host,_database,_uid,_pwd);
        }       

        /// <summary>
        /// Находит кроликам родителям по surname и secname
        /// </summary>
        private static void repairMotherAndFather_bySurnameAndSecname()
        {
            log("----- searching Mother And Father by Surname and Secname -----");
            bool needAccept = !_yestoall;          
            foreach(repRabbit rab in _rabbits)
            {
                if ((rab.Mother!=0 && rab.Father!=0) || (rab.SecnameID == 0 && rab.SurnameID == 0)) continue;
                repRabbit mbMother=null, mbFather=null;
                log("searching mother for: {0:d} | name:  {1:s}",rab.rID,rab.Name);
                if (rab.Mother==0 && rab.SurnameID != 0)
                {
                    mbMother = _rabbits.GetRabbitByID(_names.GetSurnameUse(rab.SurnameID));
                    if (mbMother == null)//если нет живого кандидата на мать, то ищем в мертвых
                    {
                        log("   we not find aliveMother, now searching in dead");
                        List<repRabbit> candidates = new List<repRabbit>();
                        foreach (repRabbit ded in _deads)
                        {
                            if (ded.Sex == Sex.Female && ded.NameId == rab.SurnameID && ded.Born < rab.Born)
                                candidates.Add(ded);
                        }
                        if (candidates.Count == 1)
                        {
                            //log("   we find only one dead Mother: {0:d}|name [{1:s}]  and we write he in Mother", candidates[0].rID,candidates[0].Name);
                            mbMother = candidates[0];
                        }
                        else if (candidates.Count > 1)
                        {

                            foreach (repRabbit c in candidates)
                            {
                                if (mbMother == null)
                                    mbMother = c;
                                if (mbMother.rID < c.rID)
                                    mbMother = c;
                            }

                            /*log("   we find a {0:d} motherCandidate", candidates.Count);
                            log("   #       name");
                            foreach (Rabbit c in candidates)
                            {
                                log("   {0:d}        {1:d}     ", c.rID, c.Name);
                            }
                            while (true)//_deadMotherNames
                            {
                                log("you nead a type '№ of candMother' or type 'n' to next");
                                string ans = Console.ReadLine();
                                if (ans == "n")
                                {
                                    log("   you chose nobody");
                                    break;
                                }
                                int n = 0;
                                int.TryParse(ans, out n);
                                bool exitloop = false;
                                foreach (Rabbit c in candidates)
                                {
                                    if (c.rID == n)
                                    {
                                        mbMother = c;
                                        exitloop = true;
                                        break;
                                    }
                                }
                                if (exitloop) break;
                                log("   you type a wrong symbol, Idiot!");
                            }*/

                        }
                    }// if (mbMother == 0)
                }

                if (rab.Father==0 && rab.SecnameID != 0)
                {
                    mbFather = _deads.GetRabbitByID(_names.GetSecnameUse(rab.SecnameID));
                    if (mbFather == null)//если нет живого кандидата на отца, то ищем в мертвых
                    {
                        log("   we not find aliveFather, now searching in dead");
                        List<repRabbit> candidates = new List<repRabbit>();
                        foreach (repRabbit ded in _deads)
                        {
                            if (ded.Sex == Sex.Male && ded.NameId == rab.SecnameID && ded.Born < rab.Born)
                                candidates.Add(ded);
                        }
                        if (candidates.Count == 1)
                        {
                            mbFather = candidates[0];
                        }
                        else if (candidates.Count > 1)
                        {
                            foreach (repRabbit c in candidates)
                            {
                                if (mbFather == null)
                                    mbFather = c;
                                if (mbFather.rID < c.rID)
                                    mbFather = c;
                            }                         
                        }
                    }//if(mbFather==0)
                }
                log("   here what we find:");
                log("                     mother: {0:s}", mbMother == null ? "-" : mbMother.Name);
                log("                     father: {0:s}", mbFather == null ? "-" : mbFather.Name);

                while (true)
                {
                    string ans = "y";
                    if (needAccept) //если надо справшивать о каждом принятии
                    {
                        log("   do you accept this Parents? [y/n], to cancel ask type 'q' ");
                        ans = Console.ReadLine();
                        if (ans == "q")
                        {
                            needAccept = false;
                            ans = "y";
                        }
                    }
                    if (ans == "n")
                        break;
                    else if (ans == "y")
                    {
                        rab.Mother = mbMother == null ? 0 : mbMother.rID;
                        rab.Father = mbFather == null ? 0 : mbFather.rID;
                        break;
                    }
                    else log("   you type a wrong symbol, Idiot!");
                }

            }
        }

        private static void repairNames()
        {
            log("----- repair names -----");
            foreach (repRabbit rabbit in _rabbits)
            {
                if (rabbit.NameId == 0) continue;
                foreach (repName name in _names)
                {
                    if (rabbit.NameId == name.nID)
                    {
                        if (rabbit.rID == name.useRabbit)
                        {
                            //log("   OK.name:{0:d} is fine", name.nID);
                            break;
                        }
                        else
                        {
                            log("   replacing current n_use: {0:d} by {0:d}",name.useRabbit,rabbit.rID);
                            name.useRabbit = rabbit.rID;
                        }
                    }
                }
            }
        }

        private static void searchYoungerMother_bySurname()
        {
            log("----- repair mother of youngers -----");
            //List<Rabbit> yongers = _rabbits.Yongers;
            log("youngers count: {0:d}", _rabbits.YongersCount);
            foreach (repRabbit yng in _rabbits.Yongers)
            {
                if (yng.Mother != 0) continue;
                log("searching mother for: {0:d}", yng.rID);
                bool next = false;
                foreach (repRabbit rab in _rabbits)
                {
                    if (rab.Sex == Sex.Female && rab.rID == yng.ParentID && rab.Born < yng.Born)
                    {
                        if (rab.NameId == yng.SurnameID)
                        {
                            log("   OK. We Find a mother: {0:d}", rab.rID);
                            yng.Mother = rab.rID;
                            next = true;
                            break;
                        }
                    }
                }
                if (next) continue;
                log("   we find nothing");
            }
        }

        private static void searchYoungerFathers_bySecname()
        {
            log("----- search father of youngers BY SURNAME -----");
            List<repRabbit> yongers = _rabbits.Yongers;
            foreach (repRabbit yng in yongers)
            {
                if (yng.Father != 0) continue;
                log("searching father for: {0:d}", yng.rID);
                bool next = false;
                foreach (repRabbit rab in _rabbits)
                {
                    if (rab.Sex == Sex.Male && rab.NameId == yng.SecnameID && rab.Born < yng.Born)
                    {
                        log("   OK. We Find a father: {0:d}", rab.rID);
                        yng.Father = rab.rID;
                        next = true;
                        break;
                    }
                }
                if (next) continue;
                log("   we find nothing");
            }
        }

        /// <summary>
        /// Выбирает из кроликов самок, у которых "r_event_date is not null".
        /// Находит в таблице fucks записи по этой крольчихи где "f_state='sukrol' AND f_date is null"
        /// Ставит дату из r_event_date в f_date
        /// </summary>
        private static void repairFucksStartDate_byRabEventDate()
        {
            log("----- rapair fucks Start_Date by Mother event_date -----");
            foreach (repRabbit rab in _rabbits.SukrolMothers)
            {
                log("searching fuck mother: {0:d}",rab.rID);
                List<repFuck> candidates = new List<repFuck>();

                int maxCand = 0;

                foreach (repFuck f in _fucks)
                {
                    if (f.SheID == rab.rID && f.StartDate == DateTime.MinValue && f.FuckState == repFuck.State.Sukrol)
                    {
                        candidates.Add(f);

                        if (f.fID > maxCand)
                            maxCand = f.fID;
                    }
                }
                if (candidates.Count == 0)
                {
                    log("   we find nothing");
                    continue;
                }
                else if (candidates.Count == 1)
                {
                    log("   we find only one fuck: {0:d}  and we write it in start_date", candidates[0].fID);
                    candidates[0].StartDate = rab.EventDate;
                    candidates[0].Children = 0;
                    continue;
                }
                else if (candidates.Count > 1)
                {
                    log("   we find a {0:d} fucks", candidates.Count);
                    foreach (repFuck f in candidates)
                    {
                        if (f.fID == maxCand)
                        {
                            f.StartDate = rab.EventDate;
                            f.Children = 0;
                            break;
                        }
                        else
                        {

                            f.FuckState = repFuck.State.Proholost;
                            f.Children = 0;
                        }
                    }
                }
            }
        }   
    
        /// <summary>
        /// Устанавливает в таблице fucks дату в поле f_end_date, крольчихе.
        /// Строчка находится по Матери и Отчу
        /// </summary>
        private static void repairFucksEndDate_byYoungers()
        {
            log("--rapair fucks End_Date by Father and Mother of Younger--");
            List<repRabbit> youngers = _rabbits.Yongers;
            foreach (repRabbit yng in youngers)
            {
                if (yng.Mother == 0 || yng.Father == 0) continue;
                log("searching fuck where m:{0:d}|f:{1:d}",yng.Mother,yng.Father);
                ///заполняем массив факов где f_rabid и f_parther совпадают с r_mother и r_father 
                int maxCand =0;
                List<repFuck> candidates = new List<repFuck>();
                foreach(repFuck f in _fucks)
                {                  
                    if(f.EndDate != DateTime.MinValue) continue;
                    if(yng.Mother == f.SheID && yng.Father == f.HeID)
                    {
                        candidates.Add(f);
                        if(f.fID > maxCand)
                            maxCand= f.fID;
                    }
                }
                ///разбираем кандидатов
                if (candidates.Count == 0)
                {
                    log("   we find nothing");
                    continue;
                }
                else if (candidates.Count == 1)
                {
                    log("   we find only one fuck: {0:d}  and we write it in end_date", candidates[0].fID);
                    candidates[0].EndDate = yng.Born;
                    candidates[0].FuckState = repFuck.State.Okrol;
                    continue;
                }
                else if (candidates.Count > 1)
                {
                    log("   we find a {0:d} fucks",candidates.Count);
                    foreach (repFuck f in candidates)
                    {
                        if (f.fID == maxCand)
                        {
                            log("fuck: {0:d} we write it in end_date", f.fID);
                            f.EndDate = yng.Born;
                            f.FuckState = repFuck.State.Okrol;
                            break;
                        }
                    }
                }//if cand.count>0
            }
        }

        /// <summary>
        /// UPDATE fucks SET f_state='okrol' WHERE f_state='sucrol' AND f_children>0;
        /// </summary>
        private static void repairFucksIfChildrenThenOkrol()
        {
            log("----- searching fucks where state is 'sukrol'  and childrens<>0 -----");
            foreach (repFuck f in _fucks)
            {
                if (f.FuckState == repFuck.State.Sukrol && f.Children != 0)
                {
                    log("   fixing a bug. chldrn: {0:d}| state: {1:s}",f.Children,f.FuckState.ToString());
                    f.FuckState = repFuck.State.Okrol;
                }
            }
        }

        private static void repairTiers_InAreaUndefinedRabbit()
        {
            log("----- searching undefined rabbit in tiers -----");
            bool b1, b2, b3, b4;
            foreach (repTier tier in _tiers)
            {
                b1 = tier.Busy1 == 0;
                b2 = tier.Busy2 == 0;
                b3 = tier.Busy3 == 0;
                b4 = tier.Busy4 == 0;

                foreach (repRabbit rab in _rabbits)
                {
                    if (rab.rID == tier.Busy1)
                        b1 = true;
                    if (rab.rID == tier.Busy2)
                        b2 = true;
                    if (rab.rID == tier.Busy3)
                        b3 = true;
                    if (rab.rID == tier.Busy4)
                        b4 = true;
                }
                if (!b1 && tier.Busy1 != -1)
                {
                    log("  in tier [{0:d}] undefined rabbit [{1:d}] in area 1", tier.tID, tier.Busy1);
                    tier.Busy1 = 0;
                }
                if (!b2 && tier.Busy2 != -1)
                {
                    log("  in tier {0:d} undefined rabbit {1:d} in area 2", tier.tID, tier.Busy2);
                    tier.Busy2 = 0;
                }
                if (!b3 && tier.Busy3 != -1)
                {
                    log("  in tier [{0:d}] undefined rabbit [{1:d}] in area 3", tier.tID, tier.Busy3);
                    tier.Busy3 = 0;
                }
                if (!b4 && tier.Busy4 != -1)
                {
                    log("  in tier [{0:d}] undefined rabbit [{1:d}] in area 4", tier.tID, tier.Busy4);
                    tier.Busy4 = 0;
                }
            }
        }
        private static void saveRabbits()
        {
            log("----- saving rabbits -----");
            foreach (repRabbit rab in _rabbits)
            {
                _cmd.CommandText = String.Format("UPDATE rabbits SET r_mother={0:d},r_father={1:d} WHERE r_id={2:d};",rab.Mother,rab.Father,rab.rID);
                _cmd.ExecuteNonQuery();
            }
        }

        private static void saveFucks()
        {
            log("----- saving fucks -----");
            foreach (repFuck f in _fucks)
            {
                if (!f.Modified) continue;
                _cmd.CommandText = String.Format("UPDATE fucks SET {0:s} {1:s} f_state='{2}',f_children={4:d} WHERE f_id={3:d};",
                    f.StartDate == DateTime.MinValue ?"":String.Format("f_date='{0:yyy-MM-dd}',",f.StartDate),
                    f.EndDate == DateTime.MinValue ? "" : String.Format("f_end_date='{0:yyy-MM-dd}',", f.EndDate),
                    f.FuckState.ToString().ToLower(),f.fID,f.Children);
                _cmd.ExecuteNonQuery();
            }
        }

        private static void saveTiers()
        {
            log("----- saving tiers -----");
            string set = "";
            foreach (repTier t in _tiers)
            {
                if (!t.Modified) continue;
                set =  (t.Busy1 != -1 ? "t_busy1="+t.Busy1.ToString():"") +
                       (t.Busy2 != -1 ? ",t_busy2="+t.Busy2.ToString():"") +
                       (t.Busy3 != -1 ? ",t_busy3="+t.Busy3.ToString():"") +
                       (t.Busy4 != -1 ? ",t_busy4="+t.Busy4.ToString():"") ;
                set.Trim(new char[] { ',' });
                _cmd.CommandText = String.Format("UPDATE tiers SET {0:s} WHERE t_id={1:d};", set,t.tID);
                _cmd.ExecuteNonQuery();
            }
        }

        #region loging
        internal static void log(string s)
        {
            Console.WriteLine(s);
            _logger.Info(s);
        }

        internal static void log(string s, object arg0)
        {
            log(String.Format(s, arg0));
        }

        internal static void log(string s, object arg0, object arg1)
        {
            log(String.Format(s, arg0,arg1));
        }

        internal static void log(string s, object arg0, object arg1, object arg2)
        {
            log(String.Format(s, arg0, arg1,arg2));
        }

        internal static void log(string s, object arg0, object arg1, object arg2, object arg3)
        {
            log(String.Format(s, arg0, arg1,arg2,arg3));
        }

        internal static void log(string s, object arg0, object arg1, object arg2, object arg3, object arg4,object arg5)
        {
            log(String.Format(s, arg0, arg1, arg2, arg3,arg4,arg5));
        }
        #endregion     

        private static void giveABreak()
        {
#if !NOBREAK
            log("--lets take a 10sec break--");
            System.Threading.Thread.Sleep(10000);
#endif
        }

        /// <summary>
        /// Обрабатывает полученные инструкиции от пользователя
        /// </summary>
        /// <param name="args"></param>
        /// <returns>resume</returns>
        private static bool setOptions(string[] args)
        {
            if (args.Length == 0) return true;
            if (args[0] == "-?" || args[0] == "help")
            {
                Console.WriteLine("-? - this help");
                Console.WriteLine("--- connection to database options ---");
                Console.WriteLine("-h [host] - set host");
                Console.WriteLine("-d [database] - set database");
                Console.WriteLine("-u [user] - set user");
                Console.WriteLine("-p [password] - set user password");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("-y - YES to all questions");
                return false;
            }
            int i = 0;
            while (i < args.Length)
            {
                if (args[i].StartsWith("-"))
                {
                    if (args[i] == "-h")
                        _host = args[++i];
                    else if (args[i] == "-d")
                        _database = args[++i];
                    else if (args[i] == "-u")
                        _uid = args[++i];
                    else if (args[i] == "-p")
                        _pwd = args[++i];
                    else if (args[i] == "-y")
                        _yestoall = true;
                }
                i++;
            }
            return true;
        }
    }

    enum Sex{Male,Female,Void}  
}
