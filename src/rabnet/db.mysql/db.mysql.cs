using System;
using System.Collections.Generic;
using System.Drawing;
using log4net;
using MySql.Data.MySqlClient;
using rabnet;
using System.Xml;

namespace db.mysql
{
    public class RabNetDbMySql:IRabNetDataLayer
    {
        private MySqlConnection psql=null;
        private ILog log = LogManager.GetLogger(typeof(RabNetDbMySql));

        public RabNetDbMySql() 
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Debug("created");
        }

        private MySqlConnection sql
        {
            get
            {
                if (psql == null) return null;
                if (psql.State == System.Data.ConnectionState.Broken || psql.State == System.Data.ConnectionState.Closed)
                    psql.Open();
                return psql;
            }
        }

        public RabNetDbMySql(String connectionString): this()
        {
            Init(connectionString);
        }

        /// <summary>
        /// Деструктор класса.
        /// </summary>
        ~RabNetDbMySql()
        {
            Close();
        }

        #region IRabNetDataLayer Members

        /// <summary>
        /// Закрывает и уничтожает подключение к Базе Данных
        /// </summary>
        public void Close()
        {
            if (psql != null)
            {
                psql.Close();
                psql = null;
            }
        }
        /// <summary>
        /// Создает и открывает новое подключение к Базе Данных
        /// </summary>
        /// <param name="connectionString">Строка подключение. Хранится в app.config</param>
        public void Init(String connectionString)
        {
            Close();
            log.Debug("init from string "+connectionString);
            psql = new MySqlConnection(connectionString);
            psql.Open();
            new Users(sql).checktb();
        }
        /// <summary>
        /// Выполняет sql-Команду
        /// </summary>
        /// <param name="cmd">sql-команда</param>
        /// <returns>Количество затронутых строк</returns>
        public int Exec(String cmd)
        {
            //log.Debug("exec query:" + cmd);
            MySqlCommand c = new MySqlCommand(cmd, sql);
            return c.ExecuteNonQuery();
        }
        /// <summary>
        /// Выполняет sql-команду и возвращает результат запроса как MySqlDataReader
        /// </summary>
        /// <param name="cmd">sql-команда</param>
        /// <returns>Результат выполнения sql-команды</returns>
        private MySqlDataReader reader(String cmd)
        {
#if DEBUG
            //log.Debug("reader query:"+cmd);
#endif
            MySqlCommand c=new MySqlCommand(cmd,sql);
            return c.ExecuteReader();
        }

        public List<sUser> GetUsers()
        {
            return new Users(sql).getUsers();
        }

        public sUser getUser(int uid)
        {
            return new Users(sql).getUser(uid);
        }

        public int checkUser(string name, string password)
        {
            return new Users(sql).checkUser(name, password);
        }

        public string getOption(string name, string subname, uint uid)
        {
            MySqlDataReader rd=reader(String.Format("SELECT o_value FROM options WHERE o_name='{0:s}' AND o_subname='{1:s}' AND (o_uid={2:d} OR o_uid=0) ORDER BY o_uid DESC;",
                name,subname,uid));
            string res="";
            //if(subname=="xls_ask")
                //res = "";
            if (rd.Read())
            {
                res = rd.GetString(0);
                rd.Close();
            }
            else
            {
                rd.Close();
                MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO options(o_name,o_subname,o_uid,o_value) VALUES('{0:s}','{1:s}',{2:d},'0');", name, subname, uid), sql);
                cmd.ExecuteNonQuery();
            }       
            return res;
        }

        public void setOption(string name, string subname, uint uid, string value)
        {
            Exec(String.Format("DELETE FROM options WHERE o_name='{0:s}' AND o_subname='{1:s}' AND o_uid={2:d};",
                name,subname,uid));
            Exec(String.Format("INSERT INTO options(o_name,o_subname,o_uid,o_value) VALUES('{0:s}','{1:s}',{2:d},'{3:s}');",
                name,subname,uid,value));
        }

