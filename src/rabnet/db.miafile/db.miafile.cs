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
        public List<String> getUsers(bool wgroup,int uid)
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

        public DateTime now()
        {
            throw new NotImplementedException();
        }

        public string[] getFilterNames(string type)
        {
            throw new NotImplementedException();
        }

        public Filters getFilter(string type, string name)
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

        public TreeData rabbitGenTree(int rabbit)
        {
            throw new NotImplementedException();
        }

        public TreeData buildingsTree()
        {
            throw new NotImplementedException();
        }

        public IDataGetter getYoungers(Filters filters)
        {
            throw new NotImplementedException();
        }

        public int[] getTiers(int farm)
        {
            throw new NotImplementedException();
        }

        public Building getBuilding(int tier)
        {
            throw new NotImplementedException();
        }

        public IBreeds getBreeds()
        {
            throw new NotImplementedException();
        }

        public IZones getZones()
        {
            throw new NotImplementedException();
        }

        public IDataGetter getNames(Filters filters)
        {
            throw new NotImplementedException();
        }

        public IDataGetter zooTeh(Filters f)
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

        public void RabNetLog(int type, int user, int r1, int r2, string a1, string a2, string text)
        {
            throw new NotImplementedException();
        }

        public Fucks getFucks(int rabbit)
        {
            throw new NotImplementedException();
        }

        public Fucks allFuckers(int female)
        {
            throw new NotImplementedException();
        }

        public void setBon(int rabbit, string bon)
        {
            throw new NotImplementedException();
        }

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

        public string makeName(int nm, int sur, int sec, int grp, OneRabbit.RabbitSex sex)
        {
            throw new NotImplementedException();
        }

        public Younger[] getSuckers(int mom)
        {
            throw new NotImplementedException();
        }

        public Building[] getFreeBuilding(Filters f)
        {
            throw new NotImplementedException();
        }

        public void replaceRabbit(int rid, int farm, int tier_id, int sec)
        {
            throw new NotImplementedException();
        }

        public void replaceYounger(int yid, int farm, int tier_id, int sec)
        {
            throw new NotImplementedException();
        }

        public int newRabbit(OneRabbit r, int mom)
        {
            throw new NotImplementedException();
        }

        public LogList getLogs(Filters f)
        {
            throw new NotImplementedException();
        }

        public ZooJobItem[] getOkrols(int days)
        {
            throw new NotImplementedException();
        }

        public void updateBuilding(Building b)
        {
            throw new NotImplementedException();
        }

        public ZooJobItem[] getVudvors(int days)
        {
            throw new NotImplementedException();
        }

        public void addName(OneRabbit.RabbitSex sex, string name, string surname)
        {
            throw new NotImplementedException();
        }

        public void changeName(string orgName, string name, string surname)
        {
            throw new NotImplementedException();
        }

        public void killRabbit(int id, DateTime when, int reason, string notes)
        {
            throw new NotImplementedException();
        }

        public void countKids(int rid, int dead, int killed, int added)
        {
            throw new NotImplementedException();
        }

        public ZooJobItem[] getCounts(int days)
        {
            throw new NotImplementedException();
        }

        public void setRabbitSex(int rid, OneRabbit.RabbitSex sex)
        {
            throw new NotImplementedException();
        }

        public int cloneRabbit(int rid, int count, int farm, int tier, int sec, OneRabbit.RabbitSex sex, int mom)
        {
            throw new NotImplementedException();
        }

        public string userGroup(int uid)
        {
            throw new NotImplementedException();
        }

        public void deleteUser(int uid)
        {
            throw new NotImplementedException();
        }

        public void changeUser(int uid, string name, int group, string password, bool chpass)
        {
            throw new NotImplementedException();
        }

        public bool hasUser(string name)
        {
            throw new NotImplementedException();
        }

        public int addUser(string name, int group, string password)
        {
            throw new NotImplementedException();
        }

        public IDataGetter getDead(Filters filters)
        {
            throw new NotImplementedException();
        }

        public void resurrect(int rid)
        {
            throw new NotImplementedException();
        }

        public ZooJobItem[] getPreokrols(int days)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
