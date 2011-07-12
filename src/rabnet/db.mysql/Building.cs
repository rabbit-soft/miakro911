using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;


namespace rabnet
{
    public static class myBuildingType
    {
        public const string Female = "female";
        public const string Female_Rus = "Крольчихин";
        public const string Female_Short = "крлч";

        public const string DualFemale = "dfemale";
        public const string DualFemale_Rus = "Двукрольчихин";
        public const string DualFemale_Short = "2крл";

        public const string Jurta = "jurta";
        public const string Jurta_Rus = "Юрта";
        public const string Jurta_Short = "юрта";

        public const string Quarta = "quarta";
        public const string Quarta_Rus = "Кварта";
        public const string Quarta_Short = "кврт";

        public const string Vertep = "vertep";
        public const string Vertep_Rus = "Вертеп";
        public const string Vertep_Short = "вртп";

        public const string Barin = "barin";
        public const string Barin_Rus = "Барин";
        public const string Barin_Short = "барн";

        public const string Cabin = "cabin";
        public const string Cabin_Rus = "Хижина";
        public const string Cabin_Short = "хижн";

        public const string Complex = "complex";
        public const string Complex_Rus = "Комплексный";
        public const string Complex_Short = "кмпл";
    }

    public class Building:IData
    {
        public int fid;
        public int ffarm;
        public int ftid;
        public int fsecs;
        public String[] fareas;
        public String[] fdeps;
        public string ftype;
        public string ftypeloc;
        public string fdelims;
        public string fnotes;
        public bool frepair;
        public string fnests;
        public string fheaters;
        public string faddress;
        public string[] fuses;
        public int[] fbusies;
        public int fnhcount;
        public string[] fullname=new string[4];
        public string[] smallname = new string[4];
        public string[] medname = new string[4];
        public Building(int id, int farm, int tier_id, string type, string typeloc, string delims, string notes, bool repair, int seccnt)
        {
            fid = id;
            ffarm = farm;
            ftid = tier_id;
            ftype=type;
            ftypeloc = typeloc;
            fdelims=delims;
            fnotes = notes;
            frepair = repair;
            fsecs = seccnt;
            for (int i = 0; i < fsecs; i++)
            {
                fullname[i] = Buildings.fullRName(ffarm, ftid, i, ftype, fdelims, false, true, true);
                smallname[i] = Buildings.fullRName(ffarm, ftid, i, ftype, fdelims, true, false, false);
                medname[i] = Buildings.fullRName(ffarm, ftid, i, ftype, fdelims, false, true, false);
            }
        }
        #region IBuilding Members
        public int id() { return fid; }
        public int farm() { return ffarm; }
        public int tier_id() { return ftid; }
        public string delims(){return fdelims;}
        public string type(){return ftypeloc;}
        public string itype(){return ftype;}
        public string notes() { return fnotes; }
        public bool repair() { return frepair; }
        public int secs() { return fsecs; }
        public string area(int id) { return fareas[id]; }
        public string dep(int id) { return fdeps[id]; }
        public string nest() { return fnests; }
        public string heater() { return fheaters; }
        public int nest_heater_count() { return fnhcount; }
        public int busy(int id) { return fbusies[id]; }
        public string use(int id) { return fuses[id]; }
        public string address() { return faddress; }
        #endregion

    }

    class Buildings : RabNetDataGetterBase
    {     
        public static bool hasnest(String type,int sec,String nests)
        {
            int c = getRNHCount(type);
            if (c == 0) return false;
            if (type==myBuildingType.DualFemale)
                return (nests[sec]=='1');
            return (nests[0]=='1');
        }

