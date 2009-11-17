using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;

namespace rabnet
{
    public class RabnetConfigHandler : IConfigurationSectionHandler
    {
        public class dataSource
        {
            public String name;
            public String type;
            public String param;
            public bool def=false;
            public String defuser="";
            public String defpassword="";
            public dataSource(String name, String type, String param)
            {
                this.name = name;
                this.type = type;
                this.param = param;
            }
        }
        public static List<dataSource> ds = new List<dataSource>();
        public object Create(object parent, object configContext, XmlNode section)
        {
            foreach (XmlNode cn in section.ChildNodes)
            {
                if (cn.Name == "dataSource")
                {
                    ds.Add(new dataSource(cn.Attributes.GetNamedItem("name").Value,
                        cn.Attributes.GetNamedItem("type").Value, cn.Attributes.GetNamedItem("param").Value));
                    dataSource td = ds[ds.Count - 1];
                    if (cn.Attributes.GetNamedItem("default") != null)
                    {
                        td.def = (cn.Attributes.GetNamedItem("default").Value == "1");
                    }
                    if (cn.Attributes.GetNamedItem("user") != null)
                    {
                        td.defuser = cn.Attributes.GetNamedItem("user").Value;
                    }
                    if (cn.Attributes.GetNamedItem("password") != null)
                    {
                        td.defpassword = cn.Attributes.GetNamedItem("password").Value;
                    }
                }
            }
            return section;
        }
    }



    public class Engine
    {
        private static RabNetEngine eng = null;
        public static RabNetEngine get()
        {
            if (eng==null)
                eng = new RabNetEngine();
            return eng;
        }
        public static IRabNetDataLayer db()
        {
            return get().db();
        }
        public static IRabNetDataLayer db2()
        {
            return get().db2();
        }
        public static Options opt()
        {
            return get().options();
        }
    }
}
