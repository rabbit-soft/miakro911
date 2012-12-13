using System;
using System.Collections.Generic;
using System.Xml;
using System.Drawing;

namespace rabnet
{
    /// <summary>
    /// Интерфейс который представляет собой одну строку IDataGetter
    /// </summary>
    public interface IData { }

    /// <summary>
    /// Интерфейс который представляет собой результат(MySqlDataReader) для заполнения информацией одну из панелей.
    /// </summary>
    public interface IDataGetter
    {
        int getCount();
        int getCount2();
        int getCount3(); //+gambit
        float getCount4(); //
        /// <summary>
        /// Закрывает DataReader
        /// </summary>
        void Close();
        /// <summary>
        /// Получить следующую строчку
        /// </summary>
        IData GetNextItem();
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
        OneRabbit[] GetNeighbors(int rabId);
        int[] getTiers(int farm);
        Building getBuilding(int tier);       
        
        /// <summary>
        /// Список Молодняка
        /// </summary>
        IDataGetter GetYoungers(Filters filters);
        IDataGetter getRabbits(Filters filters);
        IDataGetter getBuildingsRows(Filters filters);
        IDataGetter getNames(Filters filters);
        IDataGetter zooTeh(Filters f);        
        IDataGetter getDead(Filters filters);        
        List<String> getFuckMonths();
        void changeDeadReason(int rid, int reason);
        List<String> getDeadsMonths();
        OneRabbit GetRabbit(int rid);
        void SetRabbit(OneRabbit r);
        ICatalogs catalogs();
        void RabNetLog(int type, int user, int r1, int r2, string a1, string a2, String text);
        Fucks GetFucks(Filters f);
        void CancelFuckEnd(int fuckID);
        //Fucks GetAllFuckers(int female,bool geterosis,bool inbreeding,int malewait);
        Fucks GetAllFuckers(Filters f);
        void setBon(int rabbit,String bon);

        /// <summary>
        /// Случает крольчиху
        /// </summary>
        /// <param name="femaleId">ID крольчихи</param>
        /// <param name="maleId">ID самца</param>
        /// <param name="daysPast">Дней прошло</param>
        /// <param name="worker">ID пользователя (работника)</param>
        /// <param name="syntetic);">Искусственное осеменение</param>
        /// <param name="syntetic">Искуственное осеменение</param>
        void MakeFuck(int femaleId, int maleId, int daysPast, int worker, bool syntetic);
        void makeProholost(int female, int daysPast);
        int makeOkrol(int female, int daysPast, int children, int dead);
        String makeName(int nm, int sur, int sec, int grp, Rabbit.SexType sex);
        bool unblockName(int id);

        Building[] getBuildings(Filters f);
        void replaceRabbit(int rid, int farm, int tier_id, int sec);
        void replaceYounger(int yid, int farm, int tier_id, int sec);
        int newRabbit(OneRabbit r,int mom);
                
        void updateBuilding(Building b);
        void addName(Rabbit.SexType sex, string name, string surname);
        void changeName(string orgName, string name, string surname);

        /// <summary>
        /// Списание кролика
        /// </summary>
        /// <param name="id">ID кролика</param>
        /// <param name="daysPast">Дней прошло</param>
        /// <param name="reason">Причина списания</param>
        /// <param name="notes">Заметки</param>
        void KillRabbit(int id, int daysPast,int reason,string notes);

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

        AdultRabbit[] getMothers(int age,int agediff);                 
        void setBuildingName(int bid,String name);
        void addBuilding(int parent, String name);
        void replaceBuilding(int bid, int toBuilding);
        void deleteBuilding(int bid);
        int addFarm(int parent,String uppertype, String lowertype,String name,int id);
        bool FarmExists(int id);
        void ChangeFarm(int fid,String uppertype,String lowertype);
        void deleteFarm(int fid);             
        String[] getWeights(int rabbit);

        // vaccines
        RabVac[] GetRabVac(int rabId);
        List<Vaccine> GetVaccines();
        Vaccine GetVaccine(int vid);
        void EditVaccine(int id, string name, int duration, int age, int after, bool zoo,int times);
#if !DEMO
        int AddVaccine(string name, int duration, int age, int after, bool zoo,int times);
#endif

        void addWeight(int rabbit, int weight, DateTime date);
        void deleteWeight(int rabbit, DateTime date);
        OneRabbit[] getParents(int rabbit,int age);
        OneRabbit getLiveDeadRabbit(int rabbit);
        double[] getMaleChildrenProd(int male);
        void changeFucker(int fid, int fucker);
        void changeWorker(int fid, int worker);
		RabbitGen getRabbitGen(int rid);
		Dictionary<int, Color> getBreedColors();       

        //zooTech
        ZootehJob[] GetZooTechJobs(Filters f, JobType type);

        //catalogs
        ICatalog getDeadReasons();      
        ICatalog getBreeds();
        ICatalog getZones();
        ICatalog getProductTypes();
        //ICatalog getVaccines();

        void SetRabbitVaccine(int rid, int vid, DateTime date);
        void SetRabbitVaccine(int rid, int vid);

        LogList getLogs(Filters f);

        Rabbit[] GetDescendants(int ascendantId);
#if !DEMO
        void ArchLogs();
        String[] logNames();
        

        IDataGetter getButcherDates(Filters f);
        List<String> getButcherMonths();
        List<sMeat> getMeats(DateTime date);
        AdultRabbit[] GetVictims(DateTime dt);

        XmlDocument makeReport(myReportType type, Filters f);
        XmlDocument makeReport(string query);

        //for buther
        List<sMeal> getMealPeriods();
        void addMealIn(DateTime start, int amount);
        void addMealOut(DateTime start, int amount);
        void deleteMeal(int id);

        //for scale
        //List<ScalePLUSummary> getPluSummarys(DateTime date);
        //void addPLUSummary(int prodid, string prodname, int tsell, int tsumm, int tweight, DateTime cleared);
        //void deletePLUsummary(int sid,DateTime lastClear);

        //for webreports
        string WebReportGlobal(DateTime dt);
        string[] WebReportsGlobal(DateTime dt,int days);
        DateTime GetFarmStartTime();      
#endif
        IRabNetDataLayer Clone();        
    }

}