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
        IBuilding getBuilding(int tier);
        IBreeds getBreeds();
        IDataGetter getNames(Filters filters);
        IDataGetter zooTeh(Filters f);
        OneRabbit getRabbit(int rid);
        void setRabbit(OneRabbit r);
        ICatalogs catalogs();
        void RabNetLog(int type, int user, String text);
        Fucks getFucks(int rabbit);
        Fucks allFuckers(int female);
        void setBon(int rabbit,String bon);
        void makeFuck(int female, int male,DateTime date);
        void makeProholost(int female, DateTime when);
        void makeOkrol(int female, DateTime when, int children, int dead);
    }

}