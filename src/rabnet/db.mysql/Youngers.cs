using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public class Younger : IRabbit
    {
        public int fid;
        public String fname;
        public string fsex;
        public int fage;
        public String fbreed;
        public String fe = "";
        public String fcls;
        public string faddress;
        public string fnotes;
        public int fcount;
        public int fneighbours;
        public string mom;
        public int momid;
        public Younger(int id,String name,String sex,int age,String breed,String cls,int count,String notes)
        {
            fid = id;
            fname = name;
            fsex = sex;
            fage = age;
            fbreed = breed;
            fcls = cls;
            fnotes = notes;
            fcount = count;
        }
        #region IRabbit Members

        public int id(){return fid;}
        public string name() { return fname; }
        public string sex() { return fsex; }
        public int age() { return fage; }
        public string breed() { return fbreed; }
        public string weight(){return fe;}
        public string status() { return mom; }
        public string bgp() { return fe; }
        public string N() { return fcount.ToString(); }
        public int average() { return fneighbours; }
        public int rate() { return momid; }
        public string cls() { return fcls; }
        public string address() { return faddress; }
        public string notes() { return fnotes; }

        #endregion
    }

    class Youngers:RabNetDataGetterBase
    {
        public Youngers(MySqlConnection sql, Filters f)
            : base(sql, f)
        {
        }

        public override IData nextItem()
        {
            bool shr=options.safeBool("shr");
            Younger y = new Younger(rd.GetInt32("r_id"), rd.GetString("name"),
                Rabbits.getRSex(rd.GetString("r_sex")), rd.GetInt32("age"), rd.GetString("breed"),
                Rabbits.getBon(rd.GetString("r_bon"), shr), rd.GetInt32("r_group"), rd.GetString("r_notes"));
            y.fneighbours = rd.GetInt32("neighbours");
            y.mom = rd.GetString("parent");
            y.momid = rd.GetInt32("r_parent");
            y.faddress = Buildings.fullPlaceName(rd.GetString("rplace"), shr, options.safeBool("sht"), options.safeBool("sho"));
            return y;
        }

        public override string getQuery()
        {

            return @"SELECT r_id,
rabname(r_id,"+(options.safeBool("dbl")?"2":"1")+@") name,
r_group,r_sex,
(SELECT "+(options.safeBool("shr")?"b_short_name":"b_name")+@" FROM breeds WHERE b_id=r_breed) breed,
r_parent,
rabname(r_parent," + (options.safeBool("dbl") ? "2" : "1") + @") parent,
r_notes,TO_DAYS(NOW())-TO_DAYS(r_born) age,r_bon,
(SELECT SUM(rg.r_group)-rabbits.r_group FROM rabbits rg WHERE rg.r_parent=rabbits.r_parent) neighbours,
rabplace(r_parent) rplace
FROM rabbits,tiers WHERE r_parent!=0 AND r_tier=t_id ORDER BY name;";
        }

        public override string countQuery()
        {
            return "SELECT COUNT(*) FROM rabbits WHERE r_parent!=0;";
        }
    }
}
