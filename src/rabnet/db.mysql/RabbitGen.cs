using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace rabnet
{
	public class RabbitGen
	{
		public enum RabbitSex { VOID, MALE, FEMALE };
		public RabbitSex sex;
		public int rid;
		public int r_mother;
		public int r_father;
		public string name;
		public string surname;
		public string secname;
		public string breed_color_name;
		public Color breed_color;
		public int breed;
		public string t;
		public float PriplodK;
		public float RodK;
		public string fullname
		{
			get
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
																		(select b_color from breeds where b_id=r_breed) b_color,
																		r_breed
																from rabbits
																where r_id={0:d}
																limit 1;", rid), sql);
			MySqlDataReader rd = cmd.ExecuteReader();

			RabbitGen r = new RabbitGen();
			r.rid = rid;

			if (rd.Read())
			{
				r.r_father = rd.GetInt32("r_father"); //0
				r.r_mother = rd.GetInt32("r_mother"); //1
				string sx = rd.GetString("r_sex"); //2
				r.sex=RabbitGen.RabbitSex.VOID;
				if (sx == "male")
				{
					r.sex = RabbitGen.RabbitSex.MALE;
				}
				if (sx == "female")
				{
					r.sex = RabbitGen.RabbitSex.FEMALE;
				}

				r.name = rd.IsDBNull(3) ? "" : rd.GetString("name"); //3
				r.surname = rd.IsDBNull(4) ? "" : rd.GetString("surname"); //4
				r.secname = rd.IsDBNull(5) ? "" : rd.GetString("secname"); //5
				r.breed_color_name = rd.IsDBNull(6) ? "" : rd.GetString("b_color"); //5

				int res;

				if (int.TryParse(r.breed_color_name, System.Globalization.NumberStyles.HexNumber, null, out res))
				{
					r.breed_color = Color.FromArgb(res);
				}
				else
				{
					r.breed_color = Color.FromName(r.breed_color_name);
				}
				r.breed = rd.GetInt32("r_breed"); //6

			}
			else
			{
				r = null;
			}
			rd.Close();
			if (r != null)
			{
				getRabbitPriplodK(sql, ref r);
				getRabbitRodK(sql, ref r);
			}
			return r;
		}

		public static void getRabbitPriplodK(MySqlConnection sql, ref RabbitGen rabbit)
		{
			string f = "f_rabid";
			if (rabbit.sex == RabbitGen.RabbitSex.MALE)
			{
				f = "f_partner";
			}


			MySqlCommand cmd = new MySqlCommand(String.Format(@"	SELECT coalesce(sum(f_children)/(sum(f_times)-(	select count(f_state) 
																													from fucks 
																													where	{1}={0:d} 
																															and f_state='sukrol')),0) k  
																	FROM fucks 
																	where {1}={0:d};", rabbit.rid, f), sql);
			MySqlDataReader rd = cmd.ExecuteReader();
			if (rd.Read())
			{
				rabbit.PriplodK = rd.GetFloat("k");

			}
			rd.Close();
		}
		
		public static void getRabbitRodK(MySqlConnection sql, ref RabbitGen rabbit)
		{
			if (rabbit.sex == RabbitGen.RabbitSex.FEMALE)
			{
				MySqlCommand cmd = new MySqlCommand(String.Format(@"	select coalesce((sum(f_children)-sum(f_killed)+sum(f_added))/(sum(f_children)+sum(f_added)),0) k
																		from fucks 
																		where f_rabid={0:d};", rabbit.rid), sql);
				MySqlDataReader rd = cmd.ExecuteReader();
				if (rd.Read())
				{
					rabbit.RodK = rd.GetFloat("k");
				}
				rd.Close();
			}
			if (rabbit.sex == RabbitGen.RabbitSex.MALE)
			{
				MySqlCommand cmd = new MySqlCommand(String.Format(@"	select	(select count(f_state) from fucks where f_partner={0:d} and f_state='okrol' and f_times=1) o,
																				(select count(f_state) from fucks where f_partner={0:d} and f_state='proholost' and f_times=1) p;", rabbit.rid), sql);
				MySqlDataReader rd = cmd.ExecuteReader();
				if (rd.Read())
				{
					float o = rd.GetFloat("o");
					float p = rd.GetFloat("p");
					if (p + 0 == 0)
					{
						rabbit.RodK = 0;
					}
					else
					{
						rabbit.RodK = o / (o + p);
					}
				}
				rd.Close();
			}


		}
		public static Dictionary<int, Color> getBreedColors(MySqlConnection sql)
		{
			Dictionary<int, Color> Dict = new Dictionary<int, Color>();
			Color cl;
			string cl_name;
			MySqlCommand cmd = new MySqlCommand(@"	select b_id, b_color from breeds;", sql);
			MySqlDataReader rd = cmd.ExecuteReader();
			while (rd.Read())
			{

				cl_name = rd.IsDBNull(1) ? "" : rd.GetString("b_color");

				int res;

				if (int.TryParse(cl_name, System.Globalization.NumberStyles.HexNumber, null, out res))
				{
					cl = Color.FromArgb(res);
				}
				else
				{
					cl = Color.FromName(cl_name);
				}
				Dict.Add(rd.GetInt32(0), cl);
			}
			rd.Close();
			return Dict;
		}
	}
}
