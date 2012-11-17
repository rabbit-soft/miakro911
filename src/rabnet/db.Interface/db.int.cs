using System;
using System.Collections.Generic;
using System.Xml;
using System.Drawing;

namespace rabnet
{
    public class ExDBDriverNotFoud : Exception
    {
        public ExDBDriverNotFoud(String driver) : base("Database Driver " + driver + " not found!") { }
    }
    public class ExDBBadVersion : Exception
    {
        public ExDBBadVersion(int need,int has) : base(String.Format(@"Не верная версия базы данных {0:d}.
Требуется версия {1:d}.
Обновите программу и базу данных до последних версий.",has,need)) { }
    }

    /// <summary>
    /// Интерфейс который представляет собой одну строку IDataGetter
    /// </summary>
    public interface IData
    {
    }

    /// <summary>
    /// Интерфейс который представляет собой результат(MySqlDataReader) для заполнения информацией одну из панелей.
    /// </summary>
    public interface IDataGetter
    {
        int getCount();
        int getCount2();
        int getCount3(); //+gambit
        float getCount4(); //
        void stop();
        IData getNextItem();
    }

    public interface IRabNetDataLayer
    {
        void Init(String connectionString);
        void Close();
        //ENVIRONMENT
        List<sUser> GetUsers();
        sUser getUser(int uid);
        int checkUser(String name, String password);
        String getOption(String name, String subname, uint uid);
        void setOption(String name, String subname, uint uid, String value);
        DateTime now();
        String[] getFilterNames(String type);
        Filters getFilter(String type, String name);
        void setFilter(String type, String name, Filters filter);
        
        /// <summary>
        /// Получает число имеющихся МИНИферм
        /// </summary>
        /// <returns></returns>
        int getMFCount();
        string GetRabGenoms(int rId);
        RabTreeData rabbitGenTree(int rabbit);
        BldTreeData buildingsTree();       
        YoungRabbit[] GetYoungers(int momId);
        int[] getTiers(int farm);
        Building getBuilding(int tier);       
        List<sMeat> getMeats(DateTime date);
        /// <summary>
        /// Список Молодняка
        /// </summary>
        IDataGetter GetYoungers(Filters filters);
        IDataGetter getRabbits(Filters filters);
        IDataGetter getBuildings(Filters filters);
        IDataGetter getNames(Filters filters);
        IDataGetter zooTeh(Filters f);
        IDataGetter getButcherDates(Filters f);
        IDataGetter getDead(Filters filters);
        List<String> getButcherMonths();
        List<String> getFuckMonths();
        void changeDeadReason(int rid, int reason);
        List<String> getDeadsMonths();
        OneRabbit GetRabbit(int rid);
        void SetRabbit(OneRabbit r);
        ICatalogs catalogs();
        void RabNetLog(int type, int user, int r1, int r2, string a1, string a2, String text);
        Fucks getFucks(int rabbit);
        void cancelFuckEnd(int fuckID);
        Fucks GetAllFuckers(int female,bool geterosis,bool inbreeding,int malewait);
        void setBon(int rabbit,String bon);
        /// <summary>
        /// Случает крольчиху
        /// </summary>
        /// <param name="femaleId">ID крольчихи</param>
        /// <param name="maleId">ID самца</param>
        /// <param name="date">Дата случки</param>
        /// <param name="worker">ID пользователя (работника)</param>
		/// <param name="syntetic);">Искусственное осеменение</param>
        void MakeFuck(int femaleId, int maleId, DateTime date, int worker, bool syntetic);
        void makeProholost(int female, DateTime when);
        int makeOkrol(int female, DateTime when, int children, int dead);
        String makeName(int nm, int sur, int sec, int grp, Rabbit.SexType sex);
        bool unblockName(int id);

        /// <summary>
        /// Получает список всободных клеток. 
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        Building[] getFreeBuilding(Filters f);
        void replaceRabbit(int rid, int farm, int tier_id, int sec);
        void replaceYounger(int yid, int farm, int tier_id, int sec);
        int newRabbit(OneRabbit r,int mom);
        LogList getLogs(Filters f);
        void ArchLogs();
        void updateBuilding(Building b);
        void addName(Rabbit.SexType sex, string name, string surname);
        void changeName(string orgName, string name, string surname);

        /// <summary>
        /// Списание кролика
        /// </summary>
        /// <param name="id">ID кролика</param>
        /// <param name="when">Дата списания</param>
        /// <param name="reason">Причина списания</param>
        /// <param name="notes">Заметки</param>
        void KillRabbit(int id,DateTime when,int reason,string notes);

        /// <summary>
        /// Подсчет Гнездовых
        /// </summary>
        /// <param name="rid">ID кормилицы</param>
        /// <param name="dead">Количество погибших</param>
        /// <param name="killed">Количество Затоптоных</param>
        /// <param name="added">Прибавилось</param>
        /// <param name="yid">К какой группе подсосных</param>
        void СountKids(int rid, int dead, int killed, int added,int yid);       
        void setRabbitSex(int rid,Rabbit.SexType sex);
        int cloneRabbit(int rid, int count, int farm, int tier, int sec, Rabbit.SexType sex, int mom);
        string userGroup(int uid);
        void deleteUser(int uid);
        void changeUser(int uid, string name, int group, string password, bool chpass);
        bool hasUser(string name);
        int addUser(string name, int group, string password);
        
        void resurrect(int rid);       
        void placeSucker(int sucker, int mother);
        void combineGroups(int rabfrom, int rabto);
        XmlDocument makeReport(myReportType type, Filters f);
        XmlDocument makeReport(string query);
        AdultRabbit[] getMothers(int age,int agediff);        
        String[] logNames();        
        void setBuildingName(int bid,String name);
        void addBuilding(int parent, String name);
        void replaceBuilding(int bid, int toBuilding);
        void deleteBuilding(int bid);
        int addFarm(int parent,String uppertype, String lowertype,String name,int id);
        bool FarmExists(int id);
        void ChangeFarm(int fid,String uppertype,String lowertype);
        void deleteFarm(int fid);             
        String[] getWeights(int rabbit);

        RabVac[] GetRabVac(int rabId);
        List<Vaccine> GetVaccines();
        Vaccine GetVaccine(int vid);
        int AddVaccine(string name, int duration, int age, int after, bool zoo,int times);
        void EditVaccine(int id, string name, int duration, int age, int after, bool zoo,int times);

        void addWeight(int rabbit, int weight, DateTime date);
        void deleteWeight(int rabbit, DateTime date);
        OneRabbit[] getParents(int rabbit,int age);
        OneRabbit getLiveDeadRabbit(int rabbit);
        double[] getMaleChildrenProd(int male);
        void changeFucker(int fid, int fucker);
        void changeWorker(int fid, int worker);
		RabbitGen getRabbitGen(int rid);
		Dictionary<int, Color> getBreedColors();
        Rabbit[] GetVictims(DateTime dt);

        //zooTech
        ZootehJob[] GetZooTechJobs(Filters f, JobType type);

        //catalogs
        ICatalog getDeadReasons();      
        ICatalog getBreeds();
        ICatalog getZones();
        ICatalog getProductTypes();
        //ICatalog getVaccines();

        //for buther
        List<sMeal> getMealPeriods();
        void addMealIn(DateTime start, int amount);
        void addMealOut(DateTime start, int amount);
        void deleteMeal(int id);

        //for scale
        List<ScalePLUSummary> getPluSummarys(DateTime date);
        void addPLUSummary(int prodid, string prodname, int tsell, int tsumm, int tweight, DateTime cleared);
        void deletePLUsummary(int sid,DateTime lastClear);

        //for webreports
        string WebReportGlobal(DateTime dt);
        string[] WebReportsGlobal(DateTime dt,int days);
        DateTime GetFarmStartTime();

        void SetRabbitVaccine(int rid, int vid,DateTime date);
        void SetRabbitVaccine(int rid, int vid);

        IRabNetDataLayer Clone();        
    }

}