using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using rabnet;

namespace db.mysql
{
#if !DEMO
    static class Import
    {

        internal static void RabbitImp(MySqlConnection sql, int rId, int count)
        {
            MySqlCommand cmd = new MySqlCommand("",sql);
            cmd.CommandText = String.Format("INSERT INTO import(t_rab_id,t_date,t_count) VALUES({0:d},NOW(),{1:d});", rId, count);
            cmd.ExecuteNonQuery();
        }

        internal static void RabbitImp(MySqlConnection sql, int rId, int count,int clientId,int oldRID,string fileGuid)
        {
            MySqlCommand cmd = new MySqlCommand("", sql);
            cmd.CommandText = String.Format(@"INSERT INTO import(t_date,t_rab_id,t_count,t_client,t_old_r_id,t_file_guid) 
                VALUES(NOW(), {0:d}, {1:d}, {2:d}, {3:d}, '{4:s}');", rId, count, clientId, oldRID, fileGuid);
            cmd.ExecuteNonQuery();
        }

        internal static void AscendantImp(MySqlConnection sql, OneRabbit r)
        {
            MySqlCommand cmd = new MySqlCommand(
                String.Format(@"INSERT INTO import_ascendants(r_id, r_mother, r_father, r_sex, r_name, r_surname, r_secname, r_breed, r_born, r_birthplace, r_bon) 
                    VALUES({0}, {1}, {2}, '{3}', {4}, {5}, {6}, {7}, '{8}', {9}, {10});",
                        r.ID,
                        DBHelper.Nullable(r.MotherID),
                        DBHelper.Nullable(r.FatherID), 
                        Rabbit.SexToString(r.Sex),
                        DBHelper.Nullable(r.NameID),
                        DBHelper.Nullable(r.SurnameID),
                        DBHelper.Nullable(r.SecnameID), 
                        r.BreedID, 
                        r.BirthDay.ToString("yyyy-MM-dd"), 
                        r.BirthPlace, 
                        r.Bon
                    ), 
                sql
            );            
            cmd.ExecuteNonQuery();
        }

        internal static List<OneImport> Search(MySqlConnection sql,Filters f)
        {
            List<OneImport> result = new List<OneImport>();
            string query = "SELECT t_date, t_rab_id, t_count, t_client, t_old_r_id, t_file_guid FROM import";
            if(f.Count>0)
            {
                string where="";
                if (f.ContainsKey(Filters.RAB_ID)) {
                    where += "t_rab_id=" + f[Filters.RAB_ID];
                }
                if (f.ContainsKey(Filters.CLIENT)) {
                    where += " AND t_client=" + f[Filters.CLIENT];
                }
                if (f.ContainsKey(Filters.OLD_RID)) {
                    where += " AND t_old_r_id=" + f[Filters.OLD_RID];
                }
                if (f.ContainsKey(Filters.GUID)) {
                    where += String.Format("AND t_file_guid='{0:s}'", f[Filters.GUID]);
                }
                if (where != "") {
                    query += " WHERE " + where.TrimStart(" AND".ToCharArray());
                }
                if (f.ContainsKey(Filters.LIMIT)) {
                    query += " LIMIT " + f[Filters.LIMIT];
                }
            }
            MySqlCommand cmd = new MySqlCommand(query, sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read()) {
                result.Add(new OneImport(rd.GetDateTime("t_date"),
                    rd.GetInt32("t_rab_id"),
                    rd.GetInt32("t_count"),
                    rd.IsDBNull(rd.GetOrdinal("t_client")) ? 0 : rd.GetInt32("t_client"),
                    rd.IsDBNull(rd.GetOrdinal("t_old_r_id")) ? 0 : rd.GetInt32("t_old_r_id"),
                    rd.IsDBNull(rd.GetOrdinal("t_file_guid")) ? "" : rd.GetString("t_file_guid")));
            }
            rd.Close();
            return result;
        }

        /// <summary>
        /// Проверяет существование записи в Таблице с информацией о предках импортированного кролика.
        /// </summary>
        internal static bool AscendantExists(MySqlConnection sql, int rabId, int clientId)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("SELECT count(*) FROM import_ascendants WHERE r_id={0:d} AND r_birthplace={1:d}",rabId,clientId), sql);
            return !(cmd.ExecuteScalar().ToString()=="0");
        }

        internal static void AscendantDelete(MySqlConnection sql, int rabId, int clientId)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format("DELETE FROM import_ascendants WHERE r_id={0:d} AND r_birthplace={1:d}", rabId, clientId), sql);
            cmd.ExecuteNonQuery();
        }

        internal static void AdaptExportedAscendant(MySqlConnection sql, int oldId, int clientId,int newId,Rabbit.SexType sex)
        {
            Import.AscendantDelete(sql, oldId, clientId);
            string query = String.Format("UPDATE {4:s} SET r_{0:s}={1:d} WHERE r_{0:s}={2:d} AND r_birthplace={3:d}",
                sex== Rabbit.SexType.FEMALE?"mother":"father",newId,oldId,clientId,"{0:s}");
            MySqlCommand cmd = new MySqlCommand(String.Format(query, "import_ascendants"), sql);
            cmd.ExecuteNonQuery();

            cmd.CommandText = String.Format(query,"rabbits");
            cmd.ExecuteNonQuery();
        }

    }
#endif
}
