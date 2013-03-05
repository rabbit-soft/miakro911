using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace rabnet
{
#if !DEMO
    public class RabExporter
    {
        private int _myClientId = 0;
        private string _myClientName;
        private string _myDbGuid = "";

        public RabExporter(int myClientId, string clientName, string myDbGuid)
        {
            _myClientId = myClientId;
            _myClientName = clientName;
            _myDbGuid = myDbGuid;
        }

        public string Export(List<OneRabbit> exportRab,List<OneRabbit> ascendants,BreedsList breeds,RabNamesList names)
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0","UTF-8","no"));
            XmlElement rootNode = doc.CreateElement("export_rabbits");
            
            ReportHelper.AppendAttribute(rootNode, doc, "clientId", _myClientId.ToString());
            ReportHelper.AppendAttribute(rootNode, doc, "clientName", _myClientName.ToString());
            ReportHelper.AppendAttribute(rootNode, doc, "dbGuid", _myDbGuid.ToString());
            ReportHelper.AppendAttribute(rootNode, doc, "fileGuid", Guid.NewGuid().ToString());

            XmlElement tmpNode = doc.CreateElement("exports");
            foreach (OneRabbit r in exportRab)
            {
                tmpNode.AppendChild(getRabXml(tmpNode,doc,r));
            }
            rootNode.AppendChild(tmpNode);

            tmpNode = doc.CreateElement("ascendants");
            foreach (OneRabbit r in ascendants)
            {
                tmpNode.AppendChild(getRabXml(tmpNode, doc, r));
            }
            rootNode.AppendChild(tmpNode);

            tmpNode = doc.CreateElement("breeds");
            foreach (Breed b in breeds)
            {
                tmpNode.AppendChild(getBreedXml(tmpNode, doc, b));
            }
            rootNode.AppendChild(tmpNode);

            tmpNode = doc.CreateElement("names");
            foreach (RabName n in names)
            {
                tmpNode.AppendChild(getNameXml(tmpNode, doc, n));
            }
            rootNode.AppendChild(tmpNode);

            doc.AppendChild(rootNode);
            return doc.InnerXml;
        }

        public Client GetExporterInfo(string data)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);
            XmlNode rootNode = doc.FirstChild.NextSibling;
            int fromClientId = 0;
            int.TryParse(rootNode.Attributes["clientId"].Value, out  fromClientId);
            string name = rootNode.Attributes["clientName"].Value;
            return new Client(fromClientId, name, "");
        }

        public string Import(string data, out List<OneRabbit> exportRab, out List<OneRabbit> ascendants, out BreedsList breeds, out RabNamesList names)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);

            exportRab = new List<OneRabbit>();
            ascendants = new List<OneRabbit>();
            breeds = new BreedsList();
            names = new RabNamesList();

            int fromClientId;           
            XmlNode rootNode = doc.FirstChild.NextSibling;
            int.TryParse(rootNode.Attributes["clientId"].Value, out  fromClientId);
            if (fromClientId == _myClientId && rootNode.Attributes["dbGuid"].Value == _myDbGuid)
                throw new RabNetException("Файл был экспортирован с этой же фермы");

            XmlNode tmpNode = rootNode.SelectSingleNode("exports");
            foreach (XmlNode child in tmpNode.ChildNodes)
            {
                exportRab.Add(fillOneRabbit(child, fromClientId));
            }

            tmpNode = rootNode.SelectSingleNode("ascendants");
            foreach (XmlNode child in tmpNode.ChildNodes)
            {
                ascendants.Add(fillOneRabbit(child, fromClientId));
            }

            tmpNode = rootNode.SelectSingleNode("breeds");
            foreach (XmlNode child in tmpNode.ChildNodes)
            {
                breeds.Add(fillBreed(child));
            }

            tmpNode = rootNode.SelectSingleNode("names");
            foreach (XmlNode child in tmpNode.ChildNodes)
            {
                names.Add(fillName(child));
            }

            return rootNode.Attributes["fileGuid"].Value;
        }

        public string GetFileGuid(XmlDocument doc)
        {
            XmlNode rootNode = doc.FirstChild.NextSibling;
            return rootNode.Attributes["clientId"].Value;
        }

        private XmlNode getNameXml(XmlElement tmpNode, XmlDocument doc, RabName n)
        {
            XmlElement oneName = doc.CreateElement("name");
            ReportHelper.Append(oneName, doc, "id", n.ID.ToString());
            ReportHelper.Append(oneName, doc, "name", n.Name);
            ReportHelper.Append(oneName, doc, "surname", n.Surname);
            ReportHelper.Append(oneName, doc, "sex", Rabbit.SexToString(n.Sex));
            return oneName;
        }

        private XmlNode getBreedXml(XmlElement tmpNode, XmlDocument doc, Breed b)
        {
            XmlElement oneBreed = doc.CreateElement("breed");
            ReportHelper.Append(oneBreed, doc, "id", b.ID.ToString());
            ReportHelper.Append(oneBreed, doc, "name", b.Name);
            ReportHelper.Append(oneBreed, doc, "short", b.ShortName);
            return oneBreed;
        }

        private XmlElement getRabXml(XmlElement rabs, XmlDocument doc, OneRabbit r)
        {
            XmlElement oneRab = doc.CreateElement("rabbit");
            ReportHelper.Append(oneRab, doc, "id", r.ID.ToString());
            ReportHelper.Append(oneRab, doc, "sex", Rabbit.SexToString(r.Sex));
            ReportHelper.Append(oneRab, doc, "name", r.NameID.ToString());
            ReportHelper.Append(oneRab, doc, "fullname", r.NameFull);
            ReportHelper.Append(oneRab, doc, "breed", r.BreedID.ToString());
            ReportHelper.Append(oneRab, doc, "breedname", r.BreedName);
            ReportHelper.Append(oneRab, doc, "surname", r.SurnameID.ToString());
            ReportHelper.Append(oneRab, doc, "secname", r.SecnameID.ToString());
            ReportHelper.Append(oneRab, doc, "mother", r.MotherID.ToString());
            ReportHelper.Append(oneRab, doc, "father", r.FatherID.ToString());
            ReportHelper.Append(oneRab, doc, "birthday", r.BirthDay.ToShortDateString());
            ReportHelper.Append(oneRab, doc, "birthplace", r.BirthPlace.ToString());
            ReportHelper.Append(oneRab, doc, "group", r.Group.ToString());
            ReportHelper.Append(oneRab, doc, "status", r.Status.ToString());
            ReportHelper.Append(oneRab, doc, "okrol", r.Okrol.ToString());
            ReportHelper.Append(oneRab, doc, "weight", r.FWeight());
            ReportHelper.Append(oneRab, doc, "weightDate", r.WeightDate.ToShortDateString());
            ReportHelper.Append(oneRab, doc, "bon", r.Bon);
            return oneRab;
        }       

        private RabName fillName(XmlNode child)
        {
            return new RabName(
                int.Parse(ReportHelper.GetChildNodeVal(child, "id")),
                ReportHelper.GetChildNodeVal(child, "name"),
                ReportHelper.GetChildNodeVal(child, "surname"),
                ReportHelper.GetChildNodeVal(child, "sex"));            
        }

        private Breed fillBreed(XmlNode child)
        {
            return new Breed(
                int.Parse(ReportHelper.GetChildNodeVal(child, "id")),
                ReportHelper.GetChildNodeVal(child, "name"),
                ReportHelper.GetChildNodeVal(child, "short"));
        }

        private OneRabbit fillOneRabbit(XmlNode oneRab,int exportFrom)
        {
            return new OneRabbit(
                int.Parse(ReportHelper.GetChildNodeVal(oneRab, "id")),
                ReportHelper.GetChildNodeVal(oneRab, "sex"),
                DateTime.Parse(ReportHelper.GetChildNodeVal(oneRab, "birthday")), 0, Rabbit.NULL_FLAGS,
                int.Parse(ReportHelper.GetChildNodeVal(oneRab, "name")),
                int.Parse(ReportHelper.GetChildNodeVal(oneRab, "surname")),
                int.Parse(ReportHelper.GetChildNodeVal(oneRab, "secname")), Rabbit.NULL_ADDRESS,
                int.Parse(ReportHelper.GetChildNodeVal(oneRab, "group")),
                int.Parse(ReportHelper.GetChildNodeVal(oneRab, "breed")), 0, "","",
                int.Parse(ReportHelper.GetChildNodeVal(oneRab, "status")), DateTime.MinValue, Fuck.GetFuckTypeStr(FuckType.None), DateTime.MinValue, 0, 0,
                ReportHelper.GetChildNodeVal(oneRab, "fullname"),
                ReportHelper.GetChildNodeVal(oneRab, "breedname"), 
                ReportHelper.GetChildNodeVal(oneRab, "bon"), 0,
                int.Parse(ReportHelper.GetChildNodeVal(oneRab, "okrol")),
                int.Parse(ReportHelper.GetChildNodeVal(oneRab, "weight")),
                DateTime.Parse(ReportHelper.GetChildNodeVal(oneRab, "weightDate")),
                int.Parse(ReportHelper.GetChildNodeVal(oneRab, "mother")),
                int.Parse(ReportHelper.GetChildNodeVal(oneRab, "father")), 
                exportFrom);
        }
    }

    public class OneImport
    {
        public int RabId;
        public DateTime Date;
        public int Count;
        public int Birthplace;
        public int OldId;
        public string FileGuid;

        public OneImport(DateTime date, int rabId, int count, int birthPlace, int oldid, string guid)
        {
            this.RabId = rabId;
            this.Date = date;
            this.Count = count;
            this.Birthplace = birthPlace;
            this.OldId = oldid;
            this.FileGuid = guid;
        }
    }
#endif
}
