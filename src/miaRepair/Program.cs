#if DEBUG
    #define NOCATCH
    #define NOBREAK
    //#define ASK
#endif
    
using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private static MySqlConnection _sql;
        private static MySqlCommand _cmd;
        private static RabbitList _rabbits = new RabbitList();
        private static DeadList _deads = new DeadList();
        private static NameList _names = new NameList();
        private static FuckList _fucks = new FuckList();
        

        static void Main(string[] args)
        {
            if(File.Exists(LOGFILE))
                File.Delete(LOGFILE);
            string conString = "host=localhost;database=kroliki;uid=kroliki;pwd=krol;charset=utf8";
            if (args.Length > 0)
                conString = args[0];
            _sql = new MySqlConnection(conString);
#if !NOCATCH
            try
            { 
#endif
                _sql.Open();
                _cmd = new MySqlCommand("", _sql);
                _rabbits.LoadContent(_cmd);
                _names.LoadNames(_cmd);
                _deads.LoadContent(_cmd);
                _fucks.LoadFucks(_cmd);
                repairNames();
                searchYoungerMother_bySurname();
                searchYoungerFathers_bySecname();
                repairMotherAndFather_bySurnameAndSecname();
                repairFucksEndDate_byYoungers();             
                repairFucksStartDate_byRabEventDate();
                repairFucksIfChildrenThenOkrol();
                
         
                while(true)
                {
                    log("Do You Want Save This Shit? [y/n]");  
                    string ans = Console.ReadLine().ToLower();
                    if(ans == "y")
                    {
                        saveRabbits();
                        saveFucks();
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
                log("Exception: {0}",ex.Message);
            }
#endif
        }

        private static void repairMotherAndFather_bySurnameAndSecname()
        {
            log("searching Mother And Father by Surname and Secname");
            foreach(Rabbit rab in _rabbits.Adult)
            {
                if (rab.SecnameID == 0 && rab.SurnameID == 0) continue;
                Rabbit mbMother=null, mbFather=null;
                log("searching mother for: {0:d} | name:            {1:s}",rab.rID,rab.Name);
                if (rab.SurnameID != 0)
                {
                    mbMother = _rabbits.GetRabbitByID(_names.GetSurnameUse(rab.SurnameID));
                    if (mbMother == null)//если нет живого кандидата на мать, то ищем в мертвых
                    {
                        log("   we not find aliveMother, now searching in dead");
                        List<Rabbit> candidates = new List<Rabbit>();
                        foreach (Rabbit ded in _deads)
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
#if !ASK
                            foreach (Rabbit c in candidates)
                            {
                                if (mbMother == null)
                                    mbMother = c;
                                if (mbMother.rID < c.rID)
                                    mbMother = c;
                            }
#else
                            log("   we find a {0:d} motherCandidate", candidates.Count);
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
                            }
#endif
                        }
                    }// if (mbMother == 0)
                }

                if (rab.SecnameID != 0)
                {
                    mbFather = _deads.GetRabbitByID(_names.GetSecnameUse(rab.SecnameID));
                    if (mbFather == null)//если нет живого кандидата на отца, то ищем в мертвых
                    {
                        log("   we not find aliveFather, now searching in dead");
                        List<Rabbit> candidates = new List<Rabbit>();
                        foreach (Rabbit ded in _deads)
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
#if !ASK
                            foreach (Rabbit c in candidates)
                            {
                                if (mbFather == null)
                                    mbFather = c;
                                if (mbFather.rID < c.rID)
                                    mbFather = c;
                            }
#else
                            log("   we find a {0:d} fatherCandidate", candidates.Count);
                            log("   #       name");
                            foreach (Rabbit c in candidates)
                            {
                                log("   {0:d}        {1:d}     ", c.rID, c.NameId);
                            }
                            while (true)//_deadFatherNames
                            {
                                log("you nead a type '№ of candFather' or type 'n' to next");
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
                                        mbFather = c;
                                        exitloop = true;
                                        break;
                                    }
                                }
                                if (exitloop) break;
                                log("   you type a wrong symbol, Idiot!");
                            }
#endif
                        }
                    }//if(mbFather==0)
                }
                log("   here what we find:");
                log("                     mother: {0:s}", mbMother == null ? "-" : mbMother.Name);
                log("                     father: {0:s}", mbFather == null ? "-" : mbFather.Name);