        public static String getRDescr(String type, bool shr,int sec,String delims)
        {
            String res = "";
            switch (type)
            {
                case myBuildingType.Female:
                case myBuildingType.DualFemale: res = shr ? "гн+выг" : "гнездовое+выгул"; break;
                case myBuildingType.Complex: if (sec==0)
                        res = shr ? "гн+выг" : "гнездовое+выгул";
                    else
                        res = shr ? "отк" : "откормочное"; 
                    break;
                case myBuildingType.Jurta : if (sec == 0)
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
                case myBuildingType.Cabin:
                case myBuildingType.Quarta: res = shr ? "отк" : "откормочное"; break;
                case myBuildingType.Vertep :
                case myBuildingType.Barin: res = shr ? "врт" : "Вертеп"; break;
            }
            return res;
        }
        public static String getRSec(String type, int sec, String delims)
        {
            if (type == myBuildingType.Female)
                return "";
            String secnames = "абвг";
            String res = ""+secnames[sec];
            if (type == myBuildingType.Quarta && delims!="111")
            {
                for (int i = sec - 1; i >= 0 && (delims[i]=='1'); i--)
                    if (delims[i] == '0') res = secnames[i] + res;
                for (int i = sec; i < 3 && delims[i] == '1'; i++)
                    if (delims[i] == '0') res = res + secnames[i + 1];
            }
            else if (type == myBuildingType.Barin && delims[0]=='0') 
                res = "аб";
            return res;
        }
        public static String getRName(String type,bool shr)
        {
            String res="Нет";
            switch (type)
            {
                case myBuildingType.Female: res = shr ? myBuildingType.Female_Short : myBuildingType.Female_Rus; break;
                case myBuildingType.DualFemale: res = shr ? myBuildingType.DualFemale_Short : myBuildingType.DualFemale_Rus; break;
                case myBuildingType.Complex: res = shr ? myBuildingType.Complex_Short : myBuildingType.Complex_Rus; break;
                case myBuildingType.Jurta : res = shr ? myBuildingType.Jurta_Short : myBuildingType.Jurta_Rus; break;
                case myBuildingType.Quarta: res = shr ? myBuildingType.Quarta_Short : myBuildingType.Quarta_Rus; break;
                case myBuildingType.Vertep: res = shr ? myBuildingType.Vertep_Short : myBuildingType.Vertep_Rus; break;
                case myBuildingType.Barin: res = shr ? myBuildingType.Barin_Short : myBuildingType.Barin_Rus; break;
                case myBuildingType.Cabin: res = shr ? myBuildingType.Cabin_Short : myBuildingType.Cabin_Rus; break;
            }
            return res;
        }

        /// <summary>
        /// Возвращает количество секций у данного типа МИНИфермы
        /// </summary>
        public static int getRSecCount(String type)
        {
            int res = 2;
            switch (type)
            {
                case myBuildingType.Cabin:
                case myBuildingType.Female: res = 1; break;
                case myBuildingType.Complex: res = 3; break;
                case myBuildingType.Quarta: res = 4; break;
            }
            return res;
        }
        public static int getRNHCount(String type)
        {
            int res = 1;
            switch (type)
            {
                case myBuildingType.DualFemale: res = 2; break;
                case myBuildingType.Quarta: 
                case myBuildingType.Vertep:
                case myBuildingType.Barin: res = 0; break;
            }
            return res;
        }

        public static String fullRName(int farm, int tierid, int sec, String type, String delims, bool shr, bool sht, bool sho)
        {
            String res = String.Format("{0,4:d}",farm);
            if (tierid == 1) res += "^";
            if (tierid == 2) res += "-";
            res += getRSec(type, sec, delims);
            if (sht)
                res += " [" + getRName(type, shr) + "]";
            if (sho)
                res += " (" + getRDescr(type, shr, sec, delims)+")";
            return res;
        }

        public static String fullPlaceName(String rabplace,bool shr,bool sht,bool sho)
        {
            if (rabplace == "")
                return OneRabbit.NullAddress;
            String[] dts = rabplace.Split(',');
            return fullRName(int.Parse(dts[0]),int.Parse(dts[1]),int.Parse(dts[2]),dts[3],dts[4],shr,sht,sho);
        }
        public static String fullPlaceName(String rabplace)
        {
            return fullPlaceName(rabplace, false, false, false);
        }

