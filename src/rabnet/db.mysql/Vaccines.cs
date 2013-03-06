using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace rabnet
{
    class Vaccines
    {
        private MySqlConnection _sql;

        public Vaccines(MySqlConnection con)
        {
            _sql = con;
        }

        public int Add(string name, int duration, int age, int after, bool zoo, int times)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO vaccines(v_name,v_duration,v_age,v_do_after,v_zootech,v_do_times) VALUES(@name,{0:d},{1:d},{2:d},{3},{4:d});",
                duration, age, after, zoo,times), _sql);
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@name",name);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }
        public int Add(string name)
        {
            return Add(name,180,45,0,true,0);
        }

        public void Edit(int id, string name, int duration, int age, int after, bool zoo, int times)
        {
            if (id == 0) return;
            MySqlCommand cmd = new MySqlCommand(
                String.Format("UPDATE vaccines SET v_name='{0:s}',v_duration={1:d},v_age={2:d}, v_do_after={3:d}, v_zootech={4}, v_do_times={6:d} WHERE v_id={5:d};",
                    name, duration, age, after, zoo, id,times), _sql);
            cmd.ExecuteNonQuery();
        }

        public List<Vaccine> Get(bool withSpec)
        {           
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT v_id,v_name,v_duration,v_age,v_do_after,v_zootech,v_do_times FROM vaccines {0};",withSpec?"":"WHERE v_id>0"), _sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<Vaccine> vaccs = new List<Vaccine>();
            while (rd.Read())
            {
                vaccs.Add(new Vaccine(rd.GetInt32("v_id"), rd.GetString("v_name"), rd.GetInt32("v_duration"),
                    rd.GetInt32("v_age"), rd.GetInt32("v_do_after"), rd.GetBoolean("v_zootech"), rd.GetInt32("v_do_times")));                
            }
            rd.Close();
            return vaccs;
        }
    }
}
