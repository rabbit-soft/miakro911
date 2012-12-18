using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class TreeData
    {
        protected const char PATH_SPLITTER = '.';
        //public String caption;
        public int ID;
        public string Name;
        
        protected string _pPath = null;

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
        public int BreedId = 0;
        public String BreedShortName ="";
        public int NameId = 0;
        public int Age = 0;
        public String Bon = "";
        public List<RabTreeData> Parents;

        //public RabTreeData(int id,string name,int bId,int age,string bon)
        //{
        //    ID = id;
        //    Name = name;
        //    BreedId = bId;
        //    Age = age;
        //    Bon = bon;
        //}

        public String NameCombined
        {
            get
            {
                return String.Format("{0:s}, {1:d},{2:s}", Name,Age, Bon);
            }
        }

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

        public BldTreeData(int bldId, int tierId, string name, string pPath)
        {
            this.ID = bldId;
            this.TierID = tierId;
            this.Name = name;
            _pPath = pPath;
        }
        public BldTreeData(int bldId, int tierId, string name)
            : this(bldId, tierId, name, null) { }
    }
}
