using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
//using System.Windows.Forms;
using rabnet;

namespace db.mysql
{    

    public class Buildings : RabNetDataGetterBase
    {             

        internal Buildings(MySqlConnection sql, Filters filters):base(sql,filters){}

        internal static Building GetBuilding(MySqlDataReader rd,bool shr,bool rabbits)
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
            int scs = Building.GetRSecCount(tp);
            Building b = new Building(id, farm, tid, tp, Building.GetRName(tp, shr), dl,
                rd.GetString("t_notes"), (rd.GetInt32("t_repair") == 0 ? false : true), scs);
            List<string> ars = new List<string>();
            List<string> deps = new List<string>();
            List<int> bus = new List<int>();
            List<String> uses = new List<string>();
            for (int i = 0; i < b.secs(); i++)
            {
                ars.Add((tid == 0 ? "" : (tid == 1 ? "^" : "-")) + Building.GetRSec(tp, i, dl));
                deps.Add(Building.GetRDescr(tp, shr, i, dl));
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
            b.fnhcount = Building.GetRNHCount(tp);
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
                return GetBuilding(rd, shr,true);
            }
            catch (Exception ex)
            {
                _logger.Error("building exception ", ex);
                throw ex;
            }
        }

        internal String makeWhere()
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
t_repair,
t_notes,
t_busy1,t_busy2,t_busy3,t_busy4,
rabname(t_busy1," + nm + @") r1, rabname(t_busy2," + nm + @") r2,rabname(t_busy3," + nm + @") r3,rabname(t_busy4," + nm + @") r4
FROM minifarms,tiers WHERE (m_upper=t_id OR m_lower=t_id) "+makeWhere()+"ORDER BY m_id;";
        }

        public override string countQuery()
        {
            return "SELECT COUNT(t_id) FROM minifarms,tiers WHERE (m_upper=t_id OR m_lower=t_id)" + makeWhere() + ";";
        }

        internal static TreeData getTree(int parent,MySqlConnection con,TreeData par)
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
                String nm=rd.GetString(1);
                int frm = rd.GetInt32(2);
                TreeData dt=new TreeData(String.Format("{0:d}:{1:d}:{2:s}",rd.GetInt32(0), frm,(frm==0?nm:"№"+Building.Format(nm.Remove(0,1)))) );
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

        internal static Building getTier(int tier,MySqlConnection con)
        {
            MySqlCommand cmd=new MySqlCommand(@"SELECT t_id,m_upper,m_lower,m_id,t_type,t_delims,t_nest,t_heater,
t_repair,coalesce(t_notes,'') t_notes,t_busy1,t_busy2,t_busy3,t_busy4,
rabname(t_busy1,1) r1, rabname(t_busy2,1) r2,rabname(t_busy3,1) r3,rabname(t_busy4,1) r4
FROM minifarms,tiers WHERE (m_upper=t_id OR m_lower=t_id) and t_id=" + tier.ToString()+";",con);
            MySqlDataReader rd = cmd.ExecuteReader();
            Building b=null;
            if (rd.Read())
                b = GetBuilding(rd, false,true);
            rd.Close();
            return b;
        }

        internal static Building[] getFreeBuildings(MySqlConnection sql,Filters f)
        {
            List<Building> bld = new List<Building>();
            String type = "";
            if (f.safeValue("tp") != "")
                type = "t_type='" + f.safeValue("tp") + "' AND ";
            if (f.ContainsKey("nest"))
                type = String.Format("(t_type='{0}' OR t_type='{1}' OR t_type='{2}') AND",BuildingType.Jurta,BuildingType.Female,BuildingType.DualFemale);
            
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
WHERE (m_upper=t_id OR m_lower=t_id) AND t_repair=0 AND "+busy+" ORDER BY m_id;", sql);
            _logger.Debug("free Buildings cmd:"+cmd.CommandText);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
                bld.Add(GetBuilding(rd, false, false) as Building);
            rd.Close();
            return bld.ToArray();
        }

        internal static void updateBuilding(Building b,MySqlConnection sql)
        {
            MySqlCommand cmd=new MySqlCommand(String.Format(@"UPDATE tiers SET t_repair={1:d},t_delims='{2:s}',t_heater='{3:s}',t_nest='{4:s}' WHERE t_id={0:d};",
                b.fid,b.frepair?1:0,b.fdelims,b.fheaters,b.fnests),sql);
            cmd.ExecuteNonQuery();
        }

        internal static void setBuildingName(MySqlConnection sql, int bid, String name)
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
        internal static void addBuilding(MySqlConnection sql, int parent, String name,int farm)
        {
            
            int frm = farm;
            MySqlCommand cmd = new MySqlCommand(String.Format(@"INSERT INTO buildings(b_name,b_parent,b_level,b_farm) VALUES(
'{0:s}',{1:d},{3:s},{2:d});",name,parent,frm,(parent==0?"0":String.Format("(SELECT b2.b_level+1 FROM buildings b2 WHERE b2.b_id={0:d})",parent))), sql);
            _logger.Debug("db.mysql.Building: "+cmd.CommandText);
            cmd.ExecuteNonQuery();
        }

        internal static void setLevel(MySqlConnection sql,int bid,int level)
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

        internal static void replaceBuilding(MySqlConnection sql, int bid, int toBuilding)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT b_level FROM buildings WHERE b_id={0:d};",toBuilding), sql);
            MySqlDataReader rd;
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

        internal static void deleteBuilding(MySqlConnection sql, int bid)
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
        internal static int addNewTier(MySqlConnection sql, String type)
        {
            if (type == "none") return 0;
            string hn = "0";
            string delims = "1";
            string bcols = "";
            string bvals = "";

            MySqlCommand cmd = new MySqlCommand("",sql);
            switch (type)
            {                           
                case BuildingType.Quarta: delims = "111"; break;

                case BuildingType.Complex:
                    delims = "11";
                    bcols = ",t_busy1,t_busy2,t_busy3,t_busy4";
                    bvals = ",0,0,0,null";
                    break;

                case BuildingType.Barin:
                case BuildingType.DualFemale:  
                case BuildingType.Vertep:
                case BuildingType.Jurta:
                    if (type == BuildingType.DualFemale) hn = "00";
                    if (type == BuildingType.Jurta) delims = "0";
                    bcols = ",t_busy1,t_busy2,t_busy3,t_busy4";
                    bvals = ",0,0,null,null";
                    break;

                case BuildingType.Female:
                case BuildingType.Cabin:
                    delims = "0";
                    bcols = ",t_busy1,t_busy2,t_busy3,t_busy4";
                    bvals = ",0,null,null,null";
                    break;          
            }
            cmd.CommandText=String.Format(@"INSERT INTO tiers(t_type,t_delims,t_heater,t_nest,t_notes{3:s}) 
VALUES('{0:s}','{1:s}','{2:s}','{2:s}',''{4:s});", type, delims, hn,bcols,bvals);
            _logger.Debug("db.mysql.Building: "+cmd.CommandText);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }

        private static void changeTierType(MySqlConnection sql, int tid, String type)
        {
            String hn = "00";
            String delims = "000";
            String busy = "";
            //if (type == "quarta") delims = "111"; 
            //if (type == "barin") delims = "100";
            switch (type)
            {
                case BuildingType.Quarta:                    
                        delims = "111";
                        busy = getBusyString(4);
                        break;

                case BuildingType.Complex:
                        hn = "0";
                        delims = "11";
                        busy = getBusyString(3);
                        break;

                case BuildingType.Barin:                    
                        delims = "100";
                        busy = getBusyString(2); 
                        break;
                   
                case BuildingType.Vertep:
                case BuildingType.DualFemale:
                case BuildingType.Jurta:                    
                        busy = getBusyString(2);
                        break;
                    
                case BuildingType.Female:                    
                        busy = getBusyString(1);
                        break;
                    

            }
            MySqlCommand cmd = new MySqlCommand(String.Format(@"UPDATE tiers SET t_type='{0:s}',
t_delims='{1:s}',t_heater='{2:s}',t_nest='{2:s}'{4:s} WHERE t_id={3:d};", type, delims, hn,tid,busy), sql);
            cmd.ExecuteNonQuery();
        }

        /// <param name="count">Сколько клеток доступны для заселения</param>
        internal static string getBusyString(byte count)
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
        internal static int addFarm(MySqlConnection sql,int parent,String uppertype, String lowertype, String name,int id)
        {
            int frm = id;
            int t1 = addNewTier(sql,uppertype);
            int t2 = addNewTier(sql, lowertype);
            int res = 0;
            MySqlCommand cmd =new MySqlCommand(String.Format("INSERT INTO minifarms(m_upper,m_lower{2:s}) VALUES({0:d},{1:d}{3:s});",
                t1,t2,(frm==0?"":",m_id"),(frm==0?"":String.Format(",{0:d}",frm))),sql);
            _logger.Debug("db.mysql.Building: "+cmd.CommandText);
            cmd.ExecuteNonQuery();
            res = (int)cmd.LastInsertedId;

            addBuilding(sql, parent, (name != "" ? name : String.Format("№{0:d}", res)), res);
            return res;
        }
        /// <summary>
        /// Существует ли миниферма
        /// </summary>
        /// <param name="id">Номер минифермы</param>
        internal static bool farmExists(MySqlConnection sql, int id)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT COUNT(*) FROM minifarms WHERE m_id={0:d};",id),sql);
            if (cmd.ExecuteScalar().ToString() == "0") return false;
            else return true;
        }

        internal static bool hasRabbits(MySqlConnection sql,int tid)
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

        internal static int[] getTiersFromFarm(MySqlConnection sql, int fid)
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

        internal static void changeFarm(MySqlConnection sql, int fid, String uppertype, String lowertype)
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

        internal static void deleteFarm(MySqlConnection sql,int fid)
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

        internal static int GetMFCount(MySqlConnection sql)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT COUNT(*) FROM buildings WHERE b_farm<>0;"), sql);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }
    }
}
