using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using log4net;
using MySql.Data.MySqlClient;

namespace rabnet
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
            log.Debug("exec query:" + cmd);
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
            log.Debug("reader query:"+cmd);
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
            return new Rabbits(sql, filters);
        }

        public IDataGetter getBuildings(Filters filters)
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

        public TreeData rabbitGenTree(int rabbit)
        {
            return Rabbits.getRabbitGen(rabbit,sql);
        }

        public TreeData buildingsTree()
        {
            return Buildings.getTree(0, sql, null);
        }


        public IDataGetter GetYoungers(Filters filters)
        {
            return new Youngers(sql, filters);
        }
        public OneRabbit[] GetYoungers(int momId)
        {
            return RabbitGetter.GetYoungers(sql,momId);
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
            //TODO:remove item
            return new ZooTehNullGetter();
        }

        public List<String> getButcherMonths()
        {
            return Butcher.getButcherMonths(sql);
        }

        public List<String> getFuckMonths()
        {
            return FucksGetter.getFuckMonths(sql);
        }

        public void cancelFuckEnd(int fuckID)
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

        public Fucks getFucks(int rabbit)
        {
            return FucksGetter.GetFucks(sql, rabbit);
        }

        public Fucks allFuckers(int female,bool geterosis,bool inbreeding,int malewait)
        {
            return FucksGetter.AllFuckers(sql, female,geterosis,inbreeding,malewait);
        }

        public void setBon(int rabbit, string bon)
        {
            RabbitGetter.setBon(sql, rabbit, bon);
        }

        public void makeFuck(int female, int male, DateTime date,int worker)
        {
            RabbitGetter.makeFuck(sql, female, male, date,worker);
        }

        public void makeProholost(int female, DateTime when)
        {
            RabbitGetter.MakeProholost(sql, female, when);
        }

        public int makeOkrol(int female, DateTime when, int children, int dead)
        {
            return RabbitGetter.MakeOkrol(sql, female, when, children, dead);
        }

        public ICatalog getZones()
        {
            return new Zones(sql);
        }

        public ICatalog getProductTypes()
        {
            return new Products(sql);
        }

        public ICatalog getVaccines()
        {
            return new Vaccines(sql);
        }

        public string makeName(int nm, int sur, int sec, int grp, OneRabbit.RabbitSex sex)
        {
            return RabbitGetter.makeName(sql, nm, sur, sec, grp, sex);
        }

        public bool unblockName(int id)
        {
            return Names.unblockName(sql, id);
        }

        public Younger[] getSuckers(int mom)
        {
            return Youngers.getSuckers(sql, mom);
        }

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

        public LogList getLogs(Filters f)
        {
            return (new Logs(sql).getLogs(f));
        }

        public void addName(OneRabbit.RabbitSex sex, string name, string surname)
        {
            Names.addName(sql, sex, name, surname);
        }

        public void changeName(string orgName, string name, string surname)
        {
            Names.changeName(sql, orgName, name, surname);            
        }

        public void KillRabbit(int id, DateTime when, int reason, string notes)
        {
            RabbitGetter.killRabbit(sql, id, when, reason, notes);
        }

        public void СountKids(int rid, int dead, int killed, int added,int yid)
        {
            RabbitGetter.countKids(sql, rid, dead, killed, added, yid);
        }

        public void setRabbitSex(int rid, OneRabbit.RabbitSex sex)
        {
            RabbitGetter.setRabbitSex(sql, rid, sex);
        }

        public int cloneRabbit(int rid, int count, int farm, int tier, int sec, OneRabbit.RabbitSex sex, int mom)
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

        public void resurrect(int rid)
        {
            new DeadHelper(sql).resurrect(rid);
        }

        public void placeSucker(int sucker, int mother)
        {
            RabbitGetter.placeSucker(sql, sucker, mother);
        }

        public void combineGroups(int rabfrom, int rabto)
        {
            RabbitGetter.combineGroups(sql, rabfrom, rabto);
        }

        public Rabbit[] getMothers(int age, int agediff)
        {
            return RabbitGetter.getMothers(sql, age, agediff);
        }
#region zoo_tech_get
        public ZooJobItem[] ztGetVudvors(Filters f)
        {
            return new ZooTehGetter(sql, f).getVudvors(f);
        }
        public ZooJobItem[] ztGetOkrols(Filters f)
        {
            return new ZooTehGetter(sql, f).getOkrols(f.safeInt("okrol"));
        } 
        public ZooJobItem[] ztGetPreokrols(Filters f, int days, int okroldays)
        {
            return new ZooTehGetter(sql, f).getPreokrols(days, okroldays);
        }
        public ZooJobItem[] ztGetCounts(Filters f, int days, int next)
        {
            return new ZooTehGetter(sql, f).getCounts(days, next);
        }

        public ZooJobItem[] ztGetBoysGirlsOut(Filters f,int days, OneRabbit.RabbitSex sex)
        {
            return new ZooTehGetter(sql,f).getBoysGirlsOut(days, sex);
        }

        public ZooJobItem[] ztGetBoysByOne(Filters f)
        {
            return new ZooTehGetter(sql, f).getBoysByOne(f.safeInt("bbone"));
        }

        public ZooJobItem[] ztGetZooFuck(Filters f, int statedays, int firstdays, int brideage, int malewait, bool heter, bool inbr, int type)
        {
            return new ZooTehGetter(sql, f).getZooFucks(statedays, firstdays, brideage, malewait, heter, inbr, type);
        }

        public ZooJobItem[] ztGetVacc(Filters f)
        {
            return new ZooTehGetter(sql, f).getVacc(f);
        }

        public ZooJobItem[] ztGetSetNest(Filters f, int wochild, int wchild)
        {
            return new ZooTehGetter(sql, f).getSetNest(wochild, wchild);
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
        public Building[] getFreeBuilding(Filters f)
        {
            return Buildings.getFreeBuildings(sql, f);
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

        public string[] logNames()
        {
            return new Logs(sql).logNames();
        }

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
            return RabbitGetter.GetRabVac( sql,rabId);
        }

        public void SetRabbitVaccine(int rid, int vid,DateTime date)
        {
            RabbitGetter.SetRabbitVaccine(sql,rid,vid,date);
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
			return RabbitGenGetter.GetRabbit(sql, rid);
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
#region butcher
        public IDataGetter getButcherDates(Filters f)
        {
            return new Butcher(sql, f);
        }
        public List<OneRabbit> getVictims(DateTime dt)
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
        public void addPLUSummary(int prodid, string prodname, int tsell, int tsumm, int tweight, DateTime cleared)
        {
            Scale.addPLUSummary(sql, prodid, prodname, tsell, tsumm, tweight, cleared);
        }

        public List<ScalePLUSummary> getPluSummarys(DateTime date)
        {
            return Scale.getPluSummarys(sql, date);
        }

        public void deletePLUsummary(int sid, DateTime lastClear)
        {
            Scale.DeletePLUsumary(sql, sid, lastClear);
        }
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

        public DateTime GetFarmStartTime()
        {
            return Logs.getFarmStartTime(sql);
        }
        #endregion IRabNetDataLayer Members
    }        
}
