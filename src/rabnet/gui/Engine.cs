using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
