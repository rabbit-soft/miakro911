using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    public interface IProducts
    {
        CatalogData getProducts();
        void ChangeProduct(int id, String name,String unit,byte[] image);
        int AddProduct(String name, String unit,byte[] image);
    }

    public class Products : IProducts
    {
        private MySqlConnection sql = null;
        public Products(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public CatalogData getProducts()
        {
            CatalogData cd = new CatalogData();
            cd.colnames = new String[] { "Название продукции", "Единицы измерения", "#image#Изображение" };
            MySqlCommand cmd = new MySqlCommand("SELECT p_id,p_name,p_unit,p_image,p_image,p_imgsize FROM products ORDER BY p_id ASC;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<CatalogData.Row> rws = new List<CatalogData.Row>();
            while (rd.Read())
            {
                CatalogData.Row rw = new CatalogData.Row();
                rw.key = rd.GetInt32(0);
                rw.imageSize = rd.GetInt32("p_imgsize");
                rw.image = new byte[rw.imageSize];
                if(rw.imageSize !=0)
                    rd.GetBytes(rd.GetOrdinal("p_image"), 0, rw.image, 0, rw.imageSize);
                rw.data = new String[] { rd.GetString("p_name"),rd.GetString("p_unit"),"$image$" };
                rws.Add(rw);
            }
            rd.Close();
            cd.data = rws.ToArray();
            return cd;
        }

        public void ChangeProduct(int id, String name, String unit, byte[] image)
        {

            if (id == 0) return;
            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE products SET p_name='{0}',p_unit='{1}',p_image=@image,p_imgsize=@size WHERE p_id={2};", name, unit, id.ToString()), sql);
            cmd.Parameters.AddWithValue("@image", image);
            cmd.Parameters.AddWithValue("@size", image.Length);
            cmd.ExecuteNonQuery();

        }

        public int AddProduct(String name, String unit, byte[] image)
        {

            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO products(p_name,p_unit,p_image,p_imgsize) VALUES('{0}','{1}',@image,@size);", name, unit), sql);
            cmd.Parameters.AddWithValue("@image", image);
            cmd.Parameters.AddWithValue("@size", image.Length);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }
    }
}