        public DateTime now()
        {
            MySqlDataReader rd = reader("SELECT NOW();");
            rd.Read();
            DateTime res = rd.GetDateTime(0);
            rd.Close();
            return res;
        }

        public IDataGetter getRabbits(Filters filters)
        {
            return new RabbitsDataGetter(sql, filters);
        }

        public IDataGetter getBuildingsRows(Filters filters)
        {
            return new Buildings(sql, filters);
        }

        public String[] getFilterNames(string type)
        {
            MySqlDataReader rd = reader("SELECT f_name FROM filters WHERE f_type='"+type+"';");
            List<String> nms=new List<string>();
            while (rd.Read())
                nms.Add(rd.GetString(0));
            rd.Close();
            return nms.ToArray();
        }

        public Filters getFilter(string type, string name)
        {
            MySqlDataReader rd = reader("SELECT f_filter FROM filters WHERE f_type='"+type+"' AND f_name='"+name+"';");
            Filters f = new Filters();
            if (rd.Read())
                f.fromString(rd.GetString(0));
            rd.Close();
            return f;
        }

        public void setFilter(string type, string name, Filters filter)
        {
            Exec("DELETE FROM filters WHERE f_type='"+type+"' AND f_name='"+name+"';");
            Exec(String.Format("INSERT INTO filters(f_type,f_name,f_filter) VALUES('{0:s}','{1:s}','{2:s}');",
                type,name,filter.toString()));
        }

        public String GetRabGenoms(int rId)
        {
            return RabbitGenGetter.GetRabGenoms(sql, rId);
        }

        public RabTreeData rabbitGenTree(int rabbit)
        {
            return RabbitGenGetter.GetRabbitGenTree(sql,rabbit);
        }

        public BldTreeData buildingsTree()
        {
            return Buildings.getTree(0, sql, null);
        }

        public IDataGetter GetYoungers(Filters filters)
        {
            return new Youngers(sql, filters);
        }
        public YoungRabbit[] GetYoungers(int momId)
        {
            return Youngers.GetYoungers(sql,momId);
        }
        public OneRabbit[] GetNeighbors(int rabId)
        {
            return RabbitGetter.GetNeighbors(sql, rabId);
        }

        public int[] getTiers(int farm)
        {
            if (farm == 0)
                return new int[] { 0, 0 };
            MySqlDataReader rd = reader("SELECT m_upper,m_lower FROM minifarms WHERE m_id=" + farm.ToString() + ";");
            rd.Read();
            int[] trs = new int[] { rd.GetInt32(0), rd.GetInt32(1) };
/*            if (trs[1] != 0)
            {
                trs[0] ^= trs[1];
                trs[1] ^= trs[0];
                trs[0] ^= trs[1];
            }
 */
            rd.Close();
            return trs;
        } 

        public IDataGetter getNames(Filters filters)
        {
            return new Names(sql, filters);
        }

        public IDataGetter zooTeh(Filters f)
        {
            return new ZooTehNullGetter();
        }
       
        public List<String> getFuckMonths()
        {
            return FucksGetter.getFuckMonths(sql);
        }

        public void CancelFuckEnd(int fuckID)
        {
            FucksGetter.cancelFuckEnd(sql, fuckID);
        }

        public List<String> getDeadsMonths()
        {
            return DeadHelper.getDeadMonths(sql);
        }

        ICatalog IRabNetDataLayer.getBreeds()
        {
            return new Breeds(sql);
        }

        public ICatalog getDeadReasons()
        {
            return new DeadReasons(sql);
        }

        public OneRabbit GetRabbit(int rid)
        {
            return RabbitGetter.GetRabbit(sql, rid);
        }
        public void SetRabbit(OneRabbit r)
        {
            RabbitGetter.SetRabbit(sql,r);
        }
        public ICatalogs catalogs()
        {
            return new Catalogs(sql);
        }

