using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using rabnet;
using log4net;

namespace rabnet
{
    public class RabNetDBMiaFile:IRabNetDataLayer
    {
        private FileStream fs = null;
        private ILog log = LogManager.GetLogger(typeof(RabNetDBMiaFile));
        public RabNetDBMiaFile() { }
        public RabNetDBMiaFile(String connectionString):this()
        {
            init(connectionString);
        }
        ~RabNetDBMiaFile()
        {
            close();
        }
        #region IRabNetDataLayer Members

        public void close()
        {
            if (fs!=null)
            {
                fs.Close();
                fs = null;
            }
        }
        public void init(String connectionString)
        {
            close();
            fs = new FileStream(connectionString, FileMode.Open, FileAccess.ReadWrite);
        }
        public List<String> getUsers()
        {
            return null;
        }

        public int checkUser(string name, string password)
        {
            return -1;
        }

        #endregion

        #region IRabNetDataLayer Members


        public string getOption(string name, string subname, uint uid)
        {
            throw new NotImplementedException();
        }

        public void setOption(string name, string subname, uint uid, string value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members

        void IRabNetDataLayer.init(string connectionString)
        {
            throw new NotImplementedException();
        }

        void IRabNetDataLayer.close()
        {
            throw new NotImplementedException();
        }

        List<string> IRabNetDataLayer.getUsers()
        {
            throw new NotImplementedException();
        }

        int IRabNetDataLayer.checkUser(string name, string password)
        {
            throw new NotImplementedException();
        }

        string IRabNetDataLayer.getOption(string name, string subname, uint uid)
        {
            throw new NotImplementedException();
        }

        void IRabNetDataLayer.setOption(string name, string subname, uint uid, string value)
        {
            throw new NotImplementedException();
        }

        DateTime IRabNetDataLayer.now()
        {
            return DateTime.Now;
        }


        public IDataGetter getBuildings(string filters)
        {
            throw new NotImplementedException();
        }


        public IDataGetter getRabbits(string filters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        string[] IRabNetDataLayer.getFilterNames(string type)
        {
            throw new NotImplementedException();
        }

        Filters IRabNetDataLayer.getFilter(string type, string name)
        {
            throw new NotImplementedException();
        }

        public void setFilter(string type, string name, Filters filter)
        {
            throw new NotImplementedException();
        }

        public IDataGetter getRabbits(Filters filters)
        {
            throw new NotImplementedException();
        }

        public IDataGetter getBuildings(Filters filters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public TreeData rabbitGenTree(int rabbit)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public TreeData buildingsTree()
        {
            throw new NotImplementedException();
        }

        public IDataGetter getYoungers(Filters filters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public int[] getTiers(int farm)
        {
            throw new NotImplementedException();
        }

        public IBuilding getBuilding(int tier)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public Breeds getBreeds()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public IDataGetter getNames(Filters filters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public IDataGetter zooTeh(Filters f)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        IBreeds IRabNetDataLayer.getBreeds()
        {
            throw new NotImplementedException();
        }

        public OneRabbit getRabbit(int rid)
        {
            throw new NotImplementedException();
        }

        public void setRabbit(OneRabbit r)
        {
            throw new NotImplementedException();
        }

        public ICatalogs catalogs()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
