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
            cmd.CommandText = String.Format(@"SELECT f_id, f_rabid, f_partner, COALESCE(f_date,'0001-01-01') f_date, COALESCE(f_end_date,'0001-01-01') AS f_end_date, f_state, f_children 
FROM fucks 
ORDER BY f_id ASC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read()) {
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

        private readonly int _fID;
        private int _SheID;
        private int _HeID;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private State _fState;
        private int _Children;
        private bool _modified;

        internal int fID
        {
            get { return _fID; }
        }
        /// <summary>
        /// ID крольчихи
        /// </summary>
        internal int SheID
        {
            get { return _SheID; }
            set
            {
                _modified = true;
                _SheID = value;
            }
        }
        /// <summary>
        /// ID самца
        /// </summary>
        internal int HeID
        {
            get { return _HeID; }
            set
            {
                _modified = true;
                _HeID = value;
            }
        }
        internal DateTime StartDate
        {
            get { return _StartDate; }
            set
            {
                _modified = true;
                _StartDate = value;
            }
        }
        internal DateTime EndDate
        {
            get { return _EndDate; }
            set
            {
                _modified = true;
                _EndDate = value;
            }
        }
        internal State fState
        {
            get { return _fState; }
            set
            {
                _modified = true;
                _fState = value;
            }
        }
        internal int Children
        {
            get { return _Children; }
            set
            {
                _modified = true;
                _Children = value;
            }
        }
        internal bool Modifyed
        {
            get { return _modified; }
        }


        internal repFuck(int id, int rabid, int partner, DateTime date, DateTime end_date, string state, int children)
        {
            _fID = id;
            _SheID = rabid;
            _HeID = partner;
            _StartDate = date;
            _EndDate = end_date;
            switch (state) {
                case "okrol": _fState = State.Okrol; break;
                case "sukrol": _fState = State.Sukrol; break;
                case "proholost": _fState = State.Proholost; break;
            }
            _Children = children;
        }
    }
}
