using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class TreeData
    {
        protected const char PATH_SPLITTER = '.';
        //public String caption;
        public readonly int ID;
        public readonly string Name;
        
        protected string _pPath = null;

        protected TreeData(int id,string name)
        {
            this.ID = id;
            this.Name = name;
        }
        protected TreeData() { }

        public string Path
        {
            get
            {
                if (_pPath == null) return ID.ToString();
                return String.Concat(_pPath, PATH_SPLITTER, ID.ToString());
            }
        }
    }

    public class RabTreeData : TreeData
    {
        public readonly int BreedId = 0;      
        public readonly int NameId = 0;
        public readonly DateTime BirthDay;

        public String BreedShortName ="";
        public String Bon = "";
        public RabTreeData Mother;
        public RabTreeData Father;
        public int BirthPlace;
        public RabAliveState State;

        public RabTreeData(int rId,string name,int nameId,DateTime born, int breedId):base(rId,name)
        {
            this.NameId = nameId;
            this.BirthDay = born;
            this.BreedId = breedId;
        }

        public String NameCombined
        {
            get
            {
                return String.Format("{0:s}, {1:d},{2:s}", Name,Age, Bon);
            }
        }

        public virtual int Age { get { return DateTime.Now.Subtract(BirthDay).Days; } }
        

        public String NameFormat(string format)
        {
            return format.Replace("B", BreedId.ToString())
                .Replace("b", BreedShortName)
                .Replace("N", NameId.ToString())
                .Replace("n", Name)
                .Replace("A", Age.ToString())
                .Replace("C",Bon);
        }
    }

    public class BldTreeData:TreeData
    {
        public int TierID;
        public List<BldTreeData> ChildNodes;

        public BldTreeData(int bldId, int tierId, string name, string pPath):base(bldId,name)
        {
            this.TierID = tierId;
            _pPath = pPath;
        }
        public BldTreeData(int bldId, int tierId, string name)
            : this(bldId, tierId, name, null) { }
    }
}
