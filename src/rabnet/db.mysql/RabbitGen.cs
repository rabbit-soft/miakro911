using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
	public class RabbitGen
	{
		public enum RabbitSex { VOID, MALE, FEMALE };
		public RabbitSex sex;
		public int rid;
		public string name;
		public string surname;
		public string secname;
		public string t;
		public string fullname()
		{
			string n = name;
			string surn = surname;
			string secn = secname;
			if (sex == RabbitSex.FEMALE)
			{
				if (surn != "")
				{
					surn += "a";
				}
				if (secn != "")
				{
					secn += "a";
				}
			}

			if ((secn != "") && (surn != ""))
			{
				n += " " + surn + "-" + secn;
			}
			else
			{
				if (secn != "")
				{
					n += " " + secn;
				}
				if (surn != "")
				{
					n += " " + surn;
				}
			}
			return n;
		}
	}
	
	public class RabbitGenGetter
	{
		public static RabbitGen GetRabbit(MySqlConnection sql, int rid)
		{
			MySqlCommand cmd = new MySqlCommand(String.Format(@"SELECT	r_mother, 
																		r_father, 
																		r_sex, 
																		(select n_name from names where n_id=r_name) name, 
																		(select n_surname from names where n_id=r_surname) surname, 
																		(select n_surname from names where n_id=r_secname) secname,
																		r_breed
																from rabbits
																where r_id={0:d}
																limit 1;", rid), sql);
			MySqlDataReader rd = cmd.ExecuteReader();

			RabbitGen r = new RabbitGen();
			r.rid = rid;

			if (rd.Read())
			{
				string sx=rd.GetString("r_sex");
				r.sex=RabbitGen.RabbitSex.VOID;
				if (sx == "male")
				{
					r.sex = RabbitGen.RabbitSex.MALE;
				}
				if (sx == "female")
				{
					r.sex = RabbitGen.RabbitSex.FEMALE;
				}

				r.name = rd.IsDBNull(3) ? "" : rd.GetString("name");
				r.surname = rd.IsDBNull(4) ? "" : rd.GetString("surname");
				r.secname = rd.IsDBNull(5) ? "" : rd.GetString("secname");
				r.t = r.fullname();
				//				f.addFuck(rd.GetString("partner"), rd.GetInt32("f_partner"), rd.GetInt32("f_times"),
				//					rd.IsDBNull(5) ? DateTime.MinValue : rd.GetDateTime("f_date"),
				//					rd.IsDBNull(6) ? DateTime.MinValue : rd.GetDateTime("f_end_date"),
				//					rd.GetString("f_state"), rd.GetInt32("f_children"), rd.GetInt32("f_dead"),
				//					rd.GetInt32("breed"), rd.IsDBNull(12) ? "" : rd.GetString("genom"), rd.GetString("f_type"),
				//					rd.GetInt32("f_killed"), rd.GetInt32("f_added"), (rd.GetInt32("dead") == 1)
				//					);


			}
			else
			{
				r = null;
			}
			rd.Close();
#if TRIAL
            Buildings.checkFarms3(sql);
#endif
			return r;
		}
	}
}
