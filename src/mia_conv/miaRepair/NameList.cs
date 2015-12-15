using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace mia_conv
{
    class NameList : List<repName>
    {
        internal void LoadNames(MySqlCommand cmd)
        {
            miaRepair.log("fill all names");
            cmd.CommandText = String.Format("SELECT n_id,n_sex,n_name,n_use,COALESCE(n_block_date,'0001-01-01')dt FROM names WHERE n_use<>0 ORDER BY n_use ASC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read()) {
                this.Add(new repName(rd.GetInt32("n_id"), rd.GetString("n_sex"), rd.GetString("n_name"), rd.GetInt32("n_use"), rd.GetDateTime("dt")) );
            }
            rd.Close();
            miaRepair.log(" |name count: {0:d}", this.Count);
        }

        internal int GetSurnameUse(int surname)
        {
            foreach (repName n in this) {
                if (n.nameSex == Sex.Female && n.nID == surname) {
                    return n.useRabbit;
                }
            }
            return 0;
        }

        internal int GetSecnameUse(int secname)
        {
            foreach (repName n in this) {
                if (n.nameSex == Sex.Male && n.nID == secname) {
                    return n.useRabbit;
                }
            }
            return 0;
        }
    }

    class repName
    {
        internal readonly int nID;
        internal readonly Sex nameSex;
        internal readonly string NameStr;
        internal int useRabbit;
        internal readonly bool Blocked = false;
        internal readonly DateTime BlockDate;

        internal repName(int id, string sex, string name, int use, DateTime block)
        {
            this.nID = id;
            switch (sex) {
                case "male": this.nameSex = Sex.Male; break;
                case "female": this.nameSex = Sex.Female; break;
                case "void": this.nameSex = Sex.Void; break;
            }
            this.NameStr = name;
            this.useRabbit = use;
            if (block != DateTime.MinValue) {
                this.Blocked = true;
            }
            this.BlockDate = block;
        }
    }

}
