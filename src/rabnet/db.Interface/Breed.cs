using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class Breed
    {
        public readonly int ID;
        public readonly string Name;
        public readonly string ShortName;
        public string Color;

        public Breed(int id, string name, string shrt,string color)
        {
            this.ID = id;
            this.Name = name;
            this.ShortName = shrt;
            this.Color = color;
        }

        public Breed(int id, string name, string shrt) : this(id, name, shrt, "") { }
    }

    public class BreedsList:List<Breed>
    {
        /// <summary>
        /// Ищет породу по ID
        /// </summary>
        /// <param name="bId">ID породы</param>
        /// <returns></returns>
        public Breed Search(int bId)
        {
            foreach (Breed b in this)
                if (b.ID == bId)
                    return b;
            return null;
        }

        /// <summary>
        /// Ищет породу по названию
        /// </summary>
        /// <param name="name">Название породы</param>
        /// <returns></returns>
        public Breed Search(string name)
        {
            foreach (Breed b in this)
                if (b.Name == name)
                    return b;
            return null;
        }

        public string GetNameByID(int bId)
        {
            foreach (Breed b in this)
                if (b.ID == bId)
                    return b.Name;
            return "";
        }

        
    }
}