        public void RabNetLog(int type, int user, int r1,int r2,string a1,string a2,string text)
        {
            Logs.addLog(sql,type,user,r1,r2,a1,a2,text);
        }

        public Fucks GetFucks(Filters f)
        {
            return FucksGetter.GetFucks(sql, f);
        }

        public FuckPartner[] GetAllFuckers(Filters f)
        {
            return FucksGetter.AllFuckers(sql, f);
        }

        public void setBon(int rabbit, string bon)
        {
            RabbitGetter.setBon(sql, rabbit, bon);
        }

        public void MakeFuck(int female, int male, int daysPast,int worker,bool syntetic)
        {
            FucksGetter.MakeFuck(sql, female, male, daysPast, worker, syntetic);
        }

        public void makeProholost(int female, int daysPast)
        {
            RabbitGetter.MakeProholost(sql, female, daysPast);
        }

        public int makeOkrol(int female, int daysPast, int children, int dead)
        {
            return RabbitGetter.MakeOkrol(sql, female, daysPast, children, dead);
        }

        public ICatalog getZones()
        {
            return new Zones(sql);
        }

        public ICatalog getProductTypes()
        {
            return new Products(sql);
        }

        //public ICatalog getVaccines()
        //{
        //    return new Vaccines(sql);
        //}

        public string makeName(int nm, int sur, int sec, int grp, Rabbit.SexType sex)
        {
            return RabbitGetter.makeName(sql, nm, sur, sec, grp, sex);
        }

        public bool unblockName(int id)
        {
            return Names.unblockName(sql, id);
        }

        //public YoungRabbit[] getSuckers(int mom)
        //{
        //    return Youngers.GetYoungers(sql, mom);
        //}

        public void replaceRabbit(int rid, int farm, int tier_id, int sec)
        {
            RabbitGetter.replaceRabbit(sql, rid, farm, tier_id, sec);
        }
        public void replaceYounger(int rid, int farm, int tier_id, int sec)
        {
            RabbitGetter.replaceYounger(sql, rid, farm, tier_id, sec);
        }

        public int newRabbit(OneRabbit r, int mom)
        {
            return RabbitGetter.newRabbit(sql, r, mom);
        }
       
        public void addName(Rabbit.SexType sex, string name, string surname)
        {
            Names.addName(sql, sex, name, surname);
        }

        public void changeName(string orgName, string name, string surname)
        {
            Names.changeName(sql, orgName, name, surname);            
        }

        public void KillRabbit(int id, int daysPast, int reason, string notes)
        {
            RabbitGetter.killRabbit(sql, id, daysPast, reason, notes);
        }

        public void СountKids(int rid, int dead, int killed, int added,int yid)
        {
            RabbitGetter.countKids(sql, rid, dead, killed, added, yid);
        }

        public void setRabbitSex(int rid, Rabbit.SexType sex)
        {
            RabbitGetter.setRabbitSex(sql, rid, sex);
        }

        public int cloneRabbit(int rid, int count, int farm, int tier, int sec, Rabbit.SexType sex, int mom)
        {
            return RabbitGetter.cloneRabbit(sql, rid, count, farm, tier, sec, sex, mom);
        }
#region users
        public string userGroup(int uid)
        {
            return new Users(sql).getUserGroup(uid);
        }

        public void deleteUser(int uid)
        {
            new Users(sql).deleteUser(uid);
        }

        public void changeUser(int uid, string name, int group, string password, bool chpass)
        {
            new Users(sql).updateUser(uid, name, group, password, chpass);
        }

        public bool hasUser(string name)
        {
            return new Users(sql).hasUser(name);
        }

        public int addUser(string name, int group, string password)
        {
            return new Users(sql).addUser(name, group, password);
        }
#endregion users

        public IDataGetter getDead(Filters filters)
        {
            return new Deads(sql, filters);
        }

        public void changeDeadReason(int rid, int reason)
        {
            DeadHelper.changeDeadReason(sql,rid,reason );
        }

