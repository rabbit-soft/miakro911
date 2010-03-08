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

    public interface IData
    {
    }

    public interface IDataGetter
    {
        int getCount();
        int getCount2();
        void stop();
        IData getNextItem();
    }

    public interface IRabNetDataLayer
    {
        void init(String connectionString);
        void close();
        //ENVIRONMENT
        List<String> getUsers(bool wgroup,int uid);
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
        TreeData rabbitGenTree(int rabbit);
        TreeData buildingsTree();
        IDataGetter getYoungers(Filters filters);
        int[] getTiers(int farm);
        Building getBuilding(int tier);
        IBreeds getBreeds();
        IZones getZones();
        IDataGetter getNames(Filters filters);
        IDataGetter zooTeh(Filters f);
        OneRabbit getRabbit(int rid);
        void setRabbit(OneRabbit r);
        ICatalogs catalogs();
        void RabNetLog(int type, int user, int r1,int r2,string a1,string a2,String text);
        Fucks getFucks(int rabbit);
        Fucks allFuckers(int female,bool geterosis,bool inbreeding,int malewait);
        void setBon(int rabbit,String bon);
        void makeFuck(int female, int male,DateTime date,int worker);
        void makeProholost(int female, DateTime when);
        void makeOkrol(int female, DateTime when, int children, int dead);
        String makeName(int nm, int sur, int sec,int grp, OneRabbit.RabbitSex sex);
        Younger[] getSuckers(int mom);
        Building[] getFreeBuilding(Filters f);
        void replaceRabbit(int rid, int farm, int tier_id, int sec);
        void replaceYounger(int yid, int farm, int tier_id, int sec);
        int newRabbit(OneRabbit r,int mom);
        LogList getLogs(Filters f);
        ZooJobItem[] getOkrols(Filters f, int days);
        void updateBuilding(Building b);
        ZooJobItem[] getVudvors(Filters f, int days);
        void addName(OneRabbit.RabbitSex sex, string name, string surname);
        void changeName(string orgName, string name, string surname);
        void killRabbit(int id,DateTime when,int reason,string notes);
        void countKids(int rid, int dead, int killed, int added,int yid);
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
        XmlDocument makeReport(ReportType.Type type, Filters f);
        Rabbit[] getMothers(int age,int agediff);
        ZooJobItem[] getBoysGirlsOut(Filters f, int days, OneRabbit.RabbitSex sex);
        String[] logNames();
        ZooJobItem[] getZooFuck(Filters f, int statedays, int firstdays, int brideage, int malewait, bool heterosis, bool inbreeding,int type);
        void setBuildingName(int bid,String name);
        void addBuilding(int parent, String name);
        void replaceBuilding(int bid, int toBuilding);
        void deleteBuilding(int bid);
        int addFarm(int parent,String uppertype, String lowertype,String name,int id);
        void changeFarm(int fid,String uppertype,String lowertype);
        void deleteFarm(int fid);
        ZooJobItem[] getVacc(Filters f,int days);
        ZooJobItem[] getSetNest(Filters f,int wochild,int wchild);
        IDeadReasons getDeadReasons();
        String[] getWeights(int rabbit);
        void addWeight(int rabbit, int weight, DateTime date);
        void deleteWeight(int rabbit, DateTime date);
        OneRabbit[] getParents(int rabbit,int age);
        OneRabbit getLiveDeadRabbit(int rabbit);
        double[] getMaleChildrenProd(int male);

		RabbitGen getRabbitGen(int rid);
		Dictionary<int, Color> getBreedColors();
    }

}