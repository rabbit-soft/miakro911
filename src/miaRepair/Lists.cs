using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace miaRepair
{
    class RabbitList : List<repRabbit>
    {
        private int _youngCount = 0;
        private int _adultCount = 0;

        internal int RabbitsCount
        {
            get { return this.Count; }
        }
        internal int YongersCount
        {
            get { return _youngCount; }
        }
        internal int AdultCount
        {
            get { return _adultCount; }
        }


        internal virtual void LoadContent(MySqlCommand cmd)
        {
            Program.log("Loading All Rabbits");
            cmd.CommandText = String.Format("SELECT r_id,r_mother,r_father,r_sex,r_name,r_surname,r_secname,r_born,r_parent,COALESCE(r_event_date,'0001-01-01')ev_date,rabname(r_id,2) nm FROM rabbits ORDER BY r_id ASC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                this.Add(new repRabbit(rd.GetInt32("r_id"), rd.GetInt32("r_mother"), rd.GetInt32("r_father"),
                                        rd.GetString("r_sex"), rd.GetInt32("r_name"), rd.GetInt32("r_surname"), rd.GetInt32("r_secname"),
                                        rd.GetDateTime("r_born"), rd.GetInt32("r_parent"), rd.GetDateTime("ev_date"), rd.GetString("nm")));
            }
            rd.Close();
            Program.log(" |rabbits count: {0:d}", this.Count);
        }

        internal List<repRabbit> Adult
        {
            get
            {
                List<repRabbit> result = new List<repRabbit>();
                foreach (repRabbit rab in this)
                {
                    if (rab.ParentID == 0)
                        result.Add(rab);
                }
                _adultCount = result.Count;
                return result;
            }
        }

        internal List<repRabbit> Yongers
        {
            get
            {
                List<repRabbit> result = new List<repRabbit>();
                foreach (repRabbit rab in this)
                {
                    if (rab.ParentID != 0)
                        result.Add(rab);
                }
                _youngCount = result.Count;
                return result;
            }
        }

        internal List<repRabbit> SukrolMothers
        {
            get
            {
                List<repRabbit> result = new List<repRabbit>();
                foreach (repRabbit rab in this)
                {
                    if (rab.Sex == Sex.Female && rab.EventDate != DateTime.MinValue)
                        result.Add(rab);
                }
                return result;
            }
        }

        internal repRabbit GetRabbitByID(int id)
        {
            foreach (repRabbit r in this)
            {
                if (r.rID == id)
                    return r;
            }
            return null;
        }

    }

    class DeadList : RabbitList
    {
        internal override void LoadContent(MySqlCommand cmd)
        {
            Program.log("Loading dead Rabbits");
            cmd.CommandText = String.Format("SELECT r_id,r_mother,r_father,r_sex,r_name,r_surname,r_secname,r_born,r_parent,deadname(r_id,2) nm FROM dead ORDER BY r_id DESC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                this.Add(new repRabbit(rd.GetInt32("r_id"), rd.GetInt32("r_mother"), rd.GetInt32("r_father"),
                                        rd.GetString("r_sex"), rd.GetInt32("r_name"), rd.GetInt32("r_surname"), rd.GetInt32("r_secname"),
                                        rd.GetDateTime("r_born"), rd.GetInt32("r_parent"), DateTime.MinValue, rd.GetString("nm"))
                            );
            }
            rd.Close();
            Program.log(" |deads count: {0:d}", this.Count);
        }
    }
    class NameList : List<repName>
    {
        internal void LoadNames(MySqlCommand cmd)
        {
            Program.log("Loading all names");
            cmd.CommandText = String.Format("SELECT n_id,n_sex,n_name,n_use,COALESCE(n_block_date,'0001-01-01')dt FROM names WHERE n_use<>0 ORDER BY n_use ASC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                this.Add(new repName(rd.GetInt32("n_id"), rd.GetString("n_sex"),
                                    rd.GetString("n_name"), rd.GetInt32("n_use"), rd.GetDateTime("dt"))
                           );
            }
            rd.Close();
            Program.log(" |name count: {0:d}", this.Count);
        }

        internal int GetSurnameUse(int surname)
        {
            foreach (repName n in this)
            {
                if (n.nameSex == Sex.Female && n.nID == surname)
                    return n.useRabbit;
            }
            return 0;
        }

        internal int GetSecnameUse(int secname)
        {
            foreach (repName n in this)
            {
                if (n.nameSex == Sex.Male && n.nID == secname)
                    return n.useRabbit;
            }
            return 0;
        }
    }

    class FuckList : List<repFuck>
    {
        internal void LoadFucks(MySqlCommand cmd)
        {
            Program.log("Loading fucks");
            cmd.CommandText = String.Format("SELECT f_id,f_rabid,f_partner,COALESCE(f_date,'0001-01-01') f_date, COALESCE(f_end_date,'0001-01-01')f_end_date,f_state,f_children FROM fucks ORDER BY f_id ASC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                this.Add(new repFuck(rd.GetInt32("f_id"), rd.GetInt32("f_rabid"), rd.GetInt32("f_partner"),
                                    rd.GetDateTime("f_date"), rd.GetDateTime("f_end_date"), rd.GetString("f_state"),
                                    rd.GetInt32("f_children"))
                          );
            }
            rd.Close();
            Program.log(" |fucks count: {0:d}", this.Count);
        }
    }

    class TiersList : List<repTier>
    {
        internal void LoadTiers(MySqlCommand cmd)
        {
            Program.log("Loading tiers");
            cmd.CommandText = String.Format("SELECT t_id,t_type,t_busy1,t_busy2,t_busy3,t_busy4 FROM tiers ORDER BY t_id ASC");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                this.Add(new repTier(rd.GetInt32("t_id"),rd.GetString("t_type"),
                    rd.IsDBNull(2) ? -1 : rd.GetInt32("t_busy1"),
                    rd.IsDBNull(3) ? -1 : rd.GetInt32("t_busy2"),
                    rd.IsDBNull(4) ? -1 : rd.GetInt32("t_busy3"),
                    rd.IsDBNull(5) ? -1 : rd.GetInt32("t_busy4")));
            }
            rd.Close();
            Program.log(" |tiers count: {0:d}", this.Count);
        }
    }

}