        public void ResurrectRabbit(int rid)
        {
            new DeadHelper(sql).ResurrectRabbit(rid);
        }

        public void placeSucker(int sucker, int mother)
        {
            RabbitGetter.placeSucker(sql, sucker, mother);
        }

        public void combineGroups(int rabfrom, int rabto)
        {
            RabbitGetter.combineGroups(sql, rabfrom, rabto);
        }

        public AdultRabbit[] getMothers(int age, int agediff)
        {
            return RabbitGetter.getMothers(sql, age, agediff);
        }
#region zoo_tech_get

        public ZootehJob[] GetZooTechJobs(Filters f, JobType type)
        {
            return new ZooTehGetter(sql, f).GetZooTechJobs(type);
        }

#endregion zoo_tech_get
   
#region buildings
        public Building getBuilding(int tier)
        {
            if (tier == 0)
                return null;
            return Buildings.getTier(tier, sql);
        }
        public void updateBuilding(Building b)
        {
            Buildings.updateBuilding(b, sql);
        }
        public int getMFCount()
        {
            return Buildings.GetMFCount(sql);
        }
        public Building[] getBuildings(Filters f)
        {
            return Buildings.getBuildings(sql, f);
        }
        public void setBuildingName(int bid, string name)
        {
            Buildings.setBuildingName(sql, bid, name);
        }

        public void addBuilding(int parent, string name)
        {
            Buildings.addBuilding(sql, parent, name, 0);
        }

        public void replaceBuilding(int bid, int toBuilding)
        {
            Buildings.replaceBuilding(sql, bid, toBuilding);
        }

        public void deleteBuilding(int bid)
        {
            Buildings.deleteBuilding(sql, bid);
        }

        public int addFarm(int parent, string uppertype, string lowertype, string name, int id)
        {
#if DEMO
            const int DEMO_MAX_FARMS = 10;
            if (getMFCount() >= DEMO_MAX_FARMS)
                throw new FarmCountOverdarwException();
#endif
            return Buildings.addFarm(sql, parent, uppertype, lowertype, name, id);
        }

        public bool FarmExists(int id)
        {
            return Buildings.farmExists(sql, id);
        }

        public void ChangeFarm(int fid, string uppertype, string lowertype)
        {
            Buildings.changeFarm(sql, fid, uppertype, lowertype);
        }

        public void deleteFarm(int fid)
        {
            Buildings.deleteFarm(sql, fid);
        }
#endregion buildings
        
        public string[] getWeights(int rabbit)
        {
            return new Weight(sql).getWeights(rabbit);
        }

        public void addWeight(int rabbit, int weight, DateTime date)
        {
            new Weight(sql).addWeight(rabbit, weight, date);
        }

        public void deleteWeight(int rabbit, DateTime date)
        {
            new Weight(sql).deleteWeight(rabbit, date);
        }
#region vaccines
        public RabVac[] GetRabVac(int rabId)
        {
            return RabbitGetter.GetRabVacs( sql,rabId);
        }

        public void SetRabbitVaccine(int rid, int vid,DateTime date)
        {
            RabbitGetter.SetRabbitVaccine(sql,rid,vid,date);
        }

        public void SetRabbitVaccine(int rid, int vid)
        {
            RabbitGetter.SetRabbitVaccine(sql, rid, vid);
        }

        public void RabVacUnable(int rid, int vid,bool unable)
        {
            RabbitGetter.RabVacUnable(sql, rid, vid,unable);
        }

        public List<Vaccine> GetVaccines(bool withSpec)
        {
            return new Vaccines(sql).Get(withSpec);
        }
        public Vaccine GetVaccine(int vid)
        {
            List<Vaccine> vaccs = new Vaccines(sql).Get(true);
            foreach (Vaccine v in vaccs)
                if (v.ID == vid)
                    return v;
            return null;
        }
#if !DEMO
        public int AddVaccine(string name, int duration, int age, int after, bool zoo, int times)
        {
            return new Vaccines(sql).Add(name, duration, age, after, zoo, times);
        }
#endif

