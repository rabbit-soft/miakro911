#if DEBUG
    #define NOCATCH
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using MySql.Data.MySqlClient;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace miaRepair
{
    class Program
    {
        private static ILog _logger = LogManager.GetLogger("miaRepair");
        
        private static MySqlConnection _sql;
        private static MySqlCommand _cmd;
        private static RabbitList _rabbits = new RabbitList();
        private static List<Rabbit> _deads = new List<Rabbit>();
        private static List<Name> _names = new List<Name>();
        private static List<Fuck> _fucks = new List<Fuck>();

        static void Main(string[] args)
        {
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
                fillAllRabbits();
                fillDeadRabbits();
                fillNames();
                fillFucks();
                searchYoungerMother_bySurname();
                searchYoungerFathers_bySecname();
                repairFucksIfChildrenThenOkrol();
                repairFucksEndDateByYoungers();

                
                saveRabbits();
                saveFucks();
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

        private static void repairFucksEndDateByYoungers()
        {
            log("rapair fucks End_Date by Father and Mother");
            List<Rabbit> youngers = _rabbits.Yongers;
            foreach (Rabbit yng in youngers)
            {
                if (yng.Mother == 0 || yng.Father == 0) continue;
                log("searching fuck where m:{0:d}|f:{1:d}",yng.Mother,yng.Father);
                List<Fuck> candidates = new List<Fuck>();
                foreach(Fuck f in _fucks)
                {
                    
                    if(f.EndDate != DateTime.MinValue) continue;
                    if(yng.Mother == f.SheID && yng.Father == f.HeID)
                    {
                        log("   we fing a f_id: {0:d}",f.fID);
                        candidates.Add(f);
                        break;
                    }
                }
                if (candidates.Count == 0)
                    log("   we find nothing");
                else if (candidates.Count == 1)
                    log("   we find only one fuck: {0:d}",candidates[0].fID);
                else if (candidates.Count > 1)
                {
                    log("   we find a {0:s} fucks,you nead a chose № or type 'n' to next");
                    log("   #       mother      father      start       end     childr");
                    foreach (Fuck f in candidates)
                    {
                        log("   {0}        {1}      {2}     {3}     {4}     {5:d}");
                    }
                    while (true)
                    {
                        string ans = Console.ReadLine();
                        if (ans == "n")
                        {
                            log("   you chose nobody");
                            continue;
                        }
                        int n = 0;
                        int.TryParse(ans, out n);
                        foreach (Fuck f in candidates)
                        {
                            if (f.fID == n)
                            {
                                f.EndDate = yng.Born;
                                if (f.fState == Fuck.State.Sukrol)
                                    f.fState = Fuck.State.Okrol;
                            }
                        }
                        log("   you type a wrong symbol, Idiot!");
                    }
                }
            }
        }

        private static void repairFucksIfChildrenThenOkrol()
        {
            log("searching fucks where state is 'sukrol'  and childrens<>0");
            foreach (Fuck f in _fucks)
            {
                if (f.fState == Fuck.State.Sukrol && f.Children != 0)
                    f.fState = Fuck.State.Okrol;
            }
        }

        #region fill
        private static void fillFucks()
        {
            log("fill fucks");
            _cmd.CommandText = String.Format("SELECT f_id,f_rabid,f_partner,COALESCE(f_date,'0001-01-01') f_date, COALESCE(f_end_date,'0001-01-01')f_end_date,f_state,f_children FROM fucks ORDER BY f_id ASC;");
            MySqlDataReader rd = _cmd.ExecuteReader();
            while (rd.Read())
            {
                _fucks.Add(new Fuck(rd.GetInt32("f_id"),rd.GetInt32("f_rabid"),rd.GetInt32("f_partner"),
                                    rd.GetDateTime("f_date"),rd.GetDateTime("f_end_date"),rd.GetString("f_state"),
                                    rd.GetInt32("f_children"))
                          );
            }
            rd.Close();
            log(" |fucks count: {0:d}", _fucks.Count);
        }

        private static void fillDeadRabbits()
        {
            log("fill dead Rabbits");
            _cmd.CommandText = String.Format("SELECT r_id,r_mother,r_father,r_sex,r_name,r_surname,r_secname,r_born,r_parent FROM dead ORDER BY r_id DESC;");
            MySqlDataReader rd = _cmd.ExecuteReader();
            while (rd.Read())
            {
                _deads.Add(new Rabbit(rd.GetInt32("r_id"), rd.GetInt32("r_mother"), rd.GetInt32("r_father"),
                                        rd.GetString("r_sex"), rd.GetInt32("r_name"), rd.GetInt32("r_surname"), rd.GetInt32("r_secname"),
                                        rd.GetDateTime("r_born"), rd.GetInt32("r_parent"),DateTime.MinValue)
                            );
            }
            rd.Close();
            log(" |deads count: {0:d}", _deads.Count);
        }

        private static void fillNames()
        {
            log("fill all names");
            _cmd.CommandText = String.Format("SELECT n_id,n_sex,n_name,n_use,COALESCE(n_block_date,'0001-01-01')dt FROM names WHERE n_use<>0 ORDER BY n_use ASC;");
            MySqlDataReader rd = _cmd.ExecuteReader();
            while (rd.Read())
            {
                _names.Add(new Name(rd.GetInt32("n_id"),rd.GetString("n_sex"),
                                    rd.GetString("n_name"),rd.GetInt32("n_use"),rd.GetDateTime("dt"))
                           );
            }
            rd.Close();
            log(" |name count: {0:d}", _names.Count);
        }

        /// <summary>
        /// Заполняет Лист всеми кроликами
        /// </summary>
        private static void fillAllRabbits()
        {
            log("fill All Rabbits");
            _cmd.CommandText = String.Format("SELECT r_id,r_mother,r_father,r_sex,r_name,r_surname,r_secname,r_born,r_parent,COALESCE(r_event_date,'0001-01-01')ev_date FROM rabbits ORDER BY r_id ASC;");
            MySqlDataReader rd = _cmd.ExecuteReader();
            while (rd.Read())
            {
                _rabbits.Add(new Rabbit(rd.GetInt32("r_id"),rd.GetInt32("r_mother"),rd.GetInt32("r_father"),
                                        rd.GetString("r_sex"),rd.GetInt32("r_name"),rd.GetInt32("r_surname"),rd.GetInt32("r_secname"),
                                        rd.GetDateTime("r_born"), rd.GetInt32("r_parent"), rd.GetDateTime("ev_date"))
                            );
            }
            rd.Close();
            log(" |rabbits count: {0:d}", _rabbits.Count);
        }
        #endregion fill


        private static void searchYoungerMother_bySurname()
        {
            log("repair mother of youngers");
            List<Rabbit> yongers = _rabbits.Yongers;
            log("youngers count: {0:d}", yongers.Count);
            foreach (Rabbit yng in yongers)
            {
                if (yng.Mother != 0) continue;
                log("searching mother for: {0:d}",yng.rID);
                foreach (Rabbit rab in _rabbits)
                {
                    if (rab.Sex == Sex.Female && rab.rID == yng.ParentID )
                    {
                        if (rab.NameId == yng.SurnameID)
                        {
                            log("   OK. We Find a mother: {0:d}", rab.rID);
                            yng.Mother = rab.rID;
                            break;
                        }
                    }
                }
                log("   we find nothing");
            }            
        }

        private static void searchYoungerFathers_bySecname()
        {
            log("search father of youngers BY SURNAME");
            List<Rabbit> yongers = _rabbits.Yongers;
            foreach (Rabbit yng in yongers)
            {
                if (yng.Father != 0) continue;
                log("searching father for: {0:d}", yng.rID);
                foreach (Rabbit rab in _rabbits)
                {
                    if (rab.Sex == Sex.Male && rab.NameId == yng.SecnameID)
                    {
                        log("   OK. We Find a father: {0:d}", rab.rID);
                        yng.Father = rab.rID; 
                        break;
                    }
                }
                log("   we find nothing");
            }
        }

        private static void saveRabbits()
        {
            log("saving rabbits");
            foreach (Rabbit rab in _rabbits)
            {
                _cmd.CommandText = String.Format("UPDATE rabbits SET r_mother={0:d},r_father={1:d} WHERE r_id={2:d};",rab.Mother,rab.Father,rab.rID);
                _cmd.ExecuteNonQuery();
            }
        }

        private static void saveFucks()
        {
            log("saving fucks");
            foreach (Fuck f in _fucks)
            {
                if (f.StartDate == DateTime.MinValue && f.EndDate == DateTime.MinValue) continue;
                _cmd.CommandText = String.Format("UPDATE fucks SET {0:s} {1:s} f_state='{2}' WHERE f_id={3:d};",
                    f.StartDate == DateTime.MinValue?"":String.Format("f_date='{0:yyy-MM-dd}',",f.StartDate),
                    f.EndDate == DateTime.MinValue ? "" : String.Format("f_end_date='{0:yyy-MM-dd}',", f.EndDate),
                    f.fState.ToString().ToLower(),f.fID);
                _cmd.ExecuteNonQuery();
            }
        }

        #region loging
        private static void log(string s)
        {
            Console.WriteLine(s);
            _logger.Info(s);
        }

        private static void log(string s, object arg0)
        {
            log(String.Format(s, arg0));
        }

        private static void log(string s, object arg0,object arg1)
        {
            log(String.Format(s, arg0,arg1));
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

        internal Rabbit(int rid,int mother,int father,string sex,int name,int surname,int secname,DateTime born,int parent,DateTime ev_date)
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
        }
    }

    class RabbitList : List<Rabbit>
    {
        internal List<Rabbit> Yongers
        {
            get
            {
                List<Rabbit> yongers = new List<Rabbit>();
                foreach (Rabbit rab in this)
                {
                    if (rab.ParentID != 0)
                        yongers.Add(rab);
                }
                return yongers;
            }
        }
    }

    class Name
    {
        internal readonly int nID;
        internal readonly Sex nameSex;
        internal readonly string NameStr;
        internal readonly int useRabbit;
        internal readonly bool Blocked = false;
        internal readonly DateTime BlockDate;

        internal Name(int id,string sex,string name, int use,DateTime block)
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
        internal enum State {Sukrol,Okrol,Proholost};

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

        internal Fuck(int id,int rabid,int partner, DateTime date,DateTime end_date, string state,int children)
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
}
