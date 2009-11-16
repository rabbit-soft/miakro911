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

    public interface IRabbit : IData
    {
        int id();
        string name();
        string surname();
        string secname();
        string sex();
    }

    public interface IBuilding : IData
    {
        int id();
        string name();
        string type();
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
        String getFilterNames(String type);
        String getFilter(String type, String name);
        String setFilter(String type, String name, String filter);
        //DATA PROCEDURES
        IDataGetter getRabbits(String filters);
        IDataGetter getBuildings(String filters);


    }

}