        public static bool hasnest(String rabplace)
        {
            if (rabplace == "")
                return false;
            String[] dts = rabplace.Split(',');
            return hasnest(dts[3], int.Parse(dts[2]), dts[5]);
        }

        public Buildings(MySqlConnection sql, Filters filters):base(sql,filters){}

        public static Building getBuilding(MySqlDataReader rd,bool shr,bool rabbits)
        {
            int id = rd.GetInt32(0);
            int farm = rd.GetInt32(3);
            int tid = 0;
            if (rd.GetInt32(2) != 0)
            {
                if (rd.GetInt32(1) == id)
                    tid = 1;
                else tid = 2;
            }
            String tp = rd.GetString("t_type");
            String dl = rd.GetString("t_delims");
            int scs = getRSecCount(tp);
            Building b = new Building(id, farm, tid, tp, getRName(tp, shr), dl,
                rd.GetString("t_notes"), (rd.GetInt32("t_repair") == 0 ? false : true), scs);
            List<string> ars = new List<string>();
            List<string> deps = new List<string>();
            List<int> bus = new List<int>();
            List<String> uses = new List<string>();
            for (int i = 0; i < b.secs(); i++)
            {
                ars.Add((tid == 0 ? "" : (tid == 1 ? "^" : "-")) + getRSec(tp, i, dl));
                deps.Add(getRDescr(tp, shr, i, dl));
                int rn = (rd.IsDBNull(10+i) ? -1 : rd.GetInt32("t_busy" + (i + 1).ToString()));
                bus.Add(rn);
                if (rabbits)
                    uses.Add(rd.GetString("r" + (i + 1).ToString()));
                else
                    uses.Add("");
            }
            b.fareas = ars.ToArray();
            b.fbusies = bus.ToArray();
            b.fdeps = deps.ToArray();
            b.fuses = uses.ToArray();
            b.fnhcount = getRNHCount(tp);
            b.fnests = rd.GetString("t_nest");
            b.fheaters = rd.GetString("t_heater");
            b.faddress = "";
            return b;
        }

        public override IData nextItem()
        {
            try
            {
                bool shr = options.safeBool("shr");
                return getBuilding(rd, shr,true);
            }
            catch (Exception ex)
            {
                log.Error("building exception ", ex);
                throw ex;
            }
        }

        public String makeWhere()
        {
            
            String res = "";
            if (options.ContainsKey("frm"))
            {               
                String sres = "";
                if (options["frm"] == "1")
                    sres = "t_busy1<>0 OR t_busy2<>0 OR t_busy3<>0 OR t_busy4<>0";
                else sres = "t_busy1=0 OR t_busy2=0 OR t_busy3=0 OR t_busy4=0";
                res = "("+sres+")" ;                
            }

            if (options.ContainsKey("yar"))
            {
                String sres = "";
                if (options.safeValue("yar").Contains("v")) sres = addWhereOr(sres, "t_type='vertep'");
                if (options.safeValue("yar").Contains("u")) sres = addWhereOr(sres, "t_type='jurta'");
                if (options.safeValue("yar").Contains("q")) sres = addWhereOr(sres, "t_type='quarta'");
                if (options.safeValue("yar").Contains("b")) sres = addWhereOr(sres, "t_type='barin'");
                if (options.safeValue("yar").Contains("k")) sres = addWhereOr(sres, "t_type='female'");
                if (options.safeValue("yar").Contains("d")) sres = addWhereOr(sres, "t_type='dfemale'");
                if (options.safeValue("yar").Contains("x")) sres = addWhereOr(sres, "t_type='complex'");
                if (options.safeValue("yar").Contains("h")) sres = addWhereOr(sres, "t_type='cabin'");
                res = addWhereAnd(res, "(" + sres + ")");
            }

            if (options.ContainsKey("grlk"))
            {
                String sres = "";
                if (options["grlk"] == "1") sres = "t_heater='0' OR t_heater='00'";
                if (options["grlk"] == "2") sres = "t_heater='1' OR t_heater='3'";
                if (options["grlk"] == "3") sres = "t_heater='1'";
                if (options["grlk"] == "4") sres = "t_heater='3'";
                res = addWhereAnd(res, "(" + sres + ")");
            }

            if (options.ContainsKey("gnzd"))
            {
                String sres = "";
                if (options["gnzd"] == "1") 
                    sres = "t_nest<>'1'";
                else sres = "t_nest='1'";
                res = addWhereAnd(res, "(" + sres + ")");
            }

            if(res !="") res= "AND "+res;
            
            return res;

        }
        /// <summary>
        /// Строка запроса для вкладки "Строения"
        /// </summary>
        /// <returns>Возвращает запрос, который выполняется объектом класса RabNetDataGetterBase.</returns>
        public override string getQuery()
        {
            string nm = "1";
            if (options.safeBool("dbl"))
                nm = "2";
            return @"SELECT t_id,m_upper,m_lower,m_id,t_type,t_delims,t_nest,t_heater,
t_repair,t_notes,t_busy1,t_busy2,t_busy3,t_busy4,
rabname(t_busy1," + nm + @") r1, rabname(t_busy2," + nm + @") r2,rabname(t_busy3," + nm + @") r3,rabname(t_busy4," + nm + @") r4
FROM minifarms,tiers WHERE (m_upper=t_id OR m_lower=t_id) "+makeWhere()+"ORDER BY m_id;";
        }

