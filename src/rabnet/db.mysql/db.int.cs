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
        void init(String connectionString);
        void close();
        //ENVIRONMENT
        List<sUser> getUsers();
        sUser getUser(int uid);
        int checkUser(String name, String password);
        String getOption(String name, String subname, uint uid);
        void setOption(String name, String subname, uint uid, String value);
        DateTime now();
        String[] getFilterNames(String type);
        Filters getFilter(String type, String name);
        void setFilter(String type, String name, Filters filter);
        //DATA PROCEDURES
        IDataGetter getRabbits(Filters filters);
        IDataGetter getBuildings(Filters filters);
        /// <summary>
        /// Получает число имеющихся МИНИферм
        /// </summary>
        /// <returns></returns>
        int getMFCount();
        TreeData rabbitGenTree(int rabbit);
        TreeData buildingsTree();
        /// <summary>
        /// Список Молодняка
        /// </summary>
        IDataGetter getYoungers(Filters filters);
        int[] getTiers(int farm);
        Building getBuilding(int tier);
        IBreeds getBreeds();
        IZones getZones();
        IProducts getProductTypes();
        List<sMeat> getMeats(DateTime date);
        IDataGetter getNames(Filters filters);
        IDataGetter zooTeh(Filters f);
        IDataGetter getButcherDates(Filters f);
        List<String> getButcherMonths();
        List<String> getFuckMonths();
        void changeDeadReason(int rid, int reason);
        List<String> getDeadsMonths();
        OneRabbit getRabbit(int rid);
        void setRabbit(OneRabbit r);
        ICatalogs catalogs();
        void RabNetLog(int type, int user, int r1,int r2,string a1,string a2,String text);
        Fucks getFucks(int rabbit);
        void cancelFuckEnd(int fuckID);
        Fucks allFuckers(int female,bool geterosis,bool inbreeding,int malewait);
        void setBon(int rabbit,String bon);
        void makeFuck(int female, int male,DateTime date,int worker);
        void makeProholost(int female, DateTime when);
        int makeOkrol(int female, DateTime when, int children, int dead);
        String makeName(int nm, int sur, int sec,int grp, OneRabbit.RabbitSex sex);
        bool unblockName(int id);
        Younger[] getSuckers(int mom);
        Building[] getFreeBuilding(Filters f);
        void replaceRabbit(int rid, int farm, int tier_id, int sec);
        void replaceYounger(int yid, int farm, int tier_id, int sec);
        int newRabbit(OneRabbit r,int mom);
        LogList getLogs(Filters f);
        ZooJobItem[] getOkrols(Filters f);
        ZooJobItem[] getBoysByOne(Filters f);
        void updateBuilding(Building b);
        ZooJobItem[] getVudvors(Filters f);
        void addName(OneRabbit.RabbitSex sex, string name, string surname);
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
        ZooJobItem[] getCounts(Filters f, int days,int next);
        void setRabbitSex(int rid,OneRabbit.RabbitSex sex);
        int cloneRabbit(int rid, int count, int farm, int tier, int sec, OneRabbit.RabbitSex sex, int mom);
        string userGroup(int uid);
        void deleteUser(int uid);
        void changeUser(int uid, string name, int group, string password, bool chpass);
        bool hasUser(string name);
        int addUser(string name, int group, string password);
        IDataGetter getDead(Filters filters);
        void resurrect(int rid);
        ZooJobItem[] getPreokrols(Filters f, int days, int okroldays);
        void placeSucker(int sucker, int mother);
        void combineGroups(int rabfrom, int rabto);
        XmlDocument makeReport(myReportType type, Filters f);
        XmlDocument makeReport(string query);
        Rabbit[] getMothers(int age,int agediff);
        ZooJobItem[] getBoysGirlsOut(Filters f, int days, OneRabbit.RabbitSex sex);
        String[] logNames();
        ZooJobItem[] getZooFuck(Filters f, int statedays, int firstdays, int brideage, int malewait, bool heterosis, bool inbreeding,int type);
        void setBuildingName(int bid,String name);
        void addBuilding(int parent, String name);
        void replaceBuilding(int bid, int toBuilding);
        void deleteBuilding(int bid);
        int addFarm(int parent,String uppertype, String lowertype,String name,int id);
        bool FarmExists(int id);
        void changeFarm(int fid,String uppertype,String lowertype);
        void deleteFarm(int fid);
        ZooJobItem[] getVacc(Filters f);
        ZooJobItem[] getSetNest(Filters f,int wochild,int wchild);
        IDeadReasons getDeadReasons();
        String[] getWeights(int rabbit);
        void addWeight(int rabbit, int weight, DateTime date);
        void deleteWeight(int rabbit, DateTime date);
        OneRabbit[] getParents(int rabbit,int age);
        OneRabbit getLiveDeadRabbit(int rabbit);
        double[] getMaleChildrenProd(int male);
        void changeFucker(int fid, int fucker);
        void changeWorker(int fid, int worker);

		RabbitGen getRabbitGen(int rid);
		Dictionary<int, Color> getBreedColors();
        List<OneRabbit> getVictims(DateTime dt);
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

    }

}