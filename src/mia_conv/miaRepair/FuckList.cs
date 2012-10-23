using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace mia_conv
{
    class FuckList : List<repFuck>
    {
        internal void LoadFucks(MySqlCommand cmd)
        {
            miaRepair.log("fill fucks");
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
            miaRepair.log(" |fucks count: {0:d}", this.Count);
        }
    }

    class repFuck
    {
        internal enum State { Sukrol, Okrol, Proholost };

        internal int fID;
        /// <summary>
        /// ID крольчихи
        /// </summary>
        internal int SheID;
        /// <summary>
        /// ID самца
        /// </summary>
        internal int HeID;
        internal DateTime StartDate;
        internal DateTime EndDate;
        internal State fState;
        internal int Children;

        internal repFuck(int id, int rabid, int partner, DateTime date, DateTime end_date, string state, int children)
        {
            this.fID = id;
            this.SheID = rabid;
            this.HeID = partner;
            this.StartDate = date;
            this.EndDate = end_date;
            switch (state)
            {
                case "okrol": this.fState = State.Okrol; break;
                case "sukrol": this.fState = State.Sukrol; break;
                case "proholost": this.fState = State.Proholost; break;
            }
            this.Children = children;
        }
    }
}