        public override string countQuery()
        {
            return "SELECT COUNT(t_id) FROM minifarms,tiers WHERE (m_upper=t_id OR m_lower=t_id)" + makeWhere() + ";";
        }

        public static TreeData getTree(int parent,MySqlConnection con,TreeData par)
        {
            MySqlCommand cmd = new MySqlCommand(@"SELECT b_id,b_name,b_farm FROM buildings WHERE b_parent="+parent.ToString()+" ORDER BY b_farm ASC;", con);
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
                int frm = rd.GetInt32(2);
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


        public static Building getTier(int tier,MySqlConnection con)
        {
            MySqlCommand cmd=new MySqlCommand(@"SELECT t_id,m_upper,m_lower,m_id,t_type,t_delims,t_nest,t_heater,
t_repair,coalesce(t_notes,'') t_notes,t_busy1,t_busy2,t_busy3,t_busy4,
rabname(t_busy1,1) r1, rabname(t_busy2,1) r2,rabname(t_busy3,1) r3,rabname(t_busy4,1) r4
FROM minifarms,tiers WHERE (m_upper=t_id OR m_lower=t_id) and t_id=" + tier.ToString()+";",con);
            MySqlDataReader rd = cmd.ExecuteReader();
            Building b=null;
            if (rd.Read())
                b = getBuilding(rd, false,true);
            rd.Close();
            return b;
        }

        public static Building[] getFreeBuildings(MySqlConnection sql,Filters f)
        {
            List<Building> bld = new List<Building>();
            String type = "";
            if (f.safeValue("tp") != "")
                type = "t_type='" + f.safeValue("tp") + "' AND ";
            if (f.ContainsKey("nest"))
                type = String.Format("(t_type='{0}' OR t_type='{1}' OR t_type='{2}') AND",myBuildingType.Jurta,myBuildingType.Female,myBuildingType.DualFemale);
            String busy = "(("+type+"(t_busy1=0 OR t_busy2=0 OR t_busy3=0 OR t_busy4=0))";
            if (f.safeInt("rcnt") > 0)
                for (int i = 0; i < f.safeInt("rcnt"); i++)
                {
                    int r=f.safeInt("r"+i.ToString());
                    if (r > 0)
                        busy += String.Format(" OR (t_busy1={0:d} OR t_busy2={0:d} OR t_busy3={0:d} OR t_busy4={0:d})", r);
                }
            busy += ")";
            MySqlCommand cmd = new MySqlCommand(@"SELECT 
t_id,m_upper,m_lower,m_id,t_type,t_delims,t_nest,t_heater,t_repair,t_notes,t_busy1,t_busy2,t_busy3,t_busy4 
FROM minifarms,tiers 
WHERE (m_upper=t_id OR m_lower=t_id) AND t_repair=0 AND "+busy+";", sql);
            log.Debug("free Buildings cmd:"+cmd.CommandText);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
                bld.Add(getBuilding(rd, false, false) as Building);
            rd.Close();
            return bld.ToArray();
        }

        public static void updateBuilding(Building b,MySqlConnection sql)
        {
            MySqlCommand cmd=new MySqlCommand(String.Format(@"UPDATE tiers SET t_repair={1:d},t_delims='{2:s}',t_heater='{3:s}',t_nest='{4:s}' WHERE t_id={0:d};",
                b.fid,b.frepair?1:0,b.fdelims,b.fheaters,b.fnests),sql);
            cmd.ExecuteNonQuery();
        }

        public static void setBuildingName(MySqlConnection sql, int bid, String name)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE buildings SET b_name='{0:s}' WHERE b_id={1:d};",
                name,bid), sql);
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Добавляет ветку в дерево строений
        /// </summary>
        /// <param name="parent">Родитель</param>
        /// <param name="name">Имя ветки</param>
        /// <param name="farm">Номер фермы</param>
        public static void addBuilding(MySqlConnection sql, int parent, String name,int farm)
        {
            
            int frm = farm;
            MySqlCommand cmd = new MySqlCommand(String.Format(@"INSERT INTO buildings(b_name,b_parent,b_level,b_farm) VALUES(
'{0:s}',{1:d},{3:s},{2:d});",name,parent,frm,(parent==0?"0":String.Format("(SELECT b2.b_level+1 FROM buildings b2 WHERE b2.b_id={0:d})",parent))), sql);
            log.Debug("db.mysql.Building: "+cmd.CommandText);
            cmd.ExecuteNonQuery();
        }

        public static void setLevel(MySqlConnection sql,int bid,int level)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE buildings SET b_level={0:d} WHERE b_id={1:d};",level,bid), sql);
            cmd.ExecuteNonQuery();
            cmd.CommandText = String.Format(@"SELECT b_id FROM buildings WHERE b_parent={0:d};",bid);
            List<int> bs = new List<int>();
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
                bs.Add(rd.GetInt32(0));
            rd.Close();
            foreach (int b in bs)
                setLevel(sql, b, level + 1);
        }

        public static void replaceBuilding(MySqlConnection sql, int bid, int toBuilding)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT b_level FROM buildings WHERE b_id={0:d};",toBuilding), sql);
            MySqlDataReader rd;
            //TODO: here
            int level = 0;
            if (toBuilding != 0)
            {
                rd = cmd.ExecuteReader();
                if (rd.Read())
                    level = rd.GetInt32(0)+1;
                rd.Close();
            }
            cmd.CommandText = String.Format(@"UPDATE buildings SET b_parent={0:d} WHERE b_id={1:d};",toBuilding,bid);
            cmd.ExecuteNonQuery();
            setLevel(sql, bid, level );
        }

