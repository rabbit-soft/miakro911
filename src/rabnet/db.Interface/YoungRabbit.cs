using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class YoungRabbit : Rabbit, IData
    {
        //public String fe = "";
        public readonly int ParentId;
        public readonly string ParentName;
        public readonly int Neighbours;

        public YoungRabbit(int id, string name, string sex, DateTime born, String breedname, int group, String bon, string rawAddress, String notes, int pid, string pname, int nbrs)
            : base(id, name, sex, born, breedname, group, bon, rawAddress, notes)
        {
            ParentId = pid;
            ParentName = pname;
            Neighbours = nbrs;
        }

        //internal YoungRabbit(MySqlDataReader rd, Filters f)
        //    : this(rd.GetInt32("r_id"), rd.GetString("name"),
        //        rd.GetString("r_sex"), rd.GetDateTime("r_born"), rd.GetString("breed"),
        //        rd.GetInt32("r_group"), rd.GetString("r_bon"), rd.GetString("rplace"), rd.GetString("r_notes"),
        //        rd.GetInt32("r_parent"), rd.GetString("parent"), rd.GetInt32("neighbours"))
        //{
        //    if (f == null) return;
        //    s_short = f.safeBool(Filters.SHORT);
        //    s_adrTier = f.safeBool(Filters.SHOW_BLD_TIERS);
        //    s_adrDesc = f.safeBool(Filters.SHOW_BLD_DESCR);
        //}
        //internal YoungRabbit(MySqlDataReader rd) : this(rd, null) { }
    }
}
