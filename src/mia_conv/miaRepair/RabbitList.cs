using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace mia_conv
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
            miaRepair.log("fill All Rabbits");
            cmd.CommandText = String.Format(@"SELECT r_id,
r_mother,
r_father,
r_sex,
r_name,
r_surname,
r_secname,
r_born,
r_parent,
COALESCE(r_event_date,'0001-01-01') ev_date,
rabname(r_id,2) nm,
to_days(NOW())-to_days(r_born) age 
            FROM rabbits ORDER BY r_id ASC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                this.Add(new repRabbit(rd.GetInt32("r_id"), rd.GetInt32("r_mother"), rd.GetInt32("r_father"),
                                        rd.GetString("r_sex"), rd.GetInt32("r_name"), rd.GetInt32("r_surname"), rd.GetInt32("r_secname"),
                                        rd.GetDateTime("r_born"), rd.GetInt32("r_parent"), rd.GetDateTime("ev_date"), rd.GetString("nm"), rd.GetInt32("age"))
                            );
            }
            rd.Close();
            miaRepair.log(" |rabbits count: {0:d}", this.Count);
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
            miaRepair.log("fill dead Rabbits");
            cmd.CommandText = String.Format(@"SELECT r_id,
r_mother,
r_father,
r_sex,
r_name,
r_surname,
r_secname,
r_born,
r_parent,
deadname(r_id,2) nm,
to_days(NOW())-to_days(r_born) age 
            FROM dead ORDER BY r_id DESC;");
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                this.Add(new repRabbit(rd.GetInt32("r_id"), rd.GetInt32("r_mother"), rd.GetInt32("r_father"),
                                        rd.GetString("r_sex"), rd.GetInt32("r_name"), rd.GetInt32("r_surname"), rd.GetInt32("r_secname"),
                                        rd.GetDateTime("r_born"), rd.GetInt32("r_parent"), DateTime.MinValue, rd.GetString("nm"), rd.GetInt32("age"))
                            );
            }
            rd.Close();
            miaRepair.log(" |deads count: {0:d}", this.Count);
        }
    }

    class repRabbit
    {
        internal readonly int rID;
        internal int Mother;
        internal int Father;
        internal Sex Sex;
        internal int NameId;
        /// <summary>
        /// Фамилия по матери
        /// </summary>
        internal int SurnameID;
        /// <summary>
        /// Фамилия по отцу
        /// </summary>
        internal int SecnameID;
        internal DateTime Born = DateTime.MinValue;
        internal int ParentID = 0;
        internal DateTime EventDate;
        internal string Name;
        internal int Age;

        internal repRabbit(int rid, int mother, int father, string sex, int name, int surname, int secname, DateTime born, int parent, DateTime ev_date, string namestr, int age)
        {
            this.rID = rid;
            this.Mother = mother;
            this.Father = father;
            switch (sex)
            {
                case "male": this.Sex = Sex.Male; break;
                case "female": this.Sex = Sex.Female; break;
                case "void": this.Sex = Sex.Void; break;
            }
            this.NameId = name;
            this.SurnameID = surname;
            this.SecnameID = secname;
            this.Born = born;
            this.ParentID = parent;
            this.EventDate = ev_date;
            this.Name = namestr;
            this.Age = age;
        }
    }
}
