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

        public Building getBuilding(int tier)
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

        #region IRabNetDataLayer Members


        public void RabNetLog(int type, int user, string text)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public Fucks getFucks(int rabbit)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public Fucks allFuckers(int female)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public void setBon(int rabbit, string bon)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public void makeFuck(int female, int male, DateTime date)
        {
            throw new NotImplementedException();
        }

        public void makeProholost(int female, DateTime when)
        {
            throw new NotImplementedException();
        }

        public void makeOkrol(int female, DateTime when, int children, int dead)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public IZones getZones()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public string makeName(int nm, int sur, int sec, int grp, OneRabbit.RabbitSex sex)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public Younger[] getSuckers(int mom)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public Building[] getFreeBuilding(Filters f)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public void replaceRabbit(int rid, int farm, int tier_id, int sec)
        {
            throw new NotImplementedException();
        }

        public void replaceYounger(int yid, int farm, int tier_id, int sec)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public int newRabbit(OneRabbit r, int mom)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public void RabNetLog(int type, int user, int r1, int r2, string a1, string a2, string text)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public LogList getLogs(Filters f)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public ZooJobItem[] getOkrols(int days)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public void updateBuilding(Building b)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IRabNetDataLayer Members


        public ZooJobItem[] getVudvors(int days)
        {
            throw new NotImplementedException();
        }

        public byte addName(string sex, string name, string surname)
        {
            throw new NotImplementedException();
        }

        public byte deleteName(string name)
        {
            throw new NotImplementedException();
        }

        public void changeName(string orgName, string orgSurname, string name, string surname)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