#if ASK
                while (true)
                {
                    log("   do you accept this Parents? [y/n] ");
                    string ans = Console.ReadLine();
                    if (ans == "n")
                        break;
                    else if (ans == "y" || ans == "")
                    {
#endif
                        rab.Mother = mbMother == null ? 0 : mbMother.rID;
                        rab.Father = mbFather == null ? 0 : mbFather.rID;
#if ASK
                        break;
                    }
                    else log("   you type a wrong symbol, Idiot!");
                }
#endif

            }
        }

        private static void repairNames()
        {
            foreach (Rabbit r in _rabbits)
            {
                if (r.NameId == 0) continue;
                bool breakloop;
                foreach (Name n in _names)
                {
                    breakloop = false;
                    if (r.NameId == n.nID)
                    {
                        if (r.rID == n.useRabbit)
                        {
                            log("   OK.name:{0:d} is fine", n.nID);
                            break;
                        }
                        else
                        {
#if ASK
                            log("   OOOPS. the rabit and Use not match");
                            log("   rabid: {0:d}|name: {1:d} || n_id: {2:d}|n_use: {3:d}", r.rID, r.NameId, n.nID, n.useRabbit);
                            while (true)
                            {
                                log("   What we need to do? 'u'-set rabbit to use 'n'-skip this rabbit");
                                string ans = Console.ReadLine().ToLower();
                                if (ans == "u")
                                {
                                    breakloop = true;
#endif
                                    n.useRabbit = r.rID;
                                    
                                    break;
#if ASK
                                }
                                else if (ans != "n")
                                {
                                    log("   Are You Idiot? You must type 'u' or 'n'. Lets Ask Again.");
                                }
                                else
                                {
                                    breakloop = true;
                                    break;
                                }
                            }
                            if (breakloop) break;                                  
#endif

                        }
                    }
                }
            }
        }

        private static void repairFucksStartDate_byRabEventDate()
        {
            log("--rapair fucks Start_Date by Mother event_date --");
            foreach (Rabbit rab in _rabbits.SukrolMothers)
            {
                log("searching fuck mother: {0:d}",rab.rID);
                List<Fuck> candidates = new List<Fuck>();
                int maxCand = 0;

                foreach (Fuck f in _fucks)
                {
                    if (f.SheID == rab.rID && f.StartDate == DateTime.MinValue && f.fState == Fuck.State.Sukrol)
                    {
                        candidates.Add(f);
#if !ASK
                        if (f.fID > maxCand)
                            maxCand = f.fID;
#endif
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
#if ASK
                    log("   #       mother      father      start       end     childr");
                    foreach (Fuck f in candidates)
                    {
                        log("   {0:d}        {1:d}      {2:d}     {3:s}     {4:s}     {5:d}",f.fID, f.SheID, f.HeID,
                            f.StartDate == DateTime.MinValue ? "null" : f.StartDate.ToString("yyyy-MM--dd"),
                            f.EndDate == DateTime.MinValue ? "null" : f.EndDate.ToString("yyyy-MM--dd"),
                            f.Children);
                    }*/
                    /*while (true)//_StartDate
                    {
                        log("you nead a type '# of fuck' or type 'n' to next");
                        string ans = Console.ReadLine();
                        if (ans == "n")
                        {
                            log("   you chose nobody");
                            break;
                        }
                        int n = 0;
                        int.TryParse(ans, out n);
#else
                        int n = maxCand;
#endif
                        
                        bool exitloop = false;
                        foreach (Fuck f in candidates)
                        {
                            if (f.fID == n)
                            {
                                f.StartDate = rab.EventDate;
                                f.Children = 0;
                                exitloop = true;
                                break;
                            }
                        }
                        if (exitloop)
                        {
                            foreach (Fuck f in candidates)
                            {
                                if (f.fID != n)
                                {
                                    f.fState = Fuck.State.Proholost;
                                    f.Children = 0;
                                }
                                
                            }
                        }
                        //log("   you type a wrong symbol, Idiot!");
                    //}
                }
            }
        }       

        private static void repairFucksEndDate_byYoungers()
        {
            log("--rapair fucks End_Date by Father and Mother of Younger--");
            List<Rabbit> youngers = _rabbits.Yongers;
            foreach (Rabbit yng in youngers)
            {
                if (yng.Mother == 0 || yng.Father == 0) continue;
                log("searching fuck where m:{0:d}|f:{1:d}",yng.Mother,yng.Father);
                ///заполняем массив факов где f_rabid и f_parther совпадают с r_mother и r_father 
                List<Fuck> candidates = new List<Fuck>();
                foreach(Fuck f in _fucks)
                {                  
                    if(f.EndDate != DateTime.MinValue) continue;
                    if(yng.Mother == f.SheID && yng.Father == f.HeID)
                    {
                        candidates.Add(f);
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
                    continue;
                }
                else if (candidates.Count > 1)
                {
                    log("   we find a {0:d} fucks",candidates.Count);
                    log("   #       mother      father      start       end     childr");
                    foreach (Fuck f in candidates)
                    {
                        log("   {0:d}        {1:d}      {2:d}     {3:s}     {4:s}     {5:d}",f.fID,f.SheID,f.HeID,
                            f.StartDate == DateTime.MinValue?"null":f.StartDate.ToString("yyyy-MM--dd"),
                            f.EndDate == DateTime.MinValue ? "null" : f.EndDate.ToString("yyyy-MM--dd"), 
                            f.Children);
                    }
                    while (true)//_EndDate
                    {
                        log("you nead a type '№ of fuck' or type 'n' to next");
                        string ans = Console.ReadLine();
                        if (ans == "n")
                        {
                            log("   you chose nobody");
                            break;
                        }
                        int n = 0;
                        int.TryParse(ans, out n);
                        bool exitloop = false;
                        foreach (Fuck f in candidates)
                        {
                            if (f.fID == n)
                            {
                                f.EndDate = yng.Born;
                                if (f.fState == Fuck.State.Sukrol)
                                    f.fState = Fuck.State.Okrol;
                                exitloop = true;
                                break;
                            }
                        }
                        if (exitloop) break;
                        log("   you type a wrong symbol, Idiot!");
                    }
                }//if cand.count>0
            }
        }

        private static void repairFucksIfChildrenThenOkrol()
        {
            log("--searching fucks where state is 'sukrol'  and childrens<>0--");
            foreach (Fuck f in _fucks)
            {
                if (f.fState == Fuck.State.Sukrol && f.Children != 0)
                {
                    log("   fixing a bug. chldrn: {0:d}| state: {1:s}",f.Children,f.fState.ToString());
                    f.fState = Fuck.State.Okrol;
                }
            }
        }

        private static void searchYoungerMother_bySurname()
        {
            log("--repair mother of youngers--");
            //List<Rabbit> yongers = _rabbits.Yongers;
            log("youngers count: {0:d}", _rabbits.YongersCount);
            foreach (Rabbit yng in _rabbits.Yongers)
            {
                if (yng.Mother != 0) continue;
                log("searching mother for: {0:d}",yng.rID);
                bool next = false;
                foreach (Rabbit rab in _rabbits)
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
            log("--search father of youngers BY SURNAME--");
            List<Rabbit> yongers = _rabbits.Yongers;
            foreach (Rabbit yng in yongers)
            {
                if (yng.Father != 0) continue;
                log("searching father for: {0:d}", yng.rID);
                bool next =false;
                foreach (Rabbit rab in _rabbits)
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

        private static void saveRabbits()
        {
            log("--saving rabbits--");
            foreach (Rabbit rab in _rabbits)
            {
                _cmd.CommandText = String.Format("UPDATE rabbits SET r_mother={0:d},r_father={1:d} WHERE r_id={2:d};",rab.Mother,rab.Father,rab.rID);
                _cmd.ExecuteNonQuery();
            }
        }

        private static void saveFucks()
        {
            log("--saving fucks--");
            foreach (Fuck f in _fucks)
            {
                _cmd.CommandText = String.Format("UPDATE fucks SET {0:s} {1:s} f_state='{2}',f_choldren={4:d} WHERE f_id={3:d};",
                    f.StartDate == DateTime.MinValue ?"":String.Format("f_date='{0:yyy-MM-dd}',",f.StartDate),
                    f.EndDate == DateTime.MinValue ? "" : String.Format("f_end_date='{0:yyy-MM-dd}',", f.EndDate),
                    f.fState.ToString().ToLower(),f.fID,f.Children);
                _cmd.ExecuteNonQuery();
            }
        }

        private static void giveABreak()
        {
#if !NOBREAK
            log("--lets take a 10sec break--");
            System.Threading.Thread.Sleep(10000);
#endif
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
    }

    enum Sex{Male,Female,Void}

    class Rabbit
    {
        internal readonly int rID;
        internal int Mother;
        internal int Father;
        internal Sex Sex;
        internal int NameId;
        /// <summary>
        /// Фамилия по матери
        /// </summary>
        internal int SurnameID;
        /// <summary>
        /// Фамилия по отцу
        /// </summary>
        internal int SecnameID;
        internal DateTime Born = DateTime.MinValue;
        internal int ParentID = 0;
        internal DateTime EventDate;
        internal string Name;

        internal Rabbit(int rid,int mother,int father,string sex,int name,int surname,int secname,DateTime born,int parent,DateTime ev_date, string namestr)
        {
            this.rID = rid;
            this.Mother = mother;
            this.Father = father;
            switch (sex)
            {
                case "male": this.Sex = Sex.Male; break;
                case "female": this.Sex = Sex.Female; break;
                case "void": this.Sex = Sex.Void; break;
            }
            this.NameId = name;
            this.SurnameID = surname;
            this.SecnameID = secname;
            this.Born = born;
            this.ParentID = parent;
            this.EventDate = ev_date;
            this.Name = namestr;
        }
    }

    class Name
    {
        internal readonly int nID;
        internal readonly Sex nameSex;
        internal readonly string NameStr;
        internal int useRabbit;
        internal readonly bool Blocked = false;
        internal readonly DateTime BlockDate;

        internal Name(int id, string sex, string name, int use, DateTime block)
        {
            this.nID = id;
            switch (sex)
            {
                case "male": this.nameSex = Sex.Male; break;
                case "female": this.nameSex = Sex.Female; break;
                case "void": this.nameSex = Sex.Void; break;
            }
            this.NameStr = name;
            this.useRabbit = use;
            if (block != DateTime.MinValue)
                this.Blocked = true;
            this.BlockDate = block;
        }
    }

    class Fuck
    {
        internal enum State { Sukrol, Okrol, Proholost };

        internal int fID;
        /// <summary>
        /// ID крольчихи
        /// </summary>
        internal int SheID;
        /// <summary>
        /// ID самца
        /// </summary>
        internal int HeID;
        internal DateTime StartDate;
        internal DateTime EndDate;
        internal State fState;
        internal int Children;

        internal Fuck(int id, int rabid, int partner, DateTime date, DateTime end_date, string state, int children)
        {
            this.fID = id;
            this.SheID = rabid;
            this.HeID = partner;
            this.StartDate = date;
            this.EndDate = end_date;
            switch (state)
            {
                case "okrol": this.fState = State.Okrol; break;
                case "sukrol": this.fState = State.Sukrol; break;
                case "proholost": this.fState = State.Proholost; break;
            }
            this.Children = children;
        }
    }

    class RabbitList : List<Rabbit>
    {
        private int _youngCount = 0;
        private int _adultCount = 0;

        internal int RabbitsCount
        {
            get { return this.Count; }
        }
        internal int YongersCount
        {
            get { return _youngCount; }
        }
        internal int AdultCount
        {
            get { return _adultCount; }
        }


        internal virtual void LoadContent(MySqlCommand cmd)
        {
            Program.log("fill All Rabbits");
            cmd.CommandText = String.Format("SELECT r_id,r_mother,r_father,r_sex,r_name,r_surname,r_secname,r_born,r_parent,COALESCE(r_event_date,'0001-01-01')ev_date,rabname(r_id,2) nm FROM rabbits ORDER BY r_id ASC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                this.Add(new Rabbit(rd.GetInt32("r_id"), rd.GetInt32("r_mother"), rd.GetInt32("r_father"),
                                        rd.GetString("r_sex"), rd.GetInt32("r_name"), rd.GetInt32("r_surname"), rd.GetInt32("r_secname"),
                                        rd.GetDateTime("r_born"), rd.GetInt32("r_parent"), rd.GetDateTime("ev_date"),rd.GetString("nm"))
                            );
            }
            rd.Close();
            Program.log(" |rabbits count: {0:d}", this.Count);
        }

        internal List<Rabbit> Adult
        {
            get
            {
                List<Rabbit> result = new List<Rabbit>();
                foreach (Rabbit rab in this)
                {
                    if (rab.ParentID == 0)
                        result.Add(rab);
                }
                _adultCount = result.Count;
                return result;
            }
        }

        internal List<Rabbit> Yongers
        {
            get
            {
                List<Rabbit> result = new List<Rabbit>();
                foreach (Rabbit rab in this)
                {
                    if (rab.ParentID != 0)
                        result.Add(rab);
                }
                _youngCount = result.Count;
                return result;
            }
        }

        internal List<Rabbit> SukrolMothers
        {
            get
            {
                List<Rabbit> result = new List<Rabbit>();
                foreach (Rabbit rab in this)
                {
                    if (rab.Sex == Sex.Female && rab.EventDate !=DateTime.MinValue)
                        result.Add(rab);
                }
                return result;
            }
        }

        internal Rabbit GetRabbitByID(int id)
        {
            foreach(Rabbit r in this)
            {
                if(r.rID==id)
                    return r;
            }
            return null;
        }

    }

    class DeadList : RabbitList
    {
        internal override void LoadContent(MySqlCommand cmd)
        {
            Program.log("fill dead Rabbits");
            cmd.CommandText = String.Format("SELECT r_id,r_mother,r_father,r_sex,r_name,r_surname,r_secname,r_born,r_parent,deadname(r_id,2) nm FROM dead ORDER BY r_id DESC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                this.Add(new Rabbit(rd.GetInt32("r_id"), rd.GetInt32("r_mother"), rd.GetInt32("r_father"),
                                        rd.GetString("r_sex"), rd.GetInt32("r_name"), rd.GetInt32("r_surname"), rd.GetInt32("r_secname"),
                                        rd.GetDateTime("r_born"), rd.GetInt32("r_parent"), DateTime.MinValue, rd.GetString("nm"))
                            );
            }
            rd.Close();
            Program.log(" |deads count: {0:d}", this.Count);
        }
    }

    class NameList : List<Name>
    {
        internal void LoadNames(MySqlCommand cmd)
        {
            Program.log("fill all names");
            cmd.CommandText = String.Format("SELECT n_id,n_sex,n_name,n_use,COALESCE(n_block_date,'0001-01-01')dt FROM names WHERE n_use<>0 ORDER BY n_use ASC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                this.Add(new Name(rd.GetInt32("n_id"), rd.GetString("n_sex"),
                                    rd.GetString("n_name"), rd.GetInt32("n_use"), rd.GetDateTime("dt"))
                           );
            }
            rd.Close();
            Program.log(" |name count: {0:d}", this.Count);
        }

        internal int GetSurnameUse(int surname)
        {
            foreach(Name n in this)
            {
                if (n.nameSex == Sex.Female && n.nID == surname)
                    return n.useRabbit;
            }
            return 0;
        }

        internal int GetSecnameUse(int secname)
        {
            foreach (Name n in this)
            {
                if (n.nameSex == Sex.Male && n.nID == secname)
                    return n.useRabbit;
            }
            return 0;
        }
    }

    class FuckList : List<Fuck>
    {
        internal void LoadFucks(MySqlCommand cmd)
        {
            Program.log("fill fucks");
            cmd.CommandText = String.Format("SELECT f_id,f_rabid,f_partner,COALESCE(f_date,'0001-01-01') f_date, COALESCE(f_end_date,'0001-01-01')f_end_date,f_state,f_children FROM fucks ORDER BY f_id ASC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                this.Add(new Fuck(rd.GetInt32("f_id"), rd.GetInt32("f_rabid"), rd.GetInt32("f_partner"),
                                    rd.GetDateTime("f_date"), rd.GetDateTime("f_end_date"), rd.GetString("f_state"),
                                    rd.GetInt32("f_children"))
                          );
            }
            rd.Close();
            Program.log(" |fucks count: {0:d}", this.Count);
        }
    }


   
}
