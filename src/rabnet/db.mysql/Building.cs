using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    class Building:IBuilding
    {
        public int fid;
        public int ffarm;
        public int ftid;
        public int fsecs;
        public String[] fareas;
        public Building(int id,int farm,int tier_id)
        {
            fid = id;
            ffarm = farm;
            ftid = tier_id;
            //fsecs=secs;
        }
        #region IBuilding Members
        public int id(){return fid;}
        public int farm(){return ffarm;}
        public int tier_id(){return ftid;}
        public string sname()
        {
            throw new NotImplementedException();
        }

        public string type()
        {
            throw new NotImplementedException();
        }

        public int itype()
        {
            throw new NotImplementedException();
        }

        public int busy(int id)
        {
            throw new NotImplementedException();
        }

        string IBuilding.busy(int id)
        {
            throw new NotImplementedException();
        }

        public int areas()
        {
            throw new NotImplementedException();
        }

        public int nests(int id)
        {
            throw new NotImplementedException();
        }

        public int heaters()
        {
            throw new NotImplementedException();
        }

        public string notes()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IBuilding Members


        public int ibusy(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
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
            int id=rd.GetInt32(0);
            int farm = rd.GetInt32(3);
            int tid = 0;
            if (!rd.IsDBNull(2))
            {
                if (rd.GetInt32(1) == id)
                    tid = 1;
                else tid = 2;
            }
            Building b=new Building(rd.GetInt32(0), farm, tid);
            bool shr = options.safeBool("shr");

            return b;
        }

        public override string getQuery()
        {
            return "SELECT t_id,m_upper,m_lower,m_id,t_type,t_delims,t_nest,t_heater,t_repair,t_notes FROM minifarms,tiers WHERE m_upper=t_id OR m_lower=t_id";
        }

        public override string countQuery()
        {
            return "SELECT COUNT(t_id) FROM minifarms,tiers WHERE m_upper=t_id OR m_lower=t_id;";
        }

        public static TreeData getTree(int parent,MySqlConnection con,TreeData par)
        {
            MySqlCommand cmd = new MySqlCommand(@"SELECT b_id,b_name,b_farm FROM buildings WHERE b_parent="+parent.ToString()+";", con);
            MySqlDataReader rd = cmd.ExecuteReader();
            TreeData res=par;
            if (par == null)
            {
                res = new TreeData();
                res.caption = "farm";
            }
            List<TreeData> lst = new List<TreeData>();
            while (rd.Read())
            {
                int id=rd.GetInt32(0);
                String nm=rd.GetString(1);
                int frm=rd.GetInt32(2);
                TreeData dt=new TreeData(id.ToString() + ":" + frm.ToString() + ":" + nm);
                lst.Add(dt);
            }
            rd.Close();
            if (lst.Count > 0)
            {
                foreach (TreeData td in lst)
                {
                    int id=int.Parse(td.caption.Split(':')[0]);
                    getTree(id, con, td);
                }
                res.items = lst.ToArray();
            }
            return res;
        }

    }
}
