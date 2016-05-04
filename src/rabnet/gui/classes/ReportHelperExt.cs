using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace rabnet
{
#if !DEMO
    static class ReportHelperExt
    {
        internal static XmlDocument rabToXml(OneRabbit er, OneRabbit or)
        {
            return rabToXml(er, or, null);
        }

        /// <summary>
        /// Нужно для отчета Племенное свидетельство
        /// </summary>
        /// <param name="er"></param>
        /// <param name="or"></param>
        /// <param name="hasdoc"></param>
        /// <returns></returns>
        internal static XmlDocument rabToXml(OneRabbit er, OneRabbit or, XmlDocument hasdoc)
        {
            XmlDocument doc = null;
            if (hasdoc == null)
            {
                doc = new XmlDocument();
                doc.AppendChild(doc.CreateElement("Rows"));
            }
            else
                doc = hasdoc;

            XmlElement rw = doc.CreateElement("Row");
            doc.DocumentElement.AppendChild(rw);
            if (er != null)
            {
                or = Engine.db().getLiveDeadRabbit(er.ID);
                if (hasdoc == null)
                {
                    ReportHelper.Append(rw, doc, "header", Engine.opt().getOption(Options.OPT_ID.SVID_HEAD));
                    ReportHelper.Append(rw, doc, "num", Engine.opt().getOption(Options.OPT_ID.NEXT_SVID));
                    ReportHelper.Append(rw, doc, "date", DateTime.Now.Date.ToShortDateString());
                    ReportHelper.Append(rw, doc, "director", Engine.opt().getOption(Options.OPT_ID.SVID_GEN_DIR));
                    //rw.AppendChild(doc.CreateElement("header")).AppendChild(doc.CreateTextNode(Engine.opt().getOption(Options.OPT_ID.SVID_HEAD)));
                    //rw.AppendChild(doc.CreateElement("num")).AppendChild(doc.CreateTextNode(Engine.opt().getOption(Options.OPT_ID.NEXT_SVID)));
                    //rw.AppendChild(doc.CreateElement("date")).AppendChild(doc.CreateTextNode(DateTime.Now.Date.ToShortDateString()));
                    //rw.AppendChild(doc.CreateElement("director")).AppendChild(doc.CreateTextNode(Engine.opt().getOption(Options.OPT_ID.SVID_GEN_DIR)));
                }
                else
                {
                    rw.AppendChild(doc.CreateElement("group")).AppendChild(doc.CreateTextNode(er.Group.ToString()));
                }
                
                ReportHelper.Append(rw, doc, "sex", er.Sex == Rabbit.SexType.MALE ? "male" : (er.Sex == Rabbit.SexType.FEMALE ? "female" : "void"));
                ReportHelper.Append(rw, doc, "class", Rabbit.GetFBon(er.Bon));
                ReportHelper.Append(rw, doc, "name", er.NameFull);
                ReportHelper.Append(rw, doc, "breed", er.BreedName);
                if (er.BirthPlace != 0)
                {
                    //todo по хорошему надо писать born_place кролику при рождении на данной ферме
                    ClientsList list = Engine.db().GetClients();
                    foreach (Client c in list)
                    {
                        if (c.ID == er.BirthPlace)
                        {
                            ReportHelper.Append(rw, doc, "born_place", c.Name);
                            break;
                        }
                    }
                }
                else 
#if PROTECTED
                    if (er.Zone != 0)
#endif
                {
                    Catalog zones = Engine.db().catalogs().getZones();
                    ReportHelper.Append(rw, doc, "born_place", zones[er.Zone]);
                }
#if PROTECTED
                else
                {

                    ReportHelper.Append(rw, doc, "born_place", RabGRD.GRD.Instance.GetClientName()); //todо не очень хорошо использовать обращения к ключу здесь
                }
#endif

                ReportHelper.Append(rw, doc, "born_date", er.BirthDay.ToShortDateString());
                ReportHelper.Append(rw, doc, "age", er.Age.ToString());
                ReportHelper.Append(rw, doc, "address", er.AddressSmall);
                ReportHelper.Append(rw, doc, "weight", or.FWeight().ToString());
                ReportHelper.Append(rw, doc, "weight_date", or.WeightDate.Date.ToShortDateString());
                ReportHelper.Append(rw, doc, "weight_age", or.WeightAge.ToString());
                ReportHelper.Append(rw, doc, "born", or.KidsOverAll.ToString());//сколько родила
                ReportHelper.Append(rw, doc, "okrol", or.Okrol.ToString());
                ReportHelper.Append(rw, doc, "genom", er.Genoms.Replace(' ', ','));
                ReportHelper.Append(rw, doc, "wclass", er.Bon_Weight);
                ReportHelper.Append(rw, doc, "bclass", er.Bon_Body);
                ReportHelper.Append(rw, doc, "hclass", er.Bon_Hair);
                ReportHelper.Append(rw, doc, "cclass", er.Bon_Color);
            }
            else if (or != null)
            {
                ReportHelper.Append(rw, doc, "sex", or.Sex == Rabbit.SexType.MALE ? "male" : "female");
                //ReportHelper.Append(rw, doc, "age", or.Status.ToString()+(or.Zone==1?"(списан)":""));
                ReportHelper.Append(rw, doc, "age", or.Age.ToString());
                ReportHelper.Append(rw, doc, "weight", or.FWeight());
                ReportHelper.Append(rw, doc, "class", Rabbit.GetFBon(or.Bon));
                ReportHelper.Append(rw, doc, "name", or.NameFull);
                ReportHelper.Append(rw, doc, "wclass", Rabbit.GetFBon("" + or.Bon[1]));
                ReportHelper.Append(rw, doc, "bclass", Rabbit.GetFBon("" + or.Bon[1]));
                ReportHelper.Append(rw, doc, "hclass", Rabbit.GetFBon("" + or.Bon[3]));
                ReportHelper.Append(rw, doc, "cclass", Rabbit.GetFBon("" + or.Bon[4]));
            }
            else
            {
                ReportHelper.Append(rw, doc, "sex", "none");
                ReportHelper.Append(rw, doc, "age", "");
                ReportHelper.Append(rw, doc, "weight", "");
                ReportHelper.Append(rw, doc, "class", "");
                ReportHelper.Append(rw, doc, "name", "");
                ReportHelper.Append(rw, doc, "wclass", "");
                ReportHelper.Append(rw, doc, "bclass", "");
                ReportHelper.Append(rw, doc, "hclass", "");
                ReportHelper.Append(rw, doc, "cclass", "");
            }
            return doc;
        }

        internal static XmlDocument[] GetRabbitPlem(int rId)
        {
            XmlDocument[] docs = new XmlDocument[7];
            RabNetEngRabbit r = Engine.get().getRabbit(rId);
            docs[0] = ReportHelperExt.rabToXml(r, null);
            OneRabbit[] p1 = Engine.db().getParents(r.ID, r.Age);
            docs[1] = ReportHelperExt.rabToXml(null, p1[0]);
            docs[2] = ReportHelperExt.rabToXml(null, p1[1]);
            OneRabbit[] p2;
            if (p1[0] != null) {
                p2 = Engine.db().getParents(p1[0].ID, p1[0].Age);
            } else {
                p2 = new OneRabbit[] { null, null };
            }

            docs[3] = ReportHelperExt.rabToXml(null, p2[0]);
            docs[4] = ReportHelperExt.rabToXml(null, p2[1]);
            if (p1[1] != null) {
                p2 = Engine.db().getParents(p1[1].ID, p1[1].Age);
            } else {
                p2 = new OneRabbit[] { null, null };
            }

            docs[5] = ReportHelperExt.rabToXml(null, p2[0]);
            docs[6] = ReportHelperExt.rabToXml(null, p2[1]);

            return docs;
        }
    }
#endif
}
