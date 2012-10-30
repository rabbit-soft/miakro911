using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class Catalog : Dictionary<int, string> { }

    public interface ICatalog
    {
        CatalogData Get();
        void Change(int id, params string[] args);
        int Add(params string[] args);
    }

    /// <summary>
    /// Класс для заполнения справочников в CatalogForm
    /// </summary>
    public class CatalogData
    {
        public const string IMAGE_MARKER = "#image#";
        public const string COLOR_MARKER = "#color#";
        public const string BOOL_MARKER = "#bool#";
        public const string VACAFTER_MARKER = "#vacafter#";

        /// <summary>
        /// Одна строка DataGridView. Имеет, ID и массив значений ячеек
        /// </summary>
        public struct Row
        {
            public int key;
            public String[] data;
            //public byte[] image;
            //public int imageSize;
        }
        /// <summary>
        /// Массив имен столбцов DataGridView
        /// </summary>
        public String[] ColNames;
        /// <summary>
        /// Массив строк DataGridView
        /// </summary>
        public Row[] Rows;
    }

    public interface ICatalogs
    {
        Catalog stdCatalog(String data);
        Catalog getBreeds();
        Catalog getNames(int sex);
        Catalog getSurNames(int sex, String ends);
        Catalog getZones();
        Catalog getFreeNames(int sex, int plusid);
        Catalog getDeadReasons();
    }
}
