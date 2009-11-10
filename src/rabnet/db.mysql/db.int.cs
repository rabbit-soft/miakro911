using System;
using System.Collections.Generic;

namespace rabnet
{
    public class ExDBDriverNotFoud : Exception
    {
        public ExDBDriverNotFoud(String driver) : base("Database Driver " + driver + " not found!") { }
    }

    public interface IRabNetDataLayer
    {
        void init(String connectionString);
        void close();
        List<String> getUsers();
        int checkUser(String name, String password);
    }

}