        public void EditVaccine(int id, string name, int duration, int age, int after, bool zoo, int times)
        {
            new Vaccines(sql).Edit(id, name, duration, age, after, zoo, times);
        }
#endregion vaccines

        public OneRabbit[] getParents(int rabbit, int age)
        {
            return RabbitGetter.getParents(sql, rabbit, age);
        }

        public OneRabbit getLiveDeadRabbit(int rabbit)
        {
            return RabbitGetter.getLiveDeadRabbit(sql,rabbit);
        }

		public RabbitGen getRabbitGen(int rid)
		{
			return RabbitGenGetter.GetRabbitGen(sql, rid);
		}

		public Dictionary<int, Color> getBreedColors()
		{
			return RabbitGenGetter.getBreedColors(sql);
		}

        public double[] getMaleChildrenProd(int male)
        {
            return FucksGetter.getMaleChildrenProd(sql, male);
        }

        public void changeFucker(int fid, int fucker)
        {
            FucksGetter.changeFucker(sql,fid, fucker);
        }

        public void changeWorker(int fid, int worker)
        {
            FucksGetter.changeWorker(sql,fid, worker);
        }

        public DateTime GetFarmStartTime()
        {
            return Logs.getFarmStartTime(sql);
        }

        public IRabNetDataLayer Clone()
        {
            return this.MemberwiseClone() as IRabNetDataLayer;
        }

        public LogList getLogs(Filters f)
        {
            return (new Logs(sql).getLogs(f));
        }

        public Rabbit[] GetDescendants(int ascendantId)
        {
            return RabbitGetter.GetDescendants(sql, ascendantId);
        }

#if !DEMO

        public List<String> getButcherMonths()
        {
            return Butcher.getButcherMonths(sql);
        }

        

        public string[] logNames()
        {
            return new Logs(sql).logNames();
        }
        public void ArchLogs()
        {
            (new Logs(sql)).ArchLogs();
        }

        #region butcher
        public IDataGetter getButcherDates(Filters f)
        {
            return new Butcher(sql, f);
        }
        public DeadRabbit[] GetVictims(DateTime dt)
        {
            return Butcher.getVictims(sql,dt);
        }

        public List<sMeat> getMeats(DateTime date)
        {
            return Butcher.GetMeats(sql,date);
        }

        public List<sMeal> getMealPeriods()
        {
            return Meal.getMealPeriods(sql);
        }

        public void addMealIn(DateTime start, int amount)
        {
            Meal.AddMealIn(sql, start, amount);
        }

        public void addMealOut(DateTime start, int amount)
        {
            Meal.AddMealOut(sql, start, amount);
        }

        public void deleteMeal(int id)
        {
            Meal.DeleteMeal(sql, id);
        }
        //public void addPLUSummary(int prodid, string prodname, int tsell, int tsumm, int tweight, DateTime cleared)
        //{
        //    Scale.addPLUSummary(sql, prodid, prodname, tsell, tsumm, tweight, cleared);
        //}

        //public List<ScalePLUSummary> getPluSummarys(DateTime date)
        //{
        //    return Scale.getPluSummarys(sql, date);
        //}

        //public void deletePLUsummary(int sid, DateTime lastClear)
        //{
        //    Scale.DeletePLUsumary(sql, sid, lastClear);
        //}
        #endregion butcher

        public XmlDocument makeReport(myReportType type, Filters f)
        {
            return Reports.makeReport(sql, type, f);
        }

        public XmlDocument makeReport(string query)
        {
            return Reports.makeReport(sql, query);
        }

        public string WebReportGlobal(DateTime dt)
        {
            return WebReports.GetGlobal(sql, dt);
        }

        public string[] WebReportsGlobal(DateTime dt, int days)
        {
            return WebReports.GetGlobals(sql, dt, days);
        }
#endif

        #endregion IRabNetDataLayer Members
    }        
}
