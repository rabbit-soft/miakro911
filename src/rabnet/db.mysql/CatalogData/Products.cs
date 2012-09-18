using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Text;

namespace rabnet
{
    /*public interface IProducts
    {
        CatalogData getProducts();
        void ChangeProduct(int id, String name,String unit,byte[] image);
        int AddProduct(String name, String unit,byte[] image);
    }*/

    public class Products:ICatalog //: IProducts
    {
        private MySqlConnection sql = null;
        public Products(MySqlConnection sql)
        {
            this.sql = sql;
        }

        public CatalogData Get()
        {
            int imageSize;
            byte[] image;
            CatalogData cd = new CatalogData();
            cd.ColNames = new String[] { "Название продукции", "Единицы измерения", CatalogData.IMAGE_MARKER+"Изображение" };
            MySqlCommand cmd = new MySqlCommand("SELECT p_id,p_name,p_unit,p_image,p_image,p_imgsize FROM products ORDER BY p_id ASC;", sql);
            MySqlDataReader rd = cmd.ExecuteReader();
            List<CatalogData.Row> rws = new List<CatalogData.Row>();
            while (rd.Read())
            {
                CatalogData.Row rw = new CatalogData.Row();
                rw.key = rd.GetInt32(0);
                //rw.imageSize = rd.GetInt32("p_imgsize");
                imageSize = rd.GetInt32("p_imgsize");
                image = new byte[imageSize];
                if (imageSize != 0)                
                    rd.GetBytes(rd.GetOrdinal("p_image"), 0, image, 0, imageSize);

                rw.data = new String[] { rd.GetString("p_name"), rd.GetString("p_unit"), Convert.ToBase64String(image) };
                rws.Add(rw);
            }
            rd.Close();
            cd.Rows = rws.ToArray();
            return cd;
        }

        /// <param name="args">name, unit, base64image</param>
        public void Change(int id, params String[] args)
        {
            if (args.Length != 3) throw new Exception("incorrect parms count (" + args.Length.ToString() + ") expected: 3");
            
            byte[] image = Convert.FromBase64String(args[2]);
            if (id == 0) return;
            MySqlCommand cmd = new MySqlCommand(String.Format("UPDATE products SET p_name='{0}',p_unit='{1}',p_image=@image,p_imgsize=@size WHERE p_id={2:d};", args[0], args[1], id), sql);
            cmd.Parameters.AddWithValue("@image", image);
            cmd.Parameters.AddWithValue("@size", image.Length);
            cmd.ExecuteNonQuery();

        }

        /// <param name="args">name, unit, base64image</param>
        public int Add(params String[] args)
        {
            if (args.Length != 3) throw new Exception("incorrect parms count (" + args.Length + ") expected: 3");

            byte[] image = Convert.FromBase64String(args[2]);
            MySqlCommand cmd = new MySqlCommand(String.Format("INSERT INTO products(p_name,p_unit,p_image,p_imgsize) VALUES('{0}','{1}',@image,@size);", args[0], args[1]), sql);
            cmd.Parameters.AddWithValue("@image", image);
            cmd.Parameters.AddWithValue("@size", image.Length);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }
    }
}