        public static void deleteBuilding(MySqlConnection sql, int bid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT COUNT(b_id) FROM buildings WHERE b_parent={0:d};",bid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int cnt = 1;
            if (rd.Read())
                cnt = rd.GetInt32(0);
            rd.Close();
            if (cnt == 0)
            {
                cmd.CommandText = String.Format("DELETE FROM buildings WHERE b_id={0:d};",bid);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Добавить новый Ярус
        /// </summary>
        /// <param name="type">Тип юрты</param>
        /// <returns>ID нового яруса</returns>
        public static int addNewTier(MySqlConnection sql, String type)
        {
            if (type == "none") return 0;
            string hn = "0";
            string delims = "1";
            string bcols = "";
            string bvals = "";

            MySqlCommand cmd = new MySqlCommand("",sql);
            switch (type)
            {                           
                case myBuildingType.Quarta: delims = "111"; break;

                case myBuildingType.Complex:
                    delims = "11";
                    bcols = ",t_busy1,t_busy2,t_busy3,t_busy4";
                    bvals = ",0,0,0,null";
                    break;

                case myBuildingType.Barin:
                case myBuildingType.DualFemale:  
                case myBuildingType.Vertep:
                case myBuildingType.Jurta:
                    if (type == myBuildingType.DualFemale) hn = "00";
                    if (type == myBuildingType.Jurta) delims = "0";
                    bcols = ",t_busy1,t_busy2,t_busy3,t_busy4";
                    bvals = ",0,0,null,null";
                    break;

                case myBuildingType.Female:
                case myBuildingType.Cabin:
                    delims = "0";
                    bcols = ",t_busy1,t_busy2,t_busy3,t_busy4";
                    bvals = ",0,null,null,null";
                    break;          
            }
            cmd.CommandText=String.Format(@"INSERT INTO tiers(t_type,t_delims,t_heater,t_nest,t_notes{3:s}) 
VALUES('{0:s}','{1:s}','{2:s}','{2:s}',''{4:s});", type, delims, hn,bcols,bvals);
            log.Debug("db.mysql.Building: "+cmd.CommandText);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }

        public static void changeTierType(MySqlConnection sql, int tid, String type)
        {
            String hn = "00";
            String delims = "000";
            String busy = "";
            //if (type == "quarta") delims = "111"; 
            //if (type == "barin") delims = "100";
            switch (type)
            {
                case myBuildingType.Quarta:
                    {
                        delims = "111";
                        busy = getBusyString(4);
                        break;
                    }
                case myBuildingType.Barin:
                    {
                        delims = "100";
                        busy = getBusyString(2); 
                        break;
                    }
                case myBuildingType.Vertep:
                case myBuildingType.DualFemale:
                case myBuildingType.Jurta:
                    {
                        busy = getBusyString(2);
                        break;
                    }
                case myBuildingType.Female:
                    {
                        busy = getBusyString(1);
                        break;
                    }
            }
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE tiers SET t_type='{0:s}',
t_delims='{1:s}',t_heater='{2:s}',t_nest='{2:s}'{4:s} WHERE t_id={3:d};", type, delims, hn,tid,busy), sql);
            cmd.ExecuteNonQuery();
        }

        public static string getBusyString(byte count)
        {
            string result ="";
            for (int i = 1; i <= 4;i++ )
            {
                if (i <= count) result += String.Format(",t_busy{0:s}=0", i.ToString());
                else result += String.Format(",t_busy{0:s}=NULL", i.ToString());
            }
            return result;
        }
        /// <summary>
        /// Добавляет новую МИНИферму
        /// </summary>
        /// <param name="parent">Ветка-родитель</param>
        /// <param name="uppertype">Тип верхнего яруса</param>
        /// <param name="lowertype">Тип нижнего яруса</param>
        /// <param name="name">Название (если блок)</param>
        /// <param name="id">Номер МИНИфермы</param>
        /// <returns>Номер новой клетки</returns>
        public static int addFarm(MySqlConnection sql,int parent,String uppertype, String lowertype, String name,int id)
        {
            int frm = id;
            int t1 = addNewTier(sql,uppertype);
            int t2 = addNewTier(sql, lowertype);
            int res = 0;
            MySqlCommand cmd =new MySqlCommand(String.Format("INSERT INTO minifarms(m_upper,m_lower{2:s}) VALUES({0:d},{1:d}{3:s});",
                t1,t2,(frm==0?"":",m_id"),(frm==0?"":String.Format(",{0:d}",frm))),sql);
            log.Debug("db.mysql.Building: "+cmd.CommandText);
            cmd.ExecuteNonQuery();
            res = (int)cmd.LastInsertedId;

            addBuilding(sql, parent, (name != "" ? name : String.Format("№{0:d}", res)), res);
            return res;
        }
        /// <summary>
        /// Существует ли миниферма
        /// </summary>
        /// <param name="id">Номер минифермы</param>
        public static bool farmExists(MySqlConnection sql, int id)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT COUNT(*) FROM minifarms WHERE m_id={0:d};",id),sql);
            if (cmd.ExecuteScalar().ToString() == "0") return false;
            else return true;
        }

        public static bool hasRabbits(MySqlConnection sql,int tid)
        {
            if (tid == 0) return false;
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT t_busy1,t_busy2,t_busy3,t_busy4 
FROM tiers WHERE t_id={0:d};",tid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            bool busy = true;
            if (rd.Read())
            {
                busy = false;
                if (rd.GetInt32(0) != 0) busy = true;
                if (!rd.IsDBNull(1) && rd.GetInt32(1) != 0) busy = true;
                if (!rd.IsDBNull(2) && rd.GetInt32(2) != 0) busy = true;
                if (!rd.IsDBNull(3) && rd.GetInt32(3) != 0) busy = true;
            }
            rd.Close();
            return busy;
        }

        public static int[] getTiersFromFarm(MySqlConnection sql, int fid)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT m_upper,m_lower FROM minifarms 
WHERE m_id={0:d};", fid), sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            int t1 = 0, t2 = 0;
            if (rd.Read())
            {
                t1 = rd.GetInt32(0);
                t2 = rd.GetInt32(1);
            }
            rd.Close();
            return new int[] { t1, t2 };
        }

        public static void changeFarm(MySqlConnection sql, int fid, String uppertype, String lowertype)
        {
            int[] t = getTiersFromFarm(sql, fid);
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT t_type FROM tiers WHERE t_id={0:d};",t[0]),sql);
            String t1 = "none";
            String t2 = "none";
            MySqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())t1 = rd.GetString(0);
            rd.Close();
            if (t[1] != 0)
            {
                cmd.CommandText = String.Format(@"SELECT t_type FROM tiers WHERE t_id={0:d};",t[1]);
                rd = cmd.ExecuteReader();
                if (rd.Read()) t2 = rd.GetString(0);
                rd.Close();
            }
            if (t1 != uppertype && !hasRabbits(sql, t[0]))
                changeTierType(sql, t[0], uppertype);
            if (t2 != lowertype && !hasRabbits(sql,t[1]))
            {
                if (lowertype == "none")
                {
                    cmd.CommandText = String.Format("DELETE FROM tiers WHERE t_id={0:d};",t[1]);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText=String.Format("UPDATE minifarms SET m_lower=0 WHERE m_id={0:d};",fid);
                    cmd.ExecuteNonQuery();
                }
                else if (t2 == "none")
                {
                    int nid = addNewTier(sql, lowertype);
                    cmd.CommandText = String.Format("UPDATE minifarms SET m_lower={0:d} WHERE m_id={1:d};",nid,fid);
                    cmd.ExecuteNonQuery();
                }
                else changeTierType(sql, t[1], lowertype);
            }
        }

        public static void deleteFarm(MySqlConnection sql,int fid)
        {
            int[] t = getTiersFromFarm(sql, fid);
            if (!hasRabbits(sql,t[0]) && !hasRabbits(sql,t[1]))
            {
                MySqlCommand cmd = new MySqlCommand(String.Format(@"DELETE FROM tiers WHERE t_id={0:d} OR t_id={1:d};",t[0],t[1]), sql);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("DELETE FROM minifarms WHERE m_id={0:d};",fid);
                cmd.ExecuteNonQuery();
                cmd.CommandText = String.Format("DELETE FROM buildings WHERE b_farm={0:d};", fid);
                cmd.ExecuteNonQuery();
            }
        }


        public static int GetMFCount(MySqlConnection sql)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT COUNT(*) FROM buildings WHERE b_farm<>0;"), sql);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }
    }
}
