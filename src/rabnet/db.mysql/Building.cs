using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    class Building:IBuilding
    {
        int sid;
        string sname;
        string stype;
        public Building(int id,string name,string type)
        {
            sid = id;
            sname = name;
            stype = type;
        }
        public int id() { return sid; }
        public string name() { return sname; }
        public string type() { return stype; }
    }

    class Buildings : RabNetDataGetterBase
    {
        public static String getRDescr(String type, bool shr,int sec,String delims)
        {
            String res = "";
            switch (type)
            {
                case "female":
                case "dfemale": res = shr ? "гн+выг" : "гнездовое+выгул"; break;
                case "complex": if (sec==0)
                        res = shr ? "гн+выг" : "гнездовое+выгул";
                    else
                        res = shr ? "отк" : "откормочное"; 
                    break;
                case "jurta": if (sec == 0)
                    {
                        if (delims[0] == '0')
                            res = (shr ? "гн" : "гнездовое")+"+";
                        res += shr ? "мвг" : "м.выгул";
                    }
                    else
                    {
                        if (delims[0] == '1')
                            res = (shr ? "гн" : "гнездовое") + "+";
                        res += shr ? "бвг" : "б.выгул";
                    }
                    break;

                case "quarta": res = shr ? "отк" : "откормочное"; break;
                case "vertep":
                case "barin": res = shr ? "врт" : "Вертеп"; break;
                case "cabin": if (sec == 0)
                        res = shr ? "гн" : "гнездовое";
                    else
                        res = shr ? "отк" : "откормочное";
                    break;
            }
            return res;
        }
        public static String getRSec(String type, int sec, String delims)
        {
            if (type == "female")
                return "";
            String secnames = "абвг";
            String res = ""+secnames[sec];
            if (type == "quarta" && delims!="111")
            {
                for (int i = sec - 1; i >= 0 && (delims[i]=='1'); i--)
                    if (delims[i] == '0') res = secnames[i] + res;
                for (int i = sec; i < 3 && delims[i] == '1'; i++)
                    if (delims[i] == '0') res = res + secnames[i + 1];
            }
            else if (type == "barin" && delims[0]=='0') 
                res = "аб";
            return res;
        }
        public static String getRName(String type,bool shr)
        {
            String res="Нет";
            switch (type)
            {
                case "female": res=shr?"крлч":"Крольчихин"; break;
                case "dfemale": res = shr ? "2крл" : "Двукрольчихин"; break;
                case "complex": res = shr ? "кмпл" : "Комплексный"; break;
                case "jurta": res = shr ? "юрта" : "Юрта"; break;
                case "quarta": res = shr ? "кврт" : "Кварта"; break;
                case "vertep": res = shr ? "вртп" : "Вертеп"; break;
                case "barin": res = shr ? "барн" : "Барин"; break;
                case "cabin": res = shr ? "хижн" : "Хижина"; break;
            }
            return res;
        }
        public static String fullRName(int farm, int tierid, int sec,String type, String delims, bool shr, bool sht, bool sho)
        {
            String res = farm.ToString();
            if (tierid == 1) res += "-";
            if (tierid == 2) res += "^";
            res += getRSec(type, sec, delims);
            if (sht)
                res += " [" + getRName(type, shr) + "]";
            if (sho)
                res += " (" + getRDescr(type, shr, sec, delims)+")";
            return res;
        }

        public Buildings(MySqlConnection sql, Filters filters):base(sql,filters)
        {
        }

        public override IData nextItem()
        {
            return new Building(rd.GetInt32(0), rd.GetString(1), rd.GetString(2));
        }

        public override string getQuery()
        {
            return "SELECT m_id,t_id,t_type FROM minifarms,tiers WHERE m_upper=t_id OR m_lower=t_id";
        }

        public override string countQuery()
        {
            return "SELECT COUNT(t_id) FROM minifarms,tiers WHERE m_upper=t_id OR m_lower=t_id;";
        }
    }
}
