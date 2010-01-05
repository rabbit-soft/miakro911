using System;
using System.Collections.Generic;

namespace rabnet
{
    public class ExDBDriverNotFoud : Exception
    {
        public ExDBDriverNotFoud(String driver) : base("Database Driver " + driver + " not found!") { }
    }

    public interface IData
    {
    }

    public interface IDataGetter
    {
        int getCount();
        void stop();
        IData getNextItem();
    }

    public interface IRabNetDataLayer
    {
        void init(String connectionString);
        void close();
        //ENVIRONMENT
        List<String> getUsers();
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
        Fucks allFuckers(int female);
        void setBon(int rabbit,String bon);
        void makeFuck(int female, int male,DateTime date);
        void makeProholost(int female, DateTime when);
        void makeOkrol(int female, DateTime when, int children, int dead);
        String makeName(int nm, int sur, int sec,int grp, OneRabbit.RabbitSex sex);
        Younger[] getSuckers(int mom);
        Building[] getFreeBuilding(Filters f);
        void replaceRabbit(int rid, int farm, int tier_id, int sec);
        void replaceYounger(int yid, int farm, int tier_id, int sec);
        int newRabbit(OneRabbit r,int mom);
        LogList getLogs(Filters f);
        ZooJobItem[] getOkrols(int days);
        void updateBuilding(Building b);
        ZooJobItem[] getVudvors(int days);
        void addName(OneRabbit.RabbitSex sex, string name, string surname);
        void changeName(string orgName, string name, string surname);
    }

}